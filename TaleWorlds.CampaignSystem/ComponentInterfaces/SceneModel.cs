using System;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001FE RID: 510
	public abstract class SceneModel : MBGameModel<SceneModel>
	{
		// Token: 0x06001F9F RID: 8095
		public abstract string GetConversationSceneForMapPosition(CampaignVec2 campaignPosition);

		// Token: 0x06001FA0 RID: 8096
		public abstract string GetBattleSceneForMapPatch(MapPatchData mapPatch, bool isNavalEncounter);
	}
}
