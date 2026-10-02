using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x02000021 RID: 33
	public abstract class OrderSubjectVM : ViewModel
	{
		// Token: 0x06000305 RID: 773 RVA: 0x0000C32C File Offset: 0x0000A52C
		public OrderSubjectVM()
		{
			this.ActiveOrders = new MBBindingList<OrderItemVM>();
			this.RefreshValues();
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000C345 File Offset: 0x0000A545
		public void AddActiveOrder(OrderItemVM order)
		{
			this.ActiveOrders.Add(order);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000C353 File Offset: 0x0000A553
		public void RemoveActiveOrder(OrderItemVM order)
		{
			this.ActiveOrders.Remove(order);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000C362 File Offset: 0x0000A562
		public void ClearActiveOrders()
		{
			this.ActiveOrders.Clear();
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000C36F File Offset: 0x0000A56F
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SelectionText = new TextObject("{=xbk1WAt6}Select", null).ToString();
		}

		// Token: 0x0600030A RID: 778
		protected abstract void OnSelectionStateChanged(bool isSelected);

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600030B RID: 779 RVA: 0x0000C38D File Offset: 0x0000A58D
		// (set) Token: 0x0600030C RID: 780 RVA: 0x0000C395 File Offset: 0x0000A595
		[DataSourceProperty]
		public bool IsSelectable
		{
			get
			{
				return this._isSelectable;
			}
			set
			{
				if (value != this._isSelectable)
				{
					this._isSelectable = value;
					base.OnPropertyChangedWithValue(value, "IsSelectable");
				}
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600030D RID: 781 RVA: 0x0000C3B3 File Offset: 0x0000A5B3
		// (set) Token: 0x0600030E RID: 782 RVA: 0x0000C3BB File Offset: 0x0000A5BB
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if ((!value || this.IsSelectable) && value != this._isSelected)
				{
					this._isSelected = value;
					this.OnSelectionStateChanged(value);
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600030F RID: 783 RVA: 0x0000C3EB File Offset: 0x0000A5EB
		// (set) Token: 0x06000310 RID: 784 RVA: 0x0000C3F3 File Offset: 0x0000A5F3
		[DataSourceProperty]
		public bool IsSelectionHighlightActive
		{
			get
			{
				return this._isSelectionHighlightActive;
			}
			set
			{
				if (value != this._isSelectionHighlightActive)
				{
					this._isSelectionHighlightActive = value;
					base.OnPropertyChangedWithValue(value, "IsSelectionHighlightActive");
				}
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000311 RID: 785 RVA: 0x0000C411 File Offset: 0x0000A611
		// (set) Token: 0x06000312 RID: 786 RVA: 0x0000C419 File Offset: 0x0000A619
		[DataSourceProperty]
		public bool ShowSelectionInputs
		{
			get
			{
				return this._showSelectionInputs;
			}
			set
			{
				if (value != this._showSelectionInputs)
				{
					this._showSelectionInputs = value;
					base.OnPropertyChangedWithValue(value, "ShowSelectionInputs");
				}
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000313 RID: 787 RVA: 0x0000C437 File Offset: 0x0000A637
		// (set) Token: 0x06000314 RID: 788 RVA: 0x0000C43F File Offset: 0x0000A63F
		[DataSourceProperty]
		public int BehaviorType
		{
			get
			{
				return this._behaviorType;
			}
			set
			{
				if (value != this._behaviorType)
				{
					this._behaviorType = value;
					base.OnPropertyChangedWithValue(value, "BehaviorType");
				}
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000315 RID: 789 RVA: 0x0000C45D File Offset: 0x0000A65D
		// (set) Token: 0x06000316 RID: 790 RVA: 0x0000C465 File Offset: 0x0000A665
		[DataSourceProperty]
		public int UnderAttackOfType
		{
			get
			{
				return this._underAttackOfType;
			}
			set
			{
				if (value != this._underAttackOfType)
				{
					this._underAttackOfType = value;
					base.OnPropertyChangedWithValue(value, "UnderAttackOfType");
				}
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000317 RID: 791 RVA: 0x0000C483 File Offset: 0x0000A683
		// (set) Token: 0x06000318 RID: 792 RVA: 0x0000C48B File Offset: 0x0000A68B
		[DataSourceProperty]
		public string SelectionText
		{
			get
			{
				return this._selectionText;
			}
			set
			{
				if (value != this._selectionText)
				{
					this._selectionText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectionText");
				}
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000319 RID: 793 RVA: 0x0000C4AE File Offset: 0x0000A6AE
		// (set) Token: 0x0600031A RID: 794 RVA: 0x0000C4B6 File Offset: 0x0000A6B6
		[DataSourceProperty]
		public InputKeyItemVM ApplySelectionKey
		{
			get
			{
				return this._applySelectionKey;
			}
			set
			{
				if (value != this._applySelectionKey)
				{
					this._applySelectionKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ApplySelectionKey");
				}
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600031B RID: 795 RVA: 0x0000C4D4 File Offset: 0x0000A6D4
		// (set) Token: 0x0600031C RID: 796 RVA: 0x0000C4DC File Offset: 0x0000A6DC
		[DataSourceProperty]
		public InputKeyItemVM ToggleSelectionKey
		{
			get
			{
				return this._toggleSelectionKey;
			}
			set
			{
				if (value != this._toggleSelectionKey)
				{
					this._toggleSelectionKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ToggleSelectionKey");
				}
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600031D RID: 797 RVA: 0x0000C4FA File Offset: 0x0000A6FA
		// (set) Token: 0x0600031E RID: 798 RVA: 0x0000C502 File Offset: 0x0000A702
		[DataSourceProperty]
		public MBBindingList<OrderItemVM> ActiveOrders
		{
			get
			{
				return this._activeOrders;
			}
			set
			{
				if (value != this._activeOrders)
				{
					this._activeOrders = value;
					base.OnPropertyChangedWithValue<MBBindingList<OrderItemVM>>(value, "ActiveOrders");
				}
			}
		}

		// Token: 0x04000150 RID: 336
		private int _behaviorType;

		// Token: 0x04000151 RID: 337
		private int _underAttackOfType;

		// Token: 0x04000152 RID: 338
		private bool _isSelectable;

		// Token: 0x04000153 RID: 339
		private bool _isSelected;

		// Token: 0x04000154 RID: 340
		private bool _isSelectionHighlightActive;

		// Token: 0x04000155 RID: 341
		private bool _showSelectionInputs;

		// Token: 0x04000156 RID: 342
		private string _selectionText;

		// Token: 0x04000157 RID: 343
		private InputKeyItemVM _applySelectionKey;

		// Token: 0x04000158 RID: 344
		private InputKeyItemVM _toggleSelectionKey;

		// Token: 0x04000159 RID: 345
		private MBBindingList<OrderItemVM> _activeOrders;
	}
}
