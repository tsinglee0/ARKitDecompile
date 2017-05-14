using System;
using UnityEngine;

namespace Vuforia
{
	internal class WebCamTexAdaptorImpl : WebCamTexAdaptor
	{
		private WebCamTexture mWebCamTexture;

		private AsyncOperation mCheckCameraPermissions;

		public override bool DidUpdateThisFrame
		{
			get
			{
				return this.mWebCamTexture.didUpdateThisFrame;
			}
		}

		public override bool IsPlaying
		{
			get
			{
				return this.mWebCamTexture.isPlaying;
			}
		}

		public override Texture Texture
		{
			get
			{
				return this.mWebCamTexture;
			}
		}

		public WebCamTexAdaptorImpl(string deviceName, int requestedFPS, VuforiaRenderer.Vec2I requestedTextureSize)
		{
			this.mWebCamTexture = new WebCamTexture();
			this.mWebCamTexture.deviceName = deviceName;
			this.mWebCamTexture.requestedFPS = (float)requestedFPS;
			this.mWebCamTexture.requestedWidth = requestedTextureSize.x;
			this.mWebCamTexture.requestedHeight = requestedTextureSize.y;
		}

		public override void Play()
		{
			if (!Application.HasUserAuthorization( UserAuthorization.WebCam))
			{
				this.mCheckCameraPermissions = Application.RequestUserAuthorization( UserAuthorization.WebCam);
				return;
			}
			this.mWebCamTexture.Play();
		}

		public override void Stop()
		{
			this.mWebCamTexture.Stop();
		}

		public override void CheckPermissions()
		{
			if (this.mCheckCameraPermissions != null && this.mCheckCameraPermissions.isDone)
			{
				if (Application.HasUserAuthorization( UserAuthorization.WebCam))
				{
					this.mWebCamTexture.Play();
				}
				else
				{
					PlayModeEditorUtility.Instance.ShowErrorInMouseOverWindow("Please authorize web cam access to use Vuforia Play Mode or switch to a platform that does not require authorization, e.g. Android or iOS.");
				}
				this.mCheckCameraPermissions = null;
			}
		}
	}
}
