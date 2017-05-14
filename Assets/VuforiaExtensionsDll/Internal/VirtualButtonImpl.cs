using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class VirtualButtonImpl : VirtualButton
	{
		private string mName;

		private int mID;

		private RectangleData mArea;

		private bool mIsEnabled;

		private ImageTarget mParentImageTarget;

		private DataSetImpl mParentDataSet;

		public override string Name
		{
			get
			{
				return this.mName;
			}
		}

		public override int ID
		{
			get
			{
				return this.mID;
			}
		}

		public override bool Enabled
		{
			get
			{
				return this.mIsEnabled;
			}
		}

		public override RectangleData Area
		{
			get
			{
				return this.mArea;
			}
		}

		public VirtualButtonImpl(string name, int id, RectangleData area, ImageTarget imageTarget, DataSet dataSet)
		{
			this.mName = name;
			this.mID = id;
			this.mArea = area;
			this.mIsEnabled = true;
			this.mParentImageTarget = imageTarget;
			this.mParentDataSet = (DataSetImpl)dataSet;
		}

		public override bool SetArea(RectangleData area)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RectangleData)));
			Marshal.StructureToPtr(area, intPtr, false);
			bool arg_4F_0 = VuforiaWrapper.Instance.VirtualButtonSetAreaRectangle(this.mParentDataSet.DataSetPtr, this.mParentImageTarget.Name, this.Name, intPtr) != 0;
			Marshal.FreeHGlobal(intPtr);
			if (!arg_4F_0)
			{
				Debug.LogError("Virtual Button area rectangle could not be set.");
				return false;
			}
			return true;
		}

		public override bool SetSensitivity(VirtualButton.Sensitivity sensitivity)
		{
			if (VuforiaWrapper.Instance.VirtualButtonSetSensitivity(this.mParentDataSet.DataSetPtr, this.mParentImageTarget.Name, this.mName, (int)sensitivity) == 0)
			{
				Debug.LogError("Virtual Button sensitivity could not be set.");
				return false;
			}
			return true;
		}

		public override bool SetEnabled(bool enabled)
		{
			if (VuforiaWrapper.Instance.VirtualButtonSetEnabled(this.mParentDataSet.DataSetPtr, this.mParentImageTarget.Name, this.mName, enabled ? 1 : 0) == 0)
			{
				Debug.LogError("Virtual Button enabled value could not be set.");
				return false;
			}
			this.mIsEnabled = enabled;
			return true;
		}
	}
}
