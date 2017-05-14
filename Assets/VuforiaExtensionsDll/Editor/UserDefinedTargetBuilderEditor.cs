using System;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	[CustomEditor(typeof(UserDefinedTargetBuildingAbstractBehaviour), true)]
	public class UserDefinedTargetBuilderEditor : Editor
	{
		private SerializedProperty mStopTrackerWhileScanning;

		private SerializedProperty mStartScanningAutomatically;

		private SerializedProperty mStopScanningWhenFinshedBuilding;

		private void OnEnable()
		{
			this.mStopTrackerWhileScanning = base.serializedObject.FindProperty("StopTrackerWhileScanning");
			this.mStartScanningAutomatically = base.serializedObject.FindProperty("StartScanningAutomatically");
			this.mStopScanningWhenFinshedBuilding = base.serializedObject.FindProperty("StopScanningWhenFinshedBuilding");
		}

		public override void OnInspectorGUI()
		{
			using (base.serializedObject.Edit())
			{
				EditorGUILayout.HelpBox("If this is enabled, the Target Builder will begin to automatically scan the frame for feature points on startup.", MessageType.None);
				EditorGUILayout.PropertyField(this.mStartScanningAutomatically, new GUIContent("Start scanning automatically"), new GUILayoutOption[0]);
				EditorGUILayout.HelpBox("Check this if you want to automatically disable the ObjectTracker while the Target Builder is scanning. Once scanning mode is stopped, the ObjectTracker will be enabled again.", MessageType.None);
				EditorGUILayout.PropertyField(this.mStopTrackerWhileScanning, new GUIContent("Stop tracker while scanning"), new GUILayoutOption[0]);
				EditorGUILayout.HelpBox("If this is enabled, scanning will be automatically stopped when a new target has been created.", MessageType.None);
				EditorGUILayout.PropertyField(this.mStopScanningWhenFinshedBuilding, new GUIContent("Stop scanning after creating target"), new GUILayoutOption[0]);
			}
		}

		public void OnSceneGUI()
		{
			Component arg_30_0 = (UserDefinedTargetBuildingAbstractBehaviour)base.target;
			GUIStyle expr_10 = new GUIStyle();
			expr_10.alignment = TextAnchor.LowerRight;
			expr_10.fontSize = 18;
			expr_10.normal.textColor = Color.white;
			GUIStyle gUIStyle = expr_10;
			Handles.Label(arg_30_0.transform.position, "User Defined\n      Target Builder", gUIStyle);
		}
	}
}
