using System;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	[CustomEditor(typeof(ReconstructionAbstractBehaviour), true)]
	public class ReconstructionEditor : Editor
	{
		private SerializedProperty mMaximumExtentEnabled;

		private SerializedProperty mMaximumExtent;

		private SerializedProperty mAutomaticStart;

		private SerializedProperty mNavMeshUpdates;

		private SerializedProperty mNavMeshPadding;

		public static void EditorConfigureTarget(ReconstructionAbstractBehaviour rb, SerializedObject serializedObject = null)
		{
			if (rb == null)
			{
				Debug.LogError("ReconstructionAbstractBehaviour parameter is null !");
				return;
			}
			if (VuforiaUtilities.GetPrefabType(rb) == PrefabType.Prefab)
			{
				return;
			}
			if (!SceneManager.Instance.SceneInitialized)
			{
				SceneManager.Instance.InitScene();
			}
			if (serializedObject == null)
			{
				serializedObject = new SerializedObject(rb);
			}
			if (!EditorApplication.isPlaying)
			{
				serializedObject.Update();
				SerializedProperty serializedProperty = serializedObject.FindProperty("mInitializedInEditor");
				if (!serializedProperty.boolValue)
				{
					Debug.Log("Reconstruction added to scene, enabling SmartTerrainTracker");
					VuforiaAbstractConfiguration expr_6F = VuforiaAbstractConfigurationEditor.LoadConfigurationObject();
					expr_6F.SmartTerrainTracker.AutoInitAndStartTracker = true;
					expr_6F.SmartTerrainTracker.AutoInitBuilder = true;
					using (serializedObject.Edit())
					{
						serializedObject.FindProperty("mMaximumExtentEnabled").boolValue = false;
						serializedProperty.boolValue = true;
					}
				}
			}
		}

		public void OnEnable()
		{
			this.mMaximumExtentEnabled = base.serializedObject.FindProperty("mMaximumExtentEnabled");
			this.mMaximumExtent = base.serializedObject.FindProperty("mMaximumExtent");
			this.mAutomaticStart = base.serializedObject.FindProperty("mAutomaticStart");
			this.mNavMeshUpdates = base.serializedObject.FindProperty("mNavMeshUpdates");
			this.mNavMeshPadding = base.serializedObject.FindProperty("mNavMeshPadding");
			ReconstructionEditor.EditorConfigureTarget((ReconstructionAbstractBehaviour)base.target, null);
		}

		public void OnSceneGUI()
		{
			if (!EditorApplication.isPlaying)
			{
				ReconstructionAbstractBehaviour reconstructionAbstractBehaviour = (ReconstructionAbstractBehaviour)base.target;
				if (reconstructionAbstractBehaviour.transform.localScale != Vector3.one)
				{
					Debug.LogWarning("You currently cannot scale the smart terrain object");
					reconstructionAbstractBehaviour.transform.localScale = Vector3.one;
				}
			}
		}

		public override void OnInspectorGUI()
		{
			base.DrawDefaultInspector();
			VuforiaUtilities.DisableGuiForPrefab(base.target);
			using (base.serializedObject.Edit())
			{
				bool expr_28 = SceneManager.Instance.ExtendedTrackingEnabledOnATarget();
				if (expr_28)
				{
					EditorGUILayout.HelpBox("Smart Terrain cannot be enabled at the same time as Extended Tracking.", MessageType.Info);
				}
				bool enabled = GUI.enabled;
				GUI.enabled = !expr_28;
				EditorGUILayout.PropertyField(this.mAutomaticStart, new GUIContent("Start Automatically"), new GUILayoutOption[0]);
				GUI.enabled = enabled;
				if (((ReconstructionAbstractBehaviour)base.target).GetComponent<ReconstructionFromTargetAbstractBehaviour>() != null)
				{
					EditorGUILayout.HelpBox("Set the checkbox below to create a navigation mesh for the primary surface. A padding around props can be defined below.", MessageType.None);
					EditorGUILayout.PropertyField(this.mNavMeshUpdates, new GUIContent("Create Nav Mesh"), new GUILayoutOption[0]);
					if (this.mNavMeshUpdates.boolValue)
					{
						EditorGUILayout.PropertyField(this.mNavMeshPadding, new GUIContent("Nav Mesh Padding"), new GUILayoutOption[0]);
					}
					EditorGUILayout.HelpBox("Define the maximum area of smart terrain with an axis-aligned rectangle around the smart terrain center", MessageType.None);
					EditorGUILayout.PropertyField(this.mMaximumExtentEnabled, new GUIContent("Define Max Primary Surface Area"), new GUILayoutOption[0]);
					if (this.mMaximumExtentEnabled.boolValue)
					{
						this.mMaximumExtent.rectValue = EditorGUILayout.RectField("Maximum Area", this.mMaximumExtent.rectValue, new GUILayoutOption[0]);
					}
				}
			}
			GUI.enabled = true;
		}
	}
}
