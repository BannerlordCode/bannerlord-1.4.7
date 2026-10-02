using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200010B RID: 267
	public class DefaultCutsceneSelectionModel : CutsceneSelectionModel
	{
		// Token: 0x06001762 RID: 5986 RVA: 0x0006E1C3 File Offset: 0x0006C3C3
		public override SceneNotificationData GetKingdomDestroyedSceneNotification(Kingdom kingdom)
		{
			return new KingdomDestroyedSceneNotificationItem(kingdom, CampaignTime.Now);
		}
	}
}
