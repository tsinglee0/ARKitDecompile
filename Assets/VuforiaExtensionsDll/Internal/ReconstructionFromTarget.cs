using System;
using UnityEngine;

namespace Vuforia
{
	public interface ReconstructionFromTarget : Reconstruction
	{
		bool SetInitializationTarget(CylinderTarget cylinderTarget, Vector3 occluderMin, Vector3 occluderMax);

		bool SetInitializationTarget(CylinderTarget cylinderTarget, Vector3 occluderMin, Vector3 occluderMax, Vector3 offsetToOccluderOrigin, Quaternion rotationToOccluderOrigin);

		bool SetInitializationTarget(ImageTarget imageTarget, Vector3 occluderMin, Vector3 occluderMax);

		bool SetInitializationTarget(ImageTarget imageTarget, Vector3 occluderMin, Vector3 occluderMax, Vector3 offsetToOccluderOrigin, Quaternion rotationToOccluderOrigin);

		bool SetInitializationTarget(MultiTarget multiTarget, Vector3 occluderMin, Vector3 occluderMax);

		bool SetInitializationTarget(MultiTarget multiTarget, Vector3 occluderMin, Vector3 occluderMax, Vector3 offsetToOccluderOrigin, Quaternion rotationToOccluderOrigin);

		Trackable GetInitializationTarget(out Vector3 occluderMin, out Vector3 occluderMax);

		Trackable GetInitializationTarget(out Vector3 occluderMin, out Vector3 occluderMax, out Vector3 offsetToOccluderOrigin, out Quaternion rotationToOccluderOrigin);
	}
}
