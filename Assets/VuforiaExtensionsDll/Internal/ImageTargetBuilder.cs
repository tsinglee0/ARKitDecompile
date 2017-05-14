using System;

namespace Vuforia
{
	public abstract class ImageTargetBuilder
	{
		public enum FrameQuality
		{
			FRAME_QUALITY_NONE = -1,
			FRAME_QUALITY_LOW,
			FRAME_QUALITY_MEDIUM,
			FRAME_QUALITY_HIGH
		}

		public abstract bool Build(string targetName, float screenSizeWidth);

		public abstract void StartScan();

		public abstract void StopScan();

		public abstract ImageTargetBuilder.FrameQuality GetFrameQuality();

		public abstract TrackableSource GetTrackableSource();

		internal abstract bool IsScanning();
	}
}
