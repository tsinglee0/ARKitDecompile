using System;
using UnityEngine;

namespace Vuforia
{
	public class MixedRealityController
	{
		public enum Mode
		{
			HANDHELD_AR,
			ROTATIONAL_HANDHELD_AR,
			HANDHELD_VR,
			VIEWER_AR,
			ROTATIONAL_VIEWER_AR,
			VIEWER_VR
		}

		private static MixedRealityController mInstance;

		private VuforiaARController mVuforiaBehaviour;

		private DigitalEyewearARController mDigitalEyewearBehaviour;

		private VideoBackgroundManager mVideoBackgroundManager;

		private bool mViewerHasBeenSetExternally;

		private IViewerParameters mViewerParameters;

		private bool mFrameWorkHasBeenSetExternally;

		private DigitalEyewearARController.StereoFramework mStereoFramework;

		private Transform mCentralAnchorPoint;

		private Camera mLeftCameraOfExternalSDK;

		private Camera mRightCameraOfExternalSDK;

		private bool mObjectTrackerStopped;

		private bool mAutoStopCameraIfNotRequired = true;

		public static MixedRealityController Instance
		{
			get
			{
				if (MixedRealityController.mInstance == null && MixedRealityController.mInstance == null)
				{
					MixedRealityController.mInstance = new MixedRealityController();
				}
				return MixedRealityController.mInstance;
			}
		}

		public bool AutoStopCameraIfNotRequired
		{
			get
			{
				return this.mAutoStopCameraIfNotRequired;
			}
			set
			{
				this.mAutoStopCameraIfNotRequired = value;
			}
		}

		public void SetMode(MixedRealityController.Mode mode)
		{
			this.ResolveMembers();
			switch (mode)
			{
			case MixedRealityController.Mode.HANDHELD_AR:
				this.SetHandheldAR();
				return;
			case MixedRealityController.Mode.ROTATIONAL_HANDHELD_AR:
				this.SetRotationalHandheldAR();
				return;
			case MixedRealityController.Mode.HANDHELD_VR:
				this.SetHandheldVR();
				return;
			case MixedRealityController.Mode.VIEWER_AR:
				this.SetViewerAR();
				return;
			case MixedRealityController.Mode.ROTATIONAL_VIEWER_AR:
				this.SetRotationalViewerAR();
				return;
			case MixedRealityController.Mode.VIEWER_VR:
				this.SetViewerVR();
				return;
			default:
				return;
			}
		}

		public void SetViewerParameters(IViewerParameters viewerParameters)
		{
			this.mViewerHasBeenSetExternally = true;
			this.mViewerParameters = viewerParameters;
		}

		public void ConfigureForDifferentStereoFramework(DigitalEyewearARController.StereoFramework stereoFramework, Transform centralAnchorPoint, Camera leftCamera, Camera RightCamera)
		{
			this.mFrameWorkHasBeenSetExternally = true;
			this.mStereoFramework = stereoFramework;
			this.mCentralAnchorPoint = centralAnchorPoint;
			this.mLeftCameraOfExternalSDK = leftCamera;
			this.mRightCameraOfExternalSDK = RightCamera;
		}

		private void ResolveMembers()
		{
			if (this.mVuforiaBehaviour == null)
			{
				this.mVuforiaBehaviour = VuforiaARController.Instance;
			}
			if (this.mDigitalEyewearBehaviour == null)
			{
				this.mDigitalEyewearBehaviour = DigitalEyewearARController.Instance;
			}
			if (this.mDigitalEyewearBehaviour.GetEyewearType() == DigitalEyewearARController.EyewearType.VideoSeeThrough)
			{
				if (!this.mFrameWorkHasBeenSetExternally)
				{
					this.mStereoFramework = this.mDigitalEyewearBehaviour.GetStereoCameraConfig();
					if (!this.mViewerHasBeenSetExternally)
					{
						this.mViewerParameters = Device.Instance.GetSelectedViewer();
					}
					if (this.mStereoFramework != DigitalEyewearARController.StereoFramework.Vuforia)
					{
						this.mCentralAnchorPoint = this.mDigitalEyewearBehaviour.CentralAnchorPoint;
						this.mLeftCameraOfExternalSDK = this.mDigitalEyewearBehaviour.PrimaryCamera;
						this.mRightCameraOfExternalSDK = this.mDigitalEyewearBehaviour.SecondaryCamera;
					}
				}
			}
			else if (!this.mFrameWorkHasBeenSetExternally)
			{
				this.mStereoFramework = DigitalEyewearARController.StereoFramework.Vuforia;
				this.mDigitalEyewearBehaviour.SetCameraOffset(0.06f);
				this.mDigitalEyewearBehaviour.SetDistortionRendering(DistortionRenderingMode.SingleTexture);
				if (!this.mViewerHasBeenSetExternally)
				{
					this.mViewerParameters = null;
				}
			}
			if (this.mViewerParameters == null)
			{
				IViewerParametersList viewerList = Device.Instance.GetViewerList();
				if (viewerList != null)
				{
					this.mViewerParameters = viewerList.Get(0);
				}
			}
			if (this.mVideoBackgroundManager == null)
			{
				this.mVideoBackgroundManager = VideoBackgroundManager.Instance;
			}
		}

		private void SetVideoBackgroundEnabled(bool enabled)
		{
			this.mVideoBackgroundManager.SetVideoBackgroundEnabled(enabled);
		}

		private void SetViewer(bool viewerPresent)
		{
			VuforiaAbstractBehaviour vuforiaAbstractBehaviour = UnityEngine.Object.FindObjectOfType<VuforiaAbstractBehaviour>();
			if (vuforiaAbstractBehaviour != null && this.mDigitalEyewearBehaviour != null)
			{
				if (viewerPresent)
				{
					this.mDigitalEyewearBehaviour.SetEyewearType(DigitalEyewearARController.EyewearType.VideoSeeThrough);
				}
				else
				{
					this.mDigitalEyewearBehaviour.SetEyewearType(DigitalEyewearARController.EyewearType.None);
				}
				if (viewerPresent && this.mViewerParameters != null)
				{
					if (this.mDigitalEyewearBehaviour.GetStereoCameraConfig() != this.mStereoFramework)
					{
						this.mDigitalEyewearBehaviour.SetStereoCameraConfiguration(this.mStereoFramework);
					}
					if (Device.Instance.GetSelectedViewer() != this.mViewerParameters)
					{
						Device.Instance.SelectViewer(this.mViewerParameters);
					}
					if (this.mStereoFramework != DigitalEyewearARController.StereoFramework.Vuforia)
					{
						Camera[] componentsInChildren = vuforiaAbstractBehaviour.GetComponentsInChildren<Camera>(true);
						for (int i = 0; i < componentsInChildren.Length; i++)
						{
							componentsInChildren[i].gameObject.SetActive(false);
						}
						if (this.mDigitalEyewearBehaviour.CentralAnchorPoint != this.mCentralAnchorPoint)
						{
							this.mDigitalEyewearBehaviour.SetCentralAnchorPoint(this.mCentralAnchorPoint);
						}
						if (this.mDigitalEyewearBehaviour.PrimaryCamera != this.mLeftCameraOfExternalSDK)
						{
							this.mDigitalEyewearBehaviour.PrimaryCamera = this.mLeftCameraOfExternalSDK;
						}
						if (this.mDigitalEyewearBehaviour.SecondaryCamera != this.mRightCameraOfExternalSDK)
						{
							this.mDigitalEyewearBehaviour.SecondaryCamera = this.mRightCameraOfExternalSDK;
						}
					}
				}
				if (!viewerPresent && this.mStereoFramework != DigitalEyewearARController.StereoFramework.Vuforia)
				{
					Camera[] componentsInChildren = vuforiaAbstractBehaviour.GetComponentsInChildren<Camera>(true);
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].gameObject.SetActive(true);
					}
					Transform transform = vuforiaAbstractBehaviour.gameObject.transform;
					Camera componentInChildren = vuforiaAbstractBehaviour.GetComponentInChildren<Camera>();
					if (this.mDigitalEyewearBehaviour.CentralAnchorPoint != transform)
					{
						this.mDigitalEyewearBehaviour.SetCentralAnchorPoint(transform);
					}
					if (this.mDigitalEyewearBehaviour.PrimaryCamera != componentInChildren)
					{
						this.mDigitalEyewearBehaviour.PrimaryCamera = componentInChildren;
					}
					if (this.mDigitalEyewearBehaviour.SecondaryCamera != null)
					{
						this.mDigitalEyewearBehaviour.SecondaryCamera = null;
					}
				}
				if (Device.Instance.IsViewerActive() != viewerPresent)
				{
					this.mDigitalEyewearBehaviour.SetViewerActive(viewerPresent);
				}
			}
		}

		private void SetTargetFPS(bool isVR)
		{
			int recommendedFps;
			if (isVR)
			{
				recommendedFps = VuforiaRenderer.Instance.GetRecommendedFps(VuforiaRenderer.FpsHint.NO_VIDEOBACKGROUND);
			}
			else
			{
				recommendedFps = VuforiaRenderer.Instance.GetRecommendedFps(VuforiaRenderer.FpsHint.NONE);
			}
			Application.targetFrameRate = recommendedFps;
		}

		private void StopDeviceTracker()
		{
			DeviceTracker tracker = TrackerManager.Instance.GetTracker<DeviceTracker>();
			if (tracker != null && tracker.IsActive)
			{
				tracker.Stop();
			}
		}

		private void StartDeviceTracker(bool videoBackground, RotationalDeviceTracker.MODEL_CORRECTION_MODE mode)
		{
			RotationalDeviceTracker rotationalDeviceTracker = TrackerManager.Instance.GetTracker<RotationalDeviceTracker>();
			if (this.mStereoFramework != DigitalEyewearARController.StereoFramework.Vuforia)
			{
				if (rotationalDeviceTracker != null && rotationalDeviceTracker.IsActive)
				{
					rotationalDeviceTracker.Stop();
					return;
				}
			}
			else
			{
				CameraDevice.CameraDirection cameraDirection;
				CameraDevice.CameraDeviceMode mode2;
				if (rotationalDeviceTracker == null && CameraDevice.Instance.GetSelectedCameraDirection(out cameraDirection) && CameraDevice.Instance.GetSelectedVideoMode(out mode2))
				{
					bool expr_54 = CameraDevice.Instance.IsActive();
					if (expr_54)
					{
						CameraDevice.Instance.Stop();
					}
					CameraDevice.Instance.Deinit();
					rotationalDeviceTracker = TrackerManager.Instance.InitTracker<RotationalDeviceTracker>();
					CameraDevice.Instance.Init(cameraDirection);
					CameraDevice.Instance.SelectVideoMode(mode2);
					if (expr_54)
					{
						CameraDevice.Instance.Start();
					}
				}
				if (rotationalDeviceTracker != null)
				{
					rotationalDeviceTracker.SetPosePrediction(!videoBackground);
					rotationalDeviceTracker.SetModelCorrectionMode(mode);
					if (!rotationalDeviceTracker.IsActive)
					{
						rotationalDeviceTracker.Start();
					}
				}
			}
		}

		private void StopCameraAndTrackersIfNotRequired()
		{
			if (this.mAutoStopCameraIfNotRequired)
			{
				VuforiaRuntimeUtilities.StopCameraIfPossible(out this.mObjectTrackerStopped);
			}
		}

		private void StartCameraAndTrackersIfStopped()
		{
			if (!CameraDevice.Instance.IsActive())
			{
				CameraDevice.Instance.Start();
			}
			if (this.mObjectTrackerStopped)
			{
				TrackerManager.Instance.GetTracker<ObjectTracker>().Start();
				this.mObjectTrackerStopped = false;
			}
		}

		private void SetHandheldAR()
		{
			this.StopDeviceTracker();
			this.SetVideoBackgroundEnabled(true);
			this.SetViewer(false);
			this.SetTargetFPS(false);
			this.StartCameraAndTrackersIfStopped();
		}

		private void SetRotationalHandheldAR()
		{
			this.StartDeviceTracker(true, RotationalDeviceTracker.MODEL_CORRECTION_MODE.HANDHELD);
			this.SetVideoBackgroundEnabled(true);
			this.SetViewer(false);
			this.SetTargetFPS(true);
			this.StartCameraAndTrackersIfStopped();
		}

		private void SetHandheldVR()
		{
			this.StartDeviceTracker(false, RotationalDeviceTracker.MODEL_CORRECTION_MODE.HANDHELD);
			this.SetVideoBackgroundEnabled(false);
			this.SetViewer(false);
			this.SetTargetFPS(true);
			this.StopCameraAndTrackersIfNotRequired();
		}

		private void SetViewerAR()
		{
			this.StopDeviceTracker();
			this.SetVideoBackgroundEnabled(true);
			this.SetViewer(true);
			this.SetTargetFPS(false);
			this.StartCameraAndTrackersIfStopped();
		}

		private void SetRotationalViewerAR()
		{
			this.StartDeviceTracker(true, RotationalDeviceTracker.MODEL_CORRECTION_MODE.HEAD);
			this.SetVideoBackgroundEnabled(true);
			this.SetViewer(true);
			this.SetTargetFPS(true);
			this.StartCameraAndTrackersIfStopped();
		}

		private void SetViewerVR()
		{
			this.StartDeviceTracker(false, RotationalDeviceTracker.MODEL_CORRECTION_MODE.HEAD);
			this.SetVideoBackgroundEnabled(false);
			this.SetViewer(true);
			this.SetTargetFPS(true);
			this.StopCameraAndTrackersIfNotRequired();
		}
	}
}
