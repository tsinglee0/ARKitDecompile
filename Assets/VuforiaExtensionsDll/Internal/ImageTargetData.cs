using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ImageTargetData
	{
		public int id;

		public Vector3 size;
	}
}
