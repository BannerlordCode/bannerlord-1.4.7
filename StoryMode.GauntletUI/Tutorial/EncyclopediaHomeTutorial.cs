using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000034 RID: 52
	[Tutorial("EncyclopediaHomeTutorial")]
	public class EncyclopediaHomeTutorial : TutorialItemBase
	{
		// Token: 0x06000104 RID: 260 RVA: 0x00003E00 File Offset: 0x00002000
		public EncyclopediaHomeTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "";
			base.MouseRequired = false;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00003E21 File Offset: 0x00002021
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.EncyclopediaWindow;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00003E25 File Offset: 0x00002025
		public override bool IsConditionsMetForActivation()
		{
			this._isActive = GauntletTutorialSystem.Current.CurrentEncyclopediaPageContext == EncyclopediaPages.Home;
			return this._isActive;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00003E40 File Offset: 0x00002040
		public override bool IsConditionsMetForCompletion()
		{
			return this._isActive && GauntletTutorialSystem.Current.CurrentEncyclopediaPageContext != EncyclopediaPages.Home;
		}

		// Token: 0x0400003F RID: 63
		private bool _isActive;
	}
}
