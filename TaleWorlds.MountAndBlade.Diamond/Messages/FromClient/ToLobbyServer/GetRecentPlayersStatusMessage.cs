using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000A9 RID: 169
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetRecentPlayersStatusMessage : Message
	{
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060002FE RID: 766 RVA: 0x000040D7 File Offset: 0x000022D7
		// (set) Token: 0x060002FF RID: 767 RVA: 0x000040DF File Offset: 0x000022DF
		[JsonProperty]
		public PlayerId[] RecentPlayers { get; private set; }

		// Token: 0x06000300 RID: 768 RVA: 0x000040E8 File Offset: 0x000022E8
		public GetRecentPlayersStatusMessage()
		{
		}

		// Token: 0x06000301 RID: 769 RVA: 0x000040F0 File Offset: 0x000022F0
		public GetRecentPlayersStatusMessage(PlayerId[] recentPlayers)
		{
			this.RecentPlayers = recentPlayers;
		}
	}
}
