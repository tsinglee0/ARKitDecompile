using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
	public abstract class UserDefinedTargetBuildingAbstractBehaviour : MonoBehaviour
	{
		private ObjectTracker mObjectTracker;

		private ImageTargetBuilder.FrameQuality mLastFrameQuality = ImageTargetBuilder.FrameQuality.FRAME_QUALITY_NONE;

		private bool mCurrentlyScanning;

		private bool mWasScanningBeforeDisable;

		private bool mCurrentlyBuilding;

		private bool mWasBuildingBeforeDisable;

		private bool mOnInitializedCalled;

		private readonly List<IUserDefinedTargetEventHandler> mHandlers = new List<IUserDefinedTargetEventHandler>();

		public bool StopTrackerWhileScanning;

		public bool StartScanningAutomatically;

		public bool StopScanningWhenFinshedBuilding;

		public void RegisterEventHandler(IUserDefinedTargetEventHandler eventHandler)
		{
			this.mHandlers.Add(eventHandler);
			if (this.mOnInitializedCalled)
			{
				eventHandler.OnInitialized();
			}
		}

		public bool UnregisterEventHandler(IUserDefinedTargetEventHandler eventHandler)
		{
			return this.mHandlers.Remove(eventHandler);
		}

		public void StartScanning()
		{
			if (this.mObjectTracker != null)
			{
				if (this.StopTrackerWhileScanning)
				{
					this.mObjectTracker.Stop();
				}
				this.mObjectTracker.ImageTargetBuilder.StartScan();
				this.mCurrentlyScanning = true;
			}
			this.SetFrameQuality(ImageTargetBuilder.FrameQuality.FRAME_QUALITY_LOW);
		}

		public void BuildNewTarget(string targetName, float screenSizeWidth)
		{
			this.mCurrentlyBuilding = true;
			this.mObjectTracker.ImageTargetBuilder.Build(targetName, screenSizeWidth);
		}

		public void StopScanning()
		{
			this.mCurrentlyScanning = false;
			this.mObjectTracker.ImageTargetBuilder.StopScan();
			if (this.StopTrackerWhileScanning)
			{
				this.mObjectTracker.Start();
			}
			this.SetFrameQuality(ImageTargetBuilder.FrameQuality.FRAME_QUALITY_NONE);
		}

		private void SetFrameQuality(ImageTargetBuilder.FrameQuality frameQuality)
		{
			if (frameQuality != this.mLastFrameQuality)
			{
				using (List<IUserDefinedTargetEventHandler>.Enumerator enumerator = this.mHandlers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.OnFrameQualityChanged(frameQuality);
					}
				}
				this.mLastFrameQuality = frameQuality;
			}
		}

		private void Start()
		{
			VuforiaARController expr_05 = VuforiaARController.Instance;
			expr_05.RegisterVuforiaStartedCallback(new Action(this.OnVuforiaStarted));
			expr_05.RegisterOnPauseCallback(new Action<bool>(this.OnPause));
		}

		private void Update()
		{
			if (this.mOnInitializedCalled)
			{
				if (this.mCurrentlyScanning)
				{
					this.SetFrameQuality(this.mObjectTracker.ImageTargetBuilder.GetFrameQuality());
				}
				if (this.mCurrentlyBuilding)
				{
					TrackableSource trackableSource = this.mObjectTracker.ImageTargetBuilder.GetTrackableSource();
					if (trackableSource != null)
					{
						this.mCurrentlyBuilding = false;
						using (List<IUserDefinedTargetEventHandler>.Enumerator enumerator = this.mHandlers.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								enumerator.Current.OnNewTrackableSource(trackableSource);
							}
						}
						if (this.StopScanningWhenFinshedBuilding)
						{
							this.StopScanning();
						}
					}
				}
			}
		}

		private void OnEnable()
		{
			if (this.mOnInitializedCalled)
			{
				this.mCurrentlyScanning = this.mWasScanningBeforeDisable;
				this.mCurrentlyBuilding = this.mWasBuildingBeforeDisable;
				if (this.mWasScanningBeforeDisable)
				{
					this.StartScanning();
				}
			}
		}

		private void OnDisable()
		{
			if (this.mOnInitializedCalled)
			{
				this.mWasScanningBeforeDisable = this.mCurrentlyScanning;
				this.mWasBuildingBeforeDisable = this.mCurrentlyBuilding;
				if (this.mCurrentlyScanning)
				{
					this.StopScanning();
				}
			}
		}

		private void OnDestroy()
		{
			VuforiaARController expr_05 = VuforiaARController.Instance;
			expr_05.UnregisterVuforiaStartedCallback(new Action(this.OnVuforiaStarted));
			expr_05.UnregisterOnPauseCallback(new Action<bool>(this.OnPause));
		}

		internal void OnVuforiaStarted()
		{
			this.mOnInitializedCalled = true;
			this.mObjectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
			using (List<IUserDefinedTargetEventHandler>.Enumerator enumerator = this.mHandlers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnInitialized();
				}
			}
			if (this.StartScanningAutomatically)
			{
				this.StartScanning();
			}
		}

		internal void OnPause(bool pause)
		{
			if (pause)
			{
				this.OnDisable();
				return;
			}
			this.OnEnable();
		}
	}
}
