using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000A4 RID: 164
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetPlayerStatsMessage : Message
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0000401F File Offset: 0x0000221F
		// (set) Token: 0x060002ED RID: 749 RVA: 0x00004027 File Offset: 0x00002227
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x060002EE RID: 750 RVA: 0x00004030 File Offset: 0x00002230
		public GetPlayerStatsMessage()
		{
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00004038 File Offset: 0x00002238
		public GetPlayerStatsMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
