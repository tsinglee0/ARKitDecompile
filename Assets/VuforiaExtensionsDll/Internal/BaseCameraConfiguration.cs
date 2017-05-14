using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
	internal abstract class BaseCameraConfiguration
	{
		protected CameraDevice.CameraDeviceMode mCameraDeviceMode;

		protected VuforiaRenderer.VideoBackgroundReflection mLastVideoBackGroundMirroredFromSDK;

		protected Action mOnVideoBackgroundConfigChanged;

		protected readonly Dictionary<Camera, VideoBackgroundAbstractBehaviour> mVideoBackgroundBehaviours = new Dictionary<Camera, VideoBackgroundAbstractBehaviour>();

		protected Rect mVideoBackgroundViewportRect;

		protected bool mRenderVideoBackground = true;

		protected ScreenOrientation mProjectionOrientation;

		private VuforiaRenderer.VideoBackgroundReflection mInitialReflection;

		private BackgroundPlaneAbstractBehaviour mBackgroundPlaneBehaviour;

		protected bool mCameraParameterChanged;

		public virtual VuforiaRenderer.VideoBackgroundReflection VideoBackgroundMirrored
		{
			get
			{
				if (this.mLastVideoBackGroundMirroredFromSDK == VuforiaRenderer.VideoBackgroundReflection.DEFAULT)
				{
					return VuforiaRenderer.Instance.GetVideoBackgroundConfig().reflection;
				}
				return this.mLastVideoBackGroundMirroredFromSDK;
			}
		}

		public virtual Rect VideoBackgroundViewportRect
		{
			get
			{
				return this.mVideoBackgroundViewportRect;
			}
		}

		protected BaseCameraConfiguration(BackgroundPlaneAbstractBehaviour bgpBehaviour)
		{
			this.mBackgroundPlaneBehaviour = bgpBehaviour;
		}

		public void InitCameraDevice(CameraDevice.CameraDeviceMode cameraDeviceMode, VuforiaRenderer.VideoBackgroundReflection mirrorVideoBackground, Action onVideoBackgroundConfigChanged)
		{
			this.mCameraDeviceMode = cameraDeviceMode;
			this.mInitialReflection = mirrorVideoBackground;
			this.mOnVideoBackgroundConfigChanged = onVideoBackgroundConfigChanged;
		}

		public virtual void Init()
		{
			this.ResolveVideoBackgroundBehaviours();
			if (this.mBackgroundPlaneBehaviour != null)
			{
				this.EnableObjectRenderer(this.mBackgroundPlaneBehaviour.gameObject, this.IsVideoBackgroundEnabled());
			}
			VuforiaRenderer.VideoBGCfgData videoBackgroundConfig = VuforiaRenderer.Instance.GetVideoBackgroundConfig();
			videoBackgroundConfig.reflection = this.mInitialReflection;
			VuforiaRenderer.InternalInstance.SetVideoBackgroundConfigInternal(videoBackgroundConfig);
		}

		public virtual void ResetBackgroundPlane(bool disable)
		{
			if (this.mVideoBackgroundBehaviours != null && this.IsVideoBackgroundEnabled())
			{
				using (Dictionary<Camera, VideoBackgroundAbstractBehaviour>.ValueCollection.Enumerator enumerator = this.mVideoBackgroundBehaviours.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.ResetBackgroundPlane(disable);
					}
				}
			}
		}

		public void SetCameraParameterChanged()
		{
			this.mCameraParameterChanged = true;
		}

		private void EnableObjectRenderer(GameObject go, bool enabled)
		{
			Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = enabled;
			}
		}

		protected virtual void ResolveVideoBackgroundBehaviours()
		{
		}

		protected virtual bool CameraParameterChanged()
		{
			return this.mCameraParameterChanged;
		}

		protected bool IsVideoBackgroundEnabled()
		{
			return this.mRenderVideoBackground && VideoBackgroundManager.Instance.VideoBackgroundEnabled;
		}
	}
}
