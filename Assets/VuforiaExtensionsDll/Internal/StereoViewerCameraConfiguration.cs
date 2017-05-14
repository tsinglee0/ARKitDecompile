using System;
using UnityEngine;

namespace Vuforia
{
	internal class StereoViewerCameraConfiguration : BaseStereoViewerCameraConfiguration, ICameraConfiguration
	{
		private const float TOLERANCE = 0.01f;

		private float mLastAppliedLeftNearClipPlane;

		private float mLastAppliedLeftFarClipPlane;

		private float mLastAppliedRightNearClipPlane;

		private float mLastAppliedRightFarClipPlane;

		private float mCameraOffset;

		private bool mIsDistorted;

		public StereoViewerCameraConfiguration(Camera leftCamera, Camera rightCamera, float cameraOffset, bool isDistorted) : base(leftCamera, rightCamera)
		{
			this.mCameraOffset = cameraOffset;
			this.mIsDistorted = isDistorted;
		}

		public void ApplyCorrectedProjectionMatrix(Matrix4x4 projectionMatrix, bool primaryCamera)
		{
			Debug.LogWarning("Cannot set external projection with Vuforia VR system");
		}

		public void SetDistortion(bool isDistorted)
		{
			this.mIsDistorted = isDistorted;
		}

		public override void ConfigureVideoBackground()
		{
			float num = (float)this.mScreenWidth / (float)this.mScreenHeight;
			if (VuforiaRuntimeUtilities.IsPlayMode() && (num < 1.49f || num > 1.8f))
			{
				Debug.LogWarning("Set your game view window to a resolution with an aspect between 3:2 and 16:9 landscape for best stereo play mode experience.");
			}
			base.ConfigureVideoBackground();
		}

		protected override bool CameraParameterChanged()
		{
			return base.CameraParameterChanged() || Math.Abs(this.mPrimaryCamera.nearClipPlane - this.mLastAppliedLeftNearClipPlane) > 0.01f || Math.Abs(this.mPrimaryCamera.farClipPlane - this.mLastAppliedLeftFarClipPlane) > 0.01f || Math.Abs(this.mSecondaryCamera.nearClipPlane - this.mLastAppliedRightNearClipPlane) > 0.01f || Math.Abs(this.mSecondaryCamera.farClipPlane - this.mLastAppliedRightFarClipPlane) > 0.01f;
		}

		protected override void UpdateProjection()
		{
			if (!VuforiaRuntimeUtilities.IsVuforiaEnabled())
			{
				return;
			}
			Device instance = Device.Instance;
			this.mLastAppliedLeftNearClipPlane = this.mPrimaryCamera.nearClipPlane;
			this.mLastAppliedLeftFarClipPlane = this.mPrimaryCamera.farClipPlane;
			this.mLastAppliedRightNearClipPlane = this.mSecondaryCamera.nearClipPlane;
			this.mLastAppliedRightFarClipPlane = this.mSecondaryCamera.farClipPlane;
			Matrix4x4 projectionMatrix = instance.GetProjectionMatrix(View.VIEW_LEFTEYE, this.mLastAppliedLeftNearClipPlane, this.mLastAppliedLeftFarClipPlane, this.mProjectionOrientation);
			Matrix4x4 projectionMatrix2 = instance.GetProjectionMatrix(View.VIEW_RIGHTEYE, this.mLastAppliedRightNearClipPlane, this.mLastAppliedRightFarClipPlane, this.mProjectionOrientation);
			this.mPrimaryCamera.transform.localPosition = new Vector3(-0.5f * this.mCameraOffset, 0f, 0f);
			this.mSecondaryCamera.transform.localPosition = new Vector3(0.5f * this.mCameraOffset, 0f, 0f);
			int width;
			int height;
			if (this.mIsDistorted)
			{
				instance.GetTextureSize(View.VIEW_POSTPROCESS, out width, out height);
			}
			else
			{
				width = Screen.width;
				height = Screen.height;
			}
			this.mPrimaryCamera.rect = instance.GetNormalizedViewport(View.VIEW_LEFTEYE);
			this.mSecondaryCamera.rect = instance.GetNormalizedViewport(View.VIEW_RIGHTEYE);
			int num = (int)((float)width * this.mPrimaryCamera.rect.width);
			int num2 = (int)((float)height * this.mPrimaryCamera.rect.height);
			int num3 = (int)((float)width * this.mSecondaryCamera.rect.width);
			int num4 = (int)((float)height * this.mSecondaryCamera.rect.height);
			Vector4 effectiveFovRads = Device.Instance.GetEffectiveFovRads(View.VIEW_LEFTEYE);
			Vector4 effectiveFovRads2 = Device.Instance.GetEffectiveFovRads(View.VIEW_RIGHTEYE);
			float num5 = (effectiveFovRads.x + effectiveFovRads.y) * 57.29578f;
			float targetVerticalFoVDeg = CameraConfigurationUtility.CalculateVerticalFoVFromViewPortAspect(num5, (float)num / (float)num2);
			float num6 = (effectiveFovRads2.x + effectiveFovRads2.y) * 57.29578f;
			float targetVerticalFoVDeg2 = CameraConfigurationUtility.CalculateVerticalFoVFromViewPortAspect(num6, (float)num3 / (float)num4);
			this.mPrimaryCamera.projectionMatrix = CameraConfigurationUtility.ScalePerspectiveProjectionMatrix(projectionMatrix, targetVerticalFoVDeg, num5);
			this.mSecondaryCamera.projectionMatrix = CameraConfigurationUtility.ScalePerspectiveProjectionMatrix(projectionMatrix2, targetVerticalFoVDeg2, num6);
			Vector2 skewingValues = new Vector2(this.mPrimaryCamera.projectionMatrix[0, 2], this.mPrimaryCamera.projectionMatrix[1, 2]);
			Vector2 viewportCentreToEyeAxis = instance.GetViewportCentreToEyeAxis(View.VIEW_LEFTEYE);
			this.mVideoBackgroundBehaviours[this.mPrimaryCamera].SetVuforiaFrustumSkewValues(skewingValues, viewportCentreToEyeAxis);
			Vector2 skewingValues2 = new Vector2(this.mSecondaryCamera.projectionMatrix[0, 2], this.mSecondaryCamera.projectionMatrix[1, 2]);
			Vector2 viewportCentreToEyeAxis2 = instance.GetViewportCentreToEyeAxis(View.VIEW_RIGHTEYE);
			this.mVideoBackgroundBehaviours[this.mSecondaryCamera].SetVuforiaFrustumSkewValues(skewingValues2, viewportCentreToEyeAxis2);
			CameraConfigurationUtility.SetFovForCustomProjection(this.mPrimaryCamera);
			CameraConfigurationUtility.SetFovForCustomProjection(this.mSecondaryCamera);
			base.ComputeViewPortRect(num2, num);
		}
	}
}
