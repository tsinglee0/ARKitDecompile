using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class CylinderTargetImpl : ObjectTargetImpl, CylinderTarget, ObjectTarget, ExtendedTrackable, Trackable
	{
		private float mSideLength;

		private float mTopDiameter;

		private float mBottomDiameter;

		public CylinderTargetImpl(string name, int id, DataSet dataSet) : base(name, id, dataSet)
		{
			float[] array = new float[3];
			IntPtr intPtr = Marshal.AllocHGlobal(3 * Marshal.SizeOf(typeof(float)));
			VuforiaWrapper.Instance.CylinderTargetGetDimensions(this.mDataSet.DataSetPtr, base.Name, intPtr);
			Marshal.Copy(intPtr, array, 0, 3);
			Marshal.FreeHGlobal(intPtr);
			this.mSideLength = array[0];
			this.mTopDiameter = array[1];
			this.mBottomDiameter = array[2];
		}

		public override void SetSize(Vector3 size)
		{
			this.ScaleCylinder(size[0] / this.GetSize()[0]);
			base.SetSize(size);
		}

		public float GetSideLength()
		{
			return this.mSideLength;
		}

		public float GetTopDiameter()
		{
			return this.mTopDiameter;
		}

		public float GetBottomDiameter()
		{
			return this.mBottomDiameter;
		}

		public bool SetSideLength(float sideLength)
		{
			this.ScaleCylinder(sideLength / this.mSideLength);
			return VuforiaWrapper.Instance.CylinderTargetSetSideLength(this.mDataSet.DataSetPtr, base.Name, sideLength) == 1;
		}

		public bool SetTopDiameter(float topDiameter)
		{
			this.ScaleCylinder(topDiameter / this.mTopDiameter);
			return VuforiaWrapper.Instance.CylinderTargetSetTopDiameter(this.mDataSet.DataSetPtr, base.Name, topDiameter) == 1;
		}

		public bool SetBottomDiameter(float bottomDiameter)
		{
			this.ScaleCylinder(bottomDiameter / this.mBottomDiameter);
			return VuforiaWrapper.Instance.CylinderTargetSetBottomDiameter(this.mDataSet.DataSetPtr, base.Name, bottomDiameter) == 1;
		}

		private void ScaleCylinder(float scale)
		{
			this.mSize *= scale;
			this.mSideLength *= scale;
			this.mTopDiameter *= scale;
			this.mBottomDiameter *= scale;
		}
	}
}
