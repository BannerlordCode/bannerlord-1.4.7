using System;
using Helpers;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200000A RID: 10
	[Tutorial("ChoosingPerkUpgradesStep1")]
	public class ChoosingPerkUpgradesStep1Tutorial : TutorialItemBase
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00002585 File Offset: 0x00000785
		public ChoosingPerkUpgradesStep1Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "character_developer";
			base.MouseRequired = true;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000025A6 File Offset: 0x000007A6
		public override bool IsConditionsMetForCompletion()
		{
			return this._contextChangedToCharacterScreen;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000025AE File Offset: 0x000007AE
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000025B1 File Offset: 0x000007B1
		public override bool IsConditionsMetForActivation()
		{
			return (TutorialHelper.PlayerIsInAnySettlement || TutorialHelper.PlayerIsSafeOnMap) && PerkHelper.AvailablePerkCountOfHero(Hero.MainHero) > 1 && TutorialHelper.CurrentContext == TutorialContexts.MapWindow;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000025D8 File Offset: 0x000007D8
		public override void OnTutorialContextChanged(TutorialContextChangedEvent obj)
		{
			this._contextChangedToCharacterScreen = obj.NewContext == TutorialContexts.CharacterScreen;
		}

		// Token: 0x0400000D RID: 13
		private bool _contextChangedToCharacterScreen;
	}
}
