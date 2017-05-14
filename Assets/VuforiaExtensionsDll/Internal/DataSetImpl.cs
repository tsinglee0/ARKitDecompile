using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Vuforia
{
	internal class DataSetImpl : DataSet
	{
		private IntPtr mDataSetPtr = IntPtr.Zero;

		private string mPath = "";

		private VuforiaUnity.StorageType mStorageType = VuforiaUnity.StorageType.STORAGE_APPRESOURCE;

		private readonly Dictionary<int, Trackable> mTrackablesDict = new Dictionary<int, Trackable>();

		private const int mMaxTrackableNameLength = 128;

		public IntPtr DataSetPtr
		{
			get
			{
				return this.mDataSetPtr;
			}
		}

		public override string Path
		{
			get
			{
				return this.mPath;
			}
		}

		public override VuforiaUnity.StorageType FileStorageType
		{
			get
			{
				return this.mStorageType;
			}
		}

		public DataSetImpl(IntPtr dataSetPtr)
		{
			this.mDataSetPtr = dataSetPtr;
		}

		public override bool Load(string name)
		{
			string path = "QCAR/" + name + ".xml";
			bool flag = this.Load(path, VuforiaUnity.StorageType.STORAGE_APPRESOURCE);
			if (!flag)
			{
				path = "Vuforia/" + name + ".xml";
				flag = this.Load(path, VuforiaUnity.StorageType.STORAGE_APPRESOURCE);
			}
			if (!flag)
			{
				Debug.LogError("Did not load: " + name);
			}
			return flag;
		}

		public override bool Load(string path, VuforiaUnity.StorageType storageType)
		{
			if (this.mDataSetPtr == IntPtr.Zero)
			{
				Debug.LogError("Called Load without a data set object");
				return false;
			}
			string storagePath = VuforiaRuntimeUtilities.GetStoragePath(path, storageType);
			if (VuforiaWrapper.Instance.DataSetExists(storagePath, (int)storageType) == 0)
			{
				return false;
			}
			if (VuforiaWrapper.Instance.DataSetLoad(storagePath, (int)storageType, this.mDataSetPtr) == 0)
			{
				Debug.LogError("Did not load: " + storagePath);
				return false;
			}
			this.mPath = path;
			this.mStorageType = storageType;
			bool arg_85_0 = this.CreateImageTargets();
			bool flag = this.CreateMultiTargets();
			bool flag2 = this.CreateCylinderTargets();
			bool flag3 = this.CreateVuMarkTemplates();
			if (!arg_85_0 && !flag && !flag2 && !flag3)
			{
				this.CreateObjectTargets();
			}
			((StateManagerImpl)TrackerManager.Instance.GetStateManager()).AssociateTrackableBehavioursForDataSet(this);
			return true;
		}

		public override IEnumerable<Trackable> GetTrackables()
		{
			return this.mTrackablesDict.Values;
		}

		public override DataSetTrackableBehaviour CreateTrackable(TrackableSource trackableSource, string gameObjectName)
		{
			GameObject gameObject = new GameObject(gameObjectName);
			return this.CreateTrackable(trackableSource, gameObject);
		}

		public override DataSetTrackableBehaviour CreateTrackable(TrackableSource trackableSource, GameObject gameObject)
		{
			TrackableSourceImpl trackableSourceImpl = (TrackableSourceImpl)trackableSource;
			StringBuilder stringBuilder = new StringBuilder(128);
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SimpleTargetData)));
			int num = VuforiaWrapper.Instance.DataSetCreateTrackable(this.mDataSetPtr, trackableSourceImpl.TrackableSourcePtr, stringBuilder, 128, intPtr);
			SimpleTargetData simpleTargetData = (SimpleTargetData)Marshal.PtrToStructure(intPtr, typeof(SimpleTargetData));
			Marshal.FreeHGlobal(intPtr);
			if (num == (int)TypeMapping.GetTypeID(typeof(ImageTarget)))
			{
				ImageTarget imageTarget = new ImageTargetImpl(stringBuilder.ToString(), simpleTargetData.id, ImageTargetType.USER_DEFINED, this);
				this.mTrackablesDict[simpleTargetData.id] = imageTarget;
				Debug.Log(string.Format("Trackable created: {0}, {1}", num, stringBuilder));
				return ((StateManagerImpl)TrackerManager.Instance.GetStateManager()).FindOrCreateImageTargetBehaviourForTrackable(imageTarget, gameObject, this);
			}
			Debug.LogError("DataSet.CreateTrackable returned unknown or incompatible trackable type!");
			return null;
		}

		public override bool Destroy(Trackable trackable, bool destroyGameObject)
		{
			if (VuforiaWrapper.Instance.DataSetDestroyTrackable(this.mDataSetPtr, trackable.ID) == 0)
			{
				Debug.LogError("Could not destroy trackable with id " + trackable.ID + ".");
				return false;
			}
			this.mTrackablesDict.Remove(trackable.ID);
			if (destroyGameObject)
			{
				TrackerManager.Instance.GetStateManager().DestroyTrackableBehavioursForTrackable(trackable, true);
			}
			return true;
		}

		public override bool HasReachedTrackableLimit()
		{
			return VuforiaWrapper.Instance.DataSetHasReachedTrackableLimit(this.mDataSetPtr) == 1;
		}

		public override bool Contains(Trackable trackable)
		{
			return this.mTrackablesDict.ContainsValue(trackable);
		}

		public override void DestroyAllTrackables(bool destroyGameObject)
		{
			foreach (Trackable current in new List<Trackable>(this.mTrackablesDict.Values))
			{
				this.Destroy(current, destroyGameObject);
			}
		}

		internal static bool ExistsImpl(string path, VuforiaUnity.StorageType storageType)
		{
			path = VuforiaRuntimeUtilities.GetStoragePath(path, storageType);
			return VuforiaWrapper.Instance.DataSetExists(path, (int)storageType) == 1;
		}

		private bool CreateImageTargets()
		{
			int num = VuforiaWrapper.Instance.DataSetGetNumTrackableType((int)TypeMapping.GetTypeID(typeof(ImageTarget)), this.mDataSetPtr);
			if (num <= 0)
			{
				return false;
			}
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ImageTargetData)) * num);
			if (VuforiaWrapper.Instance.DataSetGetTrackablesOfType((int)TypeMapping.GetTypeID(typeof(ImageTarget)), intPtr, num, this.mDataSetPtr) == 0)
			{
				Debug.LogError("Could not create Image Targets");
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				ImageTargetData imageTargetData = (ImageTargetData)Marshal.PtrToStructure(new IntPtr(intPtr.ToInt64() + (long)(i * Marshal.SizeOf(typeof(ImageTargetData)))), typeof(ImageTargetData));
				if (!this.mTrackablesDict.ContainsKey(imageTargetData.id))
				{
					StringBuilder stringBuilder = new StringBuilder(128);
					VuforiaWrapper.Instance.DataSetGetTrackableName(this.mDataSetPtr, imageTargetData.id, stringBuilder, 128);
					ImageTarget value = new ImageTargetImpl(stringBuilder.ToString(), imageTargetData.id, ImageTargetType.PREDEFINED, this);
					this.mTrackablesDict[imageTargetData.id] = value;
				}
			}
			Marshal.FreeHGlobal(intPtr);
			return true;
		}

		private bool CreateMultiTargets()
		{
			int num = VuforiaWrapper.Instance.DataSetGetNumTrackableType((int)TypeMapping.GetTypeID(typeof(MultiTarget)), this.mDataSetPtr);
			if (num <= 0)
			{
				return false;
			}
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SimpleTargetData)) * num);
			if (VuforiaWrapper.Instance.DataSetGetTrackablesOfType((int)TypeMapping.GetTypeID(typeof(MultiTarget)), intPtr, num, this.mDataSetPtr) == 0)
			{
				Debug.LogError("Could not create Multi Targets");
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				SimpleTargetData simpleTargetData = (SimpleTargetData)Marshal.PtrToStructure(new IntPtr(intPtr.ToInt64() + (long)(i * Marshal.SizeOf(typeof(SimpleTargetData)))), typeof(SimpleTargetData));
				if (!this.mTrackablesDict.ContainsKey(simpleTargetData.id))
				{
					StringBuilder stringBuilder = new StringBuilder(128);
					VuforiaWrapper.Instance.DataSetGetTrackableName(this.mDataSetPtr, simpleTargetData.id, stringBuilder, 128);
					MultiTarget value = new MultiTargetImpl(stringBuilder.ToString(), simpleTargetData.id, this);
					this.mTrackablesDict[simpleTargetData.id] = value;
				}
			}
			Marshal.FreeHGlobal(intPtr);
			return true;
		}

		private bool CreateCylinderTargets()
		{
			int num = VuforiaWrapper.Instance.DataSetGetNumTrackableType((int)TypeMapping.GetTypeID(typeof(CylinderTarget)), this.mDataSetPtr);
			if (num <= 0)
			{
				return false;
			}
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SimpleTargetData)) * num);
			if (VuforiaWrapper.Instance.DataSetGetTrackablesOfType((int)TypeMapping.GetTypeID(typeof(CylinderTarget)), intPtr, num, this.mDataSetPtr) == 0)
			{
				Debug.LogError("Could not create Cylinder Targets");
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				SimpleTargetData simpleTargetData = (SimpleTargetData)Marshal.PtrToStructure(new IntPtr(intPtr.ToInt64() + (long)(i * Marshal.SizeOf(typeof(SimpleTargetData)))), typeof(SimpleTargetData));
				if (!this.mTrackablesDict.ContainsKey(simpleTargetData.id))
				{
					StringBuilder stringBuilder = new StringBuilder(128);
					VuforiaWrapper.Instance.DataSetGetTrackableName(this.mDataSetPtr, simpleTargetData.id, stringBuilder, 128);
					CylinderTarget value = new CylinderTargetImpl(stringBuilder.ToString(), simpleTargetData.id, this);
					this.mTrackablesDict[simpleTargetData.id] = value;
				}
			}
			Marshal.FreeHGlobal(intPtr);
			return true;
		}

		private bool CreateVuMarkTemplates()
		{
			int num = VuforiaWrapper.Instance.DataSetGetNumTrackableType((int)TypeMapping.GetTypeID(typeof(VuMarkTemplate)), this.mDataSetPtr);
			if (num <= 0)
			{
				return false;
			}
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SimpleTargetData)) * num);
			if (VuforiaWrapper.Instance.DataSetGetTrackablesOfType((int)TypeMapping.GetTypeID(typeof(VuMarkTemplate)), intPtr, num, this.mDataSetPtr) == 0)
			{
				Debug.LogError("Could not create VuMark Template");
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				SimpleTargetData simpleTargetData = (SimpleTargetData)Marshal.PtrToStructure(new IntPtr(intPtr.ToInt64() + (long)(i * Marshal.SizeOf(typeof(SimpleTargetData)))), typeof(SimpleTargetData));
				if (!this.mTrackablesDict.ContainsKey(simpleTargetData.id))
				{
					StringBuilder stringBuilder = new StringBuilder(128);
					VuforiaWrapper.Instance.DataSetGetTrackableName(this.mDataSetPtr, simpleTargetData.id, stringBuilder, 128);
					VuMarkTemplateImpl value = new VuMarkTemplateImpl(stringBuilder.ToString(), simpleTargetData.id, this);
					this.mTrackablesDict[simpleTargetData.id] = value;
				}
			}
			Marshal.FreeHGlobal(intPtr);
			return true;
		}

		private bool CreateObjectTargets()
		{
			int num = VuforiaWrapper.Instance.DataSetGetNumTrackableType((int)TypeMapping.GetTypeID(typeof(ObjectTarget)), this.mDataSetPtr);
			if (num <= 0)
			{
				return false;
			}
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SimpleTargetData)) * num);
			if (VuforiaWrapper.Instance.DataSetGetTrackablesOfType((int)TypeMapping.GetTypeID(typeof(ObjectTarget)), intPtr, num, this.mDataSetPtr) == 0)
			{
				Debug.LogError("Could not create Object Targets");
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				SimpleTargetData simpleTargetData = (SimpleTargetData)Marshal.PtrToStructure(new IntPtr(intPtr.ToInt64() + (long)(i * Marshal.SizeOf(typeof(SimpleTargetData)))), typeof(SimpleTargetData));
				if (!this.mTrackablesDict.ContainsKey(simpleTargetData.id))
				{
					StringBuilder stringBuilder = new StringBuilder(128);
					VuforiaWrapper.Instance.DataSetGetTrackableName(this.mDataSetPtr, simpleTargetData.id, stringBuilder, 128);
					ObjectTarget value = new ObjectTargetImpl(stringBuilder.ToString(), simpleTargetData.id, this);
					this.mTrackablesDict[simpleTargetData.id] = value;
				}
			}
			Marshal.FreeHGlobal(intPtr);
			return true;
		}
	}
}
