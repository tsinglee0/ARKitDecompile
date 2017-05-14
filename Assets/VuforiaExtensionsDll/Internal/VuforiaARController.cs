using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Vuforia
{
	public class VuforiaARController : ARController
	{
		public enum WorldCenterMode
		{
			SPECIFIC_TARGET,
			FIRST_TARGET,
			CAMERA,
			DEVICE_TRACKING
		}

		private CameraDevice.CameraDeviceMode CameraDeviceModeSetting = CameraDevice.CameraDeviceMode.MODE_DEFAULT;

		private int MaxSimultaneousImageTargets = 1;

		private int MaxSimultaneousObjectTargets = 1;

		private bool UseDelayedLoadingObjectTargets;

		private CameraDevice.CameraDirection CameraDirection;

		private VuforiaRenderer.VideoBackgroundReflection MirrorVideoBackground;

		private VuforiaARController.WorldCenterMode mWorldCenterMode = VuforiaARController.WorldCenterMode.FIRST_TARGET;

		private TrackableBehaviour mWorldCenter;

		private List<ITrackerEventHandler> mTrackerEventHandlers = new List<ITrackerEventHandler>();

		private List<IVideoBackgroundEventHandler> mVideoBgEventHandlers = new List<IVideoBackgroundEventHandler>();

		private Action mOnVuforiaInitialized;

		private Action mOnVuforiaStarted;

		private Action mOnVuforiaDeinitialized;

		private Action mOnTrackablesUpdated;

		private Action mRenderOnUpdate;

		private Action<bool> mOnPause;

		private bool mPaused;

		private Action mOnBackgroundTextureChanged;

		private bool mStartHasBeenInvoked;

		private bool mHasStarted;

		private bool mBackgroundTextureHasChanged;

		private ICameraConfiguration mCameraConfiguration = new NullCameraConfiguration();

		private DigitalEyewearARController mEyewearBehaviour;

		private VideoBackgroundManager mVideoBackgroundMgr;

		private bool mCheckStopCamera;

		private Material mClearMaterial;

		private bool mMetalRendering;

		private bool mHasStartedOnce;

		private bool mWasEnabledBeforePause;

		private bool mObjectTrackerWasActiveBeforePause;

		private bool mObjectTrackerWasActiveBeforeDisabling;

		private int mLastUpdatedFrame;

		private List<Type> mTrackersRequestedToDeinit = new List<Type>();

		private bool mMissedToApplyLeftProjectionMatrix;

		private bool mMissedToApplyRightProjectionMatrix;

		private Matrix4x4 mLeftProjectMatrixToApply = Matrix4x4.identity;

		private Matrix4x4 mRightProjectMatrixToApply = Matrix4x4.identity;

		private static VuforiaARController mInstance;

		private static object mPadlock = new object();

		public VuforiaARController.WorldCenterMode WorldCenterModeSetting
		{
			get
			{
				if (Application.isPlaying)
				{
					return VuforiaManager.Instance.WorldCenterMode;
				}
				return this.mWorldCenterMode;
			}
		}

		public TrackableBehaviour WorldCenter
		{
			get
			{
				if (Application.isPlaying)
				{
					return VuforiaManager.Instance.WorldCenter as TrackableBehaviour;
				}
				return this.mWorldCenter;
			}
		}

		public VuforiaRenderer.VideoBackgroundReflection VideoBackGroundMirrored
		{
			get
			{
				return this.mCameraConfiguration.VideoBackgroundMirrored;
			}
		}

		public CameraDevice.CameraDeviceMode CameraDeviceMode
		{
			get
			{
				CameraDevice.CameraDeviceMode result;
				if (Application.isPlaying && CameraDevice.Instance.GetSelectedVideoMode(out result))
				{
					return result;
				}
				return this.CameraDeviceModeSetting;
			}
		}

		public bool HasStarted
		{
			get
			{
				return this.mHasStarted;
			}
		}

		public static VuforiaARController Instance
		{
			get
			{
				if (VuforiaARController.mInstance == null)
				{
					object obj = VuforiaARController.mPadlock;
					lock (obj)
					{
						if (VuforiaARController.mInstance == null)
						{
							VuforiaARController.mInstance = new VuforiaARController();
						}
					}
				}
				return VuforiaARController.mInstance;
			}
		}

		internal ICameraConfiguration CameraConfiguration
		{
			get
			{
				return this.mCameraConfiguration;
			}
			set
			{
				this.mCameraConfiguration = value;
			}
		}

		private VuforiaARController()
		{
		}

		[RuntimeInitializeOnLoadMethod]
		public static void RegisterARController()
		{
			ARController.Register(VuforiaARController.Instance);
		}

		public void RegisterVuforiaInitializedCallback(Action callback)
		{
			this.mOnVuforiaInitialized = (Action)Delegate.Combine(this.mOnVuforiaInitialized, callback);
			if (this.mHasStartedOnce)
			{
				callback();
			}
		}

		public void UnregisterVuforiaInitializedCallback(Action callback)
		{
			this.mOnVuforiaInitialized = (Action)Delegate.Remove(this.mOnVuforiaInitialized, callback);
		}

		public void RegisterVuforiaStartedCallback(Action callback)
		{
			this.mOnVuforiaStarted = (Action)Delegate.Combine(this.mOnVuforiaStarted, callback);
			if (this.mHasStartedOnce)
			{
				callback();
			}
		}

		public void UnregisterVuforiaStartedCallback(Action callback)
		{
			this.mOnVuforiaStarted = (Action)Delegate.Remove(this.mOnVuforiaStarted, callback);
		}

		public void RegisterTrackablesUpdatedCallback(Action callback)
		{
			this.mOnTrackablesUpdated = (Action)Delegate.Combine(this.mOnTrackablesUpdated, callback);
		}

		public void UnregisterTrackablesUpdatedCallback(Action callback)
		{
			this.mOnTrackablesUpdated = (Action)Delegate.Remove(this.mOnTrackablesUpdated, callback);
		}

		public void RegisterOnPauseCallback(Action<bool> callback)
		{
			this.mOnPause = (Action<bool>)Delegate.Combine(this.mOnPause, callback);
		}

		public void UnregisterOnPauseCallback(Action<bool> callback)
		{
			this.mOnPause = (Action<bool>)Delegate.Remove(this.mOnPause, callback);
		}

		public void RegisterBackgroundTextureChangedCallback(Action callback)
		{
			this.mOnBackgroundTextureChanged = (Action)Delegate.Combine(this.mOnBackgroundTextureChanged, callback);
		}

		public void UnregisterBackgroundTextureChangedCallback(Action callback)
		{
			this.mOnBackgroundTextureChanged = (Action)Delegate.Remove(this.mOnBackgroundTextureChanged, callback);
		}

		[Obsolete("The ITrackerEventHandler interface will be removed with the next Vuforia release. Please use VuforiaBehaviour.RegisterVuforiaStartedCallback and RegisterTrackablesUpdatedCallback instead.")]
		public void RegisterTrackerEventHandler(ITrackerEventHandler trackerEventHandler)
		{
			this.mTrackerEventHandlers.Add(trackerEventHandler);
			if (this.mHasStarted)
			{
				trackerEventHandler.OnInitialized();
			}
		}

		public bool UnregisterTrackerEventHandler(ITrackerEventHandler trackerEventHandler)
		{
			return this.mTrackerEventHandlers.Remove(trackerEventHandler);
		}

		public void RegisterVideoBgEventHandler(IVideoBackgroundEventHandler videoBgEventHandler)
		{
			this.mVideoBgEventHandlers.Add(videoBgEventHandler);
		}

		public bool UnregisterVideoBgEventHandler(IVideoBackgroundEventHandler videoBgEventHandler)
		{
			return this.mVideoBgEventHandlers.Remove(videoBgEventHandler);
		}

		public void SetWorldCenterMode(VuforiaARController.WorldCenterMode value)
		{
			if (Application.isPlaying)
			{
				VuforiaManager.Instance.WorldCenterMode = value;
				return;
			}
			this.mWorldCenterMode = value;
		}

		public void SetWorldCenter(TrackableBehaviour trackable)
		{
			if (!(trackable == null) && !(trackable is WorldCenterTrackableBehaviour))
			{
				Debug.LogError("VuforiaARController.SetWorldCenter: TrackableBehaviour is not a WorldCenterTrackableBehaviour. Cannot set as world center.");
				return;
			}
			if (Application.isPlaying)
			{
				VuforiaManager.Instance.WorldCenter = (trackable as WorldCenterTrackableBehaviour);
				return;
			}
			this.mWorldCenter = trackable;
		}

		public Rect GetVideoBackgroundRectInViewPort()
		{
			return this.mCameraConfiguration.VideoBackgroundViewportRect;
		}

		public ScreenOrientation GetSurfaceOrientation()
		{
			return VuforiaRuntimeUtilities.ScreenOrientation;
		}

		public void UpdateState(bool forceUpdate, bool reapplyOldState)
		{
			if (VuforiaManager.Instance.Initialized)
			{
				this.UpdateStatePrivate(forceUpdate, reapplyOldState);
			}
		}

		public void ApplyCorrectedProjectionMatrix(Matrix4x4 projectionMatrix, bool primaryCamera)
		{
			bool flag = false;
			if (VuforiaManager.Instance.Initialized && this.mCameraConfiguration != null)
			{
				this.mCameraConfiguration.ApplyCorrectedProjectionMatrix(projectionMatrix, primaryCamera);
				flag = true;
			}
			if (!flag)
			{
				if (primaryCamera)
				{
					this.mLeftProjectMatrixToApply = projectionMatrix;
					this.mMissedToApplyLeftProjectionMatrix = true;
					return;
				}
				this.mRightProjectMatrixToApply = projectionMatrix;
				this.mMissedToApplyRightProjectionMatrix = true;
			}
		}

		internal void RegisterVuforiaDeinitializedCallback(Action callback)
		{
			this.mOnVuforiaDeinitialized = (Action)Delegate.Combine(this.mOnVuforiaDeinitialized, callback);
		}

		internal void UnregisterVuforiaDeinitializedCallback(Action callback)
		{
			this.mOnVuforiaDeinitialized = (Action)Delegate.Remove(this.mOnVuforiaDeinitialized, callback);
		}

		internal void InitCameraConfiguration()
		{
			VuforiaRenderer.VideoBackgroundReflection mirrorVideoBackground = this.mCameraConfiguration.IsStereo() ? VuforiaRenderer.VideoBackgroundReflection.OFF : this.MirrorVideoBackground;
			this.mCameraConfiguration.InitCameraDevice(this.CameraDeviceMode, mirrorVideoBackground, new Action(this.OnVideoBackgroundConfigChanged));
		}

		internal void ConfigureVideoBackground()
		{
			this.mCameraConfiguration.ConfigureVideoBackground();
		}

		internal void ResetBackgroundPlane(bool disable)
		{
			this.mCameraConfiguration.ResetBackgroundPlane(disable);
		}

		internal void RegisterRenderOnUpdateCallback(Action callback)
		{
			this.mRenderOnUpdate = (Action)Delegate.Combine(this.mRenderOnUpdate, callback);
		}

		internal void UnregisterRenderOnUpdateCallback(Action callback)
		{
			this.mRenderOnUpdate = (Action)Delegate.Remove(this.mRenderOnUpdate, callback);
		}

		public static bool IsValidPrimaryCamera(Camera cam)
		{
			return VuforiaARController.IsValidSecondaryCamera(cam) && cam.GetComponentsInChildren<BackgroundPlaneAbstractBehaviour>(true).Any<BackgroundPlaneAbstractBehaviour>();
		}

		public static bool IsValidSecondaryCamera(Camera cam)
		{
			return cam.GetComponent<VideoBackgroundAbstractBehaviour>() != null;
		}

		protected override void Awake()
		{
			VuforiaAbstractConfiguration.GenericVuforiaConfiguration vuforia = VuforiaAbstractConfiguration.Instance.Vuforia;
			this.CameraDeviceModeSetting = vuforia.CameraDeviceMode;
			this.MaxSimultaneousImageTargets = vuforia.MaxSimultaneousImageTargets;
			this.MaxSimultaneousObjectTargets = vuforia.MaxSimultaneousObjectTargets;
			this.UseDelayedLoadingObjectTargets = vuforia.UseDelayedLoadingObjectTargets;
			this.CameraDirection = vuforia.CameraDirection;
			this.MirrorVideoBackground = vuforia.MirrorVideoBackground;
			this.mWorldCenterMode = base.VuforiaBehaviour.WorldCenterMode;
			this.mWorldCenter = base.VuforiaBehaviour.WorldCenter;
			this.mEyewearBehaviour = DigitalEyewearARController.Instance;
			if (this.mEyewearBehaviour == null)
			{
				Debug.LogError("Failed to get an instance of DigitalEyewearBehaviour");
			}
			this.mVideoBackgroundMgr = VideoBackgroundManager.Instance;
			this.mVideoBackgroundMgr.Initialize();
		}

		protected override void Start()
		{
			if (!VuforiaRuntime.Instance.HasInitialized)
			{
				Debug.LogError("Vuforia cannot be started before it is initialized.\n Please disable Delayed Initializationin the Vuforia configuration or initialize Vuforia manually with the VuforiaRuntime-class.");
				return;
			}
			this.mStartHasBeenInvoked = true;
			if (TrackerManager.Instance.GetTracker<ObjectTracker>() == null)
			{
				TrackerManager.Instance.InitTracker<ObjectTracker>();
			}
			Screen.sleepTimeout = -1;
			this.mClearMaterial = new Material(Shader.Find("Diffuse"));
			VuforiaUnity.SetHint(VuforiaUnity.VuforiaHint.HINT_MAX_SIMULTANEOUS_IMAGE_TARGETS, this.MaxSimultaneousImageTargets);
			VuforiaUnity.SetHint(VuforiaUnity.VuforiaHint.HINT_MAX_SIMULTANEOUS_OBJECT_TARGETS, this.MaxSimultaneousObjectTargets);
			VuforiaUnity.SetHint(VuforiaUnity.VuforiaHint.HINT_DELAYED_LOADING_OBJECT_DATASETS, this.UseDelayedLoadingObjectTargets ? 1 : 0);
			VuforiaManager.Instance.WorldCenterMode = this.mWorldCenterMode;
			if (this.mWorldCenter is VuMarkAbstractBehaviour)
			{
				VuforiaManager.Instance.WorldCenter = null;
				VuforiaManager.Instance.VuMarkWorldCenter = (VuMarkAbstractBehaviour)this.mWorldCenter;
			}
			else
			{
				VuforiaManager.Instance.WorldCenter = (this.mWorldCenter as WorldCenterTrackableBehaviour);
			}
			VuforiaManager.Instance.ARCameraTransform = base.VuforiaBehaviour.transform;
			VuforiaManager.Instance.CentralAnchorPoint = base.VuforiaBehaviour.transform;
			VuforiaManager.Instance.ParentAnchorPoint = base.VuforiaBehaviour.transform;
			VuforiaManager.Instance.Init();
			if (this.mOnVuforiaInitialized != null)
			{
				this.mOnVuforiaInitialized.InvokeWithExceptionHandling();
			}
			DatabaseLoadARController.Instance.LoadDatasets();
			UnityPlayer.Instance.StartScene();
			this.StartVuforia(true);
			using (List<ITrackerEventHandler>.Enumerator enumerator = this.mTrackerEventHandlers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnInitialized();
				}
			}
			if (this.mOnVuforiaStarted != null)
			{
				this.mOnVuforiaStarted.InvokeWithExceptionHandling();
			}
			this.mHasStartedOnce = true;
			if (VuforiaRenderer.Instance.GetRendererAPI() == VuforiaRenderer.RendererAPI.METAL)
			{
				this.mMetalRendering = true;
			}
		}

		protected override void OnEnable()
		{
			if (VuforiaManager.Instance.Initialized && this.mHasStartedOnce)
			{
				this.StartVuforia(this.mObjectTrackerWasActiveBeforeDisabling);
			}
		}

		protected override void OnApplicationPause(bool pause)
		{
			if (!VuforiaRuntimeUtilities.IsPlayMode())
			{
				if (pause)
				{
					if (!this.mPaused)
					{
						if (this.mOnPause != null)
						{
							this.mOnPause.InvokeWithExceptionHandling(true);
						}
						ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
						this.mObjectTrackerWasActiveBeforePause = (tracker != null && tracker.IsActive);
						this.mWasEnabledBeforePause = base.VuforiaBehaviour.enabled;
						if (this.mWasEnabledBeforePause)
						{
							this.StopVuforia();
						}
						GL.Clear(false, true, new Color(0f, 0f, 0f, 1f));
						UnityPlayer.Instance.OnPause();
					}
				}
				else if (this.mPaused)
				{
					UnityPlayer.Instance.OnResume();
					if (this.mWasEnabledBeforePause && this.StartVuforia(this.mObjectTrackerWasActiveBeforePause))
					{
						IOSCamRecoveringHelper.SetHasJustResumed();
					}
					this.ResetBackgroundPlane(true);
					if (this.mOnPause != null)
					{
						this.mOnPause.InvokeWithExceptionHandling(false);
					}
				}
			}
			this.mPaused = pause;
		}

		protected override void OnDisable()
		{
			ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
			this.mObjectTrackerWasActiveBeforeDisabling = (tracker != null && tracker.IsActive);
			this.StopVuforia();
			this.ResetBackgroundPlane(true);
		}

		protected override void OnDestroy()
		{
			((StateManagerImpl)TrackerManager.Instance.GetStateManager()).ClearTrackableBehaviours();
			ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
			if (tracker != null)
			{
				tracker.DestroyAllDataSets(false);
				tracker.Stop();
			}
			TextTracker tracker2 = TrackerManager.Instance.GetTracker<TextTracker>();
			if (tracker2 != null)
			{
				tracker2.Stop();
			}
			SmartTerrainTracker tracker3 = TrackerManager.Instance.GetTracker<SmartTerrainTracker>();
			if (tracker3 != null)
			{
				tracker3.Stop();
			}
			DeviceTracker tracker4 = TrackerManager.Instance.GetTracker<DeviceTracker>();
			if (tracker4 != null)
			{
				tracker4.Stop();
			}
			VuforiaManager.Instance.Deinit();
			if (tracker != null)
			{
				TrackerManager.Instance.DeinitTracker<ObjectTracker>();
			}
			if (tracker2 != null)
			{
				TrackerManager.Instance.DeinitTracker<TextTracker>();
			}
			if (tracker3 != null)
			{
				tracker3.SmartTerrainBuilder.Deinit();
				TrackerManager.Instance.DeinitTracker<SmartTerrainTracker>();
			}
			if (tracker4 != null)
			{
				TrackerManager.Instance.DeinitTracker<DeviceTracker>();
			}
			this.mHasStartedOnce = false;
			if (this.mOnVuforiaDeinitialized != null)
			{
				this.mOnVuforiaDeinitialized.InvokeWithExceptionHandling();
			}
		}

		private void UpdateStatePrivate(bool forceUpdate, bool reapplyOldState)
		{
			if (this.mCheckStopCamera)
			{
				this.DisableCameraIfNotNeeded();
				this.mCheckStopCamera = false;
			}
			if (VuforiaManager.Instance.Initialized && this.mCameraConfiguration != null)
			{
				this.ApplyMissedProjectionMatrices();
			}
			if (forceUpdate || Time.frameCount > this.mLastUpdatedFrame)
			{
				this.DeinitRequestedTrackers();
				if (VuforiaManager.Instance.Initialized)
				{
					UnityPlayer.Instance.Update();
					ScreenOrientation counterRotation;
					this.mCameraConfiguration.CheckForSurfaceChanges(out counterRotation);
					if (!this.mMetalRendering)
					{
						this.mClearMaterial.SetPass(0);
					}
					if (((VuforiaManagerImpl)VuforiaManager.Instance).Update(counterRotation, reapplyOldState))
					{
						IOSCamRecoveringHelper.SetSuccessfullyRecovered();
						this.ResetBackgroundPlane(false);
						using (List<ITrackerEventHandler>.Enumerator enumerator = this.mTrackerEventHandlers.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								enumerator.Current.OnTrackablesUpdated();
							}
						}
						this.mCameraConfiguration.UpdateStereoDepth(VuforiaManager.Instance.CentralAnchorPoint);
						if (this.mOnTrackablesUpdated != null)
						{
							this.mOnTrackablesUpdated.InvokeWithExceptionHandling();
						}
					}
					else
					{
						IOSCamRecoveringHelper.TryToRecover();
					}
					if (this.mRenderOnUpdate != null)
					{
						this.mRenderOnUpdate.InvokeWithExceptionHandling();
					}
				}
				else if (VuforiaRuntimeUtilities.IsPlayMode() && !this.mStartHasBeenInvoked)
				{
					Debug.LogWarning("Scripts have been recompiled during Play mode, need to restart!");
					VuforiaWrapper.CreateRuntimeInstance();
					PlayModeEditorUtility.Instance.RestartPlayMode();
				}
				if (VuforiaRenderer.InternalInstance.HasBackgroundTextureChanged())
				{
					this.mBackgroundTextureHasChanged = true;
					if (this.mOnBackgroundTextureChanged != null)
					{
						this.mOnBackgroundTextureChanged.InvokeWithExceptionHandling();
					}
				}
				this.mEyewearBehaviour.SetFocusPoint();
				this.mLastUpdatedFrame = Time.frameCount;
			}
		}

		private bool StartVuforia(bool startObjectTracker)
		{
			Debug.Log("StartVuforia");
			if (!CameraDevice.Instance.Init(this.CameraDirection))
			{
				return false;
			}
			if (!CameraDevice.Instance.SelectVideoMode(this.CameraDeviceModeSetting))
			{
				return false;
			}
			if (!CameraDevice.Instance.Start())
			{
				return false;
			}
			if (startObjectTracker)
			{
				ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
				if (tracker != null)
				{
					tracker.Start();
				}
			}
			this.mHasStarted = true;
			this.mCheckStopCamera = true;
			return true;
		}

		private bool StopVuforia()
		{
			this.mHasStarted = false;
			Debug.Log("StopVuforia");
			ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
			if (tracker != null)
			{
				tracker.Stop();
			}
			CameraDevice.Instance.GetSelectedCameraDirection(out this.CameraDirection);
			this.CameraDeviceModeSetting = this.CameraDeviceMode;
			if (!CameraDevice.Instance.Stop())
			{
				return false;
			}
			if (!CameraDevice.Instance.Deinit())
			{
				return false;
			}
			VuforiaRenderer.Instance.ClearVideoBackgroundConfig();
			Device.Instance.DeleteRenderingPrimitives();
			return true;
		}

		private void DisableCameraIfNotNeeded()
		{
			if (this.mEyewearBehaviour.GetEyewearType() != DigitalEyewearARController.EyewearType.OpticalSeeThrough && !this.mVideoBackgroundMgr.VideoBackgroundEnabled)
			{
				bool flag = false;
				bool flag2 = false;
				DeviceTracker tracker = TrackerManager.Instance.GetTracker<DeviceTracker>();
				if (tracker != null)
				{
					flag = tracker.IsActive;
				}
				if (this.mEyewearBehaviour.GetEyewearType() == DigitalEyewearARController.EyewearType.VideoSeeThrough && this.mEyewearBehaviour.GetStereoCameraConfig() != DigitalEyewearARController.StereoFramework.Vuforia)
				{
					flag2 = true;
				}
				if (flag | flag2)
				{
					Debug.Log("Vuforia is configured for VR, checking if the camera and default trackers can be stopped...");
					bool flag3;
					bool arg_74_0 = VuforiaRuntimeUtilities.StopCameraIfPossible(out flag3);
					if (flag3)
					{
						Debug.Log("VuforiaBehaviour: The ObjectTracker was disabled because it is not in use.");
					}
					if (arg_74_0)
					{
						Debug.Log("VuforiaBehaviour: The Camera was stopped because it is not in use.");
					}
				}
			}
		}

		private void DeinitRequestedTrackers()
		{
			if (this.mTrackersRequestedToDeinit.Count > 0)
			{
				base.VuforiaBehaviour.enabled = false;
				foreach (Type current in this.mTrackersRequestedToDeinit)
				{
					if (current == typeof(ObjectTracker))
					{
						TrackerManager.Instance.DeinitTracker<ObjectTracker>();
					}
					else if (current == typeof(TextTracker))
					{
						TrackerManager.Instance.DeinitTracker<TextTracker>();
					}
					else if (current == typeof(SmartTerrainTracker))
					{
						SmartTerrainTracker tracker = TrackerManager.Instance.GetTracker<SmartTerrainTracker>();
						if (tracker != null)
						{
							tracker.SmartTerrainBuilder.Deinit();
							TrackerManager.Instance.DeinitTracker<SmartTerrainTracker>();
						}
					}
					else if (current == typeof(DeviceTracker))
					{
						TrackerManager.Instance.DeinitTracker<DeviceTracker>();
					}
					else
					{
						Debug.LogError("Could not deinitialize tracker. Unknown tracker type.");
					}
				}
				base.VuforiaBehaviour.enabled = true;
				this.mTrackersRequestedToDeinit.Clear();
			}
		}

		private void OnVideoBackgroundConfigChanged()
		{
			foreach (IVideoBackgroundEventHandler current in this.mVideoBgEventHandlers)
			{
				try
				{
					current.OnVideoBackgroundConfigChanged();
				}
				catch (Exception ex)
				{
					Debug.LogError("Exception in callback: " + ex.ToString());
				}
			}
		}

		private void EnableObjectRenderer(GameObject go, bool enabled)
		{
			Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = enabled;
			}
		}

		private void ApplyMissedProjectionMatrices()
		{
			if (this.mMissedToApplyLeftProjectionMatrix)
			{
				this.mCameraConfiguration.ApplyCorrectedProjectionMatrix(this.mLeftProjectMatrixToApply, true);
				this.mMissedToApplyLeftProjectionMatrix = false;
			}
			if (this.mMissedToApplyRightProjectionMatrix)
			{
				this.mCameraConfiguration.ApplyCorrectedProjectionMatrix(this.mRightProjectMatrixToApply, false);
				this.mMissedToApplyRightProjectionMatrix = false;
			}
		}
	}
}
