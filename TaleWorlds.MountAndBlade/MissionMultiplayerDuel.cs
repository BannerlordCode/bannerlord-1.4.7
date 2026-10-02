using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.MountAndBlade.MissionRepresentatives;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002B4 RID: 692
	public class MissionMultiplayerDuel : MissionMultiplayerGameModeBase
	{
		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06002727 RID: 10023 RVA: 0x00090EB3 File Offset: 0x0008F0B3
		public override bool IsGameModeHidingAllAgentVisuals
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06002728 RID: 10024 RVA: 0x00090EB6 File Offset: 0x0008F0B6
		public override bool IsGameModeUsingOpposingTeams
		{
			get
			{
				return false;
			}
		}

		// Token: 0x14000061 RID: 97
		// (add) Token: 0x06002729 RID: 10025 RVA: 0x00090EBC File Offset: 0x0008F0BC
		// (remove) Token: 0x0600272A RID: 10026 RVA: 0x00090EF4 File Offset: 0x0008F0F4
		public event MissionMultiplayerDuel.OnDuelEndedDelegate OnDuelEnded;

		// Token: 0x0600272B RID: 10027 RVA: 0x00090F29 File Offset: 0x0008F129
		public override MultiplayerGameType GetMissionType()
		{
			return MultiplayerGameType.Duel;
		}

		// Token: 0x0600272C RID: 10028 RVA: 0x00090F2C File Offset: 0x0008F12C
		public override void AfterStart()
		{
			base.AfterStart();
			Mission.Current.SetMissionCorpseFadeOutTimeInSeconds(1f);
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(@object, object2);
			Banner banner = new Banner(@object.Banner, multiplayerBattleColors.AttackerColors.BannerBackgroundColorUint, multiplayerBattleColors.AttackerColors.BannerForegroundColorUint);
			base.Mission.Teams.Add(BattleSideEnum.Attacker, multiplayerBattleColors.AttackerColors.BannerBackgroundColorUint, multiplayerBattleColors.AttackerColors.BannerForegroundColorUint, banner, false, false, true);
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x00090FC8 File Offset: 0x0008F1C8
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._duelAreaFlags.AddRange(Mission.Current.Scene.FindEntitiesWithTagExpression("area_flag(_\\d+)*"));
			List<GameEntity> list = new List<GameEntity>();
			list.AddRange(Mission.Current.Scene.FindEntitiesWithTagExpression("area_box(_\\d+)*"));
			this._cachedSelectedAreaFlags = new KeyValuePair<int, TroopType>[this._duelAreaFlags.Count];
			for (int i = 0; i < list.Count; i++)
			{
				VolumeBox firstScriptOfType = list[i].GetFirstScriptOfType<VolumeBox>();
				this._areaBoxes.Add(firstScriptOfType);
			}
			this._cachedSelectedVolumeBoxes = new VolumeBox[this._areaBoxes.Count];
		}

		// Token: 0x0600272E RID: 10030 RVA: 0x00091070 File Offset: 0x0008F270
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			registerer.RegisterBaseHandler<NetworkMessages.FromClient.DuelRequest>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventDuelRequest));
			registerer.RegisterBaseHandler<DuelResponse>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventDuelRequestAccepted));
			registerer.RegisterBaseHandler<RequestChangePreferredTroopType>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventDuelRequestChangePreferredTroopType));
		}

		// Token: 0x0600272F RID: 10031 RVA: 0x000910A8 File Offset: 0x0008F2A8
		protected override void HandleEarlyNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			networkPeer.AddComponent<DuelMissionRepresentative>();
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x000910B4 File Offset: 0x0008F2B4
		protected override void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			component.Team = base.Mission.AttackerTeam;
			this._peersAndSelections.Add(new KeyValuePair<MissionPeer, TroopType>(component, TroopType.Invalid));
		}

		// Token: 0x06002731 RID: 10033 RVA: 0x000910EC File Offset: 0x0008F2EC
		private bool HandleClientEventDuelRequest(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			NetworkMessages.FromClient.DuelRequest duelRequest = (NetworkMessages.FromClient.DuelRequest)baseMessage;
			MissionPeer missionPeer = ((peer != null) ? peer.GetComponent<MissionPeer>() : null);
			if (missionPeer != null)
			{
				Agent agentFromIndex = Mission.MissionNetworkHelper.GetAgentFromIndex(duelRequest.RequestedAgentIndex, false);
				if (agentFromIndex != null && agentFromIndex.IsActive())
				{
					this.DuelRequestReceived(missionPeer, agentFromIndex.MissionPeer);
				}
			}
			return true;
		}

		// Token: 0x06002732 RID: 10034 RVA: 0x00091138 File Offset: 0x0008F338
		private bool HandleClientEventDuelRequestAccepted(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			DuelResponse duelResponse = (DuelResponse)baseMessage;
			if (((peer != null) ? peer.GetComponent<MissionPeer>() : null) != null && peer.GetComponent<MissionPeer>().ControlledAgent != null)
			{
				NetworkCommunicator peer2 = duelResponse.Peer;
				if (((peer2 != null) ? peer2.GetComponent<MissionPeer>() : null) != null && duelResponse.Peer.GetComponent<MissionPeer>().ControlledAgent != null)
				{
					this.DuelRequestAccepted(duelResponse.Peer.GetComponent<DuelMissionRepresentative>().ControlledAgent, peer.GetComponent<DuelMissionRepresentative>().ControlledAgent);
				}
			}
			return true;
		}

		// Token: 0x06002733 RID: 10035 RVA: 0x000911B0 File Offset: 0x0008F3B0
		private bool HandleClientEventDuelRequestChangePreferredTroopType(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			RequestChangePreferredTroopType requestChangePreferredTroopType = (RequestChangePreferredTroopType)baseMessage;
			this.OnPeerSelectedPreferredTroopType(peer.GetComponent<MissionPeer>(), requestChangePreferredTroopType.TroopType);
			return true;
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x000911D8 File Offset: 0x0008F3D8
		public override bool CheckIfPlayerCanDespawn(MissionPeer missionPeer)
		{
			for (int i = 0; i < this._activeDuels.Count; i++)
			{
				if (this._activeDuels[i].IsPeerInThisDuel(missionPeer))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002735 RID: 10037 RVA: 0x00091212 File Offset: 0x0008F412
		public void OnPlayerDespawn(MissionPeer missionPeer)
		{
			missionPeer.GetComponent<DuelMissionRepresentative>();
		}

		// Token: 0x06002736 RID: 10038 RVA: 0x0009121C File Offset: 0x0008F41C
		public void DuelRequestReceived(MissionPeer requesterPeer, MissionPeer requesteePeer)
		{
			if (!this.IsThereARequestBetweenPeers(requesterPeer, requesteePeer) && !this.IsHavingDuel(requesterPeer) && !this.IsHavingDuel(requesteePeer))
			{
				MissionMultiplayerDuel.DuelInfo duelInfo = new MissionMultiplayerDuel.DuelInfo(requesterPeer, requesteePeer, this.GetNextAvailableDuelAreaIndex(requesterPeer.ControlledAgent));
				this._duelRequests.Add(duelInfo);
				(requesteePeer.Representative as DuelMissionRepresentative).DuelRequested(requesterPeer.ControlledAgent, duelInfo.DuelAreaTroopType);
			}
		}

		// Token: 0x06002737 RID: 10039 RVA: 0x00091284 File Offset: 0x0008F484
		private KeyValuePair<int, TroopType> GetNextAvailableDuelAreaIndex(Agent requesterAgent)
		{
			TroopType troopType = TroopType.Invalid;
			for (int i = 0; i < this._peersAndSelections.Count; i++)
			{
				if (this._peersAndSelections[i].Key == requesterAgent.MissionPeer)
				{
					troopType = this._peersAndSelections[i].Value;
					break;
				}
			}
			if (troopType == TroopType.Invalid)
			{
				troopType = this.GetAgentTroopType(requesterAgent);
			}
			bool flag = false;
			int num = 0;
			for (int j = 0; j < this._duelAreaFlags.Count; j++)
			{
				GameEntity gameEntity = this._duelAreaFlags[j];
				int num2 = int.Parse(gameEntity.Tags.Single<string>((string ft) => ft.StartsWith("area_flag_")).Replace("area_flag_", ""));
				int flagIndex = num2 - 1;
				if (this._activeDuels.All<MissionMultiplayerDuel.DuelInfo>((MissionMultiplayerDuel.DuelInfo ad) => ad.DuelAreaIndex != flagIndex) && this._restartingDuels.All<MissionMultiplayerDuel.DuelInfo>((MissionMultiplayerDuel.DuelInfo ad) => ad.DuelAreaIndex != flagIndex) && this._restartPreparationDuels.All<MissionMultiplayerDuel.DuelInfo>((MissionMultiplayerDuel.DuelInfo ad) => ad.DuelAreaIndex != flagIndex))
				{
					TroopType troopType2 = (gameEntity.HasTag("flag_infantry") ? TroopType.Infantry : (gameEntity.HasTag("flag_archery") ? TroopType.Ranged : TroopType.Cavalry));
					if (!flag && troopType2 == troopType)
					{
						flag = true;
						num = 0;
					}
					if (!flag || troopType2 == troopType)
					{
						this._cachedSelectedAreaFlags[num] = new KeyValuePair<int, TroopType>(flagIndex, troopType2);
						num++;
					}
				}
			}
			return this._cachedSelectedAreaFlags[MBRandom.RandomInt(num)];
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x0009142C File Offset: 0x0008F62C
		public void DuelRequestAccepted(Agent requesterAgent, Agent requesteeAgent)
		{
			MissionMultiplayerDuel.DuelInfo duelInfo = this._duelRequests.FirstOrDefault<MissionMultiplayerDuel.DuelInfo>((MissionMultiplayerDuel.DuelInfo dr) => dr.IsPeerInThisDuel(requesterAgent.MissionPeer) && dr.IsPeerInThisDuel(requesteeAgent.MissionPeer));
			if (duelInfo != null)
			{
				this.PrepareDuel(duelInfo);
			}
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x0009146F File Offset: 0x0008F66F
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			this.CheckRestartPreparationDuels();
			this.CheckForRestartingDuels();
			this.CheckDuelsToStart();
			this.CheckDuelRequestTimeouts();
			this.CheckEndedDuels();
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x00091498 File Offset: 0x0008F698
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (!affectedAgent.IsHuman)
			{
				return;
			}
			if (affectedAgent.MissionPeer.Team.IsDefender)
			{
				MissionMultiplayerDuel.DuelInfo duelInfo = null;
				for (int i = 0; i < this._activeDuels.Count; i++)
				{
					if (this._activeDuels[i].IsPeerInThisDuel(affectedAgent.MissionPeer))
					{
						duelInfo = this._activeDuels[i];
					}
				}
				if (duelInfo != null && !this._endingDuels.Contains(duelInfo))
				{
					duelInfo.OnDuelEnding();
					this._endingDuels.Add(duelInfo);
					return;
				}
			}
			else
			{
				for (int j = this._duelRequests.Count - 1; j >= 0; j--)
				{
					if (this._duelRequests[j].IsPeerInThisDuel(affectedAgent.MissionPeer))
					{
						this._duelRequests.RemoveAt(j);
					}
				}
			}
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x0009155F File Offset: 0x0008F75F
		private Team ActivateAndGetDuelTeam()
		{
			if (this._deactiveDuelTeams.Count <= 0)
			{
				return base.Mission.Teams.Add(BattleSideEnum.Defender, uint.MaxValue, uint.MaxValue, null, true, false, false);
			}
			return this._deactiveDuelTeams.Dequeue();
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x00091592 File Offset: 0x0008F792
		private void DeactivateDuelTeam(Team team)
		{
			this._deactiveDuelTeams.Enqueue(team);
		}

		// Token: 0x0600273D RID: 10045 RVA: 0x000915A0 File Offset: 0x0008F7A0
		private bool IsHavingDuel(MissionPeer peer)
		{
			return this._activeDuels.AnyQ<MissionMultiplayerDuel.DuelInfo>((MissionMultiplayerDuel.DuelInfo d) => d.IsPeerInThisDuel(peer));
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x000915D4 File Offset: 0x0008F7D4
		private bool IsThereARequestBetweenPeers(MissionPeer requesterAgent, MissionPeer requesteeAgent)
		{
			for (int i = 0; i < this._duelRequests.Count; i++)
			{
				if (this._duelRequests[i].IsPeerInThisDuel(requesterAgent) && this._duelRequests[i].IsPeerInThisDuel(requesteeAgent))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x00091624 File Offset: 0x0008F824
		private void CheckDuelsToStart()
		{
			for (int i = this._activeDuels.Count - 1; i >= 0; i--)
			{
				MissionMultiplayerDuel.DuelInfo duelInfo = this._activeDuels[i];
				if (!duelInfo.Started && duelInfo.Timer.IsPast && duelInfo.IsDuelStillValid(false))
				{
					this.StartDuel(duelInfo);
				}
			}
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x00091680 File Offset: 0x0008F880
		private void CheckDuelRequestTimeouts()
		{
			for (int i = this._duelRequests.Count - 1; i >= 0; i--)
			{
				MissionMultiplayerDuel.DuelInfo duelInfo = this._duelRequests[i];
				if (duelInfo.Timer.IsPast)
				{
					this._duelRequests.Remove(duelInfo);
				}
			}
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x000916D0 File Offset: 0x0008F8D0
		private void CheckForRestartingDuels()
		{
			for (int i = this._restartingDuels.Count - 1; i >= 0; i--)
			{
				if (!this._restartingDuels[i].IsDuelStillValid(true))
				{
					Debug.Print("!_restartingDuels[i].IsDuelStillValid(true)", 0, Debug.DebugColor.White, 17592186044416UL);
				}
				this._duelRequests.Add(this._restartingDuels[i]);
				this.PrepareDuel(this._restartingDuels[i]);
				this._restartingDuels.RemoveAt(i);
			}
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x00091754 File Offset: 0x0008F954
		private void CheckEndedDuels()
		{
			for (int i = this._endingDuels.Count - 1; i >= 0; i--)
			{
				MissionMultiplayerDuel.DuelInfo duelInfo = this._endingDuels[i];
				if (duelInfo.Timer.IsPast)
				{
					this.EndDuel(duelInfo);
					this._endingDuels.RemoveAt(i);
					if (!duelInfo.ChallengeEnded)
					{
						this._restartPreparationDuels.Add(duelInfo);
					}
				}
			}
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x000917C0 File Offset: 0x0008F9C0
		private void CheckRestartPreparationDuels()
		{
			for (int i = this._restartPreparationDuels.Count - 1; i >= 0; i--)
			{
				MissionMultiplayerDuel.DuelInfo duelInfo = this._restartPreparationDuels[i];
				Agent controlledAgent = duelInfo.RequesterPeer.ControlledAgent;
				Agent controlledAgent2 = duelInfo.RequesteePeer.ControlledAgent;
				if ((controlledAgent == null || controlledAgent.IsActive()) && (controlledAgent2 == null || controlledAgent2.IsActive()))
				{
					this._restartPreparationDuels.RemoveAt(i);
					this._restartingDuels.Add(duelInfo);
				}
			}
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x00091838 File Offset: 0x0008FA38
		private void PrepareDuel(MissionMultiplayerDuel.DuelInfo duel)
		{
			this._duelRequests.Remove(duel);
			if (!this.IsHavingDuel(duel.RequesteePeer) && !this.IsHavingDuel(duel.RequesterPeer))
			{
				this._activeDuels.Add(duel);
				Team team = (duel.Started ? duel.DuelingTeam : this.ActivateAndGetDuelTeam());
				duel.OnDuelPreparation(team);
				for (int i = 0; i < this._duelRequests.Count; i++)
				{
					if (this._duelRequests[i].DuelAreaIndex == duel.DuelAreaIndex)
					{
						this._duelRequests[i].UpdateDuelAreaIndex(this.GetNextAvailableDuelAreaIndex(this._duelRequests[i].RequesterPeer.ControlledAgent));
					}
				}
				return;
			}
			Debug.FailedAssert("IsHavingDuel(duel.RequesteePeer) || IsHavingDuel(duel.RequesterPeer)", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MultiplayerGameModeLogics\\ServerGameModeLogics\\MissionMultiplayerDuel.cs", "PrepareDuel", 714);
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x00091914 File Offset: 0x0008FB14
		private void StartDuel(MissionMultiplayerDuel.DuelInfo duel)
		{
			duel.OnDuelStarted();
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x0009191C File Offset: 0x0008FB1C
		private void EndDuel(MissionMultiplayerDuel.DuelInfo duel)
		{
			this._activeDuels.Remove(duel);
			duel.OnDuelEnded();
			this.CleanSpawnedEntitiesInDuelArea(duel.DuelAreaIndex);
			if (duel.ChallengeEnded)
			{
				TroopType troopType = TroopType.Invalid;
				MissionPeer challengeWinnerPeer = duel.ChallengeWinnerPeer;
				if (((challengeWinnerPeer != null) ? challengeWinnerPeer.ControlledAgent : null) != null)
				{
					troopType = this.GetAgentTroopType(challengeWinnerPeer.ControlledAgent);
				}
				MissionMultiplayerDuel.OnDuelEndedDelegate onDuelEnded = this.OnDuelEnded;
				if (onDuelEnded != null)
				{
					onDuelEnded(challengeWinnerPeer, troopType);
				}
				this.DeactivateDuelTeam(duel.DuelingTeam);
				this.HandleEndedChallenge(duel);
			}
		}

		// Token: 0x06002747 RID: 10055 RVA: 0x0009199C File Offset: 0x0008FB9C
		private TroopType GetAgentTroopType(Agent requesterAgent)
		{
			TroopType troopType = TroopType.Invalid;
			switch (requesterAgent.Character.DefaultFormationClass)
			{
			case FormationClass.Infantry:
			case FormationClass.HeavyInfantry:
				troopType = TroopType.Infantry;
				break;
			case FormationClass.Ranged:
				troopType = TroopType.Ranged;
				break;
			case FormationClass.Cavalry:
			case FormationClass.HorseArcher:
			case FormationClass.LightCavalry:
			case FormationClass.HeavyCavalry:
				troopType = TroopType.Cavalry;
				break;
			}
			return troopType;
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x000919EC File Offset: 0x0008FBEC
		private void CleanSpawnedEntitiesInDuelArea(int duelAreaIndex)
		{
			int num = duelAreaIndex + 1;
			int num2 = 0;
			for (int i = 0; i < this._areaBoxes.Count; i++)
			{
				if (this._areaBoxes[i].GameEntity.HasTag(string.Format("{0}_{1}", "area_box", num)))
				{
					this._cachedSelectedVolumeBoxes[num2] = this._areaBoxes[i];
					num2++;
				}
			}
			for (int j = 0; j < Mission.Current.ActiveMissionObjects.Count; j++)
			{
				SpawnedItemEntity spawnedItemEntity;
				if ((spawnedItemEntity = Mission.Current.ActiveMissionObjects[j] as SpawnedItemEntity) != null && !spawnedItemEntity.IsDeactivated)
				{
					for (int k = 0; k < num2; k++)
					{
						if (this._cachedSelectedVolumeBoxes[k].IsPointIn(spawnedItemEntity.GameEntity.GlobalPosition))
						{
							spawnedItemEntity.RequestDeletionOnNextTick();
							break;
						}
					}
				}
			}
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x00091AD8 File Offset: 0x0008FCD8
		private void HandleEndedChallenge(MissionMultiplayerDuel.DuelInfo duel)
		{
			MissionPeer challengeWinnerPeer = duel.ChallengeWinnerPeer;
			MissionPeer challengeLoserPeer = duel.ChallengeLoserPeer;
			if (challengeWinnerPeer != null)
			{
				DuelMissionRepresentative component = challengeWinnerPeer.GetComponent<DuelMissionRepresentative>();
				DuelMissionRepresentative component2 = challengeLoserPeer.GetComponent<DuelMissionRepresentative>();
				MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer = MultiplayerClassDivisions.GetMPHeroClassForPeer(challengeWinnerPeer, true);
				MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer2 = MultiplayerClassDivisions.GetMPHeroClassForPeer(challengeLoserPeer, true);
				float num = (float)MathF.Max(100, component2.Bounty) * MathF.Max(1f, (float)mpheroClassForPeer.TroopCasualCost / (float)mpheroClassForPeer2.TroopCasualCost) * MathF.Pow(2.7182817f, (float)component.NumberOfWins / 10f);
				component.OnDuelWon(num);
				if (challengeWinnerPeer.Peer.Communicator.IsConnectionActive)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new DuelPointsUpdateMessage(component));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
				component2.ResetBountyAndNumberOfWins();
				if (challengeLoserPeer.Peer.Communicator.IsConnectionActive)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new DuelPointsUpdateMessage(component2));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
			}
			PeerComponent peerComponent = challengeWinnerPeer ?? duel.RequesterPeer;
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new DuelEnded(peerComponent.GetNetworkPeer()));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x00091BE4 File Offset: 0x0008FDE4
		public int GetDuelAreaIndexIfDuelTeam(Team team)
		{
			if (team.IsDefender)
			{
				return this._activeDuels.FirstOrDefaultQ<MissionMultiplayerDuel.DuelInfo>((MissionMultiplayerDuel.DuelInfo ad) => ad.DuelingTeam == team).DuelAreaIndex;
			}
			return -1;
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x00091C2C File Offset: 0x0008FE2C
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			if (agent.IsHuman && agent.Team != null && agent.Team.IsDefender)
			{
				for (int i = 0; i < this._activeDuels.Count; i++)
				{
					if (this._activeDuels[i].IsPeerInThisDuel(agent.MissionPeer))
					{
						this._activeDuels[i].OnAgentBuild(agent);
						return;
					}
				}
			}
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x00091C98 File Offset: 0x0008FE98
		protected override void HandleLateNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			if (!networkPeer.IsServerPeer)
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					DuelMissionRepresentative component = networkCommunicator.GetComponent<DuelMissionRepresentative>();
					if (component != null)
					{
						GameNetwork.BeginModuleEventAsServer(networkPeer);
						GameNetwork.WriteMessage(new DuelPointsUpdateMessage(component));
						GameNetwork.EndModuleEventAsServer();
					}
					if (networkPeer != networkCommunicator)
					{
						MissionPeer component2 = networkCommunicator.GetComponent<MissionPeer>();
						if (component2 != null)
						{
							GameNetwork.BeginModuleEventAsServer(networkPeer);
							GameNetwork.WriteMessage(new SyncPerksForCurrentlySelectedTroop(networkCommunicator, component2.Perks[component2.SelectedTroopIndex]));
							GameNetwork.EndModuleEventAsServer();
						}
					}
				}
				for (int i = 0; i < this._activeDuels.Count; i++)
				{
					GameNetwork.BeginModuleEventAsServer(networkPeer);
					GameNetwork.WriteMessage(new DuelPreparationStartedForTheFirstTime(this._activeDuels[i].RequesterPeer.GetNetworkPeer(), this._activeDuels[i].RequesteePeer.GetNetworkPeer(), this._activeDuels[i].DuelAreaIndex));
					GameNetwork.EndModuleEventAsServer();
				}
			}
		}

		// Token: 0x0600274D RID: 10061 RVA: 0x00091DB4 File Offset: 0x0008FFB4
		protected override void HandleEarlyPlayerDisconnect(NetworkCommunicator networkPeer)
		{
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			for (int i = 0; i < this._peersAndSelections.Count; i++)
			{
				if (this._peersAndSelections[i].Key == component)
				{
					this._peersAndSelections.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x00091E04 File Offset: 0x00090004
		protected override void HandlePlayerDisconnect(NetworkCommunicator networkPeer)
		{
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			if (component != null)
			{
				component.Team = null;
			}
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x00091E24 File Offset: 0x00090024
		private void OnPeerSelectedPreferredTroopType(MissionPeer missionPeer, TroopType troopType)
		{
			for (int i = 0; i < this._peersAndSelections.Count; i++)
			{
				if (this._peersAndSelections[i].Key == missionPeer)
				{
					this._peersAndSelections[i] = new KeyValuePair<MissionPeer, TroopType>(missionPeer, troopType);
					return;
				}
			}
		}

		// Token: 0x04000EDA RID: 3802
		public const float DuelRequestTimeOutInSeconds = 10f;

		// Token: 0x04000EDB RID: 3803
		private const int MinBountyGain = 100;

		// Token: 0x04000EDC RID: 3804
		private const string AreaBoxTagPrefix = "area_box";

		// Token: 0x04000EDD RID: 3805
		private const string AreaFlagTagPrefix = "area_flag";

		// Token: 0x04000EDE RID: 3806
		public const int NumberOfDuelAreas = 16;

		// Token: 0x04000EDF RID: 3807
		public const float DuelEndInSeconds = 2f;

		// Token: 0x04000EE0 RID: 3808
		private const float DuelRequestTimeOutServerToleranceInSeconds = 0.5f;

		// Token: 0x04000EE1 RID: 3809
		private const float CorpseFadeOutTimeInSeconds = 1f;

		// Token: 0x04000EE3 RID: 3811
		private List<GameEntity> _duelAreaFlags = new List<GameEntity>();

		// Token: 0x04000EE4 RID: 3812
		private List<VolumeBox> _areaBoxes = new List<VolumeBox>();

		// Token: 0x04000EE5 RID: 3813
		private List<MissionMultiplayerDuel.DuelInfo> _duelRequests = new List<MissionMultiplayerDuel.DuelInfo>();

		// Token: 0x04000EE6 RID: 3814
		private List<MissionMultiplayerDuel.DuelInfo> _activeDuels = new List<MissionMultiplayerDuel.DuelInfo>();

		// Token: 0x04000EE7 RID: 3815
		private List<MissionMultiplayerDuel.DuelInfo> _endingDuels = new List<MissionMultiplayerDuel.DuelInfo>();

		// Token: 0x04000EE8 RID: 3816
		private List<MissionMultiplayerDuel.DuelInfo> _restartingDuels = new List<MissionMultiplayerDuel.DuelInfo>();

		// Token: 0x04000EE9 RID: 3817
		private List<MissionMultiplayerDuel.DuelInfo> _restartPreparationDuels = new List<MissionMultiplayerDuel.DuelInfo>();

		// Token: 0x04000EEA RID: 3818
		private readonly Queue<Team> _deactiveDuelTeams = new Queue<Team>();

		// Token: 0x04000EEB RID: 3819
		private List<KeyValuePair<MissionPeer, TroopType>> _peersAndSelections = new List<KeyValuePair<MissionPeer, TroopType>>();

		// Token: 0x04000EEC RID: 3820
		private VolumeBox[] _cachedSelectedVolumeBoxes;

		// Token: 0x04000EED RID: 3821
		private KeyValuePair<int, TroopType>[] _cachedSelectedAreaFlags;

		// Token: 0x0200058C RID: 1420
		private class DuelInfo
		{
			// Token: 0x17000A6A RID: 2666
			// (get) Token: 0x06003D76 RID: 15734 RVA: 0x000F3530 File Offset: 0x000F1730
			public MissionPeer RequesterPeer
			{
				get
				{
					return this._challengers[0].MissionPeer;
				}
			}

			// Token: 0x17000A6B RID: 2667
			// (get) Token: 0x06003D77 RID: 15735 RVA: 0x000F3543 File Offset: 0x000F1743
			public MissionPeer RequesteePeer
			{
				get
				{
					return this._challengers[1].MissionPeer;
				}
			}

			// Token: 0x17000A6C RID: 2668
			// (get) Token: 0x06003D78 RID: 15736 RVA: 0x000F3556 File Offset: 0x000F1756
			// (set) Token: 0x06003D79 RID: 15737 RVA: 0x000F355E File Offset: 0x000F175E
			public int DuelAreaIndex { get; private set; }

			// Token: 0x17000A6D RID: 2669
			// (get) Token: 0x06003D7A RID: 15738 RVA: 0x000F3567 File Offset: 0x000F1767
			// (set) Token: 0x06003D7B RID: 15739 RVA: 0x000F356F File Offset: 0x000F176F
			public TroopType DuelAreaTroopType { get; private set; }

			// Token: 0x17000A6E RID: 2670
			// (get) Token: 0x06003D7C RID: 15740 RVA: 0x000F3578 File Offset: 0x000F1778
			// (set) Token: 0x06003D7D RID: 15741 RVA: 0x000F3580 File Offset: 0x000F1780
			public MissionTime Timer { get; private set; }

			// Token: 0x17000A6F RID: 2671
			// (get) Token: 0x06003D7E RID: 15742 RVA: 0x000F3589 File Offset: 0x000F1789
			// (set) Token: 0x06003D7F RID: 15743 RVA: 0x000F3591 File Offset: 0x000F1791
			public Team DuelingTeam { get; private set; }

			// Token: 0x17000A70 RID: 2672
			// (get) Token: 0x06003D80 RID: 15744 RVA: 0x000F359A File Offset: 0x000F179A
			// (set) Token: 0x06003D81 RID: 15745 RVA: 0x000F35A2 File Offset: 0x000F17A2
			public bool Started { get; private set; }

			// Token: 0x17000A71 RID: 2673
			// (get) Token: 0x06003D82 RID: 15746 RVA: 0x000F35AB File Offset: 0x000F17AB
			// (set) Token: 0x06003D83 RID: 15747 RVA: 0x000F35B3 File Offset: 0x000F17B3
			public bool ChallengeEnded { get; private set; }

			// Token: 0x17000A72 RID: 2674
			// (get) Token: 0x06003D84 RID: 15748 RVA: 0x000F35BC File Offset: 0x000F17BC
			public MissionPeer ChallengeWinnerPeer
			{
				get
				{
					if (this._winnerChallengerType != MissionMultiplayerDuel.DuelInfo.ChallengerType.None)
					{
						return this._challengers[(int)this._winnerChallengerType].MissionPeer;
					}
					return null;
				}
			}

			// Token: 0x17000A73 RID: 2675
			// (get) Token: 0x06003D85 RID: 15749 RVA: 0x000F35DF File Offset: 0x000F17DF
			public MissionPeer ChallengeLoserPeer
			{
				get
				{
					if (this._winnerChallengerType != MissionMultiplayerDuel.DuelInfo.ChallengerType.None)
					{
						return this._challengers[(this._winnerChallengerType == MissionMultiplayerDuel.DuelInfo.ChallengerType.Requester) ? 1 : 0].MissionPeer;
					}
					return null;
				}
			}

			// Token: 0x06003D86 RID: 15750 RVA: 0x000F3608 File Offset: 0x000F1808
			public DuelInfo(MissionPeer requesterPeer, MissionPeer requesteePeer, KeyValuePair<int, TroopType> duelAreaPair)
			{
				this.DuelAreaIndex = duelAreaPair.Key;
				this.DuelAreaTroopType = duelAreaPair.Value;
				this._challengers = new MissionMultiplayerDuel.DuelInfo.Challenger[2];
				this._challengers[0] = new MissionMultiplayerDuel.DuelInfo.Challenger(requesterPeer);
				this._challengers[1] = new MissionMultiplayerDuel.DuelInfo.Challenger(requesteePeer);
				this.Timer = MissionTime.Now + MissionTime.Seconds(10.5f);
			}

			// Token: 0x06003D87 RID: 15751 RVA: 0x000F3688 File Offset: 0x000F1888
			private void DecideRoundWinner()
			{
				bool isConnectionActive = this._challengers[0].MissionPeer.Peer.Communicator.IsConnectionActive;
				bool isConnectionActive2 = this._challengers[1].MissionPeer.Peer.Communicator.IsConnectionActive;
				if (!this.Started)
				{
					if (isConnectionActive == isConnectionActive2)
					{
						this.ChallengeEnded = true;
					}
					else
					{
						this._winnerChallengerType = (isConnectionActive ? MissionMultiplayerDuel.DuelInfo.ChallengerType.Requester : MissionMultiplayerDuel.DuelInfo.ChallengerType.Requestee);
					}
				}
				else
				{
					Agent duelingAgent = this._challengers[0].DuelingAgent;
					Agent duelingAgent2 = this._challengers[1].DuelingAgent;
					if (duelingAgent.IsActive())
					{
						this._winnerChallengerType = MissionMultiplayerDuel.DuelInfo.ChallengerType.Requester;
					}
					else if (duelingAgent2.IsActive())
					{
						this._winnerChallengerType = MissionMultiplayerDuel.DuelInfo.ChallengerType.Requestee;
					}
					else
					{
						if (!isConnectionActive && !isConnectionActive2)
						{
							this.ChallengeEnded = true;
						}
						this._winnerChallengerType = MissionMultiplayerDuel.DuelInfo.ChallengerType.None;
					}
				}
				if (this._winnerChallengerType != MissionMultiplayerDuel.DuelInfo.ChallengerType.None)
				{
					this._challengers[(int)this._winnerChallengerType].IncreaseWinCount();
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new DuelRoundEnded(this._challengers[(int)this._winnerChallengerType].NetworkPeer));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					if (this._challengers[(int)this._winnerChallengerType].KillCountInDuel == MultiplayerOptions.OptionType.MinScoreToWinDuel.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) || !isConnectionActive || !isConnectionActive2)
					{
						this.ChallengeEnded = true;
					}
				}
			}

			// Token: 0x06003D88 RID: 15752 RVA: 0x000F37CC File Offset: 0x000F19CC
			public void OnDuelPreparation(Team duelTeam)
			{
				if (!this.Started)
				{
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new DuelPreparationStartedForTheFirstTime(this._challengers[0].MissionPeer.GetNetworkPeer(), this._challengers[1].MissionPeer.GetNetworkPeer(), this.DuelAreaIndex));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				}
				this.Started = false;
				this.DuelingTeam = duelTeam;
				this._winnerChallengerType = MissionMultiplayerDuel.DuelInfo.ChallengerType.None;
				for (int i = 0; i < 2; i++)
				{
					this._challengers[i].OnDuelPreparation(this.DuelingTeam);
					this._challengers[i].MissionPeer.GetComponent<DuelMissionRepresentative>().OnDuelPreparation(this._challengers[0].MissionPeer, this._challengers[1].MissionPeer);
				}
				this.Timer = MissionTime.Now + MissionTime.Seconds(3f);
			}

			// Token: 0x06003D89 RID: 15753 RVA: 0x000F38B8 File Offset: 0x000F1AB8
			public void OnDuelStarted()
			{
				this.Started = true;
				this.DuelingTeam.SetIsEnemyOf(this.DuelingTeam, true);
			}

			// Token: 0x06003D8A RID: 15754 RVA: 0x000F38D3 File Offset: 0x000F1AD3
			public void OnDuelEnding()
			{
				this.Timer = MissionTime.Now + MissionTime.Seconds(2f);
			}

			// Token: 0x06003D8B RID: 15755 RVA: 0x000F38F0 File Offset: 0x000F1AF0
			public void OnDuelEnded()
			{
				if (this.Started)
				{
					this.DuelingTeam.SetIsEnemyOf(this.DuelingTeam, false);
				}
				this.DecideRoundWinner();
				for (int i = 0; i < 2; i++)
				{
					this._challengers[i].OnDuelEnded();
					Agent agent = this._challengers[i].DuelingAgent ?? this._challengers[i].MissionPeer.ControlledAgent;
					if (this.ChallengeEnded && agent != null && agent.IsActive())
					{
						agent.FadeOut(true, false);
					}
					this._challengers[i].MissionPeer.HasSpawnedAgentVisuals = true;
				}
				for (int j = 0; j < 2; j++)
				{
					if (this._challengers[j].MountAgent != null && this._challengers[j].MountAgent.IsActive() && (this.ChallengeEnded || this._challengers[j].MountAgent.RiderAgent == null))
					{
						this._challengers[j].MountAgent.FadeOut(true, false);
					}
				}
			}

			// Token: 0x06003D8C RID: 15756 RVA: 0x000F3A08 File Offset: 0x000F1C08
			public void OnAgentBuild(Agent agent)
			{
				for (int i = 0; i < 2; i++)
				{
					if (this._challengers[i].MissionPeer == agent.MissionPeer)
					{
						this._challengers[i].SetAgents(agent);
						return;
					}
				}
			}

			// Token: 0x06003D8D RID: 15757 RVA: 0x000F3A50 File Offset: 0x000F1C50
			public bool IsDuelStillValid(bool doNotCheckAgent = false)
			{
				for (int i = 0; i < 2; i++)
				{
					if (!this._challengers[i].MissionPeer.Peer.Communicator.IsConnectionActive || (!doNotCheckAgent && !this._challengers[i].MissionPeer.IsControlledAgentActive))
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x06003D8E RID: 15758 RVA: 0x000F3AAC File Offset: 0x000F1CAC
			public bool IsPeerInThisDuel(MissionPeer peer)
			{
				for (int i = 0; i < 2; i++)
				{
					if (this._challengers[i].MissionPeer == peer)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06003D8F RID: 15759 RVA: 0x000F3ADC File Offset: 0x000F1CDC
			public void UpdateDuelAreaIndex(KeyValuePair<int, TroopType> duelAreaPair)
			{
				this.DuelAreaIndex = duelAreaPair.Key;
				this.DuelAreaTroopType = duelAreaPair.Value;
			}

			// Token: 0x04001E75 RID: 7797
			private const float DuelStartCountdown = 3f;

			// Token: 0x04001E76 RID: 7798
			private readonly MissionMultiplayerDuel.DuelInfo.Challenger[] _challengers;

			// Token: 0x04001E77 RID: 7799
			private MissionMultiplayerDuel.DuelInfo.ChallengerType _winnerChallengerType = MissionMultiplayerDuel.DuelInfo.ChallengerType.None;

			// Token: 0x020006BD RID: 1725
			private enum ChallengerType
			{
				// Token: 0x04002337 RID: 9015
				None = -1,
				// Token: 0x04002338 RID: 9016
				Requester,
				// Token: 0x04002339 RID: 9017
				Requestee,
				// Token: 0x0400233A RID: 9018
				NumChallengerType
			}

			// Token: 0x020006BE RID: 1726
			private struct Challenger
			{
				// Token: 0x17000B05 RID: 2821
				// (get) Token: 0x06004218 RID: 16920 RVA: 0x000FCC15 File Offset: 0x000FAE15
				// (set) Token: 0x06004219 RID: 16921 RVA: 0x000FCC1D File Offset: 0x000FAE1D
				public Agent DuelingAgent { get; private set; }

				// Token: 0x17000B06 RID: 2822
				// (get) Token: 0x0600421A RID: 16922 RVA: 0x000FCC26 File Offset: 0x000FAE26
				// (set) Token: 0x0600421B RID: 16923 RVA: 0x000FCC2E File Offset: 0x000FAE2E
				public Agent MountAgent { get; private set; }

				// Token: 0x17000B07 RID: 2823
				// (get) Token: 0x0600421C RID: 16924 RVA: 0x000FCC37 File Offset: 0x000FAE37
				// (set) Token: 0x0600421D RID: 16925 RVA: 0x000FCC3F File Offset: 0x000FAE3F
				public int KillCountInDuel { get; private set; }

				// Token: 0x0600421E RID: 16926 RVA: 0x000FCC48 File Offset: 0x000FAE48
				public Challenger(MissionPeer missionPeer)
				{
					this.MissionPeer = missionPeer;
					MissionPeer missionPeer2 = this.MissionPeer;
					this.NetworkPeer = ((missionPeer2 != null) ? missionPeer2.GetNetworkPeer() : null);
					this.DuelingAgent = null;
					this.MountAgent = null;
					this.KillCountInDuel = 0;
				}

				// Token: 0x0600421F RID: 16927 RVA: 0x000FCC7E File Offset: 0x000FAE7E
				public void OnDuelPreparation(Team duelingTeam)
				{
					Agent controlledAgent = this.MissionPeer.ControlledAgent;
					if (controlledAgent != null)
					{
						controlledAgent.FadeOut(true, true);
					}
					this.MissionPeer.Team = duelingTeam;
					this.MissionPeer.HasSpawnedAgentVisuals = true;
				}

				// Token: 0x06004220 RID: 16928 RVA: 0x000FCCB0 File Offset: 0x000FAEB0
				public void OnDuelEnded()
				{
					if (this.MissionPeer.Peer.Communicator.IsConnectionActive)
					{
						this.MissionPeer.Team = Mission.Current.AttackerTeam;
					}
				}

				// Token: 0x06004221 RID: 16929 RVA: 0x000FCCE0 File Offset: 0x000FAEE0
				public void IncreaseWinCount()
				{
					int killCountInDuel = this.KillCountInDuel;
					this.KillCountInDuel = killCountInDuel + 1;
				}

				// Token: 0x06004222 RID: 16930 RVA: 0x000FCCFD File Offset: 0x000FAEFD
				public void SetAgents(Agent agent)
				{
					this.DuelingAgent = agent;
					this.MountAgent = this.DuelingAgent.MountAgent;
				}

				// Token: 0x0400233B RID: 9019
				public readonly MissionPeer MissionPeer;

				// Token: 0x0400233C RID: 9020
				public readonly NetworkCommunicator NetworkPeer;
			}
		}

		// Token: 0x0200058D RID: 1421
		// (Invoke) Token: 0x06003D91 RID: 15761
		public delegate void OnDuelEndedDelegate(MissionPeer winnerPeer, TroopType troopType);
	}
}
