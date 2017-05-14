using System;
using System.Collections.Generic;

namespace Vuforia
{
	public abstract class SmartTerrainBuilder
	{
		public abstract bool Init();

		public abstract bool Deinit();

		public abstract IEnumerable<ReconstructionAbstractBehaviour> GetReconstructions();

		public abstract T CreateReconstruction<T>() where T : class, Reconstruction;

		public abstract bool AddReconstruction(ReconstructionAbstractBehaviour reconstructionBehaviour);

		public abstract bool RemoveReconstruction(ReconstructionAbstractBehaviour reconstruction);

		public abstract bool DestroyReconstruction(Reconstruction reconstruction);
	}
}
