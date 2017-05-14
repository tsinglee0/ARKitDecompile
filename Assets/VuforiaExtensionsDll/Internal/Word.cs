using System;
using UnityEngine;

namespace Vuforia
{
	public interface Word : Trackable
	{
		string StringValue
		{
			get;
		}

		Vector2 Size
		{
			get;
		}

		Image GetLetterMask();

		RectangleData[] GetLetterBoundingBoxes();
	}
}
