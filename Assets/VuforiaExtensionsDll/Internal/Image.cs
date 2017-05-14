using System;
using UnityEngine;

namespace Vuforia
{
	public abstract class Image
	{
		public enum PIXEL_FORMAT
		{
			UNKNOWN_FORMAT,
			RGB565,
			RGB888,
			GRAYSCALE = 4,
			YUV = 8,
			RGBA8888 = 16
		}

		public abstract int Width
		{
			get;
			set;
		}

		public abstract int Height
		{
			get;
			set;
		}

		public abstract int Stride
		{
			get;
			set;
		}

		public abstract int BufferWidth
		{
			get;
			set;
		}

		public abstract int BufferHeight
		{
			get;
			set;
		}

		public abstract Image.PIXEL_FORMAT PixelFormat
		{
			get;
			set;
		}

		public abstract byte[] Pixels
		{
			get;
			set;
		}

		public abstract bool IsValid();

		public abstract void CopyToTexture(Texture2D texture2D);
	}
}
