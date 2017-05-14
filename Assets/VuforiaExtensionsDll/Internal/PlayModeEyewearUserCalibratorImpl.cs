using System;
using UnityEngine;

namespace Vuforia
{
	internal class PlayModeEyewearUserCalibratorImpl : EyewearUserCalibrator
	{
		public override bool init(int surfaceWidth, int surfaceHeight, int targetWidth, int targetHeight)
		{
			return true;
		}

		public override float getMinScaleHint()
		{
			return 0f;
		}

		public override float getMaxScaleHint()
		{
			return 1f;
		}

		public override bool isStereoStretched()
		{
			return true;
		}

		public override bool getProjectionMatrix(EyewearDevice.EyewearCalibrationReading[] readings, out Matrix4x4 cameraToEyeMatrix, out Matrix4x4 projectionMatrix)
		{
			cameraToEyeMatrix = Matrix4x4.zero;
			projectionMatrix = Matrix4x4.zero;
			return false;
		}
	}
}
