using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class ViewerParameters : IViewerParameters
	{
		protected IntPtr mNativeVP;

		internal IntPtr NativePtr
		{
			get
			{
				return this.mNativeVP;
			}
		}

		internal ViewerParameters(IntPtr nativePtr)
		{
			if (nativePtr == IntPtr.Zero)
			{
				throw new ArgumentNullException();
			}
			this.mNativeVP = nativePtr;
		}

		~ViewerParameters()
		{
			if (this.mNativeVP != IntPtr.Zero)
			{
				VuforiaWrapper.CamIndependentInstance.ViewerParameters_delete(this.mNativeVP);
			}
		}

		public float GetVersion()
		{
			return VuforiaWrapper.CamIndependentInstance.ViewerParameters_GetVersion(this.NativePtr);
		}

		public string GetName()
		{
			return Marshal.PtrToStringAnsi(VuforiaWrapper.CamIndependentInstance.ViewerParameters_GetName(this.NativePtr));
		}

		public string GetManufacturer()
		{
			return Marshal.PtrToStringAnsi(VuforiaWrapper.CamIndependentInstance.ViewerParameters_GetManufacturer(this.NativePtr));
		}

		public ViewerButtonType GetButtonType()
		{
			switch (VuforiaWrapper.CamIndependentInstance.ViewerParameters_GetButtonType(this.NativePtr))
			{
			case 0:
				return ViewerButtonType.BUTTON_TYPE_NONE;
			case 1:
				return ViewerButtonType.BUTTON_TYPE_MAGNET;
			case 2:
				return ViewerButtonType.BUTTON_TYPE_FINGER_TOUCH;
			case 3:
				return ViewerButtonType.BUTTON_TYPE_BUTTON_TOUCH;
			default:
				Debug.LogWarning("Unexpected ViewerButtonType returned from Vuforia native");
				return ViewerButtonType.BUTTON_TYPE_NONE;
			}
		}

		public ViewerTrayAlignment GetTrayAlignment()
		{
			switch (VuforiaWrapper.CamIndependentInstance.ViewerParameters_GetTrayAlignment(this.NativePtr))
			{
			case 0:
				return ViewerTrayAlignment.TRAY_ALIGN_BOTTOM;
			case 1:
				return ViewerTrayAlignment.TRAY_ALIGN_CENTRE;
			case 2:
				return ViewerTrayAlignment.TRAY_ALIGN_TOP;
			default:
				Debug.LogWarning("Unexpected ViewerTrayAlignment returned from Vuforia native");
				return ViewerTrayAlignment.TRAY_ALIGN_BOTTOM;
			}
		}

		public float GetScreenToLensDistance()
		{
			return VuforiaWrapper.CamIndependentInstance.ViewerParameters_GetScreenToLensDistance(this.NativePtr);
		}

		public float GetInterLensDistance()
		{
			return VuforiaWrapper.CamIndependentInstance.ViewerParameters_GetInterLensDistance(this.NativePtr);
		}

		public float GetLensCentreToTrayDistance()
		{
			return VuforiaWrapper.CamIndependentInstance.ViewerParameters_GetLensCentreToTrayDistance(this.NativePtr);
		}

		public int GetNumDistortionCoefficients()
		{
			return VuforiaWrapper.CamIndependentInstance.ViewerParameters_GetNumDistortionCoefficients(this.NativePtr);
		}

		public float GetDistortionCoefficient(int idx)
		{
			return VuforiaWrapper.CamIndependentInstance.ViewerParameters_GetDistortionCoefficient(this.NativePtr, idx);
		}

		public Vector4 GetFieldOfView()
		{
			float[] array = new float[4];
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * array.Length);
			VuforiaWrapper.CamIndependentInstance.ViewerParameters_GetFieldOfView(this.NativePtr, intPtr);
			Marshal.Copy(intPtr, array, 0, array.Length);
			Vector4 arg_53_0 = new Vector4(array[0], array[1], array[2], array[3]);
			Marshal.FreeHGlobal(intPtr);
			return arg_53_0;
		}

		public bool ContainsMagnet()
		{
			return VuforiaWrapper.CamIndependentInstance.ViewerParameters_ContainsMagnet(this.NativePtr) == 1;
		}
	}
}
