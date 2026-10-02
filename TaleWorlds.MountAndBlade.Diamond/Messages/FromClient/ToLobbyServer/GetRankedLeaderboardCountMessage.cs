using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000A7 RID: 167
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetRankedLeaderboardCountMessage : Message
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x00004057 File Offset: 0x00002257
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x0000405F File Offset: 0x0000225F
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x060002F4 RID: 756 RVA: 0x00004068 File Offset: 0x00002268
		public GetRankedLeaderboardCountMessage()
		{
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00004070 File Offset: 0x00002270
		public GetRankedLeaderboardCountMessage(string gameType)
		{
			this.GameType = gameType;
		}
	}
}
