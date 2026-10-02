using System;
using TaleWorlds.Core.ViewModelCollection.Selector;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.ClanFinance
{
	// Token: 0x02000137 RID: 311
	public class WorkshopPercentageSelectorItemVM : SelectorItemVM
	{
		// Token: 0x06001D11 RID: 7441 RVA: 0x0006BAD5 File Offset: 0x00069CD5
		public WorkshopPercentageSelectorItemVM(string s, float percentage)
			: base(s)
		{
			this.Percentage = percentage;
		}

		// Token: 0x04000D8E RID: 3470
		public readonly float Percentage;
	}
}
