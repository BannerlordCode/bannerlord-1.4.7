using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x0200006E RID: 110
	public class MPLobbyClanLeaderboardVM : ViewModel
	{
		// Token: 0x06000AA1 RID: 2721 RVA: 0x00020B37 File Offset: 0x0001ED37
		public MPLobbyClanLeaderboardVM()
		{
			this.ClanItems = new MBBindingList<MPLobbyClanItemVM>();
			this.RefreshValues();
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x00020B50 File Offset: 0x0001ED50
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CloseText = GameTexts.FindText("str_close", null).ToString();
			this.LeaderboardText = new TextObject("{=vGF5S2hE}Leaderboard", null).ToString();
			this.ClansText = new TextObject("{=bfQLwMUp}Clans", null).ToString();
			this.NameText = new TextObject("{=PDdh1sBj}Name", null).ToString();
			this.GamesWonText = new TextObject("{=dxlkHhw5}Games Won", null).ToString();
			this.GamesLostText = new TextObject("{=BrjpmaJH}Games Lost", null).ToString();
			this.NextText = new TextObject("{=Rvr1bcu8}Next", null).ToString();
			this.PreviousText = new TextObject("{=WXAaWZVf}Previous", null).ToString();
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x00020C14 File Offset: 0x0001EE14
		private async void LoadClanLeaderboard()
		{
			this.IsDataLoading = true;
			ClanLeaderboardInfo clanLeaderboardInfo = await NetworkMain.GameClient.GetClanLeaderboardInfo();
			if (((clanLeaderboardInfo != null) ? clanLeaderboardInfo.ClanEntries : null) != null)
			{
				this._clans = clanLeaderboardInfo.ClanEntries;
			}
			else
			{
				this._clans = new ClanLeaderboardEntry[0];
			}
			this.SortController = new MPLobbyClanLeaderboardSortControllerVM(ref this._clans, new Action(this.OnClansSorted));
			this.GoToPage(0);
			this.IsDataLoading = false;
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x00020C4D File Offset: 0x0001EE4D
		private void OnClansSorted()
		{
			this.GoToPage(0);
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x00020C58 File Offset: 0x0001EE58
		private void GoToPage(int pageNumber)
		{
			int num = pageNumber * 30;
			if (this._clans == null || num > this._clans.Length - 1)
			{
				return;
			}
			this.ClanItems.Clear();
			int num2 = num;
			while (num2 < num + 30 && num2 != this._clans.Length)
			{
				ClanLeaderboardEntry clanLeaderboardEntry = this._clans[num2];
				this.ClanItems.Add(new MPLobbyClanItemVM(clanLeaderboardEntry.Name, clanLeaderboardEntry.Tag, clanLeaderboardEntry.Sigil, clanLeaderboardEntry.WinCount, clanLeaderboardEntry.LossCount, num2 + 1, clanLeaderboardEntry.ClanId.Equals(NetworkMain.GameClient.ClanID)));
				num2++;
			}
			this._currentPageNumber = pageNumber;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x00020CFE File Offset: 0x0001EEFE
		private void ExecuteGoToNextPage()
		{
			if (this._currentPageNumber + 1 <= this._clans.Length / 30)
			{
				this.GoToPage(this._currentPageNumber + 1);
				return;
			}
			this.GoToPage(0);
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x00020D2B File Offset: 0x0001EF2B
		private void ExecuteGoToPreviousPage()
		{
			if (this._currentPageNumber > 0)
			{
				this.GoToPage(this._currentPageNumber - 1);
				return;
			}
			this.GoToPage(this._clans.Length / 30);
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00020D56 File Offset: 0x0001EF56
		public void ExecuteOpenPopup()
		{
			this.IsEnabled = true;
			this.LoadClanLeaderboard();
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00020D65 File Offset: 0x0001EF65
		public void ExecuteClosePopup()
		{
			this.IsEnabled = false;
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000AAA RID: 2730 RVA: 0x00020D6E File Offset: 0x0001EF6E
		// (set) Token: 0x06000AAB RID: 2731 RVA: 0x00020D76 File Offset: 0x0001EF76
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

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000AAC RID: 2732 RVA: 0x00020D94 File Offset: 0x0001EF94
		// (set) Token: 0x06000AAD RID: 2733 RVA: 0x00020D9C File Offset: 0x0001EF9C
		[DataSourceProperty]
		public bool IsDataLoading
		{
			get
			{
				return this._isDataLoading;
			}
			set
			{
				if (value != this._isDataLoading)
				{
					this._isDataLoading = value;
					base.OnPropertyChangedWithValue(value, "IsDataLoading");
				}
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x00020DBA File Offset: 0x0001EFBA
		// (set) Token: 0x06000AAF RID: 2735 RVA: 0x00020DC2 File Offset: 0x0001EFC2
		[DataSourceProperty]
		public string LeaderboardText
		{
			get
			{
				return this._leaderboardText;
			}
			set
			{
				if (value != this._leaderboardText)
				{
					this._leaderboardText = value;
					base.OnPropertyChangedWithValue<string>(value, "LeaderboardText");
				}
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x00020DE5 File Offset: 0x0001EFE5
		// (set) Token: 0x06000AB1 RID: 2737 RVA: 0x00020DED File Offset: 0x0001EFED
		[DataSourceProperty]
		public string ClansText
		{
			get
			{
				return this._clansText;
			}
			set
			{
				if (value != this._clansText)
				{
					this._clansText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClansText");
				}
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x00020E10 File Offset: 0x0001F010
		// (set) Token: 0x06000AB3 RID: 2739 RVA: 0x00020E18 File Offset: 0x0001F018
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

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x00020E3B File Offset: 0x0001F03B
		// (set) Token: 0x06000AB5 RID: 2741 RVA: 0x00020E43 File Offset: 0x0001F043
		[DataSourceProperty]
		public string GamesWonText
		{
			get
			{
				return this._gamesWonText;
			}
			set
			{
				if (value != this._gamesWonText)
				{
					this._gamesWonText = value;
					base.OnPropertyChangedWithValue<string>(value, "GamesWonText");
				}
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x00020E66 File Offset: 0x0001F066
		// (set) Token: 0x06000AB7 RID: 2743 RVA: 0x00020E6E File Offset: 0x0001F06E
		[DataSourceProperty]
		public string GamesLostText
		{
			get
			{
				return this._gamesLostText;
			}
			set
			{
				if (value != this._gamesLostText)
				{
					this._gamesLostText = value;
					base.OnPropertyChangedWithValue<string>(value, "GamesLostText");
				}
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x00020E91 File Offset: 0x0001F091
		// (set) Token: 0x06000AB9 RID: 2745 RVA: 0x00020E99 File Offset: 0x0001F099
		[DataSourceProperty]
		public string NextText
		{
			get
			{
				return this._nextText;
			}
			set
			{
				if (value != this._nextText)
				{
					this._nextText = value;
					base.OnPropertyChangedWithValue<string>(value, "NextText");
				}
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000ABA RID: 2746 RVA: 0x00020EBC File Offset: 0x0001F0BC
		// (set) Token: 0x06000ABB RID: 2747 RVA: 0x00020EC4 File Offset: 0x0001F0C4
		[DataSourceProperty]
		public string PreviousText
		{
			get
			{
				return this._previousText;
			}
			set
			{
				if (value != this._previousText)
				{
					this._previousText = value;
					base.OnPropertyChangedWithValue<string>(value, "PreviousText");
				}
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000ABC RID: 2748 RVA: 0x00020EE7 File Offset: 0x0001F0E7
		// (set) Token: 0x06000ABD RID: 2749 RVA: 0x00020EEF File Offset: 0x0001F0EF
		[DataSourceProperty]
		public string CloseText
		{
			get
			{
				return this._closeText;
			}
			set
			{
				if (value != this._closeText)
				{
					this._closeText = value;
					base.OnPropertyChangedWithValue<string>(value, "CloseText");
				}
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x00020F12 File Offset: 0x0001F112
		// (set) Token: 0x06000ABF RID: 2751 RVA: 0x00020F1A File Offset: 0x0001F11A
		[DataSourceProperty]
		public MBBindingList<MPLobbyClanItemVM> ClanItems
		{
			get
			{
				return this._clanItems;
			}
			set
			{
				if (value != this._clanItems)
				{
					this._clanItems = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyClanItemVM>>(value, "ClanItems");
				}
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x00020F38 File Offset: 0x0001F138
		// (set) Token: 0x06000AC1 RID: 2753 RVA: 0x00020F40 File Offset: 0x0001F140
		[DataSourceProperty]
		public MPLobbyClanLeaderboardSortControllerVM SortController
		{
			get
			{
				return this._sortController;
			}
			set
			{
				if (value != this._sortController)
				{
					this._sortController = value;
					base.OnPropertyChangedWithValue<MPLobbyClanLeaderboardSortControllerVM>(value, "SortController");
				}
			}
		}

		// Token: 0x040004DC RID: 1244
		private ClanLeaderboardEntry[] _clans;

		// Token: 0x040004DD RID: 1245
		private const int _clansPerPage = 30;

		// Token: 0x040004DE RID: 1246
		private int _currentPageNumber;

		// Token: 0x040004DF RID: 1247
		private bool _isEnabled;

		// Token: 0x040004E0 RID: 1248
		private bool _isDataLoading;

		// Token: 0x040004E1 RID: 1249
		private string _leaderboardText;

		// Token: 0x040004E2 RID: 1250
		private string _clansText;

		// Token: 0x040004E3 RID: 1251
		private string _nameText;

		// Token: 0x040004E4 RID: 1252
		private string _gamesWonText;

		// Token: 0x040004E5 RID: 1253
		private string _gamesLostText;

		// Token: 0x040004E6 RID: 1254
		private string _nextText;

		// Token: 0x040004E7 RID: 1255
		private string _previousText;

		// Token: 0x040004E8 RID: 1256
		private string _closeText;

		// Token: 0x040004E9 RID: 1257
		private MBBindingList<MPLobbyClanItemVM> _clanItems;

		// Token: 0x040004EA RID: 1258
		private MPLobbyClanLeaderboardSortControllerVM _sortController;
	}
}
