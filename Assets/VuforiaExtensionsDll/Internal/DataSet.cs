using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia
{
	public abstract class DataSet
	{
		public enum StorageType
		{
			STORAGE_APP,
			STORAGE_APPRESOURCE,
			STORAGE_ABSOLUTE
		}

		public abstract string Path
		{
			get;
		}

		public abstract VuforiaUnity.StorageType FileStorageType
		{
			get;
		}

		public static bool Exists(string name)
		{
			bool flag = DataSet.Exists("QCAR/" + name + ".xml", VuforiaUnity.StorageType.STORAGE_APPRESOURCE);
			if (!flag)
			{
				flag = DataSet.Exists("Vuforia/" + name + ".xml", VuforiaUnity.StorageType.STORAGE_APPRESOURCE);
			}
			return flag;
		}

		public static bool Exists(string path, VuforiaUnity.StorageType storageType)
		{
			return DataSetImpl.ExistsImpl(path, storageType);
		}

		public abstract bool Load(string name);

		public abstract bool Load(string path, VuforiaUnity.StorageType storageType);

		public abstract IEnumerable<Trackable> GetTrackables();

		public abstract DataSetTrackableBehaviour CreateTrackable(TrackableSource trackableSource, string gameObjectName);

		public abstract DataSetTrackableBehaviour CreateTrackable(TrackableSource trackableSource, GameObject gameObject);

		public abstract bool Destroy(Trackable trackable, bool destroyGameObject);

		public abstract bool HasReachedTrackableLimit();

		public abstract bool Contains(Trackable trackable);

		public abstract void DestroyAllTrackables(bool destroyGameObject);
	}
}
