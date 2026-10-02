using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200000E RID: 14
	[Tutorial("ChoosingSkillFocusStep2")]
	public class ChoosingSkillFocusStep2Tutorial : TutorialItemBase
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00002711 File Offset: 0x00000911
		public ChoosingSkillFocusStep2Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.TopRight;
			base.HighlightedVisualElementID = "AddFocusButton";
			base.MouseRequired = true;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002732 File Offset: 0x00000932
		public override bool IsConditionsMetForCompletion()
		{
			return this._focusAdded;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000273A File Offset: 0x0000093A
		public override void OnFocusAddedByPlayer(FocusAddedByPlayerEvent obj)
		{
			this._focusAdded = true;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002743 File Offset: 0x00000943
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.CharacterScreen;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002746 File Offset: 0x00000946
		public override bool IsConditionsMetForActivation()
		{
			return Hero.MainHero.HeroDeveloper.UnspentFocusPoints > 1 && TutorialHelper.CurrentContext == TutorialContexts.CharacterScreen;
		}

		// Token: 0x04000011 RID: 17
		private bool _focusAdded;
	}
}
