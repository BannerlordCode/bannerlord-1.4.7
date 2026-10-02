using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Inventory
{
	// Token: 0x02000095 RID: 149
	public class InventoryEquipmentTypeChangedEvent : EventBase
	{
		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06000E9C RID: 3740 RVA: 0x0003D6D9 File Offset: 0x0003B8D9
		// (set) Token: 0x06000E9D RID: 3741 RVA: 0x0003D6E1 File Offset: 0x0003B8E1
		public bool IsCurrentlyWarSet { get; private set; }

		// Token: 0x06000E9E RID: 3742 RVA: 0x0003D6EA File Offset: 0x0003B8EA
		public InventoryEquipmentTypeChangedEvent(bool isCurrentlyWarSet)
		{
			this.IsCurrentlyWarSet = isCurrentlyWarSet;
		}
	}
}
