using System;
using UnityEngine;

namespace Vuforia
{
	public abstract class BackgroundPlaneAbstractBehaviour : MonoBehaviour, IVideoBackgroundEventHandler
	{
		private VuforiaRenderer.VideoTextureInfo mTextureInfo;

		private int mViewWidth;

		private int mViewHeight;

		private int mNumFramesToUpdateVideoBg;

		private Camera mCamera;

		private static float maxDisplacement = 3000f;

		private int defaultNumDivisions = 2;

		private Mesh mMesh;

		private float mStereoDepth;

		private Vector3 mBackgroundOffset;

		private VuforiaARController mVuforiaBehaviour;

		private Action mBackgroundPlacedCallback;

		[HideInInspector, SerializeField]
		private int mNumDivisions = 2;

		public int NumDivisions
		{
			get
			{
				return this.mNumDivisions;
			}
		}

		public Vector3 BackgroundOffset
		{
			get
			{
				return this.mBackgroundOffset;
			}
			set
			{
				this.mBackgroundOffset = value;
			}
		}

		internal static Quaternion DefaultRotationTowardsCamera
		{
			get
			{
				return Quaternion.AngleAxis(270f, Vector3.right);
			}
		}

		public bool CheckNumDivisions()
		{
			return this.NumDivisions >= this.defaultNumDivisions;
		}

		public void SetStereoDepth(float depth)
		{
			if (depth != this.mStereoDepth)
			{
				this.mStereoDepth = depth;
				this.PositionVideoMesh();
			}
		}

		private void Start()
		{
			this.mVuforiaBehaviour = VuforiaARController.Instance;
			this.mVuforiaBehaviour.RegisterVideoBgEventHandler(this);
			this.mVuforiaBehaviour.RegisterBackgroundTextureChangedCallback(new Action(this.OnBackgroundTextureChanged));
			this.mVuforiaBehaviour.RegisterTrackablesUpdatedCallback(new Action(this.OnTrackablesUpdated));
			this.mCamera = base.transform.parent.GetComponent<Camera>();
			this.mStereoDepth = this.mCamera.GetMaxDepthForVideoBackground();
		}

		private void OnDestroy()
		{
			if (this.mVuforiaBehaviour != null)
			{
				this.mVuforiaBehaviour.UnregisterVideoBgEventHandler(this);
				this.mVuforiaBehaviour.UnregisterBackgroundTextureChangedCallback(new Action(this.OnBackgroundTextureChanged));
				this.mVuforiaBehaviour.UnregisterTrackablesUpdatedCallback(new Action(this.OnTrackablesUpdated));
			}
		}

		private void OnValidate()
		{
			if (this.mNumDivisions < this.defaultNumDivisions)
			{
				this.mNumDivisions = this.defaultNumDivisions;
			}
		}

		private void OnTrackablesUpdated()
		{
			if (VuforiaRenderer.Instance.IsVideoBackgroundInfoAvailable())
			{
				int pixelWidthInt = this.mCamera.GetPixelWidthInt();
				int pixelHeightInt = this.mCamera.GetPixelHeightInt();
				if ((this.mNumFramesToUpdateVideoBg > 0 || this.mViewWidth != pixelWidthInt || this.mViewHeight != pixelHeightInt) && !((VuforiaManagerImpl)VuforiaManager.Instance).IsDiscardingRenderingStates())
				{
					this.mViewWidth = pixelWidthInt;
					this.mViewHeight = pixelHeightInt;
					this.mTextureInfo = VuforiaRenderer.Instance.GetVideoTextureInfo();
					this.CreateAndSetVideoMesh();
					this.PositionVideoMesh();
					this.mNumFramesToUpdateVideoBg--;
				}
			}
		}

		internal void SetBackgroundPlacedCallback(Action backgroundPlacedCallback)
		{
			this.mBackgroundPlacedCallback = backgroundPlacedCallback;
		}

		private void CreateAndSetVideoMesh()
		{
			MeshFilter meshFilter = base.GetComponent<MeshFilter>();
			if (meshFilter == null)
			{
				meshFilter = base.gameObject.AddComponent<MeshFilter>();
				this.mMesh = meshFilter.mesh;
			}
			this.mMesh.Clear();
			if (this.mNumDivisions < this.defaultNumDivisions)
			{
				this.mNumDivisions = this.defaultNumDivisions;
			}
			int num = this.mNumDivisions;
			int num2 = this.mNumDivisions;
			this.mMesh.vertices = new Vector3[num * num2];
			Vector3[] vertices = this.mMesh.vertices;
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					float num3 = (float)j / (float)(num2 - 1) - 0.5f;
					float num4 = 1f - (float)i / (float)(num - 1) - 0.5f;
					vertices[i * num2 + j].x = num3 * 2f;
					vertices[i * num2 + j].y = 0f;
					vertices[i * num2 + j].z = num4 * 2f;
				}
			}
			this.mMesh.vertices = vertices;
			this.mMesh.triangles = new int[num * num2 * 2 * 3];
			int num5 = 0;
			float num6 = (float)this.mTextureInfo.imageSize.x / (float)this.mTextureInfo.textureSize.x;
			float num7 = (float)this.mTextureInfo.imageSize.y / (float)this.mTextureInfo.textureSize.y;
			this.mMesh.uv = new Vector2[num * num2];
			int[] triangles = this.mMesh.triangles;
			Vector2[] uv = this.mMesh.uv;
			for (int k = 0; k < num - 1; k++)
			{
				for (int l = 0; l < num2 - 1; l++)
				{
					int num8 = k * num2 + l;
					int num9 = k * num2 + l + num2 + 1;
					int num10 = k * num2 + l + num2;
					int num11 = k * num2 + l + 1;
					triangles[num5++] = num8;
					triangles[num5++] = num9;
					triangles[num5++] = num10;
					triangles[num5++] = num9;
					triangles[num5++] = num8;
					triangles[num5++] = num11;
					uv[num8] = new Vector2((float)l / (float)(num2 - 1) * num6, (float)k / (float)(num - 1) * num7);
					uv[num9] = new Vector2((float)(l + 1) / (float)(num2 - 1) * num6, (float)(k + 1) / (float)(num - 1) * num7);
					uv[num10] = new Vector2((float)l / (float)(num2 - 1) * num6, (float)(k + 1) / (float)(num - 1) * num7);
					uv[num11] = new Vector2((float)(l + 1) / (float)(num2 - 1) * num6, (float)k / (float)(num - 1) * num7);
				}
			}
			this.mMesh.triangles = triangles;
			this.mMesh.uv = uv;
			this.mMesh.normals = new Vector3[this.mMesh.vertices.Length];
			this.mMesh.RecalculateNormals();
		}

		private void PositionVideoMesh()
		{
			float num = (float)this.mViewWidth / (float)this.mViewHeight;
			if (float.IsNaN(num))
			{
				return;
			}
			ScreenOrientation screenOrientation = VuforiaRuntimeUtilities.ScreenOrientation;
			base.gameObject.transform.localRotation = BackgroundPlaneAbstractBehaviour.DefaultRotationTowardsCamera;
			if (this.mVuforiaBehaviour != null)
			{
				if (screenOrientation == ScreenOrientation.LandscapeLeft)
				{
					Transform expr_4E = base.gameObject.transform;
					expr_4E.localRotation = expr_4E.localRotation * Quaternion.identity;
				}
				else if (screenOrientation == ScreenOrientation.Portrait)
				{
					Transform expr_77 = base.gameObject.transform;
					expr_77.localRotation = expr_77.localRotation * Quaternion.AngleAxis(90f, Vector3.up);
				}
				else if (screenOrientation == ScreenOrientation.LandscapeRight)
				{
					Transform expr_A7 = base.gameObject.transform;
					expr_A7.localRotation = expr_A7.localRotation * Quaternion.AngleAxis(180f, Vector3.up);
				}
				else if (screenOrientation == ScreenOrientation.PortraitUpsideDown)
				{
					Transform expr_D7 = base.gameObject.transform;
					expr_D7.localRotation = expr_D7.localRotation * Quaternion.AngleAxis(270f, Vector3.up);
				}
				if (CameraDevice.Instance.GetCameraDirection() == CameraDevice.CameraDirection.CAMERA_FRONT && (screenOrientation == ScreenOrientation.Portrait || screenOrientation == ScreenOrientation.PortraitUpsideDown))
				{
					Transform expr_116 = base.gameObject.transform;
					expr_116.localRotation = expr_116.localRotation * Quaternion.AngleAxis(180f, Vector3.up);
				}
				EyewearDevice eyewearDevice = Device.Instance as EyewearDevice;
				if (eyewearDevice != null && eyewearDevice.IsSeeThru())
				{
					base.gameObject.transform.localPosition = new Vector3(0f, 0f, this.mStereoDepth);
					base.gameObject.transform.localScale = Vector3.zero;
				}
				else
				{
					Plane plane = new Plane(this.mCamera.transform.forward, this.mCamera.transform.position + this.mCamera.transform.forward * this.mStereoDepth);
					Ray ray = this.mCamera.ScreenPointToRay(new Vector3(this.mCamera.pixelRect.xMin + (this.mCamera.pixelRect.xMax - this.mCamera.pixelRect.xMin) / 2f, this.mCamera.pixelRect.yMin + (this.mCamera.pixelRect.yMax - this.mCamera.pixelRect.yMin) / 2f, 0f));
					float num2 = 0f;
					plane.Raycast(ray, out num2);
					Vector3 localPosition = this.mCamera.transform.InverseTransformPoint(ray.GetPoint(num2));
					base.gameObject.transform.localPosition = localPosition;
					Transform expr_2BB = base.gameObject.transform;
					expr_2BB.position = expr_2BB.position + this.mBackgroundOffset;
					float num3 = 1f / this.mCamera.projectionMatrix[5];
					float num4 = this.mStereoDepth * num3;
					float num5 = num4 * num;
					Rect videoBackgroundRectInViewPort = this.mVuforiaBehaviour.GetVideoBackgroundRectInViewPort();
					if ((int)videoBackgroundRectInViewPort.height != this.mViewHeight)
					{
						num4 *= videoBackgroundRectInViewPort.height / (float)this.mViewHeight;
					}
					if ((int)videoBackgroundRectInViewPort.width != this.mViewWidth)
					{
						num5 *= videoBackgroundRectInViewPort.width / (float)this.mViewWidth;
					}
					float num6;
					if (VuforiaRuntimeUtilities.IsPortraitOrientation)
					{
						num6 = (float)this.mTextureInfo.imageSize.y / (float)this.mTextureInfo.imageSize.x;
					}
					else
					{
						num6 = (float)this.mTextureInfo.imageSize.x / (float)this.mTextureInfo.imageSize.y;
					}
					if (!float.IsNaN(num6))
					{
						if (this.ShouldFitWidth())
						{
							if (num6 > 1f)
							{
								base.gameObject.transform.localScale = new Vector3(num5, 1f, num5 / num6);
							}
							else
							{
								base.gameObject.transform.localScale = new Vector3(num5 / num6, 1f, num5);
							}
						}
						else if (num6 > 1f)
						{
							base.gameObject.transform.localScale = new Vector3(num4 * num6, 1f, num4);
						}
						else
						{
							base.gameObject.transform.localScale = new Vector3(num4, 1f, num4 * num6);
						}
					}
				}
			}
			if (this.mBackgroundPlacedCallback != null)
			{
				this.mBackgroundPlacedCallback();
			}
		}

		private bool ShouldFitWidth()
		{
			float arg_61_0 = (float)this.mViewWidth / (float)this.mViewHeight;
			float num;
			if (VuforiaRuntimeUtilities.IsPortraitOrientation)
			{
				num = (float)this.mTextureInfo.imageSize.y / (float)this.mTextureInfo.imageSize.x;
			}
			else
			{
				num = (float)this.mTextureInfo.imageSize.x / (float)this.mTextureInfo.imageSize.y;
			}
			return arg_61_0 >= num;
		}

		private void OnBackgroundTextureChanged()
		{
			Texture videoBackgroundTexture = VuforiaRenderer.Instance.VideoBackgroundTexture;
			if (videoBackgroundTexture != null)
			{
				base.GetComponent<Renderer>().material.mainTexture = videoBackgroundTexture;
			}
		}

		public void OnVideoBackgroundConfigChanged()
		{
			this.mNumFramesToUpdateVideoBg = 2;
		}
	}
}
