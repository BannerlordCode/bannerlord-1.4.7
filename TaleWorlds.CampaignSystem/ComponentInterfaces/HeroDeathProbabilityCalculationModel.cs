using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D6 RID: 470
	public abstract class HeroDeathProbabilityCalculationModel : MBGameModel<HeroDeathProbabilityCalculationModel>
	{
		// Token: 0x06001E8B RID: 7819
		public abstract float CalculateHeroDeathProbability(Hero hero);
	}
}
