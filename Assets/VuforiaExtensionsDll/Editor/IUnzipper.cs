using System;
using System.IO;

namespace Vuforia.EditorClasses
{
	public interface IUnzipper
	{
		Stream UnzipFile(string path, string fileNameinZip);
	}
}
