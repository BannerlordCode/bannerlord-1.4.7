using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200003E RID: 62
	[Serializable]
	public class GetPlayerClanInfoResult : FunctionResult
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00002E3A File Offset: 0x0000103A
		// (set) Token: 0x0600013A RID: 314 RVA: 0x00002E42 File Offset: 0x00001042
		[JsonProperty]
		public ClanInfo ClanInfo { get; private set; }

		// Token: 0x0600013B RID: 315 RVA: 0x00002E4B File Offset: 0x0000104B
		public GetPlayerClanInfoResult()
		{
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00002E53 File Offset: 0x00001053
		public GetPlayerClanInfoResult(ClanInfo clanInfo)
		{
			this.ClanInfo = clanInfo;
		}
	}
}
