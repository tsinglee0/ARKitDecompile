using System;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	public class SerializedSmartTerrainTrackable : SerializedTrackable
	{
		private readonly SerializedProperty mMeshFilterToUpdate;

		private readonly SerializedProperty mMeshColliderToUpdate;

		public SerializedProperty MeshFilterToUpdateProperty
		{
			get
			{
				return this.mMeshFilterToUpdate;
			}
		}

		public MeshFilter MeshFilterToUpdate
		{
			get
			{
				return (MeshFilter)this.mMeshFilterToUpdate.objectReferenceValue;
			}
			set
			{
				this.mMeshFilterToUpdate.objectReferenceValue = value;
			}
		}

		public SerializedProperty MeshColliderToUpdateProperty
		{
			get
			{
				return this.mMeshColliderToUpdate;
			}
		}

		public MeshCollider MeshColliderToUpdate
		{
			get
			{
				return (MeshCollider)this.mMeshColliderToUpdate.objectReferenceValue;
			}
			set
			{
				this.mMeshColliderToUpdate.objectReferenceValue = value;
			}
		}

		public SerializedSmartTerrainTrackable(SerializedObject target) : base(target)
		{
			this.mMeshFilterToUpdate = target.FindProperty("mMeshFilterToUpdate");
			this.mMeshColliderToUpdate = target.FindProperty("mMeshColliderToUpdate");
		}
	}
}
