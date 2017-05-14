using System;
using UnityEngine;

namespace Vuforia
{
	public abstract class EyewearUserCalibrator
	{
		public abstract bool init(int surfaceWidth, int surfaceHeight, int targetWidth, int targetHeight);

		public abstract float getMinScaleHint();

		public abstract float getMaxScaleHint();

		public abstract bool isStereoStretched();

		public abstract bool getProjectionMatrix(EyewearDevice.EyewearCalibrationReading[] readings, out Matrix4x4 cameraToEyeMatrix, out Matrix4x4 projectionMatrix);
	}
}
