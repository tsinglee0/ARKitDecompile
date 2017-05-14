using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class VuforiaRendererImpl : VuforiaRenderer
	{
		internal enum RenderEvent
		{
			NO_OP = 100,
			SURFACE_CREATED,
			RENDERER_BEGIN,
			RENDERER_BEGIN_AND_BIND,
			RENDERER_END,
			RENDERER_DEINIT
		}

		private VuforiaRenderer.VideoBGCfgData mVideoBGConfig;

		private bool mVideoBGConfigSet;

		private Texture mVideoBackgroundTexture;

		private bool mBackgroundTextureHasChanged;

		private VuforiaRenderer.VideoBackgroundReflection mLastSetReflection;

		private IntPtr mNativeRenderingCallback = IntPtr.Zero;

		public override Texture VideoBackgroundTexture
		{
			get
			{
				return this.mVideoBackgroundTexture;
			}
		}

		public override VuforiaRenderer.VideoBGCfgData GetVideoBackgroundConfig()
		{
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				return this.mVideoBGConfig;
			}
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VuforiaRenderer.VideoBGCfgData)));
			VuforiaWrapper.Instance.RendererGetVideoBackgroundCfg(intPtr);
			VuforiaRenderer.VideoBGCfgData arg_49_0 = (VuforiaRenderer.VideoBGCfgData)Marshal.PtrToStructure(intPtr, typeof(VuforiaRenderer.VideoBGCfgData));
			Marshal.FreeHGlobal(intPtr);
			return arg_49_0;
		}

		public override void ClearVideoBackgroundConfig()
		{
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				this.mVideoBGConfigSet = false;
			}
		}

		public override void SetVideoBackgroundConfig(VuforiaRenderer.VideoBGCfgData config)
		{
			this.SetVideoBackgroundConfigInternal(config);
			VuforiaUnityImpl.SetRendererDirty();
		}

		public override IntPtr createNativeTexture(int width, int height, int format)
		{
			return VuforiaWrapper.Instance.RendererCreateNativeTexture((uint)width, (uint)height, format);
		}

		public override bool SetVideoBackgroundTexture(Texture texture, int nativeTextureID)
		{
			this.mVideoBackgroundTexture = texture;
			this.mBackgroundTextureHasChanged = true;
			if (!VuforiaRuntimeUtilities.IsPlayMode())
			{
				if (texture != null)
				{
					((VuforiaManagerImpl)VuforiaManager.Instance).VideoBackgroundTextureSet = true;
					return VuforiaWrapper.Instance.RendererSetVideoBackgroundTextureID(nativeTextureID) != 0;
				}
				((VuforiaManagerImpl)VuforiaManager.Instance).VideoBackgroundTextureSet = false;
			}
			return true;
		}

		public override bool SetVideoBackgroundTexturePtr(Texture texture, IntPtr nativeTexturePtr)
		{
			this.mVideoBackgroundTexture = texture;
			this.mBackgroundTextureHasChanged = true;
			if (!VuforiaRuntimeUtilities.IsPlayMode())
			{
				if (texture != null)
				{
					((VuforiaManagerImpl)VuforiaManager.Instance).VideoBackgroundTextureSet = true;
					return VuforiaWrapper.Instance.RendererSetVideoBackgroundTexturePtr(nativeTexturePtr) != 0;
				}
				((VuforiaManagerImpl)VuforiaManager.Instance).VideoBackgroundTextureSet = false;
			}
			return true;
		}

		public override bool IsVideoBackgroundInfoAvailable()
		{
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				return this.mVideoBGConfigSet;
			}
			return VuforiaWrapper.Instance.RendererIsVideoBackgroundTextureInfoAvailable() != 0;
		}

		public override VuforiaRenderer.VideoTextureInfo GetVideoTextureInfo()
		{
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				return ((CameraDeviceImpl)CameraDevice.Instance).WebCam.GetVideoTextureInfo();
			}
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VuforiaRenderer.VideoTextureInfo)));
			VuforiaWrapper.Instance.RendererGetVideoBackgroundTextureInfo(intPtr);
			VuforiaRenderer.VideoTextureInfo arg_58_0 = (VuforiaRenderer.VideoTextureInfo)Marshal.PtrToStructure(intPtr, typeof(VuforiaRenderer.VideoTextureInfo));
			Marshal.FreeHGlobal(intPtr);
			return arg_58_0;
		}

		public override void Pause(bool pause)
		{
			((VuforiaManagerImpl)VuforiaManager.Instance).Pause(pause);
		}

		public override int GetRecommendedFps(VuforiaRenderer.FpsHint flags)
		{
			return VuforiaWrapper.Instance.RendererGetRecommendedFps((int)flags);
		}

		public override VuforiaRenderer.RendererAPI GetRendererAPI()
		{
			return (VuforiaRenderer.RendererAPI)VuforiaWrapper.Instance.RendererGetGraphicsAPI();
		}

		public void UnityRenderEvent(VuforiaRendererImpl.RenderEvent renderEvent)
		{
			if (this.mNativeRenderingCallback == IntPtr.Zero)
			{
				this.mNativeRenderingCallback = VuforiaWrapper.Instance.VuforiaGetRenderEventCallback();
			}
			if (this.mNativeRenderingCallback != IntPtr.Zero)
			{
				if (VuforiaRenderer.Instance.GetRendererAPI() == VuforiaRenderer.RendererAPI.DIRECTX3D11)
				{
					GL.PushMatrix();
				}
				GL.IssuePluginEvent(this.mNativeRenderingCallback, (int)renderEvent);
				if (VuforiaRenderer.Instance.GetRendererAPI() == VuforiaRenderer.RendererAPI.DIRECTX3D11)
				{
					GL.PopMatrix();
				}
			}
		}

		public bool HasBackgroundTextureChanged()
		{
			bool arg_0D_0 = this.mBackgroundTextureHasChanged;
			this.mBackgroundTextureHasChanged = false;
			return arg_0D_0;
		}

		public void SetVideoBackgroundConfigInternal(VuforiaRenderer.VideoBGCfgData config)
		{
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				config.reflection = VuforiaRenderer.VideoBackgroundReflection.OFF;
				this.mVideoBGConfig = config;
				this.mVideoBGConfigSet = true;
			}
			this.mLastSetReflection = config.reflection;
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VuforiaRenderer.VideoBGCfgData)));
			Marshal.StructureToPtr(config, intPtr, true);
			VuforiaWrapper.Instance.RendererSetVideoBackgroundCfg(intPtr);
			Marshal.FreeHGlobal(intPtr);
		}

		public VuforiaRenderer.VideoBackgroundReflection GetLastSetReflection()
		{
			return this.mLastSetReflection;
		}
	}
}
