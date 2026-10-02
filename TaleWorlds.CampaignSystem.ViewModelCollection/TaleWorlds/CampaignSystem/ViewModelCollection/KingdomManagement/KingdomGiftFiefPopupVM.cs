using System;
using Helpers;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Clans;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement
{
	// Token: 0x02000063 RID: 99
	public class KingdomGiftFiefPopupVM : ViewModel
	{
		// Token: 0x06000766 RID: 1894 RVA: 0x000234CA File Offset: 0x000216CA
		public KingdomGiftFiefPopupVM(Action onSettlementGranted)
		{
			this._clans = new MBBindingList<KingdomClanItemVM>();
			this._onSettlementGranted = onSettlementGranted;
			this.ClanSortController = new KingdomClanSortControllerVM(ref this._clans);
			this.RefreshValues();
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x000234FC File Offset: 0x000216FC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=rOKAvjtT}Gift Settlement", null).ToString();
			this.GiftText = GameTexts.FindText("str_gift", null).ToString();
			this.CancelText = GameTexts.FindText("str_cancel", null).ToString();
			this.NameText = GameTexts.FindText("str_scoreboard_header", "name").ToString();
			this.InfluenceText = GameTexts.FindText("str_influence", null).ToString();
			this.FiefsText = GameTexts.FindText("str_fiefs", null).ToString();
			this.MembersText = GameTexts.FindText("str_members", null).ToString();
			this.BannerText = GameTexts.FindText("str_banner", null).ToString();
			this.TypeText = GameTexts.FindText("str_sort_by_type_label", null).ToString();
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x000235D9 File Offset: 0x000217D9
		private void SetCurrentSelectedClan(KingdomClanItemVM clan)
		{
			if (clan != this.CurrentSelectedClan)
			{
				if (this.CurrentSelectedClan != null)
				{
					this.CurrentSelectedClan.IsSelected = false;
				}
				this.CurrentSelectedClan = clan;
				this.CurrentSelectedClan.IsSelected = true;
				this.IsAnyClanSelected = true;
			}
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00023614 File Offset: 0x00021814
		private void RefreshClanList()
		{
			this.Clans.Clear();
			foreach (Clan clan in Clan.PlayerClan.Kingdom.Clans)
			{
				if (FactionHelper.CanClanBeGrantedFief(clan))
				{
					this.Clans.Add(new KingdomClanItemVM(clan, new Action<KingdomClanItemVM>(this.SetCurrentSelectedClan)));
				}
			}
			if (this.Clans.Count > 0)
			{
				this.SetCurrentSelectedClan(this.Clans[0]);
			}
			if (this.ClanSortController != null)
			{
				this.ClanSortController.SortByCurrentState();
			}
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x000236CC File Offset: 0x000218CC
		public void OpenWith(Settlement settlement)
		{
			this._settlementToGive = settlement;
			this.RefreshClanList();
			this.IsOpen = true;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x000236E4 File Offset: 0x000218E4
		public void ExecuteGiftSettlement()
		{
			if (this._settlementToGive != null && this.CurrentSelectedClan != null)
			{
				Campaign.Current.KingdomManager.GiftSettlementOwnership(this._settlementToGive, this.CurrentSelectedClan.Clan);
				this.ExecuteClose();
				this._onSettlementGranted();
			}
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00023732 File Offset: 0x00021932
		public void ExecuteClose()
		{
			this._settlementToGive = null;
			this.IsOpen = false;
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x00023742 File Offset: 0x00021942
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey == null)
			{
				return;
			}
			cancelInputKey.OnFinalize();
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0002376B File Offset: 0x0002196B
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0002377A File Offset: 0x0002197A
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x00023789 File Offset: 0x00021989
		// (set) Token: 0x06000771 RID: 1905 RVA: 0x00023791 File Offset: 0x00021991
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
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000772 RID: 1906 RVA: 0x000237AF File Offset: 0x000219AF
		// (set) Token: 0x06000773 RID: 1907 RVA: 0x000237B7 File Offset: 0x000219B7
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
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x000237D5 File Offset: 0x000219D5
		// (set) Token: 0x06000775 RID: 1909 RVA: 0x000237DD File Offset: 0x000219DD
		[DataSourceProperty]
		public bool IsAnyClanSelected
		{
			get
			{
				return this._isAnyClanSelected;
			}
			set
			{
				if (value != this._isAnyClanSelected)
				{
					this._isAnyClanSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyClanSelected");
				}
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000776 RID: 1910 RVA: 0x000237FB File Offset: 0x000219FB
		// (set) Token: 0x06000777 RID: 1911 RVA: 0x00023803 File Offset: 0x00021A03
		[DataSourceProperty]
		public MBBindingList<KingdomClanItemVM> Clans
		{
			get
			{
				return this._clans;
			}
			set
			{
				if (value != this._clans)
				{
					this._clans = value;
					base.OnPropertyChangedWithValue<MBBindingList<KingdomClanItemVM>>(value, "Clans");
				}
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x00023821 File Offset: 0x00021A21
		// (set) Token: 0x06000779 RID: 1913 RVA: 0x00023829 File Offset: 0x00021A29
		[DataSourceProperty]
		public KingdomClanItemVM CurrentSelectedClan
		{
			get
			{
				return this._currentSelectedClan;
			}
			set
			{
				if (value != this._currentSelectedClan)
				{
					this._currentSelectedClan = value;
					base.OnPropertyChangedWithValue<KingdomClanItemVM>(value, "CurrentSelectedClan");
				}
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x00023847 File Offset: 0x00021A47
		// (set) Token: 0x0600077B RID: 1915 RVA: 0x0002384F File Offset: 0x00021A4F
		[DataSourceProperty]
		public KingdomClanSortControllerVM ClanSortController
		{
			get
			{
				return this._clanSortController;
			}
			set
			{
				if (value != this._clanSortController)
				{
					this._clanSortController = value;
					base.OnPropertyChangedWithValue<KingdomClanSortControllerVM>(value, "ClanSortController");
				}
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x0600077C RID: 1916 RVA: 0x0002386D File Offset: 0x00021A6D
		// (set) Token: 0x0600077D RID: 1917 RVA: 0x00023875 File Offset: 0x00021A75
		[DataSourceProperty]
		public bool IsOpen
		{
			get
			{
				return this._isOpen;
			}
			set
			{
				if (value != this._isOpen)
				{
					this._isOpen = value;
					base.OnPropertyChangedWithValue(value, "IsOpen");
				}
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x0600077E RID: 1918 RVA: 0x00023893 File Offset: 0x00021A93
		// (set) Token: 0x0600077F RID: 1919 RVA: 0x0002389B File Offset: 0x00021A9B
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
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x000238BE File Offset: 0x00021ABE
		// (set) Token: 0x06000781 RID: 1921 RVA: 0x000238C6 File Offset: 0x00021AC6
		[DataSourceProperty]
		public string GiftText
		{
			get
			{
				return this._giftText;
			}
			set
			{
				if (value != this._giftText)
				{
					this._giftText = value;
					base.OnPropertyChangedWithValue<string>(value, "GiftText");
				}
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x000238E9 File Offset: 0x00021AE9
		// (set) Token: 0x06000783 RID: 1923 RVA: 0x000238F1 File Offset: 0x00021AF1
		[DataSourceProperty]
		public string CancelText
		{
			get
			{
				return this._cancelText;
			}
			set
			{
				if (value != this._cancelText)
				{
					this._cancelText = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelText");
				}
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x00023914 File Offset: 0x00021B14
		// (set) Token: 0x06000785 RID: 1925 RVA: 0x0002391C File Offset: 0x00021B1C
		[DataSourceProperty]
		public string BannerText
		{
			get
			{
				return this._bannerText;
			}
			set
			{
				if (value != this._bannerText)
				{
					this._bannerText = value;
					base.OnPropertyChangedWithValue<string>(value, "BannerText");
				}
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x0002393F File Offset: 0x00021B3F
		// (set) Token: 0x06000787 RID: 1927 RVA: 0x00023947 File Offset: 0x00021B47
		[DataSourceProperty]
		public string TypeText
		{
			get
			{
				return this._typeText;
			}
			set
			{
				if (value != this._typeText)
				{
					this._typeText = value;
					base.OnPropertyChangedWithValue<string>(value, "TypeText");
				}
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x0002396A File Offset: 0x00021B6A
		// (set) Token: 0x06000789 RID: 1929 RVA: 0x00023972 File Offset: 0x00021B72
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x0600078A RID: 1930 RVA: 0x00023995 File Offset: 0x00021B95
		// (set) Token: 0x0600078B RID: 1931 RVA: 0x0002399D File Offset: 0x00021B9D
		[DataSourceProperty]
		public string InfluenceText
		{
			get
			{
				return this._influenceText;
			}
			set
			{
				if (value != this._influenceText)
				{
					this._influenceText = value;
					base.OnPropertyChangedWithValue<string>(value, "InfluenceText");
				}
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x000239C0 File Offset: 0x00021BC0
		// (set) Token: 0x0600078D RID: 1933 RVA: 0x000239C8 File Offset: 0x00021BC8
		[DataSourceProperty]
		public string FiefsText
		{
			get
			{
				return this._fiefsText;
			}
			set
			{
				if (value != this._fiefsText)
				{
					this._fiefsText = value;
					base.OnPropertyChangedWithValue<string>(value, "FiefsText");
				}
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x0600078E RID: 1934 RVA: 0x000239EB File Offset: 0x00021BEB
		// (set) Token: 0x0600078F RID: 1935 RVA: 0x000239F3 File Offset: 0x00021BF3
		[DataSourceProperty]
		public string MembersText
		{
			get
			{
				return this._membersText;
			}
			set
			{
				if (value != this._membersText)
				{
					this._membersText = value;
					base.OnPropertyChangedWithValue<string>(value, "MembersText");
				}
			}
		}

		// Token: 0x04000339 RID: 825
		private Settlement _settlementToGive;

		// Token: 0x0400033A RID: 826
		private Action _onSettlementGranted;

		// Token: 0x0400033B RID: 827
		private InputKeyItemVM _doneInputKey;

		// Token: 0x0400033C RID: 828
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x0400033D RID: 829
		private bool _isAnyClanSelected;

		// Token: 0x0400033E RID: 830
		private MBBindingList<KingdomClanItemVM> _clans;

		// Token: 0x0400033F RID: 831
		private KingdomClanItemVM _currentSelectedClan;

		// Token: 0x04000340 RID: 832
		private KingdomClanSortControllerVM _clanSortController;

		// Token: 0x04000341 RID: 833
		private bool _isOpen;

		// Token: 0x04000342 RID: 834
		private string _titleText;

		// Token: 0x04000343 RID: 835
		private string _giftText;

		// Token: 0x04000344 RID: 836
		private string _cancelText;

		// Token: 0x04000345 RID: 837
		private string _bannerText;

		// Token: 0x04000346 RID: 838
		private string _nameText;

		// Token: 0x04000347 RID: 839
		private string _influenceText;

		// Token: 0x04000348 RID: 840
		private string _membersText;

		// Token: 0x04000349 RID: 841
		private string _fiefsText;

		// Token: 0x0400034A RID: 842
		private string _typeText;
	}
}
