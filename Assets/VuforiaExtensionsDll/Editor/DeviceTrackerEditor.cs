using System;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	internal class DeviceTrackerEditor : ConfigurationEditor
	{
		private SerializedProperty mAutoInitTracker;

		private SerializedProperty mAutoStartTracker;

		private SerializedProperty mPosePrediction;

		private SerializedProperty mModelCorrectionMode;

		private SerializedProperty mModelTransformEnabled;

		private SerializedProperty mModelTransform;

		public override string Title
		{
			get
			{
				return "Device Tracker";
			}
		}

		public override void FindSerializedProperties(SerializedObject serializedObject)
		{
			this.mAutoInitTracker = serializedObject.FindProperty("deviceTracker.autoInitTracker");
			this.mAutoStartTracker = serializedObject.FindProperty("deviceTracker.autoStartTracker");
			this.mPosePrediction = serializedObject.FindProperty("deviceTracker.posePrediction");
			this.mModelCorrectionMode = serializedObject.FindProperty("deviceTracker.modelCorrectionMode");
			this.mModelTransformEnabled = serializedObject.FindProperty("deviceTracker.modelTransformEnabled");
			this.mModelTransform = serializedObject.FindProperty("deviceTracker.modelTransform");
		}

		public override void DrawInspectorGUI()
		{
			bool boolValue = this.mAutoInitTracker.boolValue;
			EditorGUILayout.PropertyField(this.mAutoInitTracker, new GUIContent("Enable device pose tracking"), new GUILayoutOption[0]);
			if (this.mAutoInitTracker.boolValue != boolValue)
			{
				boolValue = this.mAutoInitTracker.boolValue;
				this.mAutoStartTracker.boolValue = boolValue;
			}
			if (boolValue)
			{
				EditorGUILayout.PropertyField(this.mPosePrediction, new GUIContent("Enable prediction"), new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(this.mModelCorrectionMode, new GUIContent("Model Correction Mode"), new GUILayoutOption[0]);
				RotationalDeviceTracker.MODEL_CORRECTION_MODE enumValueIndex = (RotationalDeviceTracker.MODEL_CORRECTION_MODE)this.mModelCorrectionMode.enumValueIndex;
				if (enumValueIndex != RotationalDeviceTracker.MODEL_CORRECTION_MODE.NONE)
				{
					EditorGUILayout.PropertyField(this.mModelTransformEnabled, new GUIContent("Custom model transform"), new GUILayoutOption[0]);
					if (this.mModelTransformEnabled.boolValue)
					{
						this.mModelTransform.vector3Value = EditorGUILayout.Vector3Field("Pivot point", this.mModelTransform.vector3Value, new GUILayoutOption[0]);
						if (GUILayout.Button(new GUIContent("Set default pivot point", "Reset pivot point to default value of this model correction mode"), new GUILayoutOption[0]))
						{
							if (enumValueIndex == RotationalDeviceTracker.MODEL_CORRECTION_MODE.HEAD)
							{
								this.mModelTransform.vector3Value = DeviceTrackerARController.DEFAULT_HEAD_PIVOT;
								return;
							}
							if (enumValueIndex != RotationalDeviceTracker.MODEL_CORRECTION_MODE.HANDHELD)
							{
								return;
							}
							this.mModelTransform.vector3Value = DeviceTrackerARController.DEFAULT_HANDHELD_PIVOT;
						}
					}
				}
			}
		}
	}
}
