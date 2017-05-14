using System;
using UnityEngine;

namespace Vuforia
{
	internal class DedicatedEyewearCameraConfiguration : BaseCameraConfiguration, ICameraConfiguration
	{
		private const float TOLERANCE = 0.01f;

		private readonly Camera mPrimaryCamera;

		private readonly Camera mSecondaryCamera;

		private int mScreenWidth;

		private int mScreenHeight;

		private bool mNeedToCheckStereo;

		private float mLastAppliedNearClipPlane;

		private float mLastAppliedFarClipPlane;

		private float mLastAppliedVirtualFoV;

		private float mNewNearClipPlane;

		private float mNewFarClipPlane;

		private float mNewVirtualFoV;

		private EyewearDevice mEyewearDevice;

		public DedicatedEyewearCameraConfiguration(Camera leftCamera, Camera rightCamera) : base(leftCamera.GetComponent<BackgroundPlaneAbstractBehaviour>())
		{
			this.mPrimaryCamera = leftCamera;
			this.mSecondaryCamera = rightCamera;
			this.mNewNearClipPlane = leftCamera.nearClipPlane;
			this.mNewFarClipPlane = leftCamera.farClipPlane;
			this.mNewVirtualFoV = leftCamera.fieldOfView;
			this.mEyewearDevice = (Device.Instance as EyewearDevice);
		}

		public override void Init()
		{
			this.mNeedToCheckStereo = true;
			Debug.Log("Detected supported Eyewear device");
			if (this.mEyewearDevice.IsSeeThru())
			{
				Debug.Log("Disabling video background rendering");
				this.mRenderVideoBackground = false;
			}
			else
			{
				this.mRenderVideoBackground = true;
			}
			Screen.autorotateToPortrait = false;
			Screen.autorotateToPortraitUpsideDown = false;
			Screen.autorotateToLandscapeLeft = false;
			Screen.autorotateToLandscapeRight = false;
			ScreenOrientation screenOrientation = this.mEyewearDevice.GetScreenOrientation();
			if (screenOrientation == ScreenOrientation.Unknown)
			{
				screenOrientation = ScreenOrientation.Landscape;
				Debug.Log("Unknown screen orientation returned for Eyewear; defaulting to " + screenOrientation);
			}
			Screen.orientation = screenOrientation;
			Debug.Log("Set screen orientation for Eyewear to " + Screen.orientation.ToString());
			base.Init();
		}

		public void ConfigureVideoBackground()
		{
			Device.Instance.DeleteRenderingPrimitives();
			VuforiaRenderer.VideoBGCfgData videoBackgroundConfig = VuforiaRenderer.Instance.GetVideoBackgroundConfig();
			CameraDevice.VideoModeData videoMode = CameraDevice.Instance.GetVideoMode(this.mCameraDeviceMode);
			videoBackgroundConfig.enabled = 1;
			videoBackgroundConfig.position = new VuforiaRenderer.Vec2I(0, 0);
			float num = (float)this.mScreenHeight;
			float num2 = (float)this.mScreenWidth;
			if (!this.mEyewearDevice.IsSeeThru())
			{
				ScreenOrientation surfaceOrientation = SurfaceUtilities.GetSurfaceOrientation();
				if (surfaceOrientation == ScreenOrientation.Landscape || surfaceOrientation == ScreenOrientation.LandscapeLeft || surfaceOrientation == ScreenOrientation.LandscapeRight)
				{
					num2 = num * (float)videoMode.width / (float)videoMode.height;
				}
				else
				{
					num2 = num * (float)videoMode.height / (float)videoMode.width;
				}
			}
			videoBackgroundConfig.size = new VuforiaRenderer.Vec2I((int)num2, (int)num);
			videoBackgroundConfig.reflection = VuforiaRenderer.VideoBackgroundReflection.OFF;
			VuforiaRenderer.InternalInstance.SetVideoBackgroundConfigInternal(videoBackgroundConfig);
			int num3 = videoBackgroundConfig.position.x + (this.mScreenWidth - videoBackgroundConfig.size.x) / 2;
			int num4 = videoBackgroundConfig.position.y + (this.mScreenHeight - videoBackgroundConfig.size.y) / 2;
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
			ScreenOrientation surfaceOrientation = SurfaceUtilities.GetSurfaceOrientation();
			bool result = false;
			if (Screen.height != this.mScreenHeight || Screen.width != this.mScreenWidth || this.mProjectionOrientation != surfaceOrientation)
			{
				this.mScreenWidth = Screen.width;
				this.mScreenHeight = Screen.height;
				this.mProjectionOrientation = surfaceOrientation;
				SurfaceUtilities.OnSurfaceChanged(this.mScreenWidth, this.mScreenHeight);
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
			if (Math.Abs(this.mSecondaryCamera.fieldOfView - this.mPrimaryCamera.fieldOfView) > 0.01f)
			{
				Debug.LogWarning("Field of view values of left and right camera are not identical. This is currently not supported by Vuforia!");
			}
			if (Math.Abs(this.mSecondaryCamera.nearClipPlane - this.mPrimaryCamera.nearClipPlane) > 0.01f || Math.Abs(this.mSecondaryCamera.farClipPlane - this.mPrimaryCamera.farClipPlane) > 0.01f)
			{
				Debug.LogWarning("Clip plane positions of left and right camera are not identical. This is currently not supported by Vuforia!");
			}
			if (this.mNeedToCheckStereo && this.mEyewearDevice.IsDualDisplay() && !this.mEyewearDevice.IsDisplayExtended())
			{
				if (this.mSecondaryCamera != null)
				{
					Debug.Log("Detecting stereo camera setup, setting stereo mode.");
					this.mEyewearDevice.SetDisplayExtended(true);
				}
				else
				{
					Debug.LogWarning("Device is a stereo capable eyewear device, but only one camera has been set up.");
				}
				this.mNeedToCheckStereo = false;
			}
			return result;
		}

		public void UpdateStereoDepth(Transform trackingReference)
		{
		}

		public bool IsStereo()
		{
			return true;
		}

		public virtual void ApplyCorrectedProjectionMatrix(Matrix4x4 projectionMatrix, bool primaryCamera)
		{
			Debug.LogWarning("Cannot set external projection dedicated eyewear");
		}

		protected override void ResolveVideoBackgroundBehaviours()
		{
			this.mVideoBackgroundBehaviours.Clear();
			this.mVideoBackgroundBehaviours.Add(this.mPrimaryCamera, this.mPrimaryCamera.GetComponent<VideoBackgroundAbstractBehaviour>());
			this.mVideoBackgroundBehaviours.Add(this.mSecondaryCamera, this.mSecondaryCamera.GetComponent<VideoBackgroundAbstractBehaviour>());
		}

		protected override bool CameraParameterChanged()
		{
			return base.CameraParameterChanged() || Math.Abs(this.mNewNearClipPlane - this.mLastAppliedNearClipPlane) > 0.01f || Math.Abs(this.mNewFarClipPlane - this.mLastAppliedFarClipPlane) > 0.01f;
		}

		private void UpdateProjection()
		{
			if (!VuforiaRuntimeUtilities.IsVuforiaEnabled())
			{
				return;
			}
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				Matrix4x4 projectionGL = VuforiaUnity.GetProjectionGL(this.mPrimaryCamera.nearClipPlane, this.mPrimaryCamera.farClipPlane, this.mProjectionOrientation);
				this.ApplyMatrix(this.mPrimaryCamera, projectionGL);
				this.ApplyMatrix(this.mSecondaryCamera, projectionGL);
				return;
			}
			Device instance = Device.Instance;
			this.mLastAppliedNearClipPlane = this.mNewNearClipPlane;
			this.mLastAppliedFarClipPlane = this.mNewFarClipPlane;
			Matrix4x4 projectionMatrix = instance.GetProjectionMatrix(View.VIEW_LEFTEYE, this.mPrimaryCamera.nearClipPlane, this.mPrimaryCamera.farClipPlane, this.mProjectionOrientation);
			Matrix4x4 projectionMatrix2 = instance.GetProjectionMatrix(View.VIEW_RIGHTEYE, this.mPrimaryCamera.nearClipPlane, this.mPrimaryCamera.farClipPlane, this.mProjectionOrientation);
			Matrix4x4 eyeDisplayAdjustmentMatrix = instance.GetEyeDisplayAdjustmentMatrix(View.VIEW_LEFTEYE);
			Matrix4x4 eyeDisplayAdjustmentMatrix2 = instance.GetEyeDisplayAdjustmentMatrix(View.VIEW_RIGHTEYE);
			EyewearDevice eyewearDevice = Device.Instance as EyewearDevice;
			if (eyewearDevice != null && eyewearDevice.IsSeeThru())
			{
				DedicatedEyewearCameraConfiguration.SetProjectionAndOffset(this.mPrimaryCamera, projectionMatrix, eyeDisplayAdjustmentMatrix);
				DedicatedEyewearCameraConfiguration.SetProjectionAndOffset(this.mSecondaryCamera, projectionMatrix2, eyeDisplayAdjustmentMatrix2);
			}
			else
			{
				this.mPrimaryCamera.projectionMatrix = projectionMatrix;
				this.mSecondaryCamera.projectionMatrix = projectionMatrix2;
				this.mPrimaryCamera.transform.localPosition = -eyeDisplayAdjustmentMatrix.GetColumn(3);
				this.mPrimaryCamera.transform.localRotation = StateManagerImpl.ExtractRotationFromMatrix(eyeDisplayAdjustmentMatrix);
				this.mSecondaryCamera.transform.localPosition = -eyeDisplayAdjustmentMatrix2.GetColumn(3);
				this.mSecondaryCamera.transform.localRotation = StateManagerImpl.ExtractRotationFromMatrix(eyeDisplayAdjustmentMatrix2);
			}
			this.mPrimaryCamera.rect = instance.GetNormalizedViewport(View.VIEW_LEFTEYE);
			this.mSecondaryCamera.rect = instance.GetNormalizedViewport(View.VIEW_RIGHTEYE);
		}

		private static void SetProjectionAndOffset(Camera cam, Matrix4x4 proj, Matrix4x4 adjustment)
		{
			Matrix4x4 identity = Matrix4x4.identity;
			identity[1, 1] = -1f;
			identity[2, 2] = -1f;
			Matrix4x4 inverse = (proj * identity.inverse * adjustment * identity).inverse;
			Vector3 vector = inverse.MultiplyPoint(new Vector3(0f, 0f, -1f));
			Vector3 vector2 = inverse.MultiplyPoint(new Vector3(0f, 0f, 1f));
			Vector3 vector3 = inverse.MultiplyPoint(new Vector3(0f, 1f, -1f));
			Vector3 vector4 = new Vector3(vector.x, vector.y, -vector.z);
			Vector3 vector5 = new Vector3(vector2.x, vector2.y, -vector2.z);
			Vector3 arg_102_0 = new Vector3(vector3.x, vector3.y, -vector3.z);
			Vector3 normalized = (vector5 - vector4).normalized;
			Vector3 normalized2 = (arg_102_0 - vector4).normalized;
			Vector3 localPosition = vector4 + normalized * -cam.nearClipPlane;
			cam.transform.localPosition = localPosition;
			cam.transform.localRotation = Quaternion.LookRotation(normalized, normalized2);
			cam.projectionMatrix = proj;
		}

		private void ApplyMatrix(Camera cam, Matrix4x4 inputMatrix)
		{
			if (this.mVideoBackgroundViewportRect.width != (float)this.mScreenWidth)
			{
				float num = this.mVideoBackgroundViewportRect.width / (float)this.mScreenWidth * 2f;
				inputMatrix[0] = inputMatrix[0] * num;
				inputMatrix[8] = inputMatrix[8] * num;
			}
			if (this.mVideoBackgroundViewportRect.height != (float)this.mScreenHeight)
			{
				float num2 = this.mVideoBackgroundViewportRect.height / (float)this.mScreenHeight;
				inputMatrix[5] = inputMatrix[5] * num2;
				inputMatrix[9] = inputMatrix[9] * num2;
			}
			cam.projectionMatrix = inputMatrix;
			CameraConfigurationUtility.SetFovForCustomProjection(cam);
		}

		public void SetSkewFrustum(bool setSkewing)
		{
		}
	}
}
