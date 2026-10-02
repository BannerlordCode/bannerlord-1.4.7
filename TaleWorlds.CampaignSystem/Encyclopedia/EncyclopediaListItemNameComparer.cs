using System;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x02000178 RID: 376
	internal class EncyclopediaListItemNameComparer : EncyclopediaListItemComparerBase
	{
		// Token: 0x06001B79 RID: 7033 RVA: 0x0008E09D File Offset: 0x0008C29D
		public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
		{
			return base.ResolveEquality(x, y);
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x0008E0A7 File Offset: 0x0008C2A7
		public override string GetComparedValueText(EncyclopediaListItem item)
		{
			return "";
		}
	}
}
