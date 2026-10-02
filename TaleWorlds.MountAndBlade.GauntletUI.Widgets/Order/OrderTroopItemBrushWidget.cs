using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Order
{
	// Token: 0x02000075 RID: 117
	public class OrderTroopItemBrushWidget : BrushWidget
	{
		// Token: 0x06000640 RID: 1600 RVA: 0x0001284C File Offset: 0x00010A4C
		public OrderTroopItemBrushWidget(UIContext context)
			: base(context)
		{
			base.AddState("Selected");
			base.AddState("Disabled");
			this.UpdateBrush();
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00012878 File Offset: 0x00010A78
		private void SelectionStateChanged()
		{
			this.UpdateBackgroundState();
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00012880 File Offset: 0x00010A80
		private void SelectableStateChanged()
		{
			this.UpdateBackgroundState();
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x00012888 File Offset: 0x00010A88
		private void CurrentMemberCountChanged()
		{
			this.UpdateBackgroundState();
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00012890 File Offset: 0x00010A90
		private void UpdateBackgroundState()
		{
			if (this.CurrentMemberCount <= 0 || !this.IsSelectable)
			{
				this.SetState("Disabled");
				return;
			}
			this.SetState(this.IsSelected ? "Selected" : "Default");
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x000128C9 File Offset: 0x00010AC9
		private void UpdateBrush()
		{
			if (this.MeleeCardBrush == null || this.RangedCardBrush == null)
			{
				return;
			}
			if (this.HasAmmo)
			{
				base.Brush = this.RangedCardBrush;
			}
			else
			{
				base.Brush = this.MeleeCardBrush;
			}
			this.UpdateBackgroundState();
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x00012904 File Offset: 0x00010B04
		// (set) Token: 0x06000647 RID: 1607 RVA: 0x0001290C File Offset: 0x00010B0C
		[Editor(false)]
		public int CurrentMemberCount
		{
			get
			{
				return this._currentMemberCount;
			}
			set
			{
				if (this._currentMemberCount != value)
				{
					this._currentMemberCount = value;
					base.OnPropertyChanged(value, "CurrentMemberCount");
					this.CurrentMemberCountChanged();
				}
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x00012930 File Offset: 0x00010B30
		// (set) Token: 0x06000649 RID: 1609 RVA: 0x00012938 File Offset: 0x00010B38
		[Editor(false)]
		public bool IsSelectable
		{
			get
			{
				return this._isSelectable;
			}
			set
			{
				if (this._isSelectable != value)
				{
					this._isSelectable = value;
					base.OnPropertyChanged(value, "IsSelectable");
					this.SelectableStateChanged();
				}
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x0001295C File Offset: 0x00010B5C
		// (set) Token: 0x0600064B RID: 1611 RVA: 0x00012964 File Offset: 0x00010B64
		[Editor(false)]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (this._isSelected != value)
				{
					this._isSelected = value;
					base.OnPropertyChanged(value, "IsSelected");
					this.SelectionStateChanged();
				}
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x00012988 File Offset: 0x00010B88
		// (set) Token: 0x0600064D RID: 1613 RVA: 0x00012990 File Offset: 0x00010B90
		[Editor(false)]
		public bool HasAmmo
		{
			get
			{
				return this._hasAmmo;
			}
			set
			{
				if (this._hasAmmo != value)
				{
					this._hasAmmo = value;
					base.OnPropertyChanged(value, "HasAmmo");
					this.UpdateBrush();
				}
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x000129B4 File Offset: 0x00010BB4
		// (set) Token: 0x0600064F RID: 1615 RVA: 0x000129BC File Offset: 0x00010BBC
		[Editor(false)]
		public Brush RangedCardBrush
		{
			get
			{
				return this._rangedCardBrush;
			}
			set
			{
				if (value != this._rangedCardBrush)
				{
					this._rangedCardBrush = value;
					base.OnPropertyChanged<Brush>(value, "RangedCardBrush");
					this.UpdateBrush();
				}
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x000129E0 File Offset: 0x00010BE0
		// (set) Token: 0x06000651 RID: 1617 RVA: 0x000129E8 File Offset: 0x00010BE8
		[Editor(false)]
		public Brush MeleeCardBrush
		{
			get
			{
				return this._meleeCardBrush;
			}
			set
			{
				if (value != this._meleeCardBrush)
				{
					this._meleeCardBrush = value;
					base.OnPropertyChanged<Brush>(value, "MeleeCardBrush");
					this.UpdateBrush();
				}
			}
		}

		// Token: 0x040002B0 RID: 688
		private int _currentMemberCount;

		// Token: 0x040002B1 RID: 689
		private bool _isSelectable;

		// Token: 0x040002B2 RID: 690
		private bool _isSelected;

		// Token: 0x040002B3 RID: 691
		private bool _hasAmmo = true;

		// Token: 0x040002B4 RID: 692
		private Brush _rangedCardBrush;

		// Token: 0x040002B5 RID: 693
		private Brush _meleeCardBrush;
	}
}
