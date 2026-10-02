using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C3 RID: 451
	public abstract class SettlementMilitiaModel : MBGameModel<SettlementMilitiaModel>
	{
		// Token: 0x06001DF2 RID: 7666
		public abstract int MilitiaToSpawnAfterSiege(Town town);

		// Token: 0x06001DF3 RID: 7667
		public abstract ExplainedNumber CalculateMilitiaChange(Settlement settlement, bool includeDescriptions = false);

		// Token: 0x06001DF4 RID: 7668
		public abstract ExplainedNumber CalculateVeteranMilitiaSpawnChance(Settlement settlement);

		// Token: 0x06001DF5 RID: 7669
		public abstract void CalculateMilitiaSpawnRate(Settlement settlement, out float meleeTroopRate, out float rangedTroopRate);
	}
}
