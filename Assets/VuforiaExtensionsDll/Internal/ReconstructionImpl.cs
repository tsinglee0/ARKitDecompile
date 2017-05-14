using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal abstract class ReconstructionImpl
	{
		protected IntPtr mNativeReconstructionPtr;

		private bool mMaximumAreaIsSet;

		private Rect mMaximumArea;

		private float mNavMeshPadding;

		private bool mNavMeshUpdatesEnabled;

		internal IntPtr NativePtr
		{
			get
			{
				return this.mNativeReconstructionPtr;
			}
		}

		public float NavMeshPadding
		{
			get
			{
				return this.mNavMeshPadding;
			}
		}

		protected ReconstructionImpl(IntPtr nativeReconstructionPtr)
		{
			this.mNativeReconstructionPtr = nativeReconstructionPtr;
		}

		public bool SetMaximumArea(Rect maximumArea)
		{
			RectangleData rectangleData;
			rectangleData.leftTopX = maximumArea.xMin;
			rectangleData.leftTopY = maximumArea.yMin;
			rectangleData.rightBottomX = maximumArea.xMax;
			rectangleData.rightBottomY = maximumArea.yMax;
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RectangleData)));
			Marshal.StructureToPtr(rectangleData, intPtr, false);
			bool arg_74_0 = VuforiaWrapper.Instance.ReconstructionSetMaximumArea(this.mNativeReconstructionPtr, intPtr) == 1;
			Marshal.FreeHGlobal(intPtr);
			if (arg_74_0)
			{
				this.mMaximumAreaIsSet = true;
				this.mMaximumArea = maximumArea;
			}
			return arg_74_0;
		}

		public bool GetMaximumArea(out Rect rect)
		{
			rect = this.mMaximumArea;
			return this.mMaximumAreaIsSet;
		}

		public bool Stop()
		{
			return VuforiaWrapper.Instance.ReconstructionStop(this.mNativeReconstructionPtr) == 1;
		}

		public virtual bool Start()
		{
			bool flag = false;
			SmartTerrainTracker tracker = TrackerManager.Instance.GetTracker<SmartTerrainTracker>();
			if (tracker != null && tracker.IsActive)
			{
				tracker.Stop();
				flag = true;
			}
			bool arg_3D_0 = VuforiaWrapper.Instance.ReconstructionStart(this.mNativeReconstructionPtr) == 1;
			if (flag)
			{
				tracker.Start();
			}
			return arg_3D_0;
		}

		public bool IsReconstructing()
		{
			return VuforiaWrapper.Instance.ReconstructionIsReconstructing(this.mNativeReconstructionPtr) == 1;
		}

		public void SetNavMeshPadding(float padding)
		{
			VuforiaWrapper.Instance.ReconstructionSetNavMeshPadding(this.mNativeReconstructionPtr, padding);
			this.mNavMeshPadding = padding;
		}

		public void StartNavMeshUpdates()
		{
			this.mNavMeshUpdatesEnabled = true;
		}

		public void StopNavMeshUpdates()
		{
			this.mNavMeshUpdatesEnabled = false;
		}

		public bool IsNavMeshUpdating()
		{
			return this.mNavMeshUpdatesEnabled;
		}

		public virtual bool Reset()
		{
			SmartTerrainTracker tracker = TrackerManager.Instance.GetTracker<SmartTerrainTracker>();
			if (tracker != null && tracker.IsActive)
			{
				Debug.LogError("Tracker should be stopped prior to calling Reset()");
				return false;
			}
			bool flag = VuforiaWrapper.Instance.ReconstructionReset(this.mNativeReconstructionPtr) == 1;
			if (flag)
			{
				SmartTerrainTracker tracker2 = TrackerManager.Instance.GetTracker<SmartTerrainTracker>();
				if (tracker2 != null)
				{
					foreach (ReconstructionAbstractBehaviour current in tracker2.SmartTerrainBuilder.GetReconstructions())
					{
						if (current.Reconstruction == this)
						{
							current.ClearOnReset();
						}
					}
				}
			}
			return flag;
		}
	}
}
