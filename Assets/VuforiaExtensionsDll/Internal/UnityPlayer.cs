using System;

namespace Vuforia
{
	internal static class UnityPlayer
	{
		private static IUnityPlayer sPlayer;

		public static IUnityPlayer Instance
		{
			get
			{
				if (UnityPlayer.sPlayer == null)
				{
					UnityPlayer.sPlayer = new NullUnityPlayer();
				}
				return UnityPlayer.sPlayer;
			}
		}

		public static void SetImplementation(IUnityPlayer implementation)
		{
			UnityPlayer.sPlayer = implementation;
		}
	}
}
