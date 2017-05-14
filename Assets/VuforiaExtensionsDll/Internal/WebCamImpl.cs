using System;
using UnityEngine;

namespace Vuforia
{
	internal class WebCamImpl : IWebCam
	{
		private readonly Camera[] mARCameras;

		private readonly int[] mOriginalCameraCullMask;

		private readonly WebCamTexAdaptor mWebCamTexture;

		private CameraDevice.VideoModeData mVideoModeData;

		private VuforiaRenderer.VideoTextureInfo mVideoTextureInfo;

		private TextureRenderer mTextureRenderer;

		private Texture2D mBufferReadTexture;

		private Rect mReadPixelsRect;

		private WebCamProfile.ProfileData mWebCamProfile;

		private readonly bool mFlipHorizontally;

		private bool mIsDirty = true;

		private int mLastFrameIdx = -1;

		private readonly int mRenderTextureLayer;

		private bool mWebcamPlaying;

		public bool DidUpdateThisFrame
		{
			get
			{
				return this.IsTextureSizeAvailable && this.mWebCamTexture.DidUpdateThisFrame;
			}
		}

		public bool IsPlaying
		{
			get
			{
				return this.mWebCamTexture.IsPlaying;
			}
		}

		public int ActualWidth
		{
			get
			{
				return this.mTextureRenderer.Width;
			}
		}

		public int ActualHeight
		{
			get
			{
				return this.mTextureRenderer.Height;
			}
		}

		public bool IsTextureSizeAvailable
		{
			get;
			private set;
		}

		public bool FlipHorizontally
		{
			get
			{
				return this.mFlipHorizontally;
			}
		}

		public VuforiaRenderer.Vec2I ResampledTextureSize
		{
			get
			{
				return this.mWebCamProfile.ResampledTextureSize;
			}
		}

		private void ComputeResampledTextureSize()
		{
			float num = (float)this.mWebCamTexture.Texture.height / (float)this.mWebCamTexture.Texture.width;
			float num2 = (float)this.mWebCamProfile.ResampledTextureSize.x * num;
			int v = (int)num2;
			if (Math.Abs(480f - num2) <= 1f)
			{
				v = 480;
			}
			if (Math.Abs(360f - num2) <= 1f)
			{
				v = 360;
			}
			WebCamProfile.ProfileData profileData = new WebCamProfile.ProfileData
			{
				RequestedTextureSize = this.mWebCamProfile.RequestedTextureSize,
				ResampledTextureSize = new VuforiaRenderer.Vec2I(this.mWebCamProfile.ResampledTextureSize.x, v),
				RequestedFPS = this.mWebCamProfile.RequestedFPS
			};
			this.mWebCamProfile = profileData;
		}

		public WebCamImpl(Camera[] arCameras, int renderTextureLayer, string webcamDeviceName, bool flipHorizontally)
		{
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				this.mRenderTextureLayer = renderTextureLayer;
				this.mARCameras = arCameras;
				this.mOriginalCameraCullMask = new int[this.mARCameras.Length];
				for (int i = 0; i < this.mARCameras.Length; i++)
				{
					this.mOriginalCameraCullMask[i] = this.mARCameras[i].cullingMask;
					Camera expr_60 = this.mARCameras[i];
					expr_60.cullingMask = expr_60.cullingMask & ~(1 << this.mRenderTextureLayer);
				}
				WebCamProfile webCamProfile = new WebCamProfile();
				if (VuforiaRuntimeUtilities.IsVuforiaEnabled() && WebCamTexture.devices.Length != 0)
				{
					bool flag = false;
					WebCamDevice[] devices = WebCamTexture.devices;
					for (int j = 0; j < devices.Length; j++)
					{
						WebCamDevice webCamDevice = devices[j];
						if (webCamDevice.name.Equals(webcamDeviceName))
						{
							flag = true;
						}
					}
					if (!flag)
					{
						webcamDeviceName = WebCamTexture.devices[0].name;
					}
					this.mWebCamProfile = webCamProfile.GetProfile(webcamDeviceName);
					this.mWebCamTexture = new WebCamTexAdaptorImpl(webcamDeviceName, this.mWebCamProfile.RequestedFPS, this.mWebCamProfile.RequestedTextureSize);
				}
				else
				{
					this.mWebCamProfile = webCamProfile.Default;
					this.mWebCamTexture = new NullWebCamTexAdaptor(this.mWebCamProfile.RequestedFPS, this.mWebCamProfile.RequestedTextureSize);
				}
				this.mFlipHorizontally = flipHorizontally;
			}
		}

		public void StartCamera()
		{
			this.mWebcamPlaying = true;
			if (!this.mWebCamTexture.IsPlaying)
			{
				this.mWebCamTexture.Play();
			}
		}

		public void StopCamera()
		{
			this.mWebcamPlaying = false;
			this.mWebCamTexture.Stop();
		}

		public void ResetPlaying()
		{
			if (this.mWebcamPlaying)
			{
				this.StartCamera();
				return;
			}
			this.StopCamera();
		}

		public Color32[] GetPixels32AndBufferFrame()
		{
			RenderTexture renderTexture = this.mTextureRenderer.Render();
			RenderTexture.active = renderTexture;
			this.mBufferReadTexture.ReadPixels(this.mReadPixelsRect, 0, 0, false);
			Color32[] arg_37_0 = this.mBufferReadTexture.GetPixels32();
			RenderTexture.ReleaseTemporary(renderTexture);
			return arg_37_0;
		}

		public void RenderFrame(int frameIndex)
		{
			if (this.mLastFrameIdx != frameIndex)
			{
				Texture videoBackgroundTexture = VuforiaRenderer.Instance.VideoBackgroundTexture;
				if (videoBackgroundTexture != null && videoBackgroundTexture is Texture2D)
				{
					Texture2D texture2D = (Texture2D)videoBackgroundTexture;
					ImageImpl imageImpl = (ImageImpl)CameraDevice.Instance.GetCameraImage(Image.PIXEL_FORMAT.RGBA8888);
					if (imageImpl != null)
					{
						if (texture2D.width != imageImpl.BufferWidth || texture2D.height != imageImpl.BufferHeight || texture2D.format !=  TextureFormat.RGB24)
						{
							texture2D.Resize(imageImpl.BufferWidth, imageImpl.BufferHeight, TextureFormat.RGB24, false);
						}
						texture2D.SetPixels32(imageImpl.GetPixels32());
						texture2D.Apply();
					}
				}
				this.mLastFrameIdx = frameIndex;
			}
		}

		public CameraDevice.VideoModeData GetVideoMode()
		{
			return this.mVideoModeData;
		}

		public VuforiaRenderer.VideoTextureInfo GetVideoTextureInfo()
		{
			return this.mVideoTextureInfo;
		}

		public bool IsRendererDirty()
		{
			bool expr_11 = this.IsTextureSizeAvailable && this.mIsDirty;
			if (expr_11)
			{
				this.mIsDirty = false;
			}
			return expr_11;
		}

		public void OnDestroy()
		{
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				for (int i = 0; i < this.mARCameras.Length; i++)
				{
					if (this.mARCameras[i] != null)
					{
						this.mARCameras[i].cullingMask = this.mOriginalCameraCullMask[i];
					}
				}
				this.IsTextureSizeAvailable = false;
				if (this.mTextureRenderer != null)
				{
					this.mTextureRenderer.Destroy();
				}
			}
		}

		public void Update()
		{
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				this.mWebCamTexture.CheckPermissions();
				if (!this.IsTextureSizeAvailable && this.mWebCamTexture.DidUpdateThisFrame)
				{
					this.IsTextureSizeAvailable = true;
					if (this.mWebCamProfile.ResampledTextureSize.y == 0)
					{
						this.ComputeResampledTextureSize();
						VuforiaWrapper.Instance.CameraDeviceStopCamera();
						VuforiaWrapper.Instance.CameraDeviceDeinitCamera();
						VuforiaWrapper.Instance.CameraDeviceSetCameraConfiguration(this.mWebCamProfile.ResampledTextureSize.x, this.mWebCamProfile.ResampledTextureSize.y);
						VuforiaWrapper.Instance.CameraDeviceInitCamera(1);
						VuforiaWrapper.Instance.CameraDeviceSelectVideoMode(-1);
						VuforiaWrapper.Instance.CameraDeviceStartCamera();
					}
					VuforiaRenderer.Vec2I resampledTextureSize = this.mWebCamProfile.ResampledTextureSize;
					this.mVideoModeData = new CameraDevice.VideoModeData
					{
						width = resampledTextureSize.x,
						height = resampledTextureSize.y,
						frameRate = (float)this.mWebCamProfile.RequestedFPS
					};
					this.mVideoTextureInfo = new VuforiaRenderer.VideoTextureInfo
					{
						imageSize = resampledTextureSize,
						textureSize = resampledTextureSize
					};
					this.mTextureRenderer = new TextureRenderer(this.mWebCamTexture.Texture, this.mRenderTextureLayer, resampledTextureSize);
					this.mBufferReadTexture = new Texture2D(resampledTextureSize.x, resampledTextureSize.y);
					this.mReadPixelsRect = new Rect(0f, 0f, (float)resampledTextureSize.x, (float)resampledTextureSize.y);
				}
			}
		}
	}
}
