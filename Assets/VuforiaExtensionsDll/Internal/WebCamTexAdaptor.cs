using System;
using UnityEngine;

namespace Vuforia
{
	internal abstract class WebCamTexAdaptor
	{
		public abstract bool DidUpdateThisFrame
		{
			get;
		}

		public abstract bool IsPlaying
		{
			get;
		}

		public abstract Texture Texture
		{
			get;
		}

		public abstract void Play();

		public abstract void Stop();

		public abstract void CheckPermissions();
	}
}
