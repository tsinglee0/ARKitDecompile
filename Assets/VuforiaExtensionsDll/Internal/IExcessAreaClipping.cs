using System;
using UnityEngine;

namespace Vuforia
{
	internal interface IExcessAreaClipping
	{
		void SetPlanesRenderingActive(bool activeflag);

		bool IsPlanesRenderingActive();

		void OnPreCull();

		void OnPostRender();

		void Start();

		void OnDestroy();

		void Update(Vector3 planeOffset);
	}
}
