using System;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	public static class SerializedObjectExtension
	{
		public class EditHandle : IDisposable
		{
			private readonly SerializedObject mSerializedObject;

			public EditHandle(SerializedObject serializedObject)
			{
				this.mSerializedObject = serializedObject;
				this.mSerializedObject.Update();
			}

			public void Dispose()
			{
				this.mSerializedObject.ApplyModifiedProperties();
			}
		}

		public static SerializedObjectExtension.EditHandle Edit(this SerializedObject objectToEdit)
		{
			return new SerializedObjectExtension.EditHandle(objectToEdit);
		}

		public static bool FixApproximatelyEqualFloatValues(this SerializedProperty property)
		{
			if (property.hasMultipleDifferentValues)
			{
				float floatValue = property.floatValue;
                UnityEngine.Object[] targetObjects = property.serializedObject.targetObjects;
				for (int i = 0; i < targetObjects.Length; i++)
				{
					float floatValue2 = new SerializedObject(targetObjects[i]).FindProperty(property.propertyPath).floatValue;
					if (!Mathf.Approximately(floatValue, floatValue2))
					{
						return false;
					}
				}
				property.floatValue = floatValue;
			}
			return true;
		}

		public static void GetArrayItems(this SerializedProperty property, out string[] result)
		{
			SerializedProperty serializedProperty = property.Copy();
			int arraySizeAndAdvanceToFirstItem = SerializedObjectExtension.GetArraySizeAndAdvanceToFirstItem(serializedProperty);
			result = new string[arraySizeAndAdvanceToFirstItem];
			for (int i = 0; i < arraySizeAndAdvanceToFirstItem; i++)
			{
				serializedProperty.Next(false);
				result[i] = serializedProperty.stringValue;
			}
		}

		public static void RemoveArrayItem(this SerializedProperty property, string item)
		{
			SerializedProperty serializedProperty = property.Copy();
			int arraySizeAndAdvanceToFirstItem = SerializedObjectExtension.GetArraySizeAndAdvanceToFirstItem(serializedProperty);
			for (int i = 0; i < arraySizeAndAdvanceToFirstItem; i++)
			{
				serializedProperty.Next(false);
				if (serializedProperty.stringValue == item)
				{
					property.DeleteArrayElementAtIndex(i);
					return;
				}
			}
		}

		public static void AddArrayItem(this SerializedProperty property, string item)
		{
			property.InsertArrayElementAtIndex(0);
			property.GetArrayElementAtIndex(0).stringValue = item;
		}

		private static int GetArraySizeAndAdvanceToFirstItem(SerializedProperty property)
		{
			if (!property.isArray)
			{
				Debug.LogError("Property " + property.name + " is not an array");
				return 0;
			}
			property.Next(true);
			property.Next(true);
			return property.intValue;
		}
	}
}
