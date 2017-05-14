using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
	internal abstract class SmartTerrainTrackableImpl : TrackableImpl, SmartTerrainTrackable, Trackable
	{
		protected readonly List<SmartTerrainTrackable> mChildren = new List<SmartTerrainTrackable>();

		protected Mesh mMesh;

		protected int mMeshRevision;

		protected VuforiaManagerImpl.PoseData mLocalPose;

		public IEnumerable<SmartTerrainTrackable> Children
		{
			get
			{
				return this.mChildren;
			}
		}

		public int MeshRevision
		{
			get
			{
				return this.mMeshRevision;
			}
		}

		public SmartTerrainTrackable Parent
		{
			get;
			private set;
		}

		public Vector3 LocalPosition
		{
			get
			{
				return new Vector3(this.mLocalPose.position.x, this.mLocalPose.position.z, -this.mLocalPose.position.y);
			}
		}

		protected SmartTerrainTrackableImpl(string name, int id, SmartTerrainTrackable parent) : base(name, id)
		{
			this.mMeshRevision = 0;
			this.Parent = parent;
			this.mLocalPose = new VuforiaManagerImpl.PoseData
			{
				orientation = Quaternion.identity,
				position = Vector3.zero
			};
		}

		public Mesh GetMesh()
		{
			return this.mMesh;
		}

		internal void SetLocalPose(VuforiaManagerImpl.PoseData localPose)
		{
			this.mLocalPose = localPose;
		}

		internal void DestroyMesh()
		{
			UnityEngine.Object.Destroy(this.mMesh);
		}

		internal void AddChild(SmartTerrainTrackable newChild)
		{
			if (!this.mChildren.Contains(newChild))
			{
				this.mChildren.Add(newChild);
			}
		}

		internal void RemoveChild(SmartTerrainTrackable removedChild)
		{
			if (this.mChildren.Contains(removedChild))
			{
				this.mChildren.Remove(removedChild);
			}
		}
	}
}
