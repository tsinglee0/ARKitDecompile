using System;
using UnityEngine;

namespace Vuforia
{
	internal interface ICameraConfiguration
	{
		VuforiaRenderer.VideoBackgroundReflection VideoBackgroundMirrored
		{
			get;
		}

		Rect VideoBackgroundViewportRect
		{
			get;
		}

		void Init();

		void InitCameraDevice(CameraDevice.CameraDeviceMode cameraDeviceMode, VuforiaRenderer.VideoBackgroundReflection mirrorVideoBackground, Action onVideoBackgroundConfigChanged);

		void ConfigureVideoBackground();

		bool CheckForSurfaceChanges(out ScreenOrientation orientation);

		void UpdateStereoDepth(Transform trackingReference);

		bool IsStereo();

		void ResetBackgroundPlane(bool disable);

		void ApplyCorrectedProjectionMatrix(Matrix4x4 projectionMatrix, bool primaryCamera);

		void SetSkewFrustum(bool setSkewing);

		void SetCameraParameterChanged();
	}
}
