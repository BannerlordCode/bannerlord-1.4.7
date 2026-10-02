using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004CA RID: 1226
	public static class StartMercenaryServiceAction
	{
		// Token: 0x06004AFD RID: 19197 RVA: 0x0017C090 File Offset: 0x0017A290
		private static void ApplyStart(Clan clan, Kingdom kingdom, int awardMultiplier, StartMercenaryServiceAction.StartMercenaryServiceActionDetails details)
		{
			if (clan.IsUnderMercenaryService)
			{
				EndMercenaryServiceAction.EndByLeavingKingdom(clan);
			}
			clan.MercenaryAwardMultiplier = awardMultiplier;
			clan.Kingdom = kingdom;
			clan.StartMercenaryService();
			if (clan == Clan.PlayerClan)
			{
				Campaign.Current.KingdomManager.PlayerMercenaryServiceNextRenewalDay = Campaign.CurrentTime + 30f * (float)CampaignTime.HoursInDay;
			}
			CampaignEventDispatcher.Instance.OnMercenaryServiceStarted(clan, details);
		}

		// Token: 0x06004AFE RID: 19198 RVA: 0x0017C0F4 File Offset: 0x0017A2F4
		public static void ApplyByDefault(Clan clan, Kingdom kingdom, int awardMultiplier)
		{
			StartMercenaryServiceAction.ApplyStart(clan, kingdom, awardMultiplier, StartMercenaryServiceAction.StartMercenaryServiceActionDetails.ApplyByDefault);
		}

		// Token: 0x020008A5 RID: 2213
		public enum StartMercenaryServiceActionDetails
		{
			// Token: 0x040024F7 RID: 9463
			ApplyByDefault
		}
	}
}
