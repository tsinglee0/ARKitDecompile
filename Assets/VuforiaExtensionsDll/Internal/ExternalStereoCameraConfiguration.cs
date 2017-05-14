using System;
using UnityEngine;

namespace Vuforia
{
	internal class ExternalStereoCameraConfiguration : BaseStereoViewerCameraConfiguration, ICameraConfiguration
	{
		private const float TOLERANCE = 0.01f;

		private float mLastAppliedLeftNearClipPlane;

		private float mLastAppliedLeftFarClipPlane;

		private float mLastAppliedRightNearClipPlane;

		private float mLastAppliedRightFarClipPlane;

		private float mLastAppliedLeftVerticalVirtualFoV;

		private float mLastAppliedLeftHorizontalVirtualFoV;

		private float mLastAppliedRightVerticalVirtualFoV;

		private float mLastAppliedRightHorizontalVirtualFoV;

		private Matrix4x4 mLastAppliedLeftProjection;

		private Matrix4x4 mLastAppliedRightProjection;

		private float mNewLeftNearClipPlane;

		private float mNewLeftFarClipPlane;

		private float mNewRightNearClipPlane;

		private float mNewRightFarClipPlane;

		private float mNewLeftVerticalVirtualFoV;

		private float mNewLeftHorizontalVirtualFoV;

		private float mNewRightVerticalVirtualFoV;

		private float mNewRightHorizontalVirtualFoV;

		private Matrix4x4 mExternallySetLeftMatrix = Matrix4x4.identity;

		private Matrix4x4 mExternallySetRightMatrix = Matrix4x4.identity;

		public ExternalStereoCameraConfiguration(Camera leftCamera, Camera rightCamera) : base(leftCamera, rightCamera)
		{
			this.mNewLeftNearClipPlane = leftCamera.nearClipPlane;
			this.mNewLeftFarClipPlane = leftCamera.farClipPlane;
			this.mNewLeftVerticalVirtualFoV = leftCamera.fieldOfView;
			this.mNewLeftHorizontalVirtualFoV = CameraConfigurationUtility.CalculateHorizontalFoVFromViewPortAspect(leftCamera.fieldOfView, leftCamera.aspect);
			this.mNewRightNearClipPlane = rightCamera.nearClipPlane;
			this.mNewRightFarClipPlane = rightCamera.farClipPlane;
			this.mNewRightVerticalVirtualFoV = rightCamera.fieldOfView;
			this.mNewRightHorizontalVirtualFoV = CameraConfigurationUtility.CalculateHorizontalFoVFromViewPortAspect(rightCamera.fieldOfView, rightCamera.aspect);
		}

		public override bool CheckForSurfaceChanges(out ScreenOrientation orientation)
		{
			bool arg_84_0 = base.CheckForSurfaceChanges(out orientation);
			if (Math.Abs(this.mSecondaryCamera.fieldOfView - this.mPrimaryCamera.fieldOfView) > 0.01f)
			{
				Debug.LogWarning("Field of view values of left and right camera are not identical. This is currently not supported by Vuforia!");
			}
			if (Math.Abs(this.mSecondaryCamera.nearClipPlane - this.mPrimaryCamera.nearClipPlane) > 0.01f || Math.Abs(this.mSecondaryCamera.farClipPlane - this.mPrimaryCamera.farClipPlane) > 0.01f)
			{
				Debug.LogWarning("Clip plane positions of left and right camera are not identical. This is currently not supported by Vuforia!");
			}
			return arg_84_0;
		}

		public void ApplyCorrectedProjectionMatrix(Matrix4x4 projectionMatrix, bool primaryCamera)
		{
			Matrix4x4 inverse = projectionMatrix.inverse;
			if (primaryCamera)
			{
				this.mExternallySetLeftMatrix = projectionMatrix;
				CameraConfigurationUtility.ExtractCameraClippingPlanes(inverse, out this.mNewLeftNearClipPlane, out this.mNewLeftFarClipPlane);
				this.mNewLeftVerticalVirtualFoV = CameraConfigurationUtility.ExtractVerticalCameraFoV(inverse);
				this.mNewLeftHorizontalVirtualFoV = CameraConfigurationUtility.ExtractHorizontalCameraFoV(inverse);
			}
			else
			{
				this.mExternallySetRightMatrix = projectionMatrix;
				CameraConfigurationUtility.ExtractCameraClippingPlanes(inverse, out this.mNewRightNearClipPlane, out this.mNewRightFarClipPlane);
				this.mNewRightVerticalVirtualFoV = CameraConfigurationUtility.ExtractVerticalCameraFoV(inverse);
				this.mNewRightHorizontalVirtualFoV = CameraConfigurationUtility.ExtractHorizontalCameraFoV(inverse);
			}
			if (this.CameraParameterChanged())
			{
				ScreenOrientation screenOrientation;
				this.CheckForSurfaceChanges(out screenOrientation);
			}
			this.mPrimaryCamera.projectionMatrix = this.mLastAppliedLeftProjection;
			this.mSecondaryCamera.projectionMatrix = this.mLastAppliedRightProjection;
			if (base.IsVideoBackgroundEnabled())
			{
				this.mVideoBackgroundBehaviours[this.mPrimaryCamera].ApplyStereoDepthToMatrices();
				this.mVideoBackgroundBehaviours[this.mSecondaryCamera].ApplyStereoDepthToMatrices();
			}
		}

		protected override bool CameraParameterChanged()
		{
			return base.CameraParameterChanged() || Math.Abs(this.mNewLeftNearClipPlane - this.mLastAppliedLeftNearClipPlane) > 0.01f || Math.Abs(this.mNewLeftFarClipPlane - this.mLastAppliedLeftFarClipPlane) > 0.01f || Math.Abs(this.mNewLeftVerticalVirtualFoV - this.mLastAppliedLeftVerticalVirtualFoV) > 0.01f || Math.Abs(this.mNewRightHorizontalVirtualFoV - this.mLastAppliedLeftHorizontalVirtualFoV) > 0.01f || Math.Abs(this.mNewRightNearClipPlane - this.mLastAppliedRightNearClipPlane) > 0.01f || Math.Abs(this.mNewRightFarClipPlane - this.mLastAppliedRightFarClipPlane) > 0.01f || Math.Abs(this.mNewRightVerticalVirtualFoV - this.mLastAppliedRightVerticalVirtualFoV) > 0.01f || Math.Abs(this.mNewRightHorizontalVirtualFoV - this.mLastAppliedRightHorizontalVirtualFoV) > 0.01f;
		}

		protected override void UpdateProjection()
		{
			if (!VuforiaRuntimeUtilities.IsVuforiaEnabled())
			{
				return;
			}
			Device instance = Device.Instance;
			this.mLastAppliedLeftNearClipPlane = this.mNewLeftNearClipPlane;
			this.mLastAppliedLeftFarClipPlane = this.mNewLeftFarClipPlane;
			this.mLastAppliedLeftVerticalVirtualFoV = this.mNewLeftVerticalVirtualFoV;
			this.mLastAppliedLeftHorizontalVirtualFoV = this.mNewLeftHorizontalVirtualFoV;
			this.mLastAppliedRightNearClipPlane = this.mNewRightNearClipPlane;
			this.mLastAppliedRightFarClipPlane = this.mNewRightFarClipPlane;
			this.mLastAppliedRightVerticalVirtualFoV = this.mNewRightVerticalVirtualFoV;
			this.mLastAppliedRightHorizontalVirtualFoV = this.mNewRightHorizontalVirtualFoV;
			if (Device.Instance.GetMode() == Device.Mode.MODE_AR)
			{
				Matrix4x4 projectionMatrix = instance.GetProjectionMatrix(View.VIEW_LEFTEYE, this.mLastAppliedLeftNearClipPlane, this.mLastAppliedLeftFarClipPlane, this.mProjectionOrientation);
				Matrix4x4 projectionMatrix2 = instance.GetProjectionMatrix(View.VIEW_RIGHTEYE, this.mLastAppliedRightNearClipPlane, this.mLastAppliedRightFarClipPlane, this.mProjectionOrientation);
				this.mPrimaryCamera.projectionMatrix = CameraConfigurationUtility.ScalePerspectiveProjectionMatrix(projectionMatrix, this.mLastAppliedLeftVerticalVirtualFoV, this.mLastAppliedLeftHorizontalVirtualFoV);
				this.mSecondaryCamera.projectionMatrix = CameraConfigurationUtility.ScalePerspectiveProjectionMatrix(projectionMatrix2, this.mLastAppliedLeftVerticalVirtualFoV, this.mLastAppliedLeftHorizontalVirtualFoV);
				Vector2 skewingValues = new Vector2(this.mPrimaryCamera.projectionMatrix[0, 2], this.mPrimaryCamera.projectionMatrix[1, 2]);
				this.mVideoBackgroundBehaviours[this.mPrimaryCamera].SetVuforiaFrustumSkewValues(skewingValues, Vector2.zero);
				Vector2 skewingValues2 = new Vector2(this.mSecondaryCamera.projectionMatrix[0, 2], this.mSecondaryCamera.projectionMatrix[1, 2]);
				this.mVideoBackgroundBehaviours[this.mSecondaryCamera].SetVuforiaFrustumSkewValues(skewingValues2, Vector2.zero);
			}
			else
			{
				this.mPrimaryCamera.projectionMatrix = this.mExternallySetLeftMatrix;
				this.mSecondaryCamera.projectionMatrix = this.mExternallySetRightMatrix;
				this.mVideoBackgroundBehaviours[this.mPrimaryCamera].SetVuforiaFrustumSkewValues(Vector2.zero, Vector2.zero);
				this.mVideoBackgroundBehaviours[this.mSecondaryCamera].SetVuforiaFrustumSkewValues(Vector2.zero, Vector2.zero);
			}
			CameraConfigurationUtility.SetFovForCustomProjection(this.mPrimaryCamera);
			CameraConfigurationUtility.SetFovForCustomProjection(this.mSecondaryCamera);
			this.mLastAppliedLeftProjection = this.mPrimaryCamera.projectionMatrix;
			this.mLastAppliedRightProjection = this.mSecondaryCamera.projectionMatrix;
			int pixelWidthInt = this.mPrimaryCamera.GetPixelWidthInt();
			int pixelHeightInt = this.mPrimaryCamera.GetPixelHeightInt();
			base.ComputeViewPortRect(pixelHeightInt, pixelWidthInt);
		}
	}
}
