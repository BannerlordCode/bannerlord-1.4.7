using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200002E RID: 46
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class EnterBattleWithPartyAnswer : Message
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00002B84 File Offset: 0x00000D84
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00002B8C File Offset: 0x00000D8C
		[JsonProperty]
		public bool Successful { get; private set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00002B95 File Offset: 0x00000D95
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x00002B9D File Offset: 0x00000D9D
		[JsonProperty]
		public string[] SelectedAndEnabledGameTypes { get; private set; }

		// Token: 0x060000F9 RID: 249 RVA: 0x00002BA6 File Offset: 0x00000DA6
		public EnterBattleWithPartyAnswer()
		{
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00002BAE File Offset: 0x00000DAE
		public EnterBattleWithPartyAnswer(bool successful, string[] selectedAndEnabledGameTypes)
		{
			this.Successful = successful;
			this.SelectedAndEnabledGameTypes = selectedAndEnabledGameTypes;
		}
	}
}
