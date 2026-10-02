using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromCustomBattleServerManager.ToCustomBattleServer
{
	// Token: 0x02000012 RID: 18
	[MessageDescription("CustomBattleServerManager", "CustomBattleServer", true)]
	[Serializable]
	public class PlayerDisconnectedFromLobbyMessage : Message
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600007C RID: 124 RVA: 0x000026A0 File Offset: 0x000008A0
		// (set) Token: 0x0600007D RID: 125 RVA: 0x000026A8 File Offset: 0x000008A8
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x0600007E RID: 126 RVA: 0x000026B1 File Offset: 0x000008B1
		public PlayerDisconnectedFromLobbyMessage()
		{
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000026B9 File Offset: 0x000008B9
		public PlayerDisconnectedFromLobbyMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
