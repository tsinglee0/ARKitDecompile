using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Vuforia
{
	internal class ObjectTrackerImpl : ObjectTracker
	{
		private List<DataSetImpl> mActiveDataSets = new List<DataSetImpl>();

		private List<DataSet> mDataSets = new List<DataSet>();

		private ImageTargetBuilder mImageTargetBuilder;

		private TargetFinder mTargetFinder;

		public override ImageTargetBuilder ImageTargetBuilder
		{
			get
			{
				return this.mImageTargetBuilder;
			}
		}

		public override TargetFinder TargetFinder
		{
			get
			{
				return this.mTargetFinder;
			}
		}

		public ObjectTrackerImpl()
		{
			this.mImageTargetBuilder = new ImageTargetBuilderImpl();
			this.mTargetFinder = new TargetFinderImpl();
		}

		public override bool Start()
		{
			if (VuforiaWrapper.Instance.TrackerStart((int)TypeMapping.GetTypeID(typeof(ObjectTracker))) != 1)
			{
				this.IsActive = false;
				Debug.LogError("Could not start tracker.");
				return false;
			}
			this.IsActive = true;
			return true;
		}

		public override void Stop()
		{
			VuforiaWrapper.Instance.TrackerStop((int)TypeMapping.GetTypeID(typeof(ObjectTracker)));
			this.IsActive = false;
			StateManagerImpl stateManagerImpl = (StateManagerImpl)TrackerManager.Instance.GetStateManager();
			using (List<DataSetImpl>.Enumerator enumerator = this.mActiveDataSets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					foreach (Trackable current in enumerator.Current.GetTrackables())
					{
						stateManagerImpl.SetTrackableBehavioursForTrackableToNotFound(current);
					}
				}
			}
		}

		public override DataSet CreateDataSet()
		{
			IntPtr intPtr = VuforiaWrapper.Instance.ObjectTrackerCreateDataSet();
			if (intPtr == IntPtr.Zero)
			{
				Debug.LogError("Could not create dataset.");
				return null;
			}
			DataSet dataSet = new DataSetImpl(intPtr);
			this.mDataSets.Add(dataSet);
			return dataSet;
		}

		public override bool DestroyDataSet(DataSet dataSet, bool destroyTrackables)
		{
			if (dataSet == null)
			{
				Debug.LogError("Dataset is null.");
				return false;
			}
			if (destroyTrackables)
			{
				dataSet.DestroyAllTrackables(true);
			}
			DataSetImpl dataSetImpl = (DataSetImpl)dataSet;
			if (VuforiaWrapper.Instance.ObjectTrackerDestroyDataSet(dataSetImpl.DataSetPtr) == 0)
			{
				Debug.LogError("Could not destroy dataset.");
				return false;
			}
			this.mDataSets.Remove(dataSet);
			return true;
		}

		public override bool ActivateDataSet(DataSet dataSet)
		{
			if (dataSet == null)
			{
				Debug.LogError("Dataset is null.");
				return false;
			}
			DataSetImpl dataSetImpl = (DataSetImpl)dataSet;
			if (VuforiaWrapper.Instance.ObjectTrackerActivateDataSet(dataSetImpl.DataSetPtr) == 0)
			{
				Debug.LogError("Could not activate dataset.");
				return false;
			}
			StateManagerImpl stateManagerImpl = (StateManagerImpl)TrackerManager.Instance.GetStateManager();
			foreach (Trackable current in dataSetImpl.GetTrackables())
			{
				stateManagerImpl.EnableTrackableBehavioursForTrackable(current, true);
			}
			this.mActiveDataSets.Add(dataSetImpl);
			return true;
		}

		public override bool DeactivateDataSet(DataSet dataSet)
		{
			if (dataSet == null)
			{
				Debug.LogError("Dataset is null.");
				return false;
			}
			DataSetImpl dataSetImpl = (DataSetImpl)dataSet;
			if (VuforiaWrapper.Instance.ObjectTrackerDeactivateDataSet(dataSetImpl.DataSetPtr) == 0)
			{
				Debug.LogError("Could not deactivate dataset.");
				return false;
			}
			StateManagerImpl stateManagerImpl = (StateManagerImpl)TrackerManager.Instance.GetStateManager();
			foreach (Trackable current in dataSet.GetTrackables())
			{
				stateManagerImpl.EnableTrackableBehavioursForTrackable(current, false);
			}
			this.mActiveDataSets.Remove(dataSetImpl);
			return true;
		}

		public override IEnumerable<DataSet> GetActiveDataSets()
		{
			return this.mActiveDataSets.Cast<DataSet>();
		}

		public override IEnumerable<DataSet> GetDataSets()
		{
			return this.mDataSets;
		}

		public override void DestroyAllDataSets(bool destroyTrackables)
		{
			foreach (DataSetImpl current in new List<DataSetImpl>(this.mActiveDataSets))
			{
				this.DeactivateDataSet(current);
			}
			for (int i = this.mDataSets.Count - 1; i >= 0; i--)
			{
				this.DestroyDataSet(this.mDataSets[i], destroyTrackables);
			}
			this.mDataSets.Clear();
		}

		public override bool PersistExtendedTracking(bool on)
		{
			return ((StateManagerImpl)TrackerManager.Instance.GetStateManager()).GetExtendedTrackingManager().PersistExtendedTracking(on);
		}

		public override bool ResetExtendedTracking()
		{
			return ((StateManagerImpl)TrackerManager.Instance.GetStateManager()).GetExtendedTrackingManager().ResetExtendedTracking(this.IsActive);
		}
	}
}
