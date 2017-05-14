using System;
using System.Collections.Generic;
using UnityEditor;

namespace Vuforia.EditorClasses
{
	internal class MultiTargetAccessor : TrackableAccessor
	{
		private readonly SerializedMultiTarget mSerializedObject;

		public MultiTargetAccessor(MultiTargetAbstractBehaviour target)
		{
			this.mTarget = target;
			this.mSerializedObject = new SerializedMultiTarget(new SerializedObject(this.mTarget));
		}

		public override void ApplyDataSetProperties()
		{
			if (VuforiaUtilities.GetPrefabType(this.mTarget) == PrefabType.Prefab)
			{
				return;
			}
			using (this.mSerializedObject.Edit())
			{
				ConfigData.MultiTargetData multiTargetData;
				if (this.TrackableInDataSet(this.mSerializedObject.TrackableName, this.mSerializedObject.GetDataSetName()))
				{
					ConfigDataManager.Instance.GetConfigData(this.mSerializedObject.GetDataSetName()).GetMultiTarget(this.mSerializedObject.TrackableName, out multiTargetData);
				}
				else
				{
					ConfigDataManager.Instance.GetConfigData("--- EMPTY ---").GetMultiTarget("--- EMPTY ---", out multiTargetData);
					this.mSerializedObject.DataSetPath = "--- EMPTY ---";
					this.mSerializedObject.TrackableName = "--- EMPTY ---";
				}
				List<ConfigData.MultiTargetPartData> parts = multiTargetData.parts;
				MultiTargetEditor.UpdateParts(this.mSerializedObject, parts.ToArray());
			}
		}

		public override void ApplyDataSetAppearance()
		{
			this.ApplyDataSetProperties();
		}

		private bool TrackableInDataSet(string trackableName, string dataSetName)
		{
			return ConfigDataManager.Instance.ConfigDataExists(dataSetName) && ConfigDataManager.Instance.GetConfigData(dataSetName).MultiTargetExists(trackableName);
		}
	}
}
