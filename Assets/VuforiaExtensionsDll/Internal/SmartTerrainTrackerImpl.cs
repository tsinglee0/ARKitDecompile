using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
	internal class SmartTerrainTrackerImpl : SmartTerrainTracker
	{
		private float mScaleToMillimeter;

		private readonly SmartTerrainBuilderImpl mSmartTerrainBuilder = new SmartTerrainBuilderImpl();

		public override float ScaleToMillimeter
		{
			get
			{
				return this.mScaleToMillimeter;
			}
		}

		public override SmartTerrainBuilder SmartTerrainBuilder
		{
			get
			{
				return this.mSmartTerrainBuilder;
			}
		}

		public override bool SetScaleToMillimeter(float scaleFactor)
		{
			this.mScaleToMillimeter = scaleFactor;
			return VuforiaWrapper.Instance.SmartTerrainTrackerSetScaleToMillimeter(scaleFactor) == 1;
		}

		public override bool Start()
		{
			if (VuforiaWrapper.Instance.TrackerStart((int)TypeMapping.GetTypeID(typeof(SmartTerrainTracker))) != 1)
			{
				this.IsActive = false;
				Debug.LogError("Could not start tracker.");
				return false;
			}
			this.IsActive = true;
			return true;
		}

		public override void Stop()
		{
			VuforiaWrapper.Instance.TrackerStop((int)TypeMapping.GetTypeID(typeof(SmartTerrainTracker)));
			this.IsActive = false;
			using (IEnumerator<ReconstructionAbstractBehaviour> enumerator = this.mSmartTerrainBuilder.GetReconstructions().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.SetBehavioursToNotFound();
				}
			}
		}
	}
}
