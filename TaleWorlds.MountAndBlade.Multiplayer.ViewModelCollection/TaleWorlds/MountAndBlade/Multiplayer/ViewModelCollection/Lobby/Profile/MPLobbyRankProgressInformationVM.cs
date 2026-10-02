using System;
using System.Linq;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond.Ranked;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x0200003C RID: 60
	public class MPLobbyRankProgressInformationVM : ViewModel
	{
		// Token: 0x060005B2 RID: 1458 RVA: 0x000130C4 File Offset: 0x000112C4
		public MPLobbyRankProgressInformationVM(Func<string> getExitText)
		{
			this._getExitText = getExitText;
			this.AllRanks = new MBBindingList<StringPairItemVM>();
			this.RefreshValues();
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00013124 File Offset: 0x00011324
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=XEGaQB2G}Rank Progression", null).ToString();
			this.AllRanks.Clear();
			string[] rankIds = Ranks.RankIds;
			for (int i = 0; i < rankIds.Length; i++)
			{
				string rank = rankIds[i];
				this.AllRanks.Add(new StringPairItemVM(rank, string.Empty, new BasicTooltipViewModel(() => MPLobbyVM.GetLocalizedRankName(rank))));
			}
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x000131A7 File Offset: 0x000113A7
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.ExecuteClosePopup();
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x000131B8 File Offset: 0x000113B8
		public void OpenWith(MPLobbyPlayerBaseVM player)
		{
			this.IsEnabled = true;
			Func<string> getExitText = this._getExitText;
			this.ClickToCloseText = ((getExitText != null) ? getExitText() : null);
			if (player.RankInfo == null)
			{
				Debug.FailedAssert("Can't request rank progression information of another player.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Profile\\MPLobbyRankProgressInformationVM.cs", "OpenWith", 54);
				return;
			}
			this._basePlayer = player;
			this.Player = new MPLobbyPlayerBaseVM(player.ProvidedID, "", null, null);
			this.Player.UpdateRating(new Action(this.OnRatingReceived));
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001323C File Offset: 0x0001143C
		private void OnRatingReceived()
		{
			MPLobbyPlayerBaseVM player = this.Player;
			if (player == null)
			{
				return;
			}
			bool flag = true;
			Action<string> action = new Action<string>(this.RefreshRankInfo);
			MPLobbyGameTypeVM mplobbyGameTypeVM = this._basePlayer.GameTypes.FirstOrDefault<MPLobbyGameTypeVM>((MPLobbyGameTypeVM gt) => gt.IsSelected);
			player.RefreshSelectableGameTypes(flag, action, (mplobbyGameTypeVM != null) ? mplobbyGameTypeVM.GameTypeID : null);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x000132A4 File Offset: 0x000114A4
		private void RefreshRankInfo(string gameType)
		{
			GameTypeRankInfo[] rankInfo = this.Player.RankInfo;
			GameTypeRankInfo gameTypeRankInfo = ((rankInfo != null) ? rankInfo.FirstOrDefault<GameTypeRankInfo>((GameTypeRankInfo r) => r.GameType == gameType) : null);
			if (gameTypeRankInfo == null || gameTypeRankInfo.RankBarInfo == null)
			{
				this.IsEnabled = false;
				return;
			}
			RankBarInfo rankBarInfo = gameTypeRankInfo.RankBarInfo;
			this.CurrentRankID = rankBarInfo.RankId;
			this.CurrentRankTitleText = MPLobbyVM.GetLocalizedRankName(this.CurrentRankID);
			this.CurrentRating = rankBarInfo.Rating;
			this.NextRankRating = this.CurrentRating + rankBarInfo.RatingToNextRank;
			this.AllRanks.ApplyActionOnAllItems(delegate(StringPairItemVM r)
			{
				r.Value = " ";
			});
			StringPairItemVM stringPairItemVM = this.AllRanks.FirstOrDefault<StringPairItemVM>((StringPairItemVM r) => r.Definition == this.CurrentRankID);
			if (stringPairItemVM != null)
			{
				stringPairItemVM.Value = new TextObject("{=sWnQva5O}Current Rank", null).ToString();
			}
			this.IsAtFinalRank = string.IsNullOrEmpty(rankBarInfo.NextRankId);
			this.IsEvaluating = rankBarInfo.IsEvaluating;
			if (rankBarInfo.IsEvaluating)
			{
				this.RatingRatio = MathF.Floor((float)rankBarInfo.EvaluationMatchesPlayed / (float)rankBarInfo.TotalEvaluationMatchesRequired * 100f);
				this.NextRankID = string.Empty;
				this.PreviousRankID = string.Empty;
				this.CurrentRating = rankBarInfo.EvaluationMatchesPlayed;
				this.NextRankRating = rankBarInfo.TotalEvaluationMatchesRequired;
				this._evaluationTextObject.SetTextVariable("PLAYED_GAMES", rankBarInfo.EvaluationMatchesPlayed);
				this._evaluationTextObject.SetTextVariable("TOTAL_GAMES", rankBarInfo.TotalEvaluationMatchesRequired);
				this.RatingRemainingTitleText = this._evaluationTextObject.ToString();
				return;
			}
			if (this.IsAtFinalRank)
			{
				this.RatingRatio = 100;
				this.NextRankID = string.Empty;
				this.PreviousRankID = string.Empty;
				this.RatingRemainingTitleText = this._finalRankTextObject.ToString();
				return;
			}
			this.RatingRatio = MathF.Floor(rankBarInfo.ProgressPercentage);
			this.NextRankID = rankBarInfo.NextRankId;
			this.PreviousRankID = rankBarInfo.PreviousRankId;
			this._ratingRemainingTitleTextObject.SetTextVariable("RATING", rankBarInfo.RatingToNextRank);
			this.RatingRemainingTitleText = this._ratingRemainingTitleTextObject.ToString();
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x000134DF File Offset: 0x000116DF
		public void ExecuteClosePopup()
		{
			this.Player = null;
			this.IsEnabled = false;
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x000134EF File Offset: 0x000116EF
		// (set) Token: 0x060005BA RID: 1466 RVA: 0x000134F7 File Offset: 0x000116F7
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

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x00013515 File Offset: 0x00011715
		// (set) Token: 0x060005BC RID: 1468 RVA: 0x0001351D File Offset: 0x0001171D
		[DataSourceProperty]
		public bool IsAtFinalRank
		{
			get
			{
				return this._isAtFinalRank;
			}
			set
			{
				if (value != this._isAtFinalRank)
				{
					this._isAtFinalRank = value;
					base.OnPropertyChangedWithValue(value, "IsAtFinalRank");
				}
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x0001353B File Offset: 0x0001173B
		// (set) Token: 0x060005BE RID: 1470 RVA: 0x00013543 File Offset: 0x00011743
		[DataSourceProperty]
		public bool IsEvaluating
		{
			get
			{
				return this._isEvaluating;
			}
			set
			{
				if (value != this._isEvaluating)
				{
					this._isEvaluating = value;
					base.OnPropertyChangedWithValue(value, "IsEvaluating");
				}
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x00013561 File Offset: 0x00011761
		// (set) Token: 0x060005C0 RID: 1472 RVA: 0x00013569 File Offset: 0x00011769
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

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x0001358C File Offset: 0x0001178C
		// (set) Token: 0x060005C2 RID: 1474 RVA: 0x00013594 File Offset: 0x00011794
		[DataSourceProperty]
		public string ClickToCloseText
		{
			get
			{
				return this._clickToCloseText;
			}
			set
			{
				if (value != this._clickToCloseText)
				{
					this._clickToCloseText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClickToCloseText");
				}
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x000135B7 File Offset: 0x000117B7
		// (set) Token: 0x060005C4 RID: 1476 RVA: 0x000135BF File Offset: 0x000117BF
		[DataSourceProperty]
		public string CurrentRankTitleText
		{
			get
			{
				return this._currentRankTitleText;
			}
			set
			{
				if (value != this._currentRankTitleText)
				{
					this._currentRankTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentRankTitleText");
				}
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x000135E2 File Offset: 0x000117E2
		// (set) Token: 0x060005C6 RID: 1478 RVA: 0x000135EA File Offset: 0x000117EA
		[DataSourceProperty]
		public string RatingRemainingTitleText
		{
			get
			{
				return this._ratingRemainingTitleText;
			}
			set
			{
				if (value != this._ratingRemainingTitleText)
				{
					this._ratingRemainingTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "RatingRemainingTitleText");
				}
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x0001360D File Offset: 0x0001180D
		// (set) Token: 0x060005C8 RID: 1480 RVA: 0x00013615 File Offset: 0x00011815
		[DataSourceProperty]
		public string CurrentRankID
		{
			get
			{
				return this._currentRankID;
			}
			set
			{
				if (value != this._currentRankID)
				{
					this._currentRankID = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentRankID");
				}
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x00013638 File Offset: 0x00011838
		// (set) Token: 0x060005CA RID: 1482 RVA: 0x00013640 File Offset: 0x00011840
		[DataSourceProperty]
		public string PreviousRankID
		{
			get
			{
				return this._previousRankID;
			}
			set
			{
				if (value != this._previousRankID)
				{
					this._previousRankID = value;
					base.OnPropertyChangedWithValue<string>(value, "PreviousRankID");
				}
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x00013663 File Offset: 0x00011863
		// (set) Token: 0x060005CC RID: 1484 RVA: 0x0001366B File Offset: 0x0001186B
		[DataSourceProperty]
		public string NextRankID
		{
			get
			{
				return this._nextRankID;
			}
			set
			{
				if (value != this._nextRankID)
				{
					this._nextRankID = value;
					base.OnPropertyChangedWithValue<string>(value, "NextRankID");
				}
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x0001368E File Offset: 0x0001188E
		// (set) Token: 0x060005CE RID: 1486 RVA: 0x00013696 File Offset: 0x00011896
		[DataSourceProperty]
		public int CurrentRating
		{
			get
			{
				return this._currentRating;
			}
			set
			{
				if (value != this._currentRating)
				{
					this._currentRating = value;
					base.OnPropertyChangedWithValue(value, "CurrentRating");
				}
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x000136B4 File Offset: 0x000118B4
		// (set) Token: 0x060005D0 RID: 1488 RVA: 0x000136BC File Offset: 0x000118BC
		[DataSourceProperty]
		public int NextRankRating
		{
			get
			{
				return this._nextRankRating;
			}
			set
			{
				if (value != this._nextRankRating)
				{
					this._nextRankRating = value;
					base.OnPropertyChangedWithValue(value, "NextRankRating");
				}
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x000136DA File Offset: 0x000118DA
		// (set) Token: 0x060005D2 RID: 1490 RVA: 0x000136E2 File Offset: 0x000118E2
		[DataSourceProperty]
		public int RatingRatio
		{
			get
			{
				return this._ratingRatio;
			}
			set
			{
				if (value != this._ratingRatio)
				{
					this._ratingRatio = value;
					base.OnPropertyChangedWithValue(value, "RatingRatio");
				}
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x00013700 File Offset: 0x00011900
		// (set) Token: 0x060005D4 RID: 1492 RVA: 0x00013708 File Offset: 0x00011908
		[DataSourceProperty]
		public MPLobbyPlayerBaseVM Player
		{
			get
			{
				return this._player;
			}
			set
			{
				if (value != this._player)
				{
					this._player = value;
					base.OnPropertyChangedWithValue<MPLobbyPlayerBaseVM>(value, "Player");
				}
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x00013726 File Offset: 0x00011926
		// (set) Token: 0x060005D6 RID: 1494 RVA: 0x0001372E File Offset: 0x0001192E
		[DataSourceProperty]
		public MBBindingList<StringPairItemVM> AllRanks
		{
			get
			{
				return this._allRanks;
			}
			set
			{
				if (value != this._allRanks)
				{
					this._allRanks = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemVM>>(value, "AllRanks");
				}
			}
		}

		// Token: 0x040002AF RID: 687
		private MPLobbyPlayerBaseVM _basePlayer;

		// Token: 0x040002B0 RID: 688
		private readonly Func<string> _getExitText;

		// Token: 0x040002B1 RID: 689
		private TextObject _ratingRemainingTitleTextObject = new TextObject("{=7gQkFJqA}{RATING} points remaining to next rank", null);

		// Token: 0x040002B2 RID: 690
		private TextObject _finalRankTextObject = new TextObject("{=6mZymVS8}You are at the final rank", null);

		// Token: 0x040002B3 RID: 691
		private TextObject _evaluationTextObject = new TextObject("{=Ise5gWw3}{PLAYED_GAMES} / {TOTAL_GAMES} Evaluation matches played", null);

		// Token: 0x040002B4 RID: 692
		private bool _isEnabled;

		// Token: 0x040002B5 RID: 693
		private bool _isAtFinalRank;

		// Token: 0x040002B6 RID: 694
		private bool _isEvaluating;

		// Token: 0x040002B7 RID: 695
		private string _titleText;

		// Token: 0x040002B8 RID: 696
		private string _clickToCloseText;

		// Token: 0x040002B9 RID: 697
		private string _currentRankTitleText;

		// Token: 0x040002BA RID: 698
		private string _ratingRemainingTitleText;

		// Token: 0x040002BB RID: 699
		private string _currentRankID;

		// Token: 0x040002BC RID: 700
		private string _previousRankID;

		// Token: 0x040002BD RID: 701
		private string _nextRankID;

		// Token: 0x040002BE RID: 702
		private int _currentRating;

		// Token: 0x040002BF RID: 703
		private int _nextRankRating;

		// Token: 0x040002C0 RID: 704
		private int _ratingRatio;

		// Token: 0x040002C1 RID: 705
		private MPLobbyPlayerBaseVM _player;

		// Token: 0x040002C2 RID: 706
		private MBBindingList<StringPairItemVM> _allRanks;
	}
}
