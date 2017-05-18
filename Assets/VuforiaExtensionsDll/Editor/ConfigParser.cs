using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	internal class ConfigParser
	{
		private static ConfigParser mInstance;

		public static ConfigParser Instance
		{
			get
			{
				if (ConfigParser.mInstance == null)
				{
					ConfigParser.mInstance = new ConfigParser();
				}
				return ConfigParser.mInstance;
			}
		}
		
		internal static uint ComputeStringHash(string s)
        	{
			uint num = 0;
            		if (s != null)
            		{
                		num = 0x811c9dc5;
                		for (int i = 0; i < s.Length; i++)
                		{
                    			num = (s[i] ^ num) * 0x1000193;
                		}
            		}
            		return num;
        	}
		
		public bool fileToStruct(string configXMLPath, string authoringInfoXMLPath, ConfigData configData)
		{
			if (!File.Exists(configXMLPath))
			{
				return false;
			}
			AuthoringInfo authoringInfo;
			if (File.Exists(authoringInfoXMLPath))
			{
				authoringInfo = new AuthoringInfo(authoringInfoXMLPath);
			}
			else
			{
				authoringInfo = new AuthoringInfo();
			}
			List<ConfigData.CylinderTargetData> list = new List<ConfigData.CylinderTargetData>();
			List<ConfigData.ObjectTargetData> list2 = new List<ConfigData.ObjectTargetData>();
			using (XmlTextReader xmlTextReader = new XmlTextReader(configXMLPath))
			{
				while (xmlTextReader.Read())
				{
					if (xmlTextReader.NodeType == XmlNodeType.Element)
					{
						string name = xmlTextReader.Name;                        
                        			uint num = ComputeStringHash(name);
						
						if (num <= 2826972859u)
						{
							if (num != 1357157938u)
							{
								if (num != 1777789069u)
								{
									if (num == 2826972859u)
									{
										if (name == "ObjectTarget")
										{
											string attribute = xmlTextReader.GetAttribute("name");
											xmlTextReader.MoveToElement();
											list2.Add(new ConfigData.ObjectTargetData
											{
												name = attribute
											});
										}
									}
								}
								else if (name == "VuMark")
								{
									string attribute2 = xmlTextReader.GetAttribute("name");
									Vector2 zero = Vector2.zero;
									string attribute3 = xmlTextReader.GetAttribute("size");
									if (attribute3 != null)
									{
										if (!VuforiaUtilities.SizeFromStringArray(out zero, attribute3.Split(new char[]
										{
											' '
										})))
										{
											Debug.LogWarning("Found illegal size attribute for VuMark Target " + attribute2 + " in config.xml. VuMark Target will be ignored.");
										}
										else
										{
											xmlTextReader.MoveToElement();
											ConfigData.VuMarkData item;
											if (!authoringInfo.TryGetInfo(attribute2, out item))
											{
												Debug.LogWarning("Couldn't find VuMark " + attribute2 + " in authoring info");
												Debug.LogWarning("Use device database unitypackage as downloaded from Target Manager!\nTarget is not rendered correctly in editor.");
											}
											item.name = attribute2;
											item.size = zero;
											configData.SetVuMarkTarget(item, attribute2);
										}
									}
									else
									{
										Debug.LogWarning("VuMark Target " + attribute2 + " is missing a size attribut in config.xml. VuMark Target will be ignored.");
									}
								}
							}
							else if (name == "CylinderTarget")
							{
								string attribute4 = xmlTextReader.GetAttribute("name");
								if (attribute4 == null)
								{
									Debug.LogWarning("Found Cylinder Target without name attribute in config.xml. Cylinder Target will be ignored.");
								}
								else
								{
									string attribute5 = xmlTextReader.GetAttribute("sideLength");
									float sideLength = -1f;
									if (attribute5 != null)
									{
										sideLength = float.Parse(attribute5, CultureInfo.InvariantCulture);
									}
									xmlTextReader.MoveToElement();
									list.Add(new ConfigData.CylinderTargetData
									{
										name = attribute4,
										sideLength = sideLength
									});
								}
							}
						}
						else if (num <= 3803554136u)
						{
							if (num != 3027079795u)
							{
								if (num == 3803554136u)
								{
									if (name == "VirtualButton")
									{
										string attribute6 = xmlTextReader.GetAttribute("name");
										if (attribute6 == null)
										{
											Debug.LogWarning("Found VirtualButton without name attribute in config.xml. Virtual Button will be ignored.");
										}
										else
										{
											Vector4 zero2 = Vector4.zero;
											string[] array = xmlTextReader.GetAttribute("rectangle").Split(new char[]
											{
												' '
											});
											if (array != null)
											{
												if (!VuforiaUtilities.RectangleFromStringArray(out zero2, array))
												{
													Debug.LogWarning("Found invalid rectangle attribute for Virtual Button " + attribute6 + " in config.xml. Virtual Button will be ignored.");
												}
												else
												{
													bool enabled = true;
													string attribute7 = xmlTextReader.GetAttribute("enabled");
													if (attribute7 != null)
													{
														if (string.Compare(attribute7, "true", true) == 0)
														{
															enabled = true;
														}
														else if (string.Compare(attribute7, "false", true) == 0)
														{
															enabled = false;
														}
														else
														{
															Debug.LogWarning("Found invalid enabled attribute for Virtual Button " + attribute6 + " in config.xml. Default setting will be used.");
														}
													}
													VirtualButton.Sensitivity sensitivity = VirtualButton.Sensitivity.LOW;
													string attribute8 = xmlTextReader.GetAttribute("sensitivity");
													if (attribute8 != null)
													{
														if (string.Compare(attribute8, "low", true) == 0)
														{
															sensitivity = VirtualButton.Sensitivity.LOW;
														}
														else if (string.Compare(attribute8, "medium", true) == 0)
														{
															sensitivity = VirtualButton.Sensitivity.MEDIUM;
														}
														else if (string.Compare(attribute8, "high", true) == 0)
														{
															sensitivity = VirtualButton.Sensitivity.HIGH;
														}
														else
														{
															Debug.LogWarning("Found illegal sensitivity attribute for Virtual Button " + attribute6 + " in config.xml. Default setting will be used.");
														}
													}
													xmlTextReader.MoveToElement();
													ConfigData.VirtualButtonData item2 = default(ConfigData.VirtualButtonData);
													string latestITName = ConfigParser.GetLatestITName(configData);
													item2.name = attribute6;
													item2.rectangle = zero2;
													item2.enabled = enabled;
													item2.sensitivity = sensitivity;
													if (configData.ImageTargetExists(latestITName))
													{
														configData.AddVirtualButton(item2, latestITName);
													}
													else
													{
														Debug.LogWarning(string.Concat(new string[]
														{
															"Image Target with name ",
															latestITName,
															" could not be found. Virtual Button ",
															attribute6,
															"will not be added."
														}));
													}
												}
											}
											else
											{
												Debug.LogWarning("Virtual Button " + attribute6 + " has no rectangle attribute in config.xml. Virtual Button will be ignored.");
											}
										}
									}
								}
							}
							else if (name == "MultiTarget")
							{
								string attribute9 = xmlTextReader.GetAttribute("name");
								if (attribute9 == null)
								{
									Debug.LogWarning("Found Multi Target without name attribute in config.xml. Multi Target will be ignored.");
								}
								else
								{
									xmlTextReader.MoveToElement();
									configData.SetMultiTarget(new ConfigData.MultiTargetData
									{
										parts = new List<ConfigData.MultiTargetPartData>()
									}, attribute9);
								}
							}
						}
						else if (num != 3814285364u)
						{
							if (num == 4124202875u)
							{
								if (name == "ImageTarget")
								{
									string attribute10 = xmlTextReader.GetAttribute("name");
									if (attribute10 == null)
									{
										Debug.LogWarning("Found ImageTarget without name attribute in config.xml. Image Target will be ignored.");
									}
									else
									{
										Vector2 zero3 = Vector2.zero;
										string[] array2 = xmlTextReader.GetAttribute("size").Split(new char[]
										{
											' '
										});
										if (array2 != null)
										{
											if (!VuforiaUtilities.SizeFromStringArray(out zero3, array2))
											{
												Debug.LogWarning("Found illegal itSize attribute for Image Target " + attribute10 + " in config.xml. Image Target will be ignored.");
											}
											else
											{
												xmlTextReader.MoveToElement();
												configData.SetImageTarget(new ConfigData.ImageTargetData
												{
													size = zero3,
													virtualButtons = new List<ConfigData.VirtualButtonData>()
												}, attribute10);
											}
										}
										else
										{
											Debug.LogWarning("Image Target " + attribute10 + " is missing a itSize attribut in config.xml. Image Target will be ignored.");
										}
									}
								}
							}
						}
						else if (name == "Part")
						{
							string attribute11 = xmlTextReader.GetAttribute("name");
							if (attribute11 == null)
							{
								Debug.LogWarning("Found Multi Target Part without name attribute in config.xml. Part will be ignored.");
							}
							else
							{
								Vector3 zero4 = Vector3.zero;
								string[] array3 = xmlTextReader.GetAttribute("translation").Split(new char[]
								{
									' '
								});
								if (array3 != null)
								{
									if (!VuforiaUtilities.TransformFromStringArray(out zero4, array3))
									{
										Debug.LogWarning("Found illegal transform attribute for Part " + attribute11 + " in config.xml. Part will be ignored.");
									}
									else
									{
										Quaternion quaternion = Quaternion.identity;
										string attribute12 = xmlTextReader.GetAttribute("rotation");
										if (attribute12 == null)
										{
											Debug.LogWarning("Multi Target Part " + attribute11 + " has no rotation attribute in config.xml. Part will be ignored.");
										}
										else
										{
											string[] array4 = attribute12.Split(new char[]
											{
												';'
											});
											string str = "";
											if (array4.Length != 0)
											{
												str = array4[0].Split(new char[]
												{
													' '
												})[0];
											}
											for (int i = 0; i < array4.Length; i++)
											{
												string text = array4[i];
												if (i != 0)
												{
													text = str + " " + text.Trim();
												}
												Quaternion quaternion2 = this.parseQuaternionFromAttrString(text);
												quaternion *= quaternion2;
											}
											xmlTextReader.MoveToElement();
											ConfigData.MultiTargetPartData item3 = default(ConfigData.MultiTargetPartData);
											string latestMTName = ConfigParser.GetLatestMTName(configData);
											item3.name = attribute11;
											item3.rotation = quaternion;
											item3.translation = zero4;
											if (configData.MultiTargetExists(latestMTName))
											{
												configData.AddMultiTargetPart(item3, latestMTName);
											}
											else
											{
												Debug.LogWarning(string.Concat(new string[]
												{
													"Multi Target with name ",
													latestMTName,
													" could not be found. Multi Target Part ",
													attribute11,
													"will not be added."
												}));
											}
										}
									}
								}
								else
								{
									Debug.LogWarning("Multi Target Part " + attribute11 + " has no translation attribute in config.xml. Part will be ignored.");
								}
							}
						}
					}
				}
			}
			if (list.Count > 0)
			{
				string arg_796_0 = configXMLPath.Substring(0, configXMLPath.Length - 3) + "dat";
				ConfigData.CylinderTargetData[] array5 = list.ToArray();
				CylinderDatasetReader.Read(arg_796_0, array5);
				ConfigData.CylinderTargetData[] array6 = array5;
				for (int j = 0; j < array6.Length; j++)
				{
					ConfigData.CylinderTargetData cylinderTargetData = array6[j];
					configData.SetCylinderTarget(cylinderTargetData, cylinderTargetData.name);
				}
			}
			if (list2.Count > 0)
			{
				ConfigData.ObjectTargetData[] array7 = list2.ToArray();
				string[] array8 = configXMLPath.Split(new char[]
				{
					'/'
				});
				string text2 = array8[(array8.Length - 1 > 0) ? (array8.Length - 1) : 0];
				int num2 = 4;
				if (text2.Contains("_OT"))
				{
					num2 += 3;
				}
				string text3 = "Assets/Editor/Vuforia/TargetsetData/" + text2.Substring(0, text2.Length - num2) + "_info.xml";
				string text4 = "Assets/Editor/QCAR/TargetsetData/" + text2.Substring(0, text2.Length - num2) + "_info.xml";
				if (File.Exists(text3))
				{
					ObjectEditorConfigurationReader.Read(text3, array7);
				}
				else if (File.Exists(text4))
				{
					ObjectEditorConfigurationReader.Read(text4, array7);
				}
				else
				{
					Debug.LogError("No editor configuration file available for " + text2 + " (Only use unity dataset package generated from the Vuforia developer portal)");
					for (int k = 0; k < array7.Length; k++)
					{
						array7[k].targetID = "";
						array7[k].bboxMin = new Vector3(0f, 0f, 0f);
						array7[k].bboxMax = new Vector3(200f, 200f, 200f);
						array7[k].size = new Vector3(200f, 200f, 200f);
					}
				}
				for (int l = 0; l < array7.Length; l++)
				{
					configData.SetObjectTarget(array7[l], array7[l].name);
				}
			}
			return true;
		}

		private Quaternion parseQuaternionFromAttrString(string rotationStr)
		{
			Quaternion identity = Quaternion.identity;
			string[] array = rotationStr.Trim().Split(new char[]
			{
				' '
			});
			if (array != null && !VuforiaUtilities.OrientationFromStringArray(out identity, array))
			{
				Debug.LogWarning("Found illegal rotation attribute for Part " + rotationStr + " in config.xml.  Part will be ignored.");
				return Quaternion.identity;
			}
			return identity;
		}

		private static string GetLatestITName(ConfigData backlog)
		{
			if (backlog == null)
			{
				return null;
			}
			string[] array = new string[backlog.NumImageTargets];
			try
			{
				backlog.CopyImageTargetNames(array, 0);
			}
			catch
			{
				return null;
			}
			return array[backlog.NumImageTargets - 1];
		}

		private static string GetLatestMTName(ConfigData backlog)
		{
			if (backlog == null)
			{
				return null;
			}
			string[] array = new string[backlog.NumMultiTargets];
			try
			{
				backlog.CopyMultiTargetNames(array, 0);
			}
			catch
			{
				return null;
			}
			return array[backlog.NumMultiTargets - 1];
		}
	}
}
