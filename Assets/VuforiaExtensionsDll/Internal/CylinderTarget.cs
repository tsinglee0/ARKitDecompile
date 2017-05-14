using System;

namespace Vuforia
{
	public interface CylinderTarget : ObjectTarget, ExtendedTrackable, Trackable
	{
		float GetSideLength();

		float GetTopDiameter();

		float GetBottomDiameter();

		bool SetSideLength(float sideLength);

		bool SetTopDiameter(float topDiameter);

		bool SetBottomDiameter(float bottomDiameter);
	}
}
