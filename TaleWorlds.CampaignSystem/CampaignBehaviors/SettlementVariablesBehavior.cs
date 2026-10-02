using System;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000441 RID: 1089
	public class SettlementVariablesBehavior : CampaignBehaviorBase
	{
		// Token: 0x060045FB RID: 17915 RVA: 0x0015C230 File Offset: 0x0015A430
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.HourlyTickSettlement));
		}

		// Token: 0x060045FC RID: 17916 RVA: 0x0015C249 File Offset: 0x0015A449
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x060045FD RID: 17917 RVA: 0x0015C24C File Offset: 0x0015A44C
		private void HourlyTickSettlement(Settlement settlement)
		{
			if (settlement.LastAttackerParty != null && settlement.Party.MapEvent == null && settlement.Party.SiegeEvent == null && settlement.LastThreatTime.ElapsedDaysUntilNow > this._resetLastAttackerPartyAsDays)
			{
				settlement.LastAttackerParty = null;
			}
		}

		// Token: 0x040013A9 RID: 5033
		private float _resetLastAttackerPartyAsDays = 1f;
	}
}
