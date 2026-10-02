using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000074 RID: 116
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class AcceptPartyJoinRequestMessage : Message
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00003951 File Offset: 0x00001B51
		// (set) Token: 0x06000243 RID: 579 RVA: 0x00003959 File Offset: 0x00001B59
		[JsonProperty]
		public PlayerId RequesterPlayerId { get; private set; }

		// Token: 0x06000244 RID: 580 RVA: 0x00003962 File Offset: 0x00001B62
		public AcceptPartyJoinRequestMessage()
		{
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000396A File Offset: 0x00001B6A
		public AcceptPartyJoinRequestMessage(PlayerId requesterPlayerId)
		{
			this.RequesterPlayerId = requesterPlayerId;
		}
	}
}
