using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x02000497 RID: 1175
	public static class ChangeClanInfluenceAction
	{
		// Token: 0x06004A1C RID: 18972 RVA: 0x0017711B File Offset: 0x0017531B
		private static void ApplyInternal(Clan clan, float amount)
		{
			clan.Influence += amount;
			CampaignEventDispatcher.Instance.OnClanInfluenceChanged(clan, amount);
		}

		// Token: 0x06004A1D RID: 18973 RVA: 0x00177137 File Offset: 0x00175337
		public static void Apply(Clan clan, float amount)
		{
			ChangeClanInfluenceAction.ApplyInternal(clan, amount);
		}
	}
}
