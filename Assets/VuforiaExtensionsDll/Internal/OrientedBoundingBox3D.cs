using System;
using UnityEngine;

namespace Vuforia
{
	public struct OrientedBoundingBox3D
	{
		public Vector3 Center
		{
			get;
			private set;
		}

		public Vector3 HalfExtents
		{
			get;
			private set;
		}

		public float RotationY
		{
			get;
			private set;
		}

		public OrientedBoundingBox3D(Vector3 center, Vector3 halfExtents, float rotationY)
		{
			this = default(OrientedBoundingBox3D);
			this.Center = center;
			this.HalfExtents = halfExtents;
			this.RotationY = rotationY;
		}
	}
}
