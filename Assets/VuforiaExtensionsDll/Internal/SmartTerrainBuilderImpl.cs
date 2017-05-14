using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
	internal class SmartTerrainBuilderImpl : SmartTerrainBuilder
	{
		private readonly List<ReconstructionAbstractBehaviour> mReconstructionBehaviours = new List<ReconstructionAbstractBehaviour>();

		private bool mIsInitialized;

		public override bool Init()
		{
			if (!this.mIsInitialized)
			{
				this.mIsInitialized = (VuforiaWrapper.Instance.SmartTerrainTrackerInitBuilder() == 1);
				return this.mIsInitialized;
			}
			return false;
		}

		public override bool Deinit()
		{
			if (this.mIsInitialized)
			{
				ReconstructionAbstractBehaviour[] array = this.mReconstructionBehaviours.ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					ReconstructionAbstractBehaviour reconstructionAbstractBehaviour = array[i];
					this.RemoveReconstruction(reconstructionAbstractBehaviour);
					this.DestroyReconstruction(reconstructionAbstractBehaviour.Reconstruction);
				}
				this.mReconstructionBehaviours.Clear();
				bool expr_53 = VuforiaWrapper.Instance.SmartTerrainTrackerDeinitBuilder() == 1;
				if (expr_53)
				{
					this.mIsInitialized = false;
				}
				return expr_53;
			}
			return false;
		}

		public override IEnumerable<ReconstructionAbstractBehaviour> GetReconstructions()
		{
			return this.mReconstructionBehaviours;
		}

		public override T CreateReconstruction<T>()
		{
			if (typeof(T) == typeof(ReconstructionFromTarget))
			{
				return new ReconstructionFromTargetImpl(VuforiaWrapper.Instance.SmartTerrainBuilderCreateReconstructionFromTarget()) as T;
			}
			return PremiumObjectFactory.Instance.CreateReconstruction<T>();
		}

		public override bool AddReconstruction(ReconstructionAbstractBehaviour reconstructionBehaviour)
		{
			if (this.mReconstructionBehaviours.Count == 0)
			{
				ReconstructionImpl reconstructionImpl = reconstructionBehaviour.Reconstruction as ReconstructionImpl;
				if (reconstructionImpl != null)
				{
					bool flag = VuforiaWrapper.Instance.SmartTerrainBuilderAddReconstruction(reconstructionImpl.NativePtr) == 1;
					if (flag)
					{
						if (reconstructionImpl is ReconstructionFromTargetImpl)
						{
							(reconstructionImpl as ReconstructionFromTargetImpl).CanAutoSetInitializationTarget = false;
						}
						this.mReconstructionBehaviours.Add(reconstructionBehaviour);
						SmartTerrainTrackableBehaviour[] componentsInChildren = reconstructionBehaviour.GetComponentsInChildren<SmartTerrainTrackableBehaviour>();
						for (int i = 0; i < componentsInChildren.Length; i++)
						{
							componentsInChildren[i].gameObject.SetActive(false);
						}
					}
					else
					{
						Debug.LogError("Could not add Reconstruction to SmartTerrainBuilder");
					}
					return flag;
				}
			}
			else
			{
				Debug.LogError("Could not add Reconstruction to SmartTerrainBuilder, only one reconstruction can be added at a time");
			}
			return false;
		}

		public override bool RemoveReconstruction(ReconstructionAbstractBehaviour reconstruction)
		{
			if (this.mReconstructionBehaviours.Contains(reconstruction))
			{
				bool result = false;
				ReconstructionImpl reconstructionImpl = reconstruction.Reconstruction as ReconstructionImpl;
				if (reconstructionImpl != null)
				{
					result = (VuforiaWrapper.Instance.SmartTerrainBuilderRemoveReconstruction(reconstructionImpl.NativePtr) == 1);
				}
				this.mReconstructionBehaviours.Remove(reconstruction);
				reconstruction.OnReconstructionRemoved();
				return result;
			}
			return false;
		}

		public override bool DestroyReconstruction(Reconstruction reconstruction)
		{
			ReconstructionImpl reconstructionImpl = reconstruction as ReconstructionImpl;
			return reconstructionImpl != null && VuforiaWrapper.Instance.SmartTerrainBuilderRemoveReconstruction(reconstructionImpl.NativePtr) == 1;
		}

		internal void UpdateSmartTerrainData(VuforiaManagerImpl.SmartTerrainRevisionData[] smartTerrainRevisions, VuforiaManagerImpl.SurfaceData[] updatedSurfaces, VuforiaManagerImpl.PropData[] updatedProps)
		{
			using (List<ReconstructionAbstractBehaviour>.Enumerator enumerator = this.mReconstructionBehaviours.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.UpdateSmartTerrainData(smartTerrainRevisions, updatedSurfaces, updatedProps);
				}
			}
		}
	}
}
