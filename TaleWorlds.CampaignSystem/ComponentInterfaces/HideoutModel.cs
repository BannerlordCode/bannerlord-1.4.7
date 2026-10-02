using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B2 RID: 434
	public abstract class HideoutModel : MBGameModel<HideoutModel>
	{
		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06001D60 RID: 7520
		public abstract CampaignTime HideoutHiddenDuration { get; }

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06001D61 RID: 7521
		public abstract int CanAttackHideoutStartTime { get; }

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06001D62 RID: 7522
		public abstract int CanAttackHideoutEndTime { get; }

		// Token: 0x06001D63 RID: 7523
		public abstract float GetRogueryXpGainAsGhost();

		// Token: 0x06001D64 RID: 7524
		public abstract float GetRogueryXpGainOnHideoutMissionEnd(bool isSucceeded);

		// Token: 0x06001D65 RID: 7525
		public abstract float GetSendTroopsSuccessChance(Hideout hideout);
	}
}
