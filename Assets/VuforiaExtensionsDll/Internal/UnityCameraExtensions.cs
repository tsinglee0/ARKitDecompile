using System;
using UnityEngine;

namespace Vuforia
{
	internal static class UnityCameraExtensions
	{
		private const float MAX_FAR_PLANE_FACTOR = 0.99f;

		private const float MIN_NEAR_PLANE_FACTOR = 1.01f;

		internal static int GetPixelHeightInt(this Camera camera)
		{
			return (int)camera.pixelRect.height;
		}

		internal static int GetPixelWidthInt(this Camera camera)
		{
			return (int)camera.pixelRect.width;
		}

		internal static float GetMaxDepthForVideoBackground(this Camera camera)
		{
			return camera.farClipPlane * 0.99f;
		}

		internal static float GetMinDepthForVideoBackground(this Camera camera)
		{
			return camera.nearClipPlane * 1.01f;
		}
	}
}
