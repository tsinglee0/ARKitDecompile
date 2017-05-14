using System;
using UnityEditor;

namespace Vuforia.EditorClasses
{
	internal class ImageTargetAccessor : TrackableAccessor
	{
		private readonly SerializedImageTarget mSerializedObject;

		public ImageTargetAccessor(ImageTargetAbstractBehaviour target)
		{
			this.mTarget = target;
			this.mSerializedObject = new SerializedImageTarget(new SerializedObject(this.mTarget));
		}

		public override void ApplyDataSetProperties()
		{
			if (VuforiaUtilities.GetPrefabType(this.mTarget) == PrefabType.Prefab)
			{
				return;
			}
			ImageTargetAbstractBehaviour it = (ImageTargetAbstractBehaviour)this.mTarget;
			ConfigData.ImageTargetData imageTargetData;
			using (this.mSerializedObject.Edit())
			{
				if (this.TrackableInDataSet(this.mSerializedObject.TrackableName, this.mSerializedObject.GetDataSetName()))
				{
					ConfigDataManager.Instance.GetConfigData(this.mSerializedObject.GetDataSetName()).GetImageTarget(this.mSerializedObject.TrackableName, out imageTargetData);
				}
				else if (this.mSerializedObject.ImageTargetType != ImageTargetType.PREDEFINED)
				{
					imageTargetData = VuforiaUtilities.CreateDefaultImageTarget();
				}
				else
				{
					ConfigDataManager.Instance.GetConfigData("--- EMPTY ---").GetImageTarget("--- EMPTY ---", out imageTargetData);
					this.mSerializedObject.DataSetPath = "--- EMPTY ---";
					this.mSerializedObject.TrackableName = "--- EMPTY ---";
				}
				ImageTargetEditor.UpdateScale(this.mSerializedObject, imageTargetData.size);
			}
			VirtualButtonEditor.UpdateVirtualButtons(it, imageTargetData.virtualButtons.ToArray());
		}

		public override void ApplyDataSetAppearance()
		{
			if (VuforiaUtilities.GetPrefabType(this.mTarget) == PrefabType.Prefab)
			{
				return;
			}
			using (this.mSerializedObject.Edit())
			{
				ConfigData.ImageTargetData imageTargetData;
				if (this.TrackableInDataSet(this.mSerializedObject.TrackableName, this.mSerializedObject.GetDataSetName()))
				{
					ConfigDataManager.Instance.GetConfigData(this.mSerializedObject.GetDataSetName()).GetImageTarget(this.mSerializedObject.TrackableName, out imageTargetData);
				}
				else if (this.mSerializedObject.ImageTargetType != ImageTargetType.PREDEFINED)
				{
					imageTargetData = VuforiaUtilities.CreateDefaultImageTarget();
				}
				else
				{
					ConfigDataManager.Instance.GetConfigData("--- EMPTY ---").GetImageTarget("--- EMPTY ---", out imageTargetData);
					this.mSerializedObject.DataSetPath = "--- EMPTY ---";
					this.mSerializedObject.TrackableName = "--- EMPTY ---";
				}
				ImageTargetEditor.UpdateAspectRatio(this.mSerializedObject, imageTargetData.size);
				ImageTargetEditor.UpdateMaterial(this.mSerializedObject);
			}
		}

		private bool TrackableInDataSet(string trackableName, string dataSetName)
		{
			return ConfigDataManager.Instance.ConfigDataExists(dataSetName) && ConfigDataManager.Instance.GetConfigData(dataSetName).ImageTargetExists(trackableName);
		}
	}
}
