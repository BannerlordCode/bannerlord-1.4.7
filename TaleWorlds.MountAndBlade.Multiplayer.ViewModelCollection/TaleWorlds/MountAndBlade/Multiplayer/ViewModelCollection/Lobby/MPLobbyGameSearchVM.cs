using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.CustomGame;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x0200002A RID: 42
	public class MPLobbyGameSearchVM : ViewModel
	{
		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600030F RID: 783 RVA: 0x0000BE29 File Offset: 0x0000A029
		// (set) Token: 0x06000310 RID: 784 RVA: 0x0000BE31 File Offset: 0x0000A031
		public MPCustomGameVM.CustomGameMode CustomGameMode { get; private set; }

		// Token: 0x06000311 RID: 785 RVA: 0x0000BE3A File Offset: 0x0000A03A
		public MPLobbyGameSearchVM()
		{
			this.GameTypesText = new TextObject("{=cK5DE88I}N/A", null).ToString();
			this.RefreshValues();
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000BE74 File Offset: 0x0000A074
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.CustomGameMode == MPCustomGameVM.CustomGameMode.PremadeGame)
			{
				this.TitleText = new TextObject("{=dkPL25g9}Waiting for an opponent team", null).ToString();
				this.GameTypesText = "";
				this.ShowStats = false;
			}
			else
			{
				this.TitleText = new TextObject("{=FD7EQDmW}Looking for game", null).ToString();
				this.ShowStats = true;
			}
			GameTexts.SetVariable("STR1", "");
			GameTexts.SetVariable("STR2", new TextObject("{=mFMPj9zg}Searching for matches", null));
			this.CurrentWaitingTimeDescription = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			GameTexts.SetVariable("STR2", new TextObject("{=18yFEEIL}Estimated wait time", null));
			this.AverageWaitingTimeDescription = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			this.CancelText = new TextObject("{=3CpNUnVl}Cancel", null).ToString();
			this.PracticeText = new TextObject("{=cjBboOaH}Practice while waiting", null).ToString();
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000BF68 File Offset: 0x0000A168
		public void OnTick(float dt)
		{
			if (this.IsEnabled)
			{
				this._waitingTimeElapsed += dt;
				this.CurrentWaitingTime = this.SecondsToString(this._waitingTimeElapsed);
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000BF92 File Offset: 0x0000A192
		public void SetEnabled(bool enabled)
		{
			this.IsEnabled = enabled;
			if (enabled)
			{
				this.CanCancelSearch = true;
				this.CanEnterPracticeBattle = false;
				this._waitingTimeElapsed = 0f;
			}
			this.RefreshValues();
			if (this.CustomGameMode != MPCustomGameVM.CustomGameMode.PremadeGame)
			{
				this.UpdateCanCancel();
			}
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000BFCC File Offset: 0x0000A1CC
		public void UpdateData(MatchmakingWaitTimeStats matchmakingWaitTimeStats, string[] gameTypeInfo)
		{
			this.ShowStats = true;
			this.CustomGameMode = MPCustomGameVM.CustomGameMode.CustomServer;
			this.TitleText = new TextObject("{=FD7EQDmW}Looking for game", null).ToString();
			int num = 0;
			string[] array = this.GameTypesText.Replace(" ", "").Split(new char[] { ',' });
			foreach (string text in array)
			{
				WaitTimeStatType waitTimeStatType = WaitTimeStatType.SoloDuo;
				if (NetworkMain.GameClient.PlayersInParty.Count >= 3 && NetworkMain.GameClient.PlayersInParty.Count <= 5)
				{
					waitTimeStatType = WaitTimeStatType.Party;
				}
				else if (NetworkMain.GameClient.IsPartyFull)
				{
					waitTimeStatType = WaitTimeStatType.Premade;
				}
				num += matchmakingWaitTimeStats.GetWaitTime(MultiplayerMain.GetUserCurrentRegion(), text, waitTimeStatType);
			}
			this.AverageWaitingTime = this.SecondsToString((float)(num / array.Length));
			if (gameTypeInfo != null)
			{
				this.GameTypesText = MPLobbyVM.GetLocalizedGameTypesString(gameTypeInfo);
			}
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000C0A8 File Offset: 0x0000A2A8
		public void UpdatePremadeGameData()
		{
			this.ShowStats = false;
			this.CustomGameMode = MPCustomGameVM.CustomGameMode.PremadeGame;
			this.TitleText = new TextObject("{=dkPL25g9}Waiting for an opponent team", null).ToString();
			this.GameTypesText = "";
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000C0D9 File Offset: 0x0000A2D9
		public void OnJoinPremadeGameRequestSuccessful()
		{
			this.TitleText = new TextObject("{=5coyTZOI}Game is starting!", null).ToString();
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000C0F1 File Offset: 0x0000A2F1
		public void OnRequestedToCancelSearchBattle()
		{
			this.CanCancelSearch = false;
			this.CanEnterPracticeBattle = false;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000C101 File Offset: 0x0000A301
		public void UpdateCanCancel()
		{
			this.CanCancelSearch = !NetworkMain.GameClient.IsInParty || NetworkMain.GameClient.IsPartyLeader;
			this.CanEnterPracticeBattle = false;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000C129 File Offset: 0x0000A329
		private void ExecuteCancel()
		{
			if (this.CustomGameMode != MPCustomGameVM.CustomGameMode.PremadeGame)
			{
				NetworkMain.GameClient.CancelFindGame();
				return;
			}
			NetworkMain.GameClient.CancelCreatingPremadeGame();
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000C14C File Offset: 0x0000A34C
		private string SecondsToString(float seconds)
		{
			return TimeSpan.FromSeconds((double)seconds).ToString((seconds >= 3600f) ? this._longTimeTextFormat : this._shortTimeTextFormat);
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600031C RID: 796 RVA: 0x0000C17E File Offset: 0x0000A37E
		// (set) Token: 0x0600031D RID: 797 RVA: 0x0000C186 File Offset: 0x0000A386
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

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600031E RID: 798 RVA: 0x0000C1A4 File Offset: 0x0000A3A4
		// (set) Token: 0x0600031F RID: 799 RVA: 0x0000C1AC File Offset: 0x0000A3AC
		[DataSourceProperty]
		public bool CanEnterPracticeBattle
		{
			get
			{
				return this._canEnterPracticeBattle;
			}
			set
			{
				if (value != this._canEnterPracticeBattle)
				{
					this._canEnterPracticeBattle = value;
					base.OnPropertyChangedWithValue(value, "CanEnterPracticeBattle");
				}
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000320 RID: 800 RVA: 0x0000C1CA File Offset: 0x0000A3CA
		// (set) Token: 0x06000321 RID: 801 RVA: 0x0000C1D2 File Offset: 0x0000A3D2
		[DataSourceProperty]
		public bool CanCancelSearch
		{
			get
			{
				return this._canCancelSearch;
			}
			set
			{
				if (value != this._canCancelSearch)
				{
					this._canCancelSearch = value;
					base.OnPropertyChangedWithValue(value, "CanCancelSearch");
				}
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0000C1F0 File Offset: 0x0000A3F0
		// (set) Token: 0x06000323 RID: 803 RVA: 0x0000C1F8 File Offset: 0x0000A3F8
		[DataSourceProperty]
		public bool ShowStats
		{
			get
			{
				return this._showStats;
			}
			set
			{
				if (value != this._showStats)
				{
					this._showStats = value;
					base.OnPropertyChangedWithValue(value, "ShowStats");
				}
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000324 RID: 804 RVA: 0x0000C216 File Offset: 0x0000A416
		// (set) Token: 0x06000325 RID: 805 RVA: 0x0000C21E File Offset: 0x0000A41E
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

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000326 RID: 806 RVA: 0x0000C241 File Offset: 0x0000A441
		// (set) Token: 0x06000327 RID: 807 RVA: 0x0000C249 File Offset: 0x0000A449
		[DataSourceProperty]
		public string GameTypesText
		{
			get
			{
				return this._gameTypesText;
			}
			set
			{
				if (value != this._gameTypesText)
				{
					this._gameTypesText = value;
					base.OnPropertyChangedWithValue<string>(value, "GameTypesText");
				}
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000328 RID: 808 RVA: 0x0000C26C File Offset: 0x0000A46C
		// (set) Token: 0x06000329 RID: 809 RVA: 0x0000C274 File Offset: 0x0000A474
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

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600032A RID: 810 RVA: 0x0000C297 File Offset: 0x0000A497
		// (set) Token: 0x0600032B RID: 811 RVA: 0x0000C29F File Offset: 0x0000A49F
		[DataSourceProperty]
		public string PracticeText
		{
			get
			{
				return this._practiceText;
			}
			set
			{
				if (value != this._practiceText)
				{
					this._practiceText = value;
					base.OnPropertyChangedWithValue<string>(value, "PracticeText");
				}
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600032C RID: 812 RVA: 0x0000C2C2 File Offset: 0x0000A4C2
		// (set) Token: 0x0600032D RID: 813 RVA: 0x0000C2CA File Offset: 0x0000A4CA
		[DataSourceProperty]
		public string AverageWaitingTime
		{
			get
			{
				return this._averageWaitingTime;
			}
			set
			{
				if (value != this._averageWaitingTime)
				{
					this._averageWaitingTime = value;
					base.OnPropertyChangedWithValue<string>(value, "AverageWaitingTime");
				}
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600032E RID: 814 RVA: 0x0000C2ED File Offset: 0x0000A4ED
		// (set) Token: 0x0600032F RID: 815 RVA: 0x0000C2F5 File Offset: 0x0000A4F5
		[DataSourceProperty]
		public string AverageWaitingTimeDescription
		{
			get
			{
				return this._averageWaitingTimeDescription;
			}
			set
			{
				if (value != this._averageWaitingTimeDescription)
				{
					this._averageWaitingTimeDescription = value;
					base.OnPropertyChangedWithValue<string>(value, "AverageWaitingTimeDescription");
				}
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000330 RID: 816 RVA: 0x0000C318 File Offset: 0x0000A518
		// (set) Token: 0x06000331 RID: 817 RVA: 0x0000C320 File Offset: 0x0000A520
		[DataSourceProperty]
		public string CurrentWaitingTime
		{
			get
			{
				return this._currentWaitingTime;
			}
			set
			{
				if (value != this._currentWaitingTime)
				{
					this._currentWaitingTime = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentWaitingTime");
				}
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000332 RID: 818 RVA: 0x0000C343 File Offset: 0x0000A543
		// (set) Token: 0x06000333 RID: 819 RVA: 0x0000C34B File Offset: 0x0000A54B
		[DataSourceProperty]
		public string CurrentWaitingTimeDescription
		{
			get
			{
				return this._currentWaitingTimeDescription;
			}
			set
			{
				if (value != this._currentWaitingTimeDescription)
				{
					this._currentWaitingTimeDescription = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentWaitingTimeDescription");
				}
			}
		}

		// Token: 0x04000198 RID: 408
		private float _waitingTimeElapsed;

		// Token: 0x04000199 RID: 409
		private string _shortTimeTextFormat = "mm\\:ss";

		// Token: 0x0400019A RID: 410
		private string _longTimeTextFormat = "hh\\:mm\\:ss";

		// Token: 0x0400019B RID: 411
		private bool _isEnabled;

		// Token: 0x0400019C RID: 412
		private bool _canCancelSearch;

		// Token: 0x0400019D RID: 413
		private bool _canEnterPracticeBattle;

		// Token: 0x0400019E RID: 414
		private bool _showStats;

		// Token: 0x0400019F RID: 415
		private string _titleText;

		// Token: 0x040001A0 RID: 416
		private string _gameTypesText;

		// Token: 0x040001A1 RID: 417
		private string _cancelText;

		// Token: 0x040001A2 RID: 418
		private string _practiceText;

		// Token: 0x040001A3 RID: 419
		private string _averageWaitingTime;

		// Token: 0x040001A4 RID: 420
		private string _averageWaitingTimeDescription;

		// Token: 0x040001A5 RID: 421
		private string _currentWaitingTime;

		// Token: 0x040001A6 RID: 422
		private string _currentWaitingTimeDescription;
	}
}
