using System;
using UnityEngine;

namespace Vuforia
{
	public interface ExtendedTrackable : Trackable
	{
		bool StartExtendedTracking();

		bool StopExtendedTracking();

		Vector3 GetSize();

		float GetLargestSizeComponent();

		void SetSize(Vector3 size);
	}
}
