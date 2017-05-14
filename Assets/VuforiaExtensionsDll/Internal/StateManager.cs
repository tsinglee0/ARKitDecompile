using System;
using System.Collections.Generic;

namespace Vuforia
{
	public abstract class StateManager
	{
		public abstract IEnumerable<TrackableBehaviour> GetActiveTrackableBehaviours();

		public abstract IEnumerable<TrackableBehaviour> GetTrackableBehaviours();

		public abstract void DestroyTrackableBehavioursForTrackable(Trackable trackable, bool destroyGameObjects = true);

		public abstract void ReassociateTrackables();

		public abstract WordManager GetWordManager();

		public abstract VuMarkManager GetVuMarkManager();
	}
}
