using System;
using UnityEngine;

namespace Vuforia
{
	internal class MultiTargetImpl : ObjectTargetImpl, MultiTarget, ObjectTarget, ExtendedTrackable, Trackable
	{
		public MultiTargetImpl(string name, int id, DataSet dataSet) : base(name, id, dataSet)
		{
		}

		public override Vector3 GetSize()
		{
			Debug.LogError("Getting the size of multi targets is currently not supported.");
			return Vector3.zero;
		}

		public override float GetLargestSizeComponent()
		{
			return VuforiaWrapper.Instance.MultiTargetGetLargestSizeComponent(this.mDataSet.DataSetPtr, base.Name);
		}

		public override void SetSize(Vector3 size)
		{
			Debug.LogError("Setting the size of multi targets is currently not supported.");
		}
	}
}
