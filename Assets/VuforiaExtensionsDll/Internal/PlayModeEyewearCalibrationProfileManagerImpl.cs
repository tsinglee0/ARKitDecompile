using System;
using UnityEngine;

namespace Vuforia
{
	internal class PlayModeEyewearCalibrationProfileManagerImpl : EyewearCalibrationProfileManager
	{
		public override int getMaxCount()
		{
			return 0;
		}

		public override int getUsedCount()
		{
			return 0;
		}

		public override bool isProfileUsed(int profileID)
		{
			return false;
		}

		public override int getActiveProfile()
		{
			return -1;
		}

		public override bool setActiveProfile(int profileID)
		{
			return false;
		}

		public override Matrix4x4 getCameraToEyePose(int profileID, EyewearDevice.EyeID eyeID)
		{
			return Matrix4x4.zero;
		}

		public override Matrix4x4 getEyeProjection(int profileID, EyewearDevice.EyeID eyeID)
		{
			return Matrix4x4.zero;
		}

		public override bool setCameraToEyePose(int profileID, EyewearDevice.EyeID eyeID, Matrix4x4 projectionMatrix)
		{
			return false;
		}

		public override bool setEyeProjection(int profileID, EyewearDevice.EyeID eyeID, Matrix4x4 projectionMatrix)
		{
			return false;
		}

		public override string getProfileName(int profileID)
		{
			return string.Empty;
		}

		public override bool setProfileName(int profileID, string name)
		{
			return false;
		}

		public override bool clearProfile(int profileID)
		{
			return false;
		}
	}
}
