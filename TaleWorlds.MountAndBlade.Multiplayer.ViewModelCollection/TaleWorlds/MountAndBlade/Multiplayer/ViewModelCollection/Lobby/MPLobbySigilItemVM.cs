using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x02000030 RID: 48
	public class MPLobbySigilItemVM : ViewModel
	{
		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600039A RID: 922 RVA: 0x0000CEDF File Offset: 0x0000B0DF
		// (set) Token: 0x0600039B RID: 923 RVA: 0x0000CEE7 File Offset: 0x0000B0E7
		public int IconID { get; private set; }

		// Token: 0x0600039C RID: 924 RVA: 0x0000CEF0 File Offset: 0x0000B0F0
		public MPLobbySigilItemVM()
		{
			this.RefreshWith(0);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000CEFF File Offset: 0x0000B0FF
		public MPLobbySigilItemVM(int iconID, Action<MPLobbySigilItemVM> onSelection)
		{
			this.RefreshWith(iconID);
			this._onSelection = onSelection;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000CF15 File Offset: 0x0000B115
		public void RefreshWith(int iconID)
		{
			this.IconPath = iconID.ToString();
			this.IconID = iconID;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000CF2B File Offset: 0x0000B12B
		public void RefreshWith(Banner banner)
		{
			this.RefreshWith(banner.GetIconMeshId());
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000CF39 File Offset: 0x0000B139
		public void RefreshWith(string bannerCode)
		{
			this.RefreshWith(new Banner(bannerCode));
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000CF47 File Offset: 0x0000B147
		private void ExecuteSelectIcon()
		{
			Action<MPLobbySigilItemVM> onSelection = this._onSelection;
			if (onSelection == null)
			{
				return;
			}
			onSelection(this);
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x0000CF5A File Offset: 0x0000B15A
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x0000CF62 File Offset: 0x0000B162
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
					base.OnPropertyChanged("IconPath");
				}
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0000CF84 File Offset: 0x0000B184
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x0000CF8C File Offset: 0x0000B18C
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
					base.OnPropertyChanged("IsSelected");
				}
			}
		}

		// Token: 0x040001D3 RID: 467
		private readonly Action<MPLobbySigilItemVM> _onSelection;

		// Token: 0x040001D4 RID: 468
		private string _iconPath;

		// Token: 0x040001D5 RID: 469
		private bool _isSelected;
	}
}
