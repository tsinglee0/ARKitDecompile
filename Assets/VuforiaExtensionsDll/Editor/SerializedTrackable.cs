using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	public class SerializedTrackable
	{
		protected readonly SerializedObject mSerializedObject;

		private readonly SerializedProperty mTrackableName;

		private readonly SerializedProperty mPreserveChildSize;

		private readonly SerializedProperty mInitializedInEditor;

		public SerializedObject SerializedObject
		{
			get
			{
				return this.mSerializedObject;
			}
		}

		public SerializedProperty TrackableNameProperty
		{
			get
			{
				return this.mTrackableName;
			}
		}

		public string TrackableName
		{
			get
			{
				return this.mTrackableName.stringValue;
			}
			set
			{
				this.mTrackableName.stringValue = value;
			}
		}

		public SerializedProperty PreserveChildSizeProperty
		{
			get
			{
				return this.mPreserveChildSize;
			}
		}

		public bool PreserveChildSize
		{
			get
			{
				return this.mPreserveChildSize.boolValue;
			}
			set
			{
				this.mPreserveChildSize.boolValue = value;
			}
		}

		public SerializedProperty InitializedInEditorProperty
		{
			get
			{
				return this.mInitializedInEditor;
			}
		}

		public bool InitializedInEditor
		{
			get
			{
				return this.mInitializedInEditor.boolValue;
			}
			set
			{
				this.mInitializedInEditor.boolValue = value;
			}
		}

		public SerializedTrackable(SerializedObject target)
		{
			this.mSerializedObject = target;
			this.mTrackableName = this.mSerializedObject.FindProperty("mTrackableName");
			this.mPreserveChildSize = this.mSerializedObject.FindProperty("mPreserveChildSize");
			this.mInitializedInEditor = this.mSerializedObject.FindProperty("mInitializedInEditor");
		}

		public SerializedObjectExtension.EditHandle Edit()
		{
			return new SerializedObjectExtension.EditHandle(this.mSerializedObject);
		}

		public Material GetMaterial()
		{
			return ((MonoBehaviour)this.mSerializedObject.targetObject).GetComponent<Renderer>().sharedMaterial;
		}

		public Material[] GetMaterials()
		{
			return ((MonoBehaviour)this.mSerializedObject.targetObject).GetComponent<Renderer>().sharedMaterials;
		}

		public void SetMaterial(Material material)
		{
            UnityEngine.Object[] targetObjects = this.mSerializedObject.targetObjects;
			for (int i = 0; i < targetObjects.Length; i++)
			{
				((MonoBehaviour)targetObjects[i]).GetComponent<Renderer>().sharedMaterial = material;
			}
			SceneManager.Instance.UnloadUnusedAssets();
		}

		public void SetMaterial(Material[] materials)
		{
            UnityEngine.Object[] targetObjects = this.mSerializedObject.targetObjects;
			for (int i = 0; i < targetObjects.Length; i++)
			{
				((MonoBehaviour)targetObjects[i]).GetComponent<Renderer>().sharedMaterials = materials;
			}
			SceneManager.Instance.UnloadUnusedAssets();
		}

		public List<GameObject> GetGameObjects()
		{
			List<GameObject> list = new List<GameObject>();
            UnityEngine.Object[] targetObjects = this.mSerializedObject.targetObjects;
			for (int i = 0; i < targetObjects.Length; i++)
			{
                UnityEngine.Object @object = targetObjects[i];
				list.Add(((MonoBehaviour)@object).gameObject);
			}
			return list;
		}
	}
}
