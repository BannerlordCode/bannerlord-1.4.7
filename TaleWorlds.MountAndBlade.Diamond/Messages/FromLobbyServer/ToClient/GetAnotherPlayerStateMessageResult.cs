using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000033 RID: 51
	[Serializable]
	public class GetAnotherPlayerStateMessageResult : FunctionResult
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00002C7C File Offset: 0x00000E7C
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00002C84 File Offset: 0x00000E84
		[JsonProperty]
		public AnotherPlayerData AnotherPlayerData { get; private set; }

		// Token: 0x0600010F RID: 271 RVA: 0x00002C8D File Offset: 0x00000E8D
		public GetAnotherPlayerStateMessageResult()
		{
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00002C95 File Offset: 0x00000E95
		public GetAnotherPlayerStateMessageResult(AnotherPlayerState anotherPlayerState, int anotherPlayerExperience)
		{
			this.AnotherPlayerData = new AnotherPlayerData(anotherPlayerState, anotherPlayerExperience);
		}
	}
}
