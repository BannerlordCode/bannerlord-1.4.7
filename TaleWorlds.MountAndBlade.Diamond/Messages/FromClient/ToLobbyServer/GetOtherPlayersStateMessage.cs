using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x0200009E RID: 158
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetOtherPlayersStateMessage : Message
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x00003F57 File Offset: 0x00002157
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x00003F5F File Offset: 0x0000215F
		[JsonProperty]
		public List<PlayerId> Players { get; private set; }

		// Token: 0x060002DA RID: 730 RVA: 0x00003F68 File Offset: 0x00002168
		public GetOtherPlayersStateMessage()
		{
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00003F70 File Offset: 0x00002170
		public GetOtherPlayersStateMessage(List<PlayerId> players)
		{
			this.Players = players;
		}
	}
}
