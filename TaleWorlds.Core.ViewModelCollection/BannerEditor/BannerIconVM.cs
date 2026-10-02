using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.BannerEditor
{
	// Token: 0x0200002D RID: 45
	public class BannerIconVM : ViewModel
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x00005FDA File Offset: 0x000041DA
		public int IconID { get; }

		// Token: 0x060001E5 RID: 485 RVA: 0x00005FE2 File Offset: 0x000041E2
		public BannerIconVM(int iconID, Action<BannerIconVM> onSelection)
		{
			this.IconPath = iconID.ToString();
			this.IconID = iconID;
			this._onSelection = onSelection;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00006005 File Offset: 0x00004205
		public void ExecuteSelectIcon()
		{
			this._onSelection(this);
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00006013 File Offset: 0x00004213
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x0000601B File Offset: 0x0000421B
		[DataSourceProperty]
		public string IconPath
		{
			get
			{
				return this._iconPath;
			}
			set
			{
				if (value != this._iconPath)
				{
					this._iconPath = value;
					base.OnPropertyChangedWithValue<string>(value, "IconPath");
				}
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000603E File Offset: 0x0000423E
		// (set) Token: 0x060001EA RID: 490 RVA: 0x00006046 File Offset: 0x00004246
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

		// Token: 0x040000CB RID: 203
		private readonly Action<BannerIconVM> _onSelection;

		// Token: 0x040000CC RID: 204
		private string _iconPath;

		// Token: 0x040000CD RID: 205
		private bool _isSelected;
	}
}
