using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using Storymode.Missions;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200002C RID: 44
	[Tutorial("StealthDarkZoneTutorial")]
	public class StealthDarkZoneTutorial : TutorialItemBase
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x0000387B File Offset: 0x00001A7B
		public StealthDarkZoneTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000388A File Offset: 0x00001A8A
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000388D File Offset: 0x00001A8D
		public override bool IsConditionsMetForActivation()
		{
			return SneakIntoTheVillaMissionController.IsStealthTutorialReadyForActivation(SneakIntoTheVillaMissionController.MissionState.DarkZone);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00003895 File Offset: 0x00001A95
		public override bool IsConditionsMetForCompletion()
		{
			return SneakIntoTheVillaMissionController.IsStealthTutorialReadyForCompletion(SneakIntoTheVillaMissionController.MissionState.DarkZone);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000389D File Offset: 0x00001A9D
		public override bool IsConditionsMetForVisibility()
		{
			return base.IsConditionsMetForVisibility() && SneakIntoTheVillaMissionController.Instance != null;
		}
	}
}
