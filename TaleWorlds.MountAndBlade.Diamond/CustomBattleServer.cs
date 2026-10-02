using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Messages.FromCustomBattleServer.ToCustomBattleServerManager;
using Messages.FromCustomBattleServerManager.ToCustomBattleServer;
using TaleWorlds.Diamond;
using TaleWorlds.Diamond.ClientApplication;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000112 RID: 274
	public class CustomBattleServer : Client<CustomBattleServer>
	{
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060005E8 RID: 1512 RVA: 0x00007542 File Offset: 0x00005742
		public bool Finished
		{
			get
			{
				return this._state == CustomBattleServer.State.Finished;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x0000754D File Offset: 0x0000574D
		public bool IsRegistered
		{
			get
			{
				return this._state == CustomBattleServer.State.RegisteredGame || this._state == CustomBattleServer.State.RegisteredServer;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x00007563 File Offset: 0x00005763
		public bool IsPlaying
		{
			get
			{
				return this._state == CustomBattleServer.State.RegisteredGame;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x0000756E File Offset: 0x0000576E
		public bool Connected
		{
			get
			{
				return this.CurrentState != CustomBattleServer.State.Working && this.CurrentState > CustomBattleServer.State.Idle;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x00007584 File Offset: 0x00005784
		// (set) Token: 0x060005ED RID: 1517 RVA: 0x0000758C File Offset: 0x0000578C
		public CustomBattleServer.State CurrentState
		{
			get
			{
				return this._state;
			}
			private set
			{
				if (this._state != value)
				{
					CustomBattleServer.State state = this._state;
					this._state = value;
					if (this._handler != null)
					{
						this._handler.OnStateChanged(state);
					}
				}
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x000075C4 File Offset: 0x000057C4
		public bool IsIdle
		{
			get
			{
				return this._state == CustomBattleServer.State.RegisteredGame && this._customBattlePlayers.Count == 0 && this._useTimeoutTimer && this._timeoutTimer.ElapsedMilliseconds > (long)this._timeoutDuration;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x000075FC File Offset: 0x000057FC
		// (set) Token: 0x060005F0 RID: 1520 RVA: 0x00007604 File Offset: 0x00005804
		public string CustomGameType { get; private set; }

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x0000760D File Offset: 0x0000580D
		// (set) Token: 0x060005F2 RID: 1522 RVA: 0x00007615 File Offset: 0x00005815
		public string CustomGameScene { get; private set; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x0000761E File Offset: 0x0000581E
		// (set) Token: 0x060005F4 RID: 1524 RVA: 0x00007626 File Offset: 0x00005826
		public int Port { get; private set; }

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x0000762F File Offset: 0x0000582F
		// (set) Token: 0x060005F6 RID: 1526 RVA: 0x00007637 File Offset: 0x00005837
		public MultipleBattleResult BattleResult { get; private set; }

		// Token: 0x060005F7 RID: 1527 RVA: 0x00007640 File Offset: 0x00005840
		public CustomBattleServer(DiamondClientApplication diamondClientApplication, IClientSessionProvider<CustomBattleServer> provider)
			: base(diamondClientApplication, provider, false)
		{
			this._peerId = new PeerId(Guid.NewGuid());
			this._customBattlePlayers = new List<PlayerId>();
			this._requestedPlayers = new List<PlayerId>();
			this._timeoutTimer = new Stopwatch();
			this._terminationTime = null;
			this._state = CustomBattleServer.State.Idle;
			this._timer = new Stopwatch();
			this._timer.Start();
			if (!base.Application.Parameters.TryGetParameterAsInt("CustomBattleServer.TimeoutDuration", out this._timeoutDuration))
			{
				this._timeoutDuration = this._defaultServerTimeoutDuration;
			}
			this._badgeComponent = null;
			this._badgeComponentPlayers = new List<PlayerData>();
			this.BattleResult = new MultipleBattleResult();
			this._pendingDisconnects = new List<PlayerDisconnectData>();
			this._pendingJoinResponses = new List<PlayerJoinGameResponseDataFromHost>();
			base.AddMessageHandler<ClientWantsToConnectCustomGameMessage>(new ClientMessageHandler<ClientWantsToConnectCustomGameMessage>(this.OnClientWantsToConnectCustomGameMessage));
			base.AddMessageHandler<ClientQuitFromCustomGameMessage>(new ClientMessageHandler<ClientQuitFromCustomGameMessage>(this.OnClientQuitFromCustomGameMessage));
			base.AddMessageHandler<TerminateOperationCustomMessage>(new ClientMessageHandler<TerminateOperationCustomMessage>(this.OnTerminateOperationCustomMessage));
			base.AddMessageHandler<SetChatFilterListsMessage>(new ClientMessageHandler<SetChatFilterListsMessage>(this.OnSetChatFilterListsMessage));
			base.AddMessageHandler<PlayerDisconnectedFromLobbyMessage>(new ClientMessageHandler<PlayerDisconnectedFromLobbyMessage>(this.OnPlayerDisconnectedFromLobbyMessage));
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00007774 File Offset: 0x00005974
		public void SetBadgeComponent(IBadgeComponent badgeComponent)
		{
			this._badgeComponent = badgeComponent;
			if (this._badgeComponent != null)
			{
				foreach (PlayerData playerData in this._badgeComponentPlayers)
				{
					this._badgeComponent.OnPlayerJoin(playerData);
				}
			}
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x000077DC File Offset: 0x000059DC
		public void Connect(ICustomBattleServerSessionHandler handler, string authToken, bool isSinglePlatformServer, string[] loadedModuleIDs, bool allowsOptionalModules, bool isPlayerHosted)
		{
			this._handler = handler;
			this._authToken = authToken;
			this._allowsOptionalModules = allowsOptionalModules;
			this._useTimeoutTimer = !isPlayerHosted;
			this._isSinglePlatformServer = isSinglePlatformServer;
			this._loadedModules = new List<ModuleInfoModel>();
			foreach (ModuleInfo moduleInfo in ModuleHelper.GetSortedModules(loadedModuleIDs))
			{
				if (!allowsOptionalModules && moduleInfo.Category == ModuleCategory.MultiplayerOptional)
				{
					throw new InvalidOperationException("Optional modules are explicitly disallowed, yet an optional module (" + moduleInfo.Id + ") was loaded! You must use category 'Server' instead of 'MultiplayerOptional'.");
				}
				ModuleInfoModel moduleInfoModel;
				if (ModuleInfoModel.TryCreateForSession(moduleInfo, out moduleInfoModel))
				{
					this._loadedModules.Add(moduleInfoModel);
				}
			}
			this.CurrentState = CustomBattleServer.State.Working;
			base.BeginConnect();
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x000078A8 File Offset: 0x00005AA8
		public override void OnConnected()
		{
			base.OnConnected();
			this.CurrentState = CustomBattleServer.State.Connected;
			if (this._handler != null)
			{
				this._handler.OnConnected();
			}
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x000078CA File Offset: 0x00005ACA
		public override void OnCantConnect()
		{
			base.OnCantConnect();
			this.CurrentState = CustomBattleServer.State.Idle;
			if (this._handler != null)
			{
				this._handler.OnCantConnect();
			}
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x000078EC File Offset: 0x00005AEC
		public override void OnDisconnected()
		{
			base.OnDisconnected();
			this.CurrentState = CustomBattleServer.State.Idle;
			if (this._handler != null)
			{
				this._handler.OnDisconnected();
			}
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00007910 File Offset: 0x00005B10
		protected override void OnTick()
		{
			if (this._terminationTime != null && this._terminationTime < DateTime.UtcNow)
			{
				throw new Exception("Now I am become Death, the destroyer of worlds");
			}
			long elapsedMilliseconds = this._timer.ElapsedMilliseconds;
			float num = (float)(elapsedMilliseconds - this._previousTimeInMS);
			this._previousTimeInMS = elapsedMilliseconds;
			float num2 = num / 1000f;
			this._battleResultUpdateTimeElapsed += num2;
			if (this._battleResultUpdateTimeElapsed >= 5f)
			{
				if (this._latestQueuedBattleResult != null && this._latestQueuedTeamScores != null && this._latestQueuedPlayerScores != null)
				{
					base.SendMessage(new CustomBattleServerStatsUpdateMessage(this._latestQueuedBattleResult, this._latestQueuedTeamScores, this._latestQueuedPlayerScores));
					this._latestQueuedBattleResult = null;
					this._latestQueuedTeamScores = null;
					this._latestQueuedPlayerScores = null;
				}
				this._battleResultUpdateTimeElapsed = 0f;
			}
			this._disconnectBatchTimeElapsed += num2;
			if (this._disconnectBatchTimeElapsed >= 3f)
			{
				this.FlushDisconnectBatch();
				this._disconnectBatchTimeElapsed = 0f;
			}
			this._joinResponseBatchTimeElapsed += num2;
			if (this._joinResponseBatchTimeElapsed >= 3f)
			{
				this.FlushJoinResponseBatch();
				this._joinResponseBatchTimeElapsed = 0f;
			}
			CustomBattleServer.State state = this._state;
			if (state == CustomBattleServer.State.Connected)
			{
				this.DoLogin();
			}
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00007A5C File Offset: 0x00005C5C
		private async void DoLogin()
		{
			this._state = CustomBattleServer.State.SessionRequested;
			LoginResult loginResult = await base.Login(new CustomBattleServerReadyMessage(this._peerId, base.ApplicationVersion, this._authToken, this._loadedModules.ToArray(), this._allowsOptionalModules));
			if (loginResult != null && loginResult.Successful)
			{
				this._state = CustomBattleServer.State.RegisteredServer;
			}
			else
			{
				Console.WriteLine("Login Failed! Server is shutting down.");
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00007A95 File Offset: 0x00005C95
		private void OnClientWantsToConnectCustomGameMessage(ClientWantsToConnectCustomGameMessage message)
		{
			this.HandleOnClientWantsToConnectCustomGameMessage(message);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00007AA0 File Offset: 0x00005CA0
		private async void HandleOnClientWantsToConnectCustomGameMessage(ClientWantsToConnectCustomGameMessage message)
		{
			if (this.CurrentState == CustomBattleServer.State.Finished)
			{
				PlayerJoinGameData[] playerJoinGameData = message.PlayerJoinGameData;
				List<PlayerJoinGameResponseDataFromHost> list = new List<PlayerJoinGameResponseDataFromHost>();
				foreach (PlayerJoinGameData playerJoinGameData2 in playerJoinGameData)
				{
					list.Add(new PlayerJoinGameResponseDataFromHost
					{
						PlayerId = playerJoinGameData2.PlayerId,
						PeerIndex = -1,
						SessionKey = -1,
						CustomGameJoinResponse = CustomGameJoinResponse.CustomGameServerFinishing
					});
				}
				this.ResponseCustomGameClientConnection(list.ToArray());
			}
			else
			{
				PlayerJoinGameData[] requestedPlayers = message.PlayerJoinGameData;
				for (int k = 0; k < requestedPlayers.Length; k++)
				{
					if (requestedPlayers[k] != null)
					{
						PlayerJoinGameData playerJoinGameData3 = requestedPlayers[k];
						Debug.Print(string.Concat(new object[] { "Player ", playerJoinGameData3.Name, " - ", playerJoinGameData3.PlayerId, " with IP address ", playerJoinGameData3.IpAddress, " wants to join the game" }), 0, Debug.DebugColor.White, 17592186044416UL);
					}
				}
				int j;
				for (int i = 0; i < requestedPlayers.Length; i = j + 1)
				{
					if (requestedPlayers[i] != null)
					{
						List<PlayerJoinGameData> requestedGroup = new List<PlayerJoinGameData>();
						PlayerJoinGameData playerJoinGameData4 = requestedPlayers[i];
						Guid? guid = playerJoinGameData4.PartyId;
						if (guid == null)
						{
							requestedGroup.Add(playerJoinGameData4);
						}
						else
						{
							for (int l = i; l < requestedPlayers.Length; l++)
							{
								PlayerJoinGameData playerJoinGameData5 = requestedPlayers[l];
								guid = playerJoinGameData4.PartyId;
								if (guid.Equals((playerJoinGameData5 != null) ? playerJoinGameData5.PartyId : null))
								{
									requestedGroup.Add(playerJoinGameData5);
									requestedPlayers[l] = null;
								}
							}
						}
						bool flag = true;
						foreach (PlayerJoinGameData playerJoinGameData6 in requestedGroup)
						{
							if (this._requestedPlayers.Contains(playerJoinGameData6.PlayerId) || this._customBattlePlayers.Contains(playerJoinGameData6.PlayerId))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							this._timeoutTimer.Restart();
							foreach (PlayerJoinGameData playerJoinGameData7 in requestedGroup)
							{
								this._requestedPlayers.Add(playerJoinGameData7.PlayerId);
							}
							if (this._handler != null)
							{
								PlayerJoinGameResponseDataFromHost[] array2 = await this._handler.OnClientWantsToConnectCustomGame(requestedGroup.ToArray());
								if (this._badgeComponent != null)
								{
									foreach (PlayerJoinGameResponseDataFromHost playerJoinGameResponseDataFromHost in array2)
									{
										if (playerJoinGameResponseDataFromHost.CustomGameJoinResponse == CustomGameJoinResponse.Success)
										{
											foreach (PlayerJoinGameData playerJoinGameData8 in requestedGroup)
											{
												if (playerJoinGameData8.PlayerId.Equals(playerJoinGameResponseDataFromHost.PlayerId))
												{
													this._badgeComponent.OnPlayerJoin(playerJoinGameData8.PlayerData);
													this._badgeComponentPlayers.Add(playerJoinGameData8.PlayerData);
												}
											}
										}
									}
								}
								this._pendingJoinResponses.AddRange(array2);
							}
						}
						else
						{
							foreach (PlayerJoinGameData playerJoinGameData9 in requestedGroup)
							{
								this._pendingJoinResponses.Add(new PlayerJoinGameResponseDataFromHost
								{
									PlayerId = playerJoinGameData9.PlayerId,
									PeerIndex = -1,
									SessionKey = -1,
									CustomGameJoinResponse = CustomGameJoinResponse.NotAllPlayersReady
								});
							}
						}
						requestedGroup = null;
					}
					j = i;
				}
				requestedPlayers = null;
			}
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00007AE4 File Offset: 0x00005CE4
		private void OnClientQuitFromCustomGameMessage(ClientQuitFromCustomGameMessage message)
		{
			if (this.CurrentState == CustomBattleServer.State.RegisteredGame && this._customBattlePlayers.Contains(message.PlayerId))
			{
				if (this._handler != null)
				{
					this._handler.OnClientQuitFromCustomGame(message.PlayerId);
				}
				this._customBattlePlayers.Remove(message.PlayerId);
			}
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00007B38 File Offset: 0x00005D38
		public void OnPlayerDisconnectedFromLobbyMessage(PlayerDisconnectedFromLobbyMessage message)
		{
			this.HandlePlayerDisconnect(message.PlayerId, DisconnectType.DisconnectedFromLobby);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00007B48 File Offset: 0x00005D48
		private void OnTerminateOperationCustomMessage(TerminateOperationCustomMessage message)
		{
			Random random = new Random();
			this._terminationTime = new DateTime?(DateTime.UtcNow.AddMilliseconds((double)random.Next(3000, 10000)));
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00007B84 File Offset: 0x00005D84
		private void OnSetChatFilterListsMessage(SetChatFilterListsMessage message)
		{
			if (this._handler != null)
			{
				this._handler.OnChatFilterListsReceived(message.ProfanityList, message.AllowList);
			}
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00007BA8 File Offset: 0x00005DA8
		public void ResponseCustomGameClientConnection(PlayerJoinGameResponseDataFromHost[] playerJoinData)
		{
			if (this.CurrentState == CustomBattleServer.State.RegisteredGame)
			{
				foreach (PlayerJoinGameResponseDataFromHost playerJoinGameResponseDataFromHost in playerJoinData)
				{
					this._requestedPlayers.Remove(playerJoinGameResponseDataFromHost.PlayerId);
					if (playerJoinGameResponseDataFromHost.CustomGameJoinResponse == CustomGameJoinResponse.Success)
					{
						this._customBattlePlayers.Add(playerJoinGameResponseDataFromHost.PlayerId);
					}
				}
				base.SendMessage(new ResponseCustomGameClientConnectionMessage(playerJoinData));
			}
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00007C0C File Offset: 0x00005E0C
		public async Task RegisterGame(string gameModule, string gameType, string serverName, int maxPlayerCount, string scene, string uniqueSceneId, int port, string region, string gamePassword, string adminPassword, int permission)
		{
			await this.RegisterGame(0, gameModule, gameType, serverName, maxPlayerCount, scene, uniqueSceneId, port, region, gamePassword, adminPassword, permission, string.Empty);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00007CB4 File Offset: 0x00005EB4
		public async Task RegisterGame(int gameDefinitionId, string gameModule, string gameType, string serverName, int maxPlayerCount, string scene, string uniqueSceneId, int port, string region, string gamePassword, string adminPassword, int permission, string overriddenIP)
		{
			this.Port = port;
			this.CustomGameType = gameType;
			this.CustomGameScene = scene;
			string text = null;
			bool flag = false;
			if (base.Application.Parameters.TryGetParameter("CustomBattleServer.Host.Address", out text))
			{
				flag = true;
			}
			if (overriddenIP != string.Empty)
			{
				flag = true;
				text = overriddenIP;
			}
			RegisterCustomGameMessageResponseMessage registerCustomGameMessageResponseMessage = await base.CallFunction<RegisterCustomGameMessageResponseMessage>(new RegisterCustomGameMessage(gameDefinitionId, gameModule, gameType, serverName, text, maxPlayerCount, scene, uniqueSceneId, gamePassword, adminPassword, port, region, permission, !this._isSinglePlatformServer, flag));
			this._shouldReportActivities = registerCustomGameMessageResponseMessage.ShouldReportActivities;
			this.CurrentState = CustomBattleServer.State.RegisteredGame;
			this._timeoutTimer.Start();
			if (this._handler != null)
			{
				this._handler.OnSuccessfulGameRegister();
			}
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00007D6B File Offset: 0x00005F6B
		public void UpdateCustomGameData(string newGameType, string newMap, int newCount)
		{
			base.SendMessage(new UpdateCustomGameData(newGameType, newMap, newCount));
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00007D7B File Offset: 0x00005F7B
		public void KickPlayer(PlayerId id, bool banPlayer)
		{
			ICustomBattleServerSessionHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnPlayerKickRequested(id, banPlayer);
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00007D8F File Offset: 0x00005F8F
		public void HandlePlayerDisconnect(PlayerId playerId, DisconnectType disconnectType)
		{
			this._timeoutTimer.Restart();
			this._customBattlePlayers.Remove(playerId);
			this._pendingDisconnects.Add(new PlayerDisconnectData(playerId, disconnectType));
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00007DBB File Offset: 0x00005FBB
		private void FlushDisconnectBatch()
		{
			if (this._pendingDisconnects.Count == 0)
			{
				return;
			}
			base.SendMessage(new PlayersDisconnectedMessage(this._pendingDisconnects.ToArray()));
			this._pendingDisconnects.Clear();
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00007DEC File Offset: 0x00005FEC
		private void FlushJoinResponseBatch()
		{
			if (this._pendingJoinResponses.Count == 0)
			{
				return;
			}
			this.ResponseCustomGameClientConnection(this._pendingJoinResponses.ToArray());
			this._pendingJoinResponses.Clear();
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00007E18 File Offset: 0x00006018
		public void FinishAsIdle(GameLog[] gameLogs)
		{
			this.FinishGame(gameLogs);
			base.BeginDisconnect();
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00007E28 File Offset: 0x00006028
		public void FinishGame(GameLog[] gameLogs)
		{
			this.CurrentState = CustomBattleServer.State.Finished;
			if (this._handler != null)
			{
				this._handler.OnGameFinished();
			}
			this.FlushJoinResponseBatch();
			this.FlushDisconnectBatch();
			IBadgeComponent badgeComponent = this._badgeComponent;
			base.SendMessage(new CustomBattleServerFinishingMessage(gameLogs, (badgeComponent != null) ? badgeComponent.DataDictionary : null, this.BattleResult));
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00007E7F File Offset: 0x0000607F
		public void UpdateGameProperties(string gameType, string scene, string uniqueSceneId)
		{
			this.CustomGameType = gameType;
			this.CustomGameScene = scene;
			base.SendMessage(new UpdateGamePropertiesMessage(gameType, scene, uniqueSceneId));
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00007E9D File Offset: 0x0000609D
		public void BeforeStartingNextBattle(GameLog[] gameLogs)
		{
			IBadgeComponent badgeComponent = this._badgeComponent;
			if (badgeComponent != null)
			{
				badgeComponent.OnStartingNextBattle();
			}
			if (gameLogs != null && gameLogs.Length != 0)
			{
				base.SendMessage(new AddGameLogsMessage(gameLogs));
			}
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00007EC3 File Offset: 0x000060C3
		public void BattleStarted(Dictionary<PlayerId, int> playerTeams, string cultureTeam1, string cultureTeam2)
		{
			if (this._shouldReportActivities)
			{
				base.SendMessage(new CustomBattleStartedMessage(this.CustomGameType, playerTeams, new List<string> { cultureTeam2, cultureTeam1 }));
			}
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00007EF2 File Offset: 0x000060F2
		public void BattleFinished(BattleResult battleResult, Dictionary<int, int> teamScores, Dictionary<PlayerId, int> playerScores)
		{
			if (this._shouldReportActivities)
			{
				base.SendMessage(new CustomBattleFinishedMessage(battleResult, teamScores, playerScores));
			}
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00007F0A File Offset: 0x0000610A
		public void UpdateBattleStats(BattleResult battleResult, Dictionary<int, int> teamScores, Dictionary<PlayerId, int> playerScores)
		{
			if (this._shouldReportActivities)
			{
				this._latestQueuedBattleResult = battleResult;
				this._latestQueuedTeamScores = teamScores;
				this._latestQueuedPlayerScores = playerScores;
			}
		}

		// Token: 0x04000239 RID: 569
		private CustomBattleServer.State _state;

		// Token: 0x0400023A RID: 570
		private string _authToken;

		// Token: 0x0400023B RID: 571
		private List<ModuleInfoModel> _loadedModules;

		// Token: 0x0400023C RID: 572
		private bool _allowsOptionalModules;

		// Token: 0x0400023D RID: 573
		private bool _isSinglePlatformServer;

		// Token: 0x0400023E RID: 574
		private Stopwatch _timer;

		// Token: 0x0400023F RID: 575
		private long _previousTimeInMS;

		// Token: 0x04000244 RID: 580
		private ICustomBattleServerSessionHandler _handler;

		// Token: 0x04000245 RID: 581
		private PeerId _peerId;

		// Token: 0x04000246 RID: 582
		private List<PlayerId> _customBattlePlayers;

		// Token: 0x04000247 RID: 583
		private List<PlayerId> _requestedPlayers;

		// Token: 0x04000248 RID: 584
		private int _defaultServerTimeoutDuration = 600000;

		// Token: 0x04000249 RID: 585
		private int _timeoutDuration;

		// Token: 0x0400024A RID: 586
		private Stopwatch _timeoutTimer;

		// Token: 0x0400024B RID: 587
		private DateTime? _terminationTime;

		// Token: 0x0400024C RID: 588
		private bool _useTimeoutTimer;

		// Token: 0x0400024D RID: 589
		private IBadgeComponent _badgeComponent;

		// Token: 0x0400024E RID: 590
		private readonly List<PlayerData> _badgeComponentPlayers;

		// Token: 0x0400024F RID: 591
		private bool _shouldReportActivities;

		// Token: 0x04000250 RID: 592
		private const float BattleResultUpdatePeriod = 5f;

		// Token: 0x04000251 RID: 593
		private float _battleResultUpdateTimeElapsed;

		// Token: 0x04000252 RID: 594
		private BattleResult _latestQueuedBattleResult;

		// Token: 0x04000253 RID: 595
		private Dictionary<int, int> _latestQueuedTeamScores;

		// Token: 0x04000254 RID: 596
		private Dictionary<PlayerId, int> _latestQueuedPlayerScores;

		// Token: 0x04000255 RID: 597
		private const float DisconnectBatchFlushPeriod = 3f;

		// Token: 0x04000256 RID: 598
		private float _disconnectBatchTimeElapsed;

		// Token: 0x04000257 RID: 599
		private List<PlayerDisconnectData> _pendingDisconnects;

		// Token: 0x04000258 RID: 600
		private const float JoinResponseBatchFlushPeriod = 3f;

		// Token: 0x04000259 RID: 601
		private float _joinResponseBatchTimeElapsed;

		// Token: 0x0400025A RID: 602
		private List<PlayerJoinGameResponseDataFromHost> _pendingJoinResponses;

		// Token: 0x0200018F RID: 399
		public enum State
		{
			// Token: 0x0400055A RID: 1370
			Idle,
			// Token: 0x0400055B RID: 1371
			Working,
			// Token: 0x0400055C RID: 1372
			Connected,
			// Token: 0x0400055D RID: 1373
			SessionRequested,
			// Token: 0x0400055E RID: 1374
			RegisteredServer,
			// Token: 0x0400055F RID: 1375
			RegisteredGame,
			// Token: 0x04000560 RID: 1376
			Finished
		}
	}
}
