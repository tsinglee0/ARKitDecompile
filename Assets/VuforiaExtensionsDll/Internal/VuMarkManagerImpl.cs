using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class VuMarkManagerImpl : VuMarkManager
	{
		private Dictionary<int, List<VuMarkAbstractBehaviour>> mBehaviours = new Dictionary<int, List<VuMarkAbstractBehaviour>>();

		private List<VuMarkTarget> mActiveVuMarkTargets = new List<VuMarkTarget>();

		private List<VuMarkAbstractBehaviour> mDestroyedBehaviours = new List<VuMarkAbstractBehaviour>();

		private Action<VuMarkTarget> mOnVuMarkDetected;

		private Action<VuMarkTarget> mOnVuMarkLost;

		private Action<VuMarkAbstractBehaviour> mOnVuMarkBehaviourDetected;

		public override IEnumerable<VuMarkTarget> GetActiveVuMarks()
		{
			return this.mActiveVuMarkTargets;
		}

		public override IEnumerable<VuMarkAbstractBehaviour> GetActiveBehaviours(VuMarkTarget vuMark)
		{
			int iD = vuMark.Template.ID;
			if (!this.mBehaviours.ContainsKey(iD))
			{
				return Enumerable.Empty<VuMarkAbstractBehaviour>();
			}
			List<VuMarkAbstractBehaviour> arg_32_0 = this.mBehaviours[iD];
			List<VuMarkAbstractBehaviour> list = new List<VuMarkAbstractBehaviour>();
			foreach (VuMarkAbstractBehaviour current in arg_32_0)
			{
				if (current.VuMarkTarget == vuMark)
				{
					list.Add(current);
				}
			}
			return list;
		}

		public override IEnumerable<VuMarkAbstractBehaviour> GetActiveBehaviours()
		{
			List<VuMarkAbstractBehaviour> list = new List<VuMarkAbstractBehaviour>();
			using (Dictionary<int, List<VuMarkAbstractBehaviour>>.ValueCollection.Enumerator enumerator = this.mBehaviours.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					foreach (VuMarkAbstractBehaviour current in enumerator.Current)
					{
						if (current.VuMarkTarget != null)
						{
							list.Add(current);
						}
					}
				}
			}
			return list;
		}

		public override IEnumerable<VuMarkAbstractBehaviour> GetAllBehaviours()
		{
			List<VuMarkAbstractBehaviour> list = new List<VuMarkAbstractBehaviour>();
			using (Dictionary<int, List<VuMarkAbstractBehaviour>>.ValueCollection.Enumerator enumerator = this.mBehaviours.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					foreach (VuMarkAbstractBehaviour current in enumerator.Current)
					{
						list.Add(current);
					}
				}
			}
			return list;
		}

		public override void RegisterVuMarkDetectedCallback(Action<VuMarkTarget> callback)
		{
			this.mOnVuMarkDetected = (Action<VuMarkTarget>)Delegate.Combine(this.mOnVuMarkDetected, callback);
		}

		public override void UnregisterVuMarkDetectedCallback(Action<VuMarkTarget> callback)
		{
			this.mOnVuMarkDetected = (Action<VuMarkTarget>)Delegate.Remove(this.mOnVuMarkDetected, callback);
		}

		public override void RegisterVuMarkLostCallback(Action<VuMarkTarget> callback)
		{
			this.mOnVuMarkLost = (Action<VuMarkTarget>)Delegate.Combine(this.mOnVuMarkLost, callback);
		}

		public override void UnregisterVuMarkLostCallback(Action<VuMarkTarget> callback)
		{
			this.mOnVuMarkLost = (Action<VuMarkTarget>)Delegate.Remove(this.mOnVuMarkLost, callback);
		}

		public override void RegisterVuMarkBehaviourDetectedCallback(Action<VuMarkAbstractBehaviour> callback)
		{
			this.mOnVuMarkBehaviourDetected = (Action<VuMarkAbstractBehaviour>)Delegate.Combine(this.mOnVuMarkBehaviourDetected, callback);
		}

		public override void UnregisterVuMarkBehaviourDetectedCallback(Action<VuMarkAbstractBehaviour> callback)
		{
			this.mOnVuMarkBehaviourDetected = (Action<VuMarkAbstractBehaviour>)Delegate.Remove(this.mOnVuMarkBehaviourDetected, callback);
		}

		internal void UpdateVuMarks(VuforiaManagerImpl.VuMarkTargetData[] newVuMarks, VuforiaManagerImpl.VuMarkTargetResultData[] vuMarkResults)
		{
			this.UpdateNewVuMarks(newVuMarks);
			HashSet<int> hashSet = new HashSet<int>();
			for (int i = 0; i < vuMarkResults.Length; i++)
			{
				VuforiaManagerImpl.VuMarkTargetResultData vuMarkTargetResultData = vuMarkResults[i];
				hashSet.Add(vuMarkTargetResultData.targetID);
			}
			this.UpdateLostVuMarks(hashSet);
			this.UpdateVuMarkResults(vuMarkResults);
		}

		internal void UpdateVuMarkPoses(Transform arCameraTransform, VuforiaManagerImpl.VuMarkTargetResultData[] vuMarkResults, int originTrackableResultId)
		{
			for (int i = 0; i < vuMarkResults.Length; i++)
			{
				VuforiaManagerImpl.VuMarkTargetResultData vuMarkTargetResultData = vuMarkResults[i];
				VuMarkAbstractBehaviour activeBehaviour = this.GetActiveBehaviour(vuMarkTargetResultData.targetID, vuMarkTargetResultData.resultID);
				if (activeBehaviour != null)
				{
					if (vuMarkTargetResultData.resultID != originTrackableResultId)
					{
						StateManagerImpl.PositionTrackable(activeBehaviour, arCameraTransform, vuMarkTargetResultData.pose, vuMarkTargetResultData.timeStamp);
					}
				}
				else
				{
					Debug.Log(string.Format("Have a null behaviour while iterating over VuMarkResults: resultID={0}, targetID={1}", vuMarkTargetResultData.targetID, vuMarkTargetResultData.resultID));
				}
			}
		}

		internal bool AddTemplateBehaviour(VuMarkAbstractBehaviour behaviour)
		{
			if (!this.mDestroyedBehaviours.Contains(behaviour))
			{
				this.mBehaviours[behaviour.VuMarkTemplate.ID] = new List<VuMarkAbstractBehaviour>
				{
					behaviour
				};
				return true;
			}
			return false;
		}

		internal bool ContainsVuMarkTemplate(int templateId)
		{
			using (Dictionary<int, List<VuMarkAbstractBehaviour>>.KeyCollection.Enumerator enumerator = this.mBehaviours.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == templateId)
					{
						return true;
					}
				}
			}
			return false;
		}

		internal void ClearBehaviours()
		{
			this.DestroyBehaviourCopies();
			this.mBehaviours.Clear();
			this.mActiveVuMarkTargets.Clear();
		}

		internal void DestroyBehaviourCopies()
		{
			this.mDestroyedBehaviours.Clear();
			foreach (List<VuMarkAbstractBehaviour> current in this.mBehaviours.Values)
			{
				for (int i = 1; i < current.Count; i++)
				{
					if (!this.mDestroyedBehaviours.Contains(current[i]))
					{
						this.mDestroyedBehaviours.Add(current[i]);
						if (current[i] != null)
						{
                            UnityEngine.Object.Destroy(current[i].gameObject);
						}
					}
				}
				VuMarkAbstractBehaviour vuMarkAbstractBehaviour = current[0];
				vuMarkAbstractBehaviour.UnregisterVuMarkTarget();
				current.Clear();
				current.Add(vuMarkAbstractBehaviour);
			}
		}

		internal void RemoveDestroyedTrackables()
		{
			foreach (List<VuMarkAbstractBehaviour> current in this.mBehaviours.Values)
			{
				for (int i = current.Count - 1; i >= 0; i--)
				{
					if (current[i] == null)
					{
						current.RemoveAt(i);
					}
				}
			}
			int[] array = this.mBehaviours.Keys.ToArray<int>();
			for (int j = 0; j < array.Length; j++)
			{
				int key = array[j];
				if (this.mBehaviours[key].Count == 0)
				{
					this.mBehaviours.Remove(key);
				}
			}
		}

		internal bool IsBehaviourMarkedForDeletion(TrackableBehaviour behaviour)
		{
			VuMarkAbstractBehaviour vuMarkAbstractBehaviour = behaviour as VuMarkAbstractBehaviour;
			return vuMarkAbstractBehaviour != null && this.mDestroyedBehaviours.Contains(vuMarkAbstractBehaviour);
		}

		internal VuMarkAbstractBehaviour GetBehaviourWithResultID(int resultId)
		{
			foreach (VuMarkAbstractBehaviour current in this.GetActiveBehaviours())
			{
				if (current.VuMarkResultId == resultId)
				{
					return current;
				}
			}
			return null;
		}

		private void UpdateNewVuMarks(VuforiaManagerImpl.VuMarkTargetData[] newVuMarks)
		{
			for (int i = 0; i < newVuMarks.Length; i++)
			{
				VuforiaManagerImpl.VuMarkTargetData newVuMark = newVuMarks[i];
				VuMarkTarget vuMarkTarget = this.CreateVuMarkTarget(newVuMark);
				this.mActiveVuMarkTargets.Add(vuMarkTarget);
				if (this.mOnVuMarkDetected != null)
				{
					this.mOnVuMarkDetected.InvokeWithExceptionHandling(vuMarkTarget);
				}
			}
		}

		private void UpdateLostVuMarks(HashSet<int> trackedIndices)
		{
			List<VuMarkTarget> list = new List<VuMarkTarget>();
			foreach (VuMarkTarget current in this.mActiveVuMarkTargets)
			{
				if (current != null && !trackedIndices.Contains(current.ID))
				{
					list.Add(current);
				}
			}
			foreach (VuMarkTarget current2 in list)
			{
				this.mActiveVuMarkTargets.Remove(current2);
				if (this.mOnVuMarkLost != null)
				{
					this.mOnVuMarkLost.InvokeWithExceptionHandling(current2);
				}
			}
		}

		private VuMarkTarget CreateVuMarkTarget(VuforiaManagerImpl.VuMarkTargetData newVuMark)
		{
			VuMarkTemplateImpl template = (VuMarkTemplateImpl)this.GetVuMarkTemplate(newVuMark.templateId);
			uint dataLength = newVuMark.instanceId.dataLength;
			byte[] array = new byte[dataLength];
			if (array.Length != 0)
			{
				Marshal.Copy(newVuMark.instanceId.buffer, array, 0, array.Length);
			}
			return new VuMarkTargetImpl(newVuMark.id, array, newVuMark.instanceId.numericValue, (InstanceIdType)newVuMark.instanceId.dataType, dataLength, template);
		}

		private VuMarkAbstractBehaviour CreateOrGetUnusedBehaviour(int templateId)
		{
			if (!this.mBehaviours.ContainsKey(templateId))
			{
				Debug.LogError("Template " + templateId + " is not available in behaviour list");
			}
			VuMarkAbstractBehaviour vuMarkAbstractBehaviour = null;
			foreach (VuMarkAbstractBehaviour current in this.mBehaviours[templateId])
			{
				if (current.VuMarkTarget == null)
				{
					vuMarkAbstractBehaviour = current;
				}
			}
			if (vuMarkAbstractBehaviour == null)
			{
				vuMarkAbstractBehaviour = this.CopyGameObject(this.mBehaviours[templateId][0]);
				this.mBehaviours[templateId].Add(vuMarkAbstractBehaviour);
			}
			if (this.mOnVuMarkBehaviourDetected != null)
			{
				this.mOnVuMarkBehaviourDetected.InvokeWithExceptionHandling(vuMarkAbstractBehaviour);
			}
			return vuMarkAbstractBehaviour;
		}

		private VuMarkTemplate GetVuMarkTemplate(int templateId)
		{
			if (!this.mBehaviours.ContainsKey(templateId))
			{
				Debug.LogError("Template " + templateId + " is not available in behaviour list");
				return null;
			}
			return this.mBehaviours[templateId][0].VuMarkTemplate;
		}

		private VuMarkAbstractBehaviour CopyGameObject(VuMarkAbstractBehaviour bhvr)
		{
			VuMarkAbstractBehaviour expr_10 = UnityEngine.Object.Instantiate<GameObject>(bhvr.gameObject).GetComponent<VuMarkAbstractBehaviour>();
			expr_10.InitializeTarget(bhvr.VuMarkTemplate, false);
			return expr_10;
		}

		private void MarkBehaviourUntracked(VuMarkAbstractBehaviour behaviour)
		{
			if (behaviour.VuMarkTarget != null && behaviour.VuMarkResultId != -1)
			{
				behaviour.OnTrackerUpdate(TrackableBehaviour.Status.NOT_FOUND);
				behaviour.UnregisterVuMarkTarget();
				behaviour.VuMarkResultId = -1;
				behaviour.gameObject.SetActive(false);
			}
		}

		private void AssociateTargetWithBehaviour(VuMarkTarget vuMark, VuMarkAbstractBehaviour bhvr)
		{
			bhvr.gameObject.SetActive(true);
			bhvr.RegisterVuMarkTarget(vuMark);
		}

		private void UpdateVuMarkResults(VuforiaManagerImpl.VuMarkTargetResultData[] vuMarkResults)
		{
			HashSet<int> hashSet = new HashSet<int>();
			VuforiaManagerImpl.VuMarkTargetResultData[] array = vuMarkResults;
			for (int i = 0; i < array.Length; i++)
			{
				VuforiaManagerImpl.VuMarkTargetResultData vuMarkTargetResultData = array[i];
				hashSet.Add(vuMarkTargetResultData.resultID);
			}
			foreach (VuMarkAbstractBehaviour current in this.GetActiveBehaviours().ToList<VuMarkAbstractBehaviour>())
			{
				if (!hashSet.Contains(current.VuMarkResultId))
				{
					this.MarkBehaviourUntracked(current);
				}
			}
			array = vuMarkResults;
			for (int i = 0; i < array.Length; i++)
			{
				VuforiaManagerImpl.VuMarkTargetResultData vuMarkTargetResultData2 = array[i];
				VuMarkTarget vuMarkTarget = this.GetVuMarkTarget(vuMarkTargetResultData2.targetID);
				if (vuMarkTarget == null)
				{
					Debug.LogError("Error: Target is not available");
				}
				VuMarkAbstractBehaviour vuMarkAbstractBehaviour = null;
				foreach (VuMarkAbstractBehaviour current2 in this.GetActiveBehaviours(vuMarkTarget).ToList<VuMarkAbstractBehaviour>())
				{
					if (current2.VuMarkResultId == vuMarkTargetResultData2.resultID)
					{
						vuMarkAbstractBehaviour = current2;
						break;
					}
				}
				if (vuMarkAbstractBehaviour == null)
				{
					VuMarkAbstractBehaviour vuMarkAbstractBehaviour2 = this.CreateOrGetUnusedBehaviour(vuMarkTarget.Template.ID);
					this.AssociateTargetWithBehaviour(vuMarkTarget, vuMarkAbstractBehaviour2);
					vuMarkAbstractBehaviour = vuMarkAbstractBehaviour2;
					vuMarkAbstractBehaviour.VuMarkResultId = vuMarkTargetResultData2.resultID;
				}
			}
		}

		private VuMarkAbstractBehaviour GetActiveBehaviour(int targetId, int resultId)
		{
			VuMarkTarget vuMarkTarget = this.GetVuMarkTarget(targetId);
			if (vuMarkTarget == null)
			{
				return null;
			}
			foreach (VuMarkAbstractBehaviour current in this.GetActiveBehaviours(vuMarkTarget).ToList<VuMarkAbstractBehaviour>())
			{
				if (current.VuMarkResultId == resultId)
				{
					return current;
				}
			}
			return null;
		}

		private VuMarkTarget GetVuMarkTarget(int targetId)
		{
			foreach (VuMarkTarget current in this.GetActiveVuMarks())
			{
				if (current.ID == targetId)
				{
					return current;
				}
			}
			return null;
		}

		private Dictionary<int, List<int>> GroupResultsByTargetId(VuforiaManagerImpl.VuMarkTargetResultData[] vuMarkResults)
		{
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			for (int i = 0; i < vuMarkResults.Length; i++)
			{
				int targetID = vuMarkResults[i].targetID;
				if (!dictionary.ContainsKey(targetID))
				{
					dictionary[targetID] = new List<int>();
				}
				dictionary[targetID].Add(i);
			}
			return dictionary;
		}
	}
}
