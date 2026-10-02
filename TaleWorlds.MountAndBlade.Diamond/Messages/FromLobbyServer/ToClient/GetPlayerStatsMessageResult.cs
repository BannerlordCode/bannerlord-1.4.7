using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000041 RID: 65
	[Serializable]
	public class GetPlayerStatsMessageResult : FunctionResult
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00002EB2 File Offset: 0x000010B2
		// (set) Token: 0x06000146 RID: 326 RVA: 0x00002EBA File Offset: 0x000010BA
		[JsonProperty]
		public PlayerStatsBase[] PlayerStats { get; private set; }

		// Token: 0x06000147 RID: 327 RVA: 0x00002EC3 File Offset: 0x000010C3
		public GetPlayerStatsMessageResult()
		{
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00002ECB File Offset: 0x000010CB
		public GetPlayerStatsMessageResult(PlayerStatsBase[] playerStats)
		{
			this.PlayerStats = playerStats;
		}
	}
}
