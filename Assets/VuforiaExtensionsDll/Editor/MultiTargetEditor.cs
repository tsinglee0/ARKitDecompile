using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	[CanEditMultipleObjects, CustomEditor(typeof(MultiTargetAbstractBehaviour), true)]
	public class MultiTargetEditor : Editor
	{
		private SerializedMultiTarget mSerializedObject;

		private static Mesh mPlaneMesh;

		internal static void UpdateParts(SerializedMultiTarget serializedObject, ConfigData.MultiTargetPartData[] prtConfigs)
		{
			foreach (MultiTargetAbstractBehaviour current in serializedObject.GetBehaviours())
			{
				Transform transform = current.transform.Find("ChildTargets");
				if (transform == null)
				{
					GameObject expr_38 = new GameObject();
					expr_38.name = "ChildTargets";
					expr_38.transform.parent = current.transform;
					transform = expr_38.transform;
				}
				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;
				transform.localScale = Vector3.one;
				Material maskMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Vuforia/Materials/DepthMask.mat");
				ConfigData configData = ConfigDataManager.Instance.GetConfigData(serializedObject.GetDataSetName());
				if (configData == null)
				{
					Debug.LogError("Could not update Multi Target parts. A data set with the given name does not exist.");
					UnityEngine.Object.DestroyImmediate(transform);
				}
				else
				{
					if (MultiTargetEditor.mPlaneMesh == null)
					{
						Mesh mesh = new Mesh();
						mesh.vertices = new Vector3[]
						{
							new Vector3(-0.5f, 0f, -0.5f),
							new Vector3(-0.5f, 0f, 0.5f),
							new Vector3(0.5f, 0f, -0.5f),
							new Vector3(0.5f, 0f, 0.5f)
						};
						mesh.uv = new Vector2[]
						{
							new Vector2(1f, 1f),
							new Vector2(1f, 0f),
							new Vector2(0f, 1f),
							new Vector2(0f, 0f)
						};
						mesh.normals = new Vector3[4];
						mesh.triangles = new int[]
						{
							0,
							1,
							2,
							2,
							1,
							3
						};
						MultiTargetEditor.mPlaneMesh = mesh;
						MultiTargetEditor.mPlaneMesh.RecalculateNormals();
					}
					List<GameObject> list = new List<GameObject>();
					for (int i = 0; i < transform.childCount; i++)
					{
						list.Add(transform.GetChild(i).gameObject);
					}
					int num = prtConfigs.Length;
					for (int j = 0; j < num; j++)
					{
						if (!configData.ImageTargetExists(prtConfigs[j].name))
						{
							Debug.LogError("No Image Target named " + prtConfigs[j].name);
						}
						else
						{
							ConfigData.ImageTargetData imageTargetData;
							configData.GetImageTarget(prtConfigs[j].name, out imageTargetData);
							GameObject gameObject = null;
							if (list.Count > 0)
							{
								gameObject = list[0];
								list.RemoveAt(0);
								if (gameObject.GetComponent<MeshRenderer>() == null || gameObject.GetComponent<MeshFilter>() == null || gameObject.GetComponent<MaskOutAbstractBehaviour>() == null)
								{
									UnityEngine.Object.DestroyImmediate(gameObject);
									gameObject = null;
								}
							}
							if (gameObject == null)
							{
								gameObject = new GameObject();
								gameObject.AddComponent<MeshRenderer>();
								gameObject.AddComponent<MeshFilter>().sharedMesh = MultiTargetEditor.mPlaneMesh;
								gameObject.transform.parent = transform.transform;
								MaskOutAbstractBehaviour maskOutAbstractBehaviour = BehaviourComponentFactory.Instance.AddMaskOutBehaviour(gameObject);
								if (maskOutAbstractBehaviour != null)
								{
									maskOutAbstractBehaviour.maskMaterial = maskMaterial;
								}
							}
							gameObject.name = prtConfigs[j].name;
							Vector2 size = imageTargetData.size;
							Vector3 localScale = new Vector3(size.x, 1f, size.y);
							gameObject.transform.localPosition = prtConfigs[j].translation;
							gameObject.transform.localRotation = prtConfigs[j].rotation;
							gameObject.transform.localScale = localScale;
							MultiTargetEditor.UpdateMaterial(serializedObject.GetDataSetName(), gameObject);
						}
					}
					using (List<GameObject>.Enumerator enumerator2 = list.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							UnityEngine.Object.DestroyImmediate(enumerator2.Current);
						}
					}
				}
			}
		}

		public static void EditorConfigureTarget(MultiTargetAbstractBehaviour mtb, SerializedMultiTarget serializedObject)
		{
			if (mtb == null)
			{
				Debug.LogError("MultiTargetAbstractBehaviour parameter is null !");
				return;
			}
			if (VuforiaUtilities.GetPrefabType(mtb) == PrefabType.Prefab)
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
					ConfigData.MultiTargetData multiTargetData;
					ConfigDataManager.Instance.GetConfigData("--- EMPTY ---").GetMultiTarget("--- EMPTY ---", out multiTargetData);
					serializedObject.DataSetPath = "--- EMPTY ---";
					serializedObject.TrackableName = "--- EMPTY ---";
					List<ConfigData.MultiTargetPartData> parts = multiTargetData.parts;
					MultiTargetEditor.UpdateParts(serializedObject, parts.ToArray());
					serializedObject.InitializedInEditor = true;
				}
				else if (!EditorApplication.isPlaying)
				{
					MultiTargetEditor.CheckMesh(mtb);
				}
			}
		}

		public void OnEnable()
		{
			MultiTargetAbstractBehaviour arg_22_0 = (MultiTargetAbstractBehaviour)base.target;
			this.mSerializedObject = new SerializedMultiTarget(base.serializedObject);
			MultiTargetEditor.EditorConfigureTarget(arg_22_0, this.mSerializedObject);
		}

		public void OnSceneGUI()
		{
			TrackableBehaviour trackableBehaviour = (TrackableBehaviour)base.target;
			if (trackableBehaviour.transform.localScale.x != 1f || trackableBehaviour.transform.localScale.y != 1f || trackableBehaviour.transform.localScale.z != 1f)
			{
				Debug.LogError("You cannot scale a Multi target in the editor. Please edit the config.xml file to scale this target.");
				trackableBehaviour.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			if ((MultiTargetAbstractBehaviour)trackableBehaviour != null)
			{
				SmartTerrainInitializationTargetEditorExtension.DrawSceneGUIForInitializationTarget(this.mSerializedObject, false);
			}
		}

		private static string[] GetTrackableNames(ConfigData dataSetData)
		{
			string[] array = new string[dataSetData.NumMultiTargets];
			dataSetData.CopyMultiTargetNames(array, 0);
			return array;
		}

		public override void OnInspectorGUI()
		{
			base.DrawDefaultInspector();
			VuforiaUtilities.DisableGuiForPrefab(base.target);
			if (ConfigDataManager.Instance.NumConfigDataObjects > 1)
			{
				bool flag = false;
				using (this.mSerializedObject.Edit())
				{
					flag = VuforiaUtilities.DrawDatasetTrackableInspector(this.mSerializedObject, false, new Func<ConfigData, string[]>(MultiTargetEditor.GetTrackableNames), "Multi Target");
					VuforiaUtilities.DrawTrackableOptions(this.mSerializedObject, false, true, true);
				}
				if (flag)
				{
					UnityEngine.Object[] targetObjects = this.mSerializedObject.SerializedObject.targetObjects;
					for (int i = 0; i < targetObjects.Length; i++)
					{
						TrackableAccessor trackableAccessor = AccessorFactory.Create((MultiTargetAbstractBehaviour)targetObjects[i]);
						if (trackableAccessor != null)
						{
							trackableAccessor.ApplyDataSetProperties();
						}
					}
					return;
				}
			}
			else
			{
				VuforiaUtilities.DrawMissingTargetsButton();
			}
		}

		private static void CheckMesh(MultiTargetAbstractBehaviour mtb)
		{
			bool flag = false;
			Transform transform = mtb.transform.Find("ChildTargets");
			if (transform == null)
			{
				flag = true;
			}
			else
			{
				for (int i = 0; i < transform.childCount; i++)
				{
					Transform expr_2B = transform.GetChild(i);
					MeshFilter component = expr_2B.GetComponent<MeshFilter>();
					MeshRenderer component2 = expr_2B.GetComponent<MeshRenderer>();
					if (component == null || component.sharedMesh == null || component2 == null || component2.sharedMaterials.Length == 0 || component2.sharedMaterials[0] == null)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				TrackableAccessor trackableAccessor = AccessorFactory.Create(mtb);
				if (trackableAccessor != null)
				{
					trackableAccessor.ApplyDataSetProperties();
				}
			}
		}

		private static void UpdateMaterial(string dataSetName, GameObject go)
		{
			Material material = VuforiaUtilities.LoadReferenceMaterial();
			if (material == null)
			{
				return;
			}
			string text = go.name + "_scaled";
			string text2;
			if (!VuforiaUtilities.GetImagePathWithExtension("Assets/Editor/Vuforia/ImageTargetTextures/" + dataSetName + "/" + text, out text2))
			{
				VuforiaUtilities.GetImagePathWithExtension("Assets/Editor/QCAR/ImageTargetTextures/" + dataSetName + "/" + text, out text2);
			}
			Material material2 = go.GetComponent<Renderer>().sharedMaterial;
			if (material2 == null || material2 == material)
			{
				material2 = new Material(material);
				go.GetComponent<Renderer>().sharedMaterial = material2;
			}
			Texture2D mainTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(text2);
			material2.mainTexture = mainTexture;
			material2.name = text + "Material";
			material2.mainTextureScale = new Vector2(-1f, -1f);
			SceneManager.Instance.UnloadUnusedAssets();
		}
	}
}
