using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TournamentLeaderboard
{
	// Token: 0x020000AF RID: 175
	public class TournamentLeaderboardVM : ViewModel
	{
		// Token: 0x060010F1 RID: 4337 RVA: 0x00044658 File Offset: 0x00042858
		public TournamentLeaderboardVM()
		{
			this.Entries = new MBBindingList<TournamentLeaderboardEntryItemVM>();
			List<KeyValuePair<Hero, int>> leaderboard = Campaign.Current.TournamentManager.GetLeaderboard();
			for (int i = 0; i < leaderboard.Count; i++)
			{
				this.Entries.Add(new TournamentLeaderboardEntryItemVM(leaderboard[i].Key, leaderboard[i].Value, i + 1));
			}
			this.SortController = new TournamentLeaderboardSortControllerVM(ref this._entries);
			this.RefreshValues();
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x000446E0 File Offset: 0x000428E0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.DoneText = GameTexts.FindText("str_done", null).ToString();
			this.Entries.ApplyActionOnAllItems(delegate(TournamentLeaderboardEntryItemVM x)
			{
				x.RefreshValues();
			});
			this.HeroText = GameTexts.FindText("str_hero", null).ToString();
			this.VictoriesText = GameTexts.FindText("str_leaderboard_victories", null).ToString();
			this.RankText = GameTexts.FindText("str_rank_sign", null).ToString();
			this.TitleText = GameTexts.FindText("str_leaderboard_title", null).ToString();
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x0004478B File Offset: 0x0004298B
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey == null)
			{
				return;
			}
			doneInputKey.OnFinalize();
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x000447A3 File Offset: 0x000429A3
		public void ExecuteDone()
		{
			this.IsEnabled = false;
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x000447AC File Offset: 0x000429AC
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x060010F6 RID: 4342 RVA: 0x000447BB File Offset: 0x000429BB
		// (set) Token: 0x060010F7 RID: 4343 RVA: 0x000447C3 File Offset: 0x000429C3
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

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x060010F8 RID: 4344 RVA: 0x000447E1 File Offset: 0x000429E1
		// (set) Token: 0x060010F9 RID: 4345 RVA: 0x000447E9 File Offset: 0x000429E9
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

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x060010FA RID: 4346 RVA: 0x00044807 File Offset: 0x00042A07
		// (set) Token: 0x060010FB RID: 4347 RVA: 0x0004480F File Offset: 0x00042A0F
		[DataSourceProperty]
		public TournamentLeaderboardSortControllerVM SortController
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
					base.OnPropertyChangedWithValue<TournamentLeaderboardSortControllerVM>(value, "SortController");
				}
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x060010FC RID: 4348 RVA: 0x0004482D File Offset: 0x00042A2D
		// (set) Token: 0x060010FD RID: 4349 RVA: 0x00044835 File Offset: 0x00042A35
		[DataSourceProperty]
		public MBBindingList<TournamentLeaderboardEntryItemVM> Entries
		{
			get
			{
				return this._entries;
			}
			set
			{
				if (value != this._entries)
				{
					this._entries = value;
					base.OnPropertyChangedWithValue<MBBindingList<TournamentLeaderboardEntryItemVM>>(value, "Entries");
				}
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x060010FE RID: 4350 RVA: 0x00044853 File Offset: 0x00042A53
		// (set) Token: 0x060010FF RID: 4351 RVA: 0x0004485B File Offset: 0x00042A5B
		[DataSourceProperty]
		public string DoneText
		{
			get
			{
				return this._doneText;
			}
			set
			{
				if (value != this._doneText)
				{
					this._doneText = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneText");
				}
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001100 RID: 4352 RVA: 0x0004487E File Offset: 0x00042A7E
		// (set) Token: 0x06001101 RID: 4353 RVA: 0x00044886 File Offset: 0x00042A86
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

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001102 RID: 4354 RVA: 0x000448A9 File Offset: 0x00042AA9
		// (set) Token: 0x06001103 RID: 4355 RVA: 0x000448B1 File Offset: 0x00042AB1
		[DataSourceProperty]
		public string HeroText
		{
			get
			{
				return this._heroText;
			}
			set
			{
				if (value != this._heroText)
				{
					this._heroText = value;
					base.OnPropertyChangedWithValue<string>(value, "HeroText");
				}
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001104 RID: 4356 RVA: 0x000448D4 File Offset: 0x00042AD4
		// (set) Token: 0x06001105 RID: 4357 RVA: 0x000448DC File Offset: 0x00042ADC
		[DataSourceProperty]
		public string VictoriesText
		{
			get
			{
				return this._victoriesText;
			}
			set
			{
				if (value != this._victoriesText)
				{
					this._victoriesText = value;
					base.OnPropertyChangedWithValue<string>(value, "VictoriesText");
				}
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001106 RID: 4358 RVA: 0x000448FF File Offset: 0x00042AFF
		// (set) Token: 0x06001107 RID: 4359 RVA: 0x00044907 File Offset: 0x00042B07
		[DataSourceProperty]
		public string RankText
		{
			get
			{
				return this._rankText;
			}
			set
			{
				if (value != this._rankText)
				{
					this._rankText = value;
					base.OnPropertyChangedWithValue<string>(value, "RankText");
				}
			}
		}

		// Token: 0x040007BE RID: 1982
		private InputKeyItemVM _doneInputKey;

		// Token: 0x040007BF RID: 1983
		private bool _isEnabled;

		// Token: 0x040007C0 RID: 1984
		private string _doneText;

		// Token: 0x040007C1 RID: 1985
		private string _heroText;

		// Token: 0x040007C2 RID: 1986
		private string _victoriesText;

		// Token: 0x040007C3 RID: 1987
		private string _rankText;

		// Token: 0x040007C4 RID: 1988
		private string _titleText;

		// Token: 0x040007C5 RID: 1989
		private MBBindingList<TournamentLeaderboardEntryItemVM> _entries;

		// Token: 0x040007C6 RID: 1990
		private TournamentLeaderboardSortControllerVM _sortController;
	}
}
