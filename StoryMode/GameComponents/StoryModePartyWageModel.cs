using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;

namespace StoryMode.GameComponents
{
	// Token: 0x02000048 RID: 72
	public class StoryModePartyWageModel : PartyWageModel
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x0001941D File Offset: 0x0001761D
		public override int MaxWagePaymentLimit
		{
			get
			{
				return base.BaseModel.MaxWagePaymentLimit;
			}
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0001942A File Offset: 0x0001762A
		public override int GetCharacterWage(CharacterObject character)
		{
			return base.BaseModel.GetCharacterWage(character);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00019438 File Offset: 0x00017638
		public override ExplainedNumber GetTotalWage(MobileParty mobileParty, TroopRoster troopRoster, bool includeDescriptions = false)
		{
			return base.BaseModel.GetTotalWage(mobileParty, troopRoster, includeDescriptions);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00019448 File Offset: 0x00017648
		public override ExplainedNumber GetTroopRecruitmentCost(CharacterObject troop, Hero buyerHero, bool withoutItemCost = false)
		{
			if (StoryModeManager.Current.MainStoryLine.TutorialPhase.IsCompleted)
			{
				return base.BaseModel.GetTroopRecruitmentCost(troop, buyerHero, withoutItemCost);
			}
			if (!(troop.StringId == "tutorial_placeholder_volunteer"))
			{
				return base.BaseModel.GetTroopRecruitmentCost(troop, buyerHero, withoutItemCost);
			}
			return new ExplainedNumber(50f, false, null);
		}

		// Token: 0x04000193 RID: 403
		private const int StoryModeTutorialTroopCost = 50;
	}
}
