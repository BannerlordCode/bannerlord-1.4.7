using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001E7 RID: 487
	public abstract class SiegeLordsHallFightModel : MBGameModel<SiegeLordsHallFightModel>
	{
		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06001EFE RID: 7934
		public abstract float AreaLostRatio { get; }

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06001EFF RID: 7935
		public abstract float AttackerDefenderTroopCountRatio { get; }

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06001F00 RID: 7936
		public abstract int DefenderTroopNumberForSuccessfulPullBack { get; }

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06001F01 RID: 7937
		public abstract float DefenderMaxArcherRatio { get; }

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06001F02 RID: 7938
		public abstract int MaxDefenderSideTroopCount { get; }

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06001F03 RID: 7939
		public abstract int MaxDefenderArcherCount { get; }

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06001F04 RID: 7940
		public abstract int MaxAttackerSideTroopCount { get; }

		// Token: 0x06001F05 RID: 7941
		public abstract FlattenedTroopRoster GetPriorityListForLordsHallFightMission(MapEvent playerMapEvent, BattleSideEnum side, int troopCount);
	}
}
