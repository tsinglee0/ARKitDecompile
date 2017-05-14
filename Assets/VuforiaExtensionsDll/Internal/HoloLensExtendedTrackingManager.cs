using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Vuforia
{
	internal class HoloLensExtendedTrackingManager : IExtendedTrackingManager
	{
		private struct PoseInfo
		{
			public Vector3 Position;

			public Quaternion Rotation;

			public int NumFramesPoseWasOff;
		}

		private struct PoseAgeEntry
		{
			public HoloLensExtendedTrackingManager.PoseInfo Pose;

			public HoloLensExtendedTrackingManager.PoseInfo CameraPose;

			public int Age;
		}

		private int mNumFramesStablePose = 6;

		private float mMaxPoseRelDistance = 0.065f;

		private float mMaxPoseAngleDiff = 5f;

		private float mMaxCamPoseAbsDistance = 0.05f;

		private float mMaxCamPoseAngleDiff = 5f;

		private int mMinNumFramesPoseOff = 7;

		private float mMinPoseUpdateRelDistance = 0.12f;

		private float mMinPoseUpdateAngleDiff = 10f;

		private float mTrackableSizeInViewThreshold = 0.33f;

		private float mMaxDistanceFromViewCenterForPoseUpdate = 0.66f;

		private bool mSetWorldAnchors = true;

		private Dictionary<VuforiaManager.TrackableIdPair, HoloLensExtendedTrackingManager.PoseAgeEntry> mTrackingList = new Dictionary<VuforiaManager.TrackableIdPair, HoloLensExtendedTrackingManager.PoseAgeEntry>();

		private HashSet<int> mTrackablesExtendedTrackingEnabled = new HashSet<int>();

		private Dictionary<VuforiaManager.TrackableIdPair, HoloLensExtendedTrackingManager.PoseInfo> mTrackablesCurrentlyExtendedTracked = new Dictionary<VuforiaManager.TrackableIdPair, HoloLensExtendedTrackingManager.PoseInfo>();

		private Dictionary<VuforiaManager.TrackableIdPair, TrackableBehaviour.Status> mExtendedTrackablesState = new Dictionary<VuforiaManager.TrackableIdPair, TrackableBehaviour.Status>();

		public void ApplyTrackingState(TrackableBehaviour trackableBehaviour, TrackableBehaviour.Status vuforiaStatus, Transform cameraTransform)
		{
			ExtendedTrackable extendedTrackable = trackableBehaviour.Trackable as ExtendedTrackable;
			if (extendedTrackable == null)
			{
				return;
			}
			float expr_16 = extendedTrackable.GetLargestSizeComponent();
			float maxPoseDistance = expr_16 * this.mMaxPoseRelDistance;
			float minPoseUpdateDistance = expr_16 * this.mMinPoseUpdateRelDistance;
			int iD = trackableBehaviour.Trackable.ID;
			VuforiaManager.TrackableIdPair trackableIdPair;
			if (trackableBehaviour is VuMarkAbstractBehaviour)
			{
				trackableIdPair = VuforiaManager.TrackableIdPair.FromTrackableAndResultId(iD, ((VuMarkAbstractBehaviour)trackableBehaviour).VuMarkResultId);
			}
			else
			{
				trackableIdPair = VuforiaManager.TrackableIdPair.FromTrackableId(iD);
			}
			if (vuforiaStatus == TrackableBehaviour.Status.TRACKED && this.mTrackablesExtendedTrackingEnabled.Contains(iD))
			{
				bool flag = true;
				if (this.mTrackingList.ContainsKey(trackableIdPair))
				{
					HoloLensExtendedTrackingManager.PoseAgeEntry poseAgeEntry = this.mTrackingList[trackableIdPair];
					float arg_D3_0 = (poseAgeEntry.CameraPose.Position - cameraTransform.position).magnitude;
					float num = Quaternion.Angle(poseAgeEntry.CameraPose.Rotation, cameraTransform.rotation);
					if (arg_D3_0 <= this.mMaxCamPoseAbsDistance && num <= this.mMaxCamPoseAngleDiff)
					{
						if (!this.mTrackablesCurrentlyExtendedTracked.ContainsKey(trackableIdPair))
						{
							this.CheckHandoverToHoloLens(poseAgeEntry, trackableBehaviour, trackableIdPair, maxPoseDistance, false, out flag);
						}
						else
						{
							this.CheckForHoloLensPoseUpdates(poseAgeEntry, trackableBehaviour, trackableIdPair, iD, minPoseUpdateDistance, maxPoseDistance, cameraTransform, out flag);
						}
					}
				}
				if (flag)
				{
					this.mTrackingList[trackableIdPair] = new HoloLensExtendedTrackingManager.PoseAgeEntry
					{
						Pose = new HoloLensExtendedTrackingManager.PoseInfo
						{
							Position = trackableBehaviour.transform.position,
							Rotation = trackableBehaviour.transform.rotation
						},
						CameraPose = new HoloLensExtendedTrackingManager.PoseInfo
						{
							Position = cameraTransform.position,
							Rotation = cameraTransform.rotation
						},
						Age = 1
					};
				}
			}
			if (this.mTrackablesCurrentlyExtendedTracked.ContainsKey(trackableIdPair))
			{
				if (!this.mSetWorldAnchors || VuforiaRuntimeUtilities.IsPlayMode())
				{
					HoloLensExtendedTrackingManager.PoseInfo poseInfo = this.mTrackablesCurrentlyExtendedTracked[trackableIdPair];
					trackableBehaviour.transform.position = poseInfo.Position;
					trackableBehaviour.transform.rotation = poseInfo.Rotation;
				}
				vuforiaStatus = this.mExtendedTrackablesState[trackableIdPair];
			}
			trackableBehaviour.OnTrackerUpdate(vuforiaStatus);
		}

		public bool StartExtendedTracking(IntPtr datasetPtr, int trackableID)
		{
			VuforiaUnity.GetHoloLensApiAbstraction().SetSpatialAnchorTrackingCallback(new Action<VuforiaManager.TrackableIdPair, bool>(this.OnSpatialAnchorTrackingCallback));
			this.mTrackablesExtendedTrackingEnabled.Add(trackableID);
			return true;
		}

		public bool StopExtendedTracking(IntPtr datasetPtr, int trackableID)
		{
			this.ResetExtendedTrackingForTrackable(trackableID);
			if (this.mTrackablesExtendedTrackingEnabled.Contains(trackableID))
			{
				this.mTrackablesExtendedTrackingEnabled.Remove(trackableID);
			}
			return true;
		}

		public bool PersistExtendedTracking(bool on)
		{
			return true;
		}

		public bool ResetExtendedTracking(bool trackerActive)
		{
			this.mTrackingList.Clear();
			IHoloLensApiAbstraction holoLensApiAbstraction = VuforiaUnity.GetHoloLensApiAbstraction();
			foreach (VuforiaManager.TrackableIdPair current in this.mTrackablesCurrentlyExtendedTracked.Keys)
			{
				holoLensApiAbstraction.DeleteWorldAnchor(current);
			}
			this.mTrackablesCurrentlyExtendedTracked.Clear();
			this.mExtendedTrackablesState.Clear();
			return true;
		}

		public IEnumerable<VuforiaManager.TrackableIdPair> GetExtendedTrackedBehaviours()
		{
			return this.mTrackablesCurrentlyExtendedTracked.Keys;
		}

		public void EnableWorldAnchorUsage(bool enable)
		{
			this.mSetWorldAnchors = enable;
		}

		public void GetExtendedTrackingParameters(out int numFramesStablePose, out float maxPoseRelDistance, out float maxPoseAngleDiff, out int minNumFramesPoseOff, out float minPoseUpdateRelDistance, out float minPoseUpdateAngleDiff)
		{
			numFramesStablePose = this.mNumFramesStablePose;
			maxPoseRelDistance = this.mMaxPoseRelDistance;
			maxPoseAngleDiff = this.mMaxPoseAngleDiff;
			minNumFramesPoseOff = this.mMinNumFramesPoseOff;
			minPoseUpdateRelDistance = this.mMinPoseUpdateRelDistance;
			minPoseUpdateAngleDiff = this.mMinPoseUpdateAngleDiff;
		}

		public void SetExtendedTrackingParameters(int numFramesStablePose, float maxPoseRelDistance, float maxPoseAngleDiff, int minNumFramesPoseOff, float minPoseUpdateRelDistance, float minPoseUpdateAngleDiff)
		{
			this.mNumFramesStablePose = numFramesStablePose;
			this.mMaxPoseRelDistance = maxPoseRelDistance;
			this.mMaxPoseAngleDiff = maxPoseAngleDiff;
			this.mMinNumFramesPoseOff = minNumFramesPoseOff;
			this.mMinPoseUpdateRelDistance = minPoseUpdateRelDistance;
			this.mMinPoseUpdateAngleDiff = minPoseUpdateAngleDiff;
		}

		private void CheckHandoverToHoloLens(HoloLensExtendedTrackingManager.PoseAgeEntry poseEntry, TrackableBehaviour trackableBehaviour, VuforiaManager.TrackableIdPair trackableResultID, float maxPoseDistance, bool isPoseUpdate, out bool resetPoseInfo)
		{
			IHoloLensApiAbstraction holoLensApiAbstraction = VuforiaUnity.GetHoloLensApiAbstraction();
			resetPoseInfo = true;
			float arg_4B_0 = (poseEntry.Pose.Position - trackableBehaviour.transform.position).magnitude;
			float num = Quaternion.Angle(poseEntry.Pose.Rotation, trackableBehaviour.transform.rotation);
			if (arg_4B_0 <= maxPoseDistance && num <= this.mMaxPoseAngleDiff)
			{
				poseEntry.Age++;
				this.mTrackingList[trackableResultID] = poseEntry;
				resetPoseInfo = false;
				if (poseEntry.Age > this.mNumFramesStablePose)
				{
					HoloLensExtendedTrackingManager.PoseInfo poseInfo = new HoloLensExtendedTrackingManager.PoseInfo
					{
						Position = trackableBehaviour.transform.position,
						Rotation = trackableBehaviour.transform.rotation,
						NumFramesPoseWasOff = 0
					};
					this.mTrackablesCurrentlyExtendedTracked[trackableResultID] = poseInfo;
					if (isPoseUpdate)
					{
						holoLensApiAbstraction.DeleteWorldAnchor(trackableResultID);
						trackableBehaviour.transform.position = poseInfo.Position;
						trackableBehaviour.transform.rotation = poseInfo.Rotation;
					}
					if (this.mSetWorldAnchors)
					{
						holoLensApiAbstraction.SetWorldAnchor(trackableBehaviour, trackableResultID);
					}
					this.mExtendedTrackablesState[trackableResultID] = TrackableBehaviour.Status.EXTENDED_TRACKED;
				}
			}
		}

		private void CheckForHoloLensPoseUpdates(HoloLensExtendedTrackingManager.PoseAgeEntry poseEntry, TrackableBehaviour trackableBehaviour, VuforiaManager.TrackableIdPair trackableResultID, int trackableID, float minPoseUpdateDistance, float maxPoseDistance, Transform cameraTransform, out bool resetPoseInfo)
		{
			resetPoseInfo = true;
			if (this.IsTargetWellInView(trackableBehaviour, cameraTransform))
			{
				HoloLensExtendedTrackingManager.PoseInfo poseInfo = this.mTrackablesCurrentlyExtendedTracked[trackableResultID];
				float magnitude = (poseInfo.Position - trackableBehaviour.transform.position).magnitude;
				float num = Quaternion.Angle(poseInfo.Rotation, trackableBehaviour.transform.rotation);
				if (this.CalculateTargetSizeInCameraViewCoeff(trackableBehaviour, cameraTransform.position) <= this.mTrackableSizeInViewThreshold)
				{
					poseInfo.NumFramesPoseWasOff = 0;
					this.mTrackablesCurrentlyExtendedTracked[trackableResultID] = poseInfo;
					if (magnitude > minPoseUpdateDistance || num > this.mMinPoseUpdateAngleDiff)
					{
						this.CheckHandoverToHoloLens(poseEntry, trackableBehaviour, trackableResultID, maxPoseDistance, true, out resetPoseInfo);
						return;
					}
				}
				else
				{
					if (magnitude > minPoseUpdateDistance || num > this.mMinPoseUpdateAngleDiff)
					{
						poseInfo.NumFramesPoseWasOff++;
					}
					else
					{
						poseInfo.NumFramesPoseWasOff = 0;
					}
					this.mTrackablesCurrentlyExtendedTracked[trackableResultID] = poseInfo;
					if (poseInfo.NumFramesPoseWasOff > this.mMinNumFramesPoseOff)
					{
						this.ResetExtendedTrackingForTrackable(trackableID);
					}
				}
			}
		}

		private bool IsTargetWellInView(TrackableBehaviour trackableBehaviour, Transform cameraTransform)
		{
			Vector3 expr_11 = cameraTransform.InverseTransformPoint(trackableBehaviour.transform.position);
			float num = Mathf.Abs(expr_11.z);
			float num2 = Mathf.Abs(expr_11.x);
			float num3 = Mathf.Abs(expr_11.y);
			Vector2 expr_48 = CameraDevice.Instance.GetCameraFieldOfViewRads() / 2f;
			float num4 = Mathf.Tan(expr_48.x) * num;
			float num5 = Mathf.Tan(expr_48.y) * num;
			bool flag = num2 <= num4 * this.mMaxDistanceFromViewCenterForPoseUpdate && num3 <= num5 * this.mMaxDistanceFromViewCenterForPoseUpdate;
			if (!flag)
			{
				float num6 = (trackableBehaviour.Trackable as ExtendedTrackable).GetLargestSizeComponent() / 2f;
				flag = (num2 + num6 <= num4 && num3 + num6 <= num5);
			}
			return flag;
		}

		private float CalculateTargetSizeInCameraViewCoeff(TrackableBehaviour trackableBehaviour, Vector3 cameraPosition)
		{
			float magnitude = (trackableBehaviour.transform.position - cameraPosition).magnitude;
			float num = Mathf.Tan(CameraDevice.Instance.GetCameraFieldOfViewRads().x / 2f) * magnitude;
			return (trackableBehaviour.Trackable as ExtendedTrackable).GetLargestSizeComponent() / num;
		}

		private void OnSpatialAnchorTrackingCallback(VuforiaManager.TrackableIdPair trackableResultID, bool tracked)
		{
			if (tracked)
			{
				this.mExtendedTrackablesState[trackableResultID] = TrackableBehaviour.Status.EXTENDED_TRACKED;
				return;
			}
			this.mExtendedTrackablesState[trackableResultID] = TrackableBehaviour.Status.NOT_FOUND;
		}

		private void ResetExtendedTrackingForTrackable(int trackableID)
		{
			IHoloLensApiAbstraction holoLensApiAbstraction = VuforiaUnity.GetHoloLensApiAbstraction();
			VuforiaManager.TrackableIdPair[] array = this.mTrackablesCurrentlyExtendedTracked.Keys.ToArray<VuforiaManager.TrackableIdPair>();
			for (int i = 0; i < array.Length; i++)
			{
				VuforiaManager.TrackableIdPair trackableIdPair = array[i];
				if (trackableIdPair.TrackableId == trackableID)
				{
					this.mTrackablesCurrentlyExtendedTracked.Remove(trackableIdPair);
					holoLensApiAbstraction.DeleteWorldAnchor(trackableIdPair);
					if (this.mExtendedTrackablesState.ContainsKey(trackableIdPair))
					{
						this.mExtendedTrackablesState.Remove(trackableIdPair);
					}
				}
			}
			array = this.mTrackingList.Keys.ToArray<VuforiaManager.TrackableIdPair>();
			for (int i = 0; i < array.Length; i++)
			{
				VuforiaManager.TrackableIdPair trackableIdPair2 = array[i];
				if (trackableIdPair2.TrackableId == trackableID)
				{
					this.mTrackingList.Remove(trackableIdPair2);
				}
			}
		}
	}
}
