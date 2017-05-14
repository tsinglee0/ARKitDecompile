using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
	internal interface IExtendedTrackingManager
	{
		void ApplyTrackingState(TrackableBehaviour trackableBehaviour, TrackableBehaviour.Status vuforiaStatus, Transform cameraTransform);

		bool StartExtendedTracking(IntPtr datasetPtr, int trackableID);

		bool StopExtendedTracking(IntPtr datasetPtr, int trackableID);

		bool PersistExtendedTracking(bool on);

		bool ResetExtendedTracking(bool trackerActive);

		IEnumerable<VuforiaManager.TrackableIdPair> GetExtendedTrackedBehaviours();

		void EnableWorldAnchorUsage(bool enable);

		void GetExtendedTrackingParameters(out int numFramesStablePose, out float maxPoseRelDistance, out float maxPoseAngleDiff, out int minNumFramesPoseOff, out float minPoseUpdateRelDistance, out float minPoseUpdateAngleDiff);

		void SetExtendedTrackingParameters(int numFramesStablePose, float maxPoseRelDistance, float maxPoseAngleDiff, int minNumFramesPoseOff, float minPoseUpdateRelDistance, float minPoseUpdateAngleDiff);
	}
}
