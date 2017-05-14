using System;

namespace Vuforia
{
	[Obsolete("This interface will be removed with the next Vuforia release. Please use ReconstructionBehaviour.Register...Callback instead.")]
	public interface ISmartTerrainEventHandler
	{
		void OnInitialized(SmartTerrainInitializationInfo initializationInfo);

		void OnPropCreated(Prop prop);

		void OnPropUpdated(Prop prop);

		void OnPropDeleted(Prop prop);

		void OnSurfaceCreated(Surface surface);

		void OnSurfaceUpdated(SurfaceAbstractBehaviour surfaceBehaviour);
	}
}
