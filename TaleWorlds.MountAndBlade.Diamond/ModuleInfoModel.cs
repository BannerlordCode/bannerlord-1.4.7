using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000138 RID: 312
	[Serializable]
	public class ModuleInfoModel
	{
		// Token: 0x1700029C RID: 668
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x0000C407 File Offset: 0x0000A607
		// (set) Token: 0x0600086D RID: 2157 RVA: 0x0000C40F File Offset: 0x0000A60F
		public string Id { get; private set; }

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x0000C418 File Offset: 0x0000A618
		// (set) Token: 0x0600086F RID: 2159 RVA: 0x0000C420 File Offset: 0x0000A620
		public string Name { get; private set; }

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x0000C429 File Offset: 0x0000A629
		// (set) Token: 0x06000871 RID: 2161 RVA: 0x0000C431 File Offset: 0x0000A631
		public ModuleCategory Category { get; private set; }

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x0000C43A File Offset: 0x0000A63A
		// (set) Token: 0x06000873 RID: 2163 RVA: 0x0000C442 File Offset: 0x0000A642
		public string Version { get; private set; }

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x0000C44B File Offset: 0x0000A64B
		[JsonIgnore]
		public bool IsOptional
		{
			get
			{
				return this.Category == ModuleCategory.MultiplayerOptional;
			}
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x0000C456 File Offset: 0x0000A656
		[JsonConstructor]
		private ModuleInfoModel(string id, string name, string version, ModuleCategory category)
		{
			this.Id = id;
			this.Name = name;
			this.Version = version;
			this.Category = category;
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x0000C47C File Offset: 0x0000A67C
		internal ModuleInfoModel(ModuleInfo moduleInfo)
			: this(moduleInfo.Id, moduleInfo.Name, moduleInfo.Version.ToString(), moduleInfo.Category)
		{
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0000C4B5 File Offset: 0x0000A6B5
		public static bool ShouldIncludeInSession(ModuleInfo moduleInfo)
		{
			return !moduleInfo.IsOfficial && moduleInfo.HasMultiplayerCategory;
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0000C4C7 File Offset: 0x0000A6C7
		public static bool TryCreateForSession(ModuleInfo moduleInfo, out ModuleInfoModel moduleInfoModel)
		{
			if (ModuleInfoModel.ShouldIncludeInSession(moduleInfo))
			{
				moduleInfoModel = new ModuleInfoModel(moduleInfo);
				return true;
			}
			moduleInfoModel = null;
			return false;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0000C4E0 File Offset: 0x0000A6E0
		public override bool Equals(object obj)
		{
			ModuleInfoModel moduleInfoModel;
			return (moduleInfoModel = obj as ModuleInfoModel) != null && this.Id == moduleInfoModel.Id && this.Version == moduleInfoModel.Version;
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0000C51D File Offset: 0x0000A71D
		public override int GetHashCode()
		{
			return (-612338121 * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Id)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Version);
		}
	}
}
