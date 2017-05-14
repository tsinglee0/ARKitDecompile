using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	public class SerializedImageTarget : SerializedDataSetTrackable
	{
		private readonly SerializedProperty mAspectRatio;

		private readonly SerializedProperty mImageTargetType;

		private readonly SerializedProperty mWidth;

		private readonly SerializedProperty mHeight;

		public SerializedProperty AspectRatioProperty
		{
			get
			{
				return this.mAspectRatio;
			}
		}

		public float AspectRatio
		{
			get
			{
				return this.mAspectRatio.floatValue;
			}
			set
			{
				this.mAspectRatio.floatValue = value;
			}
		}

		public SerializedProperty ImageTargetTypeProperty
		{
			get
			{
				return this.mImageTargetType;
			}
		}

		public ImageTargetType ImageTargetType
		{
			get
			{
				return (ImageTargetType)this.mImageTargetType.enumValueIndex;
			}
			set
			{
				this.mImageTargetType.enumValueIndex = (int)value;
			}
		}

		public SerializedProperty WidthProperty
		{
			get
			{
				return this.mWidth;
			}
		}

		public float Width
		{
			get
			{
				return this.mWidth.floatValue;
			}
			set
			{
				this.mWidth.floatValue = value;
			}
		}

		public SerializedProperty HeightProperty
		{
			get
			{
				return this.mHeight;
			}
		}

		public float Height
		{
			get
			{
				return this.mWidth.floatValue;
			}
			set
			{
				this.mWidth.floatValue = value;
			}
		}

		public SerializedImageTarget(SerializedObject target) : base(target)
		{
			this.mAspectRatio = target.FindProperty("mAspectRatio");
			this.mImageTargetType = target.FindProperty("mImageTargetType");
			this.mWidth = this.mSerializedObject.FindProperty("mWidth");
			this.mHeight = this.mSerializedObject.FindProperty("mHeight");
		}

		public List<ImageTargetAbstractBehaviour> GetBehaviours()
		{
			List<ImageTargetAbstractBehaviour> list = new List<ImageTargetAbstractBehaviour>();
            UnityEngine.Object[] targetObjects = this.mSerializedObject.targetObjects;
			for (int i = 0; i < targetObjects.Length; i++)
			{
                UnityEngine.Object @object = targetObjects[i];
				list.Add((ImageTargetAbstractBehaviour)@object);
			}
			return list;
		}
	}
}
