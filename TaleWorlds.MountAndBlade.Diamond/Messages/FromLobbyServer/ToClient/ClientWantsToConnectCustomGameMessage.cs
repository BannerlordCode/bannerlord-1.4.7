using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000027 RID: 39
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class ClientWantsToConnectCustomGameMessage : Message
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00002A5C File Offset: 0x00000C5C
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00002A64 File Offset: 0x00000C64
		[JsonProperty]
		public PlayerJoinGameData[] PlayerJoinGameData { get; private set; }

		// Token: 0x060000DA RID: 218 RVA: 0x00002A6D File Offset: 0x00000C6D
		public ClientWantsToConnectCustomGameMessage()
		{
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00002A75 File Offset: 0x00000C75
		public ClientWantsToConnectCustomGameMessage(PlayerJoinGameData[] playerJoinGameData)
		{
			this.PlayerJoinGameData = playerJoinGameData;
		}
	}
}
