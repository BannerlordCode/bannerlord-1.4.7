using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000CD RID: 205
	[Serializable]
	public class PlatformPlayerJoinedToPlayerSessionMessageResult : FunctionResult
	{
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x0000485E File Offset: 0x00002A5E
		// (set) Token: 0x060003B3 RID: 947 RVA: 0x00004866 File Offset: 0x00002A66
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x060003B4 RID: 948 RVA: 0x0000486F File Offset: 0x00002A6F
		public PlatformPlayerJoinedToPlayerSessionMessageResult()
		{
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00004877 File Offset: 0x00002A77
		public PlatformPlayerJoinedToPlayerSessionMessageResult(bool successful)
		{
			this.Successful = successful;
		}
	}
}
