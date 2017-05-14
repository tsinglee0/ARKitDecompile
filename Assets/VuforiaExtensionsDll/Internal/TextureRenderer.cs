using System;
using UnityEngine;

namespace Vuforia
{
	internal class TextureRenderer
	{
		private Camera mTextureBufferCamera;

		private int mTextureWidth;

		private int mTextureHeight;

		public int Width
		{
			get
			{
				return this.mTextureWidth;
			}
		}

		public int Height
		{
			get
			{
				return this.mTextureHeight;
			}
		}

		public TextureRenderer(Texture textureToRender, int renderTextureLayer, VuforiaRenderer.Vec2I requestedTextureSize)
		{
			if (renderTextureLayer > 31)
			{
				Debug.LogError("WebCamBehaviour.SetupTextureBufferCamera: configured layer > 31 is not supported by Unity!");
				return;
			}
			this.mTextureWidth = requestedTextureSize.x;
			this.mTextureHeight = requestedTextureSize.y;
			float num = (float)this.mTextureHeight / (float)this.mTextureWidth * 0.5f;
			GameObject gameObject = new GameObject("TextureBufferCamera");
			this.mTextureBufferCamera = gameObject.AddComponent<Camera>();
			this.mTextureBufferCamera.orthographic = true;
			this.mTextureBufferCamera.orthographicSize = num;
			this.mTextureBufferCamera.aspect = (float)this.mTextureWidth / (float)this.mTextureHeight;
			this.mTextureBufferCamera.nearClipPlane = 0.5f;
			this.mTextureBufferCamera.farClipPlane = 1.5f;
			this.mTextureBufferCamera.cullingMask = 1 << renderTextureLayer;
			this.mTextureBufferCamera.enabled = false;
			this.mTextureBufferCamera.transform.position = new Vector3(-1000f, -1000f, -1000f);
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
			GameObject gameObject2 = new GameObject("TextureBufferMesh", new Type[]
			{
				typeof(MeshFilter),
				typeof(MeshRenderer)
			});
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.layer = renderTextureLayer;
			Mesh mesh = new Mesh();
			mesh.vertices = new Vector3[]
			{
				new Vector3(-0.5f, num, 1f),
				new Vector3(0.5f, num, 1f),
				new Vector3(-0.5f, -num, 1f),
				new Vector3(0.5f, -num, 1f)
			};
			mesh.uv = new Vector2[]
			{
				new Vector2(0f, 0f),
				new Vector2(1f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 1f)
			};
			mesh.triangles = new int[]
			{
				0,
				1,
				2,
				2,
				1,
				3
			};
			Mesh mesh2 = mesh;
			MeshRenderer expr_23F = gameObject2.GetComponent<MeshRenderer>();
			expr_23F.material = new Material(Shader.Find("Unlit/Texture"));
			expr_23F.material.mainTexture = textureToRender;
			gameObject2.GetComponent<MeshFilter>().mesh = mesh2;
		}

		public RenderTexture Render()
		{
			RenderTexture temporary = RenderTexture.GetTemporary(this.mTextureWidth, this.mTextureHeight, 16);
			this.mTextureBufferCamera.targetTexture = temporary;
			this.mTextureBufferCamera.Render();
			return temporary;
		}

		public void Destroy()
		{
			if (this.mTextureBufferCamera != null)
			{
                UnityEngine.Object.Destroy(this.mTextureBufferCamera.gameObject);
			}
		}
	}
}
