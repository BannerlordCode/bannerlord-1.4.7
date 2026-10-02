using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000067 RID: 103
	public class MPLobbyClanChangeSigilPopupVM : ViewModel
	{
		// Token: 0x060009DA RID: 2522 RVA: 0x0001EB7C File Offset: 0x0001CD7C
		public MPLobbyClanChangeSigilPopupVM()
		{
			this.PrepareSigilIconsList();
			this.CanChangeSigil = false;
			this.RefreshValues();
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0001EB97 File Offset: 0x0001CD97
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=q7VcSSbp}Choose Sigil", null).ToString();
			this.ApplyText = new TextObject("{=BAaS5Dkc}Apply", null).ToString();
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0001EBCC File Offset: 0x0001CDCC
		private void PrepareSigilIconsList()
		{
			this.IconsList = new MBBindingList<MPLobbySigilItemVM>();
			this._selectedSigilIcon = null;
			foreach (BannerIconGroup bannerIconGroup in BannerManager.Instance.BannerIconGroups)
			{
				if (!bannerIconGroup.IsPattern)
				{
					foreach (KeyValuePair<int, BannerIconData> keyValuePair in bannerIconGroup.AvailableIcons)
					{
						MPLobbySigilItemVM mplobbySigilItemVM = new MPLobbySigilItemVM(keyValuePair.Key, new Action<MPLobbySigilItemVM>(this.OnSigilIconSelection));
						this.IconsList.Add(mplobbySigilItemVM);
					}
				}
			}
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0001EC98 File Offset: 0x0001CE98
		private void OnSigilIconSelection(MPLobbySigilItemVM sigilIcon)
		{
			if (sigilIcon != this._selectedSigilIcon)
			{
				if (this._selectedSigilIcon != null)
				{
					this._selectedSigilIcon.IsSelected = false;
				}
				this._selectedSigilIcon = sigilIcon;
				if (this._selectedSigilIcon != null)
				{
					this._selectedSigilIcon.IsSelected = true;
					this.CanChangeSigil = true;
				}
			}
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0001ECE4 File Offset: 0x0001CEE4
		public void ExecuteOpenPopup()
		{
			this.IsSelected = true;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0001ECED File Offset: 0x0001CEED
		public void ExecuteClosePopup()
		{
			this.IsSelected = false;
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0001ECF8 File Offset: 0x0001CEF8
		public void ExecuteChangeSigil()
		{
			BasicCultureObject @object = Game.Current.ObjectManager.GetObject<BasicCultureObject>(NetworkMain.GameClient.ClanInfo.Faction);
			Banner banner = new Banner(@object.Banner, @object.BackgroundColor1, @object.ForegroundColor1);
			banner.SetIconMeshId(this._selectedSigilIcon.IconID);
			NetworkMain.GameClient.ChangeClanSigil(banner.Serialize());
			this.ExecuteClosePopup();
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0001ED63 File Offset: 0x0001CF63
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey == null)
			{
				return;
			}
			doneInputKey.OnFinalize();
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0001ED8C File Offset: 0x0001CF8C
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0001ED9B File Offset: 0x0001CF9B
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x060009E4 RID: 2532 RVA: 0x0001EDAA File Offset: 0x0001CFAA
		// (set) Token: 0x060009E5 RID: 2533 RVA: 0x0001EDB2 File Offset: 0x0001CFB2
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChanged("CancelInputKey");
				}
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060009E6 RID: 2534 RVA: 0x0001EDCF File Offset: 0x0001CFCF
		// (set) Token: 0x060009E7 RID: 2535 RVA: 0x0001EDD7 File Offset: 0x0001CFD7
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChanged("DoneInputKey");
				}
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0001EDF4 File Offset: 0x0001CFF4
		// (set) Token: 0x060009E9 RID: 2537 RVA: 0x0001EDFC File Offset: 0x0001CFFC
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

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x060009EA RID: 2538 RVA: 0x0001EE19 File Offset: 0x0001D019
		// (set) Token: 0x060009EB RID: 2539 RVA: 0x0001EE21 File Offset: 0x0001D021
		[DataSourceProperty]
		public bool CanChangeSigil
		{
			get
			{
				return this._canChangeSigil;
			}
			set
			{
				if (value != this._canChangeSigil)
				{
					this._canChangeSigil = value;
					base.OnPropertyChanged("CanChangeSigil");
				}
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060009EC RID: 2540 RVA: 0x0001EE3E File Offset: 0x0001D03E
		// (set) Token: 0x060009ED RID: 2541 RVA: 0x0001EE46 File Offset: 0x0001D046
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChanged("TitleText");
				}
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060009EE RID: 2542 RVA: 0x0001EE68 File Offset: 0x0001D068
		// (set) Token: 0x060009EF RID: 2543 RVA: 0x0001EE70 File Offset: 0x0001D070
		[DataSourceProperty]
		public string ApplyText
		{
			get
			{
				return this._applyText;
			}
			set
			{
				if (value != this._applyText)
				{
					this._applyText = value;
					base.OnPropertyChanged("ApplyText");
				}
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x060009F0 RID: 2544 RVA: 0x0001EE92 File Offset: 0x0001D092
		// (set) Token: 0x060009F1 RID: 2545 RVA: 0x0001EE9A File Offset: 0x0001D09A
		[DataSourceProperty]
		public MBBindingList<MPLobbySigilItemVM> IconsList
		{
			get
			{
				return this._iconsList;
			}
			set
			{
				if (value != this._iconsList)
				{
					this._iconsList = value;
					base.OnPropertyChanged("IconsList");
				}
			}
		}

		// Token: 0x04000489 RID: 1161
		private MPLobbySigilItemVM _selectedSigilIcon;

		// Token: 0x0400048A RID: 1162
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x0400048B RID: 1163
		private InputKeyItemVM _doneInputKey;

		// Token: 0x0400048C RID: 1164
		private bool _isSelected;

		// Token: 0x0400048D RID: 1165
		private bool _canChangeSigil;

		// Token: 0x0400048E RID: 1166
		private string _titleText;

		// Token: 0x0400048F RID: 1167
		private string _applyText;

		// Token: 0x04000490 RID: 1168
		private MBBindingList<MPLobbySigilItemVM> _iconsList;
	}
}
