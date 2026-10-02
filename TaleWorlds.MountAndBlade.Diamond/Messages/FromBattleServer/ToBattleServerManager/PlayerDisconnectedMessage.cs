using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServer.ToBattleServerManager
{
	// Token: 0x020000DA RID: 218
	[MessageDescription("BattleServer", "BattleServerManager", false)]
	[Serializable]
	public class PlayerDisconnectedMessage : Message
	{
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x00004C1E File Offset: 0x00002E1E
		// (set) Token: 0x06000401 RID: 1025 RVA: 0x00004C26 File Offset: 0x00002E26
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x00004C2F File Offset: 0x00002E2F
		// (set) Token: 0x06000403 RID: 1027 RVA: 0x00004C37 File Offset: 0x00002E37
		[JsonProperty]
		public DisconnectType Type { get; private set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x00004C40 File Offset: 0x00002E40
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x00004C48 File Offset: 0x00002E48
		[JsonProperty]
		public bool IsAllowedLeave { get; private set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x00004C51 File Offset: 0x00002E51
		// (set) Token: 0x06000407 RID: 1031 RVA: 0x00004C59 File Offset: 0x00002E59
		[JsonProperty]
		public BattleResult BattleResult { get; private set; }

		// Token: 0x06000408 RID: 1032 RVA: 0x00004C62 File Offset: 0x00002E62
		public PlayerDisconnectedMessage()
		{
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00004C6A File Offset: 0x00002E6A
		public PlayerDisconnectedMessage(PlayerId playerId, DisconnectType type, bool isAllowedLeave, BattleResult battleResult)
		{
			this.PlayerId = playerId;
			this.Type = type;
			this.IsAllowedLeave = isAllowedLeave;
			this.BattleResult = battleResult;
		}
	}
}
