using System;
using UnityEngine;

namespace Vuforia
{
	public class WebCamARController : ARController
	{
		public int RenderTextureLayer;

		private string mDeviceNameSetInEditor;

		private bool mFlipHorizontally;

		private WebCamImpl mWebCamImpl;

		private static WebCamARController mInstance;

		private static object mPadlock = new object();

		public static WebCamARController Instance
		{
			get
			{
				if (WebCamARController.mInstance == null)
				{
					object obj = WebCamARController.mPadlock;
					lock (obj)
					{
						if (WebCamARController.mInstance == null)
						{
							WebCamARController.mInstance = new WebCamARController();
						}
					}
				}
				return WebCamARController.mInstance;
			}
		}

		public string DeviceName
		{
			get
			{
				return this.mDeviceNameSetInEditor;
			}
			set
			{
				this.mDeviceNameSetInEditor = value;
			}
		}

		public bool FlipHorizontally
		{
			get
			{
				return this.mFlipHorizontally;
			}
			set
			{
				this.mFlipHorizontally = value;
			}
		}

		public bool IsPlaying
		{
			get
			{
				return this.mWebCamImpl.IsPlaying;
			}
		}

		internal WebCamImpl ImplementationClass
		{
			get
			{
				return this.mWebCamImpl;
			}
		}

		private WebCamARController()
		{
		}

		[RuntimeInitializeOnLoadMethod]
		public static void RegisterARController()
		{
			ARController.Register(WebCamARController.Instance);
		}

		internal void InitCamera()
		{
			if (this.mWebCamImpl == null)
			{
				Application.runInBackground = true;
				Camera[] componentsInChildren = base.VuforiaBehaviour.GetComponentsInChildren<Camera>();
				this.mWebCamImpl = new WebCamImpl(componentsInChildren, this.RenderTextureLayer, this.mDeviceNameSetInEditor, this.mFlipHorizontally);
			}
		}

		protected override void Awake()
		{
			VuforiaAbstractConfiguration.WebCamConfiguration webCam = VuforiaAbstractConfiguration.Instance.WebCam;
			this.RenderTextureLayer = webCam.RenderTextureLayer;
			this.mDeviceNameSetInEditor = webCam.DeviceNameSetInEditor;
			this.mFlipHorizontally = webCam.FlipHorizontally;
		}

		protected override void OnLevelWasLoaded()
		{
			if (VuforiaRuntimeUtilities.IsPlayMode() && this.mWebCamImpl != null)
			{
				this.mWebCamImpl.ResetPlaying();
			}
		}

		protected override void OnDestroy()
		{
			if (VuforiaRuntimeUtilities.IsPlayMode() && this.mWebCamImpl != null)
			{
				this.mWebCamImpl.OnDestroy();
			}
		}

		protected override void Update()
		{
			if (VuforiaRuntimeUtilities.IsPlayMode() && this.mWebCamImpl != null)
			{
				this.mWebCamImpl.Update();
			}
		}
	}
}
