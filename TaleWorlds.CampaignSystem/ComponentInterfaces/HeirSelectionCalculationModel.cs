using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D5 RID: 469
	public abstract class HeirSelectionCalculationModel : MBGameModel<HeirSelectionCalculationModel>
	{
		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06001E88 RID: 7816
		public abstract int HighestSkillPoint { get; }

		// Token: 0x06001E89 RID: 7817
		public abstract int CalculateHeirSelectionPoint(Hero candidateHeir, Hero deadHero, ref Hero maxSkillHero);
	}
}
