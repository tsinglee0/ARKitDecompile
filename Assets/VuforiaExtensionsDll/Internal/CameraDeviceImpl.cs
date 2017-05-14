using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Vuforia
{
	internal class CameraDeviceImpl : CameraDevice
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct CameraFieldData
		{
			public string KeyValue;

			public int DataType;

			public int Unused;
		}

		private Dictionary<Image.PIXEL_FORMAT, Image> mCameraImages;

		private List<Image.PIXEL_FORMAT> mForcedCameraFormats;

		private static IWebCam mWebCam;

		private bool mCameraReady;

		private bool mIsDirty;

		private CameraDevice.CameraDirection mActualCameraDirection;

		private CameraDevice.CameraDirection mSelectedCameraDirection;

		private CameraDevice.CameraDeviceMode mCameraDeviceMode = CameraDevice.CameraDeviceMode.MODE_DEFAULT;

		private CameraDevice.VideoModeData mVideoModeData;

		private bool mVideoModeDataNeedsUpdate = true;

		private bool mHasCameraDeviceModeBeenSet;

		private bool mCameraActive;

		public IWebCam WebCam
		{
			get
			{
				return CameraDeviceImpl.mWebCam;
			}
			internal set
			{
				CameraDeviceImpl.mWebCam = value;
			}
		}

		public bool CameraReady
		{
			get
			{
				if (VuforiaRuntimeUtilities.IsPlayMode())
				{
					return CameraDeviceImpl.mWebCam != null && CameraDeviceImpl.mWebCam.IsTextureSizeAvailable;
				}
				return this.mCameraReady;
			}
		}

		public override bool Init(CameraDevice.CameraDirection cameraDirection)
		{
			if (this.InitCameraDevice((int)cameraDirection) == 0)
			{
				return false;
			}
			this.mSelectedCameraDirection = cameraDirection;
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				this.mActualCameraDirection = CameraDevice.CameraDirection.CAMERA_BACK;
			}
			else
			{
				int num = VuforiaWrapper.Instance.CameraDeviceGetCameraDirection();
				if (num != 1)
				{
					if (num != 2)
					{
						this.mActualCameraDirection = CameraDevice.CameraDirection.CAMERA_DEFAULT;
					}
					else
					{
						this.mActualCameraDirection = CameraDevice.CameraDirection.CAMERA_FRONT;
					}
				}
				else
				{
					this.mActualCameraDirection = CameraDevice.CameraDirection.CAMERA_BACK;
				}
			}
			this.mCameraReady = true;
			if (this.CameraReady)
			{
				VuforiaARController.Instance.ResetBackgroundPlane(true);
			}
			return true;
		}

		public override bool Deinit()
		{
			if (this.DeinitCameraDevice() == 0)
			{
				return false;
			}
			this.mCameraReady = false;
			return true;
		}

		public override bool Start()
		{
			this.mIsDirty = true;
			this.mVideoModeDataNeedsUpdate = true;
			GL.Clear(false, true, new Color(0f, 0f, 0f, 1f));
			if (this.StartCameraDevice() == 0)
			{
				return false;
			}
			if (VuforiaRuntimeUtilities.IsPlayMode() && VuforiaRuntimeUtilities.IsVuforiaEnabled())
			{
				this.ForceFrameFormat(Image.PIXEL_FORMAT.RGBA8888, true);
			}
			this.mCameraActive = true;
			return true;
		}

		public override bool Stop()
		{
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				this.ForceFrameFormat(Image.PIXEL_FORMAT.RGBA8888, false);
			}
			if (this.StopCameraDevice() == 0)
			{
				return false;
			}
			this.mCameraActive = false;
			return true;
		}

		public override bool IsActive()
		{
			return this.mCameraActive;
		}

		public override CameraDevice.VideoModeData GetVideoMode()
		{
			if (this.mVideoModeDataNeedsUpdate)
			{
				this.mVideoModeData = this.GetVideoMode(this.mCameraDeviceMode);
				this.mVideoModeDataNeedsUpdate = false;
			}
			return this.mVideoModeData;
		}

		public override CameraDevice.VideoModeData GetVideoMode(CameraDevice.CameraDeviceMode mode)
		{
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				return this.WebCam.GetVideoMode();
			}
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CameraDevice.VideoModeData)));
			VuforiaWrapper.Instance.CameraDeviceGetVideoMode((int)mode, intPtr);
			CameraDevice.VideoModeData arg_4F_0 = (CameraDevice.VideoModeData)Marshal.PtrToStructure(intPtr, typeof(CameraDevice.VideoModeData));
			Marshal.FreeHGlobal(intPtr);
			return arg_4F_0;
		}

		public override bool SelectVideoMode(CameraDevice.CameraDeviceMode mode)
		{
			if (VuforiaWrapper.Instance.CameraDeviceSelectVideoMode((int)mode) == 0)
			{
				return false;
			}
			this.mCameraDeviceMode = mode;
			this.mHasCameraDeviceModeBeenSet = true;
			this.mVideoModeDataNeedsUpdate = true;
			return true;
		}

		public override bool GetSelectedVideoMode(out CameraDevice.CameraDeviceMode mode)
		{
			mode = this.mCameraDeviceMode;
			return this.mHasCameraDeviceModeBeenSet;
		}

		public override bool SetFlashTorchMode(bool on)
		{
			bool flag = VuforiaWrapper.Instance.CameraDeviceSetFlashTorchMode(on ? 1 : 0) != 0;
			Debug.Log("Toggle flash " + (on ? "ON" : "OFF") + " " + (flag ? "WORKED" : "FAILED"));
			return flag;
		}

		public override bool SetFocusMode(CameraDevice.FocusMode mode)
		{
			bool flag = VuforiaWrapper.Instance.CameraDeviceSetFocusMode((int)mode) != 0;
			Debug.Log("Requested Focus mode " + mode + (flag ? " successfully." : ".  Not supported on this device."));
			return flag;
		}

		public override bool SetFrameFormat(Image.PIXEL_FORMAT format, bool enabled)
		{
			if (enabled)
			{
				if (!this.mCameraImages.ContainsKey(format))
				{
					if (VuforiaWrapper.Instance.QcarSetFrameFormat((int)format, 1) == 0)
					{
						Debug.LogError("Failed to set frame format");
						return false;
					}
					Image image = new ImageImpl();
					image.PixelFormat = format;
					this.mCameraImages.Add(format, image);
					return true;
				}
			}
			else if (this.mCameraImages.ContainsKey(format) && !this.mForcedCameraFormats.Contains(format))
			{
				if (VuforiaWrapper.Instance.QcarSetFrameFormat((int)format, 0) == 0)
				{
					Debug.LogError("Failed to set frame format");
					return false;
				}
				return this.mCameraImages.Remove(format);
			}
			return true;
		}

		public override Image GetCameraImage(Image.PIXEL_FORMAT format)
		{
			if (this.mCameraImages.ContainsKey(format))
			{
				Image image = this.mCameraImages[format];
				if (image.IsValid())
				{
					return image;
				}
			}
			return null;
		}

		public override CameraDevice.CameraDirection GetCameraDirection()
		{
			return this.mActualCameraDirection;
		}

		public override bool GetSelectedCameraDirection(out CameraDevice.CameraDirection cameraDirection)
		{
			cameraDirection = this.mSelectedCameraDirection;
			return this.mHasCameraDeviceModeBeenSet;
		}

		public override Vector2 GetCameraFieldOfViewRads()
		{
			Vector2 zero = Vector2.zero;
			float[] array = new float[2];
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * array.Length);
			if (VuforiaWrapper.Instance.CameraDeviceGetCameraFieldOfViewRads(intPtr) == 1)
			{
				Marshal.Copy(intPtr, array, 0, array.Length);
				zero = new Vector2(array[0], array[1]);
			}
			Marshal.FreeHGlobal(intPtr);
			return zero;
		}

		public override IEnumerable<CameraDevice.CameraField> GetCameraFields()
		{
			List<CameraDevice.CameraField> list = new List<CameraDevice.CameraField>();
			int num = VuforiaWrapper.Instance.CameraDeviceGetNumCameraFields();
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CameraDeviceImpl.CameraFieldData)));
			for (int i = 0; i < num; i++)
			{
				VuforiaWrapper.Instance.CameraDeviceGetCameraField(intPtr, i);
				CameraDeviceImpl.CameraFieldData cameraFieldData = (CameraDeviceImpl.CameraFieldData)Marshal.PtrToStructure(intPtr, typeof(CameraDeviceImpl.CameraFieldData));
				list.Add(new CameraDevice.CameraField
				{
					Key = cameraFieldData.KeyValue,
					Type = (CameraDevice.CameraField.DataType)cameraFieldData.DataType
				});
			}
			Marshal.FreeHGlobal(intPtr);
			return list;
		}

		public override bool SetField(string key, string value)
		{
			return VuforiaWrapper.Instance.CameraDeviceSetFieldString(key, value) == 1;
		}

		public override bool SetField(string key, long value)
		{
			return VuforiaWrapper.Instance.CameraDeviceSetFieldInt64(key, value) == 1;
		}

		public override bool SetField(string key, float value)
		{
			return VuforiaWrapper.Instance.CameraDeviceSetFieldFloat(key, value) == 1;
		}

		public override bool SetField(string key, bool value)
		{
			return VuforiaWrapper.Instance.CameraDeviceSetFieldBool(key, value) == 1;
		}

		public override bool SetField(string key, CameraDevice.Int64Range value)
		{
			return VuforiaWrapper.Instance.CameraDeviceSetFieldInt64Range(key, value.fromInt, value.toInt) == 1;
		}

		public override bool GetField(string key, out string value)
		{
			int num = 256;
			StringBuilder stringBuilder = new StringBuilder(num);
			int arg_23_0 = VuforiaWrapper.Instance.CameraDeviceGetFieldString(key, stringBuilder, num);
			value = stringBuilder.ToString();
			return arg_23_0 == 1;
		}

		public override bool GetField(string key, out long value)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(long)));
			bool expr_24 = VuforiaWrapper.Instance.CameraDeviceGetFieldInt64(key, intPtr) == 1;
			if (expr_24)
			{
				value = (long)Marshal.ReadInt32(intPtr);
			}
			else
			{
				value = -1L;
			}
			Marshal.FreeHGlobal(intPtr);
			return expr_24;
		}

		public override bool GetField(string key, out float value)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)));
			bool expr_24 = VuforiaWrapper.Instance.CameraDeviceGetFieldFloat(key, intPtr) == 1;
			if (expr_24)
			{
				value = (float)Marshal.PtrToStructure(intPtr, typeof(float));
			}
			else
			{
				value = 0f;
			}
			Marshal.FreeHGlobal(intPtr);
			return expr_24;
		}

		public override bool GetField(string key, out bool value)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
			bool expr_24 = VuforiaWrapper.Instance.CameraDeviceGetFieldBool(key, intPtr) == 1;
			if (expr_24)
			{
				value = (Marshal.ReadInt32(intPtr) == 1);
			}
			else
			{
				value = false;
			}
			Marshal.FreeHGlobal(intPtr);
			return expr_24;
		}

		public override bool GetField(string key, out CameraDevice.Int64Range value)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CameraDevice.Int64Range)));
			bool expr_24 = VuforiaWrapper.Instance.CameraDeviceGetFieldInt64Range(key, intPtr) == 1;
			if (expr_24)
			{
				value = (CameraDevice.Int64Range)Marshal.PtrToStructure(intPtr, typeof(CameraDevice.Int64Range));
			}
			else
			{
				value = default(CameraDevice.Int64Range);
			}
			Marshal.FreeHGlobal(intPtr);
			return expr_24;
		}

		public Dictionary<Image.PIXEL_FORMAT, Image> GetAllImages()
		{
			return this.mCameraImages;
		}

		public bool IsDirty()
		{
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				return this.WebCam.IsRendererDirty() || this.mIsDirty;
			}
			return this.mIsDirty;
		}

		public void ResetDirtyFlag()
		{
			this.mIsDirty = false;
		}

		public CameraDeviceImpl()
		{
			this.mCameraImages = new Dictionary<Image.PIXEL_FORMAT, Image>();
			this.mForcedCameraFormats = new List<Image.PIXEL_FORMAT>();
		}

		private void ForceFrameFormat(Image.PIXEL_FORMAT format, bool enabled)
		{
			if (!enabled && this.mForcedCameraFormats.Contains(format))
			{
				this.mForcedCameraFormats.Remove(format);
			}
			bool flag = this.SetFrameFormat(format, enabled);
			if ((enabled & flag) && !this.mForcedCameraFormats.Contains(format))
			{
				this.mForcedCameraFormats.Add(format);
			}
		}

		private int InitCameraDevice(int camera)
		{
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				int result = 0;
				try
				{
					WebCamARController expr_0E = WebCamARController.Instance;
					expr_0E.InitCamera();
					CameraDeviceImpl.mWebCam = expr_0E.ImplementationClass;
					VuforiaRenderer.Vec2I resampledTextureSize = CameraDeviceImpl.mWebCam.ResampledTextureSize;
					if (resampledTextureSize.y != 0)
					{
						VuforiaWrapper.Instance.CameraDeviceSetCameraConfiguration(resampledTextureSize.x, resampledTextureSize.y);
					}
					result = 1;
				}
				catch (NullReferenceException arg_4B_0)
				{
					Debug.LogError(arg_4B_0.Message);
				}
				VuforiaWrapper.Instance.CameraDeviceInitCamera(camera);
				this.mCameraImages.Clear();
				return result;
			}
			return VuforiaWrapper.Instance.CameraDeviceInitCamera(camera);
		}

		private int DeinitCameraDevice()
		{
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				int result = 0;
				if (CameraDeviceImpl.mWebCam != null)
				{
					CameraDeviceImpl.mWebCam.StopCamera();
					result = 1;
				}
				VuforiaWrapper.Instance.CameraDeviceDeinitCamera();
				return result;
			}
			((VuforiaManagerImpl)VuforiaManager.Instance).SetStatesToDiscard();
			return VuforiaWrapper.Instance.CameraDeviceDeinitCamera();
		}

		private int StartCameraDevice()
		{
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				int result = 0;
				if (CameraDeviceImpl.mWebCam != null)
				{
					CameraDeviceImpl.mWebCam.StartCamera();
					result = 1;
				}
				VuforiaWrapper.Instance.CameraDeviceStartCamera();
				return result;
			}
			return VuforiaWrapper.Instance.CameraDeviceStartCamera();
		}

		private int StopCameraDevice()
		{
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				int result = 0;
				if (CameraDeviceImpl.mWebCam != null)
				{
					CameraDeviceImpl.mWebCam.StopCamera();
					result = 1;
				}
				VuforiaWrapper.Instance.CameraDeviceStopCamera();
				return result;
			}
			return VuforiaWrapper.Instance.CameraDeviceStopCamera();
		}
	}
}
