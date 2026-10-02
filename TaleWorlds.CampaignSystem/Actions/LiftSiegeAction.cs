using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004BB RID: 1211
	public static class LiftSiegeAction
	{
		// Token: 0x06004AC6 RID: 19142 RVA: 0x0017A7B7 File Offset: 0x001789B7
		private static void ApplyInternal(MobileParty side1Party, Settlement settlement)
		{
			settlement.SiegeEvent.BesiegerCamp.RemoveAllSiegeParties();
		}

		// Token: 0x06004AC7 RID: 19143 RVA: 0x0017A7C9 File Offset: 0x001789C9
		public static void GetGameAction(MobileParty side1Party)
		{
			LiftSiegeAction.ApplyInternal(side1Party, side1Party.BesiegedSettlement);
		}
	}
}
