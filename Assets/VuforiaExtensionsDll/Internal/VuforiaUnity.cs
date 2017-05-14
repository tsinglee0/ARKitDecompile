using System;
using UnityEngine;

namespace Vuforia
{
	public static class VuforiaUnity
	{
		public enum InitError
		{
			INIT_EXTERNAL_DEVICE_NOT_DETECTED = -10,
			INIT_LICENSE_ERROR_PRODUCT_TYPE_MISMATCH,
			INIT_LICENSE_ERROR_CANCELED_KEY,
			INIT_LICENSE_ERROR_NO_NETWORK_TRANSIENT,
			INIT_LICENSE_ERROR_NO_NETWORK_PERMANENT,
			INIT_LICENSE_ERROR_INVALID_KEY,
			INIT_LICENSE_ERROR_MISSING_KEY,
			INIT_NO_CAMERA_ACCESS,
			INIT_DEVICE_NOT_SUPPORTED,
			INIT_ERROR,
			INIT_SUCCESS
		}

		public enum VuforiaHint
		{
			HINT_MAX_SIMULTANEOUS_IMAGE_TARGETS,
			HINT_MAX_SIMULTANEOUS_OBJECT_TARGETS,
			HINT_DELAYED_LOADING_OBJECT_DATASETS
		}

		public enum StorageType
		{
			STORAGE_APP,
			STORAGE_APPRESOURCE,
			STORAGE_ABSOLUTE
		}

		private static IHoloLensApiAbstraction mHoloLensApiAbstraction = new NullHoloLensApiAbstraction();

		public static void Deinit()
		{
			VuforiaUnityImpl.Deinit();
		}

		public static bool IsRendererDirty()
		{
			return VuforiaUnityImpl.IsRendererDirty();
		}

		public static bool SetHint(VuforiaUnity.VuforiaHint hint, int value)
		{
			return VuforiaUnityImpl.SetHint(hint, value);
		}

		public static bool SetHint(uint hint, int value)
		{
			return VuforiaUnityImpl.SetHint(hint, value);
		}

		public static Matrix4x4 GetProjectionGL(float nearPlane, float farPlane, ScreenOrientation screenOrientation)
		{
			return VuforiaUnityImpl.GetProjectionGL(nearPlane, farPlane, screenOrientation);
		}

		public static void OnPause()
		{
			VuforiaUnityImpl.OnPause();
		}

		public static void OnResume()
		{
			VuforiaUnityImpl.OnResume();
		}

		public static void SetRendererDirty()
		{
			VuforiaUnityImpl.SetRendererDirty();
		}

		public static string GetVuforiaLibraryVersion()
		{
			return VuforiaUnityImpl.GetVuforiaLibraryVersion();
		}

		public static bool SetHolographicAppCoordinateSystem(IntPtr appSpecifiedCS)
		{
			return VuforiaUnityImpl.SetHolographicAppCoordinateSystem(appSpecifiedCS);
		}

		public static void SetHoloLensApiAbstraction(IHoloLensApiAbstraction holoLensApiAbstraction)
		{
			VuforiaUnity.mHoloLensApiAbstraction = holoLensApiAbstraction;
		}

		public static void SetAssetInitializationParameters()
		{
			VuforiaUnityImpl.SetAssetInitializationParameters();
		}

		public static void SetStandardInitializationParameters()
		{
			VuforiaUnityImpl.SetStandardInitializationParameters();
		}

		internal static IHoloLensApiAbstraction GetHoloLensApiAbstraction()
		{
			return VuforiaUnity.mHoloLensApiAbstraction;
		}
	}
}
