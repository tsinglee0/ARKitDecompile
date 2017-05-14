using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	internal class SceneManager
	{
		private EditorApplication.CallbackFunction mUpdateCallback;

		private bool mDoDeserialization;

		private bool mApplyProperties;

		private bool mApplyAppearance;

		private bool mSceneInitialized;

		private bool mValidateScene;

		private bool mGoToTargetManagerPage;

		private bool mGoToSampleAppPage;

		private static SceneManager mInstance;

		private bool mUnloadUnusedAssets;

		public static SceneManager Instance
		{
			get
			{
				if (SceneManager.mInstance == null)
				{
					Type typeFromHandle = typeof(SceneManager);
					lock (typeFromHandle)
					{
						if (SceneManager.mInstance == null)
						{
							SceneManager.mInstance = new SceneManager();
						}
					}
				}
				return SceneManager.mInstance;
			}
		}

		public bool SceneInitialized
		{
			get
			{
				return this.mSceneInitialized;
			}
		}

		private SceneManager()
		{
			this.mUpdateCallback = new EditorApplication.CallbackFunction(this.EditorUpdate);
			if (EditorApplication.update == null)
			{
				EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.update, this.mUpdateCallback);
			}
			else if (!EditorApplication.update.Equals(this.mUpdateCallback))
			{
				EditorApplication.update = (EditorApplication.CallbackFunction)Delegate.Combine(EditorApplication.update, this.mUpdateCallback);
			}
			this.mDoDeserialization = true;
			this.mSceneInitialized = false;
		}

		public void InitScene()
		{
			DataSetTrackableBehaviour[] trackables = (DataSetTrackableBehaviour[])UnityEngine.Object.FindObjectsOfType(typeof(DataSetTrackableBehaviour));
			ConfigDataManager.Instance.DoRead();
			this.UpdateTrackableAppearance(trackables);
			this.mValidateScene = true;
			this.mSceneInitialized = true;
		}

		public void EditorUpdate()
		{
			TrackableBehaviour[] trackables = (TrackableBehaviour[])UnityEngine.Object.FindObjectsOfType(typeof(TrackableBehaviour));
			VirtualButtonAbstractBehaviour[] arg_31_0 = (VirtualButtonAbstractBehaviour[])UnityEngine.Object.FindObjectsOfType(typeof(VirtualButtonAbstractBehaviour));
			this.CorrectTrackableScales(trackables);
			VirtualButtonEditor.CorrectPoses(arg_31_0);
			if (this.mDoDeserialization)
			{
				ConfigDataManager.Instance.DoRead();
				bool flag = DatabaseLoadEditor.OnConfigDataChanged();
				this.ApplyDataSetAppearance();
				this.mDoDeserialization = !flag;
			}
			if (this.mApplyAppearance)
			{
				this.UpdateTrackableAppearance(trackables);
				this.mApplyAppearance = false;
			}
			if (this.mApplyProperties)
			{
				this.UpdateTrackableProperties(trackables);
				this.mApplyProperties = false;
			}
			if (this.mValidateScene)
			{
				this.CheckForDuplicates(trackables);
				WordEditor.CheckForDuplicates();
				VirtualButtonEditor.Validate();
				this.mValidateScene = false;
			}
			if (this.mGoToTargetManagerPage)
			{
				this.mGoToTargetManagerPage = false;
				Process.Start("https://developer.vuforia.com/target-manager");
			}
			if (this.mGoToSampleAppPage)
			{
				this.mGoToSampleAppPage = false;
				Process.Start("https://developer.vuforia.com/downloads/samples");
			}
			if (this.mUnloadUnusedAssets)
			{
				Resources.UnloadUnusedAssets();
				this.mUnloadUnusedAssets = false;
			}
		}

		public void UnloadUnusedAssets()
		{
			this.mUnloadUnusedAssets = true;
		}

		public string[] GetImageTargetNames(string dataSetName)
		{
			ConfigData expr_0B = ConfigDataManager.Instance.GetConfigData(dataSetName);
			string[] array = new string[expr_0B.NumImageTargets + 1];
			array[0] = "--- EMPTY ---";
			expr_0B.CopyImageTargetNames(array, 1);
			return array;
		}

		public string[] GetMultiTargetNames(string dataSetName)
		{
			ConfigData expr_0B = ConfigDataManager.Instance.GetConfigData(dataSetName);
			string[] array = new string[expr_0B.NumMultiTargets + 1];
			array[0] = "--- EMPTY ---";
			expr_0B.CopyMultiTargetNames(array, 1);
			return array;
		}

		public string[] GetCylinderTargetNames(string dataSetName)
		{
			ConfigData expr_0B = ConfigDataManager.Instance.GetConfigData(dataSetName);
			string[] array = new string[expr_0B.NumCylinderTargets + 1];
			array[0] = "--- EMPTY ---";
			expr_0B.CopyCylinderTargetNames(array, 1);
			return array;
		}

		public string[] GetObjectTargetNames(string dataSetName)
		{
			ConfigData expr_0B = ConfigDataManager.Instance.GetConfigData(dataSetName);
			string[] array = new string[expr_0B.NumObjectTargets + 1];
			array[0] = "--- EMPTY ---";
			expr_0B.CopyObjectTargetNames(array, 1);
			return array;
		}

		public string[] GetVuMarkTargetNames(string dataSetName)
		{
			ConfigData expr_0B = ConfigDataManager.Instance.GetConfigData(dataSetName);
			string[] array = new string[expr_0B.NumVuMarkTargets + 1];
			array[0] = "--- EMPTY ---";
			expr_0B.CopyVuMarkTargetNames(array, 1);
			return array;
		}

		public bool ExtendedTrackingEnabledOnATarget()
		{
			DataSetTrackableBehaviour[] array = UnityEngine.Object.FindObjectsOfType<DataSetTrackableBehaviour>();
			for (int i = 0; i < array.Length; i++)
			{
				if (((IEditDataSetBehaviour)array[i]).ExtendedTracking)
				{
					return true;
				}
			}
			return false;
		}

		public bool SmartTerrainInitializationEnabled()
		{
			if (VuforiaAbstractConfigurationEditor.LoadConfigurationObject().SmartTerrainTracker.AutoInitAndStartTracker)
			{
				return true;
			}
			DataSetTrackableBehaviour[] array = UnityEngine.Object.FindObjectsOfType<DataSetTrackableBehaviour>();
			for (int i = 0; i < array.Length; i++)
			{
				IEditDataSetBehaviour editDataSetBehaviour = array[i];
				if (editDataSetBehaviour.InitializeSmartTerrain && editDataSetBehaviour.ReconstructionToInitialize != null)
				{
					return true;
				}
			}
			return false;
		}

		public void SceneUpdated()
		{
			this.mValidateScene = true;
		}

		public void FilesUpdated()
		{
			this.mDoDeserialization = true;
		}

		public void ApplyDataSetProperties()
		{
			this.mApplyProperties = true;
		}

		public void ApplyDataSetAppearance()
		{
			this.mApplyAppearance = true;
		}

		public void GoToTargetManagerPage()
		{
			this.mGoToTargetManagerPage = true;
		}

		public void GoToSampleAppPage()
		{
			this.mGoToSampleAppPage = true;
		}

		private void UpdateTrackableAppearance(TrackableBehaviour[] trackables)
		{
			if (!Application.isPlaying)
			{
				for (int i = 0; i < trackables.Length; i++)
				{
					TrackableBehaviour trackableBehaviour = trackables[i];
					if (trackableBehaviour is DataSetTrackableBehaviour)
					{
						TrackableAccessor trackableAccessor = AccessorFactory.Create((DataSetTrackableBehaviour)trackableBehaviour);
						if (trackableAccessor != null)
						{
							trackableAccessor.ApplyDataSetAppearance();
						}
					}
				}
			}
		}

		private void UpdateTrackableProperties(TrackableBehaviour[] trackables)
		{
			for (int i = 0; i < trackables.Length; i++)
			{
				TrackableBehaviour trackableBehaviour = trackables[i];
				if (trackableBehaviour is DataSetTrackableBehaviour)
				{
					TrackableAccessor trackableAccessor = AccessorFactory.Create((DataSetTrackableBehaviour)trackableBehaviour);
					if (trackableAccessor != null)
					{
						trackableAccessor.ApplyDataSetProperties();
					}
				}
			}
		}

		private void CheckForDuplicates(TrackableBehaviour[] trackables)
		{
			HashSet<string> hashSet = new HashSet<string>();
			for (int i = 0; i < trackables.Length; i++)
			{
				string trackableName = trackables[i].TrackableName;
				if (!(trackableName == "--- EMPTY ---"))
				{
					for (int j = i + 1; j < trackables.Length; j++)
					{
						string trackableName2 = trackables[j].TrackableName;
						if (!(trackableName2 == "--- EMPTY ---") && trackables[i] is DataSetTrackableBehaviour && trackables[j] is DataSetTrackableBehaviour)
						{
							DataSetTrackableBehaviour dataSetTrackableBehaviour = trackables[i] as DataSetTrackableBehaviour;
							DataSetTrackableBehaviour arg_70_0 = trackables[j] as DataSetTrackableBehaviour;
							string dataSetName = dataSetTrackableBehaviour.DataSetName;
							string dataSetName2 = arg_70_0.DataSetName;
							if (!(dataSetName != dataSetName2) && trackableName == trackableName2)
							{
								hashSet.Add(trackableName);
							}
						}
					}
				}
			}
			if (hashSet.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = true;
				foreach (string current in hashSet)
				{
					if (flag)
					{
						stringBuilder.AppendFormat("'{0}'", current);
						flag = false;
					}
					else
					{
						stringBuilder.AppendFormat(", '{0}'", current);
					}
				}
				UnityEngine.Debug.LogWarning("Duplicate Trackables detected for: " + stringBuilder.ToString() + ".\nOnly one of the Trackables and its respective Augmentation will be selected for use at runtime - that selection is indeterminate here.");
			}
		}

		private bool CorrectTrackableScales(TrackableBehaviour[] trackables)
		{
			bool result = false;
			for (int i = 0; i < trackables.Length; i++)
			{
				if (trackables[i].CorrectScale())
				{
					result = true;
				}
			}
			return result;
		}
	}
}
