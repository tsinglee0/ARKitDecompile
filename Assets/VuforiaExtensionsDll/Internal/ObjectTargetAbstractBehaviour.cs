using System;
using UnityEngine;

namespace Vuforia
{
	public abstract class ObjectTargetAbstractBehaviour : DataSetTrackableBehaviour
	{
		private ObjectTarget mObjectTarget;

		[HideInInspector, SerializeField]
		private float mAspectRatioXY;

		[HideInInspector, SerializeField]
		private float mAspectRatioXZ;

		[HideInInspector, SerializeField]
		private bool mShowBoundingBox;

		[HideInInspector, SerializeField]
		private Vector3 mBBoxMin;

		[HideInInspector, SerializeField]
		private Vector3 mBBoxMax;

		[HideInInspector, SerializeField]
		private Texture2D mPreviewImage;

		[HideInInspector, SerializeField]
		private float mLength;

		[HideInInspector, SerializeField]
		private float mWidth;

		[HideInInspector, SerializeField]
		private float mHeight;

		private float mLastTransformScale = -1f;

		private Vector3 mLastSize = new Vector3(-1f, -1f, -1f);

		public ObjectTarget ObjectTarget
		{
			get
			{
				return this.mObjectTarget;
			}
		}

		public ObjectTargetAbstractBehaviour()
		{
			this.mAspectRatioXY = 1f;
			this.mAspectRatioXZ = 1f;
		}

        protected override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			if (this.mShowBoundingBox)
			{
				Gizmos.matrix = Matrix4x4.TRS(base.gameObject.transform.position, base.gameObject.transform.rotation, base.gameObject.transform.localScale);
				Gizmos.color = new Color(0.2f, 0.6f, 1f, 1f);
				Vector3 vector = new Vector3(1f, this.mBBoxMax.y / this.mBBoxMax.x, this.mBBoxMax.z / this.mBBoxMax.x);
				Gizmos.DrawWireCube(new Vector3(vector.x / 2f, vector.y / 2f, vector.z / 2f), vector);
				Gizmos.color = Color.black;
				float num = 10f;
				int num2 = (int)(this.mBBoxMax.x / num);
				int num3 = (int)(this.mBBoxMax.z / num);
				float num4 = num * (vector.x / this.mBBoxMax.x);
				float num5 = num * (vector.z / this.mBBoxMax.z);
				for (int i = 0; i < num2; i++)
				{
					Gizmos.DrawLine(new Vector3((float)i * num4, 0f, 0f), new Vector3((float)i * num4, 0f, vector.z));
				}
				for (int j = 0; j < num3; j++)
				{
					Gizmos.DrawLine(new Vector3(0f, 0f, (float)j * num5), new Vector3(vector.x, 0f, (float)j * num5));
				}
				Gizmos.color = new Color(1f, 1f, 1f, 0.8f);
				Gizmos.DrawCube(new Vector3(vector.x / 2f, 0f, vector.z / 2f), new Vector3(vector.x, 0f, vector.z));
				Gizmos.color = new Color(0.2f, 0.6f, 1f, 0.2f);
				Gizmos.DrawCube(new Vector3(vector.x / 2f, vector.y / 2f, 0f), new Vector3(vector.x, vector.y, 0f));
				Gizmos.DrawCube(new Vector3(0f, vector.y / 2f, vector.z / 2f), new Vector3(0f, vector.y, vector.z));
			}
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
			this.mTrackable = (this.mObjectTarget = null);
		}

		protected override void CalculateDefaultOccluderBounds(out Vector3 boundsMin, out Vector3 boundsMax)
		{
			boundsMin = Vector3.zero;
			boundsMax = Vector3.zero;
		}

		protected override void ProtectedSetAsSmartTerrainInitializationTarget(ReconstructionFromTarget reconstructionFromTarget)
		{
		}

		public void SetBoundingBox(Vector3 minBBox, Vector3 maxBBox)
		{
			this.mBBoxMin = minBBox;
			this.mBBoxMax = maxBBox;
		}

		public Vector3 GetSize()
		{
			if (this.mAspectRatioXY <= 1f)
			{
				return new Vector3(base.transform.localScale.x, base.transform.localScale.x * this.mAspectRatioXY, base.transform.localScale.x * this.mAspectRatioXZ);
			}
			return new Vector3(base.transform.localScale.x / this.mAspectRatioXY, base.transform.localScale.x, base.transform.localScale.x);
		}

		public void SetLength(float length)
		{
			base.SetScale(length, this.mAspectRatioXY, this.mAspectRatioXZ);
			this.mLength = length;
		}

		public void SetWidth(float width)
		{
			float aspectRatio = 1f / this.mAspectRatioXY;
			float aspectRatio2 = this.mAspectRatioXY / this.mAspectRatioXZ;
			base.SetScale(width, aspectRatio, aspectRatio2);
			this.mWidth = width;
		}

		public void SetHeight(float height)
		{
			float aspectRatio = 1f / this.mAspectRatioXZ;
			float aspectRatio2 = this.mAspectRatioXZ / this.mAspectRatioXY;
			base.SetScale(height, aspectRatio, aspectRatio2);
			this.mHeight = height;
		}

		internal override bool InitializeTarget(Trackable trackable, bool applyTargetScaleToBehaviour)
		{
			base.InitializeTarget(trackable, applyTargetScaleToBehaviour);
			ObjectTargetImpl objectTargetImpl = trackable as ObjectTargetImpl;
			if (objectTargetImpl == null)
			{
				return false;
			}
			this.mTrackable = (this.mObjectTarget = objectTargetImpl);
			this.mTrackableName = objectTargetImpl.Name;
			this.mDataSetPath = objectTargetImpl.DataSet.Path;
			Vector3 size = objectTargetImpl.GetSize();
			this.mAspectRatioXY = size.y / size.x;
			this.mAspectRatioXZ = size.z / size.x;
			if (applyTargetScaleToBehaviour)
			{
				float num = Mathf.Max(new float[]
				{
					size.x,
					size.y,
					size.z
				});
				base.transform.localScale = new Vector3(num, num, num);
				base.CorrectScale();
			}
			Vector3 size2 = this.GetSize();
			objectTargetImpl.SetSize(size2);
			if (this.mExtendedTracking)
			{
				this.mObjectTarget.StartExtendedTracking();
			}
			return true;
		}

		protected override void OnValidate()
		{
			base.OnValidate();
			bool flag = false;
			if (!Mathf.Approximately(this.mLastTransformScale, base.transform.localScale.x))
			{
				flag = true;
			}
			else if (!Mathf.Approximately(this.mLastSize.x, this.mLength))
			{
				this.SetLength(this.mLength);
				flag = true;
			}
			else if (!Mathf.Approximately(this.mLastSize.y, this.mWidth))
			{
				this.SetWidth(this.mWidth);
				flag = true;
			}
			else if (!Mathf.Approximately(this.mLastSize.z, this.mHeight))
			{
				this.SetHeight(this.mHeight);
				flag = true;
			}
			if (flag)
			{
				Vector3 size = this.GetSize();
				this.mLength = size.x;
				this.mWidth = size.y;
				this.mHeight = size.z;
			}
			this.mLastSize = new Vector3(this.mLength, this.mWidth, this.mHeight);
			this.mLastTransformScale = base.transform.localScale.x;
		}
	}
}
