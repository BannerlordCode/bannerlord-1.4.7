using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000A8 RID: 168
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class GetRankedLeaderboardMessage : Message
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000407F File Offset: 0x0000227F
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x00004087 File Offset: 0x00002287
		[JsonProperty]
		public string GameType { get; private set; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x00004090 File Offset: 0x00002290
		// (set) Token: 0x060002F9 RID: 761 RVA: 0x00004098 File Offset: 0x00002298
		[JsonProperty]
		public int StartIndex { get; private set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060002FA RID: 762 RVA: 0x000040A1 File Offset: 0x000022A1
		// (set) Token: 0x060002FB RID: 763 RVA: 0x000040A9 File Offset: 0x000022A9
		[JsonProperty]
		public int Count { get; private set; }

		// Token: 0x060002FC RID: 764 RVA: 0x000040B2 File Offset: 0x000022B2
		public GetRankedLeaderboardMessage()
		{
		}

		// Token: 0x060002FD RID: 765 RVA: 0x000040BA File Offset: 0x000022BA
		public GetRankedLeaderboardMessage(string gameType, int startIndex, int count)
		{
			this.GameType = gameType;
			this.StartIndex = startIndex;
			this.Count = count;
		}
	}
}
