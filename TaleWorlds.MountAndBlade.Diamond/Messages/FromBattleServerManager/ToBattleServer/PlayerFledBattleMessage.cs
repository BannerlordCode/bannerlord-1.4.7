using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServerManager.ToBattleServer
{
	// Token: 0x020000E4 RID: 228
	[MessageDescription("BattleServerManager", "BattleServer", true)]
	[Serializable]
	public class PlayerFledBattleMessage : Message
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x00004DF0 File Offset: 0x00002FF0
		// (set) Token: 0x0600042D RID: 1069 RVA: 0x00004DF8 File Offset: 0x00002FF8
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x0600042E RID: 1070 RVA: 0x00004E01 File Offset: 0x00003001
		public PlayerFledBattleMessage()
		{
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00004E09 File Offset: 0x00003009
		public PlayerFledBattleMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
