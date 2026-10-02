using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000042 RID: 66
	[Serializable]
	public class GetPremadeGameListResult : FunctionResult
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00002EDA File Offset: 0x000010DA
		// (set) Token: 0x0600014A RID: 330 RVA: 0x00002EE2 File Offset: 0x000010E2
		[JsonProperty]
		public PremadeGameList GameList { get; private set; }

		// Token: 0x0600014B RID: 331 RVA: 0x00002EEB File Offset: 0x000010EB
		public GetPremadeGameListResult()
		{
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00002EF3 File Offset: 0x000010F3
		public GetPremadeGameListResult(PremadeGameList gameList)
		{
			this.GameList = gameList;
		}
	}
}
