using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000025 RID: 37
	[Tutorial("NavigateOnMapTutorialStep2")]
	public class NavigateOnMapTutorialStep2 : TutorialItemBase
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x00003614 File Offset: 0x00001814
		public NavigateOnMapTutorialStep2()
		{
			base.Placement = TutorialItemVM.ItemPlacements.TopRight;
			base.HighlightedVisualElementID = "village_ES3_2";
			base.MouseRequired = true;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00003635 File Offset: 0x00001835
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00003638 File Offset: 0x00001838
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.CurrentContext == TutorialContexts.MapWindow;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00003642 File Offset: 0x00001842
		public override bool IsConditionsMetForCompletion()
		{
			MobileParty mainParty = MobileParty.MainParty;
			string text;
			if (mainParty == null)
			{
				text = null;
			}
			else
			{
				Settlement targetSettlement = mainParty.TargetSettlement;
				text = ((targetSettlement != null) ? targetSettlement.StringId : null);
			}
			return text == "village_ES3_2";
		}

		// Token: 0x04000034 RID: 52
		private const string TargetQuestVillage = "village_ES3_2";
	}
}
