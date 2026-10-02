using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServer.ToBattleServerManager
{
	// Token: 0x020000D2 RID: 210
	[MessageDescription("BattleServer", "BattleServerManager", true)]
	[Serializable]
	public class BattleInitializedMessage : Message
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060003CF RID: 975 RVA: 0x000049EC File Offset: 0x00002BEC
		[JsonProperty]
		public string GameType { get; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x000049F4 File Offset: 0x00002BF4
		[JsonProperty]
		public List<PlayerId> AssignedPlayers { get; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x000049FC File Offset: 0x00002BFC
		[JsonProperty]
		public string Faction1 { get; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x00004A04 File Offset: 0x00002C04
		[JsonProperty]
		public string Faction2 { get; }

		// Token: 0x060003D3 RID: 979 RVA: 0x00004A0C File Offset: 0x00002C0C
		public BattleInitializedMessage()
		{
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00004A14 File Offset: 0x00002C14
		public BattleInitializedMessage(string gameType, List<PlayerId> assignedPlayers, string faction1, string faction2)
		{
			this.GameType = gameType;
			this.AssignedPlayers = assignedPlayers;
			this.Faction1 = faction1;
			this.Faction2 = faction2;
		}
	}
}
