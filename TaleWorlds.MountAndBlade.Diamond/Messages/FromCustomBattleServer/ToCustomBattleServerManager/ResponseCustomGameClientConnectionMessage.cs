using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromCustomBattleServer.ToCustomBattleServerManager
{
	// Token: 0x0200000C RID: 12
	[MessageDescription("CustomBattleServer", "CustomBattleServerManager", false)]
	[Serializable]
	public class ResponseCustomGameClientConnectionMessage : Message
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002570 File Offset: 0x00000770
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00002578 File Offset: 0x00000778
		[JsonProperty]
		public PlayerJoinGameResponseDataFromHost[] PlayerJoinData { get; private set; }

		// Token: 0x06000061 RID: 97 RVA: 0x00002581 File Offset: 0x00000781
		public ResponseCustomGameClientConnectionMessage()
		{
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002589 File Offset: 0x00000789
		public ResponseCustomGameClientConnectionMessage(PlayerJoinGameResponseDataFromHost[] playerJoinData)
		{
			this.PlayerJoinData = playerJoinData;
		}
	}
}
