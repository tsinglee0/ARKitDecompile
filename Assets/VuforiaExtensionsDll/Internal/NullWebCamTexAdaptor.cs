using System;
using UnityEngine;

namespace Vuforia
{
	internal class NullWebCamTexAdaptor : WebCamTexAdaptor
	{
		private readonly Texture2D mTexture;

		private bool mPseudoPlaying = true;

		private readonly double mMsBetweenFrames;

		private DateTime mLastFrame;

		private const string ERROR_MSG = "No camera connected!\nTo run your application using Play Mode, please connect a webcam to your computer.";

		public override bool DidUpdateThisFrame
		{
			get
			{
				if ((DateTime.Now - this.mLastFrame).TotalMilliseconds > this.mMsBetweenFrames)
				{
					this.mLastFrame = DateTime.Now;
					return true;
				}
				return false;
			}
		}

		public override bool IsPlaying
		{
			get
			{
				return this.mPseudoPlaying;
			}
		}

		public override Texture Texture
		{
			get
			{
				return this.mTexture;
			}
		}

		public NullWebCamTexAdaptor(int requestedFPS, VuforiaRenderer.Vec2I requestedTextureSize)
		{
			this.mTexture = new Texture2D(requestedTextureSize.x, requestedTextureSize.y);
			this.mMsBetweenFrames = 1000.0 / (double)requestedFPS;
			this.mLastFrame = DateTime.Now - TimeSpan.FromDays(1.0);
			if (VuforiaRuntimeUtilities.IsVuforiaEnabled())
			{
				PlayModeEditorUtility.Instance.DisplayDialog("Error occurred!", "No camera connected!\nTo run your application using Play Mode, please connect a webcam to your computer.", "Ok");
				Debug.LogError("No camera connected!\nTo run your application using Play Mode, please connect a webcam to your computer.");
			}
		}

		public override void Play()
		{
			this.mPseudoPlaying = true;
		}

		public override void Stop()
		{
			this.mPseudoPlaying = false;
		}

		public override void CheckPermissions()
		{
		}
	}
}
