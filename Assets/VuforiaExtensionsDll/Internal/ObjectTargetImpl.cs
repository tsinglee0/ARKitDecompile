using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class ObjectTargetImpl : TrackableImpl, ObjectTarget, ExtendedTrackable, Trackable
	{
		protected Vector3 mSize;

		protected readonly DataSetImpl mDataSet;

		internal DataSetImpl DataSet
		{
			get
			{
				return this.mDataSet;
			}
		}

		public ObjectTargetImpl(string name, int id, DataSet dataSet) : base(name, id)
		{
			this.mDataSet = (DataSetImpl)dataSet;
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Vector3)));
			VuforiaWrapper.Instance.ObjectTargetGetSize(this.mDataSet.DataSetPtr, base.Name, intPtr);
			this.mSize = (Vector3)Marshal.PtrToStructure(intPtr, typeof(Vector3));
			Marshal.FreeHGlobal(intPtr);
		}

		public virtual Vector3 GetSize()
		{
			return this.mSize;
		}

		public virtual float GetLargestSizeComponent()
		{
			return Mathf.Max(Mathf.Max(this.mSize.x, this.mSize.y), this.mSize.z);
		}

		public virtual void SetSize(Vector3 size)
		{
			this.mSize = size;
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Vector3)));
			Marshal.StructureToPtr(this.mSize, intPtr, false);
			VuforiaWrapper.Instance.ObjectTargetSetSize(this.mDataSet.DataSetPtr, base.Name, intPtr);
			Marshal.FreeHGlobal(intPtr);
		}

		public virtual bool StartExtendedTracking()
		{
			return ((StateManagerImpl)TrackerManager.Instance.GetStateManager()).GetExtendedTrackingManager().StartExtendedTracking(this.mDataSet.DataSetPtr, base.ID);
		}

		public virtual bool StopExtendedTracking()
		{
			return ((StateManagerImpl)TrackerManager.Instance.GetStateManager()).GetExtendedTrackingManager().StopExtendedTracking(this.mDataSet.DataSetPtr, base.ID);
		}
	}
}
