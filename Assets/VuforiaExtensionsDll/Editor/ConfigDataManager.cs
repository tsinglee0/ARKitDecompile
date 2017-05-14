using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	internal class ConfigDataManager
	{
		private Dictionary<string, ConfigData> mConfigData;

		private TextConfigData mTextConfigData;

		private static ConfigDataManager mInstance;

		public static ConfigDataManager Instance
		{
			get
			{
				if (ConfigDataManager.mInstance == null)
				{
					Type typeFromHandle = typeof(ConfigDataManager);
					lock (typeFromHandle)
					{
						if (ConfigDataManager.mInstance == null)
						{
							ConfigDataManager.mInstance = new ConfigDataManager();
						}
					}
				}
				return ConfigDataManager.mInstance;
			}
		}

		public int NumConfigDataObjects
		{
			get
			{
				return this.mConfigData.Count;
			}
		}

		private ConfigDataManager()
		{
			this.mConfigData = new Dictionary<string, ConfigData>();
			this.mTextConfigData = new TextConfigData();
		}

		public void DoRead()
		{
			List<string> filePaths = this.GetFilePaths("Assets/StreamingAssets/Vuforia/", "xml");
			filePaths.AddRange(this.GetFilePaths("Assets/StreamingAssets/QCAR/", "xml"));
			List<string> arg_4F_0 = this.CorrectXMLFileList(filePaths);
			this.mConfigData.Clear();
			this.mConfigData.Add("--- EMPTY ---", this.CreateDefaultDataSet());
			foreach (string current in arg_4F_0)
			{
				this.ReadConfigData(current);
			}
			this.ReadTextConfigData();
		}

		public ConfigData GetConfigData(string dataSetName)
		{
			return this.mConfigData[dataSetName];
		}

		public void GetConfigDataNames(string[] configDataNames)
		{
			try
			{
				this.GetConfigDataNames(configDataNames, true);
			}
			catch
			{
				throw;
			}
		}

		public void GetConfigDataNames(string[] configDataNames, bool includeDefault)
		{
			try
			{
				if (includeDefault)
				{
					this.mConfigData.Keys.CopyTo(configDataNames, 0);
				}
				else
				{
					Dictionary<string, ConfigData>.KeyCollection.Enumerator enumerator = this.mConfigData.Keys.GetEnumerator();
					int num = 0;
					enumerator.MoveNext();
					while (enumerator.MoveNext())
					{
						configDataNames[num] = enumerator.Current;
						num++;
					}
				}
			}
			catch
			{
				throw;
			}
		}

		public bool ConfigDataExists(string configDataName)
		{
			return this.mConfigData.ContainsKey(configDataName);
		}

		public TextConfigData GetTextConfigData()
		{
			return this.mTextConfigData;
		}

		private List<string> CorrectXMLFileList(List<string> xmlFileList)
		{
			List<string> list = new List<string>(xmlFileList.Count);
			List<string> filePaths = this.GetFilePaths("Assets/StreamingAssets/Vuforia/", "dat");
			filePaths.AddRange(this.GetFilePaths("Assets/StreamingAssets/QCAR/", "dat"));
			foreach (string current in xmlFileList)
			{
				bool flag = false;
				string value = current.Remove(current.Length - 4);
				using (List<string>.Enumerator enumerator2 = filePaths.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.IndexOf(value) == 0)
						{
							list.Add(current);
							flag = true;
						}
					}
				}
				if (!flag)
				{
					Debug.LogWarning(current + " ignored. No corresponding DAT file found.");
				}
			}
			return list;
		}

		private List<string> GetFilePaths(string directoryPath, string fileType)
		{
			List<string> list = new List<string>();
			if (Directory.Exists(directoryPath))
			{
				string[] files = Directory.GetFiles(directoryPath);
				for (int i = 0; i < files.Length; i++)
				{
					string text = files[i];
					if (VuforiaRuntimeUtilities.StripExtensionFromPath(text).IndexOf(fileType, StringComparison.OrdinalIgnoreCase) == 0)
					{
						list.Add(text);
					}
				}
			}
			return list;
		}

		private void ReadConfigData(string dataSetFilePath)
		{
			ConfigData configData = new ConfigData(dataSetFilePath);
			string expr_0D = VuforiaRuntimeUtilities.StripFileNameFromPath(dataSetFilePath);
			string text = expr_0D.Remove(expr_0D.Length - 4);
			string authoringInfoXMLPath = "Assets/Editor/Vuforia/" + text + "/authoringinfo.xml";
			ConfigParser.Instance.fileToStruct(dataSetFilePath, authoringInfoXMLPath, configData);
			this.mConfigData[text] = configData;
		}

		private ConfigData CreateDefaultDataSet()
		{
			ConfigData expr_0A = new ConfigData("Assets/StreamingAssets/QCAR/--- EMPTY ---.xml");
			expr_0A.SetImageTarget(VuforiaUtilities.CreateDefaultImageTarget(), "--- EMPTY ---");
			expr_0A.SetMultiTarget(this.CreateDefaultMultiTarget(), "--- EMPTY ---");
			expr_0A.SetCylinderTarget(this.CreateDefaultCylinderTarget(), "--- EMPTY ---");
			expr_0A.SetObjectTarget(this.CreateDefaultObjectTarget(), "--- EMPTY ---");
			expr_0A.SetVuMarkTarget(this.CreateDefaultVuMarkTarget(), "--- EMPTY ---");
			return expr_0A;
		}

		private ConfigData.MultiTargetData CreateDefaultMultiTarget()
		{
			return new ConfigData.MultiTargetData
			{
				parts = this.CreateDefaultParts()
			};
		}

		private ConfigData.CylinderTargetData CreateDefaultCylinderTarget()
		{
			return new ConfigData.CylinderTargetData
			{
				sideLength = 1f,
				topDiameter = 0.5f,
				bottomDiameter = 0.5f,
				hasTopGeometry = false,
				hasBottomGeometry = false
			};
		}

		private ConfigData.ObjectTargetData CreateDefaultObjectTarget()
		{
			ConfigData.ObjectTargetData result = default(ConfigData.ObjectTargetData);
			result.name = "--- EMPTY ---";
			result.bboxMin = new Vector3(0f, 0f, 0f);
			result.bboxMax = new Vector3(140f, 90f, 90f);
			result.size.x = 140f;
			result.size.y = 90f;
			result.size.z = 90f;
			return result;
		}

		private ConfigData.VuMarkData CreateDefaultVuMarkTarget()
		{
			ConfigData.VuMarkData result = default(ConfigData.VuMarkData);
			result.name = "--- EMPTY ---";
			result.size.x = 1f;
			result.size.y = 1f;
			result.boundingBox2D = new Vector4(0f, 0f, 1f, 1f);
			result.origin = new Vector2(0.5f, 0.5f);
			result.idType = InstanceIdType.BYTES;
			result.idLength = 0;
			return result;
		}

		private List<ConfigData.MultiTargetPartData> CreateDefaultParts()
		{
			List<ConfigData.MultiTargetPartData> arg_6C_0 = new List<ConfigData.MultiTargetPartData>(6);
			float num = VuforiaUtilities.CreateDefaultImageTarget().size.x * 0.5f;
			arg_6C_0.Add(new ConfigData.MultiTargetPartData
			{
				translation = new Vector3(0f, num, 0f),
				rotation = Quaternion.AngleAxis(0f, new Vector3(1f, 0f, 0f)),
				name = "--- EMPTY ---"
			});
			arg_6C_0.Add(new ConfigData.MultiTargetPartData
			{
				translation = new Vector3(0f, -num, 0f),
				rotation = Quaternion.AngleAxis(180f, new Vector3(1f, 0f, 0f)),
				name = "--- EMPTY ---"
			});
			arg_6C_0.Add(new ConfigData.MultiTargetPartData
			{
				translation = new Vector3(-num, 0f, 0f),
				rotation = Quaternion.AngleAxis(90f, new Vector3(0f, 0f, 1f)),
				name = "--- EMPTY ---"
			});
			arg_6C_0.Add(new ConfigData.MultiTargetPartData
			{
				translation = new Vector3(num, 0f, 0f),
				rotation = Quaternion.AngleAxis(-90f, new Vector3(0f, 0f, 1f)),
				name = "--- EMPTY ---"
			});
			arg_6C_0.Add(new ConfigData.MultiTargetPartData
			{
				translation = new Vector3(0f, 0f, num),
				rotation = Quaternion.AngleAxis(90f, new Vector3(1f, 0f, 0f)),
				name = "--- EMPTY ---"
			});
			arg_6C_0.Add(new ConfigData.MultiTargetPartData
			{
				translation = new Vector3(0f, 0f, -num),
				rotation = Quaternion.AngleAxis(-90f, new Vector3(1f, 0f, 0f)),
				name = "--- EMPTY ---"
			});
			return arg_6C_0;
		}

		private void ReadTextConfigData()
		{
			TextConfigData textConfigData = new TextConfigData();
			textConfigData.SetDictionaryData(default(TextConfigData.DictionaryData), "--- EMPTY ---");
			using (List<string>.Enumerator enumerator = this.GetFilePaths("Assets/StreamingAssets/Vuforia/WordLists/", "vwl").GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(enumerator.Current);
					string binaryFile = "Vuforia/WordLists/" + fileNameWithoutExtension + ".vwl";
					textConfigData.SetDictionaryData(new TextConfigData.DictionaryData
					{
						BinaryFile = binaryFile
					}, fileNameWithoutExtension);
				}
			}
			using (List<string>.Enumerator enumerator = this.GetFilePaths("Assets/StreamingAssets/QCAR/", "vwl").GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string fileNameWithoutExtension2 = Path.GetFileNameWithoutExtension(enumerator.Current);
					string binaryFile2 = "QCAR/" + fileNameWithoutExtension2 + ".vwl";
					textConfigData.SetDictionaryData(new TextConfigData.DictionaryData
					{
						BinaryFile = binaryFile2
					}, fileNameWithoutExtension2);
				}
			}
			textConfigData.SetWordListData(default(TextConfigData.WordListData), "--- EMPTY ---");
			using (List<string>.Enumerator enumerator = this.GetFilePaths("Assets/StreamingAssets/Vuforia/WordLists/", "lst").GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string fileNameWithoutExtension3 = Path.GetFileNameWithoutExtension(enumerator.Current);
					string textFile = "Vuforia/WordLists/" + fileNameWithoutExtension3 + ".lst";
					textConfigData.SetWordListData(new TextConfigData.WordListData
					{
						TextFile = textFile
					}, fileNameWithoutExtension3);
				}
			}
			using (List<string>.Enumerator enumerator = this.GetFilePaths("Assets/StreamingAssets/QCAR/", "lst").GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string fileNameWithoutExtension4 = Path.GetFileNameWithoutExtension(enumerator.Current);
					string textFile2 = "QCAR/" + fileNameWithoutExtension4 + ".lst";
					textConfigData.SetWordListData(new TextConfigData.WordListData
					{
						TextFile = textFile2
					}, fileNameWithoutExtension4);
				}
			}
			this.mTextConfigData = textConfigData;
		}
	}
}
