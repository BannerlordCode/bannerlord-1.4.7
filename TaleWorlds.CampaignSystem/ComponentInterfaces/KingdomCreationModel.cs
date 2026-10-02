using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A4 RID: 420
	public abstract class KingdomCreationModel : MBGameModel<KingdomCreationModel>
	{
		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06001CC1 RID: 7361
		public abstract int MinimumClanTierToCreateKingdom { get; }

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06001CC2 RID: 7362
		public abstract int MinimumNumberOfSettlementsOwnedToCreateKingdom { get; }

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06001CC3 RID: 7363
		public abstract int MinimumTroopCountToCreateKingdom { get; }

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06001CC4 RID: 7364
		public abstract int MaximumNumberOfInitialPolicies { get; }

		// Token: 0x06001CC5 RID: 7365
		public abstract bool IsPlayerKingdomCreationPossible(out List<TextObject> explanations);

		// Token: 0x06001CC6 RID: 7366
		public abstract bool IsPlayerKingdomAbdicationPossible(out List<TextObject> explanations);

		// Token: 0x06001CC7 RID: 7367
		public abstract IEnumerable<CultureObject> GetAvailablePlayerKingdomCultures();
	}
}
