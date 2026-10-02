using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D2 RID: 466
	public abstract class ClanTierModel : MBGameModel<ClanTierModel>
	{
		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x06001E67 RID: 7783
		public abstract int MinClanTier { get; }

		// Token: 0x17000794 RID: 1940
		// (get) Token: 0x06001E68 RID: 7784
		public abstract int MaxClanTier { get; }

		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x06001E69 RID: 7785
		public abstract int MercenaryEligibleTier { get; }

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x06001E6A RID: 7786
		public abstract int VassalEligibleTier { get; }

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06001E6B RID: 7787
		public abstract int BannerEligibleTier { get; }

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06001E6C RID: 7788
		public abstract int RebelClanStartingTier { get; }

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06001E6D RID: 7789
		public abstract int CompanionToLordClanStartingTier { get; }

		// Token: 0x06001E6E RID: 7790
		public abstract int CalculateInitialRenown(Clan clan);

		// Token: 0x06001E6F RID: 7791
		public abstract int CalculateInitialInfluence(Clan clan);

		// Token: 0x06001E70 RID: 7792
		public abstract int CalculateTier(Clan clan);

		// Token: 0x06001E71 RID: 7793
		public abstract ValueTuple<ExplainedNumber, bool> HasUpcomingTier(Clan clan, out TextObject extraExplanation, bool includeDescriptions = false);

		// Token: 0x06001E72 RID: 7794
		public abstract int GetRequiredRenownForTier(int tier);

		// Token: 0x06001E73 RID: 7795
		public abstract int GetPartyLimitForTier(Clan clan, int clanTierToCheck);

		// Token: 0x06001E74 RID: 7796
		public abstract int GetCompanionLimit(Clan clan);
	}
}
