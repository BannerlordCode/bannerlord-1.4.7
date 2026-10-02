using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002A9 RID: 681
	public abstract class MissionLobbyComponent : MissionNetwork
	{
		// Token: 0x14000046 RID: 70
		// (add) Token: 0x0600256C RID: 9580 RVA: 0x000874C0 File Offset: 0x000856C0
		// (remove) Token: 0x0600256D RID: 9581 RVA: 0x000874F8 File Offset: 0x000856F8
		public event Action OnPostMatchEnded;

		// Token: 0x14000047 RID: 71
		// (add) Token: 0x0600256E RID: 9582 RVA: 0x00087530 File Offset: 0x00085730
		// (remove) Token: 0x0600256F RID: 9583 RVA: 0x00087568 File Offset: 0x00085768
		public event Action OnCultureSelectionRequested;

		// Token: 0x14000048 RID: 72
		// (add) Token: 0x06002570 RID: 9584 RVA: 0x000875A0 File Offset: 0x000857A0
		// (remove) Token: 0x06002571 RID: 9585 RVA: 0x000875D8 File Offset: 0x000857D8
		public event Action<string, bool> OnAdminMessageRequested;

		// Token: 0x14000049 RID: 73
		// (add) Token: 0x06002572 RID: 9586 RVA: 0x00087610 File Offset: 0x00085810
		// (remove) Token: 0x06002573 RID: 9587 RVA: 0x00087648 File Offset: 0x00085848
		public event Action OnClassRestrictionChanged;

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06002574 RID: 9588 RVA: 0x0008767D File Offset: 0x0008587D
		public bool IsInWarmup
		{
			get
			{
				return this._warmupComponent != null && this._warmupComponent.IsInWarmup;
			}
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x00087694 File Offset: 0x00085894
		static MissionLobbyComponent()
		{
			MissionLobbyComponent.AddLobbyComponentType(typeof(MissionBattleSchedulerClientComponent), LobbyMissionType.Matchmaker, false);
			MissionLobbyComponent.AddLobbyComponentType(typeof(MissionCustomGameClientComponent), LobbyMissionType.Custom, false);
			MissionLobbyComponent.AddLobbyComponentType(typeof(MissionCommunityClientComponent), LobbyMissionType.Community, false);
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x000876F2 File Offset: 0x000858F2
		public static void AddLobbyComponentType(Type type, LobbyMissionType missionType, bool isSeverComponent)
		{
			MissionLobbyComponent._lobbyComponentTypes.Add(new Tuple<LobbyMissionType, bool>(missionType, isSeverComponent), type);
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x00087708 File Offset: 0x00085908
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this.CurrentMultiplayerState = MissionLobbyComponent.MultiplayerGameState.WaitingFirstPlayers;
			if (GameNetwork.IsServerOrRecorder)
			{
				MissionMultiplayerGameModeBase missionBehavior = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBase>();
				if (missionBehavior != null && !missionBehavior.AllowCustomPlayerBanners())
				{
					this._usingFixedBanners = true;
					return;
				}
			}
			else
			{
				this._inactivityTimer = new Timer(base.Mission.CurrentTime, MissionLobbyComponent.InactivityThreshold, true);
			}
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x00087764 File Offset: 0x00085964
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClient)
			{
				registerer.RegisterBaseHandler<KillDeathCountChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventKillDeathCountChangeEvent));
				registerer.RegisterBaseHandler<MissionStateChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventMissionStateChange));
				registerer.RegisterBaseHandler<NetworkMessages.FromServer.CreateBanner>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventCreateBannerForPeer));
				registerer.RegisterBaseHandler<ChangeCulture>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventChangeCulture));
				registerer.RegisterBaseHandler<ChangeClassRestrictions>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventChangeClassRestrictions));
				return;
			}
			if (GameNetwork.IsClientOrReplay)
			{
				registerer.RegisterBaseHandler<ChangeCulture>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventChangeCulture));
				return;
			}
			if (GameNetwork.IsServer)
			{
				registerer.RegisterBaseHandler<NetworkMessages.FromClient.CreateBanner>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventCreateBannerForPeer));
				registerer.RegisterBaseHandler<RequestCultureChange>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventRequestCultureChange));
				registerer.RegisterBaseHandler<RequestChangeCharacterMessage>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventRequestChangeCharacterMessage));
			}
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x0008782A File Offset: 0x00085A2A
		protected override void OnUdpNetworkHandlerClose()
		{
			if (GameNetwork.IsServerOrRecorder || this._usingFixedBanners)
			{
				this._usingFixedBanners = false;
			}
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x00087842 File Offset: 0x00085A42
		public static MissionLobbyComponent CreateBehavior()
		{
			return (MissionLobbyComponent)Activator.CreateInstance(MissionLobbyComponent._lobbyComponentTypes[new Tuple<LobbyMissionType, bool>(BannerlordNetwork.LobbyMissionType, GameNetwork.IsDedicatedServer)]);
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x00087867 File Offset: 0x00085A67
		public virtual void QuitMission()
		{
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x0008786C File Offset: 0x00085A6C
		public override void AfterStart()
		{
			base.Mission.DeploymentPlan.MakeDefaultDeploymentPlans();
			this._missionScoreboardComponent = base.Mission.GetMissionBehavior<MissionScoreboardComponent>();
			this._gameMode = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBase>();
			this._timerComponent = base.Mission.GetMissionBehavior<MultiplayerTimerComponent>();
			this._roundComponent = base.Mission.GetMissionBehavior<IRoundComponent>();
			this._warmupComponent = base.Mission.GetMissionBehavior<MultiplayerWarmupComponent>();
			if (GameNetwork.IsClient)
			{
				base.Mission.GetMissionBehavior<MissionNetworkComponent>().OnMyClientSynchronized += this.OnMyClientSynchronized;
			}
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x00087904 File Offset: 0x00085B04
		private void OnMyClientSynchronized()
		{
			base.Mission.GetMissionBehavior<MissionNetworkComponent>().OnMyClientSynchronized -= this.OnMyClientSynchronized;
			MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
			if (component != null && component.Culture == null)
			{
				this.RequestCultureSelection();
			}
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x0008794C File Offset: 0x00085B4C
		public override void EarlyStart()
		{
			if (GameNetwork.IsServer)
			{
				base.Mission.SpectatorTeam = base.Mission.Teams.Add(BattleSideEnum.None, uint.MaxValue, uint.MaxValue, null, true, false, true);
			}
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x00087984 File Offset: 0x00085B84
		public override void OnMissionTick(float dt)
		{
			if (GameNetwork.IsClient && this._inactivityTimer.Check(base.Mission.CurrentTime))
			{
				NetworkMain.GameClient.IsInCriticalState = MBAPI.IMBNetwork.ElapsedTimeSinceLastUdpPacketArrived() > (double)MissionLobbyComponent.InactivityThreshold;
			}
			if (this.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.WaitingFirstPlayers)
			{
				if (GameNetwork.IsServer && (this._warmupComponent == null || (!this._warmupComponent.IsInWarmup && this._timerComponent.CheckIfTimerPassed())))
				{
					int num = GameNetwork.NetworkPeers.Count<NetworkCommunicator>((NetworkCommunicator x) => x.IsSynchronized);
					int num2 = MultiplayerOptions.OptionType.NumberOfBotsTeam1.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) + MultiplayerOptions.OptionType.NumberOfBotsTeam2.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
					int intValue = MultiplayerOptions.OptionType.MinNumberOfPlayersForMatchStart.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
					if (num + num2 >= intValue || MBCommon.CurrentGameType == MBCommon.GameType.MultiClientServer)
					{
						this.SetStatePlayingAsServer();
						return;
					}
				}
			}
			else if (this.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.Playing)
			{
				bool flag = this._timerComponent.CheckIfTimerPassed();
				if (GameNetwork.IsServerOrRecorder && this._gameMode.RoundController == null && (flag || this._gameMode.CheckForMatchEnd()))
				{
					this._gameMode.GetWinnerTeam();
					this._gameMode.SpawnComponent.SpawningBehavior.RequestStopSpawnSession();
					this._gameMode.SpawnComponent.SpawningBehavior.SetRemainingAgentsInvulnerable();
					this.SetStateEndingAsServer();
				}
			}
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x00087AD6 File Offset: 0x00085CD6
		protected override void OnUdpNetworkHandlerTick()
		{
			if (this.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.Ending && this._timerComponent.CheckIfTimerPassed() && GameNetwork.IsServer)
			{
				this.EndGameAsServer();
			}
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x00087AFB File Offset: 0x00085CFB
		public override void OnRemoveBehavior()
		{
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			this.QuitMission();
			base.OnRemoveBehavior();
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x00087B0F File Offset: 0x00085D0F
		public bool IsClassAvailable(FormationClass formationClass)
		{
			return !this._classRestrictions[(int)formationClass];
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x00087B1C File Offset: 0x00085D1C
		public void ChangeClassRestriction(FormationClass classToChangeRestriction, bool value)
		{
			this._classRestrictions[(int)classToChangeRestriction] = value;
			Action onClassRestrictionChanged = this.OnClassRestrictionChanged;
			if (onClassRestrictionChanged == null)
			{
				return;
			}
			onClassRestrictionChanged();
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x00087B38 File Offset: 0x00085D38
		private void HandleServerEventMissionStateChange(GameNetworkMessage baseMessage)
		{
			MissionStateChange missionStateChange = (MissionStateChange)baseMessage;
			this.CurrentMultiplayerState = missionStateChange.CurrentState;
			if (this.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.WaitingFirstPlayers)
			{
				if (this.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.Playing && this._warmupComponent != null)
				{
					base.Mission.RemoveMissionBehavior(this._warmupComponent);
					this._warmupComponent = null;
				}
				float num = ((this.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.Playing) ? ((float)(MultiplayerOptions.OptionType.MapTimeLimit.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) * 60)) : MissionLobbyComponent.PostMatchWaitDuration);
				this._timerComponent.StartTimerAsClient(missionStateChange.StateStartTimeInSeconds, num);
			}
			if (this.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.Ending)
			{
				this.SetStateEndingAsClient();
			}
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x00087BC8 File Offset: 0x00085DC8
		private void HandleServerEventKillDeathCountChangeEvent(GameNetworkMessage baseMessage)
		{
			KillDeathCountChange killDeathCountChange = (KillDeathCountChange)baseMessage;
			if (killDeathCountChange.VictimPeer != null)
			{
				MissionPeer component = killDeathCountChange.VictimPeer.GetComponent<MissionPeer>();
				NetworkCommunicator attackerPeer = killDeathCountChange.AttackerPeer;
				MissionPeer missionPeer = ((attackerPeer != null) ? attackerPeer.GetComponent<MissionPeer>() : null);
				if (component != null)
				{
					component.KillCount = killDeathCountChange.KillCount;
					component.AssistCount = killDeathCountChange.AssistCount;
					component.DeathCount = killDeathCountChange.DeathCount;
					component.Score = killDeathCountChange.Score;
					if (missionPeer != null)
					{
						missionPeer.OnKillAnotherPeer(component);
					}
					if (killDeathCountChange.KillCount == 0 && killDeathCountChange.AssistCount == 0 && killDeathCountChange.DeathCount == 0 && killDeathCountChange.Score == 0)
					{
						component.ResetKillRegistry();
					}
				}
				if (this._missionScoreboardComponent != null)
				{
					this._missionScoreboardComponent.PlayerPropertiesChanged(killDeathCountChange.VictimPeer);
				}
			}
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x00087C84 File Offset: 0x00085E84
		private void HandleServerEventCreateBannerForPeer(GameNetworkMessage baseMessage)
		{
			NetworkMessages.FromServer.CreateBanner createBanner = (NetworkMessages.FromServer.CreateBanner)baseMessage;
			MissionPeer component = createBanner.Peer.GetComponent<MissionPeer>();
			if (component != null)
			{
				component.Peer.BannerCode = createBanner.BannerCode;
			}
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x00087CB8 File Offset: 0x00085EB8
		private void HandleServerEventChangeCulture(GameNetworkMessage baseMessage)
		{
			ChangeCulture changeCulture = (ChangeCulture)baseMessage;
			MissionPeer component = changeCulture.Peer.GetComponent<MissionPeer>();
			if (component != null)
			{
				component.Culture = changeCulture.Culture;
			}
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x00087CE8 File Offset: 0x00085EE8
		private void HandleServerEventChangeClassRestrictions(GameNetworkMessage baseMessage)
		{
			ChangeClassRestrictions changeClassRestrictions = (ChangeClassRestrictions)baseMessage;
			this.ChangeClassRestriction(changeClassRestrictions.ClassToChangeRestriction, changeClassRestrictions.NewValue);
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x00087D10 File Offset: 0x00085F10
		private bool HandleClientEventRequestCultureChange(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			RequestCultureChange requestCultureChange = (RequestCultureChange)baseMessage;
			MissionPeer component = peer.GetComponent<MissionPeer>();
			if (component != null && this._gameMode.CheckIfPlayerCanDespawn(component))
			{
				component.Culture = requestCultureChange.Culture;
				this.DespawnPlayer(component);
			}
			return true;
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x00087D50 File Offset: 0x00085F50
		private bool HandleClientEventCreateBannerForPeer(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			NetworkMessages.FromClient.CreateBanner createBanner = (NetworkMessages.FromClient.CreateBanner)baseMessage;
			MissionMultiplayerGameModeBase missionBehavior = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBase>();
			if (missionBehavior == null || !missionBehavior.AllowCustomPlayerBanners())
			{
				return false;
			}
			MissionPeer component = peer.GetComponent<MissionPeer>();
			if (component == null)
			{
				return false;
			}
			component.Peer.BannerCode = createBanner.BannerCode;
			MissionLobbyComponent.SyncBannersToAllClients(createBanner.BannerCode, component.GetNetworkPeer());
			return true;
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x00087DAC File Offset: 0x00085FAC
		private bool HandleClientEventRequestChangeCharacterMessage(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			MissionPeer component = ((RequestChangeCharacterMessage)baseMessage).NetworkPeer.GetComponent<MissionPeer>();
			if (component != null && this._gameMode.CheckIfPlayerCanDespawn(component))
			{
				this.DespawnPlayer(component);
			}
			return true;
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x00087DE3 File Offset: 0x00085FE3
		private static void SyncBannersToAllClients(string bannerCode, NetworkCommunicator ownerPeer)
		{
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new NetworkMessages.FromServer.CreateBanner(ownerPeer, bannerCode));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeTargetPlayer, ownerPeer);
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x00087DFD File Offset: 0x00085FFD
		protected override void HandleNewClientConnect(PlayerConnectionInfo clientConnectionInfo)
		{
			base.HandleNewClientConnect(clientConnectionInfo);
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x00087E06 File Offset: 0x00086006
		protected override void HandleLateNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			if (!networkPeer.IsServerPeer)
			{
				this.SendExistingObjectsToPeer(networkPeer);
			}
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x00087E18 File Offset: 0x00086018
		private void SendExistingObjectsToPeer(NetworkCommunicator peer)
		{
			long num = 0L;
			if (this.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.WaitingFirstPlayers)
			{
				num = this._timerComponent.GetCurrentTimerStartTime().NumberOfTicks;
			}
			GameNetwork.BeginModuleEventAsServer(peer);
			GameNetwork.WriteMessage(new MissionStateChange(this.CurrentMultiplayerState, num));
			GameNetwork.EndModuleEventAsServer();
			this.SendPeerInformationsToPeer(peer);
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x00087E68 File Offset: 0x00086068
		private void SendPeerInformationsToPeer(NetworkCommunicator peer)
		{
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeersIncludingDisconnectedPeers)
			{
				bool flag = networkCommunicator.VirtualPlayer != GameNetwork.VirtualPlayers[networkCommunicator.VirtualPlayer.Index];
				if (flag || networkCommunicator.IsSynchronized || networkCommunicator.JustReconnecting)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (component != null)
					{
						GameNetwork.BeginModuleEventAsServer(peer);
						GameNetwork.WriteMessage(new KillDeathCountChange(component.GetNetworkPeer(), null, component.KillCount, component.AssistCount, component.DeathCount, component.Score));
						GameNetwork.EndModuleEventAsServer();
						if (component.BotsUnderControlAlive != 0 || component.BotsUnderControlTotal != 0)
						{
							GameNetwork.BeginModuleEventAsServer(peer);
							GameNetwork.WriteMessage(new BotsControlledChange(component.GetNetworkPeer(), component.BotsUnderControlAlive, component.BotsUnderControlTotal));
							GameNetwork.EndModuleEventAsServer();
						}
					}
					else
					{
						Debug.Print(">#< SendPeerInformationsToPeer MissionPeer is null.", 0, Debug.DebugColor.BrightWhite, 17179869184UL);
					}
				}
				else
				{
					Debug.Print(string.Concat(new string[] { ">#< Can't send the info of ", networkCommunicator.UserName, " to ", peer.UserName, "." }), 0, Debug.DebugColor.BrightWhite, 17179869184UL);
					Debug.Print(string.Format("isDisconnectedPeer: {0}", flag), 0, Debug.DebugColor.BrightWhite, 17179869184UL);
					Debug.Print(string.Format("networkPeer.IsSynchronized: {0}", networkCommunicator.IsSynchronized), 0, Debug.DebugColor.BrightWhite, 17179869184UL);
					Debug.Print(string.Format("peer == networkPeer: {0}", peer == networkCommunicator), 0, Debug.DebugColor.BrightWhite, 17179869184UL);
					Debug.Print(string.Format("networkPeer.JustReconnecting: {0}", networkCommunicator.JustReconnecting), 0, Debug.DebugColor.BrightWhite, 17179869184UL);
				}
			}
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x00088064 File Offset: 0x00086264
		public void DespawnPlayer(MissionPeer missionPeer)
		{
			if (missionPeer.ControlledAgent != null && missionPeer.ControlledAgent.IsActive())
			{
				Agent controlledAgent = missionPeer.ControlledAgent;
				if (controlledAgent == null)
				{
					return;
				}
				controlledAgent.FadeOut(true, true);
			}
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x0008808D File Offset: 0x0008628D
		public override void OnScoreHit(Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon, bool isBlocked, bool isSiegeEngineHit, in Blow blow, in AttackCollisionData collisionData, float damagedHp, float hitDistance, float shotDifficulty)
		{
			if (affectorAgent != null && GameNetwork.IsServer && !isBlocked && affectorAgent != affectedAgent && affectorAgent.MissionPeer != null && damagedHp > 0f)
			{
				affectedAgent.AddHitter(affectorAgent.MissionPeer, damagedHp, affectorAgent.IsFriendOf(affectedAgent));
			}
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x000880C8 File Offset: 0x000862C8
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, killingBlow);
			if (GameNetwork.IsServer)
			{
				if (this.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.Ending)
				{
					return;
				}
				if ((agentState == AgentState.Killed || agentState == AgentState.Unconscious || agentState == AgentState.Routed) && affectedAgent != null && affectedAgent.IsHuman)
				{
					MissionPeer missionPeer = ((affectorAgent != null) ? affectorAgent.MissionPeer : null) ?? ((affectorAgent != null) ? affectorAgent.OwningAgentMissionPeer : null);
					MissionPeer missionPeer2 = this.RemoveHittersAndGetAssistorPeer((affectorAgent != null) ? affectorAgent.MissionPeer : null, affectedAgent);
					if (affectedAgent.MissionPeer != null)
					{
						this.OnPlayerDies(affectedAgent.MissionPeer, missionPeer, missionPeer2);
					}
					else
					{
						this.OnBotDies(affectedAgent, missionPeer, missionPeer2);
					}
					if (affectorAgent != null && affectorAgent.IsHuman)
					{
						if (affectorAgent != affectedAgent)
						{
							if (affectorAgent.MissionPeer != null)
							{
								this.OnPlayerKills(affectorAgent.MissionPeer, affectedAgent, missionPeer2);
								return;
							}
							this.OnBotKills(affectorAgent, affectedAgent);
							return;
						}
						else if (affectorAgent.MissionPeer != null)
						{
							affectorAgent.MissionPeer.Score -= (int)((float)this._gameMode.GetScoreForKill(affectedAgent) * 1.5f);
							this._missionScoreboardComponent.PlayerPropertiesChanged(affectorAgent.MissionPeer.GetNetworkPeer());
							GameNetwork.BeginBroadcastModuleEvent();
							GameNetwork.WriteMessage(new KillDeathCountChange(affectorAgent.MissionPeer.GetNetworkPeer(), affectedAgent.MissionPeer.GetNetworkPeer(), affectorAgent.MissionPeer.KillCount, affectorAgent.MissionPeer.AssistCount, affectorAgent.MissionPeer.DeathCount, affectorAgent.MissionPeer.Score));
							GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
						}
					}
				}
			}
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x0008823C File Offset: 0x0008643C
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			if (GameNetwork.IsServer)
			{
				if (agent.IsMount)
				{
					return;
				}
				if (agent.MissionPeer == null)
				{
					if (agent.OwningAgentMissionPeer != null)
					{
						MissionPeer owningAgentMissionPeer = agent.OwningAgentMissionPeer;
						int num = owningAgentMissionPeer.BotsUnderControlAlive;
						owningAgentMissionPeer.BotsUnderControlAlive = num + 1;
						MissionPeer owningAgentMissionPeer2 = agent.OwningAgentMissionPeer;
						num = owningAgentMissionPeer2.BotsUnderControlTotal;
						owningAgentMissionPeer2.BotsUnderControlTotal = num + 1;
						return;
					}
					this._missionScoreboardComponent.Sides[(int)agent.Team.Side].BotScores.AliveCount++;
				}
			}
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x000882C0 File Offset: 0x000864C0
		protected virtual void OnPlayerKills(MissionPeer killerPeer, Agent killedAgent, MissionPeer assistorPeer)
		{
			if (killedAgent.MissionPeer == null)
			{
				NetworkCommunicator networkCommunicator = GameNetwork.NetworkPeers.SingleOrDefault<NetworkCommunicator>((NetworkCommunicator x) => x.GetComponent<MissionPeer>() != null && x.GetComponent<MissionPeer>().ControlledFormation != null && x.GetComponent<MissionPeer>().ControlledFormation == killedAgent.Formation);
				if (networkCommunicator != null)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					killerPeer.OnKillAnotherPeer(component);
				}
			}
			else
			{
				killerPeer.OnKillAnotherPeer(killedAgent.MissionPeer);
			}
			if (killerPeer.Team.IsEnemyOf(killedAgent.Team))
			{
				killerPeer.Score += this._gameMode.GetScoreForKill(killedAgent);
				int num = killerPeer.KillCount;
				killerPeer.KillCount = num + 1;
			}
			else
			{
				killerPeer.Score -= (int)((float)this._gameMode.GetScoreForKill(killedAgent) * 1.5f);
				int num = killerPeer.KillCount;
				killerPeer.KillCount = num - 1;
			}
			this._missionScoreboardComponent.PlayerPropertiesChanged(killerPeer.GetNetworkPeer());
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new KillDeathCountChange(killerPeer.GetNetworkPeer(), null, killerPeer.KillCount, killerPeer.AssistCount, killerPeer.DeathCount, killerPeer.Score));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x000883E8 File Offset: 0x000865E8
		protected virtual void OnPlayerDies(MissionPeer peer, MissionPeer affectorPeer, MissionPeer assistorPeer)
		{
			if (assistorPeer != null)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new KillDeathCountChange(assistorPeer.GetNetworkPeer(), null, assistorPeer.KillCount, assistorPeer.AssistCount, assistorPeer.DeathCount, assistorPeer.Score));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
			int deathCount = peer.DeathCount;
			peer.DeathCount = deathCount + 1;
			peer.SpawnTimer.Reset(Mission.Current.CurrentTime, (float)MissionLobbyComponent.GetSpawnPeriodDurationForPeer(peer));
			peer.WantsToSpawnAsBot = false;
			peer.HasSpawnTimerExpired = false;
			this._missionScoreboardComponent.PlayerPropertiesChanged(peer.GetNetworkPeer());
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new KillDeathCountChange(peer.GetNetworkPeer(), (affectorPeer != null) ? affectorPeer.GetNetworkPeer() : null, peer.KillCount, peer.AssistCount, peer.DeathCount, peer.Score));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x000884B8 File Offset: 0x000866B8
		protected virtual void OnBotKills(Agent botAgent, Agent killedAgent)
		{
			Agent botAgent2 = botAgent;
			if (((botAgent2 != null) ? botAgent2.Team : null) != null)
			{
				Formation formation = botAgent.Formation;
				if (((formation != null) ? formation.PlayerOwner : null) != null)
				{
					NetworkCommunicator networkCommunicator = GameNetwork.NetworkPeers.SingleOrDefault<NetworkCommunicator>((NetworkCommunicator x) => x.GetComponent<MissionPeer>() != null && x.GetComponent<MissionPeer>().ControlledFormation == botAgent.Formation);
					if (networkCommunicator != null)
					{
						MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
						MissionPeer missionPeer = killedAgent.MissionPeer;
						NetworkCommunicator networkCommunicator2 = ((missionPeer != null) ? missionPeer.GetNetworkPeer() : null);
						if (killedAgent.MissionPeer == null)
						{
							NetworkCommunicator networkCommunicator3 = GameNetwork.NetworkPeers.SingleOrDefault<NetworkCommunicator>((NetworkCommunicator x) => x.GetComponent<MissionPeer>() != null && x.GetComponent<MissionPeer>().ControlledFormation == killedAgent.Formation);
							if (networkCommunicator3 != null)
							{
								NetworkCommunicator networkCommunicator4 = networkCommunicator3;
								component.OnKillAnotherPeer(networkCommunicator4.GetComponent<MissionPeer>());
							}
						}
						else
						{
							component.OnKillAnotherPeer(killedAgent.MissionPeer);
						}
						if (botAgent.Team.IsEnemyOf(killedAgent.Team))
						{
							MissionPeer missionPeer2 = component;
							int num = missionPeer2.KillCount;
							missionPeer2.KillCount = num + 1;
							component.Score += this._gameMode.GetScoreForKill(killedAgent);
						}
						else
						{
							MissionPeer missionPeer3 = component;
							int num = missionPeer3.KillCount;
							missionPeer3.KillCount = num - 1;
							component.Score -= (int)((float)this._gameMode.GetScoreForKill(killedAgent) * 1.5f);
						}
						this._missionScoreboardComponent.PlayerPropertiesChanged(networkCommunicator);
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new KillDeathCountChange(networkCommunicator, null, component.KillCount, component.AssistCount, component.DeathCount, component.Score));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					}
				}
				else
				{
					MissionScoreboardComponent.MissionScoreboardSide sideSafe = this._missionScoreboardComponent.GetSideSafe(botAgent.Team.Side);
					BotData botScores = sideSafe.BotScores;
					if (botAgent.Team.IsEnemyOf(killedAgent.Team))
					{
						botScores.KillCount++;
					}
					else
					{
						botScores.KillCount--;
					}
					this._missionScoreboardComponent.BotPropertiesChanged(sideSafe.Side);
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new BotData(sideSafe.Side, botScores.KillCount, botScores.AssistCount, botScores.DeathCount, botScores.AliveCount));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
				this._missionScoreboardComponent.BotPropertiesChanged(botAgent.Team.Side);
			}
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x00088724 File Offset: 0x00086924
		protected virtual void OnBotDies(Agent botAgent, MissionPeer affectorPeer, MissionPeer assistorPeer)
		{
			if (assistorPeer != null)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new KillDeathCountChange(assistorPeer.GetNetworkPeer(), (affectorPeer != null) ? affectorPeer.GetNetworkPeer() : null, assistorPeer.KillCount, assistorPeer.AssistCount, assistorPeer.DeathCount, assistorPeer.Score));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
			if (botAgent != null)
			{
				Formation formation = botAgent.Formation;
				if (((formation != null) ? formation.PlayerOwner : null) != null)
				{
					NetworkCommunicator networkCommunicator = GameNetwork.NetworkPeers.SingleOrDefault<NetworkCommunicator>((NetworkCommunicator x) => x.GetComponent<MissionPeer>() != null && x.GetComponent<MissionPeer>().ControlledFormation == botAgent.Formation);
					if (networkCommunicator != null)
					{
						MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
						MissionPeer missionPeer = component;
						int num = missionPeer.DeathCount;
						missionPeer.DeathCount = num + 1;
						MissionPeer missionPeer2 = component;
						num = missionPeer2.BotsUnderControlAlive;
						missionPeer2.BotsUnderControlAlive = num - 1;
						this._missionScoreboardComponent.PlayerPropertiesChanged(networkCommunicator);
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new KillDeathCountChange(networkCommunicator, (affectorPeer != null) ? affectorPeer.GetNetworkPeer() : null, component.KillCount, component.AssistCount, component.DeathCount, component.Score));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new BotsControlledChange(networkCommunicator, component.BotsUnderControlAlive, component.BotsUnderControlTotal));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					}
				}
				else
				{
					MissionScoreboardComponent.MissionScoreboardSide sideSafe = this._missionScoreboardComponent.GetSideSafe(botAgent.Team.Side);
					BotData botScores = sideSafe.BotScores;
					botScores.DeathCount++;
					botScores.AliveCount--;
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new BotData(sideSafe.Side, botScores.KillCount, botScores.AssistCount, botScores.DeathCount, botScores.AliveCount));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
				this._missionScoreboardComponent.BotPropertiesChanged(botAgent.Team.Side);
			}
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x000888EC File Offset: 0x00086AEC
		public override void OnClearScene()
		{
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeersIncludingDisconnectedPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (component != null)
				{
					component.BotsUnderControlAlive = 0;
					component.BotsUnderControlTotal = 0;
					component.ControlledFormation = null;
				}
			}
		}

		// Token: 0x0600259A RID: 9626 RVA: 0x00088950 File Offset: 0x00086B50
		public static int GetSpawnPeriodDurationForPeer(MissionPeer peer)
		{
			return Mission.Current.GetMissionBehavior<SpawnComponent>().GetMaximumReSpawnPeriodForPeer(peer);
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x00088964 File Offset: 0x00086B64
		public virtual void SetStateEndingAsServer()
		{
			this.CurrentMultiplayerState = MissionLobbyComponent.MultiplayerGameState.Ending;
			MBDebug.Print("Multiplayer game mission ending", 0, Debug.DebugColor.White, 17592186044416UL);
			this._timerComponent.StartTimerAsServer(MissionLobbyComponent.PostMatchWaitDuration);
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new MissionStateChange(this.CurrentMultiplayerState, this._timerComponent.GetCurrentTimerStartTime().NumberOfTicks));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			Debug.Print(string.Format("Current multiplayer state sent to clients: {0}", this.CurrentMultiplayerState), 0, Debug.DebugColor.White, 17592186044416UL);
			Action onPostMatchEnded = this.OnPostMatchEnded;
			if (onPostMatchEnded == null)
			{
				return;
			}
			onPostMatchEnded();
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x00088A04 File Offset: 0x00086C04
		private void SetStatePlayingAsServer()
		{
			this._warmupComponent = null;
			this.CurrentMultiplayerState = MissionLobbyComponent.MultiplayerGameState.Playing;
			this._timerComponent.StartTimerAsServer((float)(MultiplayerOptions.OptionType.MapTimeLimit.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) * 60));
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new MissionStateChange(this.CurrentMultiplayerState, this._timerComponent.GetCurrentTimerStartTime().NumberOfTicks));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
		}

		// Token: 0x0600259D RID: 9629 RVA: 0x00088A65 File Offset: 0x00086C65
		protected virtual void EndGameAsServer()
		{
		}

		// Token: 0x0600259E RID: 9630 RVA: 0x00088A68 File Offset: 0x00086C68
		private MissionPeer RemoveHittersAndGetAssistorPeer(MissionPeer killerPeer, Agent killedAgent)
		{
			Agent.Hitter assistingHitter = killedAgent.GetAssistingHitter(killerPeer);
			if (((assistingHitter != null) ? assistingHitter.HitterPeer : null) != null)
			{
				if (!assistingHitter.IsFriendlyHit)
				{
					MissionPeer hitterPeer = assistingHitter.HitterPeer;
					int num = hitterPeer.AssistCount;
					hitterPeer.AssistCount = num + 1;
				}
				else
				{
					MissionPeer hitterPeer2 = assistingHitter.HitterPeer;
					int num = hitterPeer2.AssistCount;
					hitterPeer2.AssistCount = num - 1;
				}
			}
			if (assistingHitter == null)
			{
				return null;
			}
			return assistingHitter.HitterPeer;
		}

		// Token: 0x0600259F RID: 9631 RVA: 0x00088ACA File Offset: 0x00086CCA
		private void SetStateEndingAsClient()
		{
			Action onPostMatchEnded = this.OnPostMatchEnded;
			if (onPostMatchEnded == null)
			{
				return;
			}
			onPostMatchEnded();
		}

		// Token: 0x060025A0 RID: 9632 RVA: 0x00088ADC File Offset: 0x00086CDC
		public void RequestCultureSelection()
		{
			Action onCultureSelectionRequested = this.OnCultureSelectionRequested;
			if (onCultureSelectionRequested == null)
			{
				return;
			}
			onCultureSelectionRequested();
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x00088AEE File Offset: 0x00086CEE
		public void RequestAdminMessage(string message, bool isBroadcast)
		{
			Action<string, bool> onAdminMessageRequested = this.OnAdminMessageRequested;
			if (onAdminMessageRequested == null)
			{
				return;
			}
			onAdminMessageRequested(message, isBroadcast);
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x00088B04 File Offset: 0x00086D04
		public void RequestTroopSelection()
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new RequestChangeCharacterMessage(GameNetwork.MyPeer));
				GameNetwork.EndModuleEventAsClient();
				return;
			}
			if (GameNetwork.IsServer)
			{
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				if (component != null && this._gameMode.CheckIfPlayerCanDespawn(component))
				{
					this.DespawnPlayer(component);
				}
			}
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x00088B5C File Offset: 0x00086D5C
		public void OnCultureSelected(BasicCultureObject culture)
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new RequestCultureChange(culture));
				GameNetwork.EndModuleEventAsClient();
				return;
			}
			if (GameNetwork.IsServer)
			{
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				if (component != null && this._gameMode.CheckIfPlayerCanDespawn(component))
				{
					component.Culture = culture;
					this.DespawnPlayer(component);
				}
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x060025A4 RID: 9636 RVA: 0x00088BB7 File Offset: 0x00086DB7
		// (set) Token: 0x060025A5 RID: 9637 RVA: 0x00088BBF File Offset: 0x00086DBF
		public MultiplayerGameType MissionType { get; set; }

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x060025A6 RID: 9638 RVA: 0x00088BC8 File Offset: 0x00086DC8
		// (set) Token: 0x060025A7 RID: 9639 RVA: 0x00088BD0 File Offset: 0x00086DD0
		public MissionLobbyComponent.MultiplayerGameState CurrentMultiplayerState
		{
			get
			{
				return this._currentMultiplayerState;
			}
			private set
			{
				if (this._currentMultiplayerState != value)
				{
					this._currentMultiplayerState = value;
					Action<MissionLobbyComponent.MultiplayerGameState> currentMultiplayerStateChanged = this.CurrentMultiplayerStateChanged;
					if (currentMultiplayerStateChanged == null)
					{
						return;
					}
					currentMultiplayerStateChanged(value);
				}
			}
		}

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x060025A8 RID: 9640 RVA: 0x00088BF4 File Offset: 0x00086DF4
		// (remove) Token: 0x060025A9 RID: 9641 RVA: 0x00088C2C File Offset: 0x00086E2C
		public event Action<MissionLobbyComponent.MultiplayerGameState> CurrentMultiplayerStateChanged;

		// Token: 0x060025AA RID: 9642 RVA: 0x00088C61 File Offset: 0x00086E61
		public int GetRandomFaceSeedForCharacter(BasicCharacterObject character, int addition = 0)
		{
			IRoundComponent roundComponent = this._roundComponent;
			return character.GetDefaultFaceSeed(addition + ((roundComponent != null) ? roundComponent.RoundCount : 0)) % 2000;
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x00088C84 File Offset: 0x00086E84
		[CommandLineFunctionality.CommandLineArgumentFunction("kill_player", "mp_host")]
		public static string MPHostChangeParam(List<string> strings)
		{
			if (Mission.Current == null)
			{
				return "kill_player can only be called within a mission.";
			}
			if (!GameNetwork.IsServer)
			{
				return "kill_player can only be called by the server.";
			}
			if (strings == null || strings.Count == 0)
			{
				return "usage: kill_player {UserName}.";
			}
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				if (networkCommunicator.UserName == strings[0] && networkCommunicator.ControlledAgent != null)
				{
					Mission.Current.KillAgentCheat(networkCommunicator.ControlledAgent);
					return "Success.";
				}
			}
			return "Could not find the player " + strings[0] + " or the agent.";
		}

		// Token: 0x04000E6C RID: 3692
		private static readonly float InactivityThreshold = 2f;

		// Token: 0x04000E6D RID: 3693
		public static readonly float PostMatchWaitDuration = 15f;

		// Token: 0x04000E70 RID: 3696
		private bool[] _classRestrictions = new bool[8];

		// Token: 0x04000E73 RID: 3699
		private MissionScoreboardComponent _missionScoreboardComponent;

		// Token: 0x04000E74 RID: 3700
		private MissionMultiplayerGameModeBase _gameMode;

		// Token: 0x04000E75 RID: 3701
		private MultiplayerTimerComponent _timerComponent;

		// Token: 0x04000E76 RID: 3702
		private IRoundComponent _roundComponent;

		// Token: 0x04000E77 RID: 3703
		private Timer _inactivityTimer;

		// Token: 0x04000E78 RID: 3704
		private MultiplayerWarmupComponent _warmupComponent;

		// Token: 0x04000E79 RID: 3705
		private static readonly Dictionary<Tuple<LobbyMissionType, bool>, Type> _lobbyComponentTypes = new Dictionary<Tuple<LobbyMissionType, bool>, Type>();

		// Token: 0x04000E7A RID: 3706
		private bool _usingFixedBanners;

		// Token: 0x04000E7C RID: 3708
		private MissionLobbyComponent.MultiplayerGameState _currentMultiplayerState;

		// Token: 0x02000574 RID: 1396
		public enum MultiplayerGameState
		{
			// Token: 0x04001E4B RID: 7755
			WaitingFirstPlayers,
			// Token: 0x04001E4C RID: 7756
			Playing,
			// Token: 0x04001E4D RID: 7757
			Ending
		}
	}
}
