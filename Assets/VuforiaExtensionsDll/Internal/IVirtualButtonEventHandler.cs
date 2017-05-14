using System;

namespace Vuforia
{
	public interface IVirtualButtonEventHandler
	{
		void OnButtonPressed(VirtualButtonAbstractBehaviour vb);

		void OnButtonReleased(VirtualButtonAbstractBehaviour vb);
	}
}
