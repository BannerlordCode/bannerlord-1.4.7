using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x0200013F RID: 319
	public class InventoryItemPreviewWidget : Widget
	{
		// Token: 0x06001092 RID: 4242 RVA: 0x0002D8C0 File Offset: 0x0002BAC0
		public InventoryItemPreviewWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001093 RID: 4243 RVA: 0x0002D8C9 File Offset: 0x0002BAC9
		// (set) Token: 0x06001094 RID: 4244 RVA: 0x0002D8D1 File Offset: 0x0002BAD1
		[Editor(false)]
		public bool IsPreviewOpen
		{
			get
			{
				return this._isPreviewOpen;
			}
			set
			{
				if (this._isPreviewOpen != value)
				{
					this._isPreviewOpen = value;
					base.IsVisible = value;
					base.OnPropertyChanged(value, "IsPreviewOpen");
				}
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001095 RID: 4245 RVA: 0x0002D8F6 File Offset: 0x0002BAF6
		// (set) Token: 0x06001096 RID: 4246 RVA: 0x0002D8FE File Offset: 0x0002BAFE
		[Editor(false)]
		public ItemTableauWidget ItemTableau
		{
			get
			{
				return this._itemTableau;
			}
			set
			{
				if (this._itemTableau != value)
				{
					this._itemTableau = value;
					base.OnPropertyChanged<ItemTableauWidget>(value, "ItemTableau");
				}
			}
		}

		// Token: 0x04000779 RID: 1913
		private ItemTableauWidget _itemTableau;

		// Token: 0x0400077A RID: 1914
		private bool _isPreviewOpen;
	}
}
