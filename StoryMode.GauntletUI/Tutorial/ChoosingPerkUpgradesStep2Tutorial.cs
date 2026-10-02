using System;
using Helpers;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper.PerkSelection;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200000B RID: 11
	[Tutorial("ChoosingPerkUpgradesStep2")]
	public class ChoosingPerkUpgradesStep2Tutorial : TutorialItemBase
	{
		// Token: 0x06000032 RID: 50 RVA: 0x000025E9 File Offset: 0x000007E9
		public ChoosingPerkUpgradesStep2Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.TopRight;
			base.HighlightedVisualElementID = "AvailablePerks";
			base.MouseRequired = true;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000260A File Offset: 0x0000080A
		public override bool IsConditionsMetForCompletion()
		{
			return this._perkPopupOpened;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002612 File Offset: 0x00000812
		public override void OnPerkSelectionToggle(PerkSelectionToggleEvent obj)
		{
			this._perkPopupOpened = true;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000261B File Offset: 0x0000081B
		public override bool IsConditionsMetForActivation()
		{
			return (TutorialHelper.PlayerIsInAnySettlement || TutorialHelper.PlayerIsSafeOnMap) && PerkHelper.AvailablePerkCountOfHero(Hero.MainHero) > 1 && TutorialHelper.CurrentContext == TutorialContexts.CharacterScreen;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002642 File Offset: 0x00000842
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.CharacterScreen;
		}

		// Token: 0x0400000E RID: 14
		private bool _perkPopupOpened;
	}
}
