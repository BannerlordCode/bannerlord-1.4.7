using System;
using Helpers;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.Party;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000027 RID: 39
	[Tutorial("TakeAndRescuePrisonerTutorial")]
	public class TakingPrisonersTutorial : TutorialItemBase
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x000036BD File Offset: 0x000018BD
		public TakingPrisonersTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Top;
			base.HighlightedVisualElementID = "TransferButtonOnlyOtherPrisoners";
			base.MouseRequired = true;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000036DE File Offset: 0x000018DE
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.PartyScreen;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000036E4 File Offset: 0x000018E4
		public override bool IsConditionsMetForActivation()
		{
			PartyState partyState;
			return (partyState = GameStateManager.Current.ActiveState as PartyState) != null && partyState.PartyScreenMode == PartyScreenHelper.PartyScreenMode.Loot && partyState.PartyScreenLogic.PrisonerRosters[0].Count > 0 && TutorialHelper.CurrentContext == TutorialContexts.InventoryScreen;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000372C File Offset: 0x0000192C
		public override void OnPlayerMoveTroop(PlayerMoveTroopEvent obj)
		{
			base.OnPlayerMoveTroop(obj);
			if (obj.IsPrisoner && obj.ToSide == PartyScreenLogic.PartyRosterSide.Right && obj.Amount > 0)
			{
				this._playerMovedOtherPrisonerTroop = true;
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00003756 File Offset: 0x00001956
		public override bool IsConditionsMetForCompletion()
		{
			return this._playerMovedOtherPrisonerTroop;
		}

		// Token: 0x04000036 RID: 54
		private bool _playerMovedOtherPrisonerTroop;
	}
}
