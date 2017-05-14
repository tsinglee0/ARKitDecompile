using System;

namespace Vuforia
{
	public interface IUnityPlayer : IDisposable
	{
		void LoadNativeLibraries();

		void InitializePlatform();

		VuforiaUnity.InitError InitializeVuforia(string licenseKey);

		void StartScene();

		void Update();

		void OnPause();

		void OnResume();

		void OnDestroy();
	}
}
