using System;

namespace Vuforia.EditorClasses
{
	internal abstract class TrackableAccessor
	{
		protected TrackableBehaviour mTarget;

		public abstract void ApplyDataSetProperties();

		public abstract void ApplyDataSetAppearance();
	}
}
