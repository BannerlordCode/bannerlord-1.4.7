using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServerManager.ToBattleServer
{
	// Token: 0x020000E3 RID: 227
	[MessageDescription("BattleServerManager", "BattleServer", true)]
	[Serializable]
	public class PlayerDisconnectedFromLobbyMessage : Message
	{
		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x00004DC8 File Offset: 0x00002FC8
		// (set) Token: 0x06000429 RID: 1065 RVA: 0x00004DD0 File Offset: 0x00002FD0
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x0600042A RID: 1066 RVA: 0x00004DD9 File Offset: 0x00002FD9
		public PlayerDisconnectedFromLobbyMessage()
		{
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00004DE1 File Offset: 0x00002FE1
		public PlayerDisconnectedFromLobbyMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
