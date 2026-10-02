using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200019C RID: 412
	public abstract class BribeCalculationModel : MBGameModel<BribeCalculationModel>
	{
		// Token: 0x06001C83 RID: 7299
		public abstract int GetBribeToEnterLordsHall(Settlement settlement);

		// Token: 0x06001C84 RID: 7300
		public abstract int GetBribeToEnterDungeon(Settlement settlement);

		// Token: 0x06001C85 RID: 7301
		public abstract bool IsBribeNotNeededToEnterKeep(Settlement settlement);

		// Token: 0x06001C86 RID: 7302
		public abstract bool IsBribeNotNeededToEnterDungeon(Settlement settlement);
	}
}
