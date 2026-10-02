using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServer.ToBattleServerManager
{
	// Token: 0x020000CF RID: 207
	[MessageDescription("BattleServer", "BattleServerManager", true)]
	[Serializable]
	public class BattleCancelledDueToPlayerQuitMessage : Message
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060003BA RID: 954 RVA: 0x000048AE File Offset: 0x00002AAE
		// (set) Token: 0x060003BB RID: 955 RVA: 0x000048B6 File Offset: 0x00002AB6
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060003BC RID: 956 RVA: 0x000048BF File Offset: 0x00002ABF
		// (set) Token: 0x060003BD RID: 957 RVA: 0x000048C7 File Offset: 0x00002AC7
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x060003BE RID: 958 RVA: 0x000048D0 File Offset: 0x00002AD0
		public BattleCancelledDueToPlayerQuitMessage()
		{
		}

		// Token: 0x060003BF RID: 959 RVA: 0x000048D8 File Offset: 0x00002AD8
		public BattleCancelledDueToPlayerQuitMessage(PlayerId playerId, string gameType)
		{
			this.PlayerId = playerId;
			this.GameType = gameType;
		}
	}
}
