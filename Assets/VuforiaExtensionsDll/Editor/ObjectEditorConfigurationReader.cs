using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	internal static class ObjectEditorConfigurationReader
	{
		private const string OBJECT_TARGET_ELEMENT = "ObjectTarget";

		private const string NAME_ATTRIBUTE = "name";

		private const string BBOX_ATTRIBUTE = "bbox";

		private const string TARGETID_ATTRIBUTE = "targetId";

		public static void Read(string editorConfigurationFile, ConfigData.ObjectTargetData[] objectTargetData)
		{
			CultureInfo arg_05_0 = CultureInfo.InvariantCulture;
			using (XmlTextReader xmlTextReader = new XmlTextReader(editorConfigurationFile))
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				for (int i = 0; i < objectTargetData.Length; i++)
				{
					dictionary[objectTargetData[i].name] = i;
				}
				while (xmlTextReader.Read())
				{
					if (xmlTextReader.NodeType == XmlNodeType.Element)
					{
						string name = xmlTextReader.Name;
						if (name == "ObjectTarget")
						{
							string attribute = xmlTextReader.GetAttribute("name");
							if (attribute != null && dictionary.ContainsKey(attribute))
							{
								int num = dictionary[attribute];
								ConfigData.ObjectTargetData objectTargetData2 = objectTargetData[num];
								string attribute2 = xmlTextReader.GetAttribute("targetId");
								string[] array = xmlTextReader.GetAttribute("bbox").Split(new char[]
								{
									' '
								});
								if (array != null && attribute2 != null)
								{
									Vector3 bboxMin;
									Vector3 bboxMax;
									if (!VuforiaUtilities.BoundinBoxFromStringArray(out bboxMin, out bboxMax, array))
									{
										Debug.LogWarning("Found illegal bbox attribute for ObjectTarget " + array + " in editor configuration.xml. ObjectTarget will be ignored.");
									}
									else
									{
										objectTargetData2.targetID = attribute2;
										objectTargetData2.bboxMin = bboxMin;
										objectTargetData2.bboxMax = bboxMax;
										objectTargetData2.size = objectTargetData2.bboxMax - objectTargetData2.bboxMin;
										objectTargetData[num] = objectTargetData2;
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
