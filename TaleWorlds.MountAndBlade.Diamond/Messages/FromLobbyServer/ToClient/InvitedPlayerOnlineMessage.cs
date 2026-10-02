using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200004A RID: 74
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class InvitedPlayerOnlineMessage : Message
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600017D RID: 381 RVA: 0x0000310D File Offset: 0x0000130D
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00003115 File Offset: 0x00001315
		[JsonProperty]
		public PlayerId PlayerId { get; set; }

		// Token: 0x0600017F RID: 383 RVA: 0x0000311E File Offset: 0x0000131E
		public InvitedPlayerOnlineMessage()
		{
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00003126 File Offset: 0x00001326
		public InvitedPlayerOnlineMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
