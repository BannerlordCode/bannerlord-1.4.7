using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200002C RID: 44
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class CustomGameServerListResponse : FunctionResult
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00002B54 File Offset: 0x00000D54
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00002B5C File Offset: 0x00000D5C
		[JsonProperty]
		public AvailableCustomGames AvailableCustomGames { get; private set; }

		// Token: 0x060000F2 RID: 242 RVA: 0x00002B65 File Offset: 0x00000D65
		public CustomGameServerListResponse()
		{
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00002B6D File Offset: 0x00000D6D
		public CustomGameServerListResponse(AvailableCustomGames availableCustomGames)
		{
			this.AvailableCustomGames = availableCustomGames;
		}
	}
}
