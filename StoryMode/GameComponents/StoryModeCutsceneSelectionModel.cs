using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.Core;

namespace StoryMode.GameComponents
{
	// Token: 0x02000040 RID: 64
	public class StoryModeCutsceneSelectionModel : CutsceneSelectionModel
	{
		// Token: 0x0600043E RID: 1086 RVA: 0x00018F62 File Offset: 0x00017162
		public override SceneNotificationData GetKingdomDestroyedSceneNotification(Kingdom kingdom)
		{
			if (StoryModeManager.Current.MainStoryLine.PlayerSupportedKingdom == kingdom)
			{
				return new SupportedFactionDefeatedSceneNotificationItem(kingdom, StoryModeManager.Current.MainStoryLine.IsOnImperialQuestLine);
			}
			return base.BaseModel.GetKingdomDestroyedSceneNotification(kingdom);
		}
	}
}
