using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Vuforia
{
	internal static class VuforiaUnityImpl
	{
		private const int NO_WRAPPER = 768955;

		private const int UNITY_SDK_WRAPPER = 1817531;

		private const int UNITY_SDK_WRAPPER_AS = 2866107;

		private static bool mRendererDirty = false;

		private static int mWrapperType = 768955;

		public static void Deinit()
		{
			VuforiaWrapper.Instance.QcarDeinit();
		}

		public static bool IsRendererDirty()
		{
			CameraDeviceImpl arg_16_0 = (CameraDeviceImpl)CameraDevice.Instance;
			bool flag = VuforiaUnityImpl.mRendererDirty;
			VuforiaUnityImpl.mRendererDirty = false;
			return arg_16_0.IsDirty() | flag;
		}

		public static bool SetHint(VuforiaUnity.VuforiaHint hint, int value)
		{
			Debug.Log("SetHint");
			return VuforiaWrapper.Instance.QcarSetHint((uint)hint, value) == 1;
		}

		public static bool SetHint(uint hint, int value)
		{
			Debug.Log("SetHint");
			return VuforiaWrapper.Instance.QcarSetHint(hint, value) == 1;
		}

		public static Matrix4x4 GetProjectionGL(float nearPlane, float farPlane, ScreenOrientation screenOrientation)
		{
			float[] array = new float[16];
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * array.Length);
			VuforiaWrapper.Instance.GetProjectionGL(nearPlane, farPlane, intPtr, (int)screenOrientation);
			Marshal.Copy(intPtr, array, 0, array.Length);
			Matrix4x4 identity = Matrix4x4.identity;
			for (int i = 0; i < 16; i++)
			{
				identity[i] = array[i];
			}
			Marshal.FreeHGlobal(intPtr);
			return identity;
		}

		public static bool SetHolographicAppCoordinateSystem(IntPtr appSpecifiedCS)
		{
			return VuforiaWrapper.Instance.SetHolographicAppCS(appSpecifiedCS) == 1;
		}

		public static void SetApplicationEnvironment()
		{
			int unityVersionMajor = 0;
			int unityVersionMinor = 0;
			int unityVersionChange = 0;
			string[] array = Regex.Split(Application.unityVersion, "[^0-9]");
			if (array.Length >= 3)
			{
				unityVersionMajor = int.Parse(array[0]);
				unityVersionMinor = int.Parse(array[1]);
				unityVersionChange = int.Parse(array[2]);
			}
			VuforiaWrapper.Instance.SetApplicationEnvironment(unityVersionMajor, unityVersionMinor, unityVersionChange, VuforiaUnityImpl.mWrapperType);
		}

		public static void OnPause()
		{
			VuforiaWrapper.Instance.OnPause();
		}

		public static void OnResume()
		{
			VuforiaWrapper.Instance.OnResume();
		}

		public static void SetRendererDirty()
		{
			VuforiaUnityImpl.mRendererDirty = true;
		}

		public static string GetVuforiaLibraryVersion()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			VuforiaWrapper.Instance.GetVuforiaLibraryVersion(stringBuilder, 64);
			return stringBuilder.ToString();
		}

		public static void SetAssetInitializationParameters()
		{
			VuforiaUnityImpl.mWrapperType = 2866107;
		}

		public static void SetStandardInitializationParameters()
		{
			VuforiaUnityImpl.mWrapperType = 1817531;
		}
	}
}
