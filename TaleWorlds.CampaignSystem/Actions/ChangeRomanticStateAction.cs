using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004A1 RID: 1185
	public static class ChangeRomanticStateAction
	{
		// Token: 0x06004A49 RID: 19017 RVA: 0x0017833C File Offset: 0x0017653C
		private static void ApplyInternal(Hero hero1, Hero hero2, Romance.RomanceLevelEnum toWhat)
		{
			Romance.SetRomanticState(hero1, hero2, toWhat);
			CampaignEventDispatcher.Instance.OnRomanticStateChanged(hero1, hero2, toWhat);
		}

		// Token: 0x06004A4A RID: 19018 RVA: 0x00178353 File Offset: 0x00176553
		public static void Apply(Hero person1, Hero person2, Romance.RomanceLevelEnum toWhat)
		{
			ChangeRomanticStateAction.ApplyInternal(person1, person2, toWhat);
		}
	}
}
