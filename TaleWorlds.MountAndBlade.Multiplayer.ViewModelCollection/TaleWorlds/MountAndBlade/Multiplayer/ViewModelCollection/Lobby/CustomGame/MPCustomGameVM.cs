using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Lobby;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.HostGame;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.HostGame.HostGameOptions;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.CustomGame
{
	// Token: 0x02000061 RID: 97
	public class MPCustomGameVM : ViewModel
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000927 RID: 2343 RVA: 0x0001CFD4 File Offset: 0x0001B1D4
		// (remove) Token: 0x06000928 RID: 2344 RVA: 0x0001D008 File Offset: 0x0001B208
		public static event Action<bool> OnMapCheckingStateChanged;

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x0001D03B File Offset: 0x0001B23B
		public static bool IsPingInfoAvailable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0001D040 File Offset: 0x0001B240
		public MPCustomGameVM(LobbyState lobbyState, MPCustomGameVM.CustomGameMode customGameMode)
		{
			this._lobbyState = lobbyState;
			this._currentCustomGameList = new List<GameServerEntry>();
			this._customGameMode = customGameMode;
			this.HostGame = new MPHostGameVM(this._lobbyState, this._customGameMode);
			this.FiltersData = new MPCustomGameFiltersVM();
			this.GameList = new MBBindingList<MPCustomGameItemVM>();
			this.SortController = new MPCustomGameSortControllerVM(ref this._gameList, this._customGameMode);
			this.CustomServerActionsList = new MBBindingList<StringPairItemWithActionVM>();
			this._currentCustomGameList = new List<GameServerEntry>();
			if (customGameMode == MPCustomGameVM.CustomGameMode.CustomServer)
			{
				this._lobbyState.RegisterForCustomServerAction(new Func<GameServerEntry, List<CustomServerAction>>(this.OnServerActionRequested));
			}
			this.UpdateCanJoinOfficialServersAsAdmin();
			this.InitializeCallbacks();
			this.RefreshValues();
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0001D0F4 File Offset: 0x0001B2F4
		private async void UpdateCanJoinOfficialServersAsAdmin()
		{
			Badge[] array = await NetworkMain.GameClient.GetPlayerBadges();
			this._canJoinOfficialServersAsAdmin = array.Any<Badge>((Badge b) => b.StringId == "badge_official_server_admin");
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0001D12D File Offset: 0x0001B32D
		private void InitializeCallbacks()
		{
			MPCustomGameFiltersVM filtersData = this.FiltersData;
			filtersData.OnFiltersApplied = (Action)Delegate.Combine(filtersData.OnFiltersApplied, new Action(this.RefreshFiltersAndSort));
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0001D156 File Offset: 0x0001B356
		private void FinalizeCallbacks()
		{
			MPCustomGameFiltersVM filtersData = this.FiltersData;
			filtersData.OnFiltersApplied = (Action)Delegate.Remove(filtersData.OnFiltersApplied, new Action(this.RefreshFiltersAndSort));
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0001D180 File Offset: 0x0001B380
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.IsPasswordProtectedHint = new HintViewModel(new TextObject("{=dMdmyb3Y}Password Protected", null), null);
			this.CreateServerText = new TextObject("{=gzdNEM76}Create a Game", null).ToString();
			this.CloseText = new TextObject("{=6MQaCah5}Join a Game", null).ToString();
			this.RefreshText = new TextObject("{=qFPBhVh4}Refresh", null).ToString();
			this.JoinText = new TextObject("{=lWDq0Uss}JOIN", null).ToString();
			this.PasswordText = new TextObject("{=8nJFaJio}Password", null).ToString();
			this.ServerNameText = new TextObject("{=OVcoYxj1}Server Name", null).ToString();
			this.GameTypeText = new TextObject("{=JPimShCw}Game Type", null).ToString();
			this.MapText = new TextObject("{=w9m11T1y}Map", null).ToString();
			this.PlayerCountText = new TextObject("{=RfXJdNye}Players", null).ToString();
			this.PingText = new TextObject("{=7qySRF2T}Ping", null).ToString();
			this.FirstFactionText = new TextObject("{=FhnKJODX}Faction A", null).ToString();
			this.SecondFactionText = new TextObject("{=a9TcHtVw}Faction B", null).ToString();
			this.RegionText = new TextObject("{=uoVKchoC}Region", null).ToString();
			this.PremadeMatchTypeText = new TextObject("{=OzifZbSB}Match Type", null).ToString();
			this.HostText = new TextObject("{=2baWg4Gq}Host", null).ToString();
			this.GameList.ApplyActionOnAllItems(delegate(MPCustomGameItemVM x)
			{
				x.RefreshValues();
			});
			this.SortController.RefreshValues();
			this.FiltersData.RefreshValues();
			MPHostGameVM hostGame = this.HostGame;
			if (hostGame == null)
			{
				return;
			}
			hostGame.RefreshValues();
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0001D344 File Offset: 0x0001B544
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (this._lobbyState != null)
			{
				this._lobbyState.UnregisterForCustomServerAction(new Func<GameServerEntry, List<CustomServerAction>>(this.OnServerActionRequested));
			}
			InputKeyItemVM refreshInputKey = this.RefreshInputKey;
			if (refreshInputKey != null)
			{
				refreshInputKey.OnFinalize();
			}
			this.FinalizeCallbacks();
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0001D384 File Offset: 0x0001B584
		public void OnTick(float dt)
		{
			for (int i = 0; i < this.GameList.Count; i++)
			{
				this.GameList[i].UpdateIsFavorite();
			}
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0001D3B8 File Offset: 0x0001B5B8
		public void SetPremadeGameList(PremadeGameEntry[] entries)
		{
			this.OnGameSelected(null);
			this.GameList.Clear();
			if (entries != null)
			{
				foreach (PremadeGameEntry premadeGameEntry in entries)
				{
					this.GameList.Add(new MPCustomGameItemVM(premadeGameEntry, new Action<MPCustomGameItemVM>(this.OnJoinGame)));
				}
			}
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0001D409 File Offset: 0x0001B609
		public void SetCustomGameServerList(AvailableCustomGames availableCustomGames)
		{
			this.OnGameSelected(null);
			this._currentCustomGameList = availableCustomGames.CustomGameServerInfos;
			this.RefreshFiltersAndSort();
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0001D424 File Offset: 0x0001B624
		private void RefreshFiltersAndSort()
		{
			this.OnGameSelected(null);
			this.GameList.Clear();
			List<GameServerEntry> filteredServerList = this.FiltersData.GetFilteredServerList(this._currentCustomGameList);
			bool? hasCrossplayPrivilege = this._lobbyState.HasCrossplayPrivilege;
			bool flag = true;
			GameServerEntry.FilterGameServerEntriesBasedOnCrossplay(ref filteredServerList, (hasCrossplayPrivilege.GetValueOrDefault() == flag) & (hasCrossplayPrivilege != null));
			foreach (GameServerEntry gameServerEntry in filteredServerList)
			{
				this.GameList.Add(new MPCustomGameItemVM(gameServerEntry, new Action<MPCustomGameItemVM>(this.OnGameSelected), new Action<MPCustomGameItemVM>(this.OnJoinGame), new Action<MPCustomGameItemVM>(this.OnShowActionsForEntry), new Action<MPCustomGameItemVM>(this.OnToggleFavoriteServer)));
			}
			this.SortController.SortByCurrentState();
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0001D504 File Offset: 0x0001B704
		public async void ExecuteRefresh()
		{
			if (this.IsEnabled)
			{
				if (this.IsRefreshing)
				{
					Debug.FailedAssert("Trying to refresh game list but list is already being refreshed", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\CustomGame\\MPCustomGameVM.cs", "ExecuteRefresh", 198);
				}
				else
				{
					this.IsRefreshing = true;
					this.OnGameSelected(null);
					this.GameList.Clear();
					Task task = null;
					if (this._customGameMode == MPCustomGameVM.CustomGameMode.CustomServer)
					{
						task = NetworkMain.GameClient.GetCustomGameServerList();
						MultiplayerOptions.Instance.CurrentOptionsCategory = MultiplayerOptions.OptionsCategory.Default;
					}
					else if (this._customGameMode == MPCustomGameVM.CustomGameMode.PremadeGame)
					{
						task = NetworkMain.GameClient.GetPremadeGameList();
						MultiplayerOptions.Instance.CurrentOptionsCategory = MultiplayerOptions.OptionsCategory.PremadeMatch;
					}
					if (task != null)
					{
						DateTime refreshBeginTime = DateTime.Now;
						await Task.WhenAny(new Task[]
						{
							task,
							Task.Delay(10000)
						});
						TimeSpan timeSpan = DateTime.Now - refreshBeginTime;
						if (timeSpan.TotalSeconds < 3.0)
						{
							await Task.Delay((int)((3.0 - timeSpan.TotalSeconds) * 1000.0));
						}
					}
					MultiplayerOptions.Instance.OnGameTypeChanged(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
					foreach (GenericHostGameOptionDataVM genericHostGameOptionDataVM in this.HostGame.HostGameOptions.GeneralOptions)
					{
						MultipleSelectionHostGameOptionDataVM multipleSelectionHostGameOptionDataVM = genericHostGameOptionDataVM as MultipleSelectionHostGameOptionDataVM;
						if (multipleSelectionHostGameOptionDataVM != null)
						{
							multipleSelectionHostGameOptionDataVM.RefreshList();
						}
					}
					this.IsRefreshing = false;
				}
			}
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0001D540 File Offset: 0x0001B740
		private void OnShowActionsForEntry(MPCustomGameItemVM serverVM)
		{
			if (((serverVM != null) ? serverVM.GameServerInfo : null) == null)
			{
				return;
			}
			this.CustomServerActionsList.Clear();
			List<CustomServerAction> customActionsForServer = this._lobbyState.GetCustomActionsForServer(serverVM.GameServerInfo);
			if (customActionsForServer.Count > 0)
			{
				for (int i = 0; i < customActionsForServer.Count; i++)
				{
					CustomServerAction customServerAction = customActionsForServer[i];
					this.CustomServerActionsList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteSelectCustomServerAction), customServerAction.Name, customServerAction.Name, customServerAction));
				}
			}
			if (this.CustomServerActionsList.Count > 0)
			{
				this.IsCustomServerActionsActive = false;
				this.IsCustomServerActionsActive = true;
			}
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0001D5E0 File Offset: 0x0001B7E0
		private void OnGameSelected(MPCustomGameItemVM gameItem)
		{
			if (this.SelectedGame != null)
			{
				this.SelectedGame.IsSelected = false;
			}
			this.SelectedGame = gameItem;
			if (this.SelectedGame != null)
			{
				this.SelectedGame.IsSelected = true;
			}
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0001D611 File Offset: 0x0001B811
		public void ExecuteJoinSelectedGame()
		{
			if (this.IsJoinEnabled)
			{
				this.OnJoinGame(this.SelectedGame);
			}
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0001D628 File Offset: 0x0001B828
		public void OnJoinGame(MPCustomGameItemVM gameItem)
		{
			if (gameItem == null)
			{
				Debug.FailedAssert("Server to join is null.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\CustomGame\\MPCustomGameVM.cs", "OnJoinGame", 299);
				return;
			}
			if (gameItem.IsPasswordProtected)
			{
				string text = GameTexts.FindText("str_password_required", null).ToString();
				string text2 = GameTexts.FindText("str_enter_password", null).ToString();
				string text3 = GameTexts.FindText("str_ok", null).ToString();
				string text4 = GameTexts.FindText("str_cancel", null).ToString();
				InformationManager.ShowTextInquiry(new TextInquiryData(text, text2, true, true, text3, text4, this.GetOnTryPasswordForServerAction(gameItem), null, true, null, "", ""), false, false);
				return;
			}
			if (this._customGameMode == MPCustomGameVM.CustomGameMode.CustomServer)
			{
				this.JoinCustomGame(gameItem.GameServerInfo, "", false);
				return;
			}
			if (this._customGameMode == MPCustomGameVM.CustomGameMode.PremadeGame)
			{
				this.JoinPremadeGame(gameItem.PremadeGameInfo, "");
			}
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0001D6F8 File Offset: 0x0001B8F8
		private void OnToggleFavoriteServer(MPCustomGameItemVM gameItem)
		{
			GameServerEntry gameServerInfo = gameItem.GameServerInfo;
			FavoriteServerData favoriteServerData;
			if (MultiplayerLocalDataManager.Instance.FavoriteServers.TryGetServerData(gameServerInfo, out favoriteServerData))
			{
				MultiplayerLocalDataManager.Instance.FavoriteServers.RemoveEntry(favoriteServerData);
				return;
			}
			FavoriteServerData favoriteServerData2 = FavoriteServerData.CreateFrom(gameServerInfo);
			MultiplayerLocalDataManager.Instance.FavoriteServers.AddEntry(favoriteServerData2);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0001D748 File Offset: 0x0001B948
		private Action<string> GetOnTryPasswordForServerAction(MPCustomGameItemVM serverItem)
		{
			if (this._customGameMode == MPCustomGameVM.CustomGameMode.CustomServer)
			{
				GameServerEntry serverInfo2 = serverItem.GameServerInfo;
				return delegate(string passwordInput)
				{
					this.JoinCustomGame(serverInfo2, passwordInput, false);
				};
			}
			if (this._customGameMode == MPCustomGameVM.CustomGameMode.PremadeGame)
			{
				PremadeGameEntry serverInfo = serverItem.PremadeGameInfo;
				return delegate(string passwordInput)
				{
					this.JoinPremadeGame(serverInfo, passwordInput);
				};
			}
			return delegate(string _)
			{
				Debug.FailedAssert("Fell through game modes, should never happen", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\CustomGame\\MPCustomGameVM.cs", "GetOnTryPasswordForServerAction", 353);
			};
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0001D7D0 File Offset: 0x0001B9D0
		private List<CustomServerAction> OnServerActionRequested(GameServerEntry serverEntry)
		{
			List<CustomServerAction> list = new List<CustomServerAction>();
			if (this._canJoinOfficialServersAsAdmin || !serverEntry.IsOfficial)
			{
				Action<string> <>9__1;
				CustomServerAction customServerAction = new CustomServerAction(delegate
				{
					string text = new TextObject("{=FzG3CmEe}Join as Admin", null).ToString();
					string text2 = new TextObject("{=MNXyaVCT}Enter Admin Password", null).ToString();
					bool flag = true;
					bool flag2 = true;
					string text3 = new TextObject("{=es0Y3Bxc}Join", null).ToString();
					string text4 = new TextObject("{=3CpNUnVl}Cancel", null).ToString();
					Action<string> action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate(string passwordInput)
						{
							this.JoinCustomGame(serverEntry, passwordInput, true);
						});
					}
					InformationManager.ShowTextInquiry(new TextInquiryData(text, text2, flag, flag2, text3, text4, action, null, true, null, "", ""), false, false);
				}, serverEntry, new TextObject("{=FzG3CmEe}Join as Admin", null).ToString());
				list.Add(customServerAction);
			}
			return list;
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0001D83C File Offset: 0x0001BA3C
		private async void JoinCustomGame(GameServerEntry selectedServer, string passwordInput = "", bool isJoinAsAdmin = false)
		{
			Action<bool> onMapCheckingStateChanged = MPCustomGameVM.OnMapCheckingStateChanged;
			if (onMapCheckingStateChanged != null)
			{
				onMapCheckingStateChanged(true);
			}
			ValueTuple<bool, string> valueTuple = await MapCheckHelpers.CheckMaps(selectedServer);
			Action<bool> onMapCheckingStateChanged2 = MPCustomGameVM.OnMapCheckingStateChanged;
			if (onMapCheckingStateChanged2 != null)
			{
				onMapCheckingStateChanged2(false);
			}
			if (valueTuple.Item1)
			{
				this._lobbyState.OnClientRefusedToJoinCustomServer(selectedServer);
				string text = new TextObject("{=sVVaMyvb}You don't have at least one map ({MAP_NAME}) being played on the server or the local map is not identical. Download all missing maps from the server if you would like to join it.", null).SetTextVariable("MAP_NAME", valueTuple.Item2).ToString();
				InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_couldnt_join_server", null).ToString(), text, false, true, "", GameTexts.FindText("str_dismiss", null).ToString(), null, null, "", 0f, null, null, null), false, false);
			}
			else
			{
				TaskAwaiter<bool> taskAwaiter = NetworkMain.GameClient.RequestJoinCustomGame(selectedServer.Id, passwordInput, isJoinAsAdmin).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter.GetResult())
				{
					InformationManager.ShowInquiry(new InquiryData("", GameTexts.FindText("str_couldnt_join_server", null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", null, null, "", 0f, null, null, null), false, false);
				}
			}
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0001D88D File Offset: 0x0001BA8D
		private void JoinPremadeGame(PremadeGameEntry selectedGame, string passwordInput = "")
		{
			NetworkMain.GameClient.RequestToJoinPremadeGame(selectedGame.Id, passwordInput);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0001D8A0 File Offset: 0x0001BAA0
		private void ExecuteSelectCustomServerAction(object actionParam)
		{
			(actionParam as CustomServerAction).Execute();
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0001D8B2 File Offset: 0x0001BAB2
		public void ExecuteOpenCreateGamePanel()
		{
			if (this.CanPlayerCreateGame)
			{
				this.IsCreateGamePanelActive = true;
			}
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0001D8C3 File Offset: 0x0001BAC3
		public void ExecuteCloseCreateGamePanel()
		{
			this.IsCreateGamePanelActive = false;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0001D8CC File Offset: 0x0001BACC
		private void UpdateCanPlayerCreateGame()
		{
			this.CanPlayerCreateGame = (this.IsPlayerBasedCustomBattleEnabled || this.IsPremadeGameEnabled) && (this.IsPartyLeader || !this.IsInParty);
			if (!this.CanPlayerCreateGame)
			{
				this.IsCreateGamePanelActive = false;
			}
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0001D90A File Offset: 0x0001BB0A
		private void UpdateIsJoinEnabled()
		{
			this.IsJoinEnabled = this.IsAnyGameSelected && (this.IsPartyLeader || !this.IsInParty);
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0001D931 File Offset: 0x0001BB31
		public void SetRefreshInputKey(HotKey hotKey)
		{
			this.RefreshInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000944 RID: 2372 RVA: 0x0001D940 File Offset: 0x0001BB40
		// (set) Token: 0x06000945 RID: 2373 RVA: 0x0001D948 File Offset: 0x0001BB48
		public InputKeyItemVM RefreshInputKey
		{
			get
			{
				return this._refreshInputKey;
			}
			set
			{
				if (value != this._refreshInputKey)
				{
					this._refreshInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "RefreshInputKey");
				}
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000946 RID: 2374 RVA: 0x0001D966 File Offset: 0x0001BB66
		// (set) Token: 0x06000947 RID: 2375 RVA: 0x0001D96E File Offset: 0x0001BB6E
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
					if (this.IsEnabled && !this.IsRefreshing)
					{
						this.ExecuteRefresh();
					}
				}
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000948 RID: 2376 RVA: 0x0001D9A2 File Offset: 0x0001BBA2
		// (set) Token: 0x06000949 RID: 2377 RVA: 0x0001D9AA File Offset: 0x0001BBAA
		[DataSourceProperty]
		public bool IsAnyGameSelected
		{
			get
			{
				return this._isAnyGameSelected;
			}
			set
			{
				if (value != this._isAnyGameSelected)
				{
					this._isAnyGameSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyGameSelected");
					this.UpdateIsJoinEnabled();
				}
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x0600094A RID: 2378 RVA: 0x0001D9CE File Offset: 0x0001BBCE
		// (set) Token: 0x0600094B RID: 2379 RVA: 0x0001D9D6 File Offset: 0x0001BBD6
		[DataSourceProperty]
		public bool IsCreateGamePanelActive
		{
			get
			{
				return this._isCreateGamePanelActive;
			}
			set
			{
				if (value != this._isCreateGamePanelActive)
				{
					this._isCreateGamePanelActive = value;
					base.OnPropertyChangedWithValue(value, "IsCreateGamePanelActive");
				}
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x0001D9F4 File Offset: 0x0001BBF4
		// (set) Token: 0x0600094D RID: 2381 RVA: 0x0001D9FC File Offset: 0x0001BBFC
		[DataSourceProperty]
		public bool CanPlayerCreateGame
		{
			get
			{
				return this._canPlayerCreateGame;
			}
			set
			{
				if (value != this._canPlayerCreateGame)
				{
					this._canPlayerCreateGame = value;
					base.OnPropertyChangedWithValue(value, "CanPlayerCreateGame");
				}
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x0600094E RID: 2382 RVA: 0x0001DA1A File Offset: 0x0001BC1A
		// (set) Token: 0x0600094F RID: 2383 RVA: 0x0001DA22 File Offset: 0x0001BC22
		[DataSourceProperty]
		public bool IsJoinEnabled
		{
			get
			{
				return this._isJoinEnabled;
			}
			set
			{
				if (value != this._isJoinEnabled)
				{
					this._isJoinEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsJoinEnabled");
				}
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x0001DA40 File Offset: 0x0001BC40
		// (set) Token: 0x06000951 RID: 2385 RVA: 0x0001DA48 File Offset: 0x0001BC48
		[DataSourceProperty]
		public MPCustomGameItemVM SelectedGame
		{
			get
			{
				return this._selectedGame;
			}
			set
			{
				if (value != this._selectedGame)
				{
					this._selectedGame = value;
					base.OnPropertyChangedWithValue<MPCustomGameItemVM>(value, "SelectedGame");
					this.IsAnyGameSelected = this._selectedGame != null;
				}
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x0001DA75 File Offset: 0x0001BC75
		// (set) Token: 0x06000953 RID: 2387 RVA: 0x0001DA7D File Offset: 0x0001BC7D
		[DataSourceProperty]
		public MPCustomGameFiltersVM FiltersData
		{
			get
			{
				return this._filtersData;
			}
			set
			{
				if (value != this._filtersData)
				{
					this._filtersData = value;
					base.OnPropertyChangedWithValue<MPCustomGameFiltersVM>(value, "FiltersData");
				}
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x0001DA9B File Offset: 0x0001BC9B
		// (set) Token: 0x06000955 RID: 2389 RVA: 0x0001DAA3 File Offset: 0x0001BCA3
		[DataSourceProperty]
		public MPHostGameVM HostGame
		{
			get
			{
				return this._hostGame;
			}
			set
			{
				if (value != this._hostGame)
				{
					this._hostGame = value;
					base.OnPropertyChangedWithValue<MPHostGameVM>(value, "HostGame");
				}
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x0001DAC1 File Offset: 0x0001BCC1
		// (set) Token: 0x06000957 RID: 2391 RVA: 0x0001DAC9 File Offset: 0x0001BCC9
		[DataSourceProperty]
		public MPCustomGameSortControllerVM SortController
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
					base.OnPropertyChangedWithValue<MPCustomGameSortControllerVM>(value, "SortController");
				}
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x0001DAE7 File Offset: 0x0001BCE7
		// (set) Token: 0x06000959 RID: 2393 RVA: 0x0001DAEF File Offset: 0x0001BCEF
		[DataSourceProperty]
		public MBBindingList<MPCustomGameItemVM> GameList
		{
			get
			{
				return this._gameList;
			}
			set
			{
				if (value != this._gameList)
				{
					this._gameList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPCustomGameItemVM>>(value, "GameList");
				}
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x0001DB0D File Offset: 0x0001BD0D
		// (set) Token: 0x0600095B RID: 2395 RVA: 0x0001DB15 File Offset: 0x0001BD15
		[DataSourceProperty]
		public HintViewModel IsPasswordProtectedHint
		{
			get
			{
				return this._isPasswordProtectedHint;
			}
			set
			{
				if (value != this._isPasswordProtectedHint)
				{
					this._isPasswordProtectedHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "IsPasswordProtectedHint");
				}
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x0001DB33 File Offset: 0x0001BD33
		// (set) Token: 0x0600095D RID: 2397 RVA: 0x0001DB3B File Offset: 0x0001BD3B
		[DataSourceProperty]
		public bool IsRefreshing
		{
			get
			{
				return this._isRefreshing;
			}
			set
			{
				if (value != this._isRefreshing)
				{
					this._isRefreshing = value;
					base.OnPropertyChangedWithValue(value, "IsRefreshing");
				}
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x0001DB59 File Offset: 0x0001BD59
		// (set) Token: 0x0600095F RID: 2399 RVA: 0x0001DB61 File Offset: 0x0001BD61
		[DataSourceProperty]
		public bool IsPartyLeader
		{
			get
			{
				return this._isPartyLeader;
			}
			set
			{
				if (value != this._isPartyLeader)
				{
					this._isPartyLeader = value;
					base.OnPropertyChangedWithValue(value, "IsPartyLeader");
					this.UpdateCanPlayerCreateGame();
					this.UpdateIsJoinEnabled();
				}
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x0001DB8B File Offset: 0x0001BD8B
		// (set) Token: 0x06000961 RID: 2401 RVA: 0x0001DB93 File Offset: 0x0001BD93
		[DataSourceProperty]
		public bool IsInParty
		{
			get
			{
				return this._isInParty;
			}
			set
			{
				if (value != this._isInParty)
				{
					this._isInParty = value;
					base.OnPropertyChangedWithValue(value, "IsInParty");
					this.UpdateCanPlayerCreateGame();
					this.UpdateIsJoinEnabled();
				}
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x0001DBBD File Offset: 0x0001BDBD
		// (set) Token: 0x06000963 RID: 2403 RVA: 0x0001DBC5 File Offset: 0x0001BDC5
		[DataSourceProperty]
		public string CreateServerText
		{
			get
			{
				return this._createServerText;
			}
			set
			{
				if (value != this._createServerText)
				{
					this._createServerText = value;
					base.OnPropertyChangedWithValue<string>(value, "CreateServerText");
				}
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000964 RID: 2404 RVA: 0x0001DBE8 File Offset: 0x0001BDE8
		// (set) Token: 0x06000965 RID: 2405 RVA: 0x0001DBF0 File Offset: 0x0001BDF0
		[DataSourceProperty]
		public bool IsCustomServerActionsActive
		{
			get
			{
				return this._isCustomServerActionsActive;
			}
			set
			{
				if (value != this._isCustomServerActionsActive)
				{
					this._isCustomServerActionsActive = value;
					base.OnPropertyChangedWithValue(value, "IsCustomServerActionsActive");
				}
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x0001DC0E File Offset: 0x0001BE0E
		// (set) Token: 0x06000967 RID: 2407 RVA: 0x0001DC16 File Offset: 0x0001BE16
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

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x0001DC39 File Offset: 0x0001BE39
		// (set) Token: 0x06000969 RID: 2409 RVA: 0x0001DC41 File Offset: 0x0001BE41
		[DataSourceProperty]
		public string RefreshText
		{
			get
			{
				return this._refreshText;
			}
			set
			{
				if (value != this._refreshText)
				{
					this._refreshText = value;
					base.OnPropertyChangedWithValue<string>(value, "RefreshText");
				}
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x0001DC64 File Offset: 0x0001BE64
		// (set) Token: 0x0600096B RID: 2411 RVA: 0x0001DC6C File Offset: 0x0001BE6C
		[DataSourceProperty]
		public string JoinText
		{
			get
			{
				return this._joinText;
			}
			set
			{
				if (value != this._joinText)
				{
					this._joinText = value;
					base.OnPropertyChangedWithValue<string>(value, "JoinText");
				}
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x0001DC8F File Offset: 0x0001BE8F
		// (set) Token: 0x0600096D RID: 2413 RVA: 0x0001DC97 File Offset: 0x0001BE97
		[DataSourceProperty]
		public string ServerNameText
		{
			get
			{
				return this._serverNameText;
			}
			set
			{
				if (value != this._serverNameText)
				{
					this._serverNameText = value;
					base.OnPropertyChangedWithValue<string>(value, "ServerNameText");
				}
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x0001DCBA File Offset: 0x0001BEBA
		// (set) Token: 0x0600096F RID: 2415 RVA: 0x0001DCC2 File Offset: 0x0001BEC2
		[DataSourceProperty]
		public string GameTypeText
		{
			get
			{
				return this._gameTypeText;
			}
			set
			{
				if (value != this._gameTypeText)
				{
					this._gameTypeText = value;
					base.OnPropertyChangedWithValue<string>(value, "GameTypeText");
				}
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x0001DCE5 File Offset: 0x0001BEE5
		// (set) Token: 0x06000971 RID: 2417 RVA: 0x0001DCED File Offset: 0x0001BEED
		[DataSourceProperty]
		public string MapText
		{
			get
			{
				return this._mapText;
			}
			set
			{
				if (value != this._mapText)
				{
					this._mapText = value;
					base.OnPropertyChangedWithValue<string>(value, "MapText");
				}
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000972 RID: 2418 RVA: 0x0001DD10 File Offset: 0x0001BF10
		// (set) Token: 0x06000973 RID: 2419 RVA: 0x0001DD18 File Offset: 0x0001BF18
		[DataSourceProperty]
		public string PlayerCountText
		{
			get
			{
				return this._playerCountText;
			}
			set
			{
				if (value != this._playerCountText)
				{
					this._playerCountText = value;
					base.OnPropertyChangedWithValue<string>(value, "PlayerCountText");
				}
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000974 RID: 2420 RVA: 0x0001DD3B File Offset: 0x0001BF3B
		// (set) Token: 0x06000975 RID: 2421 RVA: 0x0001DD43 File Offset: 0x0001BF43
		[DataSourceProperty]
		public string PingText
		{
			get
			{
				return this._pingText;
			}
			set
			{
				if (value != this._pingText)
				{
					this._pingText = value;
					base.OnPropertyChangedWithValue<string>(value, "PingText");
				}
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x0001DD66 File Offset: 0x0001BF66
		// (set) Token: 0x06000977 RID: 2423 RVA: 0x0001DD6E File Offset: 0x0001BF6E
		[DataSourceProperty]
		public string PasswordText
		{
			get
			{
				return this._passwordText;
			}
			set
			{
				if (value != this._passwordText)
				{
					this._passwordText = value;
					base.OnPropertyChangedWithValue<string>(value, "PasswordText");
				}
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000978 RID: 2424 RVA: 0x0001DD91 File Offset: 0x0001BF91
		// (set) Token: 0x06000979 RID: 2425 RVA: 0x0001DD99 File Offset: 0x0001BF99
		[DataSourceProperty]
		public string FirstFactionText
		{
			get
			{
				return this._firstFactionText;
			}
			set
			{
				if (value != this._firstFactionText)
				{
					this._firstFactionText = value;
					base.OnPropertyChanged("FirstFactionText");
				}
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x0600097A RID: 2426 RVA: 0x0001DDBB File Offset: 0x0001BFBB
		// (set) Token: 0x0600097B RID: 2427 RVA: 0x0001DDC3 File Offset: 0x0001BFC3
		[DataSourceProperty]
		public string SecondFactionText
		{
			get
			{
				return this._secondFactionText;
			}
			set
			{
				if (value != this._secondFactionText)
				{
					this._secondFactionText = value;
					base.OnPropertyChanged("SecondFactionText");
				}
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x0600097C RID: 2428 RVA: 0x0001DDE5 File Offset: 0x0001BFE5
		// (set) Token: 0x0600097D RID: 2429 RVA: 0x0001DDED File Offset: 0x0001BFED
		[DataSourceProperty]
		public string RegionText
		{
			get
			{
				return this._regionText;
			}
			set
			{
				if (value != this._regionText)
				{
					this._regionText = value;
					base.OnPropertyChanged("RegionText");
				}
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x0600097E RID: 2430 RVA: 0x0001DE0F File Offset: 0x0001C00F
		// (set) Token: 0x0600097F RID: 2431 RVA: 0x0001DE17 File Offset: 0x0001C017
		[DataSourceProperty]
		public string PremadeMatchTypeText
		{
			get
			{
				return this._premadeMatchTypeText;
			}
			set
			{
				if (value != this._premadeMatchTypeText)
				{
					this._premadeMatchTypeText = value;
					base.OnPropertyChanged("PremadeMatchTypeText");
				}
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000980 RID: 2432 RVA: 0x0001DE39 File Offset: 0x0001C039
		// (set) Token: 0x06000981 RID: 2433 RVA: 0x0001DE41 File Offset: 0x0001C041
		[DataSourceProperty]
		public string HostText
		{
			get
			{
				return this._hostText;
			}
			set
			{
				if (value != this._hostText)
				{
					this._hostText = value;
					base.OnPropertyChanged("HostText");
				}
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000982 RID: 2434 RVA: 0x0001DE63 File Offset: 0x0001C063
		// (set) Token: 0x06000983 RID: 2435 RVA: 0x0001DE6C File Offset: 0x0001C06C
		[DataSourceProperty]
		public bool IsPlayerBasedCustomBattleEnabled
		{
			get
			{
				return this._isPlayerBasedCustomBattleEnabled;
			}
			set
			{
				if (this._customGameMode == MPCustomGameVM.CustomGameMode.CustomServer)
				{
					this.CreateServerText = (value ? new TextObject("{=gzdNEM76}Create a Game", null).ToString() : new TextObject("{=LrE2cUnG}Currently Disabled", null).ToString());
					if (value != this._isPlayerBasedCustomBattleEnabled)
					{
						this._isPlayerBasedCustomBattleEnabled = value;
						base.OnPropertyChangedWithValue(value, "IsPlayerBasedCustomBattleEnabled");
						this.UpdateCanPlayerCreateGame();
					}
				}
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x0001DECE File Offset: 0x0001C0CE
		// (set) Token: 0x06000985 RID: 2437 RVA: 0x0001DED8 File Offset: 0x0001C0D8
		public bool IsPremadeGameEnabled
		{
			get
			{
				return this._isPremadeGameEnabled;
			}
			set
			{
				if (this._customGameMode == MPCustomGameVM.CustomGameMode.PremadeGame)
				{
					this.CreateServerText = (value ? new TextObject("{=gzdNEM76}Create a Game", null).ToString() : new TextObject("{=LrE2cUnG}Currently Disabled", null).ToString());
					if (value != this._isPremadeGameEnabled)
					{
						this._isPremadeGameEnabled = value;
						base.OnPropertyChangedWithValue(value, "IsPremadeGameEnabled");
						this.UpdateCanPlayerCreateGame();
					}
				}
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x0001DF3B File Offset: 0x0001C13B
		// (set) Token: 0x06000987 RID: 2439 RVA: 0x0001DF43 File Offset: 0x0001C143
		[DataSourceProperty]
		public MBBindingList<StringPairItemWithActionVM> CustomServerActionsList
		{
			get
			{
				return this._customServerActionsList;
			}
			set
			{
				if (value != this._customServerActionsList)
				{
					this._customServerActionsList = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemWithActionVM>>(value, "CustomServerActionsList");
				}
			}
		}

		// Token: 0x0400043F RID: 1087
		private readonly LobbyState _lobbyState;

		// Token: 0x04000440 RID: 1088
		private List<GameServerEntry> _currentCustomGameList;

		// Token: 0x04000441 RID: 1089
		private MPCustomGameVM.CustomGameMode _customGameMode;

		// Token: 0x04000442 RID: 1090
		private bool _canJoinOfficialServersAsAdmin;

		// Token: 0x04000443 RID: 1091
		private const string _officialServerAdminBadgeName = "badge_official_server_admin";

		// Token: 0x04000444 RID: 1092
		private InputKeyItemVM _refreshInputKey;

		// Token: 0x04000445 RID: 1093
		private bool _isEnabled;

		// Token: 0x04000446 RID: 1094
		private bool _isRefreshing;

		// Token: 0x04000447 RID: 1095
		private bool _isPlayerBasedCustomBattleEnabled;

		// Token: 0x04000448 RID: 1096
		private bool _isPremadeGameEnabled;

		// Token: 0x04000449 RID: 1097
		private bool _isInParty;

		// Token: 0x0400044A RID: 1098
		private bool _isPartyLeader;

		// Token: 0x0400044B RID: 1099
		private bool _isCustomServerActionsActive;

		// Token: 0x0400044C RID: 1100
		private bool _isAnyGameSelected;

		// Token: 0x0400044D RID: 1101
		private bool _isCreateGamePanelActive;

		// Token: 0x0400044E RID: 1102
		private bool _canPlayerCreateGame;

		// Token: 0x0400044F RID: 1103
		private bool _isJoinEnabled;

		// Token: 0x04000450 RID: 1104
		private MPCustomGameItemVM _selectedGame;

		// Token: 0x04000451 RID: 1105
		private MPCustomGameFiltersVM _filtersData;

		// Token: 0x04000452 RID: 1106
		private MPHostGameVM _hostGame;

		// Token: 0x04000453 RID: 1107
		private MPCustomGameSortControllerVM _sortController;

		// Token: 0x04000454 RID: 1108
		private MBBindingList<MPCustomGameItemVM> _gameList;

		// Token: 0x04000455 RID: 1109
		private MBBindingList<StringPairItemWithActionVM> _customServerActionsList;

		// Token: 0x04000456 RID: 1110
		private string _createServerText;

		// Token: 0x04000457 RID: 1111
		private string _closeText;

		// Token: 0x04000458 RID: 1112
		private string _refreshText;

		// Token: 0x04000459 RID: 1113
		private string _joinText;

		// Token: 0x0400045A RID: 1114
		private string _serverNameText;

		// Token: 0x0400045B RID: 1115
		private string _gameTypeText;

		// Token: 0x0400045C RID: 1116
		private string _mapText;

		// Token: 0x0400045D RID: 1117
		private string _playerCountText;

		// Token: 0x0400045E RID: 1118
		private string _pingText;

		// Token: 0x0400045F RID: 1119
		private string _passwordText;

		// Token: 0x04000460 RID: 1120
		private string _firstFactionText;

		// Token: 0x04000461 RID: 1121
		private string _secondFactionText;

		// Token: 0x04000462 RID: 1122
		private string _regionText;

		// Token: 0x04000463 RID: 1123
		private string _premadeMatchTypeText;

		// Token: 0x04000464 RID: 1124
		private string _hostText;

		// Token: 0x04000465 RID: 1125
		private HintViewModel _isPasswordProtectedHint;

		// Token: 0x0200013A RID: 314
		public enum CustomGameMode
		{
			// Token: 0x04000969 RID: 2409
			CustomServer,
			// Token: 0x0400096A RID: 2410
			PremadeGame
		}
	}
}
