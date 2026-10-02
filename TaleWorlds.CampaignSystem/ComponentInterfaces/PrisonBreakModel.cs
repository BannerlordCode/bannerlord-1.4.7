using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001F0 RID: 496
	public abstract class PrisonBreakModel : MBGameModel<PrisonBreakModel>
	{
		// Token: 0x06001F43 RID: 8003
		public abstract int GetNumberOfGuardsToSpawn(Settlement settlement);

		// Token: 0x06001F44 RID: 8004
		public abstract bool CanPlayerStagePrisonBreak(Settlement settlement);

		// Token: 0x06001F45 RID: 8005
		public abstract int GetPrisonBreakStartCost(Hero prisonerHero);

		// Token: 0x06001F46 RID: 8006
		public abstract int GetRelationRewardOnPrisonBreak(Hero prisonerHero);

		// Token: 0x06001F47 RID: 8007
		public abstract float GetRogueryRewardOnPrisonBreak(Hero prisonerHero, bool isSuccess);
	}
}
