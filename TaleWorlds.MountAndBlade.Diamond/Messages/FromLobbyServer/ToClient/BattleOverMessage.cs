using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Ranked;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000017 RID: 23
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class BattleOverMessage : Message
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00002760 File Offset: 0x00000960
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00002768 File Offset: 0x00000968
		[JsonProperty]
		public int OldExperience { get; private set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00002771 File Offset: 0x00000971
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00002779 File Offset: 0x00000979
		[JsonProperty]
		public int NewExperience { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00002782 File Offset: 0x00000982
		// (set) Token: 0x06000094 RID: 148 RVA: 0x0000278A File Offset: 0x0000098A
		[JsonProperty]
		public List<string> EarnedBadges { get; private set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00002793 File Offset: 0x00000993
		// (set) Token: 0x06000096 RID: 150 RVA: 0x0000279B File Offset: 0x0000099B
		[JsonProperty]
		public int GoldGained { get; private set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000027A4 File Offset: 0x000009A4
		// (set) Token: 0x06000098 RID: 152 RVA: 0x000027AC File Offset: 0x000009AC
		[JsonProperty]
		public RankBarInfo OldInfo { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000027B5 File Offset: 0x000009B5
		// (set) Token: 0x0600009A RID: 154 RVA: 0x000027BD File Offset: 0x000009BD
		[JsonProperty]
		public RankBarInfo NewInfo { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600009B RID: 155 RVA: 0x000027C6 File Offset: 0x000009C6
		// (set) Token: 0x0600009C RID: 156 RVA: 0x000027CE File Offset: 0x000009CE
		[JsonProperty]
		public BattleCancelReason BattleCancelReason { get; private set; }

		// Token: 0x0600009D RID: 157 RVA: 0x000027D7 File Offset: 0x000009D7
		public BattleOverMessage()
		{
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000027DF File Offset: 0x000009DF
		public BattleOverMessage(int oldExperience, int newExperience, List<string> earnedBadges, int goldGained, BattleCancelReason battleCancelReason = BattleCancelReason.None)
		{
			this.OldExperience = oldExperience;
			this.NewExperience = newExperience;
			this.EarnedBadges = earnedBadges;
			this.GoldGained = goldGained;
			this.BattleCancelReason = battleCancelReason;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000280C File Offset: 0x00000A0C
		public BattleOverMessage(BattleCancelReason battleCancelReason)
		{
			this.BattleCancelReason = battleCancelReason;
		}
	}
}
