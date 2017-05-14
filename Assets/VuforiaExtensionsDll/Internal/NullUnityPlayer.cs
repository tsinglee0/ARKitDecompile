using System;

namespace Vuforia
{
	public class NullUnityPlayer : IUnityPlayer, IDisposable
	{
		public void LoadNativeLibraries()
		{
		}

		public void InitializePlatform()
		{
		}

		public VuforiaUnity.InitError InitializeVuforia(string licenseKey)
		{
			return VuforiaUnity.InitError.INIT_SUCCESS;
		}

		public void StartScene()
		{
		}

		public void Update()
		{
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
		}
	}
}
