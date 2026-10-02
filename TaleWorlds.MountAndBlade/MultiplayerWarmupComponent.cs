using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002C3 RID: 707
	public class MultiplayerWarmupComponent : MissionNetwork
	{
		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06002895 RID: 10389 RVA: 0x00099ECF File Offset: 0x000980CF
		public static float TotalWarmupDuration
		{
			get
			{
				return (float)MultiplayerOptions.OptionType.WarmupTimeLimitInSeconds.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			}
		}

		// Token: 0x14000077 RID: 119
		// (add) Token: 0x06002896 RID: 10390 RVA: 0x00099EDC File Offset: 0x000980DC
		// (remove) Token: 0x06002897 RID: 10391 RVA: 0x00099F14 File Offset: 0x00098114
		public event Action OnWarmupEnding;

		// Token: 0x14000078 RID: 120
		// (add) Token: 0x06002898 RID: 10392 RVA: 0x00099F4C File Offset: 0x0009814C
		// (remove) Token: 0x06002899 RID: 10393 RVA: 0x00099F84 File Offset: 0x00098184
		public event Action OnWarmupEnded;

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x0600289A RID: 10394 RVA: 0x00099FB9 File Offset: 0x000981B9
		public bool IsInWarmup
		{
			get
			{
				return this.WarmupState != MultiplayerWarmupComponent.WarmupStates.Ended;
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x0600289B RID: 10395 RVA: 0x00099FC7 File Offset: 0x000981C7
		// (set) Token: 0x0600289C RID: 10396 RVA: 0x00099FD0 File Offset: 0x000981D0
		private MultiplayerWarmupComponent.WarmupStates WarmupState
		{
			get
			{
				return this._warmupState;
			}
			set
			{
				this._warmupState = value;
				if (GameNetwork.IsServer)
				{
					this._currentStateStartTime = MissionTime.Now;
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new WarmupStateChange(this._warmupState, this._currentStateStartTime.NumberOfTicks));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
			}
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x0009A01D File Offset: 0x0009821D
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._gameMode = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBase>();
			this._timerComponent = base.Mission.GetMissionBehavior<MultiplayerTimerComponent>();
			this._lobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x0009A058 File Offset: 0x00098258
		public override void AfterStart()
		{
			base.AfterStart();
			this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x0009A067 File Offset: 0x00098267
		protected override void OnUdpNetworkHandlerClose()
		{
			this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x0009A070 File Offset: 0x00098270
		private void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode mode)
		{
			GameNetwork.NetworkMessageHandlerRegisterer networkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegisterer(mode);
			if (GameNetwork.IsClient)
			{
				networkMessageHandlerRegisterer.Register<WarmupStateChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<WarmupStateChange>(this.HandleServerEventWarmupStateChange));
			}
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x0009A09D File Offset: 0x0009829D
		public bool CheckForWarmupProgressEnd()
		{
			return this._gameMode.CheckForWarmupEnd() || this._timerComponent.GetRemainingTime(false) <= 30f;
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x0009A0C4 File Offset: 0x000982C4
		public override void OnPreDisplayMissionTick(float dt)
		{
			if (GameNetwork.IsServer && this._lobbyComponent.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending)
			{
				switch (this.WarmupState)
				{
				case MultiplayerWarmupComponent.WarmupStates.WaitingForPlayers:
					this.BeginWarmup();
					return;
				case MultiplayerWarmupComponent.WarmupStates.InProgress:
					if (this.CheckForWarmupProgressEnd())
					{
						this.EndWarmupProgress();
						return;
					}
					break;
				case MultiplayerWarmupComponent.WarmupStates.Ending:
					if (this._timerComponent.CheckIfTimerPassed())
					{
						this.EndWarmup();
						return;
					}
					break;
				case MultiplayerWarmupComponent.WarmupStates.Ended:
					if (this._timerComponent.CheckIfTimerPassed())
					{
						base.Mission.RemoveMissionBehavior(this);
						return;
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x0009A150 File Offset: 0x00098350
		private void BeginWarmup()
		{
			this.WarmupState = MultiplayerWarmupComponent.WarmupStates.InProgress;
			Mission.Current.ResetMission();
			this._gameMode.MultiplayerTeamSelectComponent.BalanceTeams();
			this._timerComponent.StartTimerAsServer(MultiplayerWarmupComponent.TotalWarmupDuration);
			this._gameMode.SpawnComponent.SpawningBehavior.Clear();
			SpawnComponent.SetWarmupSpawningBehavior();
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x0009A1A8 File Offset: 0x000983A8
		public void EndWarmupProgress()
		{
			this.WarmupState = MultiplayerWarmupComponent.WarmupStates.Ending;
			this._timerComponent.StartTimerAsServer(30f);
			Action onWarmupEnding = this.OnWarmupEnding;
			if (onWarmupEnding == null)
			{
				return;
			}
			onWarmupEnding();
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x0009A1D4 File Offset: 0x000983D4
		private void EndWarmup()
		{
			this.WarmupState = MultiplayerWarmupComponent.WarmupStates.Ended;
			this._timerComponent.StartTimerAsServer(3f);
			Action onWarmupEnded = this.OnWarmupEnded;
			if (onWarmupEnded != null)
			{
				onWarmupEnded();
			}
			if (!GameNetwork.IsDedicatedServer)
			{
				this.PlayBattleStartingSound();
			}
			Mission.Current.ResetMission();
			this._gameMode.MultiplayerTeamSelectComponent.BalanceTeams();
			this._gameMode.SpawnComponent.SpawningBehavior.Clear();
			SpawnComponent.SetSpawningBehaviorForCurrentGameType(this._gameMode.GetMissionType());
			if (!this.CanMatchStartAfterWarmup())
			{
				this._lobbyComponent.SetStateEndingAsServer();
			}
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x0009A268 File Offset: 0x00098468
		public bool CanMatchStartAfterWarmup()
		{
			bool[] array = new bool[2];
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (((component != null) ? component.Team : null) != null && component.Team.Side != BattleSideEnum.None)
				{
					array[(int)component.Team.Side] = true;
				}
				if (array[1] && array[0])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x0009A2FC File Offset: 0x000984FC
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
			this.OnWarmupEnding = null;
			this.OnWarmupEnded = null;
			if (GameNetwork.IsServer && !this._gameMode.UseRoundController() && this._lobbyComponent.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending)
			{
				this._gameMode.SpawnComponent.SpawningBehavior.RequestStartSpawnSession();
			}
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x0009A354 File Offset: 0x00098554
		protected override void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			if (this.IsInWarmup && !networkPeer.IsServerPeer)
			{
				GameNetwork.BeginModuleEventAsServer(networkPeer);
				GameNetwork.WriteMessage(new WarmupStateChange(this._warmupState, this._currentStateStartTime.NumberOfTicks));
				GameNetwork.EndModuleEventAsServer();
			}
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x0009A38C File Offset: 0x0009858C
		private void HandleServerEventWarmupStateChange(WarmupStateChange message)
		{
			this.WarmupState = message.WarmupState;
			switch (this.WarmupState)
			{
			case MultiplayerWarmupComponent.WarmupStates.InProgress:
				this._timerComponent.StartTimerAsClient(message.StateStartTimeInSeconds, MultiplayerWarmupComponent.TotalWarmupDuration);
				return;
			case MultiplayerWarmupComponent.WarmupStates.Ending:
			{
				this._timerComponent.StartTimerAsClient(message.StateStartTimeInSeconds, 30f);
				Action onWarmupEnding = this.OnWarmupEnding;
				if (onWarmupEnding == null)
				{
					return;
				}
				onWarmupEnding();
				return;
			}
			case MultiplayerWarmupComponent.WarmupStates.Ended:
			{
				this._timerComponent.StartTimerAsClient(message.StateStartTimeInSeconds, 3f);
				Action onWarmupEnded = this.OnWarmupEnded;
				if (onWarmupEnded != null)
				{
					onWarmupEnded();
				}
				this.PlayBattleStartingSound();
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x0009A42C File Offset: 0x0009862C
		private void PlayBattleStartingSound()
		{
			MatrixFrame cameraFrame = Mission.Current.GetCameraFrame();
			Vec3 vec = cameraFrame.origin + cameraFrame.rotation.u;
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
			if (((missionPeer != null) ? missionPeer.Team : null) != null)
			{
				string text = ((missionPeer.Team.Side == BattleSideEnum.Attacker) ? MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) : MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
				MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/alerts/rally/" + text.ToLower()), vec);
				return;
			}
			MBSoundEvent.PlaySound(SoundEvent.GetEventIdFromString("event:/alerts/rally/generic"), vec);
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x0009A4CC File Offset: 0x000986CC
		[CommandLineFunctionality.CommandLineArgumentFunction("end_warmup", "mp_host")]
		public static string CommandEndWarmup(List<string> strings)
		{
			if (Mission.Current == null)
			{
				return "end_warmup can only be called within a mission.";
			}
			if (!GameNetwork.IsServer)
			{
				return "end_warmup can only be called by the server.";
			}
			MultiplayerWarmupComponent missionBehavior = Mission.Current.GetMissionBehavior<MultiplayerWarmupComponent>();
			if (missionBehavior == null)
			{
				return "end_warmup can only be called when the game is in warmup.";
			}
			missionBehavior.EndWarmupProgress();
			return "Success";
		}

		// Token: 0x04000F92 RID: 3986
		public const int RespawnPeriodInWarmup = 3;

		// Token: 0x04000F93 RID: 3987
		public const int WarmupEndWaitTime = 30;

		// Token: 0x04000F96 RID: 3990
		private MissionMultiplayerGameModeBase _gameMode;

		// Token: 0x04000F97 RID: 3991
		private MultiplayerTimerComponent _timerComponent;

		// Token: 0x04000F98 RID: 3992
		private MissionLobbyComponent _lobbyComponent;

		// Token: 0x04000F99 RID: 3993
		private MissionTime _currentStateStartTime;

		// Token: 0x04000F9A RID: 3994
		private MultiplayerWarmupComponent.WarmupStates _warmupState;

		// Token: 0x020005AC RID: 1452
		public enum WarmupStates
		{
			// Token: 0x04001EDA RID: 7898
			WaitingForPlayers,
			// Token: 0x04001EDB RID: 7899
			InProgress,
			// Token: 0x04001EDC RID: 7900
			Ending,
			// Token: 0x04001EDD RID: 7901
			Ended
		}
	}
}
