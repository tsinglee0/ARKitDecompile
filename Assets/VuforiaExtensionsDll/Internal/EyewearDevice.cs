using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	public abstract class EyewearDevice : Device
	{
		public enum EyeID
		{
			EYEID_MONOCULAR,
			EYEID_LEFT,
			EYEID_RIGHT
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct EyewearCalibrationReading
		{
			public float[] pose;

			public float scale;

			public float centerX;

			public float centerY;

			public int unused;
		}

		public abstract bool IsSeeThru();

		public abstract bool IsDualDisplay();

		public abstract bool SetDisplayExtended(bool enable);

		public abstract bool IsDisplayExtended();

		public abstract bool IsDisplayExtendedGLOnly();

		public abstract ScreenOrientation GetScreenOrientation();

		public abstract bool SetPredictiveTracking(bool enable);

		public abstract bool IsPredictiveTrackingEnabled();

		public abstract EyewearCalibrationProfileManager GetCalibrationProfileManager();

		public abstract EyewearUserCalibrator GetUserCalibrator();
	}
}
