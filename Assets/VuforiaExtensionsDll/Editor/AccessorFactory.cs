using System;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	internal class AccessorFactory
	{
		public static TrackableAccessor Create(TrackableBehaviour target)
		{
			if (target is ImageTargetAbstractBehaviour)
			{
				return new ImageTargetAccessor((ImageTargetAbstractBehaviour)target);
			}
			if (target is MultiTargetAbstractBehaviour)
			{
				return new MultiTargetAccessor((MultiTargetAbstractBehaviour)target);
			}
			if (target is CylinderTargetAbstractBehaviour)
			{
				return new CylinderTargetAccessor((CylinderTargetAbstractBehaviour)target);
			}
			if (target is VuMarkAbstractBehaviour)
			{
				return new VuMarkAccessor((VuMarkAbstractBehaviour)target);
			}
			if (target is ObjectTargetAbstractBehaviour)
			{
				return new ObjectTargetAccessor((ObjectTargetAbstractBehaviour)target);
			}
			Debug.LogWarning(target.GetType().ToString() + " is not derived from TrackableBehaviour.");
			return null;
		}
	}
}
