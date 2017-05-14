using System;
using UnityEngine;

namespace Vuforia
{
	internal class PropImpl : SmartTerrainTrackableImpl, Prop, SmartTerrainTrackable, Trackable
	{
		protected OrientedBoundingBox3D mOrientedBoundingBox3D;

		public OrientedBoundingBox3D BoundingBox
		{
			get
			{
				return this.mOrientedBoundingBox3D;
			}
		}

		public PropImpl(int id, SmartTerrainTrackable parent) : base("Prop", id, parent)
		{
		}

		internal void SetMesh(int meshRev, Mesh mesh)
		{
			this.mMesh = mesh;
			this.mMeshRevision = meshRev;
		}

		internal void SetObb(OrientedBoundingBox3D obb3D)
		{
			this.mOrientedBoundingBox3D = obb3D;
		}
	}
}
