using System;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x02000097 RID: 151
	public class InventoryItemInspectedEvent : EventBase
	{
		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x0003D719 File Offset: 0x0003B919
		// (set) Token: 0x06000EA3 RID: 3747 RVA: 0x0003D721 File Offset: 0x0003B921
		public ItemRosterElement Item { get; private set; }

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x0003D72A File Offset: 0x0003B92A
		// (set) Token: 0x06000EA5 RID: 3749 RVA: 0x0003D732 File Offset: 0x0003B932
		public InventoryLogic.InventorySide ItemSide { get; private set; }

		// Token: 0x06000EA6 RID: 3750 RVA: 0x0003D73B File Offset: 0x0003B93B
		public InventoryItemInspectedEvent(ItemRosterElement item, InventoryLogic.InventorySide itemSide)
		{
			this.ItemSide = itemSide;
			this.Item = item;
		}
	}
}
