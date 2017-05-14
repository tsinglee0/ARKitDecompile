using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	[CanEditMultipleObjects, CustomEditor(typeof(ImageTargetAbstractBehaviour), true)]
	public class ImageTargetEditor : Editor
	{
		private SerializedImageTarget mSerializedObject;

		internal static void UpdateAspectRatio(SerializedImageTarget it, Vector2 size)
		{
			it.AspectRatio = size[1] / size[0];
			using (List<ImageTargetAbstractBehaviour>.Enumerator enumerator = it.GetBehaviours().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ImageTargetEditor.UpdateMesh(enumerator.Current.gameObject, it.AspectRatio);
				}
			}
		}

		internal static void UpdateScale(SerializedImageTarget serializedObject, Vector2 size)
		{
			foreach (ImageTargetAbstractBehaviour current in serializedObject.GetBehaviours())
			{
				float num = current.GetSize()[0] / size[0];
				if (serializedObject.AspectRatio <= 1f)
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

		internal static void UpdateMaterial(SerializedImageTarget serializedObject)
		{
			Material material = serializedObject.GetMaterial();
			material = ImageTargetEditor.UpdateMaterial(serializedObject.GetDataSetName(), serializedObject.TrackableName, serializedObject.ImageTargetType, material);
			serializedObject.SetMaterial(material);
		}

		internal static Material UpdateMaterial(string dataSetName, string trackableName, ImageTargetType imageTargetType, Material oldMaterial)
		{
			Material material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Vuforia/Materials/UserDefinedTarget.mat");
			Material material2 = AssetDatabase.LoadAssetAtPath<Material>("Assets/Vuforia/Materials/CloudRecoTarget.mat");
			if (imageTargetType == ImageTargetType.USER_DEFINED)
			{
				return material;
			}
			if (imageTargetType == ImageTargetType.CLOUD_RECO)
			{
				return material2;
			}
			Material material3 = VuforiaUtilities.LoadReferenceMaterial();
			if (material3 == null)
			{
				return null;
			}
			Material material4 = oldMaterial;
			if (material4 == null || material4 == material3 || material4 == material || material4 == material2)
			{
				material4 = new Material(material3);
			}
			string text;
			if (!VuforiaUtilities.GetImagePathWithExtension(string.Concat(new string[]
			{
				"Assets/Editor/Vuforia/ImageTargetTextures/",
				dataSetName,
				"/",
				trackableName,
				"_scaled"
			}), out text))
			{
				VuforiaUtilities.GetImagePathWithExtension(string.Concat(new string[]
				{
					"Assets/Editor/QCAR/ImageTargetTextures/",
					dataSetName,
					"/",
					trackableName,
					"_scaled"
				}), out text);
			}
			Texture2D mainTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(text);
			material4.mainTexture = mainTexture;
			material4.mainTextureScale = new Vector2(1f, 1f);
			material4.name = trackableName + "Material";
			return material4;
		}

		public static void EditorConfigureTarget(ImageTargetAbstractBehaviour itb, SerializedImageTarget serializedObject)
		{
			if (itb == null)
			{
				Debug.LogError("ImageTargetAbstractBehaviour parameter is null !");
				return;
			}
			if (VuforiaUtilities.GetPrefabType(itb) == PrefabType.Prefab)
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
					ConfigData.ImageTargetData imageTargetData;
					ConfigDataManager.Instance.GetConfigData("--- EMPTY ---").GetImageTarget("--- EMPTY ---", out imageTargetData);
					serializedObject.DataSetPath = "--- EMPTY ---";
					serializedObject.TrackableName = "--- EMPTY ---";
					ImageTargetEditor.UpdateAspectRatio(serializedObject, imageTargetData.size);
					ImageTargetEditor.UpdateScale(serializedObject, imageTargetData.size);
					ImageTargetEditor.UpdateMaterial(serializedObject);
					serializedObject.InitializedInEditor = true;
				}
				else if (!EditorApplication.isPlaying)
				{
					ImageTargetEditor.CheckMesh(serializedObject);
				}
			}
		}

		public void OnEnable()
		{
			ImageTargetAbstractBehaviour arg_22_0 = (ImageTargetAbstractBehaviour)base.target;
			this.mSerializedObject = new SerializedImageTarget(base.serializedObject);
			ImageTargetEditor.EditorConfigureTarget(arg_22_0, this.mSerializedObject);
		}

		public override void OnInspectorGUI()
		{
			base.DrawDefaultInspector();
			VuforiaUtilities.DisableGuiForPrefab(base.target);
			using (this.mSerializedObject.Edit())
			{
				ImageTargetType imageTargetType = this.mSerializedObject.ImageTargetType;
				string[] array = new string[]
				{
					"Predefined",
					"User Defined",
					"Cloud Reco"
				};
				ImageTargetType[] array2 = new ImageTargetType[]
				{
					ImageTargetType.PREDEFINED,
					ImageTargetType.USER_DEFINED,
					ImageTargetType.CLOUD_RECO
				};
				this.mSerializedObject.ImageTargetType = array2[EditorGUILayout.Popup("Type", array2.ToList<ImageTargetType>().IndexOf(this.mSerializedObject.ImageTargetType), array, new GUILayoutOption[0])];
				bool flag = this.mSerializedObject.ImageTargetType != imageTargetType;
				if (flag)
				{
					this.mSerializedObject.AutoSetOccluderFromTargetSize = false;
				}
				if (this.mSerializedObject.ImageTargetType == ImageTargetType.PREDEFINED)
				{
					this.DrawPredefinedTargetInspectorUI(flag);
				}
				else if (this.mSerializedObject.ImageTargetType == ImageTargetType.USER_DEFINED)
				{
					this.DrawUserDefinedTargetInspectorUI(flag);
				}
				else
				{
					this.DrawCloudRecoTargetInspectorUI(flag);
				}
			}
		}

		private void OnSceneGUI()
		{
			if ((ImageTargetAbstractBehaviour)base.target != null)
			{
				if (this.mSerializedObject.ImageTargetType == ImageTargetType.PREDEFINED)
				{
					SmartTerrainInitializationTargetEditorExtension.DrawSceneGUIForInitializationTarget(this.mSerializedObject, false);
					return;
				}
				if (this.mSerializedObject.ImageTargetType == ImageTargetType.CLOUD_RECO)
				{
					SmartTerrainInitializationTargetEditorExtension.DrawSceneGUIForInitializationTarget(this.mSerializedObject, true);
				}
			}
		}

		private static void UpdateDataSetAppearance(SerializedImageTarget serializedObject)
		{
			ConfigData configData = ConfigDataManager.Instance.GetConfigData(serializedObject.GetDataSetName());
			if (configData.ImageTargetExists(serializedObject.TrackableName))
			{
				ConfigData.ImageTargetData imageTargetData;
				configData.GetImageTarget(serializedObject.TrackableName, out imageTargetData);
				ImageTargetEditor.UpdateAspectRatio(serializedObject, imageTargetData.size);
				ImageTargetEditor.UpdateScale(serializedObject, imageTargetData.size);
			}
			ImageTargetEditor.UpdateMaterial(serializedObject);
		}

		private static void CheckMesh(SerializedImageTarget serializedObject)
		{
			using (List<ImageTargetAbstractBehaviour>.Enumerator enumerator = serializedObject.GetBehaviours().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject = enumerator.Current.gameObject;
					MeshFilter component = gameObject.GetComponent<MeshFilter>();
					if ((component == null || component.sharedMesh == null) && !serializedObject.AspectRatioProperty.hasMultipleDifferentValues)
					{
						ImageTargetEditor.UpdateMesh(gameObject, serializedObject.AspectRatio);
					}
				}
			}
			if (!serializedObject.DataSetPathProperty.hasMultipleDifferentValues && !serializedObject.TrackableNameProperty.hasMultipleDifferentValues)
			{
				ImageTargetEditor.UpdateMaterial(serializedObject);
			}
		}

		private static void UpdateMesh(GameObject itObject, float aspectRatio)
		{
			MeshFilter meshFilter = itObject.GetComponent<MeshFilter>();
			if (!meshFilter)
			{
				meshFilter = itObject.AddComponent<MeshFilter>();
			}
			Vector3 vector;
			Vector3 vector2;
			Vector3 vector3;
			Vector3 vector4;
			if (aspectRatio <= 1f)
			{
				vector = new Vector3(-0.5f, 0f, -aspectRatio * 0.5f);
				vector2 = new Vector3(-0.5f, 0f, aspectRatio * 0.5f);
				vector3 = new Vector3(0.5f, 0f, -aspectRatio * 0.5f);
				vector4 = new Vector3(0.5f, 0f, aspectRatio * 0.5f);
			}
			else
			{
				float num = 1f / aspectRatio;
				vector = new Vector3(-num * 0.5f, 0f, -0.5f);
				vector2 = new Vector3(-num * 0.5f, 0f, 0.5f);
				vector3 = new Vector3(num * 0.5f, 0f, -0.5f);
				vector4 = new Vector3(num * 0.5f, 0f, 0.5f);
			}
			Mesh mesh = meshFilter.sharedMesh ?? new Mesh();
			mesh.vertices = new Vector3[]
			{
				vector,
				vector2,
				vector3,
				vector4
			};
			mesh.triangles = new int[]
			{
				0,
				1,
				2,
				2,
				1,
				3
			};
			mesh.normals = new Vector3[mesh.vertices.Length];
			mesh.uv = new Vector2[]
			{
				new Vector2(0f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 0f),
				new Vector2(1f, 1f)
			};
			mesh.RecalculateNormals();
			meshFilter.sharedMesh = mesh;
			if (!itObject.GetComponent<MeshRenderer>())
			{
				itObject.AddComponent<MeshRenderer>();
			}
			SceneManager.Instance.UnloadUnusedAssets();
		}

		private void DrawCloudRecoTargetInspectorUI(bool typeChanged)
		{
			if (typeChanged)
			{
				ConfigData.ImageTargetData imageTargetData = VuforiaUtilities.CreateDefaultImageTarget();
				this.mSerializedObject.TrackableName = string.Empty;
				ImageTargetEditor.UpdateAspectRatio(this.mSerializedObject, imageTargetData.size);
				this.mSerializedObject.SetMaterial(ImageTargetEditor.UpdateMaterial("", "", ImageTargetType.CLOUD_RECO, this.mSerializedObject.GetMaterial()));
			}
			EditorGUILayout.PropertyField(this.mSerializedObject.PreserveChildSizeProperty, new GUIContent("Preserve child size"), new GUILayoutOption[0]);
			bool expr_7B = SceneManager.Instance.SmartTerrainInitializationEnabled();
			if (expr_7B)
			{
				EditorGUILayout.HelpBox("Extended Tracking cannot be enabled at the same time as Smart Terrain.", MessageType.Info);
			}
			GUI.enabled = !expr_7B;
			EditorGUILayout.PropertyField(this.mSerializedObject.ExtendedTrackingProperty, new GUIContent("Enable Extended Tracking"), new GUILayoutOption[0]);
			GUI.enabled = true;
			SmartTerrainInitializationTargetEditorExtension.DrawInspectorForInitializationTarget(this.mSerializedObject, true);
		}

		private void DrawUserDefinedTargetInspectorUI(bool typeChanged)
		{
			if (typeChanged)
			{
				ConfigData.ImageTargetData imageTargetData = VuforiaUtilities.CreateDefaultImageTarget();
				this.mSerializedObject.TrackableName = string.Empty;
				ImageTargetEditor.UpdateAspectRatio(this.mSerializedObject, imageTargetData.size);
				this.mSerializedObject.SetMaterial(ImageTargetEditor.UpdateMaterial("", "", ImageTargetType.USER_DEFINED, this.mSerializedObject.GetMaterial()));
			}
			if (this.mSerializedObject.TrackableName.Length > 64)
			{
				EditorGUILayout.HelpBox("Target name must not exceed 64 character limit!", MessageType.Error);
			}
			EditorGUILayout.PropertyField(this.mSerializedObject.TrackableNameProperty, new GUIContent("Target Name"), new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.mSerializedObject.PreserveChildSizeProperty, new GUIContent("Preserve child size"), new GUILayoutOption[0]);
			bool expr_BB = SceneManager.Instance.SmartTerrainInitializationEnabled();
			if (expr_BB)
			{
				EditorGUILayout.HelpBox("Extended Tracking cannot be enabled at the same time as Smart Terrain.", MessageType.Info);
			}
			GUI.enabled = !expr_BB;
			EditorGUILayout.PropertyField(this.mSerializedObject.ExtendedTrackingProperty, new GUIContent("Enable Extended Tracking"), new GUILayoutOption[0]);
			GUI.enabled = true;
		}

		private static string[] GetTrackableNames(ConfigData dataSetData)
		{
			string[] array = new string[dataSetData.NumImageTargets];
			dataSetData.CopyImageTargetNames(array, 0);
			return array;
		}

		private void DrawPredefinedTargetInspectorUI(bool typeChanged)
		{
			if (ConfigDataManager.Instance.NumConfigDataObjects > 1)
			{
				if (VuforiaUtilities.DrawDatasetTrackableInspector(this.mSerializedObject, typeChanged, new Func<ConfigData, string[]>(ImageTargetEditor.GetTrackableNames), "Image Target") | typeChanged)
				{
					if (base.serializedObject.isEditingMultipleObjects)
					{
						this.mSerializedObject.SerializedObject.ApplyModifiedPropertiesWithoutUndo();
						UnityEngine.Object[] targetObjects = this.mSerializedObject.SerializedObject.targetObjects;
						for (int i = 0; i < targetObjects.Length; i++)
						{
							ImageTargetEditor.UpdateDataSetAppearance(new SerializedImageTarget(new SerializedObject(targetObjects[i])));
						}
					}
					else
					{
						ImageTargetEditor.UpdateDataSetAppearance(this.mSerializedObject);
					}
				}
				this.mSerializedObject.WidthProperty.FixApproximatelyEqualFloatValues();
				EditorGUILayout.PropertyField(this.mSerializedObject.WidthProperty, new GUIContent("Width"), new GUILayoutOption[0]);
				this.mSerializedObject.HeightProperty.FixApproximatelyEqualFloatValues();
				EditorGUILayout.PropertyField(this.mSerializedObject.HeightProperty, new GUIContent("Height"), new GUILayoutOption[0]);
				VuforiaUtilities.DrawTrackableOptions(this.mSerializedObject, true, true, true);
				return;
			}
			if (typeChanged)
			{
				ImageTargetEditor.UpdateMaterial(this.mSerializedObject);
			}
			VuforiaUtilities.DrawMissingTargetsButton();
		}
	}
}
