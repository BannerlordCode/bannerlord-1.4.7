using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200002F RID: 47
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class EnterCustomBattleWithPartyAnswer : Message
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00002BC4 File Offset: 0x00000DC4
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00002BCC File Offset: 0x00000DCC
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x060000FD RID: 253 RVA: 0x00002BD5 File Offset: 0x00000DD5
		public EnterCustomBattleWithPartyAnswer()
		{
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00002BDD File Offset: 0x00000DDD
		public EnterCustomBattleWithPartyAnswer(bool successful)
		{
			this.Successful = successful;
		}
	}
}
