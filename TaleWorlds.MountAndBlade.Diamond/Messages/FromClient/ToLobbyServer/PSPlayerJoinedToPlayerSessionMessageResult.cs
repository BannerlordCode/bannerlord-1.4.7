using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000CE RID: 206
	[Serializable]
	public class PSPlayerJoinedToPlayerSessionMessageResult : FunctionResult
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x00004886 File Offset: 0x00002A86
		// (set) Token: 0x060003B7 RID: 951 RVA: 0x0000488E File Offset: 0x00002A8E
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x060003B8 RID: 952 RVA: 0x00004897 File Offset: 0x00002A97
		public PSPlayerJoinedToPlayerSessionMessageResult()
		{
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000489F File Offset: 0x00002A9F
		public PSPlayerJoinedToPlayerSessionMessageResult(bool successful)
		{
			this.Successful = successful;
		}
	}
}
