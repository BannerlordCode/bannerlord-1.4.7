using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000036 RID: 54
	[Serializable]
	public class GetBannerlordIDMessageResult : FunctionResult
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00002CFA File Offset: 0x00000EFA
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00002D02 File Offset: 0x00000F02
		[JsonProperty]
		public string BannerlordID { get; private set; }

		// Token: 0x0600011B RID: 283 RVA: 0x00002D0B File Offset: 0x00000F0B
		public GetBannerlordIDMessageResult()
		{
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00002D13 File Offset: 0x00000F13
		public GetBannerlordIDMessageResult(string bannerlordID)
		{
			this.BannerlordID = bannerlordID;
		}
	}
}
