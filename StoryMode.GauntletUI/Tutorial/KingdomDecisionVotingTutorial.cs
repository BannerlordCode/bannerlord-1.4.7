using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000048 RID: 72
	[Tutorial("KingdomDecisionVotingTutorial")]
	public class KingdomDecisionVotingTutorial : TutorialItemBase
	{
		// Token: 0x06000159 RID: 345 RVA: 0x00004812 File Offset: 0x00002A12
		public KingdomDecisionVotingTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Left;
			base.HighlightedVisualElementID = "DecisionOptions";
			base.MouseRequired = false;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00004833 File Offset: 0x00002A33
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.KingdomScreen;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00004836 File Offset: 0x00002A36
		public override void OnPlayerSelectedAKingdomDecisionOption(PlayerSelectedAKingdomDecisionOptionEvent obj)
		{
			this._playerSelectedAnOption = true;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000483F File Offset: 0x00002A3F
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.IsKingdomDecisionPanelActiveAndHasOptions;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00004846 File Offset: 0x00002A46
		public override bool IsConditionsMetForCompletion()
		{
			return this._playerSelectedAnOption;
		}

		// Token: 0x0400005E RID: 94
		private bool _playerSelectedAnOption;
	}
}
