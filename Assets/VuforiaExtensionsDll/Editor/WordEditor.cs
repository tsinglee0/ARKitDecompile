using System;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	[CustomEditor(typeof(WordAbstractBehaviour), true)]
	public class WordEditor : Editor
	{
		private SerializedWord mSerializedObject;

		internal static bool CheckForDuplicates()
		{
			bool result = false;
			WordAbstractBehaviour[] array = (WordAbstractBehaviour[])UnityEngine.Object.FindObjectsOfType(typeof(WordAbstractBehaviour));
			for (int i = 0; i < array.Length; i++)
			{
				WordAbstractBehaviour wordAbstractBehaviour = array[i];
				for (int j = i + 1; j < array.Length; j++)
				{
					WordAbstractBehaviour wordAbstractBehaviour2 = array[j];
					if (wordAbstractBehaviour.IsTemplateMode && wordAbstractBehaviour2.IsTemplateMode)
					{
						Debug.LogWarning("Duplicate template word target found. Only one of the Trackables and its respective Augmentation will be selected for use at runtime - that selection is indeterminate her.");
						result = true;
					}
					else if (!wordAbstractBehaviour.IsTemplateMode && !wordAbstractBehaviour2.IsTemplateMode && wordAbstractBehaviour.SpecificWord == wordAbstractBehaviour2.SpecificWord)
					{
						Debug.LogWarning("Duplicate word target \"" + wordAbstractBehaviour.SpecificWord + "\"found. Only one of the Trackables and its respective Augmentation will be selected for use at runtime - that selection is indeterminate her.");
						result = true;
					}
				}
			}
			return result;
		}

		public static void EditorConfigureTarget(WordAbstractBehaviour wb, SerializedWord serializedObject)
		{
			if (wb == null)
			{
				Debug.LogError("WordAbstractBehaviour parameter is null !");
				return;
			}
			if (VuforiaUtilities.GetPrefabType(wb) == PrefabType.Prefab)
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
					serializedObject.Mode = WordTemplateMode.Template;
					serializedObject.SpecificWord = "Word";
					WordEditor.UpdateMesh(wb);
					serializedObject.InitializedInEditor = true;
				}
			}
		}

		public void OnEnable()
		{
			WordAbstractBehaviour arg_22_0 = (WordAbstractBehaviour)base.target;
			this.mSerializedObject = new SerializedWord(base.serializedObject);
			WordEditor.EditorConfigureTarget(arg_22_0, this.mSerializedObject);
		}

		public override void OnInspectorGUI()
		{
			base.DrawDefaultInspector();
			VuforiaUtilities.DisableGuiForPrefab(base.target);
			using (this.mSerializedObject.Edit())
			{
				int num = this.mSerializedObject.IsTemplateMode ? 0 : 1;
				string[] array = new string[]
				{
					"Word Template",
					"Predefined Word"
				};
				if (EditorGUILayout.Popup("Type", num, array, new GUILayoutOption[0]) == 0)
				{
					this.mSerializedObject.Mode = WordTemplateMode.Template;
				}
				else
				{
					this.mSerializedObject.Mode = WordTemplateMode.SpecificWord;
					EditorGUILayout.PropertyField(this.mSerializedObject.SpecificWordProperty, new GUIContent("Word to recognize"), new GUILayoutOption[0]);
				}
			}
			if (GUI.changed)
			{
				WordEditor.UpdateMesh((WordAbstractBehaviour)base.target);
				SceneManager.Instance.SceneUpdated();
			}
		}

		private static void UpdateMesh(WordAbstractBehaviour behaviour)
		{
			GameObject gameObject = behaviour.gameObject;
			Transform transform = gameObject.transform.FindChild("Text");
			GameObject gameObject2;
			if (!transform)
			{
				gameObject2 = new GameObject("Text");
				gameObject2.transform.parent = gameObject.transform;
			}
			else
			{
				gameObject2 = transform.gameObject;
			}
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.transform.localRotation = Quaternion.AngleAxis(90f, Vector3.right);
			gameObject2.transform.localPosition = Vector3.zero;
			TextMesh textMesh = gameObject2.GetComponent<TextMesh>();
			if (!textMesh)
			{
				textMesh = gameObject2.AddComponent<TextMesh>();
			}
			Font font = (Font)AssetDatabase.LoadAssetAtPath("Assets/Vuforia/Fonts/" + "SourceSansPro.ttf", typeof(Font));
			if (font != null)
			{
				textMesh.fontSize = 0;
				textMesh.font = font;
			}
			else
			{
				Debug.LogWarning("Standard font for Word-prefabs were not found. You might not be able to use it during runtime.");
				textMesh.font = Resources.Load("Arial", typeof(Font)) as Font;
				textMesh.fontSize = 36;
			}
			MeshRenderer meshRenderer = gameObject2.GetComponent<MeshRenderer>();
			if (!meshRenderer)
			{
				meshRenderer = gameObject2.AddComponent<MeshRenderer>();
			}
			Material material = meshRenderer.sharedMaterial;
			if (material == null || material == textMesh.font.material)
			{
				material = new Material(textMesh.font.material);
			}
			material.color = Color.black;
			material.shader = Shader.Find("Custom/Text3D");
			meshRenderer.sharedMaterial = material;
			textMesh.text = behaviour.IsTemplateMode ? "\"AnyWord\"" : behaviour.SpecificWord;
			Quaternion localRotation = gameObject.transform.localRotation;
			Vector3 localScale = gameObject.transform.localScale;
			Vector3 localPosition = gameObject.transform.localPosition;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
			Bounds boundsForAxisAlignedTextMesh = WordEditor.GetBoundsForAxisAlignedTextMesh(textMesh);
			WordEditor.UpdateRectangleMesh(gameObject, boundsForAxisAlignedTextMesh);
			WordEditor.UpdateRectangleMaterial(gameObject);
			gameObject.transform.localRotation = localRotation;
			gameObject.transform.localScale = localScale;
			gameObject.transform.localPosition = localPosition;
		}

		private static Bounds GetBoundsForAxisAlignedTextMesh(TextMesh textMesh)
		{
			textMesh.anchor = TextAnchor.UpperRight;
			Bounds bounds = textMesh.GetComponent<Renderer>().bounds;
			textMesh.anchor = TextAnchor.LowerLeft;
			Bounds bounds2 = textMesh.GetComponent<Renderer>().bounds;
			Vector3 vector = bounds2.min - bounds.min;
			Vector3 vector2 = new Vector3(bounds2.min.x, bounds2.min.z, 0f) + 0.5f * vector;
			return new Bounds(vector2, vector);
		}

		private static void UpdateRectangleMesh(GameObject gameObject, Bounds bounds)
		{
			MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
			if (!meshFilter)
			{
				meshFilter = gameObject.AddComponent<MeshFilter>();
			}
			Vector3[] array = new Vector3[]
			{
				new Vector3(bounds.min.x, bounds.min.y, bounds.min.z),
				new Vector3(bounds.min.x, bounds.max.y, bounds.max.z),
				new Vector3(bounds.max.x, bounds.min.y, bounds.min.z),
				new Vector3(bounds.max.x, bounds.max.y, bounds.max.z)
			};
			Vector3[] normals = new Vector3[array.Length];
			Vector2[] uv = new Vector2[]
			{
				new Vector2(0f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 0f),
				new Vector2(1f, 1f)
			};
			Mesh mesh = meshFilter.sharedMesh ?? new Mesh();
			mesh.vertices = array;
			mesh.normals = normals;
			mesh.uv = uv;
			mesh.triangles = new int[]
			{
				0,
				1,
				2,
				2,
				1,
				3
			};
			mesh.RecalculateNormals();
			meshFilter.sharedMesh = mesh;
			SceneManager.Instance.UnloadUnusedAssets();
		}

		private static void UpdateRectangleMaterial(GameObject gameObject)
		{
			MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
			if (!meshRenderer)
			{
				meshRenderer = gameObject.AddComponent<MeshRenderer>();
			}
			Material material = VuforiaUtilities.LoadReferenceMaterial();
			Material material2 = meshRenderer.sharedMaterial;
			if (material2 == null || material2 == material)
			{
				material2 = new Material(material);
			}
			material2.name = "Text";
			material2.shader = Shader.Find("Unlit/Texture");
			material2.SetColor("_Color", Color.white);
			meshRenderer.sharedMaterial = material2;
		}
	}
}
