using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class TargetFinderImpl : TargetFinder
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct TargetFinderState
		{
			public int IsRequesting;

			public int UpdateState;

			public int ResultCount;

			internal int unused;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct InternalTargetSearchResult
		{
			public IntPtr TargetNamePtr;

			public IntPtr UniqueTargetIdPtr;

			public IntPtr MetaDataPtr;

			public IntPtr TargetSearchResultPtr;

			public float TargetSize;

			public int TrackingRating;
		}

		private IntPtr mTargetFinderStatePtr;

		private TargetFinderImpl.TargetFinderState mTargetFinderState;

		private List<TargetFinder.TargetSearchResult> mNewResults;

		private Dictionary<int, ImageTarget> mImageTargets;

		public TargetFinderImpl()
		{
			this.mTargetFinderState = default(TargetFinderImpl.TargetFinderState);
			this.mTargetFinderStatePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TargetFinderImpl.TargetFinderState)));
			Marshal.StructureToPtr(this.mTargetFinderState, this.mTargetFinderStatePtr, false);
			this.mImageTargets = new Dictionary<int, ImageTarget>();
		}

		~TargetFinderImpl()
		{
			Marshal.FreeHGlobal(this.mTargetFinderStatePtr);
			this.mTargetFinderStatePtr = IntPtr.Zero;
		}

		public override bool StartInit(string userAuth, string secretAuth)
		{
			return VuforiaWrapper.Instance.TargetFinderStartInit(userAuth, secretAuth) == 1;
		}

		public override TargetFinder.InitState GetInitState()
		{
			return (TargetFinder.InitState)VuforiaWrapper.Instance.TargetFinderGetInitState();
		}

		public override bool Deinit()
		{
			return VuforiaWrapper.Instance.TargetFinderDeinit() == 1;
		}

		public override bool StartRecognition()
		{
			return VuforiaWrapper.Instance.TargetFinderStartRecognition() == 1;
		}

		public override bool Stop()
		{
			return VuforiaWrapper.Instance.TargetFinderStop() == 1;
		}

		public override bool IsRequesting()
		{
			return this.mTargetFinderState.IsRequesting == 1;
		}

		public override TargetFinder.UpdateState Update()
		{
			return this.Update(TargetFinder.FilterMode.FILTER_CURRENTLY_TRACKED);
		}

		public override TargetFinder.UpdateState Update(TargetFinder.FilterMode filterMode)
		{
			VuforiaWrapper.Instance.TargetFinderUpdate(this.mTargetFinderStatePtr, (int)filterMode);
			this.mTargetFinderState = (TargetFinderImpl.TargetFinderState)Marshal.PtrToStructure(this.mTargetFinderStatePtr, typeof(TargetFinderImpl.TargetFinderState));
			if (this.mTargetFinderState.ResultCount > 0)
			{
				IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TargetFinderImpl.InternalTargetSearchResult)) * this.mTargetFinderState.ResultCount);
				if (VuforiaWrapper.Instance.TargetFinderGetResults(intPtr, this.mTargetFinderState.ResultCount) != 1)
				{
					Debug.LogError("TargetFinder: Could not retrieve new results!");
					return TargetFinder.UpdateState.UPDATE_NO_MATCH;
				}
				this.mNewResults = new List<TargetFinder.TargetSearchResult>();
				for (int i = 0; i < this.mTargetFinderState.ResultCount; i++)
				{
					TargetFinderImpl.InternalTargetSearchResult internalTargetSearchResult = (TargetFinderImpl.InternalTargetSearchResult)Marshal.PtrToStructure(new IntPtr(intPtr.ToInt64() + (long)(i * Marshal.SizeOf(typeof(VuforiaManagerImpl.TrackableResultData)))), typeof(TargetFinderImpl.InternalTargetSearchResult));
					this.mNewResults.Add(new TargetFinder.TargetSearchResult
					{
						TargetName = Marshal.PtrToStringAnsi(internalTargetSearchResult.TargetNamePtr),
						UniqueTargetId = Marshal.PtrToStringAnsi(internalTargetSearchResult.UniqueTargetIdPtr),
						TargetSize = internalTargetSearchResult.TargetSize,
						MetaData = Marshal.PtrToStringAnsi(internalTargetSearchResult.MetaDataPtr),
						TrackingRating = (byte)internalTargetSearchResult.TrackingRating,
						TargetSearchResultPtr = internalTargetSearchResult.TargetSearchResultPtr
					});
				}
				Marshal.FreeHGlobal(intPtr);
			}
			return (TargetFinder.UpdateState)this.mTargetFinderState.UpdateState;
		}

		public override IEnumerable<TargetFinder.TargetSearchResult> GetResults()
		{
			return this.mNewResults;
		}

		public override ImageTargetAbstractBehaviour EnableTracking(TargetFinder.TargetSearchResult result, string gameObjectName)
		{
			GameObject gameObject = new GameObject(gameObjectName);
			return this.EnableTracking(result, gameObject);
		}

		public override ImageTargetAbstractBehaviour EnableTracking(TargetFinder.TargetSearchResult result, GameObject gameObject)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ImageTargetData)));
			int num = VuforiaWrapper.Instance.TargetFinderEnableTracking(result.TargetSearchResultPtr, intPtr);
			ImageTargetData imageTargetData = (ImageTargetData)Marshal.PtrToStructure(intPtr, typeof(ImageTargetData));
			Marshal.FreeHGlobal(intPtr);
			StateManagerImpl stateManagerImpl = (StateManagerImpl)TrackerManager.Instance.GetStateManager();
			ImageTargetAbstractBehaviour result2 = null;
			if (imageTargetData.id == -1)
			{
				Debug.LogError("TargetSearchResult " + result.TargetName + " could not be enabled for tracking.");
			}
			else
			{
				ImageTarget imageTarget = new CloudRecoImageTargetImpl(result.TargetName, imageTargetData.id, imageTargetData.size);
				this.mImageTargets[imageTargetData.id] = imageTarget;
				result2 = stateManagerImpl.FindOrCreateImageTargetBehaviourForTrackable(imageTarget, gameObject);
			}
			IntPtr intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * num);
			VuforiaWrapper.Instance.TargetFinderGetImageTargets(intPtr2, num);
			List<int> list = new List<int>();
			for (int i = 0; i < num; i++)
			{
				int item = Marshal.ReadInt32(new IntPtr(intPtr2.ToInt64() + (long)(i * Marshal.SizeOf(typeof(int)))));
				list.Add(item);
			}
			Marshal.FreeHGlobal(intPtr2);
			ImageTarget[] array = this.mImageTargets.Values.ToArray<ImageTarget>();
			for (int j = 0; j < array.Length; j++)
			{
				ImageTarget imageTarget2 = array[j];
				if (!list.Contains(imageTarget2.ID))
				{
					stateManagerImpl.DestroyTrackableBehavioursForTrackable(imageTarget2, true);
					this.mImageTargets.Remove(imageTarget2.ID);
				}
			}
			return result2;
		}

		public override void ClearTrackables(bool destroyGameObjects = true)
		{
			VuforiaWrapper.Instance.TargetFinderClearTrackables();
			StateManager stateManager = TrackerManager.Instance.GetStateManager();
			foreach (ImageTarget current in this.mImageTargets.Values)
			{
				stateManager.DestroyTrackableBehavioursForTrackable(current, destroyGameObjects);
			}
			this.mImageTargets.Clear();
		}

		public override IEnumerable<ImageTarget> GetImageTargets()
		{
			return this.mImageTargets.Values;
		}
	}
}
