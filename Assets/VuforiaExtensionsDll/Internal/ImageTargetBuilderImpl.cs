using System;
using UnityEngine;

namespace Vuforia
{
	internal class ImageTargetBuilderImpl : ImageTargetBuilder
	{
		private TrackableSource mTrackableSource;

		private bool mIsScanning;

		public override bool Build(string targetName, float screenSizeWidth)
		{
			if (targetName.Length > 64)
			{
				Debug.LogError("Invalid parameters to build User Defined Target:Target name exceeds 64 character limit");
				return false;
			}
			this.mTrackableSource = null;
			return VuforiaWrapper.Instance.ImageTargetBuilderBuild(targetName, screenSizeWidth) == 1;
		}

		public override void StartScan()
		{
			this.mIsScanning = true;
			VuforiaWrapper.Instance.ImageTargetBuilderStartScan();
		}

		public override void StopScan()
		{
			this.mIsScanning = false;
			VuforiaWrapper.Instance.ImageTargetBuilderStopScan();
		}

		public override ImageTargetBuilder.FrameQuality GetFrameQuality()
		{
			return (ImageTargetBuilder.FrameQuality)VuforiaWrapper.Instance.ImageTargetBuilderGetFrameQuality();
		}

		public override TrackableSource GetTrackableSource()
		{
			IntPtr intPtr = VuforiaWrapper.Instance.ImageTargetBuilderGetTrackableSource();
			if (this.mTrackableSource == null && intPtr != IntPtr.Zero)
			{
				this.mTrackableSource = new TrackableSourceImpl(intPtr);
			}
			return this.mTrackableSource;
		}

		internal override bool IsScanning()
		{
			return this.mIsScanning;
		}
	}
}
