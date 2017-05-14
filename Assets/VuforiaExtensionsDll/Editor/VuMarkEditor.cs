using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	[CanEditMultipleObjects, CustomEditor(typeof(VuMarkAbstractBehaviour), true)]
	public class VuMarkEditor : Editor
	{
		private SerializedVuMark mSerializedObject;

		internal static void UpdateAspectRatio(SerializedVuMark serializedObject, Vector2 size)
		{
			serializedObject.AspectRatio = size[1] / size[0];
			using (List<VuMarkAbstractBehaviour>.Enumerator enumerator = serializedObject.GetBehaviours().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					VuMarkEditor.UpdateMesh(enumerator.Current.gameObject, serializedObject.BoundingBox, serializedObject.Origin);
				}
			}
		}

		internal static void UpdateScale(SerializedVuMark serializedObject, Vector2 size)
		{
			foreach (VuMarkAbstractBehaviour current in serializedObject.GetBehaviours())
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

		internal static void UpdateMaterial(SerializedVuMark serializedObject)
		{
			Material material = serializedObject.GetMaterial();
			string previewImage = serializedObject.PreviewImage;
			string trackableName = serializedObject.TrackableName;
			Material material2 = VuforiaUtilities.LoadReferenceMaterial();
			if (material2 == null)
			{
				return;
			}
			Material material3 = material;
			if (material3 == null || material3 == material2)
			{
				material3 = new Material(material2);
			}
			Texture2D mainTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(previewImage);
			material3.mainTexture = mainTexture;
			material3.mainTextureScale = new Vector2(1f, 1f);
			material3.name = trackableName + "Material";
			serializedObject.SetMaterial(material3);
		}

		internal static void UpdateDataSetInfo(SerializedVuMark serializedObject, ConfigData.VuMarkData vuMarkInfo)
		{
			serializedObject.PreviewImage = "Assets/Editor/Vuforia/" + serializedObject.GetDataSetName() + "/" + vuMarkInfo.previewImage;
			serializedObject.IdType = vuMarkInfo.idType;
			serializedObject.IdLength = vuMarkInfo.idLength;
			Vector4 boundingBox2D = vuMarkInfo.boundingBox2D;
			serializedObject.BoundingBox = new Rect(boundingBox2D[0], boundingBox2D[1], boundingBox2D[2] - boundingBox2D[0], boundingBox2D[3] - boundingBox2D[1]);
			serializedObject.Origin = new Vector2(vuMarkInfo.origin.x, boundingBox2D[3] - vuMarkInfo.origin.y);
		}

		private static void CheckMesh(SerializedVuMark serializedObject)
		{
			using (List<VuMarkAbstractBehaviour>.Enumerator enumerator = serializedObject.GetBehaviours().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject = enumerator.Current.gameObject;
					MeshFilter component = gameObject.GetComponent<MeshFilter>();
					if (component == null || component.sharedMesh == null)
					{
						VuMarkEditor.UpdateMesh(gameObject, serializedObject.BoundingBox, serializedObject.Origin);
					}
				}
			}
			if (!serializedObject.PreviewImageProperty.hasMultipleDifferentValues)
			{
				VuMarkEditor.UpdateMaterial(serializedObject);
				return;
			}
			UnityEngine.Object[] targetObjects = serializedObject.SerializedObject.targetObjects;
			for (int i = 0; i < targetObjects.Length; i++)
			{
				VuMarkEditor.UpdateMaterial(new SerializedVuMark(new SerializedObject(targetObjects[i])));
			}
		}

		private static void UpdateMesh(GameObject gameObject, Rect boundingBox, Vector2 origin)
		{
			MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
			if (!meshFilter)
			{
				meshFilter = gameObject.AddComponent<MeshFilter>();
			}
			float num = 1f / Mathf.Max(boundingBox.width, boundingBox.height);
			if (float.IsInfinity(num) || float.IsNaN(num))
			{
				num = 1f;
			}
			Vector3 vector = new Vector3(-origin.x, 0f, -origin.y);
			Vector3 vector2 = num * (new Vector3(boundingBox.xMin, 0f, boundingBox.yMin) + vector);
			Vector3 vector3 = num * (new Vector3(boundingBox.xMin, 0f, boundingBox.yMax) + vector);
			Vector3 vector4 = num * (new Vector3(boundingBox.xMax, 0f, boundingBox.yMin) + vector);
			Vector3 vector5 = num * (new Vector3(boundingBox.xMax, 0f, boundingBox.yMax) + vector);
			Mesh mesh = meshFilter.sharedMesh ?? new Mesh();
			mesh.vertices = new Vector3[]
			{
				vector2,
				vector3,
				vector4,
				vector5
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
			mesh.uv = new Vector2[]
			{
				new Vector2(0f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 0f),
				new Vector2(1f, 1f)
			};
			mesh.RecalculateNormals();
			meshFilter.sharedMesh = mesh;
			if (!gameObject.GetComponent<MeshRenderer>())
			{
				gameObject.AddComponent<MeshRenderer>();
			}
			SceneManager.Instance.UnloadUnusedAssets();
		}

		public static void EditorConfigureTarget(VuMarkAbstractBehaviour vmb, SerializedVuMark serializedObject)
		{
			if (vmb == null)
			{
				Debug.LogError("VuMarkAbstractBehaviour parameter is null !");
				return;
			}
			if (VuforiaUtilities.GetPrefabType(vmb) == PrefabType.Prefab)
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
					ConfigData.VuMarkData vuMarkData;
					ConfigDataManager.Instance.GetConfigData("--- EMPTY ---").GetVuMarkTarget("--- EMPTY ---", out vuMarkData);
					serializedObject.DataSetPath = "--- EMPTY ---";
					serializedObject.TrackableName = "--- EMPTY ---";
					VuMarkEditor.UpdateDataSetInfo(serializedObject, vuMarkData);
					VuMarkEditor.UpdateAspectRatio(serializedObject, vuMarkData.size);
					VuMarkEditor.UpdateScale(serializedObject, vuMarkData.size);
					serializedObject.InitializedInEditor = true;
				}
			}
			if (!EditorApplication.isPlaying)
			{
				VuMarkEditor.CheckMesh(serializedObject);
			}
		}

		public void OnEnable()
		{
			VuMarkAbstractBehaviour arg_22_0 = (VuMarkAbstractBehaviour)base.target;
			this.mSerializedObject = new SerializedVuMark(base.serializedObject);
			VuMarkEditor.EditorConfigureTarget(arg_22_0, this.mSerializedObject);
		}

		private static string[] GetTrackableNames(ConfigData dataSetData)
		{
			string[] array = new string[dataSetData.NumVuMarkTargets];
			dataSetData.CopyVuMarkTargetNames(array, 0);
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
					if (VuforiaUtilities.DrawDatasetTrackableInspector(this.mSerializedObject, false, new Func<ConfigData, string[]>(VuMarkEditor.GetTrackableNames), "VuMark Template"))
					{
						ConfigData.VuMarkData vuMarkData;
						ConfigDataManager.Instance.GetConfigData(this.mSerializedObject.GetDataSetName()).GetVuMarkTarget(this.mSerializedObject.TrackableName, out vuMarkData);
						VuMarkEditor.UpdateDataSetInfo(this.mSerializedObject, vuMarkData);
						VuMarkEditor.UpdateAspectRatio(this.mSerializedObject, vuMarkData.size);
						VuMarkEditor.UpdateScale(this.mSerializedObject, vuMarkData.size);
						VuMarkEditor.UpdateMaterial(this.mSerializedObject);
					}
					this.mSerializedObject.WidthProperty.FixApproximatelyEqualFloatValues();
					EditorGUILayout.PropertyField(this.mSerializedObject.WidthProperty, new GUIContent("Width"), new GUILayoutOption[0]);
					this.mSerializedObject.HeightProperty.FixApproximatelyEqualFloatValues();
					EditorGUILayout.PropertyField(this.mSerializedObject.HeightProperty, new GUIContent("Height"), new GUILayoutOption[0]);
					EditorGUILayout.LabelField(new GUIContent("ID Type"), new GUIContent(this.mSerializedObject.IdType.ToString()), new GUILayoutOption[0]);
					EditorGUILayout.LabelField(new GUIContent("ID Length"), new GUIContent(this.mSerializedObject.IdLength.ToString()), new GUILayoutOption[0]);
					string text = "Define whether the VuMark has a changing background per instance,signaling to SDK how to track it. \nIf enabled, the SDK tracks this VuMark based on what is seen by the camera and not assume the template background image is useful for tracking because the background can change per instance. \nIf disabled, the SDK tracks this VuMark based on the template used to create the dataset. This is the default behavior.";
					EditorGUILayout.PropertyField(this.mSerializedObject.TrackingFromRuntimeAppearanceProperty, new GUIContent("Track From Appearance", text), new GUILayoutOption[0]);
					VuforiaUtilities.DrawTrackableOptions(this.mSerializedObject, true, true, false);
					return;
				}
			}
			VuforiaUtilities.DrawMissingTargetsButton();
		}
	}
}
