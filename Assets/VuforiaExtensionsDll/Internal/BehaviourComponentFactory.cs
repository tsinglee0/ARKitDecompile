using System;
using UnityEngine;

namespace Vuforia
{
	public class BehaviourComponentFactory
	{
		private class NullBehaviourComponentFactory : IBehaviourComponentFactory
		{
			public MaskOutAbstractBehaviour AddMaskOutBehaviour(GameObject gameObject)
			{
				return null;
			}

			public VirtualButtonAbstractBehaviour AddVirtualButtonBehaviour(GameObject gameObject)
			{
				return null;
			}

			public TurnOffAbstractBehaviour AddTurnOffBehaviour(GameObject gameObject)
			{
				return null;
			}

			public ImageTargetAbstractBehaviour AddImageTargetBehaviour(GameObject gameObject)
			{
				return null;
			}

			public MultiTargetAbstractBehaviour AddMultiTargetBehaviour(GameObject gameObject)
			{
				return null;
			}

			public CylinderTargetAbstractBehaviour AddCylinderTargetBehaviour(GameObject gameObject)
			{
				return null;
			}

			public WordAbstractBehaviour AddWordBehaviour(GameObject gameObject)
			{
				return null;
			}

			public TextRecoAbstractBehaviour AddTextRecoBehaviour(GameObject gameObject)
			{
				return null;
			}

			public ObjectTargetAbstractBehaviour AddObjectTargetBehaviour(GameObject gameObject)
			{
				return null;
			}

			public VuMarkAbstractBehaviour AddVuMarkBehaviour(GameObject gameObject)
			{
				return null;
			}

			public VuforiaAbstractConfiguration CreateVuforiaConfiguration()
			{
				return null;
			}
		}

		private static IBehaviourComponentFactory sInstance;

		public static IBehaviourComponentFactory Instance
		{
			get
			{
				if (BehaviourComponentFactory.sInstance == null)
				{
					BehaviourComponentFactory.sInstance = new BehaviourComponentFactory.NullBehaviourComponentFactory();
				}
				return BehaviourComponentFactory.sInstance;
			}
			set
			{
				BehaviourComponentFactory.sInstance = value;
			}
		}
	}
}
