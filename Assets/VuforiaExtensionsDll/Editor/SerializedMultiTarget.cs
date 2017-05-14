using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	public class SerializedMultiTarget : SerializedDataSetTrackable
	{
		public SerializedMultiTarget(SerializedObject target) : base(target)
		{
		}

		public List<MultiTargetAbstractBehaviour> GetBehaviours()
		{
			List<MultiTargetAbstractBehaviour> list = new List<MultiTargetAbstractBehaviour>();
            UnityEngine.Object[] targetObjects = this.mSerializedObject.targetObjects;
			for (int i = 0; i < targetObjects.Length; i++)
			{
                UnityEngine.Object @object = targetObjects[i];
				list.Add((MultiTargetAbstractBehaviour)@object);
			}
			return list;
		}
	}
}
