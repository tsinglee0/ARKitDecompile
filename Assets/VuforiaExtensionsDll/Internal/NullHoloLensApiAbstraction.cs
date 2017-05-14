using System;
using UnityEngine;

namespace Vuforia
{
	internal class NullHoloLensApiAbstraction : IHoloLensApiAbstraction
	{
		public void SetFocusPoint(Vector3 point, Vector3 normal)
		{
		}

		public void SetWorldAnchor(TrackableBehaviour trackableBehaviour, VuforiaManager.TrackableIdPair trackableID)
		{
		}

		public void DeleteWorldAnchor(VuforiaManager.TrackableIdPair trackableID)
		{
		}

		public void DeleteWorldAnchor(TrackableBehaviour trackableBehaviour)
		{
		}

		public void SetSpatialAnchorTrackingCallback(Action<VuforiaManager.TrackableIdPair, bool> trackingCallback)
		{
		}
	}
}
