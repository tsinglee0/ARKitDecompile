using System;
using UnityEngine;

namespace Vuforia
{
	public interface IWebCam
	{
		bool DidUpdateThisFrame
		{
			get;
		}

		bool IsPlaying
		{
			get;
		}

		int ActualWidth
		{
			get;
		}

		int ActualHeight
		{
			get;
		}

		bool IsTextureSizeAvailable
		{
			get;
		}

		bool FlipHorizontally
		{
			get;
		}

		VuforiaRenderer.Vec2I ResampledTextureSize
		{
			get;
		}

		void StartCamera();

		void StopCamera();

		void ResetPlaying();

		Color32[] GetPixels32AndBufferFrame();

		void RenderFrame(int frameIndex);

		CameraDevice.VideoModeData GetVideoMode();

		VuforiaRenderer.VideoTextureInfo GetVideoTextureInfo();

		bool IsRendererDirty();

		void Update();
	}
}
