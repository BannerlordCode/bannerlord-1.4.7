using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Ranked;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000040 RID: 64
	[Serializable]
	public class GetPlayerGameTypeRankInfoMessageResult : FunctionResult
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00002E8A File Offset: 0x0000108A
		// (set) Token: 0x06000142 RID: 322 RVA: 0x00002E92 File Offset: 0x00001092
		[JsonProperty]
		public GameTypeRankInfo[] GameTypeRankInfo { get; private set; }

		// Token: 0x06000143 RID: 323 RVA: 0x00002E9B File Offset: 0x0000109B
		public GetPlayerGameTypeRankInfoMessageResult()
		{
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00002EA3 File Offset: 0x000010A3
		public GetPlayerGameTypeRankInfoMessageResult(GameTypeRankInfo[] gameTypeRankInfo)
		{
			this.GameTypeRankInfo = gameTypeRankInfo;
		}
	}
}
