using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000015 RID: 21
	[Tutorial("RecruitmentTutorialStep2")]
	public class RecruitmentStep2Tutorial : TutorialItemBase
	{
		// Token: 0x06000064 RID: 100 RVA: 0x00002B82 File Offset: 0x00000D82
		public RecruitmentStep2Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "AvailableTroops";
			base.MouseRequired = true;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002BA3 File Offset: 0x00000DA3
		public override bool IsConditionsMetForCompletion()
		{
			return this._recruitedTroopCount >= 4;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002BB1 File Offset: 0x00000DB1
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.RecruitmentWindow;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002BB4 File Offset: 0x00000DB4
		public override void OnPlayerRecruitedUnit(CharacterObject obj, int count)
		{
			this._recruitedTroopCount += count;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002BC4 File Offset: 0x00000DC4
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.PlayerCanRecruit && TutorialHelper.CurrentContext == TutorialContexts.RecruitmentWindow;
		}

		// Token: 0x04000018 RID: 24
		private int _recruitedTroopCount;
	}
}
