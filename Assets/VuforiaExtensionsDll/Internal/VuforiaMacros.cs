using System;
using System.Runtime.InteropServices;

namespace Vuforia
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	internal struct VuforiaMacros
	{
		public const string PLATFORM_DLL_IOS = "__Internal";

		public const string PLATFORM_DLL = "VuforiaWrapper";
	}
}
