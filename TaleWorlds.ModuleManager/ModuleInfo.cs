using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TaleWorlds.Library;

namespace TaleWorlds.ModuleManager
{
	// Token: 0x02000007 RID: 7
	public class ModuleInfo
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002EB6 File Offset: 0x000010B6
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00002EBE File Offset: 0x000010BE
		public bool IsSelected { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002EC7 File Offset: 0x000010C7
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002ECF File Offset: 0x000010CF
		public string Id { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002ED8 File Offset: 0x000010D8
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002EE0 File Offset: 0x000010E0
		public string Name { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002EE9 File Offset: 0x000010E9
		public bool IsOfficial
		{
			get
			{
				return this.Type > ModuleType.Community;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002EF4 File Offset: 0x000010F4
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00002EFC File Offset: 0x000010FC
		public bool IsDefault { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002F05 File Offset: 0x00001105
		public bool IsRequiredOfficial
		{
			get
			{
				return this.Type == ModuleType.Official;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002F10 File Offset: 0x00001110
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002F18 File Offset: 0x00001118
		public bool IsActive { get; private set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002F21 File Offset: 0x00001121
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002F29 File Offset: 0x00001129
		public ApplicationVersion Version { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002F32 File Offset: 0x00001132
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00002F3A File Offset: 0x0000113A
		public ApplicationVersion RequiredBaseVersion { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002F43 File Offset: 0x00001143
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00002F4B File Offset: 0x0000114B
		public ModuleCategory Category { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002F54 File Offset: 0x00001154
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002F5C File Offset: 0x0000115C
		public string FolderPath { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002F65 File Offset: 0x00001165
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00002F6D File Offset: 0x0000116D
		public ModuleType Type { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002F76 File Offset: 0x00001176
		public bool HasMultiplayerCategory
		{
			get
			{
				return this.Category == ModuleCategory.Multiplayer || this.Category == ModuleCategory.MultiplayerOptional;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002F8C File Offset: 0x0000118C
		public bool IsNative
		{
			get
			{
				return this.Id.Equals("Native", StringComparison.OrdinalIgnoreCase);
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002F9F File Offset: 0x0000119F
		public ModuleInfo()
		{
			this.DependedModules = new List<DependedModule>();
			this.SubModules = new List<SubModuleInfo>();
			this.ModulesToLoadAfterThis = new List<DependedModule>();
			this.IncompatibleModules = new List<DependedModule>();
			this.IsActive = true;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002FDC File Offset: 0x000011DC
		public void LoadWithFullPath(string fullPath)
		{
			this.SubModules.Clear();
			this.DependedModules.Clear();
			this.ModulesToLoadAfterThis.Clear();
			this.IncompatibleModules.Clear();
			this.FolderPath = fullPath;
			string text = this.FolderPath + "/SubModule.xml";
			Debug.Print("LoadWithFullPath  subModulePath = " + text, 0, Debug.DebugColor.White, 17592186044416UL);
			StreamReader streamReader = new StreamReader(text);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(streamReader);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("Module");
			this.Name = xmlNode.SelectSingleNode("Name").Attributes["value"].InnerText;
			this.Id = xmlNode.SelectSingleNode("Id").Attributes["value"].InnerText;
			if (!this.Id.Contains(';'.ToString()))
			{
				this.Id.Contains(':'.ToString());
			}
			this.Version = ApplicationVersion.FromString(xmlNode.SelectSingleNode("Version").Attributes["value"].InnerText, 0);
			if (xmlNode.SelectSingleNode("RequiredBaseVersion") != null)
			{
				this.RequiredBaseVersion = ApplicationVersion.FromString(xmlNode.SelectSingleNode("RequiredBaseVersion").Attributes["value"].InnerText, 0);
			}
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("DefaultModule");
			this.IsDefault = xmlNode2 != null && xmlNode2.Attributes["value"].InnerText.Equals("true");
			XmlNode xmlNode3 = xmlNode.SelectSingleNode("ModuleType");
			ModuleType moduleType;
			if (xmlNode3 != null && Enum.TryParse<ModuleType>(xmlNode3.Attributes["value"].InnerText, out moduleType))
			{
				this.Type = moduleType;
			}
			this.IsSelected = this.IsNative;
			this.Category = ModuleCategory.Singleplayer;
			XmlNode xmlNode4 = xmlNode.SelectSingleNode("ModuleCategory");
			ModuleCategory moduleCategory;
			if (xmlNode4 != null && Enum.TryParse<ModuleCategory>(xmlNode4.Attributes["value"].InnerText, out moduleCategory))
			{
				this.Category = moduleCategory;
			}
			XmlNode xmlNode5 = xmlNode.SelectSingleNode("DependedModules");
			XmlNodeList xmlNodeList = ((xmlNode5 != null) ? xmlNode5.SelectNodes("DependedModule") : null);
			if (xmlNodeList != null)
			{
				for (int i = 0; i < xmlNodeList.Count; i++)
				{
					string innerText = xmlNodeList[i].Attributes["Id"].InnerText;
					ApplicationVersion applicationVersion = ApplicationVersion.Empty;
					bool flag = false;
					if (xmlNodeList[i].Attributes["DependentVersion"] != null)
					{
						try
						{
							applicationVersion = ApplicationVersion.FromString(xmlNodeList[i].Attributes["DependentVersion"].InnerText, 0);
						}
						catch
						{
							string.Concat(new string[] { "Couldn't parse dependent version of ", innerText, " for ", this.Id, ". Using default version." });
						}
					}
					XmlAttribute xmlAttribute = xmlNodeList[i].Attributes["Optional"];
					bool flag2;
					if (bool.TryParse((xmlAttribute != null) ? xmlAttribute.InnerText : null, out flag2))
					{
						flag = flag2;
					}
					this.DependedModules.Add(new DependedModule(innerText, applicationVersion, flag));
				}
			}
			XmlNode xmlNode6 = xmlNode.SelectSingleNode("ModulesToLoadAfterThis");
			XmlNodeList xmlNodeList2 = ((xmlNode6 != null) ? xmlNode6.SelectNodes("Module") : null);
			if (xmlNodeList2 != null)
			{
				for (int j = 0; j < xmlNodeList2.Count; j++)
				{
					string innerText2 = xmlNodeList2[j].Attributes["Id"].InnerText;
					this.ModulesToLoadAfterThis.Add(new DependedModule(innerText2, ApplicationVersion.Empty, false));
				}
			}
			XmlNode xmlNode7 = xmlNode.SelectSingleNode("IncompatibleModules");
			XmlNodeList xmlNodeList3 = ((xmlNode7 != null) ? xmlNode7.SelectNodes("Module") : null);
			if (xmlNodeList3 != null)
			{
				for (int k = 0; k < xmlNodeList3.Count; k++)
				{
					string innerText3 = xmlNodeList3[k].Attributes["Id"].InnerText;
					this.IncompatibleModules.Add(new DependedModule(innerText3, ApplicationVersion.Empty, false));
				}
			}
			XmlNode xmlNode8 = xmlNode.SelectSingleNode("SubModules");
			XmlNodeList xmlNodeList4 = ((xmlNode8 != null) ? xmlNode8.SelectNodes("SubModule") : null);
			if (xmlNodeList4 != null)
			{
				for (int l = 0; l < xmlNodeList4.Count; l++)
				{
					SubModuleInfo subModuleInfo = new SubModuleInfo();
					try
					{
						subModuleInfo.LoadFrom(xmlNodeList4[l], this.FolderPath, this.IsOfficial);
					}
					catch
					{
						string.Format("Cannot load a submodule {0} under {1}", l, this.FolderPath);
					}
					this.SubModules.Add(subModuleInfo);
				}
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000034A0 File Offset: 0x000016A0
		public void ActivateModule()
		{
			this.IsActive = true;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000034A9 File Offset: 0x000016A9
		public void DeactivateModule()
		{
			this.IsActive = false;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000034B4 File Offset: 0x000016B4
		public void UpdateVersionChangeSet()
		{
			this.Version = new ApplicationVersion(this.Version.ApplicationVersionType, this.Version.Major, this.Version.Minor, this.Version.Revision, 117484);
		}

		// Token: 0x04000011 RID: 17
		private const int ModuleDefaultChangeSet = 0;

		// Token: 0x0400001C RID: 28
		public readonly List<SubModuleInfo> SubModules;

		// Token: 0x0400001D RID: 29
		public readonly List<DependedModule> DependedModules;

		// Token: 0x0400001E RID: 30
		public readonly List<DependedModule> ModulesToLoadAfterThis;

		// Token: 0x0400001F RID: 31
		public readonly List<DependedModule> IncompatibleModules;
	}
}
