using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001F6 RID: 502
	public abstract class CutsceneSelectionModel : MBGameModel<CutsceneSelectionModel>
	{
		// Token: 0x06001F62 RID: 8034
		public abstract SceneNotificationData GetKingdomDestroyedSceneNotification(Kingdom kingdom);
	}
}
