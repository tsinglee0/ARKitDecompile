using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
	internal abstract class BaseStereoViewerCameraConfiguration : BaseCameraConfiguration
	{
		protected readonly Camera mPrimaryCamera;

		protected readonly Camera mSecondaryCamera;

		protected bool mSkewFrustum = true;

		protected int mScreenWidth;

		protected int mScreenHeight;

		public BaseStereoViewerCameraConfiguration(Camera leftCamera, Camera rightCamera) : base(leftCamera.GetComponent<BackgroundPlaneAbstractBehaviour>())
		{
			this.mPrimaryCamera = leftCamera;
			this.mSecondaryCamera = rightCamera;
		}

		public virtual void ConfigureVideoBackground()
		{
			Device.Instance.DeleteRenderingPrimitives();
			VuforiaRenderer.VideoBGCfgData videoBackgroundConfig = VuforiaRenderer.Instance.GetVideoBackgroundConfig();
			CameraDevice.VideoModeData videoMode = CameraDevice.Instance.GetVideoMode(this.mCameraDeviceMode);
			videoBackgroundConfig.enabled = 1;
			videoBackgroundConfig.position = new VuforiaRenderer.Vec2I(0, 0);
			videoBackgroundConfig.reflection = VuforiaRenderer.VideoBackgroundReflection.OFF;
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
				float num = (float)videoMode.height * ((float)this.mScreenWidth / (float)videoMode.width);
				videoBackgroundConfig.size = new VuforiaRenderer.Vec2I(this.mScreenWidth, (int)num);
				if (videoBackgroundConfig.size.y < this.mScreenHeight)
				{
					videoBackgroundConfig.size.x = (int)((float)this.mScreenHeight * ((float)videoMode.width / (float)videoMode.height));
					videoBackgroundConfig.size.y = this.mScreenHeight;
				}
			}
			else
			{
				float num2 = (float)videoMode.height * ((float)this.mScreenHeight / (float)videoMode.width);
				videoBackgroundConfig.size = new VuforiaRenderer.Vec2I((int)num2, this.mScreenHeight);
				if (videoBackgroundConfig.size.x < this.mScreenWidth)
				{
					videoBackgroundConfig.size.x = this.mScreenWidth;
					videoBackgroundConfig.size.y = (int)((float)this.mScreenWidth * ((float)videoMode.width / (float)videoMode.height));
				}
			}
			VuforiaRenderer.InternalInstance.SetVideoBackgroundConfigInternal(videoBackgroundConfig);
			this.mLastVideoBackGroundMirroredFromSDK = VuforiaRenderer.Instance.GetVideoBackgroundConfig().reflection;
			this.UpdateProjection();
			if (this.mOnVideoBackgroundConfigChanged != null)
			{
				this.mOnVideoBackgroundConfigChanged();
			}
		}

		public virtual bool CheckForSurfaceChanges(out ScreenOrientation orientation)
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
			return result;
		}

		public void UpdateStereoDepth(Transform trackingReference)
		{
			if (this.mSkewFrustum && base.IsVideoBackgroundEnabled())
			{
				StateManager arg_26_0 = TrackerManager.Instance.GetStateManager();
				float num = float.PositiveInfinity;
				using (IEnumerator<TrackableBehaviour> enumerator = arg_26_0.GetActiveTrackableBehaviours().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Vector3 position = enumerator.Current.gameObject.transform.position;
						float z = trackingReference.InverseTransformPoint(position).z;
						if (z < num)
						{
							num = z;
						}
					}
				}
				using (Dictionary<Camera, VideoBackgroundAbstractBehaviour>.ValueCollection.Enumerator enumerator2 = this.mVideoBackgroundBehaviours.Values.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.Current.SetStereoDepth(num);
					}
				}
			}
		}

		public bool IsStereo()
		{
			return true;
		}

		public void SetSkewFrustum(bool setSkewing)
		{
			this.mSkewFrustum = setSkewing;
			if (!this.mSkewFrustum && base.IsVideoBackgroundEnabled())
			{
				using (Dictionary<Camera, VideoBackgroundAbstractBehaviour>.ValueCollection.Enumerator enumerator = this.mVideoBackgroundBehaviours.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.SetStereoDepth(float.PositiveInfinity);
					}
				}
			}
		}

		protected override void ResolveVideoBackgroundBehaviours()
		{
			this.mVideoBackgroundBehaviours.Clear();
			this.mVideoBackgroundBehaviours.Add(this.mPrimaryCamera, this.mPrimaryCamera.GetComponent<VideoBackgroundAbstractBehaviour>());
			this.mVideoBackgroundBehaviours.Add(this.mSecondaryCamera, this.mSecondaryCamera.GetComponent<VideoBackgroundAbstractBehaviour>());
		}

		protected abstract void UpdateProjection();

		protected void ComputeViewPortRect(int leftCameraViewPortHeight, int leftCameraViewPortWidth)
		{
			float num = CameraConfigurationUtility.ExtractVerticalCameraFoV(this.mPrimaryCamera.projectionMatrix.inverse) * 0.0174532924f;
			float num2 = (float)(Math.Tan((double)(CameraDevice.Instance.GetCameraFieldOfViewRads().y / 2f)) / Math.Tan((double)(num / 2f)));
			float num3 = (float)leftCameraViewPortHeight * num2;
			CameraDevice.VideoModeData videoMode = CameraDevice.Instance.GetVideoMode(this.mCameraDeviceMode);
			ScreenOrientation surfaceOrientation = SurfaceUtilities.GetSurfaceOrientation();
			float num4;
			if (surfaceOrientation == ScreenOrientation.Landscape || surfaceOrientation == ScreenOrientation.LandscapeLeft || surfaceOrientation == ScreenOrientation.LandscapeRight)
			{
				num4 = num3 * (float)videoMode.width / (float)videoMode.height;
			}
			else
			{
				num4 = num3 * (float)videoMode.height / (float)videoMode.width;
			}
			int num5 = (leftCameraViewPortWidth - (int)num4) / 2;
			int num6 = (leftCameraViewPortHeight - (int)num3) / 2;
			this.mVideoBackgroundViewportRect = new Rect((float)num5, (float)num6, (float)((int)num4), (float)((int)num3));
			if (base.IsVideoBackgroundEnabled())
			{
				this.mVideoBackgroundBehaviours[this.mPrimaryCamera].ApplyStereoDepthToMatrices();
				this.mVideoBackgroundBehaviours[this.mSecondaryCamera].ApplyStereoDepthToMatrices();
			}
		}
	}
}
