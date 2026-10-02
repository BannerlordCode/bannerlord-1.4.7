using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A2 RID: 418
	public abstract class MobilePartyFoodConsumptionModel : MBGameModel<MobilePartyFoodConsumptionModel>
	{
		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06001CB4 RID: 7348
		public abstract int NumberOfMenOnMapToEatOneFood { get; }

		// Token: 0x06001CB5 RID: 7349
		public abstract ExplainedNumber CalculateDailyBaseFoodConsumptionf(MobileParty party, bool includeDescription = false);

		// Token: 0x06001CB6 RID: 7350
		public abstract ExplainedNumber CalculateDailyFoodConsumptionf(MobileParty party, ExplainedNumber baseConsumption);

		// Token: 0x06001CB7 RID: 7351
		public abstract bool DoesPartyConsumeFood(MobileParty mobileParty);
	}
}
