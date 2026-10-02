using System;
using Helpers;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper.PerkSelection;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200000C RID: 12
	[Tutorial("ChoosingPerkUpgradesStep3")]
	public class ChoosingPerkUpgradesStep3Tutorial : TutorialItemBase
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00002645 File Offset: 0x00000845
		public ChoosingPerkUpgradesStep3Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.BottomRight;
			base.HighlightedVisualElementID = "PerkSelectionContainer";
			base.MouseRequired = true;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002666 File Offset: 0x00000866
		public override bool IsConditionsMetForCompletion()
		{
			return this._perkSelectedByPlayer;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000266E File Offset: 0x0000086E
		public override void OnPerkSelectedByPlayer(PerkSelectedByPlayerEvent obj)
		{
			this._perkSelectedByPlayer = true;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002677 File Offset: 0x00000877
		public override bool IsConditionsMetForActivation()
		{
			return (TutorialHelper.PlayerIsInAnySettlement || TutorialHelper.PlayerIsSafeOnMap) && PerkHelper.AvailablePerkCountOfHero(Hero.MainHero) > 1 && TutorialHelper.CurrentContext == TutorialContexts.CharacterScreen;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000269E File Offset: 0x0000089E
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.CharacterScreen;
		}

		// Token: 0x0400000F RID: 15
		private bool _perkSelectedByPlayer;
	}
}
