using System;
using UnityEngine;

namespace Vuforia
{
	public abstract class RotationalDeviceTracker : DeviceTracker
	{
		public enum MODEL_CORRECTION_MODE
		{
			NONE,
			HEAD,
			HANDHELD
		}

		public abstract void RecenterPose();

		public abstract void RecenterPose(bool resetToCurrentPose);

		public abstract void SetPosePrediction(bool mode);

		public abstract bool GetPosePrediction();

		public abstract void SetModelCorrectionMode(RotationalDeviceTracker.MODEL_CORRECTION_MODE mode);

		public abstract RotationalDeviceTracker.MODEL_CORRECTION_MODE GetModelCorrectionMode();

		public abstract void SetModelCorrectionModeWithTransform(RotationalDeviceTracker.MODEL_CORRECTION_MODE mode, Vector3 transform);

		public abstract Vector3 GetModelCorrectionTransform();
	}
}
