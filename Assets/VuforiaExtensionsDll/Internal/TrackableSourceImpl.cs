using System;

namespace Vuforia
{
	internal class TrackableSourceImpl : TrackableSource
	{
		public IntPtr TrackableSourcePtr
		{
			get;
			private set;
		}

		public TrackableSourceImpl(IntPtr trackableSourcePtr)
		{
			this.TrackableSourcePtr = trackableSourcePtr;
		}
	}
}
