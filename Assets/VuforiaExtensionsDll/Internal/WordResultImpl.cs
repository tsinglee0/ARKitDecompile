using System;
using UnityEngine;

namespace Vuforia
{
	internal class WordResultImpl : WordResult
	{
		private OrientedBoundingBox mObb;

		private Vector3 mPosition;

		private Quaternion mOrientation;

		private readonly Word mWord;

		private TrackableBehaviour.Status mStatus;

		public override Word Word
		{
			get
			{
				return this.mWord;
			}
		}

		public override Vector3 Position
		{
			get
			{
				return this.mPosition;
			}
		}

		public override Quaternion Orientation
		{
			get
			{
				return this.mOrientation;
			}
		}

		public override OrientedBoundingBox Obb
		{
			get
			{
				return this.mObb;
			}
		}

		public override TrackableBehaviour.Status CurrentStatus
		{
			get
			{
				return this.mStatus;
			}
		}

		public WordResultImpl(Word word)
		{
			this.mWord = word;
		}

		public void SetPose(Vector3 position, Quaternion orientation)
		{
			this.mPosition = position;
			this.mOrientation = orientation;
		}

		public void SetObb(OrientedBoundingBox obb)
		{
			this.mObb = obb;
		}

		public void SetStatus(TrackableBehaviour.Status status)
		{
			this.mStatus = status;
		}
	}
}
