using System;

namespace Vuforia
{
	public abstract class SurfaceAbstractBehaviour : SmartTerrainTrackableBehaviour, WorldCenterTrackableBehaviour
	{
		private Surface mSurface;

		public Surface Surface
		{
			get
			{
				return this.mSurface;
			}
		}

		protected override void InternalUnregisterTrackable()
		{
			this.mTrackable = (this.mSmartTerrainTrackable = (this.mSurface = null));
		}

		internal void InitializeSurface(Surface surface)
		{
			this.mSurface = surface;
			this.mSmartTerrainTrackable = surface;
			this.mTrackable = surface;
			this.mTrackableName = base.gameObject.name;
		}
	}
}
