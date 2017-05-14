using System;
using UnityEngine;

namespace Vuforia
{
	public interface IViewerParameters
	{
		float GetVersion();

		string GetName();

		string GetManufacturer();

		ViewerButtonType GetButtonType();

		float GetScreenToLensDistance();

		float GetInterLensDistance();

		ViewerTrayAlignment GetTrayAlignment();

		float GetLensCentreToTrayDistance();

		int GetNumDistortionCoefficients();

		float GetDistortionCoefficient(int idx);

		Vector4 GetFieldOfView();

		bool ContainsMagnet();
	}
}
