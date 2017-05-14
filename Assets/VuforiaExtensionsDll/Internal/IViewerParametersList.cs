using System;
using System.Collections.Generic;

namespace Vuforia
{
	public interface IViewerParametersList
	{
		int Size();

		IViewerParameters Get(int idx);

		IViewerParameters Get(string name, string manufacturer);

		void SetSDKFilter(string filter);

		IEnumerable<IViewerParameters> GetAllViewers();
	}
}
