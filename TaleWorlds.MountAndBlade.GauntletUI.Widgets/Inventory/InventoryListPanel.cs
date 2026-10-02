using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x02000142 RID: 322
	public class InventoryListPanel : NavigatableListPanel
	{
		// Token: 0x060010D8 RID: 4312 RVA: 0x0002E354 File Offset: 0x0002C554
		public InventoryListPanel(UIContext context)
			: base(context)
		{
			this._sortByTypeClickHandler = new Action<Widget>(this.OnSortByType);
			this._sortByNameClickHandler = new Action<Widget>(this.OnSortByName);
			this._sortByQuantityClickHandler = new Action<Widget>(this.OnSortByQuantity);
			this._sortByCostClickHandler = new Action<Widget>(this.OnSortByCost);
			base.ClearSelectedOnRemoval = true;
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x0002E3B7 File Offset: 0x0002C5B7
		private void OnSortByType(Widget widget)
		{
			base.RefreshChildNavigationIndices();
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x0002E3BF File Offset: 0x0002C5BF
		private void OnSortByName(Widget widget)
		{
			base.RefreshChildNavigationIndices();
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x0002E3C7 File Offset: 0x0002C5C7
		private void OnSortByQuantity(Widget widget)
		{
			base.RefreshChildNavigationIndices();
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x0002E3CF File Offset: 0x0002C5CF
		private void OnSortByCost(Widget widget)
		{
			base.RefreshChildNavigationIndices();
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x060010DD RID: 4317 RVA: 0x0002E3D7 File Offset: 0x0002C5D7
		// (set) Token: 0x060010DE RID: 4318 RVA: 0x0002E3E0 File Offset: 0x0002C5E0
		[Editor(false)]
		public ButtonWidget SortByTypeBtn
		{
			get
			{
				return this._sortByTypeBtn;
			}
			set
			{
				if (this._sortByTypeBtn != value)
				{
					if (this._sortByTypeBtn != null)
					{
						this._sortByTypeBtn.ClickEventHandlers.Remove(this._sortByTypeClickHandler);
					}
					this._sortByTypeBtn = value;
					if (this._sortByTypeBtn != null)
					{
						this._sortByTypeBtn.ClickEventHandlers.Add(this._sortByTypeClickHandler);
					}
					base.OnPropertyChanged<ButtonWidget>(value, "SortByTypeBtn");
				}
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x060010DF RID: 4319 RVA: 0x0002E446 File Offset: 0x0002C646
		// (set) Token: 0x060010E0 RID: 4320 RVA: 0x0002E450 File Offset: 0x0002C650
		[Editor(false)]
		public ButtonWidget SortByNameBtn
		{
			get
			{
				return this._sortByNameBtn;
			}
			set
			{
				if (this._sortByNameBtn != value)
				{
					if (this._sortByNameBtn != null)
					{
						this._sortByNameBtn.ClickEventHandlers.Remove(this._sortByNameClickHandler);
					}
					this._sortByNameBtn = value;
					if (this._sortByNameBtn != null)
					{
						this._sortByNameBtn.ClickEventHandlers.Add(this._sortByNameClickHandler);
					}
					base.OnPropertyChanged<ButtonWidget>(value, "SortByNameBtn");
				}
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x060010E1 RID: 4321 RVA: 0x0002E4B6 File Offset: 0x0002C6B6
		// (set) Token: 0x060010E2 RID: 4322 RVA: 0x0002E4C0 File Offset: 0x0002C6C0
		[Editor(false)]
		public ButtonWidget SortByQuantityBtn
		{
			get
			{
				return this._sortByQuantityBtn;
			}
			set
			{
				if (this._sortByQuantityBtn != value)
				{
					if (this._sortByQuantityBtn != null)
					{
						this._sortByQuantityBtn.ClickEventHandlers.Remove(this._sortByQuantityClickHandler);
					}
					this._sortByQuantityBtn = value;
					if (this._sortByQuantityBtn != null)
					{
						this._sortByQuantityBtn.ClickEventHandlers.Add(this._sortByQuantityClickHandler);
					}
					base.OnPropertyChanged<ButtonWidget>(value, "SortByQuantityBtn");
				}
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x060010E3 RID: 4323 RVA: 0x0002E526 File Offset: 0x0002C726
		// (set) Token: 0x060010E4 RID: 4324 RVA: 0x0002E530 File Offset: 0x0002C730
		[Editor(false)]
		public ButtonWidget SortByCostBtn
		{
			get
			{
				return this._sortByCostBtn;
			}
			set
			{
				if (this._sortByCostBtn != value)
				{
					if (this._sortByCostBtn != null)
					{
						this._sortByCostBtn.ClickEventHandlers.Remove(this._sortByCostClickHandler);
					}
					this._sortByCostBtn = value;
					if (this._sortByCostBtn != null)
					{
						this._sortByCostBtn.ClickEventHandlers.Remove(this._sortByCostClickHandler);
					}
					base.OnPropertyChanged<ButtonWidget>(value, "SortByCostBtn");
				}
			}
		}

		// Token: 0x04000798 RID: 1944
		private Action<Widget> _sortByTypeClickHandler;

		// Token: 0x04000799 RID: 1945
		private Action<Widget> _sortByNameClickHandler;

		// Token: 0x0400079A RID: 1946
		private Action<Widget> _sortByQuantityClickHandler;

		// Token: 0x0400079B RID: 1947
		private Action<Widget> _sortByCostClickHandler;

		// Token: 0x0400079C RID: 1948
		private ButtonWidget _sortByTypeBtn;

		// Token: 0x0400079D RID: 1949
		private ButtonWidget _sortByNameBtn;

		// Token: 0x0400079E RID: 1950
		private ButtonWidget _sortByQuantityBtn;

		// Token: 0x0400079F RID: 1951
		private ButtonWidget _sortByCostBtn;
	}
}
