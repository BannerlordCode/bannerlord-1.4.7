using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Messages.FromBattleServer.ToBattleServerManager;
using Messages.FromBattleServerManager.ToBattleServer;
using TaleWorlds.Diamond;
using TaleWorlds.Diamond.ClientApplication;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000FC RID: 252
	public class BattleServer : Client<BattleServer>
	{
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x00005D9B File Offset: 0x00003F9B
		// (set) Token: 0x0600050F RID: 1295 RVA: 0x00005DA3 File Offset: 0x00003FA3
		public string SceneName { get; private set; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x00005DAC File Offset: 0x00003FAC
		// (set) Token: 0x06000511 RID: 1297 RVA: 0x00005DB4 File Offset: 0x00003FB4
		public string GameType { get; private set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x00005DBD File Offset: 0x00003FBD
		// (set) Token: 0x06000513 RID: 1299 RVA: 0x00005DC5 File Offset: 0x00003FC5
		public string Faction1 { get; private set; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x00005DCE File Offset: 0x00003FCE
		// (set) Token: 0x06000515 RID: 1301 RVA: 0x00005DD6 File Offset: 0x00003FD6
		public string Faction2 { get; private set; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x00005DDF File Offset: 0x00003FDF
		// (set) Token: 0x06000517 RID: 1303 RVA: 0x00005DE7 File Offset: 0x00003FE7
		public int MinRequiredPlayerCountToStartBattle { get; private set; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x00005DF0 File Offset: 0x00003FF0
		// (set) Token: 0x06000519 RID: 1305 RVA: 0x00005DF8 File Offset: 0x00003FF8
		public int BattleSize { get; private set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x00005E01 File Offset: 0x00004001
		// (set) Token: 0x0600051B RID: 1307 RVA: 0x00005E09 File Offset: 0x00004009
		public int RoundThreshold { get; private set; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x00005E12 File Offset: 0x00004012
		// (set) Token: 0x0600051D RID: 1309 RVA: 0x00005E1A File Offset: 0x0000401A
		public float MoraleThreshold { get; private set; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x00005E23 File Offset: 0x00004023
		// (set) Token: 0x0600051F RID: 1311 RVA: 0x00005E2B File Offset: 0x0000402B
		public Guid BattleId { get; private set; }

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x00005E34 File Offset: 0x00004034
		// (set) Token: 0x06000521 RID: 1313 RVA: 0x00005E3C File Offset: 0x0000403C
		public bool UseAnalytics { get; private set; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x00005E45 File Offset: 0x00004045
		// (set) Token: 0x06000523 RID: 1315 RVA: 0x00005E4D File Offset: 0x0000404D
		public bool CaptureMovementData { get; private set; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x00005E56 File Offset: 0x00004056
		// (set) Token: 0x06000525 RID: 1317 RVA: 0x00005E5E File Offset: 0x0000405E
		public string AnalyticsServiceAddress { get; private set; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x00005E67 File Offset: 0x00004067
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x00005E6F File Offset: 0x0000406F
		public bool IsPremadeGame { get; private set; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00005E78 File Offset: 0x00004078
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x00005E80 File Offset: 0x00004080
		public PremadeGameType PremadeGameType { get; private set; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00005E89 File Offset: 0x00004089
		// (set) Token: 0x0600052B RID: 1323 RVA: 0x00005E91 File Offset: 0x00004091
		public PlayerId[] AssignedPlayers { get; private set; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00005E9A File Offset: 0x0000409A
		public bool IsActive
		{
			get
			{
				return this._state == BattleServer.State.BattleAssigned || this._state == BattleServer.State.Running || this._state == BattleServer.State.WaitingBattle;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x00005EB9 File Offset: 0x000040B9
		public bool IsFinished
		{
			get
			{
				return this._state == BattleServer.State.Finished;
			}
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00005EC4 File Offset: 0x000040C4
		public BattleServer(DiamondClientApplication diamondClientApplication, IClientSessionProvider<BattleServer> provider)
			: base(diamondClientApplication, provider, false)
		{
			this._state = BattleServer.State.Idle;
			this._peerId = new PeerId(Guid.NewGuid());
			base.Application.Parameters.TryGetParameter("BattleServer.Host.Address", out this._assignedAddress);
			base.Application.Parameters.TryGetParameterAsUInt16("BattleServer.Host.Port", out this._assignedPort);
			base.Application.Parameters.TryGetParameter("BattleServer.Host.Region", out this._region);
			base.Application.Parameters.TryGetParameterAsSByte("BattleServer.Host.Priority", out this._priority);
			base.Application.Parameters.TryGetParameterAsByte("BattleServer.Host.NumCores", out this._numCores);
			base.Application.Parameters.TryGetParameter("BattleServer.Password", out this._password);
			base.Application.Parameters.TryGetParameter("BattleServer.Host.GameMode", out this._gameMode);
			if (!base.Application.Parameters.TryGetParameterAsInt("BattleServer.TimeoutDuration", out this._timeoutDuration))
			{
				this._timeoutDuration = this._defaultServerTimeoutDuration;
			}
			this._passedTimeSinceLastMaxAllowedPriorityRequest = this._requestMaxAllowedPriorityIntervalInSeconds * 2f;
			this._peers = new List<BattlePeer>();
			this._timer = new Stopwatch();
			this._timer.Start();
			this._timeoutTimer = new Stopwatch();
			this._terminationTime = null;
			this._maxAllowedPriority = sbyte.MaxValue;
			this._newPlayerRequests = new Queue<NewPlayerMessage>();
			this._isWarmupEnded = false;
			this._playerSpawnCounts = new Dictionary<PlayerId, int>();
			this._badgeComponent = null;
			this._playerPartyMap = new Dictionary<PlayerId, Guid>();
			this._playerRoundFriendlyDamageMap = new Dictionary<PlayerId, Dictionary<int, ValueTuple<int, float>>>();
			this._maxFriendlyKillCount = int.MaxValue;
			this._maxFriendlyDamage = float.MaxValue;
			this._maxFriendlyDamagePerSingleRound = float.MaxValue;
			this._roundFriendlyDamageLimit = float.MaxValue;
			this._maxRoundsOverLimitCount = int.MaxValue;
			base.AddMessageHandler<NewPlayerMessage>(new ClientMessageHandler<NewPlayerMessage>(this.OnNewPlayerMessage));
			base.AddMessageHandler<StartBattleMessage>(new ClientMessageHandler<StartBattleMessage>(this.OnStartBattleMessage));
			base.AddMessageHandler<PlayerFledBattleMessage>(new ClientMessageHandler<PlayerFledBattleMessage>(this.OnPlayerFledBattleMessage));
			base.AddMessageHandler<PlayerDisconnectedFromLobbyMessage>(new ClientMessageHandler<PlayerDisconnectedFromLobbyMessage>(this.OnPlayerDisconnectedFromLobbyMessage));
			base.AddMessageHandler<TerminateOperationMatchmakingMessage>(new ClientMessageHandler<TerminateOperationMatchmakingMessage>(this.OnTerminateOperationMatchmakingMessage));
			base.AddMessageHandler<FriendlyDamageKickPlayerResponseMessage>(new ClientMessageHandler<FriendlyDamageKickPlayerResponseMessage>(this.OnFriendlyDamageKickPlayerResponseMessage));
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0000612A File Offset: 0x0000432A
		public void Initialize(IBattleServerSessionHandler handler)
		{
			this._handler = handler;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00006133 File Offset: 0x00004333
		public void SetBadgeComponent(IBadgeComponent badgeComponent)
		{
			this._badgeComponent = badgeComponent;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0000613C File Offset: 0x0000433C
		public void StartServer()
		{
			this._state = BattleServer.State.Connecting;
			base.BeginConnect();
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0000614C File Offset: 0x0000434C
		protected override void OnTick()
		{
			if (this._terminationTime != null && this._terminationTime < DateTime.UtcNow)
			{
				throw new Exception("I am sorry Dave, I am afraid I can't do that");
			}
			long elapsedMilliseconds = this._timer.ElapsedMilliseconds;
			float num = (float)(elapsedMilliseconds - this._previousTimeInMS);
			this._previousTimeInMS = elapsedMilliseconds;
			float num2 = num / 1000f;
			this._passedTimeSinceLastMaxAllowedPriorityRequest += num2;
			this._battleResultUpdateTimeElapsed += num2;
			if (this._battleResultUpdateTimeElapsed >= 5f)
			{
				if (this._latestQueuedBattleResult != null && this._latestQueuedTeamScores != null)
				{
					base.SendMessage(new BattleServerStatsUpdateMessage(this._latestQueuedBattleResult, this._latestQueuedTeamScores));
					this._latestQueuedBattleResult = null;
					this._latestQueuedTeamScores = null;
				}
				this._battleResultUpdateTimeElapsed = 0f;
			}
			switch (this._state)
			{
			case BattleServer.State.Idle:
			case BattleServer.State.Connecting:
			case BattleServer.State.Connected:
			case BattleServer.State.LoggingIn:
			case BattleServer.State.BattleAssigned:
			case BattleServer.State.Running:
			case BattleServer.State.Finishing:
			case BattleServer.State.Finished:
				break;
			case BattleServer.State.WaitingBattle:
				if (this._passedTimeSinceLastMaxAllowedPriorityRequest > this._requestMaxAllowedPriorityIntervalInSeconds)
				{
					this.UpdateMaxAllowedPriority();
				}
				if (this._priority > this._maxAllowedPriority || this._timeoutTimer.ElapsedMilliseconds > (long)this._timeoutDuration)
				{
					this.Shutdown();
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00006298 File Offset: 0x00004498
		private async void DoLogin()
		{
			this._state = BattleServer.State.LoggingIn;
			LoginResult loginResult = await base.Login(new BattleServerReadyMessage(this._peerId, base.ApplicationVersion, this._assignedAddress, this._assignedPort, this._region, this._priority, this._password, this._gameMode));
			if (loginResult != null && loginResult.Successful)
			{
				this._state = BattleServer.State.WaitingBattle;
				this._timeoutTimer.Reset();
				this._timeoutTimer.Start();
			}
			else
			{
				this._state = BattleServer.State.Finished;
			}
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x000062D1 File Offset: 0x000044D1
		public override void OnConnected()
		{
			base.OnConnected();
			this._state = BattleServer.State.Connected;
			this._handler.OnConnected();
			this.DoLogin();
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x000062F1 File Offset: 0x000044F1
		public override void OnCantConnect()
		{
			base.OnCantConnect();
			this._handler.OnCantConnect();
			this._state = BattleServer.State.Finished;
			if (this._handler != null)
			{
				this._handler.OnStopServer();
			}
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0000631E File Offset: 0x0000451E
		public override void OnDisconnected()
		{
			base.OnDisconnected();
			this._handler.OnDisconnected();
			this._state = BattleServer.State.Finished;
			if (this._handler != null)
			{
				this._handler.OnStopServer();
			}
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0000634C File Offset: 0x0000454C
		private void OnNewPlayerMessage(NewPlayerMessage message)
		{
			if (this._battleBecomeReady)
			{
				PlayerBattleInfo playerBattleInfo = message.PlayerBattleInfo;
				PlayerData playerData = message.PlayerData;
				this.ProcessNewPlayer(playerBattleInfo, playerData, message.PlayerParty, message.UsedCosmetics);
				return;
			}
			this._newPlayerRequests.Enqueue(message);
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00006390 File Offset: 0x00004590
		private void ProcessNewPlayer(PlayerBattleInfo playerBattleInfo, PlayerData playerData, Guid playerParty, Dictionary<string, List<string>> usedCosmetics)
		{
			string name = playerBattleInfo.Name;
			PlayerId playerId = playerBattleInfo.PlayerId;
			int teamNo = playerBattleInfo.TeamNo;
			this._playerPartyMap[playerData.PlayerId] = playerParty;
			BattlePeer battlePeer = this.GetPeer(playerId);
			if (battlePeer == null)
			{
				battlePeer = new BattlePeer(name, playerData, usedCosmetics, teamNo, playerBattleInfo.JoinType);
				this._peers.Add(battlePeer);
			}
			else
			{
				battlePeer.Rejoin(teamNo);
			}
			if (!this._playerSpawnCounts.ContainsKey(playerId))
			{
				this._playerSpawnCounts.Add(playerId, 0);
			}
			this._handler.OnNewPlayer(battlePeer);
			IBadgeComponent badgeComponent = this._badgeComponent;
			if (badgeComponent != null)
			{
				badgeComponent.OnPlayerJoin(playerData);
			}
			PlayerBattleServerInformation playerBattleServerInformation = new PlayerBattleServerInformation(battlePeer.Index, battlePeer.SessionKey);
			base.SendMessage(new NewPlayerResponseMessage(playerId, playerBattleServerInformation));
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0000644F File Offset: 0x0000464F
		public void BeginEndMission()
		{
			this._state = BattleServer.State.Finishing;
			base.SendMessage(new BattleEndingMessage());
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00006464 File Offset: 0x00004664
		public void EndMission(BattleResult battleResult, GameLog[] gameLogs, int gameTime, Dictionary<int, int> teamScores, Dictionary<PlayerId, int> playerScores)
		{
			this._state = BattleServer.State.Finished;
			this.SetBattleJoinTypes(battleResult);
			IBadgeComponent badgeComponent = this._badgeComponent;
			base.SendMessage(new BattleEndedMessage(battleResult, gameLogs, (badgeComponent != null) ? badgeComponent.DataDictionary : null, gameTime, teamScores, playerScores));
			if (this._handler != null)
			{
				this._handler.OnEndMission();
			}
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x000064B6 File Offset: 0x000046B6
		public void BattleCancelledForPlayerLeaving(PlayerId leaverID)
		{
			base.SendMessage(new BattleCancelledDueToPlayerQuitMessage(leaverID, this.GameType));
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x000064CC File Offset: 0x000046CC
		public void BattleStarted(BattleResult battleResult)
		{
			if (this._shouldReportActivities)
			{
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				foreach (KeyValuePair<string, BattlePlayerEntry> keyValuePair in battleResult.PlayerEntries)
				{
					dictionary.Add(keyValuePair.Key, keyValuePair.Value.TeamNo);
				}
				base.SendMessage(new BattleStartedMessage(true, dictionary));
				return;
			}
			base.SendMessage(new BattleStartedMessage(false));
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0000655C File Offset: 0x0000475C
		public void UpdateBattleStats(BattleResult battleResult, Dictionary<int, int> teamScores)
		{
			if (this._shouldReportActivities)
			{
				this._latestQueuedBattleResult = battleResult;
				this._latestQueuedTeamScores = teamScores;
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00006574 File Offset: 0x00004774
		private void Shutdown()
		{
			this._state = BattleServer.State.Finished;
			base.BeginDisconnect();
			this._handler.OnDisconnected();
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00006590 File Offset: 0x00004790
		private void OnStartBattleMessage(StartBattleMessage message)
		{
			this.BattleId = message.BattleId;
			this.SceneName = message.SceneName;
			this.Faction1 = message.Faction1;
			this.Faction2 = message.Faction2;
			this.GameType = message.GameType;
			this.MinRequiredPlayerCountToStartBattle = message.MinRequiredPlayerCountToStartBattle;
			this.BattleSize = message.BattleSize;
			this.RoundThreshold = message.RoundThreshold;
			this.MoraleThreshold = message.MoraleThreshold;
			this.UseAnalytics = message.UseAnalytics;
			this.CaptureMovementData = message.CaptureMovementData;
			this.AnalyticsServiceAddress = message.AnalyticsServiceAddress;
			this.IsPremadeGame = message.IsPremadeGame;
			this.PremadeGameType = message.PremadeGameType;
			this.AssignedPlayers = message.AssignedPlayers;
			this._maxFriendlyKillCount = message.MaxFriendlyKillCount;
			this._maxFriendlyDamage = message.MaxFriendlyDamage;
			this._maxFriendlyDamagePerSingleRound = message.MaxFriendlyDamagePerSingleRound;
			this._roundFriendlyDamageLimit = message.RoundFriendlyDamageLimit;
			this._maxRoundsOverLimitCount = message.MaxRoundsOverLimitCount;
			this._handler.OnStartGame(this.SceneName, this.GameType, this.Faction1, this.Faction2, this.MinRequiredPlayerCountToStartBattle, this.BattleSize, message.ProfanityList, message.AllowList);
			this._state = BattleServer.State.BattleAssigned;
			base.SendMessage(new BattleInitializedMessage(this.GameType, this.AssignedPlayers.ToList<PlayerId>(), this.Faction2, this.Faction1));
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x000066F8 File Offset: 0x000048F8
		private void OnPlayerFledBattleMessage(PlayerFledBattleMessage message)
		{
			if (this._state == BattleServer.State.Finished)
			{
				return;
			}
			PlayerId playerId = message.PlayerId;
			BattlePeer battlePeer = this._peers.First<BattlePeer>((BattlePeer peer) => peer.PlayerId == playerId);
			if (!battlePeer.Quit)
			{
				battlePeer.Flee();
				BattleResult battleResult;
				this._handler.OnPlayerFledBattle(battlePeer, out battleResult, true);
				int num;
				bool flag = !this._isWarmupEnded || this._state == BattleServer.State.Finishing || !this._playerSpawnCounts.TryGetValue(playerId, out num) || num <= 0;
				base.SendMessage(new PlayerFledBattleAnswerMessage(playerId, battleResult, flag));
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0000679C File Offset: 0x0000499C
		private void OnPlayerDisconnectedFromLobbyMessage(PlayerDisconnectedFromLobbyMessage message)
		{
			PlayerId playerId = message.PlayerId;
			BattlePeer battlePeer = this._peers.First<BattlePeer>((BattlePeer peer) => peer.PlayerId == playerId);
			if (!battlePeer.Quit)
			{
				BattleResult battleResult;
				this._handler.OnPlayerFledBattle(battlePeer, out battleResult, false);
				battlePeer.SetPlayerDisconnectdFromLobby();
			}
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x000067F0 File Offset: 0x000049F0
		private void OnFriendlyDamageKickPlayerResponseMessage(FriendlyDamageKickPlayerResponseMessage message)
		{
			PlayerId playerId = message.PlayerId;
			BattlePeer battlePeer = this._peers.First<BattlePeer>((BattlePeer peer) => peer.PlayerId == playerId);
			if (!battlePeer.Quit)
			{
				BattleResult battleResult;
				this._handler.OnPlayerFledBattle(battlePeer, out battleResult, false);
				battlePeer.SetPlayerKickedDueToFriendlyDamage();
			}
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00006844 File Offset: 0x00004A44
		private void OnTerminateOperationMatchmakingMessage(TerminateOperationMatchmakingMessage message)
		{
			Random random = new Random();
			this._terminationTime = new DateTime?(DateTime.UtcNow.AddMilliseconds((double)random.Next(3000, 10000)));
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00006880 File Offset: 0x00004A80
		public void DoNotAcceptNewPlayers()
		{
			base.SendMessage(new StopAcceptingNewPlayersMessage());
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0000688D File Offset: 0x00004A8D
		public void OnWarmupEnded()
		{
			this._isWarmupEnded = true;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00006898 File Offset: 0x00004A98
		public void OnPlayerSpawned(PlayerId playerId)
		{
			int num;
			if (!this._playerSpawnCounts.TryGetValue(playerId, out num))
			{
				num = 0;
			}
			this._playerSpawnCounts[playerId] = num + 1;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x000068C8 File Offset: 0x00004AC8
		public BattlePeer GetPeer(string name)
		{
			return this._peers.First<BattlePeer>((BattlePeer peer) => peer.Name == name);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x000068FC File Offset: 0x00004AFC
		public BattlePeer GetPeer(PlayerId playerId)
		{
			return this._peers.FirstOrDefault<BattlePeer>((BattlePeer peer) => peer.PlayerId == playerId);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00006930 File Offset: 0x00004B30
		public Guid GetPlayerParty(PlayerId playerId)
		{
			Guid empty;
			if (!this._playerPartyMap.TryGetValue(playerId, out empty))
			{
				empty = Guid.Empty;
			}
			return empty;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00006954 File Offset: 0x00004B54
		public void HandlePlayerDisconnect(PlayerId playerId, DisconnectType disconnectType, BattleResult battleResult)
		{
			BattlePeer battlePeer = this._peers.First<BattlePeer>((BattlePeer peer) => peer.PlayerId == playerId);
			if (!battlePeer.Quit)
			{
				battlePeer.SetPlayerDisconnectdFromGameSession();
				int num;
				bool flag = !this._isWarmupEnded || this._state == BattleServer.State.Finishing || !this._playerSpawnCounts.TryGetValue(playerId, out num) || num <= 0;
				base.SendMessage(new PlayerDisconnectedMessage(playerId, disconnectType, flag, battleResult));
			}
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x000069DC File Offset: 0x00004BDC
		public async void InformGameServerReady()
		{
			BattleReadyResponseMessage battleReadyResponseMessage = await base.CallFunction<BattleReadyResponseMessage>(new BattleReadyMessage());
			this._shouldReportActivities = battleReadyResponseMessage.ShouldReportActivities;
			this._state = BattleServer.State.Running;
			this._battleBecomeReady = true;
			while (this._newPlayerRequests.Count > 0)
			{
				NewPlayerMessage newPlayerMessage = this._newPlayerRequests.Dequeue();
				this.ProcessNewPlayer(newPlayerMessage.PlayerBattleInfo, newPlayerMessage.PlayerData, newPlayerMessage.PlayerParty, newPlayerMessage.UsedCosmetics);
			}
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00006A18 File Offset: 0x00004C18
		private async void UpdateMaxAllowedPriority()
		{
			this._passedTimeSinceLastMaxAllowedPriorityRequest = 0f;
			sbyte b = await this.GetMaxAllowedPriority();
			this._maxAllowedPriority = b;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00006A54 File Offset: 0x00004C54
		public void OnFriendlyHit(int round, PlayerId hitter, PlayerId victim, float damage)
		{
			if (!this._isWarmupEnded || damage <= 0f || round < 0)
			{
				return;
			}
			Dictionary<int, ValueTuple<int, float>> dictionary;
			if (!this._playerRoundFriendlyDamageMap.TryGetValue(hitter, out dictionary))
			{
				dictionary = new Dictionary<int, ValueTuple<int, float>>();
				this._playerRoundFriendlyDamageMap.Add(hitter, dictionary);
			}
			ValueTuple<int, float> valueTuple;
			if (dictionary.TryGetValue(round, out valueTuple))
			{
				dictionary[round] = new ValueTuple<int, float>(valueTuple.Item1, valueTuple.Item2 + damage);
			}
			else
			{
				dictionary.Add(round, new ValueTuple<int, float>(0, damage));
			}
			float num = 0f;
			int num2 = 0;
			bool flag = false;
			foreach (KeyValuePair<int, ValueTuple<int, float>> keyValuePair in dictionary)
			{
				num += keyValuePair.Value.Item2;
				if (num > this._maxFriendlyDamage || keyValuePair.Value.Item2 > this._maxFriendlyDamagePerSingleRound)
				{
					flag = true;
					break;
				}
				if (keyValuePair.Value.Item2 > this._roundFriendlyDamageLimit)
				{
					num2++;
					if (num2 > this._maxRoundsOverLimitCount)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				base.SendMessage(new FriendlyDamageKickPlayerMessage(hitter, dictionary));
			}
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00006B80 File Offset: 0x00004D80
		public void OnFriendlyKill(int round, PlayerId killer, PlayerId victim)
		{
			if (!this._isWarmupEnded || round < 0)
			{
				return;
			}
			Dictionary<int, ValueTuple<int, float>> dictionary;
			if (!this._playerRoundFriendlyDamageMap.TryGetValue(killer, out dictionary))
			{
				dictionary = new Dictionary<int, ValueTuple<int, float>>();
				this._playerRoundFriendlyDamageMap.Add(killer, dictionary);
			}
			ValueTuple<int, float> valueTuple;
			if (dictionary.TryGetValue(round, out valueTuple))
			{
				dictionary[round] = new ValueTuple<int, float>(valueTuple.Item1 + 1, valueTuple.Item2);
			}
			else
			{
				dictionary.Add(round, new ValueTuple<int, float>(1, 0f));
			}
			int num = 0;
			foreach (KeyValuePair<int, ValueTuple<int, float>> keyValuePair in dictionary)
			{
				num += keyValuePair.Value.Item1;
				if (num > this._maxFriendlyKillCount)
				{
					base.SendMessage(new FriendlyDamageKickPlayerMessage(killer, dictionary));
					break;
				}
			}
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00006C5C File Offset: 0x00004E5C
		private async Task<sbyte> GetMaxAllowedPriority()
		{
			sbyte b;
			try
			{
				b = (await base.CallFunction<RequestMaxAllowedPriorityResponse>(new RequestMaxAllowedPriorityMessage())).Priority;
			}
			catch (Exception)
			{
				b = sbyte.MaxValue;
			}
			return b;
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00006CA4 File Offset: 0x00004EA4
		private void SetBattleJoinTypes(BattleResult battleResult)
		{
			foreach (BattlePlayerEntry battlePlayerEntry in battleResult.PlayerEntries.Values)
			{
				foreach (BattlePeer battlePeer in this._peers)
				{
					if (battlePeer.PlayerId == battlePlayerEntry.PlayerId)
					{
						battlePlayerEntry.BattleJoinType = battlePeer.BattleJoinType;
						break;
					}
				}
			}
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00006D54 File Offset: 0x00004F54
		public bool AllPlayersConnected()
		{
			PlayerId[] assignedPlayers = this.AssignedPlayers;
			for (int i = 0; i < assignedPlayers.Length; i++)
			{
				PlayerId playerId = assignedPlayers[i];
				if (this._peers.FirstOrDefault<BattlePeer>((BattlePeer p) => p.PlayerId == playerId) == null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040001B1 RID: 433
		private BattleServer.State _state = BattleServer.State.Connecting;

		// Token: 0x040001C1 RID: 449
		private IBattleServerSessionHandler _handler;

		// Token: 0x040001C2 RID: 450
		private List<BattlePeer> _peers;

		// Token: 0x040001C3 RID: 451
		private string _assignedAddress;

		// Token: 0x040001C4 RID: 452
		private ushort _assignedPort;

		// Token: 0x040001C5 RID: 453
		private string _region;

		// Token: 0x040001C6 RID: 454
		private sbyte _priority;

		// Token: 0x040001C7 RID: 455
		private sbyte _maxAllowedPriority;

		// Token: 0x040001C8 RID: 456
		private byte _numCores;

		// Token: 0x040001C9 RID: 457
		private string _password;

		// Token: 0x040001CA RID: 458
		private string _gameMode;

		// Token: 0x040001CB RID: 459
		private PeerId _peerId;

		// Token: 0x040001CC RID: 460
		private float _requestMaxAllowedPriorityIntervalInSeconds = 10f;

		// Token: 0x040001CD RID: 461
		private float _passedTimeSinceLastMaxAllowedPriorityRequest;

		// Token: 0x040001CE RID: 462
		private Stopwatch _timer;

		// Token: 0x040001CF RID: 463
		private long _previousTimeInMS;

		// Token: 0x040001D0 RID: 464
		private Queue<NewPlayerMessage> _newPlayerRequests;

		// Token: 0x040001D1 RID: 465
		private bool _battleBecomeReady;

		// Token: 0x040001D2 RID: 466
		private int _defaultServerTimeoutDuration = 600000;

		// Token: 0x040001D3 RID: 467
		private int _timeoutDuration;

		// Token: 0x040001D4 RID: 468
		private Stopwatch _timeoutTimer;

		// Token: 0x040001D5 RID: 469
		private DateTime? _terminationTime;

		// Token: 0x040001D6 RID: 470
		private bool _isWarmupEnded;

		// Token: 0x040001D7 RID: 471
		private Dictionary<PlayerId, int> _playerSpawnCounts;

		// Token: 0x040001D8 RID: 472
		private IBadgeComponent _badgeComponent;

		// Token: 0x040001D9 RID: 473
		private Dictionary<PlayerId, Guid> _playerPartyMap;

		// Token: 0x040001DA RID: 474
		[TupleElementNames(new string[] { "killCount", "damage" })]
		private Dictionary<PlayerId, Dictionary<int, ValueTuple<int, float>>> _playerRoundFriendlyDamageMap;

		// Token: 0x040001DB RID: 475
		private int _maxFriendlyKillCount;

		// Token: 0x040001DC RID: 476
		private float _maxFriendlyDamage;

		// Token: 0x040001DD RID: 477
		private float _maxFriendlyDamagePerSingleRound;

		// Token: 0x040001DE RID: 478
		private float _roundFriendlyDamageLimit;

		// Token: 0x040001DF RID: 479
		private int _maxRoundsOverLimitCount;

		// Token: 0x040001E0 RID: 480
		private bool _shouldReportActivities;

		// Token: 0x040001E1 RID: 481
		private const float BattleResultUpdatePeriod = 5f;

		// Token: 0x040001E2 RID: 482
		private float _battleResultUpdateTimeElapsed;

		// Token: 0x040001E3 RID: 483
		private BattleResult _latestQueuedBattleResult;

		// Token: 0x040001E4 RID: 484
		private Dictionary<int, int> _latestQueuedTeamScores;

		// Token: 0x02000183 RID: 387
		private enum State
		{
			// Token: 0x04000539 RID: 1337
			Idle,
			// Token: 0x0400053A RID: 1338
			Connecting,
			// Token: 0x0400053B RID: 1339
			Connected,
			// Token: 0x0400053C RID: 1340
			LoggingIn,
			// Token: 0x0400053D RID: 1341
			WaitingBattle,
			// Token: 0x0400053E RID: 1342
			BattleAssigned,
			// Token: 0x0400053F RID: 1343
			Running,
			// Token: 0x04000540 RID: 1344
			Finishing,
			// Token: 0x04000541 RID: 1345
			Finished
		}
	}
}
