using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Vuforia
{
	internal class VuMarkTemplateImpl : ObjectTargetImpl, VuMarkTemplate, ObjectTarget, ExtendedTrackable, Trackable
	{
		private string mUserData;

		private Vector2 mOrigin;

		private bool mTrackingFromRuntimeAppearance;

		public string VuMarkUserData
		{
			get
			{
				if (this.mUserData == null)
				{
					StringBuilder stringBuilder = new StringBuilder(0);
					int num = VuforiaWrapper.Instance.VuMarkTemplateGetVuMarkUserData(this.mDataSet.DataSetPtr, base.Name, stringBuilder, 0u);
					if (num <= 0)
					{
						this.mUserData = "";
					}
					else
					{
						stringBuilder = new StringBuilder(num);
						VuforiaWrapper.Instance.VuMarkTemplateGetVuMarkUserData(this.mDataSet.DataSetPtr, base.Name, stringBuilder, (uint)num);
						this.mUserData = stringBuilder.ToString();
					}
				}
				return this.mUserData;
			}
		}

		public bool TrackingFromRuntimeAppearance
		{
			get
			{
				return this.mTrackingFromRuntimeAppearance;
			}
			set
			{
				if (VuforiaWrapper.Instance.VuMarkTemplateSetTrackingFromRuntimeAppearance(this.mDataSet.DataSetPtr, base.Name, value) == 1)
				{
					this.mTrackingFromRuntimeAppearance = value;
				}
			}
		}

		public virtual Vector2 Origin
		{
			get
			{
				return this.mOrigin;
			}
		}

		public VuMarkTemplateImpl(string name, int id, DataSet dataSet) : base(name, id, dataSet)
		{
		}

		public override void SetSize(Vector3 size)
		{
			base.SetSize(size);
			this.UpdateOrigin();
		}

		private void UpdateOrigin()
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Vector2)));
			if (VuforiaWrapper.Instance.VuMarkTemplateGetOrigin(this.mDataSet.DataSetPtr, base.Name, intPtr) == 0)
			{
				Debug.LogError(string.Format("Unable to retrieve the origin for {0}", base.Name));
				return;
			}
			this.mOrigin = (Vector2)Marshal.PtrToStructure(intPtr, typeof(Vector2));
			Marshal.FreeHGlobal(intPtr);
		}
	}
}
