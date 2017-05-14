using System;
using UnityEngine;

namespace Vuforia
{
	internal class StencilHideExcessAreaClipping : IExcessAreaClipping
	{
		private GameObject mGameObject;

		private Shader mMatteShader;

		private GameObject mClippingPlane;

		private Camera mCamera;

		private float mCameraNearPlane;

		private float mCameraFarPlane;

		private Rect mCameraPixelRect = new Rect(0f, 0f, 0f, 0f);

		private float mCameraFieldOfView;

		private VuforiaARController mVuforiaBehaviour;

		private bool mPlanesActivated;

		private GameObject mBgPlane;

		private Vector3 mBgPlaneLocalPos = Vector3.zero;

		private Vector3 mBgPlaneLocalScale = Vector3.zero;

		private GameObject CreateQuad(GameObject parent, string name, Vector3 position, Quaternion rotation, Vector3 scale, int layer)
		{
			GameObject expr_06 = GameObject.CreatePrimitive( PrimitiveType.Quad);
			expr_06.transform.parent = parent.transform;
			expr_06.name = name;
			expr_06.transform.localPosition = position;
			expr_06.transform.localRotation = rotation;
			expr_06.transform.localScale = scale;
			Material material = new Material(this.mMatteShader);
			expr_06.GetComponent<Renderer>().material = material;
			Collider component = expr_06.GetComponent<Collider>();
			if (component)
			{
				UnityEngine.Object.Destroy(component);
			}
			expr_06.layer = layer;
			return expr_06;
		}

		private bool HasCalculationDataChanged()
		{
			if (this.mBgPlane != null)
			{
				bool expr_9D = this.mBgPlaneLocalPos != this.mBgPlane.transform.localPosition || this.mBgPlaneLocalScale != this.mBgPlane.transform.localScale || this.mCameraNearPlane != this.mCamera.nearClipPlane || this.mCameraFarPlane != this.mCamera.farClipPlane || this.mCameraFieldOfView != this.mCamera.fieldOfView || this.mCameraPixelRect != this.mCamera.pixelRect;
				if (expr_9D)
				{
					this.mBgPlaneLocalPos = this.mBgPlane.transform.localPosition;
					this.mBgPlaneLocalScale = this.mBgPlane.transform.localScale;
					this.mCameraFieldOfView = this.mCamera.fieldOfView;
					this.mCameraNearPlane = this.mCamera.nearClipPlane;
					this.mCameraFarPlane = this.mCamera.farClipPlane;
					this.mCameraPixelRect = this.mCamera.pixelRect;
				}
				return expr_9D;
			}
			return false;
		}

		public StencilHideExcessAreaClipping(GameObject gameObject, Shader matteShader)
		{
			this.mGameObject = gameObject;
			this.mMatteShader = matteShader;
		}

		public void SetPlanesRenderingActive(bool activeflag)
		{
			if (this.mClippingPlane != null)
			{
				this.mClippingPlane.SetActive(activeflag);
				this.mPlanesActivated = activeflag;
			}
		}

		public bool IsPlanesRenderingActive()
		{
			return this.mPlanesActivated;
		}

		public void OnPreCull()
		{
		}

		public void OnPostRender()
		{
		}

		public void Start()
		{
			DigitalEyewearARController instance = DigitalEyewearARController.Instance;
			if (instance != null)
			{
				this.mCamera = this.mGameObject.GetComponent<Camera>();
				BackgroundPlaneAbstractBehaviour componentInChildren = instance.CentralAnchorPoint.GetComponentInChildren<BackgroundPlaneAbstractBehaviour>();
				if (componentInChildren != null)
				{
					this.mBgPlane = componentInChildren.gameObject;
				}
				else
				{
					componentInChildren = instance.CentralAnchorPoint.transform.parent.GetComponentInChildren<BackgroundPlaneAbstractBehaviour>();
					if (componentInChildren != null)
					{
						this.mBgPlane = componentInChildren.gameObject;
					}
				}
				if (this.mBgPlane != null)
				{
					this.mClippingPlane = this.CreateQuad(this.mGameObject, "ClippingPlane", Vector3.zero, Quaternion.identity, Vector3.one, this.mBgPlane.layer);
				}
			}
		}

		public void OnDestroy()
		{
			if (this.mClippingPlane != null)
			{
				UnityEngine.Object.Destroy(this.mClippingPlane);
			}
		}

		public void Update(Vector3 planeOffset)
		{
			if (!this.HasCalculationDataChanged())
			{
				return;
			}
			this.SetPlanesRenderingActive(true);
			float num = (this.mCameraNearPlane + this.mCameraFarPlane) / 2f;
			Plane plane = new Plane(this.mCamera.transform.forward, this.mCamera.transform.position + this.mCamera.transform.forward * num);
			Ray ray = this.mCamera.ViewportPointToRay(new Vector3(0f, 0f, 0f));
			Ray ray2 = this.mCamera.ViewportPointToRay(new Vector3(1f, 1f, 0f));
			float num2 = 0f;
			plane.Raycast(ray, out num2);
			Vector3 vector = this.mCamera.transform.InverseTransformPoint(ray.GetPoint(num2));
			plane.Raycast(ray2, out num2);
			Vector3 expr_F7 = this.mCamera.transform.InverseTransformPoint(ray2.GetPoint(num2));
			float num3 = expr_F7.x - vector.x;
			float num4 = expr_F7.y - vector.y;
			this.mClippingPlane.transform.localPosition = new Vector3(vector.x + num3 * 0.5f, vector.y + num4 * 0.5f, num) + planeOffset;
			this.mClippingPlane.transform.localScale = new Vector3(num3 * 1.5f, num4 * 1.5f, 1f);
		}
	}
}
