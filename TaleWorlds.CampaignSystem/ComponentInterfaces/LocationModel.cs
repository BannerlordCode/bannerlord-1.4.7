using System;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200018A RID: 394
	public abstract class LocationModel : MBGameModel<LocationModel>
	{
		// Token: 0x06001C1B RID: 7195
		public abstract int GetSettlementUpgradeLevel(LocationEncounter locationEncounter);

		// Token: 0x06001C1C RID: 7196
		public abstract string GetCivilianSceneLevel(Settlement settlement);

		// Token: 0x06001C1D RID: 7197
		public abstract string GetCivilianUpgradeLevelTag(int upgradeLevel);

		// Token: 0x06001C1E RID: 7198
		public abstract string GetUpgradeLevelTag(int upgradeLevel);
	}
}
