using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromCustomBattleServerManager.ToCustomBattleServer
{
	// Token: 0x0200000F RID: 15
	[MessageDescription("CustomBattleServerManager", "CustomBattleServer", true)]
	[Serializable]
	public class ClientQuitFromCustomGameMessage : Message
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00002648 File Offset: 0x00000848
		// (set) Token: 0x06000074 RID: 116 RVA: 0x00002650 File Offset: 0x00000850
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x06000075 RID: 117 RVA: 0x00002659 File Offset: 0x00000859
		public ClientQuitFromCustomGameMessage()
		{
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002661 File Offset: 0x00000861
		public ClientQuitFromCustomGameMessage(PlayerId playerId)
		{
			this.PlayerId = playerId;
		}
	}
}
