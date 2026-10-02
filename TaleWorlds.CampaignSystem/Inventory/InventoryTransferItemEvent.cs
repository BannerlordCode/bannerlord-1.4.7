using System;
using TaleWorlds.Core;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.Inventory
{
	// Token: 0x020000DB RID: 219
	public class InventoryTransferItemEvent : EventBase
	{
		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001507 RID: 5383 RVA: 0x00060F59 File Offset: 0x0005F159
		// (set) Token: 0x06001508 RID: 5384 RVA: 0x00060F61 File Offset: 0x0005F161
		public ItemObject Item { get; private set; }

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001509 RID: 5385 RVA: 0x00060F6A File Offset: 0x0005F16A
		// (set) Token: 0x0600150A RID: 5386 RVA: 0x00060F72 File Offset: 0x0005F172
		public bool IsBuyForPlayer { get; private set; }

		// Token: 0x0600150B RID: 5387 RVA: 0x00060F7B File Offset: 0x0005F17B
		public InventoryTransferItemEvent(ItemObject item, bool isBuyForPlayer)
		{
			this.Item = item;
			this.IsBuyForPlayer = isBuyForPlayer;
		}
	}
}
