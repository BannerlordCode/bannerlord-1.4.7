using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004B0 RID: 1200
	public static class EndMercenaryServiceAction
	{
		// Token: 0x06004A8D RID: 19085 RVA: 0x001791C2 File Offset: 0x001773C2
		private static void Apply(Clan clan, EndMercenaryServiceAction.EndMercenaryServiceActionDetails details)
		{
			clan.EndMercenaryService(details == EndMercenaryServiceAction.EndMercenaryServiceActionDetails.ApplyByLeavingKingdom);
			CampaignEventDispatcher.Instance.OnMercenaryServiceEnded(clan, details);
		}

		// Token: 0x06004A8E RID: 19086 RVA: 0x001791DA File Offset: 0x001773DA
		public static void EndByDefault(Clan clan)
		{
			EndMercenaryServiceAction.Apply(clan, EndMercenaryServiceAction.EndMercenaryServiceActionDetails.ApplyByDefault);
		}

		// Token: 0x06004A8F RID: 19087 RVA: 0x001791E3 File Offset: 0x001773E3
		public static void EndByLeavingKingdom(Clan clan)
		{
			EndMercenaryServiceAction.Apply(clan, EndMercenaryServiceAction.EndMercenaryServiceActionDetails.ApplyByLeavingKingdom);
		}

		// Token: 0x06004A90 RID: 19088 RVA: 0x001791EC File Offset: 0x001773EC
		public static void EndByBecomingVassal(Clan clan)
		{
			EndMercenaryServiceAction.Apply(clan, EndMercenaryServiceAction.EndMercenaryServiceActionDetails.ApplyByBecomingVassal);
		}

		// Token: 0x02000896 RID: 2198
		public enum EndMercenaryServiceActionDetails
		{
			// Token: 0x040024B1 RID: 9393
			ApplyByDefault,
			// Token: 0x040024B2 RID: 9394
			ApplyByLeavingKingdom,
			// Token: 0x040024B3 RID: 9395
			ApplyByBecomingVassal
		}
	}
}
