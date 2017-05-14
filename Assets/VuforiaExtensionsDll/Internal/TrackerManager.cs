using System;

namespace Vuforia
{
	public abstract class TrackerManager
	{
		private static TrackerManager mInstance;

		public static TrackerManager Instance
		{
			get
			{
				if (TrackerManager.mInstance == null)
				{
					Type typeFromHandle = typeof(TrackerManager);
					lock (typeFromHandle)
					{
						if (TrackerManager.mInstance == null)
						{
							TrackerManager.mInstance = new TrackerManagerImpl();
						}
					}
				}
				return TrackerManager.mInstance;
			}
		}

		public abstract T GetTracker<T>() where T : Tracker;

		public abstract T InitTracker<T>() where T : Tracker;

		public abstract bool DeinitTracker<T>() where T : Tracker;

		public abstract StateManager GetStateManager();
	}
}
