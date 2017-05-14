using System;

namespace Vuforia
{
	public class PlayModeUnityPlayer : IUnityPlayer, IDisposable
	{
		public void LoadNativeLibraries()
		{
		}

		public void InitializePlatform()
		{
			VuforiaWrapper.Instance.InitPlatformNative();
			SurfaceUtilities.SetSurfaceOrientation( UnityEngine.ScreenOrientation.Landscape);
		}

		private void InitializeSurface()
		{
			SurfaceUtilities.OnSurfaceCreated();
		}

		public VuforiaUnity.InitError InitializeVuforia(string licenseKey)
		{
			return (VuforiaUnity.InitError)VuforiaWrapper.Instance.QcarInit(licenseKey);
		}

		public void StartScene()
		{
		}

		public void Update()
		{
			if (SurfaceUtilities.HasSurfaceBeenRecreated())
			{
				this.InitializeSurface();
			}
		}

		public void Dispose()
		{
		}

		public void OnPause()
		{
		}

		public void OnResume()
		{
		}

		public void OnDestroy()
		{
			VuforiaUnity.Deinit();
		}
	}
}
