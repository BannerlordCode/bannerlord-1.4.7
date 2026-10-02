using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServer.ToBattleServerManager
{
	// Token: 0x020000DB RID: 219
	[MessageDescription("BattleServer", "BattleServerManager", false)]
	[Serializable]
	public class PlayerFledBattleAnswerMessage : Message
	{
		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x00004C8F File Offset: 0x00002E8F
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x00004C97 File Offset: 0x00002E97
		[JsonProperty]
		public BattleResult BattleResult { get; private set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x00004CA0 File Offset: 0x00002EA0
		// (set) Token: 0x0600040D RID: 1037 RVA: 0x00004CA8 File Offset: 0x00002EA8
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x00004CB1 File Offset: 0x00002EB1
		// (set) Token: 0x0600040F RID: 1039 RVA: 0x00004CB9 File Offset: 0x00002EB9
		[JsonProperty]
		public bool IsAllowedLeave { get; private set; }

		// Token: 0x06000410 RID: 1040 RVA: 0x00004CC2 File Offset: 0x00002EC2
		public PlayerFledBattleAnswerMessage()
		{
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00004CCA File Offset: 0x00002ECA
		public PlayerFledBattleAnswerMessage(PlayerId playerId, BattleResult battleResult, bool isAllowedLeave)
		{
			this.PlayerId = playerId;
			this.BattleResult = battleResult;
			this.IsAllowedLeave = isAllowedLeave;
		}
	}
}
