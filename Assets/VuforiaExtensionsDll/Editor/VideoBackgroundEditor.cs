using System;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	internal class VideoBackgroundEditor : ConfigurationEditor
	{
		private SerializedProperty mClippingMode;

		private SerializedProperty mMatteShader;

		private SerializedProperty mVideoBackgroundEnabled;

		public override string Title
		{
			get
			{
				return "Video Background";
			}
		}

		public override void FindSerializedProperties(SerializedObject serializedObject)
		{
			this.mClippingMode = serializedObject.FindProperty("videoBackground.clippingMode");
			this.mMatteShader = serializedObject.FindProperty("videoBackground.matteShader");
			this.mVideoBackgroundEnabled = serializedObject.FindProperty("videoBackground.videoBackgroundEnabled");
		}

		public override void DrawInspectorGUI()
		{
			EditorGUILayout.PropertyField(this.mVideoBackgroundEnabled, new GUIContent("Enable video background"), new GUILayoutOption[0]);
			HideExcessAreaAbstractBehaviour.CLIPPING_MODE cLIPPING_MODE = (HideExcessAreaAbstractBehaviour.CLIPPING_MODE)this.mClippingMode.enumValueIndex;
			EditorGUILayout.PropertyField(this.mClippingMode, new GUIContent("Overflow geometry"), new GUILayoutOption[0]);
			HideExcessAreaAbstractBehaviour.CLIPPING_MODE enumValueIndex = (HideExcessAreaAbstractBehaviour.CLIPPING_MODE)this.mClippingMode.enumValueIndex;
			if (cLIPPING_MODE != enumValueIndex)
			{
				cLIPPING_MODE = enumValueIndex;
				this.mMatteShader.objectReferenceValue = VuforiaAbstractConfiguration.VideoBackgroundConfiguration.GetDefaultMatteShader(cLIPPING_MODE);
			}
			if (cLIPPING_MODE != HideExcessAreaAbstractBehaviour.CLIPPING_MODE.NONE)
			{
				EditorGUILayout.PropertyField(this.mMatteShader, new GUIContent("Matte Shader"), new GUILayoutOption[0]);
			}
		}
	}
}
