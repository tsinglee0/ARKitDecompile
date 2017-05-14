using System;
using UnityEngine;

namespace Vuforia
{
	internal static class CameraConfigurationUtility
	{
		private static readonly Vector4 MIN_CENTER = new Vector4(0f, 0f, -1f, 1f);

		private static readonly Vector4 MAX_CENTER = new Vector4(0f, 0f, 1f, 1f);

		private static readonly Vector4 MAX_BOTTOM = new Vector4(0f, -1f, 1f, 1f);

		private static readonly Vector4 MAX_TOP = new Vector4(0f, 1f, 1f, 1f);

		private static readonly Vector4 MAX_LEFT = new Vector4(-1f, 0f, 1f, 1f);

		private static readonly Vector4 MAX_RIGHT = new Vector4(1f, 0f, 1f, 1f);

		public static void ExtractCameraClippingPlanes(Matrix4x4 inverseProjMatrix, out float near, out float far)
		{
			Vector3 vector = CameraConfigurationUtility.HomogenizedVec3(inverseProjMatrix * CameraConfigurationUtility.MIN_CENTER);
			Vector3 vector2 = CameraConfigurationUtility.HomogenizedVec3(inverseProjMatrix * CameraConfigurationUtility.MAX_CENTER);
			near = vector.z * -1f;
			far = vector2.z * -1f;
		}

		public static float ExtractVerticalCameraFoV(Matrix4x4 inverseProjMatrix)
		{
			Vector3 arg_22_0 = CameraConfigurationUtility.HomogenizedVec3(inverseProjMatrix * CameraConfigurationUtility.MAX_BOTTOM);
			Vector3 vector = CameraConfigurationUtility.HomogenizedVec3(inverseProjMatrix * CameraConfigurationUtility.MAX_TOP);
			return Vector3.Angle(arg_22_0, vector);
		}

		public static float ExtractHorizontalCameraFoV(Matrix4x4 inverseProjMatrix)
		{
			Vector3 arg_22_0 = CameraConfigurationUtility.HomogenizedVec3(inverseProjMatrix * CameraConfigurationUtility.MAX_LEFT);
			Vector3 vector = CameraConfigurationUtility.HomogenizedVec3(inverseProjMatrix * CameraConfigurationUtility.MAX_RIGHT);
			return Vector3.Angle(arg_22_0, vector);
		}

		public static Matrix4x4 ScalePerspectiveProjectionMatrix(Matrix4x4 inputMatrix, float targetVerticalFoVDeg, float targetHorizontalFoVDeg)
		{
			Matrix4x4 result = inputMatrix;
			float num = targetVerticalFoVDeg * 0.0174532924f;
			float num2 = targetHorizontalFoVDeg * 0.0174532924f;
			double arg_3C_0 = (double)(CameraConfigurationUtility.ExtractVerticalCameraFoV(inputMatrix.inverse) * 0.0174532924f);
			float num3 = CameraConfigurationUtility.ExtractHorizontalCameraFoV(inputMatrix.inverse) * 0.0174532924f;
			float num4 = (float)(Math.Tan(arg_3C_0 / (double)2f) / Math.Tan((double)(num / 2f)));
			float num5 = (float)(Math.Tan((double)(num3 / 2f)) / Math.Tan((double)(num2 / 2f)));
			result[0] = result[0] * num5;
			result[8] = result[8] * num5;
			result[5] = result[5] * num4;
			result[9] = result[9] * num4;
			return result;
		}

		public static float CalculateHorizontalFoVFromViewPortAspect(float verticalFoVDeg, float viewportAspect)
		{
			float num = verticalFoVDeg * 0.0174532924f;
			return 2f * Mathf.Atan(Mathf.Tan(num / 2f) * viewportAspect) * 57.29578f;
		}

		public static float CalculateVerticalFoVFromViewPortAspect(float horizontalFoVDeg, float viewportAspect)
		{
			float num = horizontalFoVDeg * 0.0174532924f;
			return 2f * Mathf.Atan(Mathf.Tan(num / 2f) / viewportAspect) * 57.29578f;
		}

		public static void SetFovForCustomProjection(Camera camera)
		{
			float fieldOfView = Mathf.Atan(1f / camera.projectionMatrix[5]) * 2f * 57.29578f;
			camera.fieldOfView = fieldOfView;
		}

		private static Vector3 HomogenizedVec3(Vector4 vec4)
		{
			return new Vector3(vec4.x / vec4.w, vec4.y / vec4.w, vec4.z / vec4.w);
		}
	}
}
