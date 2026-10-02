using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000044 RID: 68
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class GetRankedLeaderboardCountMessageResult : FunctionResult
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00002F2A File Offset: 0x0000112A
		// (set) Token: 0x06000152 RID: 338 RVA: 0x00002F32 File Offset: 0x00001132
		[JsonProperty]
		public int Count { get; private set; }

		// Token: 0x06000153 RID: 339 RVA: 0x00002F3B File Offset: 0x0000113B
		public GetRankedLeaderboardCountMessageResult()
		{
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00002F43 File Offset: 0x00001143
		public GetRankedLeaderboardCountMessageResult(int count)
		{
			this.Count = count;
		}
	}
}
