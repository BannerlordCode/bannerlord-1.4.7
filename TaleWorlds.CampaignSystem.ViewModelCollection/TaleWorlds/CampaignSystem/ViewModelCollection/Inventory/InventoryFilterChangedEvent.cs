using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x02000096 RID: 150
	public class InventoryFilterChangedEvent : EventBase
	{
		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06000E9F RID: 3743 RVA: 0x0003D6F9 File Offset: 0x0003B8F9
		// (set) Token: 0x06000EA0 RID: 3744 RVA: 0x0003D701 File Offset: 0x0003B901
		public SPInventoryVM.Filters NewFilter { get; private set; }

		// Token: 0x06000EA1 RID: 3745 RVA: 0x0003D70A File Offset: 0x0003B90A
		public InventoryFilterChangedEvent(SPInventoryVM.Filters newFilter)
		{
			this.NewFilter = newFilter;
		}
	}
}
