using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x02000095 RID: 149
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetAnotherPlayerStateMessage : Message
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x00003ECF File Offset: 0x000020CF
		// (set) Token: 0x060002CA RID: 714 RVA: 0x00003ED7 File Offset: 0x000020D7
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x060002CB RID: 715 RVA: 0x00003EE0 File Offset: 0x000020E0
		public GetAnotherPlayerStateMessage()
		{
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00003EE8 File Offset: 0x000020E8
		public GetAnotherPlayerStateMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
