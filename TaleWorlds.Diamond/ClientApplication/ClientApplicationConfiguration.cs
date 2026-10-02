using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.Diamond.ClientApplication
{
	// Token: 0x02000043 RID: 67
	public class ClientApplicationConfiguration
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00004E2E File Offset: 0x0000302E
		// (set) Token: 0x06000185 RID: 389 RVA: 0x00004E36 File Offset: 0x00003036
		public string Name { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00004E3F File Offset: 0x0000303F
		// (set) Token: 0x06000187 RID: 391 RVA: 0x00004E47 File Offset: 0x00003047
		public string InheritFrom { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00004E50 File Offset: 0x00003050
		// (set) Token: 0x06000189 RID: 393 RVA: 0x00004E58 File Offset: 0x00003058
		public string[] Clients { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00004E61 File Offset: 0x00003061
		// (set) Token: 0x0600018B RID: 395 RVA: 0x00004E69 File Offset: 0x00003069
		public SessionProviderType SessionProviderType { get; set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00004E72 File Offset: 0x00003072
		// (set) Token: 0x0600018D RID: 397 RVA: 0x00004E7A File Offset: 0x0000307A
		public ParameterContainer Parameters { get; set; }

		// Token: 0x0600018E RID: 398 RVA: 0x00004E83 File Offset: 0x00003083
		public ClientApplicationConfiguration()
		{
			this.Name = "NewlyCreated";
			this.InheritFrom = "";
			this.Clients = new string[0];
			this.Parameters = new ParameterContainer();
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00004EB8 File Offset: 0x000030B8
		private void FillFromBase(ClientApplicationConfiguration baseConfiguration)
		{
			this.SessionProviderType = baseConfiguration.SessionProviderType;
			this.Parameters = baseConfiguration.Parameters.Clone();
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00004ED8 File Offset: 0x000030D8
		public static string GetDefaultConfigurationFromFile()
		{
			XmlDocument xmlDocument = new XmlDocument();
			string fileContent = VirtualFolders.GetFileContent(BasePath.Name + "Parameters/ClientProfile.xml", null);
			if (fileContent == "")
			{
				return "";
			}
			xmlDocument.LoadXml(fileContent);
			return xmlDocument.ChildNodes[0].Attributes["Value"].InnerText;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00004F3B File Offset: 0x0000313B
		public static void SetDefaultConfigurationCategory(string category)
		{
			ClientApplicationConfiguration._defaultConfigurationCategory = category;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00004F43 File Offset: 0x00003143
		public void FillFrom(string configurationName)
		{
			if (string.IsNullOrEmpty(ClientApplicationConfiguration._defaultConfigurationCategory))
			{
				ClientApplicationConfiguration._defaultConfigurationCategory = ClientApplicationConfiguration.GetDefaultConfigurationFromFile();
			}
			this.FillFrom(ClientApplicationConfiguration._defaultConfigurationCategory, configurationName);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00004F68 File Offset: 0x00003168
		public void FillFrom(string configurationCategory, string configurationName)
		{
			XmlDocument xmlDocument = new XmlDocument();
			if (configurationCategory == "")
			{
				return;
			}
			string fileContent = VirtualFolders.GetFileContent(string.Concat(new string[]
			{
				BasePath.Name,
				"Parameters/ClientProfiles/",
				configurationCategory,
				"/",
				configurationName,
				".xml"
			}), null);
			if (fileContent == "")
			{
				return;
			}
			xmlDocument.LoadXml(fileContent);
			this.Name = Path.GetFileNameWithoutExtension(configurationName);
			XmlNode firstChild = xmlDocument.FirstChild;
			if (firstChild.Attributes != null && firstChild.Attributes["InheritFrom"] != null)
			{
				this.InheritFrom = firstChild.Attributes["InheritFrom"].InnerText;
				ClientApplicationConfiguration clientApplicationConfiguration = new ClientApplicationConfiguration();
				clientApplicationConfiguration.FillFrom(configurationCategory, this.InheritFrom);
				this.FillFromBase(clientApplicationConfiguration);
			}
			ParameterLoader.LoadParametersInto(string.Concat(new string[] { "ClientProfiles/", configurationCategory, "/", configurationName, ".xml" }), this.Parameters);
			foreach (object obj in firstChild.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "SessionProvider")
				{
					string innerText = xmlNode.Attributes["Type"].InnerText;
					this.SessionProviderType = (SessionProviderType)Enum.Parse(typeof(SessionProviderType), innerText);
				}
				else if (xmlNode.Name == "Clients")
				{
					List<string> list = new List<string>();
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						string innerText2 = ((XmlNode)obj2).Attributes["Type"].InnerText;
						list.Add(innerText2);
					}
					this.Clients = list.ToArray();
				}
				else
				{
					xmlNode.Name == "Parameters";
				}
			}
		}

		// Token: 0x04000090 RID: 144
		private static string _defaultConfigurationCategory = "";
	}
}
