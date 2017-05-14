using System;
using UnityEngine;

namespace Vuforia
{
	internal class SurfaceImpl : SmartTerrainTrackableImpl, Surface, SmartTerrainTrackable, Trackable
	{
		private Mesh mNavMesh;

		private int[] mMeshBoundaries = new int[0];

		private Rect mBoundingBox;

		private float mSurfaceArea;

		private bool mAreaNeedsUpdate;

		public Rect BoundingBox
		{
			get
			{
				return this.mBoundingBox;
			}
		}

		public SurfaceImpl(int id, SmartTerrainTrackable parent) : base("SmartTerrain", id, parent)
		{
		}

		internal void SetID(int trackableID)
		{
			base.ID = trackableID;
		}

		internal void SetMesh(int meshRev, Mesh mesh, Mesh navMesh, int[] meshBoundaries)
		{
			this.mMesh = mesh;
			this.mNavMesh = navMesh;
			this.mMeshRevision = meshRev;
			this.mMeshBoundaries = meshBoundaries;
			this.mAreaNeedsUpdate = true;
		}

		internal void SetBoundingBox(Rect boundingBox)
		{
			this.mBoundingBox = boundingBox;
			if (this.mMesh != null)
			{
				float num = float.PositiveInfinity;
				float num2 = float.PositiveInfinity;
				float num3 = float.NegativeInfinity;
				float num4 = float.NegativeInfinity;
				int[] array = this.mMeshBoundaries;
				for (int i = 0; i < array.Length; i++)
				{
					int num5 = array[i];
					Vector3 expr_56 = this.mMesh.vertices[num5];
					num = Mathf.Min(expr_56.x, num);
					num2 = Mathf.Min(expr_56.z, num2);
					num3 = Mathf.Max(expr_56.x, num3);
					num4 = Mathf.Max(expr_56.z, num4);
				}
				this.mBoundingBox = new Rect(num, num2, num3 - num, num4 - num2);
			}
		}

		public Mesh GetNavMesh()
		{
			return this.mNavMesh;
		}

		public int[] GetMeshBoundaries()
		{
			return this.mMeshBoundaries;
		}

		public float GetArea()
		{
			if (this.mAreaNeedsUpdate)
			{
				this.mSurfaceArea = 0f;
				if (this.mMesh != null)
				{
					Vector3[] vertices = this.mMesh.vertices;
					int num = this.mMeshBoundaries.Length - 1;
					int i = 0;
					while (i < this.mMeshBoundaries.Length)
					{
						this.mSurfaceArea += Vector3.Cross(vertices[this.mMeshBoundaries[i]], vertices[this.mMeshBoundaries[num]]).magnitude;
						num = i++;
					}
					this.mSurfaceArea *= 0.5f;
				}
				this.mAreaNeedsUpdate = false;
			}
			return this.mSurfaceArea;
		}
	}
}
