using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
	public interface SmartTerrainTrackable : Trackable
	{
		int MeshRevision
		{
			get;
		}

		Vector3 LocalPosition
		{
			get;
		}

		SmartTerrainTrackable Parent
		{
			get;
		}

		IEnumerable<SmartTerrainTrackable> Children
		{
			get;
		}

		Mesh GetMesh();
	}
}
