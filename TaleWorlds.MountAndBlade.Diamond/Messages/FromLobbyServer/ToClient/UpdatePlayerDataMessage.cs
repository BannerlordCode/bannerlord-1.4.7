using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200006B RID: 107
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class UpdatePlayerDataMessage : Message
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00003801 File Offset: 0x00001A01
		// (set) Token: 0x06000222 RID: 546 RVA: 0x00003809 File Offset: 0x00001A09
		[JsonProperty]
		public PlayerData PlayerData { get; private set; }

		// Token: 0x06000223 RID: 547 RVA: 0x00003812 File Offset: 0x00001A12
		public UpdatePlayerDataMessage()
		{
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000381A File Offset: 0x00001A1A
		public UpdatePlayerDataMessage(PlayerData playerData)
		{
			this.PlayerData = playerData;
		}
	}
}
