using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000066 RID: 102
	[Serializable]
	public class RequestJoinPlayerPartyMessageResult : FunctionResult
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000371C File Offset: 0x0000191C
		// (set) Token: 0x0600020C RID: 524 RVA: 0x00003724 File Offset: 0x00001924
		[JsonProperty]
		public bool Success { get; private set; }

		// Token: 0x0600020D RID: 525 RVA: 0x0000372D File Offset: 0x0000192D
		public RequestJoinPlayerPartyMessageResult()
		{
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00003735 File Offset: 0x00001935
		public RequestJoinPlayerPartyMessageResult(bool success)
		{
			this.Success = success;
		}
	}
}
