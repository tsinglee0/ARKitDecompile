using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	public class SerializedVuMark : SerializedDataSetTrackable
	{
		private readonly SerializedProperty mAspectRatio;

		private readonly SerializedProperty mWidth;

		private readonly SerializedProperty mHeight;

		private readonly SerializedProperty mPreviewImage;

		private readonly SerializedProperty mIdType;

		private readonly SerializedProperty mIdLength;

		private readonly SerializedProperty mTrackingFromRuntimeAppearance;

		private readonly SerializedProperty mBoundingBox;

		private readonly SerializedProperty mOrigin;

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

		public SerializedProperty PreviewImageProperty
		{
			get
			{
				return this.mPreviewImage;
			}
		}

		public string PreviewImage
		{
			get
			{
				return this.mPreviewImage.stringValue;
			}
			set
			{
				this.mPreviewImage.stringValue = value;
			}
		}

		public SerializedProperty IdTypeProperty
		{
			get
			{
				return this.mIdType;
			}
		}

		public InstanceIdType IdType
		{
			get
			{
				return (InstanceIdType)this.mIdType.enumValueIndex;
			}
			set
			{
				this.mIdType.enumValueIndex = (int)value;
			}
		}

		public SerializedProperty IdLengthProperty
		{
			get
			{
				return this.mIdLength;
			}
		}

		public int IdLength
		{
			get
			{
				return this.mIdLength.intValue;
			}
			set
			{
				this.mIdLength.intValue = value;
			}
		}

		public SerializedProperty BoundingBoxProperty
		{
			get
			{
				return this.mBoundingBox;
			}
		}

		public Rect BoundingBox
		{
			get
			{
				return this.mBoundingBox.rectValue;
			}
			set
			{
				this.mBoundingBox.rectValue = value;
			}
		}

		public SerializedProperty OriginProperty
		{
			get
			{
				return this.mOrigin;
			}
		}

		public Vector2 Origin
		{
			get
			{
				return this.mOrigin.vector2Value;
			}
			set
			{
				this.mOrigin.vector2Value = value;
			}
		}

		public SerializedProperty TrackingFromRuntimeAppearanceProperty
		{
			get
			{
				return this.mTrackingFromRuntimeAppearance;
			}
		}

		public bool TrackingFromRuntimeAppearance
		{
			get
			{
				return this.mTrackingFromRuntimeAppearance.boolValue;
			}
			set
			{
				this.mTrackingFromRuntimeAppearance.boolValue = value;
			}
		}

		public SerializedVuMark(SerializedObject target) : base(target)
		{
			this.mAspectRatio = target.FindProperty("mAspectRatio");
			this.mWidth = this.mSerializedObject.FindProperty("mWidth");
			this.mHeight = this.mSerializedObject.FindProperty("mHeight");
			this.mPreviewImage = this.mSerializedObject.FindProperty("mPreviewImage");
			this.mIdType = this.mSerializedObject.FindProperty("mIdType");
			this.mIdLength = this.mSerializedObject.FindProperty("mIdLength");
			this.mTrackingFromRuntimeAppearance = this.mSerializedObject.FindProperty("mTrackingFromRuntimeAppearance");
			this.mBoundingBox = this.mSerializedObject.FindProperty("mBoundingBox");
			this.mOrigin = this.mSerializedObject.FindProperty("mOrigin");
		}

		public List<VuMarkAbstractBehaviour> GetBehaviours()
		{
			List<VuMarkAbstractBehaviour> list = new List<VuMarkAbstractBehaviour>();
            UnityEngine.Object[] targetObjects = this.mSerializedObject.targetObjects;
			for (int i = 0; i < targetObjects.Length; i++)
			{
                UnityEngine.Object @object = targetObjects[i];
				list.Add((VuMarkAbstractBehaviour)@object);
			}
			return list;
		}
	}
}
