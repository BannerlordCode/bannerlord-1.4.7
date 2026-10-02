using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000038 RID: 56
	[Serializable]
	public class GetClanLeaderboardResult : FunctionResult
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00002D4A File Offset: 0x00000F4A
		// (set) Token: 0x06000122 RID: 290 RVA: 0x00002D52 File Offset: 0x00000F52
		[JsonProperty]
		public ClanLeaderboardInfo ClanLeaderboardInfo { get; private set; }

		// Token: 0x06000123 RID: 291 RVA: 0x00002D5B File Offset: 0x00000F5B
		public GetClanLeaderboardResult()
		{
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00002D63 File Offset: 0x00000F63
		public GetClanLeaderboardResult(ClanLeaderboardInfo info)
		{
			this.ClanLeaderboardInfo = info;
		}
	}
}
