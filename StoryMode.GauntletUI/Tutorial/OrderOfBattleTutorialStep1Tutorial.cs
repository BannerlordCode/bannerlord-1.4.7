using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000040 RID: 64
	[Tutorial("OrderOfBattleTutorialStep1")]
	public class OrderOfBattleTutorialStep1Tutorial : TutorialItemBase
	{
		// Token: 0x0600012B RID: 299 RVA: 0x000044A0 File Offset: 0x000026A0
		public OrderOfBattleTutorialStep1Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Center;
			base.HighlightedVisualElementID = "AssignCaptain";
			base.MouseRequired = false;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000044C1 File Offset: 0x000026C1
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000044C4 File Offset: 0x000026C4
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.IsOrderOfBattleOpenAndReady && TutorialHelper.IsPlayerEncounterLeader && !TutorialHelper.IsNavalMission;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000044DE File Offset: 0x000026DE
		public override void OnOrderOfBattleHeroAssignedToFormation(OrderOfBattleHeroAssignedToFormationEvent obj)
		{
			this._playerAssignedACaptainToFormationInOoB = true;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000044E7 File Offset: 0x000026E7
		public override bool IsConditionsMetForCompletion()
		{
			return this._playerAssignedACaptainToFormationInOoB;
		}

		// Token: 0x04000050 RID: 80
		private bool _playerAssignedACaptainToFormationInOoB;
	}
}
