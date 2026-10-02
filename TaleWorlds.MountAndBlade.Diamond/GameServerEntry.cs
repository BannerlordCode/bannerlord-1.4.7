using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200011B RID: 283
	[Serializable]
	public class GameServerEntry
	{
		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x00008074 File Offset: 0x00006274
		// (set) Token: 0x06000631 RID: 1585 RVA: 0x0000807C File Offset: 0x0000627C
		[JsonProperty]
		public CustomBattleId Id { get; private set; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x00008085 File Offset: 0x00006285
		// (set) Token: 0x06000633 RID: 1587 RVA: 0x0000808D File Offset: 0x0000628D
		[JsonProperty]
		public string Address { get; private set; }

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x00008096 File Offset: 0x00006296
		// (set) Token: 0x06000635 RID: 1589 RVA: 0x0000809E File Offset: 0x0000629E
		[JsonProperty]
		public int Port { get; private set; }

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000636 RID: 1590 RVA: 0x000080A7 File Offset: 0x000062A7
		// (set) Token: 0x06000637 RID: 1591 RVA: 0x000080AF File Offset: 0x000062AF
		[JsonProperty]
		public string Region { get; private set; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x000080B8 File Offset: 0x000062B8
		// (set) Token: 0x06000639 RID: 1593 RVA: 0x000080C0 File Offset: 0x000062C0
		[JsonProperty]
		public int PlayerCount { get; private set; }

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x000080C9 File Offset: 0x000062C9
		// (set) Token: 0x0600063B RID: 1595 RVA: 0x000080D1 File Offset: 0x000062D1
		[JsonProperty]
		public int MaxPlayerCount { get; private set; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x000080DA File Offset: 0x000062DA
		// (set) Token: 0x0600063D RID: 1597 RVA: 0x000080E2 File Offset: 0x000062E2
		[JsonProperty]
		public string ServerName { get; private set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x000080EB File Offset: 0x000062EB
		// (set) Token: 0x0600063F RID: 1599 RVA: 0x000080F3 File Offset: 0x000062F3
		[JsonProperty]
		public string GameModule { get; private set; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x000080FC File Offset: 0x000062FC
		// (set) Token: 0x06000641 RID: 1601 RVA: 0x00008104 File Offset: 0x00006304
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x0000810D File Offset: 0x0000630D
		// (set) Token: 0x06000643 RID: 1603 RVA: 0x00008115 File Offset: 0x00006315
		[JsonProperty]
		public string Map { get; private set; }

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x0000811E File Offset: 0x0000631E
		// (set) Token: 0x06000645 RID: 1605 RVA: 0x00008126 File Offset: 0x00006326
		[JsonProperty]
		public string UniqueMapId { get; private set; }

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x0000812F File Offset: 0x0000632F
		// (set) Token: 0x06000647 RID: 1607 RVA: 0x00008137 File Offset: 0x00006337
		[JsonProperty]
		public int Ping { get; private set; }

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x00008140 File Offset: 0x00006340
		// (set) Token: 0x06000649 RID: 1609 RVA: 0x00008148 File Offset: 0x00006348
		[JsonProperty]
		public bool IsOfficial { get; private set; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x00008151 File Offset: 0x00006351
		// (set) Token: 0x0600064B RID: 1611 RVA: 0x00008159 File Offset: 0x00006359
		[JsonProperty]
		public bool ByOfficialProvider { get; private set; }

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x00008162 File Offset: 0x00006362
		// (set) Token: 0x0600064D RID: 1613 RVA: 0x0000816A File Offset: 0x0000636A
		[JsonProperty]
		public bool PasswordProtected { get; private set; }

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x00008173 File Offset: 0x00006373
		// (set) Token: 0x0600064F RID: 1615 RVA: 0x0000817B File Offset: 0x0000637B
		[JsonProperty]
		public int Permission { get; private set; }

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x00008184 File Offset: 0x00006384
		// (set) Token: 0x06000651 RID: 1617 RVA: 0x0000818C File Offset: 0x0000638C
		[JsonProperty]
		public bool CrossplayEnabled { get; private set; }

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x00008195 File Offset: 0x00006395
		// (set) Token: 0x06000653 RID: 1619 RVA: 0x0000819D File Offset: 0x0000639D
		[JsonProperty]
		public PlayerId HostId { get; private set; }

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x000081A6 File Offset: 0x000063A6
		// (set) Token: 0x06000655 RID: 1621 RVA: 0x000081AE File Offset: 0x000063AE
		[JsonProperty]
		public string HostName { get; private set; }

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x000081B7 File Offset: 0x000063B7
		// (set) Token: 0x06000657 RID: 1623 RVA: 0x000081BF File Offset: 0x000063BF
		[JsonProperty]
		public List<ModuleInfoModel> LoadedModules { get; private set; }

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x000081C8 File Offset: 0x000063C8
		// (set) Token: 0x06000659 RID: 1625 RVA: 0x000081D0 File Offset: 0x000063D0
		[JsonProperty]
		public bool AllowsOptionalModules { get; private set; }

		// Token: 0x0600065A RID: 1626 RVA: 0x000081D9 File Offset: 0x000063D9
		public GameServerEntry()
		{
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x000081E4 File Offset: 0x000063E4
		public GameServerEntry(CustomBattleId id, string serverName, string address, int port, string region, string gameModule, string gameType, string map, string uniqueMapId, int playerCount, int maxPlayerCount, bool isOfficial, bool byOfficialProvider, bool crossplayEnabled, PlayerId hostId, string hostName, List<ModuleInfoModel> loadedModules, bool allowsOptionalModules, bool passwordProtected = false, int permission = 0)
		{
			this.Id = id;
			this.ServerName = serverName;
			this.Address = address;
			this.GameModule = gameModule;
			this.GameType = gameType;
			this.Map = map;
			this.UniqueMapId = uniqueMapId;
			this.PlayerCount = playerCount;
			this.MaxPlayerCount = maxPlayerCount;
			this.Port = port;
			this.Region = region;
			this.IsOfficial = isOfficial;
			this.ByOfficialProvider = byOfficialProvider;
			this.CrossplayEnabled = crossplayEnabled;
			this.HostId = hostId;
			this.HostName = hostName;
			this.LoadedModules = loadedModules;
			this.AllowsOptionalModules = allowsOptionalModules;
			this.PasswordProtected = passwordProtected;
			this.Permission = permission;
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00008294 File Offset: 0x00006494
		public static void FilterGameServerEntriesBasedOnCrossplay(ref List<GameServerEntry> serverList, bool hasCrossplayPrivilege)
		{
			bool flag = ApplicationPlatform.CurrentPlatform == Platform.GDKDesktop;
			if (flag && !hasCrossplayPrivilege)
			{
				serverList.RemoveAll((GameServerEntry s) => s.CrossplayEnabled);
				return;
			}
			if (!flag)
			{
				serverList.RemoveAll((GameServerEntry s) => !s.CrossplayEnabled);
			}
		}
	}
}
