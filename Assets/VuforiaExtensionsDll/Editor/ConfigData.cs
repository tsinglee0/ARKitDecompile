using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	internal class ConfigData
	{
		public struct VirtualButtonData
		{
			public string name;

			public bool enabled;

			public Vector4 rectangle;

			public VirtualButton.Sensitivity sensitivity;
		}

		public struct ImageTargetData
		{
			public Vector2 size;

			public List<ConfigData.VirtualButtonData> virtualButtons;

			public string previewImage;
		}

		public struct MultiTargetPartData
		{
			public string name;

			public Vector3 translation;

			public Quaternion rotation;
		}

		public struct MultiTargetData
		{
			public List<ConfigData.MultiTargetPartData> parts;
		}

		public struct CylinderTargetData
		{
			public string name;

			public float sideLength;

			public float topDiameter;

			public float bottomDiameter;

			public bool hasTopGeometry;

			public bool hasBottomGeometry;

			public string previewSide;

			public string previewTop;

			public string previewBottom;
		}

		public struct ObjectTargetData
		{
			public string name;

			public string targetID;

			public Vector3 size;

			public Vector3 bboxMin;

			public Vector3 bboxMax;

			public string previewImage;
		}

		public struct VuMarkData
		{
			public string name;

			public Vector2 size;

			public string previewImage;

			public Vector4 boundingBox2D;

			public Vector2 origin;

			public InstanceIdType idType;

			public int idLength;

			public int maxId;
		}

		private Dictionary<string, ConfigData.ImageTargetData> imageTargets;

		private Dictionary<string, ConfigData.MultiTargetData> multiTargets;

		private Dictionary<string, ConfigData.CylinderTargetData> cylinderTargets;

		private Dictionary<string, ConfigData.ObjectTargetData> objectTargets;

		private Dictionary<string, ConfigData.VuMarkData> vuMarkTargets;

		private string fullPath;

		public string FullPath
		{
			get
			{
				return this.fullPath;
			}
		}

		public int NumImageTargets
		{
			get
			{
				return this.imageTargets.Count;
			}
		}

		public int NumMultiTargets
		{
			get
			{
				return this.multiTargets.Count;
			}
		}

		public int NumCylinderTargets
		{
			get
			{
				return this.cylinderTargets.Count;
			}
		}

		public int NumObjectTargets
		{
			get
			{
				return this.objectTargets.Count;
			}
		}

		public int NumVuMarkTargets
		{
			get
			{
				return this.vuMarkTargets.Count;
			}
		}

		public int NumTrackables
		{
			get
			{
				return this.NumImageTargets + this.NumMultiTargets + this.NumCylinderTargets + this.NumObjectTargets + this.NumVuMarkTargets;
			}
		}

		public ConfigData(string path)
		{
			this.fullPath = path;
			this.imageTargets = new Dictionary<string, ConfigData.ImageTargetData>();
			this.multiTargets = new Dictionary<string, ConfigData.MultiTargetData>();
			this.cylinderTargets = new Dictionary<string, ConfigData.CylinderTargetData>();
			this.objectTargets = new Dictionary<string, ConfigData.ObjectTargetData>();
			this.vuMarkTargets = new Dictionary<string, ConfigData.VuMarkData>();
		}

		public ConfigData(ConfigData original)
		{
			this.fullPath = original.FullPath;
			this.imageTargets = new Dictionary<string, ConfigData.ImageTargetData>(original.imageTargets);
			this.multiTargets = new Dictionary<string, ConfigData.MultiTargetData>(original.multiTargets);
			this.cylinderTargets = new Dictionary<string, ConfigData.CylinderTargetData>(original.cylinderTargets);
			this.objectTargets = new Dictionary<string, ConfigData.ObjectTargetData>(original.objectTargets);
			this.vuMarkTargets = new Dictionary<string, ConfigData.VuMarkData>(original.vuMarkTargets);
		}

		public void SetImageTarget(ConfigData.ImageTargetData item, string name)
		{
			this.imageTargets[name] = item;
		}

		public void SetMultiTarget(ConfigData.MultiTargetData item, string name)
		{
			this.multiTargets[name] = item;
		}

		public void SetCylinderTarget(ConfigData.CylinderTargetData item, string name)
		{
			this.cylinderTargets[name] = item;
		}

		public void SetObjectTarget(ConfigData.ObjectTargetData item, string name)
		{
			this.objectTargets[name] = item;
		}

		public void SetVuMarkTarget(ConfigData.VuMarkData item, string name)
		{
			this.vuMarkTargets[name] = item;
		}

		public void AddVirtualButton(ConfigData.VirtualButtonData item, string imageTargetName)
		{
			try
			{
				this.imageTargets[imageTargetName].virtualButtons.Add(item);
			}
			catch
			{
				throw;
			}
		}

		public void AddMultiTargetPart(ConfigData.MultiTargetPartData item, string multiTargetName)
		{
			try
			{
				this.multiTargets[multiTargetName].parts.Add(item);
			}
			catch
			{
				throw;
			}
		}

		public void ClearAll()
		{
			this.ClearImageTargets();
			this.ClearMultiTargets();
			this.ClearCylinderTargets();
			this.ClearObjectTargets();
			this.ClearVuMarkTargets();
		}

		public void ClearImageTargets()
		{
			this.imageTargets.Clear();
		}

		public void ClearMultiTargets()
		{
			this.multiTargets.Clear();
		}

		public void ClearCylinderTargets()
		{
			this.cylinderTargets.Clear();
		}

		public void ClearObjectTargets()
		{
			this.objectTargets.Clear();
		}

		public void ClearVuMarkTargets()
		{
			this.vuMarkTargets.Clear();
		}

		public void ClearVirtualButtons()
		{
			using (Dictionary<string, ConfigData.ImageTargetData>.ValueCollection.Enumerator enumerator = this.imageTargets.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.virtualButtons.Clear();
				}
			}
		}

		public bool RemoveImageTarget(string name)
		{
			return this.imageTargets.Remove(name);
		}

		public bool RemoveMultiTarget(string name)
		{
			return this.multiTargets.Remove(name);
		}

		public bool RemoveCylinderTarget(string name)
		{
			return this.cylinderTargets.Remove(name);
		}

		public bool RemoveObjectTarget(string name)
		{
			return this.objectTargets.Remove(name);
		}

		public bool RemoveVuMarkTarget(string name)
		{
			return this.vuMarkTargets.Remove(name);
		}

		public void GetImageTarget(string name, out ConfigData.ImageTargetData it)
		{
			try
			{
				it = this.imageTargets[name];
			}
			catch
			{
				throw;
			}
		}

		public void GetMultiTarget(string name, out ConfigData.MultiTargetData mt)
		{
			try
			{
				mt = this.multiTargets[name];
			}
			catch
			{
				throw;
			}
		}

		public void GetCylinderTarget(string name, out ConfigData.CylinderTargetData ct)
		{
			try
			{
				ct = this.cylinderTargets[name];
			}
			catch
			{
				throw;
			}
		}

		public void GetObjectTarget(string name, out ConfigData.ObjectTargetData ot)
		{
			try
			{
				ot = this.objectTargets[name];
			}
			catch
			{
				throw;
			}
		}

		public void GetVuMarkTarget(string name, out ConfigData.VuMarkData vm)
		{
			try
			{
				vm = this.vuMarkTargets[name];
			}
			catch
			{
				throw;
			}
		}

		public void GetVirtualButton(string name, string imageTargetName, out ConfigData.VirtualButtonData vb)
		{
			vb = default(ConfigData.VirtualButtonData);
			try
			{
				ConfigData.ImageTargetData imageTargetData;
				this.GetImageTarget(imageTargetName, out imageTargetData);
				List<ConfigData.VirtualButtonData> virtualButtons = imageTargetData.virtualButtons;
				for (int i = 0; i < virtualButtons.Count; i++)
				{
					if (virtualButtons[i].name == name)
					{
						vb = virtualButtons[i];
					}
				}
			}
			catch
			{
				throw;
			}
		}

		public bool ImageTargetExists(string name)
		{
			return this.imageTargets.ContainsKey(name);
		}

		public bool MultiTargetExists(string name)
		{
			return this.multiTargets.ContainsKey(name);
		}

		public bool CylinderTargetExists(string name)
		{
			return this.cylinderTargets.ContainsKey(name);
		}

		public bool ObjectTargetExists(string name)
		{
			return this.objectTargets.ContainsKey(name);
		}

		public bool VuMarkTargetExists(string name)
		{
			return this.vuMarkTargets.ContainsKey(name);
		}

		public void CopyImageTargetNames(string[] arrayToFill, int index)
		{
			try
			{
				this.imageTargets.Keys.CopyTo(arrayToFill, index);
			}
			catch
			{
				throw;
			}
		}

		public void CopyMultiTargetNames(string[] arrayToFill, int index)
		{
			try
			{
				this.multiTargets.Keys.CopyTo(arrayToFill, index);
			}
			catch
			{
				throw;
			}
		}

		public void CopyCylinderTargetNames(string[] arrayToFill, int index)
		{
			try
			{
				this.cylinderTargets.Keys.CopyTo(arrayToFill, index);
			}
			catch
			{
				throw;
			}
		}

		public void CopyObjectTargetNames(string[] arrayToFill, int index)
		{
			try
			{
				this.objectTargets.Keys.CopyTo(arrayToFill, index);
			}
			catch
			{
				throw;
			}
		}

		public void CopyVuMarkTargetNames(string[] arrayToFill, int index)
		{
			try
			{
				this.vuMarkTargets.Keys.CopyTo(arrayToFill, index);
			}
			catch
			{
				throw;
			}
		}
	}
}
