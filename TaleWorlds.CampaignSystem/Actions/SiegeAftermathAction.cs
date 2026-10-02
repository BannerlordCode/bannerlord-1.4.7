using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004C8 RID: 1224
	public static class SiegeAftermathAction
	{
		// Token: 0x06004AF5 RID: 19189 RVA: 0x0017BC64 File Offset: 0x00179E64
		private static void ApplyInternal(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType, Clan previousSettlementOwner, Dictionary<MobileParty, float> partyContributions)
		{
			CampaignEventDispatcher.Instance.OnSiegeAftermathApplied(attackerParty, settlement, aftermathType, previousSettlementOwner, partyContributions);
		}

		// Token: 0x06004AF6 RID: 19190 RVA: 0x0017BC76 File Offset: 0x00179E76
		public static void ApplyAftermath(MobileParty attackerParty, Settlement settlement, SiegeAftermathAction.SiegeAftermath aftermathType, Clan previousSettlementOwner, Dictionary<MobileParty, float> partyContributions)
		{
			SiegeAftermathAction.ApplyInternal(attackerParty, settlement, aftermathType, previousSettlementOwner, partyContributions);
		}

		// Token: 0x020008A3 RID: 2211
		public enum SiegeAftermath
		{
			// Token: 0x040024F1 RID: 9457
			Devastate,
			// Token: 0x040024F2 RID: 9458
			Pillage,
			// Token: 0x040024F3 RID: 9459
			ShowMercy
		}
	}
}
