using System;
using UnityEngine;

namespace Vuforia
{
	public class SmartTerrainTrackerARController : ARController
	{
		private bool mAutoInitTracker;

		private bool mAutoStartTracker;

		private bool mAutoInitBuilder;

		private float mSceneUnitsToMillimeter;

		private Action mTrackerStarted;

		private bool mTrackerWasActiveBeforePause;

		private bool mTrackerWasActiveBeforeDisabling;

		private static SmartTerrainTrackerARController mInstance;

		private static object mPadlock = new object();

		public static SmartTerrainTrackerARController Instance
		{
			get
			{
				if (SmartTerrainTrackerARController.mInstance == null)
				{
					object obj = SmartTerrainTrackerARController.mPadlock;
					lock (obj)
					{
						if (SmartTerrainTrackerARController.mInstance == null)
						{
							SmartTerrainTrackerARController.mInstance = new SmartTerrainTrackerARController();
						}
					}
				}
				return SmartTerrainTrackerARController.mInstance;
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

		internal bool AutoInitBuilder
		{
			get
			{
				return this.mAutoInitBuilder;
			}
		}

		internal float SceneUnitsToMillimeter
		{
			get
			{
				return this.mSceneUnitsToMillimeter;
			}
		}

		private SmartTerrainTrackerARController()
		{
		}

		[RuntimeInitializeOnLoadMethod]
		public static void RegisterARController()
		{
			ARController.Register(SmartTerrainTrackerARController.Instance);
		}

		protected override void Awake()
		{
			if (!VuforiaRuntimeUtilities.IsVuforiaEnabled())
			{
				return;
			}
			VuforiaAbstractConfiguration.SmartTerrainTrackerConfiguration smartTerrainTracker = VuforiaAbstractConfiguration.Instance.SmartTerrainTracker;
			this.mAutoInitTracker = smartTerrainTracker.AutoInitAndStartTracker;
			this.mAutoStartTracker = smartTerrainTracker.AutoInitAndStartTracker;
			this.mAutoInitBuilder = smartTerrainTracker.AutoInitBuilder;
			this.mSceneUnitsToMillimeter = smartTerrainTracker.SceneUnitsToMillimeter;
			VuforiaARController expr_48 = VuforiaARController.Instance;
			expr_48.RegisterVuforiaInitializedCallback(new Action(this.OnVuforiaInitialized));
			expr_48.RegisterVuforiaStartedCallback(new Action(this.OnVuforiaStarted));
			expr_48.RegisterOnPauseCallback(new Action<bool>(this.OnPause));
		}

		protected override void OnEnable()
		{
			if (this.mTrackerWasActiveBeforeDisabling)
			{
				this.StartSmartTerrainTracker();
			}
		}

		protected override void OnDisable()
		{
			SmartTerrainTracker tracker = TrackerManager.Instance.GetTracker<SmartTerrainTracker>();
			if (tracker != null)
			{
				this.mTrackerWasActiveBeforeDisabling = tracker.IsActive;
				if (tracker.IsActive)
				{
					this.StopSmartTerrainTracker();
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

		public void RegisterTrackerStartedCallback(Action callback)
		{
			this.mTrackerStarted = (Action)Delegate.Combine(this.mTrackerStarted, callback);
			SmartTerrainTracker tracker = TrackerManager.Instance.GetTracker<SmartTerrainTracker>();
			if (tracker != null && tracker.IsActive)
			{
				callback();
			}
		}

		public void UnregisterTrackerStartedCallback(Action callback)
		{
			this.mTrackerStarted = (Action)Delegate.Remove(this.mTrackerStarted, callback);
		}

		private void StartSmartTerrainTracker()
		{
			Debug.Log("Starting Smart Terrain Tracker");
			SmartTerrainTracker tracker = TrackerManager.Instance.GetTracker<SmartTerrainTracker>();
			if (tracker != null)
			{
				tracker.Start();
				if (this.mTrackerStarted != null)
				{
					this.mTrackerStarted.InvokeWithExceptionHandling();
				}
			}
		}

		private void StopSmartTerrainTracker()
		{
			Debug.Log("Stopping Smart Terrain Tracker");
			SmartTerrainTracker tracker = TrackerManager.Instance.GetTracker<SmartTerrainTracker>();
			if (tracker != null)
			{
				tracker.Stop();
			}
		}

		private void InitSmartTerrainTracker()
		{
			if (TrackerManager.Instance.GetTracker<SmartTerrainTracker>() == null)
			{
				SmartTerrainTracker smartTerrainTracker = TrackerManager.Instance.InitTracker<SmartTerrainTracker>();
				smartTerrainTracker.SetScaleToMillimeter(this.mSceneUnitsToMillimeter);
				if (this.mAutoInitBuilder)
				{
					smartTerrainTracker.SmartTerrainBuilder.Init();
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
				this.InitSmartTerrainTracker();
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
				this.StartSmartTerrainTracker();
			}
		}

		internal void OnPause(bool pause)
		{
			SmartTerrainTracker tracker = TrackerManager.Instance.GetTracker<SmartTerrainTracker>();
			if (tracker != null)
			{
				if (pause)
				{
					this.mTrackerWasActiveBeforePause = tracker.IsActive;
					if (tracker.IsActive)
					{
						this.StopSmartTerrainTracker();
						return;
					}
				}
				else if (this.mTrackerWasActiveBeforePause)
				{
					this.StartSmartTerrainTracker();
				}
			}
		}
	}
}
