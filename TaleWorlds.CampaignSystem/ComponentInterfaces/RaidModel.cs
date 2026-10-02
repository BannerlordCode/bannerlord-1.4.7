using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000199 RID: 409
	public abstract class RaidModel : MBGameModel<RaidModel>
	{
		// Token: 0x06001C74 RID: 7284
		public abstract MBReadOnlyList<ValueTuple<ItemObject, float>> GetCommonLootItemScores();

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06001C75 RID: 7285
		public abstract int GoldRewardForEachLostHearth { get; }

		// Token: 0x06001C76 RID: 7286
		public abstract ExplainedNumber CalculateHitDamage(MapEventSide attackerSide, float settlementHitPoints);

		// Token: 0x06001C77 RID: 7287
		public abstract ExplainedNumber GetRaidLootMultiplier(PartyBase receivingParty);
	}
}
