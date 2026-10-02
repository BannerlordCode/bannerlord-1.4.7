using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001E8 RID: 488
	public abstract class CompanionHiringPriceCalculationModel : MBGameModel<CompanionHiringPriceCalculationModel>
	{
		// Token: 0x06001F07 RID: 7943
		public abstract int GetCompanionHiringPrice(Hero companion);
	}
}
