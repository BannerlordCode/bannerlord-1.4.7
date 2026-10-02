using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000039 RID: 57
	[Serializable]
	public class GetDedicatedCustomServerAuthTokenMessageResult : FunctionResult
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00002D72 File Offset: 0x00000F72
		// (set) Token: 0x06000126 RID: 294 RVA: 0x00002D7A File Offset: 0x00000F7A
		[JsonProperty]
		public string AuthToken { get; private set; }

		// Token: 0x06000127 RID: 295 RVA: 0x00002D83 File Offset: 0x00000F83
		public GetDedicatedCustomServerAuthTokenMessageResult()
		{
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00002D8B File Offset: 0x00000F8B
		public GetDedicatedCustomServerAuthTokenMessageResult(string authToken)
		{
			this.AuthToken = authToken;
		}
	}
}
