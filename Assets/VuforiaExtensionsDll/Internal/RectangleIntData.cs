using System;
using System.Runtime.InteropServices;

namespace Vuforia
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct RectangleIntData
	{
		public int leftTopX;

		public int leftTopY;

		public int rightBottomX;

		public int rightBottomY;
	}
}
