using System;
using UnityEngine;

namespace Vuforia
{
	internal class TrackerManagerImpl : TrackerManager
	{
		private ObjectTracker mObjectTracker;

		private TextTracker mTextTracker;

		private SmartTerrainTracker mSmartTerrainTracker;

		private DeviceTracker mDeviceTracker;

		private StateManager mStateManager = new StateManagerImpl();

		public override T GetTracker<T>()
		{
			if (typeof(T) == typeof(ObjectTracker))
			{
				return this.mObjectTracker as T;
			}
			if (typeof(T) == typeof(TextTracker))
			{
				return this.mTextTracker as T;
			}
			if (typeof(T) == typeof(SmartTerrainTracker))
			{
				return this.mSmartTerrainTracker as T;
			}
			if (typeof(T) == typeof(DeviceTracker) || typeof(T) == typeof(RotationalDeviceTracker))
			{
				return this.mDeviceTracker as T;
			}
			Debug.LogError("Could not return tracker. Unknown tracker type.");
			return default(T);
		}

		public override T InitTracker<T>()
		{
			if (!VuforiaRuntimeUtilities.IsVuforiaEnabled())
			{
				return default(T);
			}
			bool flag = true;
			if (VuforiaRuntimeUtilities.IsPlayMode() && (typeof(T) == typeof(DeviceTracker) || typeof(T) == typeof(RotationalDeviceTracker)))
			{
				flag = false;
			}
			if (flag && VuforiaWrapper.Instance.TrackerManagerInitTracker((int)TypeMapping.GetTypeID(typeof(T))) == 0)
			{
				Debug.LogError("Could not initialize the tracker.");
				return default(T);
			}
			if (typeof(T) == typeof(ObjectTracker))
			{
				if (this.mObjectTracker == null)
				{
					this.mObjectTracker = new ObjectTrackerImpl();
				}
				return this.mObjectTracker as T;
			}
			if (typeof(T) == typeof(TextTracker))
			{
				if (this.mTextTracker == null)
				{
					this.mTextTracker = new TextTrackerImpl();
				}
				return this.mTextTracker as T;
			}
			if (typeof(T) == typeof(SmartTerrainTracker))
			{
				if (this.mSmartTerrainTracker == null)
				{
					this.mSmartTerrainTracker = new SmartTerrainTrackerImpl();
				}
				return this.mSmartTerrainTracker as T;
			}
			if (typeof(T) == typeof(DeviceTracker) || typeof(T) == typeof(RotationalDeviceTracker))
			{
				if (this.mDeviceTracker == null)
				{
					if (VuforiaRuntimeUtilities.IsPlayMode())
					{
						this.mDeviceTracker = new RotationalPlayModeDeviceTrackerImpl();
					}
					else
					{
						this.mDeviceTracker = new RotationalDeviceTrackerImpl();
					}
				}
				return this.mDeviceTracker as T;
			}
			Debug.LogError("Could not initialize tracker. Unknown tracker type.");
			return default(T);
		}

		public override bool DeinitTracker<T>()
		{
			bool flag = true;
			if (VuforiaRuntimeUtilities.IsPlayMode() && (typeof(T) == typeof(DeviceTracker) || typeof(T) == typeof(RotationalDeviceTracker)))
			{
				flag = false;
			}
			if (flag && VuforiaWrapper.Instance.TrackerManagerDeinitTracker((int)TypeMapping.GetTypeID(typeof(T))) == 0)
			{
				Debug.LogError("Could not deinitialize the tracker.");
				return false;
			}
			if (typeof(T) == typeof(ObjectTracker))
			{
				this.mObjectTracker = null;
			}
			else if (typeof(T) == typeof(TextTracker))
			{
				this.mTextTracker = null;
			}
			else if (typeof(T) == typeof(SmartTerrainTracker))
			{
				this.mSmartTerrainTracker = null;
			}
			else
			{
				if (typeof(T) != typeof(DeviceTracker) && typeof(T) != typeof(RotationalDeviceTracker))
				{
					Debug.LogError("Could not deinitialize tracker. Unknown tracker type.");
					return false;
				}
				this.mDeviceTracker = null;
			}
			return true;
		}

		public override StateManager GetStateManager()
		{
			return this.mStateManager;
		}
	}
}
