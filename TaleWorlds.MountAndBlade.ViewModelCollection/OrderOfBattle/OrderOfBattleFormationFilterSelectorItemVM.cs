using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x02000031 RID: 49
	public class OrderOfBattleFormationFilterSelectorItemVM : ViewModel
	{
		// Token: 0x060003A9 RID: 937 RVA: 0x0000D922 File Offset: 0x0000BB22
		public OrderOfBattleFormationFilterSelectorItemVM(FormationFilterType filterType, Action<OrderOfBattleFormationFilterSelectorItemVM> onToggled)
		{
			this.FilterType = filterType;
			this.FilterTypeValue = (int)filterType;
			this._onToggled = onToggled;
			this.RefreshValues();
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000D945 File Offset: 0x0000BB45
		public override void RefreshValues()
		{
			this.Hint = new HintViewModel(this.FilterType.GetFilterDescription(), null);
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060003AB RID: 939 RVA: 0x0000D95E File Offset: 0x0000BB5E
		// (set) Token: 0x060003AC RID: 940 RVA: 0x0000D966 File Offset: 0x0000BB66
		[DataSourceProperty]
		public int FilterTypeValue
		{
			get
			{
				return this._filterType;
			}
			set
			{
				if (value != this._filterType)
				{
					this._filterType = value;
					base.OnPropertyChangedWithValue(value, "FilterTypeValue");
				}
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060003AD RID: 941 RVA: 0x0000D984 File Offset: 0x0000BB84
		// (set) Token: 0x060003AE RID: 942 RVA: 0x0000D98C File Offset: 0x0000BB8C
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
					Action<OrderOfBattleFormationFilterSelectorItemVM> onToggled = this._onToggled;
					if (onToggled == null)
					{
						return;
					}
					onToggled(this);
				}
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060003AF RID: 943 RVA: 0x0000D9BB File Offset: 0x0000BBBB
		// (set) Token: 0x060003B0 RID: 944 RVA: 0x0000D9C3 File Offset: 0x0000BBC3
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x0000D9E1 File Offset: 0x0000BBE1
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x0000D9E9 File Offset: 0x0000BBE9
		[DataSourceProperty]
		public HintViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x0400019F RID: 415
		public readonly FormationFilterType FilterType;

		// Token: 0x040001A0 RID: 416
		private Action<OrderOfBattleFormationFilterSelectorItemVM> _onToggled;

		// Token: 0x040001A1 RID: 417
		private int _filterType;

		// Token: 0x040001A2 RID: 418
		private bool _isActive;

		// Token: 0x040001A3 RID: 419
		private bool _isEnabled;

		// Token: 0x040001A4 RID: 420
		private HintViewModel _hint;
	}
}
