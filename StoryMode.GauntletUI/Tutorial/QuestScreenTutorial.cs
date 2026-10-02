using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000026 RID: 38
	[Tutorial("GetQuestTutorial")]
	public class QuestScreenTutorial : TutorialItemBase
	{
		// Token: 0x060000BB RID: 187 RVA: 0x0000366B File Offset: 0x0000186B
		public QuestScreenTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = "QuestsButton";
			base.MouseRequired = true;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000368C File Offset: 0x0000188C
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000368F File Offset: 0x0000188F
		public override bool IsConditionsMetForActivation()
		{
			return Mission.Current == null && TutorialHelper.CurrentContext == TutorialContexts.QuestsScreen;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000036A3 File Offset: 0x000018A3
		public override bool IsConditionsMetForCompletion()
		{
			return this._contextChangedToQuestsScreen;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000036AB File Offset: 0x000018AB
		public override void OnTutorialContextChanged(TutorialContextChangedEvent obj)
		{
			this._contextChangedToQuestsScreen = obj.NewContext == TutorialContexts.QuestsScreen;
		}

		// Token: 0x04000035 RID: 53
		private bool _contextChangedToQuestsScreen;
	}
}
