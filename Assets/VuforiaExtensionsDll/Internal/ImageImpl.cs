using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class ImageImpl : Image
	{
		private int mWidth;

		private int mHeight;

		private int mStride;

		private int mBufferWidth;

		private int mBufferHeight;

		private Image.PIXEL_FORMAT mPixelFormat;

		private byte[] mData;

		private IntPtr mUnmanagedData;

		private bool mDataSet;

		private Color32[] mPixel32;

		public override int Width
		{
			get
			{
				return this.mWidth;
			}
			set
			{
				this.mWidth = value;
			}
		}

		public override int Height
		{
			get
			{
				return this.mHeight;
			}
			set
			{
				this.mHeight = value;
			}
		}

		public override int Stride
		{
			get
			{
				return this.mStride;
			}
			set
			{
				this.mStride = value;
			}
		}

		public override int BufferWidth
		{
			get
			{
				return this.mBufferWidth;
			}
			set
			{
				this.mBufferWidth = value;
			}
		}

		public override int BufferHeight
		{
			get
			{
				return this.mBufferHeight;
			}
			set
			{
				this.mBufferHeight = value;
			}
		}

		public override Image.PIXEL_FORMAT PixelFormat
		{
			get
			{
				return this.mPixelFormat;
			}
			set
			{
				this.mPixelFormat = value;
			}
		}

		public override byte[] Pixels
		{
			get
			{
				return this.mData;
			}
			set
			{
				this.mData = value;
			}
		}

		public IntPtr UnmanagedData
		{
			get
			{
				return this.mUnmanagedData;
			}
			set
			{
				this.mUnmanagedData = value;
			}
		}

		public ImageImpl()
		{
			this.mWidth = 0;
			this.mHeight = 0;
			this.mStride = 0;
			this.mBufferWidth = 0;
			this.mBufferHeight = 0;
			this.mPixelFormat = Image.PIXEL_FORMAT.UNKNOWN_FORMAT;
			this.mData = null;
			this.mUnmanagedData = IntPtr.Zero;
			this.mDataSet = false;
			this.mPixel32 = new Color32[0];
		}

		~ImageImpl()
		{
			Marshal.FreeHGlobal(this.mUnmanagedData);
			this.mUnmanagedData = IntPtr.Zero;
		}

		public override bool IsValid()
		{
			return this.mWidth > 0 && this.mHeight > 0 && this.mStride > 0 && this.mBufferWidth > 0 && this.mBufferHeight > 0 && this.mData != null && this.mDataSet;
		}

		public override void CopyToTexture(Texture2D texture2D)
		{
			TextureFormat textureFormat = this.ConvertPixelFormat(this.mPixelFormat);
			if (texture2D.width != this.mWidth || texture2D.height != this.mHeight || textureFormat != texture2D.format)
			{
				texture2D.Resize(this.mWidth, this.mHeight, textureFormat, false);
			}
			int num = 1;
			Image.PIXEL_FORMAT pIXEL_FORMAT = this.mPixelFormat;
			if (pIXEL_FORMAT != Image.PIXEL_FORMAT.RGB565 && pIXEL_FORMAT != Image.PIXEL_FORMAT.RGB888)
			{
				if (pIXEL_FORMAT == Image.PIXEL_FORMAT.RGBA8888)
				{
					num = 4;
				}
			}
			else
			{
				num = 3;
			}
			Color[] pixels = texture2D.GetPixels();
			int num2 = 0;
			for (int i = 0; i < pixels.Length; i++)
			{
				for (int j = 0; j < num; j++)
				{
					pixels[i][j] = (float)this.mData[num2++] / 255f;
				}
				for (int k = num; k < 4; k++)
				{
					pixels[i][k] = pixels[i][k - 1];
				}
			}
			texture2D.SetPixels(pixels);
		}

		internal void CopyPixelsFromUnmanagedBuffer()
		{
			if (this.mData == null || this.mUnmanagedData == IntPtr.Zero)
			{
				Debug.LogError("Image: Cannot copy image image data.");
				return;
			}
			int length = VuforiaWrapper.Instance.QcarGetBufferSize(this.mBufferWidth, this.mBufferHeight, (int)this.mPixelFormat);
			Marshal.Copy(this.mUnmanagedData, this.mData, 0, length);
			this.mDataSet = true;
		}

		internal Color32[] GetPixels32()
		{
			int num = this.mBufferWidth * this.mBufferHeight;
			if (this.mPixel32.Length != num)
			{
				this.mPixel32 = new Color32[num];
			}
			GCHandle gCHandle = GCHandle.Alloc(this.mPixel32, GCHandleType.Pinned);
			IntPtr destination = gCHandle.AddrOfPinnedObject();
			Marshal.Copy(this.mData, 0, destination, this.mData.Length);
			gCHandle.Free();
			return this.mPixel32;
		}

		private TextureFormat ConvertPixelFormat(Image.PIXEL_FORMAT input)
		{
			Image.PIXEL_FORMAT pIXEL_FORMAT = this.mPixelFormat;
			if (pIXEL_FORMAT == Image.PIXEL_FORMAT.RGB565)
			{
				return TextureFormat.RGB565;
			}
			if (pIXEL_FORMAT == Image.PIXEL_FORMAT.RGB888)
			{
				return TextureFormat.RGB24;
			}
			if (pIXEL_FORMAT == Image.PIXEL_FORMAT.RGBA8888)
			{
				return TextureFormat.RGBA32;
			}
			return TextureFormat.Alpha8;
		}
	}
}
