using System;
using System.Collections.Generic;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002D3 RID: 723
	public abstract class SpawningBehaviorBase
	{
		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x060029A2 RID: 10658 RVA: 0x0009D9C2 File Offset: 0x0009BBC2
		// (set) Token: 0x060029A3 RID: 10659 RVA: 0x0009D9CA File Offset: 0x0009BBCA
		private protected MultiplayerMissionAgentVisualSpawnComponent AgentVisualSpawnComponent { protected get; private set; }

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x060029A4 RID: 10660 RVA: 0x0009D9D3 File Offset: 0x0009BBD3
		protected Mission Mission
		{
			get
			{
				return this.SpawnComponent.Mission;
			}
		}

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x060029A5 RID: 10661 RVA: 0x0009D9E0 File Offset: 0x0009BBE0
		// (remove) Token: 0x060029A6 RID: 10662 RVA: 0x0009DA18 File Offset: 0x0009BC18
		protected event Action<MissionPeer> OnAllAgentsFromPeerSpawnedFromVisuals;

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x060029A7 RID: 10663 RVA: 0x0009DA50 File Offset: 0x0009BC50
		// (remove) Token: 0x060029A8 RID: 10664 RVA: 0x0009DA88 File Offset: 0x0009BC88
		protected event Action<MissionPeer> OnPeerSpawnedFromVisuals;

		// Token: 0x14000080 RID: 128
		// (add) Token: 0x060029A9 RID: 10665 RVA: 0x0009DAC0 File Offset: 0x0009BCC0
		// (remove) Token: 0x060029AA RID: 10666 RVA: 0x0009DAF8 File Offset: 0x0009BCF8
		public event SpawningBehaviorBase.OnSpawningEndedEventDelegate OnSpawningEnded;

		// Token: 0x060029AB RID: 10667 RVA: 0x0009DB30 File Offset: 0x0009BD30
		public virtual void Initialize(SpawnComponent spawnComponent)
		{
			this.SpawnComponent = spawnComponent;
			this.AgentVisualSpawnComponent = this.Mission.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>();
			this.GameMode = this.Mission.GetMissionBehavior<MissionMultiplayerGameModeBase>();
			this.MissionLobbyComponent = this.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this.MissionLobbyEquipmentNetworkComponent = this.Mission.GetMissionBehavior<MissionLobbyEquipmentNetworkComponent>();
			this.MissionLobbyEquipmentNetworkComponent.OnEquipmentRefreshed += this.OnPeerEquipmentUpdated;
			this.SpawnCheckTimer = new Timer(Mission.Current.CurrentTime, 0.2f, true);
			this._agentsToBeSpawnedCache = new List<AgentBuildData>();
			this._nextTimeToCleanUpMounts = MissionTime.Now;
			this._botsCountForSides = new int[2];
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x0009DBDC File Offset: 0x0009BDDC
		public virtual void Clear()
		{
			this.MissionLobbyEquipmentNetworkComponent.OnEquipmentRefreshed -= this.OnPeerEquipmentUpdated;
			this._agentsToBeSpawnedCache = null;
		}

		// Token: 0x060029AD RID: 10669 RVA: 0x0009DBFC File Offset: 0x0009BDFC
		public virtual void OnTick(float dt)
		{
			int count = Mission.Current.AllAgents.Count;
			int num = 0;
			this._agentsToBeSpawnedCache.Clear();
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(@object, object2);
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				if (networkCommunicator.IsSynchronized)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (component != null && component.ControlledAgent == null && component.HasSpawnedAgentVisuals && !this.CanUpdateSpawnEquipment(component))
					{
						MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer = MultiplayerClassDivisions.GetMPHeroClassForPeer(component, false);
						MPPerkObject.MPOnSpawnPerkHandler onSpawnPerkHandler = MPPerkObject.GetOnSpawnPerkHandler(component);
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SyncPerksForCurrentlySelectedTroop(networkCommunicator, component.Perks[component.SelectedTroopIndex]));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeOtherTeamPlayers, networkCommunicator);
						int num2 = 0;
						bool flag = false;
						int intValue = MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
						if (intValue > 0 && (this.GameMode.WarmupComponent == null || !this.GameMode.WarmupComponent.IsInWarmup))
						{
							num2 = MPPerkObject.GetTroopCount(mpheroClassForPeer, intValue, onSpawnPerkHandler);
							using (List<MPPerkObject>.Enumerator enumerator2 = component.SelectedPerks.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									if (enumerator2.Current.HasBannerBearer)
									{
										flag = true;
										break;
									}
								}
							}
						}
						if (num2 > 0)
						{
							num2 = (int)((float)num2 * this.GameMode.GetTroopNumberMultiplierForMissingPlayer(component));
						}
						num2 += (flag ? 2 : 1);
						IEnumerable<ValueTuple<EquipmentIndex, EquipmentElement>> enumerable = ((onSpawnPerkHandler != null) ? onSpawnPerkHandler.GetAlternativeEquipments(false) : null);
						int i = 0;
						while (i < num2)
						{
							bool flag2 = i == 0;
							BasicCharacterObject basicCharacterObject = (flag2 ? mpheroClassForPeer.HeroCharacter : ((flag && i == 1) ? mpheroClassForPeer.BannerBearerCharacter : mpheroClassForPeer.TroopCharacter));
							MultiplayerBattleColors.MultiplayerCultureColorInfo peerColors = multiplayerBattleColors.GetPeerColors(component);
							uint clothingColor1Uint = peerColors.ClothingColor1Uint;
							uint clothingColor2Uint = peerColors.ClothingColor2Uint;
							uint bannerBackgroundColorUint = peerColors.BannerBackgroundColorUint;
							uint bannerForegroundColorUint = peerColors.BannerForegroundColorUint;
							Banner banner = new Banner(component.Peer.BannerCode, bannerBackgroundColorUint, bannerForegroundColorUint);
							AgentBuildData agentBuildData = new AgentBuildData(basicCharacterObject).VisualsIndex(i).Team(component.Team).TroopOrigin(new BasicBattleAgentOrigin(basicCharacterObject))
								.Formation(component.ControlledFormation)
								.IsFemale(flag2 ? component.Peer.IsFemale : basicCharacterObject.IsFemale)
								.ClothingColor1(clothingColor1Uint)
								.ClothingColor2(clothingColor2Uint)
								.Banner(banner);
							if (flag2)
							{
								agentBuildData.MissionPeer(component);
							}
							else
							{
								agentBuildData.OwningMissionPeer(component);
							}
							Equipment equipment = (flag2 ? basicCharacterObject.Equipment.Clone(false) : Equipment.GetRandomEquipmentElements(basicCharacterObject, false, Equipment.EquipmentType.Battle, MBRandom.RandomInt()));
							IEnumerable<ValueTuple<EquipmentIndex, EquipmentElement>> enumerable2 = (flag2 ? ((onSpawnPerkHandler != null) ? onSpawnPerkHandler.GetAlternativeEquipments(true) : null) : enumerable);
							if (enumerable2 != null)
							{
								foreach (ValueTuple<EquipmentIndex, EquipmentElement> valueTuple in enumerable2)
								{
									equipment[valueTuple.Item1] = valueTuple.Item2;
								}
							}
							agentBuildData.Equipment(equipment);
							if (flag2)
							{
								this.GameMode.AddCosmeticItemsToEquipment(equipment, this.GameMode.GetUsedCosmeticsFromPeer(component, basicCharacterObject));
							}
							if (flag2)
							{
								agentBuildData.BodyProperties(this.GetBodyProperties(component, component.Culture));
								agentBuildData.Age((int)agentBuildData.AgentBodyProperties.Age);
							}
							else
							{
								agentBuildData.EquipmentSeed(this.MissionLobbyComponent.GetRandomFaceSeedForCharacter(basicCharacterObject, agentBuildData.AgentVisualsIndex));
								agentBuildData.BodyProperties(BodyProperties.GetRandomBodyProperties(agentBuildData.AgentRace, agentBuildData.AgentIsFemale, basicCharacterObject.GetBodyPropertiesMin(false), basicCharacterObject.GetBodyPropertiesMax(false), (int)agentBuildData.AgentOverridenSpawnEquipment.HairCoverType, agentBuildData.AgentEquipmentSeed, basicCharacterObject.BodyPropertyRange.HairTags, basicCharacterObject.BodyPropertyRange.BeardTags, basicCharacterObject.BodyPropertyRange.TattooTags, 0f));
							}
							if (component.ControlledFormation != null && component.ControlledFormation.Banner == null)
							{
								component.ControlledFormation.Banner = banner;
							}
							MatrixFrame spawnFrame = this.SpawnComponent.GetSpawnFrame(component.Team, equipment[EquipmentIndex.ArmorItemEndSlot].Item != null, component.SpawnCountThisRound == 0);
							if (spawnFrame.IsIdentity)
							{
								goto IL_04FF;
							}
							Vec2 vec;
							if (!(spawnFrame.origin != agentBuildData.AgentInitialPosition))
							{
								vec = spawnFrame.rotation.f.AsVec2.Normalized();
								Vec2? agentInitialDirection = agentBuildData.AgentInitialDirection;
								if (!(vec != agentInitialDirection))
								{
									goto IL_04FF;
								}
							}
							agentBuildData.InitialPosition(in spawnFrame.origin);
							AgentBuildData agentBuildData2 = agentBuildData;
							vec = spawnFrame.rotation.f.AsVec2;
							vec = vec.Normalized();
							agentBuildData2.InitialDirection(in vec);
							IL_0518:
							if (component.ControlledAgent != null && !flag2)
							{
								MatrixFrame frame = component.ControlledAgent.Frame;
								frame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
								MatrixFrame matrixFrame = frame;
								matrixFrame.origin -= matrixFrame.rotation.f.NormalizedCopy() * 3.5f;
								Mat3 rotation = matrixFrame.rotation;
								rotation.MakeUnit();
								bool flag3 = !basicCharacterObject.Equipment[EquipmentIndex.ArmorItemEndSlot].IsEmpty;
								int num3 = MathF.Min(num2, 10);
								MatrixFrame matrixFrame2 = Formation.GetFormationFramesForBeforeFormationCreation((float)num3 * Formation.GetDefaultUnitDiameter(flag3) + (float)(num3 - 1) * Formation.GetDefaultMinimumUnitInterval(flag3), num2, flag3, new WorldPosition(Mission.Current.Scene, matrixFrame.origin), rotation)[i - 1].ToGroundMatrixFrame();
								agentBuildData.InitialPosition(in matrixFrame2.origin);
								AgentBuildData agentBuildData3 = agentBuildData;
								vec = matrixFrame2.rotation.f.AsVec2;
								vec = vec.Normalized();
								agentBuildData3.InitialDirection(in vec);
							}
							this._agentsToBeSpawnedCache.Add(agentBuildData);
							num++;
							if (!agentBuildData.AgentOverridenSpawnEquipment[EquipmentIndex.ArmorItemEndSlot].IsEmpty)
							{
								num++;
							}
							i++;
							continue;
							IL_04FF:
							Debug.FailedAssert("Spawn frame could not be found.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\SpawnBehaviors\\SpawningBehaviors\\SpawningBehaviorBase.cs", "OnTick", 213);
							goto IL_0518;
						}
					}
				}
			}
			int num4 = num + count;
			if (num4 > SpawningBehaviorBase._agentCountThreshold && this._nextTimeToCleanUpMounts.IsPast)
			{
				this._nextTimeToCleanUpMounts = MissionTime.SecondsFromNow(5f);
				for (int j = Mission.Current.MountsWithoutRiders.Count - 1; j >= 0; j--)
				{
					KeyValuePair<Agent, MissionTime> keyValuePair = Mission.Current.MountsWithoutRiders[j];
					Agent key = keyValuePair.Key;
					if (keyValuePair.Value.ElapsedSeconds > 30f)
					{
						key.FadeOut(false, false);
					}
				}
			}
			int num5 = SpawningBehaviorBase._maxAgentCount - num4;
			if (num5 >= 0)
			{
				for (int k = this._agentsToBeSpawnedCache.Count - 1; k >= 0; k--)
				{
					AgentBuildData agentBuildData4 = this._agentsToBeSpawnedCache[k];
					bool flag4 = agentBuildData4.AgentMissionPeer != null;
					MissionPeer missionPeer = (flag4 ? agentBuildData4.AgentMissionPeer : agentBuildData4.OwningAgentMissionPeer);
					MPPerkObject.MPOnSpawnPerkHandler onSpawnPerkHandler2 = MPPerkObject.GetOnSpawnPerkHandler(missionPeer);
					Agent agent = this.Mission.SpawnAgent(agentBuildData4, true);
					agent.AddComponent(new MPPerksAgentComponent(agent));
					Agent mountAgent = agent.MountAgent;
					if (mountAgent != null)
					{
						mountAgent.UpdateAgentProperties();
					}
					agent.HealthLimit += ((onSpawnPerkHandler2 != null) ? onSpawnPerkHandler2.GetHitpoints(flag4) : 0f);
					agent.Health = agent.HealthLimit;
					if (!flag4)
					{
						agent.SetWatchState(Agent.WatchState.Alarmed);
					}
					agent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
					if (flag4)
					{
						MissionPeer missionPeer2 = missionPeer;
						int spawnCountThisRound = missionPeer2.SpawnCountThisRound;
						missionPeer2.SpawnCountThisRound = spawnCountThisRound + 1;
						Action<MissionPeer> onPeerSpawnedFromVisuals = this.OnPeerSpawnedFromVisuals;
						if (onPeerSpawnedFromVisuals != null)
						{
							onPeerSpawnedFromVisuals(missionPeer);
						}
						Action<MissionPeer> onAllAgentsFromPeerSpawnedFromVisuals = this.OnAllAgentsFromPeerSpawnedFromVisuals;
						if (onAllAgentsFromPeerSpawnedFromVisuals != null)
						{
							onAllAgentsFromPeerSpawnedFromVisuals(missionPeer);
						}
						this.AgentVisualSpawnComponent.RemoveAgentVisuals(missionPeer, true);
						if (GameNetwork.IsServerOrRecorder)
						{
							GameNetwork.BeginBroadcastModuleEvent();
							GameNetwork.WriteMessage(new RemoveAgentVisualsForPeer(missionPeer.GetNetworkPeer()));
							GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
						}
						missionPeer.HasSpawnedAgentVisuals = false;
						MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(missionPeer);
						if (perkHandler != null)
						{
							perkHandler.OnEvent(MPPerkCondition.PerkEventFlags.SpawnEnd);
						}
					}
				}
				int intValue2 = MultiplayerOptions.OptionType.NumberOfBotsTeam1.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				int intValue3 = MultiplayerOptions.OptionType.NumberOfBotsTeam2.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				if (this.GameMode.IsGameModeUsingOpposingTeams && (intValue2 > 0 || intValue3 > 0))
				{
					ValueTuple<Team, BasicCultureObject, int>[] array = new ValueTuple<Team, BasicCultureObject, int>[]
					{
						new ValueTuple<Team, BasicCultureObject, int>(this.Mission.DefenderTeam, MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)), intValue3 - this._botsCountForSides[0]),
						new ValueTuple<Team, BasicCultureObject, int>(this.Mission.AttackerTeam, MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)), intValue2 - this._botsCountForSides[1])
					};
					if (num5 >= 4)
					{
						int l = Math.Min(num5 / 2, array[0].Item3 + array[1].Item3);
						BattleSideEnum battleSideEnum = BattleSideEnum.Defender;
						while (l > 0)
						{
							int num6 = (int)battleSideEnum;
							if (array[num6].Item3 > 0)
							{
								this.SpawnBot(array[num6].Item1, array[num6].Item2);
								ValueTuple<Team, BasicCultureObject, int>[] array2 = array;
								int num7 = num6;
								array2[num7].Item3 = array2[num7].Item3 - 1;
								l--;
							}
							battleSideEnum = battleSideEnum.GetOppositeSide();
						}
					}
				}
			}
			if (!this.IsSpawningEnabled && this.IsRoundInProgress())
			{
				if (this.SpawningDelayTimer >= this.SpawningEndDelay && !this._hasCalledSpawningEnded)
				{
					Mission.Current.AllowAiTicking = true;
					if (this.OnSpawningEnded != null)
					{
						this.OnSpawningEnded();
					}
					this._hasCalledSpawningEnded = true;
				}
				this.SpawningDelayTimer += dt;
			}
		}

		// Token: 0x060029AE RID: 10670 RVA: 0x0009E66C File Offset: 0x0009C86C
		public bool AreAgentsSpawning()
		{
			return this.IsSpawningEnabled;
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x0009E674 File Offset: 0x0009C874
		protected void ResetSpawnCounts()
		{
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (component != null)
				{
					component.SpawnCountThisRound = 0;
				}
			}
		}

		// Token: 0x060029B0 RID: 10672 RVA: 0x0009E6D0 File Offset: 0x0009C8D0
		protected void ResetSpawnTimers()
		{
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (component != null)
				{
					component.SpawnTimer.Reset(Mission.Current.CurrentTime, 0f);
				}
			}
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x0009E740 File Offset: 0x0009C940
		public virtual void RequestStartSpawnSession()
		{
			this.IsSpawningEnabled = true;
			this.SpawningDelayTimer = 0f;
			this._hasCalledSpawningEnded = false;
			this.ResetSpawnCounts();
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x0009E764 File Offset: 0x0009C964
		public void RequestStopSpawnSession()
		{
			this.IsSpawningEnabled = false;
			this.SpawningDelayTimer = 0f;
			this._hasCalledSpawningEnded = false;
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (component != null)
				{
					this.AgentVisualSpawnComponent.RemoveAgentVisuals(component, true);
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new RemoveAgentVisualsForPeer(component.GetNetworkPeer()));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					}
					component.HasSpawnedAgentVisuals = false;
				}
			}
			foreach (NetworkCommunicator networkCommunicator2 in GameNetwork.DisconnectedNetworkPeers)
			{
				MissionPeer missionPeer = ((networkCommunicator2 != null) ? networkCommunicator2.GetComponent<MissionPeer>() : null);
				if (missionPeer != null)
				{
					this.AgentVisualSpawnComponent.RemoveAgentVisuals(missionPeer, false);
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new RemoveAgentVisualsForPeer(missionPeer.GetNetworkPeer()));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
					}
					missionPeer.HasSpawnedAgentVisuals = false;
				}
			}
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x0009E888 File Offset: 0x0009CA88
		public void SetRemainingAgentsInvulnerable()
		{
			foreach (Agent agent in this.Mission.Agents)
			{
				agent.SetMortalityState(Agent.MortalityState.Invulnerable);
			}
		}

		// Token: 0x060029B4 RID: 10676
		protected abstract void SpawnAgents();

		// Token: 0x060029B5 RID: 10677 RVA: 0x0009E8E0 File Offset: 0x0009CAE0
		protected BodyProperties GetBodyProperties(MissionPeer missionPeer, BasicCultureObject cultureLimit)
		{
			NetworkCommunicator networkPeer = missionPeer.GetNetworkPeer();
			if (networkPeer != null)
			{
				return networkPeer.PlayerConnectionInfo.GetParameter<PlayerData>("PlayerData").BodyProperties;
			}
			Debug.FailedAssert("networkCommunicator != null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\Multiplayer\\SpawnBehaviors\\SpawningBehaviors\\SpawningBehaviorBase.cs", "GetBodyProperties", 518);
			Team team = missionPeer.Team;
			BasicCharacterObject troopCharacter = MultiplayerClassDivisions.GetMPHeroClasses(cultureLimit).ToMBList<MultiplayerClassDivisions.MPHeroClass>().GetRandomElement<MultiplayerClassDivisions.MPHeroClass>()
				.TroopCharacter;
			MatrixFrame spawnFrame = this.SpawnComponent.GetSpawnFrame(team, troopCharacter.HasMount(), true);
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(@object, object2);
			MultiplayerBattleColors.MultiplayerCultureColorInfo multiplayerCultureColorInfo = ((cultureLimit == @object) ? multiplayerBattleColors.AttackerColors : multiplayerBattleColors.DefenderColors);
			AgentBuildData agentBuildData = new AgentBuildData(troopCharacter).Team(team).InitialPosition(in spawnFrame.origin);
			Vec2 vec = spawnFrame.rotation.f.AsVec2;
			vec = vec.Normalized();
			AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in vec).TroopOrigin(new BasicBattleAgentOrigin(troopCharacter)).EquipmentSeed(this.MissionLobbyComponent.GetRandomFaceSeedForCharacter(troopCharacter, 0))
				.ClothingColor1(multiplayerCultureColorInfo.ClothingColor1Uint)
				.ClothingColor2(multiplayerCultureColorInfo.ClothingColor2Uint)
				.IsFemale(troopCharacter.IsFemale);
			agentBuildData2.Equipment(Equipment.GetRandomEquipmentElements(troopCharacter, !GameNetwork.IsMultiplayer, Equipment.EquipmentType.Battle, agentBuildData2.AgentEquipmentSeed));
			agentBuildData2.BodyProperties(BodyProperties.GetRandomBodyProperties(agentBuildData2.AgentRace, agentBuildData2.AgentIsFemale, troopCharacter.GetBodyPropertiesMin(false), troopCharacter.GetBodyPropertiesMax(false), (int)agentBuildData2.AgentOverridenSpawnEquipment.HairCoverType, agentBuildData2.AgentEquipmentSeed, troopCharacter.BodyPropertyRange.HairTags, troopCharacter.BodyPropertyRange.BeardTags, troopCharacter.BodyPropertyRange.TattooTags, 0f));
			return agentBuildData2.AgentBodyProperties;
		}

		// Token: 0x060029B6 RID: 10678 RVA: 0x0009EAA8 File Offset: 0x0009CCA8
		protected void SpawnBot(Team agentTeam, BasicCultureObject cultureLimit)
		{
			BasicCharacterObject troopCharacter = MultiplayerClassDivisions.GetMPHeroClasses(cultureLimit).ToMBList<MultiplayerClassDivisions.MPHeroClass>().GetRandomElement<MultiplayerClassDivisions.MPHeroClass>()
				.TroopCharacter;
			MatrixFrame spawnFrame = this.SpawnComponent.GetSpawnFrame(agentTeam, troopCharacter.HasMount(), true);
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(@object, object2);
			MultiplayerBattleColors.MultiplayerCultureColorInfo multiplayerCultureColorInfo = ((cultureLimit == @object) ? multiplayerBattleColors.AttackerColors : multiplayerBattleColors.DefenderColors);
			AgentBuildData agentBuildData = new AgentBuildData(troopCharacter).Team(agentTeam).InitialPosition(in spawnFrame.origin);
			Vec2 vec = spawnFrame.rotation.f.AsVec2;
			vec = vec.Normalized();
			AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(in vec).TroopOrigin(new BasicBattleAgentOrigin(troopCharacter)).EquipmentSeed(this.MissionLobbyComponent.GetRandomFaceSeedForCharacter(troopCharacter, 0))
				.ClothingColor1(multiplayerCultureColorInfo.ClothingColor1Uint)
				.ClothingColor2(multiplayerCultureColorInfo.ClothingColor2Uint)
				.IsFemale(troopCharacter.IsFemale);
			agentBuildData2.Equipment(Equipment.GetRandomEquipmentElements(troopCharacter, !GameNetwork.IsMultiplayer, Equipment.EquipmentType.Battle, agentBuildData2.AgentEquipmentSeed));
			agentBuildData2.BodyProperties(BodyProperties.GetRandomBodyProperties(agentBuildData2.AgentRace, agentBuildData2.AgentIsFemale, troopCharacter.GetBodyPropertiesMin(false), troopCharacter.GetBodyPropertiesMax(false), (int)agentBuildData2.AgentOverridenSpawnEquipment.HairCoverType, agentBuildData2.AgentEquipmentSeed, troopCharacter.BodyPropertyRange.HairTags, troopCharacter.BodyPropertyRange.BeardTags, troopCharacter.BodyPropertyRange.TattooTags, 0f));
			Agent agent = this.Mission.SpawnAgent(agentBuildData2, false);
			agent.SetAlarmState(Agent.AIStateFlag.Alarmed);
			this._botsCountForSides[(int)agent.Team.Side]++;
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x0009EC58 File Offset: 0x0009CE58
		private void OnPeerEquipmentUpdated(MissionPeer peer)
		{
			if (this.IsSpawningEnabled && this.CanUpdateSpawnEquipment(peer))
			{
				peer.HasSpawnedAgentVisuals = false;
				Debug.Print("HasSpawnedAgentVisuals = false for peer: " + peer.Name + " because he just updated his equipment", 0, Debug.DebugColor.White, 17592186044416UL);
				if (peer.ControlledFormation != null)
				{
					peer.ControlledFormation.HasBeenPositioned = false;
					peer.ControlledFormation.SetSpawnIndex(0);
				}
			}
		}

		// Token: 0x060029B8 RID: 10680 RVA: 0x0009ECC3 File Offset: 0x0009CEC3
		public virtual bool CanUpdateSpawnEquipment(MissionPeer missionPeer)
		{
			return !missionPeer.EquipmentUpdatingExpired && !this._equipmentUpdatingExpired;
		}

		// Token: 0x060029B9 RID: 10681 RVA: 0x0009ECD8 File Offset: 0x0009CED8
		public void ToggleUpdatingSpawnEquipment(bool canUpdate)
		{
			this._equipmentUpdatingExpired = !canUpdate;
		}

		// Token: 0x060029BA RID: 10682
		public abstract bool AllowEarlyAgentVisualsDespawning(MissionPeer missionPeer);

		// Token: 0x060029BB RID: 10683 RVA: 0x0009ECE4 File Offset: 0x0009CEE4
		public virtual int GetMaximumReSpawnPeriodForPeer(MissionPeer peer)
		{
			return 3;
		}

		// Token: 0x060029BC RID: 10684
		protected abstract bool IsRoundInProgress();

		// Token: 0x060029BD RID: 10685 RVA: 0x0009ECE8 File Offset: 0x0009CEE8
		public virtual void OnClearScene()
		{
			for (int i = 0; i < this._botsCountForSides.Length; i++)
			{
				this._botsCountForSides[i] = 0;
			}
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x0009ED11 File Offset: 0x0009CF11
		public void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (affectedAgent.IsHuman && affectedAgent.MissionPeer == null && affectedAgent.OwningAgentMissionPeer == null)
			{
				this._botsCountForSides[(int)affectedAgent.Team.Side]--;
			}
		}

		// Token: 0x04000FFA RID: 4090
		private const float SecondsToWaitForEachMountBeforeSelectingToFadeOut = 30f;

		// Token: 0x04000FFB RID: 4091
		private const float SecondsToWaitBeforeNextMountCleanup = 5f;

		// Token: 0x04000FFC RID: 4092
		private static readonly int _maxAgentCount = MBAPI.IMBAgent.GetMaximumNumberOfAgents();

		// Token: 0x04000FFD RID: 4093
		private static readonly int _agentCountThreshold = (int)((float)SpawningBehaviorBase._maxAgentCount * 0.9f);

		// Token: 0x04000FFF RID: 4095
		protected MissionMultiplayerGameModeBase GameMode;

		// Token: 0x04001000 RID: 4096
		protected SpawnComponent SpawnComponent;

		// Token: 0x04001001 RID: 4097
		private bool _equipmentUpdatingExpired;

		// Token: 0x04001002 RID: 4098
		protected bool IsSpawningEnabled;

		// Token: 0x04001003 RID: 4099
		protected Timer SpawnCheckTimer;

		// Token: 0x04001004 RID: 4100
		protected float SpawningEndDelay = 1f;

		// Token: 0x04001005 RID: 4101
		protected float SpawningDelayTimer;

		// Token: 0x04001006 RID: 4102
		private bool _hasCalledSpawningEnded;

		// Token: 0x04001007 RID: 4103
		protected MissionLobbyComponent MissionLobbyComponent;

		// Token: 0x04001008 RID: 4104
		protected MissionLobbyEquipmentNetworkComponent MissionLobbyEquipmentNetworkComponent;

		// Token: 0x0400100B RID: 4107
		private List<AgentBuildData> _agentsToBeSpawnedCache;

		// Token: 0x0400100C RID: 4108
		private MissionTime _nextTimeToCleanUpMounts;

		// Token: 0x0400100E RID: 4110
		private int[] _botsCountForSides;

		// Token: 0x020005B8 RID: 1464
		// (Invoke) Token: 0x06003E30 RID: 15920
		public delegate void OnSpawningEndedEventDelegate();
	}
}
