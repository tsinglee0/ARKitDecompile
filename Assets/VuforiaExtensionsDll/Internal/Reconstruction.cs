using System;
using UnityEngine;

namespace Vuforia
{
	public interface Reconstruction
	{
		float NavMeshPadding
		{
			get;
		}

		bool SetMaximumArea(Rect maximumArea);

		bool GetMaximumArea(out Rect rect);

		bool Stop();

		bool Start();

		bool IsReconstructing();

		void SetNavMeshPadding(float padding);

		void StartNavMeshUpdates();

		void StopNavMeshUpdates();

		bool IsNavMeshUpdating();

		bool Reset();
	}
}
