using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Vuforia
{
	internal class ImageTargetImpl : ObjectTargetImpl, ImageTarget, ObjectTarget, ExtendedTrackable, Trackable
	{
		private readonly ImageTargetType mImageTargetType;

		private readonly Dictionary<int, VirtualButton> mVirtualButtons;

		public ImageTargetType ImageTargetType
		{
			get
			{
				return this.mImageTargetType;
			}
		}

		public ImageTargetImpl(string name, int id, ImageTargetType imageTargetType, DataSet dataSet) : base(name, id, dataSet)
		{
			this.mImageTargetType = imageTargetType;
			this.mVirtualButtons = new Dictionary<int, VirtualButton>();
			this.CreateVirtualButtonsFromNative();
		}

		public VirtualButton CreateVirtualButton(string name, RectangleData area)
		{
			VirtualButton expr_08 = this.CreateNewVirtualButtonInNative(name, area);
			if (expr_08 == null)
			{
				Debug.LogError("Could not create Virtual Button.");
				return expr_08;
			}
			Debug.Log("Created Virtual Button successfully.");
			return expr_08;
		}

		public VirtualButton GetVirtualButtonByName(string name)
		{
			foreach (VirtualButton current in this.mVirtualButtons.Values)
			{
				if (current.Name == name)
				{
					return current;
				}
			}
			return null;
		}

		public IEnumerable<VirtualButton> GetVirtualButtons()
		{
			return this.mVirtualButtons.Values;
		}

		public bool DestroyVirtualButton(VirtualButton vb)
		{
			bool result = false;
			ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
			if (tracker != null)
			{
				bool flag = false;
				foreach (DataSet current in tracker.GetActiveDataSets())
				{
					if (this.mDataSet == current)
					{
						flag = true;
					}
				}
				if (flag)
				{
					tracker.DeactivateDataSet(this.mDataSet);
				}
				if (this.UnregisterVirtualButtonInNative(vb))
				{
					Debug.Log("Unregistering virtual button successfully");
					result = true;
					this.mVirtualButtons.Remove(vb.ID);
				}
				else
				{
					Debug.LogError("Failed to unregister virtual button.");
				}
				if (flag)
				{
					tracker.ActivateDataSet(this.mDataSet);
				}
			}
			return result;
		}

		private VirtualButton CreateNewVirtualButtonInNative(string name, RectangleData rectangleData)
		{
			if (this.ImageTargetType != ImageTargetType.PREDEFINED)
			{
				Debug.LogError("DataSet.RegisterVirtualButton: virtual button '" + name + "' cannot be registered for a user defined target.");
				return null;
			}
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RectangleData)));
			Marshal.StructureToPtr(rectangleData, intPtr, false);
			bool arg_63_0 = VuforiaWrapper.Instance.ImageTargetCreateVirtualButton(this.mDataSet.DataSetPtr, base.Name, name, intPtr) != 0;
			VirtualButton virtualButton = null;
			if (arg_63_0)
			{
				int num = VuforiaWrapper.Instance.VirtualButtonGetId(this.mDataSet.DataSetPtr, base.Name, name);
				if (!this.mVirtualButtons.ContainsKey(num))
				{
					virtualButton = new VirtualButtonImpl(name, num, rectangleData, this, this.mDataSet);
					this.mVirtualButtons.Add(num, virtualButton);
				}
				else
				{
					virtualButton = this.mVirtualButtons[num];
				}
			}
			return virtualButton;
		}

		private bool UnregisterVirtualButtonInNative(VirtualButton vb)
		{
			int key = VuforiaWrapper.Instance.VirtualButtonGetId(this.mDataSet.DataSetPtr, base.Name, vb.Name);
			bool flag = false;
			if (VuforiaWrapper.Instance.ImageTargetDestroyVirtualButton(this.mDataSet.DataSetPtr, base.Name, vb.Name) != 0 && this.mVirtualButtons.Remove(key))
			{
				flag = true;
			}
			if (!flag)
			{
				Debug.LogError("UnregisterVirtualButton: Failed to destroy the Virtual Button.");
			}
			return flag;
		}

		private void CreateVirtualButtonsFromNative()
		{
			int num = VuforiaWrapper.Instance.ImageTargetGetNumVirtualButtons(this.mDataSet.DataSetPtr, base.Name);
			if (num > 0)
			{
				IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VuforiaManagerImpl.VirtualButtonData)) * num);
				IntPtr intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RectangleData)) * num);
				VuforiaWrapper.Instance.ImageTargetGetVirtualButtons(intPtr, intPtr2, num, this.mDataSet.DataSetPtr, base.Name);
				for (int i = 0; i < num; i++)
				{
					VuforiaManagerImpl.VirtualButtonData virtualButtonData = (VuforiaManagerImpl.VirtualButtonData)Marshal.PtrToStructure(new IntPtr(intPtr.ToInt64() + (long)(i * Marshal.SizeOf(typeof(VuforiaManagerImpl.VirtualButtonData)))), typeof(VuforiaManagerImpl.VirtualButtonData));
					if (!this.mVirtualButtons.ContainsKey(virtualButtonData.id))
					{
						RectangleData area = (RectangleData)Marshal.PtrToStructure(new IntPtr(intPtr2.ToInt64() + (long)(i * Marshal.SizeOf(typeof(RectangleData)))), typeof(RectangleData));
						int num2 = 128;
						StringBuilder stringBuilder = new StringBuilder(num2);
						if (VuforiaWrapper.Instance.ImageTargetGetVirtualButtonName(this.mDataSet.DataSetPtr, base.Name, i, stringBuilder, num2) == 0)
						{
							Debug.LogError("Failed to get virtual button name.");
						}
						else
						{
							VirtualButton value = new VirtualButtonImpl(stringBuilder.ToString(), virtualButtonData.id, area, this, this.mDataSet);
							this.mVirtualButtons.Add(virtualButtonData.id, value);
						}
					}
				}
				Marshal.FreeHGlobal(intPtr);
				Marshal.FreeHGlobal(intPtr2);
			}
		}
	}
}
