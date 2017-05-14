using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class WordManagerImpl : WordManager
	{
		private readonly Dictionary<int, WordResult> mTrackedWords = new Dictionary<int, WordResult>();

		private readonly List<WordResult> mNewWords = new List<WordResult>();

		private readonly List<Word> mLostWords = new List<Word>();

		private readonly Dictionary<int, WordAbstractBehaviour> mActiveWordBehaviours = new Dictionary<int, WordAbstractBehaviour>();

		private readonly List<WordAbstractBehaviour> mWordBehavioursMarkedForDeletion = new List<WordAbstractBehaviour>();

		private readonly List<Word> mWaitingQueue = new List<Word>();

		private const string TEMPLATE_IDENTIFIER = "Template_ID";

		private readonly Dictionary<string, List<WordAbstractBehaviour>> mWordBehaviours = new Dictionary<string, List<WordAbstractBehaviour>>();

		private bool mAutomaticTemplate;

		private int mMaxInstances = 1;

		private WordPrefabCreationMode mWordPrefabCreationMode;

		private VuforiaARController mVuforiaBehaviour;

		public override IEnumerable<WordResult> GetActiveWordResults()
		{
			return this.mTrackedWords.Values;
		}

		public override IEnumerable<WordResult> GetNewWords()
		{
			return this.mNewWords;
		}

		public override IEnumerable<Word> GetLostWords()
		{
			return this.mLostWords;
		}

		public override bool TryGetWordBehaviour(Word word, out WordAbstractBehaviour behaviour)
		{
			return this.mActiveWordBehaviours.TryGetValue(word.ID, out behaviour);
		}

		public override IEnumerable<WordAbstractBehaviour> GetTrackableBehaviours()
		{
			List<WordAbstractBehaviour> list = new List<WordAbstractBehaviour>();
			foreach (List<WordAbstractBehaviour> current in this.mWordBehaviours.Values)
			{
				list.AddRange(current);
			}
			return list;
		}

		public override void DestroyWordBehaviour(WordAbstractBehaviour behaviour, bool destroyGameObject = true)
		{
			string[] array = this.mWordBehaviours.Keys.ToArray<string>();
			for (int i = 0; i < array.Length; i++)
			{
				string key = array[i];
				if (this.mWordBehaviours[key].Contains(behaviour))
				{
					this.mWordBehaviours[key].Remove(behaviour);
					if (this.mWordBehaviours[key].Count == 0)
					{
						this.mWordBehaviours.Remove(key);
					}
					if (destroyGameObject)
					{
                        UnityEngine.Object.Destroy(behaviour.gameObject);
						this.mWordBehavioursMarkedForDeletion.Add(behaviour);
					}
					else
					{
						behaviour.UnregisterTrackable();
					}
				}
			}
		}

		internal void InitializeWordBehaviourTemplates(WordPrefabCreationMode wordPrefabCreationMode, int maxInstances)
		{
			this.mWordPrefabCreationMode = wordPrefabCreationMode;
			this.mMaxInstances = maxInstances;
			this.InitializeWordBehaviourTemplates();
		}

		internal void InitializeWordBehaviourTemplates()
		{
			if (this.mWordPrefabCreationMode == WordPrefabCreationMode.DUPLICATE)
			{
				List<WordAbstractBehaviour> list = this.mWordBehavioursMarkedForDeletion.ToList<WordAbstractBehaviour>();
				if (this.mAutomaticTemplate && this.mWordBehaviours.ContainsKey("Template_ID"))
				{
					foreach (WordAbstractBehaviour current in this.mWordBehaviours["Template_ID"])
					{
						list.Add(current);
                        UnityEngine.Object.Destroy(current.gameObject);
					}
					this.mWordBehaviours.Remove("Template_ID");
				}
				WordAbstractBehaviour[] array = (WordAbstractBehaviour[])UnityEngine.Object.FindObjectsOfType(typeof(WordAbstractBehaviour));
				for (int i = 0; i < array.Length; i++)
				{
					WordAbstractBehaviour wordAbstractBehaviour = array[i];
					if (!list.Contains(wordAbstractBehaviour))
					{
						string text = wordAbstractBehaviour.IsTemplateMode ? "Template_ID" : wordAbstractBehaviour.SpecificWord.ToLowerInvariant();
						if (!this.mWordBehaviours.ContainsKey(text))
						{
							this.mWordBehaviours[text] = new List<WordAbstractBehaviour>
							{
								wordAbstractBehaviour
							};
							if (text == "Template_ID")
							{
								this.mAutomaticTemplate = false;
							}
						}
					}
				}
				if (!this.mWordBehaviours.ContainsKey("Template_ID"))
				{
					WordAbstractBehaviour item = WordManagerImpl.CreateWordBehaviour();
					this.mWordBehaviours.Add("Template_ID", new List<WordAbstractBehaviour>
					{
						item
					});
					this.mAutomaticTemplate = true;
				}
			}
			this.mWordBehavioursMarkedForDeletion.Clear();
		}

		internal void RemoveDestroyedTrackables()
		{
			foreach (List<WordAbstractBehaviour> current in this.mWordBehaviours.Values)
			{
				for (int i = current.Count - 1; i >= 0; i--)
				{
					if (current[i] == null)
					{
						current.RemoveAt(i);
					}
				}
			}
			string[] array = this.mWordBehaviours.Keys.ToArray<string>();
			for (int j = 0; j < array.Length; j++)
			{
				string key = array[j];
				if (this.mWordBehaviours[key].Count == 0)
				{
					this.mWordBehaviours.Remove(key);
				}
			}
			int[] array2 = this.mActiveWordBehaviours.Keys.ToArray<int>();
			for (int j = 0; j < array2.Length; j++)
			{
				int key2 = array2[j];
				if (this.mActiveWordBehaviours[key2] == null)
				{
					this.mActiveWordBehaviours.Remove(key2);
				}
			}
		}

		internal void UpdateWords(Transform arCameraTransform, VuforiaManagerImpl.WordData[] newWordData, VuforiaManagerImpl.WordResultData[] wordResults)
		{
			this.UpdateWords(newWordData, wordResults);
			this.UpdateWordResultPoses(arCameraTransform, wordResults);
		}

		internal void SetWordBehavioursToNotFound()
		{
			using (Dictionary<int, WordAbstractBehaviour>.ValueCollection.Enumerator enumerator = this.mActiveWordBehaviours.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnTrackerUpdate(TrackableBehaviour.Status.NOT_FOUND);
				}
			}
		}

		internal void CleanupWordBehaviours()
		{
			this.mNewWords.Clear();
			this.mLostWords.Clear();
			this.mWaitingQueue.Clear();
			this.mTrackedWords.Clear();
			this.mActiveWordBehaviours.Clear();
			this.mWordBehaviours.Clear();
		}

		private void UpdateWords(IEnumerable<VuforiaManagerImpl.WordData> newWordData, IEnumerable<VuforiaManagerImpl.WordResultData> wordResults)
		{
			this.mNewWords.Clear();
			this.mLostWords.Clear();
			foreach (VuforiaManagerImpl.WordData current in newWordData)
			{
				if (!this.mTrackedWords.ContainsKey(current.id))
				{
					WordResultImpl wordResultImpl = new WordResultImpl(new WordImpl(current.id, Marshal.PtrToStringUni(current.stringValue), current.size));
					this.mTrackedWords.Add(current.id, wordResultImpl);
					this.mNewWords.Add(wordResultImpl);
				}
			}
			List<int> list = new List<int>();
			foreach (VuforiaManagerImpl.WordResultData current2 in wordResults)
			{
				list.Add(current2.id);
			}
			foreach (int current3 in this.mTrackedWords.Keys.ToList<int>())
			{
				if (!list.Contains(current3))
				{
					this.mLostWords.Add(this.mTrackedWords[current3].Word);
					this.mTrackedWords.Remove(current3);
				}
			}
			if (this.mWordPrefabCreationMode == WordPrefabCreationMode.DUPLICATE)
			{
				this.UnregisterLostWords();
				this.AssociateWordResultsWithBehaviours();
			}
		}

		private void UpdateWordResultPoses(Transform arCameraTransform, IEnumerable<VuforiaManagerImpl.WordResultData> wordResults)
		{
			if (this.mVuforiaBehaviour == null)
			{
				this.mVuforiaBehaviour = VuforiaARController.Instance;
			}
			Rect videoBackgroundRectInViewPort = this.mVuforiaBehaviour.GetVideoBackgroundRectInViewPort();
			bool isTextureMirrored = this.mVuforiaBehaviour.VideoBackGroundMirrored == VuforiaRenderer.VideoBackgroundReflection.ON;
			CameraDevice.VideoModeData videoMode = CameraDevice.Instance.GetVideoMode();
			foreach (VuforiaManagerImpl.WordResultData current in wordResults)
			{
				WordResultImpl arg_A9_0 = (WordResultImpl)this.mTrackedWords[current.id];
				Vector3 position = arCameraTransform.TransformPoint(current.pose.position);
				Quaternion orientation = current.pose.orientation;
				Quaternion orientation2 = arCameraTransform.rotation * orientation * Quaternion.AngleAxis(270f, Vector3.left);
				arg_A9_0.SetPose(position, orientation2);
				arg_A9_0.SetStatus(current.status);
				OrientedBoundingBox cameraFrameObb = new OrientedBoundingBox(current.orientedBoundingBox.center, current.orientedBoundingBox.halfExtents, current.orientedBoundingBox.rotation);
				arg_A9_0.SetObb(VuforiaRuntimeUtilities.CameraFrameToScreenSpaceCoordinates(cameraFrameObb, videoBackgroundRectInViewPort, isTextureMirrored, videoMode));
			}
			if (this.mWordPrefabCreationMode == WordPrefabCreationMode.DUPLICATE)
			{
				this.UpdateWordBehaviourPoses();
			}
		}

		private void AssociateWordResultsWithBehaviours()
		{
			foreach (Word current in new List<Word>(this.mWaitingQueue))
			{
				if (this.mTrackedWords.ContainsKey(current.ID))
				{
					WordResult wordResult = this.mTrackedWords[current.ID];
					if (this.AssociateWordBehaviour(wordResult) != null)
					{
						this.mWaitingQueue.Remove(current);
					}
				}
				else
				{
					this.mWaitingQueue.Remove(current);
				}
			}
			foreach (WordResult current2 in this.mNewWords)
			{
				if (this.AssociateWordBehaviour(current2) == null)
				{
					this.mWaitingQueue.Add(current2.Word);
				}
			}
		}

		private void UnregisterLostWords()
		{
			foreach (Word current in this.mLostWords)
			{
				if (this.mActiveWordBehaviours.ContainsKey(current.ID))
				{
					WordAbstractBehaviour expr_3A = this.mActiveWordBehaviours[current.ID];
					expr_3A.OnTrackerUpdate(TrackableBehaviour.Status.NOT_FOUND);
					expr_3A.UnregisterTrackable();
					this.mActiveWordBehaviours.Remove(current.ID);
				}
			}
		}

		private void UpdateWordBehaviourPoses()
		{
			foreach (KeyValuePair<int, WordAbstractBehaviour> current in this.mActiveWordBehaviours)
			{
				if (this.mTrackedWords.ContainsKey(current.Key))
				{
					WordResult wordResult = this.mTrackedWords[current.Key];
					Vector3 position = wordResult.Position;
					Quaternion orientation = wordResult.Orientation;
					Vector2 size = wordResult.Word.Size;
					current.Value.transform.rotation = orientation;
					Vector3 vector = current.Value.transform.rotation * new Vector3(-size.x * 0.5f, 0f, -size.y * 0.5f);
					current.Value.transform.position = position + vector;
					current.Value.OnTrackerUpdate(wordResult.CurrentStatus);
				}
			}
		}

		private WordAbstractBehaviour AssociateWordBehaviour(WordResult wordResult)
		{
			string text = wordResult.Word.StringValue.ToLowerInvariant();
			List<WordAbstractBehaviour> list;
			if (this.mWordBehaviours.ContainsKey(text))
			{
				list = this.mWordBehaviours[text];
			}
			else
			{
				if (!this.mWordBehaviours.ContainsKey("Template_ID"))
				{
					Debug.Log("No prefab available for string value " + text);
					return null;
				}
				list = this.mWordBehaviours["Template_ID"];
			}
			foreach (WordAbstractBehaviour current in list)
			{
				if (current.Trackable == null)
				{
					WordAbstractBehaviour result = this.AssociateWordBehaviour(wordResult, current);
					return result;
				}
			}
			if (list.Count < this.mMaxInstances)
			{
				WordAbstractBehaviour wordAbstractBehaviour = WordManagerImpl.InstantiateWordBehaviour(list.First<WordAbstractBehaviour>());
				list.Add(wordAbstractBehaviour);
				return this.AssociateWordBehaviour(wordResult, wordAbstractBehaviour);
			}
			return null;
		}

		private WordAbstractBehaviour AssociateWordBehaviour(WordResult wordResult, WordAbstractBehaviour wordBehaviourTemplate)
		{
			if (this.mActiveWordBehaviours.Count >= this.mMaxInstances)
			{
				return null;
			}
			Word word = wordResult.Word;
			wordBehaviourTemplate.InitializeWord(word);
			this.mActiveWordBehaviours.Add(word.ID, wordBehaviourTemplate);
			return wordBehaviourTemplate;
		}

		private static WordAbstractBehaviour InstantiateWordBehaviour(WordAbstractBehaviour input)
		{
			return UnityEngine.Object.Instantiate<GameObject>(input.gameObject).GetComponent<WordAbstractBehaviour>();
		}

		private static WordAbstractBehaviour CreateWordBehaviour()
		{
			GameObject gameObject = new GameObject("Word-AutoTemplate");
			WordAbstractBehaviour arg_20_0 = BehaviourComponentFactory.Instance.AddWordBehaviour(gameObject);
			Debug.Log("Creating Word Behaviour");
			return arg_20_0;
		}
	}
}
