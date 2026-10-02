using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000B1 RID: 177
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class KickPlayerFromPartyMessage : Message
	{
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000324 RID: 804 RVA: 0x00004271 File Offset: 0x00002471
		// (set) Token: 0x06000325 RID: 805 RVA: 0x00004279 File Offset: 0x00002479
		[JsonProperty]
		public PlayerId KickedPlayerId { get; private set; }

		// Token: 0x06000326 RID: 806 RVA: 0x00004282 File Offset: 0x00002482
		public KickPlayerFromPartyMessage()
		{
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000428A File Offset: 0x0000248A
		public KickPlayerFromPartyMessage(PlayerId kickedPlayerId)
		{
			this.KickedPlayerId = kickedPlayerId;
		}
	}
}
