using System;
using System.Runtime.InteropServices;

namespace Vuforia
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct SimpleTargetData
	{
		public int id;

		internal int unused;
	}
}
