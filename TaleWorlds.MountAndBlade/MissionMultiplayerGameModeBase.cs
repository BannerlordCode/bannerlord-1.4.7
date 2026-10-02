using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002B6 RID: 694
	public abstract class MissionMultiplayerGameModeBase : MissionNetwork
	{
		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x0600278C RID: 10124
		public abstract bool IsGameModeHidingAllAgentVisuals { get; }

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x0600278D RID: 10125
		public abstract bool IsGameModeUsingOpposingTeams { get; }

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x0600278E RID: 10126 RVA: 0x0009440A File Offset: 0x0009260A
		public virtual bool IsGameModeAllowChargeDamageOnFriendly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x0600278F RID: 10127 RVA: 0x0009440D File Offset: 0x0009260D
		// (set) Token: 0x06002790 RID: 10128 RVA: 0x00094415 File Offset: 0x00092615
		public SpawnComponent SpawnComponent { get; private set; }

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06002791 RID: 10129 RVA: 0x0009441E File Offset: 0x0009261E
		// (set) Token: 0x06002792 RID: 10130 RVA: 0x00094426 File Offset: 0x00092626
		private protected bool CanGameModeSystemsTickThisFrame { protected get; private set; }

		// Token: 0x06002793 RID: 10131
		public abstract MultiplayerGameType GetMissionType();

		// Token: 0x06002794 RID: 10132 RVA: 0x0009442F File Offset: 0x0009262F
		public virtual bool CheckIfOvertime()
		{
			return false;
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x00094434 File Offset: 0x00092634
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this.MultiplayerTeamSelectComponent = base.Mission.GetMissionBehavior<MultiplayerTeamSelectComponent>();
			this.MissionLobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this.GameModeBaseClient = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this.NotificationsComponent = base.Mission.GetMissionBehavior<MultiplayerGameNotificationsComponent>();
			this.RoundController = base.Mission.GetMissionBehavior<MultiplayerRoundController>();
			this.WarmupComponent = base.Mission.GetMissionBehavior<MultiplayerWarmupComponent>();
			this.TimerComponent = base.Mission.GetMissionBehavior<MultiplayerTimerComponent>();
			this.SpawnComponent = Mission.Current.GetMissionBehavior<SpawnComponent>();
			this._agentVisualSpawnComponent = base.Mission.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>();
			this._lastPerkTickTime = Mission.Current.CurrentTime;
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x000944F0 File Offset: 0x000926F0
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (Mission.Current.CurrentTime - this._lastPerkTickTime >= 1f)
			{
				this._lastPerkTickTime = Mission.Current.CurrentTime;
				MPPerkObject.TickAllPeerPerks((int)(this._lastPerkTickTime / 1f));
			}
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x0009453E File Offset: 0x0009273E
		public virtual bool CheckForWarmupEnd()
		{
			return false;
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x00094541 File Offset: 0x00092741
		public virtual bool CheckForRoundEnd()
		{
			return false;
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x00094544 File Offset: 0x00092744
		public virtual bool CheckForMatchEnd()
		{
			return false;
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x00094547 File Offset: 0x00092747
		public virtual bool UseCultureSelection()
		{
			return false;
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x0009454A File Offset: 0x0009274A
		public virtual bool UseRoundController()
		{
			return false;
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x0009454D File Offset: 0x0009274D
		public virtual Team GetWinnerTeam()
		{
			return null;
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x00094550 File Offset: 0x00092750
		public virtual void OnPeerChangedTeam(NetworkCommunicator peer, Team oldTeam, Team newTeam)
		{
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x00094552 File Offset: 0x00092752
		public override void OnClearScene()
		{
			base.OnClearScene();
			if (this.RoundController == null)
			{
				this.ClearPeerCounts();
			}
			this._lastPerkTickTime = Mission.Current.CurrentTime;
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x00094578 File Offset: 0x00092778
		public void ClearPeerCounts()
		{
			List<MissionPeer> list = VirtualPlayer.Peers<MissionPeer>();
			for (int i = 0; i < list.Count; i++)
			{
				MissionPeer missionPeer = list[i];
				missionPeer.AssistCount = 0;
				missionPeer.DeathCount = 0;
				missionPeer.KillCount = 0;
				missionPeer.Score = 0;
				missionPeer.ResetRequestedKickPollCount();
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new KillDeathCountChange(missionPeer.GetNetworkPeer(), null, missionPeer.KillCount, missionPeer.AssistCount, missionPeer.DeathCount, missionPeer.Score));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
		}

		// Token: 0x060027A0 RID: 10144 RVA: 0x000945FC File Offset: 0x000927FC
		public bool ShouldSpawnVisualsForServer(NetworkCommunicator spawningNetworkPeer)
		{
			if (GameNetwork.IsDedicatedServer)
			{
				return false;
			}
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
			if (missionPeer != null)
			{
				MissionPeer component = spawningNetworkPeer.GetComponent<MissionPeer>();
				return (!this.IsGameModeHidingAllAgentVisuals && component.Team == missionPeer.Team) || spawningNetworkPeer.IsServerPeer;
			}
			return false;
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x0009465C File Offset: 0x0009285C
		public void HandleAgentVisualSpawning(NetworkCommunicator spawningNetworkPeer, AgentBuildData spawningAgentBuildData, int troopCountInFormation = 0, bool useCosmetics = true)
		{
			MissionPeer component = spawningNetworkPeer.GetComponent<MissionPeer>();
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new SyncPerksForCurrentlySelectedTroop(spawningNetworkPeer, component.Perks[component.SelectedTroopIndex]));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeOtherTeamPlayers, spawningNetworkPeer);
			component.HasSpawnedAgentVisuals = true;
			component.EquipmentUpdatingExpired = false;
			if (useCosmetics)
			{
				this.AddCosmeticItemsToEquipment(spawningAgentBuildData.AgentOverridenSpawnEquipment, this.GetUsedCosmeticsFromPeer(component, spawningAgentBuildData.AgentCharacter));
			}
			if (!this.IsGameModeHidingAllAgentVisuals)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new CreateAgentVisuals(spawningNetworkPeer, spawningAgentBuildData, component.SelectedTroopIndex, troopCountInFormation));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeOtherTeamPlayers, spawningNetworkPeer);
				return;
			}
			if (!spawningNetworkPeer.IsServerPeer)
			{
				GameNetwork.BeginModuleEventAsServer(spawningNetworkPeer);
				GameNetwork.WriteMessage(new CreateAgentVisuals(spawningNetworkPeer, spawningAgentBuildData, component.SelectedTroopIndex, troopCountInFormation));
				GameNetwork.EndModuleEventAsServer();
			}
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x00094713 File Offset: 0x00092913
		public virtual bool AllowCustomPlayerBanners()
		{
			return true;
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x00094716 File Offset: 0x00092916
		public virtual int GetScoreForKill(Agent killedAgent)
		{
			return 20;
		}

		// Token: 0x060027A4 RID: 10148 RVA: 0x0009471A File Offset: 0x0009291A
		public virtual float GetTroopNumberMultiplierForMissingPlayer(MissionPeer spawningPeer)
		{
			return 1f;
		}

		// Token: 0x060027A5 RID: 10149 RVA: 0x00094721 File Offset: 0x00092921
		public int GetCurrentGoldForPeer(MissionPeer peer)
		{
			return peer.Representative.Gold;
		}

		// Token: 0x060027A6 RID: 10150 RVA: 0x00094730 File Offset: 0x00092930
		public void ChangeCurrentGoldForPeer(MissionPeer peer, int newAmount)
		{
			if (newAmount >= 0)
			{
				newAmount = MBMath.ClampInt(newAmount, 0, 2000);
			}
			if (peer.Peer.Communicator.IsConnectionActive)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SyncGoldsForSkirmish(peer.Peer, newAmount));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
			if (this.GameModeBaseClient != null)
			{
				this.GameModeBaseClient.OnGoldAmountChangedForRepresentative(peer.Representative, newAmount);
			}
		}

		// Token: 0x060027A7 RID: 10151 RVA: 0x00094798 File Offset: 0x00092998
		protected override void HandleLateNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			if (this.GameModeBaseClient.IsGameModeUsingGold)
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					if (networkCommunicator != networkPeer)
					{
						MissionRepresentativeBase component = networkCommunicator.GetComponent<MissionRepresentativeBase>();
						if (component != null)
						{
							GameNetwork.BeginModuleEventAsServer(networkPeer);
							GameNetwork.WriteMessage(new SyncGoldsForSkirmish(component.Peer, component.Gold));
							GameNetwork.EndModuleEventAsServer();
						}
					}
				}
			}
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x00094820 File Offset: 0x00092A20
		public virtual bool CheckIfPlayerCanDespawn(MissionPeer missionPeer)
		{
			return false;
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x00094823 File Offset: 0x00092A23
		public override void OnPreMissionTick(float dt)
		{
			this.CanGameModeSystemsTickThisFrame = false;
			this._gameModeSystemTickTimer += dt;
			if (this._gameModeSystemTickTimer >= 0.25f)
			{
				this._gameModeSystemTickTimer -= 0.25f;
				this.CanGameModeSystemsTickThisFrame = true;
			}
		}

		// Token: 0x060027AA RID: 10154 RVA: 0x00094860 File Offset: 0x00092A60
		public Dictionary<string, string> GetUsedCosmeticsFromPeer(MissionPeer missionPeer, BasicCharacterObject selectedTroopCharacter)
		{
			if (missionPeer.Peer.UsedCosmetics != null)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				MBReadOnlyList<MultiplayerClassDivisions.MPHeroClass> objectTypeList = MBObjectManager.Instance.GetObjectTypeList<MultiplayerClassDivisions.MPHeroClass>();
				int num = -1;
				for (int i = 0; i < objectTypeList.Count; i++)
				{
					if (objectTypeList[i].HeroCharacter == selectedTroopCharacter || objectTypeList[i].TroopCharacter == selectedTroopCharacter)
					{
						num = i;
						break;
					}
				}
				List<int> list;
				missionPeer.Peer.UsedCosmetics.TryGetValue(num, out list);
				if (list != null)
				{
					foreach (int num2 in list)
					{
						ClothingCosmeticElement clothingCosmeticElement;
						if ((clothingCosmeticElement = CosmeticsManager.CosmeticElementsList[num2] as ClothingCosmeticElement) != null)
						{
							foreach (string text in clothingCosmeticElement.ReplaceItemsId)
							{
								dictionary.Add(text, CosmeticsManager.CosmeticElementsList[num2].Id);
							}
							foreach (Tuple<string, string> tuple in clothingCosmeticElement.ReplaceItemless)
							{
								if (tuple.Item1 == objectTypeList[num].StringId)
								{
									dictionary.Add(tuple.Item2, CosmeticsManager.CosmeticElementsList[num2].Id);
									break;
								}
							}
						}
					}
				}
				return dictionary;
			}
			return null;
		}

		// Token: 0x060027AB RID: 10155 RVA: 0x00094A14 File Offset: 0x00092C14
		public void AddCosmeticItemsToEquipment(Equipment equipment, Dictionary<string, string> choosenCosmetics)
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.ArmorItemEndSlot; equipmentIndex++)
			{
				if (equipment[equipmentIndex].Item == null)
				{
					string text = equipmentIndex.ToString();
					switch (equipmentIndex)
					{
					case EquipmentIndex.NumAllWeaponSlots:
						text = "Head";
						break;
					case EquipmentIndex.Body:
						text = "Body";
						break;
					case EquipmentIndex.Leg:
						text = "Leg";
						break;
					case EquipmentIndex.Gloves:
						text = "Gloves";
						break;
					case EquipmentIndex.Cape:
						text = "Cape";
						break;
					}
					string text2 = null;
					if (choosenCosmetics != null)
					{
						choosenCosmetics.TryGetValue(text, out text2);
					}
					if (text2 != null)
					{
						ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>(text2);
						EquipmentElement equipmentElement = equipment[equipmentIndex];
						equipmentElement.CosmeticItem = @object;
						equipment[equipmentIndex] = equipmentElement;
					}
				}
				else
				{
					string stringId = equipment[equipmentIndex].Item.StringId;
					string text3 = null;
					if (choosenCosmetics != null)
					{
						choosenCosmetics.TryGetValue(stringId, out text3);
					}
					if (text3 != null)
					{
						ItemObject object2 = MBObjectManager.Instance.GetObject<ItemObject>(text3);
						EquipmentElement equipmentElement2 = equipment[equipmentIndex];
						equipmentElement2.CosmeticItem = object2;
						equipment[equipmentIndex] = equipmentElement2;
					}
				}
			}
		}

		// Token: 0x060027AC RID: 10156 RVA: 0x00094B2C File Offset: 0x00092D2C
		public bool IsClassAvailable(MultiplayerClassDivisions.MPHeroClass heroClass)
		{
			FormationClass formationClass;
			if (Enum.TryParse<FormationClass>(heroClass.ClassGroup.StringId, out formationClass))
			{
				return this.MissionLobbyComponent.IsClassAvailable(formationClass);
			}
			Debug.FailedAssert("\"" + heroClass.ClassGroup.StringId + "\" does not match with any FormationClass.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\MissionNetworkLogics\\MultiplayerGameModeLogics\\ServerGameModeLogics\\MissionMultiplayerGameModeBase.cs", "IsClassAvailable", 389);
			return false;
		}

		// Token: 0x04000F12 RID: 3858
		public const int GoldCap = 2000;

		// Token: 0x04000F13 RID: 3859
		public const float PerkTickPeriod = 1f;

		// Token: 0x04000F14 RID: 3860
		public const float GameModeSystemTickPeriod = 0.25f;

		// Token: 0x04000F15 RID: 3861
		private float _lastPerkTickTime;

		// Token: 0x04000F17 RID: 3863
		private MultiplayerMissionAgentVisualSpawnComponent _agentVisualSpawnComponent;

		// Token: 0x04000F18 RID: 3864
		public MultiplayerTeamSelectComponent MultiplayerTeamSelectComponent;

		// Token: 0x04000F19 RID: 3865
		protected MissionLobbyComponent MissionLobbyComponent;

		// Token: 0x04000F1A RID: 3866
		protected MultiplayerGameNotificationsComponent NotificationsComponent;

		// Token: 0x04000F1B RID: 3867
		public MultiplayerRoundController RoundController;

		// Token: 0x04000F1C RID: 3868
		public MultiplayerWarmupComponent WarmupComponent;

		// Token: 0x04000F1D RID: 3869
		public MultiplayerTimerComponent TimerComponent;

		// Token: 0x04000F1E RID: 3870
		protected MissionMultiplayerGameModeBaseClient GameModeBaseClient;

		// Token: 0x04000F20 RID: 3872
		private float _gameModeSystemTickTimer;
	}
}
