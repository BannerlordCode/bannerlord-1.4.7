using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.MissionRepresentatives;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002B1 RID: 689
	public class MissionMultiplayerGameModeFlagDominationClient : MissionMultiplayerGameModeBaseClient, ICommanderInfo, IMissionBehavior
	{
		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x060026BE RID: 9918 RVA: 0x0008ED76 File Offset: 0x0008CF76
		public override bool IsGameModeUsingGold
		{
			get
			{
				return this.GameType != MultiplayerGameType.Captain;
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x060026BF RID: 9919 RVA: 0x0008ED84 File Offset: 0x0008CF84
		public override bool IsGameModeTactical
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x060026C0 RID: 9920 RVA: 0x0008ED87 File Offset: 0x0008CF87
		public override bool IsGameModeUsingRoundCountdown
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x060026C1 RID: 9921 RVA: 0x0008ED8A File Offset: 0x0008CF8A
		public override MultiplayerGameType GameType
		{
			get
			{
				return this._currentGameType;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x060026C2 RID: 9922 RVA: 0x0008ED92 File Offset: 0x0008CF92
		public override bool IsGameModeUsingCasualGold
		{
			get
			{
				return false;
			}
		}

		// Token: 0x14000055 RID: 85
		// (add) Token: 0x060026C3 RID: 9923 RVA: 0x0008ED98 File Offset: 0x0008CF98
		// (remove) Token: 0x060026C4 RID: 9924 RVA: 0x0008EDD0 File Offset: 0x0008CFD0
		public event Action<NetworkCommunicator> OnBotsControlledChangedEvent;

		// Token: 0x14000056 RID: 86
		// (add) Token: 0x060026C5 RID: 9925 RVA: 0x0008EE08 File Offset: 0x0008D008
		// (remove) Token: 0x060026C6 RID: 9926 RVA: 0x0008EE40 File Offset: 0x0008D040
		public event Action<BattleSideEnum, float> OnTeamPowerChangedEvent;

		// Token: 0x14000057 RID: 87
		// (add) Token: 0x060026C7 RID: 9927 RVA: 0x0008EE78 File Offset: 0x0008D078
		// (remove) Token: 0x060026C8 RID: 9928 RVA: 0x0008EEB0 File Offset: 0x0008D0B0
		public event Action<BattleSideEnum, float> OnMoraleChangedEvent;

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x060026C9 RID: 9929 RVA: 0x0008EEE8 File Offset: 0x0008D0E8
		// (remove) Token: 0x060026CA RID: 9930 RVA: 0x0008EF20 File Offset: 0x0008D120
		public event Action OnFlagNumberChangedEvent;

		// Token: 0x14000059 RID: 89
		// (add) Token: 0x060026CB RID: 9931 RVA: 0x0008EF58 File Offset: 0x0008D158
		// (remove) Token: 0x060026CC RID: 9932 RVA: 0x0008EF90 File Offset: 0x0008D190
		public event Action<FlagCapturePoint, Team> OnCapturePointOwnerChangedEvent;

		// Token: 0x1400005A RID: 90
		// (add) Token: 0x060026CD RID: 9933 RVA: 0x0008EFC8 File Offset: 0x0008D1C8
		// (remove) Token: 0x060026CE RID: 9934 RVA: 0x0008F000 File Offset: 0x0008D200
		public event Action<GoldGain> OnGoldGainEvent;

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x060026CF RID: 9935 RVA: 0x0008F035 File Offset: 0x0008D235
		// (set) Token: 0x060026D0 RID: 9936 RVA: 0x0008F03D File Offset: 0x0008D23D
		public IEnumerable<FlagCapturePoint> AllCapturePoints { get; private set; }

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x060026D1 RID: 9937 RVA: 0x0008F046 File Offset: 0x0008D246
		public bool AreMoralesIndependent
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x0008F04C File Offset: 0x0008D24C
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._scoreboardComponent = Mission.Current.GetMissionBehavior<MissionScoreboardComponent>();
			if (MultiplayerOptions.OptionType.SingleSpawn.GetBoolValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions))
			{
				this._currentGameType = ((MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) > 0) ? MultiplayerGameType.Captain : MultiplayerGameType.Battle);
			}
			else
			{
				this._currentGameType = MultiplayerGameType.Skirmish;
			}
			this.ResetTeamPowers(1f);
			this._capturePointOwners = new Team[3];
			this.AllCapturePoints = Mission.Current.MissionObjects.FindAllWithType<FlagCapturePoint>();
			base.RoundComponent.OnPreparationEnded += this.OnPreparationEnded;
			base.MissionNetworkComponent.OnMyClientSynchronized += this.OnMyClientSynchronized;
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x0008F0F1 File Offset: 0x0008D2F1
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
			base.RoundComponent.OnPreparationEnded -= this.OnPreparationEnded;
			base.MissionNetworkComponent.OnMyClientSynchronized -= this.OnMyClientSynchronized;
		}

		// Token: 0x060026D4 RID: 9940 RVA: 0x0008F127 File Offset: 0x0008D327
		private void OnMyClientSynchronized()
		{
			this._myRepresentative = GameNetwork.MyPeer.GetComponent<FlagDominationMissionRepresentative>();
		}

		// Token: 0x060026D5 RID: 9941 RVA: 0x0008F139 File Offset: 0x0008D339
		public override void AfterStart()
		{
			Mission.Current.SetMissionMode(MissionMode.Battle, true);
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x0008F148 File Offset: 0x0008D348
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClient)
			{
				registerer.RegisterBaseHandler<BotsControlledChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventBotsControlledChangeEvent));
				registerer.RegisterBaseHandler<FlagDominationMoraleChangeMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleMoraleChangedMessage));
				registerer.RegisterBaseHandler<SyncGoldsForSkirmish>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventUpdateGold));
				registerer.RegisterBaseHandler<FlagDominationFlagsRemovedMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleFlagsRemovedMessage));
				registerer.RegisterBaseHandler<FlagDominationCapturePointMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventPointCapturedMessage));
				registerer.RegisterBaseHandler<FormationWipedMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventFormationWipedMessage));
				registerer.RegisterBaseHandler<GoldGain>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventPersonalGoldGain));
			}
		}

		// Token: 0x060026D7 RID: 9943 RVA: 0x0008F1DC File Offset: 0x0008D3DC
		public void OnPreparationEnded()
		{
			this.AllCapturePoints = Mission.Current.MissionObjects.FindAllWithType<FlagCapturePoint>();
			Action onFlagNumberChangedEvent = this.OnFlagNumberChangedEvent;
			if (onFlagNumberChangedEvent != null)
			{
				onFlagNumberChangedEvent();
			}
			foreach (FlagCapturePoint flagCapturePoint in this.AllCapturePoints)
			{
				Action<FlagCapturePoint, Team> onCapturePointOwnerChangedEvent = this.OnCapturePointOwnerChangedEvent;
				if (onCapturePointOwnerChangedEvent != null)
				{
					onCapturePointOwnerChangedEvent(flagCapturePoint, null);
				}
			}
		}

		// Token: 0x060026D8 RID: 9944 RVA: 0x0008F25C File Offset: 0x0008D45C
		public override SpectatorCameraTypes GetMissionCameraLockMode(bool lockedToMainPlayer)
		{
			SpectatorCameraTypes spectatorCameraTypes = SpectatorCameraTypes.Invalid;
			MissionPeer missionPeer = (GameNetwork.IsMyPeerReady ? GameNetwork.MyPeer.GetComponent<MissionPeer>() : null);
			if (!lockedToMainPlayer && missionPeer != null)
			{
				if (missionPeer.Team != base.Mission.SpectatorTeam)
				{
					if (this.GameType == MultiplayerGameType.Captain && base.IsRoundInProgress)
					{
						Formation controlledFormation = missionPeer.ControlledFormation;
						if (controlledFormation != null)
						{
							if (controlledFormation.HasUnitsWithCondition((Agent agent) => !agent.IsPlayerControlled && agent.IsActive()))
							{
								spectatorCameraTypes = SpectatorCameraTypes.LockToPlayerFormation;
							}
						}
					}
				}
				else
				{
					spectatorCameraTypes = SpectatorCameraTypes.Free;
				}
			}
			return spectatorCameraTypes;
		}

		// Token: 0x060026D9 RID: 9945 RVA: 0x0008F2E4 File Offset: 0x0008D4E4
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (base.IsRoundInProgress && !affectedAgent.IsMount)
			{
				Team team = affectedAgent.Team;
				if (this.IsGameModeUsingGold)
				{
					this.UpdateTeamPowerBasedOnGold(team);
					return;
				}
				this.UpdateTeamPowerBasedOnTroopCount(team);
			}
		}

		// Token: 0x060026DA RID: 9946 RVA: 0x0008F320 File Offset: 0x0008D520
		public override void OnClearScene()
		{
			this._informedAboutFlagRemoval = false;
			this.AllCapturePoints = Mission.Current.MissionObjects.FindAllWithType<FlagCapturePoint>();
			foreach (FlagCapturePoint flagCapturePoint in this.AllCapturePoints)
			{
				this._capturePointOwners[flagCapturePoint.FlagIndex] = null;
			}
			this.ResetTeamPowers(1f);
			if (this._bellSoundEvent != null)
			{
				this._remainingTimeForBellSoundToStop = float.MinValue;
				this._bellSoundEvent.Stop();
				this._bellSoundEvent = null;
			}
		}

		// Token: 0x060026DB RID: 9947 RVA: 0x0008F3C0 File Offset: 0x0008D5C0
		protected override int GetWarningTimer()
		{
			int num = 0;
			if (base.IsRoundInProgress)
			{
				float num2 = -1f;
				switch (this.GameType)
				{
				case MultiplayerGameType.Battle:
					num2 = 210f;
					break;
				case MultiplayerGameType.Captain:
					num2 = 180f;
					break;
				case MultiplayerGameType.Skirmish:
					num2 = 120f;
					break;
				default:
					Debug.FailedAssert("A flag domination mode cannot be " + this.GameType + ".", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MultiplayerGameModeLogics\\ClientGameModeLogics\\MissionMultiplayerGameModeFlagDominationClient.cs", "GetWarningTimer", 207);
					break;
				}
				float num3 = (float)MultiplayerOptions.OptionType.RoundTimeLimit.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) - num2;
				float num4 = num3 + 30f;
				if (base.RoundComponent.RemainingRoundTime <= num4 && base.RoundComponent.RemainingRoundTime > num3)
				{
					num = MathF.Ceiling(30f - (num4 - base.RoundComponent.RemainingRoundTime));
					if (!this._informedAboutFlagRemoval)
					{
						this._informedAboutFlagRemoval = true;
						base.NotificationsComponent.FlagsWillBeRemovedInXSeconds(30);
					}
				}
			}
			return num;
		}

		// Token: 0x060026DC RID: 9948 RVA: 0x0008F4AB File Offset: 0x0008D6AB
		public Team GetFlagOwner(FlagCapturePoint flag)
		{
			return this._capturePointOwners[flag.FlagIndex];
		}

		// Token: 0x060026DD RID: 9949 RVA: 0x0008F4BC File Offset: 0x0008D6BC
		private void HandleServerEventBotsControlledChangeEvent(GameNetworkMessage baseMessage)
		{
			BotsControlledChange botsControlledChange = (BotsControlledChange)baseMessage;
			MissionPeer component = botsControlledChange.Peer.GetComponent<MissionPeer>();
			this.OnBotsControlledChanged(component, botsControlledChange.AliveCount, botsControlledChange.TotalCount);
		}

		// Token: 0x060026DE RID: 9950 RVA: 0x0008F4F0 File Offset: 0x0008D6F0
		private void HandleMoraleChangedMessage(GameNetworkMessage baseMessage)
		{
			FlagDominationMoraleChangeMessage flagDominationMoraleChangeMessage = (FlagDominationMoraleChangeMessage)baseMessage;
			this.OnMoraleChanged(flagDominationMoraleChangeMessage.Morale);
		}

		// Token: 0x060026DF RID: 9951 RVA: 0x0008F510 File Offset: 0x0008D710
		private void HandleServerEventUpdateGold(GameNetworkMessage baseMessage)
		{
			SyncGoldsForSkirmish syncGoldsForSkirmish = (SyncGoldsForSkirmish)baseMessage;
			FlagDominationMissionRepresentative component = syncGoldsForSkirmish.VirtualPlayer.GetComponent<FlagDominationMissionRepresentative>();
			this.OnGoldAmountChangedForRepresentative(component, syncGoldsForSkirmish.GoldAmount);
		}

		// Token: 0x060026E0 RID: 9952 RVA: 0x0008F53D File Offset: 0x0008D73D
		private void HandleFlagsRemovedMessage(GameNetworkMessage baseMessage)
		{
			this.OnNumberOfFlagsChanged();
		}

		// Token: 0x060026E1 RID: 9953 RVA: 0x0008F548 File Offset: 0x0008D748
		private void HandleServerEventPointCapturedMessage(GameNetworkMessage baseMessage)
		{
			FlagDominationCapturePointMessage flagDominationCapturePointMessage = (FlagDominationCapturePointMessage)baseMessage;
			foreach (FlagCapturePoint flagCapturePoint in this.AllCapturePoints)
			{
				if (flagCapturePoint.FlagIndex == flagDominationCapturePointMessage.FlagIndex)
				{
					this.OnCapturePointOwnerChanged(flagCapturePoint, Mission.MissionNetworkHelper.GetTeamFromTeamIndex(flagDominationCapturePointMessage.OwnerTeamIndex));
					break;
				}
			}
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x0008F5B8 File Offset: 0x0008D7B8
		private void HandleServerEventFormationWipedMessage(GameNetworkMessage baseMessage)
		{
			MatrixFrame cameraFrame = Mission.Current.GetCameraFrame();
			Vec3 vec = cameraFrame.origin + cameraFrame.rotation.u;
			MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/alerts/report/squad_wiped"), vec);
		}

		// Token: 0x060026E3 RID: 9955 RVA: 0x0008F5F8 File Offset: 0x0008D7F8
		private void HandleServerEventPersonalGoldGain(GameNetworkMessage baseMessage)
		{
			GoldGain goldGain = (GoldGain)baseMessage;
			Action<GoldGain> onGoldGainEvent = this.OnGoldGainEvent;
			if (onGoldGainEvent == null)
			{
				return;
			}
			onGoldGainEvent(goldGain);
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x0008F61D File Offset: 0x0008D81D
		public void OnTeamPowerChanged(BattleSideEnum teamSide, float power)
		{
			Action<BattleSideEnum, float> onTeamPowerChangedEvent = this.OnTeamPowerChangedEvent;
			if (onTeamPowerChangedEvent == null)
			{
				return;
			}
			onTeamPowerChangedEvent(teamSide, power);
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x0008F634 File Offset: 0x0008D834
		public void OnMoraleChanged(float morale)
		{
			for (int i = 0; i < 2; i++)
			{
				float num = (morale + 1f) / 2f;
				if (i == 0)
				{
					Action<BattleSideEnum, float> onMoraleChangedEvent = this.OnMoraleChangedEvent;
					if (onMoraleChangedEvent != null)
					{
						onMoraleChangedEvent(BattleSideEnum.Defender, 1f - num);
					}
				}
				else if (i == 1)
				{
					Action<BattleSideEnum, float> onMoraleChangedEvent2 = this.OnMoraleChangedEvent;
					if (onMoraleChangedEvent2 != null)
					{
						onMoraleChangedEvent2(BattleSideEnum.Attacker, num);
					}
				}
			}
			FlagDominationMissionRepresentative myRepresentative = this._myRepresentative;
			if (((myRepresentative != null) ? myRepresentative.MissionPeer.Team : null) != null && this._myRepresentative.MissionPeer.Team.Side != BattleSideEnum.None)
			{
				float num2 = MathF.Abs(morale);
				if (this._remainingTimeForBellSoundToStop < 0f)
				{
					if (num2 >= 0.6f && num2 < 1f)
					{
						this._remainingTimeForBellSoundToStop = float.MaxValue;
					}
					else
					{
						this._remainingTimeForBellSoundToStop = float.MinValue;
					}
					if (this._remainingTimeForBellSoundToStop > 0f)
					{
						BattleSideEnum side = this._myRepresentative.MissionPeer.Team.Side;
						if ((side == BattleSideEnum.Defender && morale >= 0.6f) || (side == BattleSideEnum.Attacker && morale <= -0.6f))
						{
							this._bellSoundEvent = SoundEvent.CreateEventFromString("event:/multiplayer/warning_bells_defender", base.Mission.Scene);
						}
						else
						{
							this._bellSoundEvent = SoundEvent.CreateEventFromString("event:/multiplayer/warning_bells_attacker", base.Mission.Scene);
						}
						MatrixFrame globalFrame = this.AllCapturePoints.Where<FlagCapturePoint>((FlagCapturePoint cp) => !cp.IsDeactivated).GetRandomElementInefficiently<FlagCapturePoint>().GameEntity.GetGlobalFrame();
						this._bellSoundEvent.PlayInPosition(globalFrame.origin + globalFrame.rotation.u * 3f);
						return;
					}
				}
				else if (num2 >= 1f || num2 < 0.6f)
				{
					this._remainingTimeForBellSoundToStop = float.MinValue;
				}
			}
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x0008F804 File Offset: 0x0008DA04
		public override void OnGoldAmountChangedForRepresentative(MissionRepresentativeBase representative, int goldAmount)
		{
			if (representative != null)
			{
				MissionPeer component = representative.GetComponent<MissionPeer>();
				if (component != null)
				{
					representative.UpdateGold(goldAmount);
					this._scoreboardComponent.PlayerPropertiesChanged(component);
					if (this.IsGameModeUsingGold && base.IsRoundInProgress && component.Team != null && component.Team.Side != BattleSideEnum.None)
					{
						this.UpdateTeamPowerBasedOnGold(component.Team);
					}
				}
			}
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x0008F863 File Offset: 0x0008DA63
		public void OnNumberOfFlagsChanged()
		{
			Action onFlagNumberChangedEvent = this.OnFlagNumberChangedEvent;
			if (onFlagNumberChangedEvent == null)
			{
				return;
			}
			onFlagNumberChangedEvent();
		}

		// Token: 0x060026E8 RID: 9960 RVA: 0x0008F875 File Offset: 0x0008DA75
		public void OnBotsControlledChanged(MissionPeer missionPeer, int botAliveCount, int botTotalCount)
		{
			missionPeer.BotsUnderControlAlive = botAliveCount;
			missionPeer.BotsUnderControlTotal = botTotalCount;
			Action<NetworkCommunicator> onBotsControlledChangedEvent = this.OnBotsControlledChangedEvent;
			if (onBotsControlledChangedEvent == null)
			{
				return;
			}
			onBotsControlledChangedEvent(missionPeer.GetNetworkPeer());
		}

		// Token: 0x060026E9 RID: 9961 RVA: 0x0008F89C File Offset: 0x0008DA9C
		public void OnCapturePointOwnerChanged(FlagCapturePoint flagCapturePoint, Team ownerTeam)
		{
			this._capturePointOwners[flagCapturePoint.FlagIndex] = ownerTeam;
			Action<FlagCapturePoint, Team> onCapturePointOwnerChangedEvent = this.OnCapturePointOwnerChangedEvent;
			if (onCapturePointOwnerChangedEvent != null)
			{
				onCapturePointOwnerChangedEvent(flagCapturePoint, ownerTeam);
			}
			if (this._myRepresentative != null && this._myRepresentative.MissionPeer.Team != null)
			{
				MatrixFrame cameraFrame = Mission.Current.GetCameraFrame();
				Vec3 vec = cameraFrame.origin + cameraFrame.rotation.u;
				if (this._myRepresentative.MissionPeer.Team == ownerTeam)
				{
					MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/alerts/report/flag_captured"), vec);
					return;
				}
				MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/alerts/report/flag_lost"), vec);
			}
		}

		// Token: 0x060026EA RID: 9962 RVA: 0x0008F93C File Offset: 0x0008DB3C
		public void OnRequestForfeitSpawn()
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new RequestForfeitSpawn());
				GameNetwork.EndModuleEventAsClient();
				return;
			}
			Mission.Current.GetMissionBehavior<MissionMultiplayerFlagDomination>().ForfeitSpawning(GameNetwork.MyPeer);
		}

		// Token: 0x060026EB RID: 9963 RVA: 0x0008F96E File Offset: 0x0008DB6E
		private void ResetTeamPowers(float value = 1f)
		{
			Action<BattleSideEnum, float> onTeamPowerChangedEvent = this.OnTeamPowerChangedEvent;
			if (onTeamPowerChangedEvent != null)
			{
				onTeamPowerChangedEvent(BattleSideEnum.Attacker, value);
			}
			Action<BattleSideEnum, float> onTeamPowerChangedEvent2 = this.OnTeamPowerChangedEvent;
			if (onTeamPowerChangedEvent2 == null)
			{
				return;
			}
			onTeamPowerChangedEvent2(BattleSideEnum.Defender, value);
		}

		// Token: 0x060026EC RID: 9964 RVA: 0x0008F998 File Offset: 0x0008DB98
		private void UpdateTeamPowerBasedOnGold(Team team)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (((component != null) ? component.Team : null) != null && component.Team.Side == team.Side)
				{
					int gold = component.GetComponent<FlagDominationMissionRepresentative>().Gold;
					if (gold >= 100)
					{
						num2 += gold;
					}
					if (component.ControlledAgent != null && component.ControlledAgent.IsActive())
					{
						MultiplayerClassDivisions.MPHeroClass mpheroClassForCharacter = MultiplayerClassDivisions.GetMPHeroClassForCharacter(component.ControlledAgent.Character);
						num2 += ((this._currentGameType == MultiplayerGameType.Battle) ? mpheroClassForCharacter.TroopBattleCost : mpheroClassForCharacter.TroopCost);
					}
					num++;
				}
			}
			if (this._currentGameType == MultiplayerGameType.Battle)
			{
				num3 = 120;
			}
			else
			{
				num3 = 300;
			}
			num += ((team.Side == BattleSideEnum.Attacker) ? MultiplayerOptions.OptionType.NumberOfBotsTeam1.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) : MultiplayerOptions.OptionType.NumberOfBotsTeam2.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			using (List<Agent>.Enumerator enumerator2 = team.ActiveAgents.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.MissionPeer == null)
					{
						num2 += num3;
					}
				}
			}
			int num4 = num * num3;
			float num5 = ((num4 == 0) ? 0f : ((float)num2 / (float)num4));
			num5 = MathF.Min(1f, num5);
			Action<BattleSideEnum, float> onTeamPowerChangedEvent = this.OnTeamPowerChangedEvent;
			if (onTeamPowerChangedEvent == null)
			{
				return;
			}
			onTeamPowerChangedEvent(team.Side, num5);
		}

		// Token: 0x060026ED RID: 9965 RVA: 0x0008FB30 File Offset: 0x0008DD30
		private void UpdateTeamPowerBasedOnTroopCount(Team team)
		{
			int count = team.ActiveAgents.Count;
			int num = count + team.QuerySystem.DeathCount;
			float num2 = (float)count / (float)num;
			Action<BattleSideEnum, float> onTeamPowerChangedEvent = this.OnTeamPowerChangedEvent;
			if (onTeamPowerChangedEvent == null)
			{
				return;
			}
			onTeamPowerChangedEvent(team.Side, num2);
		}

		// Token: 0x060026EE RID: 9966 RVA: 0x0008FB74 File Offset: 0x0008DD74
		public override List<CompassItemUpdateParams> GetCompassTargets()
		{
			List<CompassItemUpdateParams> list = new List<CompassItemUpdateParams>();
			if (!GameNetwork.IsMyPeerReady || !base.IsRoundInProgress)
			{
				return list;
			}
			MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
			if (component == null || component.Team == null || component.Team.Side == BattleSideEnum.None)
			{
				return list;
			}
			foreach (FlagCapturePoint flagCapturePoint in this.AllCapturePoints.Where<FlagCapturePoint>((FlagCapturePoint cp) => !cp.IsDeactivated))
			{
				int num = 17 + flagCapturePoint.FlagIndex;
				list.Add(new CompassItemUpdateParams(flagCapturePoint, (TargetIconType)num, flagCapturePoint.Position, flagCapturePoint.GetFlagColor(), flagCapturePoint.GetFlagColor2()));
			}
			bool flag = true;
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component2 = networkCommunicator.GetComponent<MissionPeer>();
				if (((component2 != null) ? component2.Team : null) != null && component2.Team.Side != BattleSideEnum.None)
				{
					bool flag2 = component2.ControlledFormation != null;
					if (!flag2)
					{
						flag = false;
					}
					if (flag || component2.Team == component.Team)
					{
						MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer = MultiplayerClassDivisions.GetMPHeroClassForPeer(component2, false);
						if (flag2)
						{
							Formation controlledFormation = component2.ControlledFormation;
							if (controlledFormation.CountOfUnits != 0)
							{
								WorldPosition cachedMedianPosition = controlledFormation.CachedMedianPosition;
								Vec2 vec = controlledFormation.SmoothedAverageUnitPosition;
								if (!vec.IsValid)
								{
									vec = controlledFormation.CachedAveragePosition;
								}
								cachedMedianPosition.SetVec2(vec);
								Banner banner = null;
								bool flag3 = false;
								bool flag4 = false;
								if (controlledFormation.Team != null)
								{
									if (controlledFormation.Banner == null)
									{
										controlledFormation.Banner = new Banner(controlledFormation.BannerCode, controlledFormation.Team.Color, controlledFormation.Team.Color2);
									}
									flag3 = controlledFormation.Team.IsAttacker;
									flag4 = controlledFormation.Team.IsPlayerAlly;
									banner = controlledFormation.Banner;
								}
								TargetIconType targetIconType = ((mpheroClassForPeer != null) ? mpheroClassForPeer.IconType : TargetIconType.None);
								list.Add(new CompassItemUpdateParams(controlledFormation, targetIconType, cachedMedianPosition.GetNavMeshVec3(), banner, flag3, flag4));
							}
						}
						else
						{
							Agent controlledAgent = component2.ControlledAgent;
							if (controlledAgent != null && controlledAgent.IsActive() && controlledAgent.Controller != AgentControllerType.Player)
							{
								Banner banner2 = new Banner(component2.Peer.BannerCode, component2.Team.Color, component2.Team.Color2);
								list.Add(new CompassItemUpdateParams(controlledAgent, mpheroClassForPeer.IconType, controlledAgent.Position, banner2, component2.Team.IsAttacker, component2.Team.IsPlayerAlly));
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060026EF RID: 9967 RVA: 0x0008FE6C File Offset: 0x0008E06C
		public override int GetGoldAmount()
		{
			if (this._myRepresentative != null)
			{
				return this._myRepresentative.Gold;
			}
			return 0;
		}

		// Token: 0x060026F0 RID: 9968 RVA: 0x0008FE84 File Offset: 0x0008E084
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._remainingTimeForBellSoundToStop > 0f)
			{
				this._remainingTimeForBellSoundToStop -= dt;
			}
			if (this._bellSoundEvent != null && (this._remainingTimeForBellSoundToStop <= 0f || base.MissionLobbyComponent.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Playing))
			{
				this._remainingTimeForBellSoundToStop = float.MinValue;
				this._bellSoundEvent.Stop();
				this._bellSoundEvent = null;
			}
		}

		// Token: 0x04000EB1 RID: 3761
		private const float MySideMoraleDropThreshold = 0.4f;

		// Token: 0x04000EB2 RID: 3762
		private float _remainingTimeForBellSoundToStop = float.MinValue;

		// Token: 0x04000EB3 RID: 3763
		private SoundEvent _bellSoundEvent;

		// Token: 0x04000EB4 RID: 3764
		private FlagDominationMissionRepresentative _myRepresentative;

		// Token: 0x04000EB5 RID: 3765
		private MissionScoreboardComponent _scoreboardComponent;

		// Token: 0x04000EB6 RID: 3766
		private MultiplayerGameType _currentGameType;

		// Token: 0x04000EB7 RID: 3767
		private Team[] _capturePointOwners;

		// Token: 0x04000EB9 RID: 3769
		private bool _informedAboutFlagRemoval;
	}
}
