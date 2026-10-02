using System;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting
{
	// Token: 0x020000FD RID: 253
	public class CraftingSecondaryUsageItemVM : SelectorItemVM
	{
		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x060016C8 RID: 5832 RVA: 0x00058466 File Offset: 0x00056666
		public int UsageIndex { get; }

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x060016C9 RID: 5833 RVA: 0x0005846E File Offset: 0x0005666E
		public int SelectorIndex { get; }

		// Token: 0x060016CA RID: 5834 RVA: 0x00058476 File Offset: 0x00056676
		public CraftingSecondaryUsageItemVM(TextObject name, int index, int usageIndex, SelectorVM<CraftingSecondaryUsageItemVM> parentSelector)
			: base(name)
		{
			this._parentSelector = parentSelector;
			this.SelectorIndex = index;
			this.UsageIndex = usageIndex;
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x00058495 File Offset: 0x00056695
		public void ExecuteSelect()
		{
			this._parentSelector.SelectedIndex = this.SelectorIndex;
		}

		// Token: 0x04000A6D RID: 2669
		private SelectorVM<CraftingSecondaryUsageItemVM> _parentSelector;
	}
}
