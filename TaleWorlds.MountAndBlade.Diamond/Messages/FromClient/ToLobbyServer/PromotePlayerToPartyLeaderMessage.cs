using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000B4 RID: 180
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class PromotePlayerToPartyLeaderMessage : Message
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000330 RID: 816 RVA: 0x000042E9 File Offset: 0x000024E9
		// (set) Token: 0x06000331 RID: 817 RVA: 0x000042F1 File Offset: 0x000024F1
		[JsonProperty]
		public PlayerId PromotedPlayerId { get; private set; }

		// Token: 0x06000332 RID: 818 RVA: 0x000042FA File Offset: 0x000024FA
		public PromotePlayerToPartyLeaderMessage()
		{
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00004302 File Offset: 0x00002502
		public PromotePlayerToPartyLeaderMessage(PlayerId promotedPlayerId)
		{
			this.PromotedPlayerId = promotedPlayerId;
		}
	}
}
