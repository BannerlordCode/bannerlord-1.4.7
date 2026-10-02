using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200001C RID: 28
	[Tutorial("ArmyCohesionStep1")]
	public class ArmyCohesionStep1Tutorial : TutorialItemBase
	{
		// Token: 0x06000087 RID: 135 RVA: 0x00002FD0 File Offset: 0x000011D0
		public ArmyCohesionStep1Tutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "ArmyOverlayArmyManagementButton";
			base.MouseRequired = true;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00002FF1 File Offset: 0x000011F1
		public override bool IsConditionsMetForCompletion()
		{
			return this._playerArmyNeedsCohesion && this._playerOpenedArmyManagement;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003003 File Offset: 0x00001203
		public override void OnTutorialContextChanged(TutorialContextChangedEvent obj)
		{
			this._playerOpenedArmyManagement = this._playerArmyNeedsCohesion && obj.NewContext == TutorialContexts.ArmyManagement;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003020 File Offset: 0x00001220
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003024 File Offset: 0x00001224
		public override bool IsConditionsMetForActivation()
		{
			bool playerArmyNeedsCohesion = this._playerArmyNeedsCohesion;
			Army army = MobileParty.MainParty.Army;
			float? num = ((army != null) ? new float?(army.Cohesion) : null);
			float maxCohesionForCohesionTutorial = TutorialHelper.MaxCohesionForCohesionTutorial;
			this._playerArmyNeedsCohesion = playerArmyNeedsCohesion | ((num.GetValueOrDefault() < maxCohesionForCohesionTutorial) & (num != null));
			return TutorialHelper.CurrentContext == TutorialContexts.MapWindow && MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty && MobileParty.MainParty.Army.Cohesion < TutorialHelper.MaxCohesionForCohesionTutorial;
		}

		// Token: 0x04000023 RID: 35
		private bool _playerOpenedArmyManagement;

		// Token: 0x04000024 RID: 36
		private bool _playerArmyNeedsCohesion;
	}
}
