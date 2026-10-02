using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000023 RID: 35
	[Tutorial("EnterVillageTutorial")]
	public class EnterVillageTutorial : TutorialItemBase
	{
		// Token: 0x060000AD RID: 173 RVA: 0x0000349E File Offset: 0x0000169E
		public EnterVillageTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "storymode_tutorial_village_enter";
			base.MouseRequired = true;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000034BF File Offset: 0x000016BF
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000034C2 File Offset: 0x000016C2
		public override bool IsConditionsMetForActivation()
		{
			if (!TutorialHelper.IsCharacterPopUpWindowOpen && TutorialHelper.CurrentContext == TutorialContexts.MapWindow)
			{
				Settlement currentSettlement = Settlement.CurrentSettlement;
				return ((currentSettlement != null) ? currentSettlement.StringId : null) == "village_ES3_2";
			}
			return false;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000034F0 File Offset: 0x000016F0
		public override void OnGameMenuOptionSelected(GameMenuOption obj)
		{
			base.OnGameMenuOptionSelected(obj);
			this._isEnterOptionSelected = obj.IdString == "storymode_tutorial_village_enter";
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000350F File Offset: 0x0000170F
		public override bool IsConditionsMetForCompletion()
		{
			return this._isEnterOptionSelected;
		}

		// Token: 0x0400002E RID: 46
		private bool _isEnterOptionSelected;

		// Token: 0x0400002F RID: 47
		private const string _enterGameMenuOptionId = "storymode_tutorial_village_enter";
	}
}
