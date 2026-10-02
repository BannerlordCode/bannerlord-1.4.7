using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200001A RID: 26
	[Serializable]
	public class BuyCosmeticMessageResult : FunctionResult
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x0000282B File Offset: 0x00000A2B
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00002833 File Offset: 0x00000A33
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x0000283C File Offset: 0x00000A3C
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x00002844 File Offset: 0x00000A44
		[JsonProperty]
		public int Gold { get; private set; }

		// Token: 0x060000A6 RID: 166 RVA: 0x0000284D File Offset: 0x00000A4D
		public BuyCosmeticMessageResult()
		{
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00002855 File Offset: 0x00000A55
		public BuyCosmeticMessageResult(bool successful, int gold)
		{
			this.Successful = successful;
			this.Gold = gold;
		}
	}
}
