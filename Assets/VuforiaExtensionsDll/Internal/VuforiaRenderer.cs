using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	public abstract class VuforiaRenderer
	{
		[Flags]
		public enum FpsHint
		{
			NONE = 0,
			NO_VIDEOBACKGROUND = 1,
			POWEREFFICIENCY = 2,
			FAST = 4
		}

		public enum VideoBackgroundReflection
		{
			DEFAULT,
			ON,
			OFF
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct VideoBGCfgData
		{
			public VuforiaRenderer.Vec2I position;

			public VuforiaRenderer.Vec2I size;

			public int enabled;

			internal int reflectionInteger;

			public VuforiaRenderer.VideoBackgroundReflection reflection
			{
				get
				{
					return (VuforiaRenderer.VideoBackgroundReflection)this.reflectionInteger;
				}
				set
				{
					this.reflectionInteger = (int)value;
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct Vec2I
		{
			public int x;

			public int y;

			public Vec2I(int v1, int v2)
			{
				this.x = v1;
				this.y = v2;
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct VideoTextureInfo
		{
			public VuforiaRenderer.Vec2I textureSize;

			public VuforiaRenderer.Vec2I imageSize;
		}

		public enum RendererAPI
		{
			GL_20 = 1,
			METAL,
			DIRECTX3D11 = 4,
			GL_30 = 8
		}

		private static VuforiaRenderer sInstance;

		public static VuforiaRenderer Instance
		{
			get
			{
				if (VuforiaRenderer.sInstance == null)
				{
					Type typeFromHandle = typeof(VuforiaRenderer);
					lock (typeFromHandle)
					{
						if (VuforiaRenderer.sInstance == null)
						{
							VuforiaRenderer.sInstance = new VuforiaRendererImpl();
						}
					}
				}
				return VuforiaRenderer.sInstance;
			}
		}

		internal static VuforiaRendererImpl InternalInstance
		{
			get
			{
				return (VuforiaRendererImpl)VuforiaRenderer.Instance;
			}
		}

		public abstract Texture VideoBackgroundTexture
		{
			get;
		}

		public abstract VuforiaRenderer.VideoBGCfgData GetVideoBackgroundConfig();

		public abstract void ClearVideoBackgroundConfig();

		public abstract void SetVideoBackgroundConfig(VuforiaRenderer.VideoBGCfgData config);

		public abstract IntPtr createNativeTexture(int width, int height, int format);

		public abstract bool SetVideoBackgroundTexture(Texture texture, int nativeTextureID);

		public abstract bool SetVideoBackgroundTexturePtr(Texture texture, IntPtr nativeTexturePtr);

		public abstract bool IsVideoBackgroundInfoAvailable();

		public abstract VuforiaRenderer.VideoTextureInfo GetVideoTextureInfo();

		public abstract void Pause(bool pause);

		public abstract int GetRecommendedFps(VuforiaRenderer.FpsHint flags);

		public abstract VuforiaRenderer.RendererAPI GetRendererAPI();
	}
}
