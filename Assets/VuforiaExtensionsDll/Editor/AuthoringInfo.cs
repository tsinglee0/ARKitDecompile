using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	internal class AuthoringInfo
	{
		private const string XML_NAME = "name";

		private const string XML_PREVIEW_IMAGE = "previewImage";

		private Dictionary<string, ConfigData.ImageTargetData> mImageTargets = new Dictionary<string, ConfigData.ImageTargetData>();

		private Dictionary<string, ConfigData.CylinderTargetData> mCylinderTargets = new Dictionary<string, ConfigData.CylinderTargetData>();

		private Dictionary<string, ConfigData.VuMarkData> mVuMarks = new Dictionary<string, ConfigData.VuMarkData>();

		private Dictionary<string, ConfigData.ObjectTargetData> mObjectTargets = new Dictionary<string, ConfigData.ObjectTargetData>();

		public AuthoringInfo()
		{
		}

		public AuthoringInfo(string xmlConfigFile)
		{
			this.ReadXmlFile(xmlConfigFile);
		}

		public bool TryGetInfo(string name, out ConfigData.ImageTargetData info)
		{
			return this.mImageTargets.TryGetValue(name, out info);
		}

		public bool TryGetInfo(string name, out ConfigData.CylinderTargetData info)
		{
			return this.mCylinderTargets.TryGetValue(name, out info);
		}

		public bool TryGetInfo(string name, out ConfigData.VuMarkData info)
		{
			return this.mVuMarks.TryGetValue(name, out info);
		}

		public bool TryGetInfo(string name, out ConfigData.ObjectTargetData info)
		{
			return this.mObjectTargets.TryGetValue(name, out info);
		}

		private void ReadXmlFile(string xmlConfigFile)
		{
			using (XmlTextReader xmlTextReader = new XmlTextReader(xmlConfigFile))
			{
				while (xmlTextReader.Read())
				{
					if (xmlTextReader.NodeType == XmlNodeType.Element)
					{
						string name = xmlTextReader.Name;
						if (!(name == "ImageTarget"))
						{
							if (!(name == "CylinderTarget"))
							{
								if (!(name == "VuMark"))
								{
									if (name == "ObjectTarget")
									{
										this.ReadObjectTarget(xmlTextReader);
									}
								}
								else
								{
									this.ReadVuMark(xmlTextReader);
								}
							}
							else
							{
								this.ReadCylinderTarget(xmlTextReader);
							}
						}
						else
						{
							this.ReadImageTarget(xmlTextReader);
						}
					}
				}
			}
		}

		private void ReadImageTarget(XmlTextReader configReader)
		{
			ConfigData.ImageTargetData value = default(ConfigData.ImageTargetData);
			string attribute = configReader.GetAttribute("name");
			if (attribute == null)
			{
				Debug.LogWarning("Info parser: Image target element without name is ignored.");
				return;
			}
			value.previewImage = configReader.GetAttribute("previewImage");
			this.mImageTargets.Add(attribute, value);
		}

		private void ReadVuMark(XmlTextReader configReader)
		{
			ConfigData.VuMarkData value = default(ConfigData.VuMarkData);
			string attribute = configReader.GetAttribute("name");
			if (attribute == null)
			{
				Debug.LogWarning("Info parser: VuMark element without name is ignored.");
				return;
			}
			value.previewImage = configReader.GetAttribute("previewImage");
			XmlReader xmlReader = configReader.ReadSubtree();
			while (xmlReader.Read())
			{
				if (xmlReader.NodeType == XmlNodeType.Element)
				{
					if (xmlReader.Name == "Code")
					{
						this.ReadVuMarkCode(xmlReader, ref value.idType, ref value.idLength);
					}
					else if (xmlReader.Name == "Shape")
					{
						this.ReadVuMarkShape(xmlReader, ref value.boundingBox2D, ref value.origin);
					}
				}
			}
			this.mVuMarks.Add(attribute, value);
		}

		private void ReadVuMarkCode(XmlReader configReader, ref InstanceIdType idType, ref int idLength)
		{
			string attribute = configReader.GetAttribute("IDtype");
			if (!(attribute == "numeric"))
			{
				if (!(attribute == "bytes"))
				{
					if (!(attribute == "string"))
					{
						Debug.LogWarning("Info parser: invalid VuMark ID Type");
					}
					else
					{
						idType = InstanceIdType.STRING;
					}
				}
				else
				{
					idType = InstanceIdType.BYTES;
				}
			}
			else
			{
				idType = InstanceIdType.NUMERIC;
			}
			if (!int.TryParse(configReader.GetAttribute("IDlength"), out idLength) && idType == InstanceIdType.STRING)
			{
				Debug.LogWarning("Info parser: invalid VuMark ID Length");
			}
		}

		private void ReadVuMarkShape(XmlReader configReader, ref Vector4 boundingBox2D, ref Vector2 origin)
		{
			string attribute = configReader.GetAttribute("bbox2D");
			if (attribute == null || !VuforiaUtilities.RectangleFromStringArray(out boundingBox2D, attribute.Split(new char[]
			{
				' '
			})))
			{
				Debug.LogWarning("Info parser: can't read bounding box of VuMark");
			}
			string attribute2 = configReader.GetAttribute("origin");
			if (attribute2 == null || !VuforiaUtilities.SizeFromStringArray(out origin, attribute2.Split(new char[]
			{
				' '
			})))
			{
				Debug.LogWarning("Info parser: can't read origin of VuMark");
			}
		}

		private void ReadCylinderTarget(XmlTextReader configReader)
		{
			ConfigData.CylinderTargetData cylinderTargetData = default(ConfigData.CylinderTargetData);
			string attribute = configReader.GetAttribute("name");
			if (attribute == null)
			{
				Debug.LogWarning("Info parser: Cylinder target element without name is ignored.");
				return;
			}
			float sideLength;
			if (float.TryParse(configReader.GetAttribute("sideLength"), out sideLength))
			{
				cylinderTargetData.sideLength = sideLength;
			}
			else
			{
				Debug.LogWarning("Info parser: invalid Cylinder Side Length");
			}
			float topDiameter;
			if (float.TryParse(configReader.GetAttribute("topDiameter"), out topDiameter))
			{
				cylinderTargetData.topDiameter = topDiameter;
			}
			else
			{
				Debug.LogWarning("Info parser: invalid Cylinder Top Diameter");
			}
			float bottomDiameter;
			if (float.TryParse(configReader.GetAttribute("bottomDiameter"), out bottomDiameter))
			{
				cylinderTargetData.bottomDiameter = bottomDiameter;
			}
			else
			{
				Debug.LogWarning("Info parser: invalid Cylinder Bottom Diameter");
			}
			cylinderTargetData.previewSide = configReader.GetAttribute("PreviewSide");
			string attribute2 = configReader.GetAttribute("PreviewTop");
			cylinderTargetData.hasTopGeometry = (attribute2 != null);
			cylinderTargetData.previewTop = (cylinderTargetData.hasTopGeometry ? attribute2 : "");
			string attribute3 = configReader.GetAttribute("PreviewBottom");
			cylinderTargetData.hasBottomGeometry = (attribute3 != null);
			cylinderTargetData.previewBottom = (cylinderTargetData.hasTopGeometry ? attribute3 : "");
			this.mCylinderTargets.Add(attribute, cylinderTargetData);
		}

		private void ReadObjectTarget(XmlTextReader configReader)
		{
			ConfigData.ObjectTargetData value = default(ConfigData.ObjectTargetData);
			string attribute = configReader.GetAttribute("name");
			if (attribute == null)
			{
				Debug.LogWarning("Info parser: Object target element without name is ignored.");
				return;
			}
			value.previewImage = configReader.GetAttribute("previewImage");
			this.mObjectTargets.Add(attribute, value);
		}
	}
}
