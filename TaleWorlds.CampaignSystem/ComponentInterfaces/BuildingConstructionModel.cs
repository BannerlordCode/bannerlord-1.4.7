using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D7 RID: 471
	public abstract class BuildingConstructionModel : MBGameModel<BuildingConstructionModel>
	{
		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06001E8D RID: 7821
		public abstract int TownBoostCost { get; }

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06001E8E RID: 7822
		public abstract int TownBoostBonus { get; }

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06001E8F RID: 7823
		public abstract int CastleBoostCost { get; }

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06001E90 RID: 7824
		public abstract int CastleBoostBonus { get; }

		// Token: 0x06001E91 RID: 7825
		public abstract ExplainedNumber CalculateDailyConstructionPower(Town town, bool includeDescriptions = false);

		// Token: 0x06001E92 RID: 7826
		public abstract int CalculateDailyConstructionPowerWithoutBoost(Town town);

		// Token: 0x06001E93 RID: 7827
		public abstract int GetBoostCost(Town town);

		// Token: 0x06001E94 RID: 7828
		public abstract int GetBoostAmount(Town town);
	}
}
