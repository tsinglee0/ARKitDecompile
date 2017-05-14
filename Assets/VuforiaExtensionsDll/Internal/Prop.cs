using System;

namespace Vuforia
{
	public interface Prop : SmartTerrainTrackable, Trackable
	{
		OrientedBoundingBox3D BoundingBox
		{
			get;
		}
	}
}
