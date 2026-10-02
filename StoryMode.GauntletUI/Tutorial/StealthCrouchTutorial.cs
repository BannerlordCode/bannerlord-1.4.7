using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using Storymode.Missions;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000028 RID: 40
	[Tutorial("StealthCrouchTutorial")]
	public class StealthCrouchTutorial : TutorialItemBase
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x0000375E File Offset: 0x0000195E
		public StealthCrouchTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000376D File Offset: 0x0000196D
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00003770 File Offset: 0x00001970
		public override bool IsConditionsMetForActivation()
		{
			return SneakIntoTheVillaMissionController.IsStealthTutorialReadyForActivation(SneakIntoTheVillaMissionController.MissionState.Crouch);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00003778 File Offset: 0x00001978
		public override bool IsConditionsMetForCompletion()
		{
			return Agent.Main != null && Agent.Main.CrouchMode && SneakIntoTheVillaMissionController.Instance != null;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00003797 File Offset: 0x00001997
		public override bool IsConditionsMetForVisibility()
		{
			return base.IsConditionsMetForVisibility() && SneakIntoTheVillaMissionController.Instance != null;
		}
	}
}
