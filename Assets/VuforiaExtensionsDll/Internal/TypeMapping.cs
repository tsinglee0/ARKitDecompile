using System;
using System.Collections.Generic;

namespace Vuforia
{
	internal static class TypeMapping
	{
		private static Dictionary<Type, ushort> sTypes = new Dictionary<Type, ushort>
		{
			{
				typeof(ImageTarget),
				1
			},
			{
				typeof(MultiTarget),
				2
			},
			{
				typeof(CylinderTarget),
				3
			},
			{
				typeof(VuMarkTemplate),
				4
			},
			{
				typeof(VuMarkTarget),
				5
			},
			{
				typeof(Word),
				6
			},
			{
				typeof(ObjectTarget),
				7
			},
			{
				typeof(ObjectTracker),
				8
			},
			{
				typeof(TextTracker),
				9
			},
			{
				typeof(SmartTerrainTracker),
				10
			},
			{
				typeof(DeviceTracker),
				11
			},
			{
				typeof(RotationalDeviceTracker),
				11
			}
		};

		internal static ushort GetTypeID(Type type)
		{
			return TypeMapping.sTypes[type];
		}
	}
}
