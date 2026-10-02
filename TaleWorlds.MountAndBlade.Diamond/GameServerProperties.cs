using System;
using System.Collections.Generic;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200012B RID: 299
	[Serializable]
	public class GameServerProperties
	{
		// Token: 0x17000272 RID: 626
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x0000BA15 File Offset: 0x00009C15
		// (set) Token: 0x060007BE RID: 1982 RVA: 0x0000BA1D File Offset: 0x00009C1D
		public string Name { get; set; }

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x0000BA26 File Offset: 0x00009C26
		// (set) Token: 0x060007C0 RID: 1984 RVA: 0x0000BA2E File Offset: 0x00009C2E
		public string Address { get; set; }

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x0000BA37 File Offset: 0x00009C37
		// (set) Token: 0x060007C2 RID: 1986 RVA: 0x0000BA3F File Offset: 0x00009C3F
		public int Port { get; set; }

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x0000BA48 File Offset: 0x00009C48
		// (set) Token: 0x060007C4 RID: 1988 RVA: 0x0000BA50 File Offset: 0x00009C50
		public string Region { get; set; }

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x0000BA59 File Offset: 0x00009C59
		// (set) Token: 0x060007C6 RID: 1990 RVA: 0x0000BA61 File Offset: 0x00009C61
		public string GameModule { get; set; }

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x0000BA6A File Offset: 0x00009C6A
		// (set) Token: 0x060007C8 RID: 1992 RVA: 0x0000BA72 File Offset: 0x00009C72
		public string GameType { get; set; }

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x0000BA7B File Offset: 0x00009C7B
		// (set) Token: 0x060007CA RID: 1994 RVA: 0x0000BA83 File Offset: 0x00009C83
		public string Map { get; set; }

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x0000BA8C File Offset: 0x00009C8C
		// (set) Token: 0x060007CC RID: 1996 RVA: 0x0000BA94 File Offset: 0x00009C94
		public string UniqueMapId { get; set; }

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x0000BA9D File Offset: 0x00009C9D
		// (set) Token: 0x060007CE RID: 1998 RVA: 0x0000BAA5 File Offset: 0x00009CA5
		public string GamePassword { get; set; }

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x0000BAAE File Offset: 0x00009CAE
		// (set) Token: 0x060007D0 RID: 2000 RVA: 0x0000BAB6 File Offset: 0x00009CB6
		public string AdminPassword { get; set; }

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0000BABF File Offset: 0x00009CBF
		// (set) Token: 0x060007D2 RID: 2002 RVA: 0x0000BAC7 File Offset: 0x00009CC7
		public int MaxPlayerCount { get; set; }

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x0000BAD0 File Offset: 0x00009CD0
		// (set) Token: 0x060007D4 RID: 2004 RVA: 0x0000BAD8 File Offset: 0x00009CD8
		public bool PasswordProtected { get; set; }

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x0000BAE1 File Offset: 0x00009CE1
		// (set) Token: 0x060007D6 RID: 2006 RVA: 0x0000BAE9 File Offset: 0x00009CE9
		public bool IsOfficial { get; set; }

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x0000BAF2 File Offset: 0x00009CF2
		// (set) Token: 0x060007D8 RID: 2008 RVA: 0x0000BAFA File Offset: 0x00009CFA
		public bool ByOfficialProvider { get; set; }

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x0000BB03 File Offset: 0x00009D03
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x0000BB0B File Offset: 0x00009D0B
		public bool CrossplayEnabled { get; set; }

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x0000BB14 File Offset: 0x00009D14
		// (set) Token: 0x060007DC RID: 2012 RVA: 0x0000BB1C File Offset: 0x00009D1C
		public int Permission { get; set; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x0000BB25 File Offset: 0x00009D25
		// (set) Token: 0x060007DE RID: 2014 RVA: 0x0000BB2D File Offset: 0x00009D2D
		public PlayerId HostId { get; set; }

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x0000BB36 File Offset: 0x00009D36
		// (set) Token: 0x060007E0 RID: 2016 RVA: 0x0000BB3E File Offset: 0x00009D3E
		public string HostName { get; set; }

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x0000BB47 File Offset: 0x00009D47
		// (set) Token: 0x060007E2 RID: 2018 RVA: 0x0000BB4F File Offset: 0x00009D4F
		public List<ModuleInfoModel> LoadedModules { get; set; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x0000BB58 File Offset: 0x00009D58
		// (set) Token: 0x060007E4 RID: 2020 RVA: 0x0000BB60 File Offset: 0x00009D60
		public bool AllowsOptionalModules { get; set; }

		// Token: 0x060007E5 RID: 2021 RVA: 0x0000BB69 File Offset: 0x00009D69
		public GameServerProperties()
		{
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0000BB74 File Offset: 0x00009D74
		public GameServerProperties(string name, string address, int port, string region, string gameModule, string gameType, string map, string uniqueMapId, string gamePassword, string adminPassword, int maxPlayerCount, bool isOfficial, bool byOfficialProvider, bool crossplayEnabled, PlayerId hostId, string hostName, List<ModuleInfoModel> loadedModules, bool allowsOptionalModules, int permission)
		{
			this.Name = name;
			this.Address = address;
			this.Port = port;
			this.Region = region;
			this.GameModule = gameModule;
			this.GameType = gameType;
			this.Map = map;
			this.GamePassword = gamePassword;
			this.UniqueMapId = uniqueMapId;
			this.AdminPassword = adminPassword;
			this.MaxPlayerCount = maxPlayerCount;
			this.IsOfficial = isOfficial;
			this.ByOfficialProvider = byOfficialProvider;
			this.CrossplayEnabled = crossplayEnabled;
			this.HostId = hostId;
			this.HostName = hostName;
			this.LoadedModules = loadedModules;
			this.AllowsOptionalModules = allowsOptionalModules;
			this.PasswordProtected = gamePassword != null;
			this.Permission = permission;
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0000BC28 File Offset: 0x00009E28
		public void CheckAndReplaceProxyAddress(IReadOnlyDictionary<string, string> proxyAddressMap)
		{
			string text;
			if (proxyAddressMap != null && proxyAddressMap.TryGetValue(this.Address, out text))
			{
				this.Address = text;
			}
		}
	}
}
