using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A3 RID: 419
	public abstract class PartyMoraleModel : MBGameModel<PartyMoraleModel>
	{
		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06001CB9 RID: 7353
		public abstract float HighMoraleValue { get; }

		// Token: 0x06001CBA RID: 7354
		public abstract int GetDailyStarvationMoralePenalty(PartyBase party);

		// Token: 0x06001CBB RID: 7355
		public abstract int GetDailyNoWageMoralePenalty(MobileParty party);

		// Token: 0x06001CBC RID: 7356
		public abstract float GetStandardBaseMorale(PartyBase party);

		// Token: 0x06001CBD RID: 7357
		public abstract float GetVictoryMoraleChange(PartyBase party);

		// Token: 0x06001CBE RID: 7358
		public abstract float GetDefeatMoraleChange(PartyBase party);

		// Token: 0x06001CBF RID: 7359
		public abstract ExplainedNumber GetEffectivePartyMorale(MobileParty party, bool includeDescription = false);
	}
}
