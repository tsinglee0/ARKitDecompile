using System;
using UnityEditor;

namespace Vuforia.EditorClasses
{
	internal class TargetDataPostprocessor : AssetPostprocessor
	{
		public enum ImportState
		{
			NONE,
			ADDED,
			RENAMED,
			DELETED
		}

		public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			bool flag = false;
			for (int i = 0; i < importedAssets.Length; i++)
			{
				if (TargetDataPostprocessor.IsVuforiaAssetChanged(importedAssets[i]))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int i = 0; i < deletedAssets.Length; i++)
				{
					if (TargetDataPostprocessor.IsVuforiaAssetChanged(deletedAssets[i]))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				for (int i = 0; i < movedAssets.Length; i++)
				{
					if (TargetDataPostprocessor.IsVuforiaAssetChanged(movedAssets[i]))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				SceneManager.Instance.FilesUpdated();
			}
		}

		private static bool IsVuforiaAssetChanged(string assetFileString)
		{
			return assetFileString.IndexOf("Assets/StreamingAssets/Vuforia/", StringComparison.OrdinalIgnoreCase) != -1
                || assetFileString.IndexOf("Assets/StreamingAssets/Vuforia/WordLists/", StringComparison.OrdinalIgnoreCase) != -1
                || assetFileString.IndexOf("Assets/Editor/Vuforia/TargetsetData/", StringComparison.OrdinalIgnoreCase) != -1
                || assetFileString.IndexOf("Assets/StreamingAssets/QCAR/", StringComparison.OrdinalIgnoreCase) != -1
                || assetFileString.IndexOf("Assets/StreamingAssets/QCAR/", StringComparison.OrdinalIgnoreCase) != -1
                || assetFileString.IndexOf("Assets/Editor/QCAR/TargetsetData/", StringComparison.OrdinalIgnoreCase) != -1;
		}
	}
}
