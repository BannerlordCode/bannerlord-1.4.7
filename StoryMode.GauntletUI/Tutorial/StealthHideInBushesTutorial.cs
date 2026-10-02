using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using Storymode.Missions;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x0200002A RID: 42
	[Tutorial("StealthHideInBushesTutorial")]
	public class StealthHideInBushesTutorial : TutorialItemBase
	{
		// Token: 0x060000CF RID: 207 RVA: 0x000037F8 File Offset: 0x000019F8
		public StealthHideInBushesTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00003807 File Offset: 0x00001A07
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000380A File Offset: 0x00001A0A
		public override bool IsConditionsMetForActivation()
		{
			return SneakIntoTheVillaMissionController.IsStealthTutorialReadyForActivation(SneakIntoTheVillaMissionController.MissionState.HideInBushes);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00003812 File Offset: 0x00001A12
		public override bool IsConditionsMetForCompletion()
		{
			return SneakIntoTheVillaMissionController.IsStealthTutorialReadyForCompletion(SneakIntoTheVillaMissionController.MissionState.HideInBushes);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000381A File Offset: 0x00001A1A
		public override bool IsConditionsMetForVisibility()
		{
			return base.IsConditionsMetForVisibility() && SneakIntoTheVillaMissionController.Instance != null;
		}
	}
}
