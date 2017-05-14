using System;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	internal class GenericVuforiaConfigurationEditor : ConfigurationEditor
	{
		private SerializedProperty mVuforiaLicenseKey;

		private SerializedProperty mDelayedInitialization;

		private SerializedProperty mCameraDeviceModeSetting;

		private SerializedProperty mMaxSimultaneousImageTargets;

		private SerializedProperty mMaxSimultaneousObjectTargets;

		private SerializedProperty mUseDelayedLoadingObjectTargets;

		private SerializedProperty mCameraDirection;

		private SerializedProperty mMirrorVideoBackground;

		private SerializedProperty mAutoInitDeviceTracker;

		private SerializedProperty mAutoStartDeviceTracker;

		public override string Title
		{
			get
			{
				return "Vuforia";
			}
		}

		public override void FindSerializedProperties(SerializedObject serializedObject)
		{
			this.mVuforiaLicenseKey = serializedObject.FindProperty("vuforia.vuforiaLicenseKey");
			this.mDelayedInitialization = serializedObject.FindProperty("vuforia.delayedInitialization");
			this.mCameraDeviceModeSetting = serializedObject.FindProperty("vuforia.cameraDeviceModeSetting");
			this.mMaxSimultaneousImageTargets = serializedObject.FindProperty("vuforia.maxSimultaneousImageTargets");
			this.mMaxSimultaneousObjectTargets = serializedObject.FindProperty("vuforia.maxSimultaneousObjectTargets");
			this.mUseDelayedLoadingObjectTargets = serializedObject.FindProperty("vuforia.useDelayedLoadingObjectTargets");
			this.mCameraDirection = serializedObject.FindProperty("vuforia.cameraDirection");
			this.mMirrorVideoBackground = serializedObject.FindProperty("vuforia.mirrorVideoBackground");
			this.mAutoInitDeviceTracker = serializedObject.FindProperty("deviceTracker.autoInitTracker");
			this.mAutoStartDeviceTracker = serializedObject.FindProperty("deviceTracker.autoStartTracker");
		}

		public override void DrawInspectorGUI()
		{
			EditorStyles.textField.wordWrap = true;
			EditorGUILayout.PropertyField(this.mVuforiaLicenseKey, new GUIContent("App License Key"), new GUILayoutOption[]
			{
				GUILayout.MinHeight(40f),
				GUILayout.MaxHeight(280f)
			});
			this.mVuforiaLicenseKey.stringValue = this.mVuforiaLicenseKey.stringValue.Replace(" ", "").Replace("\n", "").Replace("\r", "");
			EditorStyles.textField.wordWrap = false;
			EditorGUILayout.PropertyField(this.mDelayedInitialization, new GUIContent("Delayed Initialization"), new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.mCameraDeviceModeSetting, new GUIContent("Camera Device Mode"), new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.mMaxSimultaneousImageTargets, new GUIContent("Max Simultaneous Tracked Images"), new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.mMaxSimultaneousObjectTargets, new GUIContent("Max Simultaneous Tracked Objects"), new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.mUseDelayedLoadingObjectTargets, new GUIContent("Load Object Targets on Detection"), new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.mCameraDirection, new GUIContent("Camera Direction"), new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(this.mMirrorVideoBackground, new GUIContent("Mirror Video Background"), new GUILayoutOption[0]);
		}
	}
}
