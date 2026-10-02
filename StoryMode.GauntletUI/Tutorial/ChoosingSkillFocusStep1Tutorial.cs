using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200000D RID: 13
	[Tutorial("ChoosingSkillFocusStep1")]
	public class ChoosingSkillFocusStep1Tutorial : TutorialItemBase
	{
		// Token: 0x0600003C RID: 60 RVA: 0x000026A1 File Offset: 0x000008A1
		public ChoosingSkillFocusStep1Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "character_developer";
			base.MouseRequired = true;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000026C2 File Offset: 0x000008C2
		public override bool IsConditionsMetForCompletion()
		{
			return this._characterWindowOpened;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000026CA File Offset: 0x000008CA
		public override void OnTutorialContextChanged(TutorialContextChangedEvent obj)
		{
			this._characterWindowOpened = obj.NewContext == TutorialContexts.CharacterScreen;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000026DB File Offset: 0x000008DB
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000026DE File Offset: 0x000008DE
		public override bool IsConditionsMetForActivation()
		{
			return Settlement.CurrentSettlement == null && Hero.MainHero.HeroDeveloper.UnspentFocusPoints > 1 && (TutorialHelper.PlayerIsInAnySettlement || TutorialHelper.PlayerIsSafeOnMap) && TutorialHelper.CurrentContext == TutorialContexts.MapWindow;
		}

		// Token: 0x04000010 RID: 16
		private bool _characterWindowOpened;
	}
}
