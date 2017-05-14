using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
	public abstract class VirtualButtonAbstractBehaviour : MonoBehaviour
	{
		public const float TARGET_OFFSET = 0.001f;

		[HideInInspector, SerializeField]
		private string mName;

		[HideInInspector, SerializeField]
		private VirtualButton.Sensitivity mSensitivity;

		[HideInInspector, SerializeField]
		private bool mHasUpdatedPose;

		[HideInInspector, SerializeField]
		private Matrix4x4 mPrevTransform = Matrix4x4.zero;

		[HideInInspector, SerializeField]
		private GameObject mPrevParent;

		private bool mSensitivityDirty;

		private VirtualButton.Sensitivity mPreviousSensitivity;

		private bool mPreviouslyEnabled;

		private bool mPressed;

		private List<IVirtualButtonEventHandler> mHandlers;

		private Vector2 mLeftTop;

		private Vector2 mRightBottom;

		private bool mUnregisterOnDestroy;

		private VirtualButton mVirtualButton;

		public string VirtualButtonName
		{
			get
			{
				return this.mName;
			}
		}

		public bool Pressed
		{
			get
			{
				return this.mPressed;
			}
		}

		public bool HasUpdatedPose
		{
			get
			{
				return this.mHasUpdatedPose;
			}
		}

		public bool UnregisterOnDestroy
		{
			get
			{
				return this.mUnregisterOnDestroy;
			}
			set
			{
				this.mUnregisterOnDestroy = value;
			}
		}

		public VirtualButton VirtualButton
		{
			get
			{
				return this.mVirtualButton;
			}
		}

		internal VirtualButton.Sensitivity Sensitivity
		{
			get
			{
				return this.mSensitivity;
			}
			set
			{
				this.mSensitivity = value;
			}
		}

		public Matrix4x4 PreviousTransform
		{
			get
			{
				return this.mPrevTransform;
			}
			set
			{
				this.mPrevTransform = value;
			}
		}

		public GameObject PreviousParent
		{
			get
			{
				return this.mPrevParent;
			}
			set
			{
				this.mPrevParent = value;
			}
		}

		public VirtualButtonAbstractBehaviour()
		{
			this.mName = "";
			this.mPressed = false;
			this.mSensitivity = VirtualButton.Sensitivity.LOW;
			this.mSensitivityDirty = false;
			this.mHandlers = new List<IVirtualButtonEventHandler>();
			this.mHasUpdatedPose = false;
		}

		public void RegisterEventHandler(IVirtualButtonEventHandler eventHandler)
		{
			this.mHandlers.Add(eventHandler);
		}

		public bool UnregisterEventHandler(IVirtualButtonEventHandler eventHandler)
		{
			return this.mHandlers.Remove(eventHandler);
		}

		public bool CalculateButtonArea(out Vector2 topLeft, out Vector2 bottomRight)
		{
			ImageTargetAbstractBehaviour imageTargetBehaviour = this.GetImageTargetBehaviour();
			if (imageTargetBehaviour == null)
			{
				topLeft = (bottomRight = Vector2.zero);
				return false;
			}
			Vector3 vector = imageTargetBehaviour.transform.InverseTransformPoint(base.transform.position);
			float num = imageTargetBehaviour.transform.lossyScale[0];
			Vector2 vector2 = new Vector2(vector[0] * num, vector[2] * num);
			Vector2 vector3 = Vector2.Scale(new Vector2(base.transform.lossyScale[0], base.transform.lossyScale[2]) * 0.5f, new Vector2(1f, -1f));
			topLeft = vector2 - vector3;
			bottomRight = vector2 + vector3;
			return true;
		}

		public bool UpdateAreaRectangle()
		{
			RectangleData area = default(RectangleData);
			area.leftTopX = this.mLeftTop.x;
			area.leftTopY = this.mLeftTop.y;
			area.rightBottomX = this.mRightBottom.x;
			area.rightBottomY = this.mRightBottom.y;
			return this.mVirtualButton != null && this.mVirtualButton.SetArea(area);
		}

		public bool UpdateSensitivity()
		{
			return this.mVirtualButton != null && this.mVirtualButton.SetSensitivity(this.mSensitivity);
		}

		private bool UpdateEnabled()
		{
			return this.mVirtualButton != null && this.mVirtualButton.SetEnabled(base.enabled);
		}

		public bool UpdatePose()
		{
			ImageTargetAbstractBehaviour imageTargetBehaviour = this.GetImageTargetBehaviour();
			if (imageTargetBehaviour == null)
			{
				return false;
			}
			Transform parent = base.transform.parent;
			while (parent != null)
			{
				if (parent.localScale[0] != parent.localScale[1] || parent.localScale[0] != parent.localScale[2])
				{
					Debug.LogWarning("Detected non-uniform scale in virtual  button object hierarchy. Forcing uniform scaling of object '" + parent.name + "'.");
					parent.localScale = new Vector3(parent.localScale[0], parent.localScale[0], parent.localScale[0]);
				}
				parent = parent.parent;
			}
			this.mHasUpdatedPose = true;
			if (base.transform.parent != null && base.transform.parent.gameObject != imageTargetBehaviour.gameObject)
			{
				base.transform.localPosition = Vector3.zero;
			}
			Vector3 vector = imageTargetBehaviour.transform.InverseTransformPoint(base.transform.position);
			vector.y = 0.001f;
			Vector3 position = imageTargetBehaviour.transform.TransformPoint(vector);
			base.transform.position = position;
			base.transform.rotation = imageTargetBehaviour.transform.rotation;
			Vector2 vec;
			Vector2 vec2;
			this.CalculateButtonArea(out vec, out vec2);
			float threshold = imageTargetBehaviour.transform.localScale[0] * 0.001f;
			if (!VirtualButtonAbstractBehaviour.Equals(vec, this.mLeftTop, threshold) || !VirtualButtonAbstractBehaviour.Equals(vec2, this.mRightBottom, threshold))
			{
				this.mLeftTop = vec;
				this.mRightBottom = vec2;
				return true;
			}
			return false;
		}

		public void OnTrackerUpdated(bool pressed)
		{
			if (this.mPreviouslyEnabled != base.enabled)
			{
				this.mPreviouslyEnabled = base.enabled;
				this.UpdateEnabled();
			}
			if (!base.enabled)
			{
				return;
			}
			if (this.mPressed != pressed && this.mHandlers != null)
			{
				if (pressed)
				{
					using (List<IVirtualButtonEventHandler>.Enumerator enumerator = this.mHandlers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							enumerator.Current.OnButtonPressed(this);
						}
						goto IL_A6;
					}
				}
				using (List<IVirtualButtonEventHandler>.Enumerator enumerator = this.mHandlers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.OnButtonReleased(this);
					}
				}
			}
			IL_A6:
			this.mPressed = pressed;
		}

		public ImageTargetAbstractBehaviour GetImageTargetBehaviour()
		{
			if (base.transform.parent == null)
			{
				return null;
			}
			GameObject gameObject = base.transform.parent.gameObject;
			while (gameObject != null)
			{
				ImageTargetAbstractBehaviour component = gameObject.GetComponent<ImageTargetAbstractBehaviour>();
				if (component != null)
				{
					return component;
				}
				if (gameObject.transform.parent == null)
				{
					return null;
				}
				gameObject = gameObject.transform.parent.gameObject;
			}
			return null;
		}

		internal void SetVirtualButtonName(string name)
		{
			this.mName = name;
		}

		internal void InitializeVirtualButton(VirtualButton virtualButton)
		{
			this.mVirtualButton = virtualButton;
		}

		public bool SetPosAndScaleFromButtonArea(Vector2 topLeft, Vector2 bottomRight)
		{
			ImageTargetAbstractBehaviour imageTargetBehaviour = this.GetImageTargetBehaviour();
			if (imageTargetBehaviour == null)
			{
				return false;
			}
			float num = imageTargetBehaviour.transform.lossyScale[0];
			Vector2 vector = (topLeft + bottomRight) * 0.5f;
			Vector2 vector2 = new Vector2(bottomRight[0] - topLeft[0], topLeft[1] - bottomRight[1]);
			Vector3 vector3 = new Vector3(vector[0] / num, 0.001f, vector[1] / num);
			Vector3 vector4 = new Vector3(vector2[0], (vector2[0] + vector2[1]) * 0.5f, vector2[1]);
			base.transform.position = imageTargetBehaviour.transform.TransformPoint(vector3);
			base.transform.localScale = vector4 / base.transform.parent.lossyScale[0];
			return true;
		}

		private void LateUpdate()
		{
			if (this.UpdatePose())
			{
				this.UpdateAreaRectangle();
			}
			if (this.mSensitivityDirty && this.UpdateSensitivity())
			{
				this.mSensitivityDirty = false;
			}
		}

		private void OnDisable()
		{
			if (VuforiaRuntimeUtilities.IsVuforiaEnabled())
			{
				if (this.mPreviouslyEnabled != base.enabled)
				{
					this.mPreviouslyEnabled = base.enabled;
					this.UpdateEnabled();
				}
				if (this.mPressed && this.mHandlers != null)
				{
					using (List<IVirtualButtonEventHandler>.Enumerator enumerator = this.mHandlers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							enumerator.Current.OnButtonReleased(this);
						}
					}
				}
				this.mPressed = false;
			}
		}

		private void OnDestroy()
		{
			if (Application.isPlaying && this.mUnregisterOnDestroy)
			{
				ImageTargetAbstractBehaviour imageTargetBehaviour = this.GetImageTargetBehaviour();
				if (imageTargetBehaviour != null)
				{
					imageTargetBehaviour.ImageTarget.DestroyVirtualButton(this.mVirtualButton);
				}
			}
		}

		private void OnValidate()
		{
			if (this.mSensitivity != this.mPreviousSensitivity)
			{
				this.mSensitivityDirty = true;
				this.mPreviousSensitivity = this.mSensitivity;
			}
		}

		private static bool Equals(Vector2 vec1, Vector2 vec2, float threshold)
		{
			Vector2 vector = vec1 - vec2;
			return Math.Abs(vector.x) < threshold && Math.Abs(vector.y) < threshold;
		}
	}
}
