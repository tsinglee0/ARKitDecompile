using System;

namespace Vuforia
{
	internal interface IPremiumObjectFactory
	{
		T CreateReconstruction<T>() where T : class, Reconstruction;
	}
}
