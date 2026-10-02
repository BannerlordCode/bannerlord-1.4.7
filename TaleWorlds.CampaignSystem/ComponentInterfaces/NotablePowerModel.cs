using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001E0 RID: 480
	public abstract class NotablePowerModel : MBGameModel<NotablePowerModel>
	{
		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06001EC2 RID: 7874
		public abstract int RegularNotableMaxPowerLevel { get; }

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06001EC3 RID: 7875
		public abstract int NotableDisappearPowerLimit { get; }

		// Token: 0x06001EC4 RID: 7876
		public abstract ExplainedNumber CalculateDailyPowerChangeForHero(Hero hero, bool includeDescriptions = false);

		// Token: 0x06001EC5 RID: 7877
		public abstract TextObject GetPowerRankName(Hero hero);

		// Token: 0x06001EC6 RID: 7878
		public abstract float GetInfluenceBonusToClan(Hero hero);

		// Token: 0x06001EC7 RID: 7879
		public abstract int GetInitialPower(Hero hero);

		// Token: 0x06001EC8 RID: 7880
		public abstract int GetInitialNotableSupporterCost(Hero hero);
	}
}
