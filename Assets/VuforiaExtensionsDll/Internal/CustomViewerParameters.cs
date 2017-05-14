using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class CustomViewerParameters : ViewerParameters, ICustomViewerParameters, IViewerParameters
	{
		private float mVersion;

		private string mName;

		private string mManufacturer;

		private ViewerButtonType mButtonType;

		private float mScreenToLensDistance;

		private ViewerTrayAlignment mTrayAlignment;

		private bool mMagnet;

		public CustomViewerParameters(float version, string viewerName, string manufacturer) : base(VuforiaWrapper.Instance.CustomViewerParameters_new(version, viewerName, manufacturer))
		{
		}

		~CustomViewerParameters()
		{
			VuforiaWrapper.Instance.CustomViewerParameters_delete(this.mNativeVP);
			this.mNativeVP = IntPtr.Zero;
		}

		public void SetButtonType(ViewerButtonType val)
		{
			int val2 = 0;
			switch (val)
			{
			case ViewerButtonType.BUTTON_TYPE_NONE:
				val2 = 0;
				break;
			case ViewerButtonType.BUTTON_TYPE_MAGNET:
				val2 = 1;
				break;
			case ViewerButtonType.BUTTON_TYPE_FINGER_TOUCH:
				val2 = 2;
				break;
			case ViewerButtonType.BUTTON_TYPE_BUTTON_TOUCH:
				val2 = 3;
				break;
			}
			VuforiaWrapper.Instance.CustomViewerParameters_SetButtonType(this.mNativeVP, val2);
		}

		public void SetScreenToLensDistance(float val)
		{
			VuforiaWrapper.Instance.CustomViewerParameters_SetScreenToLensDistance(this.mNativeVP, val);
		}

		public void SetInterLensDistance(float val)
		{
			VuforiaWrapper.Instance.CustomViewerParameters_SetInterLensDistance(this.mNativeVP, val);
		}

		public void SetTrayAlignment(ViewerTrayAlignment val)
		{
			int val2 = 0;
			switch (val)
			{
			case ViewerTrayAlignment.TRAY_ALIGN_BOTTOM:
				val2 = 0;
				break;
			case ViewerTrayAlignment.TRAY_ALIGN_CENTRE:
				val2 = 1;
				break;
			case ViewerTrayAlignment.TRAY_ALIGN_TOP:
				val2 = 2;
				break;
			}
			VuforiaWrapper.Instance.CustomViewerParameters_SetTrayAlignment(this.mNativeVP, val2);
		}

		public void SetLensCentreToTrayDistance(float val)
		{
			VuforiaWrapper.Instance.CustomViewerParameters_SetLensCentreToTrayDistance(this.mNativeVP, val);
		}

		public void ClearDistortionCoefficients()
		{
			VuforiaWrapper.Instance.CustomViewerParameters_ClearDistortionCoefficients(this.mNativeVP);
		}

		public void AddDistortionCoefficient(float val)
		{
			VuforiaWrapper.Instance.CustomViewerParameters_AddDistortionCoefficient(this.mNativeVP, val);
		}

		public void SetFieldOfView(Vector4 val)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Vector4)));
			Marshal.StructureToPtr(val, intPtr, false);
			VuforiaWrapper.Instance.CustomViewerParameters_SetFieldOfView(this.mNativeVP, intPtr);
			Marshal.FreeHGlobal(intPtr);
		}

		public void SetContainsMagnet(bool val)
		{
			VuforiaWrapper.Instance.CustomViewerParameters_SetContainsMagnet(this.mNativeVP, val);
		}
	}
}
