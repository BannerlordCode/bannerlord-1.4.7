using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000037 RID: 55
	[Serializable]
	public class GetClanHomeInfoResult : FunctionResult
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00002D22 File Offset: 0x00000F22
		// (set) Token: 0x0600011E RID: 286 RVA: 0x00002D2A File Offset: 0x00000F2A
		[JsonProperty]
		public ClanHomeInfo ClanHomeInfo { get; private set; }

		// Token: 0x0600011F RID: 287 RVA: 0x00002D33 File Offset: 0x00000F33
		public GetClanHomeInfoResult()
		{
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00002D3B File Offset: 0x00000F3B
		public GetClanHomeInfoResult(ClanHomeInfo clanHomeInfo)
		{
			this.ClanHomeInfo = clanHomeInfo;
		}
	}
}
