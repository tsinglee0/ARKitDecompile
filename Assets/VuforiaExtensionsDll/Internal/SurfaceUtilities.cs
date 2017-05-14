using System;
using UnityEngine;

namespace Vuforia
{
	public static class SurfaceUtilities
	{
		private static ScreenOrientation mScreenOrientation;

		public static void OnSurfaceCreated()
		{
			VuforiaRenderer.InternalInstance.UnityRenderEvent(VuforiaRendererImpl.RenderEvent.SURFACE_CREATED);
		}

		public static void OnSurfaceDeinit()
		{
			VuforiaRenderer.InternalInstance.UnityRenderEvent(VuforiaRendererImpl.RenderEvent.RENDERER_DEINIT);
		}

		public static bool HasSurfaceBeenRecreated()
		{
			return VuforiaWrapper.Instance.HasSurfaceBeenRecreated() == 1;
		}

		public static void OnSurfaceChanged(int screenWidth, int screenHeight)
		{
			VuforiaUnityImpl.SetRendererDirty();
			VuforiaWrapper.Instance.OnSurfaceChanged(screenWidth, screenHeight);
		}

		public static void SetSurfaceOrientation(ScreenOrientation screenOrientation)
		{
			SurfaceUtilities.mScreenOrientation = screenOrientation;
		}

		public static ScreenOrientation GetSurfaceOrientation()
		{
			return SurfaceUtilities.mScreenOrientation;
		}
	}
}
