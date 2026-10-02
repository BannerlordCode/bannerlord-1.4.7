using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Party
{
	// Token: 0x0200002A RID: 42
	public class TroopSortSelectorItemVM : SelectorItemVM
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000349 RID: 841 RVA: 0x00016690 File Offset: 0x00014890
		// (set) Token: 0x0600034A RID: 842 RVA: 0x00016698 File Offset: 0x00014898
		public PartyScreenLogic.TroopSortType SortType { get; private set; }

		// Token: 0x0600034B RID: 843 RVA: 0x000166A1 File Offset: 0x000148A1
		public TroopSortSelectorItemVM(TextObject s, PartyScreenLogic.TroopSortType sortType)
			: base(s)
		{
			this.SortType = sortType;
		}
	}
}
