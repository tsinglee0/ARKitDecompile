using System;
using UnityEngine;

namespace Vuforia
{
	public class VuforiaRuntime
	{
		private Action<VuforiaUnity.InitError> mOnVuforiaInitError;

		private bool mFailedToInitialize;

		private VuforiaUnity.InitError mInitError;

		private bool mHasInitialized;

		private static VuforiaRuntime mInstance;

		private static object mPadlock = new object();

		public static VuforiaRuntime Instance
		{
			get
			{
				if (VuforiaRuntime.mInstance == null)
				{
					object obj = VuforiaRuntime.mPadlock;
					lock (obj)
					{
						if (VuforiaRuntime.mInstance == null)
						{
							VuforiaRuntime.mInstance = new VuforiaRuntime();
						}
					}
				}
				return VuforiaRuntime.mInstance;
			}
		}

		public bool HasInitialized
		{
			get
			{
				return this.mHasInitialized;
			}
		}

		private VuforiaRuntime()
		{
		}

		public void InitPlatform(IUnityPlayer player)
		{
			UnityPlayer.SetImplementation(player);
			UnityPlayer.Instance.InitializePlatform();
		}

		public void InitVuforia()
		{
			if (!this.mHasInitialized)
			{
				VuforiaUnityImpl.SetApplicationEnvironment();
				VuforiaUnity.InitError initError = UnityPlayer.Instance.InitializeVuforia(VuforiaAbstractConfiguration.Instance.Vuforia.LicenseKey);
				if (initError != VuforiaUnity.InitError.INIT_SUCCESS)
				{
					Debug.LogError("Vuforia initialization failed");
					if (this.mOnVuforiaInitError != null)
					{
						this.mOnVuforiaInitError.InvokeWithExceptionHandling(initError);
					}
					this.mHasInitialized = false;
					this.mFailedToInitialize = true;
					this.mInitError = initError;
					return;
				}
				Debug.Log("Vuforia initialization successful");
				this.CreateDeinitHelper();
				this.mHasInitialized = true;
			}
		}

		public void Deinit()
		{
			if (this.mHasInitialized)
			{
				UnityPlayer.Instance.OnDestroy();
				UnityPlayer.Instance.Dispose();
				this.mHasInitialized = false;
			}
		}

		public void RegisterVuforiaInitErrorCallback(Action<VuforiaUnity.InitError> callback)
		{
			this.mOnVuforiaInitError = (Action<VuforiaUnity.InitError>)Delegate.Combine(this.mOnVuforiaInitError, callback);
			if (this.mFailedToInitialize)
			{
				callback(this.mInitError);
			}
		}

		public void UnregisterVuforiaInitErrorCallback(Action<VuforiaUnity.InitError> callback)
		{
			this.mOnVuforiaInitError = (Action<VuforiaUnity.InitError>)Delegate.Remove(this.mOnVuforiaInitError, callback);
		}

		private void CreateDeinitHelper()
		{
			GameObject expr_0A = new GameObject("VuforiaRuntime");
			expr_0A.AddComponent<VuforiaDeinitBehaviour>();
			expr_0A.hideFlags = HideFlags.HideInHierarchy;
		}
	}
}
