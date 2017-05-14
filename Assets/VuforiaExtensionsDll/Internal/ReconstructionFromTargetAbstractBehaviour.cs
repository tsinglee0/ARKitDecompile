using System;
using System.Linq;
using UnityEngine;

namespace Vuforia
{
	[RequireComponent(typeof(ReconstructionAbstractBehaviour))]
	public abstract class ReconstructionFromTargetAbstractBehaviour : MonoBehaviour
	{
		private ReconstructionFromTarget mReconstructionFromTarget;

		private ReconstructionAbstractBehaviour mReconstructionBehaviour;

		public ReconstructionAbstractBehaviour ReconstructionBehaviour
		{
			get
			{
				return this.mReconstructionBehaviour;
			}
		}

		public ReconstructionFromTarget ReconstructionFromTarget
		{
			get
			{
				return this.mReconstructionFromTarget;
			}
		}

		private void Awake()
		{
			this.mReconstructionBehaviour = base.GetComponent<ReconstructionAbstractBehaviour>();
			if (this.mReconstructionBehaviour == null)
			{
				Debug.LogError("ReconstructionFromTargetAbstractBehaviour: No ReconstructionAbstractBehaviour found!");
			}
			SmartTerrainTrackerARController.Instance.RegisterTrackerStartedCallback(new Action(this.OnTrackerStarted));
		}

		private void OnDestroy()
		{
			if (this.mReconstructionBehaviour != null)
			{
				SmartTerrainTracker tracker = TrackerManager.Instance.GetTracker<SmartTerrainTracker>();
				if (tracker != null)
				{
					if (tracker.SmartTerrainBuilder.GetReconstructions().Contains(this.mReconstructionBehaviour))
					{
						tracker.SmartTerrainBuilder.RemoveReconstruction(this.mReconstructionBehaviour);
					}
					if (this.mReconstructionBehaviour.Reconstruction != null)
					{
						tracker.SmartTerrainBuilder.DestroyReconstruction(this.mReconstructionFromTarget);
					}
				}
				this.mReconstructionBehaviour.Deinitialize();
			}
			SmartTerrainTrackerARController.Instance.UnregisterTrackerStartedCallback(new Action(this.OnTrackerStarted));
		}

		public void Initialize()
		{
			if (this.mReconstructionBehaviour != null)
			{
				this.mReconstructionBehaviour.Initialize(this.mReconstructionFromTarget);
				return;
			}
			Debug.LogError("ReconstructionFromTargetAbstractBehaviour.Initialize: Need a ReconstructionAbstractBehaviour to initialize!");
		}

		private void OnTrackerStarted()
		{
			if (this.mReconstructionFromTarget == null)
			{
				SmartTerrainTracker tracker = TrackerManager.Instance.GetTracker<SmartTerrainTracker>();
				if (tracker != null)
				{
					this.mReconstructionFromTarget = tracker.SmartTerrainBuilder.CreateReconstruction<ReconstructionFromTarget>();
				}
			}
		}
	}
}
