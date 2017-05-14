using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	public class SerializedCylinderTarget : SerializedDataSetTrackable
	{
		private readonly SerializedProperty mSideLength;

		private readonly SerializedProperty mTopDiameter;

		private readonly SerializedProperty mBottomDiameter;

		private readonly SerializedProperty mTopDiameterRatio;

		private readonly SerializedProperty mBottomDiameterRatio;

		public SerializedProperty SideLengthProperty
		{
			get
			{
				return this.mSideLength;
			}
		}

		public float SideLength
		{
			get
			{
				return this.mSideLength.floatValue;
			}
			set
			{
				this.mSideLength.floatValue = value;
			}
		}

		public SerializedProperty TopDiameterProperty
		{
			get
			{
				return this.mTopDiameter;
			}
		}

		public float TopDiameter
		{
			get
			{
				return this.mTopDiameter.floatValue;
			}
			set
			{
				this.mTopDiameter.floatValue = value;
			}
		}

		public SerializedProperty BottomDiameterProperty
		{
			get
			{
				return this.mBottomDiameter;
			}
		}

		public float BottomDiameter
		{
			get
			{
				return this.mBottomDiameter.floatValue;
			}
			set
			{
				this.mBottomDiameter.floatValue = value;
			}
		}

		public SerializedProperty TopDiameterRatioProperty
		{
			get
			{
				return this.mTopDiameterRatio;
			}
		}

		public float TopDiameterRatio
		{
			get
			{
				return this.mTopDiameterRatio.floatValue;
			}
			set
			{
				this.mTopDiameterRatio.floatValue = value;
			}
		}

		public SerializedProperty BottomDiameterRatioProperty
		{
			get
			{
				return this.mBottomDiameterRatio;
			}
		}

		public float BottomDiameterRatio
		{
			get
			{
				return this.mBottomDiameterRatio.floatValue;
			}
			set
			{
				this.mBottomDiameterRatio.floatValue = value;
			}
		}

		public SerializedCylinderTarget(SerializedObject target) : base(target)
		{
			this.mSideLength = this.mSerializedObject.FindProperty("mSideLength");
			this.mTopDiameter = this.mSerializedObject.FindProperty("mTopDiameter");
			this.mBottomDiameter = this.mSerializedObject.FindProperty("mBottomDiameter");
			this.mTopDiameterRatio = this.mSerializedObject.FindProperty("mTopDiameterRatio");
			this.mBottomDiameterRatio = this.mSerializedObject.FindProperty("mBottomDiameterRatio");
		}

		public List<CylinderTargetAbstractBehaviour> GetBehaviours()
		{
			List<CylinderTargetAbstractBehaviour> list = new List<CylinderTargetAbstractBehaviour>();
			UnityEngine.Object[] targetObjects = this.mSerializedObject.targetObjects;
			for (int i = 0; i < targetObjects.Length; i++)
			{
                UnityEngine.Object @object = targetObjects[i];
				list.Add((CylinderTargetAbstractBehaviour)@object);
			}
			return list;
		}
	}
}
