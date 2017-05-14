using System;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	[CustomEditor(typeof(BackgroundPlaneAbstractBehaviour), true)]
	public class BackgroundPlaneBehaviourEditor : Editor
	{
		private static string NumDivisionsText = "Number Divisions";

		private static string ToolTipNumDivisions = "Specify the number of divisions (n*n) of the background mesh. the value has to be greater than 1.";

		private SerializedProperty mNumDivisions;

		private void OnEnable()
		{
			this.mNumDivisions = base.serializedObject.FindProperty("mNumDivisions");
		}

		public override void OnInspectorGUI()
		{
			base.DrawDefaultInspector();
			VuforiaUtilities.DisableGuiForPrefab(base.target);
			if (Application.isPlaying)
			{
				GUI.enabled = false;
			}
			using (base.serializedObject.Edit())
			{
				EditorGUILayout.PropertyField(this.mNumDivisions, new GUIContent(BackgroundPlaneBehaviourEditor.NumDivisionsText, BackgroundPlaneBehaviourEditor.ToolTipNumDivisions), new GUILayoutOption[0]);
			}
			GUI.enabled = true;
		}
	}
}
