using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Vuforia
{
	internal class VuforiaExtendedTrackingManager : IExtendedTrackingManager
	{
		public void ApplyTrackingState(TrackableBehaviour trackableBehaviour, TrackableBehaviour.Status vuforiaStatus, Transform cameraTransform)
		{
			trackableBehaviour.OnTrackerUpdate(vuforiaStatus);
		}

		public bool StartExtendedTracking(IntPtr datasetPtr, int trackableID)
		{
			return VuforiaWrapper.Instance.StartExtendedTracking(datasetPtr, trackableID) > 0;
		}

		public bool StopExtendedTracking(IntPtr datasetPtr, int trackableID)
		{
			return VuforiaWrapper.Instance.StopExtendedTracking(datasetPtr, trackableID) > 0;
		}

		public bool PersistExtendedTracking(bool on)
		{
			if (VuforiaWrapper.Instance.ObjectTrackerPersistExtendedTracking(on ? 1 : 0) == 0)
			{
				Debug.LogError("Could not set persist extended tracking.");
				return false;
			}
			return true;
		}

		public bool ResetExtendedTracking(bool trackerActive)
		{
			if (trackerActive)
			{
				Debug.LogError("Tracker should be stopped prior to calling ResetExtendedTracking()");
				return false;
			}
			if (VuforiaWrapper.Instance.ObjectTrackerResetExtendedTracking() == 0)
			{
				Debug.LogError("Could not reset extended tracking.");
				return false;
			}
			return true;
		}

		public IEnumerable<VuforiaManager.TrackableIdPair> GetExtendedTrackedBehaviours()
		{
			return Enumerable.Empty<VuforiaManager.TrackableIdPair>();
		}

		public void EnableWorldAnchorUsage(bool enable)
		{
		}

		public void GetExtendedTrackingParameters(out int numFramesStablePose, out float maxPoseRelDistance, out float maxPoseAngleDiff, out int minNumFramesPoseOff, out float minPoseUpdateRelDistance, out float minPoseUpdateAngleDiff)
		{
			numFramesStablePose = 0;
			maxPoseRelDistance = 0f;
			maxPoseAngleDiff = 0f;
			minNumFramesPoseOff = 0;
			minPoseUpdateRelDistance = 0f;
			minPoseUpdateAngleDiff = 0f;
		}

		public void SetExtendedTrackingParameters(int numFramesStablePose, float maxPoseRelDistance, float maxPoseAngleDiff, int minNumFramesPoseOff, float minPoseUpdateRelDistance, float minPoseUpdateAngleDiff)
		{
		}
	}
}
