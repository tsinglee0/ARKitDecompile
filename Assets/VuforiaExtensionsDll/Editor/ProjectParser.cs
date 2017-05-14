using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	internal static class ProjectParser
	{
		private static void SetLicenseKeyFromFromCommandLineParam()
		{
			string licenseKeyInGlobalConfiguration;
			if (ProjectParser.GetKeyFromCommandLine(out licenseKeyInGlobalConfiguration))
			{
				ProjectParser.SetLicenseKeyInGlobalConfiguration(licenseKeyInGlobalConfiguration);
				return;
			}
			Debug.LogError("License key was not correctly specified via command line");
		}

		private static bool GetKeyFromCommandLine(out string key)
		{
			key = "";
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			int i = 0;
			while (i < commandLineArgs.Length - 2)
			{
				if (commandLineArgs[i] == "-executeMethod")
				{
					if (commandLineArgs[i + 2].Length > 0 && commandLineArgs[i + 2][0] != '-')
					{
						key = commandLineArgs[i + 2];
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			if (key.Length == 0)
			{
				Debug.LogError("No license key defined!");
				return false;
			}
			if (key.Equals("EMPTY"))
			{
				key = string.Empty;
			}
			return true;
		}

		private static void SetLicenseKeyInGlobalConfiguration(string licenseKey)
		{
			VuforiaAbstractConfiguration expr_05 = VuforiaAbstractConfigurationEditor.LoadConfigurationObject();
			Undo.RecordObject(expr_05, "Setting License Key");
			expr_05.Vuforia.LicenseKey = licenseKey;
			EditorUtility.SetDirty(expr_05);
			AssetDatabase.SaveAssets();
		}

		private static List<string> GetScenesInDirectory(string root)
		{
			List<string> list = new List<string>();
			string[] array = null;
			try
			{
				array = Directory.GetFiles(root);
			}
			catch (UnauthorizedAccessException arg_11_0)
			{
				Debug.LogWarning(arg_11_0.Message);
			}
			catch (DirectoryNotFoundException arg_1D_0)
			{
				Debug.LogWarning(arg_1D_0.Message);
			}
			if (array != null)
			{
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text = array2[i];
					if (string.Compare(ProjectParser.StripExtensionFromPath(text), "unity", true) == 0)
					{
						list.Add(text);
					}
				}
				array2 = Directory.GetDirectories(root);
				for (int i = 0; i < array2.Length; i++)
				{
					string root2 = array2[i];
					list.AddRange(ProjectParser.GetScenesInDirectory(root2));
				}
			}
			list.Sort();
			return list;
		}

		private static string StripExtensionFromPath(string fullPath)
		{
			string[] array = fullPath.Split(new char[]
			{
				'.'
			});
			if (array.Length <= 1)
			{
				return "";
			}
			return array[array.Length - 1];
		}
	}
}
