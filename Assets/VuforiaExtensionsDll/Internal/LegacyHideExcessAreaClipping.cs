using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
	internal class LegacyHideExcessAreaClipping : IExcessAreaClipping
	{
		private GameObject mGameObject;

		private Shader mMatteShader;

		private GameObject mBgPlane;

		private GameObject mLeftPlane;

		private GameObject mRightPlane;

		private GameObject mTopPlane;

		private GameObject mBottomPlane;

		private Camera mCamera;

		private Vector3 mBgPlaneLocalPos = Vector3.zero;

		private Vector3 mBgPlaneLocalScale = Vector3.zero;

		private float mCameraNearPlane;

		private Rect mCameraPixelRect = new Rect(0f, 0f, 0f, 0f);

		private float mCameraFieldOFView;

		private VuforiaARController mVuforiaBehaviour;

		private HideExcessAreaAbstractBehaviour[] mHideBehaviours;

		private List<HideExcessAreaAbstractBehaviour> mDeactivatedHideBehaviours;

		private bool mPlanesActivated;

		private Vector3 mLeftPlaneCachedScale = Vector3.one;

		private Vector3 mRightPlaneCachedScale = Vector3.one;

		private Vector3 mBottomPlaneCachedScale = Vector3.one;

		private Vector3 mTopPlaneCachedScale = Vector3.one;

		private GameObject CreateQuad(GameObject parent, string name, Vector3 position, Quaternion rotation, Vector3 scale, int layer)
		{
			GameObject expr_06 = GameObject.CreatePrimitive(PrimitiveType.Quad);
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
				bool expr_8A = this.mBgPlaneLocalPos != this.mBgPlane.transform.localPosition || this.mBgPlaneLocalScale != this.mBgPlane.transform.localScale || this.mCameraNearPlane != this.mCamera.nearClipPlane || this.mCameraFieldOFView != this.mCamera.fieldOfView || this.mCameraPixelRect != this.mCamera.pixelRect;
				if (expr_8A)
				{
					this.mBgPlaneLocalPos = this.mBgPlane.transform.localPosition;
					this.mBgPlaneLocalScale = this.mBgPlane.transform.localScale;
					this.mCameraFieldOFView = this.mCamera.fieldOfView;
					this.mCameraNearPlane = this.mCamera.nearClipPlane;
					this.mCameraPixelRect = this.mCamera.pixelRect;
				}
				return expr_8A;
			}
			return false;
		}

		public LegacyHideExcessAreaClipping(GameObject gameObject, Shader matteShader)
		{
			this.mGameObject = gameObject;
			this.mMatteShader = matteShader;
		}

		public void SetPlanesRenderingActive(bool activeflag)
		{
			if (this.mLeftPlane != null && this.mRightPlane != null && this.mTopPlane != null && this.mBottomPlane != null)
			{
				if (!activeflag)
				{
					this.mLeftPlane.transform.localScale = Vector3.zero;
					this.mRightPlane.transform.localScale = Vector3.zero;
					this.mTopPlane.transform.localScale = Vector3.zero;
					this.mBottomPlane.transform.localScale = Vector3.zero;
				}
				else
				{
					this.mLeftPlane.transform.localScale = this.mLeftPlaneCachedScale;
					this.mRightPlane.transform.localScale = this.mRightPlaneCachedScale;
					this.mTopPlane.transform.localScale = this.mTopPlaneCachedScale;
					this.mBottomPlane.transform.localScale = this.mBottomPlaneCachedScale;
				}
				this.mPlanesActivated = activeflag;
			}
		}

		public bool IsPlanesRenderingActive()
		{
			return this.mPlanesActivated;
		}

		public void OnPreCull()
		{
			if (this.mDeactivatedHideBehaviours != null)
			{
				this.SetPlanesRenderingActive(true);
				this.mDeactivatedHideBehaviours.Clear();
				HideExcessAreaAbstractBehaviour[] array = this.mHideBehaviours;
				for (int i = 0; i < array.Length; i++)
				{
					HideExcessAreaAbstractBehaviour hideExcessAreaAbstractBehaviour = array[i];
					if (hideExcessAreaAbstractBehaviour != null && hideExcessAreaAbstractBehaviour.gameObject != this.mGameObject && hideExcessAreaAbstractBehaviour.IsPlanesRenderingActive())
					{
						hideExcessAreaAbstractBehaviour.SetPlanesRenderingActive(false);
						this.mDeactivatedHideBehaviours.Add(hideExcessAreaAbstractBehaviour);
					}
				}
			}
		}

		public void OnPostRender()
		{
			if (this.mDeactivatedHideBehaviours != null)
			{
				this.SetPlanesRenderingActive(false);
				using (List<HideExcessAreaAbstractBehaviour>.Enumerator enumerator = this.mDeactivatedHideBehaviours.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.SetPlanesRenderingActive(true);
					}
				}
				this.mDeactivatedHideBehaviours.Clear();
			}
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
					this.mHideBehaviours = UnityEngine.Object.FindObjectsOfType<HideExcessAreaAbstractBehaviour>();
					this.mDeactivatedHideBehaviours = new List<HideExcessAreaAbstractBehaviour>();
					this.mLeftPlane = this.CreateQuad(this.mGameObject, "LeftPlane", Vector3.zero, Quaternion.identity, Vector3.one, this.mBgPlane.layer);
					this.mRightPlane = this.CreateQuad(this.mGameObject, "RightPlane", Vector3.zero, Quaternion.identity, Vector3.one, this.mBgPlane.layer);
					this.mTopPlane = this.CreateQuad(this.mGameObject, "TopPlane", Vector3.zero, Quaternion.identity, Vector3.one, this.mBgPlane.layer);
					this.mBottomPlane = this.CreateQuad(this.mGameObject, "BottomPlane", Vector3.zero, Quaternion.identity, Vector3.one, this.mBgPlane.layer);
					this.SetPlanesRenderingActive(false);
				}
			}
		}

		public void OnDestroy()
		{
			if (this.mDeactivatedHideBehaviours != null)
			{
				this.mDeactivatedHideBehaviours.Clear();
			}
			if (this.mLeftPlane)
			{
                UnityEngine.Object.Destroy(this.mLeftPlane);
			}
			if (this.mRightPlane)
			{
                UnityEngine.Object.Destroy(this.mRightPlane);
			}
			if (this.mTopPlane)
			{
                UnityEngine.Object.Destroy(this.mTopPlane);
			}
			if (this.mBottomPlane)
			{
                UnityEngine.Object.Destroy(this.mBottomPlane);
			}
		}

		public void Update(Vector3 planeOffset)
		{
			if (!this.HasCalculationDataChanged())
			{
				return;
			}
			this.SetPlanesRenderingActive(true);
			float num = this.mCameraNearPlane * 1.01f;
			Plane plane = new Plane(this.mCamera.transform.forward, this.mCamera.transform.position + this.mCamera.transform.forward * num);
			Ray ray = this.mCamera.ScreenPointToRay(new Vector3(this.mCameraPixelRect.xMin, this.mCameraPixelRect.yMin, 0f));
			Ray ray2 = this.mCamera.ScreenPointToRay(new Vector3(this.mCameraPixelRect.xMax, this.mCameraPixelRect.yMax, 0f));
			float num2 = 0f;
			plane.Raycast(ray, out num2);
			Vector3 vector = this.mCamera.transform.InverseTransformPoint(ray.GetPoint(num2));
			plane.Raycast(ray2, out num2);
			Vector3 arg_228_0 = this.mCamera.transform.InverseTransformPoint(ray2.GetPoint(num2));
			Quaternion quaternion = Quaternion.Inverse(this.mBgPlane.transform.localRotation) * Quaternion.AngleAxis(270f, Vector3.right);
			Vector3 vector2 = this.mBgPlane.transform.TransformPoint(quaternion * new Vector3(-1f, 0f, -1f));
			Vector3 vector3 = this.mCamera.WorldToScreenPoint(vector2);
			Ray ray3 = this.mCamera.ScreenPointToRay(vector3);
			Vector3 vector4 = this.mBgPlane.transform.TransformPoint(quaternion * new Vector3(1f, 0f, 1f));
			Vector3 vector5 = this.mCamera.WorldToScreenPoint(vector4);
			Ray ray4 = this.mCamera.ScreenPointToRay(vector5);
			plane.Raycast(ray3, out num2);
			Vector3 vector6 = this.mCamera.transform.InverseTransformPoint(ray3.GetPoint(num2));
			plane.Raycast(ray4, out num2);
			Vector3 vector7 = this.mCamera.transform.InverseTransformPoint(ray4.GetPoint(num2));
			float num3 = vector6.x - vector.x;
			float num4 = arg_228_0.y - vector.y;
			this.mLeftPlane.transform.localPosition = new Vector3(vector.x + num3 * 0.5f, vector.y + num4 * 0.5f, num) + planeOffset;
			this.mLeftPlane.transform.localScale = new Vector3(num3, num4, 1f);
			float num5 = arg_228_0.x - vector7.x;
			float num6 = arg_228_0.y - vector.y;
			this.mRightPlane.transform.localPosition = new Vector3(vector7.x + num5 * 0.5f, vector.y + num6 * 0.5f, num) + planeOffset;
			this.mRightPlane.transform.localScale = new Vector3(num5, num6, 1f);
			float num7 = vector7.x - vector6.x;
			float num8 = arg_228_0.y - vector7.y;
			this.mTopPlane.transform.localPosition = new Vector3(vector6.x + num7 * 0.5f, vector7.y + num8 * 0.5f, num) + planeOffset;
			this.mTopPlane.transform.localScale = new Vector3(num7, num8, 1f);
			float num9 = vector7.x - vector6.x;
			float num10 = vector6.y - vector.y;
			this.mBottomPlane.transform.localPosition = new Vector3(vector6.x + num9 * 0.5f, vector.y + num10 * 0.5f, num) + planeOffset;
			this.mBottomPlane.transform.localScale = new Vector3(num9, num10, 1f);
			this.mLeftPlaneCachedScale = this.mLeftPlane.transform.localScale;
			this.mRightPlaneCachedScale = this.mRightPlane.transform.localScale;
			this.mBottomPlaneCachedScale = this.mBottomPlane.transform.localScale;
			this.mTopPlaneCachedScale = this.mTopPlane.transform.localScale;
		}
	}
}
