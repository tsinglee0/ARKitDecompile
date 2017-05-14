using System;
using UnityEngine;

namespace Vuforia
{
	public interface Surface : SmartTerrainTrackable, Trackable
	{
		Rect BoundingBox
		{
			get;
		}

		Mesh GetNavMesh();

		int[] GetMeshBoundaries();

		float GetArea();
	}
}
