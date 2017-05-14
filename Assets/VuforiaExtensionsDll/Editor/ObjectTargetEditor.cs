using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	[CanEditMultipleObjects, CustomEditor(typeof(ObjectTargetAbstractBehaviour), true)]
	public class ObjectTargetEditor : Editor
	{
		private SerializedObjectTarget mSerializedObject;

		internal static void UpdateAspectRatio(SerializedObjectTarget serializedObject, Vector3 size)
		{
			serializedObject.AspectRatioXY = size[1] / size[0];
			serializedObject.AspectRatioXZ = size[2] / size[0];
		}

		internal static void UpdateScale(SerializedObjectTarget serializedObject, Vector3 size)
		{
			foreach (ObjectTargetAbstractBehaviour current in serializedObject.GetBehaviours())
			{
				float num = current.GetSize()[0] / size[0];
				if (serializedObject.AspectRatioXY <= 1f)
				{
					current.transform.localScale = new Vector3(size[0], size[0], size[0]);
				}
				else
				{
					current.transform.localScale = new Vector3(size[1], size[1], size[1]);
				}
				if (serializedObject.PreserveChildSize)
				{
					foreach (Transform transform in current.transform)
					{
						transform.localPosition = new Vector3(transform.localPosition.x * num, transform.localPosition.y * num, transform.localPosition.z * num);
						transform.localScale = new Vector3(transform.localScale.x * num, transform.localScale.y * num, transform.localScale.z * num);
					}
				}
			}
		}

		internal static void UpdateBoundingBox(SerializedObjectTarget serializedObject, Vector3 bboxMin, Vector3 bboxMax)
		{
			serializedObject.BBoxMin = bboxMin;
			serializedObject.BBoxMax = bboxMax;
		}

		internal static void UpdatePreviewImage(SerializedObjectTarget serializedObject, string targetId)
		{
			if (serializedObject.GetDataSetName().Length > 3)
			{
				string text = serializedObject.GetDataSetName().Substring(0, serializedObject.GetDataSetName().Length - 3);
				string text2 = string.Concat(new string[]
				{
					"Assets/Editor/Vuforia/TargetsetData/",
					text,
					"/",
					targetId,
					"_preview.jpg"
				});
				if (!File.Exists(text2))
				{
					text2 = string.Concat(new string[]
					{
						"Assets/Editor/QCAR/TargetsetData/",
						text,
						"/",
						targetId,
						"_preview.jpg"
					});
				}
				Texture2D previewImage = (Texture2D)AssetDatabase.LoadAssetAtPath(text2, typeof(Texture2D));
				serializedObject.PreviewImage = previewImage;
				SceneManager.Instance.UnloadUnusedAssets();
			}
		}

		public static void EditorConfigureTarget(ObjectTargetAbstractBehaviour otb, SerializedObjectTarget serializedObject)
		{
			if (VuforiaUtilities.GetPrefabType(otb) == PrefabType.Prefab)
			{
				return;
			}
			if (!SceneManager.Instance.SceneInitialized)
			{
				SceneManager.Instance.InitScene();
			}
			using (serializedObject.Edit())
			{
				if (!serializedObject.InitializedInEditor && !EditorApplication.isPlaying)
				{
					ConfigData.ObjectTargetData objectTargetData;
					ConfigDataManager.Instance.GetConfigData("--- EMPTY ---").GetObjectTarget("--- EMPTY ---", out objectTargetData);
					ObjectTargetEditor.UpdateAspectRatio(serializedObject, objectTargetData.size);
					ObjectTargetEditor.UpdateScale(serializedObject, objectTargetData.size);
					ObjectTargetEditor.UpdateBoundingBox(serializedObject, objectTargetData.bboxMin, objectTargetData.bboxMax);
					ObjectTargetEditor.UpdatePreviewImage(serializedObject, objectTargetData.targetID);
					serializedObject.DataSetPath = "--- EMPTY ---";
					serializedObject.TrackableName = "--- EMPTY ---";
					serializedObject.InitializedInEditor = true;
				}
			}
		}

		public void OnEnable()
		{
			ObjectTargetAbstractBehaviour arg_22_0 = (ObjectTargetAbstractBehaviour)base.target;
			this.mSerializedObject = new SerializedObjectTarget(base.serializedObject);
			ObjectTargetEditor.EditorConfigureTarget(arg_22_0, this.mSerializedObject);
		}

		private static string[] GetTrackableNames(ConfigData dataSetData)
		{
			string[] array = new string[dataSetData.NumObjectTargets];
			dataSetData.CopyObjectTargetNames(array, 0);
			return array;
		}

		public override void OnInspectorGUI()
		{
			base.DrawDefaultInspector();
			VuforiaUtilities.DisableGuiForPrefab(base.target);
			if (ConfigDataManager.Instance.NumConfigDataObjects > 1)
			{
				using (this.mSerializedObject.Edit())
				{
					bool flag = VuforiaUtilities.DrawDatasetTrackableInspector(this.mSerializedObject, false, new Func<ConfigData, string[]>(ObjectTargetEditor.GetTrackableNames), "Object Target");
					this.mSerializedObject.LengthProperty.FixApproximatelyEqualFloatValues();
					EditorGUILayout.PropertyField(this.mSerializedObject.LengthProperty, new GUIContent("Length"), new GUILayoutOption[0]);
					this.mSerializedObject.WidthProperty.FixApproximatelyEqualFloatValues();
					EditorGUILayout.PropertyField(this.mSerializedObject.WidthProperty, new GUIContent("Width"), new GUILayoutOption[0]);
					this.mSerializedObject.HeightProperty.FixApproximatelyEqualFloatValues();
					EditorGUILayout.PropertyField(this.mSerializedObject.HeightProperty, new GUIContent("Height"), new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.mSerializedObject.ShowBoundingBoxProperty, new GUIContent("Show Bounding Box"), new GUILayoutOption[0]);
					VuforiaUtilities.DrawTrackableOptions(this.mSerializedObject, true, true, true);
					if (flag)
					{
						ConfigData.ObjectTargetData objectTargetData;
						ConfigDataManager.Instance.GetConfigData(this.mSerializedObject.GetDataSetName()).GetObjectTarget(this.mSerializedObject.TrackableName, out objectTargetData);
						ObjectTargetEditor.UpdateAspectRatio(this.mSerializedObject, objectTargetData.size);
						ObjectTargetEditor.UpdateBoundingBox(this.mSerializedObject, objectTargetData.bboxMin, objectTargetData.bboxMax);
						ObjectTargetEditor.UpdateScale(this.mSerializedObject, objectTargetData.size);
						ObjectTargetEditor.UpdatePreviewImage(this.mSerializedObject, objectTargetData.targetID);
					}
					if (this.mSerializedObject.PreviewImage)
					{
						GUILayout.Label(this.mSerializedObject.PreviewImage, new GUILayoutOption[]
						{
							GUILayout.Width(512f)
						});
					}
					return;
				}
			}
			VuforiaUtilities.DrawMissingTargetsButton();
		}

		private void OnSceneGUI()
		{
		}
	}
}
