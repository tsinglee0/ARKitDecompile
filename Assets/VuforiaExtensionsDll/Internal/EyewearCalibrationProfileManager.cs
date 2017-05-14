using System;
using UnityEngine;

namespace Vuforia
{
	public abstract class EyewearCalibrationProfileManager
	{
		public abstract int getMaxCount();

		public abstract int getUsedCount();

		public abstract bool isProfileUsed(int profileID);

		public abstract int getActiveProfile();

		public abstract bool setActiveProfile(int profileID);

		public abstract Matrix4x4 getCameraToEyePose(int profileID, EyewearDevice.EyeID eyeID);

		public abstract Matrix4x4 getEyeProjection(int profileID, EyewearDevice.EyeID eyeID);

		public abstract bool setCameraToEyePose(int profileID, EyewearDevice.EyeID eyeID, Matrix4x4 projectionMatrix);

		public abstract bool setEyeProjection(int profileID, EyewearDevice.EyeID eyeID, Matrix4x4 projectionMatrix);

		public abstract string getProfileName(int profileID);

		public abstract bool setProfileName(int profileID, string name);

		public abstract bool clearProfile(int profileID);
	}
}
