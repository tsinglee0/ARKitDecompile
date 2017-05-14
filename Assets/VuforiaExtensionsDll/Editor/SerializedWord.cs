using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	public class SerializedWord : SerializedTrackable
	{
		private readonly SerializedProperty mMode;

		private readonly SerializedProperty mSpecificWord;

		public SerializedProperty ModeProperty
		{
			get
			{
				return this.mMode;
			}
		}

		public WordTemplateMode Mode
		{
			get
			{
				return (WordTemplateMode)this.mMode.enumValueIndex;
			}
			set
			{
				this.mMode.enumValueIndex = (int)value;
			}
		}

		public SerializedProperty SpecificWordProperty
		{
			get
			{
				return this.mSpecificWord;
			}
		}

		public string SpecificWord
		{
			get
			{
				return this.mSpecificWord.stringValue;
			}
			set
			{
				this.mSpecificWord.stringValue = value;
			}
		}

		public bool IsTemplateMode
		{
			get
			{
				return this.Mode == WordTemplateMode.Template;
			}
		}

		public bool IsSpecificWordMode
		{
			get
			{
				return this.Mode == WordTemplateMode.SpecificWord;
			}
		}

		public SerializedWord(SerializedObject target) : base(target)
		{
			this.mMode = target.FindProperty("mMode");
			this.mSpecificWord = target.FindProperty("mSpecificWord");
		}

		public List<WordAbstractBehaviour> GetBehaviours()
		{
			List<WordAbstractBehaviour> list = new List<WordAbstractBehaviour>();
            UnityEngine.Object[] targetObjects = this.mSerializedObject.targetObjects;
			for (int i = 0; i < targetObjects.Length; i++)
			{
                UnityEngine.Object @object = targetObjects[i];
				list.Add((WordAbstractBehaviour)@object);
			}
			return list;
		}
	}
}
