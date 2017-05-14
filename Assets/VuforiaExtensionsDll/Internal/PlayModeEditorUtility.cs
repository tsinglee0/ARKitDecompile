using System;

namespace Vuforia
{
	public class PlayModeEditorUtility
	{
		private class NullPlayModeEditorUtility : IPlayModeEditorUtility
		{
			public void DisplayDialog(string title, string message, string ok)
			{
			}

			public WebCamProfile.ProfileCollection LoadAndParseWebcamProfiles(string path)
			{
				return default(WebCamProfile.ProfileCollection);
			}

			public void RestartPlayMode()
			{
			}

			public void ShowErrorInMouseOverWindow(string error)
			{
			}
		}

		private static IPlayModeEditorUtility sInstance;

		public static IPlayModeEditorUtility Instance
		{
			get
			{
				if (PlayModeEditorUtility.sInstance == null)
				{
					PlayModeEditorUtility.sInstance = new PlayModeEditorUtility.NullPlayModeEditorUtility();
				}
				return PlayModeEditorUtility.sInstance;
			}
			set
			{
				PlayModeEditorUtility.sInstance = value;
			}
		}
	}
}
