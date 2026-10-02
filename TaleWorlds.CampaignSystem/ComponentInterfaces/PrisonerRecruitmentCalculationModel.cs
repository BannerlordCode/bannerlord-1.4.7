using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001EB RID: 491
	public abstract class PrisonerRecruitmentCalculationModel : MBGameModel<PrisonerRecruitmentCalculationModel>
	{
		// Token: 0x06001F18 RID: 7960
		public abstract int GetConformityNeededToRecruitPrisoner(CharacterObject character);

		// Token: 0x06001F19 RID: 7961
		public abstract ExplainedNumber GetConformityChangePerHour(PartyBase party, CharacterObject character);

		// Token: 0x06001F1A RID: 7962
		public abstract int GetPrisonerRecruitmentMoraleEffect(PartyBase party, CharacterObject character, int num);

		// Token: 0x06001F1B RID: 7963
		public abstract bool IsPrisonerRecruitable(PartyBase party, CharacterObject character, out int conformityNeeded);

		// Token: 0x06001F1C RID: 7964
		public abstract bool ShouldPartyRecruitPrisoners(PartyBase party);

		// Token: 0x06001F1D RID: 7965
		public abstract int CalculateRecruitableNumber(PartyBase party, CharacterObject character);
	}
}
