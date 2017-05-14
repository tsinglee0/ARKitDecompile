using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class TextTrackerImpl : TextTracker
	{
		private enum UpDirection
		{
			TEXTTRACKER_UP_IS_0_HRS = 1,
			TEXTTRACKER_UP_IS_3_HRS,
			TEXTTRACKER_UP_IS_6_HRS,
			TEXTTRACKER_UP_IS_9_HRS
		}

		private readonly WordList mWordList = new WordListImpl();

		public override WordList WordList
		{
			get
			{
				return this.mWordList;
			}
		}

		private TextTrackerImpl.UpDirection CurrentUpDirection
		{
			get
			{
				TextTrackerImpl.UpDirection result;
				switch (VuforiaRuntimeUtilities.ScreenOrientation)
				{
				case ScreenOrientation.Portrait:
					result = TextTrackerImpl.UpDirection.TEXTTRACKER_UP_IS_9_HRS;
					return result;
				case ScreenOrientation.PortraitUpsideDown:
					result = TextTrackerImpl.UpDirection.TEXTTRACKER_UP_IS_3_HRS;
					return result;
				case ScreenOrientation.LandscapeRight:
					result = TextTrackerImpl.UpDirection.TEXTTRACKER_UP_IS_6_HRS;
					return result;
				}
				result = TextTrackerImpl.UpDirection.TEXTTRACKER_UP_IS_0_HRS;
				return result;
			}
		}

		public override bool Start()
		{
			if (VuforiaWrapper.Instance.TrackerStart((int)TypeMapping.GetTypeID(typeof(TextTracker))) != 1)
			{
				this.IsActive = false;
				Debug.LogError("Could not start tracker.");
				return false;
			}
			this.IsActive = true;
			return true;
		}

		public override void Stop()
		{
			VuforiaWrapper.Instance.TrackerStop((int)TypeMapping.GetTypeID(typeof(TextTracker)));
			this.IsActive = false;
			((WordManagerImpl)TrackerManager.Instance.GetStateManager().GetWordManager()).SetWordBehavioursToNotFound();
		}

		public override bool SetRegionOfInterest(Rect detectionRegion, Rect trackingRegion)
		{
			VuforiaARController expr_05 = VuforiaARController.Instance;
			Rect videoBackgroundRectInViewPort = expr_05.GetVideoBackgroundRectInViewPort();
			bool flag = expr_05.VideoBackGroundMirrored == VuforiaRenderer.VideoBackgroundReflection.ON;
			CameraDevice.VideoModeData videoMode = CameraDevice.Instance.GetVideoMode();
			Vector2 screenSpaceCoordinate;
			Vector2 screenSpaceCoordinate2;
			VuforiaRuntimeUtilities.SelectRectTopLeftAndBottomRightForLandscapeLeft(detectionRegion, flag, out screenSpaceCoordinate, out screenSpaceCoordinate2);
			Vector2 screenSpaceCoordinate3;
			Vector2 screenSpaceCoordinate4;
			VuforiaRuntimeUtilities.SelectRectTopLeftAndBottomRightForLandscapeLeft(trackingRegion, flag, out screenSpaceCoordinate3, out screenSpaceCoordinate4);
			VuforiaRenderer.Vec2I vec2I = VuforiaRuntimeUtilities.ScreenSpaceToCameraFrameCoordinates(screenSpaceCoordinate, videoBackgroundRectInViewPort, flag, videoMode);
			VuforiaRenderer.Vec2I vec2I2 = VuforiaRuntimeUtilities.ScreenSpaceToCameraFrameCoordinates(screenSpaceCoordinate2, videoBackgroundRectInViewPort, flag, videoMode);
			VuforiaRenderer.Vec2I vec2I3 = VuforiaRuntimeUtilities.ScreenSpaceToCameraFrameCoordinates(screenSpaceCoordinate3, videoBackgroundRectInViewPort, flag, videoMode);
			VuforiaRenderer.Vec2I vec2I4 = VuforiaRuntimeUtilities.ScreenSpaceToCameraFrameCoordinates(screenSpaceCoordinate4, videoBackgroundRectInViewPort, flag, videoMode);
			if (VuforiaWrapper.Instance.TextTrackerSetRegionOfInterest(vec2I.x, vec2I.y, vec2I2.x, vec2I2.y, vec2I3.x, vec2I3.y, vec2I4.x, vec2I4.y, (int)this.CurrentUpDirection) == 0)
			{
				Debug.LogError(string.Format("Could not set region of interest: ({0}, {1}, {2}, {3}) - ({4}, {5}, {6}, {7})", new object[]
				{
					detectionRegion.x,
					detectionRegion.y,
					detectionRegion.width,
					detectionRegion.height,
					trackingRegion.x,
					trackingRegion.y,
					trackingRegion.width,
					trackingRegion.height
				}));
				return false;
			}
			return true;
		}

		public override bool GetRegionOfInterest(out Rect detectionRegion, out Rect trackingRegion)
		{
			VuforiaARController expr_05 = VuforiaARController.Instance;
			Rect videoBackgroundRectInViewPort = expr_05.GetVideoBackgroundRectInViewPort();
			bool isTextureMirrored = expr_05.VideoBackGroundMirrored == VuforiaRenderer.VideoBackgroundReflection.ON;
			CameraDevice.VideoModeData videoMode = CameraDevice.Instance.GetVideoMode();
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RectangleIntData)));
			IntPtr intPtr2 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(RectangleIntData)));
			VuforiaWrapper.Instance.TextTrackerGetRegionOfInterest(intPtr, intPtr2);
			RectangleIntData camSpaceRectData = (RectangleIntData)Marshal.PtrToStructure(intPtr, typeof(RectangleIntData));
			RectangleIntData camSpaceRectData2 = (RectangleIntData)Marshal.PtrToStructure(intPtr2, typeof(RectangleIntData));
			Marshal.FreeHGlobal(intPtr);
			Marshal.FreeHGlobal(intPtr2);
			detectionRegion = this.ScreenSpaceRectFromCamSpaceRectData(camSpaceRectData, videoBackgroundRectInViewPort, isTextureMirrored, videoMode);
			trackingRegion = this.ScreenSpaceRectFromCamSpaceRectData(camSpaceRectData2, videoBackgroundRectInViewPort, isTextureMirrored, videoMode);
			return true;
		}

		private Rect ScreenSpaceRectFromCamSpaceRectData(RectangleIntData camSpaceRectData, Rect bgTextureViewPortRect, bool isTextureMirrored, CameraDevice.VideoModeData videoModeData)
		{
			Vector2 arg_3B_0 = VuforiaRuntimeUtilities.CameraFrameToScreenSpaceCoordinates(new Vector2((float)camSpaceRectData.leftTopX, (float)camSpaceRectData.leftTopY), bgTextureViewPortRect, isTextureMirrored, videoModeData);
			Vector2 bottomRight = VuforiaRuntimeUtilities.CameraFrameToScreenSpaceCoordinates(new Vector2((float)camSpaceRectData.rightBottomX, (float)camSpaceRectData.rightBottomY), bgTextureViewPortRect, isTextureMirrored, videoModeData);
			return VuforiaRuntimeUtilities.CalculateRectFromLandscapeLeftCorners(arg_3B_0, bottomRight, isTextureMirrored);
		}
	}
}
