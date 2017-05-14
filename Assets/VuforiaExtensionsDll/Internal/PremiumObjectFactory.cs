using System;

namespace Vuforia
{
	internal class PremiumObjectFactory
	{
		private class NullPremiumObjectFactory : IPremiumObjectFactory
		{
			public T CreateReconstruction<T>() where T : class, Reconstruction
			{
				return default(T);
			}
		}

		private static IPremiumObjectFactory sInstance;

		internal static IPremiumObjectFactory Instance
		{
			get
			{
				if (PremiumObjectFactory.sInstance == null)
				{
					PremiumObjectFactory.sInstance = new PremiumObjectFactory.NullPremiumObjectFactory();
				}
				return PremiumObjectFactory.sInstance;
			}
			set
			{
				PremiumObjectFactory.sInstance = value;
			}
		}
	}
}
