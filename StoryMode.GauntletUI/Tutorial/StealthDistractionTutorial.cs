using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using Storymode.Missions;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200002B RID: 43
	[Tutorial("StealthDistractionTutorial")]
	public class StealthDistractionTutorial : TutorialItemBase
	{
		// Token: 0x060000D4 RID: 212 RVA: 0x0000382E File Offset: 0x00001A2E
		public StealthDistractionTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000383D File Offset: 0x00001A3D
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00003840 File Offset: 0x00001A40
		public override bool IsConditionsMetForActivation()
		{
			return SneakIntoTheVillaMissionController.IsStealthTutorialReadyForActivation(SneakIntoTheVillaMissionController.MissionState.Distraction);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00003848 File Offset: 0x00001A48
		public override bool IsConditionsMetForCompletion()
		{
			return SneakIntoTheVillaMissionController.IsStealthTutorialReadyForCompletion(SneakIntoTheVillaMissionController.MissionState.Distraction) || (SneakIntoTheVillaMissionController.Instance != null && SneakIntoTheVillaMissionController.Instance.IsTargetAgentDistracted());
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00003867 File Offset: 0x00001A67
		public override bool IsConditionsMetForVisibility()
		{
			return base.IsConditionsMetForVisibility() && SneakIntoTheVillaMissionController.Instance != null;
		}
	}
}
