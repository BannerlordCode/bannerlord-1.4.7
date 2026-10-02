using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001DA RID: 474
	public abstract class DailyTroopXpBonusModel : MBGameModel<DailyTroopXpBonusModel>
	{
		// Token: 0x06001E9A RID: 7834
		public abstract int CalculateDailyTroopXpBonus(Town town);

		// Token: 0x06001E9B RID: 7835
		public abstract float CalculateGarrisonXpBonusMultiplier(Town town);
	}
}
