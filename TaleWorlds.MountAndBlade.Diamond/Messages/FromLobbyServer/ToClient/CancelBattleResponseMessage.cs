using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200001B RID: 27
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class CancelBattleResponseMessage : Message
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x0000286B File Offset: 0x00000A6B
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x00002873 File Offset: 0x00000A73
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x060000AA RID: 170 RVA: 0x0000287C File Offset: 0x00000A7C
		public CancelBattleResponseMessage()
		{
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00002884 File Offset: 0x00000A84
		public CancelBattleResponseMessage(bool successful)
		{
			this.Successful = successful;
		}
	}
}
