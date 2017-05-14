using System;

namespace Vuforia
{
	public interface VuMarkTarget : ObjectTarget, ExtendedTrackable, Trackable
	{
		InstanceId InstanceId
		{
			get;
		}

		VuMarkTemplate Template
		{
			get;
		}

		Image InstanceImage
		{
			get;
		}
	}
}
