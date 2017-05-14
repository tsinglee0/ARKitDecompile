using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class ReconstructionFromTargetImpl : ReconstructionImpl, ReconstructionFromTarget, Reconstruction
	{
		private Vector3 mOccluderMin = Vector3.zero;

		private Vector3 mOccluderMax = Vector3.zero;

		private Vector3 mOccluderOffset = Vector3.zero;

		private Quaternion mOccluderRotation = Quaternion.identity;

		private Trackable mInitializationTarget;

		private bool mCanAutoSetInitializationTarget = true;

		internal bool CanAutoSetInitializationTarget
		{
			get
			{
				return this.mCanAutoSetInitializationTarget;
			}
			set
			{
				this.mCanAutoSetInitializationTarget = value;
			}
		}

		public ReconstructionFromTargetImpl(IntPtr nativeReconstructionPtr) : base(nativeReconstructionPtr)
		{
		}

		public bool SetInitializationTarget(CylinderTarget cylinderTarget, Vector3 occluderMin, Vector3 occluderMax)
		{
			return this.SetInitializationTarget(cylinderTarget, occluderMin, occluderMax, Vector3.zero, Quaternion.identity);
		}

		public bool SetInitializationTarget(CylinderTarget cylinderTarget, Vector3 occluderMin, Vector3 occluderMax, Vector3 offsetToOccluderOrigin, Quaternion rotationToOccluderOrigin)
		{
			return this.SetInitializationTarget(((CylinderTargetImpl)cylinderTarget).DataSet.DataSetPtr, cylinderTarget, occluderMin, occluderMax, offsetToOccluderOrigin, rotationToOccluderOrigin);
		}

		public bool SetInitializationTarget(ImageTarget imageTarget, Vector3 occluderMin, Vector3 occluderMax)
		{
			return this.SetInitializationTarget(imageTarget, occluderMin, occluderMax, Vector3.zero, Quaternion.identity);
		}

		public bool SetInitializationTarget(ImageTarget imageTarget, Vector3 occluderMin, Vector3 occluderMax, Vector3 offsetToOccluderOrigin, Quaternion rotationToOccluderOrigin)
		{
			IntPtr datasetPtr = IntPtr.Zero;
			if (imageTarget is ImageTargetImpl)
			{
				datasetPtr = ((ImageTargetImpl)imageTarget).DataSet.DataSetPtr;
			}
			return this.SetInitializationTarget(datasetPtr, imageTarget, occluderMin, occluderMax, offsetToOccluderOrigin, rotationToOccluderOrigin);
		}

		public bool SetInitializationTarget(MultiTarget multiTarget, Vector3 occluderMin, Vector3 occluderMax)
		{
			return this.SetInitializationTarget(multiTarget, occluderMin, occluderMax, Vector3.zero, Quaternion.identity);
		}

		public bool SetInitializationTarget(MultiTarget multiTarget, Vector3 occluderMin, Vector3 occluderMax, Vector3 offsetToOccluderOrigin, Quaternion rotationToOccluderOrigin)
		{
			return this.SetInitializationTarget(((MultiTargetImpl)multiTarget).DataSet.DataSetPtr, multiTarget, occluderMin, occluderMax, offsetToOccluderOrigin, rotationToOccluderOrigin);
		}

		public Trackable GetInitializationTarget(out Vector3 occluderMin, out Vector3 occluderMax)
		{
			occluderMin = this.mOccluderMin;
			occluderMax = this.mOccluderMax;
			return this.mInitializationTarget;
		}

		public Trackable GetInitializationTarget(out Vector3 occluderMin, out Vector3 occluderMax, out Vector3 offsetToOccluderOrigin, out Quaternion rotationToOccluderOrigin)
		{
			offsetToOccluderOrigin = this.mOccluderOffset;
			rotationToOccluderOrigin = this.mOccluderRotation;
			return this.GetInitializationTarget(out occluderMin, out occluderMax);
		}

		public override bool Reset()
		{
			bool expr_06 = base.Reset();
			if (expr_06 && !base.IsReconstructing())
			{
				this.mCanAutoSetInitializationTarget = true;
			}
			return expr_06;
		}

		public override bool Start()
		{
			bool expr_06 = base.Start();
			if (expr_06)
			{
				this.mCanAutoSetInitializationTarget = false;
			}
			return expr_06;
		}

		private bool SetInitializationTarget(IntPtr datasetPtr, Trackable trackable, Vector3 occluderMin, Vector3 occluderMax, Vector3 offsetToOccluderOrigin, Quaternion rotationToOccluderOrigin)
		{
			this.mInitializationTarget = trackable;
			this.mOccluderMin = occluderMin;
			this.mOccluderMax = occluderMax;
			this.mOccluderOffset = offsetToOccluderOrigin;
			this.mOccluderRotation = rotationToOccluderOrigin;
			float num;
			Vector3 vector;
			rotationToOccluderOrigin.ToAngleAxis(out num, out vector);
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Vector3)));
			Marshal.StructureToPtr(occluderMin, intPtr, false);
			IntPtr intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Vector3)));
			Marshal.StructureToPtr(occluderMax, intPtr2, false);
			IntPtr intPtr3 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Vector3)));
			Marshal.StructureToPtr(offsetToOccluderOrigin, intPtr3, false);
			IntPtr intPtr4 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Vector3)));
			Marshal.StructureToPtr(vector, intPtr4, false);
			bool expr_E6 = VuforiaWrapper.Instance.ReconstructionFromTargetSetInitializationTarget(this.mNativeReconstructionPtr, datasetPtr, trackable.ID, intPtr, intPtr2, intPtr3, intPtr4, 360f - num) == 1;
			if (expr_E6)
			{
				Debug.Log(trackable.Name + " set as Smart Terrain initialization target.");
			}
			else
			{
				Debug.LogError(trackable.Name + " could not be set as Smart Terrain initialization target.");
			}
			Marshal.FreeHGlobal(intPtr);
			Marshal.FreeHGlobal(intPtr2);
			Marshal.FreeHGlobal(intPtr3);
			Marshal.FreeHGlobal(intPtr4);
			return expr_E6;
		}
	}
}
