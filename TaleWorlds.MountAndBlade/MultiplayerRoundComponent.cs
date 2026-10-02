using System;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002BD RID: 701
	public class MultiplayerRoundComponent : MissionNetwork, IRoundComponent, IMissionBehavior
	{
		// Token: 0x14000067 RID: 103
		// (add) Token: 0x06002821 RID: 10273 RVA: 0x00098234 File Offset: 0x00096434
		// (remove) Token: 0x06002822 RID: 10274 RVA: 0x0009826C File Offset: 0x0009646C
		public event Action OnRoundStarted;

		// Token: 0x14000068 RID: 104
		// (add) Token: 0x06002823 RID: 10275 RVA: 0x000982A4 File Offset: 0x000964A4
		// (remove) Token: 0x06002824 RID: 10276 RVA: 0x000982DC File Offset: 0x000964DC
		public event Action OnPreparationEnded;

		// Token: 0x14000069 RID: 105
		// (add) Token: 0x06002825 RID: 10277 RVA: 0x00098314 File Offset: 0x00096514
		// (remove) Token: 0x06002826 RID: 10278 RVA: 0x0009834C File Offset: 0x0009654C
		public event Action OnPreRoundEnding;

		// Token: 0x1400006A RID: 106
		// (add) Token: 0x06002827 RID: 10279 RVA: 0x00098384 File Offset: 0x00096584
		// (remove) Token: 0x06002828 RID: 10280 RVA: 0x000983BC File Offset: 0x000965BC
		public event Action OnRoundEnding;

		// Token: 0x1400006B RID: 107
		// (add) Token: 0x06002829 RID: 10281 RVA: 0x000983F4 File Offset: 0x000965F4
		// (remove) Token: 0x0600282A RID: 10282 RVA: 0x0009842C File Offset: 0x0009662C
		public event Action OnPostRoundEnded;

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x0600282B RID: 10283 RVA: 0x00098464 File Offset: 0x00096664
		// (remove) Token: 0x0600282C RID: 10284 RVA: 0x0009849C File Offset: 0x0009669C
		public event Action OnCurrentRoundStateChanged;

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x0600282D RID: 10285 RVA: 0x000984D1 File Offset: 0x000966D1
		public float RemainingRoundTime
		{
			get
			{
				return this._gameModeClient.TimerComponent.GetRemainingTime(true);
			}
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x0600282E RID: 10286 RVA: 0x000984E4 File Offset: 0x000966E4
		// (set) Token: 0x0600282F RID: 10287 RVA: 0x000984EC File Offset: 0x000966EC
		public float LastRoundEndRemainingTime { get; private set; }

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06002830 RID: 10288 RVA: 0x000984F5 File Offset: 0x000966F5
		// (set) Token: 0x06002831 RID: 10289 RVA: 0x000984FD File Offset: 0x000966FD
		public MultiplayerRoundState CurrentRoundState { get; private set; }

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06002832 RID: 10290 RVA: 0x00098506 File Offset: 0x00096706
		// (set) Token: 0x06002833 RID: 10291 RVA: 0x0009850E File Offset: 0x0009670E
		public int RoundCount { get; private set; }

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06002834 RID: 10292 RVA: 0x00098517 File Offset: 0x00096717
		// (set) Token: 0x06002835 RID: 10293 RVA: 0x0009851F File Offset: 0x0009671F
		public BattleSideEnum RoundWinner { get; private set; }

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06002836 RID: 10294 RVA: 0x00098528 File Offset: 0x00096728
		// (set) Token: 0x06002837 RID: 10295 RVA: 0x00098530 File Offset: 0x00096730
		public RoundEndReason RoundEndReason { get; private set; }

		// Token: 0x06002838 RID: 10296 RVA: 0x00098539 File Offset: 0x00096739
		public override void AfterStart()
		{
			this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
			this._gameModeClient = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x00098552 File Offset: 0x00096752
		protected override void OnUdpNetworkHandlerClose()
		{
			this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x0009855C File Offset: 0x0009675C
		private void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode mode)
		{
			GameNetwork.NetworkMessageHandlerRegisterer networkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegisterer(mode);
			if (GameNetwork.IsClient)
			{
				networkMessageHandlerRegisterer.Register<RoundStateChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<RoundStateChange>(this.HandleServerEventChangeRoundState));
				networkMessageHandlerRegisterer.Register<RoundCountChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<RoundCountChange>(this.HandleServerEventRoundCountChange));
				networkMessageHandlerRegisterer.Register<RoundWinnerChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<RoundWinnerChange>(this.HandleServerEventRoundWinnerChange));
				networkMessageHandlerRegisterer.Register<RoundEndReasonChange>(new GameNetworkMessage.ServerMessageHandlerDelegate<RoundEndReasonChange>(this.HandleServerEventRoundEndReasonChange));
			}
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x000985C0 File Offset: 0x000967C0
		private void HandleServerEventChangeRoundState(RoundStateChange message)
		{
			if (this.CurrentRoundState == MultiplayerRoundState.InProgress)
			{
				this.LastRoundEndRemainingTime = (float)message.RemainingTimeOnPreviousState;
			}
			this.CurrentRoundState = message.RoundState;
			switch (this.CurrentRoundState)
			{
			case MultiplayerRoundState.Preparation:
				this._gameModeClient.TimerComponent.StartTimerAsClient(message.StateStartTimeInSeconds, (float)MultiplayerOptions.OptionType.RoundPreparationTimeLimit.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
				if (this.OnRoundStarted != null)
				{
					this.OnRoundStarted();
				}
				break;
			case MultiplayerRoundState.InProgress:
				this._gameModeClient.TimerComponent.StartTimerAsClient(message.StateStartTimeInSeconds, (float)MultiplayerOptions.OptionType.RoundTimeLimit.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
				if (this.OnPreparationEnded != null)
				{
					this.OnPreparationEnded();
				}
				break;
			case MultiplayerRoundState.Ending:
				this._gameModeClient.TimerComponent.StartTimerAsClient(message.StateStartTimeInSeconds, 3f);
				if (this.OnPreRoundEnding != null)
				{
					this.OnPreRoundEnding();
				}
				if (this.OnRoundEnding != null)
				{
					this.OnRoundEnding();
				}
				break;
			case MultiplayerRoundState.Ended:
				this._gameModeClient.TimerComponent.StartTimerAsClient(message.StateStartTimeInSeconds, 5f);
				if (this.OnPostRoundEnded != null)
				{
					this.OnPostRoundEnded();
				}
				break;
			case MultiplayerRoundState.MatchEnded:
				this._gameModeClient.TimerComponent.StartTimerAsClient(message.StateStartTimeInSeconds, 5f);
				break;
			}
			Action onCurrentRoundStateChanged = this.OnCurrentRoundStateChanged;
			if (onCurrentRoundStateChanged == null)
			{
				return;
			}
			onCurrentRoundStateChanged();
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x00098729 File Offset: 0x00096929
		private void HandleServerEventRoundCountChange(RoundCountChange message)
		{
			this.RoundCount = message.RoundCount;
		}

		// Token: 0x0600283D RID: 10301 RVA: 0x00098737 File Offset: 0x00096937
		private void HandleServerEventRoundWinnerChange(RoundWinnerChange message)
		{
			this.RoundWinner = message.RoundWinner;
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x00098745 File Offset: 0x00096945
		private void HandleServerEventRoundEndReasonChange(RoundEndReasonChange message)
		{
			this.RoundEndReason = message.RoundEndReason;
		}

		// Token: 0x04000F59 RID: 3929
		public const int RoundEndDelayTime = 3;

		// Token: 0x04000F5A RID: 3930
		public const int RoundEndWaitTime = 8;

		// Token: 0x04000F5B RID: 3931
		public const int MatchEndWaitTime = 5;

		// Token: 0x04000F5C RID: 3932
		public const int WarmupEndWaitTime = 30;

		// Token: 0x04000F63 RID: 3939
		private MissionMultiplayerGameModeBaseClient _gameModeClient;
	}
}
