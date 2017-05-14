using System;
using UnityEngine;

namespace Vuforia
{
	public interface IHoloLensApiAbstraction
	{
		void SetFocusPoint(Vector3 point, Vector3 normal);

		void SetWorldAnchor(TrackableBehaviour trackableBehaviour, VuforiaManager.TrackableIdPair trackableID);

		void DeleteWorldAnchor(VuforiaManager.TrackableIdPair trackableID);

		void DeleteWorldAnchor(TrackableBehaviour trackableBehaviour);

		void SetSpatialAnchorTrackingCallback(Action<VuforiaManager.TrackableIdPair, bool> trackingCallback);
	}
}
