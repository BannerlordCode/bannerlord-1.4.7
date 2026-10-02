using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002C1 RID: 705
	public class MultiplayerTeamSelectComponent : MissionNetwork
	{
		// Token: 0x14000073 RID: 115
		// (add) Token: 0x06002872 RID: 10354 RVA: 0x000991AC File Offset: 0x000973AC
		// (remove) Token: 0x06002873 RID: 10355 RVA: 0x000991E4 File Offset: 0x000973E4
		public event MultiplayerTeamSelectComponent.OnSelectingTeamDelegate OnSelectingTeam;

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x06002874 RID: 10356 RVA: 0x0009921C File Offset: 0x0009741C
		// (remove) Token: 0x06002875 RID: 10357 RVA: 0x00099254 File Offset: 0x00097454
		public event Action OnMyTeamChange;

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x06002876 RID: 10358 RVA: 0x0009928C File Offset: 0x0009748C
		// (remove) Token: 0x06002877 RID: 10359 RVA: 0x000992C4 File Offset: 0x000974C4
		public event Action OnUpdateTeams;

		// Token: 0x14000076 RID: 118
		// (add) Token: 0x06002878 RID: 10360 RVA: 0x000992FC File Offset: 0x000974FC
		// (remove) Token: 0x06002879 RID: 10361 RVA: 0x00099334 File Offset: 0x00097534
		public event Action OnUpdateFriendsPerTeam;

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x0600287A RID: 10362 RVA: 0x00099369 File Offset: 0x00097569
		// (set) Token: 0x0600287B RID: 10363 RVA: 0x00099371 File Offset: 0x00097571
		public bool TeamSelectionEnabled { get; private set; }

		// Token: 0x0600287D RID: 10365 RVA: 0x00099382 File Offset: 0x00097582
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._missionNetworkComponent = base.Mission.GetMissionBehavior<MissionNetworkComponent>();
			this._gameModeServer = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBase>();
			if (BannerlordNetwork.LobbyMissionType == LobbyMissionType.Matchmaker)
			{
				this.TeamSelectionEnabled = false;
				return;
			}
			this.TeamSelectionEnabled = true;
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x000993C2 File Offset: 0x000975C2
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsServer)
			{
				registerer.RegisterBaseHandler<TeamChange>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventTeamChange));
			}
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x000993E0 File Offset: 0x000975E0
		private void OnMyClientSynchronized()
		{
			base.Mission.GetMissionBehavior<MissionNetworkComponent>().OnMyClientSynchronized -= this.OnMyClientSynchronized;
			if (Mission.Current.GetMissionBehavior<MissionLobbyComponent>().CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending && GameNetwork.MyPeer.GetComponent<MissionPeer>().Team == null)
			{
				this.SelectTeam();
			}
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x00099434 File Offset: 0x00097634
		public override void AfterStart()
		{
			this._platformFriends = new HashSet<PlayerId>();
			foreach (PlayerId playerId in FriendListService.GetAllFriendsInAllPlatforms())
			{
				this._platformFriends.Add(playerId);
			}
			this._friendsPerTeam = new Dictionary<Team, IEnumerable<VirtualPlayer>>();
			MissionPeer.OnTeamChanged += this.UpdateTeams;
			if (GameNetwork.IsClient)
			{
				MissionNetworkComponent missionBehavior = base.Mission.GetMissionBehavior<MissionNetworkComponent>();
				if (this.TeamSelectionEnabled)
				{
					missionBehavior.OnMyClientSynchronized += this.OnMyClientSynchronized;
				}
			}
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x000994DC File Offset: 0x000976DC
		public override void OnRemoveBehavior()
		{
			MissionPeer.OnTeamChanged -= this.UpdateTeams;
			this.OnMyTeamChange = null;
			base.OnRemoveBehavior();
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x000994FC File Offset: 0x000976FC
		private bool HandleClientEventTeamChange(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			TeamChange teamChange = (TeamChange)baseMessage;
			if (this.TeamSelectionEnabled)
			{
				if (teamChange.AutoAssign)
				{
					this.AutoAssignTeam(peer);
				}
				else
				{
					Team teamFromTeamIndex = Mission.MissionNetworkHelper.GetTeamFromTeamIndex(teamChange.TeamIndex);
					this.ChangeTeamServer(peer, teamFromTeamIndex);
				}
			}
			return true;
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x00099540 File Offset: 0x00097740
		public void SelectTeam()
		{
			if (this.OnSelectingTeam != null)
			{
				List<Team> disabledTeams = this.GetDisabledTeams();
				this.OnSelectingTeam(disabledTeams);
			}
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x00099568 File Offset: 0x00097768
		public void UpdateTeams(NetworkCommunicator peer, Team oldTeam, Team newTeam)
		{
			if (this.OnUpdateTeams != null)
			{
				this.OnUpdateTeams();
			}
			if (GameNetwork.IsMyPeerReady)
			{
				this.CacheFriendsForTeams();
			}
			if (newTeam.Side != BattleSideEnum.None)
			{
				MissionPeer component = peer.GetComponent<MissionPeer>();
				component.SelectedTroopIndex = 0;
				component.NextSelectedTroopIndex = 0;
				component.OverrideCultureWithTeamCulture();
			}
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x000995B8 File Offset: 0x000977B8
		public List<Team> GetDisabledTeams()
		{
			List<Team> list = new List<Team>();
			if (MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) == 0)
			{
				return list;
			}
			Team myTeam = (GameNetwork.IsMyPeerReady ? GameNetwork.MyPeer.GetComponent<MissionPeer>().Team : null);
			Team[] array = base.Mission.Teams.Where<Team>((Team q) => q != this.Mission.SpectatorTeam).OrderBy<Team, int>(delegate(Team q)
			{
				if (myTeam == null)
				{
					return this.GetPlayerCountForTeam(q);
				}
				if (q != myTeam)
				{
					return this.GetPlayerCountForTeam(q);
				}
				return this.GetPlayerCountForTeam(q) - 1;
			}).ToArray<Team>();
			foreach (Team team in array)
			{
				int num = this.GetPlayerCountForTeam(team);
				int num2 = this.GetPlayerCountForTeam(array[0]);
				if (myTeam == team)
				{
					num--;
				}
				if (myTeam == array[0])
				{
					num2--;
				}
				if (num - num2 >= MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions))
				{
					list.Add(team);
				}
			}
			return list;
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x000996A0 File Offset: 0x000978A0
		public void ChangeTeamServer(NetworkCommunicator networkPeer, Team team)
		{
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			Team team2 = component.Team;
			if (team2 != null && team2 != base.Mission.SpectatorTeam && team2 != team && component.ControlledAgent != null)
			{
				Blow blow = new Blow(component.ControlledAgent.Index);
				blow.DamageType = DamageTypes.Invalid;
				blow.BaseMagnitude = 10000f;
				blow.GlobalPosition = component.ControlledAgent.Position;
				blow.DamagedPercentage = 1f;
				component.ControlledAgent.Die(blow, Agent.KillInfo.TeamSwitch);
			}
			component.Team = team;
			BasicCultureObject basicCultureObject = (component.Team.IsAttacker ? MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)) : MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)));
			component.Culture = basicCultureObject;
			if (team != team2)
			{
				if (component.HasSpawnedAgentVisuals)
				{
					component.HasSpawnedAgentVisuals = false;
					MBDebug.Print("HasSpawnedAgentVisuals = false for peer: " + component.Name + " because he just changed his team", 0, Debug.DebugColor.White, 17592186044416UL);
					component.SpawnCountThisRound = 0;
					Mission.Current.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>().RemoveAgentVisuals(component, true);
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new RemoveAgentVisualsForPeer(component.GetNetworkPeer()));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					}
					component.HasSpawnedAgentVisuals = false;
				}
				if (!this._gameModeServer.IsGameModeHidingAllAgentVisuals && !networkPeer.IsServerPeer)
				{
					MissionNetworkComponent missionNetworkComponent = this._missionNetworkComponent;
					if (missionNetworkComponent != null)
					{
						missionNetworkComponent.OnPeerSelectedTeam(component);
					}
				}
				this._gameModeServer.OnPeerChangedTeam(networkPeer, team2, team);
				component.SpawnTimer.Reset(Mission.Current.CurrentTime, 0.1f);
				component.WantsToSpawnAsBot = false;
				component.HasSpawnTimerExpired = false;
			}
			this.UpdateTeams(networkPeer, team2, team);
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x00099850 File Offset: 0x00097A50
		public void ChangeTeam(Team team)
		{
			if (team != GameNetwork.MyPeer.GetComponent<MissionPeer>().Team)
			{
				if (GameNetwork.IsServer)
				{
					Mission.Current.PlayerTeam = team;
					this.ChangeTeamServer(GameNetwork.MyPeer, team);
				}
				else
				{
					foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
					{
						MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
						if (component != null)
						{
							component.ClearAllVisuals(false);
						}
					}
					GameNetwork.BeginModuleEventAsClient();
					GameNetwork.WriteMessage(new TeamChange(false, team.TeamIndex));
					GameNetwork.EndModuleEventAsClient();
				}
				if (this.OnMyTeamChange != null)
				{
					this.OnMyTeamChange();
				}
			}
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x00099910 File Offset: 0x00097B10
		public int GetPlayerCountForTeam(Team team)
		{
			int num = 0;
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (((component != null) ? component.Team : null) != null && component.Team == team)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x00099980 File Offset: 0x00097B80
		private void CacheFriendsForTeams()
		{
			this._friendsPerTeam.Clear();
			if (this._platformFriends.Count > 0)
			{
				List<MissionPeer> list = new List<MissionPeer>();
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (component != null && this._platformFriends.Contains(networkCommunicator.VirtualPlayer.Id))
					{
						list.Add(component);
					}
				}
				using (List<Team>.Enumerator enumerator2 = base.Mission.Teams.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Team team = enumerator2.Current;
						if (team != null)
						{
							this._friendsPerTeam.Add(team, from x in list
								where x.Team == team
								select x.Peer);
						}
					}
				}
				if (this.OnUpdateFriendsPerTeam != null)
				{
					this.OnUpdateFriendsPerTeam();
				}
			}
		}

		// Token: 0x0600288A RID: 10378 RVA: 0x00099ACC File Offset: 0x00097CCC
		public IEnumerable<VirtualPlayer> GetFriendsForTeam(Team team)
		{
			if (this._friendsPerTeam.ContainsKey(team))
			{
				return this._friendsPerTeam[team];
			}
			return new List<VirtualPlayer>();
		}

		// Token: 0x0600288B RID: 10379 RVA: 0x00099AF0 File Offset: 0x00097CF0
		public void BalanceTeams()
		{
			if (MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) != 0)
			{
				int i = this.GetPlayerCountForTeam(Mission.Current.AttackerTeam);
				int j = this.GetPlayerCountForTeam(Mission.Current.DefenderTeam);
				while (i > j + MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions))
				{
					MissionPeer missionPeer = null;
					foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
					{
						if (networkCommunicator.IsSynchronized)
						{
							MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
							if (((component != null) ? component.Team : null) != null && component.Team == base.Mission.AttackerTeam && (missionPeer == null || component.JoinTime >= missionPeer.JoinTime))
							{
								missionPeer = component;
							}
						}
					}
					this.ChangeTeamServer(missionPeer.GetNetworkPeer(), Mission.Current.DefenderTeam);
					i--;
					j++;
				}
				while (j > i + MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions))
				{
					MissionPeer missionPeer2 = null;
					foreach (NetworkCommunicator networkCommunicator2 in GameNetwork.NetworkPeers)
					{
						if (networkCommunicator2.IsSynchronized)
						{
							MissionPeer component2 = networkCommunicator2.GetComponent<MissionPeer>();
							if (((component2 != null) ? component2.Team : null) != null && component2.Team == base.Mission.DefenderTeam && (missionPeer2 == null || component2.JoinTime >= missionPeer2.JoinTime))
							{
								missionPeer2 = component2;
							}
						}
					}
					this.ChangeTeamServer(missionPeer2.GetNetworkPeer(), Mission.Current.AttackerTeam);
					i++;
					j--;
				}
			}
		}

		// Token: 0x0600288C RID: 10380 RVA: 0x00099CB8 File Offset: 0x00097EB8
		public void AutoAssignTeam(NetworkCommunicator peer)
		{
			if (!GameNetwork.IsServer)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new TeamChange(true, -1));
				GameNetwork.EndModuleEventAsClient();
				if (this.OnMyTeamChange != null)
				{
					this.OnMyTeamChange();
				}
				return;
			}
			List<Team> disabledTeams = this.GetDisabledTeams();
			List<Team> list = base.Mission.Teams.Where<Team>((Team x) => !disabledTeams.Contains(x) && x.Side != BattleSideEnum.None).ToList<Team>();
			Team team;
			if (list.Count > 1)
			{
				int[] array = new int[list.Count];
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (((component != null) ? component.Team : null) != null)
					{
						for (int i = 0; i < list.Count; i++)
						{
							if (component.Team == list[i])
							{
								array[i]++;
							}
						}
					}
				}
				int num = -1;
				int num2 = -1;
				for (int j = 0; j < array.Length; j++)
				{
					if (num2 < 0 || array[j] < num)
					{
						num2 = j;
						num = array[j];
					}
				}
				team = list[num2];
			}
			else
			{
				team = list[0];
			}
			if (!peer.IsMine)
			{
				this.ChangeTeamServer(peer, team);
				return;
			}
			this.ChangeTeam(team);
		}

		// Token: 0x04000F8B RID: 3979
		private MissionNetworkComponent _missionNetworkComponent;

		// Token: 0x04000F8C RID: 3980
		private MissionMultiplayerGameModeBase _gameModeServer;

		// Token: 0x04000F8D RID: 3981
		private HashSet<PlayerId> _platformFriends;

		// Token: 0x04000F8E RID: 3982
		private Dictionary<Team, IEnumerable<VirtualPlayer>> _friendsPerTeam;

		// Token: 0x020005A7 RID: 1447
		// (Invoke) Token: 0x06003DF3 RID: 15859
		public delegate void OnSelectingTeamDelegate(List<Team> disableTeams);
	}
}
