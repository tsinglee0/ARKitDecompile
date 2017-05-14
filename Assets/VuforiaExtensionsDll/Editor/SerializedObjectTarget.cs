using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	public class SerializedObjectTarget : SerializedDataSetTrackable
	{
		private readonly SerializedProperty mAspectRatioXY;

		private readonly SerializedProperty mAspectRatioXZ;

		private readonly SerializedProperty mShowBoundingBox;

		private readonly SerializedProperty mBBoxMin;

		private readonly SerializedProperty mBBoxMax;

		private readonly SerializedProperty mPreviewImage;

		private readonly SerializedProperty mLength;

		private readonly SerializedProperty mWidth;

		private readonly SerializedProperty mHeight;

		public SerializedProperty AspectRatioXYProperty
		{
			get
			{
				return this.mAspectRatioXY;
			}
		}

		public float AspectRatioXY
		{
			get
			{
				return this.mAspectRatioXY.floatValue;
			}
			set
			{
				this.mAspectRatioXY.floatValue = value;
			}
		}

		public SerializedProperty AspectRatioXZProperty
		{
			get
			{
				return this.mAspectRatioXZ;
			}
		}

		public float AspectRatioXZ
		{
			get
			{
				return this.mAspectRatioXZ.floatValue;
			}
			set
			{
				this.mAspectRatioXZ.floatValue = value;
			}
		}

		public SerializedProperty ShowBoundingBoxProperty
		{
			get
			{
				return this.mShowBoundingBox;
			}
		}

		public bool ShowBoundingBox
		{
			get
			{
				return this.mShowBoundingBox.boolValue;
			}
			set
			{
				this.mShowBoundingBox.boolValue = value;
			}
		}

		public SerializedProperty BBoxMinProperty
		{
			get
			{
				return this.mBBoxMin;
			}
		}

		public Vector3 BBoxMin
		{
			get
			{
				return this.mBBoxMin.vector3Value;
			}
			set
			{
				this.mBBoxMin.vector3Value = value;
			}
		}

		public SerializedProperty BBoxMaxProperty
		{
			get
			{
				return this.mBBoxMax;
			}
		}

		public Vector3 BBoxMax
		{
			get
			{
				return this.mBBoxMax.vector3Value;
			}
			set
			{
				this.mBBoxMax.vector3Value = value;
			}
		}

		public SerializedProperty PreviewImageProperty
		{
			get
			{
				return this.mPreviewImage;
			}
		}

		public Texture2D PreviewImage
		{
			get
			{
				return this.mPreviewImage.objectReferenceValue as Texture2D;
			}
			set
			{
				this.mPreviewImage.objectReferenceValue = value;
			}
		}

		public SerializedProperty LengthProperty
		{
			get
			{
				return this.mLength;
			}
		}

		public float Length
		{
			get
			{
				return this.mLength.floatValue;
			}
			set
			{
				this.mLength.floatValue = value;
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
				return this.mHeight.floatValue;
			}
			set
			{
				this.mHeight.floatValue = value;
			}
		}

		public SerializedObjectTarget(SerializedObject target) : base(target)
		{
			this.mAspectRatioXY = target.FindProperty("mAspectRatioXY");
			this.mAspectRatioXZ = target.FindProperty("mAspectRatioXZ");
			this.mShowBoundingBox = target.FindProperty("mShowBoundingBox");
			this.mBBoxMin = target.FindProperty("mBBoxMin");
			this.mBBoxMax = target.FindProperty("mBBoxMax");
			this.mPreviewImage = target.FindProperty("mPreviewImage");
			this.mLength = target.FindProperty("mLength");
			this.mWidth = target.FindProperty("mWidth");
			this.mHeight = target.FindProperty("mHeight");
		}

		public List<ObjectTargetAbstractBehaviour> GetBehaviours()
		{
			List<ObjectTargetAbstractBehaviour> list = new List<ObjectTargetAbstractBehaviour>();
            UnityEngine.Object[] targetObjects = this.mSerializedObject.targetObjects;
			for (int i = 0; i < targetObjects.Length; i++)
			{
                UnityEngine.Object @object = targetObjects[i];
				list.Add((ObjectTargetAbstractBehaviour)@object);
			}
			return list;
		}
	}
}
