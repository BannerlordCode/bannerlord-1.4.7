using System;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001EA RID: 490
	public abstract class IssueModel : MBGameModel<IssueModel>
	{
		// Token: 0x06001F0C RID: 7948
		public abstract float GetIssueDifficultyMultiplier();

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06001F0D RID: 7949
		public abstract int IssueOwnerCoolDownInDays { get; }

		// Token: 0x06001F0E RID: 7950
		public abstract void GetIssueEffectsOfSettlement(IssueEffect issueEffect, Settlement settlement, ref ExplainedNumber explainedNumber);

		// Token: 0x06001F0F RID: 7951
		public abstract void GetIssueEffectOfHero(IssueEffect issueEffect, Hero hero, ref ExplainedNumber explainedNumber);

		// Token: 0x06001F10 RID: 7952
		public abstract void GetIssueEffectOfClan(IssueEffect issueEffect, Clan clan, ref ExplainedNumber explainedNumber);

		// Token: 0x06001F11 RID: 7953
		public abstract ValueTuple<int, int> GetCausalityForHero(Hero alternativeSolutionHero, IssueBase issue);

		// Token: 0x06001F12 RID: 7954
		public abstract float GetFailureRiskForHero(Hero alternativeSolutionHero, IssueBase issue);

		// Token: 0x06001F13 RID: 7955
		public abstract CampaignTime GetDurationOfResolutionForHero(Hero alternativeSolutionHero, IssueBase issue);

		// Token: 0x06001F14 RID: 7956
		public abstract int GetTroopsRequiredForHero(Hero alternativeSolutionHero, IssueBase issue);

		// Token: 0x06001F15 RID: 7957
		public abstract bool CanTroopsReturnFromAlternativeSolution();

		// Token: 0x06001F16 RID: 7958
		public abstract ValueTuple<SkillObject, int> GetIssueAlternativeSolutionSkill(Hero hero, IssueBase issue);
	}
}
