using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class VuforiaManagerImpl : VuforiaManager
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct PoseData
		{
			internal Vector3 position;

			internal Quaternion orientation;

			internal int csInteger;

			internal TrackableBehaviour.CoordinateSystem coordinateSystem
			{
				get
				{
					return (TrackableBehaviour.CoordinateSystem)this.csInteger;
				}
				set
				{
					this.csInteger = (int)value;
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct TrackableResultData
		{
			internal VuforiaManagerImpl.PoseData pose;

			internal double timeStamp;

			internal int statusInteger;

			internal int id;

			internal TrackableBehaviour.Status status
			{
				get
				{
					return (TrackableBehaviour.Status)this.statusInteger;
				}
				set
				{
					this.statusInteger = (int)value;
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct VirtualButtonData
		{
			internal int id;

			internal int isPressed;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct Obb2D
		{
			internal Vector2 center;

			internal Vector2 halfExtents;

			internal float rotation;

			internal int unused;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct Obb3D
		{
			internal Vector3 center;

			internal Vector3 halfExtents;

			internal float rotationZ;

			internal int unused;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct WordResultData
		{
			internal VuforiaManagerImpl.PoseData pose;

			internal double timeStamp;

			internal int statusInteger;

			internal int id;

			internal VuforiaManagerImpl.Obb2D orientedBoundingBox;

			internal TrackableBehaviour.Status status
			{
				get
				{
					return (TrackableBehaviour.Status)this.statusInteger;
				}
				set
				{
					this.statusInteger = (int)value;
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct WordData
		{
			internal IntPtr stringValue;

			internal int id;

			internal Vector2 size;

			internal int unused;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct ImageHeaderData
		{
			internal IntPtr data;

			internal int width;

			internal int height;

			internal int stride;

			internal int bufferWidth;

			internal int bufferHeight;

			internal int format;

			internal int reallocate;

			internal int updated;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct MeshData
		{
			internal IntPtr positionsArray;

			internal IntPtr normalsArray;

			internal IntPtr texCoordsArray;

			internal IntPtr triangleIdxArray;

			internal int numVertexValues;

			internal int hasNormals;

			internal int hasTexCoords;

			internal int numTriangleIndices;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct InstanceIdData
		{
			public ulong numericValue;

			public IntPtr buffer;

			internal IntPtr reserved;

			public uint dataLength;

			public int dataType;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct VuMarkTargetData
		{
			public VuforiaManagerImpl.InstanceIdData instanceId;

			public int id;

			public int templateId;

			public Vector3 size;

			public int unused;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct SmartTerrainRevisionData
		{
			internal int id;

			internal int revision;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct SurfaceData
		{
			internal IntPtr meshBoundaryArray;

			internal VuforiaManagerImpl.MeshData meshData;

			internal VuforiaManagerImpl.MeshData navMeshData;

			internal RectangleData boundingBox;

			internal VuforiaManagerImpl.PoseData localPose;

			internal int id;

			internal int parentID;

			internal int numBoundaryIndices;

			internal int revision;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct PropData
		{
			internal VuforiaManagerImpl.MeshData meshData;

			internal int id;

			internal int parentID;

			internal VuforiaManagerImpl.Obb3D boundingBox;

			internal Vector2 localPosition;

			internal VuforiaManagerImpl.PoseData localPose;

			internal int revision;

			internal int unused;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct VuMarkTargetResultData
		{
			internal VuforiaManagerImpl.PoseData pose;

			internal double timeStamp;

			internal int statusInteger;

			internal int targetID;

			internal int resultID;

			internal int unused;

			internal TrackableBehaviour.Status status
			{
				get
				{
					return (TrackableBehaviour.Status)this.statusInteger;
				}
				set
				{
					this.statusInteger = (int)value;
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct FrameState
		{
			internal IntPtr trackableDataArray;

			internal IntPtr vbDataArray;

			internal IntPtr wordResultArray;

			internal IntPtr newWordDataArray;

			internal IntPtr vuMarkResultArray;

			internal IntPtr newVuMarkDataArray;

			internal IntPtr propTrackableDataArray;

			internal IntPtr smartTerrainRevisionsArray;

			internal IntPtr updatedSurfacesArray;

			internal IntPtr updatedPropsArray;

			internal int numTrackableResults;

			internal int numVirtualButtonResults;

			internal int frameIndex;

			internal int numWordResults;

			internal int numNewWords;

			internal int numVuMarkResults;

			internal int numNewVuMarks;

			internal int numPropTrackableResults;

			internal int numSmartTerrainRevisions;

			internal int numUpdatedSurfaces;

			internal int numUpdatedProps;

			internal int deviceTrackableId;
		}

		internal struct AutoRotationState
		{
			internal bool setOnPause;

			internal bool autorotateToPortrait;

			internal bool autorotateToPortraitUpsideDown;

			internal bool autorotateToLandscapeLeft;

			internal bool autorotateToLandscapeRight;
		}

		private VuforiaARController.WorldCenterMode mWorldCenterMode;

		private WorldCenterTrackableBehaviour mWorldCenter;

		private VuMarkAbstractBehaviour mVuMarkWorldCenter;

		private Transform mARCameraTransform;

		private Transform mCentralAnchorPoint;

		private Transform mParentAnchorPoint;

		private VuforiaManagerImpl.TrackableResultData[] mTrackableResultDataArray;

		private VuforiaManagerImpl.WordData[] mWordDataArray;

		private VuforiaManagerImpl.WordResultData[] mWordResultDataArray;

		private VuforiaManagerImpl.VuMarkTargetData[] mVuMarkDataArray;

		private VuforiaManagerImpl.VuMarkTargetResultData[] mVuMarkResultDataArray;

		private LinkedList<VuforiaManager.TrackableIdPair> mTrackableFoundQueue = new LinkedList<VuforiaManager.TrackableIdPair>();

		private IntPtr mImageHeaderData = IntPtr.Zero;

		private int mNumImageHeaders;

		private int mInjectedFrameIdx;

		private IntPtr mLastProcessedFrameStatePtr = IntPtr.Zero;

		private bool mInitialized;

		private bool mPaused;

		private VuforiaManagerImpl.FrameState mFrameState;

		private VuforiaManagerImpl.AutoRotationState mAutoRotationState;

		private bool mVideoBackgroundNeedsRedrawing;

		private int mDiscardStatesForRendering;

		private int mLastFrameIdx = -1;

		private bool mIsSeeThroughDevice;

		public override VuforiaARController.WorldCenterMode WorldCenterMode
		{
			get
			{
				return this.mWorldCenterMode;
			}
			set
			{
				this.mWorldCenterMode = value;
			}
		}

		public override WorldCenterTrackableBehaviour WorldCenter
		{
			get
			{
				return this.mWorldCenter;
			}
			set
			{
				this.mWorldCenter = value;
			}
		}

		public override VuMarkAbstractBehaviour VuMarkWorldCenter
		{
			get
			{
				return this.mVuMarkWorldCenter;
			}
			set
			{
				this.mVuMarkWorldCenter = value;
			}
		}

		public override Transform ARCameraTransform
		{
			get
			{
				return this.mARCameraTransform;
			}
			set
			{
				this.mARCameraTransform = value;
			}
		}

		public override Transform CentralAnchorPoint
		{
			get
			{
				return this.mCentralAnchorPoint;
			}
			set
			{
				this.mCentralAnchorPoint = value;
			}
		}

		public override Transform ParentAnchorPoint
		{
			get
			{
				return this.mParentAnchorPoint;
			}
			set
			{
				this.mParentAnchorPoint = value;
			}
		}

		public override bool Initialized
		{
			get
			{
				return this.mInitialized;
			}
		}

		public override int CurrentFrameIndex
		{
			get
			{
				return this.mFrameState.frameIndex;
			}
		}

		public bool VideoBackgroundTextureSet
		{
			private get;
			set;
		}

		public override bool Init()
		{
			this.mTrackableResultDataArray = new VuforiaManagerImpl.TrackableResultData[0];
			this.mWordDataArray = new VuforiaManagerImpl.WordData[0];
			this.mWordResultDataArray = new VuforiaManagerImpl.WordResultData[0];
			this.mTrackableFoundQueue = new LinkedList<VuforiaManager.TrackableIdPair>();
			this.mImageHeaderData = IntPtr.Zero;
			this.mNumImageHeaders = 0;
			this.mInjectedFrameIdx = 0;
			this.mLastProcessedFrameStatePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VuforiaManagerImpl.FrameState)));
			VuforiaWrapper.Instance.InitFrameState(this.mLastProcessedFrameStatePtr);
			this.InitializeTrackableContainer(0);
			this.mInitialized = true;
			EyewearDevice eyewearDevice = Device.Instance as EyewearDevice;
			this.mIsSeeThroughDevice = (eyewearDevice != null && eyewearDevice.IsSeeThru());
			return true;
		}

		public override void Deinit()
		{
			if (this.mInitialized)
			{
				Marshal.FreeHGlobal(this.mImageHeaderData);
				VuforiaWrapper.Instance.DeinitFrameState(this.mLastProcessedFrameStatePtr);
				Marshal.FreeHGlobal(this.mLastProcessedFrameStatePtr);
				this.mInitialized = false;
				this.mPaused = false;
			}
		}

		internal bool Update(ScreenOrientation counterRotation, bool reapplyOldState)
		{
			bool flag = true;
			if (this.mPaused)
			{
				flag = false;
			}
			else
			{
				if (!VuforiaRuntimeUtilities.IsVuforiaEnabled())
				{
					this.UpdateTrackablesEditor();
					return true;
				}
				this.UpdateImageContainer();
				if (VuforiaRuntimeUtilities.IsPlayMode() && ((CameraDeviceImpl)CameraDevice.Instance).WebCam.DidUpdateThisFrame)
				{
					this.InjectCameraFrame();
				}
				if (TrackerManager.Instance.GetTracker<DeviceTracker>() != null && TrackerManager.Instance.GetTracker<DeviceTracker>().IsActive)
				{
					reapplyOldState = true;
				}
				if (VuforiaWrapper.Instance.UpdateQCAR(this.mImageHeaderData, this.mNumImageHeaders, this.mLastProcessedFrameStatePtr, (int)counterRotation) == 0)
				{
					flag = false;
				}
				else
				{
					if (this.mDiscardStatesForRendering > 0)
					{
						this.mDiscardStatesForRendering--;
					}
					this.mFrameState = (VuforiaManagerImpl.FrameState)Marshal.PtrToStructure(this.mLastProcessedFrameStatePtr, typeof(VuforiaManagerImpl.FrameState));
					if (this.mFrameState.frameIndex < 0)
					{
						flag = false;
					}
				}
			}
			if (flag | reapplyOldState)
			{
				if (this.mARCameraTransform != this.mCentralAnchorPoint)
				{
					this.mARCameraTransform.position = this.mCentralAnchorPoint.position;
					this.mARCameraTransform.rotation = this.mCentralAnchorPoint.rotation;
				}
				this.InitializeTrackableContainer(this.mFrameState.numTrackableResults + this.mFrameState.numPropTrackableResults);
				if (flag)
				{
					this.UpdateCameraFrame();
				}
				this.UpdateTrackers(this.mFrameState);
				if (flag)
				{
					if (VuforiaRuntimeUtilities.IsPlayMode())
					{
						((CameraDeviceImpl)CameraDevice.Instance).WebCam.RenderFrame(this.mFrameState.frameIndex);
					}
					if (this.mFrameState.frameIndex != this.mLastFrameIdx)
					{
						this.mVideoBackgroundNeedsRedrawing = true;
						this.mLastFrameIdx = this.mFrameState.frameIndex;
					}
				}
			}
			return flag;
		}

		internal void StartRendering()
		{
			if (!VuforiaRuntimeUtilities.IsPlayMode() && this.mDiscardStatesForRendering == 0)
			{
				if (this.VideoBackgroundTextureSet && this.mVideoBackgroundNeedsRedrawing && !this.mIsSeeThroughDevice)
				{
					VuforiaRenderer.InternalInstance.UnityRenderEvent(VuforiaRendererImpl.RenderEvent.RENDERER_BEGIN_AND_BIND);
					this.mVideoBackgroundNeedsRedrawing = false;
					return;
				}
				VuforiaRenderer.InternalInstance.UnityRenderEvent(VuforiaRendererImpl.RenderEvent.RENDERER_BEGIN);
			}
		}

		internal void FinishRendering()
		{
			if (!VuforiaRuntimeUtilities.IsPlayMode() && this.mDiscardStatesForRendering == 0)
			{
				VuforiaRenderer.InternalInstance.UnityRenderEvent(VuforiaRendererImpl.RenderEvent.RENDERER_END);
			}
		}

		internal void Pause(bool pause)
		{
			if (pause)
			{
				this.mAutoRotationState = new VuforiaManagerImpl.AutoRotationState
				{
					autorotateToLandscapeLeft = Screen.autorotateToLandscapeLeft,
					autorotateToLandscapeRight = Screen.autorotateToLandscapeRight,
					autorotateToPortrait = Screen.autorotateToPortrait,
					autorotateToPortraitUpsideDown = Screen.autorotateToPortraitUpsideDown,
					setOnPause = true
				};
				Screen.autorotateToLandscapeLeft = false;
				Screen.autorotateToLandscapeRight = false;
				Screen.autorotateToPortrait = false;
				Screen.autorotateToPortraitUpsideDown = false;
			}
			else if (this.mAutoRotationState.setOnPause)
			{
				Screen.autorotateToLandscapeLeft = this.mAutoRotationState.autorotateToLandscapeLeft;
				Screen.autorotateToLandscapeRight = this.mAutoRotationState.autorotateToLandscapeRight;
				Screen.autorotateToPortrait = this.mAutoRotationState.autorotateToPortrait;
				Screen.autorotateToPortraitUpsideDown = this.mAutoRotationState.autorotateToPortraitUpsideDown;
			}
			this.mPaused = pause;
		}

		internal void SetStatesToDiscard()
		{
			this.mDiscardStatesForRendering = 3;
		}

		internal bool IsDiscardingRenderingStates()
		{
			return this.mDiscardStatesForRendering > 0;
		}

		internal static bool IsDetectedOrTracked(TrackableBehaviour.Status status)
		{
			return status == TrackableBehaviour.Status.DETECTED || status == TrackableBehaviour.Status.TRACKED || status == TrackableBehaviour.Status.EXTENDED_TRACKED;
		}

		private void InitializeTrackableContainer(int numTrackableResults)
		{
			if (this.mTrackableResultDataArray.Length != numTrackableResults)
			{
				this.mTrackableResultDataArray = new VuforiaManagerImpl.TrackableResultData[numTrackableResults];
			}
		}

		private void UpdateTrackers(VuforiaManagerImpl.FrameState frameState)
		{
			this.UnmarshalTrackables(frameState);
			this.UnmarshalWordTrackables(frameState);
			this.UnmarshalVuMarkTrackables(frameState);
			this.UpdateTrackableFoundQueue();
			StateManagerImpl stateManagerImpl = (StateManagerImpl)TrackerManager.Instance.GetStateManager();
			this.UpdateSmartTerrain(frameState, stateManagerImpl);
			this.UpdateExtendedTrackedVuMarks();
			stateManagerImpl.UpdateVuMarks(this.mVuMarkDataArray, this.mVuMarkResultDataArray);
			VuforiaManager.TrackableIdPair trackableIdPair = new VuforiaManager.TrackableIdPair
			{
				ResultId = -1,
				TrackableId = -1
			};
			int num = -1;
			if (this.mWorldCenterMode == VuforiaARController.WorldCenterMode.SPECIFIC_TARGET)
			{
				if (this.mVuMarkWorldCenter != null && this.mVuMarkWorldCenter.VuMarkTemplate != null)
				{
					this.mWorldCenter = this.GetVuMarkWorldCenter(this.mVuMarkWorldCenter.VuMarkTemplate.ID);
				}
				if (this.mWorldCenter != null && this.mWorldCenter.Trackable != null)
				{
					if (this.mWorldCenter is VuMarkAbstractBehaviour)
					{
						trackableIdPair.ResultId = ((VuMarkAbstractBehaviour)this.mWorldCenter).VuMarkResultId;
					}
					else
					{
						trackableIdPair.TrackableId = this.mWorldCenter.Trackable.ID;
					}
				}
			}
			else if (this.mWorldCenterMode == VuforiaARController.WorldCenterMode.FIRST_TARGET)
			{
				stateManagerImpl.RemoveDisabledTrackablesFromQueue(ref this.mTrackableFoundQueue);
				if (this.mTrackableFoundQueue.Count > 0)
				{
					trackableIdPair = this.mTrackableFoundQueue.First.Value;
				}
			}
			else if (this.mWorldCenterMode == VuforiaARController.WorldCenterMode.DEVICE_TRACKING)
			{
				num = frameState.deviceTrackableId;
			}
			DeviceTrackingManager deviceTrackingManager = stateManagerImpl.GetDeviceTrackingManager();
			if (num != -1)
			{
				deviceTrackingManager.UpdateCamera(this.mCentralAnchorPoint, this.mTrackableResultDataArray, num);
			}
			else
			{
				RotationalPlayModeDeviceTrackerImpl rotationalPlayModeDeviceTrackerImpl = TrackerManager.Instance.GetTracker<DeviceTracker>() as RotationalPlayModeDeviceTrackerImpl;
				if (rotationalPlayModeDeviceTrackerImpl != null && rotationalPlayModeDeviceTrackerImpl.IsActive)
				{
					VuforiaManagerImpl.TrackableResultData trackable = rotationalPlayModeDeviceTrackerImpl.GetTrackable();
					deviceTrackingManager.UpdateCamera(this.mCentralAnchorPoint, new VuforiaManagerImpl.TrackableResultData[]
					{
						trackable
					}, trackable.id);
				}
				else if (trackableIdPair.TrackableId >= 0 || trackableIdPair.ResultId >= 0)
				{
					VuforiaManagerImpl.TrackableResultData trackableResultData = this.GetTrackableResultData(trackableIdPair, true);
					if (VuforiaManagerImpl.IsDetectedOrTracked(trackableResultData.status))
					{
						stateManagerImpl.UpdateCameraPoseWRTTrackable(this.mCentralAnchorPoint, this.mParentAnchorPoint, trackableIdPair, trackableResultData.pose);
					}
				}
			}
			if (this.mARCameraTransform != this.mCentralAnchorPoint)
			{
				this.mARCameraTransform.position = this.mCentralAnchorPoint.position;
				this.mARCameraTransform.rotation = this.mCentralAnchorPoint.rotation;
			}
			stateManagerImpl.UpdateTrackablePoses(this.mARCameraTransform, this.mTrackableResultDataArray, this.mVuMarkResultDataArray, trackableIdPair, frameState.frameIndex);
			stateManagerImpl.UpdateWords(this.mARCameraTransform, this.mWordDataArray, this.mWordResultDataArray);
			stateManagerImpl.UpdateVirtualButtons(frameState.numVirtualButtonResults, frameState.vbDataArray);
		}

		private void UpdateSmartTerrain(VuforiaManagerImpl.FrameState frameState, StateManagerImpl stateManager)
		{
			SmartTerrainTracker tracker = TrackerManager.Instance.GetTracker<SmartTerrainTracker>();
			if (tracker != null && tracker.SmartTerrainBuilder.GetReconstructions().Any<ReconstructionAbstractBehaviour>())
			{
				VuforiaManagerImpl.SmartTerrainRevisionData[] array = new VuforiaManagerImpl.SmartTerrainRevisionData[frameState.numSmartTerrainRevisions];
				VuforiaManagerImpl.SurfaceData[] array2 = new VuforiaManagerImpl.SurfaceData[frameState.numUpdatedSurfaces];
				VuforiaManagerImpl.PropData[] array3 = new VuforiaManagerImpl.PropData[frameState.numUpdatedProps];
				for (int i = 0; i < frameState.numSmartTerrainRevisions; i++)
				{
					VuforiaManagerImpl.SmartTerrainRevisionData smartTerrainRevisionData = (VuforiaManagerImpl.SmartTerrainRevisionData)Marshal.PtrToStructure(new IntPtr(frameState.smartTerrainRevisionsArray.ToInt64() + (long)(i * Marshal.SizeOf(typeof(VuforiaManagerImpl.SmartTerrainRevisionData)))), typeof(VuforiaManagerImpl.SmartTerrainRevisionData));
					array[i] = smartTerrainRevisionData;
				}
				for (int j = 0; j < frameState.numUpdatedSurfaces; j++)
				{
					VuforiaManagerImpl.SurfaceData surfaceData = (VuforiaManagerImpl.SurfaceData)Marshal.PtrToStructure(new IntPtr(frameState.updatedSurfacesArray.ToInt64() + (long)(j * Marshal.SizeOf(typeof(VuforiaManagerImpl.SurfaceData)))), typeof(VuforiaManagerImpl.SurfaceData));
					array2[j] = surfaceData;
				}
				for (int k = 0; k < frameState.numUpdatedProps; k++)
				{
					VuforiaManagerImpl.PropData propData = (VuforiaManagerImpl.PropData)Marshal.PtrToStructure(new IntPtr(frameState.updatedPropsArray.ToInt64() + (long)(k * Marshal.SizeOf(typeof(VuforiaManagerImpl.PropData)))), typeof(VuforiaManagerImpl.PropData));
					array3[k] = propData;
				}
				((SmartTerrainBuilderImpl)tracker.SmartTerrainBuilder).UpdateSmartTerrainData(array, array2, array3);
			}
		}

		private void UpdateTrackablesEditor()
		{
			TrackableBehaviour[] array = (TrackableBehaviour[])UnityEngine.Object.FindObjectsOfType(typeof(TrackableBehaviour));
			for (int i = 0; i < array.Length; i++)
			{
				TrackableBehaviour trackableBehaviour = array[i];
				if (trackableBehaviour.enabled)
				{
					if (trackableBehaviour is WordAbstractBehaviour)
					{
						WordAbstractBehaviour wordAbstractBehaviour = (WordAbstractBehaviour)trackableBehaviour;
						string text = wordAbstractBehaviour.IsSpecificWordMode ? wordAbstractBehaviour.SpecificWord : "AnyWord";
						wordAbstractBehaviour.InitializeWord(new WordImpl(0, text, new Vector2(500f, 100f)));
					}
					trackableBehaviour.OnTrackerUpdate(TrackableBehaviour.Status.TRACKED);
				}
			}
		}

		private void UnmarshalTrackables(VuforiaManagerImpl.FrameState frameState)
		{
			for (int i = 0; i < frameState.numTrackableResults; i++)
			{
				VuforiaManagerImpl.TrackableResultData trackableResultData = (VuforiaManagerImpl.TrackableResultData)Marshal.PtrToStructure(new IntPtr(frameState.trackableDataArray.ToInt64() + (long)(i * Marshal.SizeOf(typeof(VuforiaManagerImpl.TrackableResultData)))), typeof(VuforiaManagerImpl.TrackableResultData));
				this.mTrackableResultDataArray[i] = trackableResultData;
			}
			for (int j = 0; j < frameState.numPropTrackableResults; j++)
			{
				VuforiaManagerImpl.TrackableResultData trackableResultData2 = (VuforiaManagerImpl.TrackableResultData)Marshal.PtrToStructure(new IntPtr(frameState.propTrackableDataArray.ToInt64() + (long)(j * Marshal.SizeOf(typeof(VuforiaManagerImpl.TrackableResultData)))), typeof(VuforiaManagerImpl.TrackableResultData));
				this.mTrackableResultDataArray[frameState.numTrackableResults + j] = trackableResultData2;
			}
		}

		private void UnmarshalWordTrackables(VuforiaManagerImpl.FrameState frameState)
		{
			this.mWordDataArray = new VuforiaManagerImpl.WordData[frameState.numNewWords];
			for (int i = 0; i < frameState.numNewWords; i++)
			{
				IntPtr ptr = new IntPtr(frameState.newWordDataArray.ToInt64() + (long)(i * Marshal.SizeOf(typeof(VuforiaManagerImpl.WordData))));
				this.mWordDataArray[i] = (VuforiaManagerImpl.WordData)Marshal.PtrToStructure(ptr, typeof(VuforiaManagerImpl.WordData));
			}
			this.mWordResultDataArray = new VuforiaManagerImpl.WordResultData[frameState.numWordResults];
			for (int j = 0; j < frameState.numWordResults; j++)
			{
				IntPtr ptr2 = new IntPtr(frameState.wordResultArray.ToInt64() + (long)(j * Marshal.SizeOf(typeof(VuforiaManagerImpl.WordResultData))));
				this.mWordResultDataArray[j] = (VuforiaManagerImpl.WordResultData)Marshal.PtrToStructure(ptr2, typeof(VuforiaManagerImpl.WordResultData));
			}
		}

		private void UnmarshalVuMarkTrackables(VuforiaManagerImpl.FrameState frameState)
		{
			this.mVuMarkDataArray = new VuforiaManagerImpl.VuMarkTargetData[frameState.numNewVuMarks];
			for (int i = 0; i < frameState.numNewVuMarks; i++)
			{
				IntPtr ptr = new IntPtr(frameState.newVuMarkDataArray.ToInt64() + (long)(i * Marshal.SizeOf(typeof(VuforiaManagerImpl.VuMarkTargetData))));
				this.mVuMarkDataArray[i] = (VuforiaManagerImpl.VuMarkTargetData)Marshal.PtrToStructure(ptr, typeof(VuforiaManagerImpl.VuMarkTargetData));
			}
			this.mVuMarkResultDataArray = new VuforiaManagerImpl.VuMarkTargetResultData[frameState.numVuMarkResults];
			for (int j = 0; j < frameState.numVuMarkResults; j++)
			{
				IntPtr ptr2 = new IntPtr(frameState.vuMarkResultArray.ToInt64() + (long)(j * Marshal.SizeOf(typeof(VuforiaManagerImpl.VuMarkTargetResultData))));
				this.mVuMarkResultDataArray[j] = (VuforiaManagerImpl.VuMarkTargetResultData)Marshal.PtrToStructure(ptr2, typeof(VuforiaManagerImpl.VuMarkTargetResultData));
			}
		}

		private void UpdateImageContainer()
		{
			CameraDeviceImpl cameraDeviceImpl = (CameraDeviceImpl)CameraDevice.Instance;
			if (this.mNumImageHeaders != cameraDeviceImpl.GetAllImages().Count || (cameraDeviceImpl.GetAllImages().Count > 0 && this.mImageHeaderData == IntPtr.Zero))
			{
				this.mNumImageHeaders = cameraDeviceImpl.GetAllImages().Count;
				Marshal.FreeHGlobal(this.mImageHeaderData);
				this.mImageHeaderData = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(VuforiaManagerImpl.ImageHeaderData)) * this.mNumImageHeaders);
			}
			int num = 0;
			using (Dictionary<Image.PIXEL_FORMAT, Image>.ValueCollection.Enumerator enumerator = cameraDeviceImpl.GetAllImages().Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ImageImpl imageImpl = (ImageImpl)enumerator.Current;
					IntPtr ptr = new IntPtr(this.mImageHeaderData.ToInt64() + (long)(num * Marshal.SizeOf(typeof(VuforiaManagerImpl.ImageHeaderData))));
					Marshal.StructureToPtr(new VuforiaManagerImpl.ImageHeaderData
					{
						width = imageImpl.Width,
						height = imageImpl.Height,
						stride = imageImpl.Stride,
						bufferWidth = imageImpl.BufferWidth,
						bufferHeight = imageImpl.BufferHeight,
						format = (int)imageImpl.PixelFormat,
						reallocate = 0,
						updated = 0,
						data = imageImpl.UnmanagedData
					}, ptr, false);
					num++;
				}
			}
		}

		private void UpdateCameraFrame()
		{
			int num = 0;
			using (Dictionary<Image.PIXEL_FORMAT, Image>.ValueCollection.Enumerator enumerator = ((CameraDeviceImpl)CameraDevice.Instance).GetAllImages().Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ImageImpl imageImpl = (ImageImpl)enumerator.Current;
					VuforiaManagerImpl.ImageHeaderData imageHeaderData = (VuforiaManagerImpl.ImageHeaderData)Marshal.PtrToStructure(new IntPtr(this.mImageHeaderData.ToInt64() + (long)(num * Marshal.SizeOf(typeof(VuforiaManagerImpl.ImageHeaderData)))), typeof(VuforiaManagerImpl.ImageHeaderData));
					imageImpl.Width = imageHeaderData.width;
					imageImpl.Height = imageHeaderData.height;
					imageImpl.Stride = imageHeaderData.stride;
					imageImpl.BufferWidth = imageHeaderData.bufferWidth;
					imageImpl.BufferHeight = imageHeaderData.bufferHeight;
					imageImpl.PixelFormat = (Image.PIXEL_FORMAT)imageHeaderData.format;
					if (imageHeaderData.reallocate == 1)
					{
						imageImpl.Pixels = new byte[VuforiaWrapper.Instance.QcarGetBufferSize(imageImpl.BufferWidth, imageImpl.BufferHeight, (int)imageImpl.PixelFormat)];
						Marshal.FreeHGlobal(imageImpl.UnmanagedData);
						imageImpl.UnmanagedData = Marshal.AllocHGlobal(VuforiaWrapper.Instance.QcarGetBufferSize(imageImpl.BufferWidth, imageImpl.BufferHeight, (int)imageImpl.PixelFormat));
					}
					else if (imageHeaderData.updated == 1)
					{
						imageImpl.CopyPixelsFromUnmanagedBuffer();
					}
					num++;
				}
			}
		}

		private void InjectCameraFrame()
		{
			CameraDeviceImpl cameraDeviceImpl = (CameraDeviceImpl)CameraDevice.Instance;
			GCHandle gCHandle = GCHandle.Alloc(cameraDeviceImpl.WebCam.GetPixels32AndBufferFrame(), GCHandleType.Pinned);
			IntPtr pixels = gCHandle.AddrOfPinnedObject();
			int actualWidth = cameraDeviceImpl.WebCam.ActualWidth;
			int actualHeight = cameraDeviceImpl.WebCam.ActualHeight;
			VuforiaWrapper.Instance.QcarAddCameraFrame(pixels, actualWidth, actualHeight, 16, 4 * actualWidth, this.mInjectedFrameIdx, cameraDeviceImpl.WebCam.FlipHorizontally ? 1 : 0);
			this.mInjectedFrameIdx++;
			pixels = IntPtr.Zero;
			gCHandle.Free();
		}

		private void UpdateTrackableFoundQueue()
		{
			VuforiaManagerImpl.TrackableResultData[] array = this.mTrackableResultDataArray;
			for (int i = 0; i < array.Length; i++)
			{
				VuforiaManagerImpl.TrackableResultData trackableResultData = array[i];
				this.UpdateTrackableFoundQueue(trackableResultData.status, VuforiaManager.TrackableIdPair.FromTrackableId(trackableResultData.id));
			}
			VuforiaManagerImpl.VuMarkTargetResultData[] array2 = this.mVuMarkResultDataArray;
			for (int i = 0; i < array2.Length; i++)
			{
				VuforiaManagerImpl.VuMarkTargetResultData vuMarkTargetResultData = array2[i];
				this.UpdateTrackableFoundQueue(vuMarkTargetResultData.status, VuforiaManager.TrackableIdPair.FromResultId(vuMarkTargetResultData.resultID));
			}
			using (List<VuforiaManager.TrackableIdPair>.Enumerator enumerator = new List<VuforiaManager.TrackableIdPair>(this.mTrackableFoundQueue).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					VuforiaManager.TrackableIdPair id = enumerator.Current;
					if (Array.Exists<VuforiaManagerImpl.TrackableResultData>(this.mTrackableResultDataArray, (VuforiaManagerImpl.TrackableResultData tr) => tr.id == id.TrackableId))
					{
						break;
					}
					if (Array.Exists<VuforiaManagerImpl.VuMarkTargetResultData>(this.mVuMarkResultDataArray, (VuforiaManagerImpl.VuMarkTargetResultData tr) => tr.resultID == id.ResultId))
					{
						break;
					}
					this.mTrackableFoundQueue.Remove(id);
				}
			}
		}

		private void UpdateTrackableFoundQueue(TrackableBehaviour.Status status, VuforiaManager.TrackableIdPair trackableId)
		{
			if (VuforiaManagerImpl.IsDetectedOrTracked(status))
			{
				if (!this.mTrackableFoundQueue.Contains(trackableId))
				{
					this.mTrackableFoundQueue.AddLast(trackableId);
					return;
				}
			}
			else if (this.mTrackableFoundQueue.Contains(trackableId))
			{
				this.mTrackableFoundQueue.Remove(trackableId);
			}
		}

		private VuforiaManagerImpl.TrackableResultData GetTrackableResultData(VuforiaManager.TrackableIdPair trackableId, bool includeVuMarks)
		{
			VuforiaManagerImpl.TrackableResultData[] array = this.mTrackableResultDataArray;
			for (int i = 0; i < array.Length; i++)
			{
				VuforiaManagerImpl.TrackableResultData trackableResultData = array[i];
				if (trackableResultData.id == trackableId.TrackableId)
				{
					return trackableResultData;
				}
			}
			VuforiaManagerImpl.TrackableResultData result;
			result.id = -1;
			result.pose = default(VuforiaManagerImpl.PoseData);
			result.statusInteger = 1;
			result.timeStamp = 0.0;
			if (includeVuMarks)
			{
				VuforiaManagerImpl.VuMarkTargetResultData[] array2 = this.mVuMarkResultDataArray;
				for (int i = 0; i < array2.Length; i++)
				{
					VuforiaManagerImpl.VuMarkTargetResultData vuMarkTargetResultData = array2[i];
					if (vuMarkTargetResultData.resultID == trackableId.ResultId)
					{
						result.id = vuMarkTargetResultData.resultID;
						result.statusInteger = vuMarkTargetResultData.statusInteger;
						result.pose = vuMarkTargetResultData.pose;
						result.timeStamp = vuMarkTargetResultData.timeStamp;
						break;
					}
				}
			}
			return result;
		}

		private VuMarkAbstractBehaviour GetVuMarkWorldCenter(int vuMarkTemplateId)
		{
			List<VuMarkAbstractBehaviour> list = ((VuMarkManagerImpl)TrackerManager.Instance.GetStateManager().GetVuMarkManager()).GetActiveBehaviours().ToList<VuMarkAbstractBehaviour>();
			List<int> list2 = new List<int>();
			List<int> list3 = new List<int>();
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].VuMarkTemplate.ID == vuMarkTemplateId)
				{
					list2.Add(i);
					list3.Add(list[i].VuMarkResultId);
				}
			}
			foreach (VuforiaManager.TrackableIdPair current in this.mTrackableFoundQueue)
			{
				int num = list3.IndexOf(current.ResultId);
				if (num >= 0)
				{
					return list[list2[num]];
				}
			}
			return null;
		}

		private void UpdateExtendedTrackedVuMarks()
		{
			StateManagerImpl expr_0F = (StateManagerImpl)TrackerManager.Instance.GetStateManager();
			VuforiaManager.TrackableIdPair[] array = expr_0F.GetExtendedTrackingManager().GetExtendedTrackedBehaviours().ToArray<VuforiaManager.TrackableIdPair>();
			VuMarkManagerImpl vuMarkManagerImpl = (VuMarkManagerImpl)expr_0F.GetVuMarkManager();
			List<int> list = new List<int>();
			for (int i = 0; i < this.mVuMarkResultDataArray.Length; i++)
			{
				list.Add(this.mVuMarkResultDataArray[i].resultID);
			}
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			VuforiaManager.TrackableIdPair[] array2 = array;
			for (int j = 0; j < array2.Length; j++)
			{
				int resultId = array2[j].ResultId;
				VuMarkAbstractBehaviour behaviourWithResultID = vuMarkManagerImpl.GetBehaviourWithResultID(resultId);
				if (behaviourWithResultID != null)
				{
					int iD = behaviourWithResultID.VuMarkTarget.ID;
					if (!list.Contains(resultId))
					{
						if (!dictionary.ContainsKey(iD))
						{
							dictionary[iD] = new List<int>();
						}
						dictionary[iD].Add(resultId);
					}
					else
					{
						list.Remove(resultId);
					}
				}
			}
			for (int k = 0; k < this.mVuMarkResultDataArray.Length; k++)
			{
				int targetID = this.mVuMarkResultDataArray[k].targetID;
				int resultID = this.mVuMarkResultDataArray[k].resultID;
				if (list.Contains(resultID) && dictionary.ContainsKey(targetID))
				{
					List<int> list2 = dictionary[targetID];
					if (list2.Count > 0)
					{
						int num = list2[list2.Count - 1];
						list2.RemoveAt(list2.Count - 1);
						Debug.Log(string.Concat(new object[]
						{
							"CHANGE RESULT ID ",
							this.mVuMarkResultDataArray[k].resultID,
							" => ",
							num
						}));
						this.mVuMarkResultDataArray[k].resultID = num;
					}
				}
			}
			List<VuforiaManagerImpl.VuMarkTargetResultData> list3 = this.mVuMarkResultDataArray.ToList<VuforiaManagerImpl.VuMarkTargetResultData>();
			array2 = array;
			for (int j = 0; j < array2.Length; j++)
			{
				VuforiaManager.TrackableIdPair trackableIdPair = array2[j];
				if (trackableIdPair.ResultId >= 0)
				{
					bool flag = false;
					VuforiaManagerImpl.VuMarkTargetResultData[] array3 = this.mVuMarkResultDataArray;
					for (int l = 0; l < array3.Length; l++)
					{
						if (array3[l].resultID == trackableIdPair.ResultId)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						VuMarkAbstractBehaviour behaviourWithResultID2 = vuMarkManagerImpl.GetBehaviourWithResultID(trackableIdPair.ResultId);
						list3.Add(new VuforiaManagerImpl.VuMarkTargetResultData
						{
							pose = new VuforiaManagerImpl.PoseData
							{
								coordinateSystem = TrackableBehaviour.CoordinateSystem.WORLD,
								position = behaviourWithResultID2.transform.position,
								orientation = behaviourWithResultID2.transform.rotation * Quaternion.AngleAxis(90f, Vector3.right)
							},
							resultID = trackableIdPair.ResultId,
							targetID = behaviourWithResultID2.VuMarkTarget.ID,
							status = TrackableBehaviour.Status.EXTENDED_TRACKED
						});
					}
				}
			}
			this.mVuMarkResultDataArray = list3.ToArray();
			HashSet<int> hashSet = new HashSet<int>();
			array2 = array;
			for (int j = 0; j < array2.Length; j++)
			{
				VuforiaManager.TrackableIdPair trackableIdPair2 = array2[j];
				if (trackableIdPair2.ResultId >= 0)
				{
					VuMarkAbstractBehaviour behaviourWithResultID3 = vuMarkManagerImpl.GetBehaviourWithResultID(trackableIdPair2.ResultId);
					if (behaviourWithResultID3 != null && behaviourWithResultID3.VuMarkTarget != null)
					{
						hashSet.Add(behaviourWithResultID3.VuMarkTarget.ID);
					}
				}
			}
			List<VuforiaManagerImpl.VuMarkTargetData> list4 = new List<VuforiaManagerImpl.VuMarkTargetData>();
			for (int m = 0; m < this.mVuMarkDataArray.Length; m++)
			{
				int id = this.mVuMarkDataArray[m].id;
				if (!hashSet.Contains(id))
				{
					list4.Add(this.mVuMarkDataArray[m]);
				}
			}
			this.mVuMarkDataArray = list4.ToArray();
		}
	}
}
