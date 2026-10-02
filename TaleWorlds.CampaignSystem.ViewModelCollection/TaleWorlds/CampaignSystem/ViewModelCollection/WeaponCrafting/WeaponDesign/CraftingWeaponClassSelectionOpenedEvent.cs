using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x02000106 RID: 262
	public class CraftingWeaponClassSelectionOpenedEvent : EventBase
	{
		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x060017A4 RID: 6052 RVA: 0x0005ACE2 File Offset: 0x00058EE2
		// (set) Token: 0x060017A5 RID: 6053 RVA: 0x0005ACEA File Offset: 0x00058EEA
		public bool IsOpen { get; private set; }

		// Token: 0x060017A6 RID: 6054 RVA: 0x0005ACF3 File Offset: 0x00058EF3
		public CraftingWeaponClassSelectionOpenedEvent(bool isOpen)
		{
			this.IsOpen = isOpen;
		}
	}
}
