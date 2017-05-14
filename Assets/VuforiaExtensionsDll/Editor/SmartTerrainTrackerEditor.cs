using System;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	internal class SmartTerrainTrackerEditor : ConfigurationEditor
	{
		private SerializedProperty mAutoInitTracker;

		private SerializedProperty mAutoStartTracker;

		private SerializedProperty mAutoInitBuilder;

		private SerializedProperty mSceneUnitsToMillimeter;

		public override string Title
		{
			get
			{
				return "Smart Terrain Tracker";
			}
		}

		public override void FindSerializedProperties(SerializedObject serializedObject)
		{
			this.mAutoInitTracker = serializedObject.FindProperty("smartTerrainTracker.autoInitTracker");
			this.mAutoStartTracker = serializedObject.FindProperty("smartTerrainTracker.autoStartTracker");
			this.mAutoInitBuilder = serializedObject.FindProperty("smartTerrainTracker.autoInitBuilder");
			this.mSceneUnitsToMillimeter = serializedObject.FindProperty("smartTerrainTracker.sceneUnitsToMillimeter");
		}

		public override void DrawInspectorGUI()
		{
			bool expr_0A = SceneManager.Instance.ExtendedTrackingEnabledOnATarget();
			if (expr_0A)
			{
				EditorGUILayout.HelpBox("Smart Terrain cannot be enabled at the same time as Extended Tracking.", MessageType.Info);
			}
			GUI.enabled = !expr_0A;
			EditorGUILayout.PropertyField(this.mAutoInitTracker, new GUIContent("Start Automatically"), new GUILayoutOption[0]);
			this.mAutoStartTracker.boolValue = this.mAutoInitTracker.boolValue;
			this.mAutoInitBuilder.boolValue = this.mAutoInitTracker.boolValue;
			if (this.mAutoInitTracker.boolValue)
			{
				EditorGUILayout.HelpBox("Enter a scale factor that defines how a scene unit needs to be scaled to be in real world millimeters.\nE.g. if 1 scene unit should be 100mm in the real word, set this scale value to 100.0", MessageType.None);
				EditorGUILayout.PropertyField(this.mSceneUnitsToMillimeter, new GUIContent("Scene unit in mm"), new GUILayoutOption[0]);
			}
			GUI.enabled = true;
		}
	}
}
