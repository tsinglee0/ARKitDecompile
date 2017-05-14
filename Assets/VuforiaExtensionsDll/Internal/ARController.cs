using System;
using UnityEngine;

namespace Vuforia
{
	public abstract class ARController
	{
		private VuforiaAbstractBehaviour mVuforiaBehaviour;

		protected VuforiaAbstractBehaviour VuforiaBehaviour
		{
			get
			{
				return this.mVuforiaBehaviour;
			}
		}

		protected virtual void Awake()
		{
		}

		protected virtual void OnEnable()
		{
		}

		protected virtual void Start()
		{
		}

		protected virtual void Update()
		{
		}

		protected virtual void OnLevelWasLoaded()
		{
		}

		protected virtual void OnApplicationPause(bool pause)
		{
		}

		protected virtual void OnDisable()
		{
		}

		protected virtual void OnDestroy()
		{
		}

		protected static void Register(ARController controller)
		{
			VuforiaAbstractBehaviour.BehaviourCreated += delegate(VuforiaAbstractBehaviour bhvr)
			{
				controller.RegisterCamera(bhvr);
			};
			VuforiaAbstractBehaviour.BehaviourDestroyed += delegate(VuforiaAbstractBehaviour bhvr)
			{
				controller.UnregisterCamera(bhvr);
			};
		}

		private void RegisterCamera(VuforiaAbstractBehaviour bhvr)
		{
			if (this.mVuforiaBehaviour != null)
			{
				Debug.LogError("It's not possible to have two ARCameras at the same time");
				return;
			}
			this.mVuforiaBehaviour = bhvr;
			bhvr.AwakeEvent += new Action(this.Awake);
			bhvr.OnEnableEvent += new Action(this.OnEnable);
			bhvr.StartEvent += new Action(this.Start);
			bhvr.UpdateEvent += new Action(this.Update);
			bhvr.OnLevelWasLoadedEvent += new Action(this.OnLevelWasLoaded);
			bhvr.OnApplicationPauseEvent += new Action<bool>(this.OnApplicationPause);
			bhvr.OnDisableEvent += new Action(this.OnDisable);
			bhvr.OnDestroyEvent += new Action(this.OnDestroy);
		}

		private void UnregisterCamera(VuforiaAbstractBehaviour bhvr)
		{
			if (bhvr == this.mVuforiaBehaviour)
			{
				bhvr.AwakeEvent -= new Action(this.Awake);
				this.mVuforiaBehaviour = null;
				return;
			}
			Debug.LogWarning("Tried to unregister camera which hasn't been registered before");
		}
	}
}
