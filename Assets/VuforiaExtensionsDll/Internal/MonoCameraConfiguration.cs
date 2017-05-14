using System;
using UnityEngine;

namespace Vuforia
{
	internal class MonoCameraConfiguration : BaseCameraConfiguration, ICameraConfiguration
	{
		private const float TOLERANCE = 0.01f;

		private readonly Camera mPrimaryCamera;

		private int mCameraViewPortWidth;

		private int mCameraViewPortHeight;

		private float mLastAppliedNearClipPlane;

		private float mLastAppliedFarClipPlane;

		private float mLastAppliedFoV;

		public MonoCameraConfiguration(Camera leftCamera) : base(leftCamera.GetComponent<BackgroundPlaneAbstractBehaviour>())
		{
			this.mPrimaryCamera = leftCamera;
		}

		public virtual void ConfigureVideoBackground()
		{
			Device.Instance.DeleteRenderingPrimitives();
			VuforiaRenderer.VideoBGCfgData videoBackgroundConfig = VuforiaRenderer.Instance.GetVideoBackgroundConfig();
			CameraDevice.VideoModeData videoMode = CameraDevice.Instance.GetVideoMode(this.mCameraDeviceMode);
			videoBackgroundConfig.enabled = 1;
			videoBackgroundConfig.position = new VuforiaRenderer.Vec2I(0, 0);
			videoBackgroundConfig.reflection = VuforiaRenderer.InternalInstance.GetLastSetReflection();
			bool flag;
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				flag = true;
			}
			else
			{
				ScreenOrientation surfaceOrientation = SurfaceUtilities.GetSurfaceOrientation();
				flag = (surfaceOrientation == ScreenOrientation.Landscape || surfaceOrientation == ScreenOrientation.LandscapeLeft || surfaceOrientation == ScreenOrientation.LandscapeRight);
			}
			if (flag)
			{
				float num = (float)videoMode.height * ((float)this.mCameraViewPortWidth / (float)videoMode.width);
				videoBackgroundConfig.size = new VuforiaRenderer.Vec2I(this.mCameraViewPortWidth, (int)num);
				if (videoBackgroundConfig.size.y < this.mCameraViewPortHeight)
				{
					videoBackgroundConfig.size.x = (int)((float)this.mCameraViewPortHeight * ((float)videoMode.width / (float)videoMode.height));
					videoBackgroundConfig.size.y = this.mCameraViewPortHeight;
				}
			}
			else
			{
				float num2 = (float)videoMode.height * ((float)this.mCameraViewPortHeight / (float)videoMode.width);
				videoBackgroundConfig.size = new VuforiaRenderer.Vec2I((int)num2, this.mCameraViewPortHeight);
				if (videoBackgroundConfig.size.x < this.mCameraViewPortWidth)
				{
					videoBackgroundConfig.size.x = this.mCameraViewPortWidth;
					videoBackgroundConfig.size.y = (int)((float)this.mCameraViewPortWidth * ((float)videoMode.width / (float)videoMode.height));
				}
			}
			VuforiaRenderer.InternalInstance.SetVideoBackgroundConfigInternal(videoBackgroundConfig);
			int num3 = videoBackgroundConfig.position.x + (this.mCameraViewPortWidth - videoBackgroundConfig.size.x) / 2;
			int num4 = videoBackgroundConfig.position.y + (this.mCameraViewPortHeight - videoBackgroundConfig.size.y) / 2;
			this.mVideoBackgroundViewportRect = new Rect((float)num3, (float)num4, (float)videoBackgroundConfig.size.x, (float)videoBackgroundConfig.size.y);
			this.mLastVideoBackGroundMirroredFromSDK = VuforiaRenderer.Instance.GetVideoBackgroundConfig().reflection;
			this.UpdateProjection();
			if (this.mOnVideoBackgroundConfigChanged != null)
			{
				this.mOnVideoBackgroundConfigChanged();
			}
		}

		public bool CheckForSurfaceChanges(out ScreenOrientation orientation)
		{
			int pixelHeightInt = this.mPrimaryCamera.GetPixelHeightInt();
			int pixelWidthInt = this.mPrimaryCamera.GetPixelWidthInt();
			ScreenOrientation surfaceOrientation = SurfaceUtilities.GetSurfaceOrientation();
			bool result = false;
			if (pixelHeightInt != this.mCameraViewPortHeight || pixelWidthInt != this.mCameraViewPortWidth || this.mProjectionOrientation != surfaceOrientation)
			{
				this.mCameraViewPortHeight = pixelHeightInt;
				this.mCameraViewPortWidth = pixelWidthInt;
				this.mProjectionOrientation = surfaceOrientation;
				SurfaceUtilities.OnSurfaceChanged(this.mCameraViewPortWidth, this.mCameraViewPortHeight);
			}
			CameraDeviceImpl cameraDeviceImpl = (CameraDeviceImpl)CameraDevice.Instance;
			if (cameraDeviceImpl.CameraReady && (VuforiaUnity.IsRendererDirty() || this.CameraParameterChanged()))
			{
				this.ConfigureVideoBackground();
				cameraDeviceImpl.ResetDirtyFlag();
				result = true;
				this.mCameraParameterChanged = false;
			}
			orientation = this.mProjectionOrientation;
			return result;
		}

		public void UpdateStereoDepth(Transform trackingReference)
		{
		}

		public bool IsStereo()
		{
			return false;
		}

		public void ApplyCorrectedProjectionMatrix(Matrix4x4 projectionMatrix, bool primaryCamera)
		{
			Debug.LogWarning("Cannot set external projection for single camera rendering.");
		}

		public void SetSkewFrustum(bool setSkewing)
		{
		}

		private void UpdateProjection()
		{
			if (!VuforiaRuntimeUtilities.IsVuforiaEnabled())
			{
				return;
			}
			this.mLastAppliedNearClipPlane = this.mPrimaryCamera.nearClipPlane;
			this.mLastAppliedFarClipPlane = this.mPrimaryCamera.farClipPlane;
			this.mLastAppliedFoV = this.mPrimaryCamera.fieldOfView;
			Device instance = Device.Instance;
			this.mPrimaryCamera.projectionMatrix = instance.GetProjectionMatrix(View.VIEW_SINGULAR, this.mLastAppliedNearClipPlane, this.mLastAppliedFarClipPlane, this.mProjectionOrientation);
			if (Device.Instance.GetMode() == Device.Mode.MODE_VR)
			{
				float targetHorizontalFoVDeg = CameraConfigurationUtility.CalculateHorizontalFoVFromViewPortAspect(this.mLastAppliedFoV, (float)this.mCameraViewPortWidth / (float)this.mCameraViewPortHeight);
				this.mPrimaryCamera.projectionMatrix = CameraConfigurationUtility.ScalePerspectiveProjectionMatrix(this.mPrimaryCamera.projectionMatrix, this.mLastAppliedFoV, targetHorizontalFoVDeg);
			}
			else
			{
				CameraConfigurationUtility.SetFovForCustomProjection(this.mPrimaryCamera);
				this.mLastAppliedFoV = this.mPrimaryCamera.fieldOfView;
			}
			this.mPrimaryCamera.transform.localPosition = new Vector3(0f, 0f, 0f);
			this.mPrimaryCamera.transform.localRotation = Quaternion.identity;
		}

		protected override void ResolveVideoBackgroundBehaviours()
		{
			this.mVideoBackgroundBehaviours.Clear();
			this.mVideoBackgroundBehaviours.Add(this.mPrimaryCamera, this.mPrimaryCamera.GetComponent<VideoBackgroundAbstractBehaviour>());
		}

		protected override bool CameraParameterChanged()
		{
			return base.CameraParameterChanged() || Math.Abs(this.mPrimaryCamera.nearClipPlane - this.mLastAppliedNearClipPlane) > 0.01f || Math.Abs(this.mPrimaryCamera.farClipPlane - this.mLastAppliedFarClipPlane) > 0.01f || Math.Abs(this.mPrimaryCamera.fieldOfView - this.mLastAppliedFoV) > 0.01f;
		}
	}
}
