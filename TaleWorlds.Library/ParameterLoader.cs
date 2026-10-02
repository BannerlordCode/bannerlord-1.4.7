using System;
using System.IO;
using System.Xml;

namespace TaleWorlds.Library
{
	// Token: 0x02000078 RID: 120
	public class ParameterLoader
	{
		// Token: 0x06000452 RID: 1106 RVA: 0x0000F2FC File Offset: 0x0000D4FC
		public static ParameterContainer LoadParametersFromClientProfile(string configurationName)
		{
			ParameterContainer parameterContainer = new ParameterContainer();
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(VirtualFolders.GetFileContent(BasePath.Name + "Parameters/ClientProfile.xml", null));
			string innerText = xmlDocument.ChildNodes[0].Attributes["Value"].InnerText;
			ParameterLoader.LoadParametersInto(string.Concat(new string[] { "ClientProfiles/", innerText, "/", configurationName, ".xml" }), parameterContainer);
			return parameterContainer;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000F384 File Offset: 0x0000D584
		public static void LoadParametersInto(string fileFullName, ParameterContainer parameters)
		{
			XmlDocument xmlDocument = new XmlDocument();
			string text = BasePath.Name + "Parameters/" + fileFullName;
			xmlDocument.LoadXml(VirtualFolders.GetFileContent(text, null));
			foreach (object obj in xmlDocument.FirstChild.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "Parameters")
				{
					XmlAttributeCollection attributes = xmlNode.Attributes;
					string text2;
					if (attributes == null)
					{
						text2 = null;
					}
					else
					{
						XmlAttribute xmlAttribute = attributes["Platforms"];
						text2 = ((xmlAttribute != null) ? xmlAttribute.InnerText : null);
					}
					string text3 = text2;
					if (!string.IsNullOrWhiteSpace(text3))
					{
						if (text3.Split(new char[] { ',' }).FindIndex<string>((string p) => p.Trim().Equals(string.Concat(ApplicationPlatform.CurrentPlatform))) < 0)
						{
							continue;
						}
					}
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (xmlNode2.NodeType != XmlNodeType.Comment)
						{
							string innerText = xmlNode2.Attributes["Name"].InnerText;
							string text4;
							string text5;
							string text6;
							if (ParameterLoader.TryGetFromFile(xmlNode2, out text4))
							{
								text5 = text4;
							}
							else if (ParameterLoader.TryGetFromEnvironment(xmlNode2, out text6))
							{
								text5 = text6;
							}
							else if (xmlNode2.Attributes["DefaultValue"] != null)
							{
								text5 = xmlNode2.Attributes["DefaultValue"].InnerText;
							}
							else
							{
								text5 = xmlNode2.Attributes["Value"].InnerText;
							}
							parameters.AddParameter(innerText, text5, true);
						}
					}
				}
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000F588 File Offset: 0x0000D788
		private static bool TryGetFromFile(XmlNode node, out string value)
		{
			value = "";
			XmlAttributeCollection attributes = node.Attributes;
			if (((attributes != null) ? attributes["LoadFromFile"] : null) != null && node.Attributes["LoadFromFile"].InnerText.ToLower() == "true")
			{
				string innerText = node.Attributes["File"].InnerText;
				if (File.Exists(innerText))
				{
					string text = File.ReadAllText(innerText);
					value = text;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000F608 File Offset: 0x0000D808
		private static bool TryGetFromEnvironment(XmlNode node, out string value)
		{
			value = "";
			XmlAttributeCollection attributes = node.Attributes;
			if (((attributes != null) ? attributes["GetFromEnvironment"] : null) != null && node.Attributes["GetFromEnvironment"].InnerText.ToLower() == "true")
			{
				string innerText = node.Attributes["Name"].InnerText;
				string text = Environment.GetEnvironmentVariable(innerText);
				if (string.IsNullOrEmpty(text))
				{
					text = Environment.GetEnvironmentVariable(ParameterLoader.GetAltEnvironmentVariableName(innerText));
				}
				if (!string.IsNullOrEmpty(text))
				{
					value = text;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0000F69A File Offset: 0x0000D89A
		private static string GetAltEnvironmentVariableName(string name)
		{
			return name.Replace(".", "_").Replace(":", "__");
		}
	}
}
