using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.ArmyManagement;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200001F RID: 31
	[Tutorial("CreateArmyStep2")]
	public class CreateArmyStep2Tutorial : TutorialItemBase
	{
		// Token: 0x06000096 RID: 150 RVA: 0x000031DF File Offset: 0x000013DF
		public CreateArmyStep2Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.TopRight;
			base.HighlightedVisualElementID = "GatherArmyPartiesPanel";
			base.MouseRequired = true;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003200 File Offset: 0x00001400
		public override bool IsConditionsMetForCompletion()
		{
			return this._playerAddedPartyToArmy;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003208 File Offset: 0x00001408
		public override void OnPartyAddedToArmyByPlayer(PartyAddedToArmyByPlayerEvent obj)
		{
			this._playerAddedPartyToArmy = true;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003211 File Offset: 0x00001411
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.ArmyManagement;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003215 File Offset: 0x00001415
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.CurrentContext == TutorialContexts.ArmyManagement && Campaign.Current.CurrentMenuContext == null && Clan.PlayerClan.Kingdom != null && MobileParty.MainParty.Army == null;
		}

		// Token: 0x04000027 RID: 39
		private bool _playerAddedPartyToArmy;
	}
}
