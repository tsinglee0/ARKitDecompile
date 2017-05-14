using System;
using System.Collections.Generic;

namespace Vuforia
{
	public abstract class ObjectTracker : Tracker
	{
		public abstract ImageTargetBuilder ImageTargetBuilder
		{
			get;
		}

		public abstract TargetFinder TargetFinder
		{
			get;
		}

		public abstract DataSet CreateDataSet();

		public abstract bool DestroyDataSet(DataSet dataSet, bool destroyTrackables);

		public abstract bool ActivateDataSet(DataSet dataSet);

		public abstract bool DeactivateDataSet(DataSet dataSet);

		public abstract IEnumerable<DataSet> GetActiveDataSets();

		public abstract IEnumerable<DataSet> GetDataSets();

		public abstract void DestroyAllDataSets(bool destroyTrackables);

		public abstract bool PersistExtendedTracking(bool on);

		public abstract bool ResetExtendedTracking();
	}
}
