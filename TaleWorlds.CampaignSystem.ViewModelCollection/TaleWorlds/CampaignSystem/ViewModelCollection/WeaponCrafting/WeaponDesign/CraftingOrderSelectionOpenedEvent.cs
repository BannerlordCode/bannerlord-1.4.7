using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x02000108 RID: 264
	public class CraftingOrderSelectionOpenedEvent : EventBase
	{
		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x060017AA RID: 6058 RVA: 0x0005AD22 File Offset: 0x00058F22
		// (set) Token: 0x060017AB RID: 6059 RVA: 0x0005AD2A File Offset: 0x00058F2A
		public bool IsOpen { get; private set; }

		// Token: 0x060017AC RID: 6060 RVA: 0x0005AD33 File Offset: 0x00058F33
		public CraftingOrderSelectionOpenedEvent(bool isOpen)
		{
			this.IsOpen = isOpen;
		}
	}
}
