using System;
using System.Collections.Generic;

namespace Vuforia
{
	public class ViewerParametersList : IViewerParametersList
	{
		private IntPtr mNativeVPL;

		private static ViewerParametersList mListForAuthoringTools;

		public static ViewerParametersList ListForAuthoringTools
		{
			get
			{
				if (ViewerParametersList.mListForAuthoringTools == null)
				{
					Type typeFromHandle = typeof(Device);
					lock (typeFromHandle)
					{
						if (ViewerParametersList.mListForAuthoringTools == null)
						{
							ViewerParametersList.mListForAuthoringTools = new ViewerParametersList(VuforiaWrapper.CamIndependentInstance.ViewerParametersList_GetListForAuthoringTools());
						}
					}
				}
				return ViewerParametersList.mListForAuthoringTools;
			}
		}

		internal ViewerParametersList(IntPtr intPtr)
		{
			this.mNativeVPL = intPtr;
		}

		public void SetSDKFilter(string filter)
		{
			VuforiaWrapper.CamIndependentInstance.ViewerParametersList_SetSDKFilter(this.mNativeVPL, filter);
		}

		public int Size()
		{
			return VuforiaWrapper.CamIndependentInstance.ViewerParametersList_Size(this.mNativeVPL);
		}

		public IViewerParameters Get(int idx)
		{
			IntPtr intPtr = VuforiaWrapper.Instance.ViewerParametersList_GetByIndex(this.mNativeVPL, idx);
			if (intPtr != IntPtr.Zero)
			{
				return new ViewerParameters(intPtr);
			}
			return null;
		}

		public IViewerParameters Get(string name, string manufacturer)
		{
			IntPtr intPtr = VuforiaWrapper.CamIndependentInstance.ViewerParametersList_GetByNameManufacturer(this.mNativeVPL, name, manufacturer);
			if (intPtr != IntPtr.Zero)
			{
				return new ViewerParameters(intPtr);
			}
			return null;
		}

		public IEnumerable<IViewerParameters> GetAllViewers()
		{
			List<IViewerParameters> list = new List<IViewerParameters>(this.Size());
			IntPtr intPtr = VuforiaWrapper.CamIndependentInstance.ViewerParametersList_Begin(this.mNativeVPL);
			while (intPtr != IntPtr.Zero)
			{
				list.Add(new ViewerParameters(VuforiaWrapper.CamIndependentInstance.ViewerParameters_copy(intPtr)));
				intPtr = VuforiaWrapper.CamIndependentInstance.ViewerParametersList_Next(this.mNativeVPL, intPtr);
			}
			return list;
		}
	}
}
