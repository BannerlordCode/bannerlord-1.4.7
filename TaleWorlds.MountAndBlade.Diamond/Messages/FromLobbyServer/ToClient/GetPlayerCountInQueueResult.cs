using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200003F RID: 63
	[Serializable]
	public class GetPlayerCountInQueueResult : FunctionResult
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00002E62 File Offset: 0x00001062
		// (set) Token: 0x0600013E RID: 318 RVA: 0x00002E6A File Offset: 0x0000106A
		[JsonProperty]
		public MatchmakingQueueStats MatchmakingQueueStats { get; private set; }

		// Token: 0x0600013F RID: 319 RVA: 0x00002E73 File Offset: 0x00001073
		public GetPlayerCountInQueueResult()
		{
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00002E7B File Offset: 0x0000107B
		public GetPlayerCountInQueueResult(MatchmakingQueueStats matchmakingQueueStats)
		{
			this.MatchmakingQueueStats = matchmakingQueueStats;
		}
	}
}
