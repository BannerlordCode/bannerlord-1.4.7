using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000490 RID: 1168
	public static class AddCompanionAction
	{
		// Token: 0x06004A07 RID: 18951 RVA: 0x00176455 File Offset: 0x00174655
		private static void ApplyInternal(Clan clan, Hero companion)
		{
			if (companion.CompanionOf != null)
			{
				RemoveCompanionAction.ApplyByFire(companion.CompanionOf, companion);
			}
			companion.CompanionOf = clan;
			CampaignEventDispatcher.Instance.OnNewCompanionAdded(companion);
		}

		// Token: 0x06004A08 RID: 18952 RVA: 0x0017647D File Offset: 0x0017467D
		public static void Apply(Clan clan, Hero companion)
		{
			AddCompanionAction.ApplyInternal(clan, companion);
		}
	}
}
