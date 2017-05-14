using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
	public abstract class TextRecoAbstractBehaviour : MonoBehaviour
	{
		private bool mHasInitialized;

		private bool mTrackerWasActiveBeforePause;

		private bool mTrackerWasActiveBeforeDisabling;

		[HideInInspector, SerializeField]
		private string mWordListFile;

		[HideInInspector, SerializeField]
		private string mCustomWordListFile;

		[HideInInspector, SerializeField]
		private string mAdditionalCustomWords;

		[HideInInspector, SerializeField]
		private WordFilterMode mFilterMode;

		[HideInInspector, SerializeField]
		private string mFilterListFile;

		[HideInInspector, SerializeField]
		private string mAdditionalFilterWords;

		[HideInInspector, SerializeField]
		private WordPrefabCreationMode mWordPrefabCreationMode;

		[HideInInspector, SerializeField]
		private int mMaximumWordInstances;

		private List<ITextRecoEventHandler> mTextRecoEventHandlers = new List<ITextRecoEventHandler>();

		public bool IsInitialized
		{
			get
			{
				return this.mHasInitialized;
			}
		}

		private void Awake()
		{
			if (!VuforiaRuntimeUtilities.IsVuforiaEnabled())
			{
				return;
			}
			VuforiaARController instance = VuforiaARController.Instance;
			if (instance != null)
			{
				instance.RegisterVuforiaInitializedCallback(new Action(this.OnVuforiaInitialized));
				instance.RegisterVuforiaStartedCallback(new Action(this.OnVuforiaStarted));
				instance.RegisterTrackablesUpdatedCallback(new Action(this.OnTrackablesUpdated));
				instance.RegisterOnPauseCallback(new Action<bool>(this.OnPause));
			}
		}

		private void OnEnable()
		{
			if (this.mTrackerWasActiveBeforeDisabling)
			{
				this.StartTextTracker();
			}
		}

		private void OnDisable()
		{
			TextTracker tracker = TrackerManager.Instance.GetTracker<TextTracker>();
			if (tracker != null)
			{
				this.mTrackerWasActiveBeforeDisabling = tracker.IsActive;
				if (tracker.IsActive)
				{
					this.StopTextTracker();
				}
			}
			WordManagerImpl wordManagerImpl = (WordManagerImpl)TrackerManager.Instance.GetStateManager().GetWordManager();
			if (wordManagerImpl != null)
			{
				wordManagerImpl.CleanupWordBehaviours();
			}
		}

		private void OnDestroy()
		{
			VuforiaARController expr_05 = VuforiaARController.Instance;
			expr_05.UnregisterVuforiaInitializedCallback(new Action(this.OnVuforiaInitialized));
			expr_05.UnregisterVuforiaStartedCallback(new Action(this.OnVuforiaStarted));
			expr_05.UnregisterTrackablesUpdatedCallback(new Action(this.OnTrackablesUpdated));
			expr_05.UnregisterOnPauseCallback(new Action<bool>(this.OnPause));
			TextTracker tracker = TrackerManager.Instance.GetTracker<TextTracker>();
			if (tracker != null)
			{
				tracker.WordList.UnloadAllLists();
			}
		}

		public void RegisterTextRecoEventHandler(ITextRecoEventHandler trackableEventHandler)
		{
			this.mTextRecoEventHandlers.Add(trackableEventHandler);
			if (this.mHasInitialized)
			{
				trackableEventHandler.OnInitialized();
			}
		}

		public bool UnregisterTextRecoEventHandler(ITextRecoEventHandler trackableEventHandler)
		{
			return this.mTextRecoEventHandlers.Remove(trackableEventHandler);
		}

		private void StartTextTracker()
		{
			Debug.Log("Starting Text Tracker");
			TextTracker tracker = TrackerManager.Instance.GetTracker<TextTracker>();
			if (tracker != null)
			{
				tracker.Start();
			}
		}

		private void StopTextTracker()
		{
			Debug.Log("Stopping Text Tracker");
			TextTracker tracker = TrackerManager.Instance.GetTracker<TextTracker>();
			if (tracker != null)
			{
				tracker.Stop();
			}
		}

		private void SetupWordList()
		{
			TextTracker tracker = TrackerManager.Instance.GetTracker<TextTracker>();
			if (tracker != null)
			{
				WordList wordList = tracker.WordList;
				wordList.LoadWordListFile(this.mWordListFile);
				if (this.mCustomWordListFile != "")
				{
					wordList.AddWordsFromFile(this.mCustomWordListFile);
				}
				if (this.mAdditionalCustomWords != null)
				{
					string[] array = this.mAdditionalCustomWords.Split(new char[]
					{
						'\r',
						'\n'
					});
					for (int i = 0; i < array.Length; i++)
					{
						string text = array[i];
						if (text.Length > 0)
						{
							wordList.AddWord(text);
						}
					}
				}
				wordList.SetFilterMode(this.mFilterMode);
				if (this.mFilterMode != WordFilterMode.NONE)
				{
					if (this.mFilterListFile != "")
					{
						wordList.LoadFilterListFile(this.mFilterListFile);
					}
					if (this.mAdditionalFilterWords != null)
					{
						string[] array = this.mAdditionalFilterWords.Split(new char[]
						{
							'\n'
						});
						for (int i = 0; i < array.Length; i++)
						{
							string text2 = array[i];
							if (text2.Length > 0)
							{
								wordList.AddWordToFilterList(text2);
							}
						}
					}
				}
			}
		}

		private void NotifyEventHandlersOfChanges(IEnumerable<Word> lostWords, IEnumerable<WordResult> newWords)
		{
			foreach (Word current in lostWords)
			{
				using (List<ITextRecoEventHandler>.Enumerator enumerator2 = this.mTextRecoEventHandlers.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.Current.OnWordLost(current);
					}
				}
			}
			foreach (WordResult current2 in newWords)
			{
				using (List<ITextRecoEventHandler>.Enumerator enumerator2 = this.mTextRecoEventHandlers.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.Current.OnWordDetected(current2);
					}
				}
			}
		}

		internal void OnVuforiaInitialized()
		{
			bool flag = false;
			VuforiaAbstractBehaviour vuforiaAbstractBehaviour = UnityEngine.Object.FindObjectOfType<VuforiaAbstractBehaviour>();
			if (VuforiaARController.Instance.HasStarted && vuforiaAbstractBehaviour != null)
			{
				vuforiaAbstractBehaviour.enabled = false;
				flag = true;
			}
			if (TrackerManager.Instance.GetTracker<TextTracker>() == null)
			{
				TrackerManager.Instance.InitTracker<TextTracker>();
			}
			if (flag)
			{
				vuforiaAbstractBehaviour.enabled = true;
			}
		}

		internal void OnVuforiaStarted()
		{
			this.SetupWordList();
			this.StartTextTracker();
			this.mHasInitialized = true;
			((WordManagerImpl)TrackerManager.Instance.GetStateManager().GetWordManager()).InitializeWordBehaviourTemplates(this.mWordPrefabCreationMode, this.mMaximumWordInstances);
			using (List<ITextRecoEventHandler>.Enumerator enumerator = this.mTextRecoEventHandlers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnInitialized();
				}
			}
		}

		internal void OnTrackablesUpdated()
		{
			WordManagerImpl expr_14 = (WordManagerImpl)TrackerManager.Instance.GetStateManager().GetWordManager();
			IEnumerable<WordResult> newWords = expr_14.GetNewWords();
			IEnumerable<Word> lostWords = expr_14.GetLostWords();
			this.NotifyEventHandlersOfChanges(lostWords, newWords);
		}

		internal void OnPause(bool pause)
		{
			TextTracker tracker = TrackerManager.Instance.GetTracker<TextTracker>();
			if (tracker != null)
			{
				if (pause)
				{
					this.mTrackerWasActiveBeforePause = tracker.IsActive;
					if (tracker.IsActive)
					{
						this.StopTextTracker();
						return;
					}
				}
				else if (this.mTrackerWasActiveBeforePause)
				{
					this.StartTextTracker();
				}
			}
		}
	}
}
