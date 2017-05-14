using System;
using UnityEngine;

namespace Vuforia
{
	internal class DedicatedEyewearDevice : EyewearDevice
	{
		private EyewearCalibrationProfileManager mProfileManager;

		private EyewearUserCalibrator mCalibrator;

		public override bool IsSeeThru()
		{
			return VuforiaWrapper.Instance.EyewearDeviceIsSeeThru() == 1;
		}

		public override bool IsDualDisplay()
		{
			return VuforiaWrapper.Instance.EyewearDeviceIsDualDisplay() == 1;
		}

		public override bool SetDisplayExtended(bool enable)
		{
			return VuforiaWrapper.Instance.EyewearDeviceSetDisplayExtended(enable) == 1;
		}

		public override bool IsDisplayExtended()
		{
			return VuforiaWrapper.Instance.EyewearDeviceIsDisplayExtended() == 1;
		}

		public override bool IsDisplayExtendedGLOnly()
		{
			return VuforiaWrapper.Instance.EyewearDeviceIsDisplayExtendedGLOnly() == 1;
		}

		public override ScreenOrientation GetScreenOrientation()
		{
			switch (VuforiaWrapper.Instance.EyewearDeviceGetScreenOrientation())
			{
			case 1:
				return ScreenOrientation.Portrait;
			case 2:
				return ScreenOrientation.Landscape;
			case 3:
				return ScreenOrientation.LandscapeRight;
			default:
				return 0;
			}
		}

		public override bool SetPredictiveTracking(bool enable)
		{
			return VuforiaWrapper.Instance.EyewearDeviceSetPredictiveTracking(enable) == 1;
		}

		public override bool IsPredictiveTrackingEnabled()
		{
			return VuforiaWrapper.Instance.EyewearDeviceIsPredictiveTrackingEnabled() == 1;
		}

		public override EyewearCalibrationProfileManager GetCalibrationProfileManager()
		{
			if (this.mProfileManager == null)
			{
				lock (this)
				{
					if (this.mProfileManager == null)
					{
						this.mProfileManager = new EyewearCalibrationProfileManagerImpl();
					}
				}
			}
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
						this.mCalibrator = new EyewearUserCalibratorImpl();
					}
				}
			}
			return this.mCalibrator;
		}
	}
}
