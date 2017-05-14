using System;
using UnityEngine;

namespace Vuforia
{
	public abstract class SmartTerrainTrackableBehaviour : TrackableBehaviour
	{
		protected SmartTerrainTrackable mSmartTerrainTrackable;

		protected bool mDisableAutomaticUpdates;

		[HideInInspector, SerializeField]
		protected MeshFilter mMeshFilterToUpdate;

		[HideInInspector, SerializeField]
		protected MeshCollider mMeshColliderToUpdate;

		public SmartTerrainTrackable SmartTerrainTrackable
		{
			get
			{
				return this.mSmartTerrainTrackable;
			}
		}

		public bool AutomaticUpdatesDisabled
		{
			get
			{
				return this.mDisableAutomaticUpdates;
			}
		}

		public virtual void UpdateMeshAndColliders()
		{
			if (this.mSmartTerrainTrackable != null && !this.mDisableAutomaticUpdates)
			{
				Mesh mesh = this.mSmartTerrainTrackable.GetMesh();
				if (mesh == null)
				{
					return;
				}
				if (this.mMeshFilterToUpdate != null && this.mMeshFilterToUpdate.sharedMesh != mesh)
				{
                    UnityEngine.Object.Destroy(this.mMeshFilterToUpdate.mesh);
					this.mMeshFilterToUpdate.sharedMesh = mesh;
				}
				if (this.mMeshColliderToUpdate != null)
				{
					this.mMeshColliderToUpdate.sharedMesh = null;
					this.mMeshColliderToUpdate.sharedMesh = mesh;
				}
			}
		}

		public void SetAutomaticUpdatesDisabled(bool disabled)
		{
			bool flag = this.mDisableAutomaticUpdates;
			this.mDisableAutomaticUpdates = disabled;
			if (!disabled & flag)
			{
				this.UpdateMeshAndColliders();
				return;
			}
			if (disabled && !flag && this.mMeshFilterToUpdate != null)
			{
				Mesh sharedMesh = UnityEngine.Object.Instantiate<Mesh>(this.mMeshFilterToUpdate.sharedMesh);
				this.mMeshFilterToUpdate.sharedMesh = sharedMesh;
				if (this.mMeshColliderToUpdate != null)
				{
					this.mMeshColliderToUpdate.sharedMesh = sharedMesh;
				}
			}
		}

		protected virtual void Start()
		{
			if (this.mMeshFilterToUpdate != null && !(this.mMeshFilterToUpdate.gameObject == base.gameObject))
			{
				if (this.mMeshFilterToUpdate.gameObject.transform.parent == base.gameObject.transform)
				{
					this.mMeshFilterToUpdate.transform.localPosition = new Vector3(0f, 0f, 0f);
					this.mMeshFilterToUpdate.transform.localScale = new Vector3(1f, 1f, 1f);
					this.mMeshFilterToUpdate.transform.localRotation = Quaternion.identity;
				}
				else
				{
					Debug.LogError("SmartTerrainTrackable id=" + this.mSmartTerrainTrackable.ID + ": mesh filter to update needs to be a component of the same game object or a child object!");
				}
			}
			if (this.mMeshColliderToUpdate != null && !(this.mMeshColliderToUpdate.gameObject == base.gameObject))
			{
				if (this.mMeshColliderToUpdate.gameObject.transform.parent == base.gameObject.transform)
				{
					this.mMeshColliderToUpdate.transform.localPosition = new Vector3(0f, 0f, 0f);
					this.mMeshColliderToUpdate.transform.localScale = new Vector3(1f, 1f, 1f);
					this.mMeshColliderToUpdate.transform.localRotation = Quaternion.identity;
					return;
				}
				Debug.LogError("SmartTerrainTrackable id=" + this.mSmartTerrainTrackable.ID + ": mesh collider to update needs to be a component of the same game object or a child object!");
			}
		}
	}
}
