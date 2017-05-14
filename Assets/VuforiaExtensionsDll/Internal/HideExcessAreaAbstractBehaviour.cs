using System;
using UnityEngine;

namespace Vuforia
{
	public abstract class HideExcessAreaAbstractBehaviour : MonoBehaviour
	{
		public enum CLIPPING_MODE
		{
			STENCIL,
			PLANES,
			NONE
		}

		private IExcessAreaClipping mClippingImpl = new NullHideExcessAreaClipping();

		private HideExcessAreaAbstractBehaviour.CLIPPING_MODE mClippingMode = HideExcessAreaAbstractBehaviour.CLIPPING_MODE.NONE;

		private VuforiaARController mVuforiaBehaviour;

		private VideoBackgroundManager mVideoBgMgr;

		private Vector3 mPlaneOffset = Vector3.zero;

		private bool mSceneScaledDown;

		private bool mStarted;

		public Vector3 PlaneOffset
		{
			get
			{
				return this.mPlaneOffset;
			}
			set
			{
				this.mPlaneOffset = value;
			}
		}

		private void OnTrackablesUpdated()
		{
			if (this.mSceneScaledDown && base.isActiveAndEnabled)
			{
				this.CheckForChangedClippingMode();
				if (!this.mStarted)
				{
					this.mClippingImpl.Start();
					this.mStarted = true;
				}
				this.mClippingImpl.Update(this.mPlaneOffset);
			}
		}

		private void OnVuforiaStarted()
		{
			this.OnConfigurationChanged();
			if (this.mSceneScaledDown && base.isActiveAndEnabled)
			{
				this.mClippingImpl.Start();
				this.mClippingImpl.SetPlanesRenderingActive(true);
				this.mStarted = true;
			}
		}

		private void CheckForChangedClippingMode()
		{
			if (this.mVideoBgMgr == null)
			{
				return;
			}
			HideExcessAreaAbstractBehaviour.CLIPPING_MODE clippingMode = this.mVideoBgMgr.ClippingMode;
			if (clippingMode == this.mClippingMode)
			{
				return;
			}
			this.mClippingMode = clippingMode;
			if (this.mClippingImpl != null)
			{
				this.mClippingImpl.OnDestroy();
			}
			this.mStarted = false;
			HideExcessAreaAbstractBehaviour.CLIPPING_MODE cLIPPING_MODE = this.mClippingMode;
			if (cLIPPING_MODE == HideExcessAreaAbstractBehaviour.CLIPPING_MODE.STENCIL)
			{
				this.mClippingImpl = new StencilHideExcessAreaClipping(base.gameObject, this.mVideoBgMgr.MatteShader);
				return;
			}
			if (cLIPPING_MODE != HideExcessAreaAbstractBehaviour.CLIPPING_MODE.PLANES)
			{
				this.mClippingImpl = new NullHideExcessAreaClipping();
				return;
			}
			this.mClippingImpl = new LegacyHideExcessAreaClipping(base.gameObject, this.mVideoBgMgr.MatteShader);
		}

		internal void SetPlanesRenderingActive(bool activeflag)
		{
			this.mClippingImpl.SetPlanesRenderingActive(activeflag);
		}

		internal bool IsPlanesRenderingActive()
		{
			return this.mClippingImpl.IsPlanesRenderingActive();
		}

		internal void OnConfigurationChanged()
		{
			this.mSceneScaledDown = Device.Instance.IsViewerActive();
			if (base.isActiveAndEnabled && this.mStarted)
			{
				this.mClippingImpl.SetPlanesRenderingActive(this.mSceneScaledDown);
			}
		}

		private void OnPreCull()
		{
			if (this.mSceneScaledDown)
			{
				this.mClippingImpl.OnPreCull();
			}
		}

		private void OnPostRender()
		{
			if (this.mSceneScaledDown)
			{
				this.mClippingImpl.OnPostRender();
			}
		}

		private void Start()
		{
			this.mVideoBgMgr = VideoBackgroundManager.Instance;
			this.CheckForChangedClippingMode();
			this.mVuforiaBehaviour = VuforiaARController.Instance;
			this.mVuforiaBehaviour.RegisterVuforiaStartedCallback(new Action(this.OnVuforiaStarted));
			this.mVuforiaBehaviour.RegisterTrackablesUpdatedCallback(new Action(this.OnTrackablesUpdated));
		}

		private void OnDisable()
		{
			this.mClippingImpl.SetPlanesRenderingActive(false);
		}

		private void OnEnable()
		{
			if (this.mSceneScaledDown)
			{
				this.mClippingImpl.SetPlanesRenderingActive(true);
			}
		}

		private void OnDestroy()
		{
			if (this.mVuforiaBehaviour != null)
			{
				this.mVuforiaBehaviour.UnregisterTrackablesUpdatedCallback(new Action(this.OnTrackablesUpdated));
				this.mVuforiaBehaviour.UnregisterVuforiaStartedCallback(new Action(this.OnVuforiaStarted));
			}
			this.mClippingImpl.OnDestroy();
		}
	}
}
