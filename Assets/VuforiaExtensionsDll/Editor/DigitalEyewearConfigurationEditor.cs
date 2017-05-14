using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	internal class DigitalEyewearConfigurationEditor : ConfigurationEditor
	{
		private const string TOOL_TIP_CAMERA_OFFSET = "Set an IPD value for play mode and occluded stereo devices.";

		private const string CAMERA_OFFSET_TEXT = "Camera Offset";

		private const string BUTTON_TYPE = "Button Type";

		private const string SCREEN_TO_LENS_DISTANCE = "Screen To Lens Distance";

		private const string INTER_LENS_DISTANCE = "Inter Lens Distance";

		private const string TRAY_ALIGNMENT = "Tray Alignment";

		private const string LENS_CENTER_TO_TRAY_DISTANCE = "Lens Center To Tray Distance";

		private const string DISTORTION_COEFFICIENTS = "Distortion Coefficients";

		private const string FIELD_OF_VIEW = "Field Of View";

		private const string CONTAINS_MAGNET = "Contains Magnet";

		private SerializedProperty mCameraOffset;

		private SerializedProperty mDistortionRenderingMode;

		private SerializedProperty mEyewearType;

		private SerializedProperty mStereoFramework;

		private SerializedProperty mSeeThroughConfiguration;

		private SerializedProperty mViewerName;

		private SerializedProperty mViewerManufacturer;

		private SerializedProperty mUseCustomViewer;

		private SerializedProperty mCustomViewer;

		public override string Title
		{
			get
			{
				return "Digital Eyewear";
			}
		}

		public override void FindSerializedProperties(SerializedObject serializedObject)
		{
			this.mCameraOffset = serializedObject.FindProperty("digitalEyewear.cameraOffset");
			this.mDistortionRenderingMode = serializedObject.FindProperty("digitalEyewear.distortionRenderingMode");
			this.mEyewearType = serializedObject.FindProperty("digitalEyewear.eyewearType");
			this.mStereoFramework = serializedObject.FindProperty("digitalEyewear.stereoFramework");
			this.mSeeThroughConfiguration = serializedObject.FindProperty("digitalEyewear.seeThroughConfiguration");
			this.mViewerName = serializedObject.FindProperty("digitalEyewear.viewerName");
			this.mViewerManufacturer = serializedObject.FindProperty("digitalEyewear.viewerManufacturer");
			this.mUseCustomViewer = serializedObject.FindProperty("digitalEyewear.useCustomViewer");
			this.mCustomViewer = serializedObject.FindProperty("digitalEyewear.customViewer");
		}

		public override void DrawInspectorGUI()
		{
			EditorStyles.textField.wordWrap = false;
			List<string> list = new List<string>
			{
				"None",
				"Optical See-Through",
				"Video See-Through"
			};
			int num = EditorGUILayout.Popup("Eyewear Type", this.mEyewearType.enumValueIndex, list.ToArray(), new GUILayoutOption[0]);
			DigitalEyewearARController.EyewearType eyewearType = (DigitalEyewearARController.EyewearType)num;
			this.mEyewearType.enumValueIndex = num;
			if (eyewearType == DigitalEyewearARController.EyewearType.OpticalSeeThrough)
			{
				List<string> list2 = new List<string>
				{
					"Vuforia",
					"HoloLens"
				};
				int enumValueIndex = this.mSeeThroughConfiguration.enumValueIndex;
				int enumValueIndex2 = EditorGUILayout.Popup("See Through Config", enumValueIndex, list2.ToArray(), new GUILayoutOption[0]);
				this.mSeeThroughConfiguration.enumValueIndex = enumValueIndex2;
				return;
			}
			if (eyewearType == DigitalEyewearARController.EyewearType.VideoSeeThrough)
			{
				List<string> list3 = new List<string>
				{
					"Vuforia",
					"Cardboard",
					"Gear VR (Oculus)"
				};
				int arg_F1_0 = this.mStereoFramework.intValue;
				int num2 = EditorGUILayout.Popup("Stereo Camera Config", this.mStereoFramework.intValue, list3.ToArray(), new GUILayoutOption[0]);
				bool arg_125_0 = num2 != 0;
				this.mStereoFramework.intValue = num2;
				if (!arg_125_0)
				{
					IViewerParameters[] viewerList = DigitalEyewearConfigurationEditor.GetViewerList();
					List<string> list4 = new List<string>();
					IViewerParameters[] array = viewerList;
					for (int i = 0; i < array.Length; i++)
					{
						IViewerParameters viewerParameters = array[i];
						list4.Add(DigitalEyewearConfigurationEditor.GetViewerIdentifier(viewerParameters.GetName(), viewerParameters.GetManufacturer()));
					}
					list4.Add("Custom");
					int num3 = list4.IndexOf(DigitalEyewearConfigurationEditor.GetViewerIdentifier(this.mViewerName.stringValue, this.mViewerManufacturer.stringValue));
					if (this.mUseCustomViewer.boolValue)
					{
						num3 = viewerList.Length;
					}
					else if (num3 < 0)
					{
						num3 = 0;
					}
					num3 = EditorGUILayout.Popup("Viewer Type", num3, list4.ToArray(), new GUILayoutOption[0]);
					if (num3 >= viewerList.Length)
					{
						EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
						EditorGUILayout.PropertyField(this.mCustomViewer.FindPropertyRelative("Name"), new GUIContent("Name"), new GUILayoutOption[0]);
						EditorGUILayout.PropertyField(this.mCustomViewer.FindPropertyRelative("Manufacturer"), new GUIContent("Manufacturer"), new GUILayoutOption[0]);
						EditorGUILayout.PropertyField(this.mCustomViewer.FindPropertyRelative("Version"), new GUIContent("Version"), new GUILayoutOption[0]);
						EditorGUILayout.PropertyField(this.mCustomViewer.FindPropertyRelative("ButtonType"), new GUIContent("Button Type"), new GUILayoutOption[0]);
						EditorGUILayout.PropertyField(this.mCustomViewer.FindPropertyRelative("ScreenToLensDistance"), new GUIContent("Screen To Lens Distance"), new GUILayoutOption[0]);
						EditorGUILayout.PropertyField(this.mCustomViewer.FindPropertyRelative("InterLensDistance"), new GUIContent("Inter Lens Distance"), new GUILayoutOption[0]);
						EditorGUILayout.PropertyField(this.mCustomViewer.FindPropertyRelative("TrayAlignment"), new GUIContent("Tray Alignment"), new GUILayoutOption[0]);
						EditorGUILayout.PropertyField(this.mCustomViewer.FindPropertyRelative("LensCenterToTrayDistance"), new GUIContent("Lens Center To Tray Distance"), new GUILayoutOption[0]);
						EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
						EditorGUIUtility.labelWidth = 60f;
						EditorGUILayout.LabelField("Distortion Coefficients", new GUILayoutOption[0]);
						EditorGUIUtility.labelWidth = 40f;
						SerializedProperty serializedProperty = this.mCustomViewer.FindPropertyRelative("DistortionCoefficients");
						float num4 = EditorGUILayout.FloatField("k1", serializedProperty.vector2Value.x, new GUILayoutOption[0]);
						float num5 = EditorGUILayout.FloatField("k2", serializedProperty.vector2Value.y, new GUILayoutOption[0]);
						serializedProperty.vector2Value = new Vector2(num4, num5);
						EditorGUIUtility.labelWidth = 0f;
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
						EditorGUIUtility.labelWidth = 60f;
						EditorGUILayout.LabelField("Field Of View", new GUILayoutOption[0]);
						SerializedProperty serializedProperty2 = this.mCustomViewer.FindPropertyRelative("FieldOfView");
						float num6 = EditorGUILayout.FloatField(serializedProperty2.vector4Value.x, new GUILayoutOption[0]);
						float num7 = EditorGUILayout.FloatField(serializedProperty2.vector4Value.y, new GUILayoutOption[0]);
						float num8 = EditorGUILayout.FloatField(serializedProperty2.vector4Value.z, new GUILayoutOption[0]);
						float num9 = EditorGUILayout.FloatField(serializedProperty2.vector4Value.w, new GUILayoutOption[0]);
						serializedProperty2.vector4Value = new Vector4(num6, num7, num8, num9);
						EditorGUIUtility.labelWidth = 0f;
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.PropertyField(this.mCustomViewer.FindPropertyRelative("ContainsMagnet"), new GUIContent("Contains Magnet"), new GUILayoutOption[0]);
						EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
						this.mUseCustomViewer.boolValue = true;
					}
					else
					{
						IViewerParameters viewerParameters2 = viewerList[num3];
						this.mViewerName.stringValue = viewerParameters2.GetName();
						this.mViewerManufacturer.stringValue = viewerParameters2.GetManufacturer();
						this.mUseCustomViewer.boolValue = false;
						EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
						DigitalEyewearConfigurationEditor.EditorLabel("Button Type", viewerParameters2.GetButtonType().ToString());
						DigitalEyewearConfigurationEditor.EditorLabel("Screen To Lens Distance", viewerParameters2.GetScreenToLensDistance().ToString());
						DigitalEyewearConfigurationEditor.EditorLabel("Inter Lens Distance", viewerParameters2.GetInterLensDistance().ToString());
						DigitalEyewearConfigurationEditor.EditorLabel("Tray Alignment", viewerParameters2.GetTrayAlignment().ToString());
						DigitalEyewearConfigurationEditor.EditorLabel("Lens Center To Tray Distance", viewerParameters2.GetLensCentreToTrayDistance().ToString());
						int numDistortionCoefficients = viewerParameters2.GetNumDistortionCoefficients();
						StringBuilder stringBuilder = new StringBuilder();
						for (int j = 0; j < numDistortionCoefficients; j++)
						{
							stringBuilder.Append(viewerParameters2.GetDistortionCoefficient(j).ToString());
							stringBuilder.Append("   ");
						}
						DigitalEyewearConfigurationEditor.EditorLabel("Distortion Coefficients", stringBuilder.ToString());
						Vector4 fieldOfView = viewerParameters2.GetFieldOfView();
						StringBuilder stringBuilder2 = new StringBuilder();
						for (int k = 0; k < 4; k++)
						{
							stringBuilder2.Append(fieldOfView[k]);
							stringBuilder2.Append("   ");
						}
						DigitalEyewearConfigurationEditor.EditorLabel("Field Of View", stringBuilder2.ToString());
						DigitalEyewearConfigurationEditor.EditorLabel("Contains Magnet", viewerParameters2.ContainsMagnet().ToString());
						EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
					}
					EditorGUILayout.Space();
					EditorGUILayout.PropertyField(this.mDistortionRenderingMode, new GUIContent("Distortion Mode"), new GUILayoutOption[0]);
					EditorGUILayout.PropertyField(this.mCameraOffset, new GUIContent("Camera Offset", "Set an IPD value for play mode and occluded stereo devices."), new GUILayoutOption[0]);
				}
			}
		}

		private static void EditorLabel(string label, string info)
		{
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
			EditorGUILayout.PrefixLabel(label);
			EditorGUILayout.LabelField(info, new GUILayoutOption[0]);
			EditorGUILayout.EndHorizontal();
		}

		private static IViewerParameters[] GetViewerList()
		{
			List<IViewerParameters> arg_15_0 = ViewerParametersList.ListForAuthoringTools.GetAllViewers().ToList<IViewerParameters>();
			List<IViewerParameters> list = new List<IViewerParameters>();
			foreach (IViewerParameters current in arg_15_0)
			{
				if (current.GetManufacturer() == "Vuforia")
				{
					list.Insert(0, current);
				}
				else
				{
					list.Add(current);
				}
			}
			return list.ToArray();
		}

		private static string GetViewerIdentifier(string name, string manufacturer)
		{
			return name + " (" + manufacturer + ")";
		}
	}
}
