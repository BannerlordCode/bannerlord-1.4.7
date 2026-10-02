using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromBattleServer.ToBattleServerManager
{
	// Token: 0x020000D5 RID: 213
	[MessageDescription("BattleServer", "BattleServerManager", false)]
	[Serializable]
	public class BattleServerStatsUpdateMessage : Message
	{
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x00004B00 File Offset: 0x00002D00
		// (set) Token: 0x060003E7 RID: 999 RVA: 0x00004B08 File Offset: 0x00002D08
		[JsonProperty]
		public BattleResult BattleResult { get; private set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x00004B11 File Offset: 0x00002D11
		// (set) Token: 0x060003E9 RID: 1001 RVA: 0x00004B19 File Offset: 0x00002D19
		[JsonProperty]
		public Dictionary<int, int> TeamScores { get; private set; }

		// Token: 0x060003EA RID: 1002 RVA: 0x00004B22 File Offset: 0x00002D22
		public BattleServerStatsUpdateMessage()
		{
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00004B2A File Offset: 0x00002D2A
		public BattleServerStatsUpdateMessage(BattleResult battleResult, Dictionary<int, int> teamScores)
		{
			this.BattleResult = battleResult;
			this.TeamScores = teamScores;
		}
	}
}
