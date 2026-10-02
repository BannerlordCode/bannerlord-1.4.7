using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.GameOver
{
	// Token: 0x02000058 RID: 88
	public class GameOverStatCategoryVM : ViewModel
	{
		// Token: 0x06000583 RID: 1411 RVA: 0x00014BE0 File Offset: 0x00012DE0
		public GameOverStatCategoryVM(StatCategory category, Action<GameOverStatCategoryVM> onSelect)
		{
			this._category = category;
			this._onSelect = onSelect;
			this.Items = new MBBindingList<GameOverStatItemVM>();
			this.ID = category.ID;
			this.RefreshValues();
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00014C14 File Offset: 0x00012E14
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Items.Clear();
			this.Name = GameTexts.FindText("str_game_over_stat_category", this._category.ID).ToString();
			foreach (StatItem statItem in this._category.Items)
			{
				this.Items.Add(new GameOverStatItemVM(statItem));
			}
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00014CA4 File Offset: 0x00012EA4
		public void ExecuteSelectCategory()
		{
			Action<GameOverStatCategoryVM> onSelect = this._onSelect;
			if (onSelect == null)
			{
				return;
			}
			onSelect.DynamicInvokeWithLog(new object[] { this });
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x00014CC1 File Offset: 0x00012EC1
		// (set) Token: 0x06000587 RID: 1415 RVA: 0x00014CC9 File Offset: 0x00012EC9
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x00014CEC File Offset: 0x00012EEC
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x00014CF4 File Offset: 0x00012EF4
		[DataSourceProperty]
		public string ID
		{
			get
			{
				return this._id;
			}
			set
			{
				if (value != this._id)
				{
					this._id = value;
					base.OnPropertyChangedWithValue<string>(value, "ID");
				}
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x00014D17 File Offset: 0x00012F17
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x00014D1F File Offset: 0x00012F1F
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x00014D3D File Offset: 0x00012F3D
		// (set) Token: 0x0600058D RID: 1421 RVA: 0x00014D45 File Offset: 0x00012F45
		[DataSourceProperty]
		public MBBindingList<GameOverStatItemVM> Items
		{
			get
			{
				return this._items;
			}
			set
			{
				if (value != this._items)
				{
					this._items = value;
					base.OnPropertyChangedWithValue<MBBindingList<GameOverStatItemVM>>(value, "Items");
				}
			}
		}

		// Token: 0x040002B7 RID: 695
		private readonly StatCategory _category;

		// Token: 0x040002B8 RID: 696
		private readonly Action<GameOverStatCategoryVM> _onSelect;

		// Token: 0x040002B9 RID: 697
		private string _name;

		// Token: 0x040002BA RID: 698
		private string _id;

		// Token: 0x040002BB RID: 699
		private bool _isSelected;

		// Token: 0x040002BC RID: 700
		private MBBindingList<GameOverStatItemVM> _items;
	}
}
