using System;

namespace Vuforia
{
	public abstract class VirtualButton
	{
		public enum Sensitivity
		{
			HIGH,
			MEDIUM,
			LOW
		}

		public const VirtualButton.Sensitivity DEFAULT_SENSITIVITY = VirtualButton.Sensitivity.LOW;

		public abstract string Name
		{
			get;
		}

		public abstract int ID
		{
			get;
		}

		public abstract bool Enabled
		{
			get;
		}

		public abstract RectangleData Area
		{
			get;
		}

		public abstract bool SetArea(RectangleData area);

		public abstract bool SetSensitivity(VirtualButton.Sensitivity sensitivity);

		public abstract bool SetEnabled(bool enabled);
	}
}
