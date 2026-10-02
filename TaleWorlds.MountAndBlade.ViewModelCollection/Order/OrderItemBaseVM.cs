using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x0200001D RID: 29
	public abstract class OrderItemBaseVM : ViewModel
	{
		// Token: 0x060002C2 RID: 706 RVA: 0x0000B9C8 File Offset: 0x00009BC8
		public OrderItemBaseVM(OrderController orderController)
		{
			this._orderController = orderController;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000B9D7 File Offset: 0x00009BD7
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM shortcutKey = this.ShortcutKey;
			if (shortcutKey == null)
			{
				return;
			}
			shortcutKey.OnFinalize();
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000B9EF File Offset: 0x00009BEF
		public void RefreshState()
		{
			this.OnRefreshState();
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000B9F7 File Offset: 0x00009BF7
		public void ExecuteAction(VisualOrderExecutionParameters executionParameters)
		{
			this.OnExecuteAction(executionParameters);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000BA00 File Offset: 0x00009C00
		protected virtual void OnSelectedStateChanged(bool isSelected)
		{
		}

		// Token: 0x060002C7 RID: 711
		protected abstract void OnRefreshState();

		// Token: 0x060002C8 RID: 712
		protected abstract void OnExecuteAction(VisualOrderExecutionParameters executionParameters);

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000BA02 File Offset: 0x00009C02
		// (set) Token: 0x060002CA RID: 714 RVA: 0x0000BA0A File Offset: 0x00009C0A
		[DataSourceProperty]
		public InputKeyItemVM ShortcutKey
		{
			get
			{
				return this._shortcutKey;
			}
			set
			{
				if (value != this._shortcutKey)
				{
					this._shortcutKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ShortcutKey");
				}
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000BA28 File Offset: 0x00009C28
		public void SetShortcutKey(InputKeyItemVM inputKeyItem)
		{
			this.ShortcutKey = inputKeyItem;
			InputKeyItemVM shortcutKey = this.ShortcutKey;
			if (shortcutKey == null)
			{
				return;
			}
			shortcutKey.RefreshValues();
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0000BA41 File Offset: 0x00009C41
		// (set) Token: 0x060002CD RID: 717 RVA: 0x0000BA49 File Offset: 0x00009C49
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				this._isActive = value;
				base.OnPropertyChangedWithValue(value, "IsActive");
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002CE RID: 718 RVA: 0x0000BA5E File Offset: 0x00009C5E
		// (set) Token: 0x060002CF RID: 719 RVA: 0x0000BA66 File Offset: 0x00009C66
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				this._isSelected = value;
				base.OnPropertyChangedWithValue(value, "IsSelected");
				this.OnSelectedStateChanged(value);
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0000BA82 File Offset: 0x00009C82
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x0000BA8A File Offset: 0x00009C8A
		[DataSourceProperty]
		public bool CanUseShortcuts
		{
			get
			{
				return this._canUseShortcuts;
			}
			set
			{
				if (value != this._canUseShortcuts)
				{
					this._canUseShortcuts = value;
					base.OnPropertyChangedWithValue(value, "CanUseShortcuts");
				}
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000BAA8 File Offset: 0x00009CA8
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x0000BAB0 File Offset: 0x00009CB0
		[DataSourceProperty]
		public string OrderIconId
		{
			get
			{
				return this._orderIconId;
			}
			set
			{
				if (value != this._orderIconId)
				{
					this._orderIconId = value;
					base.OnPropertyChangedWithValue<string>(value, "OrderIconId");
				}
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000BAD3 File Offset: 0x00009CD3
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x0000BADB File Offset: 0x00009CDB
		[DataSourceProperty]
		public string SelectionState
		{
			get
			{
				return this._selectionState;
			}
			set
			{
				if (value != this._selectionState)
				{
					this._selectionState = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectionState");
				}
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000BAFE File Offset: 0x00009CFE
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x0000BB06 File Offset: 0x00009D06
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

		// Token: 0x0400013A RID: 314
		protected OrderController _orderController;

		// Token: 0x0400013B RID: 315
		private InputKeyItemVM _shortcutKey;

		// Token: 0x0400013C RID: 316
		private bool _isActive;

		// Token: 0x0400013D RID: 317
		private bool _isSelected;

		// Token: 0x0400013E RID: 318
		private bool _canUseShortcuts;

		// Token: 0x0400013F RID: 319
		private string _orderIconId;

		// Token: 0x04000140 RID: 320
		private string _selectionState;

		// Token: 0x04000141 RID: 321
		private string _name;
	}
}
