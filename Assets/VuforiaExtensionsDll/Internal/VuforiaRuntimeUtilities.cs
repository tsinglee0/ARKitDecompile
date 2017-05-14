using System;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	public class VuforiaRuntimeUtilities
	{
		private enum InitializableBool
		{
			UNKNOWN,
			TRUE,
			FALSE
		}

		private static VuforiaRuntimeUtilities.InitializableBool sWebCamUsed;

		private static VuforiaRuntimeUtilities.InitializableBool sNativePluginSupport;

		public static ScreenOrientation ScreenOrientation
		{
			get
			{
				return SurfaceUtilities.GetSurfaceOrientation();
			}
		}

		public static bool IsLandscapeOrientation
		{
			get
			{
				ScreenOrientation screenOrientation = VuforiaRuntimeUtilities.ScreenOrientation;
				return screenOrientation == ScreenOrientation.Landscape || screenOrientation == ScreenOrientation.LandscapeLeft || screenOrientation == ScreenOrientation.LandscapeRight;
			}
		}

		public static bool IsPortraitOrientation
		{
			get
			{
				return !VuforiaRuntimeUtilities.IsLandscapeOrientation;
			}
		}

		public static string GetStoragePath(string path, VuforiaUnity.StorageType storageType)
		{
			string result = path;
			if (storageType == VuforiaUnity.StorageType.STORAGE_APPRESOURCE)
			{
				if (VuforiaRuntimeUtilities.IsWSARuntime())
				{
					result = "Data/StreamingAssets/" + path;
				}
				if (VuforiaRuntimeUtilities.IsPlayMode())
				{
					result = "Assets/StreamingAssets/" + path;
				}
			}
			return result;
		}

		public static string StripFileNameFromPath(string fullPath)
		{
			string[] expr_11 = fullPath.Split(new char[]
			{
				'/'
			});
			return expr_11[expr_11.Length - 1];
		}

		public static string StripStreamingAssetsFromPath(string fullPath)
		{
			string[] array = fullPath.Split(new char[]
			{
				'/'
			});
			string text = string.Empty;
			for (int i = 2; i < array.Length; i++)
			{
				text += array[i];
				if (i < array.Length - 1)
				{
					text += "/";
				}
			}
			return text;
		}

		public static string StripExtensionFromPath(string fullPath)
		{
			string[] array = fullPath.Split(new char[]
			{
				'.'
			});
			if (array.Length <= 1)
			{
				return "";
			}
			return array[array.Length - 1];
		}

		public static void ForceDisableTrackables()
		{
			TrackableBehaviour[] array = (TrackableBehaviour[])UnityEngine.Object.FindObjectsOfType(typeof(TrackableBehaviour));
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].enabled = false;
				}
			}
		}

		public static bool IsPlayMode()
		{
			return Application.isEditor;
		}

		public static bool IsWSARuntime()
		{
			return Application.platform == RuntimePlatform.WSAPlayerX86 || Application.platform == RuntimePlatform.WSAPlayerX64 || Application.platform == RuntimePlatform.WSAPlayerARM;
		}

		public static bool IsVuforiaEnabled()
		{
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				if (VuforiaRuntimeUtilities.sWebCamUsed == VuforiaRuntimeUtilities.InitializableBool.UNKNOWN)
				{
					if (VuforiaRuntimeUtilities.IsWebCamUsed())
					{
						VuforiaRuntimeUtilities.sWebCamUsed = VuforiaRuntimeUtilities.InitializableBool.TRUE;
					}
					else
					{
						VuforiaRuntimeUtilities.sWebCamUsed = VuforiaRuntimeUtilities.InitializableBool.FALSE;
					}
				}
				return VuforiaRuntimeUtilities.sWebCamUsed == VuforiaRuntimeUtilities.InitializableBool.TRUE;
			}
			return true;
		}

		public static bool IsWebCamUsed()
		{
			return !VuforiaAbstractConfiguration.Instance.WebCam.TurnOffWebCam && VuforiaRuntimeUtilities.CheckNativePluginSupport() && WebCamTexture.devices.Length != 0;
		}

		public static VuforiaRenderer.Vec2I ScreenSpaceToCameraFrameCoordinates(Vector2 screenSpaceCoordinate, Rect bgTextureViewPortRect, bool isTextureMirrored, CameraDevice.VideoModeData videoModeData)
		{
			float xMin = bgTextureViewPortRect.xMin;
			float yMin = bgTextureViewPortRect.yMin;
			float width = bgTextureViewPortRect.width;
			float height = bgTextureViewPortRect.height;
			bool flag = false;
			float num = (float)videoModeData.width;
			float num2 = (float)videoModeData.height;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			VuforiaRuntimeUtilities.PrepareCoordinateConversion(isTextureMirrored, ref num3, ref num4, ref num5, ref num6, ref flag);
			float num7 = (screenSpaceCoordinate.x - xMin) / width;
			float num8 = (screenSpaceCoordinate.y - yMin) / height;
			VuforiaRenderer.Vec2I result;
			if (flag)
			{
				result = new VuforiaRenderer.Vec2I(Mathf.RoundToInt((num3 + num5 * num8) * num), Mathf.RoundToInt((num4 + num6 * num7) * num2));
			}
			else
			{
				result = new VuforiaRenderer.Vec2I(Mathf.RoundToInt((num3 + num5 * num7) * num), Mathf.RoundToInt((num4 + num6 * num8) * num2));
			}
			return result;
		}

		public static Vector2 CameraFrameToScreenSpaceCoordinates(Vector2 cameraFrameCoordinate, Rect bgTextureViewPortRect, bool isTextureMirrored, CameraDevice.VideoModeData videoModeData)
		{
			float xMin = bgTextureViewPortRect.xMin;
			float yMin = bgTextureViewPortRect.yMin;
			float width = bgTextureViewPortRect.width;
			float height = bgTextureViewPortRect.height;
			bool flag = false;
			float num = (float)videoModeData.width;
			float num2 = (float)videoModeData.height;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			VuforiaRuntimeUtilities.PrepareCoordinateConversion(isTextureMirrored, ref num3, ref num4, ref num5, ref num6, ref flag);
			float num7 = (cameraFrameCoordinate.x / num - num3) / num5;
			float num8 = (cameraFrameCoordinate.y / num2 - num4) / num6;
			Vector2 result;
			if (flag)
			{
				result = new Vector2(width * num8 + xMin, height * num7 + yMin);
			}
			else
			{
				result = new Vector2(width * num7 + xMin, height * num8 + yMin);
			}
			return result;
		}

		public static OrientedBoundingBox CameraFrameToScreenSpaceCoordinates(OrientedBoundingBox cameraFrameObb, Rect bgTextureViewPortRect, bool isTextureMirrored, CameraDevice.VideoModeData videoModeData)
		{
			bool flag = false;
			float num = 0f;
			switch (VuforiaRuntimeUtilities.ScreenOrientation)
			{
			case ScreenOrientation.Portrait:
				num += 90f;
				flag = true;
				break;
			case ScreenOrientation.PortraitUpsideDown:
				num += 270f;
				flag = true;
				break;
			case ScreenOrientation.LandscapeRight:
				num += 180f;
				break;
			}
			float num2 = bgTextureViewPortRect.width / (float)(flag ? videoModeData.height : videoModeData.width);
			float num3 = bgTextureViewPortRect.height / (float)(flag ? videoModeData.width : videoModeData.height);
			Vector2 arg_D9_0 = VuforiaRuntimeUtilities.CameraFrameToScreenSpaceCoordinates(cameraFrameObb.Center, bgTextureViewPortRect, isTextureMirrored, videoModeData);
			Vector2 halfExtents = new Vector2(cameraFrameObb.HalfExtents.x * num2, cameraFrameObb.HalfExtents.y * num3);
			float num4 = cameraFrameObb.Rotation;
			if (isTextureMirrored)
			{
				num4 = -num4;
			}
			num4 = num4 * 180f / 3.14159274f + num;
			return new OrientedBoundingBox(arg_D9_0, halfExtents, num4);
		}

		public static void SelectRectTopLeftAndBottomRightForLandscapeLeft(Rect screenSpaceRect, bool isMirrored, out Vector2 topLeft, out Vector2 bottomRight)
		{
			if (!isMirrored)
			{
				switch (VuforiaRuntimeUtilities.ScreenOrientation)
				{
				case ScreenOrientation.Portrait:
					topLeft = new Vector2(screenSpaceRect.xMax, screenSpaceRect.yMin);
					bottomRight = new Vector2(screenSpaceRect.xMin, screenSpaceRect.yMax);
					return;
				case ScreenOrientation.PortraitUpsideDown:
					topLeft = new Vector2(screenSpaceRect.xMin, screenSpaceRect.yMax);
					bottomRight = new Vector2(screenSpaceRect.xMax, screenSpaceRect.yMin);
					return;
				case ScreenOrientation.LandscapeRight:
					topLeft = new Vector2(screenSpaceRect.xMax, screenSpaceRect.yMax);
					bottomRight = new Vector2(screenSpaceRect.xMin, screenSpaceRect.yMin);
					return;
				}
				topLeft = new Vector2(screenSpaceRect.xMin, screenSpaceRect.yMin);
				bottomRight = new Vector2(screenSpaceRect.xMax, screenSpaceRect.yMax);
				return;
			}
			switch (VuforiaRuntimeUtilities.ScreenOrientation)
			{
			case ScreenOrientation.Portrait:
				topLeft = new Vector2(screenSpaceRect.xMax, screenSpaceRect.yMax);
				bottomRight = new Vector2(screenSpaceRect.xMin, screenSpaceRect.yMin);
				return;
			case ScreenOrientation.PortraitUpsideDown:
				topLeft = new Vector2(screenSpaceRect.xMin, screenSpaceRect.yMin);
				bottomRight = new Vector2(screenSpaceRect.xMax, screenSpaceRect.yMax);
				return;
			case ScreenOrientation.LandscapeRight:
				topLeft = new Vector2(screenSpaceRect.xMin, screenSpaceRect.yMax);
				bottomRight = new Vector2(screenSpaceRect.xMax, screenSpaceRect.yMin);
				return;
			}
			topLeft = new Vector2(screenSpaceRect.xMax, screenSpaceRect.yMin);
			bottomRight = new Vector2(screenSpaceRect.xMin, screenSpaceRect.yMax);
		}

		public static Rect CalculateRectFromLandscapeLeftCorners(Vector2 topLeft, Vector2 bottomRight, bool isMirrored)
		{
			Rect result;
			if (!isMirrored)
			{
				switch (VuforiaRuntimeUtilities.ScreenOrientation)
				{
				case ScreenOrientation.Portrait:
					result = new Rect(bottomRight.x, topLeft.y, topLeft.x - bottomRight.x, bottomRight.y - topLeft.y);
					return result;
				case ScreenOrientation.PortraitUpsideDown:
					result = new Rect(topLeft.x, bottomRight.y, bottomRight.x - topLeft.x, topLeft.y - bottomRight.y);
					return result;
				case ScreenOrientation.LandscapeRight:
					result = new Rect(bottomRight.x, bottomRight.y, topLeft.x - bottomRight.x, topLeft.y - bottomRight.y);
					return result;
				}
				result = new Rect(topLeft.x, topLeft.y, bottomRight.x - topLeft.x, bottomRight.y - topLeft.y);
			}
			else
			{
				switch (VuforiaRuntimeUtilities.ScreenOrientation)
				{
				case ScreenOrientation.Portrait:
					result = new Rect(bottomRight.x, bottomRight.y, topLeft.x - bottomRight.x, topLeft.y - bottomRight.y);
					return result;
				case ScreenOrientation.PortraitUpsideDown:
					result = new Rect(topLeft.x, topLeft.y, bottomRight.x - topLeft.x, bottomRight.y - topLeft.y);
					return result;
				case ScreenOrientation.LandscapeRight:
					result = new Rect(topLeft.x, bottomRight.y, bottomRight.x - topLeft.x, topLeft.y - bottomRight.y);
					return result;
				}
				result = new Rect(bottomRight.x, topLeft.y, topLeft.x - bottomRight.x, bottomRight.y - topLeft.y);
			}
			return result;
		}

		public static void DisableSleepMode()
		{
			Screen.sleepTimeout = -1;
		}

		public static void ResetSleepMode()
		{
			Screen.sleepTimeout = -2;
		}

		public static bool MatrixIsNaN(Matrix4x4 matrix)
		{
			return float.IsNaN(matrix[0, 0]) || float.IsNaN(matrix[0, 1]) || float.IsNaN(matrix[0, 2]) || float.IsNaN(matrix[0, 3]) || float.IsNaN(matrix[1, 0]) || float.IsNaN(matrix[1, 1]) || float.IsNaN(matrix[1, 2]) || float.IsNaN(matrix[1, 3]) || float.IsNaN(matrix[2, 0]) || float.IsNaN(matrix[2, 1]) || float.IsNaN(matrix[2, 2]) || float.IsNaN(matrix[2, 3]) || float.IsNaN(matrix[3, 0]) || float.IsNaN(matrix[3, 1]) || float.IsNaN(matrix[3, 2]) || float.IsNaN(matrix[3, 3]);
		}

		public static bool CheckNativePluginSupport()
		{
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				int num = 0;
				try
				{
					num = VuforiaRuntimeUtilities.qcarCheckNativePluginSupport();
				}
				catch (Exception)
				{
					num = 0;
				}
				return num == 1;
			}
			return true;
		}

		internal static bool IsNativePluginSupportAvailable()
		{
			if (VuforiaRuntimeUtilities.IsPlayMode())
			{
				if (VuforiaRuntimeUtilities.sNativePluginSupport == VuforiaRuntimeUtilities.InitializableBool.UNKNOWN)
				{
					VuforiaRuntimeUtilities.sNativePluginSupport = (VuforiaRuntimeUtilities.CheckNativePluginSupport() ? VuforiaRuntimeUtilities.InitializableBool.TRUE : VuforiaRuntimeUtilities.InitializableBool.FALSE);
				}
				return VuforiaRuntimeUtilities.sNativePluginSupport == VuforiaRuntimeUtilities.InitializableBool.TRUE;
			}
			return true;
		}

		internal static bool StopCameraIfPossible(out bool objectTrackerWasStopped)
		{
			bool flag = false;
			bool flag2 = false;
			if (CameraDevice.Instance.IsActive())
			{
				SmartTerrainTracker tracker = TrackerManager.Instance.GetTracker<SmartTerrainTracker>();
				TextTracker tracker2 = TrackerManager.Instance.GetTracker<TextTracker>();
				if ((tracker == null || !tracker.IsActive) && (tracker2 == null || !tracker2.IsActive))
				{
					bool flag3 = false;
					ObjectTracker tracker3 = TrackerManager.Instance.GetTracker<ObjectTracker>();
					if (tracker3 != null && tracker3.IsActive)
					{
						if (tracker3.GetActiveDataSets().Any<DataSet>())
						{
							flag3 = true;
						}
						else
						{
							TargetFinder.InitState initState = tracker3.TargetFinder.GetInitState();
							if (initState == TargetFinder.InitState.INIT_RUNNING || initState == TargetFinder.InitState.INIT_SUCCESS)
							{
								flag3 = true;
							}
							else if (tracker3.TargetFinder.GetImageTargets().Any<ImageTarget>())
							{
								flag3 = true;
							}
							else if (tracker3.ImageTargetBuilder.IsScanning())
							{
								flag3 = true;
							}
							else
							{
								flag2 = true;
							}
						}
					}
					if (!flag3)
					{
						flag = true;
					}
				}
			}
			if (flag2)
			{
				TrackerManager.Instance.GetTracker<ObjectTracker>().Stop();
			}
			if (flag)
			{
				CameraDevice.Instance.Stop();
			}
			objectTrackerWasStopped = flag2;
			return flag;
		}

		internal static void CleanTrackableFromUnwantedComponents(TrackableBehaviour trackableBehaviour)
		{
			VuforiaUnity.GetHoloLensApiAbstraction().DeleteWorldAnchor(trackableBehaviour);
		}

		private static void PrepareCoordinateConversion(bool isTextureMirrored, ref float prefixX, ref float prefixY, ref float inversionMultiplierX, ref float inversionMultiplierY, ref bool isPortrait)
		{
			switch (VuforiaRuntimeUtilities.ScreenOrientation)
			{
			case ScreenOrientation.Portrait:
				isPortrait = true;
				if (!isTextureMirrored)
				{
					prefixX = 0f;
					prefixY = 1f;
					inversionMultiplierX = 1f;
					inversionMultiplierY = -1f;
					return;
				}
				prefixX = 1f;
				prefixY = 1f;
				inversionMultiplierX = -1f;
				inversionMultiplierY = -1f;
				return;
			case ScreenOrientation.PortraitUpsideDown:
				isPortrait = true;
				if (!isTextureMirrored)
				{
					prefixX = 1f;
					prefixY = 0f;
					inversionMultiplierX = -1f;
					inversionMultiplierY = 1f;
					return;
				}
				prefixX = 0f;
				prefixY = 0f;
				inversionMultiplierX = 1f;
				inversionMultiplierY = 1f;
				return;
			case ScreenOrientation.LandscapeRight:
				isPortrait = false;
				if (!isTextureMirrored)
				{
					prefixX = 1f;
					prefixY = 1f;
					inversionMultiplierX = -1f;
					inversionMultiplierY = -1f;
					return;
				}
				prefixX = 0f;
				prefixY = 1f;
				inversionMultiplierX = 1f;
				inversionMultiplierY = -1f;
				return;
			}
			isPortrait = false;
			if (!isTextureMirrored)
			{
				prefixX = 0f;
				prefixY = 0f;
				inversionMultiplierX = 1f;
				inversionMultiplierY = 1f;
				return;
			}
			prefixX = 1f;
			prefixY = 0f;
			inversionMultiplierX = -1f;
			inversionMultiplierY = 1f;
		}

		[DllImport("VuforiaWrapper")]
		private static extern int qcarCheckNativePluginSupport();
	}
}
