using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x02000057 RID: 87
	public class EquipmentActionItemVM : ViewModel
	{
		// Token: 0x0600072B RID: 1835 RVA: 0x0001A500 File Offset: 0x00018700
		public EquipmentActionItemVM(string item, string itemTypeAsString, object identifier, Action<EquipmentActionItemVM> onSelection, bool isCurrentlyWielded = false)
		{
			this.Identifier = identifier;
			this.ActionText = item;
			this.TypeAsString = itemTypeAsString;
			this.IsWielded = isCurrentlyWielded;
			this._onSelection = onSelection;
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x0001A52D File Offset: 0x0001872D
		// (set) Token: 0x0600072D RID: 1837 RVA: 0x0001A535 File Offset: 0x00018735
		[DataSourceProperty]
		public string ActionText
		{
			get
			{
				return this._actionText;
			}
			set
			{
				if (value != this._actionText)
				{
					this._actionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ActionText");
				}
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x0001A558 File Offset: 0x00018758
		// (set) Token: 0x0600072F RID: 1839 RVA: 0x0001A560 File Offset: 0x00018760
		[DataSourceProperty]
		public bool IsWielded
		{
			get
			{
				return this._isWielded;
			}
			set
			{
				if (value != this._isWielded)
				{
					this._isWielded = value;
					base.OnPropertyChangedWithValue(value, "IsWielded");
				}
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x0001A57E File Offset: 0x0001877E
		// (set) Token: 0x06000731 RID: 1841 RVA: 0x0001A586 File Offset: 0x00018786
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
					if (value)
					{
						this._onSelection(this);
					}
				}
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x0001A5B3 File Offset: 0x000187B3
		// (set) Token: 0x06000733 RID: 1843 RVA: 0x0001A5BB File Offset: 0x000187BB
		[DataSourceProperty]
		public string TypeAsString
		{
			get
			{
				return this._typeAsString;
			}
			set
			{
				if (value != this._typeAsString)
				{
					this._typeAsString = value;
					base.OnPropertyChangedWithValue<string>(value, "TypeAsString");
				}
			}
		}

		// Token: 0x0400032E RID: 814
		private readonly Action<EquipmentActionItemVM> _onSelection;

		// Token: 0x0400032F RID: 815
		public object Identifier;

		// Token: 0x04000330 RID: 816
		private string _actionText;

		// Token: 0x04000331 RID: 817
		private string _typeAsString;

		// Token: 0x04000332 RID: 818
		private bool _isSelected;

		// Token: 0x04000333 RID: 819
		private bool _isWielded;
	}
}
