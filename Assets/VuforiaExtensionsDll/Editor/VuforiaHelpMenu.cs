using System;
using System.Diagnostics;
using UnityEditor;

namespace Vuforia.EditorClasses
{
	public class VuforiaHelpMenu : Editor
	{
		[MenuItem("Vuforia/Vuforia Documentation", false, 0)]
		public static void BrowseVuforiaHelp()
		{
			Process.Start("https://developer.vuforia.com/library/getting-started");
		}

		[MenuItem("Vuforia/Release Notes", false, 1)]
		public static void BrowseVuforiaReleaseNotes()
		{
			Process.Start("https://developer.vuforia.com/library/release-notes");
		}
	}
}
