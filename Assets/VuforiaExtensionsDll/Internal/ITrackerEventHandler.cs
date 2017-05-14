using System;

namespace Vuforia
{
	[Obsolete("This interface will be removed with the next Vuforia release. Please use VuforiaBehaviour.RegisterVuforiaStartedCallback and RegisterTrackablesUpdatedCallback instead.")]
	public interface ITrackerEventHandler
	{
		void OnInitialized();

		void OnTrackablesUpdated();
	}
}
