using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class EyewearCalibrationProfileManagerImpl : EyewearCalibrationProfileManager
	{
		public override int getMaxCount()
		{
			return VuforiaWrapper.Instance.EyewearCPMGetMaxCount();
		}

		public override int getUsedCount()
		{
			return VuforiaWrapper.Instance.EyewearCPMGetUsedCount();
		}

		public override bool isProfileUsed(int profileID)
		{
			return VuforiaWrapper.Instance.EyewearCPMIsProfileUsed(profileID) == 1;
		}

		public override int getActiveProfile()
		{
			return VuforiaWrapper.Instance.EyewearCPMGetActiveProfile();
		}

		public override bool setActiveProfile(int profileID)
		{
			return VuforiaWrapper.Instance.EyewearCPMSetActiveProfile(profileID) == 1;
		}

		public override Matrix4x4 getCameraToEyePose(int profileID, EyewearDevice.EyeID eyeID)
		{
			float[] array = new float[16];
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * array.Length);
			VuforiaWrapper.Instance.EyewearCPMGetCameraToEyePose(profileID, (int)eyeID, intPtr);
			Marshal.Copy(intPtr, array, 0, array.Length);
			Matrix4x4 identity = Matrix4x4.identity;
			for (int i = 0; i < 16; i++)
			{
				identity[i] = array[i];
			}
			Marshal.FreeHGlobal(intPtr);
			return identity;
		}

		public override Matrix4x4 getEyeProjection(int profileID, EyewearDevice.EyeID eyeID)
		{
			float[] array = new float[16];
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * array.Length);
			VuforiaWrapper.Instance.EyewearCPMGetEyeProjection(profileID, (int)eyeID, intPtr);
			Marshal.Copy(intPtr, array, 0, array.Length);
			Matrix4x4 identity = Matrix4x4.identity;
			for (int i = 0; i < 16; i++)
			{
				identity[i] = array[i];
			}
			Marshal.FreeHGlobal(intPtr);
			return identity;
		}

		public override bool setCameraToEyePose(int profileID, EyewearDevice.EyeID eyeID, Matrix4x4 projectionMatrix)
		{
			float[] array = new float[16];
			for (int i = 0; i < 16; i++)
			{
				array[i] = projectionMatrix[i];
			}
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * array.Length);
			bool arg_4F_0 = VuforiaWrapper.Instance.EyewearCPMSetCameraToEyePose(profileID, (int)eyeID, intPtr) == 1;
			Marshal.FreeHGlobal(intPtr);
			return arg_4F_0;
		}

		public override bool setEyeProjection(int profileID, EyewearDevice.EyeID eyeID, Matrix4x4 projectionMatrix)
		{
			float[] array = new float[16];
			for (int i = 0; i < 16; i++)
			{
				array[i] = projectionMatrix[i];
			}
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * array.Length);
			bool arg_4F_0 = VuforiaWrapper.Instance.EyewearCPMSetEyeProjection(profileID, (int)eyeID, intPtr) == 1;
			Marshal.FreeHGlobal(intPtr);
			return arg_4F_0;
		}

		public override string getProfileName(int profileID)
		{
			return Marshal.PtrToStringUni(VuforiaWrapper.Instance.EyewearCPMGetProfileName(profileID));
		}

		public override bool setProfileName(int profileID, string name)
		{
			IntPtr name2 = Marshal.StringToHGlobalUni(name);
			return VuforiaWrapper.Instance.EyewearCPMSetProfileName(profileID, name2) == 1;
		}

		public override bool clearProfile(int profileID)
		{
			return VuforiaWrapper.Instance.EyewearCPMClearProfile(profileID) == 1;
		}
	}
}
