using System;

namespace Vuforia
{
	public abstract class SmartTerrainTracker : Tracker
	{
		public abstract float ScaleToMillimeter
		{
			get;
		}

		public abstract SmartTerrainBuilder SmartTerrainBuilder
		{
			get;
		}

		public abstract bool SetScaleToMillimeter(float scaleFactor);
	}
}
