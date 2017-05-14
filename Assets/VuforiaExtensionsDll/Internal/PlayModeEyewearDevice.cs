using System;
using UnityEngine;

namespace Vuforia
{
	internal class PlayModeEyewearDevice : EyewearDevice
	{
		private EyewearCalibrationProfileManager mProfileManager;

		private EyewearUserCalibrator mCalibrator;

		private bool mDummyPredictiveTracking;

		public override bool IsSeeThru()
		{
			return false;
		}

		public override bool IsDualDisplay()
		{
			return true;
		}

		public override bool SetDisplayExtended(bool enable)
		{
			return enable;
		}

		public override bool IsDisplayExtended()
		{
			return true;
		}

		public override bool IsDisplayExtendedGLOnly()
		{
			return false;
		}

		public override ScreenOrientation GetScreenOrientation()
		{
			return ScreenOrientation.Landscape;
		}

		public override bool SetPredictiveTracking(bool enable)
		{
			this.mDummyPredictiveTracking = enable;
			return true;
		}

		public override bool IsPredictiveTrackingEnabled()
		{
			return this.mDummyPredictiveTracking;
		}

		public override EyewearCalibrationProfileManager GetCalibrationProfileManager()
		{
			if (this.mProfileManager == null)
			{
				lock (this)
				{
					if (this.mProfileManager == null)
					{
						this.mProfileManager = new PlayModeEyewearCalibrationProfileManagerImpl();
					}
				}
			}
			Debug.LogWarning("Usage of the EyewearrCalibrationProfileManager class is not supported in Play Mode");
			return this.mProfileManager;
		}

		public override EyewearUserCalibrator GetUserCalibrator()
		{
			if (this.mCalibrator == null)
			{
				lock (this)
				{
					if (this.mCalibrator == null)
					{
						this.mCalibrator = new PlayModeEyewearUserCalibratorImpl();
					}
				}
			}
			Debug.LogWarning("Usage of the EyewearUserCalibrator class is not supported in Play Mode");
			return this.mCalibrator;
		}
	}
}
