using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	internal class VuforiaUtilities
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct GlobalVars
		{
			public const string DATA_SET_PATH = "Assets/StreamingAssets/Vuforia/";

			public const string WORD_LIST_PATH = "Assets/StreamingAssets/Vuforia/WordLists/";

			public const string EDITOR_ASSETS_PATH = "Assets/Editor/Vuforia/";

			public const string EDITOR_INFO_FILE = "authoringinfo.xml";

			public const string CONFIG_EDITOR_OTARGET_VIZ_PATH = "Assets/Editor/Vuforia/TargetsetData/";

			public const string TARGET_TEXTURES_PATH = "Assets/Editor/Vuforia/ImageTargetTextures/";

			public const string CYLINDER_TARGET_TEXTURES_PATH = "Assets/Editor/Vuforia/CylinderTargetTextures/";

			public const string VUMARK_TARGET_TEXTURES_PATH = "Assets/Editor/Vuforia/VuMarkTextures/";

			public const string DATA_SET_PATH_LEGACY = "Assets/StreamingAssets/QCAR/";

			public const string WORD_LIST_PATH_LEGACY = "Assets/StreamingAssets/QCAR/";

			public const string CONFIG_EDITOR_OTARGET_VIZ_PATH_LEGACY = "Assets/Editor/QCAR/TargetsetData/";

			public const string TARGET_TEXTURES_PATH_LEGACY = "Assets/Editor/QCAR/ImageTargetTextures/";

			public const string CYLINDER_TARGET_TEXTURES_PATH_LEGACY = "Assets/Editor/QCAR/CylinderTargetTextures/";

			public const string REFERENCE_MATERIAL_PATH = "Assets/Vuforia/Materials/DefaultTarget.mat";

			public const string MASK_MATERIAL_PATH = "Assets/Vuforia/Materials/DepthMask.mat";

			public const string VIRTUAL_BUTTON_MATERIAL_PATH = "Assets/Vuforia/Editor/VirtualButtonTextures/VirtualButtonPreviewMaterial.mat";

			public const string UDT_MATERIAL_PATH = "Assets/Vuforia/Materials/UserDefinedTarget.mat";

			public const string CL_MATERIAL_PATH = "Assets/Vuforia/Materials/CloudRecoTarget.mat";

			public const string FONT_PATH = "Assets/Vuforia/Fonts/";

			public const string PREFABS_PATH = "Assets/Vuforia/Prefabs/";

			public const string DEFAULT_TRACKABLE_NAME = "--- EMPTY ---";

			public const string DEFAULT_DATA_SET_NAME = "--- EMPTY ---";

			public const string PREDEFINED_TARGET_DROPDOWN_TEXT = "Predefined";

			public const string USER_CREATED_TARGET_DROPDOWN_TEXT = "User Defined";

			public const string CLOUD_RECO_DROPDOWN_TEXT = "Cloud Reco";
		}

		public static bool SizeFromStringArray(out Vector2 result, string[] valuesToParse)
		{
			result = Vector2.zero;
			bool flag = false;
			if (valuesToParse != null && valuesToParse.Length == 2)
			{
				flag = true;
			}
			if (!flag)
			{
				return false;
			}
			try
			{
				result = new Vector2(float.Parse(valuesToParse[0]), float.Parse(valuesToParse[1]));
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static bool TransformFromStringArray(out Vector3 result, string[] valuesToParse)
		{
			result = Vector3.zero;
			bool flag = false;
			if (valuesToParse != null && valuesToParse.Length == 3)
			{
				flag = true;
			}
			if (!flag)
			{
				return false;
			}
			try
			{
				result = new Vector3(float.Parse(valuesToParse[0]), float.Parse(valuesToParse[2]), float.Parse(valuesToParse[1]));
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static bool BoundinBoxFromStringArray(out Vector3 minBBox, out Vector3 maxBBox, string[] valuesToParse)
		{
			minBBox = Vector3.zero;
			maxBBox = Vector3.zero;
			bool flag = false;
			if (valuesToParse != null && valuesToParse.Length == 6)
			{
				flag = true;
			}
			if (!flag)
			{
				return false;
			}
			try
			{
				minBBox = new Vector3(float.Parse(valuesToParse[0]), float.Parse(valuesToParse[1]), float.Parse(valuesToParse[2]));
				maxBBox = new Vector3(float.Parse(valuesToParse[3]), float.Parse(valuesToParse[4]), float.Parse(valuesToParse[5]));
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static bool RectangleFromStringArray(out Vector4 result, string[] valuesToParse)
		{
			result = Vector4.zero;
			bool flag = false;
			if (valuesToParse != null && valuesToParse.Length == 4)
			{
				flag = true;
			}
			if (!flag)
			{
				return false;
			}
			try
			{
				result = new Vector4(float.Parse(valuesToParse[0]), float.Parse(valuesToParse[1]), float.Parse(valuesToParse[2]), float.Parse(valuesToParse[3]));
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static bool OrientationFromStringArray(out Quaternion result, string[] valuesToParse)
		{
			result = Quaternion.identity;
			bool flag = false;
			if (valuesToParse != null)
			{
				if (valuesToParse.Length == 5)
				{
					flag = true;
				}
				else if (valuesToParse.Length == 4)
				{
					Debug.LogError("Direct parsing of Quaternions is not supported. Use Axis-Angle Degrees (AD:) or Axis-Angle Radians (AR:) instead.");
				}
			}
			if (!flag)
			{
				return false;
			}
			try
			{
				float num = float.Parse(valuesToParse[4]);
				Vector3 vector = new Vector3(-float.Parse(valuesToParse[1]), float.Parse(valuesToParse[3]), -float.Parse(valuesToParse[2]));
				if (string.Compare(valuesToParse[0], "ad:", true) == 0)
				{
					result = Quaternion.AngleAxis(num, vector);
				}
				else
				{
					if (string.Compare(valuesToParse[0], "ar:", true) != 0)
					{
						bool result2 = false;
						return result2;
					}
					result = Quaternion.AngleAxis(57.29578f * num, vector);
				}
			}
			catch
			{
				bool result2 = false;
				return result2;
			}
			return true;
		}

		public static int GetIndexFromString(string stringToFind, string[] availableStrings)
		{
			int result = -1;
			for (int i = 0; i < availableStrings.Length; i++)
			{
				if (string.Compare(availableStrings[i], stringToFind, true) == 0)
				{
					result = i;
				}
			}
			return result;
		}

		public static PrefabType GetPrefabType(UnityEngine.Object target)
		{
			return PrefabUtility.GetPrefabType(target);
		}

		public static void DisableGuiForPrefab(UnityEngine.Object target)
		{
			if (VuforiaUtilities.GetPrefabType(target) == PrefabType.Prefab)
			{
				GUILayout.Label("You can't configure a prefab.", new GUILayoutOption[0]);
				GUI.enabled = false;
			}
		}

		public static ConfigData.ImageTargetData CreateDefaultImageTarget()
		{
			return new ConfigData.ImageTargetData
			{
				size = new Vector2(1f, 1f),
				virtualButtons = new List<ConfigData.VirtualButtonData>()
			};
		}

		public static bool DrawDatasetTrackableInspector(SerializedDataSetTrackable serializedObject, bool resetTrackable, Func<ConfigData, string[]> getTrackableNamesFunc, string trackableLabel)
		{
			bool result = false;
			string[] array = new string[ConfigDataManager.Instance.NumConfigDataObjects];
			ConfigDataManager.Instance.GetConfigDataNames(array);
			int num = -1;
			if (!serializedObject.DataSetPathProperty.hasMultipleDifferentValues)
			{
				num = VuforiaUtilities.GetIndexFromString(serializedObject.GetDataSetName(), array);
				if (num < 0)
				{
					num = 0;
				}
			}
			EditorGUI.BeginChangeCheck();
			int num2 = EditorGUILayout.Popup("Database", num, array, new GUILayoutOption[0]);
			bool flag = EditorGUI.EndChangeCheck();
			if (resetTrackable && num2 < 0)
			{
				num2 = 0;
				flag = true;
			}
			if (num2 >= 0)
			{
				string dataSetName = array[num2];
				ConfigData configData = ConfigDataManager.Instance.GetConfigData(dataSetName);
				string[] array2 = getTrackableNamesFunc(configData);
				int num3 = -1;
				if (!serializedObject.TrackableNameProperty.hasMultipleDifferentValues)
				{
					num3 = VuforiaUtilities.GetIndexFromString(serializedObject.TrackableName, array2);
					if (num3 < 0)
					{
						num3 = 0;
					}
				}
				if (flag)
				{
					num3 = 0;
				}
				EditorGUI.BeginChangeCheck();
				int num4 = EditorGUILayout.Popup(trackableLabel, num3, array2, new GUILayoutOption[0]);
				bool flag2 = EditorGUI.EndChangeCheck();
				if (array2.Length != 0 && (flag | flag2 | resetTrackable))
				{
					result = true;
					serializedObject.DataSetPath = "";
					serializedObject.DataSetPath = VuforiaRuntimeUtilities.StripStreamingAssetsFromPath(ConfigDataManager.Instance.GetConfigData(array[num2]).FullPath);
					if (num4 >= 0)
					{
						string trackableName = array2[num4];
						serializedObject.TrackableName = "";
						serializedObject.TrackableName = trackableName;
					}
				}
			}
			else
			{
				EditorGUILayout.Popup(trackableLabel, -1, new string[0], new GUILayoutOption[0]);
			}
			return result;
		}

		public static void DrawMissingTargetsButton()
		{
			if (GUILayout.Button("No targets defined. Press here for target creation!", new GUILayoutOption[0]))
			{
				SceneManager.Instance.GoToTargetManagerPage();
			}
		}

		public static void DrawTrackableOptions(SerializedDataSetTrackable serializedObject, bool drawPreserveChildSize = true, bool drawExtendedTracking = true, bool drawSmartTerrain = true)
		{
			if (drawPreserveChildSize)
			{
				EditorGUILayout.PropertyField(serializedObject.PreserveChildSizeProperty, new GUIContent("Preserve child size"), new GUILayoutOption[0]);
			}
			bool flag = SceneManager.Instance.SmartTerrainInitializationEnabled();
			if (drawExtendedTracking)
			{
				if (flag)
				{
					EditorGUILayout.HelpBox("Extended Tracking cannot be enabled at the same time as Smart Terrain.", MessageType.Info);
				}
				bool enabled = GUI.enabled;
				GUI.enabled = enabled && !flag;
				EditorGUILayout.PropertyField(serializedObject.ExtendedTrackingProperty, new GUIContent("Enable Extended Tracking"), new GUILayoutOption[0]);
				GUI.enabled = enabled;
			}
			if (drawSmartTerrain)
			{
				SmartTerrainInitializationTargetEditorExtension.DrawInspectorForInitializationTarget(serializedObject, false);
			}
		}

		public static Material LoadReferenceMaterial()
		{
			string text = "Assets/Vuforia/Materials/DefaultTarget.mat";
			Material expr_0C = AssetDatabase.LoadAssetAtPath<Material>(text);
			if (expr_0C == null)
			{
				Debug.LogError("Could not find reference material at " + text + " please reimport Unity package.");
			}
			return expr_0C;
		}

		public static bool GetImagePathWithExtension(string pathWithoutExtension, out string pathWithExtension)
		{
			if (File.Exists(pathWithoutExtension + ".png"))
			{
				pathWithExtension = pathWithoutExtension + ".png";
				return true;
			}
			if (File.Exists(pathWithoutExtension + ".jpg"))
			{
				pathWithExtension = pathWithoutExtension + ".jpg";
				return true;
			}
			if (File.Exists(pathWithoutExtension + ".jpeg"))
			{
				pathWithExtension = pathWithoutExtension + ".jpeg";
				return true;
			}
			pathWithExtension = "";
			return false;
		}
	}
}
