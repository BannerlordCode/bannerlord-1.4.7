using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000AE RID: 174
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class InviteToGameMessage : Message
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000316 RID: 790 RVA: 0x000041E1 File Offset: 0x000023E1
		// (set) Token: 0x06000317 RID: 791 RVA: 0x000041E9 File Offset: 0x000023E9
		[JsonProperty]
		public PlayerId InvitedPlayerId { get; private set; }

		// Token: 0x06000318 RID: 792 RVA: 0x000041F2 File Offset: 0x000023F2
		public InviteToGameMessage()
		{
		}

		// Token: 0x06000319 RID: 793 RVA: 0x000041FA File Offset: 0x000023FA
		public InviteToGameMessage(PlayerId invitedPlayerId)
		{
			this.InvitedPlayerId = invitedPlayerId;
		}
	}
}
