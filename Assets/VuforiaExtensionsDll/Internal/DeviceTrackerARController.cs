using System;
using UnityEngine;

namespace Vuforia
{
	public class DeviceTrackerARController : ARController
	{
		public static readonly Vector3 DEFAULT_HEAD_PIVOT = new Vector3(0f, 0.075f, 0.12f);

		public static readonly Vector3 DEFAULT_HANDHELD_PIVOT = new Vector3(0f, 0f, 0.264f);

		private bool mAutoInitTracker;

		private bool mAutoStartTracker;

		private bool mPosePrediction;

		private RotationalDeviceTracker.MODEL_CORRECTION_MODE mModelCorrectionMode;

		private bool mModelTransformEnabled;

		private Vector3 mModelTransform = new Vector3(0f, 0.1f, -0.1f);

		private Action mTrackerStarted;

		private bool mTrackerWasActiveBeforePause;

		private bool mTrackerWasActiveBeforeDisabling;

		private static DeviceTrackerARController mInstance;

		private static object mPadlock = new object();

		public static DeviceTrackerARController Instance
		{
			get
			{
				if (DeviceTrackerARController.mInstance == null)
				{
					object obj = DeviceTrackerARController.mPadlock;
					lock (obj)
					{
						if (DeviceTrackerARController.mInstance == null)
						{
							DeviceTrackerARController.mInstance = new DeviceTrackerARController();
						}
					}
				}
				return DeviceTrackerARController.mInstance;
			}
		}

		internal bool AutoInitTracker
		{
			get
			{
				return this.mAutoInitTracker;
			}
		}

		internal bool AutoStartTracker
		{
			get
			{
				return this.mAutoStartTracker;
			}
		}

		internal RotationalDeviceTracker.MODEL_CORRECTION_MODE ModelCorrectionMode
		{
			get
			{
				return this.mModelCorrectionMode;
			}
		}

		internal bool ModelTransformEnabled
		{
			get
			{
				return this.mModelTransformEnabled;
			}
		}

		internal Vector3 ModelTransform
		{
			get
			{
				return this.mModelTransform;
			}
		}

		private DeviceTrackerARController()
		{
		}

		[RuntimeInitializeOnLoadMethod]
		public static void RegisterARController()
		{
			ARController.Register(DeviceTrackerARController.Instance);
		}

		protected override void Awake()
		{
			if (!VuforiaRuntimeUtilities.IsVuforiaEnabled())
			{
				return;
			}
			VuforiaAbstractConfiguration.DeviceTrackerConfiguration deviceTracker = VuforiaAbstractConfiguration.Instance.DeviceTracker;
			this.mAutoInitTracker = deviceTracker.AutoInitAndStartTracker;
			this.mAutoStartTracker = deviceTracker.AutoInitAndStartTracker;
			this.mPosePrediction = deviceTracker.PosePrediction;
			this.mModelCorrectionMode = deviceTracker.ModelCorrectionMode;
			this.mModelTransformEnabled = deviceTracker.ModelTransformEnabled;
			this.mModelTransform = deviceTracker.ModelTransform;
			VuforiaARController expr_60 = VuforiaARController.Instance;
			expr_60.RegisterVuforiaInitializedCallback(new Action(this.OnVuforiaInitialized));
			expr_60.RegisterVuforiaStartedCallback(new Action(this.OnVuforiaStarted));
			expr_60.RegisterOnPauseCallback(new Action<bool>(this.OnPause));
		}

		protected override void OnEnable()
		{
			if (this.mTrackerWasActiveBeforeDisabling)
			{
				this.StartDeviceTracker();
			}
		}

		protected override void OnDisable()
		{
			DeviceTracker tracker = TrackerManager.Instance.GetTracker<DeviceTracker>();
			if (tracker != null)
			{
				this.mTrackerWasActiveBeforeDisabling = tracker.IsActive;
				if (tracker.IsActive)
				{
					this.StopDeviceTracker();
				}
			}
		}

		protected override void OnDestroy()
		{
			VuforiaARController expr_05 = VuforiaARController.Instance;
			expr_05.UnregisterVuforiaInitializedCallback(new Action(this.OnVuforiaInitialized));
			expr_05.UnregisterVuforiaStartedCallback(new Action(this.OnVuforiaStarted));
			expr_05.UnregisterOnPauseCallback(new Action<bool>(this.OnPause));
		}

		protected override void Update()
		{
			if (!VuforiaRuntimeUtilities.IsPlayMode())
			{
				return;
			}
			RotationalPlayModeDeviceTrackerImpl rotationalPlayModeDeviceTrackerImpl = TrackerManager.Instance.GetTracker<DeviceTracker>() as RotationalPlayModeDeviceTrackerImpl;
			if (rotationalPlayModeDeviceTrackerImpl == null)
			{
				return;
			}
			Vector3 rotation = rotationalPlayModeDeviceTrackerImpl.Rotation;
			if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
			{
				float num = Input.GetAxis("Mouse X") * 100f * Time.deltaTime;
				rotation.z += num;
				rotation.z = Mathf.Clamp(rotation.z, -90f, 90f);
				rotationalPlayModeDeviceTrackerImpl.Rotation = rotation;
			}
			else
			{
				float num2 = Mathf.Abs(rotation.z);
				if (num2 > 0f)
				{
					float num3 = 100f * Time.deltaTime;
					if (num2 < num3)
					{
						rotation.z = 0f;
					}
					else
					{
						rotation.z += -Mathf.Sign(rotation.z) * num3;
					}
					rotationalPlayModeDeviceTrackerImpl.Rotation = rotation;
				}
			}
			if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
			{
				float num4 = Input.GetAxis("Mouse X") * 100f * Time.deltaTime;
				rotation.y += num4;
				float num5 = -Input.GetAxis("Mouse Y") * 100f * Time.deltaTime;
				rotation.x += num5;
				rotationalPlayModeDeviceTrackerImpl.Rotation = rotation;
			}
		}

		public void RegisterTrackerStartedCallback(Action callback)
		{
			this.mTrackerStarted = (Action)Delegate.Combine(this.mTrackerStarted, callback);
			DeviceTracker tracker = TrackerManager.Instance.GetTracker<DeviceTracker>();
			if (tracker != null && tracker.IsActive)
			{
				callback();
			}
		}

		public void UnregisterTrackerStartedCallback(Action callback)
		{
			this.mTrackerStarted = (Action)Delegate.Remove(this.mTrackerStarted, callback);
		}

		public void RegisterBeforeDevicePoseUpdateCallback(Action callback)
		{
			((StateManagerImpl)TrackerManager.Instance.GetStateManager()).GetDeviceTrackingManager().RegisterBeforeDevicePoseUpdateCallback(callback);
		}

		public void UnregisterBeforeDevicePoseUpdateCallback(Action callback)
		{
			((StateManagerImpl)TrackerManager.Instance.GetStateManager()).GetDeviceTrackingManager().UnregisterBeforeDevicePoseUpdateCallback(callback);
		}

		public void RegisterDevicePoseUpdatedCallback(Action callback)
		{
			((StateManagerImpl)TrackerManager.Instance.GetStateManager()).GetDeviceTrackingManager().RegisterDevicePoseUpdatedCallback(callback);
		}

		public void UnregisterDevicePoseUpdatedCallback(Action callback)
		{
			((StateManagerImpl)TrackerManager.Instance.GetStateManager()).GetDeviceTrackingManager().UnregisterDevicePoseUpdatedCallback(callback);
		}

		private void StartDeviceTracker()
		{
			Debug.Log("Starting Device Tracker");
			DeviceTracker tracker = TrackerManager.Instance.GetTracker<DeviceTracker>();
			if (tracker != null)
			{
				tracker.Start();
				if (this.mTrackerStarted != null)
				{
					this.mTrackerStarted.InvokeWithExceptionHandling();
				}
			}
		}

		private void StopDeviceTracker()
		{
			Debug.Log("Stopping Device Tracker");
			DeviceTracker tracker = TrackerManager.Instance.GetTracker<DeviceTracker>();
			if (tracker != null)
			{
				tracker.Stop();
			}
		}

		private void InitDeviceTracker()
		{
			if (TrackerManager.Instance.GetTracker<DeviceTracker>() == null)
			{
				TrackerManager.Instance.InitTracker<DeviceTracker>();
			}
		}

		private void ApplySettings()
		{
			RotationalDeviceTracker tracker = TrackerManager.Instance.GetTracker<RotationalDeviceTracker>();
			if (tracker != null)
			{
				tracker.SetPosePrediction(this.mPosePrediction);
				tracker.SetModelCorrectionMode(this.mModelCorrectionMode);
				if (this.mModelTransformEnabled)
				{
					tracker.SetModelCorrectionModeWithTransform(this.mModelCorrectionMode, this.mModelTransform);
				}
			}
		}

		internal void OnVuforiaInitialized()
		{
			if (this.mAutoInitTracker)
			{
				bool flag = false;
				VuforiaAbstractBehaviour vuforiaAbstractBehaviour = UnityEngine.Object.FindObjectOfType<VuforiaAbstractBehaviour>();
				if (VuforiaARController.Instance.HasStarted && vuforiaAbstractBehaviour != null)
				{
					vuforiaAbstractBehaviour.enabled = false;
					flag = true;
				}
				this.InitDeviceTracker();
				this.ApplySettings();
				if (flag)
				{
					vuforiaAbstractBehaviour.enabled = true;
				}
			}
		}

		internal void OnVuforiaStarted()
		{
			if (this.mAutoStartTracker)
			{
				this.StartDeviceTracker();
			}
		}

		internal void OnPause(bool pause)
		{
			DeviceTracker tracker = TrackerManager.Instance.GetTracker<DeviceTracker>();
			if (tracker != null)
			{
				if (pause)
				{
					this.mTrackerWasActiveBeforePause = tracker.IsActive;
					if (tracker.IsActive)
					{
						this.StopDeviceTracker();
						return;
					}
				}
				else if (this.mTrackerWasActiveBeforePause)
				{
					this.StartDeviceTracker();
				}
			}
		}
	}
}
