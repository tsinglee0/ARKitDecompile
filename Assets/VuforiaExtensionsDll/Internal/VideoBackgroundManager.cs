using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
	public class VideoBackgroundManager : ARController, IVideoBackgroundEventHandler
	{
		private HideExcessAreaAbstractBehaviour.CLIPPING_MODE mClippingMode;

		private Shader mMatteShader;

		private bool mVideoBackgroundEnabled = true;

		private Texture mTexture;

		private bool mVideoBgConfigChanged;

		private IntPtr mNativeTexturePtr;

		private static VideoBackgroundManager mInstance;

		private static object mPadlock = new object();

		public static VideoBackgroundManager Instance
		{
			get
			{
				if (VideoBackgroundManager.mInstance == null)
				{
					object obj = VideoBackgroundManager.mPadlock;
					lock (obj)
					{
						if (VideoBackgroundManager.mInstance == null)
						{
							VideoBackgroundManager.mInstance = new VideoBackgroundManager();
						}
					}
				}
				return VideoBackgroundManager.mInstance;
			}
		}

		public bool VideoBackgroundEnabled
		{
			get
			{
				return this.mVideoBackgroundEnabled;
			}
		}

		public HideExcessAreaAbstractBehaviour.CLIPPING_MODE ClippingMode
		{
			get
			{
				return this.mClippingMode;
			}
		}

		public Shader MatteShader
		{
			get
			{
				return this.mMatteShader;
			}
		}

		private VideoBackgroundManager()
		{
		}

		[RuntimeInitializeOnLoadMethod]
		public static void RegisterARController()
		{
			ARController.Register(VideoBackgroundManager.Instance);
		}

		public void SetVideoBackgroundEnabled(bool value)
		{
			this.mVideoBackgroundEnabled = value;
			if (Application.isPlaying)
			{
				this.UpdateVideoBackgroundEnabled();
			}
		}

		public void SetClippingMode(HideExcessAreaAbstractBehaviour.CLIPPING_MODE value)
		{
			this.mClippingMode = value;
		}

		public void SetMatteShader(Shader value)
		{
			this.mMatteShader = value;
		}

		internal void Initialize()
		{
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				this.mTexture = new Texture2D(0, 0, TextureFormat.RGB565, false);
			}
			else
			{
				int num = 1280;
				int num2 = 720;
				IntPtr intPtr = VuforiaRenderer.Instance.createNativeTexture(num, num2, 16);
				if (intPtr != IntPtr.Zero)
				{
					this.mTexture = Texture2D.CreateExternalTexture(num, num2, TextureFormat.RGBA32, true, true, intPtr);
				}
			}
			if (this.mTexture != null)
			{
				this.mTexture.filterMode = FilterMode.Bilinear;
				this.mTexture.wrapMode = TextureWrapMode.Clamp;
				this.mNativeTexturePtr = this.mTexture.GetNativeTexturePtr();
			}
		}

		protected override void Awake()
		{
			VuforiaAbstractConfiguration.VideoBackgroundConfiguration videoBackground = VuforiaAbstractConfiguration.Instance.VideoBackground;
			this.mClippingMode = videoBackground.ClippingMode;
			this.mMatteShader = videoBackground.MatteShader;
			this.mVideoBackgroundEnabled = videoBackground.VideoBackgroundEnabled;
		}

		protected override void Start()
		{
			VuforiaARController expr_05 = VuforiaARController.Instance;
			expr_05.RegisterVideoBgEventHandler(this);
			expr_05.RegisterVuforiaInitializedCallback(new Action(this.OnVuforiaInitialized));
		}

		protected override void Update()
		{
			if (this.mVideoBgConfigChanged)
			{
				bool flag = false;
				Texture videoBackgroundTexture = VuforiaRenderer.Instance.VideoBackgroundTexture;
				if (videoBackgroundTexture != null && videoBackgroundTexture != this.mTexture)
				{
					flag = true;
				}
				if (!VuforiaRuntimeUtilities.IsPlayMode())
				{
					CameraDevice.VideoModeData videoMode = CameraDevice.Instance.GetVideoMode();
					if ((this.mTexture == null || this.mTexture.GetNativeTexturePtr() != this.mNativeTexturePtr || this.mTexture.width != videoMode.width || this.mTexture.height != videoMode.height) && videoMode.width > 0 && videoMode.height > 0)
					{
						IntPtr intPtr = VuforiaRenderer.Instance.createNativeTexture(videoMode.width, videoMode.height, 16);
						if (intPtr != IntPtr.Zero)
						{
							Texture2D texture2D = Texture2D.CreateExternalTexture(videoMode.width, videoMode.height, TextureFormat.RGBA32, true, true, intPtr);
							texture2D.filterMode = FilterMode.Bilinear;
							texture2D.wrapMode = TextureWrapMode.Clamp;
							this.mNativeTexturePtr = texture2D.GetNativeTexturePtr();
							this.mTexture = texture2D;
						}
					}
				}
				if (!flag)
				{
					if (VuforiaRenderer.Instance.GetRendererAPI() == VuforiaRenderer.RendererAPI.METAL)
					{
						if (!VuforiaRenderer.Instance.SetVideoBackgroundTexturePtr(this.mTexture, this.mNativeTexturePtr))
						{
							Debug.Log("Failed to setVideoBackgroundTexturePtr " + this.mNativeTexturePtr.ToString());
						}
					}
					else if (VuforiaRenderer.Instance.GetRendererAPI() == VuforiaRenderer.RendererAPI.DIRECTX3D11)
					{
						if (!VuforiaRenderer.Instance.SetVideoBackgroundTexturePtr(this.mTexture, this.mNativeTexturePtr))
						{
							Debug.Log("Failed to setVideoBackgroundTexturePtr " + this.mNativeTexturePtr.ToString());
						}
					}
					else if (!VuforiaRenderer.Instance.SetVideoBackgroundTexture(this.mTexture, this.mNativeTexturePtr.ToInt32()))
					{
						Debug.Log("Failed to setVideoBackgroundTexture " + this.mNativeTexturePtr.ToInt32());
					}
				}
				else
				{
					Debug.LogWarning("VideoTextureRendererBehaviour: not setting Video Background Texture because already set by application code!");
				}
				this.mVideoBgConfigChanged = false;
			}
		}

		protected override void OnDestroy()
		{
			if (VuforiaRenderer.Instance.VideoBackgroundTexture == this.mTexture)
			{
				VuforiaRenderer.Instance.SetVideoBackgroundTexture(null, 0);
			}
			VuforiaARController expr_29 = VuforiaARController.Instance;
			expr_29.UnregisterVideoBgEventHandler(this);
			expr_29.UnregisterVuforiaInitializedCallback(new Action(this.OnVuforiaInitialized));
		}

		private void OnVuforiaInitialized()
		{
			this.UpdateVideoBackgroundEnabled();
		}

		private void UpdateVideoBackgroundEnabled()
		{
			DigitalEyewearARController instance = DigitalEyewearARController.Instance;
			Camera primaryCamera = instance.PrimaryCamera;
			Camera secondaryCamera = instance.SecondaryCamera;
			GameObject gameObject = UnityEngine.Object.FindObjectOfType<VuforiaAbstractBehaviour>().gameObject;
			List<GameObject> list = new List<GameObject>
			{
				gameObject
			};
			if (primaryCamera != null)
			{
				list.Add(primaryCamera.gameObject);
			}
			if (secondaryCamera != null)
			{
				list.Add(secondaryCamera.gameObject);
			}
			HashSet<VideoBackgroundAbstractBehaviour> hashSet = new HashSet<VideoBackgroundAbstractBehaviour>();
			using (List<GameObject>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					VideoBackgroundAbstractBehaviour[] componentsInChildren = enumerator.Current.GetComponentsInChildren<VideoBackgroundAbstractBehaviour>(true);
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						VideoBackgroundAbstractBehaviour item = componentsInChildren[i];
						hashSet.Add(item);
					}
				}
			}
			using (HashSet<VideoBackgroundAbstractBehaviour>.Enumerator enumerator2 = hashSet.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.Current.enabled = this.mVideoBackgroundEnabled;
				}
			}
			HashSet<BackgroundPlaneAbstractBehaviour> hashSet2 = new HashSet<BackgroundPlaneAbstractBehaviour>();
			using (List<GameObject>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BackgroundPlaneAbstractBehaviour[] componentsInChildren2 = enumerator.Current.GetComponentsInChildren<BackgroundPlaneAbstractBehaviour>(true);
					for (int i = 0; i < componentsInChildren2.Length; i++)
					{
						BackgroundPlaneAbstractBehaviour item2 = componentsInChildren2[i];
						hashSet2.Add(item2);
					}
				}
			}
			using (HashSet<BackgroundPlaneAbstractBehaviour>.Enumerator enumerator3 = hashSet2.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					enumerator3.Current.GetComponent<Renderer>().GetComponent<Renderer>().enabled = this.mVideoBackgroundEnabled;
				}
			}
			HashSet<HideExcessAreaAbstractBehaviour> hashSet3 = new HashSet<HideExcessAreaAbstractBehaviour>();
			using (List<GameObject>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HideExcessAreaAbstractBehaviour[] componentsInChildren3 = enumerator.Current.GetComponentsInChildren<HideExcessAreaAbstractBehaviour>(true);
					for (int i = 0; i < componentsInChildren3.Length; i++)
					{
						HideExcessAreaAbstractBehaviour item3 = componentsInChildren3[i];
						hashSet3.Add(item3);
					}
				}
			}
			using (HashSet<HideExcessAreaAbstractBehaviour>.Enumerator enumerator4 = hashSet3.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					enumerator4.Current.enabled = this.mVideoBackgroundEnabled;
				}
			}
			instance.SetMode(this.mVideoBackgroundEnabled ? Device.Mode.MODE_AR : Device.Mode.MODE_VR);
			VuforiaARController.Instance.CameraConfiguration.SetSkewFrustum(this.mVideoBackgroundEnabled);
		}

		public void OnVideoBackgroundConfigChanged()
		{
			this.mVideoBgConfigChanged = true;
		}
	}
}
