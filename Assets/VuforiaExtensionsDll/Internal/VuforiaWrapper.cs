using System;
using UnityEngine;

namespace Vuforia
{
	public static class VuforiaWrapper
	{
		private static IVuforiaWrapper sWrapper;

		private static IVuforiaWrapper sCamIndependentWrapper;

		public static IVuforiaWrapper Instance
		{
			get
			{
				if (VuforiaWrapper.sWrapper == null)
				{
					UnityPlayer.Instance.LoadNativeLibraries();
					VuforiaWrapper.CreateRuntimeInstance();
				}
				return VuforiaWrapper.sWrapper;
			}
		}

		public static IVuforiaWrapper CamIndependentInstance
		{
			get
			{
				if (VuforiaWrapper.sCamIndependentWrapper == null)
				{
					if (VuforiaWrapper.Instance is VuforiaNullWrapper)
					{
						UnityPlayer.Instance.LoadNativeLibraries();
						VuforiaWrapper.CreateCamIndependentInstance();
					}
					else
					{
						VuforiaWrapper.sCamIndependentWrapper = VuforiaWrapper.Instance;
					}
				}
				return VuforiaWrapper.sCamIndependentWrapper;
			}
		}

		public static void CreateRuntimeInstance()
		{
			if (VuforiaRuntimeUtilities.IsVuforiaEnabled())
			{
				if (Application.platform != RuntimePlatform.IPhonePlayer)
				{
					VuforiaWrapper.sWrapper = new VuforiaNativeWrapper();
					return;
				}
			}
			else
			{
				VuforiaWrapper.sWrapper = new VuforiaNullWrapper();
			}
		}

		public static void CreateCamIndependentInstance()
		{
			if (VuforiaRuntimeUtilities.IsNativePluginSupportAvailable())
			{
				if (Application.platform != RuntimePlatform.IPhonePlayer)
				{
					VuforiaWrapper.sCamIndependentWrapper = new VuforiaNativeWrapper();
					return;
				}
			}
			else
			{
				VuforiaWrapper.sCamIndependentWrapper = new VuforiaNullWrapper();
			}
		}

		public static void SetImplementation(IVuforiaWrapper implementation)
		{
			VuforiaWrapper.sWrapper = implementation;
		}
	}
}
