using System;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.MountAndBlade.Diamond;

namespace Messages.FromLobbyServer.ToClient
{
	// Token: 0x02000060 RID: 96
	[MessageDescription("LobbyServer", "Client", true)]
	[Serializable]
	public class PremadeGameEligibilityStatusMessage : Message
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001EE RID: 494 RVA: 0x000035EB File Offset: 0x000017EB
		// (set) Token: 0x060001EF RID: 495 RVA: 0x000035F3 File Offset: 0x000017F3
		[JsonProperty]
		public PremadeGameType[] EligibleGameTypes { get; private set; }

		// Token: 0x060001F0 RID: 496 RVA: 0x000035FC File Offset: 0x000017FC
		public PremadeGameEligibilityStatusMessage()
		{
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00003604 File Offset: 0x00001804
		public PremadeGameEligibilityStatusMessage(PremadeGameType[] eligibleGameTypes)
		{
			this.EligibleGameTypes = eligibleGameTypes;
		}
	}
}
