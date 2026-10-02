using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromBattleServerManager.ToBattleServer
{
	// Token: 0x020000E2 RID: 226
	[MessageDescription("BattleServerManager", "BattleServer", true)]
	[Serializable]
	public class NewPlayerMessage : Message
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x00004D57 File Offset: 0x00002F57
		// (set) Token: 0x0600041F RID: 1055 RVA: 0x00004D5F File Offset: 0x00002F5F
		[JsonProperty]
		public PlayerBattleInfo PlayerBattleInfo { get; private set; }

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x00004D68 File Offset: 0x00002F68
		// (set) Token: 0x06000421 RID: 1057 RVA: 0x00004D70 File Offset: 0x00002F70
		[JsonProperty]
		public PlayerData PlayerData { get; private set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x00004D79 File Offset: 0x00002F79
		// (set) Token: 0x06000423 RID: 1059 RVA: 0x00004D81 File Offset: 0x00002F81
		[JsonProperty]
		public Guid PlayerParty { get; private set; }

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x00004D8A File Offset: 0x00002F8A
		// (set) Token: 0x06000425 RID: 1061 RVA: 0x00004D92 File Offset: 0x00002F92
		[JsonProperty]
		public Dictionary<string, List<string>> UsedCosmetics { get; private set; }

		// Token: 0x06000426 RID: 1062 RVA: 0x00004D9B File Offset: 0x00002F9B
		public NewPlayerMessage()
		{
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00004DA3 File Offset: 0x00002FA3
		public NewPlayerMessage(PlayerData playerData, PlayerBattleInfo playerBattleInfo, Guid playerParty, Dictionary<string, List<string>> usedCosmetics)
		{
			this.PlayerBattleInfo = playerBattleInfo;
			this.PlayerData = playerData;
			this.PlayerParty = playerParty;
			this.UsedCosmetics = usedCosmetics;
		}
	}
}
