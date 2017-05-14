using System;
using UnityEngine;

namespace Vuforia
{
	public interface ICustomViewerParameters : IViewerParameters
	{
		void SetButtonType(ViewerButtonType val);

		void SetScreenToLensDistance(float val);

		void SetInterLensDistance(float val);

		void SetTrayAlignment(ViewerTrayAlignment val);

		void SetLensCentreToTrayDistance(float val);

		void ClearDistortionCoefficients();

		void AddDistortionCoefficient(float val);

		void SetFieldOfView(Vector4 val);

		void SetContainsMagnet(bool val);
	}
}
