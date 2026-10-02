using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.BannerBuilder
{
	// Token: 0x02000085 RID: 133
	public class BannerBuilderColorItemVM : ViewModel
	{
		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x00027364 File Offset: 0x00025564
		// (set) Token: 0x06000AE3 RID: 2787 RVA: 0x0002736C File Offset: 0x0002556C
		public int ColorID { get; private set; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x00027375 File Offset: 0x00025575
		// (set) Token: 0x06000AE5 RID: 2789 RVA: 0x0002737D File Offset: 0x0002557D
		public BannerColor BannerColor { get; private set; }

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00027388 File Offset: 0x00025588
		public BannerBuilderColorItemVM(Action<BannerBuilderColorItemVM> onItemSelection, int key, BannerColor value)
		{
			this._onItemSelection = onItemSelection;
			this.ColorID = key;
			this.BannerColor = value;
			this.ColorAsStr = Color.FromUint(value.Color).ToString();
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x000273D0 File Offset: 0x000255D0
		public void ExecuteSelection()
		{
			Action<BannerBuilderColorItemVM> onItemSelection = this._onItemSelection;
			if (onItemSelection == null)
			{
				return;
			}
			onItemSelection(this);
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x000273E3 File Offset: 0x000255E3
		// (set) Token: 0x06000AE9 RID: 2793 RVA: 0x000273EB File Offset: 0x000255EB
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

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x00027409 File Offset: 0x00025609
		// (set) Token: 0x06000AEB RID: 2795 RVA: 0x00027411 File Offset: 0x00025611
		[DataSourceProperty]
		public string ColorAsStr
		{
			get
			{
				return this._colorAsStr;
			}
			set
			{
				if (value != this._colorAsStr)
				{
					this._colorAsStr = value;
					base.OnPropertyChangedWithValue<string>(value, "ColorAsStr");
				}
			}
		}

		// Token: 0x040004F6 RID: 1270
		private readonly Action<BannerBuilderColorItemVM> _onItemSelection;

		// Token: 0x040004F9 RID: 1273
		private bool _isSelected;

		// Token: 0x040004FA RID: 1274
		private string _colorAsStr;
	}
}
