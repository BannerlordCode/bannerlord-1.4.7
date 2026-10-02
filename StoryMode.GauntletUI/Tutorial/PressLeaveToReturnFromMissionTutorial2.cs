using System;
using System.Linq;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000019 RID: 25
	[Tutorial("PressLeaveToReturnFromMissionType2")]
	public class PressLeaveToReturnFromMissionTutorial2 : TutorialItemBase
	{
		// Token: 0x06000078 RID: 120 RVA: 0x00002E21 File Offset: 0x00001021
		public PressLeaveToReturnFromMissionTutorial2()
		{
			base.Placement = TutorialItemVM.ItemPlacements.TopRight;
			base.HighlightedVisualElementID = string.Empty;
			base.MouseRequired = false;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002E42 File Offset: 0x00001042
		public override bool IsConditionsMetForCompletion()
		{
			return this._changedContext;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00002E4A File Offset: 0x0000104A
		public override void OnTutorialContextChanged(TutorialContextChangedEvent obj)
		{
			this._changedContext = true;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00002E53 File Offset: 0x00001053
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00002E58 File Offset: 0x00001058
		public override bool IsConditionsMetForActivation()
		{
			string[] array = new string[] { "center", "lordshall", "tavern", "prison", "village_center", "arena" };
			return TutorialHelper.CurrentMissionLocation != null && array.Contains(TutorialHelper.CurrentMissionLocation.StringId) && TutorialHelper.PlayerIsInAnySettlement && !TutorialHelper.PlayerIsInAConversation && TutorialHelper.CurrentContext == TutorialContexts.Mission;
		}

		// Token: 0x0400001F RID: 31
		private bool _changedContext;
	}
}
