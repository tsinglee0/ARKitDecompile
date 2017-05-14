using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
	public abstract class CloudRecoAbstractBehaviour : MonoBehaviour
	{
		private ObjectTracker mObjectTracker;

		private bool mCurrentlyInitializing;

		private bool mInitSuccess;

		private bool mCloudRecoStarted;

		private bool mOnInitializedCalled;

		private readonly List<ICloudRecoEventHandler> mHandlers = new List<ICloudRecoEventHandler>();

		private bool mTargetFinderStartedBeforeDisable = true;

		public string AccessKey = "";

		public string SecretKey = "";

		public bool CloudRecoEnabled
		{
			get
			{
				return this.mCloudRecoStarted;
			}
			set
			{
				if (value)
				{
					this.StartCloudReco();
					return;
				}
				this.StopCloudReco();
			}
		}

		public bool CloudRecoInitialized
		{
			get
			{
				return this.mInitSuccess;
			}
		}

		private void Initialize()
		{
			this.mCurrentlyInitializing = this.mObjectTracker.TargetFinder.StartInit(this.AccessKey, this.SecretKey);
			if (!this.mCurrentlyInitializing)
			{
				Debug.LogError("CloudRecoBehaviour: TargetFinder initialization failed!");
			}
		}

		private void Deinitialize()
		{
			this.mCurrentlyInitializing = !this.mObjectTracker.TargetFinder.Deinit();
			if (this.mCurrentlyInitializing)
			{
				Debug.LogError("CloudRecoBehaviour: TargetFinder deinitialization failed!");
				return;
			}
			this.mInitSuccess = false;
		}

		private void CheckInitialization()
		{
			TargetFinder.InitState initState = this.mObjectTracker.TargetFinder.GetInitState();
			if (initState == TargetFinder.InitState.INIT_SUCCESS)
			{
				using (List<ICloudRecoEventHandler>.Enumerator enumerator = this.mHandlers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.OnInitialized();
					}
				}
				this.mCurrentlyInitializing = false;
				this.mInitSuccess = true;
				this.StartCloudReco();
				return;
			}
			if (initState < TargetFinder.InitState.INIT_DEFAULT)
			{
				using (List<ICloudRecoEventHandler>.Enumerator enumerator = this.mHandlers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.OnInitError(initState);
					}
				}
				this.mCurrentlyInitializing = false;
			}
		}

		private void StartCloudReco()
		{
			if (this.mObjectTracker != null && !this.mCloudRecoStarted)
			{
				this.mCloudRecoStarted = this.mObjectTracker.TargetFinder.StartRecognition();
				using (List<ICloudRecoEventHandler>.Enumerator enumerator = this.mHandlers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.OnStateChanged(true);
					}
				}
			}
		}

		private void StopCloudReco()
		{
			if (this.mCloudRecoStarted)
			{
				this.mCloudRecoStarted = !this.mObjectTracker.TargetFinder.Stop();
				if (this.mCloudRecoStarted)
				{
					Debug.LogError("Cloud Reco could not be stopped at this point!");
					return;
				}
				using (List<ICloudRecoEventHandler>.Enumerator enumerator = this.mHandlers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.OnStateChanged(false);
					}
				}
			}
		}

		public void RegisterEventHandler(ICloudRecoEventHandler eventHandler)
		{
			this.mHandlers.Add(eventHandler);
			if (this.mOnInitializedCalled)
			{
				eventHandler.OnInitialized();
			}
		}

		public bool UnregisterEventHandler(ICloudRecoEventHandler eventHandler)
		{
			return this.mHandlers.Remove(eventHandler);
		}

		private void OnEnable()
		{
			if (this.mOnInitializedCalled && this.mTargetFinderStartedBeforeDisable)
			{
				this.StartCloudReco();
			}
		}

		private void OnDisable()
		{
			if (VuforiaManager.Instance.Initialized && this.mOnInitializedCalled)
			{
				this.mTargetFinderStartedBeforeDisable = this.mCloudRecoStarted;
				this.StopCloudReco();
			}
		}

		private void Start()
		{
			VuforiaARController.Instance.RegisterVuforiaStartedCallback(new Action(this.OnVuforiaStarted));
		}

		private void Update()
		{
			if (this.mOnInitializedCalled)
			{
				if (this.mCurrentlyInitializing)
				{
					this.CheckInitialization();
					return;
				}
				if (this.mInitSuccess)
				{
					TargetFinder.UpdateState updateState = this.mObjectTracker.TargetFinder.Update();
					if (updateState == TargetFinder.UpdateState.UPDATE_RESULTS_AVAILABLE)
					{
						using (IEnumerator<TargetFinder.TargetSearchResult> enumerator = this.mObjectTracker.TargetFinder.GetResults().GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								TargetFinder.TargetSearchResult current = enumerator.Current;
								using (List<ICloudRecoEventHandler>.Enumerator enumerator2 = this.mHandlers.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										enumerator2.Current.OnNewSearchResult(current);
									}
								}
							}
							return;
						}
					}
					if (updateState < TargetFinder.UpdateState.UPDATE_NO_MATCH)
					{
						using (List<ICloudRecoEventHandler>.Enumerator enumerator2 = this.mHandlers.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								enumerator2.Current.OnUpdateError(updateState);
							}
						}
					}
				}
			}
		}

		private void OnDestroy()
		{
			if (VuforiaManager.Instance.Initialized && this.mOnInitializedCalled)
			{
				this.Deinitialize();
			}
			VuforiaARController.Instance.UnregisterVuforiaStartedCallback(new Action(this.OnVuforiaStarted));
		}

		private void OnValidate()
		{
			this.AccessKey = this.AccessKey.Trim();
			this.SecretKey = this.SecretKey.Trim();
		}

		internal void OnVuforiaStarted()
		{
			this.mObjectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
			if (this.mObjectTracker != null)
			{
				this.Initialize();
			}
			this.mOnInitializedCalled = true;
		}
	}
}
