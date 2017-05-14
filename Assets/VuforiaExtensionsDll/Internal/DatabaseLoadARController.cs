using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Vuforia
{
	public class DatabaseLoadARController : ARController
	{
		private bool mDatasetsLoaded;

		private List<string> mExternalDatasetRoots = new List<string>();

		private string[] mDataSetsToLoad;

		private string[] mDataSetsToActivate;

		private static DatabaseLoadARController mInstance;

		private static object mPadlock = new object();

		public static DatabaseLoadARController Instance
		{
			get
			{
				if (DatabaseLoadARController.mInstance == null)
				{
					object obj = DatabaseLoadARController.mPadlock;
					lock (obj)
					{
						if (DatabaseLoadARController.mInstance == null)
						{
							DatabaseLoadARController.mInstance = new DatabaseLoadARController();
						}
					}
				}
				return DatabaseLoadARController.mInstance;
			}
		}

		private DatabaseLoadARController()
		{
		}

		[RuntimeInitializeOnLoadMethod]
		public static void RegisterARController()
		{
			ARController.Register(DatabaseLoadARController.Instance);
		}

		public void LoadDatasets()
		{
			if (this.mDatasetsLoaded)
			{
				return;
			}
			if (!VuforiaRuntimeUtilities.IsVuforiaEnabled())
			{
				return;
			}
			ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
			string[] array = this.mDataSetsToLoad;
			int i = 0;
			while (i < array.Length)
			{
				string text = array[i];
				DataSet dataSet = null;
				if (DataSet.Exists(text))
				{
					dataSet = tracker.CreateDataSet();
					if (dataSet.Load(text))
					{
						goto IL_12A;
					}
					Debug.LogError("Failed to load data set " + text + ".");
				}
				else
				{
					if (this.mExternalDatasetRoots.Count <= 0)
					{
						goto IL_12A;
					}
					Debug.Log("Data set " + text + " not present in application package, checking extended root locations");
					using (List<string>.Enumerator enumerator = this.mExternalDatasetRoots.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string text2 = enumerator.Current + text + ".xml";
							if (DataSet.Exists(text2, VuforiaUnity.StorageType.STORAGE_ABSOLUTE))
							{
								dataSet = tracker.CreateDataSet();
								if (!dataSet.Load(text2, VuforiaUnity.StorageType.STORAGE_ABSOLUTE))
								{
									Debug.LogError("Failed to load data set " + text2 + ".");
								}
								else
								{
									Debug.Log("Loaded dataset at " + text2);
								}
							}
						}
					}
					if (dataSet == null)
					{
						Debug.LogError("Unable to find " + text + " in extended root locations");
						goto IL_12A;
					}
					goto IL_12A;
				}
				IL_15C:
				i++;
				continue;
				IL_12A:
				if (!this.mDataSetsToActivate.Contains(text))
				{
					goto IL_15C;
				}
				if (dataSet != null)
				{
					tracker.ActivateDataSet(dataSet);
					goto IL_15C;
				}
				Debug.LogError("Dataset " + text + " could not be loaded and cannot be activated.");
				goto IL_15C;
			}
			this.mDatasetsLoaded = true;
		}

		public void AddExternalDatasetSearchDir(string searchDir)
		{
			if (searchDir == null || searchDir.Equals(""))
			{
				return;
			}
			if (!searchDir.EndsWith("/"))
			{
				searchDir += "/";
			}
			this.mExternalDatasetRoots.Add(searchDir);
		}

		protected override void Awake()
		{
			VuforiaAbstractConfiguration.DatabaseLoadConfiguration databaseLoad = VuforiaAbstractConfiguration.Instance.DatabaseLoad;
			this.mDataSetsToLoad = databaseLoad.DataSetsToLoad;
			this.mDataSetsToActivate = databaseLoad.DataSetsToActivate;
		}

		protected override void Start()
		{
			if (!this.mDatasetsLoaded && VuforiaARController.Instance.HasStarted)
			{
				this.LoadDatasets();
			}
		}

		protected override void OnDestroy()
		{
			this.mDatasetsLoaded = false;
		}
	}
}
