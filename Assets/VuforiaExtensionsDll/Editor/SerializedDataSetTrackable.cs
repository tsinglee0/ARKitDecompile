using System;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	public class SerializedDataSetTrackable : SerializedTrackable
	{
		private readonly SerializedProperty mDataSetPath;

		private readonly SerializedProperty mExtendedTracking;

		private readonly SerializedProperty mInitializeSmartTerrain;

		private readonly SerializedProperty mReconstructionToInitialize;

		private readonly SerializedProperty mSmartTerrainOccluderBoundsMin;

		private readonly SerializedProperty mSmartTerrainOccluderBoundsMax;

		private readonly SerializedProperty mIsSmartTerrainOccluderOffset;

		private readonly SerializedProperty mSmartTerrainOccluderOffset;

		private readonly SerializedProperty mSmartTerrainOccluderRotation;

		private readonly SerializedProperty mAutoSetOccluderFromTargetSize;

		public SerializedProperty DataSetPathProperty
		{
			get
			{
				return this.mDataSetPath;
			}
		}

		public string DataSetPath
		{
			get
			{
				return this.mDataSetPath.stringValue;
			}
			set
			{
				this.mDataSetPath.stringValue = value;
			}
		}

		public SerializedProperty ExtendedTrackingProperty
		{
			get
			{
				return this.mExtendedTracking;
			}
		}

		public bool ExtendedTracking
		{
			get
			{
				return this.mExtendedTracking.boolValue;
			}
			set
			{
				this.mExtendedTracking.boolValue = value;
			}
		}

		public SerializedProperty InitializeSmartTerrainProperty
		{
			get
			{
				return this.mInitializeSmartTerrain;
			}
		}

		public bool InitializeSmartTerrain
		{
			get
			{
				return this.mInitializeSmartTerrain.boolValue;
			}
			set
			{
				this.mInitializeSmartTerrain.boolValue = value;
			}
		}

		public SerializedProperty ReconstructionToInitializeProperty
		{
			get
			{
				return this.mReconstructionToInitialize;
			}
		}

		public ReconstructionFromTargetAbstractBehaviour ReconstructionToInitialize
		{
			get
			{
				return (ReconstructionFromTargetAbstractBehaviour)this.mReconstructionToInitialize.objectReferenceValue;
			}
			set
			{
				this.mReconstructionToInitialize.objectReferenceValue = value;
			}
		}

		public SerializedProperty SmartTerrainOccluderBoundsMinProperty
		{
			get
			{
				return this.mSmartTerrainOccluderBoundsMin;
			}
		}

		public Vector3 SmartTerrainOccluderBoundsMin
		{
			get
			{
				return this.mSmartTerrainOccluderBoundsMin.vector3Value;
			}
			set
			{
				this.mSmartTerrainOccluderBoundsMin.vector3Value = value;
			}
		}

		public SerializedProperty SmartTerrainOccluderBoundsMaxProperty
		{
			get
			{
				return this.mSmartTerrainOccluderBoundsMax;
			}
		}

		public Vector3 SmartTerrainOccluderBoundsMax
		{
			get
			{
				return this.mSmartTerrainOccluderBoundsMax.vector3Value;
			}
			set
			{
				this.mSmartTerrainOccluderBoundsMax.vector3Value = value;
			}
		}

		public SerializedProperty IsSmartTerrainOccluderOffsetProperty
		{
			get
			{
				return this.mIsSmartTerrainOccluderOffset;
			}
		}

		public bool IsSmartTerrainOccluderOffset
		{
			get
			{
				return this.mIsSmartTerrainOccluderOffset.boolValue;
			}
			set
			{
				this.mIsSmartTerrainOccluderOffset.boolValue = value;
			}
		}

		public SerializedProperty SmartTerrainOccluderOffsetProperty
		{
			get
			{
				return this.mSmartTerrainOccluderOffset;
			}
		}

		public Vector3 SmartTerrainOccluderOffset
		{
			get
			{
				return this.mSmartTerrainOccluderOffset.vector3Value;
			}
			set
			{
				this.mSmartTerrainOccluderOffset.vector3Value = value;
			}
		}

		public SerializedProperty SmartTerrainOccluderRotationProperty
		{
			get
			{
				return this.mSmartTerrainOccluderRotation;
			}
		}

		public Quaternion SmartTerrainOccluderRotation
		{
			get
			{
				return this.mSmartTerrainOccluderRotation.quaternionValue;
			}
			set
			{
				this.mSmartTerrainOccluderRotation.quaternionValue = value;
			}
		}

		public SerializedProperty AutoSetOccluderFromTargetSizeProperty
		{
			get
			{
				return this.mAutoSetOccluderFromTargetSize;
			}
		}

		public bool AutoSetOccluderFromTargetSize
		{
			get
			{
				return this.mAutoSetOccluderFromTargetSize.boolValue;
			}
			set
			{
				this.mAutoSetOccluderFromTargetSize.boolValue = value;
			}
		}

		public SerializedDataSetTrackable(SerializedObject target) : base(target)
		{
			this.mDataSetPath = this.mSerializedObject.FindProperty("mDataSetPath");
			this.mExtendedTracking = this.mSerializedObject.FindProperty("mExtendedTracking");
			this.mInitializeSmartTerrain = this.mSerializedObject.FindProperty("mInitializeSmartTerrain");
			this.mReconstructionToInitialize = this.mSerializedObject.FindProperty("mReconstructionToInitialize");
			this.mSmartTerrainOccluderBoundsMin = this.mSerializedObject.FindProperty("mSmartTerrainOccluderBoundsMin");
			this.mSmartTerrainOccluderBoundsMax = this.mSerializedObject.FindProperty("mSmartTerrainOccluderBoundsMax");
			this.mIsSmartTerrainOccluderOffset = this.mSerializedObject.FindProperty("mIsSmartTerrainOccluderOffset");
			this.mSmartTerrainOccluderOffset = this.mSerializedObject.FindProperty("mSmartTerrainOccluderOffset");
			this.mSmartTerrainOccluderRotation = this.mSerializedObject.FindProperty("mSmartTerrainOccluderRotation");
			this.mAutoSetOccluderFromTargetSize = this.mSerializedObject.FindProperty("mAutoSetOccluderFromTargetSize");
		}

		public string GetDataSetName()
		{
			return DataSetTrackableBehaviour.GetDataSetName(this.mDataSetPath.stringValue);
		}

		public void SetDefaultOccluderBounds()
		{
			this.mSerializedObject.ApplyModifiedProperties();
            UnityEngine.Object[] targetObjects = this.mSerializedObject.targetObjects;
			for (int i = 0; i < targetObjects.Length; i++)
			{
				((DataSetTrackableBehaviour)targetObjects[i]).SetDefaultOccluderBounds();
			}
			this.mSerializedObject.Update();
			Debug.Log("default occluder " + this.SmartTerrainOccluderBoundsMax);
		}
	}
}
