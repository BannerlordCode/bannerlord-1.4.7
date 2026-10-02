using System;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x02000103 RID: 259
	public class TierFilterTypeVM : ViewModel
	{
		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06001789 RID: 6025 RVA: 0x0005A84E File Offset: 0x00058A4E
		public WeaponDesignVM.CraftingPieceTierFilter FilterType { get; }

		// Token: 0x0600178A RID: 6026 RVA: 0x0005A856 File Offset: 0x00058A56
		public TierFilterTypeVM(WeaponDesignVM.CraftingPieceTierFilter filterType, Action<WeaponDesignVM.CraftingPieceTierFilter> onSelect, string tierName)
		{
			this.FilterType = filterType;
			this._onSelect = onSelect;
			this.TierName = tierName;
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x0005A873 File Offset: 0x00058A73
		public void ExecuteSelectTier()
		{
			Action<WeaponDesignVM.CraftingPieceTierFilter> onSelect = this._onSelect;
			if (onSelect == null)
			{
				return;
			}
			onSelect(this.FilterType);
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x0600178C RID: 6028 RVA: 0x0005A88B File Offset: 0x00058A8B
		// (set) Token: 0x0600178D RID: 6029 RVA: 0x0005A893 File Offset: 0x00058A93
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

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x0600178E RID: 6030 RVA: 0x0005A8B1 File Offset: 0x00058AB1
		// (set) Token: 0x0600178F RID: 6031 RVA: 0x0005A8B9 File Offset: 0x00058AB9
		[DataSourceProperty]
		public string TierName
		{
			get
			{
				return this._tierName;
			}
			set
			{
				if (value != this._tierName)
				{
					this._tierName = value;
					base.OnPropertyChangedWithValue<string>(value, "TierName");
				}
			}
		}

		// Token: 0x04000AC4 RID: 2756
		private readonly Action<WeaponDesignVM.CraftingPieceTierFilter> _onSelect;

		// Token: 0x04000AC5 RID: 2757
		private bool _isSelected;

		// Token: 0x04000AC6 RID: 2758
		private string _tierName;
	}
}
