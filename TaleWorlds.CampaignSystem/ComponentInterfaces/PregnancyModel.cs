using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001DF RID: 479
	public abstract class PregnancyModel : MBGameModel<PregnancyModel>
	{
		// Token: 0x06001EBB RID: 7867
		public abstract float GetDailyChanceOfPregnancyForHero(Hero hero);

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06001EBC RID: 7868
		public abstract float PregnancyDurationInDays { get; }

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06001EBD RID: 7869
		public abstract float MaternalMortalityProbabilityInLabor { get; }

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06001EBE RID: 7870
		public abstract float StillbirthProbability { get; }

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06001EBF RID: 7871
		public abstract float DeliveringFemaleOffspringProbability { get; }

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06001EC0 RID: 7872
		public abstract float DeliveringTwinsProbability { get; }
	}
}
