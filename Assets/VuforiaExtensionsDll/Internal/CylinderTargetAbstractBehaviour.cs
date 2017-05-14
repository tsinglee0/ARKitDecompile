using System;
using UnityEngine;

namespace Vuforia
{
	public abstract class CylinderTargetAbstractBehaviour : DataSetTrackableBehaviour
	{
		private CylinderTarget mCylinderTarget;

		[HideInInspector, SerializeField]
		private float mTopDiameterRatio;

		[HideInInspector, SerializeField]
		private float mBottomDiameterRatio;

		[HideInInspector, SerializeField]
		private float mSideLength;

		[HideInInspector, SerializeField]
		private float mTopDiameter;

		[HideInInspector, SerializeField]
		private float mBottomDiameter;

		private int mFrameIndex = -1;

		private int mUpdateFrameIndex = -1;

		private float mFutureScale;

		private float mLastTransformScale = -1f;

		public CylinderTarget CylinderTarget
		{
			get
			{
				return this.mCylinderTarget;
			}
		}

		public float SideLength
		{
			get
			{
				return this.GetScale();
			}
		}

		public float TopDiameter
		{
			get
			{
				return this.mTopDiameterRatio * this.GetScale();
			}
		}

		public float BottomDiameter
		{
			get
			{
				return this.mBottomDiameterRatio * this.GetScale();
			}
		}

		public bool SetSideLength(float value)
		{
			return this.SetScale(value);
		}

		public bool SetTopDiameter(float value)
		{
			return !Mathf.Approximately(this.mTopDiameterRatio, 0f) && this.SetScale(value / this.mTopDiameterRatio);
		}

		public bool SetBottomDiameter(float value)
		{
			return !Mathf.Approximately(this.mBottomDiameterRatio, 0f) && this.SetScale(value / this.mBottomDiameterRatio);
		}

		public override void OnFrameIndexUpdate(int newFrameIndex)
		{
			if (this.mUpdateFrameIndex >= 0 && this.mUpdateFrameIndex != newFrameIndex)
			{
				this.ApplyScale(this.mFutureScale);
				this.mUpdateFrameIndex = -1;
			}
			this.mFrameIndex = newFrameIndex;
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
			this.mTrackable = (this.mCylinderTarget = null);
		}

		protected override void CalculateDefaultOccluderBounds(out Vector3 boundsMin, out Vector3 boundsMax)
		{
			float num = Math.Max(this.BottomDiameter, this.TopDiameter);
			num *= 1.1f;
			boundsMin = new Vector3(num * -0.5f, 0f, num * -0.5f);
			boundsMax = new Vector3(num * 0.5f, this.SideLength, num * 0.5f);
		}

		protected override void ProtectedSetAsSmartTerrainInitializationTarget(ReconstructionFromTarget reconstructionFromTarget)
		{
			if (this.mCylinderTarget != null)
			{
				if (this.mIsSmartTerrainOccluderOffset)
				{
					reconstructionFromTarget.SetInitializationTarget(this.mCylinderTarget, this.mSmartTerrainOccluderBoundsMin, this.mSmartTerrainOccluderBoundsMax, this.mSmartTerrainOccluderOffset, this.mSmartTerrainOccluderRotation);
					return;
				}
				reconstructionFromTarget.SetInitializationTarget(this.mCylinderTarget, this.mSmartTerrainOccluderBoundsMin, this.mSmartTerrainOccluderBoundsMax);
			}
		}

		internal override bool InitializeTarget(Trackable trackable, bool applyTargetScaleToBehaviour)
		{
			base.InitializeTarget(trackable, applyTargetScaleToBehaviour);
			CylinderTargetImpl cylinderTargetImpl = (CylinderTargetImpl)trackable;
			if (cylinderTargetImpl == null)
			{
				return false;
			}
			this.mTrackable = (this.mCylinderTarget = cylinderTargetImpl);
			this.mTrackableName = trackable.Name;
			this.mDataSetPath = cylinderTargetImpl.DataSet.Path;
			this.mTopDiameterRatio = cylinderTargetImpl.GetTopDiameter() / cylinderTargetImpl.GetSideLength();
			this.mBottomDiameterRatio = cylinderTargetImpl.GetBottomDiameter() / cylinderTargetImpl.GetSideLength();
			if (applyTargetScaleToBehaviour)
			{
				float sideLength = cylinderTargetImpl.GetSideLength();
				base.transform.localScale = new Vector3(sideLength, sideLength, sideLength);
				base.CorrectScale();
			}
			cylinderTargetImpl.SetSideLength(this.SideLength);
			if (this.mExtendedTracking)
			{
				this.mCylinderTarget.StartExtendedTracking();
			}
			return true;
		}

		private float GetScale()
		{
			return base.transform.localScale.x;
		}

		private bool SetScale(float value)
		{
			if (base.transform.localScale.x == value)
			{
				return true;
			}
			if (this.mCylinderTarget != null)
			{
				if (!this.mCylinderTarget.SetSideLength(value))
				{
					return false;
				}
				this.mUpdateFrameIndex = this.mFrameIndex;
				this.mFutureScale = value;
			}
			else
			{
				this.ApplyScale(value);
			}
			return true;
		}

		private void ApplyScale(float value)
		{
			base.transform.localScale = new Vector3(value, value, value);
		}

		protected override void OnValidate()
		{
			base.OnValidate();
			if (!Mathf.Approximately(this.mLastTransformScale, base.transform.localScale.x))
			{
				this.SetSideLength(base.transform.localScale.x);
			}
			else if (!Mathf.Approximately(this.mSideLength, this.SideLength))
			{
				this.SetSideLength(this.mSideLength);
			}
			else if (!Mathf.Approximately(this.mTopDiameter, this.TopDiameter))
			{
				this.SetTopDiameter(this.mTopDiameter);
			}
			else if (!Mathf.Approximately(this.mBottomDiameter, this.BottomDiameter))
			{
				this.SetBottomDiameter(this.mBottomDiameter);
			}
			this.mSideLength = this.SideLength;
			this.mTopDiameter = this.TopDiameter;
			this.mBottomDiameter = this.BottomDiameter;
			this.mLastTransformScale = this.mSideLength;
		}
	}
}
