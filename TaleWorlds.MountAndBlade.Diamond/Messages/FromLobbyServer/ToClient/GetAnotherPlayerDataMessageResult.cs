using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000032 RID: 50
	[Serializable]
	public class GetAnotherPlayerDataMessageResult : FunctionResult
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00002C54 File Offset: 0x00000E54
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00002C5C File Offset: 0x00000E5C
		[JsonProperty]
		public PlayerData AnotherPlayerData { get; private set; }

		// Token: 0x0600010B RID: 267 RVA: 0x00002C65 File Offset: 0x00000E65
		public GetAnotherPlayerDataMessageResult()
		{
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00002C6D File Offset: 0x00000E6D
		public GetAnotherPlayerDataMessageResult(PlayerData playerData)
		{
			this.AnotherPlayerData = playerData;
		}
	}
}
