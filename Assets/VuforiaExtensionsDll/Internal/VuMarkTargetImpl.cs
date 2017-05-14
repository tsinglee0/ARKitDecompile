using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class VuMarkTargetImpl : VuMarkTarget, ObjectTarget, ExtendedTrackable, Trackable
	{
		private readonly VuMarkTemplateImpl mVuMarkTemplate;

		private readonly InstanceIdImpl mInstanceId;

		private readonly int mTargetId;

		private Image mInstanceImage;

		private VuforiaManagerImpl.ImageHeaderData mInstanceImageHeader;

		public string Name
		{
			get
			{
				return this.mVuMarkTemplate.Name;
			}
		}

		public int ID
		{
			get
			{
				return this.mTargetId;
			}
		}

		public InstanceId InstanceId
		{
			get
			{
				return this.mInstanceId;
			}
		}

		public VuMarkTemplate Template
		{
			get
			{
				return this.mVuMarkTemplate;
			}
		}

		public Image InstanceImage
		{
			get
			{
				if (!VuforiaRuntimeUtilities.IsVuforiaEnabled())
				{
					return null;
				}
				if (this.mInstanceImage == null)
				{
					this.CreateInstanceImage();
				}
				return this.mInstanceImage;
			}
		}

		public VuMarkTargetImpl(int id, byte[] buffer, ulong numericValue, InstanceIdType dataType, uint dataLength, VuMarkTemplateImpl template)
		{
			this.mTargetId = id;
			this.mVuMarkTemplate = template;
			this.mInstanceId = new InstanceIdImpl(buffer, numericValue, dataType, dataLength);
		}

		public Vector3 GetSize()
		{
			return this.mVuMarkTemplate.GetSize();
		}

		public float GetLargestSizeComponent()
		{
			Vector3 size = this.GetSize();
			return Mathf.Max(Mathf.Max(size.x, size.y), size.z);
		}

		public void SetSize(Vector3 size)
		{
			Debug.LogError("Setting the size of VuMark instances is not supported.Please use the VuMarkTemplate to set the size instead.");
		}

		public bool StartExtendedTracking()
		{
			return false;
		}

		public bool StopExtendedTracking()
		{
			return false;
		}

		public bool ExtendedTrackingEnabled()
		{
			return false;
		}

		private int CallNativeGetInstanceImage()
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VuforiaManagerImpl.ImageHeaderData)));
			Marshal.StructureToPtr(this.mInstanceImageHeader, intPtr, false);
			int arg_59_0 = VuforiaWrapper.Instance.VuMarkTargetGetInstanceImage(this.ID, intPtr);
			this.mInstanceImageHeader = (VuforiaManagerImpl.ImageHeaderData)Marshal.PtrToStructure(intPtr, typeof(VuforiaManagerImpl.ImageHeaderData));
			Marshal.FreeHGlobal(intPtr);
			return arg_59_0;
		}

		private void CreateInstanceImage()
		{
			this.mInstanceImageHeader = default(VuforiaManagerImpl.ImageHeaderData);
			this.mInstanceImageHeader.width = 0;
			this.mInstanceImageHeader.height = 0;
			this.mInstanceImageHeader.format = 16;
			if (this.CallNativeGetInstanceImage() == 0)
			{
				Debug.LogWarning("Not able to create VuMark instance image.");
				return;
			}
			this.mInstanceImage = new ImageImpl();
			ImageImpl imageImpl = (ImageImpl)this.mInstanceImage;
			VuMarkTargetImpl.SetImageValues(this.mInstanceImageHeader, imageImpl);
			VuMarkTargetImpl.AllocateImage(imageImpl);
			this.mInstanceImageHeader.data = imageImpl.UnmanagedData;
			if (this.CallNativeGetInstanceImage() == 0)
			{
				Debug.LogWarning("Not able to create VuMark instance image.");
				return;
			}
			imageImpl.CopyPixelsFromUnmanagedBuffer();
			this.mInstanceImage = imageImpl;
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
			int num = VuforiaWrapper.Instance.QcarGetBufferSize(image.BufferWidth, image.BufferHeight, (int)image.PixelFormat);
			image.Pixels = new byte[num];
			Marshal.FreeHGlobal(image.UnmanagedData);
			image.UnmanagedData = Marshal.AllocHGlobal(num);
		}
	}
}
