using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class StateManagerImpl : StateManager
	{
		private readonly Dictionary<int, TrackableBehaviour> mTrackableBehaviours = new Dictionary<int, TrackableBehaviour>();

		private readonly List<int> mAutomaticallyCreatedBehaviours = new List<int>();

		private readonly List<TrackableBehaviour> mBehavioursMarkedForDeletion = new List<TrackableBehaviour>();

		private readonly List<TrackableBehaviour> mActiveTrackableBehaviours = new List<TrackableBehaviour>();

		private readonly WordManagerImpl mWordManager = new WordManagerImpl();

		private readonly VuMarkManagerImpl mVuMarkManager = new VuMarkManagerImpl();

		private readonly DeviceTrackingManager mDeviceTrackingManager = new DeviceTrackingManager();

		private GameObject mCameraPositioningHelper;

		private IExtendedTrackingManager mExtendedTrackingManager = new VuforiaExtendedTrackingManager();

		public override IEnumerable<TrackableBehaviour> GetActiveTrackableBehaviours()
		{
			List<TrackableBehaviour> list = new List<TrackableBehaviour>();
			list.AddRange(this.mActiveTrackableBehaviours);
			foreach (VuMarkAbstractBehaviour current in this.mVuMarkManager.GetActiveBehaviours())
			{
				list.Add(current);
			}
			return list;
		}

		public override IEnumerable<TrackableBehaviour> GetTrackableBehaviours()
		{
			List<TrackableBehaviour> list = new List<TrackableBehaviour>();
			list.AddRange(this.mTrackableBehaviours.Values);
			foreach (VuMarkAbstractBehaviour current in this.mVuMarkManager.GetAllBehaviours())
			{
				list.Add(current);
			}
			return list;
		}

		public override WordManager GetWordManager()
		{
			return this.mWordManager;
		}

		public override VuMarkManager GetVuMarkManager()
		{
			return this.mVuMarkManager;
		}

		public override void DestroyTrackableBehavioursForTrackable(Trackable trackable, bool destroyGameObjects = true)
		{
			TrackableBehaviour trackableBehaviour;
			if (this.mTrackableBehaviours.TryGetValue(trackable.ID, out trackableBehaviour))
			{
				if (destroyGameObjects)
				{
					this.mBehavioursMarkedForDeletion.Add(this.mTrackableBehaviours[trackable.ID]);
					UnityEngine.Object.Destroy(trackableBehaviour.gameObject);
				}
				else
				{
					trackableBehaviour.UnregisterTrackable();
				}
				this.mTrackableBehaviours.Remove(trackable.ID);
				this.mAutomaticallyCreatedBehaviours.Remove(trackable.ID);
			}
		}

		public override void ReassociateTrackables()
		{
			this.RemoveDestroyedTrackables();
			VuMarkManagerImpl expr_11 = (VuMarkManagerImpl)this.GetVuMarkManager();
			expr_11.RemoveDestroyedTrackables();
			expr_11.DestroyBehaviourCopies();
			ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
			if (tracker != null)
			{
				IEnumerable<DataSet> dataSets = tracker.GetDataSets();
				List<DataSet> list = tracker.GetActiveDataSets().ToList<DataSet>();
				foreach (DataSet current in dataSets)
				{
					if (list.Contains(current))
					{
						tracker.DeactivateDataSet(current);
					}
					this.AssociateTrackableBehavioursForDataSet(current);
					if (list.Contains(current))
					{
						tracker.ActivateDataSet(current);
					}
				}
			}
			WordManagerImpl expr_9B = (WordManagerImpl)this.GetWordManager();
			expr_9B.RemoveDestroyedTrackables();
			expr_9B.InitializeWordBehaviourTemplates();
		}

		internal void SetExtendedTrackingManager(IExtendedTrackingManager extendedTrackingManager)
		{
			this.mExtendedTrackingManager = extendedTrackingManager;
		}

		internal IExtendedTrackingManager GetExtendedTrackingManager()
		{
			return this.mExtendedTrackingManager;
		}

		internal void AssociateTrackableBehavioursForDataSet(DataSet dataSet)
		{
			DataSetTrackableBehaviour[] array = (DataSetTrackableBehaviour[])UnityEngine.Object.FindObjectsOfType(typeof(DataSetTrackableBehaviour));
			for (int i = 0; i < array.Length; i++)
			{
				DataSetTrackableBehaviour dataSetTrackableBehaviour = array[i];
				if (!this.mBehavioursMarkedForDeletion.Contains(dataSetTrackableBehaviour) && !this.mVuMarkManager.IsBehaviourMarkedForDeletion(dataSetTrackableBehaviour))
				{
					if (dataSetTrackableBehaviour.TrackableName == null)
					{
						Debug.LogError("Found Trackable without name.");
					}
					else
					{
						int num = dataSet.Path.LastIndexOf('/') + 1;
						string text = (num < dataSet.Path.Length) ? dataSet.Path.Substring(num) : null;
						if (text != null && dataSetTrackableBehaviour.DataSetPath.EndsWith(text))
						{
							bool flag = false;
							foreach (Trackable current in dataSet.GetTrackables())
							{
								if (current.Name.Equals(dataSetTrackableBehaviour.TrackableName))
								{
									if (this.mTrackableBehaviours.ContainsKey(current.ID) && this.mTrackableBehaviours[current.ID] != dataSetTrackableBehaviour)
									{
										if (!this.mAutomaticallyCreatedBehaviours.Contains(current.ID) && !this.mBehavioursMarkedForDeletion.Contains(this.mTrackableBehaviours[current.ID]))
										{
											flag = true;
											continue;
										}
										UnityEngine.Object.Destroy(this.mTrackableBehaviours[current.ID].gameObject);
										this.mTrackableBehaviours.Remove(current.ID);
										this.mAutomaticallyCreatedBehaviours.Remove(current.ID);
									}
									flag = dataSetTrackableBehaviour.InitializeTarget(current, false);
									if (dataSetTrackableBehaviour is VuMarkAbstractBehaviour)
									{
										if (this.mVuMarkManager.AddTemplateBehaviour((VuMarkAbstractBehaviour)dataSetTrackableBehaviour))
										{
											Debug.Log(string.Concat(new object[]
											{
												"Found Trackable named ",
												dataSetTrackableBehaviour.Trackable.Name,
												" with id ",
												dataSetTrackableBehaviour.Trackable.ID
											}));
										}
									}
									else
									{
										this.mTrackableBehaviours[current.ID] = dataSetTrackableBehaviour;
										Debug.Log(string.Concat(new object[]
										{
											"Found Trackable named ",
											dataSetTrackableBehaviour.Trackable.Name,
											" with id ",
											dataSetTrackableBehaviour.Trackable.ID
										}));
									}
								}
							}
							if (!flag)
							{
								Debug.LogError("Could not associate DataSetTrackableBehaviour '" + dataSetTrackableBehaviour.TrackableName + "' - no matching Trackable found in DataSet!");
							}
						}
					}
				}
			}
			VirtualButtonAbstractBehaviour[] vbBehaviours = (VirtualButtonAbstractBehaviour[])UnityEngine.Object.FindObjectsOfType(typeof(VirtualButtonAbstractBehaviour));
			this.AssociateVirtualButtonBehaviours(vbBehaviours, dataSet);
			this.CreateMissingDataSetTrackableBehaviours(dataSet);
		}

		internal void RegisterExternallyManagedTrackableBehaviour(TrackableBehaviour trackableBehaviour)
		{
			if (!this.mTrackableBehaviours.ContainsKey(trackableBehaviour.Trackable.ID))
			{
				this.mTrackableBehaviours[trackableBehaviour.Trackable.ID] = trackableBehaviour;
				return;
			}
			Debug.LogError("Cannot register trackable behaviour. A TrackableBehaviour with the same ID already exits: " + trackableBehaviour.Trackable.ID);
		}

		internal void UnregisterExternallyManagedTrackableBehaviour(int id)
		{
			if (this.mTrackableBehaviours.ContainsKey(id))
			{
				this.mTrackableBehaviours.Remove(id);
			}
		}

		internal void RemoveDestroyedTrackables()
		{
			int[] array = this.mTrackableBehaviours.Keys.ToArray<int>();
			for (int i = 0; i < array.Length; i++)
			{
				int num = array[i];
				if (this.mTrackableBehaviours[num] == null)
				{
					this.mTrackableBehaviours.Remove(num);
					this.mAutomaticallyCreatedBehaviours.Remove(num);
				}
			}
		}

		internal void ClearTrackableBehaviours()
		{
			this.mTrackableBehaviours.Clear();
			this.mActiveTrackableBehaviours.Clear();
			this.mAutomaticallyCreatedBehaviours.Clear();
			this.mBehavioursMarkedForDeletion.Clear();
			this.mVuMarkManager.ClearBehaviours();
		}

		internal ImageTargetAbstractBehaviour FindOrCreateImageTargetBehaviourForTrackable(ImageTarget trackable, GameObject gameObject)
		{
			return this.FindOrCreateImageTargetBehaviourForTrackable(trackable, gameObject, null);
		}

		internal ImageTargetAbstractBehaviour FindOrCreateImageTargetBehaviourForTrackable(ImageTarget trackable, GameObject gameObject, DataSet dataSet)
		{
			DataSetTrackableBehaviour dataSetTrackableBehaviour = gameObject.GetComponent<DataSetTrackableBehaviour>();
			if (dataSetTrackableBehaviour == null)
			{
				dataSetTrackableBehaviour = BehaviourComponentFactory.Instance.AddImageTargetBehaviour(gameObject);
			}
			if (!(dataSetTrackableBehaviour is ImageTargetAbstractBehaviour))
			{
				Debug.LogError(string.Format("DataSet.CreateTrackable: Trackable of type ImageTarget was created, but behaviour of type {0} was provided!", dataSetTrackableBehaviour.GetType()));
				return null;
			}
			dataSetTrackableBehaviour.InitializeTarget(trackable, false);
			this.mTrackableBehaviours[trackable.ID] = dataSetTrackableBehaviour;
			return dataSetTrackableBehaviour as ImageTargetAbstractBehaviour;
		}

		internal void SetTrackableBehavioursForTrackableToNotFound(Trackable trackable)
		{
			TrackableBehaviour trackableBehaviour;
			if (this.mTrackableBehaviours.TryGetValue(trackable.ID, out trackableBehaviour))
			{
				trackableBehaviour.OnTrackerUpdate(TrackableBehaviour.Status.NOT_FOUND);
			}
		}

		internal void EnableTrackableBehavioursForTrackable(Trackable trackable, bool enabled)
		{
			TrackableBehaviour trackableBehaviour;
			if (this.mTrackableBehaviours.TryGetValue(trackable.ID, out trackableBehaviour) && trackableBehaviour != null)
			{
				trackableBehaviour.enabled = enabled;
			}
		}

		internal void RemoveDisabledTrackablesFromQueue(ref LinkedList<VuforiaManager.TrackableIdPair> trackableIDs)
		{
			LinkedListNode<VuforiaManager.TrackableIdPair> arg_30_0;
			for (LinkedListNode<VuforiaManager.TrackableIdPair> linkedListNode = trackableIDs.First; linkedListNode != null; linkedListNode = arg_30_0)
			{
				arg_30_0 = linkedListNode.Next;
				TrackableBehaviour trackableBehaviour;
				if (this.TryGetBehaviour(linkedListNode.Value, out trackableBehaviour) && !trackableBehaviour.enabled)
				{
					trackableIDs.Remove(linkedListNode);
				}
			}
		}

		internal void UpdateCameraPoseWRTTrackable(Transform cameraTransform, Transform parentTransformToUpdate, VuforiaManager.TrackableIdPair trackableId, VuforiaManagerImpl.PoseData trackablePose)
		{
			TrackableBehaviour trackableBehaviour;
			if (this.TryGetBehaviour(trackableId, out trackableBehaviour) && trackableBehaviour.enabled)
			{
				this.PositionCameraToTrackable(trackableBehaviour, cameraTransform, parentTransformToUpdate, trackablePose);
			}
		}

		internal void UpdateTrackablePoses(Transform arCameraTransform, VuforiaManagerImpl.TrackableResultData[] trackableResultDataArray, VuforiaManagerImpl.VuMarkTargetResultData[] vuMarkResultDataArray, VuforiaManager.TrackableIdPair originTrackableID, int frameIndex)
		{
			Dictionary<int, TrackableBehaviour.Status> dictionary = new Dictionary<int, TrackableBehaviour.Status>();
			for (int i = 0; i < trackableResultDataArray.Length; i++)
			{
				VuforiaManagerImpl.TrackableResultData trackableResultData = trackableResultDataArray[i];
				dictionary.Add(trackableResultData.id, trackableResultData.status);
				TrackableBehaviour trackableBehaviour;
				if (this.mTrackableBehaviours.TryGetValue(trackableResultData.id, out trackableBehaviour) && trackableResultData.id != originTrackableID.TrackableId && VuforiaManagerImpl.IsDetectedOrTracked(trackableResultData.status) && trackableBehaviour.enabled)
				{
					StateManagerImpl.PositionTrackable(trackableBehaviour, arCameraTransform, trackableResultData.pose, trackableResultData.timeStamp);
				}
			}
			Dictionary<int, TrackableBehaviour.Status> dictionary2 = new Dictionary<int, TrackableBehaviour.Status>();
			for (int i = 0; i < vuMarkResultDataArray.Length; i++)
			{
				VuforiaManagerImpl.VuMarkTargetResultData vuMarkTargetResultData = vuMarkResultDataArray[i];
				dictionary2.Add(vuMarkTargetResultData.resultID, vuMarkTargetResultData.status);
			}
			this.mVuMarkManager.UpdateVuMarkPoses(arCameraTransform, vuMarkResultDataArray, originTrackableID.ResultId);
			this.mActiveTrackableBehaviours.Clear();
			TrackableBehaviour[] array = this.GetTrackableBehaviours().ToArray<TrackableBehaviour>();
			for (int i = 0; i < array.Length; i++)
			{
				TrackableBehaviour trackableBehaviour2 = array[i];
				if (trackableBehaviour2.enabled)
				{
					TrackableBehaviour.Status vuforiaStatus;
					if (dictionary.TryGetValue(trackableBehaviour2.Trackable.ID, out vuforiaStatus) || (trackableBehaviour2 is VuMarkAbstractBehaviour && dictionary2.TryGetValue(((VuMarkAbstractBehaviour)trackableBehaviour2).VuMarkResultId, out vuforiaStatus)))
					{
						this.mExtendedTrackingManager.ApplyTrackingState(trackableBehaviour2, vuforiaStatus, arCameraTransform);
						trackableBehaviour2.OnFrameIndexUpdate(frameIndex);
					}
					else
					{
						this.mExtendedTrackingManager.ApplyTrackingState(trackableBehaviour2, TrackableBehaviour.Status.NOT_FOUND, arCameraTransform);
					}
					if (VuforiaManagerImpl.IsDetectedOrTracked(trackableBehaviour2.CurrentStatus))
					{
						this.mActiveTrackableBehaviours.Add(trackableBehaviour2);
					}
				}
			}
		}

		internal void UpdateVirtualButtons(int numVirtualButtons, IntPtr virtualButtonPtr)
		{
			Dictionary<int, VuforiaManagerImpl.VirtualButtonData> dictionary = new Dictionary<int, VuforiaManagerImpl.VirtualButtonData>();
			for (int i = 0; i < numVirtualButtons; i++)
			{
				VuforiaManagerImpl.VirtualButtonData virtualButtonData = (VuforiaManagerImpl.VirtualButtonData)Marshal.PtrToStructure(new IntPtr(virtualButtonPtr.ToInt64() + (long)(i * Marshal.SizeOf(typeof(VuforiaManagerImpl.VirtualButtonData)))), typeof(VuforiaManagerImpl.VirtualButtonData));
				dictionary.Add(virtualButtonData.id, virtualButtonData);
			}
			List<VirtualButtonAbstractBehaviour> list = new List<VirtualButtonAbstractBehaviour>();
			using (Dictionary<int, TrackableBehaviour>.ValueCollection.Enumerator enumerator = this.mTrackableBehaviours.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ImageTargetAbstractBehaviour imageTargetAbstractBehaviour = enumerator.Current as ImageTargetAbstractBehaviour;
					if (imageTargetAbstractBehaviour != null && imageTargetAbstractBehaviour.enabled)
					{
						foreach (VirtualButtonAbstractBehaviour current in imageTargetAbstractBehaviour.GetVirtualButtonBehaviours())
						{
							if (current.enabled)
							{
								list.Add(current);
							}
						}
					}
				}
			}
			foreach (VirtualButtonAbstractBehaviour current2 in list)
			{
				VuforiaManagerImpl.VirtualButtonData virtualButtonData2;
				if (dictionary.TryGetValue(current2.VirtualButton.ID, out virtualButtonData2))
				{
					current2.OnTrackerUpdated(virtualButtonData2.isPressed > 0);
				}
				else
				{
					current2.OnTrackerUpdated(false);
				}
			}
		}

		internal void UpdateWords(Transform arCameraTransform, VuforiaManagerImpl.WordData[] wordData, VuforiaManagerImpl.WordResultData[] wordResultData)
		{
			this.mWordManager.UpdateWords(arCameraTransform, wordData, wordResultData);
		}

		internal void UpdateVuMarks(VuforiaManagerImpl.VuMarkTargetData[] vuMarkData, VuforiaManagerImpl.VuMarkTargetResultData[] vuMarkResultData)
		{
			this.mVuMarkManager.UpdateVuMarks(vuMarkData, vuMarkResultData);
		}

		internal DeviceTrackingManager GetDeviceTrackingManager()
		{
			return this.mDeviceTrackingManager;
		}

		private bool TryGetBehaviour(VuforiaManager.TrackableIdPair trackableId, out TrackableBehaviour trackableBehaviour)
		{
			if (trackableId.ResultId >= 0)
			{
				using (List<VuMarkAbstractBehaviour>.Enumerator enumerator = this.mVuMarkManager.GetActiveBehaviours().ToList<VuMarkAbstractBehaviour>().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						VuMarkAbstractBehaviour current = enumerator.Current;
						if (current.VuMarkResultId == trackableId.ResultId)
						{
							trackableBehaviour = current;
							return true;
						}
					}
					goto IL_6A;
				}
				goto IL_57;
				IL_6A:
				trackableBehaviour = null;
				return false;
			}
			IL_57:
			return this.mTrackableBehaviours.TryGetValue(trackableId.TrackableId, out trackableBehaviour);
		}

		private void AssociateVirtualButtonBehaviours(VirtualButtonAbstractBehaviour[] vbBehaviours, DataSet dataSet)
		{
			for (int i = 0; i < vbBehaviours.Length; i++)
			{
				VirtualButtonAbstractBehaviour virtualButtonAbstractBehaviour = vbBehaviours[i];
				if (virtualButtonAbstractBehaviour.VirtualButtonName == null)
				{
					Debug.LogError("VirtualButton at " + i + " has no name.");
				}
				else
				{
					ImageTargetAbstractBehaviour imageTargetBehaviour = virtualButtonAbstractBehaviour.GetImageTargetBehaviour();
					if (imageTargetBehaviour == null)
					{
						Debug.LogError("VirtualButton named " + virtualButtonAbstractBehaviour.VirtualButtonName + " is not attached to an ImageTarget.");
					}
					else if (dataSet.Contains(imageTargetBehaviour.Trackable))
					{
						imageTargetBehaviour.AssociateExistingVirtualButtonBehaviour(virtualButtonAbstractBehaviour);
					}
				}
			}
		}

		private void CreateMissingDataSetTrackableBehaviours(DataSet dataSet)
		{
			foreach (Trackable current in dataSet.GetTrackables())
			{
				if (!this.mTrackableBehaviours.ContainsKey(current.ID))
				{
					if (current is ImageTarget)
					{
						ImageTargetAbstractBehaviour imageTargetAbstractBehaviour = this.CreateImageTargetBehaviour((ImageTarget)current);
						imageTargetAbstractBehaviour.CreateMissingVirtualButtonBehaviours();
						this.mTrackableBehaviours[current.ID] = imageTargetAbstractBehaviour;
						this.mAutomaticallyCreatedBehaviours.Add(current.ID);
					}
					else if (current is MultiTarget)
					{
						MultiTargetAbstractBehaviour value = this.CreateMultiTargetBehaviour((MultiTarget)current);
						this.mTrackableBehaviours[current.ID] = value;
						this.mAutomaticallyCreatedBehaviours.Add(current.ID);
					}
					else if (current is CylinderTarget)
					{
						CylinderTargetAbstractBehaviour value2 = this.CreateCylinderTargetBehaviour((CylinderTarget)current);
						this.mTrackableBehaviours[current.ID] = value2;
						this.mAutomaticallyCreatedBehaviours.Add(current.ID);
					}
					else if (current is VuMarkTemplate)
					{
						if (!this.mVuMarkManager.ContainsVuMarkTemplate(current.ID))
						{
							VuMarkAbstractBehaviour behaviour = this.CreateVuMarkBehaviour((VuMarkTemplate)current);
							this.mVuMarkManager.AddTemplateBehaviour(behaviour);
						}
					}
					else if (current is ObjectTarget)
					{
						DataSetTrackableBehaviour value3 = this.CreateObjectTargetBehaviour((ObjectTarget)current);
						this.mTrackableBehaviours[current.ID] = value3;
						this.mAutomaticallyCreatedBehaviours.Add(current.ID);
					}
				}
			}
		}

		private ImageTargetAbstractBehaviour CreateImageTargetBehaviour(ImageTarget imageTarget)
		{
			GameObject gameObject = new GameObject();
			ImageTargetAbstractBehaviour imageTargetAbstractBehaviour = BehaviourComponentFactory.Instance.AddImageTargetBehaviour(gameObject);
			Debug.Log(string.Concat(new object[]
			{
				"Creating Image Target with values: \n ID:           ",
				imageTarget.ID,
				"\n Name:         ",
				imageTarget.Name,
				"\n Path:         ",
				((ImageTargetImpl)imageTarget).DataSet.Path,
				"\n Size:         ",
				imageTarget.GetSize().x,
				"x",
				imageTarget.GetSize().y
			}));
			imageTargetAbstractBehaviour.InitializeTarget(imageTarget, true);
			return imageTargetAbstractBehaviour;
		}

		private MultiTargetAbstractBehaviour CreateMultiTargetBehaviour(MultiTarget multiTarget)
		{
			GameObject gameObject = new GameObject();
			MultiTargetAbstractBehaviour multiTargetAbstractBehaviour = BehaviourComponentFactory.Instance.AddMultiTargetBehaviour(gameObject);
			Debug.Log(string.Concat(new object[]
			{
				"Creating Multi Target with values: \n ID:           ",
				multiTarget.ID,
				"\n Name:         ",
				multiTarget.Name,
				"\n Path:         ",
				((MultiTargetImpl)multiTarget).DataSet.Path
			}));
			multiTargetAbstractBehaviour.InitializeTarget(multiTarget, true);
			return multiTargetAbstractBehaviour;
		}

		private CylinderTargetAbstractBehaviour CreateCylinderTargetBehaviour(CylinderTarget cylinderTarget)
		{
			GameObject gameObject = new GameObject();
			CylinderTargetAbstractBehaviour cylinderTargetAbstractBehaviour = BehaviourComponentFactory.Instance.AddCylinderTargetBehaviour(gameObject);
			Debug.Log(string.Concat(new object[]
			{
				"Creating Cylinder Target with values: \n ID:           ",
				cylinderTarget.ID,
				"\n Name:         ",
				cylinderTarget.Name,
				"\n Path:         ",
				((CylinderTargetImpl)cylinderTarget).DataSet.Path,
				"\n Side Length:  ",
				cylinderTarget.GetSideLength(),
				"\n Top Diameter: ",
				cylinderTarget.GetTopDiameter(),
				"\n Bottom Diam.: ",
				cylinderTarget.GetBottomDiameter()
			}));
			cylinderTargetAbstractBehaviour.InitializeTarget(cylinderTarget, true);
			return cylinderTargetAbstractBehaviour;
		}

		private VuMarkAbstractBehaviour CreateVuMarkBehaviour(VuMarkTemplate target)
		{
			GameObject gameObject = new GameObject();
			VuMarkAbstractBehaviour vuMarkAbstractBehaviour = BehaviourComponentFactory.Instance.AddVuMarkBehaviour(gameObject);
			Debug.Log(string.Concat(new object[]
			{
				"Creating VuMark Template with values: \n ID:           ",
				target.ID,
				"\n Name:         ",
				target.Name,
				"\n Path:         ",
				((VuMarkTemplateImpl)target).DataSet.Path
			}));
			vuMarkAbstractBehaviour.InitializeTarget(target, true);
			return vuMarkAbstractBehaviour;
		}

		private ObjectTargetAbstractBehaviour CreateObjectTargetBehaviour(ObjectTarget objectTarget)
		{
			GameObject gameObject = new GameObject();
			ObjectTargetAbstractBehaviour objectTargetAbstractBehaviour = BehaviourComponentFactory.Instance.AddObjectTargetBehaviour(gameObject);
			Debug.Log(string.Concat(new object[]
			{
				"Creating Object Target with values: \n ID:           ",
				objectTarget.ID,
				"\n Name:         ",
				objectTarget.Name,
				"\n Path:         ",
				((ObjectTargetImpl)objectTarget).DataSet.Path,
				"\n Size:         ",
				objectTarget.GetSize().x,
				"x",
				objectTarget.GetSize().y,
				"x",
				objectTarget.GetSize().z
			}));
			objectTargetAbstractBehaviour.InitializeTarget(objectTarget, true);
			return objectTargetAbstractBehaviour;
		}

		private void PositionCameraToTrackable(TrackableBehaviour trackable, Transform cameraTransform, Transform parentTransformToUpdate, VuforiaManagerImpl.PoseData camToTargetPose)
		{
			Quaternion quaternion = Quaternion.Inverse(camToTargetPose.orientation);
			Vector3 position = trackable.transform.rotation * Quaternion.AngleAxis(90f, Vector3.left) * quaternion * -camToTargetPose.position + trackable.transform.position;
			Quaternion rotation = trackable.transform.rotation * Quaternion.AngleAxis(90f, Vector3.left) * quaternion;
			if (parentTransformToUpdate == cameraTransform)
			{
				cameraTransform.position = position;
				cameraTransform.rotation = rotation;
				return;
			}
			if (this.mCameraPositioningHelper == null)
			{
				this.mCameraPositioningHelper = new GameObject("CamPosHelper");
			}
			this.mCameraPositioningHelper.transform.position = cameraTransform.position;
			this.mCameraPositioningHelper.transform.rotation = cameraTransform.rotation;
			Transform parent = parentTransformToUpdate.parent;
			parentTransformToUpdate.parent = this.mCameraPositioningHelper.transform;
			this.mCameraPositioningHelper.transform.position = position;
			this.mCameraPositioningHelper.transform.rotation = rotation;
			parentTransformToUpdate.parent = parent;
		}

		public static Vector3 ExtractTranslationFromMatrix(Matrix4x4 matrix)
		{
			Vector3 result;
			result.x = matrix.m03;
			result.y = matrix.m13;
			result.z = matrix.m23;
			return result;
		}

		public static Quaternion ExtractRotationFromMatrix(Matrix4x4 matrix)
		{
			Vector3 vector;
			vector.x = matrix.m02;
			vector.y = matrix.m12;
			vector.z = matrix.m22;
			Vector3 vector2;
			vector2.x = matrix.m01;
			vector2.y = matrix.m11;
			vector2.z = matrix.m21;
			return Quaternion.LookRotation(vector, vector2);
		}

		public static void PositionTrackable(TrackableBehaviour trackableBehaviour, Transform arCameraTransform, VuforiaManagerImpl.PoseData camToTargetPose, double timeStamp)
		{
			TrackableBehaviour.CoordinateSystem coordinateSystem = camToTargetPose.coordinateSystem;
			if (coordinateSystem == TrackableBehaviour.CoordinateSystem.CAMERA)
			{
				trackableBehaviour.transform.position = arCameraTransform.TransformPoint(camToTargetPose.position);
				trackableBehaviour.transform.rotation = arCameraTransform.rotation * camToTargetPose.orientation * Quaternion.AngleAxis(270f, Vector3.left);
				trackableBehaviour.TimeStamp = timeStamp;
				return;
			}
			if (coordinateSystem != TrackableBehaviour.CoordinateSystem.WORLD)
			{
				Debug.LogError("Internal error: Unknown coordinate system for Trackable pose.");
				return;
			}
			trackableBehaviour.transform.position = camToTargetPose.position;
			trackableBehaviour.transform.rotation = camToTargetPose.orientation * Quaternion.AngleAxis(90f, Vector3.left);
			trackableBehaviour.TimeStamp = timeStamp;
		}
	}
}
