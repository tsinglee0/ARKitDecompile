using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	public abstract class ReconstructionAbstractBehaviour : MonoBehaviour
	{
		private bool mHasInitialized;

		private List<ISmartTerrainEventHandler> mSmartTerrainEventHandlers = new List<ISmartTerrainEventHandler>();

		private Action<SmartTerrainInitializationInfo> mOnInitialized;

		private Action<Prop> mOnPropCreated;

		private Action<Prop> mOnPropUpdated;

		private Action<Prop> mOnPropDeleted;

		private Action<Surface> mOnSurfaceCreated;

		private Action<Surface> mOnSurfaceUpdated;

		private Action<Surface> mOnSurfaceDeleted;

		[HideInInspector, SerializeField]
		private bool mInitializedInEditor;

		[HideInInspector, SerializeField]
		private bool mMaximumExtentEnabled;

		[HideInInspector, SerializeField]
		private Rect mMaximumExtent;

		[HideInInspector, SerializeField]
		private bool mAutomaticStart;

		[HideInInspector, SerializeField]
		private bool mNavMeshUpdates;

		[HideInInspector, SerializeField]
		private float mNavMeshPadding;

		private Reconstruction mReconstruction;

		private readonly Dictionary<int, Surface> mSurfaces = new Dictionary<int, Surface>();

		private readonly Dictionary<int, SurfaceAbstractBehaviour> mActiveSurfaceBehaviours = new Dictionary<int, SurfaceAbstractBehaviour>();

		private readonly Dictionary<int, Prop> mProps = new Dictionary<int, Prop>();

		private readonly Dictionary<int, PropAbstractBehaviour> mActivePropBehaviours = new Dictionary<int, PropAbstractBehaviour>();

		private SurfaceAbstractBehaviour mPreviouslySetWorldCenterSurfaceTemplate;

		private bool mIgnoreNextUpdate;

		public Reconstruction Reconstruction
		{
			get
			{
				return this.mReconstruction;
			}
		}

		internal bool AutomaticStart
		{
			get
			{
				return this.mAutomaticStart;
			}
		}

		private void OnDrawGizmos()
		{
			if (this.mMaximumExtentEnabled)
			{
				Gizmos.matrix = Matrix4x4.TRS(base.gameObject.transform.position, base.gameObject.transform.rotation, Vector3.one);
				Gizmos.color = Color.red;
				Gizmos.DrawWireCube(new Vector3(this.mMaximumExtent.center.x, 0f, this.mMaximumExtent.center.y), new Vector3(this.mMaximumExtent.width, 0f, this.mMaximumExtent.height));
			}
		}

		private void OnValidate()
		{
			if (this.mMaximumExtentEnabled)
			{
				if (this.mMaximumExtent.width < 0f)
				{
					this.mMaximumExtent.width = 0f;
				}
				if (this.mMaximumExtent.height < 0f)
				{
					this.mMaximumExtent.height = 0f;
				}
			}
		}

		[Obsolete("This ISmartTerrainEventHandler interface will be removed with the next Vuforia release. Please use ReconstructionBehaviour.Register...Callback instead.")]
		public void RegisterSmartTerrainEventHandler(ISmartTerrainEventHandler trackableEventHandler)
		{
			this.mSmartTerrainEventHandlers.Add(trackableEventHandler);
			if (this.mHasInitialized)
			{
				trackableEventHandler.OnInitialized(default(SmartTerrainInitializationInfo));
			}
		}

		public bool UnregisterSmartTerrainEventHandler(ISmartTerrainEventHandler trackableEventHandler)
		{
			return this.mSmartTerrainEventHandlers.Remove(trackableEventHandler);
		}

		public void RegisterInitializedCallback(Action<SmartTerrainInitializationInfo> callback)
		{
			this.mOnInitialized = (Action<SmartTerrainInitializationInfo>)Delegate.Combine(this.mOnInitialized, callback);
			if (this.mHasInitialized)
			{
				callback(default(SmartTerrainInitializationInfo));
			}
		}

		public void UnregisterInitializedCallback(Action<SmartTerrainInitializationInfo> callback)
		{
			this.mOnInitialized = (Action<SmartTerrainInitializationInfo>)Delegate.Remove(this.mOnInitialized, callback);
		}

		public void RegisterPropCreatedCallback(Action<Prop> callback)
		{
			this.mOnPropCreated = (Action<Prop>)Delegate.Combine(this.mOnPropCreated, callback);
		}

		public void UnregisterPropCreatedCallback(Action<Prop> callback)
		{
			this.mOnPropCreated = (Action<Prop>)Delegate.Remove(this.mOnPropCreated, callback);
		}

		public void RegisterPropUpdatedCallback(Action<Prop> callback)
		{
			this.mOnPropUpdated = (Action<Prop>)Delegate.Combine(this.mOnPropUpdated, callback);
		}

		public void UnregisterPropUpdatedCallback(Action<Prop> callback)
		{
			this.mOnPropUpdated = (Action<Prop>)Delegate.Remove(this.mOnPropUpdated, callback);
		}

		public void RegisterPropDeletedCallback(Action<Prop> callback)
		{
			this.mOnPropDeleted = (Action<Prop>)Delegate.Combine(this.mOnPropDeleted, callback);
		}

		public void UnregisterPropDeletedCallback(Action<Prop> callback)
		{
			this.mOnPropDeleted = (Action<Prop>)Delegate.Remove(this.mOnPropDeleted, callback);
		}

		public void RegisterSurfaceCreatedCallback(Action<Surface> callback)
		{
			this.mOnSurfaceCreated = (Action<Surface>)Delegate.Combine(this.mOnSurfaceCreated, callback);
		}

		public void UnregisterSurfaceCreatedCallback(Action<Surface> callback)
		{
			this.mOnSurfaceCreated = (Action<Surface>)Delegate.Remove(this.mOnSurfaceCreated, callback);
		}

		public void RegisterSurfaceUpdatedCallback(Action<Surface> callback)
		{
			this.mOnSurfaceUpdated = (Action<Surface>)Delegate.Combine(this.mOnSurfaceUpdated, callback);
		}

		public void UnregisterSurfaceUpdatedCallback(Action<Surface> callback)
		{
			this.mOnSurfaceUpdated = (Action<Surface>)Delegate.Remove(this.mOnSurfaceUpdated, callback);
		}

		public void RegisterSurfaceDeletedCallback(Action<Surface> callback)
		{
			this.mOnSurfaceDeleted = (Action<Surface>)Delegate.Combine(this.mOnSurfaceDeleted, callback);
		}

		public void UnregisterSurfaceDeletedCallback(Action<Surface> callback)
		{
			this.mOnSurfaceDeleted = (Action<Surface>)Delegate.Remove(this.mOnSurfaceDeleted, callback);
		}

		public PropAbstractBehaviour AssociateProp(PropAbstractBehaviour templateBehaviour, Prop newProp)
		{
			if (this.mActivePropBehaviours.ContainsKey(newProp.ID))
			{
				Debug.LogWarning("Prop has already an associated behaviour");
				return null;
			}
			StateManagerImpl stateManagerImpl = TrackerManager.Instance.GetStateManager() as StateManagerImpl;
			if (templateBehaviour != null)
			{
				if (templateBehaviour.transform.parent == base.transform)
				{
					PropAbstractBehaviour propAbstractBehaviour = ReconstructionAbstractBehaviour.InstantiatePropBehaviour(templateBehaviour);
					Transform transform = null;
					if (newProp.Parent == null)
					{
						transform = base.transform;
					}
					else if (newProp.Parent is Surface)
					{
						if (this.mActiveSurfaceBehaviours.ContainsKey(newProp.Parent.ID))
						{
							transform = this.mActiveSurfaceBehaviours[newProp.Parent.ID].transform;
						}
						else
						{
							Debug.LogError("Parent Surface with id " + newProp.Parent.ID + " could not be found");
						}
					}
					else if (newProp.Parent is Prop)
					{
						if (this.mActivePropBehaviours.ContainsKey(newProp.Parent.ID))
						{
							transform = this.mActivePropBehaviours[newProp.Parent.ID].transform;
						}
						else
						{
							Debug.LogError("Parent Prop with id " + newProp.Parent.ID + " could not be found");
						}
					}
					if (transform != null)
					{
						propAbstractBehaviour.transform.parent = transform;
					}
					propAbstractBehaviour.gameObject.SetActive(true);
					this.AssociatePropBehaviour(newProp, propAbstractBehaviour);
					this.mActivePropBehaviours.Add(newProp.ID, propAbstractBehaviour);
					stateManagerImpl.RegisterExternallyManagedTrackableBehaviour(propAbstractBehaviour);
					return propAbstractBehaviour;
				}
				Debug.LogError("ReconstructionBehaviour.AssociateProp: provided template needs to be a child of the Reconstruction object.");
			}
			return null;
		}

		public SurfaceAbstractBehaviour AssociateSurface(SurfaceAbstractBehaviour templateBehaviour, Surface newSurface)
		{
			if (this.mActiveSurfaceBehaviours.ContainsKey(newSurface.ID))
			{
				Debug.LogWarning("Surface has already an associated behaviour");
				return null;
			}
			StateManagerImpl stateManagerImpl = TrackerManager.Instance.GetStateManager() as StateManagerImpl;
			if (templateBehaviour != null)
			{
				if (templateBehaviour.transform.parent == base.transform)
				{
					SurfaceAbstractBehaviour surfaceAbstractBehaviour = ReconstructionAbstractBehaviour.InstantiateSurfaceBehaviour(templateBehaviour);
					Transform transform = null;
					if (newSurface.Parent == null)
					{
						transform = base.transform;
					}
					else if (newSurface.Parent is Surface)
					{
						if (this.mActiveSurfaceBehaviours.ContainsKey(newSurface.Parent.ID))
						{
							transform = this.mActiveSurfaceBehaviours[newSurface.Parent.ID].transform;
						}
						else
						{
							Debug.LogError("Parent Surface with id " + newSurface.Parent.ID + " could not be found");
						}
					}
					else if (newSurface.Parent is Prop)
					{
						if (this.mActivePropBehaviours.ContainsKey(newSurface.Parent.ID))
						{
							transform = this.mActivePropBehaviours[newSurface.Parent.ID].transform;
						}
						else
						{
							Debug.LogError("Parent Prop with id " + newSurface.Parent.ID + " could not be found");
						}
					}
					if (transform != null)
					{
						surfaceAbstractBehaviour.transform.parent = transform;
					}
					surfaceAbstractBehaviour.gameObject.SetActive(true);
					this.AssociateSurfaceBehaviour(newSurface, surfaceAbstractBehaviour);
					this.mActiveSurfaceBehaviours.Add(newSurface.ID, surfaceAbstractBehaviour);
					stateManagerImpl.RegisterExternallyManagedTrackableBehaviour(surfaceAbstractBehaviour);
					if (VuforiaManager.Instance.WorldCenterMode == VuforiaARController.WorldCenterMode.SPECIFIC_TARGET)
					{
						if (VuforiaManager.Instance.WorldCenter == (WorldCenterTrackableBehaviour)templateBehaviour)
						{
							VuforiaManager.Instance.WorldCenter = surfaceAbstractBehaviour;
							this.mPreviouslySetWorldCenterSurfaceTemplate = templateBehaviour;
						}
						else if ((VuforiaManager.Instance.WorldCenter == null || VuforiaManager.Instance.WorldCenter.Equals(null)) && this.mPreviouslySetWorldCenterSurfaceTemplate == templateBehaviour)
						{
							VuforiaManager.Instance.WorldCenter = surfaceAbstractBehaviour;
						}
					}
					return surfaceAbstractBehaviour;
				}
				Debug.LogError("ReconstructionBehaviour.AssociateSurface: provided template needs to be a child of the Reconstruction object.");
			}
			return null;
		}

		public IEnumerable<Prop> GetActiveProps()
		{
			return this.mProps.Values;
		}

		public bool TryGetPropBehaviour(Prop prop, out PropAbstractBehaviour behaviour)
		{
			if (this.mActivePropBehaviours.ContainsKey(prop.ID))
			{
				behaviour = this.mActivePropBehaviours[prop.ID];
				return true;
			}
			behaviour = null;
			return false;
		}

		public IEnumerable<Surface> GetActiveSurfaces()
		{
			return this.mSurfaces.Values;
		}

		public bool TryGetSurfaceBehaviour(Surface surface, out SurfaceAbstractBehaviour behaviour)
		{
			if (this.mActiveSurfaceBehaviours.ContainsKey(surface.ID))
			{
				behaviour = this.mActiveSurfaceBehaviours[surface.ID];
				return true;
			}
			behaviour = null;
			return false;
		}

		internal void Initialize(Reconstruction reconstruction)
		{
			this.mReconstruction = reconstruction;
			if (TrackerManager.Instance.GetTracker<SmartTerrainTracker>() != null)
			{
				if (this.mMaximumExtentEnabled)
				{
					this.mReconstruction.SetMaximumArea(this.mMaximumExtent);
				}
				if (this.mAutomaticStart)
				{
					SmartTerrainTracker tracker = TrackerManager.Instance.GetTracker<SmartTerrainTracker>();
					if (tracker != null)
					{
						tracker.SmartTerrainBuilder.AddReconstruction(this);
					}
				}
				if (this.mNavMeshUpdates)
				{
					this.mReconstruction.StartNavMeshUpdates();
					this.mReconstruction.SetNavMeshPadding(this.mNavMeshPadding);
				}
				else
				{
					this.mReconstruction.StopNavMeshUpdates();
				}
				this.mHasInitialized = true;
				using (List<ISmartTerrainEventHandler>.Enumerator enumerator = this.mSmartTerrainEventHandlers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						enumerator.Current.OnInitialized(default(SmartTerrainInitializationInfo));
					}
				}
				if (this.mOnInitialized != null)
				{
					this.mOnInitialized.InvokeWithExceptionHandling(default(SmartTerrainInitializationInfo));
					return;
				}
			}
			else
			{
				Debug.LogError("SmartTerrainTrackerBehaviour.Initialize: SmartTerrainTracker is null, aborting.");
			}
		}

		internal void Deinitialize()
		{
			this.mReconstruction = null;
			this.mHasInitialized = false;
		}

		internal void UpdateSmartTerrainData(VuforiaManagerImpl.SmartTerrainRevisionData[] smartTerrainRevisions, VuforiaManagerImpl.SurfaceData[] updatedSurfaces, VuforiaManagerImpl.PropData[] updatedProps)
		{
			if (this.mIgnoreNextUpdate)
			{
				this.mIgnoreNextUpdate = false;
				return;
			}
			this.UpdateSurfaces(smartTerrainRevisions, updatedSurfaces);
			this.UpdateProps(smartTerrainRevisions, updatedProps);
		}

		internal void SetBehavioursToNotFound()
		{
			using (Dictionary<int, PropAbstractBehaviour>.ValueCollection.Enumerator enumerator = this.mActivePropBehaviours.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.OnTrackerUpdate(TrackableBehaviour.Status.NOT_FOUND);
				}
			}
			using (Dictionary<int, SurfaceAbstractBehaviour>.ValueCollection.Enumerator enumerator2 = this.mActiveSurfaceBehaviours.Values.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					enumerator2.Current.OnTrackerUpdate(TrackableBehaviour.Status.NOT_FOUND);
				}
			}
		}

		internal void ClearOnReset()
		{
			VuforiaManagerImpl.SmartTerrainRevisionData[] smartTerrainRevisions = new VuforiaManagerImpl.SmartTerrainRevisionData[0];
			VuforiaManagerImpl.SurfaceData[] updatedSurfaceData = new VuforiaManagerImpl.SurfaceData[0];
			VuforiaManagerImpl.PropData[] updatedPropData = new VuforiaManagerImpl.PropData[0];
			this.UpdateSurfaces(smartTerrainRevisions, updatedSurfaceData);
			this.UpdateProps(smartTerrainRevisions, updatedPropData);
			this.mIgnoreNextUpdate = true;
		}

		internal void OnReconstructionRemoved()
		{
			this.UnregisterDeletedProps(this.mProps.Values.ToList<Prop>());
			this.UnregisterDeletedSurfaces(this.mSurfaces.Values.ToList<Surface>());
			this.mActivePropBehaviours.Clear();
			foreach (KeyValuePair<int, Prop> current in this.mProps)
			{
				((PropImpl)current.Value).DestroyMesh();
			}
			this.mProps.Clear();
			this.mActiveSurfaceBehaviours.Clear();
			foreach (KeyValuePair<int, Surface> current2 in this.mSurfaces)
			{
				((SurfaceImpl)current2.Value).DestroyMesh();
			}
			this.mSurfaces.Clear();
		}

		public void ScaleEditorMeshesByFactor(float scaleFactor)
		{
			if (Application.isPlaying)
			{
				return;
			}
			MeshFilter[] componentsInChildren = base.GetComponentsInChildren<MeshFilter>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				MeshFilter meshFilter = componentsInChildren[i];
				Vector3[] vertices = meshFilter.sharedMesh.vertices;
				for (int j = 0; j < vertices.Length; j++)
				{
					vertices[j] *= scaleFactor;
				}
				Mesh mesh = UnityEngine.Object.Instantiate<Mesh>(meshFilter.sharedMesh);
				if (mesh != null)
				{
					mesh.vertices = vertices;
					meshFilter.sharedMesh = mesh;
				}
			}
			Resources.UnloadUnusedAssets();
		}

		public void ScaleEditorPropPositionsByFactor(float scaleFactor)
		{
			if (Application.isPlaying)
			{
				return;
			}
			PropAbstractBehaviour[] componentsInChildren = base.GetComponentsInChildren<PropAbstractBehaviour>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Transform expr_20 = componentsInChildren[i].gameObject.transform;
				expr_20.localPosition = expr_20.localPosition * scaleFactor;
			}
		}

		private static PropAbstractBehaviour InstantiatePropBehaviour(PropAbstractBehaviour input)
		{
			GameObject expr_0B = UnityEngine.Object.Instantiate<GameObject>(input.gameObject);
			expr_0B.transform.localScale = Vector3.one;
			return expr_0B.GetComponent<PropAbstractBehaviour>();
		}

		private void AssociatePropBehaviour(Prop trackable, PropAbstractBehaviour behaviour)
		{
			behaviour.InitializeProp(trackable);
		}

		private static SurfaceAbstractBehaviour InstantiateSurfaceBehaviour(SurfaceAbstractBehaviour input)
		{
			GameObject expr_0B = UnityEngine.Object.Instantiate<GameObject>(input.gameObject);
			expr_0B.transform.localScale = Vector3.one;
			return expr_0B.GetComponent<SurfaceAbstractBehaviour>();
		}

		private void AssociateSurfaceBehaviour(Surface trackable, SurfaceAbstractBehaviour behaviour)
		{
			behaviour.InitializeSurface(trackable);
		}

		private SmartTerrainTrackable FindSmartTerrainTrackable(int id)
		{
			SmartTerrainTrackable result = null;
			if (this.mSurfaces.ContainsKey(id))
			{
				result = this.mSurfaces[id];
			}
			else if (this.mProps.ContainsKey(id))
			{
				result = this.mProps[id];
			}
			return result;
		}

		private void NotifySurfaceEventHandlers(IEnumerable<Surface> newSurfaces, IEnumerable<Surface> updatedSurfaces, IEnumerable<Surface> deletedSurfaces)
		{
			foreach (Surface current in deletedSurfaces)
			{
				if (this.mOnSurfaceDeleted != null)
				{
					this.mOnSurfaceDeleted.InvokeWithExceptionHandling(current);
				}
			}
			foreach (Surface current2 in newSurfaces)
			{
				using (List<ISmartTerrainEventHandler>.Enumerator enumerator2 = this.mSmartTerrainEventHandlers.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.Current.OnSurfaceCreated(current2);
					}
				}
				if (this.mOnSurfaceCreated != null)
				{
					this.mOnSurfaceCreated.InvokeWithExceptionHandling(current2);
				}
			}
			foreach (Surface current3 in updatedSurfaces)
			{
				SurfaceAbstractBehaviour surfaceAbstractBehaviour;
				if (this.TryGetSurfaceBehaviour(current3, out surfaceAbstractBehaviour))
				{
					surfaceAbstractBehaviour.UpdateMeshAndColliders();
					using (List<ISmartTerrainEventHandler>.Enumerator enumerator2 = this.mSmartTerrainEventHandlers.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							enumerator2.Current.OnSurfaceUpdated(surfaceAbstractBehaviour);
						}
					}
				}
				if (this.mOnSurfaceUpdated != null)
				{
					this.mOnSurfaceUpdated.InvokeWithExceptionHandling(current3);
				}
			}
		}

		private void NotifyPropEventHandlers(IEnumerable<Prop> newProps, IEnumerable<Prop> updatedProps, IEnumerable<Prop> deletedProps)
		{
			foreach (Prop current in deletedProps)
			{
				using (List<ISmartTerrainEventHandler>.Enumerator enumerator2 = this.mSmartTerrainEventHandlers.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.Current.OnPropDeleted(current);
					}
				}
				if (this.mOnPropDeleted != null)
				{
					this.mOnPropDeleted.InvokeWithExceptionHandling(current);
				}
			}
			foreach (Prop current2 in newProps)
			{
				using (List<ISmartTerrainEventHandler>.Enumerator enumerator2 = this.mSmartTerrainEventHandlers.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.Current.OnPropCreated(current2);
					}
				}
				if (this.mOnPropCreated != null)
				{
					this.mOnPropCreated.InvokeWithExceptionHandling(current2);
				}
			}
			foreach (Prop current3 in updatedProps)
			{
				PropAbstractBehaviour propAbstractBehaviour;
				if (this.TryGetPropBehaviour(current3, out propAbstractBehaviour))
				{
					propAbstractBehaviour.UpdateMeshAndColliders();
				}
				using (List<ISmartTerrainEventHandler>.Enumerator enumerator2 = this.mSmartTerrainEventHandlers.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						enumerator2.Current.OnPropUpdated(current3);
					}
				}
				if (this.mOnPropUpdated != null)
				{
					this.mOnPropUpdated.InvokeWithExceptionHandling(current3);
				}
			}
		}

		private static int[] ReadMeshBoundaries(int numBoundaries, IntPtr boundaryArray)
		{
			int[] array = new int[numBoundaries];
			byte[] array2 = new byte[numBoundaries * 2];
			Marshal.Copy(boundaryArray, array2, 0, numBoundaries * 2);
			for (int i = 0; i < numBoundaries; i++)
			{
				int startIndex = i * 2;
				array[i] = (int)BitConverter.ToUInt16(array2, startIndex);
			}
			return array;
		}

		private void UnregisterDeletedProps(List<Prop> deletedProps)
		{
			StateManagerImpl stateManagerImpl = TrackerManager.Instance.GetStateManager() as StateManagerImpl;
			foreach (Prop current in deletedProps)
			{
				if (this.mActivePropBehaviours.ContainsKey(current.ID))
				{
					PropAbstractBehaviour propAbstractBehaviour = this.mActivePropBehaviours[current.ID];
					if (propAbstractBehaviour != null)
					{
						propAbstractBehaviour.OnTrackerUpdate(TrackableBehaviour.Status.NOT_FOUND);
						propAbstractBehaviour.UnregisterTrackable();
						UnityEngine.Object.Destroy(propAbstractBehaviour.gameObject);
					}
					this.mActivePropBehaviours.Remove(current.ID);
					stateManagerImpl.UnregisterExternallyManagedTrackableBehaviour(current.ID);
				}
			}
		}

		private void UnregisterDeletedSurfaces(List<Surface> deletedSurfaces)
		{
			StateManagerImpl stateManagerImpl = TrackerManager.Instance.GetStateManager() as StateManagerImpl;
			foreach (Surface current in deletedSurfaces)
			{
				if (this.mActiveSurfaceBehaviours.ContainsKey(current.ID))
				{
					SurfaceAbstractBehaviour surfaceAbstractBehaviour = this.mActiveSurfaceBehaviours[current.ID];
					if (surfaceAbstractBehaviour != null)
					{
						surfaceAbstractBehaviour.OnTrackerUpdate(TrackableBehaviour.Status.NOT_FOUND);
						surfaceAbstractBehaviour.UnregisterTrackable();
						UnityEngine.Object.Destroy(surfaceAbstractBehaviour.gameObject);
					}
					this.mActiveSurfaceBehaviours.Remove(current.ID);
					stateManagerImpl.UnregisterExternallyManagedTrackableBehaviour(current.ID);
				}
			}
		}

		private void UpdateSurfaces(VuforiaManagerImpl.SmartTerrainRevisionData[] smartTerrainRevisions, VuforiaManagerImpl.SurfaceData[] updatedSurfaceData)
		{
			List<Surface> list = new List<Surface>();
			List<Surface> list2 = new List<Surface>();
			List<Surface> list3 = new List<Surface>();
			for (int i = 0; i < updatedSurfaceData.Length; i++)
			{
				VuforiaManagerImpl.SurfaceData surfaceData = updatedSurfaceData[i];
				bool flag = false;
				SurfaceImpl surfaceImpl;
				bool flag2;
				if (!this.mSurfaces.ContainsKey(surfaceData.id))
				{
					surfaceImpl = new SurfaceImpl(surfaceData.id, this.FindSmartTerrainTrackable(surfaceData.parentID));
					this.mSurfaces.Add(surfaceData.id, surfaceImpl);
					list.Add(surfaceImpl);
					flag2 = true;
					flag = true;
				}
				else
				{
					surfaceImpl = (SurfaceImpl)this.mSurfaces[surfaceData.id];
					flag2 = (surfaceData.revision != surfaceImpl.MeshRevision);
				}
				if (flag2)
				{
					Mesh mesh = MeshUtils.UpdateMesh(surfaceData.meshData, surfaceImpl.GetMesh(), false, true);
					if (flag)
					{
						surfaceImpl.SetLocalPose(surfaceData.localPose);
					}
					if (mesh != null)
					{
						Mesh navMesh = null;
						if (this.mReconstruction.IsNavMeshUpdating())
						{
							navMesh = MeshUtils.UpdateMesh(surfaceData.navMeshData, surfaceImpl.GetNavMesh(), true, true);
						}
						int[] meshBoundaries = ReconstructionAbstractBehaviour.ReadMeshBoundaries(surfaceData.numBoundaryIndices, surfaceData.meshBoundaryArray);
						surfaceImpl.SetMesh(surfaceData.revision, mesh, navMesh, meshBoundaries);
					}
					RectangleData boundingBox = surfaceData.boundingBox;
					Rect boundingBox2 = new Rect(boundingBox.leftTopX, boundingBox.leftTopY, boundingBox.rightBottomX - boundingBox.leftTopX, boundingBox.rightBottomY - boundingBox.leftTopY);
					surfaceImpl.SetBoundingBox(boundingBox2);
					list2.Add(surfaceImpl);
				}
				else
				{
					Debug.LogError("Inconsistency: received updated Surface, but mesh revision is unchanged!");
				}
			}
			int[] array = new int[this.mSurfaces.Count];
			this.mSurfaces.Keys.CopyTo(array, 0);
			int[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				int num = array2[i];
				bool flag3 = false;
				for (int j = 0; j < smartTerrainRevisions.Length; j++)
				{
					if (smartTerrainRevisions[j].id == num)
					{
						flag3 = true;
					}
				}
				if (!flag3)
				{
					list3.Add(this.mSurfaces[num]);
					((SurfaceImpl)this.mSurfaces[num]).DestroyMesh();
					this.mSurfaces.Remove(num);
				}
			}
			this.UnregisterDeletedSurfaces(list3);
			foreach (Surface current in list3)
			{
				if (current.Parent != null)
				{
					((SmartTerrainTrackableImpl)current.Parent).RemoveChild(current);
				}
			}
			foreach (Surface current2 in list)
			{
				if (current2.Parent != null)
				{
					((SmartTerrainTrackableImpl)current2.Parent).AddChild(current2);
				}
			}
			this.NotifySurfaceEventHandlers(list, list2, list3);
		}

		private void UpdateProps(VuforiaManagerImpl.SmartTerrainRevisionData[] smartTerrainRevisions, VuforiaManagerImpl.PropData[] updatedPropData)
		{
			List<Prop> list = new List<Prop>();
			List<Prop> list2 = new List<Prop>();
			List<Prop> list3 = new List<Prop>();
			for (int i = 0; i < updatedPropData.Length; i++)
			{
				VuforiaManagerImpl.PropData propData = updatedPropData[i];
				bool flag = false;
				PropImpl propImpl;
				bool flag2;
				if (!this.mProps.ContainsKey(propData.id))
				{
					propImpl = new PropImpl(propData.id, this.FindSmartTerrainTrackable(propData.parentID));
					this.mProps.Add(propData.id, propImpl);
					list.Add(propImpl);
					flag2 = true;
					flag = true;
				}
				else
				{
					propImpl = (PropImpl)this.mProps[propData.id];
					flag2 = (propData.revision != propImpl.MeshRevision);
				}
				if (flag2)
				{
					Mesh mesh = MeshUtils.UpdateMesh(propData.meshData, propImpl.GetMesh(), false, true);
					if (flag)
					{
						propImpl.SetLocalPose(propData.localPose);
					}
					propImpl.SetMesh(propData.revision, mesh);
					VuforiaManagerImpl.Obb3D boundingBox = propData.boundingBox;
					OrientedBoundingBox3D obb = new OrientedBoundingBox3D(boundingBox.center, boundingBox.halfExtents, boundingBox.rotationZ * 57.29578f);
					propImpl.SetObb(obb);
					list2.Add(propImpl);
				}
				else
				{
					Debug.LogError("Inconsistency: received updated Prop, but mesh revision is unchanged!");
				}
			}
			int[] array = new int[this.mProps.Count];
			this.mProps.Keys.CopyTo(array, 0);
			int[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				int num = array2[i];
				bool flag3 = false;
				for (int j = 0; j < smartTerrainRevisions.Length; j++)
				{
					if (smartTerrainRevisions[j].id == num)
					{
						flag3 = true;
					}
				}
				if (!flag3)
				{
					list3.Add(this.mProps[num]);
					((PropImpl)this.mProps[num]).DestroyMesh();
					this.mProps.Remove(num);
				}
			}
			this.UnregisterDeletedProps(list3);
			foreach (Prop current in list3)
			{
				if (current.Parent != null)
				{
					((SmartTerrainTrackableImpl)current.Parent).RemoveChild(current);
				}
			}
			foreach (Prop current2 in list)
			{
				if (current2.Parent != null)
				{
					((SmartTerrainTrackableImpl)current2.Parent).AddChild(current2);
				}
			}
			this.NotifyPropEventHandlers(list, list2, list3);
		}
	}
}
