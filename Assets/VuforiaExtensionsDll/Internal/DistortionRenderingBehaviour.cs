using System;
using UnityEngine;

namespace Vuforia
{
	public class DistortionRenderingBehaviour : MonoBehaviour, IVideoBackgroundEventHandler
	{
		private bool mSingleTexture = true;

		private int mRenderLayer = 31;

		private int[] mOriginalCullingMasks;

		private Camera[] mStereoCameras;

		private GameObject[] mMeshes;

		private RenderTexture[] mTextures;

		private bool mStarted;

		private bool mVideoBackgroundChanged;

		private Rect mOriginalLeftViewport;

		private Rect mOriginalRightViewport;

		private Rect mDualTextureLeftViewport;

		private Rect mDualTextureRightViewport;

		public bool UseSingleTexture
		{
			get
			{
				return this.mSingleTexture;
			}
			set
			{
				if (this.mStarted && this.mSingleTexture != value)
				{
					this.mSingleTexture = value;
					if (this.mSingleTexture)
					{
                        UnityEngine.Object.Destroy(this.mMeshes[1]);
						this.mMeshes = new GameObject[]
						{
							this.mMeshes[0]
						};
						if (this.mStereoCameras[1].targetTexture == this.mTextures[1])
						{
							this.mStereoCameras[1].targetTexture = null;
						}
						this.mTextures[1].Release();
						this.mTextures = new RenderTexture[]
						{
							this.mTextures[0]
						};
					}
					else
					{
						this.mMeshes = new GameObject[]
						{
							this.mMeshes[0],
							this.CreateMeshGameObject()
						};
						this.mMeshes[1].layer = base.gameObject.layer;
						RenderTexture[] expr_D9 = new RenderTexture[2];
						expr_D9[0] = this.mTextures[0];
						this.mTextures = expr_D9;
					}
					this.CreateMesh();
					this.CreateRenderTexture();
					this.AdjustDualTextureViewports(this.mSingleTexture);
				}
				this.mSingleTexture = value;
			}
		}

		public int RenderLayer
		{
			get
			{
				return this.mRenderLayer;
			}
			set
			{
				if (!this.mStarted)
				{
					this.mRenderLayer = value;
				}
			}
		}

		public bool VideoBackgroundChanged
		{
			get
			{
				return this.mVideoBackgroundChanged;
			}
			set
			{
				this.mVideoBackgroundChanged = value;
			}
		}

		private void Start()
		{
			this.mStarted = true;
			VuforiaARController.Instance.RegisterVideoBgEventHandler(this);
			VuforiaAbstractBehaviour vuforiaAbstractBehaviour = UnityEngine.Object.FindObjectOfType<VuforiaAbstractBehaviour>();
			this.mStereoCameras = vuforiaAbstractBehaviour.GetComponentsInChildren<Camera>();
			if (this.mStereoCameras.Length != 2)
			{
				Debug.LogError("There must be two cameras");
				this.mStereoCameras = null;
				return;
			}
			if (this.mSingleTexture)
			{
				this.mMeshes = new GameObject[1];
				this.mMeshes[0] = this.CreateMeshGameObject();
				this.mTextures = new RenderTexture[1];
			}
			else
			{
				this.mMeshes = new GameObject[2];
				this.mMeshes[0] = this.CreateMeshGameObject();
				this.mMeshes[1] = this.CreateMeshGameObject();
				this.mTextures = new RenderTexture[2];
			}
			if (this.mVideoBackgroundChanged)
			{
				this.mVideoBackgroundChanged = false;
				this.OnVideoBackgroundConfigChanged();
			}
			this.OnEnable();
		}

		private void OnEnable()
		{
			if (!this.mStarted)
			{
				return;
			}
			for (int i = 0; i < this.mStereoCameras.Length; i++)
			{
				this.mStereoCameras[i].targetTexture = this.mTextures[this.mSingleTexture ? 0 : i];
			}
			this.mOriginalCullingMasks = new int[this.mStereoCameras.Length];
			for (int j = 0; j < this.mStereoCameras.Length; j++)
			{
				this.mOriginalCullingMasks[j] = this.mStereoCameras[j].cullingMask;
				Camera expr_70 = this.mStereoCameras[j];
				expr_70.cullingMask = expr_70.cullingMask & ~(1 << this.mRenderLayer);
			}
			base.gameObject.layer = this.mRenderLayer;
			for (int k = 0; k < this.mMeshes.Length; k++)
			{
				this.mMeshes[k].layer = this.mRenderLayer;
			}
			if (!this.mSingleTexture)
			{
				this.AdjustDualTextureViewports(false);
			}
		}

		private void Update()
		{
			if (this.HaveDualTextureViewportsChanged())
			{
				this.AdjustDualTextureViewports(false);
			}
		}

		private void OnDisable()
		{
			if (this.mStereoCameras == null)
			{
				return;
			}
			for (int i = 0; i < this.mStereoCameras.Length; i++)
			{
				if (this.mStereoCameras[i] != null)
				{
					this.mStereoCameras[i].cullingMask = this.mOriginalCullingMasks[i];
					this.mStereoCameras[i].targetTexture = null;
				}
			}
			if (!this.mSingleTexture)
			{
				this.AdjustDualTextureViewports(true);
			}
		}

		private void OnDestroy()
		{
			VuforiaARController.Instance.UnregisterVideoBgEventHandler(this);
			if (this.mMeshes != null)
			{
                UnityEngine.Object.Destroy(this.mMeshes[0].GetComponent<MeshRenderer>().material);
				if (!this.mSingleTexture)
				{
                    UnityEngine.Object.Destroy(this.mMeshes[1].GetComponent<MeshRenderer>().material);
				}
				Resources.UnloadUnusedAssets();
			}
		}

		private void CreateMesh()
		{
			Mesh distortionMesh = Device.Instance.GetDistortionMesh(View.VIEW_POSTPROCESS, this.mMeshes[0].GetComponent<MeshFilter>().sharedMesh);
			if (this.mSingleTexture)
			{
				this.mMeshes[0].GetComponent<MeshFilter>().sharedMesh = distortionMesh;
				return;
			}
			Mesh mesh = new Mesh();
			Vector3[] array = new Vector3[distortionMesh.vertexCount / 2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = distortionMesh.vertices[i];
			}
			Vector2[] array2 = new Vector2[distortionMesh.vertexCount / 2];
			for (int j = 0; j < array2.Length; j++)
			{
				Vector2 vector = distortionMesh.uv[j];
				vector.x *= 2f;
				array2[j] = vector;
			}
			int[] array3 = new int[distortionMesh.triangles.Length / 2];
			for (int k = 0; k < array3.Length; k++)
			{
				array3[k] = distortionMesh.triangles[k];
			}
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.triangles = array3;
			Mesh mesh2 = new Mesh();
			Vector3[] array4 = new Vector3[distortionMesh.vertexCount / 2];
			for (int l = 0; l < array4.Length; l++)
			{
				array4[l] = distortionMesh.vertices[l + array4.Length];
			}
			Vector2[] array5 = new Vector2[distortionMesh.vertexCount / 2];
			for (int m = 0; m < array5.Length; m++)
			{
				Vector2 vector2 = distortionMesh.uv[m + array4.Length];
				vector2.x = (vector2.x - 0.5f) * 2f;
				array5[m] = vector2;
			}
			int[] array6 = new int[distortionMesh.triangles.Length / 2];
			for (int n = 0; n < array6.Length; n++)
			{
				array6[n] = distortionMesh.triangles[n + array6.Length] - array4.Length;
			}
			mesh2.vertices = array4;
			mesh2.uv = array5;
			mesh2.triangles = array6;
			this.mMeshes[0].GetComponent<MeshFilter>().sharedMesh = mesh;
			this.mMeshes[1].GetComponent<MeshFilter>().sharedMesh = mesh2;
		}

		private GameObject CreateMeshGameObject()
		{
			GameObject expr_0A = new GameObject("DistortionMesh");
			expr_0A.transform.parent = base.transform;
			expr_0A.AddComponent<MeshFilter>();
			expr_0A.AddComponent<MeshRenderer>().sharedMaterial = Resources.Load("Materials/DistortionStereoMaterial") as Material;
			return expr_0A;
		}

		private void CreateRenderTexture()
		{
			for (int i = 0; i < this.mTextures.Length; i++)
			{
				int num;
				int num2;
				if (this.mSingleTexture)
				{
					Device.Instance.GetTextureSize(View.VIEW_POSTPROCESS, out num, out num2);
				}
				else
				{
					Device.Instance.GetTextureSize((i == 0) ? View.VIEW_LEFTEYE : View.VIEW_RIGHTEYE, out num, out num2);
				}
				if (!(this.mTextures[i] != null) || num != this.mTextures[i].width || num2 != this.mTextures[i].height)
				{
					if (this.mTextures[i] != null && this.mTextures[i].IsCreated())
					{
						for (int j = 0; j < this.mStereoCameras.Length; j++)
						{
							if (this.mTextures[i] == this.mStereoCameras[j].targetTexture)
							{
								this.mStereoCameras[j].targetTexture = null;
							}
						}
						this.mTextures[i].Release();
					}
					int antiAliasing = (QualitySettings.antiAliasing > 0) ? QualitySettings.antiAliasing : 1;
					if (Application.platform == RuntimePlatform.OSXEditor)
					{
						antiAliasing = 1;
					}
					RenderTexture[] arg_11E_0 = this.mTextures;
					int arg_11E_1 = i;
					RenderTexture expr_102 = new RenderTexture(num, num2, 24, RenderTextureFormat.Default);
					expr_102.autoGenerateMips = false;
					expr_102.anisoLevel = 0;
					expr_102.filterMode = FilterMode.Bilinear;
					expr_102.antiAliasing = antiAliasing;
					arg_11E_0[arg_11E_1] = expr_102;
					this.mTextures[i].Create();
					this.mMeshes[i].GetComponent<MeshRenderer>().material.SetTexture("_MainTex", this.mTextures[i]);
				}
			}
			for (int k = 0; k < this.mStereoCameras.Length; k++)
			{
				this.mStereoCameras[k].targetTexture = this.mTextures[this.mSingleTexture ? 0 : k];
			}
		}

		private Camera SetupOrthographicCamera()
		{
			Camera camera = base.GetComponent<Camera>();
			if (camera == null)
			{
				camera = base.gameObject.AddComponent<Camera>();
			}
			camera.orthographic = true;
			camera.orthographicSize = 1f;
			camera.aspect = 1f;
			camera.nearClipPlane = -1f;
			camera.farClipPlane = 1f;
			Rect viewport = Device.Instance.GetViewport(View.VIEW_POSTPROCESS);
			float num = (float)Screen.width;
			float num2 = (float)Screen.height;
			float num3 = -(1f + 2f * (viewport.xMin / num));
			float num4 = 1f - 2f * (viewport.xMax / num - 1f);
			float num5 = -(1f + 2f * (viewport.yMin / num2));
			float num6 = 1f - 2f * (viewport.yMax / num2 - 1f);
			camera.orthographic = true;
			camera.projectionMatrix = Matrix4x4.Ortho(num3, num4, num5, num6, -1f, 1f);
			camera.rect = new Rect(0f, 0f, 1f, 1f);
			camera.clearFlags = CameraClearFlags.SolidColor;
			camera.backgroundColor = Color.black;
			camera.cullingMask = 1 << this.mRenderLayer;
			float num7 = 0f;
			Camera[] array = this.mStereoCameras;
			for (int i = 0; i < array.Length; i++)
			{
				Camera camera2 = array[i];
				num7 = Math.Max(num7, camera2.depth);
			}
			camera.depth = num7 + 10f;
			return camera;
		}

		private bool HaveDualTextureViewportsChanged()
		{
			return !this.mSingleTexture && (this.mStereoCameras[0].rect != this.mDualTextureLeftViewport || this.mStereoCameras[1].rect != this.mDualTextureRightViewport);
		}

		private void AdjustDualTextureViewports(bool revert)
		{
			if (!revert)
			{
				this.mOriginalLeftViewport = this.mStereoCameras[0].rect;
				this.mOriginalRightViewport = this.mStereoCameras[1].rect;
				this.mDualTextureLeftViewport = new Rect(this.mOriginalLeftViewport.xMin * 2f, this.mOriginalLeftViewport.yMin, this.mOriginalLeftViewport.width * 2f, this.mOriginalLeftViewport.height);
				this.mDualTextureRightViewport = new Rect((this.mOriginalRightViewport.xMin - 0.5f) * 2f, this.mOriginalRightViewport.yMin, this.mOriginalRightViewport.width * 2f, this.mOriginalRightViewport.height);
				this.mStereoCameras[0].rect = this.mDualTextureLeftViewport;
				this.mStereoCameras[1].rect = this.mDualTextureRightViewport;
				return;
			}
			if (this.mStereoCameras[0] != null && this.mStereoCameras[0].rect == this.mDualTextureLeftViewport)
			{
				this.mStereoCameras[0].rect = this.mOriginalLeftViewport;
			}
			if (this.mStereoCameras[1] != null && this.mStereoCameras[1].rect == this.mDualTextureRightViewport)
			{
				this.mStereoCameras[1].rect = this.mOriginalRightViewport;
			}
		}

		public void OnVideoBackgroundConfigChanged()
		{
			if (this.mStarted && base.enabled && base.gameObject.activeSelf)
			{
				this.CreateMesh();
				this.SetupOrthographicCamera();
				this.CreateRenderTexture();
				RenderTexture[] array = this.mTextures;
				for (int i = 0; i < array.Length; i++)
				{
					RenderTexture.active = array[i];
					GL.Clear(true, true, Color.black);
				}
			}
		}
	}
}
