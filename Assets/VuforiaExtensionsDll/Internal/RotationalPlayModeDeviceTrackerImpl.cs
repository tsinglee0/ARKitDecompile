using System;
using UnityEngine;

namespace Vuforia
{
	internal class RotationalPlayModeDeviceTrackerImpl : RotationalDeviceTracker
	{
		private Vector3 mRotation = Vector3.zero;

		private Vector3 mModelCorrectionTransform = Vector3.zero;

		private RotationalDeviceTracker.MODEL_CORRECTION_MODE mModelCorrection;

		public Vector3 Rotation
		{
			get
			{
				return this.mRotation;
			}
			set
			{
				this.mRotation = value;
			}
		}

		public override bool Start()
		{
			VuforiaManager.Instance.WorldCenterMode = VuforiaARController.WorldCenterMode.DEVICE_TRACKING;
			this.IsActive = true;
			this.RecenterPoseToCurrentAnchorPointPosition();
			return true;
		}

		public override void Stop()
		{
			this.IsActive = false;
			VuforiaManager.Instance.WorldCenterMode = VuforiaARController.WorldCenterMode.CAMERA;
		}

		public override void RecenterPose()
		{
			this.RecenterPose(true);
		}

		public override void RecenterPose(bool resetToCurrentPose)
		{
			this.mRotation.y = 0f;
			if (resetToCurrentPose)
			{
				this.RecenterPoseToCurrentAnchorPointPosition();
			}
		}

		public override void SetPosePrediction(bool mode)
		{
		}

		public override bool GetPosePrediction()
		{
			return false;
		}

		public override void SetModelCorrectionMode(RotationalDeviceTracker.MODEL_CORRECTION_MODE mode)
		{
			this.mModelCorrection = mode;
			if (mode == RotationalDeviceTracker.MODEL_CORRECTION_MODE.HEAD)
			{
				this.mModelCorrectionTransform = DeviceTrackerARController.DEFAULT_HEAD_PIVOT;
				return;
			}
			if (mode == RotationalDeviceTracker.MODEL_CORRECTION_MODE.HANDHELD)
			{
				this.mModelCorrectionTransform = DeviceTrackerARController.DEFAULT_HANDHELD_PIVOT;
			}
		}

		public override RotationalDeviceTracker.MODEL_CORRECTION_MODE GetModelCorrectionMode()
		{
			return this.mModelCorrection;
		}

		public override void SetModelCorrectionModeWithTransform(RotationalDeviceTracker.MODEL_CORRECTION_MODE mode, Vector3 transform)
		{
			this.mModelCorrection = mode;
			this.mModelCorrectionTransform = transform;
		}

		public override Vector3 GetModelCorrectionTransform()
		{
			if (this.mModelCorrection == RotationalDeviceTracker.MODEL_CORRECTION_MODE.NONE)
			{
				return Vector3.zero;
			}
			return this.mModelCorrectionTransform;
		}

		public VuforiaManagerImpl.TrackableResultData GetTrackable()
		{
			Quaternion quaternion = Quaternion.Euler(this.mRotation);
			Vector3 position = quaternion * this.GetModelCorrectionTransform();
			return new VuforiaManagerImpl.TrackableResultData
			{
				id = 0,
				pose = new VuforiaManagerImpl.PoseData
				{
					orientation = quaternion,
					position = position
				},
				status = TrackableBehaviour.Status.TRACKED,
				timeStamp = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds
			};
		}

		private void RecenterPoseToCurrentAnchorPointPosition()
		{
			((StateManagerImpl)TrackerManager.Instance.GetStateManager()).GetDeviceTrackingManager().RecenterPose(VuforiaManager.Instance.CentralAnchorPoint, this.GetModelCorrectionTransform());
		}
	}
}
