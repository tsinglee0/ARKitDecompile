using System;
using UnityEditor;

namespace Vuforia.EditorClasses
{
	public class DataSetToTrackableMenu : Editor
	{
		[MenuItem("Vuforia/Apply Database Properties", false, 2)]
		public static void ApplyDataSetProperties()
		{
			SceneManager.Instance.ApplyDataSetProperties();
		}
	}
}
