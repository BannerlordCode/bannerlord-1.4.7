using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServerManager.ToBattleServer
{
	// Token: 0x020000E1 RID: 225
	[MessageDescription("BattleServerManager", "BattleServer", true)]
	[Serializable]
	public class FriendlyDamageKickPlayerResponseMessage : Message
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x00004D2F File Offset: 0x00002F2F
		// (set) Token: 0x0600041B RID: 1051 RVA: 0x00004D37 File Offset: 0x00002F37
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x0600041C RID: 1052 RVA: 0x00004D40 File Offset: 0x00002F40
		public FriendlyDamageKickPlayerResponseMessage()
		{
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00004D48 File Offset: 0x00002F48
		public FriendlyDamageKickPlayerResponseMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
