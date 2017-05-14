using System;
using System.IO;

namespace Vuforia.EditorClasses
{
	public class Unzipper
	{
		private class NullUnzipper : IUnzipper
		{
			public Stream UnzipFile(string path, string fileNameinZip)
			{
				return null;
			}
		}

		private static IUnzipper sInstance;

		public static IUnzipper Instance
		{
			get
			{
				if (Unzipper.sInstance == null)
				{
					Unzipper.sInstance = new Unzipper.NullUnzipper();
				}
				return Unzipper.sInstance;
			}
			set
			{
				Unzipper.sInstance = value;
			}
		}
	}
}
