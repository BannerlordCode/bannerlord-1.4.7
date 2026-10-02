using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000046 RID: 70
	[Serializable]
	public class GetUserCosmeticsInfoMessageResult : FunctionResult
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00002F7A File Offset: 0x0000117A
		// (set) Token: 0x0600015A RID: 346 RVA: 0x00002F82 File Offset: 0x00001182
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00002F8B File Offset: 0x0000118B
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00002F93 File Offset: 0x00001193
		[JsonProperty]
		public List<string> OwnedCosmetics { get; private set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00002F9C File Offset: 0x0000119C
		// (set) Token: 0x0600015E RID: 350 RVA: 0x00002FA4 File Offset: 0x000011A4
		[JsonProperty]
		public Dictionary<string, List<string>> UsedCosmetics { get; private set; }

		// Token: 0x0600015F RID: 351 RVA: 0x00002FAD File Offset: 0x000011AD
		public GetUserCosmeticsInfoMessageResult()
		{
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00002FB5 File Offset: 0x000011B5
		public GetUserCosmeticsInfoMessageResult(bool successful, List<string> ownedCosmetics, Dictionary<string, List<string>> usedCosmetics)
		{
			this.Successful = successful;
			this.OwnedCosmetics = ownedCosmetics;
			this.UsedCosmetics = usedCosmetics;
		}
	}
}
