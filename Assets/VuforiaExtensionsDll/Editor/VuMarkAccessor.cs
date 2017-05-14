using System;
using UnityEditor;

namespace Vuforia.EditorClasses
{
	internal class VuMarkAccessor : TrackableAccessor
	{
		private readonly SerializedVuMark mSerializedObject;

		public VuMarkAccessor(VuMarkAbstractBehaviour target)
		{
			this.mTarget = target;
			this.mSerializedObject = new SerializedVuMark(new SerializedObject(target));
		}

		public override void ApplyDataSetProperties()
		{
			if (VuforiaUtilities.GetPrefabType(this.mTarget) == PrefabType.Prefab)
			{
				return;
			}
			using (this.mSerializedObject.Edit())
			{
				ConfigData.VuMarkData vuMarkData;
				if (this.TrackableInDataSet(this.mSerializedObject.TrackableName, this.mSerializedObject.GetDataSetName()))
				{
					ConfigDataManager.Instance.GetConfigData(this.mSerializedObject.GetDataSetName()).GetVuMarkTarget(this.mSerializedObject.TrackableName, out vuMarkData);
				}
				else
				{
					ConfigDataManager.Instance.GetConfigData("--- EMPTY ---").GetVuMarkTarget("--- EMPTY ---", out vuMarkData);
					this.mSerializedObject.DataSetPath = "--- EMPTY ---";
					this.mSerializedObject.TrackableName = "--- EMPTY ---";
				}
				VuMarkEditor.UpdateDataSetInfo(this.mSerializedObject, vuMarkData);
				VuMarkEditor.UpdateScale(this.mSerializedObject, vuMarkData.size);
			}
		}

		public override void ApplyDataSetAppearance()
		{
			if (VuforiaUtilities.GetPrefabType(this.mTarget) == PrefabType.Prefab)
			{
				return;
			}
			using (this.mSerializedObject.Edit())
			{
				ConfigData.VuMarkData vuMarkData;
				if (this.TrackableInDataSet(this.mSerializedObject.TrackableName, this.mSerializedObject.GetDataSetName()))
				{
					ConfigDataManager.Instance.GetConfigData(this.mSerializedObject.GetDataSetName()).GetVuMarkTarget(this.mSerializedObject.TrackableName, out vuMarkData);
				}
				else
				{
					ConfigDataManager.Instance.GetConfigData("--- EMPTY ---").GetVuMarkTarget("--- EMPTY ---", out vuMarkData);
					this.mSerializedObject.DataSetPath = "--- EMPTY ---";
					this.mSerializedObject.TrackableName = "--- EMPTY ---";
				}
				VuMarkEditor.UpdateDataSetInfo(this.mSerializedObject, vuMarkData);
				VuMarkEditor.UpdateAspectRatio(this.mSerializedObject, vuMarkData.size);
				VuMarkEditor.UpdateMaterial(this.mSerializedObject);
			}
		}

		private bool TrackableInDataSet(string trackableName, string dataSetName)
		{
			return ConfigDataManager.Instance.ConfigDataExists(dataSetName) && ConfigDataManager.Instance.GetConfigData(dataSetName).VuMarkTargetExists(trackableName);
		}
	}
}
