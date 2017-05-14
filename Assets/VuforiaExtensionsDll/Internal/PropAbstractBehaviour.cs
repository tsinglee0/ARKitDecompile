using System;
using UnityEngine;

namespace Vuforia
{
	public abstract class PropAbstractBehaviour : SmartTerrainTrackableBehaviour
	{
		private Prop mProp;

		[HideInInspector, SerializeField]
		private BoxCollider mBoxColliderToUpdate;

		public Prop Prop
		{
			get
			{
				return this.mProp;
			}
		}

		public override void UpdateMeshAndColliders()
		{
			base.UpdateMeshAndColliders();
			if (this.mProp != null && !this.mDisableAutomaticUpdates && this.mBoxColliderToUpdate != null)
			{
				this.mBoxColliderToUpdate.gameObject.transform.localPosition = this.mProp.BoundingBox.Center;
				this.mBoxColliderToUpdate.gameObject.transform.localRotation = Quaternion.AngleAxis(this.mProp.BoundingBox.RotationY, Vector3.up);
				this.mBoxColliderToUpdate.gameObject.transform.localScale = this.mProp.BoundingBox.HalfExtents * 2f;
			}
		}

		protected override void Start()
		{
			base.Start();
			if (this.mBoxColliderToUpdate != null)
			{
				if (this.mBoxColliderToUpdate.gameObject.transform.parent == base.gameObject.transform)
				{
					this.mBoxColliderToUpdate.transform.localPosition = new Vector3(0f, 0f, 0f);
					this.mBoxColliderToUpdate.transform.localScale = new Vector3(1f, 1f, 1f);
					this.mBoxColliderToUpdate.transform.localRotation = Quaternion.identity;
					return;
				}
				Debug.LogError("Prop: box collider to update needs to be a child game object!");
			}
		}

		protected override void InternalUnregisterTrackable()
		{
			this.mTrackable = (this.mSmartTerrainTrackable = (this.mProp = null));
		}

		internal void InitializeProp(Prop prop)
		{
			this.mProp = prop;
			this.mSmartTerrainTrackable = prop;
			this.mTrackable = prop;
			this.mTrackableName = base.gameObject.name;
		}
	}
}
