using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002AD RID: 685
	public class MissionScoreboardComponent : MissionNetwork
	{
		// Token: 0x1400004F RID: 79
		// (add) Token: 0x06002650 RID: 9808 RVA: 0x0008D6D0 File Offset: 0x0008B8D0
		// (remove) Token: 0x06002651 RID: 9809 RVA: 0x0008D708 File Offset: 0x0008B908
		public event Action OnRoundPropertiesChanged;

		// Token: 0x14000050 RID: 80
		// (add) Token: 0x06002652 RID: 9810 RVA: 0x0008D740 File Offset: 0x0008B940
		// (remove) Token: 0x06002653 RID: 9811 RVA: 0x0008D778 File Offset: 0x0008B978
		public event Action<BattleSideEnum> OnBotPropertiesChanged;

		// Token: 0x14000051 RID: 81
		// (add) Token: 0x06002654 RID: 9812 RVA: 0x0008D7B0 File Offset: 0x0008B9B0
		// (remove) Token: 0x06002655 RID: 9813 RVA: 0x0008D7E8 File Offset: 0x0008B9E8
		public event Action<Team, Team, MissionPeer> OnPlayerSideChanged;

		// Token: 0x14000052 RID: 82
		// (add) Token: 0x06002656 RID: 9814 RVA: 0x0008D820 File Offset: 0x0008BA20
		// (remove) Token: 0x06002657 RID: 9815 RVA: 0x0008D858 File Offset: 0x0008BA58
		public event Action<BattleSideEnum, MissionPeer> OnPlayerPropertiesChanged;

		// Token: 0x14000053 RID: 83
		// (add) Token: 0x06002658 RID: 9816 RVA: 0x0008D890 File Offset: 0x0008BA90
		// (remove) Token: 0x06002659 RID: 9817 RVA: 0x0008D8C8 File Offset: 0x0008BAC8
		public event Action<MissionPeer, int> OnMVPSelected;

		// Token: 0x14000054 RID: 84
		// (add) Token: 0x0600265A RID: 9818 RVA: 0x0008D900 File Offset: 0x0008BB00
		// (remove) Token: 0x0600265B RID: 9819 RVA: 0x0008D938 File Offset: 0x0008BB38
		public event Action OnScoreboardInitialized;

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x0600265C RID: 9820 RVA: 0x0008D96D File Offset: 0x0008BB6D
		public bool IsOneSided
		{
			get
			{
				return this._scoreboardSides == MissionScoreboardComponent.ScoreboardSides.OneSide;
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x0600265D RID: 9821 RVA: 0x0008D978 File Offset: 0x0008BB78
		public BattleSideEnum RoundWinner
		{
			get
			{
				IRoundComponent roundComponent = this._mpGameModeBase.RoundComponent;
				if (roundComponent == null)
				{
					return BattleSideEnum.None;
				}
				return roundComponent.RoundWinner;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x0600265E RID: 9822 RVA: 0x0008D990 File Offset: 0x0008BB90
		public MissionScoreboardComponent.ScoreboardHeader[] Headers
		{
			get
			{
				return this._scoreboardData.GetScoreboardHeaders();
			}
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x0008D99D File Offset: 0x0008BB9D
		public MissionScoreboardComponent(IScoreboardData scoreboardData)
		{
			this._scoreboardData = scoreboardData;
			this._spectators = new List<MissionPeer>();
			this._sides = new MissionScoreboardComponent.MissionScoreboardSide[2];
			this._roundWinnerList = new List<BattleSideEnum>();
			this._mvpCountPerPeer = new List<ValueTuple<MissionPeer, int>>();
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06002660 RID: 9824 RVA: 0x0008D9D9 File Offset: 0x0008BBD9
		public IEnumerable<BattleSideEnum> RoundWinnerList
		{
			get
			{
				return this._roundWinnerList.AsReadOnly();
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06002661 RID: 9825 RVA: 0x0008D9E6 File Offset: 0x0008BBE6
		public MissionScoreboardComponent.MissionScoreboardSide[] Sides
		{
			get
			{
				return this._sides;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06002662 RID: 9826 RVA: 0x0008D9EE File Offset: 0x0008BBEE
		public List<MissionPeer> Spectators
		{
			get
			{
				return this._spectators;
			}
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x0008D9F8 File Offset: 0x0008BBF8
		public override void AfterStart()
		{
			this._spectators.Clear();
			this._missionLobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this._missionNetworkComponent = base.Mission.GetMissionBehavior<MissionNetworkComponent>();
			this._mpGameModeBase = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			if (this._missionLobbyComponent.MissionType == MultiplayerGameType.Duel)
			{
				this._scoreboardSides = MissionScoreboardComponent.ScoreboardSides.OneSide;
			}
			else
			{
				this._scoreboardSides = MissionScoreboardComponent.ScoreboardSides.TwoSides;
			}
			MissionPeer.OnTeamChanged += this.TeamChange;
			this._missionNetworkComponent.OnMyClientSynchronized += this.OnMyClientSynchronized;
			if (GameNetwork.IsServerOrRecorder && this._mpGameModeBase.RoundComponent != null)
			{
				this._mpGameModeBase.RoundComponent.OnRoundEnding += this.OnRoundEnding;
				this._mpGameModeBase.RoundComponent.OnPreRoundEnding += this.OnPreRoundEnding;
			}
			this.LateInitScoreboard();
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x0008DADB File Offset: 0x0008BCDB
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClient)
			{
				registerer.RegisterBaseHandler<UpdateRoundScores>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerUpdateRoundScoresMessage));
				registerer.RegisterBaseHandler<SetRoundMVP>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerSetRoundMVP));
				registerer.RegisterBaseHandler<BotData>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventBotDataMessage));
			}
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x0008DB1C File Offset: 0x0008BD1C
		public override void OnRemoveBehavior()
		{
			this._spectators.Clear();
			for (int i = 0; i < 2; i++)
			{
				if (this._sides[i] != null)
				{
					this._sides[i].Clear();
				}
			}
			MissionPeer.OnTeamChanged -= this.TeamChange;
			if (this._missionNetworkComponent != null)
			{
				this._missionNetworkComponent.OnMyClientSynchronized -= this.OnMyClientSynchronized;
			}
			if (GameNetwork.IsServerOrRecorder && this._mpGameModeBase.RoundComponent != null)
			{
				this._mpGameModeBase.RoundComponent.OnRoundEnding -= this.OnRoundEnding;
			}
			base.OnRemoveBehavior();
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x0008DBC0 File Offset: 0x0008BDC0
		public void ResetBotScores()
		{
			foreach (MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide in this._sides)
			{
				if (((missionScoreboardSide != null) ? missionScoreboardSide.BotScores : null) != null)
				{
					missionScoreboardSide.BotScores.ResetKillDeathAssist();
				}
			}
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x0008DC00 File Offset: 0x0008BE00
		public void ChangeTeamScore(Team team, int scoreChange)
		{
			MissionScoreboardComponent.MissionScoreboardSide sideSafe = this.GetSideSafe(team.Side);
			sideSafe.SideScore += scoreChange;
			sideSafe.SideScore = MBMath.ClampInt(sideSafe.SideScore, -1023000, 1023000);
			if (GameNetwork.IsServer)
			{
				int num = ((this._scoreboardSides != MissionScoreboardComponent.ScoreboardSides.OneSide) ? this._sides[0].SideScore : 0);
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new UpdateRoundScores(this._sides[1].SideScore, num));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
			if (this.OnRoundPropertiesChanged != null)
			{
				this.OnRoundPropertiesChanged();
			}
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x0008DC98 File Offset: 0x0008BE98
		private void UpdateRoundScores()
		{
			foreach (MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide in this._sides)
			{
				if (missionScoreboardSide != null && missionScoreboardSide.Side == this.RoundWinner)
				{
					this._roundWinnerList.Add(this.RoundWinner);
					if (this.RoundWinner != BattleSideEnum.None)
					{
						this._sides[(int)this.RoundWinner].SideScore++;
					}
				}
			}
			if (this.OnRoundPropertiesChanged != null)
			{
				this.OnRoundPropertiesChanged();
			}
			if (GameNetwork.IsServer)
			{
				int num = ((this._scoreboardSides != MissionScoreboardComponent.ScoreboardSides.OneSide) ? this._sides[0].SideScore : 0);
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new UpdateRoundScores(this._sides[1].SideScore, num));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x0008DD5A File Offset: 0x0008BF5A
		public MissionScoreboardComponent.MissionScoreboardSide GetSideSafe(BattleSideEnum battleSide)
		{
			if (this._scoreboardSides == MissionScoreboardComponent.ScoreboardSides.OneSide)
			{
				return this._sides[1];
			}
			return this._sides[(int)battleSide];
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x0008DD75 File Offset: 0x0008BF75
		public int GetRoundScore(BattleSideEnum side)
		{
			if (side > (BattleSideEnum)this._sides.Length || side < BattleSideEnum.Defender)
			{
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MissionScoreboardComponent.cs", "GetRoundScore", 462);
				return 0;
			}
			return this.GetSideSafe(side).SideScore;
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x0008DDB0 File Offset: 0x0008BFB0
		public void HandleServerUpdateRoundScoresMessage(GameNetworkMessage baseMessage)
		{
			UpdateRoundScores updateRoundScores = (UpdateRoundScores)baseMessage;
			this._sides[1].SideScore = updateRoundScores.AttackerTeamScore;
			if (this._scoreboardSides != MissionScoreboardComponent.ScoreboardSides.OneSide)
			{
				this._sides[0].SideScore = updateRoundScores.DefenderTeamScore;
			}
			if (this.OnRoundPropertiesChanged != null)
			{
				this.OnRoundPropertiesChanged();
			}
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x0008DE08 File Offset: 0x0008C008
		public void HandleServerSetRoundMVP(GameNetworkMessage baseMessage)
		{
			SetRoundMVP setRoundMVP = (SetRoundMVP)baseMessage;
			Action<MissionPeer, int> onMVPSelected = this.OnMVPSelected;
			if (onMVPSelected != null)
			{
				onMVPSelected(setRoundMVP.MVPPeer.GetComponent<MissionPeer>(), setRoundMVP.MVPCount);
			}
			this.PlayerPropertiesChanged(setRoundMVP.MVPPeer);
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x0008DE4C File Offset: 0x0008C04C
		public void CalculateTotalNumbers()
		{
			foreach (MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide in this._sides)
			{
				if (missionScoreboardSide != null)
				{
					int num = missionScoreboardSide.BotScores.DeathCount;
					int num2 = missionScoreboardSide.BotScores.AssistCount;
					int num3 = missionScoreboardSide.BotScores.KillCount;
					foreach (MissionPeer missionPeer in missionScoreboardSide.Players)
					{
						num2 += missionPeer.AssistCount;
						num += missionPeer.DeathCount;
						num3 += missionPeer.KillCount;
					}
				}
			}
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x0008DF04 File Offset: 0x0008C104
		private void TeamChange(NetworkCommunicator player, Team oldTeam, Team nextTeam)
		{
			if (oldTeam == null && GameNetwork.VirtualPlayers[player.VirtualPlayer.Index] != player.VirtualPlayer)
			{
				Debug.Print("Ignoring team change call for {}, dced peer.", 0, Debug.DebugColor.White, 17179869184UL);
				return;
			}
			MissionPeer component = player.GetComponent<MissionPeer>();
			if (oldTeam != null)
			{
				if (oldTeam == base.Mission.SpectatorTeam)
				{
					this._spectators.Remove(component);
				}
				else
				{
					this.GetSideSafe(oldTeam.Side).RemovePlayer(component);
				}
			}
			if (nextTeam != null)
			{
				if (nextTeam == base.Mission.SpectatorTeam)
				{
					this._spectators.Add(component);
				}
				else
				{
					Debug.Print(string.Format(">SBC => {0} is switching from {1} to {2}. Adding to scoreboard side {3}.", new object[]
					{
						player.UserName,
						(oldTeam == null) ? "NULL" : oldTeam.Side.ToString(),
						nextTeam.Side.ToString(),
						nextTeam.Side
					}), 0, Debug.DebugColor.Blue, 17179869184UL);
					this.GetSideSafe(nextTeam.Side).AddPlayer(component);
				}
			}
			if (this.OnPlayerSideChanged != null)
			{
				this.OnPlayerSideChanged(oldTeam, nextTeam, component);
			}
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x0008E03C File Offset: 0x0008C23C
		public override void OnClearScene()
		{
			if (this._mpGameModeBase.RoundComponent == null && GameNetwork.IsServer)
			{
				this.ClearSideScores();
			}
			foreach (MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide in this.Sides)
			{
				if (missionScoreboardSide != null)
				{
					missionScoreboardSide.BotScores.AliveCount = 0;
				}
			}
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x0008E08C File Offset: 0x0008C28C
		public override void OnPlayerConnectedToServer(NetworkCommunicator networkPeer)
		{
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			if (component != null && component.Team != null)
			{
				this.TeamChange(networkPeer, null, component.Team);
			}
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x0008E0BC File Offset: 0x0008C2BC
		public override void OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer)
		{
			MissionPeer missionPeer = networkPeer.GetComponent<MissionPeer>();
			if (missionPeer != null)
			{
				bool flag = this._spectators.Contains(missionPeer);
				bool flag2 = this._sides.Any<MissionScoreboardComponent.MissionScoreboardSide>((MissionScoreboardComponent.MissionScoreboardSide x) => x != null && x.Players.Contains(missionPeer));
				if (flag)
				{
					this._spectators.Remove(missionPeer);
					return;
				}
				if (flag2)
				{
					this.GetSideSafe(missionPeer.Team.Side).RemovePlayer(missionPeer);
					Formation controlledFormation = missionPeer.ControlledFormation;
					if (controlledFormation != null)
					{
						Team team = missionPeer.Team;
						BotData botScores = this.Sides[(int)team.Side].BotScores;
						botScores.AliveCount += controlledFormation.GetCountOfUnitsWithCondition((Agent agent) => agent.IsActive());
						this.BotPropertiesChanged(team.Side);
					}
					Action<Team, Team, MissionPeer> onPlayerSideChanged = this.OnPlayerSideChanged;
					if (onPlayerSideChanged == null)
					{
						return;
					}
					onPlayerSideChanged(missionPeer.Team, null, missionPeer);
				}
			}
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x0008E1D7 File Offset: 0x0008C3D7
		private void BotsControlledChanged(NetworkCommunicator peer)
		{
			this.PlayerPropertiesChanged(peer);
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x0008E1E0 File Offset: 0x0008C3E0
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			if (agent.IsActive() && !agent.IsMount)
			{
				if (agent.MissionPeer == null)
				{
					this.BotPropertiesChanged(agent.Team.Side);
					return;
				}
				if (agent.MissionPeer != null)
				{
					this.PlayerPropertiesChanged(agent.MissionPeer.GetNetworkPeer());
				}
			}
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x0008E230 File Offset: 0x0008C430
		public override void OnAssignPlayerAsSergeantOfFormation(Agent agent)
		{
			if (agent.MissionPeer != null)
			{
				this.PlayerPropertiesChanged(agent.MissionPeer.GetNetworkPeer());
			}
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x0008E24B File Offset: 0x0008C44B
		public void BotPropertiesChanged(BattleSideEnum side)
		{
			if (this.OnBotPropertiesChanged != null)
			{
				this.OnBotPropertiesChanged(side);
			}
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x0008E264 File Offset: 0x0008C464
		public void PlayerPropertiesChanged(NetworkCommunicator player)
		{
			if (GameNetwork.IsDedicatedServer)
			{
				return;
			}
			MissionPeer component = player.GetComponent<MissionPeer>();
			if (component != null)
			{
				this.PlayerPropertiesChanged(component);
			}
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x0008E28C File Offset: 0x0008C48C
		public void PlayerPropertiesChanged(MissionPeer player)
		{
			if (GameNetwork.IsDedicatedServer)
			{
				return;
			}
			this.CalculateTotalNumbers();
			if (this.OnPlayerPropertiesChanged != null && player.Team != null && player.Team != Mission.Current.SpectatorTeam)
			{
				BattleSideEnum side = player.Team.Side;
				this.OnPlayerPropertiesChanged(side, player);
			}
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x0008E2E4 File Offset: 0x0008C4E4
		protected override void HandleLateNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			networkPeer.GetComponent<MissionPeer>();
			foreach (MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide in this._sides)
			{
				if (missionScoreboardSide != null && !networkPeer.IsServerPeer)
				{
					if (missionScoreboardSide.BotScores.IsAnyValid)
					{
						GameNetwork.BeginModuleEventAsServer(networkPeer);
						GameNetwork.WriteMessage(new BotData(missionScoreboardSide.Side, missionScoreboardSide.BotScores.KillCount, missionScoreboardSide.BotScores.AssistCount, missionScoreboardSide.BotScores.DeathCount, missionScoreboardSide.BotScores.AliveCount));
						GameNetwork.EndModuleEventAsServer();
					}
					if (this._mpGameModeBase != null)
					{
						int num = ((this._scoreboardSides != MissionScoreboardComponent.ScoreboardSides.OneSide) ? this._sides[0].SideScore : 0);
						GameNetwork.BeginModuleEventAsServer(networkPeer);
						GameNetwork.WriteMessage(new UpdateRoundScores(this._sides[1].SideScore, num));
						GameNetwork.EndModuleEventAsServer();
					}
				}
			}
			if (!networkPeer.IsServerPeer && this._mvpCountPerPeer != null)
			{
				foreach (ValueTuple<MissionPeer, int> valueTuple in this._mvpCountPerPeer)
				{
					GameNetwork.BeginModuleEventAsServer(networkPeer);
					GameNetwork.WriteMessage(new SetRoundMVP(valueTuple.Item1.GetNetworkPeer(), valueTuple.Item2));
					GameNetwork.EndModuleEventAsServer();
				}
			}
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x0008E43C File Offset: 0x0008C63C
		public void HandleServerEventBotDataMessage(GameNetworkMessage baseMessage)
		{
			BotData botData = (BotData)baseMessage;
			MissionScoreboardComponent.MissionScoreboardSide sideSafe = this.GetSideSafe(botData.Side);
			sideSafe.BotScores.KillCount = botData.KillCount;
			sideSafe.BotScores.AssistCount = botData.AssistCount;
			sideSafe.BotScores.DeathCount = botData.DeathCount;
			sideSafe.BotScores.AliveCount = botData.AliveBotCount;
			this.BotPropertiesChanged(botData.Side);
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x0008E4AC File Offset: 0x0008C6AC
		private void ClearSideScores()
		{
			this._sides[1].SideScore = 0;
			if (this._scoreboardSides == MissionScoreboardComponent.ScoreboardSides.TwoSides)
			{
				this._sides[0].SideScore = 0;
			}
			if (GameNetwork.IsServer)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new UpdateRoundScores(0, 0));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
			if (this.OnRoundPropertiesChanged != null)
			{
				this.OnRoundPropertiesChanged();
			}
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x0008E510 File Offset: 0x0008C710
		public void OnRoundEnding()
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				this.UpdateRoundScores();
			}
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x0008E51F File Offset: 0x0008C71F
		private void OnMyClientSynchronized()
		{
			this.LateInitializeHeaders();
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x0008E528 File Offset: 0x0008C728
		private void LateInitScoreboard()
		{
			MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide = new MissionScoreboardComponent.MissionScoreboardSide(BattleSideEnum.Attacker);
			this._sides[1] = missionScoreboardSide;
			this._sides[1].BotScores = new BotData();
			if (this._scoreboardSides == MissionScoreboardComponent.ScoreboardSides.TwoSides)
			{
				MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide2 = new MissionScoreboardComponent.MissionScoreboardSide(BattleSideEnum.Defender);
				this._sides[0] = missionScoreboardSide2;
				this._sides[0].BotScores = new BotData();
			}
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x0008E584 File Offset: 0x0008C784
		private void LateInitializeHeaders()
		{
			if (this._isInitialized)
			{
				return;
			}
			this._isInitialized = true;
			foreach (MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide in this._sides)
			{
				if (missionScoreboardSide != null)
				{
					missionScoreboardSide.UpdateHeader(this.Headers);
				}
			}
			if (this.OnScoreboardInitialized != null)
			{
				this.OnScoreboardInitialized();
			}
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x0008E5DC File Offset: 0x0008C7DC
		public void OnMultiplayerGameClientBehaviorInitialized(ref Action<NetworkCommunicator> onBotsControlledChanged)
		{
			onBotsControlledChanged = (Action<NetworkCommunicator>)Delegate.Combine(onBotsControlledChanged, new Action<NetworkCommunicator>(this.BotsControlledChanged));
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x0008E5F8 File Offset: 0x0008C7F8
		public BattleSideEnum GetMatchWinnerSide()
		{
			List<int> scores = new List<int>();
			KeyValuePair<BattleSideEnum, int> keyValuePair = new KeyValuePair<BattleSideEnum, int>(BattleSideEnum.None, -1);
			for (int i = 0; i < 2; i++)
			{
				BattleSideEnum battleSideEnum = (BattleSideEnum)i;
				MissionScoreboardComponent.MissionScoreboardSide sideSafe = this.GetSideSafe(battleSideEnum);
				if (sideSafe.SideScore > keyValuePair.Value && sideSafe.CurrentPlayerCount > 0)
				{
					keyValuePair = new KeyValuePair<BattleSideEnum, int>(battleSideEnum, sideSafe.SideScore);
				}
				scores.Add(sideSafe.SideScore);
			}
			if (!scores.IsEmpty<int>() && scores.All<int>((int s) => s == scores[0]))
			{
				return BattleSideEnum.None;
			}
			return keyValuePair.Key;
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x0008E6A0 File Offset: 0x0008C8A0
		private void OnPreRoundEnding()
		{
			if (GameNetwork.IsServer)
			{
				KeyValuePair<MissionPeer, int> keyValuePair2;
				KeyValuePair<MissionPeer, int> keyValuePair4;
				foreach (MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide in this.Sides)
				{
					if (missionScoreboardSide.Side == BattleSideEnum.Attacker)
					{
						KeyValuePair<MissionPeer, int> keyValuePair = missionScoreboardSide.CalculateAndGetMVPScoreWithPeer();
						if (keyValuePair2.Key == null || keyValuePair2.Value < keyValuePair.Value)
						{
							keyValuePair2 = keyValuePair;
						}
					}
					else if (missionScoreboardSide.Side == BattleSideEnum.Defender)
					{
						KeyValuePair<MissionPeer, int> keyValuePair3 = missionScoreboardSide.CalculateAndGetMVPScoreWithPeer();
						if (keyValuePair4.Key == null || keyValuePair4.Value < keyValuePair3.Value)
						{
							keyValuePair4 = keyValuePair3;
						}
					}
				}
				if (keyValuePair2.Key != null)
				{
					this.SetPeerAsMVP(keyValuePair2.Key);
				}
				if (keyValuePair4.Key != null)
				{
					this.SetPeerAsMVP(keyValuePair4.Key);
				}
			}
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x0008E75C File Offset: 0x0008C95C
		private void SetPeerAsMVP(MissionPeer peer)
		{
			int num = -1;
			for (int i = 0; i < this._mvpCountPerPeer.Count; i++)
			{
				if (peer == this._mvpCountPerPeer[i].Item1)
				{
					num = i;
					break;
				}
			}
			int num2 = 1;
			if (num != -1)
			{
				num2 = this._mvpCountPerPeer[num].Item2 + 1;
				this._mvpCountPerPeer.RemoveAt(num);
			}
			this._mvpCountPerPeer.Add(new ValueTuple<MissionPeer, int>(peer, num2));
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new SetRoundMVP(peer.GetNetworkPeer(), num2));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			Action<MissionPeer, int> onMVPSelected = this.OnMVPSelected;
			if (onMVPSelected == null)
			{
				return;
			}
			onMVPSelected(peer, num2);
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x0008E800 File Offset: 0x0008CA00
		public override void OnScoreHit(Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon, bool isBlocked, bool isSiegeEngineHit, in Blow blow, in AttackCollisionData collisionData, float damagedHp, float hitDistance, float shotDifficulty)
		{
			if (affectorAgent != null && GameNetwork.IsServer && !isBlocked && damagedHp > 0f)
			{
				if (affectorAgent.IsMount)
				{
					affectorAgent = affectorAgent.RiderAgent;
				}
				if (affectorAgent != null)
				{
					MissionPeer missionPeer = affectorAgent.MissionPeer ?? ((affectorAgent.IsAIControlled && affectorAgent.OwningAgentMissionPeer != null) ? affectorAgent.OwningAgentMissionPeer : null);
					if (missionPeer != null)
					{
						int num = (int)damagedHp;
						if (affectedAgent.IsMount)
						{
							num = (int)(damagedHp * 0.35f);
							affectedAgent = affectedAgent.RiderAgent;
						}
						if (affectedAgent != null && affectorAgent != affectedAgent)
						{
							if (!affectorAgent.IsFriendOf(affectedAgent))
							{
								missionPeer.Score += num;
							}
							else
							{
								missionPeer.Score -= (int)((float)num * 1.5f);
							}
							GameNetwork.BeginBroadcastModuleEvent();
							GameNetwork.WriteMessage(new KillDeathCountChange(missionPeer.GetNetworkPeer(), null, missionPeer.KillCount, missionPeer.AssistCount, missionPeer.DeathCount, missionPeer.Score));
							GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
						}
					}
				}
			}
		}

		// Token: 0x04000E8D RID: 3725
		private const int TotalSideCount = 2;

		// Token: 0x04000E8E RID: 3726
		private MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x04000E8F RID: 3727
		private MissionNetworkComponent _missionNetworkComponent;

		// Token: 0x04000E90 RID: 3728
		private MissionMultiplayerGameModeBaseClient _mpGameModeBase;

		// Token: 0x04000E91 RID: 3729
		private IScoreboardData _scoreboardData;

		// Token: 0x04000E98 RID: 3736
		private List<MissionPeer> _spectators;

		// Token: 0x04000E99 RID: 3737
		private MissionScoreboardComponent.MissionScoreboardSide[] _sides;

		// Token: 0x04000E9A RID: 3738
		private bool _isInitialized;

		// Token: 0x04000E9B RID: 3739
		private List<BattleSideEnum> _roundWinnerList;

		// Token: 0x04000E9C RID: 3740
		private MissionScoreboardComponent.ScoreboardSides _scoreboardSides;

		// Token: 0x04000E9D RID: 3741
		private List<ValueTuple<MissionPeer, int>> _mvpCountPerPeer;

		// Token: 0x02000585 RID: 1413
		private enum ScoreboardSides
		{
			// Token: 0x04001E61 RID: 7777
			OneSide,
			// Token: 0x04001E62 RID: 7778
			TwoSides
		}

		// Token: 0x02000586 RID: 1414
		public struct ScoreboardHeader
		{
			// Token: 0x06003D5B RID: 15707 RVA: 0x000F2F67 File Offset: 0x000F1167
			public ScoreboardHeader(string id, Func<MissionPeer, string> playerGetterFunc, Func<BotData, string> botGetterFunc)
			{
				this.Id = id;
				this.Name = GameTexts.FindText("str_scoreboard_header", id);
				this._playerGetterFunc = playerGetterFunc;
				this._botGetterFunc = botGetterFunc;
			}

			// Token: 0x06003D5C RID: 15708 RVA: 0x000F2F90 File Offset: 0x000F1190
			public string GetValueOf(MissionPeer missionPeer)
			{
				if (missionPeer == null || this._playerGetterFunc == null)
				{
					string text = "Scoreboard header values are invalid: Peer: ";
					string text2 = ((missionPeer != null) ? missionPeer.ToString() : null) ?? "NULL";
					string text3 = " Getter: ";
					Func<MissionPeer, string> playerGetterFunc = this._playerGetterFunc;
					Debug.FailedAssert(text + text2 + text3 + (((playerGetterFunc != null) ? playerGetterFunc.ToString() : null) ?? "NULL"), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MissionScoreboardComponent.cs", "GetValueOf", 43);
					return string.Empty;
				}
				string text4;
				try
				{
					text4 = this._playerGetterFunc(missionPeer);
				}
				catch (Exception ex)
				{
					Debug.FailedAssert(string.Format("An error occured while trying to get scoreboard value ({0}) for peer: {1}. Exception: {2}", this.Id, missionPeer.Name, ex.InnerException), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MissionScoreboardComponent.cs", "GetValueOf", 53);
					text4 = string.Empty;
				}
				return text4;
			}

			// Token: 0x06003D5D RID: 15709 RVA: 0x000F3058 File Offset: 0x000F1258
			public string GetValueOf(BotData botData)
			{
				if (botData == null || this._botGetterFunc == null)
				{
					string text = "Scoreboard header values are invalid: Bot Data: ";
					string text2 = ((botData != null) ? botData.ToString() : null) ?? "NULL";
					string text3 = " Getter: ";
					Func<BotData, string> botGetterFunc = this._botGetterFunc;
					Debug.FailedAssert(text + text2 + text3 + (((botGetterFunc != null) ? botGetterFunc.ToString() : null) ?? "NULL"), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MissionScoreboardComponent.cs", "GetValueOf", 62);
					return string.Empty;
				}
				string text4;
				try
				{
					text4 = this._botGetterFunc(botData);
				}
				catch (Exception ex)
				{
					Debug.FailedAssert(string.Format("An error occured while trying to get scoreboard value ({0}) for a bot. Exception: {1}", this.Id, ex.InnerException), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MissionScoreboardComponent.cs", "GetValueOf", 72);
					text4 = string.Empty;
				}
				return text4;
			}

			// Token: 0x04001E63 RID: 7779
			private readonly Func<MissionPeer, string> _playerGetterFunc;

			// Token: 0x04001E64 RID: 7780
			private readonly Func<BotData, string> _botGetterFunc;

			// Token: 0x04001E65 RID: 7781
			public readonly string Id;

			// Token: 0x04001E66 RID: 7782
			public readonly TextObject Name;
		}

		// Token: 0x02000587 RID: 1415
		public class MissionScoreboardSide
		{
			// Token: 0x17000A68 RID: 2664
			// (get) Token: 0x06003D5E RID: 15710 RVA: 0x000F3118 File Offset: 0x000F1318
			public int CurrentPlayerCount
			{
				get
				{
					return this._players.Count;
				}
			}

			// Token: 0x17000A69 RID: 2665
			// (get) Token: 0x06003D5F RID: 15711 RVA: 0x000F3125 File Offset: 0x000F1325
			public IEnumerable<MissionPeer> Players
			{
				get
				{
					return this._players;
				}
			}

			// Token: 0x06003D60 RID: 15712 RVA: 0x000F312D File Offset: 0x000F132D
			public MissionScoreboardSide(BattleSideEnum side)
			{
				this.Side = side;
				this._players = new List<MissionPeer>();
				this._playerLastRoundScoreMap = new List<int>();
			}

			// Token: 0x06003D61 RID: 15713 RVA: 0x000F3152 File Offset: 0x000F1352
			public void AddPlayer(MissionPeer peer)
			{
				if (!this._players.Contains(peer))
				{
					this._players.Add(peer);
					this._playerLastRoundScoreMap.Add(0);
				}
			}

			// Token: 0x06003D62 RID: 15714 RVA: 0x000F317C File Offset: 0x000F137C
			public void RemovePlayer(MissionPeer peer)
			{
				for (int i = 0; i < this._players.Count; i++)
				{
					if (this._players[i] == peer)
					{
						this._players.RemoveAt(i);
						this._playerLastRoundScoreMap.RemoveAt(i);
						return;
					}
				}
			}

			// Token: 0x06003D63 RID: 15715 RVA: 0x000F31C8 File Offset: 0x000F13C8
			public string[] GetValuesOf(MissionPeer peer)
			{
				if (this._properties == null)
				{
					return new string[0];
				}
				string[] array = new string[this._properties.Length];
				if (peer == null)
				{
					for (int i = 0; i < this._properties.Length; i++)
					{
						array[i] = this._properties[i].GetValueOf(this.BotScores);
					}
					return array;
				}
				for (int j = 0; j < this._properties.Length; j++)
				{
					array[j] = this._properties[j].GetValueOf(peer);
				}
				return array;
			}

			// Token: 0x06003D64 RID: 15716 RVA: 0x000F3250 File Offset: 0x000F1450
			public string[] GetHeaderNames()
			{
				if (this._properties == null)
				{
					return new string[0];
				}
				string[] array = new string[this._properties.Length];
				for (int i = 0; i < this._properties.Length; i++)
				{
					array[i] = this._properties[i].Name.ToString();
				}
				return array;
			}

			// Token: 0x06003D65 RID: 15717 RVA: 0x000F32A8 File Offset: 0x000F14A8
			public string[] GetHeaderIds()
			{
				if (this._properties == null)
				{
					return new string[0];
				}
				string[] array = new string[this._properties.Length];
				for (int i = 0; i < this._properties.Length; i++)
				{
					array[i] = this._properties[i].Id;
				}
				return array;
			}

			// Token: 0x06003D66 RID: 15718 RVA: 0x000F32FC File Offset: 0x000F14FC
			public int GetScore(MissionPeer peer)
			{
				if (this._properties == null)
				{
					return 0;
				}
				string text;
				if (peer == null)
				{
					if (this._properties.Any<MissionScoreboardComponent.ScoreboardHeader>((MissionScoreboardComponent.ScoreboardHeader p) => p.Id == "score"))
					{
						text = this._properties.FirstOrDefault<MissionScoreboardComponent.ScoreboardHeader>((MissionScoreboardComponent.ScoreboardHeader x) => x.Id == "score").GetValueOf(this.BotScores);
					}
					else
					{
						text = string.Empty;
					}
				}
				else if (this._properties.Any<MissionScoreboardComponent.ScoreboardHeader>((MissionScoreboardComponent.ScoreboardHeader p) => p.Id == "score"))
				{
					text = this._properties.Single<MissionScoreboardComponent.ScoreboardHeader>((MissionScoreboardComponent.ScoreboardHeader x) => x.Id == "score").GetValueOf(peer);
				}
				else
				{
					text = string.Empty;
				}
				int num = 0;
				int.TryParse(text, out num);
				return num;
			}

			// Token: 0x06003D67 RID: 15719 RVA: 0x000F33F9 File Offset: 0x000F15F9
			public void UpdateHeader(MissionScoreboardComponent.ScoreboardHeader[] headers)
			{
				this._properties = headers;
			}

			// Token: 0x06003D68 RID: 15720 RVA: 0x000F3402 File Offset: 0x000F1602
			public void Clear()
			{
				this._players.Clear();
			}

			// Token: 0x06003D69 RID: 15721 RVA: 0x000F3410 File Offset: 0x000F1610
			public KeyValuePair<MissionPeer, int> CalculateAndGetMVPScoreWithPeer()
			{
				KeyValuePair<MissionPeer, int> keyValuePair = default(KeyValuePair<MissionPeer, int>);
				for (int i = 0; i < this._players.Count; i++)
				{
					int num = this._players[i].Score - this._playerLastRoundScoreMap[i];
					this._playerLastRoundScoreMap[i] = this._players[i].Score;
					if (keyValuePair.Key == null || keyValuePair.Value < num)
					{
						keyValuePair = new KeyValuePair<MissionPeer, int>(this._players[i], num);
					}
				}
				return keyValuePair;
			}

			// Token: 0x04001E67 RID: 7783
			public readonly BattleSideEnum Side;

			// Token: 0x04001E68 RID: 7784
			private MissionScoreboardComponent.ScoreboardHeader[] _properties;

			// Token: 0x04001E69 RID: 7785
			public BotData BotScores;

			// Token: 0x04001E6A RID: 7786
			public int SideScore;

			// Token: 0x04001E6B RID: 7787
			private List<MissionPeer> _players;

			// Token: 0x04001E6C RID: 7788
			private List<int> _playerLastRoundScoreMap;
		}
	}
}
