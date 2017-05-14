using System;
using UnityEngine;

namespace Vuforia
{
	public interface IEditDataSetBehaviour
	{
		bool ExtendedTracking
		{
			get;
			set;
		}

		bool InitializeSmartTerrain
		{
			get;
			set;
		}

		ReconstructionFromTargetAbstractBehaviour ReconstructionToInitialize
		{
			get;
			set;
		}

		Vector3 SmartTerrainOccluderBoundsMin
		{
			get;
			set;
		}

		Vector3 SmartTerrainOccluderBoundsMax
		{
			get;
			set;
		}
	}
}
