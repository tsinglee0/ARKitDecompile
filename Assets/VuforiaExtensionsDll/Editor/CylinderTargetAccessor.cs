using System;
using UnityEditor;

namespace Vuforia.EditorClasses
{
	internal class CylinderTargetAccessor : TrackableAccessor
	{
		private readonly SerializedCylinderTarget mSerializedObject;

		public CylinderTargetAccessor(CylinderTargetAbstractBehaviour target)
		{
			this.mTarget = target;
			this.mSerializedObject = new SerializedCylinderTarget(new SerializedObject(this.mTarget));
		}

		public override void ApplyDataSetProperties()
		{
			if (VuforiaUtilities.GetPrefabType(this.mTarget) == PrefabType.Prefab)
			{
				return;
			}
			using (this.mSerializedObject.Edit())
			{
				ConfigData.CylinderTargetData cylinderTargetData;
				if (this.TrackableInDataSet(this.mSerializedObject.TrackableName, this.mSerializedObject.GetDataSetName()))
				{
					ConfigDataManager.Instance.GetConfigData(this.mSerializedObject.GetDataSetName()).GetCylinderTarget(this.mSerializedObject.TrackableName, out cylinderTargetData);
				}
				else
				{
					ConfigDataManager.Instance.GetConfigData("--- EMPTY ---").GetCylinderTarget("--- EMPTY ---", out cylinderTargetData);
					this.mSerializedObject.DataSetPath = "--- EMPTY ---";
					this.mSerializedObject.TrackableName = "--- EMPTY ---";
				}
				CylinderTargetEditor.UpdateScale(this.mSerializedObject, cylinderTargetData.sideLength);
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
				ConfigData.CylinderTargetData ctConfig;
				if (this.TrackableInDataSet(this.mSerializedObject.TrackableName, this.mSerializedObject.GetDataSetName()))
				{
					ConfigDataManager.Instance.GetConfigData(this.mSerializedObject.GetDataSetName()).GetCylinderTarget(this.mSerializedObject.TrackableName, out ctConfig);
				}
				else
				{
					ConfigDataManager.Instance.GetConfigData("--- EMPTY ---").GetCylinderTarget("--- EMPTY ---", out ctConfig);
					this.mSerializedObject.DataSetPath = "--- EMPTY ---";
					this.mSerializedObject.TrackableName = "--- EMPTY ---";
				}
				CylinderTargetEditor.UpdateAspectRatio(this.mSerializedObject, ctConfig);
			}
		}

		private bool TrackableInDataSet(string trackableName, string dataSetName)
		{
			return ConfigDataManager.Instance.ConfigDataExists(dataSetName) && ConfigDataManager.Instance.GetConfigData(dataSetName).CylinderTargetExists(trackableName);
		}
	}
}
