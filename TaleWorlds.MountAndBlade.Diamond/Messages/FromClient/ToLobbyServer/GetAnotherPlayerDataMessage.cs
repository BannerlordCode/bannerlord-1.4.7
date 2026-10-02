using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000094 RID: 148
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetAnotherPlayerDataMessage : Message
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x00003EA7 File Offset: 0x000020A7
		// (set) Token: 0x060002C6 RID: 710 RVA: 0x00003EAF File Offset: 0x000020AF
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x060002C7 RID: 711 RVA: 0x00003EB8 File Offset: 0x000020B8
		public GetAnotherPlayerDataMessage()
		{
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00003EC0 File Offset: 0x000020C0
		public GetAnotherPlayerDataMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
