using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Messages.FromClient.ToLobbyServer;
using Messages.FromLobbyServer.ToClient;
using TaleWorlds.Core;
using TaleWorlds.Diamond;
using TaleWorlds.Diamond.ClientApplication;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;
using TaleWorlds.MountAndBlade.Diamond.Ranked;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000124 RID: 292
	public class LobbyClient : Client<LobbyClient>
	{
		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000690 RID: 1680 RVA: 0x00008854 File Offset: 0x00006A54
		// (set) Token: 0x06000691 RID: 1681 RVA: 0x0000885B File Offset: 0x00006A5B
		private static int FriendListCheckDelay
		{
			get
			{
				return LobbyClient._friendListCheckDelay;
			}
			set
			{
				if (value != LobbyClient._friendListCheckDelay)
				{
					LobbyClient._friendListCheckDelay = value;
				}
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x0000886B File Offset: 0x00006A6B
		// (set) Token: 0x06000693 RID: 1683 RVA: 0x00008873 File Offset: 0x00006A73
		public PlayerData PlayerData { get; private set; }

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000694 RID: 1684 RVA: 0x0000887C File Offset: 0x00006A7C
		// (set) Token: 0x06000695 RID: 1685 RVA: 0x00008884 File Offset: 0x00006A84
		public SupportedFeatures SupportedFeatures { get; private set; }

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x0000888D File Offset: 0x00006A8D
		// (set) Token: 0x06000697 RID: 1687 RVA: 0x00008895 File Offset: 0x00006A95
		public ClanInfo ClanInfo { get; private set; }

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x0000889E File Offset: 0x00006A9E
		// (set) Token: 0x06000699 RID: 1689 RVA: 0x000088A6 File Offset: 0x00006AA6
		public ClanHomeInfo ClanHomeInfo { get; private set; }

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x000088AF File Offset: 0x00006AAF
		public IReadOnlyList<string> OwnedCosmetics
		{
			get
			{
				return this._ownedCosmetics;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x000088B7 File Offset: 0x00006AB7
		public IReadOnlyDictionary<string, List<string>> UsedCosmetics
		{
			get
			{
				return this._usedCosmetics;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x000088BF File Offset: 0x00006ABF
		// (set) Token: 0x0600069D RID: 1693 RVA: 0x000088C7 File Offset: 0x00006AC7
		public AvailableScenes AvailableScenes { get; private set; }

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x000088D0 File Offset: 0x00006AD0
		public PlayerId PlayerID
		{
			get
			{
				return this._playerId;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x000088D8 File Offset: 0x00006AD8
		// (set) Token: 0x060006A0 RID: 1696 RVA: 0x000088E0 File Offset: 0x00006AE0
		public bool IsRefreshingPlayerData { get; set; }

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x000088E9 File Offset: 0x00006AE9
		// (set) Token: 0x060006A2 RID: 1698 RVA: 0x000088F4 File Offset: 0x00006AF4
		public LobbyClient.State CurrentState
		{
			get
			{
				return this._state;
			}
			private set
			{
				if (this._state != value)
				{
					LobbyClient.State state = this._state;
					this._state = value;
					ILobbyClientSessionHandler handler = this._handler;
					if (handler == null)
					{
						return;
					}
					handler.OnGameClientStateChange(state);
				}
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x0000892C File Offset: 0x00006B2C
		public override long AliveCheckTimeInMiliSeconds
		{
			get
			{
				switch (this.CurrentState)
				{
				case LobbyClient.State.Idle:
				case LobbyClient.State.Working:
				case LobbyClient.State.Connected:
				case LobbyClient.State.SessionRequested:
				case LobbyClient.State.AtLobby:
					return 6000L;
				case LobbyClient.State.SearchingToRejoinBattle:
				case LobbyClient.State.RequestingToSearchBattle:
				case LobbyClient.State.RequestingToCancelSearchBattle:
				case LobbyClient.State.SearchingBattle:
				case LobbyClient.State.QuittingFromBattle:
				case LobbyClient.State.WaitingToRegisterCustomGame:
				case LobbyClient.State.HostingCustomGame:
				case LobbyClient.State.WaitingToJoinCustomGame:
					return 3500L;
				case LobbyClient.State.AtBattle:
				case LobbyClient.State.InCustomGame:
					return 60000L;
				}
				return 1000L;
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060006A4 RID: 1700 RVA: 0x000089A7 File Offset: 0x00006BA7
		public bool AtLobby
		{
			get
			{
				return this.CurrentState == LobbyClient.State.AtLobby;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x000089B2 File Offset: 0x00006BB2
		public bool CanPerformLobbyActions
		{
			get
			{
				return this.CurrentState == LobbyClient.State.AtLobby || this.CurrentState == LobbyClient.State.RequestingToSearchBattle || this.CurrentState == LobbyClient.State.SearchingBattle || this.CurrentState == LobbyClient.State.WaitingToJoinCustomGame;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060006A6 RID: 1702 RVA: 0x000089DB File Offset: 0x00006BDB
		public string Name
		{
			get
			{
				return this._userName;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x000089E3 File Offset: 0x00006BE3
		// (set) Token: 0x060006A8 RID: 1704 RVA: 0x000089EB File Offset: 0x00006BEB
		public string LastBattleServerAddressForClient { get; private set; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x000089F4 File Offset: 0x00006BF4
		// (set) Token: 0x060006AA RID: 1706 RVA: 0x000089FC File Offset: 0x00006BFC
		public ushort LastBattleServerPortForClient { get; private set; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x00008A05 File Offset: 0x00006C05
		// (set) Token: 0x060006AC RID: 1708 RVA: 0x00008A0D File Offset: 0x00006C0D
		public bool LastBattleIsOfficial { get; private set; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x00008A16 File Offset: 0x00006C16
		public bool Connected
		{
			get
			{
				return this.CurrentState != LobbyClient.State.Working && this.CurrentState > LobbyClient.State.Idle;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x00008A2C File Offset: 0x00006C2C
		public bool IsIdle
		{
			get
			{
				return this.CurrentState == LobbyClient.State.Idle;
			}
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00008A37 File Offset: 0x00006C37
		public void Logout(TextObject logOutReason)
		{
			base.BeginDisconnect();
			this._logOutReason = logOutReason;
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x00008A46 File Offset: 0x00006C46
		public bool LoggedIn
		{
			get
			{
				return this.CurrentState != LobbyClient.State.Idle && this.CurrentState != LobbyClient.State.Working && this.CurrentState != LobbyClient.State.Connected && this.CurrentState != LobbyClient.State.SessionRequested;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x00008A70 File Offset: 0x00006C70
		public bool IsInGame
		{
			get
			{
				return this.CurrentState == LobbyClient.State.AtBattle || this.CurrentState == LobbyClient.State.HostingCustomGame || this.CurrentState == LobbyClient.State.InCustomGame;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x00008A92 File Offset: 0x00006C92
		public bool IsHostingCustomGame
		{
			get
			{
				return this._state == LobbyClient.State.HostingCustomGame;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x00008A9E File Offset: 0x00006C9E
		public bool IsMatchmakingAvailable
		{
			get
			{
				ServerStatus serverStatus = this._serverStatus;
				return serverStatus != null && serverStatus.IsMatchmakingEnabled;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x00008AB1 File Offset: 0x00006CB1
		public bool IsAbleToSearchForGame
		{
			get
			{
				return this.IsMatchmakingAvailable && this._matchmakerBlockedTime <= DateTime.Now;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060006B5 RID: 1717 RVA: 0x00008ACD File Offset: 0x00006CCD
		public bool PartySystemAvailable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x00008AD0 File Offset: 0x00006CD0
		public bool IsCustomBattleAvailable
		{
			get
			{
				ServerStatus serverStatus = this._serverStatus;
				return serverStatus != null && serverStatus.IsCustomBattleEnabled;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x00008AE3 File Offset: 0x00006CE3
		public IReadOnlyList<ModuleInfoModel> LoadedUnofficialModules
		{
			get
			{
				return this._loadedUnofficialModules;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x00008AEB File Offset: 0x00006CEB
		public bool HasUnofficialModulesLoaded
		{
			get
			{
				return this.LoadedUnofficialModules.Count > 0;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x00008AFB File Offset: 0x00006CFB
		// (set) Token: 0x060006BA RID: 1722 RVA: 0x00008B03 File Offset: 0x00006D03
		public bool HasUserGeneratedContentPrivilege { get; private set; }

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x00008B0C File Offset: 0x00006D0C
		public bool IsPartyLeader
		{
			get
			{
				if (this.Connected)
				{
					object obj = true;
					PartyPlayerInLobbyClient partyPlayerInLobbyClient = this.PlayersInParty.Find((PartyPlayerInLobbyClient p) => p.PlayerId == this._playerId);
					return object.Equals(obj, (partyPlayerInLobbyClient != null) ? new bool?(partyPlayerInLobbyClient.IsPartyLeader) : null);
				}
				return false;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x00008B63 File Offset: 0x00006D63
		public bool IsClanLeader
		{
			get
			{
				ClanPlayer clanPlayer = this.PlayersInClan.Find((ClanPlayer p) => p.PlayerId == this._playerId);
				return clanPlayer != null && clanPlayer.Role == ClanPlayerRole.Leader;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x00008B8A File Offset: 0x00006D8A
		public bool IsClanOfficer
		{
			get
			{
				ClanPlayer clanPlayer = this.PlayersInClan.Find((ClanPlayer p) => p.PlayerId == this._playerId);
				return clanPlayer != null && clanPlayer.Role == ClanPlayerRole.Officer;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x00008BB1 File Offset: 0x00006DB1
		// (set) Token: 0x060006BF RID: 1727 RVA: 0x00008BB9 File Offset: 0x00006DB9
		public bool IsEligibleToCreatePremadeGame { get; private set; }

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x00008BC2 File Offset: 0x00006DC2
		// (set) Token: 0x060006C1 RID: 1729 RVA: 0x00008BCA File Offset: 0x00006DCA
		public CustomBattleId CustomBattleId { get; private set; }

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x00008BD3 File Offset: 0x00006DD3
		// (set) Token: 0x060006C3 RID: 1731 RVA: 0x00008BDB File Offset: 0x00006DDB
		public string CustomGameType { get; private set; }

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x00008BE4 File Offset: 0x00006DE4
		// (set) Token: 0x060006C5 RID: 1733 RVA: 0x00008BEC File Offset: 0x00006DEC
		public string CustomGameScene { get; private set; }

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x00008BF5 File Offset: 0x00006DF5
		// (set) Token: 0x060006C7 RID: 1735 RVA: 0x00008BFD File Offset: 0x00006DFD
		public AvailableCustomGames AvailableCustomGames { get; private set; }

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x00008C06 File Offset: 0x00006E06
		// (set) Token: 0x060006C9 RID: 1737 RVA: 0x00008C0E File Offset: 0x00006E0E
		public PremadeGameList AvailablePremadeGames { get; private set; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060006CA RID: 1738 RVA: 0x00008C17 File Offset: 0x00006E17
		// (set) Token: 0x060006CB RID: 1739 RVA: 0x00008C1F File Offset: 0x00006E1F
		public List<PartyPlayerInLobbyClient> PlayersInParty { get; private set; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x00008C28 File Offset: 0x00006E28
		// (set) Token: 0x060006CD RID: 1741 RVA: 0x00008C30 File Offset: 0x00006E30
		public List<ClanPlayer> PlayersInClan { get; private set; }

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x00008C39 File Offset: 0x00006E39
		// (set) Token: 0x060006CF RID: 1743 RVA: 0x00008C41 File Offset: 0x00006E41
		public List<ClanPlayerInfo> PlayerInfosInClan { get; private set; }

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x00008C4A File Offset: 0x00006E4A
		// (set) Token: 0x060006D1 RID: 1745 RVA: 0x00008C52 File Offset: 0x00006E52
		public FriendInfo[] FriendInfos { get; private set; }

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x00008C5B File Offset: 0x00006E5B
		public bool IsInParty
		{
			get
			{
				return this.Connected && this.PlayersInParty.Count > 0;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x00008C75 File Offset: 0x00006E75
		public bool IsPartyFull
		{
			get
			{
				return this.PlayersInParty.Count == Parameters.MaxPlayerCountInParty;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x00008C89 File Offset: 0x00006E89
		// (set) Token: 0x060006D5 RID: 1749 RVA: 0x00008C91 File Offset: 0x00006E91
		public string CurrentMatchId { get; private set; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x00008C9A File Offset: 0x00006E9A
		public bool IsInClan
		{
			get
			{
				return this.PlayersInClan.Count > 0;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x00008CAA File Offset: 0x00006EAA
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x00008CB2 File Offset: 0x00006EB2
		public bool IsPartyInvitationPopupActive { get; private set; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x00008CBB File Offset: 0x00006EBB
		// (set) Token: 0x060006DA RID: 1754 RVA: 0x00008CC3 File Offset: 0x00006EC3
		public bool IsPartyJoinRequestPopupActive { get; private set; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x00008CCC File Offset: 0x00006ECC
		public bool CanInvitePlayers
		{
			get
			{
				SupportedFeatures supportedFeatures = this.SupportedFeatures;
				return supportedFeatures != null && supportedFeatures.SupportsFeatures(Features.Party) && (!this.IsInParty || this.IsPartyLeader);
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x00008CF5 File Offset: 0x00006EF5
		public bool CanSuggestPlayers
		{
			get
			{
				SupportedFeatures supportedFeatures = this.SupportedFeatures;
				return supportedFeatures != null && supportedFeatures.SupportsFeatures(Features.Party) && this.IsInParty && !this.IsPartyLeader;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x00008D1F File Offset: 0x00006F1F
		// (set) Token: 0x060006DE RID: 1758 RVA: 0x00008D27 File Offset: 0x00006F27
		public Guid ClanID { get; private set; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x00008D30 File Offset: 0x00006F30
		// (set) Token: 0x060006E0 RID: 1760 RVA: 0x00008D38 File Offset: 0x00006F38
		public List<PlayerId> FriendIDs { get; private set; }

		// Token: 0x060006E1 RID: 1761 RVA: 0x00008D44 File Offset: 0x00006F44
		public LobbyClient(DiamondClientApplication diamondClientApplication, IClientSessionProvider<LobbyClient> sessionProvider)
			: base(diamondClientApplication, sessionProvider, false)
		{
			this._serverStatusTimer = new Stopwatch();
			this._serverStatusTimer.Start();
			this._matchmakerBlockedTime = DateTime.MinValue;
			this._friendListTimer = new Stopwatch();
			this._friendListTimer.Start();
			this._recentPlayersTimer = new Stopwatch();
			this._recentPlayersTimer.Start();
			this.PlayersInParty = new List<PartyPlayerInLobbyClient>();
			this.PlayersInClan = new List<ClanPlayer>();
			this.PlayerInfosInClan = new List<ClanPlayerInfo>();
			this.FriendInfos = new FriendInfo[0];
			this.ClanID = Guid.Empty;
			this.FriendIDs = new List<PlayerId>();
			this.SupportedFeatures = new SupportedFeatures();
			this._ownedCosmetics = new List<string>();
			this._usedCosmetics = new Dictionary<string, List<string>>();
			this._cachedRankInfos = new TimedDictionaryCache<PlayerId, GameTypeRankInfo[]>(TimeSpan.FromSeconds(10.0));
			this._cachedPlayerStats = new TimedDictionaryCache<PlayerId, PlayerStatsBase[]>(TimeSpan.FromSeconds(10.0));
			this._cachedPlayerDatas = new TimedDictionaryCache<PlayerId, PlayerData>(TimeSpan.FromSeconds(10.0));
			this._cachedPlayerBannerlordIDs = new TimedDictionaryCache<PlayerId, string>(TimeSpan.FromSeconds(30.0));
			this._pendingPlayerRequests = new Dictionary<ValueTuple<LobbyClient.PendingRequest, PlayerId>, Task>();
			base.AddMessageHandler<FindGameAnswerMessage>(new ClientMessageHandler<FindGameAnswerMessage>(this.OnFindGameAnswerMessage));
			base.AddMessageHandler<JoinBattleMessage>(new ClientMessageHandler<JoinBattleMessage>(this.OnJoinBattleMessage));
			base.AddMessageHandler<BattleResultMessage>(new ClientMessageHandler<BattleResultMessage>(this.OnBattleResultMessage));
			base.AddMessageHandler<BattleServerLostMessage>(new ClientMessageHandler<BattleServerLostMessage>(this.OnBattleServerLostMessage));
			base.AddMessageHandler<BattleOverMessage>(new ClientMessageHandler<BattleOverMessage>(this.OnBattleOverMessage));
			base.AddMessageHandler<CancelBattleResponseMessage>(new ClientMessageHandler<CancelBattleResponseMessage>(this.OnCancelBattleResponseMessage));
			base.AddMessageHandler<RejoinRequestRejectedMessage>(new ClientMessageHandler<RejoinRequestRejectedMessage>(this.OnRejoinRequestRejectedMessage));
			base.AddMessageHandler<CancelFindGameMessage>(new ClientMessageHandler<CancelFindGameMessage>(this.OnCancelFindGameMessage));
			base.AddMessageHandler<RequestJoinPartyMessage>(new ClientMessageHandler<RequestJoinPartyMessage>(this.OnRequestJoinPartyMessage));
			base.AddMessageHandler<WhisperReceivedMessage>(new ClientMessageHandler<WhisperReceivedMessage>(this.OnWhisperMessageReceivedMessage));
			base.AddMessageHandler<ClanMessageReceivedMessage>(new ClientMessageHandler<ClanMessageReceivedMessage>(this.OnClanMessageReceivedMessage));
			base.AddMessageHandler<PartyMessageReceivedMessage>(new ClientMessageHandler<PartyMessageReceivedMessage>(this.OnPartyMessageReceivedMessage));
			base.AddMessageHandler<SystemMessage>(new ClientMessageHandler<SystemMessage>(this.OnSystemMessage));
			base.AddMessageHandler<InvitationToPartyMessage>(new ClientMessageHandler<InvitationToPartyMessage>(this.OnInvitationToPartyMessage));
			base.AddMessageHandler<PartyInvitationInvalidMessage>(new ClientMessageHandler<PartyInvitationInvalidMessage>(this.OnPartyInvitationInvalidMessage));
			base.AddMessageHandler<UpdatePlayerDataMessage>(new ClientMessageHandler<UpdatePlayerDataMessage>(this.OnUpdatePlayerDataMessage));
			base.AddMessageHandler<RecentPlayerStatusesMessage>(new ClientMessageHandler<RecentPlayerStatusesMessage>(this.OnRecentPlayerStatusesMessage));
			base.AddMessageHandler<PlayerQuitFromMatchmakerGameResult>(new ClientMessageHandler<PlayerQuitFromMatchmakerGameResult>(this.OnPlayerQuitFromMatchmakerGameResult));
			base.AddMessageHandler<PlayerRemovedFromMatchmakerGame>(new ClientMessageHandler<PlayerRemovedFromMatchmakerGame>(this.OnPlayerRemovedFromMatchmakerGameMessage));
			base.AddMessageHandler<EnterBattleWithPartyAnswer>(new ClientMessageHandler<EnterBattleWithPartyAnswer>(this.OnEnterBattleWithPartyAnswerMessage));
			base.AddMessageHandler<JoinCustomGameResultMessage>(new ClientMessageHandler<JoinCustomGameResultMessage>(this.OnJoinCustomGameResultMessage));
			base.AddMessageHandler<ClientWantsToConnectCustomGameMessage>(new ClientMessageHandler<ClientWantsToConnectCustomGameMessage>(this.OnClientWantsToConnectCustomGameMessage));
			base.AddMessageHandler<ClientQuitFromCustomGameMessage>(new ClientMessageHandler<ClientQuitFromCustomGameMessage>(this.OnClientQuitFromCustomGameMessage));
			base.AddMessageHandler<PlayerRemovedFromCustomGame>(new ClientMessageHandler<PlayerRemovedFromCustomGame>(this.OnPlayerRemovedFromCustomGame));
			base.AddMessageHandler<EnterCustomBattleWithPartyAnswer>(new ClientMessageHandler<EnterCustomBattleWithPartyAnswer>(this.OnEnterCustomBattleWithPartyAnswerMessage));
			base.AddMessageHandler<PlayerInvitedToPartyMessage>(new ClientMessageHandler<PlayerInvitedToPartyMessage>(this.OnPlayerInvitedToPartyMessage));
			base.AddMessageHandler<PlayersAddedToPartyMessage>(new ClientMessageHandler<PlayersAddedToPartyMessage>(this.OnPlayerAddedToPartyMessage));
			base.AddMessageHandler<PlayerRemovedFromPartyMessage>(new ClientMessageHandler<PlayerRemovedFromPartyMessage>(this.OnPlayerRemovedFromPartyMessage));
			base.AddMessageHandler<PlayerAssignedPartyLeaderMessage>(new ClientMessageHandler<PlayerAssignedPartyLeaderMessage>(this.OnPlayerAssignedPartyLeaderMessage));
			base.AddMessageHandler<PlayerSuggestedToPartyMessage>(new ClientMessageHandler<PlayerSuggestedToPartyMessage>(this.OnPlayerSuggestedToPartyMessage));
			base.AddMessageHandler<ServerStatusMessage>(new ClientMessageHandler<ServerStatusMessage>(this.OnServerStatusMessage));
			base.AddMessageHandler<MatchmakerDisabledMessage>(new ClientMessageHandler<MatchmakerDisabledMessage>(this.OnMatchmakerDisabledMessage));
			base.AddMessageHandler<FriendListMessage>(new ClientMessageHandler<FriendListMessage>(this.OnFriendListMessage));
			base.AddMessageHandler<AdminMessage>(new ClientMessageHandler<AdminMessage>(this.OnAdminMessage));
			base.AddMessageHandler<CreateClanAnswerMessage>(new ClientMessageHandler<CreateClanAnswerMessage>(this.OnCreateClanAnswerMessage));
			base.AddMessageHandler<ClanCreationRequestMessage>(new ClientMessageHandler<ClanCreationRequestMessage>(this.OnClanCreationRequestMessage));
			base.AddMessageHandler<ClanCreationRequestAnsweredMessage>(new ClientMessageHandler<ClanCreationRequestAnsweredMessage>(this.OnClanCreationRequestAnsweredMessage));
			base.AddMessageHandler<ClanCreationFailedMessage>(new ClientMessageHandler<ClanCreationFailedMessage>(this.OnClanCreationFailedMessage));
			base.AddMessageHandler<ClanCreationSuccessfulMessage>(new ClientMessageHandler<ClanCreationSuccessfulMessage>(this.OnClanCreationSuccessfulMessage));
			base.AddMessageHandler<ClanInfoChangedMessage>(new ClientMessageHandler<ClanInfoChangedMessage>(this.OnClanInfoChangedMessage));
			base.AddMessageHandler<InvitationToClanMessage>(new ClientMessageHandler<InvitationToClanMessage>(this.OnInvitationToClanMessage));
			base.AddMessageHandler<ClanDisbandedMessage>(new ClientMessageHandler<ClanDisbandedMessage>(this.OnClanDisbandedMessage));
			base.AddMessageHandler<KickedFromClanMessage>(new ClientMessageHandler<KickedFromClanMessage>(this.OnKickedFromClan));
			base.AddMessageHandler<PartyPlayerLeftClanMessage>(new ClientMessageHandler<PartyPlayerLeftClanMessage>(this.OnPartyPlayerLeftClan));
			base.AddMessageHandler<JoinPremadeGameAnswerMessage>(new ClientMessageHandler<JoinPremadeGameAnswerMessage>(this.OnJoinPremadeGameAnswerMessage));
			base.AddMessageHandler<PremadeGameEligibilityStatusMessage>(new ClientMessageHandler<PremadeGameEligibilityStatusMessage>(this.OnPremadeGameEligibilityStatusMessage));
			base.AddMessageHandler<CreatePremadeGameAnswerMessage>(new ClientMessageHandler<CreatePremadeGameAnswerMessage>(this.OnCreatePremadeGameAnswerMessage));
			base.AddMessageHandler<JoinPremadeGameRequestMessage>(new ClientMessageHandler<JoinPremadeGameRequestMessage>(this.OnJoinPremadeGameRequestMessage));
			base.AddMessageHandler<JoinPremadeGameRequestResultMessage>(new ClientMessageHandler<JoinPremadeGameRequestResultMessage>(this.OnJoinPremadeGameRequestResultMessage));
			base.AddMessageHandler<ClanGameCreationCancelledMessage>(new ClientMessageHandler<ClanGameCreationCancelledMessage>(this.OnClanGameCreationCancelledMessage));
			base.AddMessageHandler<SigilChangeAnswerMessage>(new ClientMessageHandler<SigilChangeAnswerMessage>(this.OnSigilChangeAnswerMessage));
			base.AddMessageHandler<LobbyNotificationsMessage>(new ClientMessageHandler<LobbyNotificationsMessage>(this.OnLobbyNotificationsMessage));
			base.AddMessageHandler<CustomBattleOverMessage>(new ClientMessageHandler<CustomBattleOverMessage>(this.OnCustomBattleOverMessage));
			base.AddMessageHandler<RejoinBattleRequestAnswerMessage>(new ClientMessageHandler<RejoinBattleRequestAnswerMessage>(this.OnRejoinBattleRequestAnswerMessage));
			base.AddMessageHandler<PendingBattleRejoinMessage>(new ClientMessageHandler<PendingBattleRejoinMessage>(this.OnPendingBattleRejoinMessage));
			base.AddMessageHandler<ShowAnnouncementMessage>(new ClientMessageHandler<ShowAnnouncementMessage>(this.OnShowAnnouncementMessage));
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0000926C File Offset: 0x0000746C
		public void SetLoadedModules(string[] moduleIDs)
		{
			if (this._loadedUnofficialModules == null)
			{
				this._loadedUnofficialModules = new List<ModuleInfoModel>();
				using (List<ModuleInfo>.Enumerator enumerator = ModuleHelper.GetSortedModules(moduleIDs).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ModuleInfoModel moduleInfoModel;
						if (ModuleInfoModel.TryCreateForSession(enumerator.Current, out moduleInfoModel))
						{
							this._loadedUnofficialModules.Add(moduleInfoModel);
						}
					}
				}
			}
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x000092E0 File Offset: 0x000074E0
		public async Task<AvailableCustomGames> GetCustomGameServerList()
		{
			this.AssertCanPerformLobbyActions();
			CustomGameServerListResponse customGameServerListResponse = await base.CallFunction<CustomGameServerListResponse>(new RequestCustomGameServerListMessage());
			Debug.Print("Custom game server list received", 0, Debug.DebugColor.White, 17592186044416UL);
			AvailableCustomGames availableCustomGames;
			if (customGameServerListResponse != null)
			{
				this.AvailableCustomGames = customGameServerListResponse.AvailableCustomGames;
				ILobbyClientSessionHandler handler = this._handler;
				if (handler != null)
				{
					handler.OnCustomGameServerListReceived(this.AvailableCustomGames);
				}
				availableCustomGames = this.AvailableCustomGames;
			}
			else
			{
				availableCustomGames = null;
			}
			return availableCustomGames;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00009325 File Offset: 0x00007525
		public void QuitFromCustomGame()
		{
			base.SendMessage(new QuitFromCustomGameMessage());
			this.CurrentState = LobbyClient.State.AtLobby;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnQuitFromCustomGame();
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00009349 File Offset: 0x00007549
		public void QuitFromMatchmakerGame()
		{
			if (this.CurrentState == LobbyClient.State.AtBattle)
			{
				this.CheckAndSendMessage(new QuitFromMatchmakerGameMessage());
				this.CurrentState = LobbyClient.State.QuittingFromBattle;
				ILobbyClientSessionHandler handler = this._handler;
				if (handler == null)
				{
					return;
				}
				handler.OnQuitFromMatchmakerGame();
			}
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00009378 File Offset: 0x00007578
		public async Task<bool> RequestJoinCustomGame(CustomBattleId serverId, string password, bool isJoinAsAdmin = false)
		{
			this.CurrentState = LobbyClient.State.WaitingToJoinCustomGame;
			this.CustomBattleId = serverId;
			string text = ((!string.IsNullOrEmpty(password)) ? Common.CalculateMD5Hash(password) : null);
			base.SendMessage(new RequestJoinCustomGameMessage(serverId, text, isJoinAsAdmin));
			while (this.CurrentState == LobbyClient.State.WaitingToJoinCustomGame)
			{
				await Task.Yield();
			}
			bool flag;
			if (this.CurrentState == LobbyClient.State.InCustomGame)
			{
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x000093D8 File Offset: 0x000075D8
		public async Task<bool> RequestJoinPlayerParty(PlayerId targetPlayer, bool inviteRequest)
		{
			this.AssertCanPerformLobbyActions();
			RequestJoinPlayerPartyMessageResult requestJoinPlayerPartyMessageResult = await base.CallFunction<RequestJoinPlayerPartyMessageResult>(new RequestJoinPlayerPartyMessage(targetPlayer, inviteRequest));
			bool flag;
			if (requestJoinPlayerPartyMessageResult != null)
			{
				flag = requestJoinPlayerPartyMessageResult.Success;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0000942D File Offset: 0x0000762D
		public void CancelFindGame()
		{
			this.CurrentState = LobbyClient.State.RequestingToCancelSearchBattle;
			this.CheckAndSendMessage(new CancelBattleRequestMessage());
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00009441 File Offset: 0x00007641
		public void FindGame()
		{
			this.CurrentState = LobbyClient.State.RequestingToSearchBattle;
			this.CheckAndSendMessage(new FindGameMessage());
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00009458 File Offset: 0x00007658
		public async Task<bool> FindCustomGame(string[] selectedCustomGameTypes, bool? hasCrossplayPrivilege, string region)
		{
			this.CurrentState = LobbyClient.State.WaitingToJoinCustomGame;
			int i = 0;
			while (i < LobbyClient.CheckForCustomGamesCount)
			{
				CustomGameServerListResponse customGameServerListResponse = await base.CallFunction<CustomGameServerListResponse>(new RequestCustomGameServerListMessage());
				if (customGameServerListResponse != null && customGameServerListResponse.AvailableCustomGames.CustomGameServerInfos.Count > 0)
				{
					List<GameServerEntry> list = customGameServerListResponse.AvailableCustomGames.CustomGameServerInfos.OrderByDescending<GameServerEntry, int>((GameServerEntry c) => c.PlayerCount).ToList<GameServerEntry>();
					bool? flag = hasCrossplayPrivilege;
					bool flag2 = true;
					GameServerEntry.FilterGameServerEntriesBasedOnCrossplay(ref list, (flag.GetValueOrDefault() == flag2) & (flag != null));
					foreach (string text in selectedCustomGameTypes)
					{
						foreach (GameServerEntry gameServerEntry in list)
						{
							if (gameServerEntry.IsOfficial && gameServerEntry.GameType == text && gameServerEntry.Region == region && !gameServerEntry.PasswordProtected && gameServerEntry.MaxPlayerCount >= gameServerEntry.PlayerCount + this.PlayersInParty.Count)
							{
								base.SendMessage(new RequestJoinCustomGameMessage(gameServerEntry.Id, "", false));
								while (this.CurrentState == LobbyClient.State.WaitingToJoinCustomGame)
								{
									await Task.Yield();
								}
								if (this.CurrentState == LobbyClient.State.InCustomGame)
								{
									return true;
								}
								return false;
							}
						}
						List<GameServerEntry>.Enumerator enumerator = default(List<GameServerEntry>.Enumerator);
					}
					await Task.Delay(LobbyClient.CheckForCustomGamesDelay);
				}
				int j = i++;
			}
			this.CurrentState = LobbyClient.State.AtLobby;
			return false;
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x000094B8 File Offset: 0x000076B8
		public async Task<LobbyClientConnectResult> Connect(ILobbyClientSessionHandler lobbyClientSessionHandler, ILoginAccessProvider lobbyClientLoginAccessProvider, string overridenUserName, bool hasUserGeneratedContentPrivilege, PlatformInitParams initParams, Func<Task<bool>> preLoginTask)
		{
			base.AccessProvider = lobbyClientLoginAccessProvider;
			base.AccessProvider.Initialize(overridenUserName, initParams);
			this._handler = lobbyClientSessionHandler;
			this.CurrentState = LobbyClient.State.Working;
			this.HasUserGeneratedContentPrivilege = hasUserGeneratedContentPrivilege;
			base.BeginConnect();
			while (this.CurrentState == LobbyClient.State.Working)
			{
				await Task.Yield();
			}
			LobbyClientConnectResult lobbyClientConnectResult;
			if (this.CurrentState != LobbyClient.State.Connected)
			{
				lobbyClientConnectResult = new LobbyClientConnectResult(false, new TextObject("{=3cWg0cWt}Could not connect to server.", null));
			}
			else
			{
				AccessObjectResult accessObjectResult = AccessObjectResult.CreateFailed(new TextObject("{=gAeQdLU5}Failed to acquire access data from platform", null));
				Task getAccessObjectTask = Task.Run(delegate
				{
					accessObjectResult = this.AccessProvider.CreateAccessObject();
				});
				while (!getAccessObjectTask.IsCompleted)
				{
					await Task.Yield();
				}
				if (getAccessObjectTask.IsFaulted)
				{
					throw getAccessObjectTask.Exception ?? new Exception("Get access object task faulted without exception");
				}
				if (getAccessObjectTask.IsCanceled)
				{
					throw new Exception("Get access object task canceled");
				}
				if (!accessObjectResult.Success)
				{
					base.BeginDisconnect();
					lobbyClientConnectResult = new LobbyClientConnectResult(false, accessObjectResult.FailReason ?? new TextObject("{=JO37PkfW}Your platform service is not initialized.", null));
				}
				else
				{
					bool flag = preLoginTask != null;
					if (flag)
					{
						TaskAwaiter<bool> taskAwaiter = preLoginTask().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						flag = !taskAwaiter.GetResult();
					}
					if (flag)
					{
						base.BeginDisconnect();
						lobbyClientConnectResult = new LobbyClientConnectResult(false, new TextObject("{=63X8LERm}Couldn't receive login result from server.", null));
					}
					else
					{
						this._userName = base.AccessProvider.GetUserName();
						this._playerId = base.AccessProvider.GetPlayerId();
						this.CurrentState = LobbyClient.State.SessionRequested;
						string environmentVariable = Environment.GetEnvironmentVariable("Bannerlord.ConnectionPassword");
						LoginResult loginResult = await base.Login(new InitializeSession(this._playerId, this._userName, accessObjectResult.AccessObject, base.Application.ApplicationVersion, environmentVariable, this._loadedUnofficialModules.ToArray()));
						if (loginResult == null)
						{
							base.BeginDisconnect();
							lobbyClientConnectResult = new LobbyClientConnectResult(false, new TextObject("{=63X8LERm}Couldn't receive login result from server.", null));
						}
						else if (!loginResult.Successful)
						{
							base.BeginDisconnect();
							lobbyClientConnectResult = LobbyClientConnectResult.FromServerConnectResult(loginResult.ErrorCode, loginResult.ErrorParameters);
						}
						else
						{
							InitializeSessionResponse initializeSessionResponse = (InitializeSessionResponse)loginResult.LoginResultObject;
							this.PlayerData = initializeSessionResponse.PlayerData;
							this._serverStatus = initializeSessionResponse.ServerStatus;
							this.SupportedFeatures = initializeSessionResponse.SupportedFeatures;
							this.AvailableScenes = initializeSessionResponse.AvailableScenes;
							this._logOutReason = new TextObject("{=i4MNr0bo}Disconnected from the Lobby.", null);
							await PermaMuteList.LoadMutedPlayers(this.PlayerData.PlayerId);
							this._ownedCosmetics.Clear();
							this._usedCosmetics.Clear();
							ILobbyClientSessionHandler handler = this._handler;
							if (handler != null)
							{
								handler.OnPlayerDataReceived(this.PlayerData);
							}
							ILobbyClientSessionHandler handler2 = this._handler;
							if (handler2 != null)
							{
								handler2.OnServerStatusReceived(initializeSessionResponse.ServerStatus);
							}
							LobbyClient.FriendListCheckDelay = this._serverStatus.FriendListUpdatePeriod * 1000;
							if (initializeSessionResponse.HasPendingRejoin)
							{
								ILobbyClientSessionHandler handler3 = this._handler;
								if (handler3 != null)
								{
									handler3.OnPendingRejoin();
								}
							}
							this.CurrentState = LobbyClient.State.AtLobby;
							lobbyClientConnectResult = new LobbyClientConnectResult(true, null);
						}
					}
				}
			}
			return lobbyClientConnectResult;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00009530 File Offset: 0x00007730
		public void KickPlayer(PlayerId id, bool banPlayer)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x00009537 File Offset: 0x00007737
		public void ChangeRegion(string region)
		{
			if (this.PlayerData == null || this.PlayerData.LastRegion != region)
			{
				this.CheckAndSendMessage(new ChangeRegionMessage(region));
			}
			if (this.CurrentState == LobbyClient.State.AtLobby)
			{
				this.PlayerData.LastRegion = region;
			}
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x00009578 File Offset: 0x00007778
		public void ChangeGameTypes(string[] gameTypes)
		{
			bool flag = this.PlayerData == null || this.PlayerData.LastGameTypes.Length != gameTypes.Length;
			if (!flag)
			{
				foreach (string text in gameTypes)
				{
					if (!this.PlayerData.LastGameTypes.Contains(text))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				this.CheckAndSendMessage(new ChangeGameTypesMessage(gameTypes));
			}
			if (this.CurrentState == LobbyClient.State.AtLobby)
			{
				this.PlayerData.LastGameTypes = gameTypes;
			}
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x000095F8 File Offset: 0x000077F8
		private void CheckAndSendMessage(Message message)
		{
			base.SendMessage(message);
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x00009601 File Offset: 0x00007801
		public override void OnConnected()
		{
			base.OnConnected();
			this.CurrentState = LobbyClient.State.Connected;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnConnected();
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x00009620 File Offset: 0x00007820
		public override void OnCantConnect()
		{
			base.OnCantConnect();
			this.CurrentState = LobbyClient.State.Idle;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnCantConnect();
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00009640 File Offset: 0x00007840
		public override void OnDisconnected()
		{
			base.OnDisconnected();
			bool loggedIn = this.LoggedIn;
			this.CurrentState = LobbyClient.State.Idle;
			this.PlayerData = null;
			this.PlayersInParty.Clear();
			this.PlayersInClan.Clear();
			this.ClanHomeInfo = null;
			this._matchmakerBlockedTime = DateTime.MinValue;
			this.FriendInfos = new FriendInfo[0];
			PermaMuteList.SaveMutedPlayers();
			this._ownedCosmetics.Clear();
			this._usedCosmetics.Clear();
			ILobbyClientSessionHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnDisconnected(loggedIn ? this._logOutReason : null);
			}
			this.RemoveLobbyClientHandler();
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x000096DA File Offset: 0x000078DA
		public void RemoveLobbyClientHandler()
		{
			this._handler = null;
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x000096E3 File Offset: 0x000078E3
		private void OnFindGameAnswerMessage(FindGameAnswerMessage message)
		{
			if (!message.Successful)
			{
				this.CurrentState = LobbyClient.State.AtLobby;
			}
			else
			{
				this.CurrentState = LobbyClient.State.SearchingBattle;
			}
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnFindGameAnswer(message.Successful, message.SelectedAndEnabledGameTypes, false);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0000971C File Offset: 0x0000791C
		private void OnJoinBattleMessage(JoinBattleMessage message)
		{
			BattleServerInformationForClient battleServerInformation = message.BattleServerInformation;
			string text;
			if (base.Application.ProxyAddressMap.TryGetValue(battleServerInformation.ServerAddress, out text))
			{
				battleServerInformation.ServerAddress = text;
			}
			this.LastBattleServerAddressForClient = battleServerInformation.ServerAddress;
			this.LastBattleServerPortForClient = battleServerInformation.ServerPort;
			this.CurrentMatchId = battleServerInformation.MatchId;
			this.LastBattleIsOfficial = true;
			string text2 = "Successful matchmaker game join response\n";
			text2 = text2 + "Address: " + this.LastBattleServerAddressForClient + "\n";
			text2 = string.Concat(new object[] { text2, "Port: ", this.LastBattleServerPortForClient, "\n" });
			text2 = text2 + "Match Id: " + this.CurrentMatchId + "\n";
			Debug.Print(text2, 0, Debug.DebugColor.White, 17592186044416UL);
			ILobbyClientSessionHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnBattleServerInformationReceived(battleServerInformation);
			}
			this.CurrentState = LobbyClient.State.AtBattle;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00009810 File Offset: 0x00007A10
		private void OnBattleOverMessage(BattleOverMessage message)
		{
			if (this.CurrentState == LobbyClient.State.AtBattle || this.CurrentState == LobbyClient.State.QuittingFromBattle || this.CurrentState == LobbyClient.State.AtLobby)
			{
				this.CurrentState = LobbyClient.State.AtLobby;
				ILobbyClientSessionHandler handler = this._handler;
				if (handler == null)
				{
					return;
				}
				handler.OnMatchmakerGameOver(message.OldExperience, message.NewExperience, message.EarnedBadges, message.GoldGained, message.OldInfo, message.NewInfo, message.BattleCancelReason);
			}
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0000987B File Offset: 0x00007A7B
		private void OnBattleResultMessage(BattleResultMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnBattleResultReceived();
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0000988D File Offset: 0x00007A8D
		private void OnBattleServerLostMessage(BattleServerLostMessage message)
		{
			if (this.CurrentState == LobbyClient.State.AtBattle || this.CurrentState == LobbyClient.State.SearchingToRejoinBattle)
			{
				this.CurrentState = LobbyClient.State.AtLobby;
			}
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnBattleServerLost();
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x000098B9 File Offset: 0x00007AB9
		private void OnCancelBattleResponseMessage(CancelBattleResponseMessage message)
		{
			if (message.Successful)
			{
				ILobbyClientSessionHandler handler = this._handler;
				if (handler != null)
				{
					handler.OnCancelJoiningBattle();
				}
				this.CurrentState = LobbyClient.State.AtLobby;
				return;
			}
			if (this.CurrentState == LobbyClient.State.RequestingToCancelSearchBattle)
			{
				this.CurrentState = LobbyClient.State.SearchingBattle;
			}
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x000098EC File Offset: 0x00007AEC
		private void OnRejoinRequestRejectedMessage(RejoinRequestRejectedMessage message)
		{
			this.CurrentState = LobbyClient.State.AtLobby;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnRejoinRequestRejected();
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00009905 File Offset: 0x00007B05
		private void OnCancelFindGameMessage(CancelFindGameMessage message)
		{
			if (this.CurrentState == LobbyClient.State.SearchingBattle)
			{
				this.CancelFindGame();
			}
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00009916 File Offset: 0x00007B16
		private void OnWhisperMessageReceivedMessage(WhisperReceivedMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnWhisperMessageReceived(message.FromPlayer, message.ToPlayer, message.Message);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0000993A File Offset: 0x00007B3A
		private void OnClanMessageReceivedMessage(ClanMessageReceivedMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanMessageReceived(message.PlayerName, message.Message);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00009958 File Offset: 0x00007B58
		private void OnPartyMessageReceivedMessage(PartyMessageReceivedMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPartyMessageReceived(message.PlayerName, message.Message);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00009976 File Offset: 0x00007B76
		private void OnPlayerQuitFromMatchmakerGameResult(PlayerQuitFromMatchmakerGameResult message)
		{
			if (this.CurrentState == LobbyClient.State.QuittingFromBattle)
			{
				this.CurrentState = LobbyClient.State.AtLobby;
			}
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0000998C File Offset: 0x00007B8C
		private void OnEnterBattleWithPartyAnswerMessage(EnterBattleWithPartyAnswer message)
		{
			if (!message.Successful)
			{
				this.CurrentState = LobbyClient.State.AtLobby;
				return;
			}
			if (this.CurrentState == LobbyClient.State.AtLobby || this.CurrentState == LobbyClient.State.RequestingToSearchBattle)
			{
				this.CurrentState = LobbyClient.State.SearchingBattle;
			}
			else if (this.CurrentState != LobbyClient.State.SearchingBattle)
			{
				LobbyClient.State currentState = this.CurrentState;
			}
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnEnterBattleWithPartyAnswer(message.SelectedAndEnabledGameTypes);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x000099EC File Offset: 0x00007BEC
		private void OnJoinCustomGameResultMessage(JoinCustomGameResultMessage message)
		{
			if (!message.Success && message.Response == CustomGameJoinResponse.AlreadyRequestedWaitingForServerResponse)
			{
				ILobbyClientSessionHandler handler = this._handler;
				if (handler == null)
				{
					return;
				}
				handler.OnSystemMessageReceived(new TextObject("{=ivKntfNA}Already requested to join, waiting for server response", null).ToString());
				return;
			}
			else if (message.Success)
			{
				message.JoinGameData.GameServerProperties.CheckAndReplaceProxyAddress(base.Application.ProxyAddressMap);
				this.CurrentState = LobbyClient.State.InCustomGame;
				this.LastBattleServerAddressForClient = message.JoinGameData.GameServerProperties.Address;
				this.LastBattleServerPortForClient = (ushort)message.JoinGameData.GameServerProperties.Port;
				this.LastBattleIsOfficial = message.JoinGameData.GameServerProperties.IsOfficial;
				this.CurrentMatchId = message.MatchId;
				string text = "Successful custom game join response\n";
				text = text + "Server Name: " + message.JoinGameData.GameServerProperties.Name + "\n";
				text = text + "Host Name: " + message.JoinGameData.GameServerProperties.HostName + "\n";
				text = text + "Address: " + this.LastBattleServerAddressForClient + "\n";
				text = string.Concat(new object[] { text, "Port: ", this.LastBattleServerPortForClient, "\n" });
				text = text + "Match Id: " + this.CurrentMatchId + "\n";
				text = text + "Is Official: " + message.JoinGameData.GameServerProperties.IsOfficial.ToString() + "\n";
				Debug.Print(text, 0, Debug.DebugColor.White, 17592186044416UL);
				ILobbyClientSessionHandler handler2 = this._handler;
				if (handler2 == null)
				{
					return;
				}
				handler2.OnJoinCustomGameResponse(message.Success, message.JoinGameData, message.Response, message.IsAdmin);
				return;
			}
			else
			{
				this.CurrentState = LobbyClient.State.AtLobby;
				ILobbyClientSessionHandler handler3 = this._handler;
				if (handler3 == null)
				{
					return;
				}
				handler3.OnJoinCustomGameFailureResponse(message.Response);
				return;
			}
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00009BD0 File Offset: 0x00007DD0
		private void OnClientWantsToConnectCustomGameMessage(ClientWantsToConnectCustomGameMessage message)
		{
			this.AssertCanPerformLobbyActions();
			List<PlayerJoinGameResponseDataFromHost> list = new List<PlayerJoinGameResponseDataFromHost>();
			PlayerJoinGameData[] playerJoinGameData = message.PlayerJoinGameData;
			for (int i = 0; i < playerJoinGameData.Length; i++)
			{
				if (playerJoinGameData[i] != null)
				{
					List<PlayerJoinGameData> list2 = new List<PlayerJoinGameData>();
					PlayerJoinGameData playerJoinGameData2 = playerJoinGameData[i];
					Guid? guid = playerJoinGameData2.PartyId;
					if (guid == null)
					{
						list2.Add(playerJoinGameData2);
					}
					else
					{
						for (int j = i; j < playerJoinGameData.Length; j++)
						{
							PlayerJoinGameData playerJoinGameData3 = playerJoinGameData[j];
							guid = playerJoinGameData2.PartyId;
							if (guid.Equals((playerJoinGameData3 != null) ? playerJoinGameData3.PartyId : null))
							{
								list2.Add(playerJoinGameData3);
								playerJoinGameData[j] = null;
							}
						}
					}
					if (this._handler != null)
					{
						PlayerJoinGameResponseDataFromHost[] array = this._handler.OnClientWantsToConnectCustomGame(list2.ToArray());
						list.AddRange(array);
					}
				}
			}
			this.ResponseCustomGameClientConnection(list.ToArray());
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x00009CBB File Offset: 0x00007EBB
		private void OnClientQuitFromCustomGameMessage(ClientQuitFromCustomGameMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClientQuitFromCustomGame(message.PlayerId);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x00009CD3 File Offset: 0x00007ED3
		private void OnEnterCustomBattleWithPartyAnswerMessage(EnterCustomBattleWithPartyAnswer message)
		{
			if (!message.Successful)
			{
				this.CurrentState = LobbyClient.State.AtLobby;
				return;
			}
			if (this.CurrentState == LobbyClient.State.AtLobby)
			{
				this.CurrentState = LobbyClient.State.WaitingToJoinCustomGame;
			}
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnEnterCustomBattleWithPartyAnswer();
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00009D06 File Offset: 0x00007F06
		private void OnPlayerRemovedFromMatchmakerGameMessage(PlayerRemovedFromMatchmakerGame message)
		{
			this.CurrentState = LobbyClient.State.AtLobby;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnRemovedFromMatchmakerGame(message.DisconnectType);
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00009D25 File Offset: 0x00007F25
		private void OnPlayerRemovedFromCustomGame(PlayerRemovedFromCustomGame message)
		{
			this.CurrentState = LobbyClient.State.AtLobby;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnRemovedFromCustomGame(message.DisconnectType);
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00009D44 File Offset: 0x00007F44
		private void OnSystemMessage(SystemMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnSystemMessageReceived(message.GetDescription().ToString());
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00009D61 File Offset: 0x00007F61
		private void OnAdminMessage(AdminMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnAdminMessageReceived(message.Message);
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00009D79 File Offset: 0x00007F79
		private void OnInvitationToPartyMessage(InvitationToPartyMessage message)
		{
			this.IsPartyInvitationPopupActive = true;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPartyInvitationReceived(message.InviterPlayerName, message.InviterPlayerId);
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00009D9E File Offset: 0x00007F9E
		private void OnPartyInvitationInvalidMessage(PartyInvitationInvalidMessage message)
		{
			this.IsPartyInvitationPopupActive = false;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPartyInvitationInvalidated();
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00009DB7 File Offset: 0x00007FB7
		private void OnRequestJoinPartyMessage(RequestJoinPartyMessage message)
		{
			this.IsPartyJoinRequestPopupActive = true;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPartyJoinRequestReceived(message.PlayerId, message.ViaPlayerId, message.ViaPlayerName);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00009DE2 File Offset: 0x00007FE2
		private void OnPlayerInvitedToPartyMessage(PlayerInvitedToPartyMessage message)
		{
			this.PlayersInParty.Add(new PartyPlayerInLobbyClient(message.PlayerId, message.PlayerName, false));
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerInvitedToParty(message.PlayerId);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00009E18 File Offset: 0x00008018
		private void OnPlayerAddedToPartyMessage(PlayersAddedToPartyMessage message)
		{
			foreach (ValueTuple<PlayerId, string, bool> valueTuple in message.Players)
			{
				PlayerId playerId = valueTuple.Item1;
				string item = valueTuple.Item2;
				bool item2 = valueTuple.Item3;
				PartyPlayerInLobbyClient partyPlayerInLobbyClient = this.PlayersInParty.Find((PartyPlayerInLobbyClient p) => p.PlayerId == playerId);
				if (partyPlayerInLobbyClient != null)
				{
					partyPlayerInLobbyClient.SetAtParty();
				}
				else
				{
					partyPlayerInLobbyClient = new PartyPlayerInLobbyClient(playerId, item, item2);
					this.PlayersInParty.Add(partyPlayerInLobbyClient);
					partyPlayerInLobbyClient.SetAtParty();
				}
				if (playerId != this.PlayerID)
				{
					RecentPlayersManager.AddOrUpdatePlayerEntry(playerId, item, InteractionType.InPartyTogether, -1);
				}
			}
			foreach (ValueTuple<PlayerId, string> valueTuple2 in message.InvitedPlayers)
			{
				PlayerId item3 = valueTuple2.Item1;
				string item4 = valueTuple2.Item2;
				this.PlayersInParty.Add(new PartyPlayerInLobbyClient(item3, item4, false));
			}
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayersAddedToParty(message.Players, message.InvitedPlayers);
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00009F74 File Offset: 0x00008174
		private void OnPlayerRemovedFromPartyMessage(PlayerRemovedFromPartyMessage message)
		{
			if (message.PlayerId == this._playerId)
			{
				this.PlayersInParty.Clear();
			}
			else
			{
				this.PlayersInParty.RemoveAll((PartyPlayerInLobbyClient partyPlayer) => partyPlayer.PlayerId == message.PlayerId);
			}
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerRemovedFromParty(message.PlayerId, message.Reason);
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00009FF4 File Offset: 0x000081F4
		private void OnPlayerAssignedPartyLeaderMessage(PlayerAssignedPartyLeaderMessage message)
		{
			PartyPlayerInLobbyClient partyPlayerInLobbyClient = this.PlayersInParty.FirstOrDefault<PartyPlayerInLobbyClient>((PartyPlayerInLobbyClient p) => p.IsPartyLeader);
			if (partyPlayerInLobbyClient != null)
			{
				partyPlayerInLobbyClient.SetMember();
			}
			PartyPlayerInLobbyClient partyPlayerInLobbyClient2 = this.PlayersInParty.FirstOrDefault<PartyPlayerInLobbyClient>((PartyPlayerInLobbyClient partyPlayer) => partyPlayer.PlayerId == message.PartyLeaderId);
			if (partyPlayerInLobbyClient2 != null)
			{
				partyPlayerInLobbyClient2.SetLeader();
			}
			else
			{
				this.KickPlayerFromParty(this.PlayerID);
			}
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerAssignedPartyLeader(message.PartyLeaderId);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0000A08D File Offset: 0x0000828D
		private void OnPlayerSuggestedToPartyMessage(PlayerSuggestedToPartyMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerSuggestedToParty(message.PlayerId, message.PlayerName, message.SuggestingPlayerId, message.SuggestingPlayerName);
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0000A0B7 File Offset: 0x000082B7
		private void OnUpdatePlayerDataMessage(UpdatePlayerDataMessage updatePlayerDataMessage)
		{
			this.PlayerData = updatePlayerDataMessage.PlayerData;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerDataReceived(this.PlayerData);
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0000A0DC File Offset: 0x000082DC
		private void OnServerStatusMessage(ServerStatusMessage serverStatusMessage)
		{
			this._serverStatusTimer.Restart();
			this._serverStatus = serverStatusMessage.ServerStatus;
			if (!this.IsAbleToSearchForGame && this.CurrentState == LobbyClient.State.SearchingBattle)
			{
				this.CancelFindGame();
			}
			if (this._handler != null)
			{
				this._handler.OnServerStatusReceived(this._serverStatus);
				LobbyClient.FriendListCheckDelay = this._serverStatus.FriendListUpdatePeriod * 1000;
			}
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0000A146 File Offset: 0x00008346
		private void OnFriendListMessage(FriendListMessage friendListMessage)
		{
			this._friendListTimer.Restart();
			this.FriendInfos = friendListMessage.Friends;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnFriendListReceived(friendListMessage.Friends);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0000A178 File Offset: 0x00008378
		private void OnMatchmakerDisabledMessage(MatchmakerDisabledMessage matchmakerDisabledMessage)
		{
			if (matchmakerDisabledMessage.RemainingTime > 0)
			{
				this._matchmakerBlockedTime = DateTime.Now.AddSeconds((double)matchmakerDisabledMessage.RemainingTime);
				return;
			}
			this._matchmakerBlockedTime = DateTime.MinValue;
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0000A1B4 File Offset: 0x000083B4
		private void OnClanCreationRequestMessage(ClanCreationRequestMessage clanCreationRequestMessage)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanInvitationReceived(clanCreationRequestMessage.ClanName, clanCreationRequestMessage.ClanTag, true);
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0000A1D3 File Offset: 0x000083D3
		private void OnClanCreationRequestAnsweredMessage(ClanCreationRequestAnsweredMessage clanCreationRequestAnsweredMessage)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanInvitationAnswered(clanCreationRequestAnsweredMessage.PlayerId, clanCreationRequestAnsweredMessage.ClanCreationAnswer);
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0000A1F1 File Offset: 0x000083F1
		private void OnClanCreationSuccessfulMessage(ClanCreationSuccessfulMessage clanCreationSuccessfulMessage)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanCreationSuccessful();
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0000A203 File Offset: 0x00008403
		private void OnClanCreationFailedMessage(ClanCreationFailedMessage clanCreationFailedMessage)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanCreationFailed();
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0000A215 File Offset: 0x00008415
		private void OnCreateClanAnswerMessage(CreateClanAnswerMessage createClanAnswerMessage)
		{
			if (createClanAnswerMessage.Successful)
			{
				ILobbyClientSessionHandler handler = this._handler;
				if (handler == null)
				{
					return;
				}
				handler.OnClanCreationStarted();
			}
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0000A22F File Offset: 0x0000842F
		public void SendWhisper(string playerName, string message)
		{
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0000A231 File Offset: 0x00008431
		private void OnRecentPlayerStatusesMessage(RecentPlayerStatusesMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnRecentPlayerStatusesReceived(message.Friends);
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0000A249 File Offset: 0x00008449
		public void FleeBattle()
		{
			this.CheckAndSendMessage(new RejoinBattleRequestMessage(false));
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0000A257 File Offset: 0x00008457
		public void SendPartyMessage(string message)
		{
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0000A259 File Offset: 0x00008459
		private void OnClanInfoChangedMessage(ClanInfoChangedMessage clanInfoChangedMessage)
		{
			this.UpdateClanInfo(clanInfoChangedMessage.ClanHomeInfo);
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0000A268 File Offset: 0x00008468
		protected override void OnTick()
		{
			if (this.LoggedIn && !this.IsInGame)
			{
				if (this._serverStatusTimer != null && this._serverStatusTimer.ElapsedMilliseconds > (long)LobbyClient.ServerStatusCheckDelay)
				{
					this._serverStatusTimer.Restart();
					this.CheckAndSendMessage(new GetServerStatusMessage());
				}
				if (this._friendListTimer != null && this._friendListTimer.ElapsedMilliseconds > (long)LobbyClient.FriendListCheckDelay)
				{
					this._friendListTimer.Restart();
					this.CheckAndSendMessage(new GetFriendListMessage());
				}
				if (this._recentPlayersTimer != null && this._recentPlayersTimer.ElapsedMilliseconds > (long)LobbyClient.RecentPlayersCheckDelay)
				{
					this._recentPlayersTimer.Restart();
					RecentPlayersManager.TrimPlayers();
					PlayerId[] recentPlayerIds = RecentPlayersManager.GetRecentPlayerIds();
					if (recentPlayerIds.Length != 0)
					{
						this.CheckAndSendMessage(new GetRecentPlayersStatusMessage(recentPlayerIds));
					}
				}
			}
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0000A32E File Offset: 0x0000852E
		private void OnInvitationToClanMessage(InvitationToClanMessage invitationToClanMessage)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanInvitationReceived(invitationToClanMessage.ClanName, invitationToClanMessage.ClanTag, false);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0000A34D File Offset: 0x0000854D
		public void RejoinBattle()
		{
			this.CheckAndSendMessage(new RejoinBattleRequestMessage(true));
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0000A35B File Offset: 0x0000855B
		private void OnJoinPremadeGameAnswerMessage(JoinPremadeGameAnswerMessage joinPremadeGameAnswerMessage)
		{
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0000A35D File Offset: 0x0000855D
		public void OnBattleResultsSeen()
		{
			this.AssertCanPerformLobbyActions();
			this.CheckAndSendMessage(new BattleResultSeenMessage());
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0000A370 File Offset: 0x00008570
		private void OnCreatePremadeGameAnswerMessage(CreatePremadeGameAnswerMessage createPremadeGameAnswerMessage)
		{
			if (createPremadeGameAnswerMessage.Successful)
			{
				ILobbyClientSessionHandler handler = this._handler;
				if (handler == null)
				{
					return;
				}
				handler.OnPremadeGameCreated();
			}
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0000A38A File Offset: 0x0000858A
		private void OnJoinPremadeGameRequestMessage(JoinPremadeGameRequestMessage joinPremadeGameRequestMessage)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnJoinPremadeGameRequested(joinPremadeGameRequestMessage.ClanName, joinPremadeGameRequestMessage.Sigil, joinPremadeGameRequestMessage.ChallengerPartyId, joinPremadeGameRequestMessage.ChallengerPlayers, joinPremadeGameRequestMessage.ChallengerPartyLeaderId, joinPremadeGameRequestMessage.PremadeGameType);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0000A3C0 File Offset: 0x000085C0
		private void OnJoinPremadeGameRequestResultMessage(JoinPremadeGameRequestResultMessage joinPremadeGameRequestResultMessage)
		{
			if (joinPremadeGameRequestResultMessage.Successful)
			{
				ILobbyClientSessionHandler handler = this._handler;
				if (handler != null)
				{
					handler.OnJoinPremadeGameRequestSuccessful();
				}
				this.CurrentState = LobbyClient.State.WaitingToJoinPremadeGame;
			}
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0000A3E4 File Offset: 0x000085E4
		private async void OnClanDisbandedMessage(ClanDisbandedMessage clanDisbandedMessage)
		{
			ClanHomeInfo clanHomeInfo = await this.GetClanHomeInfo();
			this.UpdateClanInfo(clanHomeInfo);
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0000A41D File Offset: 0x0000861D
		private void OnClanGameCreationCancelledMessage(ClanGameCreationCancelledMessage clanGameCreationCancelledMessage)
		{
			this.CurrentState = LobbyClient.State.AtLobby;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPremadeGameCreationCancelled();
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0000A436 File Offset: 0x00008636
		private void OnPremadeGameEligibilityStatusMessage(PremadeGameEligibilityStatusMessage premadeGameEligibilityStatusMessage)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnPremadeGameEligibilityStatusReceived(premadeGameEligibilityStatusMessage.EligibleGameTypes.Length != 0);
			}
			this.IsEligibleToCreatePremadeGame = premadeGameEligibilityStatusMessage.EligibleGameTypes.Length != 0;
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0000A464 File Offset: 0x00008664
		private async void OnKickedFromClan(KickedFromClanMessage kickedFromClanMessage)
		{
			ClanHomeInfo clanHomeInfo = await this.GetClanHomeInfo();
			this.UpdateClanInfo(clanHomeInfo);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0000A4A0 File Offset: 0x000086A0
		private async void OnPartyPlayerLeftClan(PartyPlayerLeftClanMessage partyPlayerLeftClanMessage)
		{
			ClanHomeInfo clanHomeInfo = await this.GetClanHomeInfo();
			this.UpdateClanInfo(clanHomeInfo);
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0000A4D9 File Offset: 0x000086D9
		private void OnCustomBattleOverMessage(CustomBattleOverMessage message)
		{
			this.CurrentState = LobbyClient.State.AtLobby;
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnMatchmakerGameOver(message.OldExperience, message.NewExperience, new List<string>(), message.GoldGain, null, null, BattleCancelReason.None);
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0000A50C File Offset: 0x0000870C
		public void AcceptClanInvitation()
		{
			this.CheckAndSendMessage(new AcceptClanInvitationMessage());
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0000A519 File Offset: 0x00008719
		public void DeclineClanInvitation()
		{
			this.CheckAndSendMessage(new DeclineClanInvitationMessage());
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0000A526 File Offset: 0x00008726
		private void OnShowAnnouncementMessage(ShowAnnouncementMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnAnnouncementReceived(message.Announcement);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0000A540 File Offset: 0x00008740
		public void MarkNotificationAsRead(int notificationID)
		{
			UpdateNotificationsMessage updateNotificationsMessage = new UpdateNotificationsMessage(new int[] { notificationID });
			this.CheckAndSendMessage(updateNotificationsMessage);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0000A564 File Offset: 0x00008764
		private void OnRejoinBattleRequestAnswerMessage(RejoinBattleRequestAnswerMessage rejoinBattleRequestAnswerMessage)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnRejoinBattleRequestAnswered(rejoinBattleRequestAnswerMessage.IsSuccessful);
			}
			if (rejoinBattleRequestAnswerMessage.IsSuccessful && rejoinBattleRequestAnswerMessage.IsRejoinAccepted)
			{
				this.CurrentState = LobbyClient.State.SearchingBattle;
			}
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0000A594 File Offset: 0x00008794
		public void AcceptClanCreationRequest()
		{
			this.CheckAndSendMessage(new AcceptClanCreationRequestMessage());
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0000A5A1 File Offset: 0x000087A1
		private void OnPendingBattleRejoinMessage(PendingBattleRejoinMessage pendingBattleRejoinMessage)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPendingRejoin();
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0000A5B3 File Offset: 0x000087B3
		private void OnSigilChangeAnswerMessage(SigilChangeAnswerMessage message)
		{
			if (message.Successful)
			{
				ILobbyClientSessionHandler handler = this._handler;
				if (handler == null)
				{
					return;
				}
				handler.OnSigilChanged();
			}
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0000A5CD File Offset: 0x000087CD
		public void DeclineClanCreationRequest()
		{
			this.CheckAndSendMessage(new DeclineClanCreationRequestMessage());
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0000A5DA File Offset: 0x000087DA
		public void PromoteToClanLeader(PlayerId playerId, bool dontUseNameForUnknownPlayer)
		{
			this.CheckAndSendMessage(new PromoteToClanLeaderMessage(playerId, dontUseNameForUnknownPlayer));
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0000A5E9 File Offset: 0x000087E9
		private void OnLobbyNotificationsMessage(LobbyNotificationsMessage message)
		{
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnNotificationsReceived(message.Notifications);
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0000A601 File Offset: 0x00008801
		public void KickFromClan(PlayerId playerId)
		{
			this.CheckAndSendMessage(new KickFromClanMessage(playerId));
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0000A610 File Offset: 0x00008810
		public async Task<CheckClanParameterValidResult> ClanNameExists(string clanName)
		{
			return await base.CallFunction<CheckClanParameterValidResult>(new CheckClanNameValidMessage(clanName));
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0000A660 File Offset: 0x00008860
		public async Task<CheckClanParameterValidResult> ClanTagExists(string clanTag)
		{
			return await base.CallFunction<CheckClanParameterValidResult>(new CheckClanTagValidMessage(clanTag));
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0000A6B0 File Offset: 0x000088B0
		public async Task<ClanHomeInfo> GetClanHomeInfo()
		{
			GetClanHomeInfoResult getClanHomeInfoResult = await base.CallFunction<GetClanHomeInfoResult>(new GetClanHomeInfoMessage());
			ClanHomeInfo clanHomeInfo;
			if (getClanHomeInfoResult != null)
			{
				this.UpdateClanInfo(getClanHomeInfoResult.ClanHomeInfo);
				clanHomeInfo = getClanHomeInfoResult.ClanHomeInfo;
			}
			else
			{
				this.UpdateClanInfo(null);
				clanHomeInfo = null;
			}
			return clanHomeInfo;
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0000A6F5 File Offset: 0x000088F5
		public void AssignAsClanOfficer(PlayerId playerId, bool dontUseNameForUnknownPlayer)
		{
			this.CheckAndSendMessage(new AssignAsClanOfficerMessage(playerId, dontUseNameForUnknownPlayer));
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0000A704 File Offset: 0x00008904
		public void RemoveClanOfficerRoleForPlayer(PlayerId playerId)
		{
			this.CheckAndSendMessage(new RemoveClanOfficerRoleForPlayerMessage(playerId));
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0000A714 File Offset: 0x00008914
		private void UpdateClanInfo(ClanHomeInfo clanHomeInfo)
		{
			this.PlayersInClan.Clear();
			this.PlayerInfosInClan.Clear();
			this.ClanID = Guid.Empty;
			this.ClanInfo = null;
			this.ClanHomeInfo = clanHomeInfo;
			if (clanHomeInfo != null)
			{
				if (clanHomeInfo.IsInClan)
				{
					foreach (ClanPlayer clanPlayer in clanHomeInfo.ClanInfo.Players)
					{
						this.PlayersInClan.Add(clanPlayer);
					}
					foreach (ClanPlayerInfo clanPlayerInfo in clanHomeInfo.ClanPlayerInfos)
					{
						this.PlayerInfosInClan.Add(clanPlayerInfo);
					}
					this.ClanID = clanHomeInfo.ClanInfo.ClanId;
				}
				this.ClanInfo = clanHomeInfo.ClanInfo;
			}
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnClanInfoChanged();
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0000A7DC File Offset: 0x000089DC
		public async Task<ClanLeaderboardInfo> GetClanLeaderboardInfo()
		{
			GetClanLeaderboardResult getClanLeaderboardResult = await base.CallFunction<GetClanLeaderboardResult>(new GetClanLeaderboardMessage());
			ClanLeaderboardInfo clanLeaderboardInfo;
			if (getClanLeaderboardResult != null)
			{
				clanLeaderboardInfo = getClanLeaderboardResult.ClanLeaderboardInfo;
			}
			else
			{
				clanLeaderboardInfo = null;
			}
			return clanLeaderboardInfo;
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0000A824 File Offset: 0x00008A24
		public async Task<ClanInfo> GetPlayerClanInfo(PlayerId playerId)
		{
			GetPlayerClanInfoResult getPlayerClanInfoResult = await base.CallFunction<GetPlayerClanInfoResult>(new GetPlayerClanInfo(playerId));
			ClanInfo clanInfo;
			if (((getPlayerClanInfoResult != null) ? getPlayerClanInfoResult.ClanInfo : null) != null)
			{
				clanInfo = getPlayerClanInfoResult.ClanInfo;
			}
			else
			{
				clanInfo = null;
			}
			return clanInfo;
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0000A871 File Offset: 0x00008A71
		public void SendClanMessage(string message)
		{
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0000A874 File Offset: 0x00008A74
		public async Task<PremadeGameList> GetPremadeGameList()
		{
			GetPremadeGameListResult getPremadeGameListResult = await base.CallFunction<GetPremadeGameListResult>(new GetPremadeGameListMessage());
			PremadeGameList premadeGameList;
			if (getPremadeGameListResult != null)
			{
				this.AvailablePremadeGames = getPremadeGameListResult.GameList;
				ILobbyClientSessionHandler handler = this._handler;
				if (handler != null)
				{
					handler.OnPremadeGameListReceived();
				}
				premadeGameList = getPremadeGameListResult.GameList;
			}
			else
			{
				premadeGameList = null;
			}
			return premadeGameList;
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0000A8BC File Offset: 0x00008ABC
		public async Task<AvailableScenes> GetAvailableScenes()
		{
			GetAvailableScenesResult getAvailableScenesResult = await base.CallFunction<GetAvailableScenesResult>(new GetAvailableScenesMessage());
			AvailableScenes availableScenes;
			if (getAvailableScenesResult != null)
			{
				availableScenes = getAvailableScenesResult.AvailableScenes;
			}
			else
			{
				availableScenes = null;
			}
			return availableScenes;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0000A904 File Offset: 0x00008B04
		public async Task<PublishedLobbyNewsArticle[]> GetLobbyNews()
		{
			GetPublishedLobbyNewsMessageResult getPublishedLobbyNewsMessageResult = await base.CallFunction<GetPublishedLobbyNewsMessageResult>(new GetPublishedLobbyNewsMessage());
			PublishedLobbyNewsArticle[] array;
			if (getPublishedLobbyNewsMessageResult != null)
			{
				array = getPublishedLobbyNewsMessageResult.Content;
			}
			else
			{
				array = null;
			}
			return array;
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0000A949 File Offset: 0x00008B49
		public void SetClanInformationText(string informationText)
		{
			this.CheckAndSendMessage(new SetClanInformationMessage(informationText));
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0000A957 File Offset: 0x00008B57
		public void AddClanAnnouncement(string announcement)
		{
			this.CheckAndSendMessage(new AddClanAnnouncementMessage(announcement));
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0000A965 File Offset: 0x00008B65
		public void EditClanAnnouncement(int announcementId, string text)
		{
			this.CheckAndSendMessage(new EditClanAnnouncementMessage(announcementId, text));
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0000A974 File Offset: 0x00008B74
		public void RemoveClanAnnouncement(int announcementId)
		{
			this.CheckAndSendMessage(new RemoveClanAnnouncementMessage(announcementId));
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0000A982 File Offset: 0x00008B82
		public void ChangeClanFaction(string faction)
		{
			this.CheckAndSendMessage(new ChangeClanFactionMessage(faction));
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0000A990 File Offset: 0x00008B90
		public void ChangeClanSigil(string sigil)
		{
			this.CheckAndSendMessage(new ChangeClanSigilMessage(sigil));
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0000A99E File Offset: 0x00008B9E
		public void DestroyClan()
		{
			this.CheckAndSendMessage(new DestroyClanMessage());
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0000A9AB File Offset: 0x00008BAB
		public void InviteToClan(PlayerId invitedPlayerId, bool dontUseNameForUnknownPlayer)
		{
			this.CheckAndSendMessage(new InviteToClanMessage(invitedPlayerId, dontUseNameForUnknownPlayer));
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0000A9BC File Offset: 0x00008BBC
		public async void CreatePremadeGame(string name, string gameType, string mapName, string factionA, string factionB, string password, PremadeGameType premadeGameType)
		{
			this.CurrentState = LobbyClient.State.WaitingToCreatePremadeGame;
			string text = ((!string.IsNullOrEmpty(password)) ? Common.CalculateMD5Hash(password) : null);
			CreatePremadeGameMessageResult createPremadeGameMessageResult = await base.CallFunction<CreatePremadeGameMessageResult>(new CreatePremadeGameMessage(name, gameType, mapName, factionA, factionB, text, premadeGameType));
			if (createPremadeGameMessageResult == null || !createPremadeGameMessageResult.Successful)
			{
				this.CurrentState = LobbyClient.State.AtLobby;
			}
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0000AA31 File Offset: 0x00008C31
		public void CancelCreatingPremadeGame()
		{
			this.CheckAndSendMessage(new CancelCreatingPremadeGameMessage());
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0000AA40 File Offset: 0x00008C40
		public void RequestToJoinPremadeGame(Guid gameId, string password)
		{
			string text = Common.CalculateMD5Hash(password);
			this.CheckAndSendMessage(new RequestToJoinPremadeGameMessage(gameId, text));
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0000AA61 File Offset: 0x00008C61
		public void AcceptJoinPremadeGameRequest(Guid partyId)
		{
			this.CheckAndSendMessage(new AcceptJoinPremadeGameRequestMessage(partyId));
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0000AA6F File Offset: 0x00008C6F
		public void DeclineJoinPremadeGameRequest(Guid partyId)
		{
			this.CheckAndSendMessage(new DeclineJoinPremadeGameRequestMessage(partyId));
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0000AA7D File Offset: 0x00008C7D
		public void InviteToParty(PlayerId playerId, bool dontUseNameForUnknownPlayer)
		{
			this.CheckAndSendMessage(new InviteToPartyMessage(playerId, dontUseNameForUnknownPlayer));
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0000AA8C File Offset: 0x00008C8C
		public void DisbandParty()
		{
			this.CheckAndSendMessage(new DisbandPartyMessage());
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0000AA99 File Offset: 0x00008C99
		public void KickPlayerFromParty(PlayerId playerId)
		{
			this.CheckAndSendMessage(new KickPlayerFromPartyMessage(playerId));
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0000AAA7 File Offset: 0x00008CA7
		public void OnPlayerNameUpdated(string name)
		{
			this._userName = name;
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0000AAB0 File Offset: 0x00008CB0
		public void ToggleUseClanSigil(bool isUsed)
		{
			this.CheckAndSendMessage(new UpdateUsingClanSigil(isUsed));
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0000AABE File Offset: 0x00008CBE
		public void PromotePlayerToPartyLeader(PlayerId playerId)
		{
			this.CheckAndSendMessage(new PromotePlayerToPartyLeaderMessage(playerId));
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0000AACC File Offset: 0x00008CCC
		public void ChangeSigil(string sigilId)
		{
			this.CheckAndSendMessage(new ChangePlayerSigilMessage(sigilId));
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0000AADC File Offset: 0x00008CDC
		public async Task<bool> InviteToPlatformSession(PlayerId playerId)
		{
			bool flag = false;
			if (this._handler != null)
			{
				flag = await this._handler.OnInviteToPlatformSession(playerId);
			}
			return flag;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0000AB2C File Offset: 0x00008D2C
		public async void EndCustomGame()
		{
			await base.CallFunction<EndHostingCustomGameResult>(new EndHostingCustomGameMessage());
			ILobbyClientSessionHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnCustomGameEnd();
			}
			this.CurrentState = LobbyClient.State.AtLobby;
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0000AB68 File Offset: 0x00008D68
		public async void RegisterCustomGame(string gameModule, string gameType, string serverName, int maxPlayerCount, string map, string uniqueMapId, string gamePassword, string adminPassword, int port)
		{
			this.CustomGameType = gameType;
			this.CustomGameScene = map;
			this.CurrentState = LobbyClient.State.WaitingToRegisterCustomGame;
			RegisterCustomGameResult registerCustomGameResult = await base.CallFunction<RegisterCustomGameResult>(new RegisterCustomGameMessage(gameModule, gameType, serverName, maxPlayerCount, map, uniqueMapId, gamePassword, adminPassword, port));
			Debug.Print("Register custom game server response received", 0, Debug.DebugColor.White, 17592186044416UL);
			if (registerCustomGameResult.Success)
			{
				this.CurrentState = LobbyClient.State.HostingCustomGame;
				ILobbyClientSessionHandler handler = this._handler;
				if (handler != null)
				{
					handler.OnRegisterCustomGameServerResponse();
				}
			}
			else
			{
				this.CurrentState = LobbyClient.State.AtLobby;
			}
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0000ABEF File Offset: 0x00008DEF
		public void UpdateCustomGameData(string newGameType, string newMap, int newCount)
		{
			base.SendMessage(new UpdateCustomGameData(newGameType, newMap, newCount));
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0000ABFF File Offset: 0x00008DFF
		public void ResponseCustomGameClientConnection(PlayerJoinGameResponseDataFromHost[] playerJoinData)
		{
			base.SendMessage(new ResponseCustomGameClientConnectionMessage(playerJoinData));
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0000AC0D File Offset: 0x00008E0D
		public void AcceptPartyInvitation()
		{
			this.IsPartyInvitationPopupActive = false;
			this.CheckAndSendMessage(new AcceptPartyInvitationMessage());
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0000AC21 File Offset: 0x00008E21
		public void DeclinePartyInvitation()
		{
			this.IsPartyInvitationPopupActive = false;
			this.CheckAndSendMessage(new DeclinePartyInvitationMessage());
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0000AC35 File Offset: 0x00008E35
		public void AcceptPartyJoinRequest(PlayerId playerId)
		{
			this.IsPartyJoinRequestPopupActive = false;
			this.CheckAndSendMessage(new AcceptPartyJoinRequestMessage(playerId));
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0000AC4A File Offset: 0x00008E4A
		public void DeclinePartyJoinRequest(PlayerId playerId, PartyJoinDeclineReason reason)
		{
			this.IsPartyJoinRequestPopupActive = false;
			this.CheckAndSendMessage(new DeclinePartyJoinRequestMessage(playerId, reason));
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0000AC60 File Offset: 0x00008E60
		public void UpdateCharacter(BodyProperties bodyProperties, bool isFemale)
		{
			this.AssertCanPerformLobbyActions();
			base.SendMessage(new UpdateCharacterMessage(bodyProperties, isFemale));
			if (this.CanPerformLobbyActions)
			{
				this.PlayerData.BodyProperties = bodyProperties;
				this.PlayerData.IsFemale = isFemale;
			}
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0000AC98 File Offset: 0x00008E98
		public async Task<bool> UpdateShownBadgeId(string shownBadgeId)
		{
			this.AssertCanPerformLobbyActions();
			UpdateShownBadgeIdMessageResult updateShownBadgeIdMessageResult = await base.CallFunction<UpdateShownBadgeIdMessageResult>(new UpdateShownBadgeIdMessage(shownBadgeId));
			if (updateShownBadgeIdMessageResult != null && updateShownBadgeIdMessageResult.Successful)
			{
				this.PlayerData.ShownBadgeId = shownBadgeId;
			}
			return updateShownBadgeIdMessageResult != null && updateShownBadgeIdMessageResult.Successful;
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0000ACE8 File Offset: 0x00008EE8
		public async Task<AnotherPlayerData> GetAnotherPlayerState(PlayerId playerId)
		{
			this.AssertCanPerformLobbyActions();
			GetAnotherPlayerStateMessageResult getAnotherPlayerStateMessageResult = await base.CallFunction<GetAnotherPlayerStateMessageResult>(new GetAnotherPlayerStateMessage(playerId));
			AnotherPlayerData anotherPlayerData;
			if (getAnotherPlayerStateMessageResult != null)
			{
				anotherPlayerData = getAnotherPlayerStateMessageResult.AnotherPlayerData;
			}
			else
			{
				anotherPlayerData = new AnotherPlayerData(AnotherPlayerState.NoAnswer, 0);
			}
			return anotherPlayerData;
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0000AD38 File Offset: 0x00008F38
		public async Task<PlayerData> GetAnotherPlayerData(PlayerId playerID)
		{
			this.AssertCanPerformLobbyActions();
			await this.WaitForPendingRequestCompletion(LobbyClient.PendingRequest.PlayerData, playerID);
			PlayerData playerData;
			PlayerData playerData2;
			if (this._cachedPlayerDatas.TryGetValue(playerID, out playerData))
			{
				playerData2 = playerData;
			}
			else
			{
				GetAnotherPlayerDataMessageResult getAnotherPlayerDataMessageResult = await this.CreatePendingRequest<GetAnotherPlayerDataMessageResult>(LobbyClient.PendingRequest.PlayerData, playerID, base.CallFunction<GetAnotherPlayerDataMessageResult>(new GetAnotherPlayerDataMessage(playerID)));
				if (((getAnotherPlayerDataMessageResult != null) ? getAnotherPlayerDataMessageResult.AnotherPlayerData : null) != null)
				{
					this._cachedPlayerDatas[playerID] = getAnotherPlayerDataMessageResult.AnotherPlayerData;
				}
				playerData2 = ((getAnotherPlayerDataMessageResult != null) ? getAnotherPlayerDataMessageResult.AnotherPlayerData : null);
			}
			return playerData2;
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0000AD88 File Offset: 0x00008F88
		public async Task<MatchmakingQueueStats> GetPlayerCountInQueue()
		{
			GetPlayerCountInQueueResult getPlayerCountInQueueResult = await base.CallFunction<GetPlayerCountInQueueResult>(new GetPlayerCountInQueue());
			MatchmakingQueueStats matchmakingQueueStats;
			if (getPlayerCountInQueueResult != null)
			{
				matchmakingQueueStats = getPlayerCountInQueueResult.MatchmakingQueueStats;
			}
			else
			{
				matchmakingQueueStats = MatchmakingQueueStats.Empty;
			}
			return matchmakingQueueStats;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0000ADD0 File Offset: 0x00008FD0
		public async Task<List<ValueTuple<PlayerId, AnotherPlayerData>>> GetOtherPlayersState(List<PlayerId> players)
		{
			this.AssertCanPerformLobbyActions();
			GetOtherPlayersStateMessageResult getOtherPlayersStateMessageResult = await base.CallFunction<GetOtherPlayersStateMessageResult>(new GetOtherPlayersStateMessage(players));
			return (getOtherPlayersStateMessageResult != null) ? getOtherPlayersStateMessageResult.States : null;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0000AE20 File Offset: 0x00009020
		public async Task<MatchmakingWaitTimeStats> GetMatchmakingWaitTimes()
		{
			GetAverageMatchmakingWaitTimesResult getAverageMatchmakingWaitTimesResult = await base.CallFunction<GetAverageMatchmakingWaitTimesResult>(new GetAverageMatchmakingWaitTimesMessage());
			MatchmakingWaitTimeStats matchmakingWaitTimeStats;
			if (getAverageMatchmakingWaitTimesResult != null)
			{
				matchmakingWaitTimeStats = getAverageMatchmakingWaitTimesResult.MatchmakingWaitTimeStats;
			}
			else
			{
				matchmakingWaitTimeStats = MatchmakingWaitTimeStats.Empty;
			}
			return matchmakingWaitTimeStats;
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0000AE68 File Offset: 0x00009068
		public async Task<Badge[]> GetPlayerBadges()
		{
			GetPlayerBadgesMessageResult getPlayerBadgesMessageResult = await base.CallFunction<GetPlayerBadgesMessageResult>(new GetPlayerBadgesMessage());
			List<Badge> list = new List<Badge>();
			if (getPlayerBadgesMessageResult != null)
			{
				string[] badges = getPlayerBadgesMessageResult.Badges;
				for (int i = 0; i < badges.Length; i++)
				{
					Badge byId = BadgeManager.GetById(badges[i]);
					if (byId != null)
					{
						list.Add(byId);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0000AEB0 File Offset: 0x000090B0
		public async Task<PlayerStatsBase[]> GetPlayerStats(PlayerId playerID)
		{
			PlayerStatsBase[] array;
			PlayerStatsBase[] array2;
			if (this._cachedPlayerStats.TryGetValue(playerID, out array))
			{
				array2 = array;
			}
			else
			{
				GetPlayerStatsMessageResult getPlayerStatsMessageResult = await base.CallFunction<GetPlayerStatsMessageResult>(new GetPlayerStatsMessage(playerID));
				if (((getPlayerStatsMessageResult != null) ? getPlayerStatsMessageResult.PlayerStats : null) != null)
				{
					this._cachedPlayerStats[playerID] = getPlayerStatsMessageResult.PlayerStats;
				}
				array2 = ((getPlayerStatsMessageResult != null) ? getPlayerStatsMessageResult.PlayerStats : null);
			}
			return array2;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0000AF00 File Offset: 0x00009100
		public async Task<GameTypeRankInfo[]> GetGameTypeRankInfo(PlayerId playerID)
		{
			await this.WaitForPendingRequestCompletion(LobbyClient.PendingRequest.RankInfo, playerID);
			GameTypeRankInfo[] array;
			GameTypeRankInfo[] array2;
			if (this._cachedRankInfos.TryGetValue(playerID, out array))
			{
				array2 = array;
			}
			else
			{
				GetPlayerGameTypeRankInfoMessageResult getPlayerGameTypeRankInfoMessageResult = await this.CreatePendingRequest<GetPlayerGameTypeRankInfoMessageResult>(LobbyClient.PendingRequest.RankInfo, playerID, base.CallFunction<GetPlayerGameTypeRankInfoMessageResult>(new GetPlayerGameTypeRankInfoMessage(playerID)));
				if (((getPlayerGameTypeRankInfoMessageResult != null) ? getPlayerGameTypeRankInfoMessageResult.GameTypeRankInfo : null) != null)
				{
					this._cachedRankInfos[playerID] = getPlayerGameTypeRankInfoMessageResult.GameTypeRankInfo;
				}
				array2 = ((getPlayerGameTypeRankInfoMessageResult != null) ? getPlayerGameTypeRankInfoMessageResult.GameTypeRankInfo : null);
			}
			return array2;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0000AF50 File Offset: 0x00009150
		public async Task<int> GetRankedLeaderboardCount(string gameType)
		{
			GetRankedLeaderboardCountMessageResult getRankedLeaderboardCountMessageResult = await base.CallFunction<GetRankedLeaderboardCountMessageResult>(new GetRankedLeaderboardCountMessage(gameType));
			return (getRankedLeaderboardCountMessageResult != null) ? getRankedLeaderboardCountMessageResult.Count : 0;
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0000AFA0 File Offset: 0x000091A0
		public async Task<PlayerLeaderboardData[]> GetRankedLeaderboard(string gameType, int startIndex, int count)
		{
			GetRankedLeaderboardMessageResult getRankedLeaderboardMessageResult = await base.CallFunction<GetRankedLeaderboardMessageResult>(new GetRankedLeaderboardMessage(gameType, startIndex, count));
			return (getRankedLeaderboardMessageResult != null) ? getRankedLeaderboardMessageResult.LeaderboardPlayers : null;
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0000AFFD File Offset: 0x000091FD
		public void SendCreateClanMessage(string clanName, string clanTag, string clanFaction, string clanSigil)
		{
			this.AssertCanPerformLobbyActions();
			base.SendMessage(new CreateClanMessage(clanName, clanTag, clanFaction, clanSigil));
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0000B015 File Offset: 0x00009215
		public void GetFriendList()
		{
			this.CheckAndSendMessage(new GetFriendListMessage());
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0000B022 File Offset: 0x00009222
		public void AddFriend(PlayerId friendId, bool dontUseNameForUnknownPlayer)
		{
			this.CheckAndSendMessage(new AddFriendMessage(friendId, dontUseNameForUnknownPlayer));
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0000B031 File Offset: 0x00009231
		public void RemoveFriend(PlayerId friendId)
		{
			this.CheckAndSendMessage(new RemoveFriendMessage(friendId));
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0000B03F File Offset: 0x0000923F
		public void RespondToFriendRequest(PlayerId playerId, bool dontUseNameForUnknownPlayer, bool isAccepted, bool isBlocked = false)
		{
			this.CheckAndSendMessage(new FriendRequestResponseMessage(playerId, dontUseNameForUnknownPlayer, isAccepted, isBlocked));
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0000B054 File Offset: 0x00009254
		public void ReportPlayer(string gameId, PlayerId player, string playerName, PlayerReportType type, string message)
		{
			Guid guid;
			if (Guid.TryParse(gameId, out guid))
			{
				this.CheckAndSendMessage(new ReportPlayerMessage(guid, player, playerName, type, message));
				return;
			}
			ILobbyClientSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnSystemMessageReceived(new TextObject("{=dnKQbXIZ}Could not report player: Game does not exist.", null).ToString());
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0000B0A0 File Offset: 0x000092A0
		public void ChangeUsername(string username)
		{
			if ((this.PlayerData == null || this.PlayerData.Username != username) && username != null && username.Length >= Parameters.UsernameMinLength && username.Length <= Parameters.UsernameMaxLength && Common.IsAllLetters(username))
			{
				this.CheckAndSendMessage(new ChangeUsernameMessage(username));
			}
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0000B0FC File Offset: 0x000092FC
		public void AddFriendByUsernameAndId(string username, int userId, bool dontUseNameForUnknownPlayer)
		{
			if (username != null && username.Length >= Parameters.UsernameMinLength && username.Length <= Parameters.UsernameMaxLength && Common.IsAllLetters(username) && userId >= 0 && userId <= Parameters.UserIdMax)
			{
				this.CheckAndSendMessage(new AddFriendByUsernameAndIdMessage(username, userId, dontUseNameForUnknownPlayer));
			}
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0000B148 File Offset: 0x00009348
		public async Task<bool> DoesPlayerWithUsernameAndIdExist(string username, int userId)
		{
			bool flag;
			if (username != null && username.Length >= Parameters.UsernameMinLength && username.Length <= Parameters.UsernameMaxLength && Common.IsAllLetters(username) && userId >= 0 && userId <= Parameters.UserIdMax)
			{
				GetPlayerByUsernameAndIdMessageResult getPlayerByUsernameAndIdMessageResult = await base.CallFunction<GetPlayerByUsernameAndIdMessageResult>(new GetPlayerByUsernameAndIdMessage(username, userId));
				flag = getPlayerByUsernameAndIdMessageResult != null && getPlayerByUsernameAndIdMessageResult.PlayerId.IsValid;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0000B1A0 File Offset: 0x000093A0
		public bool IsPlayerClanLeader(PlayerId playerID)
		{
			ClanPlayer clanPlayer = this.PlayersInClan.Find((ClanPlayer p) => p.PlayerId == playerID);
			return clanPlayer != null && clanPlayer.Role == ClanPlayerRole.Leader;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0000B1E0 File Offset: 0x000093E0
		public bool IsPlayerClanOfficer(PlayerId playerID)
		{
			ClanPlayer clanPlayer = this.PlayersInClan.Find((ClanPlayer p) => p.PlayerId == playerID);
			return clanPlayer != null && clanPlayer.Role == ClanPlayerRole.Officer;
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0000B220 File Offset: 0x00009420
		public async Task<bool> UpdateUsedCosmeticItems([TupleElementNames(new string[] { "cosmeticId", "isEquipped" })] Dictionary<string, List<ValueTuple<string, bool>>> usedCosmetics)
		{
			List<CosmeticItemInfo> list = new List<CosmeticItemInfo>();
			foreach (string text in usedCosmetics.Keys)
			{
				foreach (ValueTuple<string, bool> valueTuple in usedCosmetics[text])
				{
					CosmeticItemInfo cosmeticItemInfo = new CosmeticItemInfo(text, valueTuple.Item1, valueTuple.Item2);
					list.Add(cosmeticItemInfo);
				}
			}
			UpdateUsedCosmeticItemsMessageResult updateUsedCosmeticItemsMessageResult = await base.CallFunction<UpdateUsedCosmeticItemsMessageResult>(new UpdateUsedCosmeticItemsMessage(list));
			if (updateUsedCosmeticItemsMessageResult != null && updateUsedCosmeticItemsMessageResult.Successful)
			{
				foreach (KeyValuePair<string, List<ValueTuple<string, bool>>> keyValuePair in usedCosmetics)
				{
					if (!string.IsNullOrWhiteSpace(keyValuePair.Key))
					{
						List<string> list2;
						if (!this.UsedCosmetics.TryGetValue(keyValuePair.Key, out list2))
						{
							list2 = new List<string>();
							this._usedCosmetics.Add(keyValuePair.Key, list2);
						}
						foreach (ValueTuple<string, bool> valueTuple2 in keyValuePair.Value)
						{
							string item = valueTuple2.Item1;
							if (valueTuple2.Item2)
							{
								list2.Add(item);
							}
							else
							{
								list2.Remove(item);
							}
						}
					}
				}
			}
			return updateUsedCosmeticItemsMessageResult != null && updateUsedCosmeticItemsMessageResult.Successful;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0000B270 File Offset: 0x00009470
		[return: TupleElementNames(new string[] { "isSuccessful", "finalGold" })]
		public async Task<ValueTuple<bool, int>> BuyCosmetic(string cosmeticId)
		{
			BuyCosmeticMessageResult buyCosmeticMessageResult = await base.CallFunction<BuyCosmeticMessageResult>(new BuyCosmeticMessage(cosmeticId));
			if (buyCosmeticMessageResult != null && buyCosmeticMessageResult.Successful)
			{
				this._ownedCosmetics.Add(cosmeticId);
			}
			return new ValueTuple<bool, int>(buyCosmeticMessageResult != null && buyCosmeticMessageResult.Successful, (buyCosmeticMessageResult != null) ? buyCosmeticMessageResult.Gold : 0);
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0000B2C0 File Offset: 0x000094C0
		[return: TupleElementNames(new string[] { "isSuccessful", "ownedCosmetics", "usedCosmetics" })]
		public async Task<ValueTuple<bool, List<string>, Dictionary<string, List<string>>>> GetCosmeticsInfo()
		{
			GetUserCosmeticsInfoMessageResult getUserCosmeticsInfoMessageResult = await base.CallFunction<GetUserCosmeticsInfoMessageResult>(new GetUserCosmeticsInfoMessage());
			if (getUserCosmeticsInfoMessageResult != null)
			{
				this._usedCosmetics = getUserCosmeticsInfoMessageResult.UsedCosmetics ?? new Dictionary<string, List<string>>();
				this._ownedCosmetics = getUserCosmeticsInfoMessageResult.OwnedCosmetics ?? new List<string>();
			}
			return new ValueTuple<bool, List<string>, Dictionary<string, List<string>>>(getUserCosmeticsInfoMessageResult != null && getUserCosmeticsInfoMessageResult.Successful, (getUserCosmeticsInfoMessageResult != null) ? getUserCosmeticsInfoMessageResult.OwnedCosmetics : null, (getUserCosmeticsInfoMessageResult != null) ? getUserCosmeticsInfoMessageResult.UsedCosmetics : null);
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0000B308 File Offset: 0x00009508
		public async Task<string> GetDedicatedCustomServerAuthToken()
		{
			GetDedicatedCustomServerAuthTokenMessageResult getDedicatedCustomServerAuthTokenMessageResult = await base.CallFunction<GetDedicatedCustomServerAuthTokenMessageResult>(new GetDedicatedCustomServerAuthTokenMessage());
			return (getDedicatedCustomServerAuthTokenMessageResult != null) ? getDedicatedCustomServerAuthTokenMessageResult.AuthToken : null;
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0000B350 File Offset: 0x00009550
		public async Task<string> GetOfficialServerProviderName()
		{
			GetOfficialServerProviderNameResult getOfficialServerProviderNameResult = await base.CallFunction<GetOfficialServerProviderNameResult>(new GetOfficialServerProviderNameMessage());
			return ((getOfficialServerProviderNameResult != null) ? getOfficialServerProviderNameResult.Name : null) ?? string.Empty;
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0000B398 File Offset: 0x00009598
		public async Task<string> GetPlayerBannerlordID(PlayerId playerId)
		{
			await this.WaitForPendingRequestCompletion(LobbyClient.PendingRequest.BannerlordID, playerId);
			string text;
			string text2;
			if (this._cachedPlayerBannerlordIDs.TryGetValue(playerId, out text))
			{
				text2 = text;
			}
			else
			{
				GetBannerlordIDMessageResult getBannerlordIDMessageResult = await this.CreatePendingRequest<GetBannerlordIDMessageResult>(LobbyClient.PendingRequest.BannerlordID, playerId, base.CallFunction<GetBannerlordIDMessageResult>(new GetBannerlordIDMessage(playerId)));
				if (getBannerlordIDMessageResult != null && getBannerlordIDMessageResult.BannerlordID != null)
				{
					this._cachedPlayerBannerlordIDs[playerId] = getBannerlordIDMessageResult.BannerlordID;
				}
				text2 = ((getBannerlordIDMessageResult != null) ? getBannerlordIDMessageResult.BannerlordID : null) ?? string.Empty;
			}
			return text2;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0000B3E8 File Offset: 0x000095E8
		public bool IsKnownPlayer(PlayerId playerID)
		{
			bool flag = playerID == this._playerId;
			bool flag2 = this.FriendIDs.Contains(playerID);
			bool flag3 = this.IsInParty && this.PlayersInParty.Any<PartyPlayerInLobbyClient>((PartyPlayerInLobbyClient p) => p.PlayerId.Equals(playerID));
			bool flag4 = this.IsInClan && this.PlayersInClan.Any<ClanPlayer>((ClanPlayer p) => p.PlayerId.Equals(playerID));
			return flag || flag2 || flag3 || flag4;
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0000B474 File Offset: 0x00009674
		public async Task<long> GetPingToServer(string IpAddress)
		{
			long num;
			try
			{
				using (Ping ping = new Ping())
				{
					PingReply pingReply = await ping.SendPingAsync(IpAddress, (int)TimeSpan.FromSeconds(15.0).TotalMilliseconds);
					num = ((pingReply.Status != IPStatus.Success) ? (-1L) : pingReply.RoundtripTime);
				}
			}
			catch (Exception)
			{
				num = -1L;
			}
			return num;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0000B4B9 File Offset: 0x000096B9
		private void AssertCanPerformLobbyActions()
		{
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0000B4BC File Offset: 0x000096BC
		public async Task<bool> SendPSPlayerJoinedToPlayerSessionMessage(ulong inviterPlayerId)
		{
			PSPlayerJoinedToPlayerSessionMessage psplayerJoinedToPlayerSessionMessage = new PSPlayerJoinedToPlayerSessionMessage(inviterPlayerId);
			return (await base.CallFunction<PSPlayerJoinedToPlayerSessionMessageResult>(psplayerJoinedToPlayerSessionMessage)).Successful;
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0000B50C File Offset: 0x0000970C
		public async Task<bool> SendPlatformPlayerJoinedToPlayerSessionMessage(PlayerId inviterPlayerId)
		{
			PlatformPlayerJoinedToPlayerSessionMessage platformPlayerJoinedToPlayerSessionMessage = new PlatformPlayerJoinedToPlayerSessionMessage(inviterPlayerId);
			return (await base.CallFunction<PSPlayerJoinedToPlayerSessionMessageResult>(platformPlayerJoinedToPlayerSessionMessage)).Successful;
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0000B55C File Offset: 0x0000975C
		private Task WaitForPendingRequestCompletion(LobbyClient.PendingRequest requestType, PlayerId playerId)
		{
			Task task;
			if (this._pendingPlayerRequests.TryGetValue(new ValueTuple<LobbyClient.PendingRequest, PlayerId>(requestType, playerId), out task))
			{
				return task;
			}
			return Task.CompletedTask;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0000B588 File Offset: 0x00009788
		private async Task<T> CreatePendingRequest<T>(LobbyClient.PendingRequest requestType, PlayerId playerId, Task<T> requestTask)
		{
			ValueTuple<LobbyClient.PendingRequest, PlayerId> key = new ValueTuple<LobbyClient.PendingRequest, PlayerId>(requestType, playerId);
			T t;
			try
			{
				this._pendingPlayerRequests[key] = requestTask;
				t = await requestTask;
			}
			finally
			{
				this._pendingPlayerRequests.Remove(key);
			}
			return t;
		}

		// Token: 0x040002D5 RID: 725
		public const string TestRegionCode = "Test";

		// Token: 0x040002D6 RID: 726
		private static readonly int ServerStatusCheckDelay = 120000;

		// Token: 0x040002D7 RID: 727
		private static int _friendListCheckDelay;

		// Token: 0x040002D8 RID: 728
		private static readonly int CheckForCustomGamesCount = 5;

		// Token: 0x040002D9 RID: 729
		private static readonly int CheckForCustomGamesDelay = 5000;

		// Token: 0x040002DA RID: 730
		private ILobbyClientSessionHandler _handler;

		// Token: 0x040002DB RID: 731
		private readonly Stopwatch _serverStatusTimer;

		// Token: 0x040002DC RID: 732
		private readonly Stopwatch _friendListTimer;

		// Token: 0x040002DD RID: 733
		private readonly Stopwatch _recentPlayersTimer;

		// Token: 0x040002DE RID: 734
		private static readonly int RecentPlayersCheckDelay = 40000;

		// Token: 0x040002E3 RID: 739
		private List<string> _ownedCosmetics;

		// Token: 0x040002E4 RID: 740
		private Dictionary<string, List<string>> _usedCosmetics;

		// Token: 0x040002E7 RID: 743
		private ServerStatus _serverStatus;

		// Token: 0x040002E8 RID: 744
		private DateTime _matchmakerBlockedTime;

		// Token: 0x040002E9 RID: 745
		private TextObject _logOutReason;

		// Token: 0x040002EA RID: 746
		private LobbyClient.State _state;

		// Token: 0x040002EB RID: 747
		private string _userName;

		// Token: 0x040002EC RID: 748
		private PlayerId _playerId;

		// Token: 0x040002F0 RID: 752
		private List<ModuleInfoModel> _loadedUnofficialModules;

		// Token: 0x04000301 RID: 769
		private TimedDictionaryCache<PlayerId, GameTypeRankInfo[]> _cachedRankInfos;

		// Token: 0x04000302 RID: 770
		private TimedDictionaryCache<PlayerId, PlayerStatsBase[]> _cachedPlayerStats;

		// Token: 0x04000303 RID: 771
		private TimedDictionaryCache<PlayerId, PlayerData> _cachedPlayerDatas;

		// Token: 0x04000304 RID: 772
		private TimedDictionaryCache<PlayerId, string> _cachedPlayerBannerlordIDs;

		// Token: 0x04000305 RID: 773
		private Dictionary<ValueTuple<LobbyClient.PendingRequest, PlayerId>, Task> _pendingPlayerRequests;

		// Token: 0x02000195 RID: 405
		public enum State
		{
			// Token: 0x04000591 RID: 1425
			Idle,
			// Token: 0x04000592 RID: 1426
			Working,
			// Token: 0x04000593 RID: 1427
			Connected,
			// Token: 0x04000594 RID: 1428
			SessionRequested,
			// Token: 0x04000595 RID: 1429
			AtLobby,
			// Token: 0x04000596 RID: 1430
			SearchingToRejoinBattle,
			// Token: 0x04000597 RID: 1431
			RequestingToSearchBattle,
			// Token: 0x04000598 RID: 1432
			RequestingToCancelSearchBattle,
			// Token: 0x04000599 RID: 1433
			SearchingBattle,
			// Token: 0x0400059A RID: 1434
			AtBattle,
			// Token: 0x0400059B RID: 1435
			QuittingFromBattle,
			// Token: 0x0400059C RID: 1436
			WaitingToCreatePremadeGame,
			// Token: 0x0400059D RID: 1437
			WaitingToJoinPremadeGame,
			// Token: 0x0400059E RID: 1438
			WaitingToRegisterCustomGame,
			// Token: 0x0400059F RID: 1439
			HostingCustomGame,
			// Token: 0x040005A0 RID: 1440
			WaitingToJoinCustomGame,
			// Token: 0x040005A1 RID: 1441
			InCustomGame
		}

		// Token: 0x02000196 RID: 406
		private enum PendingRequest
		{
			// Token: 0x040005A3 RID: 1443
			RankInfo,
			// Token: 0x040005A4 RID: 1444
			PlayerData,
			// Token: 0x040005A5 RID: 1445
			BannerlordID
		}
	}
}
