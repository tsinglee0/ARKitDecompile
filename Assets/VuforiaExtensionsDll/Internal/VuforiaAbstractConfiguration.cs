using System;
using UnityEngine;

namespace Vuforia
{
	public abstract class VuforiaAbstractConfiguration : ScriptableObject
	{
		[Serializable]
		public class GenericVuforiaConfiguration
		{
			[SerializeField]
			private string vuforiaLicenseKey = "";

			[SerializeField]
			private bool delayedInitialization;

			[SerializeField]
			private CameraDevice.CameraDeviceMode cameraDeviceModeSetting = CameraDevice.CameraDeviceMode.MODE_DEFAULT;

			[SerializeField]
			private int maxSimultaneousImageTargets = 1;

			[SerializeField]
			private int maxSimultaneousObjectTargets = 1;

			[SerializeField]
			private bool useDelayedLoadingObjectTargets;

			[SerializeField]
			private CameraDevice.CameraDirection cameraDirection;

			[SerializeField]
			private VuforiaRenderer.VideoBackgroundReflection mirrorVideoBackground;

			public string LicenseKey
			{
				get
				{
					return this.vuforiaLicenseKey;
				}
				set
				{
					this.vuforiaLicenseKey = value;
				}
			}

			public bool DelayedInitialization
			{
				get
				{
					return this.delayedInitialization;
				}
				set
				{
					this.delayedInitialization = value;
				}
			}

			public CameraDevice.CameraDeviceMode CameraDeviceMode
			{
				get
				{
					return this.cameraDeviceModeSetting;
				}
				set
				{
					this.cameraDeviceModeSetting = value;
				}
			}

			public int MaxSimultaneousImageTargets
			{
				get
				{
					return this.maxSimultaneousImageTargets;
				}
				set
				{
					this.maxSimultaneousImageTargets = value;
				}
			}

			public int MaxSimultaneousObjectTargets
			{
				get
				{
					return this.maxSimultaneousObjectTargets;
				}
				set
				{
					this.maxSimultaneousObjectTargets = value;
				}
			}

			public bool UseDelayedLoadingObjectTargets
			{
				get
				{
					return this.useDelayedLoadingObjectTargets;
				}
				set
				{
					this.useDelayedLoadingObjectTargets = value;
				}
			}

			public CameraDevice.CameraDirection CameraDirection
			{
				get
				{
					return this.cameraDirection;
				}
				set
				{
					this.cameraDirection = value;
				}
			}

			public VuforiaRenderer.VideoBackgroundReflection MirrorVideoBackground
			{
				get
				{
					return this.mirrorVideoBackground;
				}
				set
				{
					this.mirrorVideoBackground = value;
				}
			}
		}

		[Serializable]
		public class DigitalEyewearConfiguration
		{
			[SerializeField]
			private float cameraOffset = 0.06f;

			[SerializeField]
			private DistortionRenderingMode distortionRenderingMode = DistortionRenderingMode.DualTexture;

			[SerializeField]
			private int distortionRenderingLayer = 31;

			[SerializeField]
			private DigitalEyewearARController.EyewearType eyewearType;

			[SerializeField]
			private DigitalEyewearARController.StereoFramework stereoFramework;

			[SerializeField]
			private DigitalEyewearARController.SeeThroughConfiguration seeThroughConfiguration;

			[SerializeField]
			private string viewerName;

			[SerializeField]
			private string viewerManufacturer;

			[SerializeField]
			private bool useCustomViewer;

			[SerializeField]
			private DigitalEyewearARController.SerializableViewerParameters customViewer;

			public float CameraOffset
			{
				get
				{
					return this.cameraOffset;
				}
				set
				{
					this.cameraOffset = value;
				}
			}

			public DistortionRenderingMode DistortionRenderingMode
			{
				get
				{
					return this.distortionRenderingMode;
				}
				set
				{
					this.distortionRenderingMode = value;
				}
			}

			public int DistortionRenderingLayer
			{
				get
				{
					return this.distortionRenderingLayer;
				}
				set
				{
					this.distortionRenderingLayer = value;
				}
			}

			public DigitalEyewearARController.EyewearType EyewearType
			{
				get
				{
					return this.eyewearType;
				}
				set
				{
					this.eyewearType = value;
				}
			}

			public DigitalEyewearARController.StereoFramework StereoFramework
			{
				get
				{
					return this.stereoFramework;
				}
				set
				{
					this.stereoFramework = value;
				}
			}

			public DigitalEyewearARController.SeeThroughConfiguration SeeThroughConfiguration
			{
				get
				{
					return this.seeThroughConfiguration;
				}
				set
				{
					this.seeThroughConfiguration = value;
				}
			}

			public string ViewerName
			{
				get
				{
					return this.viewerName;
				}
				set
				{
					this.viewerName = value;
				}
			}

			public string ViewerManufacturer
			{
				get
				{
					return this.viewerManufacturer;
				}
				set
				{
					this.viewerManufacturer = value;
				}
			}

			public bool UseCustomViewer
			{
				get
				{
					return this.useCustomViewer;
				}
				set
				{
					this.useCustomViewer = value;
				}
			}

			public DigitalEyewearARController.SerializableViewerParameters CustomViewer
			{
				get
				{
					return this.customViewer;
				}
				set
				{
					this.customViewer = value;
				}
			}
		}

		[Serializable]
		public class DatabaseLoadConfiguration
		{
			[SerializeField]
			private string[] dataSetsToLoad = new string[0];

			[SerializeField]
			private string[] dataSetsToActivate = new string[0];

			public string[] DataSetsToLoad
			{
				get
				{
					return this.dataSetsToLoad;
				}
				set
				{
					this.dataSetsToLoad = value;
				}
			}

			public string[] DataSetsToActivate
			{
				get
				{
					return this.dataSetsToActivate;
				}
				set
				{
					this.dataSetsToActivate = value;
				}
			}
		}

		[Serializable]
		public class VideoBackgroundConfiguration
		{
			[SerializeField]
			private HideExcessAreaAbstractBehaviour.CLIPPING_MODE clippingMode;

			[SerializeField]
			private Shader matteShader;

			[SerializeField]
			private bool videoBackgroundEnabled = true;

			public HideExcessAreaAbstractBehaviour.CLIPPING_MODE ClippingMode
			{
				get
				{
					return this.clippingMode;
				}
				set
				{
					this.clippingMode = value;
				}
			}

			public Shader MatteShader
			{
				get
				{
					return this.matteShader;
				}
				set
				{
					this.matteShader = value;
				}
			}

			public bool VideoBackgroundEnabled
			{
				get
				{
					return this.videoBackgroundEnabled;
				}
				set
				{
					this.videoBackgroundEnabled = value;
				}
			}

			public static Shader GetDefaultMatteShader(HideExcessAreaAbstractBehaviour.CLIPPING_MODE mode)
			{
				if (mode == HideExcessAreaAbstractBehaviour.CLIPPING_MODE.STENCIL)
				{
					return Shader.Find("ClippingMask");
				}
				if (mode != HideExcessAreaAbstractBehaviour.CLIPPING_MODE.PLANES)
				{
					return null;
				}
				return Shader.Find("DepthMask");
			}

			public void SetDefaultMatteShader()
			{
				this.matteShader = VuforiaAbstractConfiguration.VideoBackgroundConfiguration.GetDefaultMatteShader(this.clippingMode);
			}
		}

		public abstract class TrackerConfiguration
		{
			[SerializeField]
			private bool autoInitTracker;

			[SerializeField]
			private bool autoStartTracker;

			public bool AutoInitAndStartTracker
			{
				get
				{
					return this.autoInitTracker;
				}
				set
				{
					this.autoStartTracker = value;
					this.autoInitTracker = value;
				}
			}
		}

		[Serializable]
		public class SmartTerrainTrackerConfiguration : VuforiaAbstractConfiguration.TrackerConfiguration
		{
			[SerializeField]
			private bool autoInitBuilder;

			[SerializeField]
			private float sceneUnitsToMillimeter;

			public bool AutoInitBuilder
			{
				get
				{
					return this.autoInitBuilder;
				}
				set
				{
					this.autoInitBuilder = value;
				}
			}

			public float SceneUnitsToMillimeter
			{
				get
				{
					return this.sceneUnitsToMillimeter;
				}
				set
				{
					this.sceneUnitsToMillimeter = value;
				}
			}
		}

		[Serializable]
		public class DeviceTrackerConfiguration : VuforiaAbstractConfiguration.TrackerConfiguration
		{
			[SerializeField]
			private bool posePrediction;

			[SerializeField]
			private RotationalDeviceTracker.MODEL_CORRECTION_MODE modelCorrectionMode;

			[SerializeField]
			private bool modelTransformEnabled;

			[SerializeField]
			private Vector3 modelTransform = new Vector3(0f, 0.1f, -0.1f);

			public bool PosePrediction
			{
				get
				{
					return this.posePrediction;
				}
				set
				{
					this.posePrediction = value;
				}
			}

			public RotationalDeviceTracker.MODEL_CORRECTION_MODE ModelCorrectionMode
			{
				get
				{
					return this.modelCorrectionMode;
				}
				set
				{
					this.modelCorrectionMode = value;
				}
			}

			public bool ModelTransformEnabled
			{
				get
				{
					return this.modelTransformEnabled;
				}
				set
				{
					this.modelTransformEnabled = value;
				}
			}

			public Vector3 ModelTransform
			{
				get
				{
					return this.modelTransform;
				}
				set
				{
					this.modelTransform = value;
				}
			}
		}

		[Serializable]
		public class WebCamConfiguration
		{
			[SerializeField]
			private string deviceNameSetInEditor;

			[SerializeField]
			private bool flipHorizontally;

			[SerializeField]
			private bool turnOffWebCam;

			[SerializeField]
			private int renderTextureLayer = 30;

			public string DeviceNameSetInEditor
			{
				get
				{
					return this.deviceNameSetInEditor;
				}
			}

			public bool FlipHorizontally
			{
				get
				{
					return this.flipHorizontally;
				}
			}

			public bool TurnOffWebCam
			{
				get
				{
					return this.turnOffWebCam;
				}
			}

			public int RenderTextureLayer
			{
				get
				{
					return this.renderTextureLayer;
				}
			}
		}

		private static VuforiaAbstractConfiguration mInstance;

		private static object mPadlock = new object();

		[SerializeField]
		private VuforiaAbstractConfiguration.GenericVuforiaConfiguration vuforia = new VuforiaAbstractConfiguration.GenericVuforiaConfiguration();

		[SerializeField]
		private VuforiaAbstractConfiguration.DigitalEyewearConfiguration digitalEyewear = new VuforiaAbstractConfiguration.DigitalEyewearConfiguration();

		[SerializeField]
		private VuforiaAbstractConfiguration.DatabaseLoadConfiguration databaseLoad = new VuforiaAbstractConfiguration.DatabaseLoadConfiguration();

		[SerializeField]
		private VuforiaAbstractConfiguration.VideoBackgroundConfiguration videoBackground = new VuforiaAbstractConfiguration.VideoBackgroundConfiguration();

		[SerializeField]
		private VuforiaAbstractConfiguration.SmartTerrainTrackerConfiguration smartTerrainTracker = new VuforiaAbstractConfiguration.SmartTerrainTrackerConfiguration();

		[SerializeField]
		private VuforiaAbstractConfiguration.DeviceTrackerConfiguration deviceTracker = new VuforiaAbstractConfiguration.DeviceTrackerConfiguration();

		[SerializeField]
		private VuforiaAbstractConfiguration.WebCamConfiguration webcam = new VuforiaAbstractConfiguration.WebCamConfiguration();

		public static VuforiaAbstractConfiguration Instance
		{
			get
			{
				if (VuforiaAbstractConfiguration.mInstance == null)
				{
					object obj = VuforiaAbstractConfiguration.mPadlock;
					lock (obj)
					{
						if (VuforiaAbstractConfiguration.mInstance == null)
						{
							VuforiaAbstractConfiguration.mInstance = Resources.Load<VuforiaAbstractConfiguration>("VuforiaConfiguration");
						}
						if (VuforiaAbstractConfiguration.mInstance == null)
						{
							VuforiaAbstractConfiguration.mInstance = VuforiaAbstractConfiguration.CreateInstance();
						}
						VuforiaAbstractConfiguration.mInstance = UnityEngine.Object.Instantiate<VuforiaAbstractConfiguration>(VuforiaAbstractConfiguration.mInstance);
					}
				}
				return VuforiaAbstractConfiguration.mInstance;
			}
		}

		public VuforiaAbstractConfiguration.GenericVuforiaConfiguration Vuforia
		{
			get
			{
				return this.vuforia;
			}
		}

		public VuforiaAbstractConfiguration.DigitalEyewearConfiguration DigitalEyewear
		{
			get
			{
				return this.digitalEyewear;
			}
		}

		public VuforiaAbstractConfiguration.DatabaseLoadConfiguration DatabaseLoad
		{
			get
			{
				return this.databaseLoad;
			}
		}

		public VuforiaAbstractConfiguration.VideoBackgroundConfiguration VideoBackground
		{
			get
			{
				return this.videoBackground;
			}
		}

		public VuforiaAbstractConfiguration.SmartTerrainTrackerConfiguration SmartTerrainTracker
		{
			get
			{
				return this.smartTerrainTracker;
			}
		}

		public VuforiaAbstractConfiguration.DeviceTrackerConfiguration DeviceTracker
		{
			get
			{
				return this.deviceTracker;
			}
		}

		public VuforiaAbstractConfiguration.WebCamConfiguration WebCam
		{
			get
			{
				return this.webcam;
			}
		}

		public static VuforiaAbstractConfiguration CreateInstance()
		{
			VuforiaAbstractConfiguration expr_0A = BehaviourComponentFactory.Instance.CreateVuforiaConfiguration();
			expr_0A.VideoBackground.SetDefaultMatteShader();
			return expr_0A;
		}
	}
}
