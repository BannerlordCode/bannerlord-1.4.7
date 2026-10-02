using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000A3 RID: 163
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetPlayerGameTypeRankInfoMessage : Message
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x00003FF7 File Offset: 0x000021F7
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x00003FFF File Offset: 0x000021FF
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x060002EA RID: 746 RVA: 0x00004008 File Offset: 0x00002208
		public GetPlayerGameTypeRankInfoMessage()
		{
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00004010 File Offset: 0x00002210
		public GetPlayerGameTypeRankInfoMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
