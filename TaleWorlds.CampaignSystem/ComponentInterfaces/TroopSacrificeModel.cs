using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001DE RID: 478
	public abstract class TroopSacrificeModel : MBGameModel<TroopSacrificeModel>
	{
		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06001EB3 RID: 7859
		public abstract int BreakOutArmyLeaderRelationPenalty { get; }

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06001EB4 RID: 7860
		public abstract int BreakOutArmyMemberRelationPenalty { get; }

		// Token: 0x06001EB5 RID: 7861
		public abstract ExplainedNumber GetLostTroopCountForBreakingInBesiegedSettlement(MobileParty party, SiegeEvent siegeEvent);

		// Token: 0x06001EB6 RID: 7862
		public abstract ExplainedNumber GetLostTroopCountForBreakingOutOfBesiegedSettlement(MobileParty party, SiegeEvent siegeEvent, bool isBreakingOutFromPort);

		// Token: 0x06001EB7 RID: 7863
		public abstract int GetNumberOfTroopsSacrificedForTryingToGetAway(BattleSideEnum playerBattleSide, MapEvent mapEvent);

		// Token: 0x06001EB8 RID: 7864
		public abstract void GetShipsToSacrificeForTryingToGetAway(BattleSideEnum playerBattleSide, MapEvent mapEvent, out MBList<Ship> shipsToCapture, out Ship shipToTakeDamage, out float damageToApplyForLastShip);

		// Token: 0x06001EB9 RID: 7865
		public abstract bool CanPlayerGetAwayFromEncounter(out TextObject explanation);
	}
}
