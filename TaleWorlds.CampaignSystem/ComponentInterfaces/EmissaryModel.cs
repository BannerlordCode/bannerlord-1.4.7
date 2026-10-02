using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A9 RID: 425
	public abstract class EmissaryModel : MBGameModel<EmissaryModel>
	{
		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06001D20 RID: 7456
		public abstract int EmissaryRelationBonusForMainClan { get; }

		// Token: 0x06001D21 RID: 7457
		public abstract bool IsEmissary(Hero hero);
	}
}
