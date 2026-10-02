using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000A1 RID: 161
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetPlayerClanInfo : Message
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x00003FC7 File Offset: 0x000021C7
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x00003FCF File Offset: 0x000021CF
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x060002E5 RID: 741 RVA: 0x00003FD8 File Offset: 0x000021D8
		public GetPlayerClanInfo()
		{
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00003FE0 File Offset: 0x000021E0
		public GetPlayerClanInfo(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
