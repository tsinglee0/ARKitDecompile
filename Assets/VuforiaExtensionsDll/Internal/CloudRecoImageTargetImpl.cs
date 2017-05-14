using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
	internal class CloudRecoImageTargetImpl : TrackableImpl, ImageTarget, ObjectTarget, ExtendedTrackable, Trackable
	{
		private readonly Vector3 mSize;

		public ImageTargetType ImageTargetType
		{
			get
			{
				return ImageTargetType.CLOUD_RECO;
			}
		}

		public CloudRecoImageTargetImpl(string name, int id, Vector3 size) : base(name, id)
		{
			this.mSize = size;
		}

		public Vector3 GetSize()
		{
			return this.mSize;
		}

		public float GetLargestSizeComponent()
		{
			return Mathf.Max(Mathf.Max(this.mSize.x, this.mSize.y), this.mSize.z);
		}

		public void SetSize(Vector3 size)
		{
			Debug.LogError("Setting the size of cloud reco targets is currently not supported.");
		}

		public VirtualButton CreateVirtualButton(string name, RectangleData area)
		{
			Debug.LogError("Virtual buttons are currently not supported for cloud reco targets.");
			return null;
		}

		public VirtualButton GetVirtualButtonByName(string name)
		{
			Debug.LogError("Virtual buttons are currently not supported for cloud reco targets.");
			return null;
		}

		public IEnumerable<VirtualButton> GetVirtualButtons()
		{
			Debug.LogError("Virtual buttons are currently not supported for cloud reco targets.");
			return new List<VirtualButton>();
		}

		public bool DestroyVirtualButton(VirtualButton vb)
		{
			Debug.LogError("Virtual buttons are currently not supported for cloud reco targets.");
			return false;
		}

		public bool StartExtendedTracking()
		{
			return ((StateManagerImpl)TrackerManager.Instance.GetStateManager()).GetExtendedTrackingManager().StartExtendedTracking(IntPtr.Zero, base.ID);
		}

		public bool StopExtendedTracking()
		{
			return ((StateManagerImpl)TrackerManager.Instance.GetStateManager()).GetExtendedTrackingManager().StopExtendedTracking(IntPtr.Zero, base.ID);
		}
	}
}
