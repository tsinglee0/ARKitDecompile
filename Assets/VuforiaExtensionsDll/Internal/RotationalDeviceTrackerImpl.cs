using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class RotationalDeviceTrackerImpl : RotationalDeviceTracker
	{
		public override bool Start()
		{
			if (VuforiaWrapper.Instance.TrackerStart((int)TypeMapping.GetTypeID(typeof(RotationalDeviceTracker))) != 1)
			{
				this.IsActive = false;
				Debug.LogError("Could not start tracker.");
				return false;
			}
			this.RecenterPoseToCurrentAnchorPointPosition(true);
			VuforiaManager.Instance.WorldCenterMode = VuforiaARController.WorldCenterMode.DEVICE_TRACKING;
			this.IsActive = true;
			return true;
		}

		public override void Stop()
		{
			VuforiaWrapper.Instance.TrackerStop((int)TypeMapping.GetTypeID(typeof(RotationalDeviceTracker)));
			this.IsActive = false;
			VuforiaManager.Instance.WorldCenterMode = VuforiaARController.WorldCenterMode.CAMERA;
		}

		public override void RecenterPose()
		{
			this.RecenterPoseToCurrentAnchorPointPosition(true);
		}

		public override void RecenterPose(bool resetToCurrentPose)
		{
			this.RecenterPoseToCurrentAnchorPointPosition(resetToCurrentPose);
		}

		public override void SetPosePrediction(bool mode)
		{
			VuforiaWrapper.Instance.RotationalDeviceTracker_SetPosePrediction(mode);
		}

		public override bool GetPosePrediction()
		{
			return VuforiaWrapper.Instance.RotationalDeviceTracker_GetPosePrediction() == 1;
		}

		public override void SetModelCorrectionMode(RotationalDeviceTracker.MODEL_CORRECTION_MODE mode)
		{
			VuforiaWrapper.Instance.RotationalDeviceTracker_SetModelCorrectionMode((int)mode);
		}

		public override RotationalDeviceTracker.MODEL_CORRECTION_MODE GetModelCorrectionMode()
		{
			return (RotationalDeviceTracker.MODEL_CORRECTION_MODE)VuforiaWrapper.Instance.RotationalDeviceTracker_GetModelCorrectionMode();
		}

		public override void SetModelCorrectionModeWithTransform(RotationalDeviceTracker.MODEL_CORRECTION_MODE mode, Vector3 transform)
		{
			transform.z *= -1f;
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Vector3)));
			Marshal.StructureToPtr(transform, intPtr, false);
			VuforiaWrapper.Instance.RotationalDeviceTracker_SetModelCorrectionModeWithTransform((int)mode, intPtr);
			Marshal.FreeHGlobal(intPtr);
		}

		public override Vector3 GetModelCorrectionTransform()
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Vector3)));
			VuforiaWrapper.Instance.RotationalDeviceTracker_GetModelCorrectionTransform(intPtr);
			Vector3 result = (Vector3)Marshal.PtrToStructure(intPtr, typeof(Vector3));
			Marshal.FreeHGlobal(intPtr);
			result.z *= -1f;
			return result;
		}

		private void RecenterPoseToCurrentAnchorPointPosition(bool resetToCurrentPose)
		{
			if (resetToCurrentPose)
			{
				((StateManagerImpl)TrackerManager.Instance.GetStateManager()).GetDeviceTrackingManager().RecenterPose(VuforiaManager.Instance.CentralAnchorPoint, this.GetModelCorrectionTransform());
			}
			VuforiaWrapper.Instance.RotationalDeviceTracker_Recenter();
		}
	}
}
