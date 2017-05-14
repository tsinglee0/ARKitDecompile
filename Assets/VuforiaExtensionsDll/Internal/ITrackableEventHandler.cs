using System;

namespace Vuforia
{
	public interface ITrackableEventHandler
	{
		void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus);
	}
}
