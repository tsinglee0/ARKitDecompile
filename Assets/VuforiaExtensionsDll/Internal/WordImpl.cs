using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class WordImpl : TrackableImpl, Word, Trackable
	{
		private string mText;

		private Vector2 mSize;

		private Image mLetterMask;

		private VuforiaManagerImpl.ImageHeaderData mLetterImageHeader;

		private RectangleData[] mLetterBoundingBoxes;

		public string StringValue
		{
			get
			{
				return this.mText;
			}
		}

		public Vector2 Size
		{
			get
			{
				return this.mSize;
			}
		}

		public WordImpl(int id, string text, Vector2 size) : base(text, id)
		{
			this.mText = text;
			this.mSize = size;
		}

		public Image GetLetterMask()
		{
			if (!VuforiaRuntimeUtilities.IsVuforiaEnabled())
			{
				return null;
			}
			if (this.mLetterMask == null)
			{
				this.CreateLetterMask();
			}
			return this.mLetterMask;
		}

		public RectangleData[] GetLetterBoundingBoxes()
		{
			if (!VuforiaRuntimeUtilities.IsVuforiaEnabled())
			{
				return new RectangleData[0];
			}
			if (this.mLetterBoundingBoxes == null)
			{
				int length = this.mText.Length;
				this.mLetterBoundingBoxes = new RectangleData[length];
				IntPtr intPtr = Marshal.AllocHGlobal(length * Marshal.SizeOf(typeof(RectangleData)));
				VuforiaWrapper.Instance.WordGetLetterBoundingBoxes(base.ID, intPtr);
				IntPtr ptr = new IntPtr(intPtr.ToInt64());
				for (int i = 0; i < length; i++)
				{
					this.mLetterBoundingBoxes[i] = (RectangleData)Marshal.PtrToStructure(ptr, typeof(RectangleData));
					ptr = new IntPtr(ptr.ToInt64() + (long)Marshal.SizeOf(typeof(RectangleData)));
				}
				Marshal.FreeHGlobal(intPtr);
			}
			return this.mLetterBoundingBoxes;
		}

		private void InitImageHeader()
		{
			this.mLetterImageHeader = default(VuforiaManagerImpl.ImageHeaderData);
			this.mLetterImageHeader.width = (this.mLetterImageHeader.bufferWidth = (int)(this.Size.x + 1f));
			this.mLetterImageHeader.height = (this.mLetterImageHeader.bufferHeight = (int)(this.Size.y + 1f));
			this.mLetterImageHeader.format = 4;
			this.mLetterMask = new ImageImpl();
		}

		private void CreateLetterMask()
		{
			this.InitImageHeader();
			ImageImpl imageImpl = (ImageImpl)this.mLetterMask;
			WordImpl.SetImageValues(this.mLetterImageHeader, imageImpl);
			WordImpl.AllocateImage(imageImpl);
			this.mLetterImageHeader.data = imageImpl.UnmanagedData;
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VuforiaManagerImpl.ImageHeaderData)));
			Marshal.StructureToPtr(this.mLetterImageHeader, intPtr, false);
			VuforiaWrapper.Instance.WordGetLetterMask(base.ID, intPtr);
			this.mLetterImageHeader = (VuforiaManagerImpl.ImageHeaderData)Marshal.PtrToStructure(intPtr, typeof(VuforiaManagerImpl.ImageHeaderData));
			if (this.mLetterImageHeader.reallocate == 1)
			{
				Debug.LogWarning("image wasn't allocated correctly");
				return;
			}
			imageImpl.CopyPixelsFromUnmanagedBuffer();
			this.mLetterMask = imageImpl;
			Marshal.FreeHGlobal(intPtr);
		}

		private static void SetImageValues(VuforiaManagerImpl.ImageHeaderData imageHeader, ImageImpl image)
		{
			image.Width = imageHeader.width;
			image.Height = imageHeader.height;
			image.Stride = imageHeader.stride;
			image.BufferWidth = imageHeader.bufferWidth;
			image.BufferHeight = imageHeader.bufferHeight;
			image.PixelFormat = (Image.PIXEL_FORMAT)imageHeader.format;
		}

		private static void AllocateImage(ImageImpl image)
		{
			image.Pixels = new byte[VuforiaWrapper.Instance.QcarGetBufferSize(image.BufferWidth, image.BufferHeight, (int)image.PixelFormat)];
			Marshal.FreeHGlobal(image.UnmanagedData);
			image.UnmanagedData = Marshal.AllocHGlobal(VuforiaWrapper.Instance.QcarGetBufferSize(image.BufferWidth, image.BufferHeight, (int)image.PixelFormat));
		}
	}
}
