using System;
using System.Linq;
using UnityEngine;

namespace Vuforia
{
	public abstract class DataSetTrackableBehaviour : TrackableBehaviour, WorldCenterTrackableBehaviour, IEditDataSetBehaviour
	{
		[HideInInspector, SerializeField]
		protected string mDataSetPath = "";

		[HideInInspector, SerializeField]
		protected bool mExtendedTracking;

		[HideInInspector, SerializeField]
		protected bool mInitializeSmartTerrain;

		[HideInInspector, SerializeField]
		protected ReconstructionFromTargetAbstractBehaviour mReconstructionToInitialize;

		[HideInInspector, SerializeField]
		protected Vector3 mSmartTerrainOccluderBoundsMin;

		[HideInInspector, SerializeField]
		protected Vector3 mSmartTerrainOccluderBoundsMax;

		[HideInInspector, SerializeField]
		protected bool mIsSmartTerrainOccluderOffset;

		[HideInInspector, SerializeField]
		protected Vector3 mSmartTerrainOccluderOffset;

		[HideInInspector, SerializeField]
		protected Quaternion mSmartTerrainOccluderRotation;

		[HideInInspector, SerializeField]
		protected bool mAutoSetOccluderFromTargetSize;

		public string DataSetPath
		{
			get
			{
				return this.mDataSetPath;
			}
		}

		public string DataSetName
		{
			get
			{
				return DataSetTrackableBehaviour.GetDataSetName(this.mDataSetPath);
			}
		}

		bool IEditDataSetBehaviour.ExtendedTracking
		{
			get
			{
				return this.mExtendedTracking;
			}
			set
			{
				this.mExtendedTracking = value;
			}
		}

		bool IEditDataSetBehaviour.InitializeSmartTerrain
		{
			get
			{
				return this.mInitializeSmartTerrain;
			}
			set
			{
				this.mInitializeSmartTerrain = value;
			}
		}

		ReconstructionFromTargetAbstractBehaviour IEditDataSetBehaviour.ReconstructionToInitialize
		{
			get
			{
				return this.mReconstructionToInitialize;
			}
			set
			{
				this.mReconstructionToInitialize = value;
			}
		}

		Vector3 IEditDataSetBehaviour.SmartTerrainOccluderBoundsMin
		{
			get
			{
				return this.mSmartTerrainOccluderBoundsMin;
			}
			set
			{
				this.mSmartTerrainOccluderBoundsMin = value;
			}
		}

		Vector3 IEditDataSetBehaviour.SmartTerrainOccluderBoundsMax
		{
			get
			{
				return this.mSmartTerrainOccluderBoundsMax;
			}
			set
			{
				this.mSmartTerrainOccluderBoundsMax = value;
			}
		}

		protected virtual void OnDrawGizmos()
		{
			if (this.mInitializeSmartTerrain && this.mReconstructionToInitialize != null && !this.mAutoSetOccluderFromTargetSize)
			{
				if (this.mIsSmartTerrainOccluderOffset)
				{
					Vector3 vector = base.gameObject.transform.rotation * this.mSmartTerrainOccluderOffset;
					Gizmos.matrix = Matrix4x4.TRS(base.gameObject.transform.position + vector, base.gameObject.transform.rotation * this.mSmartTerrainOccluderRotation, Vector3.one);
				}
				else
				{
					Gizmos.matrix = Matrix4x4.TRS(base.gameObject.transform.position, base.gameObject.transform.rotation, Vector3.one);
				}
				Gizmos.color = Color.white;
				Vector3 vector2 = this.mSmartTerrainOccluderBoundsMax - this.mSmartTerrainOccluderBoundsMin;
				Gizmos.DrawWireCube((this.mSmartTerrainOccluderBoundsMin + this.mSmartTerrainOccluderBoundsMax) / 2f, vector2);
				float num = (vector2.x + vector2.y + vector2.z) / 2f;
				Gizmos.color = Color.gray;
				for (int i = 0; i <= 5; i++)
				{
					float num2 = -num + (float)i * num / 2.5f;
					Gizmos.DrawLine(new Vector3(-num, 0f, num2), new Vector3(num, 0f, num2));
					Gizmos.DrawLine(new Vector3(num2, 0f, -num), new Vector3(num2, 0f, num));
				}
				float num3 = num / 5f;
				Gizmos.color = Color.red;
				Gizmos.DrawLine(Vector3.zero, new Vector3(num3, 0f, 0f));
				Gizmos.color = Color.green;
				Gizmos.DrawLine(Vector3.zero, new Vector3(0f, num3, 0f));
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(Vector3.zero, new Vector3(0f, 0f, num3));
			}
		}

		protected virtual void OnValidate()
		{
			this.mSmartTerrainOccluderBoundsMin.y = 0f;
		}

		public override void OnTrackerUpdate(TrackableBehaviour.Status newStatus)
		{
			TrackableBehaviour.Status arg_06_0 = this.mStatus;
			bool flag = false;
			if (this.mInitializeSmartTerrain && this.mReconstructionToInitialize != null && (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED))
			{
				flag = true;
			}
			base.OnTrackerUpdate(newStatus);
			if (flag && this.mReconstructionToInitialize != null)
			{
				ReconstructionFromTargetImpl reconstructionFromTargetImpl = (ReconstructionFromTargetImpl)this.mReconstructionToInitialize.ReconstructionFromTarget;
				if (reconstructionFromTargetImpl != null && reconstructionFromTargetImpl.CanAutoSetInitializationTarget && this.mTrackable != null)
				{
					Vector3 vector;
					Vector3 vector2;
					if (reconstructionFromTargetImpl.GetInitializationTarget(out vector, out vector2) != this.mTrackable)
					{
						this.SetAsSmartTerrainInitializationTarget();
						return;
					}
					SmartTerrainTracker tracker = TrackerManager.Instance.GetTracker<SmartTerrainTracker>();
					if (tracker != null && this.mReconstructionToInitialize.ReconstructionBehaviour.AutomaticStart)
					{
						if (tracker.SmartTerrainBuilder.GetReconstructions().Contains(this.mReconstructionToInitialize.ReconstructionBehaviour))
						{
							reconstructionFromTargetImpl.Start();
							return;
						}
						tracker.SmartTerrainBuilder.AddReconstruction(this.mReconstructionToInitialize.ReconstructionBehaviour);
					}
				}
			}
		}

		public bool SetAsSmartTerrainInitializationTarget()
		{
			if (!this.mInitializeSmartTerrain || !(this.mReconstructionToInitialize != null))
			{
				Debug.LogError("DatasetTrackableBehaviour.SetAsSmartTerrainInitializationTarget: Target " + base.TrackableName + " was not configured as an intialization target.");
				return false;
			}
			if (this.mAutoSetOccluderFromTargetSize)
			{
				Vector3 vector;
				Vector3 vector2;
				this.CalculateDefaultOccluderBounds(out vector, out vector2);
				this.mSmartTerrainOccluderBoundsMin = vector;
				this.mSmartTerrainOccluderBoundsMax = vector2;
			}
			ReconstructionFromTarget reconstructionFromTarget = this.mReconstructionToInitialize.ReconstructionFromTarget;
			if (reconstructionFromTarget != null)
			{
				this.ProtectedSetAsSmartTerrainInitializationTarget(reconstructionFromTarget);
				this.mReconstructionToInitialize.Initialize();
				return true;
			}
			return false;
		}

		internal virtual bool InitializeTarget(Trackable trackable, bool applyTargetScaleToBehaviour)
		{
			VuforiaRuntimeUtilities.CleanTrackableFromUnwantedComponents(this);
			return true;
		}

		public void SetDefaultOccluderBounds()
		{
			Vector3 vector;
			Vector3 vector2;
			this.CalculateDefaultOccluderBounds(out vector, out vector2);
			Debug.Log("bounds max " + vector2);
			this.mSmartTerrainOccluderBoundsMin = vector;
			this.mSmartTerrainOccluderBoundsMax = vector2;
			this.mSmartTerrainOccluderOffset = Vector3.zero;
			this.mSmartTerrainOccluderRotation = Quaternion.identity;
		}

		public static string GetDataSetName(string datasetPath)
		{
			string text = VuforiaRuntimeUtilities.StripFileNameFromPath(datasetPath);
			int num = VuforiaRuntimeUtilities.StripExtensionFromPath(datasetPath).Length;
			if (num > 0)
			{
				num++;
				return text.Remove(text.Length - num);
			}
			return text;
		}

		protected abstract void CalculateDefaultOccluderBounds(out Vector3 boundsMin, out Vector3 boundsMax);

		protected abstract void ProtectedSetAsSmartTerrainInitializationTarget(ReconstructionFromTarget reconstructionFromTarget);
	}
}
