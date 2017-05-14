using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	[CanEditMultipleObjects, CustomEditor(typeof(CylinderTargetAbstractBehaviour), true)]
	public class CylinderTargetEditor : Editor
	{
		private const int NUM_PERIMETER_VERTICES = 32;

		private const bool INSIDE_MATERIAL = true;

		private SerializedCylinderTarget mSerializedObject;

		internal static void UpdateAspectRatio(SerializedCylinderTarget serializedObject, ConfigData.CylinderTargetData ctConfig)
		{
			float num = ctConfig.topDiameter / ctConfig.sideLength;
			float num2 = ctConfig.bottomDiameter / ctConfig.sideLength;
			serializedObject.TopDiameterRatio = num;
			serializedObject.BottomDiameterRatio = num2;
			UnityEngine.Object[] targetObjects = serializedObject.SerializedObject.targetObjects;
			for (int i = 0; i < targetObjects.Length; i++)
			{
				CylinderTargetEditor.UpdateGeometry(((CylinderTargetAbstractBehaviour)targetObjects[i]).gameObject, 1f, num, num2, ctConfig.hasTopGeometry, ctConfig.hasBottomGeometry);
			}
			string arg_92_0 = serializedObject.GetDataSetName();
			string trackableName = serializedObject.TrackableName;
			Material[] material = CylinderTargetEditor.UpdateMaterials(arg_92_0, trackableName, ctConfig.hasBottomGeometry, ctConfig.hasTopGeometry, true, serializedObject.GetMaterials());
			serializedObject.SetMaterial(material);
		}

		internal static void UpdateScale(SerializedCylinderTarget serializedObject, float scale)
		{
			foreach (CylinderTargetAbstractBehaviour current in serializedObject.GetBehaviours())
			{
				float num = current.transform.localScale.x / scale;
				current.transform.localScale = new Vector3(scale, scale, scale);
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

		private static void CheckMesh(SerializedCylinderTarget serializedObject)
		{
			foreach (CylinderTargetAbstractBehaviour expr_15 in serializedObject.GetBehaviours())
			{
				MeshFilter component = expr_15.gameObject.GetComponent<MeshFilter>();
				MeshRenderer component2 = expr_15.gameObject.GetComponent<MeshRenderer>();
				if (component == null || component.sharedMesh == null || component2 == null || component2.sharedMaterials.Length == 0 || component2.sharedMaterials[0] == null)
				{
					ConfigData.CylinderTargetData ctConfig;
					ConfigDataManager.Instance.GetConfigData(serializedObject.GetDataSetName()).GetCylinderTarget(serializedObject.TrackableName, out ctConfig);
					CylinderTargetEditor.UpdateAspectRatio(serializedObject, ctConfig);
				}
			}
		}

		private static void UpdateGeometry(GameObject gameObject, float sideLength, float topDiameter, float bottomDiameter, bool hasTopGeometry, bool hasBottomGeometry)
		{
			if (!gameObject.GetComponent<MeshRenderer>())
			{
				gameObject.AddComponent<MeshRenderer>();
			}
			MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
			if (!meshFilter)
			{
				meshFilter = gameObject.AddComponent<MeshFilter>();
			}
			Mesh sharedMesh = CylinderMeshFactory.CreateCylinderMesh(meshFilter.sharedMesh, sideLength, topDiameter, bottomDiameter, 32, hasTopGeometry, hasBottomGeometry, true);
			meshFilter.sharedMesh = sharedMesh;
			if (!gameObject.GetComponent<MaskOutAbstractBehaviour>())
			{
				Material maskMaterial = (Material)AssetDatabase.LoadAssetAtPath("Assets/Vuforia/Materials/DepthMask.mat", typeof(Material));
				BehaviourComponentFactory.Instance.AddMaskOutBehaviour(gameObject).maskMaterial = maskMaterial;
			}
			SceneManager.Instance.UnloadUnusedAssets();
		}

		private static Material[] UpdateMaterials(string dataSetName, string trackableName, bool hasBottomGeometry, bool hasTopGeometry, bool insideMaterial, Material[] oldMaterials)
		{
			Material material = VuforiaUtilities.LoadReferenceMaterial();
			if (material == null)
			{
				return new Material[0];
			}
			string str = "Assets/Editor/Vuforia/CylinderTargetTextures/" + dataSetName + "/" + trackableName;
			if (!File.Exists(str + ".Body_scaled.png") && !File.Exists(str + ".Body_scaled.jpg") && !File.Exists(str + ".Body_scaled.jpeg"))
			{
				str = "Assets/Editor/QCAR/CylinderTargetTextures/" + dataSetName + "/" + trackableName;
			}
			List<string> list = new List<string>
			{
				str + ".Body_scaled"
			};
			if (hasBottomGeometry)
			{
				list.Add(str + ".Bottom_scaled");
			}
			if (hasTopGeometry)
			{
				list.Add(str + ".Top_scaled");
			}
			int count = list.Count;
			Material[] array = new Material[insideMaterial ? (count * 2) : count];
			for (int i = 0; i < list.Count; i++)
			{
				string text;
				VuforiaUtilities.GetImagePathWithExtension(list[i], out text);
				Material material2 = null;
				if (i < oldMaterials.Length)
				{
					material2 = oldMaterials[i];
				}
				if (material2 == null || material2 == material)
				{
					material2 = new Material(material);
				}
				Texture2D mainTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(text);
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
				material2.mainTexture = mainTexture;
				material2.name = fileNameWithoutExtension + "Material";
				material2.mainTextureScale = new Vector2(-1f, -1f);
				array[i] = material2;
				if (insideMaterial)
				{
					int num = i + count;
					Material material3 = null;
					if (num < oldMaterials.Length)
					{
						material3 = oldMaterials[num];
					}
					if (material3 == null || material3 == material)
					{
						material3 = new Material(material2);
					}
					material3.CopyPropertiesFromMaterial(material2);
					material3.name = material2.name + "Bright";
					material3.shader = Shader.Find("Custom/BrightTexture");
					array[num] = material3;
				}
			}
			return array;
		}

		public static void EditorConfigureTarget(CylinderTargetAbstractBehaviour ctb, SerializedCylinderTarget serializedObject)
		{
			if (ctb == null)
			{
				Debug.LogError("CylinderTargetAbstractBehaviour parameter is null !");
				return;
			}
			if (VuforiaUtilities.GetPrefabType(ctb) == PrefabType.Prefab)
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
					ConfigData.CylinderTargetData cylinderTargetData;
					ConfigDataManager.Instance.GetConfigData("--- EMPTY ---").GetCylinderTarget("--- EMPTY ---", out cylinderTargetData);
					serializedObject.DataSetPath = "--- EMPTY ---";
					serializedObject.TrackableName = "--- EMPTY ---";
					CylinderTargetEditor.UpdateAspectRatio(serializedObject, cylinderTargetData);
					CylinderTargetEditor.UpdateScale(serializedObject, cylinderTargetData.sideLength);
					serializedObject.InitializedInEditor = true;
				}
				else if (!EditorApplication.isPlaying)
				{
					CylinderTargetEditor.CheckMesh(serializedObject);
				}
			}
		}

		public void OnEnable()
		{
			CylinderTargetAbstractBehaviour arg_22_0 = (CylinderTargetAbstractBehaviour)base.target;
			this.mSerializedObject = new SerializedCylinderTarget(base.serializedObject);
			CylinderTargetEditor.EditorConfigureTarget(arg_22_0, this.mSerializedObject);
		}

		private static string[] GetTrackableNames(ConfigData dataSetData)
		{
			string[] array = new string[dataSetData.NumCylinderTargets];
			dataSetData.CopyCylinderTargetNames(array, 0);
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
					if (VuforiaUtilities.DrawDatasetTrackableInspector(this.mSerializedObject, false, new Func<ConfigData, string[]>(CylinderTargetEditor.GetTrackableNames), "Cylinder Target"))
					{
						ConfigData.CylinderTargetData cylinderTargetData;
						ConfigDataManager.Instance.GetConfigData(this.mSerializedObject.GetDataSetName()).GetCylinderTarget(this.mSerializedObject.TrackableName, out cylinderTargetData);
						CylinderTargetEditor.UpdateAspectRatio(this.mSerializedObject, cylinderTargetData);
						CylinderTargetEditor.UpdateScale(this.mSerializedObject, cylinderTargetData.sideLength);
					}
					EditorGUILayout.PropertyField(this.mSerializedObject.SideLengthProperty, new GUIContent("Side length"), new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.mSerializedObject.TopDiameterProperty, new GUIContent("Top diameter"), new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.mSerializedObject.BottomDiameterProperty, new GUIContent("Bottom diameter"), new GUILayoutOption[0]);
					VuforiaUtilities.DrawTrackableOptions(this.mSerializedObject, true, true, true);
					return;
				}
			}
			VuforiaUtilities.DrawMissingTargetsButton();
		}

		private void OnSceneGUI()
		{
			if ((CylinderTargetAbstractBehaviour)base.target != null)
			{
				SmartTerrainInitializationTargetEditorExtension.DrawSceneGUIForInitializationTarget(this.mSerializedObject, false);
			}
		}
	}
}
