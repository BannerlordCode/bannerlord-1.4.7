using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000035 RID: 53
	[Serializable]
	public class GetAverageMatchmakingWaitTimesResult : FunctionResult
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00002CD2 File Offset: 0x00000ED2
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00002CDA File Offset: 0x00000EDA
		[JsonProperty]
		public MatchmakingWaitTimeStats MatchmakingWaitTimeStats { get; private set; }

		// Token: 0x06000117 RID: 279 RVA: 0x00002CE3 File Offset: 0x00000EE3
		public GetAverageMatchmakingWaitTimesResult()
		{
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00002CEB File Offset: 0x00000EEB
		public GetAverageMatchmakingWaitTimesResult(MatchmakingWaitTimeStats matchmakingWaitTimeStats)
		{
			this.MatchmakingWaitTimeStats = matchmakingWaitTimeStats;
		}
	}
}
