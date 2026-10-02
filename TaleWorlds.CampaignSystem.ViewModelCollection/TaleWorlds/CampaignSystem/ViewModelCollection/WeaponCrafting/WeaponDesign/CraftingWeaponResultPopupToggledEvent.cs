using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x0200010B RID: 267
	public class CraftingWeaponResultPopupToggledEvent : EventBase
	{
		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x060017F0 RID: 6128 RVA: 0x0005B6F1 File Offset: 0x000598F1
		public bool IsOpen { get; }

		// Token: 0x060017F1 RID: 6129 RVA: 0x0005B6F9 File Offset: 0x000598F9
		public CraftingWeaponResultPopupToggledEvent(bool isOpen)
		{
			this.IsOpen = isOpen;
		}
	}
}
