using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000020 RID: 32
	[Tutorial("CreateArmyStep3")]
	public class CreateArmyStep3Tutorial : TutorialItemBase
	{
		// Token: 0x0600009B RID: 155 RVA: 0x00003247 File Offset: 0x00001447
		public CreateArmyStep3Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = string.Empty;
			base.MouseRequired = true;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003268 File Offset: 0x00001468
		public override bool IsConditionsMetForCompletion()
		{
			return this._playerClosedArmyManagement;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003270 File Offset: 0x00001470
		public override void OnTutorialContextChanged(TutorialContextChangedEvent obj)
		{
			this._playerClosedArmyManagement = obj.NewContext != TutorialContexts.ArmyManagement;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003285 File Offset: 0x00001485
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.ArmyManagement;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003289 File Offset: 0x00001489
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.CurrentContext == TutorialContexts.ArmyManagement && Campaign.Current.CurrentMenuContext == null && Clan.PlayerClan.Kingdom != null && MobileParty.MainParty.Army == null;
		}

		// Token: 0x04000028 RID: 40
		private bool _playerClosedArmyManagement;
	}
}
