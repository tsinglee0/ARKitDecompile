using System;
using UnityEngine;

namespace Vuforia
{
	public abstract class VuforiaManager
	{
		public struct TrackableIdPair
		{
			public int TrackableId;

			public int ResultId;

			internal static VuforiaManager.TrackableIdPair FromTrackableId(int trackableId)
			{
				return new VuforiaManager.TrackableIdPair
				{
					TrackableId = trackableId,
					ResultId = -1
				};
			}

			internal static VuforiaManager.TrackableIdPair FromResultId(int resultId)
			{
				return new VuforiaManager.TrackableIdPair
				{
					TrackableId = -1,
					ResultId = resultId
				};
			}

			internal static VuforiaManager.TrackableIdPair FromTrackableAndResultId(int trackableId, int resultId)
			{
				return new VuforiaManager.TrackableIdPair
				{
					TrackableId = trackableId,
					ResultId = resultId
				};
			}
		}

		private static VuforiaManager sInstance;

		public static VuforiaManager Instance
		{
			get
			{
				if (VuforiaManager.sInstance == null)
				{
					Type typeFromHandle = typeof(VuforiaManager);
					lock (typeFromHandle)
					{
						if (VuforiaManager.sInstance == null)
						{
							VuforiaManager.sInstance = new VuforiaManagerImpl();
						}
					}
				}
				return VuforiaManager.sInstance;
			}
		}

		public abstract VuforiaARController.WorldCenterMode WorldCenterMode
		{
			get;
			set;
		}

		public abstract WorldCenterTrackableBehaviour WorldCenter
		{
			get;
			set;
		}

		public abstract VuMarkAbstractBehaviour VuMarkWorldCenter
		{
			get;
			set;
		}

		public abstract Transform ARCameraTransform
		{
			get;
			set;
		}

		public abstract Transform CentralAnchorPoint
		{
			get;
			set;
		}

		public abstract Transform ParentAnchorPoint
		{
			get;
			set;
		}

		public abstract bool Initialized
		{
			get;
		}

		public abstract int CurrentFrameIndex
		{
			get;
		}

		public abstract bool Init();

		public abstract void Deinit();
	}
}
