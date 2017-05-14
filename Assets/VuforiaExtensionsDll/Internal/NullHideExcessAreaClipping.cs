using System;
using UnityEngine;

namespace Vuforia
{
	internal class NullHideExcessAreaClipping : IExcessAreaClipping
	{
		public void SetPlanesRenderingActive(bool activeflag)
		{
		}

		public bool IsPlanesRenderingActive()
		{
			return false;
		}

		public void OnPreCull()
		{
		}

		public void OnPostRender()
		{
		}

		public void Start()
		{
		}

		public void OnDestroy()
		{
		}

		public void Update(Vector3 planeOffset)
		{
		}
	}
}
