using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	public class Device
	{
		public enum Mode
		{
			MODE_AR,
			MODE_VR
		}

		private static Device mInstance;

		public static Device Instance
		{
			get
			{
				if (Device.mInstance == null)
				{
					Type typeFromHandle = typeof(Device);
					lock (typeFromHandle)
					{
						if (Device.mInstance == null)
						{
							if (VuforiaWrapper.Instance.DeviceIsEyewearDevice() == 1)
							{
								Device.mInstance = new DedicatedEyewearDevice();
							}
							else
							{
								Device.mInstance = new Device();
							}
						}
					}
				}
				return Device.mInstance;
			}
		}

		public Device.Mode GetMode()
		{
			return (Device.Mode)VuforiaWrapper.Instance.Device_GetMode();
		}

		public bool IsViewerActive()
		{
			return VuforiaWrapper.Instance.Device_IsViewerPresent() == 1;
		}

		public IViewerParametersList GetViewerList()
		{
			IntPtr intPtr = VuforiaWrapper.Instance.Device_GetViewerList();
			if (intPtr != IntPtr.Zero)
			{
				return new ViewerParametersList(intPtr);
			}
			return null;
		}

		public bool SelectViewer(IViewerParameters vp)
		{
			if (vp is CustomViewerParameters)
			{
				return VuforiaWrapper.Instance.Device_SelectViewer(((CustomViewerParameters)vp).NativePtr) == 1;
			}
			if (vp is ViewerParameters)
			{
				return VuforiaWrapper.Instance.Device_SelectViewer(((ViewerParameters)vp).NativePtr) == 1;
			}
			Debug.LogError("Internal error: ViewerDevice. Select didn't recognise the parameter");
			return false;
		}

		public IViewerParameters GetSelectedViewer()
		{
			IntPtr intPtr = VuforiaWrapper.Instance.Device_GetSelectedViewer();
			if (intPtr != IntPtr.Zero)
			{
				return new ViewerParameters(intPtr);
			}
			return null;
		}

		public ICustomViewerParameters CreateCustomViewerParameters(float version, string name, string manufacturer)
		{
			return new CustomViewerParameters(version, name, manufacturer);
		}

		internal static void SetPlayModeEyewearDevice()
		{
			Type typeFromHandle = typeof(Device);
			lock (typeFromHandle)
			{
				Device.mInstance = new PlayModeEyewearDevice();
			}
		}

		internal static void UnsetDevice()
		{
			Type typeFromHandle = typeof(Device);
			lock (typeFromHandle)
			{
				Device.mInstance = null;
			}
		}

		internal void DeleteRenderingPrimitives()
		{
			VuforiaWrapper.Instance.RenderingPrimitives_DeleteCopy();
		}

		internal void SetViewerActive(bool isActive)
		{
			VuforiaWrapper.Instance.Device_SetViewerPresent(isActive);
		}

		internal bool SetMode(Device.Mode m)
		{
			return VuforiaWrapper.Instance.Device_SetMode((int)m) == 1;
		}

		internal Mesh GetDistortionMesh(View viewId, Mesh oldMesh)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VuforiaManagerImpl.MeshData)));
			VuforiaWrapper.Instance.RenderingPrimitives_GetDistortionMesh((int)viewId, intPtr);
			VuforiaManagerImpl.MeshData arg_3F_0 = (VuforiaManagerImpl.MeshData)Marshal.PtrToStructure(intPtr, typeof(VuforiaManagerImpl.MeshData));
			Marshal.FreeHGlobal(intPtr);
			return MeshUtils.UpdateMesh(arg_3F_0, oldMesh, false, false);
		}

		internal void GetTextureSize(View viewId, out int textureWidth, out int textureHeight)
		{
			int[] array = new int[4];
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * array.Length);
			VuforiaWrapper.Instance.RenderingPrimitives_GetDistortionMeshSize((int)viewId, intPtr);
			Marshal.Copy(intPtr, array, 0, array.Length);
			textureWidth = array[0];
			textureHeight = array[1];
			Marshal.FreeHGlobal(intPtr);
		}

		internal Matrix4x4 GetProjectionMatrix(View viewId, float near, float far, ScreenOrientation screenOrientation)
		{
			float[] array = new float[16];
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * array.Length);
			VuforiaWrapper.Instance.RenderingPrimitives_GetProjectionMatrix((int)viewId, near, far, intPtr, (int)screenOrientation);
			Marshal.Copy(intPtr, array, 0, array.Length);
			Matrix4x4 identity = Matrix4x4.identity;
			for (int i = 0; i < 16; i++)
			{
				identity[i] = array[i];
			}
			Marshal.FreeHGlobal(intPtr);
			return identity;
		}

		internal Rect GetDistortionTextureViewport(View viewId)
		{
			int[] array = new int[4];
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * array.Length);
			VuforiaWrapper.Instance.RenderingPrimitives_GetDistortionTextureViewport((int)viewId, intPtr);
			Marshal.Copy(intPtr, array, 0, array.Length);
			Rect arg_52_0 = new Rect((float)array[0], (float)array[1], (float)array[2], (float)array[3]);
			Marshal.FreeHGlobal(intPtr);
			return arg_52_0;
		}

		internal Rect GetViewport(View viewId)
		{
			int[] array = new int[4];
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * array.Length);
			VuforiaWrapper.Instance.RenderingPrimitives_GetViewport((int)viewId, intPtr);
			Marshal.Copy(intPtr, array, 0, array.Length);
			Rect arg_52_0 = new Rect((float)array[0], (float)array[1], (float)array[2], (float)array[3]);
			Marshal.FreeHGlobal(intPtr);
			return arg_52_0;
		}

		internal Rect GetNormalizedViewport(View viewId)
		{
			float[] array = new float[4];
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * array.Length);
			VuforiaWrapper.Instance.RenderingPrimitives_GetNormalizedViewport((int)viewId, intPtr);
			Marshal.Copy(intPtr, array, 0, array.Length);
			Rect arg_4E_0 = new Rect(array[0], array[1], array[2], array[3]);
			Marshal.FreeHGlobal(intPtr);
			return arg_4E_0;
		}

		internal Matrix4x4 GetEyeDisplayAdjustmentMatrix(View viewId)
		{
			float[] array = new float[16];
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * array.Length);
			VuforiaWrapper.Instance.RenderingPrimitives_GetEyeDisplayAdjustmentMatrix((int)viewId, intPtr);
			Marshal.Copy(intPtr, array, 0, array.Length);
			Matrix4x4 identity = Matrix4x4.identity;
			for (int i = 0; i < 16; i++)
			{
				identity[i] = array[i];
			}
			Marshal.FreeHGlobal(intPtr);
			return identity;
		}

		internal Vector4 GetEffectiveFovRads(View viewId)
		{
			float[] array = new float[4];
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * array.Length);
			VuforiaWrapper.Instance.RenderingPrimitives_GetEffectiveFov((int)viewId, intPtr);
			Marshal.Copy(intPtr, array, 0, array.Length);
			Vector4 arg_4E_0 = new Vector4(array[0], array[1], array[2], array[3]);
			Marshal.FreeHGlobal(intPtr);
			return arg_4E_0;
		}

		internal Vector2 GetViewportCentreToEyeAxis(View viewId)
		{
			float[] array = new float[2];
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * array.Length);
			VuforiaWrapper.Instance.RenderingPrimitives_GetViewportCentreToEyeAxis((int)viewId, intPtr);
			Marshal.Copy(intPtr, array, 0, array.Length);
			Vector2 arg_48_0 = new Vector2(array[0], array[1]);
			Marshal.FreeHGlobal(intPtr);
			return arg_48_0;
		}
	}
}
