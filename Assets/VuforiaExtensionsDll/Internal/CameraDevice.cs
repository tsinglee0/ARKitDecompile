using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	public abstract class CameraDevice
	{
		public enum CameraDeviceMode
		{
			MODE_DEFAULT = -1,
			MODE_OPTIMIZE_SPEED = -2,
			MODE_OPTIMIZE_QUALITY = -3
		}

		public enum FocusMode
		{
			FOCUS_MODE_NORMAL,
			FOCUS_MODE_TRIGGERAUTO,
			FOCUS_MODE_CONTINUOUSAUTO,
			FOCUS_MODE_INFINITY,
			FOCUS_MODE_MACRO
		}

		public enum CameraDirection
		{
			CAMERA_DEFAULT,
			CAMERA_BACK,
			CAMERA_FRONT
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct VideoModeData
		{
			public int width;

			public int height;

			public float frameRate;

			internal int unused;
		}

		public struct CameraField
		{
			public enum DataType
			{
				TypeString,
				TypeInt64,
				TypeFloat,
				TypeBool,
				TypeInt64Range,
				TypeUnknown
			}

			public CameraDevice.CameraField.DataType Type;

			public string Key;
		}

		public struct Int64Range
		{
			public long fromInt;

			public long toInt;
		}

		private static CameraDevice mInstance;

		public static CameraDevice Instance
		{
			get
			{
				if (CameraDevice.mInstance == null)
				{
					Type typeFromHandle = typeof(CameraDevice);
					lock (typeFromHandle)
					{
						if (CameraDevice.mInstance == null)
						{
							CameraDevice.mInstance = new CameraDeviceImpl();
						}
					}
				}
				return CameraDevice.mInstance;
			}
		}

		public abstract bool Init(CameraDevice.CameraDirection cameraDirection);

		public abstract bool Deinit();

		public abstract bool Start();

		public abstract bool Stop();

		public abstract bool IsActive();

		public abstract CameraDevice.VideoModeData GetVideoMode();

		public abstract CameraDevice.VideoModeData GetVideoMode(CameraDevice.CameraDeviceMode mode);

		public abstract bool SelectVideoMode(CameraDevice.CameraDeviceMode mode);

		public abstract bool GetSelectedVideoMode(out CameraDevice.CameraDeviceMode mode);

		public abstract bool SetFlashTorchMode(bool on);

		public abstract bool SetFocusMode(CameraDevice.FocusMode mode);

		public abstract bool SetFrameFormat(Image.PIXEL_FORMAT format, bool enabled);

		public abstract Image GetCameraImage(Image.PIXEL_FORMAT format);

		public abstract CameraDevice.CameraDirection GetCameraDirection();

		public abstract bool GetSelectedCameraDirection(out CameraDevice.CameraDirection cameraDirection);

		public abstract Vector2 GetCameraFieldOfViewRads();

		public abstract IEnumerable<CameraDevice.CameraField> GetCameraFields();

		public abstract bool SetField(string key, string value);

		public abstract bool SetField(string key, long value);

		public abstract bool SetField(string key, float value);

		public abstract bool SetField(string key, bool value);

		public abstract bool SetField(string key, CameraDevice.Int64Range value);

		public abstract bool GetField(string key, out string value);

		public abstract bool GetField(string key, out long value);

		public abstract bool GetField(string key, out float value);

		public abstract bool GetField(string key, out bool value);

		public abstract bool GetField(string key, out CameraDevice.Int64Range value);
	}
}
