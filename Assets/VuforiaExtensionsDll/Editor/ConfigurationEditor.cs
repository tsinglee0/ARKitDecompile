using System;
using UnityEditor;

namespace Vuforia.EditorClasses
{
	internal abstract class ConfigurationEditor
	{
		public abstract string Title
		{
			get;
		}

		public bool Foldout
		{
			get;
			private set;
		}

		private string FoldoutEditorPrefKey
		{
			get
			{
				return "Vuforia_Foldout_" + this.Title;
			}
		}

		public abstract void FindSerializedProperties(SerializedObject serializedObject);

		public abstract void DrawInspectorGUI();

		protected ConfigurationEditor()
		{
			this.Foldout = EditorPrefs.GetBool(this.FoldoutEditorPrefKey, true);
		}

		public void SetFoldout(bool foldout)
		{
			this.Foldout = foldout;
			EditorPrefs.SetBool(this.FoldoutEditorPrefKey, this.Foldout);
		}
	}
}
