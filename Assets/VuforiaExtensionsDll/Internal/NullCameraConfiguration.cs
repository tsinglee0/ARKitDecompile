using System;
using UnityEngine;

namespace Vuforia
{
	internal class NullCameraConfiguration : ICameraConfiguration
	{
		private ScreenOrientation mProjectionOrientation;

		public VuforiaRenderer.VideoBackgroundReflection VideoBackgroundMirrored
		{
			get
			{
				return VuforiaRenderer.VideoBackgroundReflection.OFF;
			}
			set
			{
			}
		}

		public Rect VideoBackgroundViewportRect
		{
			get
			{
				return default(Rect);
			}
		}

		public int EyewearUserCalibrationProfileId
		{
			get
			{
				return -1;
			}
			set
			{
			}
		}

		public void Init()
		{
		}

		public void InitCameraDevice(CameraDevice.CameraDeviceMode cameraDeviceMode, VuforiaRenderer.VideoBackgroundReflection mirrorVideoBackground, Action onVideoBackgroundConfigChanged)
		{
		}

		public void ConfigureVideoBackground()
		{
		}

		public void UpdatePlayModeParameters(WebCamARController webCamBehaviour, float cameraOffset)
		{
		}

		public bool CheckForSurfaceChanges(out ScreenOrientation orientation)
		{
			ScreenOrientation surfaceOrientation = SurfaceUtilities.GetSurfaceOrientation();
			bool expr_12 = this.mProjectionOrientation != surfaceOrientation;
			if (expr_12)
			{
				this.mProjectionOrientation = surfaceOrientation;
			}
			orientation = this.mProjectionOrientation;
			return expr_12;
		}

		public void UpdateStereoDepth(Transform trackingReference)
		{
		}

		public bool IsStereo()
		{
			return false;
		}

		public void ResetBackgroundPlane(bool disable)
		{
		}

		public void ApplyCorrectedProjectionMatrix(Matrix4x4 projectionMatrix, bool primaryCamera)
		{
		}

		public void SetSkewFrustum(bool setSkewing)
		{
		}

		public void SetCameraParameterChanged()
		{
		}
	}
}
