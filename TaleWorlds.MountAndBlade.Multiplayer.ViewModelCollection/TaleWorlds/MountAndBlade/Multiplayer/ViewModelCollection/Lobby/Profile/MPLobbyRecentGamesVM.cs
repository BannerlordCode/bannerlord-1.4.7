using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x0200003F RID: 63
	public class MPLobbyRecentGamesVM : ViewModel
	{
		// Token: 0x06000609 RID: 1545 RVA: 0x00013F6A File Offset: 0x0001216A
		public MPLobbyRecentGamesVM()
		{
			this._games = new MBBindingList<MPLobbyRecentGameItemVM>();
			this.PlayerActions = new MBBindingList<StringPairItemWithActionVM>();
			this.NoRecentGamesFoundText = new TextObject("{=TzYWE9tA}No Recent Games Found", null).ToString();
			this.RefreshValues();
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00013FA4 File Offset: 0x000121A4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RecentGamesText = new TextObject("{=NJolh9ye}Recent Games", null).ToString();
			this.CloseText = GameTexts.FindText("str_close", null).ToString();
			this.Games.ApplyActionOnAllItems(delegate(MPLobbyRecentGameItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00014010 File Offset: 0x00012210
		public void RefreshData(MBReadOnlyList<MatchHistoryData> matches)
		{
			this.Games.Clear();
			if (matches != null)
			{
				foreach (MatchHistoryData matchHistoryData in matches.OrderByDescending<MatchHistoryData, DateTime>((MatchHistoryData m) => m.MatchDate))
				{
					if (matchHistoryData != null)
					{
						MPLobbyRecentGameItemVM mplobbyRecentGameItemVM = new MPLobbyRecentGameItemVM(new Action<MPLobbyRecentGamePlayerItemVM>(this.ActivatePlayerActions));
						mplobbyRecentGameItemVM.FillFrom(matchHistoryData);
						this.Games.Add(mplobbyRecentGameItemVM);
					}
				}
			}
			this.GotItems = matches.Count > 0;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x000140BC File Offset: 0x000122BC
		public void ActivatePlayerActions(MPLobbyRecentGamePlayerItemVM playerVM)
		{
			this.PlayerActions.Clear();
			this._currentMatchOfTheActivePlayer = playerVM.MatchOfThePlayer;
			if (playerVM.ProvidedID != NetworkMain.GameClient.PlayerID)
			{
				StringPairItemWithActionVM stringPairItemWithActionVM = new StringPairItemWithActionVM(new Action<object>(this.ExecuteReport), GameTexts.FindText("str_mp_scoreboard_context_report", null).ToString(), "Report", playerVM);
				if (MultiplayerReportPlayerManager.IsPlayerReportedOverLimit(playerVM.ProvidedID))
				{
					stringPairItemWithActionVM.IsEnabled = false;
					stringPairItemWithActionVM.Hint.HintText = new TextObject("{=klkYFik9}You've already reported this player.", null);
				}
				this.PlayerActions.Add(stringPairItemWithActionVM);
				bool flag = false;
				FriendInfo[] friendInfos = NetworkMain.GameClient.FriendInfos;
				for (int i = 0; i < friendInfos.Length; i++)
				{
					if (friendInfos[i].Id == playerVM.ProvidedID)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.PlayerActions.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteRequestFriendship), new TextObject("{=UwkpJq9N}Add As Friend", null).ToString(), "RequestFriendship", playerVM));
				}
				else
				{
					this.PlayerActions.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteTerminateFriendship), new TextObject("{=2YIVRuRa}Remove From Friends", null).ToString(), "TerminateFriendship", playerVM));
				}
				MultiplayerPlayerContextMenuHelper.AddLobbyViewProfileOptions(playerVM, this.PlayerActions);
			}
			this.IsPlayerActionsActive = false;
			this.IsPlayerActionsActive = this.PlayerActions.Count > 0;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0001421C File Offset: 0x0001241C
		private void ExecuteRequestFriendship(object playerObj)
		{
			MPLobbyRecentGamePlayerItemVM mplobbyRecentGamePlayerItemVM = playerObj as MPLobbyRecentGamePlayerItemVM;
			bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(mplobbyRecentGamePlayerItemVM.ProvidedID);
			NetworkMain.GameClient.AddFriend(mplobbyRecentGamePlayerItemVM.ProvidedID, flag);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00014260 File Offset: 0x00012460
		private void ExecuteTerminateFriendship(object memberObj)
		{
			MPLobbyRecentGamePlayerItemVM mplobbyRecentGamePlayerItemVM = memberObj as MPLobbyRecentGamePlayerItemVM;
			NetworkMain.GameClient.RemoveFriend(mplobbyRecentGamePlayerItemVM.ProvidedID);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00014284 File Offset: 0x00012484
		private void ExecuteReport(object playerObj)
		{
			MPLobbyRecentGamePlayerItemVM mplobbyRecentGamePlayerItemVM = playerObj as MPLobbyRecentGamePlayerItemVM;
			MultiplayerReportPlayerManager.RequestReportPlayer(this._currentMatchOfTheActivePlayer.MatchId, mplobbyRecentGamePlayerItemVM.ProvidedID, mplobbyRecentGamePlayerItemVM.Name, false);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x000142B5 File Offset: 0x000124B5
		public void ExecuteOpenPopup()
		{
			this.IsEnabled = true;
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x000142BE File Offset: 0x000124BE
		public void ExecuteClosePopup()
		{
			this.IsEnabled = false;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x000142C8 File Offset: 0x000124C8
		public void OnFriendListUpdated(bool forceUpdate = false)
		{
			foreach (MPLobbyRecentGameItemVM mplobbyRecentGameItemVM in this.Games)
			{
				mplobbyRecentGameItemVM.OnFriendListUpdated(forceUpdate);
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x00014314 File Offset: 0x00012514
		// (set) Token: 0x06000614 RID: 1556 RVA: 0x0001431C File Offset: 0x0001251C
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

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x0001433A File Offset: 0x0001253A
		// (set) Token: 0x06000616 RID: 1558 RVA: 0x00014342 File Offset: 0x00012542
		[DataSourceProperty]
		public bool GotItems
		{
			get
			{
				return this._gotItems;
			}
			set
			{
				if (value != this._gotItems)
				{
					this._gotItems = value;
					base.OnPropertyChangedWithValue(value, "GotItems");
				}
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x00014360 File Offset: 0x00012560
		// (set) Token: 0x06000618 RID: 1560 RVA: 0x00014368 File Offset: 0x00012568
		[DataSourceProperty]
		public bool IsPlayerActionsActive
		{
			get
			{
				return this._isPlayerActionsActive;
			}
			set
			{
				if (value != this._isPlayerActionsActive)
				{
					this._isPlayerActionsActive = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerActionsActive");
				}
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x00014386 File Offset: 0x00012586
		// (set) Token: 0x0600061A RID: 1562 RVA: 0x0001438E File Offset: 0x0001258E
		[DataSourceProperty]
		public string RecentGamesText
		{
			get
			{
				return this._recentGamesText;
			}
			set
			{
				if (value != this._recentGamesText)
				{
					this._recentGamesText = value;
					base.OnPropertyChangedWithValue<string>(value, "RecentGamesText");
				}
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x000143B1 File Offset: 0x000125B1
		// (set) Token: 0x0600061C RID: 1564 RVA: 0x000143B9 File Offset: 0x000125B9
		[DataSourceProperty]
		public string NoRecentGamesFoundText
		{
			get
			{
				return this._noRecentGamesFoundText;
			}
			set
			{
				if (value != this._noRecentGamesFoundText)
				{
					this._noRecentGamesFoundText = value;
					base.OnPropertyChangedWithValue<string>(value, "NoRecentGamesFoundText");
				}
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x000143DC File Offset: 0x000125DC
		// (set) Token: 0x0600061E RID: 1566 RVA: 0x000143E4 File Offset: 0x000125E4
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

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x00014407 File Offset: 0x00012607
		// (set) Token: 0x06000620 RID: 1568 RVA: 0x0001440F File Offset: 0x0001260F
		[DataSourceProperty]
		public MBBindingList<StringPairItemWithActionVM> PlayerActions
		{
			get
			{
				return this._playerActions;
			}
			set
			{
				if (value != this._playerActions)
				{
					this._playerActions = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemWithActionVM>>(value, "PlayerActions");
				}
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x0001442D File Offset: 0x0001262D
		// (set) Token: 0x06000622 RID: 1570 RVA: 0x00014435 File Offset: 0x00012635
		[DataSourceProperty]
		public MBBindingList<MPLobbyRecentGameItemVM> Games
		{
			get
			{
				return this._games;
			}
			set
			{
				if (value != this._games)
				{
					this._games = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyRecentGameItemVM>>(value, "Games");
				}
			}
		}

		// Token: 0x040002DB RID: 731
		private MatchHistoryData _currentMatchOfTheActivePlayer;

		// Token: 0x040002DC RID: 732
		private bool _isEnabled;

		// Token: 0x040002DD RID: 733
		private bool _gotItems;

		// Token: 0x040002DE RID: 734
		private bool _isPlayerActionsActive;

		// Token: 0x040002DF RID: 735
		private string _recentGamesText;

		// Token: 0x040002E0 RID: 736
		private string _noRecentGamesFoundText;

		// Token: 0x040002E1 RID: 737
		private string _closeText;

		// Token: 0x040002E2 RID: 738
		private MBBindingList<StringPairItemWithActionVM> _playerActions;

		// Token: 0x040002E3 RID: 739
		private MBBindingList<MPLobbyRecentGameItemVM> _games;
	}
}
