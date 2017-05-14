using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Vuforia
{
	internal class VuforiaNativeWrapper : IVuforiaWrapper
	{
		public int CameraDeviceDeinitCamera()
		{
			return VuforiaNativeWrapper.cameraDeviceDeinitCamera();
		}

		public int CameraDeviceGetCameraDirection()
		{
			return VuforiaNativeWrapper.cameraDeviceGetCameraDirection();
		}

		public void CameraDeviceGetCameraField(IntPtr cameraField, int idx)
		{
			VuforiaNativeWrapper.cameraDeviceGetCameraField(cameraField, idx);
		}

		public int CameraDeviceGetCameraFieldOfViewRads(IntPtr fovVectorContainer)
		{
			return VuforiaNativeWrapper.cameraDeviceGetCameraFieldOfViewRads(fovVectorContainer);
		}

		public int CameraDeviceGetFieldBool(string key, IntPtr value)
		{
			return VuforiaNativeWrapper.cameraDeviceGetFieldBool(key, value);
		}

		public int CameraDeviceGetFieldFloat(string key, IntPtr value)
		{
			return VuforiaNativeWrapper.cameraDeviceGetFieldFloat(key, value);
		}

		public int CameraDeviceGetFieldInt64(string key, IntPtr value)
		{
			return VuforiaNativeWrapper.cameraDeviceGetFieldInt64(key, value);
		}

		public int CameraDeviceGetFieldInt64Range(string key, IntPtr intRange)
		{
			return VuforiaNativeWrapper.cameraDeviceGetFieldInt64Range(key, intRange);
		}

		public int CameraDeviceGetFieldString(string key, StringBuilder value, int maxLength)
		{
			return VuforiaNativeWrapper.cameraDeviceGetFieldString(key, value, maxLength);
		}

		public int CameraDeviceGetNumCameraFields()
		{
			return VuforiaNativeWrapper.cameraDeviceGetNumCameraFields();
		}

		public int CameraDeviceGetNumVideoModes()
		{
			return VuforiaNativeWrapper.cameraDeviceGetNumVideoModes();
		}

		public void CameraDeviceGetVideoMode(int idx, IntPtr videoMode)
		{
			VuforiaNativeWrapper.cameraDeviceGetVideoMode(idx, videoMode);
		}

		public int CameraDeviceInitCamera(int camera)
		{
			return VuforiaNativeWrapper.cameraDeviceInitCamera(camera);
		}

		public int CameraDeviceSelectVideoMode(int idx)
		{
			return VuforiaNativeWrapper.cameraDeviceSelectVideoMode(idx);
		}

		public void CameraDeviceSetCameraConfiguration(int width, int height)
		{
			VuforiaNativeWrapper.cameraDeviceSetCameraConfiguration(width, height);
		}

		public int CameraDeviceSetFieldBool(string key, bool value)
		{
			return VuforiaNativeWrapper.cameraDeviceSetFieldBool(key, value);
		}

		public int CameraDeviceSetFieldFloat(string key, float value)
		{
			return VuforiaNativeWrapper.cameraDeviceSetFieldFloat(key, value);
		}

		public int CameraDeviceSetFieldInt64(string key, long value)
		{
			return VuforiaNativeWrapper.cameraDeviceSetFieldInt64(key, value);
		}

		public int CameraDeviceSetFieldInt64Range(string key, long intRangeFrom, long intRangeTo)
		{
			return VuforiaNativeWrapper.cameraDeviceSetFieldInt64Range(key, intRangeFrom, intRangeTo);
		}

		public int CameraDeviceSetFieldString(string key, string value)
		{
			return VuforiaNativeWrapper.cameraDeviceSetFieldString(key, value);
		}

		public int CameraDeviceSetFlashTorchMode(int on)
		{
			return VuforiaNativeWrapper.cameraDeviceSetFlashTorchMode(on);
		}

		public int CameraDeviceSetFocusMode(int focusMode)
		{
			return VuforiaNativeWrapper.cameraDeviceSetFocusMode(focusMode);
		}

		public int CameraDeviceStartCamera()
		{
			return VuforiaNativeWrapper.cameraDeviceStartCamera();
		}

		public int CameraDeviceStopCamera()
		{
			return VuforiaNativeWrapper.cameraDeviceStopCamera();
		}

		public void CustomViewerParameters_AddDistortionCoefficient(IntPtr obj, float val)
		{
			VuforiaNativeWrapper.customViewerParameters_AddDistortionCoefficient(obj, val);
		}

		public void CustomViewerParameters_ClearDistortionCoefficients(IntPtr obj)
		{
			VuforiaNativeWrapper.customViewerParameters_ClearDistortionCoefficients(obj);
		}

		public void CustomViewerParameters_delete(IntPtr obj)
		{
			VuforiaNativeWrapper.customViewerParameters_delete(obj);
		}

		public IntPtr CustomViewerParameters_new(float version, string name, string manufacturer)
		{
			return VuforiaNativeWrapper.customViewerParameters_new(version, name, manufacturer);
		}

		public void CustomViewerParameters_SetButtonType(IntPtr obj, int val)
		{
			VuforiaNativeWrapper.customViewerParameters_SetButtonType(obj, val);
		}

		public void CustomViewerParameters_SetContainsMagnet(IntPtr obj, bool val)
		{
			VuforiaNativeWrapper.customViewerParameters_SetContainsMagnet(obj, val);
		}

		public void CustomViewerParameters_SetFieldOfView(IntPtr obj, IntPtr val)
		{
			VuforiaNativeWrapper.customViewerParameters_SetFieldOfView(obj, val);
		}

		public void CustomViewerParameters_SetInterLensDistance(IntPtr obj, float val)
		{
			VuforiaNativeWrapper.customViewerParameters_SetInterLensDistance(obj, val);
		}

		public void CustomViewerParameters_SetLensCentreToTrayDistance(IntPtr obj, float val)
		{
			VuforiaNativeWrapper.customViewerParameters_SetLensCentreToTrayDistance(obj, val);
		}

		public void CustomViewerParameters_SetScreenToLensDistance(IntPtr obj, float val)
		{
			VuforiaNativeWrapper.customViewerParameters_SetScreenToLensDistance(obj, val);
		}

		public void CustomViewerParameters_SetTrayAlignment(IntPtr obj, int val)
		{
			VuforiaNativeWrapper.customViewerParameters_SetTrayAlignment(obj, val);
		}

		public int CylinderTargetGetDimensions(IntPtr dataSetPtr, string trackableName, IntPtr dimensionPtr)
		{
			return VuforiaNativeWrapper.cylinderTargetGetDimensions(dataSetPtr, trackableName, dimensionPtr);
		}

		public int CylinderTargetSetBottomDiameter(IntPtr dataSetPtr, string trackableName, float bottomDiameter)
		{
			return VuforiaNativeWrapper.cylinderTargetSetBottomDiameter(dataSetPtr, trackableName, bottomDiameter);
		}

		public int CylinderTargetSetSideLength(IntPtr dataSetPtr, string trackableName, float sideLength)
		{
			return VuforiaNativeWrapper.cylinderTargetSetSideLength(dataSetPtr, trackableName, sideLength);
		}

		public int CylinderTargetSetTopDiameter(IntPtr dataSetPtr, string trackableName, float topDiameter)
		{
			return VuforiaNativeWrapper.cylinderTargetSetTopDiameter(dataSetPtr, trackableName, topDiameter);
		}

		public int DataSetCreateTrackable(IntPtr dataSetPtr, IntPtr trackableSourcePtr, StringBuilder trackableName, int nameMaxLength, IntPtr trackableData)
		{
			return VuforiaNativeWrapper.dataSetCreateTrackable(dataSetPtr, trackableSourcePtr, trackableName, nameMaxLength, trackableData);
		}

		public int DataSetDestroyTrackable(IntPtr dataSetPtr, int trackableId)
		{
			return VuforiaNativeWrapper.dataSetDestroyTrackable(dataSetPtr, trackableId);
		}

		public int DataSetExists(string relativePath, int storageType)
		{
			return VuforiaNativeWrapper.dataSetExists(relativePath, storageType);
		}

		public int DataSetGetNumTrackableType(int trackableType, IntPtr dataSetPtr)
		{
			return VuforiaNativeWrapper.dataSetGetNumTrackableType(trackableType, dataSetPtr);
		}

		public int DataSetGetTrackableName(IntPtr dataSetPtr, int trackableId, StringBuilder trackableName, int nameMaxLength)
		{
			return VuforiaNativeWrapper.dataSetGetTrackableName(dataSetPtr, trackableId, trackableName, nameMaxLength);
		}

		public int DataSetGetTrackablesOfType(int trackableType, IntPtr trackableDataArray, int trackableDataArrayLength, IntPtr dataSetPtr)
		{
			return VuforiaNativeWrapper.dataSetGetTrackablesOfType(trackableType, trackableDataArray, trackableDataArrayLength, dataSetPtr);
		}

		public int DataSetHasReachedTrackableLimit(IntPtr dataSetPtr)
		{
			return VuforiaNativeWrapper.dataSetHasReachedTrackableLimit(dataSetPtr);
		}

		public int DataSetLoad(string relativePath, int storageType, IntPtr dataSetPtr)
		{
			return VuforiaNativeWrapper.dataSetLoad(relativePath, storageType, dataSetPtr);
		}

		public void DeinitFrameState(IntPtr frameState)
		{
			VuforiaNativeWrapper.deinitFrameState(frameState);
		}

		public int Device_GetMode()
		{
			return VuforiaNativeWrapper.device_GetMode();
		}

		public IntPtr Device_GetSelectedViewer()
		{
			return VuforiaNativeWrapper.device_GetSelectedViewer();
		}

		public IntPtr Device_GetViewerList()
		{
			return VuforiaNativeWrapper.device_GetViewerList();
		}

		public int Device_IsViewerPresent()
		{
			return VuforiaNativeWrapper.device_IsViewerPresent();
		}

		public int Device_SelectViewer(IntPtr vp)
		{
			return VuforiaNativeWrapper.device_SelectViewer(vp);
		}

		public int Device_SetMode(int mode)
		{
			return VuforiaNativeWrapper.device_SetMode(mode);
		}

		public void Device_SetViewerPresent(bool present)
		{
			VuforiaNativeWrapper.device_SetViewerPresent(present);
		}

		public int DeviceIsEyewearDevice()
		{
			return VuforiaNativeWrapper.deviceIsEyewearDevice();
		}

		public int EyewearCPMClearProfile(int profileID)
		{
			return VuforiaNativeWrapper.eyewearCPMClearProfile(profileID);
		}

		public int EyewearCPMGetActiveProfile()
		{
			return VuforiaNativeWrapper.eyewearCPMGetActiveProfile();
		}

		public int EyewearCPMGetCameraToEyePose(int profileID, int eyeID, IntPtr matrix)
		{
			return VuforiaNativeWrapper.eyewearCPMGetCameraToEyePose(profileID, eyeID, matrix);
		}

		public int EyewearCPMGetEyeProjection(int profileID, int eyeID, IntPtr matrix)
		{
			return VuforiaNativeWrapper.eyewearCPMGetEyeProjection(profileID, eyeID, matrix);
		}

		public int EyewearCPMGetMaxCount()
		{
			return VuforiaNativeWrapper.eyewearCPMGetMaxCount();
		}

		public IntPtr EyewearCPMGetProfileName(int profileID)
		{
			return VuforiaNativeWrapper.eyewearCPMGetProfileName(profileID);
		}

		public int EyewearCPMGetUsedCount()
		{
			return VuforiaNativeWrapper.eyewearCPMGetUsedCount();
		}

		public int EyewearCPMIsProfileUsed(int profileID)
		{
			return VuforiaNativeWrapper.eyewearCPMIsProfileUsed(profileID);
		}

		public int EyewearCPMSetActiveProfile(int profileID)
		{
			return VuforiaNativeWrapper.eyewearCPMSetActiveProfile(profileID);
		}

		public int EyewearCPMSetCameraToEyePose(int profileID, int eyeID, IntPtr matrix)
		{
			return VuforiaNativeWrapper.eyewearCPMSetCameraToEyePose(profileID, eyeID, matrix);
		}

		public int EyewearCPMSetEyeProjection(int profileID, int eyeID, IntPtr matrix)
		{
			return VuforiaNativeWrapper.eyewearCPMSetEyeProjection(profileID, eyeID, matrix);
		}

		public int EyewearCPMSetProfileName(int profileID, IntPtr name)
		{
			return VuforiaNativeWrapper.eyewearCPMSetProfileName(profileID, name);
		}

		public int EyewearDeviceGetScreenOrientation()
		{
			return VuforiaNativeWrapper.eyewearDeviceGetScreenOrientation();
		}

		public int EyewearDeviceIsDisplayExtended()
		{
			return VuforiaNativeWrapper.eyewearDeviceIsDisplayExtended();
		}

		public int EyewearDeviceIsDisplayExtendedGLOnly()
		{
			return VuforiaNativeWrapper.eyewearDeviceIsDisplayExtendedGLOnly();
		}

		public int EyewearDeviceIsDualDisplay()
		{
			return VuforiaNativeWrapper.eyewearDeviceIsDualDisplay();
		}

		public int EyewearDeviceIsPredictiveTrackingEnabled()
		{
			return VuforiaNativeWrapper.eyewearDeviceIsPredictiveTrackingEnabled();
		}

		public int EyewearDeviceIsSeeThru()
		{
			return VuforiaNativeWrapper.eyewearDeviceIsSeeThru();
		}

		public int EyewearDeviceSetDisplayExtended(bool enable)
		{
			return VuforiaNativeWrapper.eyewearDeviceSetDisplayExtended(enable);
		}

		public int EyewearDeviceSetPredictiveTracking(bool enable)
		{
			return VuforiaNativeWrapper.eyewearDeviceSetPredictiveTracking(enable);
		}

		public float EyewearUserCalibratorGetMaxScaleHint()
		{
			return VuforiaNativeWrapper.eyewearUserCalibratorGetMaxScaleHint();
		}

		public float EyewearUserCalibratorGetMinScaleHint()
		{
			return VuforiaNativeWrapper.eyewearUserCalibratorGetMinScaleHint();
		}

		public int EyewearUserCalibratorGetProjectionMatrix(IntPtr readingsDataArray, int numReadings, IntPtr cameraToEyeContainer, IntPtr projectionContainer)
		{
			return VuforiaNativeWrapper.eyewearUserCalibratorGetProjectionMatrix(readingsDataArray, numReadings, cameraToEyeContainer, projectionContainer);
		}

		public int EyewearUserCalibratorInit(int surfaceWidth, int surfaceHeight, int targetWidth, int targetHeight)
		{
			return VuforiaNativeWrapper.eyewearUserCalibratorInit(surfaceWidth, surfaceHeight, targetWidth, targetHeight);
		}

		public int EyewearUserCalibratorIsStereoStretched()
		{
			return VuforiaNativeWrapper.eyewearUserCalibratorIsStereoStretched();
		}

		public void FrameCounterGetBenchmarkingData(IntPtr benchmarkingData)
		{
			VuforiaNativeWrapper.frameCounterGetBenchmarkingData(benchmarkingData);
		}

		public int GetCameraThreadID()
		{
			return VuforiaNativeWrapper.getCameraThreadID();
		}

		public int GetProjectionGL(float nearPlane, float farPlane, IntPtr projectionContainer, int screenOrientation)
		{
			return VuforiaNativeWrapper.getProjectionGL(nearPlane, farPlane, projectionContainer, screenOrientation);
		}

		public void GetVuforiaLibraryVersion(StringBuilder value, int maxLength)
		{
			VuforiaNativeWrapper.getVuforiaLibraryVersion(value, maxLength);
		}

		public int HasSurfaceBeenRecreated()
		{
			return VuforiaNativeWrapper.hasSurfaceBeenRecreated();
		}

		public int ImageTargetBuilderBuild(string name, float screenSizeWidth)
		{
			return VuforiaNativeWrapper.imageTargetBuilderBuild(name, screenSizeWidth);
		}

		public int ImageTargetBuilderGetFrameQuality()
		{
			return VuforiaNativeWrapper.imageTargetBuilderGetFrameQuality();
		}

		public IntPtr ImageTargetBuilderGetTrackableSource()
		{
			return VuforiaNativeWrapper.imageTargetBuilderGetTrackableSource();
		}

		public void ImageTargetBuilderStartScan()
		{
			VuforiaNativeWrapper.imageTargetBuilderStartScan();
		}

		public void ImageTargetBuilderStopScan()
		{
			VuforiaNativeWrapper.imageTargetBuilderStopScan();
		}

		public int ImageTargetCreateVirtualButton(IntPtr dataSetPtr, string trackableName, string virtualButtonName, IntPtr rectData)
		{
			return VuforiaNativeWrapper.imageTargetCreateVirtualButton(dataSetPtr, trackableName, virtualButtonName, rectData);
		}

		public int ImageTargetDestroyVirtualButton(IntPtr dataSetPtr, string trackableName, string virtualButtonName)
		{
			return VuforiaNativeWrapper.imageTargetDestroyVirtualButton(dataSetPtr, trackableName, virtualButtonName);
		}

		public int ImageTargetGetNumVirtualButtons(IntPtr dataSetPtr, string trackableName)
		{
			return VuforiaNativeWrapper.imageTargetGetNumVirtualButtons(dataSetPtr, trackableName);
		}

		public int ImageTargetGetVirtualButtonName(IntPtr dataSetPtr, string trackableName, int idx, StringBuilder vbName, int nameMaxLength)
		{
			return VuforiaNativeWrapper.imageTargetGetVirtualButtonName(dataSetPtr, trackableName, idx, vbName, nameMaxLength);
		}

		public int ImageTargetGetVirtualButtons(IntPtr virtualButtonDataArray, IntPtr rectangleDataArray, int virtualButtonDataArrayLength, IntPtr dataSetPtr, string trackableName)
		{
			return VuforiaNativeWrapper.imageTargetGetVirtualButtons(virtualButtonDataArray, rectangleDataArray, virtualButtonDataArrayLength, dataSetPtr, trackableName);
		}

		public void InitFrameState(IntPtr frameState)
		{
			VuforiaNativeWrapper.initFrameState(frameState);
		}

		public void InitPlatformNative()
		{
			VuforiaNativeWrapper.initPlatformNative();
		}

		public float MultiTargetGetLargestSizeComponent(IntPtr dataSetPtr, string trackableName)
		{
			return VuforiaNativeWrapper.multiTargetGetLargestSizeComponent(dataSetPtr, trackableName);
		}

		public int ObjectTargetGetSize(IntPtr dataSetPtr, string trackableName, IntPtr sizePtr)
		{
			return VuforiaNativeWrapper.objectTargetGetSize(dataSetPtr, trackableName, sizePtr);
		}

		public int ObjectTargetSetSize(IntPtr dataSetPtr, string trackableName, IntPtr sizePtr)
		{
			return VuforiaNativeWrapper.objectTargetSetSize(dataSetPtr, trackableName, sizePtr);
		}

		public int ObjectTrackerActivateDataSet(IntPtr dataSetPtr)
		{
			return VuforiaNativeWrapper.objectTrackerActivateDataSet(dataSetPtr);
		}

		public IntPtr ObjectTrackerCreateDataSet()
		{
			return VuforiaNativeWrapper.objectTrackerCreateDataSet();
		}

		public int ObjectTrackerDeactivateDataSet(IntPtr dataSetPtr)
		{
			return VuforiaNativeWrapper.objectTrackerDeactivateDataSet(dataSetPtr);
		}

		public int ObjectTrackerDestroyDataSet(IntPtr dataSetPtr)
		{
			return VuforiaNativeWrapper.objectTrackerDestroyDataSet(dataSetPtr);
		}

		public int ObjectTrackerPersistExtendedTracking(int on)
		{
			return VuforiaNativeWrapper.objectTrackerPersistExtendedTracking(on);
		}

		public int ObjectTrackerResetExtendedTracking()
		{
			return VuforiaNativeWrapper.objectTrackerResetExtendedTracking();
		}

		public void OnPause()
		{
			VuforiaNativeWrapper.onPause();
		}

		public void OnResume()
		{
			VuforiaNativeWrapper.onResume();
		}

		public void OnSurfaceChanged(int width, int height)
		{
			VuforiaNativeWrapper.onSurfaceChanged(width, height);
		}

		public void QcarAddCameraFrame(IntPtr pixels, int width, int height, int format, int stride, int frameIdx, int flipHorizontally)
		{
			VuforiaNativeWrapper.qcarAddCameraFrame(pixels, width, height, format, stride, frameIdx, flipHorizontally);
		}

		public void QcarDeinit()
		{
			VuforiaNativeWrapper.qcarDeinit();
		}

		public int QcarGetBufferSize(int width, int height, int format)
		{
			return VuforiaNativeWrapper.qcarGetBufferSize(width, height, format);
		}

		public int QcarInit(string licenseKey)
		{
			return VuforiaNativeWrapper.qcarInit(licenseKey);
		}

		public int QcarSetFrameFormat(int format, int enabled)
		{
			return VuforiaNativeWrapper.qcarSetFrameFormat(format, enabled);
		}

		public int QcarSetHint(uint hint, int value)
		{
			return VuforiaNativeWrapper.qcarSetHint(hint, value);
		}

		public int ReconstructioFromEnvironmentGetReconstructionState(IntPtr reconstruction)
		{
			return VuforiaNativeWrapper.reconstructioFromEnvironmentGetReconstructionState(reconstruction);
		}

		public int ReconstructionFromTargetSetInitializationTarget(IntPtr reconstruction, IntPtr dataSetPtr, int trackableID, IntPtr occluderMin, IntPtr occluderMax, IntPtr offsetToOccluder, IntPtr rotationAxisToOccluder, float rotationAngleToOccluder)
		{
			return VuforiaNativeWrapper.reconstructionFromTargetSetInitializationTarget(reconstruction, dataSetPtr, trackableID, occluderMin, occluderMax, offsetToOccluder, rotationAxisToOccluder, rotationAngleToOccluder);
		}

		public int ReconstructionIsReconstructing(IntPtr reconstruction)
		{
			return VuforiaNativeWrapper.reconstructionIsReconstructing(reconstruction);
		}

		public int ReconstructionReset(IntPtr reconstruction)
		{
			return VuforiaNativeWrapper.reconstructionReset(reconstruction);
		}

		public int ReconstructionSetMaximumArea(IntPtr reconstruction, IntPtr maximumArea)
		{
			return VuforiaNativeWrapper.reconstructionSetMaximumArea(reconstruction, maximumArea);
		}

		public void ReconstructionSetNavMeshPadding(IntPtr reconstruction, float padding)
		{
			VuforiaNativeWrapper.reconstructionSetNavMeshPadding(reconstruction, padding);
		}

		public int ReconstructionStart(IntPtr reconstruction)
		{
			return VuforiaNativeWrapper.reconstructionStart(reconstruction);
		}

		public int ReconstructionStop(IntPtr reconstruction)
		{
			return VuforiaNativeWrapper.reconstructionStop(reconstruction);
		}

		public IntPtr RendererCreateNativeTexture(uint width, uint height, int format)
		{
			return VuforiaNativeWrapper.rendererCreateNativeTexture(width, height, format);
		}

		public void RendererEnd()
		{
			VuforiaNativeWrapper.rendererEnd();
		}

		public int RendererGetGraphicsAPI()
		{
			return VuforiaNativeWrapper.rendererGetGraphicsAPI();
		}

		public int RendererGetRecommendedFps(int flags)
		{
			return VuforiaNativeWrapper.rendererGetRecommendedFps(flags);
		}

		public void RendererGetVideoBackgroundCfg(IntPtr bgCfg)
		{
			VuforiaNativeWrapper.rendererGetVideoBackgroundCfg(bgCfg);
		}

		public int RendererGetVideoBackgroundTextureInfo(IntPtr texInfo)
		{
			return VuforiaNativeWrapper.rendererGetVideoBackgroundTextureInfo(texInfo);
		}

		public int RendererIsVideoBackgroundTextureInfoAvailable()
		{
			return VuforiaNativeWrapper.rendererIsVideoBackgroundTextureInfoAvailable();
		}

		public void RendererSetVideoBackgroundCfg(IntPtr bgCfg)
		{
			VuforiaNativeWrapper.rendererSetVideoBackgroundCfg(bgCfg);
		}

		public int RendererSetVideoBackgroundTextureID(int textureID)
		{
			return VuforiaNativeWrapper.rendererSetVideoBackgroundTextureID(textureID);
		}

		public int RendererSetVideoBackgroundTexturePtr(IntPtr texturePtr)
		{
			return VuforiaNativeWrapper.rendererSetVideoBackgroundTexturePtr(texturePtr);
		}

		public void RenderingPrimitives_DeleteCopy()
		{
			VuforiaNativeWrapper.renderingPrimitives_DeleteCopy();
		}

		public void RenderingPrimitives_GetDistortionMesh(int viewId, IntPtr meshData)
		{
			VuforiaNativeWrapper.renderingPrimitives_GetDistortionMesh(viewId, meshData);
		}

		public void RenderingPrimitives_GetDistortionMeshSize(int viewId, IntPtr size)
		{
			VuforiaNativeWrapper.renderingPrimitives_GetDistortionMeshSize(viewId, size);
		}

		public void RenderingPrimitives_GetDistortionTextureViewport(int viewID, IntPtr viewportContainer)
		{
			VuforiaNativeWrapper.renderingPrimitives_GetDistortionTextureViewport(viewID, viewportContainer);
		}

		public void RenderingPrimitives_GetEffectiveFov(int viewID, IntPtr fovContainer)
		{
			VuforiaNativeWrapper.renderingPrimitives_GetEffectiveFov(viewID, fovContainer);
		}

		public void RenderingPrimitives_GetEyeDisplayAdjustmentMatrix(int viewID, IntPtr matrixContainer)
		{
			VuforiaNativeWrapper.renderingPrimitives_GetEyeDisplayAdjustmentMatrix(viewID, matrixContainer);
		}

		public void RenderingPrimitives_GetNormalizedViewport(int viewID, IntPtr viewportContainer)
		{
			VuforiaNativeWrapper.renderingPrimitives_GetNormalizedViewport(viewID, viewportContainer);
		}

		public void RenderingPrimitives_GetProjectionMatrix(int viewID, float near, float far, IntPtr projectionContainer, int screenOrientation)
		{
			VuforiaNativeWrapper.renderingPrimitives_GetProjectionMatrix(viewID, near, far, projectionContainer, screenOrientation);
		}

		public void RenderingPrimitives_GetViewport(int viewID, IntPtr viewportContainer)
		{
			VuforiaNativeWrapper.renderingPrimitives_GetViewport(viewID, viewportContainer);
		}

		public void RenderingPrimitives_GetViewportCentreToEyeAxis(int viewID, IntPtr vectorContainer)
		{
			VuforiaNativeWrapper.renderingPrimitives_GetViewportCentreToEyeAxis(viewID, vectorContainer);
		}

		public void RenderingPrimitives_UpdateCopy()
		{
			VuforiaNativeWrapper.renderingPrimitives_UpdateCopy();
		}

		public int RotationalDeviceTracker_GetModelCorrectionMode()
		{
			return VuforiaNativeWrapper.rotationalDeviceTracker_GetModelCorrectionMode();
		}

		public void RotationalDeviceTracker_GetModelCorrectionTransform(IntPtr pivot)
		{
			VuforiaNativeWrapper.rotationalDeviceTracker_GetModelCorrectionTransform(pivot);
		}

		public int RotationalDeviceTracker_GetPosePrediction()
		{
			return VuforiaNativeWrapper.rotationalDeviceTracker_GetPosePrediction();
		}

		public int RotationalDeviceTracker_Recenter()
		{
			return VuforiaNativeWrapper.rotationalDeviceTracker_Recenter();
		}

		public int RotationalDeviceTracker_SetModelCorrectionMode(int mode)
		{
			return VuforiaNativeWrapper.rotationalDeviceTracker_SetModelCorrectionMode(mode);
		}

		public int RotationalDeviceTracker_SetModelCorrectionModeWithTransform(int mode, IntPtr pivot)
		{
			return VuforiaNativeWrapper.rotationalDeviceTracker_SetModelCorrectionModeWithTransform(mode, pivot);
		}

		public int RotationalDeviceTracker_SetPosePrediction(bool mode)
		{
			return VuforiaNativeWrapper.rotationalDeviceTracker_SetPosePrediction(mode);
		}

		public void SetApplicationEnvironment(int unityVersionMajor, int unityVersionMinor, int unityVersionChange, int sdkWrapperType)
		{
			VuforiaNativeWrapper.setApplicationEnvironment(unityVersionMajor, unityVersionMinor, unityVersionChange, sdkWrapperType);
		}

		public int SetHolographicAppCS(IntPtr appSpecifiedCS)
		{
			return VuforiaNativeWrapper.setHolographicAppCS(appSpecifiedCS);
		}

		public void SetRenderBuffers(IntPtr colorBuffer)
		{
			VuforiaNativeWrapper.setRenderBuffers(colorBuffer);
		}

		public void SetStateBufferSize(int size)
		{
			VuforiaNativeWrapper.setStateBufferSize(size);
		}

		public int SmartTerrainBuilderAddReconstruction(IntPtr reconstruction)
		{
			return VuforiaNativeWrapper.smartTerrainBuilderAddReconstruction(reconstruction);
		}

		public IntPtr SmartTerrainBuilderCreateReconstructionFromEnvironment()
		{
			return VuforiaNativeWrapper.smartTerrainBuilderCreateReconstructionFromEnvironment();
		}

		public IntPtr SmartTerrainBuilderCreateReconstructionFromTarget()
		{
			return VuforiaNativeWrapper.smartTerrainBuilderCreateReconstructionFromTarget();
		}

		public int SmartTerrainBuilderDestroyReconstruction(IntPtr reconstruction)
		{
			return VuforiaNativeWrapper.smartTerrainBuilderDestroyReconstruction(reconstruction);
		}

		public int SmartTerrainBuilderRemoveReconstruction(IntPtr reconstruction)
		{
			return VuforiaNativeWrapper.smartTerrainBuilderRemoveReconstruction(reconstruction);
		}

		public int SmartTerrainTrackerDeinitBuilder()
		{
			return VuforiaNativeWrapper.smartTerrainTrackerDeinitBuilder();
		}

		public int SmartTerrainTrackerInitBuilder()
		{
			return VuforiaNativeWrapper.smartTerrainTrackerInitBuilder();
		}

		public int SmartTerrainTrackerSetScaleToMillimeter(float scaleToMillimenters)
		{
			return VuforiaNativeWrapper.smartTerrainTrackerSetScaleToMillimeter(scaleToMillimenters);
		}

		public int StartExtendedTracking(IntPtr dataSetPtr, int trackableID)
		{
			return VuforiaNativeWrapper.startExtendedTracking(dataSetPtr, trackableID);
		}

		public int StopExtendedTracking(IntPtr dataSetPtr, int trackableID)
		{
			return VuforiaNativeWrapper.stopExtendedTracking(dataSetPtr, trackableID);
		}

		public void TargetFinderClearTrackables()
		{
			VuforiaNativeWrapper.targetFinderClearTrackables();
		}

		public int TargetFinderDeinit()
		{
			return VuforiaNativeWrapper.targetFinderDeinit();
		}

		public int TargetFinderEnableTracking(IntPtr searchResult, IntPtr trackableData)
		{
			return VuforiaNativeWrapper.targetFinderEnableTracking(searchResult, trackableData);
		}

		public void TargetFinderGetImageTargets(IntPtr trackableIdArray, int trackableIdArrayLength)
		{
			VuforiaNativeWrapper.targetFinderGetImageTargets(trackableIdArray, trackableIdArrayLength);
		}

		public int TargetFinderGetInitState()
		{
			return VuforiaNativeWrapper.targetFinderGetInitState();
		}

		public int TargetFinderGetResults(IntPtr searchResultArray, int searchResultArrayLength)
		{
			return VuforiaNativeWrapper.targetFinderGetResults(searchResultArray, searchResultArrayLength);
		}

		public int TargetFinderStartInit(string userKey, string secretKey)
		{
			return VuforiaNativeWrapper.targetFinderStartInit(userKey, secretKey);
		}

		public int TargetFinderStartRecognition()
		{
			return VuforiaNativeWrapper.targetFinderStartRecognition();
		}

		public int TargetFinderStop()
		{
			return VuforiaNativeWrapper.targetFinderStop();
		}

		public void TargetFinderUpdate(IntPtr targetFinderState, int filterMode)
		{
			VuforiaNativeWrapper.targetFinderUpdate(targetFinderState, filterMode);
		}

		public void TextTrackerGetRegionOfInterest(IntPtr detectionROI, IntPtr trackingROI)
		{
			VuforiaNativeWrapper.textTrackerGetRegionOfInterest(detectionROI, trackingROI);
		}

		public int TextTrackerSetRegionOfInterest(int detectionLeftTopX, int detectionLeftTopY, int detectionRightBottomX, int detectionRightBottomY, int trackingLeftTopX, int trackingLeftTopY, int trackingRightBottomX, int trackingRightBottomY, int upDirection)
		{
			return VuforiaNativeWrapper.textTrackerSetRegionOfInterest(detectionLeftTopX, detectionLeftTopY, detectionRightBottomX, detectionRightBottomY, trackingLeftTopX, trackingLeftTopY, trackingRightBottomX, trackingRightBottomY, upDirection);
		}

		public int TrackerManagerDeinitTracker(int trackerTypeID)
		{
			return VuforiaNativeWrapper.trackerManagerDeinitTracker(trackerTypeID);
		}

		public int TrackerManagerInitTracker(int trackerTypeID)
		{
			return VuforiaNativeWrapper.trackerManagerInitTracker(trackerTypeID);
		}

		public int TrackerStart(int trackerTypeID)
		{
			return VuforiaNativeWrapper.trackerStart(trackerTypeID);
		}

		public void TrackerStop(int trackerTypeID)
		{
			VuforiaNativeWrapper.trackerStop(trackerTypeID);
		}

		public int UpdateQCAR(IntPtr imageHeaderDataArray, int imageHeaderArrayLength, IntPtr frameState, int screenOrientation)
		{
			return VuforiaNativeWrapper.updateQCAR(imageHeaderDataArray, imageHeaderArrayLength, frameState, screenOrientation);
		}

		public int ViewerParameters_ContainsMagnet(IntPtr obj)
		{
			return VuforiaNativeWrapper.viewerParameters_ContainsMagnet(obj);
		}

		public IntPtr ViewerParameters_copy(IntPtr obj)
		{
			return VuforiaNativeWrapper.viewerParameters_copy(obj);
		}

		public void ViewerParameters_delete(IntPtr obj)
		{
			VuforiaNativeWrapper.viewerParameters_delete(obj);
		}

		public int ViewerParameters_GetButtonType(IntPtr obj)
		{
			return VuforiaNativeWrapper.viewerParameters_GetButtonType(obj);
		}

		public float ViewerParameters_GetDistortionCoefficient(IntPtr obj, int idx)
		{
			return VuforiaNativeWrapper.viewerParameters_GetDistortionCoefficient(obj, idx);
		}

		public void ViewerParameters_GetFieldOfView(IntPtr obj, IntPtr result)
		{
			VuforiaNativeWrapper.viewerParameters_GetFieldOfView(obj, result);
		}

		public float ViewerParameters_GetInterLensDistance(IntPtr obj)
		{
			return VuforiaNativeWrapper.viewerParameters_GetInterLensDistance(obj);
		}

		public float ViewerParameters_GetLensCentreToTrayDistance(IntPtr obj)
		{
			return VuforiaNativeWrapper.viewerParameters_GetLensCentreToTrayDistance(obj);
		}

		public IntPtr ViewerParameters_GetManufacturer(IntPtr obj)
		{
			return VuforiaNativeWrapper.viewerParameters_GetManufacturer(obj);
		}

		public IntPtr ViewerParameters_GetName(IntPtr obj)
		{
			return VuforiaNativeWrapper.viewerParameters_GetName(obj);
		}

		public int ViewerParameters_GetNumDistortionCoefficients(IntPtr obj)
		{
			return VuforiaNativeWrapper.viewerParameters_GetNumDistortionCoefficients(obj);
		}

		public float ViewerParameters_GetScreenToLensDistance(IntPtr obj)
		{
			return VuforiaNativeWrapper.viewerParameters_GetScreenToLensDistance(obj);
		}

		public int ViewerParameters_GetTrayAlignment(IntPtr obj)
		{
			return VuforiaNativeWrapper.viewerParameters_GetTrayAlignment(obj);
		}

		public float ViewerParameters_GetVersion(IntPtr obj)
		{
			return VuforiaNativeWrapper.viewerParameters_GetVersion(obj);
		}

		public IntPtr ViewerParametersList_Begin(IntPtr vpList)
		{
			return VuforiaNativeWrapper.viewerParametersList_Begin(vpList);
		}

		public IntPtr ViewerParametersList_GetByIndex(IntPtr vpList, int idx)
		{
			return VuforiaNativeWrapper.viewerParametersList_GetByIndex(vpList, idx);
		}

		public IntPtr ViewerParametersList_GetByNameManufacturer(IntPtr vpList, string name, string manufacturer)
		{
			return VuforiaNativeWrapper.viewerParametersList_GetByNameManufacturer(vpList, name, manufacturer);
		}

		public IntPtr ViewerParametersList_GetListForAuthoringTools()
		{
			return VuforiaNativeWrapper.viewerParametersList_GetListForAuthoringTools();
		}

		public IntPtr ViewerParametersList_Next(IntPtr vpList, IntPtr vpLast)
		{
			return VuforiaNativeWrapper.viewerParametersList_Next(vpList, vpLast);
		}

		public void ViewerParametersList_SetSDKFilter(IntPtr vpList, string filter)
		{
			VuforiaNativeWrapper.viewerParametersList_SetSDKFilter(vpList, filter);
		}

		public int ViewerParametersList_Size(IntPtr vpList)
		{
			return VuforiaNativeWrapper.viewerParametersList_Size(vpList);
		}

		public int VirtualButtonGetId(IntPtr dataSetPtr, string trackableName, string virtualButtonName)
		{
			return VuforiaNativeWrapper.virtualButtonGetId(dataSetPtr, trackableName, virtualButtonName);
		}

		public int VirtualButtonSetAreaRectangle(IntPtr dataSetPtr, string trackableName, string virtualButtonName, IntPtr rectData)
		{
			return VuforiaNativeWrapper.virtualButtonSetAreaRectangle(dataSetPtr, trackableName, virtualButtonName, rectData);
		}

		public int VirtualButtonSetEnabled(IntPtr dataSetPtr, string trackableName, string virtualButtonName, int enabled)
		{
			return VuforiaNativeWrapper.virtualButtonSetEnabled(dataSetPtr, trackableName, virtualButtonName, enabled);
		}

		public int VirtualButtonSetSensitivity(IntPtr dataSetPtr, string trackableName, string virtualButtonName, int sensitivity)
		{
			return VuforiaNativeWrapper.virtualButtonSetSensitivity(dataSetPtr, trackableName, virtualButtonName, sensitivity);
		}

		public IntPtr VuforiaGetRenderEventCallback()
		{
			return VuforiaNativeWrapper.vuforiaGetRenderEventCallback();
		}

		public int VuMarkTargetGetInstanceImage(int vuMarkTargetID, IntPtr instanceImage)
		{
			return VuforiaNativeWrapper.vuMarkTargetGetInstanceImage(vuMarkTargetID, instanceImage);
		}

		public int VuMarkTemplateGetOrigin(IntPtr dataSetPtr, string trackableName, IntPtr originPtr)
		{
			return VuforiaNativeWrapper.vuMarkTemplateGetOrigin(dataSetPtr, trackableName, originPtr);
		}

		public int VuMarkTemplateGetVuMarkUserData(IntPtr dataSetPtr, string trackableName, StringBuilder data, uint dataLength)
		{
			return VuforiaNativeWrapper.vuMarkTemplateGetVuMarkUserData(dataSetPtr, trackableName, data, dataLength);
		}

		public int VuMarkTemplateSetTrackingFromRuntimeAppearance(IntPtr dataSetPtr, string trackableName, bool enable)
		{
			return VuforiaNativeWrapper.vuMarkTemplateSetTrackingFromRuntimeAppearance(dataSetPtr, trackableName, enable);
		}

		public int WordGetLetterBoundingBoxes(int wordID, IntPtr letterBoundingBoxes)
		{
			return VuforiaNativeWrapper.wordGetLetterBoundingBoxes(wordID, letterBoundingBoxes);
		}

		public int WordGetLetterMask(int wordID, IntPtr letterMaskImage)
		{
			return VuforiaNativeWrapper.wordGetLetterMask(wordID, letterMaskImage);
		}

		public int WordListAddWordsFromFile(string path, int storageType)
		{
			return VuforiaNativeWrapper.wordListAddWordsFromFile(path, storageType);
		}

		public int WordListAddWordToFilterListU(IntPtr word)
		{
			return VuforiaNativeWrapper.wordListAddWordToFilterListU(word);
		}

		public int WordListAddWordU(IntPtr word)
		{
			return VuforiaNativeWrapper.wordListAddWordU(word);
		}

		public int WordListClearFilterList()
		{
			return VuforiaNativeWrapper.wordListClearFilterList();
		}

		public int WordListContainsWordU(IntPtr word)
		{
			return VuforiaNativeWrapper.wordListContainsWordU(word);
		}

		public int WordListGetFilterListWordCount()
		{
			return VuforiaNativeWrapper.wordListGetFilterListWordCount();
		}

		public IntPtr WordListGetFilterListWordU(int i)
		{
			return VuforiaNativeWrapper.wordListGetFilterListWordU(i);
		}

		public int WordListGetFilterMode()
		{
			return VuforiaNativeWrapper.wordListGetFilterMode();
		}

		public int WordListLoadFilterList(string path, int storageType)
		{
			return VuforiaNativeWrapper.wordListLoadFilterList(path, storageType);
		}

		public int WordListLoadWordList(string path, int storageType)
		{
			return VuforiaNativeWrapper.wordListLoadWordList(path, storageType);
		}

		public int WordListRemoveWordFromFilterListU(IntPtr word)
		{
			return VuforiaNativeWrapper.wordListRemoveWordFromFilterListU(word);
		}

		public int WordListRemoveWordU(IntPtr word)
		{
			return VuforiaNativeWrapper.wordListRemoveWordU(word);
		}

		public int WordListSetFilterMode(int mode)
		{
			return VuforiaNativeWrapper.wordListSetFilterMode(mode);
		}

		public int WordListUnloadAllLists()
		{
			return VuforiaNativeWrapper.wordListUnloadAllLists();
		}

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceDeinitCamera();

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceGetCameraDirection();

		[DllImport("VuforiaWrapper")]
		private static extern void cameraDeviceGetCameraField(IntPtr cameraField, int idx);

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceGetCameraFieldOfViewRads(IntPtr fovVectorContainer);

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceGetFieldBool(string key, IntPtr value);

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceGetFieldFloat(string key, IntPtr value);

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceGetFieldInt64(string key, IntPtr value);

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceGetFieldInt64Range(string key, IntPtr intRange);

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceGetFieldString(string key, StringBuilder value, int maxLength);

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceGetNumCameraFields();

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceGetNumVideoModes();

		[DllImport("VuforiaWrapper")]
		private static extern void cameraDeviceGetVideoMode(int idx, IntPtr videoMode);

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceInitCamera(int camera);

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceSelectVideoMode(int idx);

		[DllImport("VuforiaWrapper")]
		private static extern void cameraDeviceSetCameraConfiguration(int width, int height);

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceSetFieldBool(string key, bool value);

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceSetFieldFloat(string key, float value);

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceSetFieldInt64(string key, long value);

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceSetFieldInt64Range(string key, long intRangeFrom, long intRangeTo);

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceSetFieldString(string key, string value);

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceSetFlashTorchMode(int on);

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceSetFocusMode(int focusMode);

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceStartCamera();

		[DllImport("VuforiaWrapper")]
		private static extern int cameraDeviceStopCamera();

		[DllImport("VuforiaWrapper")]
		private static extern void customViewerParameters_AddDistortionCoefficient(IntPtr obj, float val);

		[DllImport("VuforiaWrapper")]
		private static extern void customViewerParameters_ClearDistortionCoefficients(IntPtr obj);

		[DllImport("VuforiaWrapper")]
		private static extern void customViewerParameters_delete(IntPtr obj);

		[DllImport("VuforiaWrapper")]
		private static extern IntPtr customViewerParameters_new(float version, string name, string manufacturer);

		[DllImport("VuforiaWrapper")]
		private static extern void customViewerParameters_SetButtonType(IntPtr obj, int val);

		[DllImport("VuforiaWrapper")]
		private static extern void customViewerParameters_SetContainsMagnet(IntPtr obj, bool val);

		[DllImport("VuforiaWrapper")]
		private static extern void customViewerParameters_SetFieldOfView(IntPtr obj, IntPtr val);

		[DllImport("VuforiaWrapper")]
		private static extern void customViewerParameters_SetInterLensDistance(IntPtr obj, float val);

		[DllImport("VuforiaWrapper")]
		private static extern void customViewerParameters_SetLensCentreToTrayDistance(IntPtr obj, float val);

		[DllImport("VuforiaWrapper")]
		private static extern void customViewerParameters_SetScreenToLensDistance(IntPtr obj, float val);

		[DllImport("VuforiaWrapper")]
		private static extern void customViewerParameters_SetTrayAlignment(IntPtr obj, int val);

		[DllImport("VuforiaWrapper")]
		private static extern int cylinderTargetGetDimensions(IntPtr dataSetPtr, string trackableName, IntPtr dimensionPtr);

		[DllImport("VuforiaWrapper")]
		private static extern int cylinderTargetSetBottomDiameter(IntPtr dataSetPtr, string trackableName, float bottomDiameter);

		[DllImport("VuforiaWrapper")]
		private static extern int cylinderTargetSetSideLength(IntPtr dataSetPtr, string trackableName, float sideLength);

		[DllImport("VuforiaWrapper")]
		private static extern int cylinderTargetSetTopDiameter(IntPtr dataSetPtr, string trackableName, float topDiameter);

		[DllImport("VuforiaWrapper")]
		private static extern int dataSetCreateTrackable(IntPtr dataSetPtr, IntPtr trackableSourcePtr, StringBuilder trackableName, int nameMaxLength, IntPtr trackableData);

		[DllImport("VuforiaWrapper")]
		private static extern int dataSetDestroyTrackable(IntPtr dataSetPtr, int trackableId);

		[DllImport("VuforiaWrapper")]
		private static extern int dataSetExists(string relativePath, int storageType);

		[DllImport("VuforiaWrapper")]
		private static extern int dataSetGetNumTrackableType(int trackableType, IntPtr dataSetPtr);

		[DllImport("VuforiaWrapper")]
		private static extern int dataSetGetTrackableName(IntPtr dataSetPtr, int trackableId, StringBuilder trackableName, int nameMaxLength);

		[DllImport("VuforiaWrapper")]
		private static extern int dataSetGetTrackablesOfType(int trackableType, IntPtr trackableDataArray, int trackableDataArrayLength, IntPtr dataSetPtr);

		[DllImport("VuforiaWrapper")]
		private static extern int dataSetHasReachedTrackableLimit(IntPtr dataSetPtr);

		[DllImport("VuforiaWrapper")]
		private static extern int dataSetLoad(string relativePath, int storageType, IntPtr dataSetPtr);

		[DllImport("VuforiaWrapper")]
		private static extern void deinitFrameState(IntPtr frameState);

		[DllImport("VuforiaWrapper")]
		private static extern int device_GetMode();

		[DllImport("VuforiaWrapper")]
		private static extern IntPtr device_GetSelectedViewer();

		[DllImport("VuforiaWrapper")]
		private static extern IntPtr device_GetViewerList();

		[DllImport("VuforiaWrapper")]
		private static extern int device_IsViewerPresent();

		[DllImport("VuforiaWrapper")]
		private static extern int device_SelectViewer(IntPtr vp);

		[DllImport("VuforiaWrapper")]
		private static extern int device_SetMode(int mode);

		[DllImport("VuforiaWrapper")]
		private static extern void device_SetViewerPresent(bool present);

		[DllImport("VuforiaWrapper")]
		private static extern int deviceIsEyewearDevice();

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearCPMClearProfile(int profileID);

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearCPMGetActiveProfile();

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearCPMGetCameraToEyePose(int profileID, int eyeID, IntPtr matrix);

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearCPMGetEyeProjection(int profileID, int eyeID, IntPtr matrix);

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearCPMGetMaxCount();

		[DllImport("VuforiaWrapper")]
		private static extern IntPtr eyewearCPMGetProfileName(int profileID);

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearCPMGetUsedCount();

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearCPMIsProfileUsed(int profileID);

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearCPMSetActiveProfile(int profileID);

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearCPMSetCameraToEyePose(int profileID, int eyeID, IntPtr matrix);

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearCPMSetEyeProjection(int profileID, int eyeID, IntPtr matrix);

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearCPMSetProfileName(int profileID, IntPtr name);

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearDeviceGetScreenOrientation();

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearDeviceIsDisplayExtended();

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearDeviceIsDisplayExtendedGLOnly();

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearDeviceIsDualDisplay();

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearDeviceIsPredictiveTrackingEnabled();

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearDeviceIsSeeThru();

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearDeviceSetDisplayExtended(bool enable);

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearDeviceSetPredictiveTracking(bool enable);

		[DllImport("VuforiaWrapper")]
		private static extern float eyewearUserCalibratorGetMaxScaleHint();

		[DllImport("VuforiaWrapper")]
		private static extern float eyewearUserCalibratorGetMinScaleHint();

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearUserCalibratorGetProjectionMatrix(IntPtr readingsDataArray, int numReadings, IntPtr cameraToEyeContainer, IntPtr projectionContainer);

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearUserCalibratorInit(int surfaceWidth, int surfaceHeight, int targetWidth, int targetHeight);

		[DllImport("VuforiaWrapper")]
		private static extern int eyewearUserCalibratorIsStereoStretched();

		[DllImport("VuforiaWrapper")]
		private static extern void frameCounterGetBenchmarkingData(IntPtr benchmarkingData);

		[DllImport("VuforiaWrapper")]
		private static extern int getCameraThreadID();

		[DllImport("VuforiaWrapper")]
		private static extern int getProjectionGL(float nearPlane, float farPlane, IntPtr projectionContainer, int screenOrientation);

		[DllImport("VuforiaWrapper")]
		private static extern void getVuforiaLibraryVersion(StringBuilder value, int maxLength);

		[DllImport("VuforiaWrapper")]
		private static extern int hasSurfaceBeenRecreated();

		[DllImport("VuforiaWrapper")]
		private static extern int imageTargetBuilderBuild(string name, float screenSizeWidth);

		[DllImport("VuforiaWrapper")]
		private static extern int imageTargetBuilderGetFrameQuality();

		[DllImport("VuforiaWrapper")]
		private static extern IntPtr imageTargetBuilderGetTrackableSource();

		[DllImport("VuforiaWrapper")]
		private static extern void imageTargetBuilderStartScan();

		[DllImport("VuforiaWrapper")]
		private static extern void imageTargetBuilderStopScan();

		[DllImport("VuforiaWrapper")]
		private static extern int imageTargetCreateVirtualButton(IntPtr dataSetPtr, string trackableName, string virtualButtonName, IntPtr rectData);

		[DllImport("VuforiaWrapper")]
		private static extern int imageTargetDestroyVirtualButton(IntPtr dataSetPtr, string trackableName, string virtualButtonName);

		[DllImport("VuforiaWrapper")]
		private static extern int imageTargetGetNumVirtualButtons(IntPtr dataSetPtr, string trackableName);

		[DllImport("VuforiaWrapper")]
		private static extern int imageTargetGetVirtualButtonName(IntPtr dataSetPtr, string trackableName, int idx, StringBuilder vbName, int nameMaxLength);

		[DllImport("VuforiaWrapper")]
		private static extern int imageTargetGetVirtualButtons(IntPtr virtualButtonDataArray, IntPtr rectangleDataArray, int virtualButtonDataArrayLength, IntPtr dataSetPtr, string trackableName);

		[DllImport("VuforiaWrapper")]
		private static extern void initFrameState(IntPtr frameState);

		[DllImport("VuforiaWrapper")]
		private static extern void initPlatformNative();

		[DllImport("VuforiaWrapper")]
		private static extern float multiTargetGetLargestSizeComponent(IntPtr dataSetPtr, string trackableName);

		[DllImport("VuforiaWrapper")]
		private static extern int objectTargetGetSize(IntPtr dataSetPtr, string trackableName, IntPtr sizePtr);

		[DllImport("VuforiaWrapper")]
		private static extern int objectTargetSetSize(IntPtr dataSetPtr, string trackableName, IntPtr sizePtr);

		[DllImport("VuforiaWrapper")]
		private static extern int objectTrackerActivateDataSet(IntPtr dataSetPtr);

		[DllImport("VuforiaWrapper")]
		private static extern IntPtr objectTrackerCreateDataSet();

		[DllImport("VuforiaWrapper")]
		private static extern int objectTrackerDeactivateDataSet(IntPtr dataSetPtr);

		[DllImport("VuforiaWrapper")]
		private static extern int objectTrackerDestroyDataSet(IntPtr dataSetPtr);

		[DllImport("VuforiaWrapper")]
		private static extern int objectTrackerPersistExtendedTracking(int on);

		[DllImport("VuforiaWrapper")]
		private static extern int objectTrackerResetExtendedTracking();

		[DllImport("VuforiaWrapper")]
		private static extern void onPause();

		[DllImport("VuforiaWrapper")]
		private static extern void onResume();

		[DllImport("VuforiaWrapper")]
		private static extern void onSurfaceChanged(int width, int height);

		[DllImport("VuforiaWrapper")]
		private static extern void qcarAddCameraFrame(IntPtr pixels, int width, int height, int format, int stride, int frameIdx, int flipHorizontally);

		[DllImport("VuforiaWrapper")]
		private static extern void qcarDeinit();

		[DllImport("VuforiaWrapper")]
		private static extern int qcarGetBufferSize(int width, int height, int format);

		[DllImport("VuforiaWrapper")]
		private static extern int qcarInit(string licenseKey);

		[DllImport("VuforiaWrapper")]
		private static extern int qcarSetFrameFormat(int format, int enabled);

		[DllImport("VuforiaWrapper")]
		private static extern int qcarSetHint(uint hint, int value);

		[DllImport("VuforiaWrapper")]
		private static extern int reconstructioFromEnvironmentGetReconstructionState(IntPtr reconstruction);

		[DllImport("VuforiaWrapper")]
		private static extern int reconstructionFromTargetSetInitializationTarget(IntPtr reconstruction, IntPtr dataSetPtr, int trackableID, IntPtr occluderMin, IntPtr occluderMax, IntPtr offsetToOccluder, IntPtr rotationAxisToOccluder, float rotationAngleToOccluder);

		[DllImport("VuforiaWrapper")]
		private static extern int reconstructionIsReconstructing(IntPtr reconstruction);

		[DllImport("VuforiaWrapper")]
		private static extern int reconstructionReset(IntPtr reconstruction);

		[DllImport("VuforiaWrapper")]
		private static extern int reconstructionSetMaximumArea(IntPtr reconstruction, IntPtr maximumArea);

		[DllImport("VuforiaWrapper")]
		private static extern void reconstructionSetNavMeshPadding(IntPtr reconstruction, float padding);

		[DllImport("VuforiaWrapper")]
		private static extern int reconstructionStart(IntPtr reconstruction);

		[DllImport("VuforiaWrapper")]
		private static extern int reconstructionStop(IntPtr reconstruction);

		[DllImport("VuforiaWrapper")]
		private static extern IntPtr rendererCreateNativeTexture(uint width, uint height, int format);

		[DllImport("VuforiaWrapper")]
		private static extern void rendererEnd();

		[DllImport("VuforiaWrapper")]
		private static extern int rendererGetGraphicsAPI();

		[DllImport("VuforiaWrapper")]
		private static extern int rendererGetRecommendedFps(int flags);

		[DllImport("VuforiaWrapper")]
		private static extern void rendererGetVideoBackgroundCfg(IntPtr bgCfg);

		[DllImport("VuforiaWrapper")]
		private static extern int rendererGetVideoBackgroundTextureInfo(IntPtr texInfo);

		[DllImport("VuforiaWrapper")]
		private static extern int rendererIsVideoBackgroundTextureInfoAvailable();

		[DllImport("VuforiaWrapper")]
		private static extern void rendererSetVideoBackgroundCfg(IntPtr bgCfg);

		[DllImport("VuforiaWrapper")]
		private static extern int rendererSetVideoBackgroundTextureID(int textureID);

		[DllImport("VuforiaWrapper")]
		private static extern int rendererSetVideoBackgroundTexturePtr(IntPtr texturePtr);

		[DllImport("VuforiaWrapper")]
		private static extern void renderingPrimitives_DeleteCopy();

		[DllImport("VuforiaWrapper")]
		private static extern void renderingPrimitives_GetDistortionMesh(int viewId, IntPtr meshData);

		[DllImport("VuforiaWrapper")]
		private static extern void renderingPrimitives_GetDistortionMeshSize(int viewId, IntPtr size);

		[DllImport("VuforiaWrapper")]
		private static extern void renderingPrimitives_GetDistortionTextureViewport(int viewID, IntPtr viewportContainer);

		[DllImport("VuforiaWrapper")]
		private static extern void renderingPrimitives_GetEffectiveFov(int viewID, IntPtr fovContainer);

		[DllImport("VuforiaWrapper")]
		private static extern void renderingPrimitives_GetEyeDisplayAdjustmentMatrix(int viewID, IntPtr matrixContainer);

		[DllImport("VuforiaWrapper")]
		private static extern void renderingPrimitives_GetNormalizedViewport(int viewID, IntPtr viewportContainer);

		[DllImport("VuforiaWrapper")]
		private static extern void renderingPrimitives_GetProjectionMatrix(int viewID, float near, float far, IntPtr projectionContainer, int screenOrientation);

		[DllImport("VuforiaWrapper")]
		private static extern void renderingPrimitives_GetViewport(int viewID, IntPtr viewportContainer);

		[DllImport("VuforiaWrapper")]
		private static extern void renderingPrimitives_GetViewportCentreToEyeAxis(int viewID, IntPtr vectorContainer);

		[DllImport("VuforiaWrapper")]
		private static extern void renderingPrimitives_UpdateCopy();

		[DllImport("VuforiaWrapper")]
		private static extern int rotationalDeviceTracker_GetModelCorrectionMode();

		[DllImport("VuforiaWrapper")]
		private static extern void rotationalDeviceTracker_GetModelCorrectionTransform(IntPtr pivot);

		[DllImport("VuforiaWrapper")]
		private static extern int rotationalDeviceTracker_GetPosePrediction();

		[DllImport("VuforiaWrapper")]
		private static extern int rotationalDeviceTracker_Recenter();

		[DllImport("VuforiaWrapper")]
		private static extern int rotationalDeviceTracker_SetModelCorrectionMode(int mode);

		[DllImport("VuforiaWrapper")]
		private static extern int rotationalDeviceTracker_SetModelCorrectionModeWithTransform(int mode, IntPtr pivot);

		[DllImport("VuforiaWrapper")]
		private static extern int rotationalDeviceTracker_SetPosePrediction(bool mode);

		[DllImport("VuforiaWrapper")]
		private static extern void setApplicationEnvironment(int unityVersionMajor, int unityVersionMinor, int unityVersionChange, int sdkWrapperType);

		[DllImport("VuforiaWrapper")]
		private static extern int setHolographicAppCS(IntPtr appSpecifiedCS);

		[DllImport("VuforiaWrapper")]
		private static extern void setRenderBuffers(IntPtr colorBuffer);

		[DllImport("VuforiaWrapper")]
		private static extern void setStateBufferSize(int size);

		[DllImport("VuforiaWrapper")]
		private static extern int smartTerrainBuilderAddReconstruction(IntPtr reconstruction);

		[DllImport("VuforiaWrapper")]
		private static extern IntPtr smartTerrainBuilderCreateReconstructionFromEnvironment();

		[DllImport("VuforiaWrapper")]
		private static extern IntPtr smartTerrainBuilderCreateReconstructionFromTarget();

		[DllImport("VuforiaWrapper")]
		private static extern int smartTerrainBuilderDestroyReconstruction(IntPtr reconstruction);

		[DllImport("VuforiaWrapper")]
		private static extern int smartTerrainBuilderRemoveReconstruction(IntPtr reconstruction);

		[DllImport("VuforiaWrapper")]
		private static extern int smartTerrainTrackerDeinitBuilder();

		[DllImport("VuforiaWrapper")]
		private static extern int smartTerrainTrackerInitBuilder();

		[DllImport("VuforiaWrapper")]
		private static extern int smartTerrainTrackerSetScaleToMillimeter(float scaleToMillimenters);

		[DllImport("VuforiaWrapper")]
		private static extern int startExtendedTracking(IntPtr dataSetPtr, int trackableID);

		[DllImport("VuforiaWrapper")]
		private static extern int stopExtendedTracking(IntPtr dataSetPtr, int trackableID);

		[DllImport("VuforiaWrapper")]
		private static extern void targetFinderClearTrackables();

		[DllImport("VuforiaWrapper")]
		private static extern int targetFinderDeinit();

		[DllImport("VuforiaWrapper")]
		private static extern int targetFinderEnableTracking(IntPtr searchResult, IntPtr trackableData);

		[DllImport("VuforiaWrapper")]
		private static extern void targetFinderGetImageTargets(IntPtr trackableIdArray, int trackableIdArrayLength);

		[DllImport("VuforiaWrapper")]
		private static extern int targetFinderGetInitState();

		[DllImport("VuforiaWrapper")]
		private static extern int targetFinderGetResults(IntPtr searchResultArray, int searchResultArrayLength);

		[DllImport("VuforiaWrapper")]
		private static extern int targetFinderStartInit(string userKey, string secretKey);

		[DllImport("VuforiaWrapper")]
		private static extern int targetFinderStartRecognition();

		[DllImport("VuforiaWrapper")]
		private static extern int targetFinderStop();

		[DllImport("VuforiaWrapper")]
		private static extern void targetFinderUpdate(IntPtr targetFinderState, int filterMode);

		[DllImport("VuforiaWrapper")]
		private static extern void textTrackerGetRegionOfInterest(IntPtr detectionROI, IntPtr trackingROI);

		[DllImport("VuforiaWrapper")]
		private static extern int textTrackerSetRegionOfInterest(int detectionLeftTopX, int detectionLeftTopY, int detectionRightBottomX, int detectionRightBottomY, int trackingLeftTopX, int trackingLeftTopY, int trackingRightBottomX, int trackingRightBottomY, int upDirection);

		[DllImport("VuforiaWrapper")]
		private static extern int trackerManagerDeinitTracker(int trackerTypeID);

		[DllImport("VuforiaWrapper")]
		private static extern int trackerManagerInitTracker(int trackerTypeID);

		[DllImport("VuforiaWrapper")]
		private static extern int trackerStart(int trackerTypeID);

		[DllImport("VuforiaWrapper")]
		private static extern void trackerStop(int trackerTypeID);

		[DllImport("VuforiaWrapper")]
		private static extern int updateQCAR(IntPtr imageHeaderDataArray, int imageHeaderArrayLength, IntPtr frameState, int screenOrientation);

		[DllImport("VuforiaWrapper")]
		private static extern int viewerParameters_ContainsMagnet(IntPtr obj);

		[DllImport("VuforiaWrapper")]
		private static extern IntPtr viewerParameters_copy(IntPtr obj);

		[DllImport("VuforiaWrapper")]
		private static extern void viewerParameters_delete(IntPtr obj);

		[DllImport("VuforiaWrapper")]
		private static extern int viewerParameters_GetButtonType(IntPtr obj);

		[DllImport("VuforiaWrapper")]
		private static extern float viewerParameters_GetDistortionCoefficient(IntPtr obj, int idx);

		[DllImport("VuforiaWrapper")]
		private static extern void viewerParameters_GetFieldOfView(IntPtr obj, IntPtr result);

		[DllImport("VuforiaWrapper")]
		private static extern float viewerParameters_GetInterLensDistance(IntPtr obj);

		[DllImport("VuforiaWrapper")]
		private static extern float viewerParameters_GetLensCentreToTrayDistance(IntPtr obj);

		[DllImport("VuforiaWrapper")]
		private static extern IntPtr viewerParameters_GetManufacturer(IntPtr obj);

		[DllImport("VuforiaWrapper")]
		private static extern IntPtr viewerParameters_GetName(IntPtr obj);

		[DllImport("VuforiaWrapper")]
		private static extern int viewerParameters_GetNumDistortionCoefficients(IntPtr obj);

		[DllImport("VuforiaWrapper")]
		private static extern float viewerParameters_GetScreenToLensDistance(IntPtr obj);

		[DllImport("VuforiaWrapper")]
		private static extern int viewerParameters_GetTrayAlignment(IntPtr obj);

		[DllImport("VuforiaWrapper")]
		private static extern float viewerParameters_GetVersion(IntPtr obj);

		[DllImport("VuforiaWrapper")]
		private static extern IntPtr viewerParametersList_Begin(IntPtr vpList);

		[DllImport("VuforiaWrapper")]
		private static extern IntPtr viewerParametersList_GetByIndex(IntPtr vpList, int idx);

		[DllImport("VuforiaWrapper")]
		private static extern IntPtr viewerParametersList_GetByNameManufacturer(IntPtr vpList, string name, string manufacturer);

		[DllImport("VuforiaWrapper")]
		private static extern IntPtr viewerParametersList_GetListForAuthoringTools();

		[DllImport("VuforiaWrapper")]
		private static extern IntPtr viewerParametersList_Next(IntPtr vpList, IntPtr vpLast);

		[DllImport("VuforiaWrapper")]
		private static extern void viewerParametersList_SetSDKFilter(IntPtr vpList, string filter);

		[DllImport("VuforiaWrapper")]
		private static extern int viewerParametersList_Size(IntPtr vpList);

		[DllImport("VuforiaWrapper")]
		private static extern int virtualButtonGetId(IntPtr dataSetPtr, string trackableName, string virtualButtonName);

		[DllImport("VuforiaWrapper")]
		private static extern int virtualButtonSetAreaRectangle(IntPtr dataSetPtr, string trackableName, string virtualButtonName, IntPtr rectData);

		[DllImport("VuforiaWrapper")]
		private static extern int virtualButtonSetEnabled(IntPtr dataSetPtr, string trackableName, string virtualButtonName, int enabled);

		[DllImport("VuforiaWrapper")]
		private static extern int virtualButtonSetSensitivity(IntPtr dataSetPtr, string trackableName, string virtualButtonName, int sensitivity);

		[DllImport("VuforiaWrapper")]
		private static extern IntPtr vuforiaGetRenderEventCallback();

		[DllImport("VuforiaWrapper")]
		private static extern int vuMarkTargetGetInstanceImage(int vuMarkTargetID, IntPtr instanceImage);

		[DllImport("VuforiaWrapper")]
		private static extern int vuMarkTemplateGetOrigin(IntPtr dataSetPtr, string trackableName, IntPtr originPtr);

		[DllImport("VuforiaWrapper")]
		private static extern int vuMarkTemplateGetVuMarkUserData(IntPtr dataSetPtr, string trackableName, StringBuilder data, uint dataLength);

		[DllImport("VuforiaWrapper")]
		private static extern int vuMarkTemplateSetTrackingFromRuntimeAppearance(IntPtr dataSetPtr, string trackableName, bool enable);

		[DllImport("VuforiaWrapper")]
		private static extern int wordGetLetterBoundingBoxes(int wordID, IntPtr letterBoundingBoxes);

		[DllImport("VuforiaWrapper")]
		private static extern int wordGetLetterMask(int wordID, IntPtr letterMaskImage);

		[DllImport("VuforiaWrapper")]
		private static extern int wordListAddWordsFromFile(string path, int storageType);

		[DllImport("VuforiaWrapper")]
		private static extern int wordListAddWordToFilterListU(IntPtr word);

		[DllImport("VuforiaWrapper")]
		private static extern int wordListAddWordU(IntPtr word);

		[DllImport("VuforiaWrapper")]
		private static extern int wordListClearFilterList();

		[DllImport("VuforiaWrapper")]
		private static extern int wordListContainsWordU(IntPtr word);

		[DllImport("VuforiaWrapper")]
		private static extern int wordListGetFilterListWordCount();

		[DllImport("VuforiaWrapper")]
		private static extern IntPtr wordListGetFilterListWordU(int i);

		[DllImport("VuforiaWrapper")]
		private static extern int wordListGetFilterMode();

		[DllImport("VuforiaWrapper")]
		private static extern int wordListLoadFilterList(string path, int storageType);

		[DllImport("VuforiaWrapper")]
		private static extern int wordListLoadWordList(string path, int storageType);

		[DllImport("VuforiaWrapper")]
		private static extern int wordListRemoveWordFromFilterListU(IntPtr word);

		[DllImport("VuforiaWrapper")]
		private static extern int wordListRemoveWordU(IntPtr word);

		[DllImport("VuforiaWrapper")]
		private static extern int wordListSetFilterMode(int mode);

		[DllImport("VuforiaWrapper")]
		private static extern int wordListUnloadAllLists();
	}
}
