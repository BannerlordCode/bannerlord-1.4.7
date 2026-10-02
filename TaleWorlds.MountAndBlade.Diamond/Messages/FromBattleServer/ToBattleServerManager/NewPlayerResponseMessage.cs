using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServer.ToBattleServerManager
{
	// Token: 0x020000D9 RID: 217
	[MessageDescription("BattleServer", "BattleServerManager", false)]
	[Serializable]
	public class NewPlayerResponseMessage : Message
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00004BDE File Offset: 0x00002DDE
		// (set) Token: 0x060003FB RID: 1019 RVA: 0x00004BE6 File Offset: 0x00002DE6
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x00004BEF File Offset: 0x00002DEF
		// (set) Token: 0x060003FD RID: 1021 RVA: 0x00004BF7 File Offset: 0x00002DF7
		[JsonProperty]
		public PlayerBattleServerInformation PlayerBattleInformation { get; private set; }

		// Token: 0x060003FE RID: 1022 RVA: 0x00004C00 File Offset: 0x00002E00
		public NewPlayerResponseMessage()
		{
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00004C08 File Offset: 0x00002E08
		public NewPlayerResponseMessage(PlayerId playerId, PlayerBattleServerInformation playerBattleInformation)
		{
			this.PlayerId = playerId;
			this.PlayerBattleInformation = playerBattleInformation;
		}
	}
}
