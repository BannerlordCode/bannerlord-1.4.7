using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000045 RID: 69
	[Serializable]
	public class GetRankedLeaderboardMessageResult : FunctionResult
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00002F52 File Offset: 0x00001152
		// (set) Token: 0x06000156 RID: 342 RVA: 0x00002F5A File Offset: 0x0000115A
		[JsonProperty]
		public PlayerLeaderboardData[] LeaderboardPlayers { get; private set; }

		// Token: 0x06000157 RID: 343 RVA: 0x00002F63 File Offset: 0x00001163
		public GetRankedLeaderboardMessageResult()
		{
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00002F6B File Offset: 0x0000116B
		public GetRankedLeaderboardMessageResult(PlayerLeaderboardData[] leaderboardPlayers)
		{
			this.LeaderboardPlayers = leaderboardPlayers;
		}
	}
}
