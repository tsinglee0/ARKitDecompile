using System;
using System.Collections.Generic;

namespace Vuforia
{
	public interface ImageTarget : ObjectTarget, ExtendedTrackable, Trackable
	{
		ImageTargetType ImageTargetType
		{
			get;
		}

		VirtualButton CreateVirtualButton(string name, RectangleData area);

		VirtualButton GetVirtualButtonByName(string name);

		IEnumerable<VirtualButton> GetVirtualButtons();

		bool DestroyVirtualButton(VirtualButton vb);
	}
}
