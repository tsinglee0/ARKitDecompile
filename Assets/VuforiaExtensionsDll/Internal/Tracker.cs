using System;

namespace Vuforia
{
	public abstract class Tracker
	{
		public virtual bool IsActive
		{
			get;
			protected set;
		}

		public abstract bool Start();

		public abstract void Stop();
	}
}
