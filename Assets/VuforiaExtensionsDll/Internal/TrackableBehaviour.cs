using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
	public abstract class TrackableBehaviour : MonoBehaviour
	{
		public enum Status
		{
			NOT_FOUND = -1,
			UNKNOWN,
			UNDEFINED,
			DETECTED,
			TRACKED,
			EXTENDED_TRACKED
		}

		internal enum CoordinateSystem
		{
			UNKNOWN,
			CAMERA,
			WORLD
		}

		[HideInInspector, SerializeField]
		protected string mTrackableName = "";

		[HideInInspector, SerializeField]
		protected bool mPreserveChildSize;

		[HideInInspector, SerializeField]
		protected bool mInitializedInEditor;

		protected Vector3 mPreviousScale = Vector3.zero;

		protected TrackableBehaviour.Status mStatus;

		protected Trackable mTrackable;

		private List<ITrackableEventHandler> mTrackableEventHandlers = new List<ITrackableEventHandler>();

		public TrackableBehaviour.Status CurrentStatus
		{
			get
			{
				return this.mStatus;
			}
		}

		public virtual Trackable Trackable
		{
			get
			{
				return this.mTrackable;
			}
		}

		public string TrackableName
		{
			get
			{
				return this.mTrackableName;
			}
		}

		internal double TimeStamp
		{
			get;
			set;
		}

		public void RegisterTrackableEventHandler(ITrackableEventHandler trackableEventHandler)
		{
			this.mTrackableEventHandlers.Add(trackableEventHandler);
			trackableEventHandler.OnTrackableStateChanged(TrackableBehaviour.Status.UNKNOWN, this.mStatus);
		}

		public bool UnregisterTrackableEventHandler(ITrackableEventHandler trackableEventHandler)
		{
			return this.mTrackableEventHandlers.Remove(trackableEventHandler);
		}

		public virtual void OnTrackerUpdate(TrackableBehaviour.Status newStatus)
		{
			TrackableBehaviour.Status status = this.mStatus;
			this.mStatus = newStatus;
			if (status != newStatus)
			{
				using (List<ITrackableEventHandler>.Enumerator enumerator = this.mTrackableEventHandlers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.OnTrackableStateChanged(status, newStatus);
					}
				}
			}
		}

		public virtual void OnFrameIndexUpdate(int newFrameIndex)
		{
		}

		private void OnDisable()
		{
			TrackableBehaviour.Status status = this.mStatus;
			this.mStatus = TrackableBehaviour.Status.NOT_FOUND;
			if (status != TrackableBehaviour.Status.NOT_FOUND)
			{
				using (List<ITrackableEventHandler>.Enumerator enumerator = this.mTrackableEventHandlers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.OnTrackableStateChanged(status, TrackableBehaviour.Status.NOT_FOUND);
					}
				}
			}
		}

		public bool CorrectScale()
		{
			return this.CorrectScaleImpl();
		}

		internal void UnregisterTrackable()
		{
			this.InternalUnregisterTrackable();
		}

		protected abstract void InternalUnregisterTrackable();

		protected virtual bool CorrectScaleImpl()
		{
			return false;
		}

		protected bool EnforceUniformScaling()
		{
			bool result = false;
			for (int i = 0; i < 3; i++)
			{
				if (base.transform.localScale[i] != this.mPreviousScale[i])
				{
					base.transform.localScale = new Vector3(base.transform.localScale[i], base.transform.localScale[i], base.transform.localScale[i]);
					this.mPreviousScale = base.transform.localScale;
					result = true;
					break;
				}
			}
			return result;
		}

		protected float SetScaleFromWidth(float width, float aspectRatio)
		{
			float num = (aspectRatio <= 1f) ? width : (width * aspectRatio);
			base.transform.localScale = new Vector3(num, num, num);
			return num;
		}

		protected float SetScaleFromHeight(float height, float aspectRatio)
		{
			float num = (aspectRatio <= 1f) ? (height / aspectRatio) : height;
			base.transform.localScale = new Vector3(num, num, num);
			return num;
		}

		protected float SetScale(float length, float aspectRatio1, float aspectRatio2)
		{
			float num = length;
			float num2 = Mathf.Max(aspectRatio1, aspectRatio2);
			if (num2 > 1f)
			{
				num = length * num2;
			}
			base.transform.localScale = new Vector3(num, num, num);
			return num;
		}
	}
}
