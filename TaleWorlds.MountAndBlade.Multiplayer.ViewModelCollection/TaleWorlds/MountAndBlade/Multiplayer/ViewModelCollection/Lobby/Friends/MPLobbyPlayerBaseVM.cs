using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Avatar.PlayerServices;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;
using TaleWorlds.MountAndBlade.Diamond.Ranked;
using TaleWorlds.ObjectSystem;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x02000058 RID: 88
	public class MPLobbyPlayerBaseVM : ViewModel
	{
		// Token: 0x17000280 RID: 640
		// (get) Token: 0x060007CC RID: 1996 RVA: 0x00019472 File Offset: 0x00017672
		// (set) Token: 0x060007CD RID: 1997 RVA: 0x0001947A File Offset: 0x0001767A
		public MPLobbyPlayerBaseVM.OnlineStatus CurrentOnlineStatus { get; private set; }

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x00019483 File Offset: 0x00017683
		// (set) Token: 0x060007CF RID: 1999 RVA: 0x0001948B File Offset: 0x0001768B
		public PlayerId ProvidedID
		{
			get
			{
				return this._providedID;
			}
			protected set
			{
				if (this._providedID != value)
				{
					this._providedID = value;
					LobbyClient gameClient = NetworkMain.GameClient;
					this.UpdateAvatar(gameClient != null && gameClient.IsKnownPlayer(this.ProvidedID));
				}
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x000194BF File Offset: 0x000176BF
		// (set) Token: 0x060007D1 RID: 2001 RVA: 0x000194C7 File Offset: 0x000176C7
		public PlayerData PlayerData { get; private set; }

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x000194D0 File Offset: 0x000176D0
		// (set) Token: 0x060007D3 RID: 2003 RVA: 0x000194D8 File Offset: 0x000176D8
		public AnotherPlayerState State { get; protected set; }

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x000194E1 File Offset: 0x000176E1
		// (set) Token: 0x060007D5 RID: 2005 RVA: 0x000194E9 File Offset: 0x000176E9
		public float TimeSinceLastStateUpdate { get; protected set; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x000194F2 File Offset: 0x000176F2
		// (set) Token: 0x060007D7 RID: 2007 RVA: 0x000194FA File Offset: 0x000176FA
		public PlayerStatsBase[] PlayerStats { get; private set; }

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x00019503 File Offset: 0x00017703
		// (set) Token: 0x060007D9 RID: 2009 RVA: 0x0001950B File Offset: 0x0001770B
		public GameTypeRankInfo[] RankInfo { get; private set; }

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x00019514 File Offset: 0x00017714
		// (set) Token: 0x060007DB RID: 2011 RVA: 0x0001951C File Offset: 0x0001771C
		public string RankInfoGameTypeID { get; private set; }

		// Token: 0x060007DC RID: 2012 RVA: 0x00019528 File Offset: 0x00017728
		public MPLobbyPlayerBaseVM(PlayerId id, string forcedName = "", Action<PlayerId> onInviteToClan = null, Action<PlayerId> onFriendRequestAnswered = null)
		{
			this.ProvidedID = id;
			this._forcedName = forcedName;
			this.SetOnInvite(null);
			this._onInviteToClan = onInviteToClan;
			this._onFriendRequestAnswered = onFriendRequestAnswered;
			this.NameHint = new HintViewModel();
			this.ExperienceHint = new HintViewModel();
			this.RatingHint = new HintViewModel();
			LobbyClient gameClient = NetworkMain.GameClient;
			this.UpdateName(gameClient != null && gameClient.IsKnownPlayer(this.ProvidedID));
			this.CanBeInvited = true;
			this.CanInviteToParty = this._onInviteToParty != null;
			this.CanInviteToClan = this._onInviteToClan != null;
			PlatformServices.Instance.CheckPermissionWithUser(Permission.ViewUserGeneratedContent, id, delegate(bool hasBannerlordIDPrivilege)
			{
				this.CanCopyID = hasBannerlordIDPrivilege;
			});
			this.IsRankInfoLoading = true;
			this.GameTypes = new MBBindingList<MPLobbyGameTypeVM>();
			this.RefreshValues();
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00019620 File Offset: 0x00017820
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ClanInfoTitleText = new TextObject("{=j4F7tTzy}Clan", null).ToString();
			this.BadgeInfoTitleText = new TextObject("{=4PrfimcK}Badge", null).ToString();
			this.AvatarInfoTitleText = new TextObject("{=5tbWdY1j}Avatar", null).ToString();
			this.ChangeText = new TextObject("{=Ba50zU7Z}Change", null).ToString();
			this.LevelTitleText = new TextObject("{=OKUTPdaa}Level", null).ToString();
			this.GameTypeText = new TextObject("{=JPimShCw}Game Type", null).ToString();
			this.InviteToPartyHint = new HintViewModel(new TextObject("{=aZnS9ECC}Invite", null), null);
			this.InviteToClanHint = new HintViewModel(new TextObject("{=fLddxLjh}Invite to Clan", null), null);
			this.RemoveFriendHint = new HintViewModel(new TextObject("{=d7ysGcsN}Remove Friend", null), null);
			this.AcceptFriendRequestHint = new HintViewModel(new TextObject("{=BSUteZmt}Accept Friend Request", null), null);
			this.DeclineFriendRequestHint = new HintViewModel(new TextObject("{=942B3LfA}Decline Friend Request", null), null);
			this.CancelFriendRequestHint = new HintViewModel(new TextObject("{=lGbrWyEe}Cancel Friend Request", null), null);
			this.LootHint = new HintViewModel(new TextObject("{=Th8q8wC2}Loot", null), null);
			this.ClanLeaderboardHint = new HintViewModel(new TextObject("{=JdEiK70R}Clan Leaderboard", null), null);
			this.ChangeBannerlordIDHint = new HintViewModel(new TextObject("{=ozREO8ev}Change Bannerlord ID", null), null);
			this.AddFriendWithBannerlordIDHint = new HintViewModel(new TextObject("{=tC9C8TLi}Add Friend", null), null);
			this.CopyBannerlordIDHint = new HintViewModel(new TextObject("{=Pwi1YCjH}Copy Bannerlord ID", null), null);
			MBBindingList<MPLobbyPlayerStatItemVM> displayedStats = this.DisplayedStats;
			if (displayedStats != null)
			{
				displayedStats.ApplyActionOnAllItems(delegate(MPLobbyPlayerStatItemVM s)
				{
					s.RefreshValues();
				});
			}
			MPLobbyBadgeItemVM shownBadge = this.ShownBadge;
			if (shownBadge == null)
			{
				return;
			}
			shownBadge.RefreshValues();
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x000197F4 File Offset: 0x000179F4
		public void RefreshSelectableGameTypes(bool isRankedOnly, Action<string> onRefreshed, string initialGameTypeID = "")
		{
			this.GameTypes.Clear();
			this.GameTypes.Add(new MPLobbyGameTypeVM("Skirmish", false, onRefreshed));
			this.GameTypes.Add(new MPLobbyGameTypeVM("Captain", false, onRefreshed));
			if (!isRankedOnly)
			{
				this.GameTypes.Add(new MPLobbyGameTypeVM("Duel", true, onRefreshed));
				this.GameTypes.Add(new MPLobbyGameTypeVM("TeamDeathmatch", true, onRefreshed));
				this.GameTypes.Add(new MPLobbyGameTypeVM("Siege", true, new Action<string>(this.UpdateDisplayedRankInfo)));
			}
			MPLobbyGameTypeVM mplobbyGameTypeVM = this.GameTypes.FirstOrDefault<MPLobbyGameTypeVM>((MPLobbyGameTypeVM gt) => gt.GameTypeID == initialGameTypeID);
			if (mplobbyGameTypeVM != null)
			{
				mplobbyGameTypeVM.IsSelected = true;
				return;
			}
			this.GameTypes[0].IsSelected = true;
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x000198D0 File Offset: 0x00017AD0
		private void UpdateForcedAvatarIndex(bool isKnownPlayer)
		{
			if (this.ProvidedID != NetworkMain.GameClient.PlayerID)
			{
				Game game = Game.Current;
				object obj;
				if (game == null)
				{
					obj = null;
				}
				else
				{
					GameStateManager gameStateManager = game.GameStateManager;
					obj = ((gameStateManager != null) ? gameStateManager.ActiveState : null);
				}
				LobbyState lobbyState = obj as LobbyState;
				bool flag;
				if (lobbyState == null)
				{
					flag = false;
				}
				else
				{
					bool? hasUserGeneratedContentPrivilege = lobbyState.HasUserGeneratedContentPrivilege;
					bool flag2 = false;
					flag = (hasUserGeneratedContentPrivilege.GetValueOrDefault() == flag2) & (hasUserGeneratedContentPrivilege != null);
				}
				if (flag)
				{
					this._forcedAvatarIndex = AvatarServices.GetForcedAvatarIndexOfPlayer(this.ProvidedID);
					return;
				}
			}
			if (!BannerlordConfig.EnableGenericAvatars || this.ProvidedID == NetworkMain.GameClient.PlayerID || isKnownPlayer)
			{
				this._forcedAvatarIndex = -1;
				return;
			}
			this._forcedAvatarIndex = AvatarServices.GetForcedAvatarIndexOfPlayer(this.ProvidedID);
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00019988 File Offset: 0x00017B88
		protected async void UpdateName(bool isKnownPlayer)
		{
			string genericName = this._genericPlayerName.ToString();
			this.Name = genericName;
			if (this.ProvidedID != NetworkMain.GameClient.PlayerID)
			{
				Game game = Game.Current;
				object obj;
				if (game == null)
				{
					obj = null;
				}
				else
				{
					GameStateManager gameStateManager = game.GameStateManager;
					obj = ((gameStateManager != null) ? gameStateManager.ActiveState : null);
				}
				LobbyState lobbyState = obj as LobbyState;
				bool flag;
				if (lobbyState == null)
				{
					flag = false;
				}
				else
				{
					bool? hasUserGeneratedContentPrivilege = lobbyState.HasUserGeneratedContentPrivilege;
					bool flag2 = false;
					flag = (hasUserGeneratedContentPrivilege.GetValueOrDefault() == flag2) & (hasUserGeneratedContentPrivilege != null);
				}
				if (flag && this.ProvidedID.ProvidedType != PlayerIdProvidedTypes.PS && this.ProvidedID.ProvidedType != PlayerIdProvidedTypes.GDK)
				{
					this.Name = genericName;
					goto IL_0279;
				}
			}
			if (this._forcedName != string.Empty && !BannerlordConfig.EnableGenericNames)
			{
				this.Name = this._forcedName;
			}
			else if (this.ProvidedID == NetworkMain.GameClient.PlayerID)
			{
				this.Name = NetworkMain.GameClient.Name;
			}
			else if (!isKnownPlayer && BannerlordConfig.EnableGenericNames)
			{
				this.Name = genericName;
			}
			else if (this.PlayerData != null)
			{
				string lastPlayerName = this.PlayerData.LastPlayerName;
				this.Name = lastPlayerName;
			}
			else if (this.ProvidedID.IsValid)
			{
				IFriendListService[] friendListServices = PlatformServices.Instance.GetFriendListServices();
				string foundName = genericName;
				for (int i = friendListServices.Length - 1; i >= 0; i--)
				{
					string text = await friendListServices[i].GetUserName(this.ProvidedID);
					if (!string.IsNullOrEmpty(text) && text != "-" && text != genericName)
					{
						foundName = text;
						break;
					}
				}
				this.Name = foundName;
				friendListServices = null;
				foundName = null;
			}
			IL_0279:
			this.NameHint.HintText = new TextObject("{=!}" + this.Name, null);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x000199C9 File Offset: 0x00017BC9
		protected void UpdateAvatar(bool isKnownPlayer)
		{
			this.UpdateForcedAvatarIndex(isKnownPlayer);
			this.Avatar = new PlayerAvatarImageIdentifierVM(this.ProvidedID, this._forcedAvatarIndex);
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x000199EC File Offset: 0x00017BEC
		public void UpdatePlayerState(AnotherPlayerData playerData)
		{
			if (playerData == null)
			{
				return;
			}
			if (playerData.PlayerState != AnotherPlayerState.NoAnswer)
			{
				this.State = playerData.PlayerState;
				this.StateText = GameTexts.FindText("str_multiplayer_lobby_state", this.State.ToString()).ToString();
			}
			this.TimeSinceLastStateUpdate = Game.Current.ApplicationTime;
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00019A4C File Offset: 0x00017C4C
		public virtual void UpdateWith(PlayerData playerData)
		{
			if (playerData == null)
			{
				Debug.FailedAssert("PlayerData shouldn't be null at this stage!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\Friends\\MPLobbyPlayerBaseVM.cs", "UpdateWith", 280);
				return;
			}
			this.PlayerData = playerData;
			this.ProvidedID = this.PlayerData.PlayerId;
			this.UpdateNameAndAvatar(true);
			this.UpdateExperienceData();
			if (NetworkMain.GameClient != null && NetworkMain.GameClient.SupportedFeatures.SupportsFeatures(Features.Clan))
			{
				this.IsClanInfoSupported = true;
			}
			else
			{
				this.IsClanInfoSupported = false;
			}
			this.Loot = playerData.Gold;
			this.Sigil = new MPLobbySigilItemVM();
			this.Sigil.RefreshWith(playerData.Sigil);
			this.ShownBadge = new MPLobbyBadgeItemVM(BadgeManager.GetById(playerData.ShownBadgeId), null, (Badge badge) => true, null);
			this.BannerlordID = string.Format("{0}#{1}", playerData.Username, playerData.UserId);
			this.SelectedBadgeID = playerData.ShownBadgeId;
			this.StateText = "";
			this._hasReceivedPlayerStats = false;
			this._isReceivingPlayerStats = false;
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00019B6C File Offset: 0x00017D6C
		public void UpdateNameAndAvatar(bool forceUpdate = false)
		{
			bool flag = NetworkMain.GameClient.IsKnownPlayer(this.ProvidedID);
			if (this._isKnownPlayer != flag || forceUpdate)
			{
				this._isKnownPlayer = flag;
				this.UpdateAvatar(flag);
				this.UpdateName(flag);
			}
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00019BB0 File Offset: 0x00017DB0
		public void OnStatusChanged(MPLobbyPlayerBaseVM.OnlineStatus status, bool isInGameStatusActive)
		{
			this.CurrentOnlineStatus = status;
			this.StateText = "";
			this.TimeSinceLastStateUpdate = 0f;
			this.CanInviteToParty = this._onInviteToParty != null && (status == MPLobbyPlayerBaseVM.OnlineStatus.InGame || (status == MPLobbyPlayerBaseVM.OnlineStatus.Online && !isInGameStatusActive));
			this.ShowLevel = status == MPLobbyPlayerBaseVM.OnlineStatus.InGame || (status == MPLobbyPlayerBaseVM.OnlineStatus.Online && !isInGameStatusActive);
			this.CanInviteToClan = this._onInviteToClan != null && status == MPLobbyPlayerBaseVM.OnlineStatus.Online;
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00019C2A File Offset: 0x00017E2A
		public void SetOnInvite(Action<PlayerId> onInvite)
		{
			this._onInviteToParty = onInvite;
			this.CanInviteToParty = onInvite != null;
			this.RefreshValues();
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00019C44 File Offset: 0x00017E44
		public async void UpdateStats(Action onDone)
		{
			if (!this._hasReceivedPlayerStats && !this._isReceivingPlayerStats)
			{
				this._isReceivingPlayerStats = true;
				PlayerStatsBase[] array = await NetworkMain.GameClient.GetPlayerStats(this.ProvidedID);
				this.PlayerStats = array;
				this._isReceivingPlayerStats = false;
				this._hasReceivedPlayerStats = this.PlayerStats != null;
				if (this._hasReceivedPlayerStats)
				{
					Action onPlayerStatsReceived = this.OnPlayerStatsReceived;
					if (onPlayerStatsReceived != null)
					{
						onPlayerStatsReceived();
					}
					if (onDone != null)
					{
						onDone();
					}
				}
			}
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00019C88 File Offset: 0x00017E88
		public void UpdateExperienceData()
		{
			this.Level = this.PlayerData.Level;
			int num = PlayerDataExperience.ExperienceRequiredForLevel(this.PlayerData.Level + 1);
			float num2 = (float)this.PlayerData.ExperienceInCurrentLevel / (float)num;
			this.ExperienceRatio = (int)(num2 * 100f);
			string text = this.PlayerData.ExperienceInCurrentLevel + " / " + num;
			this.ExperienceHint.HintText = new TextObject("{=!}" + text, null);
			TextObject textObject = new TextObject("{=5Z0pvuNL}Level {LEVEL}", null);
			textObject.SetTextVariable("LEVEL", this.Level);
			this.LevelText = textObject.ToString();
			int experienceToNextLevel = this.PlayerData.ExperienceToNextLevel;
			TextObject textObject2 = new TextObject("{=NUSH5bJu}{EXPERIENCE} exp to next level", null);
			textObject2.SetTextVariable("EXPERIENCE", experienceToNextLevel);
			this.ExperienceText = textObject2.ToString();
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00019D74 File Offset: 0x00017F74
		public async void UpdateRating(Action onDone)
		{
			this.IsRankInfoLoading = true;
			GameTypeRankInfo[] array = await NetworkMain.GameClient.GetGameTypeRankInfo(this.ProvidedID);
			this.RankInfo = array;
			this.IsRankInfoLoading = false;
			if (onDone != null)
			{
				onDone();
			}
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00019DB8 File Offset: 0x00017FB8
		public void UpdateDisplayedRankInfo(string gameType)
		{
			GameTypeRankInfo gameTypeRankInfo = null;
			if (gameType == "Skirmish")
			{
				GameTypeRankInfo[] rankInfo = this.RankInfo;
				GameTypeRankInfo gameTypeRankInfo2;
				if (rankInfo == null)
				{
					gameTypeRankInfo2 = null;
				}
				else
				{
					gameTypeRankInfo2 = rankInfo.FirstOrDefault<GameTypeRankInfo>((GameTypeRankInfo r) => r.GameType == "Skirmish");
				}
				gameTypeRankInfo = gameTypeRankInfo2;
				this.RankInfoGameTypeID = "Skirmish";
			}
			else if (gameType == "Captain")
			{
				GameTypeRankInfo[] rankInfo2 = this.RankInfo;
				GameTypeRankInfo gameTypeRankInfo3;
				if (rankInfo2 == null)
				{
					gameTypeRankInfo3 = null;
				}
				else
				{
					gameTypeRankInfo3 = rankInfo2.FirstOrDefault<GameTypeRankInfo>((GameTypeRankInfo r) => r.GameType == "Captain");
				}
				gameTypeRankInfo = gameTypeRankInfo3;
				this.RankInfoGameTypeID = "Captain";
			}
			if (gameTypeRankInfo != null)
			{
				RankBarInfo rankBarInfo = gameTypeRankInfo.RankBarInfo;
				this.Rating = rankBarInfo.Rating;
				this.RatingID = rankBarInfo.RankId;
				this.RatingText = MPLobbyVM.GetLocalizedRankName(this.RatingID);
				if (rankBarInfo.IsEvaluating)
				{
					TextObject textObject = new TextObject("{=Ise5gWw3}{PLAYED_GAMES} / {TOTAL_GAMES} Evaluation matches played", null);
					textObject.SetTextVariable("PLAYED_GAMES", rankBarInfo.EvaluationMatchesPlayed);
					textObject.SetTextVariable("TOTAL_GAMES", rankBarInfo.TotalEvaluationMatchesRequired);
					this.RankText = textObject.ToString();
					this.RatingRatio = MathF.Floor((float)rankBarInfo.EvaluationMatchesPlayed / (float)rankBarInfo.TotalEvaluationMatchesRequired * 100f);
				}
				else
				{
					TextObject textObject2 = new TextObject("{=BUOtUW1u}{RATING} Points", null);
					textObject2.SetTextVariable("RATING", rankBarInfo.Rating);
					this.RankText = textObject2.ToString();
					this.RatingRatio = (string.IsNullOrEmpty(rankBarInfo.NextRankId) ? 100 : MathF.Floor(rankBarInfo.ProgressPercentage));
				}
				GameTexts.SetVariable("NUMBER", this.RatingRatio.ToString("0.00"));
				this.RatingHint.HintText = GameTexts.FindText("str_NUMBER_percent", null);
			}
			else
			{
				this.Rating = 0;
				this.RatingRatio = 0;
				this.RatingID = "norank";
				this.RatingText = new TextObject("{=GXosklej}Casual", null).ToString();
				this.RankText = new TextObject("{=56FyokuX}Game mode is casual", null).ToString();
				this.RatingHint.HintText = TextObject.GetEmpty();
			}
			Action<string> onRankInfoChanged = this.OnRankInfoChanged;
			if (onRankInfoChanged != null)
			{
				onRankInfoChanged(gameType);
			}
			this.IsRankInfoCasual = gameType != "Skirmish" && gameType != "Captain";
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0001A004 File Offset: 0x00018204
		public async void UpdateClanInfo()
		{
			if (!(this.ProvidedID == PlayerId.Empty))
			{
				bool isSelfPlayer = this.ProvidedID == NetworkMain.GameClient.PlayerID;
				ClanInfo clanInfo;
				if (isSelfPlayer)
				{
					clanInfo = NetworkMain.GameClient.ClanInfo;
				}
				else
				{
					clanInfo = await NetworkMain.GameClient.GetPlayerClanInfo(this.ProvidedID);
				}
				if (clanInfo != null && (isSelfPlayer || (!isSelfPlayer && clanInfo.Players.Length != 0)))
				{
					this.ClanBanner = new BannerImageIdentifierVM(new Banner(clanInfo.Sigil), true);
					this.ClanName = clanInfo.Name;
					GameTexts.SetVariable("STR", clanInfo.Tag);
					this.ClanTag = new TextObject("{=uTXYEAOg}[{STR}]", null).ToString();
				}
				else
				{
					this.ClanBanner = new BannerImageIdentifierVM(Banner.CreateOneColoredEmptyBanner(99), false);
					this.ClanName = new TextObject("{=0DnHFlia}Not In a Clan", null).ToString();
					this.ClanTag = string.Empty;
				}
			}
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0001A040 File Offset: 0x00018240
		public void FilterStatsForGameMode(string gameModeCode)
		{
			if (this.PlayerStats == null)
			{
				return;
			}
			if (this.DisplayedStats == null)
			{
				this.DisplayedStats = new MBBindingList<MPLobbyPlayerStatItemVM>();
			}
			this.DisplayedStats.Clear();
			IEnumerable<PlayerStatsBase> enumerable = this.PlayerStats.Where<PlayerStatsBase>((PlayerStatsBase s) => s.GameType == gameModeCode);
			foreach (PlayerStatsBase playerStatsBase in enumerable)
			{
				if (gameModeCode == "Skirmish" || gameModeCode == "Captain")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=WW2N3zJf}Wins", null), playerStatsBase.WinCount));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=4nr9Km6t}Losses", null), playerStatsBase.LoseCount));
				}
				if (gameModeCode == "Skirmish")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=ab2cbidI}Total Score", null), (playerStatsBase as PlayerStatsSkirmish).Score));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=fdR3xpBS}MVP Badges", null), (playerStatsBase as PlayerStatsSkirmish).MVPs));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=2FaZ6E1k}Kill Death Ratio", null), playerStatsBase.AverageKillPerDeath));
				}
				else if (gameModeCode == "Captain")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=ab2cbidI}Total Score", null), (playerStatsBase as PlayerStatsCaptain).Score));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=fdR3xpBS}MVP Badges", null), (playerStatsBase as PlayerStatsCaptain).MVPs));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=9FSk2daF}Captains Killed", null), (playerStatsBase as PlayerStatsCaptain).CaptainsKilled));
				}
				else if (gameModeCode == "Siege")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=ab2cbidI}Total Score", null), (playerStatsBase as PlayerStatsSiege).Score));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=XKWGPrYt}Siege Engines Destroyed", null), (playerStatsBase as PlayerStatsSiege).SiegeEnginesDestroyed));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=7APa598U}Kills With a Siege Engine", null), (playerStatsBase as PlayerStatsSiege).SiegeEngineKills));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=FaKWQccs}Gold Gained From Objectives", null), (playerStatsBase as PlayerStatsSiege).ObjectiveGoldGained));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=2FaZ6E1k}Kill Death Ratio", null), playerStatsBase.AverageKillPerDeath));
				}
				else if (gameModeCode == "Duel")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=SS5WyUWR}Duels Won", null), (playerStatsBase as PlayerStatsDuel).DuelsWon));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=Iu2eFSsh}Infantry Wins", null), (playerStatsBase as PlayerStatsDuel).InfantryWins));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=wyKhcvbd}Ranged Wins", null), (playerStatsBase as PlayerStatsDuel).ArcherWins));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=qipBkhys}Cavalry Wins", null), (playerStatsBase as PlayerStatsDuel).CavalryWins));
				}
				else if (gameModeCode == "TeamDeathmatch")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=ab2cbidI}Total Score", null), (playerStatsBase as PlayerStatsTeamDeathmatch).Score));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=9ET13VOe}Average Score", null), (playerStatsBase as PlayerStatsTeamDeathmatch).AverageScore));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=2FaZ6E1k}Kill Death Ratio", null), playerStatsBase.AverageKillPerDeath));
				}
				if (gameModeCode != "Duel")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=FKe05WtJ}Kills", null), playerStatsBase.KillCount));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=8eZFlPVu}Deaths", null), playerStatsBase.DeathCount));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(playerStatsBase.GameType, new TextObject("{=1imGhhZl}Assists", null), playerStatsBase.AssistCount));
				}
			}
			if (enumerable.IsEmpty<PlayerStatsBase>())
			{
				if (gameModeCode == "Skirmish" || gameModeCode == "Captain")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=WW2N3zJf}Wins", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=4nr9Km6t}Losses", null), "-"));
				}
				if (gameModeCode == "Skirmish")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=ab2cbidI}Total Score", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=fdR3xpBS}MVP Badges", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=2FaZ6E1k}Kill Death Ratio", null), "-"));
				}
				else if (gameModeCode == "Captain")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=ab2cbidI}Total Score", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=fdR3xpBS}MVP Badges", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=9FSk2daF}Captains Killed", null), "-"));
				}
				else if (gameModeCode == "Siege")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=ab2cbidI}Total Score", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=XKWGPrYt}Siege Engines Destroyed", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=7APa598U}Kills With a Siege Engine", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=FaKWQccs}Gold Gained From Objectives", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=2FaZ6E1k}Kill Death Ratio", null), "-"));
				}
				else if (gameModeCode == "Duel")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=SS5WyUWR}Duels Won", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=Iu2eFSsh}Infantry Wins", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=wyKhcvbd}Ranged Wins", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=qipBkhys}Cavalry Wins", null), "-"));
				}
				else if (gameModeCode == "TeamDeathmatch")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=ab2cbidI}Total Score", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=9ET13VOe}Average Score", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=2FaZ6E1k}Kill Death Ratio", null), "-"));
				}
				if (gameModeCode != "Duel")
				{
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=FKe05WtJ}Kills", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=8eZFlPVu}Deaths", null), "-"));
					this.DisplayedStats.Add(new MPLobbyPlayerStatItemVM(gameModeCode, new TextObject("{=1imGhhZl}Assists", null), "-"));
				}
			}
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0001A97C File Offset: 0x00018B7C
		public void RefreshCharacterVisual()
		{
			this.CharacterVisual = new CharacterViewModel();
			BasicCharacterObject @object = MBObjectManager.Instance.GetObject<BasicCharacterObject>("mp_character");
			@object.UpdatePlayerCharacterBodyProperties(this.PlayerData.BodyProperties, this.PlayerData.Race, this.PlayerData.IsFemale);
			this.CharacterVisual.FillFrom(@object, -1, null);
			this.CharacterVisual.BodyProperties = new BodyProperties(this.PlayerData.BodyProperties.DynamicProperties, @object.BodyPropertyRange.BodyPropertyMin.StaticProperties).ToString();
			this.CharacterVisual.IsFemale = this.PlayerData.IsFemale;
			this.CharacterVisual.Race = this.PlayerData.Race;
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x0001AA49 File Offset: 0x00018C49
		public void ExecuteSelectPlayer()
		{
			this.IsSelected = !this.IsSelected;
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x0001AA5A File Offset: 0x00018C5A
		public void ExecuteInviteToParty()
		{
			Action<PlayerId> onInviteToParty = this._onInviteToParty;
			if (onInviteToParty == null)
			{
				return;
			}
			onInviteToParty(this.ProvidedID);
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0001AA72 File Offset: 0x00018C72
		public void ExecuteInviteToClan()
		{
			Action<PlayerId> onInviteToClan = this._onInviteToClan;
			if (onInviteToClan == null)
			{
				return;
			}
			onInviteToClan(this.ProvidedID);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0001AA8A File Offset: 0x00018C8A
		public void ExecuteKickFromParty()
		{
			if (NetworkMain.GameClient.IsInParty && NetworkMain.GameClient.IsPartyLeader)
			{
				NetworkMain.GameClient.KickPlayerFromParty(this.ProvidedID);
			}
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0001AAB4 File Offset: 0x00018CB4
		public void ExecuteAcceptFriendRequest()
		{
			bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(this.ProvidedID);
			NetworkMain.GameClient.RespondToFriendRequest(this.ProvidedID, flag, true, false);
			Action<PlayerId> onFriendRequestAnswered = this._onFriendRequestAnswered;
			if (onFriendRequestAnswered != null)
			{
				onFriendRequestAnswered(this.ProvidedID);
			}
			if (this.HasNotification)
			{
				this.HasNotification = false;
			}
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x0001AB18 File Offset: 0x00018D18
		public void ExecuteDeclineFriendRequest()
		{
			bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(this.ProvidedID);
			NetworkMain.GameClient.RespondToFriendRequest(this.ProvidedID, flag, false, false);
			Action<PlayerId> onFriendRequestAnswered = this._onFriendRequestAnswered;
			if (onFriendRequestAnswered != null)
			{
				onFriendRequestAnswered(this.ProvidedID);
			}
			if (this.HasNotification)
			{
				this.HasNotification = false;
			}
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0001AB7C File Offset: 0x00018D7C
		public void ExecuteCancelPendingFriendRequest()
		{
			NetworkMain.GameClient.RemoveFriend(this.ProvidedID);
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x0001AB8E File Offset: 0x00018D8E
		public void ExecuteRemoveFriend()
		{
			NetworkMain.GameClient.RemoveFriend(this.ProvidedID);
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x0001ABA0 File Offset: 0x00018DA0
		public void ExecuteCopyBannerlordID()
		{
			Input.SetClipboardText(this.BannerlordID);
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x0001ABB0 File Offset: 0x00018DB0
		private void ExecuteAddFriend()
		{
			string[] array = this.BannerlordID.Split(new char[] { '#' });
			string text = array[0];
			int num;
			if (int.TryParse(array[1], out num))
			{
				bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(this.ProvidedID);
				NetworkMain.GameClient.AddFriendByUsernameAndId(text, num, flag);
			}
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x0001AC0D File Offset: 0x00018E0D
		public void ExecuteShowProfile()
		{
			Action<PlayerId> onPlayerProfileRequested = MPLobbyPlayerBaseVM.OnPlayerProfileRequested;
			if (onPlayerProfileRequested == null)
			{
				return;
			}
			onPlayerProfileRequested(this.ProvidedID);
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0001AC24 File Offset: 0x00018E24
		private void ExecuteActivateSigilChangeInformation()
		{
			this.IsSigilChangeInformationEnabled = true;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0001AC2D File Offset: 0x00018E2D
		private void ExecuteDeactivateSigilChangeInformation()
		{
			this.IsSigilChangeInformationEnabled = false;
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0001AC36 File Offset: 0x00018E36
		private void ExecuteChangeSigil()
		{
			Action<PlayerId> onSigilChangeRequested = MPLobbyPlayerBaseVM.OnSigilChangeRequested;
			if (onSigilChangeRequested == null)
			{
				return;
			}
			onSigilChangeRequested(this.ProvidedID);
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0001AC4D File Offset: 0x00018E4D
		private void ExecuteChangeBannerlordID()
		{
			Action<PlayerId> onBannerlordIDChangeRequested = MPLobbyPlayerBaseVM.OnBannerlordIDChangeRequested;
			if (onBannerlordIDChangeRequested == null)
			{
				return;
			}
			onBannerlordIDChangeRequested(this.ProvidedID);
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0001AC64 File Offset: 0x00018E64
		private void ExecuteAddFriendWithBannerlordID()
		{
			Action<PlayerId> onAddFriendWithBannerlordIDRequested = MPLobbyPlayerBaseVM.OnAddFriendWithBannerlordIDRequested;
			if (onAddFriendWithBannerlordIDRequested == null)
			{
				return;
			}
			onAddFriendWithBannerlordIDRequested(this.ProvidedID);
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0001AC7B File Offset: 0x00018E7B
		private void ExecuteChangeBadge()
		{
			Action<PlayerId> onBadgeChangeRequested = MPLobbyPlayerBaseVM.OnBadgeChangeRequested;
			if (onBadgeChangeRequested == null)
			{
				return;
			}
			onBadgeChangeRequested(this.ProvidedID);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0001AC94 File Offset: 0x00018E94
		private void ExecuteShowRankProgression()
		{
			MPLobbyGameTypeVM mplobbyGameTypeVM = this.GameTypes.FirstOrDefault<MPLobbyGameTypeVM>((MPLobbyGameTypeVM gt) => gt.IsSelected);
			if (mplobbyGameTypeVM != null && !mplobbyGameTypeVM.IsCasual)
			{
				Action<MPLobbyPlayerBaseVM> onRankProgressionRequested = MPLobbyPlayerBaseVM.OnRankProgressionRequested;
				if (onRankProgressionRequested == null)
				{
					return;
				}
				onRankProgressionRequested(this);
			}
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0001ACEC File Offset: 0x00018EEC
		private void ExecuteShowRankLeaderboard()
		{
			MPLobbyGameTypeVM mplobbyGameTypeVM = this.GameTypes.FirstOrDefault<MPLobbyGameTypeVM>((MPLobbyGameTypeVM gt) => gt.IsSelected);
			if (mplobbyGameTypeVM != null && !mplobbyGameTypeVM.IsCasual)
			{
				Action<string> onRankLeaderboardRequested = MPLobbyPlayerBaseVM.OnRankLeaderboardRequested;
				if (onRankLeaderboardRequested == null)
				{
					return;
				}
				onRankLeaderboardRequested(mplobbyGameTypeVM.GameTypeID);
			}
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0001AD44 File Offset: 0x00018F44
		private void ExecuteShowClanPage()
		{
			Action onClanPageRequested = MPLobbyPlayerBaseVM.OnClanPageRequested;
			if (onClanPageRequested == null)
			{
				return;
			}
			onClanPageRequested();
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0001AD55 File Offset: 0x00018F55
		private void ExecuteShowClanLeaderboard()
		{
			Action onClanLeaderboardRequested = MPLobbyPlayerBaseVM.OnClanLeaderboardRequested;
			if (onClanLeaderboardRequested == null)
			{
				return;
			}
			onClanLeaderboardRequested();
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000803 RID: 2051 RVA: 0x0001AD66 File Offset: 0x00018F66
		// (set) Token: 0x06000804 RID: 2052 RVA: 0x0001AD6E File Offset: 0x00018F6E
		[DataSourceProperty]
		public bool CanCopyID
		{
			get
			{
				return this._canCopyID;
			}
			set
			{
				if (value != this._canCopyID)
				{
					this._canCopyID = value;
					base.OnPropertyChangedWithValue(value, "CanCopyID");
				}
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x0001AD8C File Offset: 0x00018F8C
		// (set) Token: 0x06000806 RID: 2054 RVA: 0x0001AD94 File Offset: 0x00018F94
		[DataSourceProperty]
		public bool ShowLevel
		{
			get
			{
				return this._showLevel;
			}
			set
			{
				if (value != this._showLevel)
				{
					this._showLevel = value;
					base.OnPropertyChangedWithValue(value, "ShowLevel");
				}
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000807 RID: 2055 RVA: 0x0001ADB2 File Offset: 0x00018FB2
		// (set) Token: 0x06000808 RID: 2056 RVA: 0x0001ADBA File Offset: 0x00018FBA
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

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x0001ADD8 File Offset: 0x00018FD8
		// (set) Token: 0x0600080A RID: 2058 RVA: 0x0001ADE0 File Offset: 0x00018FE0
		[DataSourceProperty]
		public bool HasNotification
		{
			get
			{
				return this._hasNotification;
			}
			set
			{
				if (value != this._hasNotification)
				{
					this._hasNotification = value;
					base.OnPropertyChangedWithValue(value, "HasNotification");
				}
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x0001ADFE File Offset: 0x00018FFE
		// (set) Token: 0x0600080C RID: 2060 RVA: 0x0001AE06 File Offset: 0x00019006
		[DataSourceProperty]
		public bool IsFriendRequest
		{
			get
			{
				return this._isFriendRequest;
			}
			set
			{
				if (value != this._isFriendRequest)
				{
					this._isFriendRequest = value;
					base.OnPropertyChangedWithValue(value, "IsFriendRequest");
				}
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x0600080D RID: 2061 RVA: 0x0001AE24 File Offset: 0x00019024
		// (set) Token: 0x0600080E RID: 2062 RVA: 0x0001AE2C File Offset: 0x0001902C
		[DataSourceProperty]
		public bool IsPendingRequest
		{
			get
			{
				return this._isPendingRequest;
			}
			set
			{
				if (value != this._isPendingRequest)
				{
					this._isPendingRequest = value;
					base.OnPropertyChangedWithValue(value, "IsPendingRequest");
				}
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x0001AE4A File Offset: 0x0001904A
		// (set) Token: 0x06000810 RID: 2064 RVA: 0x0001AE52 File Offset: 0x00019052
		[DataSourceProperty]
		public bool CanRemove
		{
			get
			{
				return this._canRemove;
			}
			set
			{
				if (value != this._canRemove)
				{
					this._canRemove = value;
					base.OnPropertyChangedWithValue(value, "CanRemove");
				}
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x0001AE70 File Offset: 0x00019070
		// (set) Token: 0x06000812 RID: 2066 RVA: 0x0001AE78 File Offset: 0x00019078
		[DataSourceProperty]
		public bool CanBeInvited
		{
			get
			{
				return this._canBeInvited;
			}
			set
			{
				if (value != this._canBeInvited)
				{
					this._canBeInvited = value;
					base.OnPropertyChangedWithValue(value, "CanBeInvited");
				}
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x0001AE96 File Offset: 0x00019096
		// (set) Token: 0x06000814 RID: 2068 RVA: 0x0001AE9E File Offset: 0x0001909E
		[DataSourceProperty]
		public bool CanInviteToParty
		{
			get
			{
				return this._canInviteToParty;
			}
			set
			{
				if (value != this._canInviteToParty)
				{
					this._canInviteToParty = value;
					base.OnPropertyChangedWithValue(value, "CanInviteToParty");
				}
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x0001AEBC File Offset: 0x000190BC
		// (set) Token: 0x06000816 RID: 2070 RVA: 0x0001AEC4 File Offset: 0x000190C4
		[DataSourceProperty]
		public bool CanInviteToClan
		{
			get
			{
				return this._canInviteToClan;
			}
			set
			{
				if (value != this._canInviteToClan)
				{
					this._canInviteToClan = value;
					base.OnPropertyChangedWithValue(value, "CanInviteToClan");
				}
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x0001AEE2 File Offset: 0x000190E2
		// (set) Token: 0x06000818 RID: 2072 RVA: 0x0001AEEA File Offset: 0x000190EA
		[DataSourceProperty]
		public bool IsSigilChangeInformationEnabled
		{
			get
			{
				return this._isSigilChangeInformationEnabled;
			}
			set
			{
				if (value != this._isSigilChangeInformationEnabled)
				{
					this._isSigilChangeInformationEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsSigilChangeInformationEnabled");
				}
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x0001AF08 File Offset: 0x00019108
		// (set) Token: 0x0600081A RID: 2074 RVA: 0x0001AF10 File Offset: 0x00019110
		[DataSourceProperty]
		public bool IsRankInfoLoading
		{
			get
			{
				return this._isRankInfoLoading;
			}
			set
			{
				if (value != this._isRankInfoLoading)
				{
					this._isRankInfoLoading = value;
					base.OnPropertyChangedWithValue(value, "IsRankInfoLoading");
				}
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x0001AF2E File Offset: 0x0001912E
		// (set) Token: 0x0600081C RID: 2076 RVA: 0x0001AF36 File Offset: 0x00019136
		[DataSourceProperty]
		public bool IsRankInfoCasual
		{
			get
			{
				return this._isRankInfoCasual;
			}
			set
			{
				if (value != this._isRankInfoCasual)
				{
					this._isRankInfoCasual = value;
					base.OnPropertyChangedWithValue(value, "IsRankInfoCasual");
				}
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x0001AF54 File Offset: 0x00019154
		// (set) Token: 0x0600081E RID: 2078 RVA: 0x0001AF5C File Offset: 0x0001915C
		[DataSourceProperty]
		public bool IsClanInfoSupported
		{
			get
			{
				return this._isClanInfoSupported;
			}
			set
			{
				if (value != this._isClanInfoSupported)
				{
					this._isClanInfoSupported = value;
					base.OnPropertyChangedWithValue(value, "IsClanInfoSupported");
				}
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x0001AF7A File Offset: 0x0001917A
		// (set) Token: 0x06000820 RID: 2080 RVA: 0x0001AF82 File Offset: 0x00019182
		[DataSourceProperty]
		public bool IsBannerlordIDSupported
		{
			get
			{
				return this._isBannerlordIDSupported;
			}
			set
			{
				if (value != this._isBannerlordIDSupported)
				{
					this._isBannerlordIDSupported = value;
					base.OnPropertyChangedWithValue(value, "IsBannerlordIDSupported");
				}
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x0001AFA0 File Offset: 0x000191A0
		// (set) Token: 0x06000822 RID: 2082 RVA: 0x0001AFA8 File Offset: 0x000191A8
		[DataSourceProperty]
		public int Level
		{
			get
			{
				return this._level;
			}
			set
			{
				if (value != this._level)
				{
					this._level = value;
					base.OnPropertyChangedWithValue(value, "Level");
				}
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x0001AFC6 File Offset: 0x000191C6
		// (set) Token: 0x06000824 RID: 2084 RVA: 0x0001AFCE File Offset: 0x000191CE
		[DataSourceProperty]
		public int Rating
		{
			get
			{
				return this._rating;
			}
			set
			{
				if (value != this._rating)
				{
					this._rating = value;
					base.OnPropertyChangedWithValue(value, "Rating");
				}
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x0001AFEC File Offset: 0x000191EC
		// (set) Token: 0x06000826 RID: 2086 RVA: 0x0001AFF4 File Offset: 0x000191F4
		[DataSourceProperty]
		public int Loot
		{
			get
			{
				return this._loot;
			}
			set
			{
				if (value != this._loot)
				{
					this._loot = value;
					base.OnPropertyChangedWithValue(value, "Loot");
				}
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x0001B012 File Offset: 0x00019212
		// (set) Token: 0x06000828 RID: 2088 RVA: 0x0001B01A File Offset: 0x0001921A
		[DataSourceProperty]
		public int ExperienceRatio
		{
			get
			{
				return this._experienceRatio;
			}
			set
			{
				if (value != this._experienceRatio)
				{
					this._experienceRatio = value;
					base.OnPropertyChangedWithValue(value, "ExperienceRatio");
				}
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x0001B038 File Offset: 0x00019238
		// (set) Token: 0x0600082A RID: 2090 RVA: 0x0001B040 File Offset: 0x00019240
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

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x0001B05E File Offset: 0x0001925E
		// (set) Token: 0x0600082C RID: 2092 RVA: 0x0001B066 File Offset: 0x00019266
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x0001B089 File Offset: 0x00019289
		// (set) Token: 0x0600082E RID: 2094 RVA: 0x0001B091 File Offset: 0x00019291
		[DataSourceProperty]
		public string StateText
		{
			get
			{
				return this._stateText;
			}
			set
			{
				if (value != this._stateText)
				{
					this._stateText = value;
					base.OnPropertyChangedWithValue<string>(value, "StateText");
				}
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x0001B0B4 File Offset: 0x000192B4
		// (set) Token: 0x06000830 RID: 2096 RVA: 0x0001B0BC File Offset: 0x000192BC
		[DataSourceProperty]
		public string LevelText
		{
			get
			{
				return this._levelText;
			}
			set
			{
				if (value != this._levelText)
				{
					this._levelText = value;
					base.OnPropertyChangedWithValue<string>(value, "LevelText");
				}
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x0001B0DF File Offset: 0x000192DF
		// (set) Token: 0x06000832 RID: 2098 RVA: 0x0001B0E7 File Offset: 0x000192E7
		[DataSourceProperty]
		public string LevelTitleText
		{
			get
			{
				return this._levelTitleText;
			}
			set
			{
				if (value != this._levelTitleText)
				{
					this._levelTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "LevelTitleText");
				}
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x0001B10A File Offset: 0x0001930A
		// (set) Token: 0x06000834 RID: 2100 RVA: 0x0001B112 File Offset: 0x00019312
		[DataSourceProperty]
		public string RatingText
		{
			get
			{
				return this._ratingText;
			}
			set
			{
				if (value != this._ratingText)
				{
					this._ratingText = value;
					base.OnPropertyChangedWithValue<string>(value, "RatingText");
				}
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x0001B135 File Offset: 0x00019335
		// (set) Token: 0x06000836 RID: 2102 RVA: 0x0001B13D File Offset: 0x0001933D
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

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000837 RID: 2103 RVA: 0x0001B160 File Offset: 0x00019360
		// (set) Token: 0x06000838 RID: 2104 RVA: 0x0001B168 File Offset: 0x00019368
		[DataSourceProperty]
		public string RatingID
		{
			get
			{
				return this._ratingID;
			}
			set
			{
				if (value != this._ratingID)
				{
					this._ratingID = value;
					base.OnPropertyChangedWithValue<string>(value, "RatingID");
				}
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x0001B18B File Offset: 0x0001938B
		// (set) Token: 0x0600083A RID: 2106 RVA: 0x0001B193 File Offset: 0x00019393
		[DataSourceProperty]
		public string ClanName
		{
			get
			{
				return this._clanName;
			}
			set
			{
				if (value != this._clanName)
				{
					this._clanName = value;
					base.OnPropertyChangedWithValue<string>(value, "ClanName");
				}
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x0001B1B6 File Offset: 0x000193B6
		// (set) Token: 0x0600083C RID: 2108 RVA: 0x0001B1BE File Offset: 0x000193BE
		[DataSourceProperty]
		public string ClanTag
		{
			get
			{
				return this._clanTag;
			}
			set
			{
				if (value != this._clanTag)
				{
					this._clanTag = value;
					base.OnPropertyChangedWithValue<string>(value, "ClanTag");
				}
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x0001B1E1 File Offset: 0x000193E1
		// (set) Token: 0x0600083E RID: 2110 RVA: 0x0001B1E9 File Offset: 0x000193E9
		[DataSourceProperty]
		public string ChangeText
		{
			get
			{
				return this._changeText;
			}
			set
			{
				if (value != this._changeText)
				{
					this._changeText = value;
					base.OnPropertyChangedWithValue<string>(value, "ChangeText");
				}
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x0001B20C File Offset: 0x0001940C
		// (set) Token: 0x06000840 RID: 2112 RVA: 0x0001B214 File Offset: 0x00019414
		[DataSourceProperty]
		public string ClanInfoTitleText
		{
			get
			{
				return this._clanInfoTitleText;
			}
			set
			{
				if (value != this._clanInfoTitleText)
				{
					this._clanInfoTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClanInfoTitleText");
				}
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x0001B237 File Offset: 0x00019437
		// (set) Token: 0x06000842 RID: 2114 RVA: 0x0001B23F File Offset: 0x0001943F
		[DataSourceProperty]
		public string BadgeInfoTitleText
		{
			get
			{
				return this._badgeInfoTitleText;
			}
			set
			{
				if (value != this._badgeInfoTitleText)
				{
					this._badgeInfoTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "BadgeInfoTitleText");
				}
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000843 RID: 2115 RVA: 0x0001B262 File Offset: 0x00019462
		// (set) Token: 0x06000844 RID: 2116 RVA: 0x0001B26A File Offset: 0x0001946A
		[DataSourceProperty]
		public string AvatarInfoTitleText
		{
			get
			{
				return this._avatarInfoTitleText;
			}
			set
			{
				if (value != this._avatarInfoTitleText)
				{
					this._avatarInfoTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "AvatarInfoTitleText");
				}
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000845 RID: 2117 RVA: 0x0001B28D File Offset: 0x0001948D
		// (set) Token: 0x06000846 RID: 2118 RVA: 0x0001B295 File Offset: 0x00019495
		[DataSourceProperty]
		public string ExperienceText
		{
			get
			{
				return this._experienceText;
			}
			set
			{
				if (value != this._experienceText)
				{
					this._experienceText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExperienceText");
				}
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x0001B2B8 File Offset: 0x000194B8
		// (set) Token: 0x06000848 RID: 2120 RVA: 0x0001B2C0 File Offset: 0x000194C0
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

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x0001B2E3 File Offset: 0x000194E3
		// (set) Token: 0x0600084A RID: 2122 RVA: 0x0001B2EB File Offset: 0x000194EB
		[DataSourceProperty]
		public string BannerlordID
		{
			get
			{
				return this._bannerlordID;
			}
			set
			{
				if (value != this._bannerlordID)
				{
					this._bannerlordID = value;
					base.OnPropertyChangedWithValue<string>(value, "BannerlordID");
				}
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x0001B30E File Offset: 0x0001950E
		// (set) Token: 0x0600084C RID: 2124 RVA: 0x0001B316 File Offset: 0x00019516
		[DataSourceProperty]
		public string SelectedBadgeID
		{
			get
			{
				return this._selectedBadgeID;
			}
			set
			{
				if (value != this._selectedBadgeID)
				{
					this._selectedBadgeID = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectedBadgeID");
				}
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x0001B339 File Offset: 0x00019539
		// (set) Token: 0x0600084E RID: 2126 RVA: 0x0001B341 File Offset: 0x00019541
		[DataSourceProperty]
		public HintViewModel NameHint
		{
			get
			{
				return this._nameHint;
			}
			set
			{
				if (value != this._nameHint)
				{
					this._nameHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "NameHint");
				}
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x0001B35F File Offset: 0x0001955F
		// (set) Token: 0x06000850 RID: 2128 RVA: 0x0001B367 File Offset: 0x00019567
		[DataSourceProperty]
		public HintViewModel InviteToPartyHint
		{
			get
			{
				return this._inviteToPartyHint;
			}
			set
			{
				if (value != this._inviteToPartyHint)
				{
					this._inviteToPartyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "InviteToPartyHint");
				}
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000851 RID: 2129 RVA: 0x0001B385 File Offset: 0x00019585
		// (set) Token: 0x06000852 RID: 2130 RVA: 0x0001B38D File Offset: 0x0001958D
		[DataSourceProperty]
		public HintViewModel RemoveFriendHint
		{
			get
			{
				return this._removeFriendHint;
			}
			set
			{
				if (value != this._removeFriendHint)
				{
					this._removeFriendHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RemoveFriendHint");
				}
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x0001B3AB File Offset: 0x000195AB
		// (set) Token: 0x06000854 RID: 2132 RVA: 0x0001B3B3 File Offset: 0x000195B3
		[DataSourceProperty]
		public HintViewModel AcceptFriendRequestHint
		{
			get
			{
				return this._acceptFriendRequestHint;
			}
			set
			{
				if (value != this._acceptFriendRequestHint)
				{
					this._acceptFriendRequestHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AcceptFriendRequestHint");
				}
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000855 RID: 2133 RVA: 0x0001B3D1 File Offset: 0x000195D1
		// (set) Token: 0x06000856 RID: 2134 RVA: 0x0001B3D9 File Offset: 0x000195D9
		[DataSourceProperty]
		public HintViewModel DeclineFriendRequestHint
		{
			get
			{
				return this._declineFriendRequestHint;
			}
			set
			{
				if (value != this._declineFriendRequestHint)
				{
					this._declineFriendRequestHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DeclineFriendRequestHint");
				}
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x0001B3F7 File Offset: 0x000195F7
		// (set) Token: 0x06000858 RID: 2136 RVA: 0x0001B3FF File Offset: 0x000195FF
		[DataSourceProperty]
		public HintViewModel CancelFriendRequestHint
		{
			get
			{
				return this._cancelFriendRequestHint;
			}
			set
			{
				if (value != this._cancelFriendRequestHint)
				{
					this._cancelFriendRequestHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CancelFriendRequestHint");
				}
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x0001B41D File Offset: 0x0001961D
		// (set) Token: 0x0600085A RID: 2138 RVA: 0x0001B425 File Offset: 0x00019625
		[DataSourceProperty]
		public HintViewModel InviteToClanHint
		{
			get
			{
				return this._inviteToClanHint;
			}
			set
			{
				if (value != this._inviteToClanHint)
				{
					this._inviteToClanHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "InviteToClanHint");
				}
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x0001B443 File Offset: 0x00019643
		// (set) Token: 0x0600085C RID: 2140 RVA: 0x0001B44B File Offset: 0x0001964B
		[DataSourceProperty]
		public HintViewModel ChangeBannerlordIDHint
		{
			get
			{
				return this._changeBannerlordIDHint;
			}
			set
			{
				if (value != this._changeBannerlordIDHint)
				{
					this._changeBannerlordIDHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ChangeBannerlordIDHint");
				}
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x0001B469 File Offset: 0x00019669
		// (set) Token: 0x0600085E RID: 2142 RVA: 0x0001B471 File Offset: 0x00019671
		[DataSourceProperty]
		public HintViewModel CopyBannerlordIDHint
		{
			get
			{
				return this._copyBannerlordIDHint;
			}
			set
			{
				if (value != this._copyBannerlordIDHint)
				{
					this._copyBannerlordIDHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CopyBannerlordIDHint");
				}
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x0001B48F File Offset: 0x0001968F
		// (set) Token: 0x06000860 RID: 2144 RVA: 0x0001B497 File Offset: 0x00019697
		[DataSourceProperty]
		public HintViewModel AddFriendWithBannerlordIDHint
		{
			get
			{
				return this._addFriendWithBannerlordIDHint;
			}
			set
			{
				if (value != this._addFriendWithBannerlordIDHint)
				{
					this._addFriendWithBannerlordIDHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AddFriendWithBannerlordIDHint");
				}
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x0001B4B5 File Offset: 0x000196B5
		// (set) Token: 0x06000862 RID: 2146 RVA: 0x0001B4BD File Offset: 0x000196BD
		[DataSourceProperty]
		public HintViewModel ExperienceHint
		{
			get
			{
				return this._experienceHint;
			}
			set
			{
				if (value != this._experienceHint)
				{
					this._experienceHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ExperienceHint");
				}
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x0001B4DB File Offset: 0x000196DB
		// (set) Token: 0x06000864 RID: 2148 RVA: 0x0001B4E3 File Offset: 0x000196E3
		[DataSourceProperty]
		public HintViewModel RatingHint
		{
			get
			{
				return this._ratingHint;
			}
			set
			{
				if (value != this._ratingHint)
				{
					this._ratingHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RatingHint");
				}
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x0001B501 File Offset: 0x00019701
		// (set) Token: 0x06000866 RID: 2150 RVA: 0x0001B509 File Offset: 0x00019709
		[DataSourceProperty]
		public HintViewModel LootHint
		{
			get
			{
				return this._lootHint;
			}
			set
			{
				if (value != this._lootHint)
				{
					this._lootHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LootHint");
				}
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x0001B527 File Offset: 0x00019727
		// (set) Token: 0x06000868 RID: 2152 RVA: 0x0001B52F File Offset: 0x0001972F
		[DataSourceProperty]
		public HintViewModel SkirmishRatingHint
		{
			get
			{
				return this._skirmishRatingHint;
			}
			set
			{
				if (value != this._skirmishRatingHint)
				{
					this._skirmishRatingHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "SkirmishRatingHint");
				}
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000869 RID: 2153 RVA: 0x0001B54D File Offset: 0x0001974D
		// (set) Token: 0x0600086A RID: 2154 RVA: 0x0001B555 File Offset: 0x00019755
		[DataSourceProperty]
		public HintViewModel CaptainRatingHint
		{
			get
			{
				return this._captainRatingHint;
			}
			set
			{
				if (value != this._captainRatingHint)
				{
					this._captainRatingHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CaptainRatingHint");
				}
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x0600086B RID: 2155 RVA: 0x0001B573 File Offset: 0x00019773
		// (set) Token: 0x0600086C RID: 2156 RVA: 0x0001B57B File Offset: 0x0001977B
		[DataSourceProperty]
		public HintViewModel ClanLeaderboardHint
		{
			get
			{
				return this._clanLeaderboardHint;
			}
			set
			{
				if (value != this._clanLeaderboardHint)
				{
					this._clanLeaderboardHint = value;
					base.OnPropertyChanged("ClanLeaderboardHint");
				}
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x0001B598 File Offset: 0x00019798
		// (set) Token: 0x0600086E RID: 2158 RVA: 0x0001B5A0 File Offset: 0x000197A0
		[DataSourceProperty]
		public PlayerAvatarImageIdentifierVM Avatar
		{
			get
			{
				return this._avatar;
			}
			set
			{
				if (value != this._avatar)
				{
					this._avatar = value;
					base.OnPropertyChangedWithValue<PlayerAvatarImageIdentifierVM>(value, "Avatar");
				}
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x0001B5BE File Offset: 0x000197BE
		// (set) Token: 0x06000870 RID: 2160 RVA: 0x0001B5C6 File Offset: 0x000197C6
		[DataSourceProperty]
		public BannerImageIdentifierVM ClanBanner
		{
			get
			{
				return this._clanBanner;
			}
			set
			{
				if (value != this._clanBanner)
				{
					this._clanBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "ClanBanner");
				}
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x0001B5E4 File Offset: 0x000197E4
		// (set) Token: 0x06000872 RID: 2162 RVA: 0x0001B5EC File Offset: 0x000197EC
		[DataSourceProperty]
		public MPLobbySigilItemVM Sigil
		{
			get
			{
				return this._sigil;
			}
			set
			{
				if (value != this._sigil)
				{
					this._sigil = value;
					base.OnPropertyChangedWithValue<MPLobbySigilItemVM>(value, "Sigil");
				}
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000873 RID: 2163 RVA: 0x0001B60A File Offset: 0x0001980A
		// (set) Token: 0x06000874 RID: 2164 RVA: 0x0001B612 File Offset: 0x00019812
		[DataSourceProperty]
		public MPLobbyBadgeItemVM ShownBadge
		{
			get
			{
				return this._shownBadge;
			}
			set
			{
				if (value != this._shownBadge)
				{
					this._shownBadge = value;
					base.OnPropertyChangedWithValue<MPLobbyBadgeItemVM>(value, "ShownBadge");
				}
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x0001B630 File Offset: 0x00019830
		// (set) Token: 0x06000876 RID: 2166 RVA: 0x0001B638 File Offset: 0x00019838
		[DataSourceProperty]
		public CharacterViewModel CharacterVisual
		{
			get
			{
				return this._characterVisual;
			}
			set
			{
				if (value != this._characterVisual)
				{
					this._characterVisual = value;
					base.OnPropertyChangedWithValue<CharacterViewModel>(value, "CharacterVisual");
				}
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x0001B656 File Offset: 0x00019856
		// (set) Token: 0x06000878 RID: 2168 RVA: 0x0001B65E File Offset: 0x0001985E
		[DataSourceProperty]
		public MBBindingList<MPLobbyPlayerStatItemVM> DisplayedStats
		{
			get
			{
				return this._displayedStats;
			}
			set
			{
				if (value != this._displayedStats)
				{
					this._displayedStats = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyPlayerStatItemVM>>(value, "DisplayedStats");
				}
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x0001B67C File Offset: 0x0001987C
		// (set) Token: 0x0600087A RID: 2170 RVA: 0x0001B684 File Offset: 0x00019884
		[DataSourceProperty]
		public MBBindingList<MPLobbyGameTypeVM> GameTypes
		{
			get
			{
				return this._gameTypes;
			}
			set
			{
				if (value != this._gameTypes)
				{
					this._gameTypes = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyGameTypeVM>>(value, "GameTypes");
				}
			}
		}

		// Token: 0x04000394 RID: 916
		public static Action<PlayerId> OnPlayerProfileRequested;

		// Token: 0x04000395 RID: 917
		public static Action<PlayerId> OnBannerlordIDChangeRequested;

		// Token: 0x04000396 RID: 918
		public static Action<PlayerId> OnAddFriendWithBannerlordIDRequested;

		// Token: 0x04000397 RID: 919
		public static Action<PlayerId> OnSigilChangeRequested;

		// Token: 0x04000398 RID: 920
		public static Action<PlayerId> OnBadgeChangeRequested;

		// Token: 0x04000399 RID: 921
		public static Action<MPLobbyPlayerBaseVM> OnRankProgressionRequested;

		// Token: 0x0400039A RID: 922
		public static Action<string> OnRankLeaderboardRequested;

		// Token: 0x0400039B RID: 923
		public static Action OnClanPageRequested;

		// Token: 0x0400039C RID: 924
		public static Action OnClanLeaderboardRequested;

		// Token: 0x0400039D RID: 925
		private const int DefaultBannerBackgroundColorId = 99;

		// Token: 0x0400039F RID: 927
		private PlayerId _providedID;

		// Token: 0x040003A0 RID: 928
		private readonly string _forcedName = string.Empty;

		// Token: 0x040003A1 RID: 929
		private int _forcedAvatarIndex = -1;

		// Token: 0x040003A3 RID: 931
		private Action<PlayerId> _onInviteToParty;

		// Token: 0x040003A4 RID: 932
		private readonly Action<PlayerId> _onInviteToClan;

		// Token: 0x040003A5 RID: 933
		private readonly Action<PlayerId> _onFriendRequestAnswered;

		// Token: 0x040003A8 RID: 936
		private bool _isKnownPlayer;

		// Token: 0x040003A9 RID: 937
		private readonly TextObject _genericPlayerName = new TextObject("{=RN6zHak0}Player", null);

		// Token: 0x040003AA RID: 938
		public Action OnPlayerStatsReceived;

		// Token: 0x040003AB RID: 939
		protected bool _hasReceivedPlayerStats;

		// Token: 0x040003AC RID: 940
		protected bool _isReceivingPlayerStats;

		// Token: 0x040003B0 RID: 944
		private const string _skirmishGameTypeID = "Skirmish";

		// Token: 0x040003B1 RID: 945
		private const string _captainGameTypeID = "Captain";

		// Token: 0x040003B2 RID: 946
		private const string _duelGameTypeID = "Duel";

		// Token: 0x040003B3 RID: 947
		private const string _teamDeathmatchGameTypeID = "TeamDeathmatch";

		// Token: 0x040003B4 RID: 948
		private const string _siegeGameTypeID = "Siege";

		// Token: 0x040003B5 RID: 949
		public Action<string> OnRankInfoChanged;

		// Token: 0x040003B6 RID: 950
		private bool _canCopyID;

		// Token: 0x040003B7 RID: 951
		private bool _showLevel;

		// Token: 0x040003B8 RID: 952
		private bool _isSelected;

		// Token: 0x040003B9 RID: 953
		private bool _hasNotification;

		// Token: 0x040003BA RID: 954
		private bool _isFriendRequest;

		// Token: 0x040003BB RID: 955
		private bool _isPendingRequest;

		// Token: 0x040003BC RID: 956
		private bool _canRemove;

		// Token: 0x040003BD RID: 957
		private bool _canBeInvited;

		// Token: 0x040003BE RID: 958
		private bool _canInviteToParty;

		// Token: 0x040003BF RID: 959
		private bool _canInviteToClan;

		// Token: 0x040003C0 RID: 960
		private bool _isSigilChangeInformationEnabled;

		// Token: 0x040003C1 RID: 961
		private bool _isRankInfoLoading;

		// Token: 0x040003C2 RID: 962
		private bool _isRankInfoCasual;

		// Token: 0x040003C3 RID: 963
		private bool _isClanInfoSupported;

		// Token: 0x040003C4 RID: 964
		private bool _isBannerlordIDSupported;

		// Token: 0x040003C5 RID: 965
		private int _level;

		// Token: 0x040003C6 RID: 966
		private int _rating;

		// Token: 0x040003C7 RID: 967
		private int _loot;

		// Token: 0x040003C8 RID: 968
		private int _experienceRatio;

		// Token: 0x040003C9 RID: 969
		private int _ratingRatio;

		// Token: 0x040003CA RID: 970
		private string _name = "";

		// Token: 0x040003CB RID: 971
		private string _stateText;

		// Token: 0x040003CC RID: 972
		private string _levelText;

		// Token: 0x040003CD RID: 973
		private string _levelTitleText;

		// Token: 0x040003CE RID: 974
		private string _ratingText;

		// Token: 0x040003CF RID: 975
		private string _gameTypeText;

		// Token: 0x040003D0 RID: 976
		private string _ratingID;

		// Token: 0x040003D1 RID: 977
		private string _clanName;

		// Token: 0x040003D2 RID: 978
		private string _clanTag;

		// Token: 0x040003D3 RID: 979
		private string _changeText;

		// Token: 0x040003D4 RID: 980
		private string _clanInfoTitleText;

		// Token: 0x040003D5 RID: 981
		private string _badgeInfoTitleText;

		// Token: 0x040003D6 RID: 982
		private string _avatarInfoTitleText;

		// Token: 0x040003D7 RID: 983
		private string _experienceText;

		// Token: 0x040003D8 RID: 984
		private string _rankText;

		// Token: 0x040003D9 RID: 985
		private string _bannerlordID;

		// Token: 0x040003DA RID: 986
		private string _selectedBadgeID;

		// Token: 0x040003DB RID: 987
		private HintViewModel _nameHint;

		// Token: 0x040003DC RID: 988
		private HintViewModel _inviteToPartyHint;

		// Token: 0x040003DD RID: 989
		private HintViewModel _removeFriendHint;

		// Token: 0x040003DE RID: 990
		private HintViewModel _acceptFriendRequestHint;

		// Token: 0x040003DF RID: 991
		private HintViewModel _declineFriendRequestHint;

		// Token: 0x040003E0 RID: 992
		private HintViewModel _cancelFriendRequestHint;

		// Token: 0x040003E1 RID: 993
		private HintViewModel _inviteToClanHint;

		// Token: 0x040003E2 RID: 994
		private HintViewModel _changeBannerlordIDHint;

		// Token: 0x040003E3 RID: 995
		private HintViewModel _copyBannerlordIDHint;

		// Token: 0x040003E4 RID: 996
		private HintViewModel _addFriendWithBannerlordIDHint;

		// Token: 0x040003E5 RID: 997
		private HintViewModel _experienceHint;

		// Token: 0x040003E6 RID: 998
		private HintViewModel _ratingHint;

		// Token: 0x040003E7 RID: 999
		private HintViewModel _lootHint;

		// Token: 0x040003E8 RID: 1000
		private HintViewModel _skirmishRatingHint;

		// Token: 0x040003E9 RID: 1001
		private HintViewModel _captainRatingHint;

		// Token: 0x040003EA RID: 1002
		private HintViewModel _clanLeaderboardHint;

		// Token: 0x040003EB RID: 1003
		private PlayerAvatarImageIdentifierVM _avatar;

		// Token: 0x040003EC RID: 1004
		private BannerImageIdentifierVM _clanBanner;

		// Token: 0x040003ED RID: 1005
		private MPLobbySigilItemVM _sigil;

		// Token: 0x040003EE RID: 1006
		private MPLobbyBadgeItemVM _shownBadge;

		// Token: 0x040003EF RID: 1007
		private CharacterViewModel _characterVisual;

		// Token: 0x040003F0 RID: 1008
		private MBBindingList<MPLobbyPlayerStatItemVM> _displayedStats;

		// Token: 0x040003F1 RID: 1009
		private MBBindingList<MPLobbyGameTypeVM> _gameTypes;

		// Token: 0x0200011E RID: 286
		public enum OnlineStatus
		{
			// Token: 0x04000916 RID: 2326
			None,
			// Token: 0x04000917 RID: 2327
			InGame,
			// Token: 0x04000918 RID: 2328
			Online,
			// Token: 0x04000919 RID: 2329
			Offline
		}
	}
}
