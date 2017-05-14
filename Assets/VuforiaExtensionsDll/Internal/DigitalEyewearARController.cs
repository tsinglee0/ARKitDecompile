using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
	public class DigitalEyewearARController : ARController, IVideoBackgroundEventHandler
	{
		public enum EyewearType
		{
			None,
			OpticalSeeThrough,
			VideoSeeThrough
		}

		public enum StereoFramework
		{
			Vuforia,
			Cardboard,
			GearVR
		}

		public enum SeeThroughConfiguration
		{
			Vuforia,
			HoloLens
		}

		[Serializable]
		public class SerializableViewerParameters
		{
			public float Version;

			public string Name;

			public string Manufacturer;

			public ViewerButtonType ButtonType;

			public float ScreenToLensDistance;

			public float InterLensDistance;

			public ViewerTrayAlignment TrayAlignment;

			public float LensCenterToTrayDistance;

			public Vector2 DistortionCoefficients;

			public Vector4 FieldOfView;

			public bool ContainsMagnet;
		}

		internal const string LEFT_CAMERA_NAME = "StereoCameraLeft";

		internal const string RIGHT_CAMERA_NAME = "StereoCameraRight";

		public const float DEFAULT_CAMERA_OFFSET = 0.06f;

		public const string GEARVR_VIEWER = "GEARVR";

		public const string CARDBOARD_SDK_VIEWER = "CARDBOARD";

		public const float DEFAULT_VR_FOV = 60f;

		private float mCameraOffset;

		private DistortionRenderingMode mDistortionRenderingMode;

		private int mDistortionRenderingLayer = 31;

		private DigitalEyewearARController.EyewearType mEyewearType;

		private DigitalEyewearARController.StereoFramework mStereoFramework;

		private DigitalEyewearARController.SeeThroughConfiguration mSeeThroughConfiguration;

		private string mViewerName;

		private string mViewerManufacturer;

		private bool mUseCustomViewer;

		private DigitalEyewearARController.SerializableViewerParameters mCustomViewer;

		private Transform mCentralAnchorPoint;

		private Transform mParentAnchorPoint;

		private Camera mPrimaryCamera;

		private Rect mPrimaryCameraOriginalRect;

		private Camera mSecondaryCamera;

		private Rect mSecondaryCameraOriginalRect;

		private bool mSecondaryCameraDisabledLocally;

		private VuforiaARController mVuforiaBehaviour;

		private DistortionRenderingBehaviour mDistortionRenderingBhvr;

		private bool mSetFocusPlaneAutomatically = true;

		private static DigitalEyewearARController mInstance;

		private static object mPadlock = new object();

		public float CameraOffset
		{
			get
			{
				return this.mCameraOffset;
			}
			private set
			{
				this.mCameraOffset = value;
			}
		}

		public Transform CentralAnchorPoint
		{
			get
			{
				if (Application.isPlaying)
				{
					return VuforiaManager.Instance.CentralAnchorPoint;
				}
				return this.mCentralAnchorPoint;
			}
		}

		public Transform ParentAnchorPoint
		{
			get
			{
				if (Application.isPlaying)
				{
					return VuforiaManager.Instance.ParentAnchorPoint;
				}
				return this.mParentAnchorPoint;
			}
		}

		public bool IsStereoRendering
		{
			get
			{
				return this.CameraConfiguration.IsStereo();
			}
		}

		public Camera PrimaryCamera
		{
			get
			{
				return this.mPrimaryCamera;
			}
			set
			{
				this.mPrimaryCamera = value;
				this.mSecondaryCameraDisabledLocally = false;
			}
		}

		public Camera SecondaryCamera
		{
			get
			{
				return this.mSecondaryCamera;
			}
			set
			{
				this.mSecondaryCamera = value;
				this.mSecondaryCameraDisabledLocally = false;
			}
		}

		public int DistortionRenderingLayer
		{
			get
			{
				return this.mDistortionRenderingLayer;
			}
			set
			{
				this.mDistortionRenderingLayer = value;
			}
		}

		public DistortionRenderingMode DistortionRendering
		{
			get
			{
				return this.mDistortionRenderingMode;
			}
		}

		public static DigitalEyewearARController Instance
		{
			get
			{
				if (DigitalEyewearARController.mInstance == null)
				{
					object obj = DigitalEyewearARController.mPadlock;
					lock (obj)
					{
						if (DigitalEyewearARController.mInstance == null)
						{
							DigitalEyewearARController.mInstance = new DigitalEyewearARController();
						}
					}
				}
				return DigitalEyewearARController.mInstance;
			}
		}

		private ICameraConfiguration CameraConfiguration
		{
			get
			{
				if (this.mVuforiaBehaviour != null)
				{
					return this.mVuforiaBehaviour.CameraConfiguration;
				}
				return null;
			}
			set
			{
				this.mVuforiaBehaviour.CameraConfiguration = value;
			}
		}

		private DigitalEyewearARController()
		{
		}

		[RuntimeInitializeOnLoadMethod]
		public static void RegisterARController()
		{
			ARController.Register(DigitalEyewearARController.Instance);
		}

		protected override void Awake()
		{
			VuforiaAbstractConfiguration.DigitalEyewearConfiguration digitalEyewear = VuforiaAbstractConfiguration.Instance.DigitalEyewear;
			this.mCameraOffset = digitalEyewear.CameraOffset;
			this.mDistortionRenderingMode = digitalEyewear.DistortionRenderingMode;
			this.mDistortionRenderingLayer = digitalEyewear.DistortionRenderingLayer;
			this.mEyewearType = digitalEyewear.EyewearType;
			this.mStereoFramework = digitalEyewear.StereoFramework;
			this.mSeeThroughConfiguration = digitalEyewear.SeeThroughConfiguration;
			this.mViewerName = digitalEyewear.ViewerName;
			this.mViewerManufacturer = digitalEyewear.ViewerManufacturer;
			this.mUseCustomViewer = digitalEyewear.UseCustomViewer;
			this.mCustomViewer = digitalEyewear.CustomViewer;
			if (this.mEyewearType == DigitalEyewearARController.EyewearType.VideoSeeThrough && this.mStereoFramework != DigitalEyewearARController.StereoFramework.Vuforia)
			{
				this.mCentralAnchorPoint = base.VuforiaBehaviour.CentralAnchorPoint;
				this.mParentAnchorPoint = base.VuforiaBehaviour.ParentAnchorPoint;
				this.mPrimaryCamera = base.VuforiaBehaviour.PrimaryCamera;
				this.mSecondaryCamera = base.VuforiaBehaviour.SecondaryCamera;
			}
			if (this.mEyewearType == DigitalEyewearARController.EyewearType.OpticalSeeThrough && this.mSeeThroughConfiguration == DigitalEyewearARController.SeeThroughConfiguration.HoloLens)
			{
				this.mCentralAnchorPoint = base.VuforiaBehaviour.CentralAnchorPoint;
			}
			this.mVuforiaBehaviour = VuforiaARController.Instance;
			this.mVuforiaBehaviour.RegisterVuforiaInitializedCallback(new Action(this.OnVuforiaInitialized));
			this.mVuforiaBehaviour.RegisterVuforiaStartedCallback(new Action(this.OnVuforiaStarted));
			if (VuforiaRuntimeUtilities.IsPlayMode() && this.mEyewearType == DigitalEyewearARController.EyewearType.OpticalSeeThrough)
			{
				Device.SetPlayModeEyewearDevice();
				return;
			}
			Device.UnsetDevice();
		}

		protected override void Start()
		{
			this.mVuforiaBehaviour.RegisterVideoBgEventHandler(this);
		}

		protected override void Update()
		{
			if (this.mEyewearType != DigitalEyewearARController.EyewearType.VideoSeeThrough || this.mStereoFramework == DigitalEyewearARController.StereoFramework.Vuforia)
			{
				this.mVuforiaBehaviour.UpdateState(false, false);
			}
		}

		protected override void OnDestroy()
		{
			this.mVuforiaBehaviour.UnregisterVideoBgEventHandler(this);
			this.mVuforiaBehaviour.UnregisterVuforiaInitializedCallback(new Action(this.OnVuforiaInitialized));
			this.mVuforiaBehaviour.UnregisterVuforiaStartedCallback(new Action(this.OnVuforiaStarted));
		}

		public void OnVideoBackgroundConfigChanged()
		{
			if (this.mDistortionRenderingBhvr != null)
			{
				this.mDistortionRenderingBhvr.VideoBackgroundChanged = true;
			}
		}

		public void SetCentralAnchorPoint(Transform anchorPoint)
		{
			this.mCentralAnchorPoint = anchorPoint;
			if (Application.isPlaying)
			{
				if (this.mCentralAnchorPoint != null)
				{
					VuforiaManager.Instance.CentralAnchorPoint = this.mCentralAnchorPoint;
					return;
				}
				VuforiaManager.Instance.CentralAnchorPoint = base.VuforiaBehaviour.transform;
			}
		}

		public void SetParentAnchorPoint(Transform parentAnchorPoint)
		{
			this.mParentAnchorPoint = parentAnchorPoint;
			if (Application.isPlaying)
			{
				if (this.mParentAnchorPoint != null)
				{
					VuforiaManager.Instance.ParentAnchorPoint = this.mParentAnchorPoint;
					return;
				}
				VuforiaManager.Instance.ParentAnchorPoint = base.VuforiaBehaviour.transform;
			}
		}

		public void SetCameraOffset(float Offset)
		{
			this.CameraOffset = Offset;
		}

		public void SetViewerActive(bool isActive, bool reinitializeCamera)
		{
			CameraDevice.CameraDirection camDirection;
			CameraDevice.Instance.GetSelectedCameraDirection(out camDirection);
			CameraDevice.CameraDeviceMode mode;
			CameraDevice.Instance.GetSelectedVideoMode(out mode);
			this.SetViewerActive(isActive, reinitializeCamera, reinitializeCamera, camDirection, mode);
		}

		public void SetViewerActive(bool isActive)
		{
			CameraDevice.CameraDirection camDirection;
			CameraDevice.CameraDeviceMode mode;
			if (CameraDevice.Instance.GetSelectedCameraDirection(out camDirection) && CameraDevice.Instance.GetSelectedVideoMode(out mode))
			{
				this.SetViewerActive(isActive, true, true, camDirection, mode);
			}
		}

		public void SetMode(Device.Mode mode)
		{
			if (mode == Device.Mode.MODE_VR && this.CameraConfiguration != null && this.CameraConfiguration is MonoCameraConfiguration)
			{
				this.mPrimaryCamera.fieldOfView = 60f;
			}
			Device.Instance.SetMode(mode);
			if (this.CameraConfiguration != null)
			{
				this.CameraConfiguration.SetCameraParameterChanged();
			}
		}

		public void SetDistortionRendering(DistortionRenderingMode mode)
		{
			if (this.mDistortionRenderingMode != mode)
			{
				this.mDistortionRenderingMode = mode;
				if (Application.isPlaying)
				{
					if (mode == DistortionRenderingMode.None)
					{
						this.DisableDistortionRendering();
					}
					else
					{
						this.EnableDistortionRendering();
					}
					if (this.CameraConfiguration != null)
					{
						this.CameraConfiguration.SetCameraParameterChanged();
						StereoViewerCameraConfiguration stereoViewerCameraConfiguration = this.CameraConfiguration as StereoViewerCameraConfiguration;
						if (stereoViewerCameraConfiguration != null)
						{
							stereoViewerCameraConfiguration.SetDistortion(this.mDistortionRenderingMode > DistortionRenderingMode.None);
						}
					}
				}
			}
		}

		public DigitalEyewearARController.EyewearType GetEyewearType()
		{
			return this.mEyewearType;
		}

		public void SetEyewearType(DigitalEyewearARController.EyewearType type)
		{
			this.mEyewearType = type;
		}

		public DigitalEyewearARController.StereoFramework GetStereoCameraConfig()
		{
			return this.mStereoFramework;
		}

		public void SetStereoCameraConfiguration(DigitalEyewearARController.StereoFramework config)
		{
			this.mStereoFramework = config;
		}

		public DigitalEyewearARController.SeeThroughConfiguration GetSeeThroughConfiguration()
		{
			return this.mSeeThroughConfiguration;
		}

		public void SetSeeThroughConfiguration(DigitalEyewearARController.SeeThroughConfiguration config)
		{
			this.mSeeThroughConfiguration = config;
		}

		public void EnableAutomaticFocusPointSelection(bool enable)
		{
			this.mSetFocusPlaneAutomatically = enable;
		}

		public void EnableWorldAnchorUsage(bool enable)
		{
			((StateManagerImpl)TrackerManager.Instance.GetStateManager()).GetExtendedTrackingManager().EnableWorldAnchorUsage(enable);
		}

		internal void SetFocusPoint()
		{
			if (this.mSetFocusPlaneAutomatically)
			{
				StateManager arg_23_0 = TrackerManager.Instance.GetStateManager();
				Vector3 point = Vector3.zero;
				Vector3 vector = Vector3.zero;
				bool flag = false;
				using (IEnumerator<TrackableBehaviour> enumerator = arg_23_0.GetActiveTrackableBehaviours().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Vector3 position = enumerator.Current.transform.position;
						Vector3 vector2 = this.mCentralAnchorPoint.transform.InverseTransformPoint(position);
						if (vector2.z > 0.5f)
						{
							if (!flag)
							{
								point = position;
								vector = vector2;
								flag = true;
							}
							else
							{
								Vector2 vector3 = new Vector2(vector.x, vector.y);
								Vector2 vector4 = new Vector2(vector2.x, vector2.y);
								if (vector3.magnitude > vector4.magnitude)
								{
									point = position;
									vector = vector2;
								}
							}
						}
					}
				}
				if (flag)
				{
					Vector3 normal = -this.mCentralAnchorPoint.transform.forward;
					VuforiaUnity.GetHoloLensApiAbstraction().SetFocusPoint(point, normal);
				}
			}
		}

		private void OnVuforiaInitialized()
		{
			this.CorrectCameraConfiguration();
			VuforiaManager.Instance.CentralAnchorPoint = this.mCentralAnchorPoint;
			VuforiaManager.Instance.ParentAnchorPoint = this.mParentAnchorPoint;
			if (!VuforiaRuntimeUtilities.IsVuforiaEnabled())
			{
				return;
			}
			if (this.mEyewearType == DigitalEyewearARController.EyewearType.None || this.mEyewearType == DigitalEyewearARController.EyewearType.OpticalSeeThrough)
			{
				Device.Instance.SetViewerActive(false);
			}
			else if (this.mEyewearType == DigitalEyewearARController.EyewearType.VideoSeeThrough)
			{
				Device.Instance.SetViewerActive(false);
				if (this.mStereoFramework == DigitalEyewearARController.StereoFramework.Vuforia)
				{
					if (this.mUseCustomViewer)
					{
						CustomViewerParameters customViewerParameters = new CustomViewerParameters(this.mCustomViewer.Version, this.mCustomViewer.Name, this.mCustomViewer.Manufacturer);
						customViewerParameters.SetButtonType(this.mCustomViewer.ButtonType);
						customViewerParameters.SetScreenToLensDistance(this.mCustomViewer.ScreenToLensDistance);
						customViewerParameters.SetInterLensDistance(this.mCustomViewer.InterLensDistance);
						customViewerParameters.SetTrayAlignment(this.mCustomViewer.TrayAlignment);
						customViewerParameters.SetLensCentreToTrayDistance(this.mCustomViewer.LensCenterToTrayDistance);
						customViewerParameters.AddDistortionCoefficient(this.mCustomViewer.DistortionCoefficients[0]);
						customViewerParameters.AddDistortionCoefficient(this.mCustomViewer.DistortionCoefficients[1]);
						customViewerParameters.SetFieldOfView(this.mCustomViewer.FieldOfView);
						customViewerParameters.SetContainsMagnet(this.mCustomViewer.ContainsMagnet);
						Device.Instance.SelectViewer(customViewerParameters);
					}
					else
					{
						IViewerParametersList viewerList = Device.Instance.GetViewerList();
						if (viewerList != null)
						{
							IViewerParameters vp = viewerList.Get(this.mViewerName, this.mViewerManufacturer);
							if (!Device.Instance.SelectViewer(vp))
							{
								Debug.LogWarning("Couldn't select viewer " + this.mViewerName);
							}
						}
						else
						{
							Debug.LogWarning("Couldn't select viewer " + this.mViewerName + "(no viewer available)");
						}
					}
				}
				else
				{
					string text = "";
					if (this.mStereoFramework == DigitalEyewearARController.StereoFramework.GearVR)
					{
						text = "GEARVR";
					}
					else if (this.mStereoFramework == DigitalEyewearARController.StereoFramework.Cardboard)
					{
						text = "CARDBOARD";
					}
					IViewerParametersList viewerList2 = Device.Instance.GetViewerList();
					if (viewerList2 != null)
					{
						viewerList2.SetSDKFilter(text);
						if (viewerList2.Size() > 0)
						{
							Device.Instance.SelectViewer(viewerList2.Get(0));
						}
						else
						{
							Debug.LogWarning(text + " is not supported");
						}
					}
				}
				Device.Instance.SetViewerActive(true);
			}
			StateManagerImpl stateManagerImpl = (StateManagerImpl)TrackerManager.Instance.GetStateManager();
			if (this.mEyewearType == DigitalEyewearARController.EyewearType.OpticalSeeThrough && this.mSeeThroughConfiguration == DigitalEyewearARController.SeeThroughConfiguration.HoloLens)
			{
				stateManagerImpl.SetExtendedTrackingManager(new HoloLensExtendedTrackingManager());
				if (Screen.orientation != ScreenOrientation.Landscape)
				{
					Screen.autorotateToPortrait = false;
					Screen.autorotateToPortraitUpsideDown = false;
					Screen.autorotateToLandscapeLeft = false;
					Screen.autorotateToLandscapeRight = false;
					Screen.orientation = ScreenOrientation.Landscape;
					return;
				}
			}
			else
			{
				stateManagerImpl.SetExtendedTrackingManager(new VuforiaExtendedTrackingManager());
				this.mSetFocusPlaneAutomatically = false;
			}
		}

		private void OnVuforiaStarted()
		{
			this.ConfigureView();
		}

		private void SetViewerActive(bool isActive, bool deinitCam, bool initCam, CameraDevice.CameraDirection camDirection, CameraDevice.CameraDeviceMode mode)
		{
			if (deinitCam)
			{
				CameraDevice.Instance.Stop();
				CameraDevice.Instance.Deinit();
			}
			Device.Instance.SetViewerActive(isActive);
			if (initCam)
			{
				CameraDevice.Instance.Init(camDirection);
				CameraDevice.Instance.SelectVideoMode(mode);
				CameraDevice.Instance.Start();
			}
			this.ConfigureView();
		}

		private void EnableDistortionRendering()
		{
			if (this.mDistortionRenderingMode == DistortionRenderingMode.None || this.mEyewearType != DigitalEyewearARController.EyewearType.VideoSeeThrough || this.mStereoFramework != DigitalEyewearARController.StereoFramework.Vuforia)
			{
				return;
			}
			if (this.mDistortionRenderingBhvr == null)
			{
				this.mDistortionRenderingBhvr = new GameObject("DistortionCamera").AddComponent<DistortionRenderingBehaviour>();
			}
			this.mDistortionRenderingBhvr.RenderLayer = this.mDistortionRenderingLayer;
			this.mDistortionRenderingBhvr.UseSingleTexture = (this.mDistortionRenderingMode == DistortionRenderingMode.SingleTexture);
			this.mDistortionRenderingBhvr.gameObject.SetActive(true);
		}

		private void DisableDistortionRendering()
		{
			if (this.mDistortionRenderingBhvr != null)
			{
				this.mDistortionRenderingBhvr.gameObject.SetActive(false);
			}
		}

		private void CorrectCameraConfiguration()
		{
			VuforiaARController.WorldCenterMode worldCenterMode = VuforiaManager.Instance.WorldCenterMode;
			Camera camera = null;
			Camera[] componentsInChildren = base.VuforiaBehaviour.GetComponentsInChildren<Camera>(true);
			int num = 0;
			if (num < componentsInChildren.Length)
			{
				camera = componentsInChildren[num];
			}
			if (this.mEyewearType == DigitalEyewearARController.EyewearType.VideoSeeThrough && this.mStereoFramework != DigitalEyewearARController.StereoFramework.Vuforia)
			{
				if (camera != null)
				{
					camera.gameObject.SetActive(false);
				}
				AudioListener component = base.VuforiaBehaviour.GetComponent<AudioListener>();
				if (component != null)
				{
					component.enabled = false;
				}
				if (worldCenterMode == VuforiaARController.WorldCenterMode.CAMERA || worldCenterMode == VuforiaARController.WorldCenterMode.DEVICE_TRACKING)
				{
					this.mParentAnchorPoint = this.mCentralAnchorPoint;
					return;
				}
			}
			else if (this.mEyewearType == DigitalEyewearARController.EyewearType.OpticalSeeThrough && this.mSeeThroughConfiguration == DigitalEyewearARController.SeeThroughConfiguration.HoloLens)
			{
				if (camera != null)
				{
					camera.gameObject.SetActive(false);
				}
				this.mPrimaryCamera = null;
				AudioListener component2 = base.VuforiaBehaviour.GetComponent<AudioListener>();
				if (component2 != null)
				{
					component2.enabled = false;
				}
				this.mParentAnchorPoint = this.mCentralAnchorPoint;
				if (worldCenterMode != VuforiaARController.WorldCenterMode.CAMERA)
				{
					VuforiaManager.Instance.WorldCenterMode = VuforiaARController.WorldCenterMode.CAMERA;
					return;
				}
			}
			else
			{
				if (camera != null)
				{
					camera.gameObject.SetActive(true);
					this.mPrimaryCamera = camera;
				}
				this.mCentralAnchorPoint = base.VuforiaBehaviour.transform;
				this.mParentAnchorPoint = base.VuforiaBehaviour.transform;
			}
		}

		private void ConfigureView()
		{
			EyewearDevice eyewearDevice = Device.Instance as EyewearDevice;
			bool arg_4E_0 = Device.Instance.IsViewerActive() || (eyewearDevice != null && eyewearDevice.IsDualDisplay());
			bool flag = this.mCentralAnchorPoint != base.VuforiaBehaviour.transform && this.mCentralAnchorPoint != null;
			if (!arg_4E_0)
			{
				if (this.mSecondaryCamera && this.mSecondaryCamera.enabled)
				{
					this.mPrimaryCameraOriginalRect = new Rect(this.mPrimaryCamera.rect);
					this.mSecondaryCameraOriginalRect = new Rect(this.mSecondaryCamera.rect);
					this.mSecondaryCamera.enabled = false;
					this.mSecondaryCameraDisabledLocally = true;
					this.DisableDistortionRendering();
					Rect rect = new Rect(0f, 0f, 1f, 1f);
					this.mPrimaryCamera.rect = rect;
					this.mPrimaryCamera.fieldOfView = 60f;
				}
			}
			else
			{
				if (this.mSecondaryCamera && !this.mSecondaryCamera.enabled)
				{
					if (!flag)
					{
						this.mSecondaryCamera.enabled = true;
					}
					if (this.mSecondaryCameraDisabledLocally)
					{
						this.mSecondaryCameraDisabledLocally = false;
						this.mPrimaryCamera.rect = this.mPrimaryCameraOriginalRect;
						this.mSecondaryCamera.rect = this.mSecondaryCameraOriginalRect;
					}
				}
				if (!flag && this.mSecondaryCamera == null)
				{
					this.mPrimaryCamera.name = "StereoCameraLeft";
					this.mPrimaryCamera.rect = new Rect(0f, 0f, 0.5f, 1f);
					this.mSecondaryCamera = UnityEngine.Object.Instantiate<Camera>(this.mPrimaryCamera);
					this.mSecondaryCamera.transform.parent = this.mPrimaryCamera.transform.parent;
					this.mSecondaryCamera.transform.localPosition = this.mPrimaryCamera.transform.localPosition;
					this.mSecondaryCamera.transform.localRotation = this.mPrimaryCamera.transform.localRotation;
					this.mSecondaryCamera.transform.localScale = this.mPrimaryCamera.transform.localScale;
					this.mSecondaryCamera.name = "StereoCameraRight";
					this.mSecondaryCamera.rect = new Rect(0.5f, 0f, 0.5f, 1f);
					BackgroundPlaneAbstractBehaviour componentInChildren = this.mSecondaryCamera.GetComponentInChildren<BackgroundPlaneAbstractBehaviour>();
					if (componentInChildren != null)
					{
                        UnityEngine.Object.Destroy(componentInChildren.gameObject);
					}
					VideoBackgroundAbstractBehaviour component = this.mSecondaryCamera.GetComponent<VideoBackgroundAbstractBehaviour>();
					if (component != null)
					{
						component.DisconnectFromBackgroundBehaviour();
					}
				}
			}
			if (this.mPrimaryCamera && VuforiaARController.IsValidPrimaryCamera(this.mPrimaryCamera))
			{
				this.mPrimaryCamera.transform.localRotation = Quaternion.identity;
				this.mPrimaryCamera.transform.localScale = Vector3.one;
				if (this.mSecondaryCamera && (flag || this.mSecondaryCamera.enabled) && VuforiaARController.IsValidSecondaryCamera(this.mSecondaryCamera))
				{
					this.mSecondaryCamera.transform.localRotation = Quaternion.identity;
					this.mSecondaryCamera.transform.localScale = Vector3.one;
					if (flag)
					{
						this.CameraConfiguration = new ExternalStereoCameraConfiguration(this.mPrimaryCamera, this.mSecondaryCamera);
						Debug.Log("Creating an External Stereo Camera Configuration (Check for reflection)");
					}
					else if (eyewearDevice != null)
					{
						this.CameraConfiguration = new DedicatedEyewearCameraConfiguration(this.mPrimaryCamera, this.mSecondaryCamera);
						Debug.Log("Creating a Dedicated Eyewear Camera Configuration (Check for reflection)");
					}
					else
					{
						this.CameraConfiguration = new StereoViewerCameraConfiguration(this.mPrimaryCamera, this.mSecondaryCamera, this.mCameraOffset, this.mDistortionRenderingMode > DistortionRenderingMode.None);
						Debug.Log("Creating a Stereo Viewer Camera Configuration (Check for reflection)");
						this.EnableDistortionRendering();
					}
				}
				else
				{
					this.CameraConfiguration = new MonoCameraConfiguration(this.mPrimaryCamera);
					Debug.Log("Creating a Mono Camera Configuration (Check for reflection)");
				}
			}
			else
			{
				this.CameraConfiguration = new NullCameraConfiguration();
				Debug.Log("Creating a Null Camera Configuration (Check for reflection)");
			}
			this.mVuforiaBehaviour.InitCameraConfiguration();
			this.CameraConfiguration.Init();
			if (this.mPrimaryCamera != null)
			{
				HideExcessAreaAbstractBehaviour component2 = this.mPrimaryCamera.GetComponent<HideExcessAreaAbstractBehaviour>();
				if (component2)
				{
					component2.OnConfigurationChanged();
				}
			}
			if (this.mSecondaryCamera != null)
			{
				HideExcessAreaAbstractBehaviour component3 = this.mSecondaryCamera.GetComponent<HideExcessAreaAbstractBehaviour>();
				if (component3)
				{
					component3.OnConfigurationChanged();
				}
			}
		}
	}
}
