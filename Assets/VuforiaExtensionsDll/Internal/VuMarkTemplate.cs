using System;
using UnityEngine;

namespace Vuforia
{
	public interface VuMarkTemplate : ObjectTarget, ExtendedTrackable, Trackable
	{
		string VuMarkUserData
		{
			get;
		}

		Vector2 Origin
		{
			get;
		}

		bool TrackingFromRuntimeAppearance
		{
			get;
			set;
		}
	}
}
