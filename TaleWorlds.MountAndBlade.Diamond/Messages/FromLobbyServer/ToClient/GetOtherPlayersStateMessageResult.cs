using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200003B RID: 59
	[Serializable]
	public class GetOtherPlayersStateMessageResult : FunctionResult
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00002DC2 File Offset: 0x00000FC2
		// (set) Token: 0x0600012E RID: 302 RVA: 0x00002DCA File Offset: 0x00000FCA
		[JsonProperty]
		public List<ValueTuple<PlayerId, AnotherPlayerData>> States { get; private set; }

		// Token: 0x0600012F RID: 303 RVA: 0x00002DD3 File Offset: 0x00000FD3
		public GetOtherPlayersStateMessageResult()
		{
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00002DDB File Offset: 0x00000FDB
		public GetOtherPlayersStateMessageResult(List<ValueTuple<PlayerId, AnotherPlayerData>> states)
		{
			this.States = states;
		}
	}
}
