using System;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002C0 RID: 704
	public class MultiplayerRoundController : MissionNetwork, IRoundComponent, IMissionBehavior
	{
		// Token: 0x1400006D RID: 109
		// (add) Token: 0x06002840 RID: 10304 RVA: 0x0009875C File Offset: 0x0009695C
		// (remove) Token: 0x06002841 RID: 10305 RVA: 0x00098794 File Offset: 0x00096994
		public event Action OnRoundStarted;

		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06002842 RID: 10306 RVA: 0x000987CC File Offset: 0x000969CC
		// (remove) Token: 0x06002843 RID: 10307 RVA: 0x00098804 File Offset: 0x00096A04
		public event Action OnPreparationEnded;

		// Token: 0x1400006F RID: 111
		// (add) Token: 0x06002844 RID: 10308 RVA: 0x0009883C File Offset: 0x00096A3C
		// (remove) Token: 0x06002845 RID: 10309 RVA: 0x00098874 File Offset: 0x00096A74
		public event Action OnPreRoundEnding;

		// Token: 0x14000070 RID: 112
		// (add) Token: 0x06002846 RID: 10310 RVA: 0x000988AC File Offset: 0x00096AAC
		// (remove) Token: 0x06002847 RID: 10311 RVA: 0x000988E4 File Offset: 0x00096AE4
		public event Action OnRoundEnding;

		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06002848 RID: 10312 RVA: 0x0009891C File Offset: 0x00096B1C
		// (remove) Token: 0x06002849 RID: 10313 RVA: 0x00098954 File Offset: 0x00096B54
		public event Action OnPostRoundEnded;

		// Token: 0x14000072 RID: 114
		// (add) Token: 0x0600284A RID: 10314 RVA: 0x0009898C File Offset: 0x00096B8C
		// (remove) Token: 0x0600284B RID: 10315 RVA: 0x000989C4 File Offset: 0x00096BC4
		public event Action OnCurrentRoundStateChanged;

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x0600284C RID: 10316 RVA: 0x000989F9 File Offset: 0x00096BF9
		// (set) Token: 0x0600284D RID: 10317 RVA: 0x00098A01 File Offset: 0x00096C01
		public int RoundCount
		{
			get
			{
				return this._roundCount;
			}
			set
			{
				if (this._roundCount != value)
				{
					this._roundCount = value;
					if (GameNetwork.IsServer)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new RoundCountChange(this._roundCount));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					}
				}
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x0600284E RID: 10318 RVA: 0x00098A36 File Offset: 0x00096C36
		// (set) Token: 0x0600284F RID: 10319 RVA: 0x00098A3E File Offset: 0x00096C3E
		public BattleSideEnum RoundWinner
		{
			get
			{
				return this._roundWinner;
			}
			set
			{
				if (this._roundWinner != value)
				{
					this._roundWinner = value;
					if (GameNetwork.IsServer)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new RoundWinnerChange(value));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					}
				}
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06002850 RID: 10320 RVA: 0x00098A6E File Offset: 0x00096C6E
		// (set) Token: 0x06002851 RID: 10321 RVA: 0x00098A76 File Offset: 0x00096C76
		public RoundEndReason RoundEndReason
		{
			get
			{
				return this._roundEndReason;
			}
			set
			{
				if (this._roundEndReason != value)
				{
					this._roundEndReason = value;
					if (GameNetwork.IsServer)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new RoundEndReasonChange(value));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					}
				}
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06002852 RID: 10322 RVA: 0x00098AA6 File Offset: 0x00096CA6
		// (set) Token: 0x06002853 RID: 10323 RVA: 0x00098AAE File Offset: 0x00096CAE
		public bool IsMatchEnding { get; private set; }

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06002854 RID: 10324 RVA: 0x00098AB7 File Offset: 0x00096CB7
		// (set) Token: 0x06002855 RID: 10325 RVA: 0x00098ABF File Offset: 0x00096CBF
		public float LastRoundEndRemainingTime { get; private set; }

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06002856 RID: 10326 RVA: 0x00098AC8 File Offset: 0x00096CC8
		public float RemainingRoundTime
		{
			get
			{
				return this._gameModeServer.TimerComponent.GetRemainingTime(false);
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06002857 RID: 10327 RVA: 0x00098ADB File Offset: 0x00096CDB
		// (set) Token: 0x06002858 RID: 10328 RVA: 0x00098AE3 File Offset: 0x00096CE3
		public MultiplayerRoundState CurrentRoundState { get; private set; }

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06002859 RID: 10329 RVA: 0x00098AEC File Offset: 0x00096CEC
		public bool IsRoundInProgress
		{
			get
			{
				return this.CurrentRoundState == MultiplayerRoundState.InProgress;
			}
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x00098AF7 File Offset: 0x00096CF7
		public void EnableEquipmentUpdate()
		{
			this._equipmentUpdateDisabled = false;
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x00098B00 File Offset: 0x00096D00
		public override void AfterStart()
		{
			this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
			if (GameNetwork.IsServerOrRecorder)
			{
				this._gameModeServer = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBase>();
			}
			this._missionLobbyComponent = Mission.Current.GetMissionBehavior<MissionLobbyComponent>();
			this._roundCount = 0;
			this._gameModeServer.TimerComponent.StartTimerAsServer(8f);
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x00098B58 File Offset: 0x00096D58
		private void EndRound()
		{
			if (this.OnPreRoundEnding != null)
			{
				this.OnPreRoundEnding();
			}
			this.ChangeRoundState(MultiplayerRoundState.Ending);
			this._gameModeServer.TimerComponent.StartTimerAsServer(3f);
			this._roundTimeOver = false;
			if (this.OnRoundEnding != null)
			{
				this.OnRoundEnding();
			}
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x00098BAE File Offset: 0x00096DAE
		private bool CheckPostEndRound()
		{
			return this._gameModeServer.TimerComponent.CheckIfTimerPassed();
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x00098BC0 File Offset: 0x00096DC0
		private bool CheckPostMatchEnd()
		{
			return this._gameModeServer.TimerComponent.CheckIfTimerPassed();
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x00098BD4 File Offset: 0x00096DD4
		private void PostRoundEnd()
		{
			this._gameModeServer.TimerComponent.StartTimerAsServer(5f);
			this.ChangeRoundState(MultiplayerRoundState.Ended);
			if (this._roundCount == MultiplayerOptions.OptionType.RoundTotal.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) || this.CheckForMatchEndEarly() || !this.HasEnoughCharactersOnBothSides())
			{
				this.IsMatchEnding = true;
			}
			if (this.OnPostRoundEnded != null)
			{
				this.OnPostRoundEnded();
			}
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x00098C37 File Offset: 0x00096E37
		private void PostMatchEnd()
		{
			this._gameModeServer.TimerComponent.StartTimerAsServer(5f);
			this.ChangeRoundState(MultiplayerRoundState.MatchEnded);
			this._missionLobbyComponent.SetStateEndingAsServer();
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x00098C60 File Offset: 0x00096E60
		public override void OnRemoveBehavior()
		{
			GameNetwork.RemoveNetworkHandler(this);
			base.OnRemoveBehavior();
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x00098C70 File Offset: 0x00096E70
		private void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode mode)
		{
			GameNetwork.NetworkMessageHandlerRegisterer networkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegisterer(mode);
			if (!GameNetwork.IsClient && GameNetwork.IsServer)
			{
				networkMessageHandlerRegisterer.Register<CultureVoteClient>(new GameNetworkMessage.ClientMessageHandlerDelegate<CultureVoteClient>(this.HandleClientEventCultureSelect));
			}
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x00098CA4 File Offset: 0x00096EA4
		protected override void OnUdpNetworkHandlerClose()
		{
			this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x00098CB0 File Offset: 0x00096EB0
		public override void OnPreDisplayMissionTick(float dt)
		{
			if (GameNetwork.IsServer)
			{
				if (this._missionLobbyComponent.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.WaitingFirstPlayers)
				{
					if (!this.IsMatchEnding && this._missionLobbyComponent.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending && (this.CurrentRoundState == MultiplayerRoundState.WaitingForPlayers || this.CurrentRoundState == MultiplayerRoundState.Ended))
					{
						if (this.CheckForNewRound())
						{
							this.BeginNewRound();
							return;
						}
						if (this.IsMatchEnding)
						{
							this.PostMatchEnd();
							return;
						}
					}
					else if (this.CurrentRoundState == MultiplayerRoundState.Preparation)
					{
						if (this.CheckForPreparationEnd())
						{
							this.EndPreparation();
							this.StartSpawning(this._equipmentUpdateDisabled);
							return;
						}
					}
					else if (this.CurrentRoundState == MultiplayerRoundState.InProgress)
					{
						if (this.CheckForRoundEnd())
						{
							this.EndRound();
							return;
						}
					}
					else if (this.CurrentRoundState == MultiplayerRoundState.Ending)
					{
						if (this.CheckPostEndRound())
						{
							this.PostRoundEnd();
							return;
						}
					}
					else if (this.CurrentRoundState == MultiplayerRoundState.Ended && this.IsMatchEnding && this.CheckPostMatchEnd())
					{
						this.PostMatchEnd();
						return;
					}
				}
			}
			else
			{
				this._gameModeServer.TimerComponent.CheckIfTimerPassed();
			}
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x00098DA4 File Offset: 0x00096FA4
		private void ChangeRoundState(MultiplayerRoundState newRoundState)
		{
			if (this.CurrentRoundState != newRoundState)
			{
				if (this.CurrentRoundState == MultiplayerRoundState.InProgress)
				{
					this.LastRoundEndRemainingTime = this.RemainingRoundTime;
				}
				this.CurrentRoundState = newRoundState;
				this._currentRoundStateStartTime = MissionTime.Now;
				Action onCurrentRoundStateChanged = this.OnCurrentRoundStateChanged;
				if (onCurrentRoundStateChanged != null)
				{
					onCurrentRoundStateChanged();
				}
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new RoundStateChange(newRoundState, this._currentRoundStateStartTime.NumberOfTicks, MathF.Ceiling(this.LastRoundEndRemainingTime)));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x00098E1F File Offset: 0x0009701F
		protected override void HandleLateNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
		}

		// Token: 0x06002867 RID: 10343 RVA: 0x00098E21 File Offset: 0x00097021
		public bool HandleClientEventCultureSelect(NetworkCommunicator peer, CultureVoteClient message)
		{
			peer.GetComponent<MissionPeer>().HandleVoteChange(message.VotedType, message.VotedCulture);
			return true;
		}

		// Token: 0x06002868 RID: 10344 RVA: 0x00098E3C File Offset: 0x0009703C
		private bool CheckForRoundEnd()
		{
			if (!this._roundTimeOver)
			{
				this._roundTimeOver = this._gameModeServer.TimerComponent.CheckIfTimerPassed();
			}
			return (!this._gameModeServer.CheckIfOvertime() && this._roundTimeOver) || this._gameModeServer.CheckForRoundEnd();
		}

		// Token: 0x06002869 RID: 10345 RVA: 0x00098E8C File Offset: 0x0009708C
		private bool CheckForNewRound()
		{
			if (this.CurrentRoundState != MultiplayerRoundState.WaitingForPlayers && !this._gameModeServer.TimerComponent.CheckIfTimerPassed())
			{
				return false;
			}
			int[] array = new int[2];
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (networkCommunicator.IsSynchronized && ((component != null) ? component.Team : null) != null && component.Team.Side != BattleSideEnum.None)
				{
					array[(int)component.Team.Side]++;
				}
			}
			if (array.Sum() < MultiplayerOptions.OptionType.MinNumberOfPlayersForMatchStart.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) && this.RoundCount == 0)
			{
				this.IsMatchEnding = true;
				return false;
			}
			return true;
		}

		// Token: 0x0600286A RID: 10346 RVA: 0x00098F64 File Offset: 0x00097164
		private bool HasEnoughCharactersOnBothSides()
		{
			bool flag;
			if (MultiplayerOptions.OptionType.NumberOfBotsTeam1.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) <= 0)
			{
				flag = GameNetwork.NetworkPeers.Count<NetworkCommunicator>((NetworkCommunicator q) => q.GetComponent<MissionPeer>() != null && q.GetComponent<MissionPeer>().Team == Mission.Current.AttackerTeam) > 0;
			}
			else
			{
				flag = true;
			}
			bool flag2;
			if (MultiplayerOptions.OptionType.NumberOfBotsTeam2.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) <= 0)
			{
				flag2 = GameNetwork.NetworkPeers.Count<NetworkCommunicator>((NetworkCommunicator q) => q.GetComponent<MissionPeer>() != null && q.GetComponent<MissionPeer>().Team == Mission.Current.DefenderTeam) > 0;
			}
			else
			{
				flag2 = true;
			}
			bool flag3 = flag2;
			return flag && flag3;
		}

		// Token: 0x0600286B RID: 10347 RVA: 0x00098FE8 File Offset: 0x000971E8
		private void BeginNewRound()
		{
			if (this.CurrentRoundState == MultiplayerRoundState.WaitingForPlayers)
			{
				this._gameModeServer.ClearPeerCounts();
			}
			this.ChangeRoundState(MultiplayerRoundState.Preparation);
			int roundCount = this.RoundCount;
			this.RoundCount = roundCount + 1;
			Mission.Current.ResetMission();
			this._gameModeServer.MultiplayerTeamSelectComponent.BalanceTeams();
			this._gameModeServer.TimerComponent.StartTimerAsServer((float)MultiplayerOptions.OptionType.RoundPreparationTimeLimit.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			Action onRoundStarted = this.OnRoundStarted;
			if (onRoundStarted != null)
			{
				onRoundStarted();
			}
			this._gameModeServer.SpawnComponent.ToggleUpdatingSpawnEquipment(true);
		}

		// Token: 0x0600286C RID: 10348 RVA: 0x00099074 File Offset: 0x00097274
		private bool CheckForPreparationEnd()
		{
			return this.CurrentRoundState == MultiplayerRoundState.Preparation && this._gameModeServer.TimerComponent.CheckIfTimerPassed();
		}

		// Token: 0x0600286D RID: 10349 RVA: 0x00099091 File Offset: 0x00097291
		private void EndPreparation()
		{
			if (this.OnPreparationEnded != null)
			{
				this.OnPreparationEnded();
			}
		}

		// Token: 0x0600286E RID: 10350 RVA: 0x000990A6 File Offset: 0x000972A6
		private void StartSpawning(bool disableEquipmentUpdate = true)
		{
			this._gameModeServer.TimerComponent.StartTimerAsServer((float)MultiplayerOptions.OptionType.RoundTimeLimit.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			if (disableEquipmentUpdate)
			{
				this._gameModeServer.SpawnComponent.ToggleUpdatingSpawnEquipment(false);
			}
			this.ChangeRoundState(MultiplayerRoundState.InProgress);
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x000990DC File Offset: 0x000972DC
		private bool CheckForMatchEndEarly()
		{
			bool flag = false;
			MissionScoreboardComponent missionBehavior = Mission.Current.GetMissionBehavior<MissionScoreboardComponent>();
			if (missionBehavior != null)
			{
				for (int i = 0; i < 2; i++)
				{
					if (missionBehavior.GetRoundScore((BattleSideEnum)i) > MultiplayerOptions.OptionType.RoundTotal.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) / 2)
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x00099120 File Offset: 0x00097320
		protected override void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			if (!networkPeer.IsServerPeer)
			{
				GameNetwork.BeginModuleEventAsServer(networkPeer);
				GameNetwork.WriteMessage(new RoundStateChange(this.CurrentRoundState, this._currentRoundStateStartTime.NumberOfTicks, MathF.Ceiling(this.LastRoundEndRemainingTime)));
				GameNetwork.EndModuleEventAsServer();
				GameNetwork.BeginModuleEventAsServer(networkPeer);
				GameNetwork.WriteMessage(new RoundWinnerChange(this.RoundWinner));
				GameNetwork.EndModuleEventAsServer();
				GameNetwork.BeginModuleEventAsServer(networkPeer);
				GameNetwork.WriteMessage(new RoundCountChange(this.RoundCount));
				GameNetwork.EndModuleEventAsServer();
			}
		}

		// Token: 0x04000F7C RID: 3964
		private MissionMultiplayerGameModeBase _gameModeServer;

		// Token: 0x04000F7D RID: 3965
		private int _roundCount;

		// Token: 0x04000F7E RID: 3966
		private BattleSideEnum _roundWinner;

		// Token: 0x04000F7F RID: 3967
		private RoundEndReason _roundEndReason;

		// Token: 0x04000F80 RID: 3968
		private MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x04000F82 RID: 3970
		private bool _roundTimeOver;

		// Token: 0x04000F84 RID: 3972
		private MissionTime _currentRoundStateStartTime;

		// Token: 0x04000F86 RID: 3974
		private bool _equipmentUpdateDisabled = true;
	}
}
