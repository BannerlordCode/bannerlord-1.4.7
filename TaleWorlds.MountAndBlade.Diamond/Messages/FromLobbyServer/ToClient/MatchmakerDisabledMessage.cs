using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000052 RID: 82
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class MatchmakerDisabledMessage : Message
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00003340 File Offset: 0x00001540
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x00003348 File Offset: 0x00001548
		[JsonProperty]
		public int RemainingTime { get; private set; }

		// Token: 0x060001B2 RID: 434 RVA: 0x00003351 File Offset: 0x00001551
		public MatchmakerDisabledMessage()
		{
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00003359 File Offset: 0x00001559
		public MatchmakerDisabledMessage(int remainingTime)
		{
			this.RemainingTime = remainingTime;
		}
	}
}
