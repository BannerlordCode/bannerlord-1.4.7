using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.ObjectSystem
{
	// Token: 0x02000013 RID: 19
	public static class XmlResource
	{
		// Token: 0x06000099 RID: 153 RVA: 0x00005028 File Offset: 0x00003228
		public static void ReadXsdFileAndExtractInformation(string xsdFilePath)
		{
			XDocument xdocument = XDocument.Load(xsdFilePath);
			XmlResource.XsdElementDictionary[xsdFilePath] = new Dictionary<string, XmlResource.XsdElement>();
			foreach (XElement xelement in xdocument.Descendants(XmlResource.XsNamespace + "element"))
			{
				string fullXPathOfElement = XmlResource.GetFullXPathOfElement(xelement, true);
				bool alwaysPreferMerge = XmlResource.GetAlwaysPreferMerge(xelement);
				XmlResource.XsdElement xsdElement = new XmlResource.XsdElement(fullXPathOfElement, alwaysPreferMerge);
				XmlResource.XsdElementDictionary[xsdFilePath][fullXPathOfElement] = xsdElement;
			}
			foreach (XElement xelement2 in xdocument.Descendants(XmlResource.XsNamespace + "unique").Concat<XElement>(xdocument.Descendants(XmlResource.XsNamespace + "key")))
			{
				string fullXPathOfElement2 = XmlResource.GetFullXPathOfElement(xelement2, true);
				string text = "/";
				XElement xelement3 = xelement2.Element(XmlResource.XsNamespace + "selector");
				string text2;
				if (xelement3 == null)
				{
					text2 = null;
				}
				else
				{
					XAttribute xattribute = xelement3.Attribute("xpath");
					text2 = ((xattribute != null) ? xattribute.Value : null);
				}
				string text3 = fullXPathOfElement2 + text + text2;
				foreach (XElement xelement4 in xelement2.Elements(XmlResource.XsNamespace + "field"))
				{
					List<string> uniqueAttributes = XmlResource.XsdElementDictionary[xsdFilePath][text3].UniqueAttributes;
					string text4;
					if (xelement4 == null)
					{
						text4 = null;
					}
					else
					{
						XAttribute xattribute2 = xelement4.Attribute("xpath");
						text4 = ((xattribute2 != null) ? xattribute2.Value.Substring(1) : null);
					}
					uniqueAttributes.Add(text4);
				}
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00005200 File Offset: 0x00003400
		private static bool GetAlwaysPreferMerge(XElement element)
		{
			XElement xelement = element.Element(XmlResource.XsNamespace + "annotation");
			if (xelement != null)
			{
				XElement xelement2 = xelement.Element(XmlResource.XsNamespace + "appinfo");
				if (xelement2 != null)
				{
					XElement xelement3 = xelement2.Element(XNamespace.None + "appSpecificNote");
					if (xelement3 != null && xelement3.Value.Trim() == "AlwaysPreferMerge")
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00005274 File Offset: 0x00003474
		public static string GetFullXPathOfElement(XElement element, bool isXsd = true)
		{
			if (element == null)
			{
				return null;
			}
			if (isXsd)
			{
				if (element.Name != XmlResource.XsNamespace + "element")
				{
					return XmlResource.GetFullXPathOfElement(element.Parent, true) ?? "";
				}
				string text = "";
				if (element.Attribute("name") != null)
				{
					text = element.Attribute("name").Value;
				}
				else if (element.Attribute("ref") != null)
				{
					text = element.Attribute("ref").Value;
				}
				if (element.Parent == null)
				{
					return text ?? "";
				}
				return XmlResource.GetFullXPathOfElement(element.Parent, true) + "/" + text;
			}
			else
			{
				if (element.Parent == null)
				{
					return string.Format("/{0}", element.Name);
				}
				return string.Format("{0}/{1}", XmlResource.GetFullXPathOfElement(element.Parent, false), element.Name);
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00005376 File Offset: 0x00003576
		public static void InitializeXmlInformationList(List<MbObjectXmlInformation> xmlInformation)
		{
			XmlResource.XmlInformationList = xmlInformation;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005380 File Offset: 0x00003580
		public static void GetMbprojxmls(string moduleName)
		{
			string mbprojPath = ModuleHelper.GetMbprojPath(moduleName);
			if (mbprojPath.Length > 0 && File.Exists(mbprojPath))
			{
				StreamReader streamReader = new StreamReader(mbprojPath);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(streamReader);
				XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("base").SelectNodes("file");
				if (xmlNodeList != null)
				{
					foreach (object obj in xmlNodeList)
					{
						XmlNode xmlNode = (XmlNode)obj;
						string innerText = xmlNode.Attributes["id"].InnerText;
						string innerText2 = xmlNode.Attributes["name"].InnerText;
						string xsdPath = ModuleHelper.GetXsdPath(innerText);
						if (File.Exists(xsdPath))
						{
							XmlResource.ReadXsdFileAndExtractInformation(xsdPath);
						}
						MbObjectXmlInformation mbObjectXmlInformation = new MbObjectXmlInformation
						{
							Id = innerText,
							Name = innerText2,
							ModuleName = moduleName,
							GameTypesIncluded = new List<string>()
						};
						XmlResource.MbprojXmls.Add(mbObjectXmlInformation);
					}
				}
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000054A8 File Offset: 0x000036A8
		public static void GetXmlListAndApply(string moduleName)
		{
			string path = ModuleHelper.GetPath(moduleName);
			new XmlReaderSettings().IgnoreComments = true;
			StreamReader streamReader = new StreamReader(path);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(streamReader);
			XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("Module").SelectNodes("Xmls/XmlNode");
			if (xmlNodeList != null)
			{
				foreach (object obj in xmlNodeList)
				{
					XmlNode xmlNode = (XmlNode)obj;
					XmlNode xmlNode2 = xmlNode.SelectSingleNode("XmlName");
					string innerText = xmlNode2.Attributes["id"].InnerText;
					string innerText2 = xmlNode2.Attributes["path"].InnerText;
					string xsdPath = ModuleHelper.GetXsdPath(innerText);
					if (File.Exists(xsdPath))
					{
						XmlResource.ReadXsdFileAndExtractInformation(xsdPath);
					}
					List<string> list = new List<string>();
					XmlNode xmlNode3 = xmlNode.SelectSingleNode("IncludedGameTypes");
					if (xmlNode3 != null)
					{
						foreach (object obj2 in xmlNode3.ChildNodes)
						{
							XmlNode xmlNode4 = (XmlNode)obj2;
							list.Add(xmlNode4.Attributes["value"].InnerText);
						}
					}
					MbObjectXmlInformation mbObjectXmlInformation = new MbObjectXmlInformation
					{
						Id = innerText,
						Name = innerText2,
						ModuleName = moduleName,
						GameTypesIncluded = list
					};
					XmlResource.XmlInformationList.Add(mbObjectXmlInformation);
				}
			}
		}

		// Token: 0x04000010 RID: 16
		public static List<MbObjectXmlInformation> XmlInformationList = new List<MbObjectXmlInformation>();

		// Token: 0x04000011 RID: 17
		public static List<MbObjectXmlInformation> MbprojXmls = new List<MbObjectXmlInformation>();

		// Token: 0x04000012 RID: 18
		public static Dictionary<string, Dictionary<string, XmlResource.XsdElement>> XsdElementDictionary = new Dictionary<string, Dictionary<string, XmlResource.XsdElement>>();

		// Token: 0x04000013 RID: 19
		public static XNamespace XsNamespace = "http://www.w3.org/2001/XMLSchema";

		// Token: 0x0200001A RID: 26
		public struct XsdElement
		{
			// Token: 0x060000E1 RID: 225 RVA: 0x00005F8E File Offset: 0x0000418E
			public XsdElement(string xPath, bool alwaysPreferMerge)
			{
				this.XPath = xPath;
				this.AlwaysPreferMerge = alwaysPreferMerge;
				this.UniqueAttributes = new List<string>();
			}

			// Token: 0x04000025 RID: 37
			public string XPath;

			// Token: 0x04000026 RID: 38
			public bool AlwaysPreferMerge;

			// Token: 0x04000027 RID: 39
			public List<string> UniqueAttributes;
		}
	}
}
