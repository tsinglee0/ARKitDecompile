using System;
using System.Runtime.InteropServices;

namespace Vuforia
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct RectangleData
	{
		public float leftTopX;

		public float leftTopY;

		public float rightBottomX;

		public float rightBottomY;
	}
}
