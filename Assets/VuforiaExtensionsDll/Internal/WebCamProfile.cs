using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Vuforia
{
	public class WebCamProfile
	{
		public struct ProfileData
		{
			public VuforiaRenderer.Vec2I RequestedTextureSize;

			public VuforiaRenderer.Vec2I ResampledTextureSize;

			public int RequestedFPS;
		}

		public struct ProfileCollection
		{
			public readonly WebCamProfile.ProfileData DefaultProfile;

			public readonly Dictionary<string, WebCamProfile.ProfileData> Profiles;

			public ProfileCollection(WebCamProfile.ProfileData defaultProfile, Dictionary<string, WebCamProfile.ProfileData> profiles)
			{
				this.DefaultProfile = defaultProfile;
				this.Profiles = profiles;
			}
		}

		private readonly WebCamProfile.ProfileCollection mProfileCollection;

		public WebCamProfile.ProfileData Default
		{
			get
			{
				return this.mProfileCollection.DefaultProfile;
			}
		}

		public WebCamProfile()
		{
			this.mProfileCollection = PlayModeEditorUtility.Instance.LoadAndParseWebcamProfiles(Path.Combine(Application.dataPath, "Vuforia/Editor/WebcamProfiles/profiles.xml"));
		}

		internal WebCamProfile.ProfileData GetProfile(string webcamName)
		{
			WebCamProfile.ProfileData result;
			if (this.mProfileCollection.Profiles.TryGetValue(webcamName.ToLower(), out result))
			{
				return result;
			}
			return this.mProfileCollection.DefaultProfile;
		}

		public bool ProfileAvailable(string webcamName)
		{
			return this.mProfileCollection.Profiles.ContainsKey(webcamName.ToLower());
		}
	}
}
