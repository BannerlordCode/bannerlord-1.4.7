using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x0200002B RID: 43
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class CustomBattleOverMessage : Message
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00002AFC File Offset: 0x00000CFC
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00002B04 File Offset: 0x00000D04
		[JsonProperty]
		public int OldExperience { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00002B0D File Offset: 0x00000D0D
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00002B15 File Offset: 0x00000D15
		[JsonProperty]
		public int NewExperience { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00002B1E File Offset: 0x00000D1E
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00002B26 File Offset: 0x00000D26
		[JsonProperty]
		public int GoldGain { get; set; }

		// Token: 0x060000EE RID: 238 RVA: 0x00002B2F File Offset: 0x00000D2F
		public CustomBattleOverMessage()
		{
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00002B37 File Offset: 0x00000D37
		public CustomBattleOverMessage(int oldExperience, int newExperience, int goldGain)
		{
			this.OldExperience = oldExperience;
			this.NewExperience = newExperience;
			this.GoldGain = goldGain;
		}
	}
}
