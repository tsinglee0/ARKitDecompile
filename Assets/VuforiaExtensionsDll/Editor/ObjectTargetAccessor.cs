using System;
using UnityEditor;

namespace Vuforia.EditorClasses
{
	internal class ObjectTargetAccessor : TrackableAccessor
	{
		private readonly SerializedObjectTarget mSerializedObject;

		public ObjectTargetAccessor(ObjectTargetAbstractBehaviour target)
		{
			this.mTarget = target;
			this.mSerializedObject = new SerializedObjectTarget(new SerializedObject(this.mTarget));
		}

		public override void ApplyDataSetProperties()
		{
			if (VuforiaUtilities.GetPrefabType(this.mTarget) == PrefabType.Prefab)
			{
				return;
			}
			using (this.mSerializedObject.Edit())
			{
				ConfigData.ObjectTargetData objectTargetData;
				if (this.TrackableInDataSet(this.mSerializedObject.TrackableName, this.mSerializedObject.GetDataSetName()))
				{
					ConfigDataManager.Instance.GetConfigData(this.mSerializedObject.GetDataSetName()).GetObjectTarget(this.mSerializedObject.TrackableName, out objectTargetData);
				}
				else
				{
					ConfigDataManager.Instance.GetConfigData("--- EMPTY ---").GetObjectTarget("--- EMPTY ---", out objectTargetData);
					this.mSerializedObject.DataSetPath = "--- EMPTY ---";
					this.mSerializedObject.TrackableName = "--- EMPTY ---";
				}
				ObjectTargetEditor.UpdateAspectRatio(this.mSerializedObject, objectTargetData.size);
				ObjectTargetEditor.UpdateScale(this.mSerializedObject, objectTargetData.size);
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
				ConfigData.ObjectTargetData objectTargetData;
				if (this.TrackableInDataSet(this.mSerializedObject.TrackableName, this.mSerializedObject.GetDataSetName()))
				{
					ConfigDataManager.Instance.GetConfigData(this.mSerializedObject.GetDataSetName()).GetObjectTarget(this.mSerializedObject.TrackableName, out objectTargetData);
				}
				else
				{
					ConfigDataManager.Instance.GetConfigData("--- EMPTY ---").GetObjectTarget("--- EMPTY ---", out objectTargetData);
					this.mSerializedObject.DataSetPath = "--- EMPTY ---";
					this.mSerializedObject.TrackableName = "--- EMPTY ---";
				}
				ObjectTargetEditor.UpdateBoundingBox(this.mSerializedObject, objectTargetData.bboxMin, objectTargetData.bboxMax);
				ObjectTargetEditor.UpdatePreviewImage(this.mSerializedObject, objectTargetData.targetID);
			}
		}

		private bool TrackableInDataSet(string trackableName, string dataSetName)
		{
			return ConfigDataManager.Instance.ConfigDataExists(dataSetName) && ConfigDataManager.Instance.GetConfigData(dataSetName).ObjectTargetExists(trackableName);
		}
	}
}
