using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;

namespace StoryMode.GameComponents
{
	// Token: 0x02000049 RID: 73
	public class StoryModePrisonerRecruitmentCalculationModel : PrisonerRecruitmentCalculationModel
	{
		// Token: 0x0600046B RID: 1131 RVA: 0x000194AF File Offset: 0x000176AF
		public override int CalculateRecruitableNumber(PartyBase party, CharacterObject character)
		{
			return base.BaseModel.CalculateRecruitableNumber(party, character);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x000194BE File Offset: 0x000176BE
		public override ExplainedNumber GetConformityChangePerHour(PartyBase party, CharacterObject character)
		{
			if (party == PartyBase.MainParty && !StoryModeManager.Current.MainStoryLine.TutorialPhase.IsCompleted)
			{
				return new ExplainedNumber(0f, false, null);
			}
			return base.BaseModel.GetConformityChangePerHour(party, character);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x000194F8 File Offset: 0x000176F8
		public override int GetConformityNeededToRecruitPrisoner(CharacterObject character)
		{
			return base.BaseModel.GetConformityNeededToRecruitPrisoner(character);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00019506 File Offset: 0x00017706
		public override int GetPrisonerRecruitmentMoraleEffect(PartyBase party, CharacterObject character, int num)
		{
			return base.BaseModel.GetPrisonerRecruitmentMoraleEffect(party, character, num);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00019516 File Offset: 0x00017716
		public override bool IsPrisonerRecruitable(PartyBase party, CharacterObject character, out int conformityNeeded)
		{
			return base.BaseModel.IsPrisonerRecruitable(party, character, out conformityNeeded);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00019526 File Offset: 0x00017726
		public override bool ShouldPartyRecruitPrisoners(PartyBase party)
		{
			return base.BaseModel.ShouldPartyRecruitPrisoners(party);
		}
	}
}
