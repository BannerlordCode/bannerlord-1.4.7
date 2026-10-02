using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B6 RID: 438
	public abstract class RansomValueCalculationModel : MBGameModel<RansomValueCalculationModel>
	{
		// Token: 0x06001D8B RID: 7563
		public abstract int PrisonerRansomValue(CharacterObject prisoner, Hero sellerHero = null);
	}
}
