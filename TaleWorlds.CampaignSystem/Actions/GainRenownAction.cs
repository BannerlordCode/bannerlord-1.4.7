using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004B3 RID: 1203
	public static class GainRenownAction
	{
		// Token: 0x06004AA2 RID: 19106 RVA: 0x001796F1 File Offset: 0x001778F1
		private static void ApplyInternal(Hero hero, float gainedRenown, bool doNotNotify)
		{
			if (gainedRenown > 0f)
			{
				hero.Clan.AddRenown(gainedRenown, true);
				CampaignEventDispatcher.Instance.OnRenownGained(hero, (int)gainedRenown, doNotNotify);
			}
		}

		// Token: 0x06004AA3 RID: 19107 RVA: 0x00179716 File Offset: 0x00177916
		public static void Apply(Hero hero, float renownValue, bool doNotNotify = false)
		{
			GainRenownAction.ApplyInternal(hero, renownValue, doNotNotify);
		}
	}
}
