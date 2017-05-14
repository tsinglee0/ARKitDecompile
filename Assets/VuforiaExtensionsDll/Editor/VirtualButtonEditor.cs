using System;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	[CustomEditor(typeof(VirtualButtonAbstractBehaviour), true)]
	public class VirtualButtonEditor : Editor
	{
		private SerializedProperty mName;

		private SerializedProperty mSensitivity;

		internal static void UpdateVirtualButtons(ImageTargetAbstractBehaviour it, ConfigData.VirtualButtonData[] vbs)
		{
			for (int i = 0; i < vbs.Length; i++)
			{
				VirtualButtonAbstractBehaviour[] componentsInChildren = it.GetComponentsInChildren<VirtualButtonAbstractBehaviour>();
				bool flag = false;
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					if (componentsInChildren[j].VirtualButtonName == vbs[i].name)
					{
						SerializedObject serializedObject = new SerializedObject(componentsInChildren[j]);
						using (serializedObject.Edit())
						{
							componentsInChildren[j].enabled = vbs[i].enabled;
							VirtualButtonEditor.GetSensitivityProperty(serializedObject).enumValueIndex = (int)vbs[i].sensitivity;
							componentsInChildren[j].SetPosAndScaleFromButtonArea(new Vector2(vbs[i].rectangle[0], vbs[i].rectangle[1]), new Vector2(vbs[i].rectangle[2], vbs[i].rectangle[3]));
						}
						flag = true;
					}
				}
				if (!flag)
				{
					VirtualButtonEditor.AddVirtualButton(it, vbs[i]);
				}
			}
		}

		internal static void AddVirtualButtons(ImageTargetAbstractBehaviour it, ConfigData.VirtualButtonData[] vbs)
		{
			for (int i = 0; i < vbs.Length; i++)
			{
				VirtualButtonEditor.AddVirtualButton(it, vbs[i]);
			}
		}

		internal static void Validate()
		{
			ImageTargetAbstractBehaviour[] array = UnityEngine.Object.FindObjectsOfType<ImageTargetAbstractBehaviour>();
			for (int i = 0; i < array.Length; i++)
			{
				VirtualButtonEditor.DetectDuplicates(array[i]);
			}
			VirtualButtonAbstractBehaviour[] array2 = UnityEngine.Object.FindObjectsOfType<VirtualButtonAbstractBehaviour>();
			for (int i = 0; i < array2.Length; i++)
			{
				VirtualButtonAbstractBehaviour virtualButtonAbstractBehaviour = array2[i];
				ImageTargetAbstractBehaviour imageTargetBehaviour = virtualButtonAbstractBehaviour.GetImageTargetBehaviour();
				if (imageTargetBehaviour == null)
				{
					Debug.LogError("Virtual Button '" + virtualButtonAbstractBehaviour.name + "' doesn't have an Image Target as an ancestor.");
				}
				else if (imageTargetBehaviour.ImageTargetType == ImageTargetType.USER_DEFINED)
				{
					Debug.LogError("Virtual Button '" + virtualButtonAbstractBehaviour.name + "' cannot be added to a user defined target.");
				}
			}
		}

		internal static bool CorrectPoses(VirtualButtonAbstractBehaviour[] vbs)
		{
			bool result = false;
			for (int i = 0; i < vbs.Length; i++)
			{
				VirtualButtonAbstractBehaviour virtualButtonAbstractBehaviour = vbs[i];
				if (virtualButtonAbstractBehaviour.PreviousTransform != virtualButtonAbstractBehaviour.transform.localToWorldMatrix || (virtualButtonAbstractBehaviour.transform.parent != null && virtualButtonAbstractBehaviour.PreviousParent != virtualButtonAbstractBehaviour.transform.parent.gameObject) || !virtualButtonAbstractBehaviour.HasUpdatedPose)
				{
					if (virtualButtonAbstractBehaviour.UpdatePose())
					{
						result = true;
					}
					virtualButtonAbstractBehaviour.PreviousTransform = virtualButtonAbstractBehaviour.transform.localToWorldMatrix;
					virtualButtonAbstractBehaviour.PreviousParent = (virtualButtonAbstractBehaviour.transform.parent ? virtualButtonAbstractBehaviour.transform.parent.gameObject : null);
				}
			}
			return result;
		}

		internal static void CreateVBMesh(GameObject vbObject)
		{
			MeshFilter meshFilter = vbObject.GetComponent<MeshFilter>();
			if (!meshFilter)
			{
				meshFilter = vbObject.AddComponent<MeshFilter>();
			}
			Vector3 vector = new Vector3(-0.5f, 0f, -0.5f);
			Vector3 vector2 = new Vector3(-0.5f, 0f, 0.5f);
			Vector3 vector3 = new Vector3(0.5f, 0f, -0.5f);
			Vector3 vector4 = new Vector3(0.5f, 0f, 0.5f);
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
			mesh.uv = new Vector2[]
			{
				new Vector2(0f, 0f),
				new Vector2(1f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 1f)
			};
			mesh.normals = new Vector3[mesh.vertices.Length];
			mesh.RecalculateNormals();
			mesh.name = "VBPlane";
			meshFilter.sharedMesh = mesh;
			MeshRenderer meshRenderer = vbObject.GetComponent<MeshRenderer>();
			if (!meshRenderer)
			{
				meshRenderer = vbObject.AddComponent<MeshRenderer>();
			}
			meshRenderer.sharedMaterial = (Material)AssetDatabase.LoadAssetAtPath("Assets/Vuforia/Editor/VirtualButtonTextures/VirtualButtonPreviewMaterial.mat", typeof(Material));
			SceneManager.Instance.UnloadUnusedAssets();
		}

		internal static void CreateMaterial(GameObject vbObject)
		{
			string text = "Assets/Vuforia/Editor/VirtualButtonTextures/VirtualButtonPreviewMaterial.mat";
			Material material = (Material)AssetDatabase.LoadAssetAtPath(text, typeof(Material));
			if (material == null)
			{
				Debug.LogError("Could not find reference material at " + text + " please reimport Unity package.");
				return;
			}
			vbObject.GetComponent<MeshRenderer>().sharedMaterial = material;
			SceneManager.Instance.UnloadUnusedAssets();
		}

		public static void EditorConfigureTarget(VirtualButtonAbstractBehaviour vb)
		{
			if (vb == null)
			{
				Debug.LogError("VirtualButtonAbstractBehaviour parameter is null !");
				return;
			}
			if (VuforiaUtilities.GetPrefabType(vb) == PrefabType.Prefab)
			{
				return;
			}
			if (!SceneManager.Instance.SceneInitialized)
			{
				SceneManager.Instance.InitScene();
			}
			if (!EditorApplication.isPlaying)
			{
				if (!vb.HasUpdatedPose)
				{
					vb.UpdatePose();
				}
				if (!VirtualButtonEditor.IsVBMeshCreated(vb))
				{
					VirtualButtonEditor.CreateVBMesh(vb.gameObject);
				}
				VirtualButtonEditor.Validate();
			}
		}

		public void OnEnable()
		{
			VirtualButtonAbstractBehaviour arg_2D_0 = (VirtualButtonAbstractBehaviour)base.target;
			this.mName = VirtualButtonEditor.GetNameProperty(base.serializedObject);
			this.mSensitivity = VirtualButtonEditor.GetSensitivityProperty(base.serializedObject);
			VirtualButtonEditor.EditorConfigureTarget(arg_2D_0);
		}

		public override void OnInspectorGUI()
		{
			base.DrawDefaultInspector();
			VuforiaUtilities.DisableGuiForPrefab(base.target);
			using (base.serializedObject.Edit())
			{
				EditorGUILayout.PropertyField(this.mName, new GUIContent("Name"), new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(this.mSensitivity, new GUIContent("Sensitivity Setting"), new GUILayoutOption[0]);
			}
			GUI.enabled = true;
		}

		public void OnSceneGUI()
		{
			VirtualButtonAbstractBehaviour virtualButtonAbstractBehaviour = (VirtualButtonAbstractBehaviour)base.target;
			virtualButtonAbstractBehaviour.transform.localScale = new Vector3(virtualButtonAbstractBehaviour.transform.localScale[0], 1f, virtualButtonAbstractBehaviour.transform.localScale[2]);
		}

		private static void DetectDuplicates(ImageTargetAbstractBehaviour it)
		{
			VirtualButtonAbstractBehaviour[] componentsInChildren = it.GetComponentsInChildren<VirtualButtonAbstractBehaviour>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				for (int j = i + 1; j < componentsInChildren.Length; j++)
				{
					if (componentsInChildren[i].VirtualButtonName == componentsInChildren[j].VirtualButtonName)
					{
						Debug.LogError(string.Concat(new string[]
						{
							"Duplicate virtual buttons with name '",
							componentsInChildren[i].VirtualButtonName,
							"' detected in Image Target '",
							it.TrackableName,
							"'."
						}));
					}
				}
			}
		}

		private static bool IsVBMeshCreated(VirtualButtonAbstractBehaviour vb)
		{
			GameObject expr_06 = vb.gameObject;
			MeshFilter component = expr_06.GetComponent<MeshFilter>();
			MeshRenderer component2 = expr_06.GetComponent<MeshRenderer>();
			return !(component == null) && !(component2 == null) && !(component.sharedMesh == null);
		}

		private static void AddVirtualButton(ImageTargetAbstractBehaviour it, ConfigData.VirtualButtonData vb)
		{
			VirtualButtonAbstractBehaviour virtualButtonAbstractBehaviour = it.CreateVirtualButton(vb.name, new Vector2(0f, 0f), new Vector2(1f, 1f));
			if (virtualButtonAbstractBehaviour != null)
			{
				SerializedObject serializedObject = new SerializedObject(virtualButtonAbstractBehaviour);
				using (serializedObject.Edit())
				{
					VirtualButtonEditor.GetSensitivityProperty(serializedObject).enumValueIndex = (int)vb.sensitivity;
				}
				virtualButtonAbstractBehaviour.SetPosAndScaleFromButtonArea(new Vector2(vb.rectangle[0], vb.rectangle[1]), new Vector2(vb.rectangle[2], vb.rectangle[3]));
				VirtualButtonEditor.CreateVBMesh(virtualButtonAbstractBehaviour.gameObject);
				VirtualButtonEditor.CreateMaterial(virtualButtonAbstractBehaviour.gameObject);
				virtualButtonAbstractBehaviour.enabled = vb.enabled;
				BehaviourComponentFactory.Instance.AddTurnOffBehaviour(virtualButtonAbstractBehaviour.gameObject);
				virtualButtonAbstractBehaviour.UpdatePose();
				return;
			}
			Debug.LogError("VirtualButton could not be added!");
		}

		private static SerializedProperty GetNameProperty(SerializedObject obj)
		{
			return obj.FindProperty("mName");
		}

		private static SerializedProperty GetSensitivityProperty(SerializedObject obj)
		{
			return obj.FindProperty("mSensitivity");
		}
	}
}
