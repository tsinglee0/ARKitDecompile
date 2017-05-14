using System;

namespace Vuforia.EditorClasses
{
	internal class PremiumEditor
	{
		private class NullPremiumEditor : IPremiumEditor
		{
		}

		private static IPremiumEditor sInstance;

		internal static IPremiumEditor Instance
		{
			get
			{
				if (PremiumEditor.sInstance == null)
				{
					PremiumEditor.sInstance = new PremiumEditor.NullPremiumEditor();
				}
				return PremiumEditor.sInstance;
			}
			set
			{
				PremiumEditor.sInstance = value;
			}
		}
	}
}
