using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
	public abstract class ImageTargetAbstractBehaviour : DataSetTrackableBehaviour
	{
		[HideInInspector, SerializeField]
		private float mAspectRatio;

		[HideInInspector, SerializeField]
		private ImageTargetType mImageTargetType;

		[HideInInspector, SerializeField]
		private float mWidth;

		[HideInInspector, SerializeField]
		private float mHeight;

		private ImageTarget mImageTarget;

		private Dictionary<int, VirtualButtonAbstractBehaviour> mVirtualButtonBehaviours;

		private float mLastTransformScale = -1f;

		private Vector2 mLastSize = new Vector2(-1f, -1f);

		public ImageTarget ImageTarget
		{
			get
			{
				return this.mImageTarget;
			}
		}

		public ImageTargetType ImageTargetType
		{
			get
			{
				return this.mImageTargetType;
			}
		}

		public ImageTargetAbstractBehaviour()
		{
			this.mAspectRatio = 1f;
		}

		protected override bool CorrectScaleImpl()
		{
			if (base.EnforceUniformScaling())
			{
				this.OnValidate();
				return true;
			}
			return false;
		}

		protected override void InternalUnregisterTrackable()
		{
			this.mTrackable = (this.mImageTarget = null);
		}

		protected override void CalculateDefaultOccluderBounds(out Vector3 boundsMin, out Vector3 boundsMax)
		{
			Vector2 vector = this.GetSize();
			vector *= 1.1f;
			boundsMin = new Vector3(vector.x * -0.5f, 0f, vector.y * -0.5f);
			boundsMax = new Vector3(vector.x * 0.5f, 0f, vector.y * 0.5f);
		}

		protected override void ProtectedSetAsSmartTerrainInitializationTarget(ReconstructionFromTarget reconstructionFromTarget)
		{
			if (this.mImageTarget != null || this.mImageTarget.ImageTargetType != ImageTargetType.USER_DEFINED)
			{
				if (this.mIsSmartTerrainOccluderOffset)
				{
					reconstructionFromTarget.SetInitializationTarget(this.mImageTarget, this.mSmartTerrainOccluderBoundsMin, this.mSmartTerrainOccluderBoundsMax, this.mSmartTerrainOccluderOffset, this.mSmartTerrainOccluderRotation);
					return;
				}
				reconstructionFromTarget.SetInitializationTarget(this.mImageTarget, this.mSmartTerrainOccluderBoundsMin, this.mSmartTerrainOccluderBoundsMax);
			}
		}

		public VirtualButtonAbstractBehaviour CreateVirtualButton(string vbName, Vector2 position, Vector2 size)
		{
			GameObject gameObject = new GameObject(vbName);
			VirtualButtonAbstractBehaviour virtualButtonAbstractBehaviour = BehaviourComponentFactory.Instance.AddVirtualButtonBehaviour(gameObject);
			gameObject.transform.parent = base.transform;
			virtualButtonAbstractBehaviour.SetVirtualButtonName(vbName);
			virtualButtonAbstractBehaviour.transform.localScale = new Vector3(size.x, 1f, size.y);
			virtualButtonAbstractBehaviour.transform.localPosition = new Vector3(position.x, 1f, position.y);
			if (Application.isPlaying && !this.CreateNewVirtualButtonFromBehaviour(virtualButtonAbstractBehaviour))
			{
				return null;
			}
			virtualButtonAbstractBehaviour.UnregisterOnDestroy = true;
			return virtualButtonAbstractBehaviour;
		}

		public static VirtualButtonAbstractBehaviour CreateVirtualButton(string vbName, Vector2 localScale, GameObject immediateParent)
		{
			GameObject gameObject = new GameObject(vbName);
			VirtualButtonAbstractBehaviour virtualButtonAbstractBehaviour = BehaviourComponentFactory.Instance.AddVirtualButtonBehaviour(gameObject);
			ImageTargetAbstractBehaviour componentInChildren = immediateParent.transform.root.gameObject.GetComponentInChildren<ImageTargetAbstractBehaviour>();
			if (componentInChildren == null || componentInChildren.ImageTarget == null)
			{
				Debug.LogError("Could not create Virtual Button. immediateParent\"immediateParent\" object is not an Image Target or a child of one.");
                UnityEngine.Object.Destroy(gameObject);
				return null;
			}
			gameObject.transform.parent = immediateParent.transform;
			virtualButtonAbstractBehaviour.SetVirtualButtonName(vbName);
			virtualButtonAbstractBehaviour.transform.localScale = new Vector3(localScale[0], 1f, localScale[1]);
			if (Application.isPlaying && !componentInChildren.CreateNewVirtualButtonFromBehaviour(virtualButtonAbstractBehaviour))
			{
				return null;
			}
			virtualButtonAbstractBehaviour.UnregisterOnDestroy = true;
			return virtualButtonAbstractBehaviour;
		}

		public IEnumerable<VirtualButtonAbstractBehaviour> GetVirtualButtonBehaviours()
		{
			return this.mVirtualButtonBehaviours.Values;
		}

		public void DestroyVirtualButton(string vbName)
		{
			foreach (VirtualButtonAbstractBehaviour current in new List<VirtualButtonAbstractBehaviour>(this.mVirtualButtonBehaviours.Values))
			{
				if (current.VirtualButtonName == vbName)
				{
					this.mVirtualButtonBehaviours.Remove(current.VirtualButton.ID);
					current.UnregisterOnDestroy = true;
                    UnityEngine.Object.Destroy(current.gameObject);
					break;
				}
			}
		}

		public Vector2 GetSize()
		{
			if (this.mAspectRatio <= 1f)
			{
				return new Vector2(base.transform.localScale.x, base.transform.localScale.x * this.mAspectRatio);
			}
			return new Vector2(base.transform.localScale.x / this.mAspectRatio, base.transform.localScale.x);
		}

		public void SetWidth(float width)
		{
			base.SetScaleFromWidth(width, this.mAspectRatio);
			this.mWidth = width;
		}

		public void SetHeight(float height)
		{
			base.SetScaleFromHeight(height, this.mAspectRatio);
			this.mHeight = height;
		}

		internal override bool InitializeTarget(Trackable trackable, bool applyTargetScaleToBehaviour)
		{
			base.InitializeTarget(trackable, applyTargetScaleToBehaviour);
			ImageTarget imageTarget = (ImageTarget)trackable;
			if (imageTarget == null)
			{
				return false;
			}
			this.mTrackable = (this.mImageTarget = imageTarget);
			this.mTrackableName = trackable.Name;
			this.mImageTargetType = imageTarget.ImageTargetType;
			if (imageTarget is ImageTargetImpl)
			{
				this.mDataSetPath = ((ImageTargetImpl)imageTarget).DataSet.Path;
			}
			if (applyTargetScaleToBehaviour || imageTarget.ImageTargetType != ImageTargetType.PREDEFINED)
			{
				Vector3 size = imageTarget.GetSize();
				float num = Mathf.Max(size.x, size.y);
				base.transform.localScale = new Vector3(num, num, num);
				base.CorrectScale();
				this.mAspectRatio = size.y / size.x;
			}
			this.mVirtualButtonBehaviours = new Dictionary<int, VirtualButtonAbstractBehaviour>();
			if (imageTarget.ImageTargetType == ImageTargetType.PREDEFINED)
			{
				Vector2 size2 = this.GetSize();
				imageTarget.SetSize(size2);
			}
			if (this.mExtendedTracking)
			{
				this.mImageTarget.StartExtendedTracking();
			}
			return true;
		}

		internal void AssociateExistingVirtualButtonBehaviour(VirtualButtonAbstractBehaviour virtualButtonBehaviour)
		{
			VirtualButton virtualButton = this.mImageTarget.GetVirtualButtonByName(virtualButtonBehaviour.VirtualButtonName);
			if (virtualButton == null)
			{
				Vector2 vector;
				Vector2 vector2;
				virtualButtonBehaviour.CalculateButtonArea(out vector, out vector2);
				RectangleData area = new RectangleData
				{
					leftTopX = vector.x,
					leftTopY = vector.y,
					rightBottomX = vector2.x,
					rightBottomY = vector2.y
				};
				virtualButton = this.mImageTarget.CreateVirtualButton(virtualButtonBehaviour.VirtualButtonName, area);
				if (virtualButton != null)
				{
					Debug.Log("Successfully created virtual button " + virtualButtonBehaviour.VirtualButtonName + " at startup");
					virtualButtonBehaviour.UnregisterOnDestroy = true;
				}
				else
				{
					Debug.LogError("Failed to create virtual button " + virtualButtonBehaviour.VirtualButtonName + " at startup");
				}
			}
			if (virtualButton != null && !this.mVirtualButtonBehaviours.ContainsKey(virtualButton.ID))
			{
				virtualButtonBehaviour.InitializeVirtualButton(virtualButton);
				this.mVirtualButtonBehaviours.Add(virtualButton.ID, virtualButtonBehaviour);
				Debug.Log(string.Concat(new object[]
				{
					"Found VirtualButton named ",
					virtualButtonBehaviour.VirtualButton.Name,
					" with id ",
					virtualButtonBehaviour.VirtualButton.ID
				}));
				virtualButtonBehaviour.UpdatePose();
				if (!virtualButtonBehaviour.UpdateAreaRectangle() || !virtualButtonBehaviour.UpdateSensitivity())
				{
					Debug.LogError("Failed to update virtual button " + virtualButtonBehaviour.VirtualButton.Name + " at startup");
					return;
				}
				Debug.Log("Updated virtual button " + virtualButtonBehaviour.VirtualButton.Name + " at startup");
			}
		}

		internal void CreateMissingVirtualButtonBehaviours()
		{
			foreach (VirtualButton current in this.mImageTarget.GetVirtualButtons())
			{
				this.CreateVirtualButtonFromNative(current);
			}
		}

		internal bool TryGetVirtualButtonBehaviourByID(int id, out VirtualButtonAbstractBehaviour virtualButtonBehaviour)
		{
			return this.mVirtualButtonBehaviours.TryGetValue(id, out virtualButtonBehaviour);
		}

		private void CreateVirtualButtonFromNative(VirtualButton virtualButton)
		{
			GameObject gameObject = new GameObject(virtualButton.Name);
			VirtualButtonAbstractBehaviour virtualButtonAbstractBehaviour = BehaviourComponentFactory.Instance.AddVirtualButtonBehaviour(gameObject);
			virtualButtonAbstractBehaviour.transform.parent = base.transform;
			Debug.Log(string.Concat(new object[]
			{
				"Creating Virtual Button with values: \n ID:           ",
				virtualButton.ID,
				"\n Name:         ",
				virtualButton.Name,
				"\n Rectangle:    ",
				virtualButton.Area.leftTopX,
				",",
				virtualButton.Area.leftTopY,
				",",
				virtualButton.Area.rightBottomX,
				",",
				virtualButton.Area.rightBottomY
			}));
			virtualButtonAbstractBehaviour.SetVirtualButtonName(virtualButton.Name);
			virtualButtonAbstractBehaviour.SetPosAndScaleFromButtonArea(new Vector2(virtualButton.Area.leftTopX, virtualButton.Area.leftTopY), new Vector2(virtualButton.Area.rightBottomX, virtualButton.Area.rightBottomY));
			virtualButtonAbstractBehaviour.UnregisterOnDestroy = false;
			virtualButtonAbstractBehaviour.InitializeVirtualButton(virtualButton);
			this.mVirtualButtonBehaviours.Add(virtualButton.ID, virtualButtonAbstractBehaviour);
		}

		private bool CreateNewVirtualButtonFromBehaviour(VirtualButtonAbstractBehaviour newVBB)
		{
			Vector2 vector;
			Vector2 vector2;
			newVBB.CalculateButtonArea(out vector, out vector2);
			RectangleData area = new RectangleData
			{
				leftTopX = vector.x,
				leftTopY = vector.y,
				rightBottomX = vector2.x,
				rightBottomY = vector2.y
			};
			VirtualButton virtualButton = this.mImageTarget.CreateVirtualButton(newVBB.VirtualButtonName, area);
			if (virtualButton == null)
			{
                UnityEngine.Object.Destroy(newVBB.gameObject);
				return false;
			}
			newVBB.InitializeVirtualButton(virtualButton);
			this.mVirtualButtonBehaviours.Add(virtualButton.ID, newVBB);
			return true;
		}

		protected override void OnValidate()
		{
			base.OnValidate();
			if (!Mathf.Approximately(this.mLastTransformScale, base.transform.localScale.x))
			{
				Vector2 size = this.GetSize();
				this.mWidth = size.x;
				this.mHeight = size.y;
			}
			else if (!Mathf.Approximately(this.mLastSize.x, this.mWidth))
			{
				this.SetWidth(this.mWidth);
				this.mHeight = this.GetSize().y;
			}
			else if (!Mathf.Approximately(this.mLastSize.y, this.mHeight))
			{
				this.SetHeight(this.mHeight);
				this.mWidth = this.GetSize().x;
			}
			this.mLastSize = new Vector2(this.mWidth, this.mHeight);
			this.mLastTransformScale = base.transform.localScale.x;
		}
	}
}
