using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x02000107 RID: 263
	public class CraftingOrderTabOpenedEvent : EventBase
	{
		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x060017A7 RID: 6055 RVA: 0x0005AD02 File Offset: 0x00058F02
		// (set) Token: 0x060017A8 RID: 6056 RVA: 0x0005AD0A File Offset: 0x00058F0A
		public bool IsOpen { get; private set; }

		// Token: 0x060017A9 RID: 6057 RVA: 0x0005AD13 File Offset: 0x00058F13
		public CraftingOrderTabOpenedEvent(bool isOpen)
		{
			this.IsOpen = isOpen;
		}
	}
}
