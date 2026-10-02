using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromClient.ToLobbyServer
{
	// Token: 0x020000C3 RID: 195
	[MessageDescription("Client", "LobbyServer", true)]
	[Serializable]
	public class ResponseCustomGameClientConnectionMessage : Message
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000385 RID: 901 RVA: 0x0000468E File Offset: 0x0000288E
		// (set) Token: 0x06000386 RID: 902 RVA: 0x00004696 File Offset: 0x00002896
		[JsonProperty]
		public PlayerJoinGameResponseDataFromHost[] PlayerJoinData { get; private set; }

		// Token: 0x06000387 RID: 903 RVA: 0x0000469F File Offset: 0x0000289F
		public ResponseCustomGameClientConnectionMessage()
		{
		}

		// Token: 0x06000388 RID: 904 RVA: 0x000046A7 File Offset: 0x000028A7
		public ResponseCustomGameClientConnectionMessage(PlayerJoinGameResponseDataFromHost[] playerJoinData)
		{
			this.PlayerJoinData = playerJoinData;
		}
	}
}
