using System;
using UnityEngine;

namespace Vuforia
{
	internal static class IOSCamRecoveringHelper
	{
		private const int CHECKED_FAILED_FRAME_MAX = 15;

		private const int WAITED_FRAME_RECOVER_MAX = 20;

		private const int RECOVER_ATTEMPT_MAX = 10;

		private static bool sHasJustResumed = false;

		private static bool sCheckFailedFrameAfterResume = false;

		private static int sCheckedFailedFrameCounter = 0;

		private static bool sWaitToRecoverCameraRestart = true;

		private static int sWaitedFrameRecoverCounter = 0;

		private static int sRecoveryAttemptCounter = 0;

		public static void SetHasJustResumed()
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				IOSCamRecoveringHelper.sHasJustResumed = true;
			}
		}

		public static bool TryToRecover()
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer && IOSCamRecoveringHelper.sHasJustResumed)
			{
				if (!IOSCamRecoveringHelper.sWaitToRecoverCameraRestart)
				{
					if (!IOSCamRecoveringHelper.sCheckFailedFrameAfterResume)
					{
						IOSCamRecoveringHelper.sCheckFailedFrameAfterResume = true;
						IOSCamRecoveringHelper.sCheckedFailedFrameCounter = 0;
					}
					else
					{
						IOSCamRecoveringHelper.sCheckedFailedFrameCounter++;
						if (IOSCamRecoveringHelper.sCheckedFailedFrameCounter > 15)
						{
							IOSCamRecoveringHelper.sRecoveryAttemptCounter++;
							if (IOSCamRecoveringHelper.sRecoveryAttemptCounter > 10)
							{
								IOSCamRecoveringHelper.sHasJustResumed = false;
								IOSCamRecoveringHelper.sCheckFailedFrameAfterResume = false;
								IOSCamRecoveringHelper.sCheckedFailedFrameCounter = 0;
								IOSCamRecoveringHelper.sWaitToRecoverCameraRestart = false;
								IOSCamRecoveringHelper.sWaitedFrameRecoverCounter = 0;
								IOSCamRecoveringHelper.sRecoveryAttemptCounter = 0;
								return false;
							}
							if (!CameraDevice.Instance.Stop())
							{
								return false;
							}
							if (!CameraDevice.Instance.Start())
							{
								return false;
							}
							IOSCamRecoveringHelper.sWaitToRecoverCameraRestart = true;
							IOSCamRecoveringHelper.sWaitedFrameRecoverCounter = 0;
							IOSCamRecoveringHelper.sCheckFailedFrameAfterResume = false;
							IOSCamRecoveringHelper.sCheckedFailedFrameCounter = 0;
						}
					}
				}
				else
				{
					IOSCamRecoveringHelper.sWaitedFrameRecoverCounter++;
					if (IOSCamRecoveringHelper.sWaitedFrameRecoverCounter > 20)
					{
						IOSCamRecoveringHelper.sWaitToRecoverCameraRestart = false;
						IOSCamRecoveringHelper.sWaitedFrameRecoverCounter = 0;
					}
				}
			}
			return true;
		}

		public static void SetSuccessfullyRecovered()
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer && IOSCamRecoveringHelper.sHasJustResumed && (IOSCamRecoveringHelper.sWaitToRecoverCameraRestart || IOSCamRecoveringHelper.sCheckFailedFrameAfterResume))
			{
				IOSCamRecoveringHelper.sHasJustResumed = false;
				IOSCamRecoveringHelper.sCheckFailedFrameAfterResume = false;
				IOSCamRecoveringHelper.sCheckedFailedFrameCounter = 0;
				IOSCamRecoveringHelper.sWaitToRecoverCameraRestart = false;
				IOSCamRecoveringHelper.sWaitedFrameRecoverCounter = 0;
				IOSCamRecoveringHelper.sRecoveryAttemptCounter = 0;
			}
		}
	}
}
