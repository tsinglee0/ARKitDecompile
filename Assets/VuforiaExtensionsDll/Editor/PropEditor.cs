using System;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	[CustomEditor(typeof(PropAbstractBehaviour), true)]
	public class PropEditor : Editor
	{
		private SerializedProp mSerializedObject;

		public static void EditorConfigureTarget(PropAbstractBehaviour prop, SerializedProp serializedObject)
		{
			if (prop == null)
			{
				Debug.LogError("PropAbstractBehaviour parameter is null !");
				return;
			}
			if (VuforiaUtilities.GetPrefabType(prop) == PrefabType.Prefab)
			{
				return;
			}
			if (!SceneManager.Instance.SceneInitialized)
			{
				SceneManager.Instance.InitScene();
			}
			using (serializedObject.Edit())
			{
				if (!EditorApplication.isPlaying)
				{
					serializedObject.InitializedInEditor = true;
				}
			}
		}

		public void OnEnable()
		{
			PropAbstractBehaviour arg_22_0 = (PropAbstractBehaviour)base.target;
			this.mSerializedObject = new SerializedProp(base.serializedObject);
			PropEditor.EditorConfigureTarget(arg_22_0, this.mSerializedObject);
		}

		public override void OnInspectorGUI()
		{
			base.DrawDefaultInspector();
			VuforiaUtilities.DisableGuiForPrefab(base.target);
			using (this.mSerializedObject.Edit())
			{
				EditorGUILayout.HelpBox("This is a prop template that will be duplicated at runtime for newly found smart terrain props. The DefaultSmartTerrainEventHandler will use the same template for every prop at runtime. You can implement a custom ISmartTerrainEventHandler to create a different behavior.", MessageType.Info);
				EditorGUILayout.HelpBox("The mesh filter, mesh collider and/or box collider selected below will be automatically updated with new revisions of the smart terrain prop. Set them to None to ignore updates.", MessageType.None);
				EditorGUILayout.PropertyField(this.mSerializedObject.MeshFilterToUpdateProperty, new GUIContent("MeshFilter to update"), new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(this.mSerializedObject.MeshColliderToUpdateProperty, new GUIContent("MeshCollider to update"), new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(this.mSerializedObject.BoxColliderToUpdateProperty, new GUIContent("BoxCollider to update"), new GUILayoutOption[0]);
			}
			if (GUI.changed)
			{
				SceneManager.Instance.SceneUpdated();
			}
		}
	}
}
