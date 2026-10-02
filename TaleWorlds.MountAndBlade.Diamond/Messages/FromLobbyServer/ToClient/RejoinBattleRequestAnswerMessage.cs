using System;
using TaleWorlds.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000063 RID: 99
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class RejoinBattleRequestAnswerMessage : Message
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00003663 File Offset: 0x00001863
		// (set) Token: 0x060001FB RID: 507 RVA: 0x0000366B File Offset: 0x0000186B
		public bool IsRejoinAccepted { get; set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00003674 File Offset: 0x00001874
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0000367C File Offset: 0x0000187C
		public bool IsSuccessful { get; set; }

		// Token: 0x060001FE RID: 510 RVA: 0x00003685 File Offset: 0x00001885
		public RejoinBattleRequestAnswerMessage()
		{
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000368D File Offset: 0x0000188D
		public RejoinBattleRequestAnswerMessage(bool isRejoinAccepted, bool isSuccessful)
		{
			this.IsRejoinAccepted = isRejoinAccepted;
			this.IsSuccessful = isSuccessful;
		}
	}
}
