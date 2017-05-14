using System;
using UnityEngine;

namespace Vuforia
{
	internal class VuforiaDeinitBehaviour : MonoBehaviour
	{
		private static bool mAppIsQuitting;

		private void Awake()
		{
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			VuforiaARController.Instance.RegisterVuforiaDeinitializedCallback(new Action(VuforiaDeinitBehaviour.DeinitVuforia));
		}

		private void OnApplicationQuit()
		{
			VuforiaDeinitBehaviour.mAppIsQuitting = true;
			if (!VuforiaARController.Instance.HasStarted)
			{
				VuforiaDeinitBehaviour.DeinitVuforia();
			}
		}

		private static void DeinitVuforia()
		{
			if (VuforiaDeinitBehaviour.mAppIsQuitting)
			{
				VuforiaRuntime.Instance.Deinit();
				VuforiaDeinitBehaviour.mAppIsQuitting = false;
			}
		}
	}
}
