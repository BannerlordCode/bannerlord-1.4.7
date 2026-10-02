using System;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004BE RID: 1214
	public static class MakePregnantAction
	{
		// Token: 0x06004ACD RID: 19149 RVA: 0x0017A9B4 File Offset: 0x00178BB4
		private static void ApplyInternal(Hero mother)
		{
			mother.IsPregnant = true;
			CampaignEventDispatcher.Instance.OnChildConceived(mother);
		}

		// Token: 0x06004ACE RID: 19150 RVA: 0x0017A9C8 File Offset: 0x00178BC8
		public static void Apply(Hero mother)
		{
			MakePregnantAction.ApplyInternal(mother);
		}
	}
}
