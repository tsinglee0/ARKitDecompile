using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
	[RequireComponent(typeof(Camera))]
	public abstract class VideoBackgroundAbstractBehaviour : MonoBehaviour
	{
		private int mClearBuffers;

		private int mSkipStateUpdates;

		private VuforiaARController mVuforiaARController;

		private Camera mCamera;

		private BackgroundPlaneAbstractBehaviour mBackgroundBehaviour;

		private float mStereoDepth;

		private static int mFrameCounter = -1;

		private static int mRenderCounter = 0;

		private bool mResetMatrix;

		private Vector2 mVuforiaFrustumSkew = Vector2.zero;

		private Vector2 mCenterToEyeAxis = Vector2.zero;

		private HashSet<MeshRenderer> mDisabledMeshRenderers = new HashSet<MeshRenderer>();

		public void ResetBackgroundPlane(bool disable)
		{
			if (disable)
			{
				if (this.mBackgroundBehaviour != null)
				{
					MeshRenderer component = this.mBackgroundBehaviour.GetComponent<MeshRenderer>();
					if (component != null && component.enabled && !this.mDisabledMeshRenderers.Contains(component))
					{
						component.enabled = false;
						this.mDisabledMeshRenderers.Add(component);
					}
				}
				this.mClearBuffers = 16;
				this.mSkipStateUpdates = 5;
				return;
			}
			if (this.mSkipStateUpdates > 0)
			{
				this.mSkipStateUpdates--;
			}
			if (this.mSkipStateUpdates == 0)
			{
				foreach (MeshRenderer current in this.mDisabledMeshRenderers)
				{
					if (current != null)
					{
						current.enabled = true;
					}
				}
				this.mDisabledMeshRenderers.Clear();
			}
		}

		public void SetStereoDepth(float depth)
		{
			depth = Mathf.Max(Mathf.Min(depth, this.mCamera.GetMaxDepthForVideoBackground()), this.mCamera.GetMinDepthForVideoBackground());
			if (depth != this.mStereoDepth)
			{
				this.mStereoDepth = depth;
				this.ApplyStereoDepthToMatrices();
			}
			if (this.mBackgroundBehaviour != null)
			{
				this.mBackgroundBehaviour.SetBackgroundPlacedCallback(new Action(this.RestoreVuforiaFrustumSkew));
				this.mBackgroundBehaviour.SetStereoDepth(depth);
				return;
			}
			this.RestoreVuforiaFrustumSkew();
		}

		public void ApplyStereoDepthToMatrices()
		{
			Matrix4x4 projectionMatrix = this.mCamera.projectionMatrix;
			Vector3 vector = base.transform.TransformPoint(Vector3.zero);
			float arg_4D_0 = -VuforiaManager.Instance.ARCameraTransform.InverseTransformPoint(vector).x;
			float num = 1f / projectionMatrix[0, 0];
			float num2 = this.mStereoDepth * num;
			float num3 = arg_4D_0 / num2;
			projectionMatrix[0, 2] = this.mVuforiaFrustumSkew.x + num3;
			projectionMatrix[1, 2] = this.mVuforiaFrustumSkew.y;
			this.mCamera.projectionMatrix = projectionMatrix;
			this.mResetMatrix = true;
		}

		internal void DisconnectFromBackgroundBehaviour()
		{
			this.mBackgroundBehaviour = null;
			this.mDisabledMeshRenderers.Clear();
		}

		internal void SetVuforiaFrustumSkewValues(Vector2 skewingValues, Vector2 centerToEyeAxis)
		{
			this.mVuforiaFrustumSkew = skewingValues;
			this.mCenterToEyeAxis = centerToEyeAxis;
		}

		internal void RestoreVuforiaFrustumSkew()
		{
			if (this.mResetMatrix)
			{
				Matrix4x4 projectionMatrix = this.mCamera.projectionMatrix;
				projectionMatrix[0, 2] = projectionMatrix[0, 2] + this.mCenterToEyeAxis.x;
				projectionMatrix[1, 2] = projectionMatrix[1, 2] + this.mCenterToEyeAxis.y;
				this.mCamera.projectionMatrix = projectionMatrix;
				this.mResetMatrix = false;
			}
		}

		private void RenderOnUpdate()
		{
			if (this.mVuforiaARController.HasStarted && base.isActiveAndEnabled && this.mCamera.isActiveAndEnabled)
			{
				if (VideoBackgroundAbstractBehaviour.mFrameCounter != Time.frameCount)
				{
					((VuforiaManagerImpl)VuforiaManager.Instance).StartRendering();
					VideoBackgroundAbstractBehaviour.mFrameCounter = Time.frameCount;
					VideoBackgroundAbstractBehaviour.mRenderCounter = 1;
					return;
				}
				VideoBackgroundAbstractBehaviour.mRenderCounter++;
			}
		}

		private void Awake()
		{
			this.mCamera = base.GetComponent<Camera>();
			this.mVuforiaARController = VuforiaARController.Instance;
			if (this.mVuforiaARController != null)
			{
				this.mVuforiaARController.RegisterRenderOnUpdateCallback(new Action(this.RenderOnUpdate));
			}
			this.mStereoDepth = this.mCamera.GetMaxDepthForVideoBackground();
			this.mBackgroundBehaviour = base.GetComponentInChildren<BackgroundPlaneAbstractBehaviour>();
		}

		private void Start()
		{
			this.ResetBackgroundPlane(true);
		}

		private void OnPreRender()
		{
			if (this.mVuforiaARController.HasStarted && base.isActiveAndEnabled)
			{
				GL.invertCulling = this.mVuforiaARController.VideoBackGroundMirrored == VuforiaRenderer.VideoBackgroundReflection.ON;
				if (VuforiaRenderer.Instance.GetRendererAPI() == VuforiaRenderer.RendererAPI.METAL)
				{
					RenderTexture targetTexture = base.GetComponent<Camera>().targetTexture;
					RenderBuffer renderBuffer = targetTexture ? targetTexture.colorBuffer : Display.main.colorBuffer;
					VuforiaWrapper.Instance.SetRenderBuffers(renderBuffer.GetNativeRenderBufferPtr());
				}
			}
		}

		private void OnPostRender()
		{
			if (this.mVuforiaARController.HasStarted && base.isActiveAndEnabled)
			{
				if (VuforiaRenderer.Instance.GetRendererAPI() == VuforiaRenderer.RendererAPI.METAL)
				{
					RenderTexture targetTexture = base.GetComponent<Camera>().targetTexture;
					RenderBuffer renderBuffer = targetTexture ? targetTexture.colorBuffer : Display.main.colorBuffer;
					RenderBuffer renderBuffer2 = targetTexture ? targetTexture.depthBuffer : Display.main.depthBuffer;
					Graphics.SetRenderTarget(renderBuffer, renderBuffer2);
					VuforiaWrapper.Instance.SetRenderBuffers(renderBuffer.GetNativeRenderBufferPtr());
				}
				VideoBackgroundAbstractBehaviour.mRenderCounter--;
				if (VideoBackgroundAbstractBehaviour.mRenderCounter == 0)
				{
					((VuforiaManagerImpl)VuforiaManager.Instance).FinishRendering();
				}
			}
			if (this.mClearBuffers > 0)
			{
				GL.Clear(false, true, new Color(0f, 0f, 0f, 1f));
				this.mClearBuffers--;
			}
		}

		private void OnDestroy()
		{
			if (this.mVuforiaARController != null)
			{
				this.mVuforiaARController.UnregisterRenderOnUpdateCallback(new Action(this.RenderOnUpdate));
			}
		}
	}
}
