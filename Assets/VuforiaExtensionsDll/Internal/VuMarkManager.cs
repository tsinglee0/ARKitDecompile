using System;
using System.Collections.Generic;

namespace Vuforia
{
	public abstract class VuMarkManager
	{
		public abstract IEnumerable<VuMarkTarget> GetActiveVuMarks();

		public abstract IEnumerable<VuMarkAbstractBehaviour> GetActiveBehaviours(VuMarkTarget vuMark);

		public abstract IEnumerable<VuMarkAbstractBehaviour> GetActiveBehaviours();

		public abstract IEnumerable<VuMarkAbstractBehaviour> GetAllBehaviours();

		public abstract void RegisterVuMarkDetectedCallback(Action<VuMarkTarget> callback);

		public abstract void UnregisterVuMarkDetectedCallback(Action<VuMarkTarget> callback);

		public abstract void RegisterVuMarkLostCallback(Action<VuMarkTarget> callback);

		public abstract void UnregisterVuMarkLostCallback(Action<VuMarkTarget> callback);

		public abstract void RegisterVuMarkBehaviourDetectedCallback(Action<VuMarkAbstractBehaviour> callback);

		public abstract void UnregisterVuMarkBehaviourDetectedCallback(Action<VuMarkAbstractBehaviour> callback);
	}
}
