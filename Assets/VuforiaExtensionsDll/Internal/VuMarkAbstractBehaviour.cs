using System;
using UnityEngine;

namespace Vuforia
{
	public abstract class VuMarkAbstractBehaviour : DataSetTrackableBehaviour
	{
		[HideInInspector, SerializeField]
		private float mAspectRatio;

		[HideInInspector, SerializeField]
		private float mWidth;

		[HideInInspector, SerializeField]
		private float mHeight;

		[HideInInspector, SerializeField]
		private string mPreviewImage;

		[HideInInspector, SerializeField]
		private InstanceIdType mIdType;

		[HideInInspector, SerializeField]
		private int mIdLength;

		[HideInInspector, SerializeField]
		private Rect mBoundingBox;

		[HideInInspector, SerializeField]
		private Vector2 mOrigin;

		[HideInInspector, SerializeField]
		private bool mTrackingFromRuntimeAppearance;

		private VuMarkTemplate mVuMarkTemplate;

		private VuMarkTarget mVuMarkTarget;

		private int mVuMarkResultId = -1;

		private Action mOnTargetAssigned;

		private Action mOnTargetLost;

		private float mLastTransformScale = -1f;

		private Vector2 mLastSize = new Vector2(-1f, -1f);

		public VuMarkTemplate VuMarkTemplate
		{
			get
			{
				return this.mVuMarkTemplate;
			}
		}

		public VuMarkTarget VuMarkTarget
		{
			get
			{
				return this.mVuMarkTarget;
			}
		}

		public Vector2 Origin
		{
			get
			{
				if (VuforiaRuntimeUtilities.IsVuforiaEnabled())
				{
					return this.mVuMarkTemplate.Origin;
				}
				float num = this.GetSize().x / this.mBoundingBox.width;
				Vector2 arg_59_0 = this.mBoundingBox.position * num;
				Vector2 vector = this.mBoundingBox.size * num;
				Vector2 vector2 = arg_59_0 + 0.5f * vector;
				return this.mOrigin * num - vector2;
			}
		}

		internal int VuMarkResultId
		{
			get
			{
				return this.mVuMarkResultId;
			}
			set
			{
				this.mVuMarkResultId = value;
			}
		}

		public VuMarkAbstractBehaviour()
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
			this.mTrackable = (this.mVuMarkTemplate = null);
			this.mVuMarkTarget = null;
		}

		protected override void CalculateDefaultOccluderBounds(out Vector3 boundsMin, out Vector3 boundsMax)
		{
			boundsMin = Vector3.zero;
			boundsMax = Vector3.zero;
		}

		protected override void ProtectedSetAsSmartTerrainInitializationTarget(ReconstructionFromTarget reconstructionFromTarget)
		{
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

		public void RegisterVuMarkTargetAssignedCallback(Action callback)
		{
			this.mOnTargetAssigned = (Action)Delegate.Combine(this.mOnTargetAssigned, callback);
		}

		public void UnregisterVuMarkTargetAssignedCallback(Action callback)
		{
			this.mOnTargetAssigned = (Action)Delegate.Remove(this.mOnTargetAssigned, callback);
		}

		public void RegisterVuMarkTargetLostCallback(Action callback)
		{
			this.mOnTargetLost = (Action)Delegate.Combine(this.mOnTargetLost, callback);
		}

		public void UnregisterVuMarkTargetLostCallback(Action callback)
		{
			this.mOnTargetLost = (Action)Delegate.Remove(this.mOnTargetLost, callback);
		}

		internal void UnregisterVuMarkTarget()
		{
			this.mVuMarkTarget = null;
			if (this.mOnTargetLost != null)
			{
				this.mOnTargetLost.InvokeWithExceptionHandling();
			}
		}

		internal void RegisterVuMarkTarget(VuMarkTarget target)
		{
			this.mVuMarkTarget = target;
			if (this.mOnTargetAssigned != null)
			{
				this.mOnTargetAssigned.InvokeWithExceptionHandling();
			}
		}

		internal override bool InitializeTarget(Trackable trackable, bool applyTargetScaleToBehaviour)
		{
			base.InitializeTarget(trackable, applyTargetScaleToBehaviour);
			VuMarkTemplateImpl vuMarkTemplateImpl = (VuMarkTemplateImpl)trackable;
			if (vuMarkTemplateImpl == null)
			{
				return false;
			}
			this.mTrackable = (this.mVuMarkTemplate = vuMarkTemplateImpl);
			this.mTrackableName = trackable.Name;
			this.mDataSetPath = vuMarkTemplateImpl.DataSet.Path;
			Vector3 size = vuMarkTemplateImpl.GetSize();
			this.mAspectRatio = size.y / size.x;
			if (applyTargetScaleToBehaviour)
			{
				float num = Mathf.Max(size.x, size.y);
				base.transform.localScale = new Vector3(num, num, num);
				base.CorrectScale();
			}
			Vector2 size2 = this.GetSize();
			this.mVuMarkTemplate.SetSize(size2);
			this.mVuMarkTemplate.TrackingFromRuntimeAppearance = this.mTrackingFromRuntimeAppearance;
			if (this.mExtendedTracking)
			{
				this.mVuMarkTemplate.StartExtendedTracking();
			}
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
