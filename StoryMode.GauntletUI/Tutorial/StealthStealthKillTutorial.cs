using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using Storymode.Missions;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200002D RID: 45
	[Tutorial("StealthStealthKillTutorial")]
	public class StealthStealthKillTutorial : TutorialItemBase
	{
		// Token: 0x060000DE RID: 222 RVA: 0x000038B1 File Offset: 0x00001AB1
		public StealthStealthKillTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000038C0 File Offset: 0x00001AC0
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000038C3 File Offset: 0x00001AC3
		public override bool IsConditionsMetForActivation()
		{
			return SneakIntoTheVillaMissionController.IsStealthTutorialReadyForActivation(SneakIntoTheVillaMissionController.MissionState.StealthKill);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000038CB File Offset: 0x00001ACB
		public override bool IsConditionsMetForCompletion()
		{
			return SneakIntoTheVillaMissionController.IsStealthTutorialReadyForCompletion(SneakIntoTheVillaMissionController.MissionState.StealthKill) || (SneakIntoTheVillaMissionController.Instance != null && SneakIntoTheVillaMissionController.Instance.IsTargetAgentKilled());
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000038EA File Offset: 0x00001AEA
		public override bool IsConditionsMetForVisibility()
		{
			return base.IsConditionsMetForVisibility() && SneakIntoTheVillaMissionController.Instance != null;
		}
	}
}
