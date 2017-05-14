using System;
using UnityEngine;

namespace Vuforia
{
	public interface IBehaviourComponentFactory
	{
		MaskOutAbstractBehaviour AddMaskOutBehaviour(GameObject gameObject);

		VirtualButtonAbstractBehaviour AddVirtualButtonBehaviour(GameObject gameObject);

		TurnOffAbstractBehaviour AddTurnOffBehaviour(GameObject gameObject);

		ImageTargetAbstractBehaviour AddImageTargetBehaviour(GameObject gameObject);

		MultiTargetAbstractBehaviour AddMultiTargetBehaviour(GameObject gameObject);

		CylinderTargetAbstractBehaviour AddCylinderTargetBehaviour(GameObject gameObject);

		WordAbstractBehaviour AddWordBehaviour(GameObject gameObject);

		TextRecoAbstractBehaviour AddTextRecoBehaviour(GameObject gameObject);

		ObjectTargetAbstractBehaviour AddObjectTargetBehaviour(GameObject gameObject);

		VuMarkAbstractBehaviour AddVuMarkBehaviour(GameObject gameObject);

		VuforiaAbstractConfiguration CreateVuforiaConfiguration();
	}
}
