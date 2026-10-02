using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004A2 RID: 1186
	public class ChangeRulingClanAction
	{
		// Token: 0x06004A4B RID: 19019 RVA: 0x00178360 File Offset: 0x00176560
		private static void ApplyInternal(Kingdom kingdom, Clan newRulerClan)
		{
			Clan rulingClan = kingdom.RulingClan;
			kingdom.RulingClan = newRulerClan;
			CampaignEventDispatcher.Instance.OnRulingClanChanged(kingdom, rulingClan);
		}

		// Token: 0x06004A4C RID: 19020 RVA: 0x00178387 File Offset: 0x00176587
		public static void Apply(Kingdom kingdom, Clan clan)
		{
			ChangeRulingClanAction.ApplyInternal(kingdom, clan);
		}
	}
}
