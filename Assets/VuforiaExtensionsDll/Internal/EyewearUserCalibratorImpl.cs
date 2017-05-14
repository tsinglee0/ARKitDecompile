using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class EyewearUserCalibratorImpl : EyewearUserCalibrator
	{
		public override bool init(int surfaceWidth, int surfaceHeight, int targetWidth, int targetHeight)
		{
			return VuforiaWrapper.Instance.EyewearUserCalibratorInit(surfaceWidth, surfaceHeight, targetWidth, targetHeight) == 1;
		}

		public override float getMinScaleHint()
		{
			return VuforiaWrapper.Instance.EyewearUserCalibratorGetMinScaleHint();
		}

		public override float getMaxScaleHint()
		{
			return VuforiaWrapper.Instance.EyewearUserCalibratorGetMaxScaleHint();
		}

		public override bool isStereoStretched()
		{
			return VuforiaWrapper.Instance.EyewearUserCalibratorIsStereoStretched() == 1;
		}

		public override bool getProjectionMatrix(EyewearDevice.EyewearCalibrationReading[] readings, out Matrix4x4 cameraToEyeMatrix, out Matrix4x4 projectionMatrix)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(EyewearDevice.EyewearCalibrationReading)) * readings.Length);
			long num = intPtr.ToInt64();
			for (int i = 0; i < readings.Length; i++)
			{
				IntPtr ptr = new IntPtr(num);
				Marshal.StructureToPtr(readings[i], ptr, false);
				num += (long)Marshal.SizeOf(typeof(EyewearDevice.EyewearCalibrationReading));
			}
			float[] array = new float[16];
			IntPtr intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * array.Length);
			float[] array2 = new float[16];
			IntPtr intPtr3 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * array2.Length);
			bool flag = VuforiaWrapper.Instance.EyewearUserCalibratorGetProjectionMatrix(intPtr, readings.Length, intPtr3, intPtr2) == 1;
			projectionMatrix = Matrix4x4.zero;
			cameraToEyeMatrix = Matrix4x4.zero;
			if (flag)
			{
				Marshal.Copy(intPtr2, array, 0, array.Length);
				for (int j = 0; j < 16; j++)
				{
					projectionMatrix[j] = array[j];
				}
				Marshal.Copy(intPtr3, array2, 0, array2.Length);
				for (int k = 0; k < 16; k++)
				{
					cameraToEyeMatrix[k] = array2[k];
				}
			}
			Marshal.FreeHGlobal(intPtr2);
			Marshal.FreeHGlobal(intPtr3);
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}
	}
}
