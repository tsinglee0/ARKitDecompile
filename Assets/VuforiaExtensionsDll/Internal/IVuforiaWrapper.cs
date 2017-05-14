using System;
using System.Text;

namespace Vuforia
{
	public interface IVuforiaWrapper
	{
		int CameraDeviceDeinitCamera();

		int CameraDeviceGetCameraDirection();

		void CameraDeviceGetCameraField(IntPtr cameraField, int idx);

		int CameraDeviceGetCameraFieldOfViewRads(IntPtr fovVectorContainer);

		int CameraDeviceGetFieldBool(string key, IntPtr value);

		int CameraDeviceGetFieldFloat(string key, IntPtr value);

		int CameraDeviceGetFieldInt64(string key, IntPtr value);

		int CameraDeviceGetFieldInt64Range(string key, IntPtr intRange);

		int CameraDeviceGetFieldString(string key, StringBuilder value, int maxLength);

		int CameraDeviceGetNumCameraFields();

		int CameraDeviceGetNumVideoModes();

		void CameraDeviceGetVideoMode(int idx, IntPtr videoMode);

		int CameraDeviceInitCamera(int camera);

		int CameraDeviceSelectVideoMode(int idx);

		void CameraDeviceSetCameraConfiguration(int width, int height);

		int CameraDeviceSetFieldBool(string key, bool value);

		int CameraDeviceSetFieldFloat(string key, float value);

		int CameraDeviceSetFieldInt64(string key, long value);

		int CameraDeviceSetFieldInt64Range(string key, long intRangeFrom, long intRangeTo);

		int CameraDeviceSetFieldString(string key, string value);

		int CameraDeviceSetFlashTorchMode(int on);

		int CameraDeviceSetFocusMode(int focusMode);

		int CameraDeviceStartCamera();

		int CameraDeviceStopCamera();

		void CustomViewerParameters_AddDistortionCoefficient(IntPtr obj, float val);

		void CustomViewerParameters_ClearDistortionCoefficients(IntPtr obj);

		void CustomViewerParameters_delete(IntPtr obj);

		IntPtr CustomViewerParameters_new(float version, string name, string manufacturer);

		void CustomViewerParameters_SetButtonType(IntPtr obj, int val);

		void CustomViewerParameters_SetContainsMagnet(IntPtr obj, bool val);

		void CustomViewerParameters_SetFieldOfView(IntPtr obj, IntPtr val);

		void CustomViewerParameters_SetInterLensDistance(IntPtr obj, float val);

		void CustomViewerParameters_SetLensCentreToTrayDistance(IntPtr obj, float val);

		void CustomViewerParameters_SetScreenToLensDistance(IntPtr obj, float val);

		void CustomViewerParameters_SetTrayAlignment(IntPtr obj, int val);

		int CylinderTargetGetDimensions(IntPtr dataSetPtr, string trackableName, IntPtr dimensionPtr);

		int CylinderTargetSetBottomDiameter(IntPtr dataSetPtr, string trackableName, float bottomDiameter);

		int CylinderTargetSetSideLength(IntPtr dataSetPtr, string trackableName, float sideLength);

		int CylinderTargetSetTopDiameter(IntPtr dataSetPtr, string trackableName, float topDiameter);

		int DataSetCreateTrackable(IntPtr dataSetPtr, IntPtr trackableSourcePtr, StringBuilder trackableName, int nameMaxLength, IntPtr trackableData);

		int DataSetDestroyTrackable(IntPtr dataSetPtr, int trackableId);

		int DataSetExists(string relativePath, int storageType);

		int DataSetGetNumTrackableType(int trackableType, IntPtr dataSetPtr);

		int DataSetGetTrackableName(IntPtr dataSetPtr, int trackableId, StringBuilder trackableName, int nameMaxLength);

		int DataSetGetTrackablesOfType(int trackableType, IntPtr trackableDataArray, int trackableDataArrayLength, IntPtr dataSetPtr);

		int DataSetHasReachedTrackableLimit(IntPtr dataSetPtr);

		int DataSetLoad(string relativePath, int storageType, IntPtr dataSetPtr);

		void DeinitFrameState(IntPtr frameState);

		int Device_GetMode();

		IntPtr Device_GetSelectedViewer();

		IntPtr Device_GetViewerList();

		int Device_IsViewerPresent();

		int Device_SelectViewer(IntPtr vp);

		int Device_SetMode(int mode);

		void Device_SetViewerPresent(bool present);

		int DeviceIsEyewearDevice();

		int EyewearCPMClearProfile(int profileID);

		int EyewearCPMGetActiveProfile();

		int EyewearCPMGetCameraToEyePose(int profileID, int eyeID, IntPtr matrix);

		int EyewearCPMGetEyeProjection(int profileID, int eyeID, IntPtr matrix);

		int EyewearCPMGetMaxCount();

		IntPtr EyewearCPMGetProfileName(int profileID);

		int EyewearCPMGetUsedCount();

		int EyewearCPMIsProfileUsed(int profileID);

		int EyewearCPMSetActiveProfile(int profileID);

		int EyewearCPMSetCameraToEyePose(int profileID, int eyeID, IntPtr matrix);

		int EyewearCPMSetEyeProjection(int profileID, int eyeID, IntPtr matrix);

		int EyewearCPMSetProfileName(int profileID, IntPtr name);

		int EyewearDeviceGetScreenOrientation();

		int EyewearDeviceIsDisplayExtended();

		int EyewearDeviceIsDisplayExtendedGLOnly();

		int EyewearDeviceIsDualDisplay();

		int EyewearDeviceIsPredictiveTrackingEnabled();

		int EyewearDeviceIsSeeThru();

		int EyewearDeviceSetDisplayExtended(bool enable);

		int EyewearDeviceSetPredictiveTracking(bool enable);

		float EyewearUserCalibratorGetMaxScaleHint();

		float EyewearUserCalibratorGetMinScaleHint();

		int EyewearUserCalibratorGetProjectionMatrix(IntPtr readingsDataArray, int numReadings, IntPtr cameraToEyeContainer, IntPtr projectionContainer);

		int EyewearUserCalibratorInit(int surfaceWidth, int surfaceHeight, int targetWidth, int targetHeight);

		int EyewearUserCalibratorIsStereoStretched();

		void FrameCounterGetBenchmarkingData(IntPtr benchmarkingData);

		int GetCameraThreadID();

		int GetProjectionGL(float nearPlane, float farPlane, IntPtr projectionContainer, int screenOrientation);

		void GetVuforiaLibraryVersion(StringBuilder value, int maxLength);

		int HasSurfaceBeenRecreated();

		int ImageTargetBuilderBuild(string name, float screenSizeWidth);

		int ImageTargetBuilderGetFrameQuality();

		IntPtr ImageTargetBuilderGetTrackableSource();

		void ImageTargetBuilderStartScan();

		void ImageTargetBuilderStopScan();

		int ImageTargetCreateVirtualButton(IntPtr dataSetPtr, string trackableName, string virtualButtonName, IntPtr rectData);

		int ImageTargetDestroyVirtualButton(IntPtr dataSetPtr, string trackableName, string virtualButtonName);

		int ImageTargetGetNumVirtualButtons(IntPtr dataSetPtr, string trackableName);

		int ImageTargetGetVirtualButtonName(IntPtr dataSetPtr, string trackableName, int idx, StringBuilder vbName, int nameMaxLength);

		int ImageTargetGetVirtualButtons(IntPtr virtualButtonDataArray, IntPtr rectangleDataArray, int virtualButtonDataArrayLength, IntPtr dataSetPtr, string trackableName);

		void InitFrameState(IntPtr frameState);

		void InitPlatformNative();

		float MultiTargetGetLargestSizeComponent(IntPtr dataSetPtr, string trackableName);

		int ObjectTargetGetSize(IntPtr dataSetPtr, string trackableName, IntPtr sizePtr);

		int ObjectTargetSetSize(IntPtr dataSetPtr, string trackableName, IntPtr sizePtr);

		int ObjectTrackerActivateDataSet(IntPtr dataSetPtr);

		IntPtr ObjectTrackerCreateDataSet();

		int ObjectTrackerDeactivateDataSet(IntPtr dataSetPtr);

		int ObjectTrackerDestroyDataSet(IntPtr dataSetPtr);

		int ObjectTrackerPersistExtendedTracking(int on);

		int ObjectTrackerResetExtendedTracking();

		void OnPause();

		void OnResume();

		void OnSurfaceChanged(int width, int height);

		void QcarAddCameraFrame(IntPtr pixels, int width, int height, int format, int stride, int frameIdx, int flipHorizontally);

		void QcarDeinit();

		int QcarGetBufferSize(int width, int height, int format);

		int QcarInit(string licenseKey);

		int QcarSetFrameFormat(int format, int enabled);

		int QcarSetHint(uint hint, int value);

		int ReconstructioFromEnvironmentGetReconstructionState(IntPtr reconstruction);

		int ReconstructionFromTargetSetInitializationTarget(IntPtr reconstruction, IntPtr dataSetPtr, int trackableID, IntPtr occluderMin, IntPtr occluderMax, IntPtr offsetToOccluder, IntPtr rotationAxisToOccluder, float rotationAngleToOccluder);

		int ReconstructionIsReconstructing(IntPtr reconstruction);

		int ReconstructionReset(IntPtr reconstruction);

		int ReconstructionSetMaximumArea(IntPtr reconstruction, IntPtr maximumArea);

		void ReconstructionSetNavMeshPadding(IntPtr reconstruction, float padding);

		int ReconstructionStart(IntPtr reconstruction);

		int ReconstructionStop(IntPtr reconstruction);

		IntPtr RendererCreateNativeTexture(uint width, uint height, int format);

		void RendererEnd();

		int RendererGetGraphicsAPI();

		int RendererGetRecommendedFps(int flags);

		void RendererGetVideoBackgroundCfg(IntPtr bgCfg);

		int RendererGetVideoBackgroundTextureInfo(IntPtr texInfo);

		int RendererIsVideoBackgroundTextureInfoAvailable();

		void RendererSetVideoBackgroundCfg(IntPtr bgCfg);

		int RendererSetVideoBackgroundTextureID(int textureID);

		int RendererSetVideoBackgroundTexturePtr(IntPtr texturePtr);

		void RenderingPrimitives_DeleteCopy();

		void RenderingPrimitives_GetDistortionMesh(int viewId, IntPtr meshData);

		void RenderingPrimitives_GetDistortionMeshSize(int viewId, IntPtr size);

		void RenderingPrimitives_GetDistortionTextureViewport(int viewID, IntPtr viewportContainer);

		void RenderingPrimitives_GetEffectiveFov(int viewID, IntPtr fovContainer);

		void RenderingPrimitives_GetEyeDisplayAdjustmentMatrix(int viewID, IntPtr matrixContainer);

		void RenderingPrimitives_GetNormalizedViewport(int viewID, IntPtr viewportContainer);

		void RenderingPrimitives_GetProjectionMatrix(int viewID, float near, float far, IntPtr projectionContainer, int screenOrientation);

		void RenderingPrimitives_GetViewport(int viewID, IntPtr viewportContainer);

		void RenderingPrimitives_GetViewportCentreToEyeAxis(int viewID, IntPtr vectorContainer);

		void RenderingPrimitives_UpdateCopy();

		int RotationalDeviceTracker_GetModelCorrectionMode();

		void RotationalDeviceTracker_GetModelCorrectionTransform(IntPtr pivot);

		int RotationalDeviceTracker_GetPosePrediction();

		int RotationalDeviceTracker_Recenter();

		int RotationalDeviceTracker_SetModelCorrectionMode(int mode);

		int RotationalDeviceTracker_SetModelCorrectionModeWithTransform(int mode, IntPtr pivot);

		int RotationalDeviceTracker_SetPosePrediction(bool mode);

		void SetApplicationEnvironment(int unityVersionMajor, int unityVersionMinor, int unityVersionChange, int sdkWrapperType);

		int SetHolographicAppCS(IntPtr appSpecifiedCS);

		void SetRenderBuffers(IntPtr colorBuffer);

		void SetStateBufferSize(int size);

		int SmartTerrainBuilderAddReconstruction(IntPtr reconstruction);

		IntPtr SmartTerrainBuilderCreateReconstructionFromEnvironment();

		IntPtr SmartTerrainBuilderCreateReconstructionFromTarget();

		int SmartTerrainBuilderDestroyReconstruction(IntPtr reconstruction);

		int SmartTerrainBuilderRemoveReconstruction(IntPtr reconstruction);

		int SmartTerrainTrackerDeinitBuilder();

		int SmartTerrainTrackerInitBuilder();

		int SmartTerrainTrackerSetScaleToMillimeter(float scaleToMillimenters);

		int StartExtendedTracking(IntPtr dataSetPtr, int trackableID);

		int StopExtendedTracking(IntPtr dataSetPtr, int trackableID);

		void TargetFinderClearTrackables();

		int TargetFinderDeinit();

		int TargetFinderEnableTracking(IntPtr searchResult, IntPtr trackableData);

		void TargetFinderGetImageTargets(IntPtr trackableIdArray, int trackableIdArrayLength);

		int TargetFinderGetInitState();

		int TargetFinderGetResults(IntPtr searchResultArray, int searchResultArrayLength);

		int TargetFinderStartInit(string userKey, string secretKey);

		int TargetFinderStartRecognition();

		int TargetFinderStop();

		void TargetFinderUpdate(IntPtr targetFinderState, int filterMode);

		void TextTrackerGetRegionOfInterest(IntPtr detectionROI, IntPtr trackingROI);

		int TextTrackerSetRegionOfInterest(int detectionLeftTopX, int detectionLeftTopY, int detectionRightBottomX, int detectionRightBottomY, int trackingLeftTopX, int trackingLeftTopY, int trackingRightBottomX, int trackingRightBottomY, int upDirection);

		int TrackerManagerDeinitTracker(int trackerTypeID);

		int TrackerManagerInitTracker(int trackerTypeID);

		int TrackerStart(int trackerTypeID);

		void TrackerStop(int trackerTypeID);

		int UpdateQCAR(IntPtr imageHeaderDataArray, int imageHeaderArrayLength, IntPtr frameState, int screenOrientation);

		int ViewerParameters_ContainsMagnet(IntPtr obj);

		IntPtr ViewerParameters_copy(IntPtr obj);

		void ViewerParameters_delete(IntPtr obj);

		int ViewerParameters_GetButtonType(IntPtr obj);

		float ViewerParameters_GetDistortionCoefficient(IntPtr obj, int idx);

		void ViewerParameters_GetFieldOfView(IntPtr obj, IntPtr result);

		float ViewerParameters_GetInterLensDistance(IntPtr obj);

		float ViewerParameters_GetLensCentreToTrayDistance(IntPtr obj);

		IntPtr ViewerParameters_GetManufacturer(IntPtr obj);

		IntPtr ViewerParameters_GetName(IntPtr obj);

		int ViewerParameters_GetNumDistortionCoefficients(IntPtr obj);

		float ViewerParameters_GetScreenToLensDistance(IntPtr obj);

		int ViewerParameters_GetTrayAlignment(IntPtr obj);

		float ViewerParameters_GetVersion(IntPtr obj);

		IntPtr ViewerParametersList_Begin(IntPtr vpList);

		IntPtr ViewerParametersList_GetByIndex(IntPtr vpList, int idx);

		IntPtr ViewerParametersList_GetByNameManufacturer(IntPtr vpList, string name, string manufacturer);

		IntPtr ViewerParametersList_GetListForAuthoringTools();

		IntPtr ViewerParametersList_Next(IntPtr vpList, IntPtr vpLast);

		void ViewerParametersList_SetSDKFilter(IntPtr vpList, string filter);

		int ViewerParametersList_Size(IntPtr vpList);

		int VirtualButtonGetId(IntPtr dataSetPtr, string trackableName, string virtualButtonName);

		int VirtualButtonSetAreaRectangle(IntPtr dataSetPtr, string trackableName, string virtualButtonName, IntPtr rectData);

		int VirtualButtonSetEnabled(IntPtr dataSetPtr, string trackableName, string virtualButtonName, int enabled);

		int VirtualButtonSetSensitivity(IntPtr dataSetPtr, string trackableName, string virtualButtonName, int sensitivity);

		IntPtr VuforiaGetRenderEventCallback();

		int VuMarkTargetGetInstanceImage(int vuMarkTargetID, IntPtr instanceImage);

		int VuMarkTemplateGetOrigin(IntPtr dataSetPtr, string trackableName, IntPtr originPtr);

		int VuMarkTemplateGetVuMarkUserData(IntPtr dataSetPtr, string trackableName, StringBuilder data, uint dataLength);

		int VuMarkTemplateSetTrackingFromRuntimeAppearance(IntPtr dataSetPtr, string trackableName, bool enable);

		int WordGetLetterBoundingBoxes(int wordID, IntPtr letterBoundingBoxes);

		int WordGetLetterMask(int wordID, IntPtr letterMaskImage);

		int WordListAddWordsFromFile(string path, int storageType);

		int WordListAddWordToFilterListU(IntPtr word);

		int WordListAddWordU(IntPtr word);

		int WordListClearFilterList();

		int WordListContainsWordU(IntPtr word);

		int WordListGetFilterListWordCount();

		IntPtr WordListGetFilterListWordU(int i);

		int WordListGetFilterMode();

		int WordListLoadFilterList(string path, int storageType);

		int WordListLoadWordList(string path, int storageType);

		int WordListRemoveWordFromFilterListU(IntPtr word);

		int WordListRemoveWordU(IntPtr word);

		int WordListSetFilterMode(int mode);

		int WordListUnloadAllLists();
	}
}
