using System;

namespace Vuforia
{
	internal abstract class TrackableImpl : Trackable
	{
		public string Name
		{
			get;
			protected set;
		}

		public int ID
		{
			get;
			protected set;
		}

		protected TrackableImpl(string name, int id)
		{
			this.Name = name;
			this.ID = id;
		}
	}
}
