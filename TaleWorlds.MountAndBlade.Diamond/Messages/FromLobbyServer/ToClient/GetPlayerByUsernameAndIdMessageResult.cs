using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200003D RID: 61
	[Serializable]
	public class GetPlayerByUsernameAndIdMessageResult : FunctionResult
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00002E12 File Offset: 0x00001012
		// (set) Token: 0x06000136 RID: 310 RVA: 0x00002E1A File Offset: 0x0000101A
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x06000137 RID: 311 RVA: 0x00002E23 File Offset: 0x00001023
		public GetPlayerByUsernameAndIdMessageResult()
		{
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00002E2B File Offset: 0x0000102B
		public GetPlayerByUsernameAndIdMessageResult(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
