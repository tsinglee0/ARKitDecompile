using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Vuforia
{
	public class VuforiaAbstractBehaviour : MonoBehaviour
	{
		[HideInInspector, SerializeField]
		private VuforiaARController.WorldCenterMode mWorldCenterMode = VuforiaARController.WorldCenterMode.FIRST_TARGET;

		[HideInInspector, SerializeField]
		private TrackableBehaviour mWorldCenter;

		[HideInInspector, SerializeField]
		private Transform mCentralAnchorPoint;

		[HideInInspector, SerializeField]
		private Transform mParentAnchorPoint;

		[HideInInspector, SerializeField]
		private Camera mPrimaryCamera;

		[HideInInspector, SerializeField]
		private Camera mSecondaryCamera;

		[HideInInspector, SerializeField]
		private bool mWereBindingFieldsExposed;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public static event Action<VuforiaAbstractBehaviour> BehaviourCreated;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public static event Action<VuforiaAbstractBehaviour> BehaviourDestroyed;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public event Action AwakeEvent;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public event Action OnEnableEvent;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public event Action StartEvent;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public event Action UpdateEvent;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public event Action OnLevelWasLoadedEvent;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public event Action<bool> OnApplicationPauseEvent;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public event Action OnDisableEvent;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public event Action OnDestroyEvent;

		internal VuforiaARController.WorldCenterMode WorldCenterMode
		{
			get
			{
				return this.mWorldCenterMode;
			}
		}

		internal TrackableBehaviour WorldCenter
		{
			get
			{
				return this.mWorldCenter;
			}
		}

		internal Transform CentralAnchorPoint
		{
			get
			{
				return this.mCentralAnchorPoint;
			}
		}

		internal Transform ParentAnchorPoint
		{
			get
			{
				return this.mParentAnchorPoint;
			}
		}

		internal Camera PrimaryCamera
		{
			get
			{
				return this.mPrimaryCamera;
			}
		}

		internal Camera SecondaryCamera
		{
			get
			{
				return this.mSecondaryCamera;
			}
		}

		protected virtual void Awake()
		{
			if (VuforiaAbstractBehaviour.BehaviourCreated != null)
			{
				VuforiaAbstractBehaviour.BehaviourCreated.InvokeWithExceptionHandling(this);
			}
			if (this.AwakeEvent != null)
			{
				this.AwakeEvent.InvokeWithExceptionHandling();
			}
		}

		private void OnEnable()
		{
			if (this.OnEnableEvent != null)
			{
				this.OnEnableEvent.InvokeWithExceptionHandling();
			}
		}

		private void Start()
		{
			if (this.StartEvent != null)
			{
				this.StartEvent.InvokeWithExceptionHandling();
			}
		}

		private void Update()
		{
			if (this.UpdateEvent != null)
			{
				this.UpdateEvent.InvokeWithExceptionHandling();
			}
		}

		private void OnLevelWasLoaded()
		{
			if (this.OnLevelWasLoadedEvent != null)
			{
				this.OnLevelWasLoadedEvent.InvokeWithExceptionHandling();
			}
		}

		private void OnApplicationPause(bool pause)
		{
			if (this.OnApplicationPauseEvent != null)
			{
				this.OnApplicationPauseEvent.InvokeWithExceptionHandling(pause);
			}
		}

		private void OnDisable()
		{
			if (this.OnDisableEvent != null)
			{
				this.OnDisableEvent.InvokeWithExceptionHandling();
			}
		}

		private void OnDestroy()
		{
			if (this.OnDestroyEvent != null)
			{
				this.OnDestroyEvent.InvokeWithExceptionHandling();
			}
			if (VuforiaAbstractBehaviour.BehaviourDestroyed != null)
			{
				VuforiaAbstractBehaviour.BehaviourDestroyed.InvokeWithExceptionHandling(this);
			}
		}
	}
}
