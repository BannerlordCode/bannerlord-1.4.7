using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000035 RID: 53
	public abstract class EncyclopediaPageTutorialBase : TutorialItemBase
	{
		// Token: 0x06000108 RID: 264 RVA: 0x00003E5C File Offset: 0x0000205C
		public EncyclopediaPageTutorialBase(EncyclopediaPages activationPage, EncyclopediaPages alternateActivationPage)
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "";
			base.MouseRequired = false;
			this._activationPage = activationPage;
			this._alternateActivationPage = alternateActivationPage;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00003E8B File Offset: 0x0000208B
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.EncyclopediaWindow;
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00003E90 File Offset: 0x00002090
		public override bool IsConditionsMetForActivation()
		{
			EncyclopediaPages currentEncyclopediaPageContext = GauntletTutorialSystem.Current.CurrentEncyclopediaPageContext;
			bool isActive = this._isActive;
			this._isActive = currentEncyclopediaPageContext == this._activationPage || currentEncyclopediaPageContext == this._alternateActivationPage;
			if (!isActive && this._isActive)
			{
				this._lastActivatedPage = currentEncyclopediaPageContext;
			}
			return this._isActive;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00003EE0 File Offset: 0x000020E0
		public override bool IsConditionsMetForCompletion()
		{
			if (!this._isActive)
			{
				return false;
			}
			EncyclopediaPages currentEncyclopediaPageContext = GauntletTutorialSystem.Current.CurrentEncyclopediaPageContext;
			if (this._lastActivatedPage == this._alternateActivationPage)
			{
				return currentEncyclopediaPageContext != this._alternateActivationPage;
			}
			return currentEncyclopediaPageContext != EncyclopediaPages.Settlement && currentEncyclopediaPageContext != EncyclopediaPages.ListSettlements;
		}

		// Token: 0x04000040 RID: 64
		private bool _isActive;

		// Token: 0x04000041 RID: 65
		private readonly EncyclopediaPages _activationPage;

		// Token: 0x04000042 RID: 66
		private readonly EncyclopediaPages _alternateActivationPage;

		// Token: 0x04000043 RID: 67
		private EncyclopediaPages _lastActivatedPage;
	}
}
