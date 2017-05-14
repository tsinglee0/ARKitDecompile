using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	internal class DatabaseLoadEditor : ConfigurationEditor
	{
		private SerializedProperty mDataSetsToLoad;

		private SerializedProperty mDataSetsToActivate;

		public override string Title
		{
			get
			{
				return "Datasets";
			}
		}

		public override void FindSerializedProperties(SerializedObject serializedObject)
		{
			this.mDataSetsToLoad = serializedObject.FindProperty("databaseLoad.dataSetsToLoad");
			this.mDataSetsToActivate = serializedObject.FindProperty("databaseLoad.dataSetsToActivate");
			if (!SceneManager.Instance.SceneInitialized)
			{
				SceneManager.Instance.InitScene();
			}
		}

		public override void DrawInspectorGUI()
		{
			string[] array = new string[ConfigDataManager.Instance.NumConfigDataObjects - 1];
			ConfigDataManager.Instance.GetConfigDataNames(array, false);
			string[] array2;
			this.mDataSetsToLoad.GetArrayItems(out array2);
			string[] source;
			this.mDataSetsToActivate.GetArrayItems(out source);
			string[] array3 = array2;
			for (int i = 0; i < array3.Length; i++)
			{
				string text = array3[i];
				if (!array.Contains(text))
				{
					this.mDataSetsToLoad.RemoveArrayItem(text);
					this.mDataSetsToActivate.RemoveArrayItem(text);
				}
			}
			this.mDataSetsToLoad.GetArrayItems(out array2);
			this.mDataSetsToActivate.GetArrayItems(out source);
			array3 = array;
			for (int i = 0; i < array3.Length; i++)
			{
				string text2 = array3[i];
				bool flag = array2.Contains(text2);
				bool flag2 = source.Contains(text2);
				bool flag3 = EditorGUILayout.Toggle("Load " + text2 + " Database", flag, new GUILayoutOption[0]);
				bool flag4 = false;
				if (flag3)
				{
					flag4 = EditorGUILayout.Toggle("        Activate", flag2, new GUILayoutOption[0]);
				}
				if (text2 != array[array.Length - 1])
				{
					EditorGUILayout.Separator();
					EditorGUILayout.Separator();
				}
				if (flag && !flag3)
				{
					this.mDataSetsToLoad.RemoveArrayItem(text2);
				}
				else if (!flag & flag3)
				{
					this.mDataSetsToLoad.AddArrayItem(text2);
				}
				if (flag2 && !flag4)
				{
					this.mDataSetsToActivate.RemoveArrayItem(text2);
				}
				else if (!flag2 & flag4)
				{
					this.mDataSetsToActivate.AddArrayItem(text2);
				}
			}
		}

		internal static bool OnConfigDataChanged()
		{
			string[] dataSetList = new string[ConfigDataManager.Instance.NumConfigDataObjects - 1];
			ConfigDataManager.Instance.GetConfigDataNames(dataSetList, false);
			VuforiaAbstractConfiguration vuforiaAbstractConfiguration = VuforiaAbstractConfigurationEditor.LoadConfigurationObject();
			if (vuforiaAbstractConfiguration == null)
			{
				return false;
			}
			VuforiaAbstractConfiguration.DatabaseLoadConfiguration expr_45 = vuforiaAbstractConfiguration.DatabaseLoad;
			List<string> list = expr_45.DataSetsToActivate.ToList<string>();
			list.RemoveAll((string s) => Array.Find<string>(dataSetList, (string str) => str.Equals(s)) == null);
			expr_45.DataSetsToActivate = list.ToArray();
			list = expr_45.DataSetsToLoad.ToList<string>();
			list.RemoveAll((string s) => Array.Find<string>(dataSetList, (string str) => str.Equals(s)) == null);
			expr_45.DataSetsToLoad = list.ToArray();
			return true;
		}
	}
}
