using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Vuforia.EditorClasses
{
	internal static class CylinderDatasetReader
	{
		private const string CONFIG_XML = "config.info";

		private const string CYLINDER_TARGET_ELEMENT = "CylinderTarget";

		private const string KEYFRAME_ELEMENT = "Keyframe";

		private const string NAME_ATTRIBUTE = "name";

		private const string SIDELENGTH_ATTRIBUTE = "sideLength";

		private const string TOP_DIAMETER_ATTRIBUTE = "topDiameter";

		private const string BOTTOM_DIAMETER_ATTRIBUTE = "bottomDiameter";

		public static void Read(string datFile, ConfigData.CylinderTargetData[] targetData)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			Stream stream = Unzipper.Instance.UnzipFile(datFile, "config.info");
			if (stream == null)
			{
				return;
			}
			using (XmlTextReader xmlTextReader = new XmlTextReader(stream))
			{
				int num = -1;
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				for (int i = 0; i < targetData.Length; i++)
				{
					dictionary[targetData[i].name] = i;
				}
				string b = "";
				string b2 = "";
				while (xmlTextReader.Read())
				{
					if (xmlTextReader.NodeType == XmlNodeType.Element)
					{
						string name = xmlTextReader.Name;
						if (!(name == "CylinderTarget"))
						{
							if (name == "Keyframe")
							{
								if (num >= 0)
								{
									ConfigData.CylinderTargetData cylinderTargetData = targetData[num];
									string attribute = xmlTextReader.GetAttribute("name");
									if (attribute == b2)
									{
										cylinderTargetData.hasTopGeometry = true;
									}
									else if (attribute == b)
									{
										cylinderTargetData.hasBottomGeometry = true;
									}
									targetData[num] = cylinderTargetData;
								}
							}
						}
						else
						{
							string attribute2 = xmlTextReader.GetAttribute("name");
							if (attribute2 == null || !dictionary.ContainsKey(attribute2))
							{
								num = -1;
							}
							else
							{
								int num2 = dictionary[attribute2];
								ConfigData.CylinderTargetData cylinderTargetData2 = targetData[num2];
								string attribute3 = xmlTextReader.GetAttribute("sideLength");
								string attribute4 = xmlTextReader.GetAttribute("topDiameter");
								string attribute5 = xmlTextReader.GetAttribute("bottomDiameter");
								if (attribute3 != null && attribute4 != null && attribute5 != null)
								{
									float num3 = float.Parse(attribute3, invariantCulture);
									float num4 = 1f;
									if (cylinderTargetData2.sideLength > 0f)
									{
										num4 = cylinderTargetData2.sideLength / num3;
									}
									else
									{
										cylinderTargetData2.sideLength = num3;
									}
									cylinderTargetData2.topDiameter = num4 * float.Parse(attribute4, invariantCulture);
									cylinderTargetData2.bottomDiameter = num4 * float.Parse(attribute5, invariantCulture);
									num = num2;
									targetData[num2] = cylinderTargetData2;
									b = CylinderDatasetReader.GetBottomImageFile(attribute2);
									b2 = CylinderDatasetReader.GetTopImageFile(attribute2);
								}
							}
						}
					}
				}
			}
			stream.Dispose();
		}

		private static string GetBottomImageFile(string name)
		{
			return name + ".Bottom";
		}

		private static string GetTopImageFile(string name)
		{
			return name + ".Top";
		}
	}
}
