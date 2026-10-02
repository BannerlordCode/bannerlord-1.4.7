using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromCustomBattleServerManager.ToCustomBattleServer
{
	// Token: 0x02000010 RID: 16
	[MessageDescription("CustomBattleServerManager", "CustomBattleServer", true)]
	[Serializable]
	public class ClientWantsToConnectCustomGameMessage : Message
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00002670 File Offset: 0x00000870
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00002678 File Offset: 0x00000878
		[JsonProperty]
		public PlayerJoinGameData[] PlayerJoinGameData { get; private set; }

		// Token: 0x06000079 RID: 121 RVA: 0x00002681 File Offset: 0x00000881
		public ClientWantsToConnectCustomGameMessage()
		{
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002689 File Offset: 0x00000889
		public ClientWantsToConnectCustomGameMessage(PlayerJoinGameData[] playerJoinGameData)
		{
			this.PlayerJoinGameData = playerJoinGameData;
		}
	}
}
