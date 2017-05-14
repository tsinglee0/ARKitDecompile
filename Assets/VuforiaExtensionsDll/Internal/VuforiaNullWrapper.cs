using System;
using System.Text;

namespace Vuforia
{
	internal class VuforiaNullWrapper : IVuforiaWrapper
	{
		public int CameraDeviceDeinitCamera()
		{
			return 0;
		}

		public int CameraDeviceGetCameraDirection()
		{
			return 0;
		}

		public void CameraDeviceGetCameraField(IntPtr cameraField, int idx)
		{
		}

		public int CameraDeviceGetCameraFieldOfViewRads(IntPtr fovVectorContainer)
		{
			return 0;
		}

		public int CameraDeviceGetFieldBool(string key, IntPtr value)
		{
			return 0;
		}

		public int CameraDeviceGetFieldFloat(string key, IntPtr value)
		{
			return 0;
		}

		public int CameraDeviceGetFieldInt64(string key, IntPtr value)
		{
			return 0;
		}

		public int CameraDeviceGetFieldInt64Range(string key, IntPtr intRange)
		{
			return 0;
		}

		public int CameraDeviceGetFieldString(string key, StringBuilder value, int maxLength)
		{
			return 0;
		}

		public int CameraDeviceGetNumCameraFields()
		{
			return 0;
		}

		public int CameraDeviceGetNumVideoModes()
		{
			return 0;
		}

		public void CameraDeviceGetVideoMode(int idx, IntPtr videoMode)
		{
		}

		public int CameraDeviceInitCamera(int camera)
		{
			return 0;
		}

		public int CameraDeviceSelectVideoMode(int idx)
		{
			return 0;
		}

		public void CameraDeviceSetCameraConfiguration(int width, int height)
		{
		}

		public int CameraDeviceSetFieldBool(string key, bool value)
		{
			return 0;
		}

		public int CameraDeviceSetFieldFloat(string key, float value)
		{
			return 0;
		}

		public int CameraDeviceSetFieldInt64(string key, long value)
		{
			return 0;
		}

		public int CameraDeviceSetFieldInt64Range(string key, long intRangeFrom, long intRangeTo)
		{
			return 0;
		}

		public int CameraDeviceSetFieldString(string key, string value)
		{
			return 0;
		}

		public int CameraDeviceSetFlashTorchMode(int on)
		{
			return 0;
		}

		public int CameraDeviceSetFocusMode(int focusMode)
		{
			return 0;
		}

		public int CameraDeviceStartCamera()
		{
			return 0;
		}

		public int CameraDeviceStopCamera()
		{
			return 0;
		}

		public void CustomViewerParameters_AddDistortionCoefficient(IntPtr obj, float val)
		{
		}

		public void CustomViewerParameters_ClearDistortionCoefficients(IntPtr obj)
		{
		}

		public void CustomViewerParameters_delete(IntPtr obj)
		{
		}

		public IntPtr CustomViewerParameters_new(float version, string name, string manufacturer)
		{
			return IntPtr.Zero;
		}

		public void CustomViewerParameters_SetButtonType(IntPtr obj, int val)
		{
		}

		public void CustomViewerParameters_SetContainsMagnet(IntPtr obj, bool val)
		{
		}

		public void CustomViewerParameters_SetFieldOfView(IntPtr obj, IntPtr val)
		{
		}

		public void CustomViewerParameters_SetInterLensDistance(IntPtr obj, float val)
		{
		}

		public void CustomViewerParameters_SetLensCentreToTrayDistance(IntPtr obj, float val)
		{
		}

		public void CustomViewerParameters_SetScreenToLensDistance(IntPtr obj, float val)
		{
		}

		public void CustomViewerParameters_SetTrayAlignment(IntPtr obj, int val)
		{
		}

		public int CylinderTargetGetDimensions(IntPtr dataSetPtr, string trackableName, IntPtr dimensionPtr)
		{
			return 0;
		}

		public int CylinderTargetSetBottomDiameter(IntPtr dataSetPtr, string trackableName, float bottomDiameter)
		{
			return 0;
		}

		public int CylinderTargetSetSideLength(IntPtr dataSetPtr, string trackableName, float sideLength)
		{
			return 0;
		}

		public int CylinderTargetSetTopDiameter(IntPtr dataSetPtr, string trackableName, float topDiameter)
		{
			return 0;
		}

		public int DataSetCreateTrackable(IntPtr dataSetPtr, IntPtr trackableSourcePtr, StringBuilder trackableName, int nameMaxLength, IntPtr trackableData)
		{
			return 0;
		}

		public int DataSetDestroyTrackable(IntPtr dataSetPtr, int trackableId)
		{
			return 0;
		}

		public int DataSetExists(string relativePath, int storageType)
		{
			return 0;
		}

		public int DataSetGetNumTrackableType(int trackableType, IntPtr dataSetPtr)
		{
			return 0;
		}

		public int DataSetGetTrackableName(IntPtr dataSetPtr, int trackableId, StringBuilder trackableName, int nameMaxLength)
		{
			return 0;
		}

		public int DataSetGetTrackablesOfType(int trackableType, IntPtr trackableDataArray, int trackableDataArrayLength, IntPtr dataSetPtr)
		{
			return 0;
		}

		public int DataSetHasReachedTrackableLimit(IntPtr dataSetPtr)
		{
			return 0;
		}

		public int DataSetLoad(string relativePath, int storageType, IntPtr dataSetPtr)
		{
			return 0;
		}

		public void DeinitFrameState(IntPtr frameState)
		{
		}

		public int Device_GetMode()
		{
			return 0;
		}

		public IntPtr Device_GetSelectedViewer()
		{
			return IntPtr.Zero;
		}

		public IntPtr Device_GetViewerList()
		{
			return IntPtr.Zero;
		}

		public int Device_IsViewerPresent()
		{
			return 0;
		}

		public int Device_SelectViewer(IntPtr vp)
		{
			return 0;
		}

		public int Device_SetMode(int mode)
		{
			return 0;
		}

		public void Device_SetViewerPresent(bool present)
		{
		}

		public int DeviceIsEyewearDevice()
		{
			return 0;
		}

		public int EyewearCPMClearProfile(int profileID)
		{
			return 0;
		}

		public int EyewearCPMGetActiveProfile()
		{
			return 0;
		}

		public int EyewearCPMGetCameraToEyePose(int profileID, int eyeID, IntPtr matrix)
		{
			return 0;
		}

		public int EyewearCPMGetEyeProjection(int profileID, int eyeID, IntPtr matrix)
		{
			return 0;
		}

		public int EyewearCPMGetMaxCount()
		{
			return 0;
		}

		public IntPtr EyewearCPMGetProfileName(int profileID)
		{
			return IntPtr.Zero;
		}

		public int EyewearCPMGetUsedCount()
		{
			return 0;
		}

		public int EyewearCPMIsProfileUsed(int profileID)
		{
			return 0;
		}

		public int EyewearCPMSetActiveProfile(int profileID)
		{
			return 0;
		}

		public int EyewearCPMSetCameraToEyePose(int profileID, int eyeID, IntPtr matrix)
		{
			return 0;
		}

		public int EyewearCPMSetEyeProjection(int profileID, int eyeID, IntPtr matrix)
		{
			return 0;
		}

		public int EyewearCPMSetProfileName(int profileID, IntPtr name)
		{
			return 0;
		}

		public int EyewearDeviceGetScreenOrientation()
		{
			return 0;
		}

		public int EyewearDeviceIsDisplayExtended()
		{
			return 0;
		}

		public int EyewearDeviceIsDisplayExtendedGLOnly()
		{
			return 0;
		}

		public int EyewearDeviceIsDualDisplay()
		{
			return 0;
		}

		public int EyewearDeviceIsPredictiveTrackingEnabled()
		{
			return 0;
		}

		public int EyewearDeviceIsSeeThru()
		{
			return 0;
		}

		public int EyewearDeviceSetDisplayExtended(bool enable)
		{
			return 0;
		}

		public int EyewearDeviceSetPredictiveTracking(bool enable)
		{
			return 0;
		}

		public float EyewearUserCalibratorGetMaxScaleHint()
		{
			return 0f;
		}

		public float EyewearUserCalibratorGetMinScaleHint()
		{
			return 0f;
		}

		public int EyewearUserCalibratorGetProjectionMatrix(IntPtr readingsDataArray, int numReadings, IntPtr cameraToEyeContainer, IntPtr projectionContainer)
		{
			return 0;
		}

		public int EyewearUserCalibratorInit(int surfaceWidth, int surfaceHeight, int targetWidth, int targetHeight)
		{
			return 0;
		}

		public int EyewearUserCalibratorIsStereoStretched()
		{
			return 0;
		}

		public void FrameCounterGetBenchmarkingData(IntPtr benchmarkingData)
		{
		}

		public int GetCameraThreadID()
		{
			return 0;
		}

		public int GetProjectionGL(float nearPlane, float farPlane, IntPtr projectionContainer, int screenOrientation)
		{
			return 0;
		}

		public void GetVuforiaLibraryVersion(StringBuilder value, int maxLength)
		{
		}

		public int HasSurfaceBeenRecreated()
		{
			return 0;
		}

		public int ImageTargetBuilderBuild(string name, float screenSizeWidth)
		{
			return 0;
		}

		public int ImageTargetBuilderGetFrameQuality()
		{
			return 0;
		}

		public IntPtr ImageTargetBuilderGetTrackableSource()
		{
			return IntPtr.Zero;
		}

		public void ImageTargetBuilderStartScan()
		{
		}

		public void ImageTargetBuilderStopScan()
		{
		}

		public int ImageTargetCreateVirtualButton(IntPtr dataSetPtr, string trackableName, string virtualButtonName, IntPtr rectData)
		{
			return 0;
		}

		public int ImageTargetDestroyVirtualButton(IntPtr dataSetPtr, string trackableName, string virtualButtonName)
		{
			return 0;
		}

		public int ImageTargetGetNumVirtualButtons(IntPtr dataSetPtr, string trackableName)
		{
			return 0;
		}

		public int ImageTargetGetVirtualButtonName(IntPtr dataSetPtr, string trackableName, int idx, StringBuilder vbName, int nameMaxLength)
		{
			return 0;
		}

		public int ImageTargetGetVirtualButtons(IntPtr virtualButtonDataArray, IntPtr rectangleDataArray, int virtualButtonDataArrayLength, IntPtr dataSetPtr, string trackableName)
		{
			return 0;
		}

		public void InitFrameState(IntPtr frameState)
		{
		}

		public void InitPlatformNative()
		{
		}

		public float MultiTargetGetLargestSizeComponent(IntPtr dataSetPtr, string trackableName)
		{
			return 0f;
		}

		public int ObjectTargetGetSize(IntPtr dataSetPtr, string trackableName, IntPtr sizePtr)
		{
			return 0;
		}

		public int ObjectTargetSetSize(IntPtr dataSetPtr, string trackableName, IntPtr sizePtr)
		{
			return 0;
		}

		public int ObjectTrackerActivateDataSet(IntPtr dataSetPtr)
		{
			return 0;
		}

		public IntPtr ObjectTrackerCreateDataSet()
		{
			return IntPtr.Zero;
		}

		public int ObjectTrackerDeactivateDataSet(IntPtr dataSetPtr)
		{
			return 0;
		}

		public int ObjectTrackerDestroyDataSet(IntPtr dataSetPtr)
		{
			return 0;
		}

		public int ObjectTrackerPersistExtendedTracking(int on)
		{
			return 0;
		}

		public int ObjectTrackerResetExtendedTracking()
		{
			return 0;
		}

		public void OnPause()
		{
		}

		public void OnResume()
		{
		}

		public void OnSurfaceChanged(int width, int height)
		{
		}

		public void QcarAddCameraFrame(IntPtr pixels, int width, int height, int format, int stride, int frameIdx, int flipHorizontally)
		{
		}

		public void QcarDeinit()
		{
		}

		public int QcarGetBufferSize(int width, int height, int format)
		{
			return 0;
		}

		public int QcarInit(string licenseKey)
		{
			return 0;
		}

		public int QcarSetFrameFormat(int format, int enabled)
		{
			return 0;
		}

		public int QcarSetHint(uint hint, int value)
		{
			return 0;
		}

		public int ReconstructioFromEnvironmentGetReconstructionState(IntPtr reconstruction)
		{
			return 0;
		}

		public int ReconstructionFromTargetSetInitializationTarget(IntPtr reconstruction, IntPtr dataSetPtr, int trackableID, IntPtr occluderMin, IntPtr occluderMax, IntPtr offsetToOccluder, IntPtr rotationAxisToOccluder, float rotationAngleToOccluder)
		{
			return 0;
		}

		public int ReconstructionIsReconstructing(IntPtr reconstruction)
		{
			return 0;
		}

		public int ReconstructionReset(IntPtr reconstruction)
		{
			return 0;
		}

		public int ReconstructionSetMaximumArea(IntPtr reconstruction, IntPtr maximumArea)
		{
			return 0;
		}

		public void ReconstructionSetNavMeshPadding(IntPtr reconstruction, float padding)
		{
		}

		public int ReconstructionStart(IntPtr reconstruction)
		{
			return 0;
		}

		public int ReconstructionStop(IntPtr reconstruction)
		{
			return 0;
		}

		public IntPtr RendererCreateNativeTexture(uint width, uint height, int format)
		{
			return IntPtr.Zero;
		}

		public void RendererEnd()
		{
		}

		public int RendererGetGraphicsAPI()
		{
			return 0;
		}

		public int RendererGetRecommendedFps(int flags)
		{
			return 0;
		}

		public void RendererGetVideoBackgroundCfg(IntPtr bgCfg)
		{
		}

		public int RendererGetVideoBackgroundTextureInfo(IntPtr texInfo)
		{
			return 0;
		}

		public int RendererIsVideoBackgroundTextureInfoAvailable()
		{
			return 0;
		}

		public void RendererSetVideoBackgroundCfg(IntPtr bgCfg)
		{
		}

		public int RendererSetVideoBackgroundTextureID(int textureID)
		{
			return 0;
		}

		public int RendererSetVideoBackgroundTexturePtr(IntPtr texturePtr)
		{
			return 0;
		}

		public void RenderingPrimitives_DeleteCopy()
		{
		}

		public void RenderingPrimitives_GetDistortionMesh(int viewId, IntPtr meshData)
		{
		}

		public void RenderingPrimitives_GetDistortionMeshSize(int viewId, IntPtr size)
		{
		}

		public void RenderingPrimitives_GetDistortionTextureViewport(int viewID, IntPtr viewportContainer)
		{
		}

		public void RenderingPrimitives_GetEffectiveFov(int viewID, IntPtr fovContainer)
		{
		}

		public void RenderingPrimitives_GetEyeDisplayAdjustmentMatrix(int viewID, IntPtr matrixContainer)
		{
		}

		public void RenderingPrimitives_GetNormalizedViewport(int viewID, IntPtr viewportContainer)
		{
		}

		public void RenderingPrimitives_GetProjectionMatrix(int viewID, float near, float far, IntPtr projectionContainer, int screenOrientation)
		{
		}

		public void RenderingPrimitives_GetViewport(int viewID, IntPtr viewportContainer)
		{
		}

		public void RenderingPrimitives_GetViewportCentreToEyeAxis(int viewID, IntPtr vectorContainer)
		{
		}

		public void RenderingPrimitives_UpdateCopy()
		{
		}

		public int RotationalDeviceTracker_GetModelCorrectionMode()
		{
			return 0;
		}

		public void RotationalDeviceTracker_GetModelCorrectionTransform(IntPtr pivot)
		{
		}

		public int RotationalDeviceTracker_GetPosePrediction()
		{
			return 0;
		}

		public int RotationalDeviceTracker_Recenter()
		{
			return 0;
		}

		public int RotationalDeviceTracker_SetModelCorrectionMode(int mode)
		{
			return 0;
		}

		public int RotationalDeviceTracker_SetModelCorrectionModeWithTransform(int mode, IntPtr pivot)
		{
			return 0;
		}

		public int RotationalDeviceTracker_SetPosePrediction(bool mode)
		{
			return 0;
		}

		public void SetApplicationEnvironment(int unityVersionMajor, int unityVersionMinor, int unityVersionChange, int sdkWrapperType)
		{
		}

		public int SetHolographicAppCS(IntPtr appSpecifiedCS)
		{
			return 0;
		}

		public void SetRenderBuffers(IntPtr colorBuffer)
		{
		}

		public void SetStateBufferSize(int size)
		{
		}

		public int SmartTerrainBuilderAddReconstruction(IntPtr reconstruction)
		{
			return 0;
		}

		public IntPtr SmartTerrainBuilderCreateReconstructionFromEnvironment()
		{
			return IntPtr.Zero;
		}

		public IntPtr SmartTerrainBuilderCreateReconstructionFromTarget()
		{
			return IntPtr.Zero;
		}

		public int SmartTerrainBuilderDestroyReconstruction(IntPtr reconstruction)
		{
			return 0;
		}

		public int SmartTerrainBuilderRemoveReconstruction(IntPtr reconstruction)
		{
			return 0;
		}

		public int SmartTerrainTrackerDeinitBuilder()
		{
			return 0;
		}

		public int SmartTerrainTrackerInitBuilder()
		{
			return 0;
		}

		public int SmartTerrainTrackerSetScaleToMillimeter(float scaleToMillimenters)
		{
			return 0;
		}

		public int StartExtendedTracking(IntPtr dataSetPtr, int trackableID)
		{
			return 0;
		}

		public int StopExtendedTracking(IntPtr dataSetPtr, int trackableID)
		{
			return 0;
		}

		public void TargetFinderClearTrackables()
		{
		}

		public int TargetFinderDeinit()
		{
			return 0;
		}

		public int TargetFinderEnableTracking(IntPtr searchResult, IntPtr trackableData)
		{
			return 0;
		}

		public void TargetFinderGetImageTargets(IntPtr trackableIdArray, int trackableIdArrayLength)
		{
		}

		public int TargetFinderGetInitState()
		{
			return 0;
		}

		public int TargetFinderGetResults(IntPtr searchResultArray, int searchResultArrayLength)
		{
			return 0;
		}

		public int TargetFinderStartInit(string userKey, string secretKey)
		{
			return 0;
		}

		public int TargetFinderStartRecognition()
		{
			return 0;
		}

		public int TargetFinderStop()
		{
			return 0;
		}

		public void TargetFinderUpdate(IntPtr targetFinderState, int filterMode)
		{
		}

		public void TextTrackerGetRegionOfInterest(IntPtr detectionROI, IntPtr trackingROI)
		{
		}

		public int TextTrackerSetRegionOfInterest(int detectionLeftTopX, int detectionLeftTopY, int detectionRightBottomX, int detectionRightBottomY, int trackingLeftTopX, int trackingLeftTopY, int trackingRightBottomX, int trackingRightBottomY, int upDirection)
		{
			return 0;
		}

		public int TrackerManagerDeinitTracker(int trackerTypeID)
		{
			return 0;
		}

		public int TrackerManagerInitTracker(int trackerTypeID)
		{
			return 0;
		}

		public int TrackerStart(int trackerTypeID)
		{
			return 0;
		}

		public void TrackerStop(int trackerTypeID)
		{
		}

		public int UpdateQCAR(IntPtr imageHeaderDataArray, int imageHeaderArrayLength, IntPtr frameState, int screenOrientation)
		{
			return 0;
		}

		public int ViewerParameters_ContainsMagnet(IntPtr obj)
		{
			return 0;
		}

		public IntPtr ViewerParameters_copy(IntPtr obj)
		{
			return IntPtr.Zero;
		}

		public void ViewerParameters_delete(IntPtr obj)
		{
		}

		public int ViewerParameters_GetButtonType(IntPtr obj)
		{
			return 0;
		}

		public float ViewerParameters_GetDistortionCoefficient(IntPtr obj, int idx)
		{
			return 0f;
		}

		public void ViewerParameters_GetFieldOfView(IntPtr obj, IntPtr result)
		{
		}

		public float ViewerParameters_GetInterLensDistance(IntPtr obj)
		{
			return 0f;
		}

		public float ViewerParameters_GetLensCentreToTrayDistance(IntPtr obj)
		{
			return 0f;
		}

		public IntPtr ViewerParameters_GetManufacturer(IntPtr obj)
		{
			return IntPtr.Zero;
		}

		public IntPtr ViewerParameters_GetName(IntPtr obj)
		{
			return IntPtr.Zero;
		}

		public int ViewerParameters_GetNumDistortionCoefficients(IntPtr obj)
		{
			return 0;
		}

		public float ViewerParameters_GetScreenToLensDistance(IntPtr obj)
		{
			return 0f;
		}

		public int ViewerParameters_GetTrayAlignment(IntPtr obj)
		{
			return 0;
		}

		public float ViewerParameters_GetVersion(IntPtr obj)
		{
			return 0f;
		}

		public IntPtr ViewerParametersList_Begin(IntPtr vpList)
		{
			return IntPtr.Zero;
		}

		public IntPtr ViewerParametersList_GetByIndex(IntPtr vpList, int idx)
		{
			return IntPtr.Zero;
		}

		public IntPtr ViewerParametersList_GetByNameManufacturer(IntPtr vpList, string name, string manufacturer)
		{
			return IntPtr.Zero;
		}

		public IntPtr ViewerParametersList_GetListForAuthoringTools()
		{
			return IntPtr.Zero;
		}

		public IntPtr ViewerParametersList_Next(IntPtr vpList, IntPtr vpLast)
		{
			return IntPtr.Zero;
		}

		public void ViewerParametersList_SetSDKFilter(IntPtr vpList, string filter)
		{
		}

		public int ViewerParametersList_Size(IntPtr vpList)
		{
			return 0;
		}

		public int VirtualButtonGetId(IntPtr dataSetPtr, string trackableName, string virtualButtonName)
		{
			return 0;
		}

		public int VirtualButtonSetAreaRectangle(IntPtr dataSetPtr, string trackableName, string virtualButtonName, IntPtr rectData)
		{
			return 0;
		}

		public int VirtualButtonSetEnabled(IntPtr dataSetPtr, string trackableName, string virtualButtonName, int enabled)
		{
			return 0;
		}

		public int VirtualButtonSetSensitivity(IntPtr dataSetPtr, string trackableName, string virtualButtonName, int sensitivity)
		{
			return 0;
		}

		public IntPtr VuforiaGetRenderEventCallback()
		{
			return IntPtr.Zero;
		}

		public int VuMarkTargetGetInstanceImage(int vuMarkTargetID, IntPtr instanceImage)
		{
			return 0;
		}

		public int VuMarkTemplateGetOrigin(IntPtr dataSetPtr, string trackableName, IntPtr originPtr)
		{
			return 0;
		}

		public int VuMarkTemplateGetVuMarkUserData(IntPtr dataSetPtr, string trackableName, StringBuilder data, uint dataLength)
		{
			return 0;
		}

		public int VuMarkTemplateSetTrackingFromRuntimeAppearance(IntPtr dataSetPtr, string trackableName, bool enable)
		{
			return 0;
		}

		public int WordGetLetterBoundingBoxes(int wordID, IntPtr letterBoundingBoxes)
		{
			return 0;
		}

		public int WordGetLetterMask(int wordID, IntPtr letterMaskImage)
		{
			return 0;
		}

		public int WordListAddWordsFromFile(string path, int storageType)
		{
			return 0;
		}

		public int WordListAddWordToFilterListU(IntPtr word)
		{
			return 0;
		}

		public int WordListAddWordU(IntPtr word)
		{
			return 0;
		}

		public int WordListClearFilterList()
		{
			return 0;
		}

		public int WordListContainsWordU(IntPtr word)
		{
			return 0;
		}

		public int WordListGetFilterListWordCount()
		{
			return 0;
		}

		public IntPtr WordListGetFilterListWordU(int i)
		{
			return IntPtr.Zero;
		}

		public int WordListGetFilterMode()
		{
			return 0;
		}

		public int WordListLoadFilterList(string path, int storageType)
		{
			return 0;
		}

		public int WordListLoadWordList(string path, int storageType)
		{
			return 0;
		}

		public int WordListRemoveWordFromFilterListU(IntPtr word)
		{
			return 0;
		}

		public int WordListRemoveWordU(IntPtr word)
		{
			return 0;
		}

		public int WordListSetFilterMode(int mode)
		{
			return 0;
		}

		public int WordListUnloadAllLists()
		{
			return 0;
		}
	}
}
