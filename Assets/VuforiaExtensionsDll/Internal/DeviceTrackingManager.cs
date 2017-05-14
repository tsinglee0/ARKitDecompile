using System;
using UnityEngine;

namespace Vuforia
{
	internal class DeviceTrackingManager
	{
		private Vector3 mDeviceTrackerPositonOffset = Vector3.zero;

		private Quaternion mDeviceTrackerRotationOffset = Quaternion.identity;

		private Action mBeforeDevicePoseUpdated;

		private Action mAfterDevicePoseUpdated;

		public void RecenterPose(Transform cameraTransform, Vector3 modelCorrectionTransform)
		{
			modelCorrectionTransform = cameraTransform.localRotation * modelCorrectionTransform;
			Vector3 vector = cameraTransform.localRotation * new Vector3(1f, 0f, 0f);
			float num = Mathf.Atan2(-vector.z, vector.x) * 57.29578f;
			cameraTransform.localRotation = Quaternion.AngleAxis(num, new Vector3(0f, 1f, 0f));
			this.mDeviceTrackerPositonOffset = cameraTransform.localPosition - modelCorrectionTransform;
			this.mDeviceTrackerRotationOffset = cameraTransform.localRotation;
		}

		public void UpdateCamera(Transform cameraTransform, VuforiaManagerImpl.TrackableResultData[] trackableResultDataArray, int deviceTrackableID)
		{
			if (deviceTrackableID >= 0)
			{
				int i = 0;
				while (i < trackableResultDataArray.Length)
				{
					VuforiaManagerImpl.TrackableResultData trackableResultData = trackableResultDataArray[i];
					if (trackableResultData.id == deviceTrackableID)
					{
						if (trackableResultData.status != TrackableBehaviour.Status.DETECTED && trackableResultData.status != TrackableBehaviour.Status.TRACKED && trackableResultData.status != TrackableBehaviour.Status.EXTENDED_TRACKED)
						{
							break;
						}
						if (this.mBeforeDevicePoseUpdated != null)
						{
							this.mBeforeDevicePoseUpdated.InvokeWithExceptionHandling();
						}
						this.PositionCamera(this.mDeviceTrackerPositonOffset, this.mDeviceTrackerRotationOffset, cameraTransform, trackableResultData.pose);
						if (this.mAfterDevicePoseUpdated != null)
						{
							this.mAfterDevicePoseUpdated.InvokeWithExceptionHandling();
							return;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
		}

		public void RegisterBeforeDevicePoseUpdateCallback(Action callback)
		{
			this.mBeforeDevicePoseUpdated = (Action)Delegate.Combine(this.mBeforeDevicePoseUpdated, callback);
		}

		public void UnregisterBeforeDevicePoseUpdateCallback(Action callback)
		{
			this.mBeforeDevicePoseUpdated = (Action)Delegate.Remove(this.mBeforeDevicePoseUpdated, callback);
		}

		public void RegisterDevicePoseUpdatedCallback(Action callback)
		{
			this.mAfterDevicePoseUpdated = (Action)Delegate.Combine(this.mAfterDevicePoseUpdated, callback);
		}

		public void UnregisterDevicePoseUpdatedCallback(Action callback)
		{
			this.mAfterDevicePoseUpdated = (Action)Delegate.Remove(this.mAfterDevicePoseUpdated, callback);
		}

		private void PositionCamera(Vector3 localRefPosition, Quaternion localRefRotation, Transform cameraTransform, VuforiaManagerImpl.PoseData camToTargetPose)
		{
			Quaternion localRotation = localRefRotation * camToTargetPose.orientation;
			Vector3 localPosition = localRefPosition + localRefRotation * camToTargetPose.position;
			cameraTransform.localPosition = localPosition;
			cameraTransform.localRotation = localRotation;
		}
	}
}
