using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Conversation.MissionLogics;
using SandBox.Missions.AgentBehaviors;
using SandBox.Missions.MissionLogics.Hideout.Objectives;
using SandBox.Objects.AnimationPoints;
using SandBox.Objects.AreaMarkers;
using SandBox.Objects.Usables;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Missions.MissionLogics;

namespace SandBox.Missions.MissionLogics.Hideout
{
	// Token: 0x02000094 RID: 148
	public class HideoutMissionController : MissionLogic, IMissionAgentSpawnLogic, IMissionBehavior
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x00029BF3 File Offset: 0x00027DF3
		// (set) Token: 0x06000622 RID: 1570 RVA: 0x00029BFB File Offset: 0x00027DFB
		public BattleSideEnum PlayerSide { get; private set; }

		// Token: 0x06000623 RID: 1571 RVA: 0x00029C04 File Offset: 0x00027E04
		public HideoutMissionController(IMissionTroopSupplier[] suppliers, BattleSideEnum playerSide, int firstPhaseEnemyTroopCount, int firstPhasePlayerSideTroopCount)
		{
			this.PlayerSide = playerSide;
			this._areaMarkers = new List<CommonAreaMarker>();
			this._patrolAreas = new List<PatrolArea>();
			this._defenderAgentObjects = new Dictionary<Agent, HideoutMissionController.UsedObject>();
			this._firstPhaseEnemyTroopCount = firstPhaseEnemyTroopCount;
			this._firstPhasePlayerSideTroopCount = firstPhasePlayerSideTroopCount;
			this._overriddenHideoutBossCharacterObject = null;
			this._missionSides = new HideoutMissionController.MissionSide[2];
			for (int i = 0; i < 2; i++)
			{
				IMissionTroopSupplier missionTroopSupplier = suppliers[i];
				bool flag = i == (int)playerSide;
				this._missionSides[i] = new HideoutMissionController.MissionSide((BattleSideEnum)i, missionTroopSupplier, flag);
			}
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00029C91 File Offset: 0x00027E91
		public override void OnCreated()
		{
			base.OnCreated();
			base.Mission.DoesMissionRequireCivilianEquipment = false;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00029CA8 File Offset: 0x00027EA8
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._battleAgentLogic = base.Mission.GetMissionBehavior<BattleAgentLogic>();
			this._battleEndLogic = base.Mission.GetMissionBehavior<BattleEndLogic>();
			this._battleEndLogic.ChangeCanCheckForEndCondition(false);
			this._agentVictoryLogic = base.Mission.GetMissionBehavior<AgentVictoryLogic>();
			this._cinematicController = base.Mission.GetMissionBehavior<HideoutCinematicController>();
			this._missionObjectiveLogic = base.Mission.GetMissionBehavior<MissionObjectiveLogic>();
			base.Mission.IsMainAgentObjectInteractionEnabled = false;
			this._cinematicController = base.Mission.GetMissionBehavior<HideoutCinematicController>();
			foreach (StealthAreaUsePoint stealthAreaUsePoint in base.Mission.MissionObjects.FindAllWithType<StealthAreaUsePoint>())
			{
				stealthAreaUsePoint.DisableStealthAreaUsePoint();
			}
			base.Mission.GetAgentTroopClass_Override += this.GetHideoutMissionTroopClass;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00029D98 File Offset: 0x00027F98
		public override void OnObjectStoppedBeingUsed(Agent userAgent, UsableMissionObject usedObject)
		{
			if (usedObject != null && usedObject is AnimationPoint && userAgent.IsActive() && userAgent.IsAIControlled && userAgent.CurrentWatchState == Agent.WatchState.Patrolling)
			{
				PatrolArea firstScriptOfType = usedObject.GameEntity.Parent.GetFirstScriptOfType<PatrolArea>();
				if (firstScriptOfType == null)
				{
					return;
				}
				((IDetachment)firstScriptOfType).AddAgent(userAgent, -1, Agent.AIScriptedFrameFlags.None);
			}
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00029DEC File Offset: 0x00027FEC
		public override void OnAgentAlarmedStateChanged(Agent agent, Agent.AIStateFlag flag)
		{
			if (this._hideoutMissionState < HideoutMissionController.HideoutMissionState.ConversationBetweenLeaders && agent.Team == base.Mission.DefenderTeam)
			{
				bool flag2 = (flag & Agent.AIStateFlag.Alarmed) == Agent.AIStateFlag.Alarmed;
				if (flag2 || (flag & Agent.AIStateFlag.Alarmed) == Agent.AIStateFlag.Cautious)
				{
					if (agent.IsUsingGameObject)
					{
						agent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
					}
					else
					{
						agent.DisableScriptedMovement();
						if (agent.IsAIControlled && agent.AIMoveToGameObjectIsEnabled())
						{
							agent.AIMoveToGameObjectDisable();
							Formation formation = agent.Formation;
							if (formation != null)
							{
								formation.Team.DetachmentManager.RemoveScoresOfAgentFromDetachments(agent);
							}
						}
					}
					this._defenderAgentObjects[agent].IsMachineAITicked = false;
				}
				else if ((flag & Agent.AIStateFlag.Alarmed) == Agent.AIStateFlag.None)
				{
					this._defenderAgentObjects[agent].IsMachineAITicked = true;
					agent.TryToSheathWeaponInHand(Agent.HandIndex.MainHand, Agent.WeaponWieldActionType.WithAnimation);
					((IDetachment)this._defenderAgentObjects[agent].Machine).AddAgent(agent, -1, Agent.AIScriptedFrameFlags.None);
				}
				if (flag2)
				{
					agent.SetWantsToYell();
				}
			}
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00029EC8 File Offset: 0x000280C8
		public override void OnMissionTick(float dt)
		{
			if (!this._isMissionInitialized)
			{
				this.InitializeMission();
				this._isMissionInitialized = true;
				return;
			}
			if (!this._troopsInitialized)
			{
				this._troopsInitialized = true;
				foreach (Agent agent in base.Mission.Agents)
				{
					this._battleAgentLogic.OnAgentBuild(agent, null);
				}
			}
			this.UsedObjectTick(dt);
			if (!this._battleResolved)
			{
				this.CheckBattleResolved();
			}
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00029F60 File Offset: 0x00028160
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (this._clearObjectiveTargetAgents.Contains(affectedAgent))
			{
				this._clearObjectiveTargetAgents.Remove(affectedAgent);
			}
			if (this._hideoutMissionState == HideoutMissionController.HideoutMissionState.BossFightWithDuel)
			{
				using (List<Agent>.Enumerator enumerator = base.Mission.Agents.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Agent agent = enumerator.Current;
						if (agent != affectedAgent && agent != affectorAgent && agent.IsActive() && agent.GetLookAgent() == affectedAgent)
						{
							agent.SetLookAgent(null);
						}
					}
					return;
				}
			}
			if (this._hideoutMissionState == HideoutMissionController.HideoutMissionState.InitialFightBeforeBossFight && affectedAgent.IsMainAgent)
			{
				base.Mission.PlayerTeam.PlayerOrderController.SelectAllFormations(false);
				affectedAgent.Formation = null;
				base.Mission.PlayerTeam.PlayerOrderController.SetOrder(OrderType.Retreat);
			}
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0002A03C File Offset: 0x0002823C
		public override void OnMissionStateFinalized()
		{
			base.Mission.GetAgentTroopClass_Override -= this.GetHideoutMissionTroopClass;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0002A055 File Offset: 0x00028255
		public void SetOverriddenHideoutBossCharacterObject(CharacterObject characterObject)
		{
			this._overriddenHideoutBossCharacterObject = characterObject;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0002A060 File Offset: 0x00028260
		private void InitializeMission()
		{
			base.Mission.GetMissionBehavior<MissionConversationLogic>().DisableStartConversation(true);
			base.Mission.SetMissionMode(MissionMode.Stealth, true);
			this._areaMarkers.AddRange(from area in base.Mission.ActiveMissionObjects.FindAllWithType<CommonAreaMarker>()
				orderby area.AreaIndex
				select area);
			this._patrolAreas.AddRange(from area in base.Mission.ActiveMissionObjects.FindAllWithType<PatrolArea>()
				orderby area.AreaIndex
				select area);
			this.DecideMissionState();
			base.Mission.DeploymentPlan.MakeDefaultDeploymentPlans();
			for (int i = 0; i < 2; i++)
			{
				int num;
				if (this._missionSides[i].IsPlayerSide)
				{
					num = this._firstPhasePlayerSideTroopCount;
				}
				else
				{
					if (this._missionSides[i].NumberOfTroopsNotSupplied <= this._firstPhaseEnemyTroopCount)
					{
						Debug.FailedAssert("_missionSides[i].NumberOfTroopsNotSupplied <= _firstPhaseEnemyTroopCount", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Missions\\MissionLogics\\Hideout\\HideoutMissionController.cs", "InitializeMission", 569);
						this._firstPhaseEnemyTroopCount = (int)((float)this._missionSides[i].NumberOfTroopsNotSupplied * 0.7f);
					}
					num = ((this._hideoutMissionState == HideoutMissionController.HideoutMissionState.InitialFightBeforeBossFight) ? this._firstPhaseEnemyTroopCount : this._missionSides[i].NumberOfTroopsNotSupplied);
				}
				this._missionSides[i].SpawnTroops(this._areaMarkers, this._patrolAreas, this._defenderAgentObjects, num);
			}
			Mission.Current.OnDeploymentFinished();
			foreach (Agent agent in base.Mission.PlayerEnemyTeam.ActiveAgents)
			{
				this._clearObjectiveTargetAgents.Add(agent);
			}
			this._clearTheMainCampObjective = new ClearTheMainCampObjective(base.Mission, this._clearObjectiveTargetAgents);
			this._missionObjectiveLogic.StartObjective(this._clearTheMainCampObjective);
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0002A258 File Offset: 0x00028458
		private void UsedObjectTick(float dt)
		{
			foreach (KeyValuePair<Agent, HideoutMissionController.UsedObject> keyValuePair in this._defenderAgentObjects)
			{
				if (keyValuePair.Value.IsMachineAITicked)
				{
					keyValuePair.Value.MachineAI.Tick(keyValuePair.Key, null, null, dt);
				}
			}
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0002A2D0 File Offset: 0x000284D0
		protected override void OnEndMission()
		{
			int num = 0;
			if (this._hideoutMissionState == HideoutMissionController.HideoutMissionState.BossFightWithDuel)
			{
				if (Agent.Main == null || !Agent.Main.IsActive())
				{
					List<Agent> duelPhaseAllyAgents = this._duelPhaseAllyAgents;
					num = ((duelPhaseAllyAgents != null) ? duelPhaseAllyAgents.Count : 0);
				}
				else if (this._bossAgent == null || !this._bossAgent.IsActive())
				{
					PlayerEncounter.EnemySurrender = true;
				}
			}
			if (MobileParty.MainParty.MemberRoster.TotalHealthyCount <= num && MapEvent.PlayerMapEvent.BattleState == BattleState.None)
			{
				MapEvent.PlayerMapEvent.SetOverrideWinner(BattleSideEnum.Defender);
			}
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0002A354 File Offset: 0x00028554
		private void CheckBattleResolved()
		{
			if (this._hideoutMissionState != HideoutMissionController.HideoutMissionState.CutSceneBeforeBossFight && this._hideoutMissionState != HideoutMissionController.HideoutMissionState.ConversationBetweenLeaders)
			{
				if (this.IsSideDepleted(BattleSideEnum.Attacker))
				{
					if (this._hideoutMissionState == HideoutMissionController.HideoutMissionState.BossFightWithDuel)
					{
						this.OnDuelOver(BattleSideEnum.Defender);
					}
					this._battleEndLogic.ChangeCanCheckForEndCondition(true);
					this._battleResolved = true;
					this._missionObjectiveLogic.CompleteCurrentObjective();
					return;
				}
				if (this.IsSideDepleted(BattleSideEnum.Defender))
				{
					if (this._hideoutMissionState == HideoutMissionController.HideoutMissionState.InitialFightBeforeBossFight)
					{
						if (this._firstPhaseEndTimer == null)
						{
							this._firstPhaseEndTimer = new Timer(base.Mission.CurrentTime, 4f, true);
							this._oldMissionMode = Mission.Current.Mode;
							Mission.Current.SetMissionMode(MissionMode.CutScene, false);
							return;
						}
						if (this._firstPhaseEndTimer.Check(base.Mission.CurrentTime))
						{
							this._cinematicController.StartCinematic(new HideoutCinematicController.OnInitialFadeOutFinished(this.OnInitialFadeOutOver), new Action(this.OnCutSceneOver), 0.4f, 0.2f, 8f, false);
							this._missionObjectiveLogic.CompleteCurrentObjective();
							return;
						}
					}
					else
					{
						if (this._hideoutMissionState == HideoutMissionController.HideoutMissionState.BossFightWithDuel)
						{
							this.OnDuelOver(BattleSideEnum.Attacker);
						}
						this._battleEndLogic.ChangeCanCheckForEndCondition(true);
						MapEvent.PlayerMapEvent.SetOverrideWinner(BattleSideEnum.Attacker);
						this._battleResolved = true;
						this._missionObjectiveLogic.CompleteCurrentObjective();
					}
				}
			}
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0002A49B File Offset: 0x0002869B
		public void StartSpawner(BattleSideEnum side)
		{
			this._missionSides[(int)side].SetSpawnTroops(true);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0002A4AB File Offset: 0x000286AB
		public void StopSpawner(BattleSideEnum side)
		{
			this._missionSides[(int)side].SetSpawnTroops(false);
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0002A4BB File Offset: 0x000286BB
		public bool IsSideSpawnEnabled(BattleSideEnum side)
		{
			return this._missionSides[(int)side].TroopSpawningActive;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0002A4CA File Offset: 0x000286CA
		public float GetReinforcementInterval(BattleSideEnum battleSide = BattleSideEnum.None)
		{
			return 0f;
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0002A4D4 File Offset: 0x000286D4
		public unsafe bool IsSideDepleted(BattleSideEnum side)
		{
			bool flag = this._missionSides[(int)side].NumberOfActiveTroops == 0;
			if (!flag)
			{
				if ((Agent.Main == null || !Agent.Main.IsActive()) && side == BattleSideEnum.Attacker)
				{
					if (this._hideoutMissionState == HideoutMissionController.HideoutMissionState.BossFightWithDuel || this._hideoutMissionState == HideoutMissionController.HideoutMissionState.InitialFightBeforeBossFight)
					{
						flag = true;
					}
					else if (this._hideoutMissionState == HideoutMissionController.HideoutMissionState.WithoutBossFight || this._hideoutMissionState == HideoutMissionController.HideoutMissionState.BossFightWithAll)
					{
						bool flag2 = base.Mission.Teams.Attacker.FormationsIncludingEmpty.Any<Formation>(delegate(Formation f)
						{
							if (f.CountOfUnits > 0)
							{
								MovementOrder movementOrder = *f.GetReadonlyMovementOrderReference();
								return movementOrder.OrderType == OrderType.Charge;
							}
							return false;
						});
						bool flag3 = base.Mission.Teams.Defender.ActiveAgents.Any<Agent>((Agent t) => t.CurrentWatchState == Agent.WatchState.Alarmed);
						flag = !flag2 && !flag3;
					}
				}
				else if (side == BattleSideEnum.Defender && this._hideoutMissionState == HideoutMissionController.HideoutMissionState.BossFightWithDuel && (this._bossAgent == null || !this._bossAgent.IsActive()))
				{
					flag = true;
				}
			}
			else if (side == BattleSideEnum.Defender && this._hideoutMissionState == HideoutMissionController.HideoutMissionState.InitialFightBeforeBossFight && (Agent.Main == null || !Agent.Main.IsActive()))
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0002A60C File Offset: 0x0002880C
		private void DecideMissionState()
		{
			HideoutMissionController.MissionSide missionSide = this._missionSides[0];
			this._hideoutMissionState = ((!missionSide.IsPlayerSide) ? HideoutMissionController.HideoutMissionState.InitialFightBeforeBossFight : HideoutMissionController.HideoutMissionState.WithoutBossFight);
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0002A634 File Offset: 0x00028834
		private void SetWatchStateOfAIAgents(Agent.WatchState state)
		{
			foreach (Agent agent in base.Mission.Agents)
			{
				if (agent.IsAIControlled)
				{
					agent.SetWatchState(state);
				}
			}
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0002A694 File Offset: 0x00028894
		private void SpawnBossAndBodyguards()
		{
			HideoutMissionController.MissionSide missionSide = this._missionSides[0];
			MatrixFrame banditsInitialFrame = this._cinematicController.GetBanditsInitialFrame();
			missionSide.SpawnRemainingTroopsForBossFight(new List<MatrixFrame> { banditsInitialFrame }, missionSide.NumberOfTroopsNotSupplied, this._overriddenHideoutBossCharacterObject);
			this._bossAgent = this.SelectBossAgent();
			this._bossAgent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.MeleeForMainHand);
			foreach (Agent agent in this._enemyTeam.ActiveAgents)
			{
				if (agent != this._bossAgent)
				{
					agent.WieldInitialWeapons(Agent.WeaponWieldActionType.WithAnimationUninterruptible, Equipment.InitialWeaponEquipPreference.Any);
				}
			}
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0002A744 File Offset: 0x00028944
		private Agent SelectBossAgent()
		{
			Agent agent = null;
			Agent agent2 = null;
			foreach (Agent agent3 in base.Mission.Agents)
			{
				if (agent3.Team == this._enemyTeam && agent3.IsHuman)
				{
					if (agent3.IsHero)
					{
						agent = agent3;
						agent2 = agent3;
						break;
					}
					if (agent3.Character.Culture.IsBandit)
					{
						CultureObject cultureObject = agent3.Character.Culture as CultureObject;
						if (((cultureObject != null) ? cultureObject.BanditBoss : null) != null && ((CultureObject)agent3.Character.Culture).BanditBoss == agent3.Character)
						{
							agent = agent3;
						}
					}
					if (agent2 == null || agent3.Character.Level > agent2.Character.Level)
					{
						agent2 = agent3;
					}
				}
			}
			agent = agent ?? agent2;
			return agent;
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0002A840 File Offset: 0x00028A40
		private void OnInitialFadeOutOver(ref Agent playerAgent, ref List<Agent> playerCompanions, ref Agent bossAgent, ref List<Agent> bossCompanions, ref float placementPerturbation, ref float placementAngle)
		{
			this._hideoutMissionState = HideoutMissionController.HideoutMissionState.CutSceneBeforeBossFight;
			this._enemyTeam = base.Mission.PlayerEnemyTeam;
			this.SpawnBossAndBodyguards();
			base.Mission.PlayerTeam.SetIsEnemyOf(this._enemyTeam, false);
			this.SetWatchStateOfAIAgents(Agent.WatchState.Patrolling);
			if (Agent.Main.IsUsingGameObject)
			{
				Agent.Main.StopUsingGameObject(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
			}
			playerAgent = Agent.Main;
			playerCompanions = base.Mission.Agents.Where<Agent>((Agent x) => x.IsActive() && x.Team == base.Mission.PlayerTeam && x.IsHuman && x.IsAIControlled).ToList<Agent>();
			bossAgent = this._bossAgent;
			bossCompanions = base.Mission.Agents.Where<Agent>((Agent x) => x.IsActive() && x.Team == this._enemyTeam && x.IsHuman && x.IsAIControlled && x != this._bossAgent).ToList<Agent>();
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0002A8F7 File Offset: 0x00028AF7
		private void OnCutSceneOver()
		{
			Mission.Current.SetMissionMode(this._oldMissionMode, false);
			this._hideoutMissionState = HideoutMissionController.HideoutMissionState.ConversationBetweenLeaders;
			MissionConversationLogic missionBehavior = base.Mission.GetMissionBehavior<MissionConversationLogic>();
			missionBehavior.DisableStartConversation(false);
			missionBehavior.StartConversation(this._bossAgent, false, false);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0002A930 File Offset: 0x00028B30
		private void OnDuelOver(BattleSideEnum winnerSide)
		{
			AgentVictoryLogic missionBehavior = base.Mission.GetMissionBehavior<AgentVictoryLogic>();
			if (missionBehavior != null)
			{
				missionBehavior.SetCheerActionGroup(AgentVictoryLogic.CheerActionGroupEnum.HighCheerActions);
			}
			if (missionBehavior != null)
			{
				missionBehavior.SetCheerReactionTimerSettings(0.25f, 3f);
			}
			if (winnerSide == BattleSideEnum.Attacker && this._duelPhaseAllyAgents != null)
			{
				using (List<Agent>.Enumerator enumerator = this._duelPhaseAllyAgents.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Agent agent = enumerator.Current;
						if (agent.State == AgentState.Active)
						{
							agent.SetTeam(base.Mission.PlayerTeam, true);
							agent.SetWatchState(Agent.WatchState.Alarmed);
						}
					}
					return;
				}
			}
			if (winnerSide == BattleSideEnum.Defender && this._duelPhaseBanditAgents != null)
			{
				foreach (Agent agent2 in this._duelPhaseBanditAgents)
				{
					if (agent2.State == AgentState.Active)
					{
						agent2.SetTeam(this._enemyTeam, true);
						agent2.SetWatchState(Agent.WatchState.Alarmed);
					}
				}
			}
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0002AA3C File Offset: 0x00028C3C
		private FormationClass GetHideoutMissionTroopClass(BattleSideEnum battleSide, BasicCharacterObject agentCharacter)
		{
			return agentCharacter.GetFormationClass().DismountedClass();
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0002AA49 File Offset: 0x00028C49
		public static void StartBossFightDuelMode()
		{
			Mission mission = Mission.Current;
			HideoutMissionController hideoutMissionController = ((mission != null) ? mission.GetMissionBehavior<HideoutMissionController>() : null);
			if (hideoutMissionController == null)
			{
				return;
			}
			hideoutMissionController.StartBossFightDuelModeInternal();
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0002AA68 File Offset: 0x00028C68
		private void StartBossFightDuelModeInternal()
		{
			base.Mission.GetMissionBehavior<MissionConversationLogic>().DisableStartConversation(true);
			base.Mission.PlayerTeam.SetIsEnemyOf(this._enemyTeam, true);
			this._duelPhaseAllyAgents = base.Mission.Agents.Where<Agent>((Agent x) => x.IsActive() && x.Team == base.Mission.PlayerTeam && x.IsHuman && x.IsAIControlled && x != Agent.Main).ToList<Agent>();
			this._duelPhaseBanditAgents = base.Mission.Agents.Where<Agent>((Agent x) => x.IsActive() && x.Team == this._enemyTeam && x.IsHuman && x.IsAIControlled && x != this._bossAgent).ToList<Agent>();
			foreach (Agent agent in this._duelPhaseAllyAgents)
			{
				agent.SetTeam(Team.Invalid, true);
				WorldPosition worldPosition = agent.GetWorldPosition();
				agent.SetScriptedPosition(ref worldPosition, false, Agent.AIScriptedFrameFlags.None);
				agent.SetLookAgent(Agent.Main);
			}
			foreach (Agent agent2 in this._duelPhaseBanditAgents)
			{
				agent2.SetTeam(Team.Invalid, true);
				WorldPosition worldPosition2 = agent2.GetWorldPosition();
				agent2.SetScriptedPosition(ref worldPosition2, false, Agent.AIScriptedFrameFlags.None);
				agent2.SetLookAgent(this._bossAgent);
			}
			this._bossAgent.SetWatchState(Agent.WatchState.Alarmed);
			this._hideoutMissionState = HideoutMissionController.HideoutMissionState.BossFightWithDuel;
			this._defeatHideoutBossObjective = new DefeatHideoutBossObjective(base.Mission, true);
			this._missionObjectiveLogic.StartObjective(this._defeatHideoutBossObjective);
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x0002ABE8 File Offset: 0x00028DE8
		public static void StartBossFightBattleMode()
		{
			Mission mission = Mission.Current;
			HideoutMissionController hideoutMissionController = ((mission != null) ? mission.GetMissionBehavior<HideoutMissionController>() : null);
			if (hideoutMissionController == null)
			{
				return;
			}
			hideoutMissionController.StartBossFightBattleModeInternal();
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0002AC08 File Offset: 0x00028E08
		private void StartBossFightBattleModeInternal()
		{
			base.Mission.GetMissionBehavior<MissionConversationLogic>().DisableStartConversation(true);
			base.Mission.PlayerTeam.SetIsEnemyOf(this._enemyTeam, true);
			this.SetWatchStateOfAIAgents(Agent.WatchState.Alarmed);
			this._hideoutMissionState = HideoutMissionController.HideoutMissionState.BossFightWithAll;
			foreach (Formation formation in base.Mission.PlayerTeam.FormationsIncludingEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					formation.SetMovementOrder(MovementOrder.MovementOrderCharge);
					formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
				}
			}
			this._defeatHideoutBossObjective = new DefeatHideoutBossObjective(base.Mission, false);
			this._missionObjectiveLogic.StartObjective(this._defeatHideoutBossObjective);
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0002ACD8 File Offset: 0x00028ED8
		public IEnumerable<IAgentOriginBase> GetAllTroopsForSide(BattleSideEnum side)
		{
			return this._missionSides[(int)side].GetAllTroops();
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0002ACF4 File Offset: 0x00028EF4
		public int GetNumberOfPlayerControllableTroops()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0002ACFB File Offset: 0x00028EFB
		public bool GetSpawnHorses(BattleSideEnum side)
		{
			return false;
		}

		// Token: 0x0400034B RID: 843
		private const int FirstPhaseEndInSeconds = 4;

		// Token: 0x0400034D RID: 845
		private readonly List<CommonAreaMarker> _areaMarkers;

		// Token: 0x0400034E RID: 846
		private readonly List<PatrolArea> _patrolAreas;

		// Token: 0x0400034F RID: 847
		private readonly Dictionary<Agent, HideoutMissionController.UsedObject> _defenderAgentObjects;

		// Token: 0x04000350 RID: 848
		private readonly HideoutMissionController.MissionSide[] _missionSides;

		// Token: 0x04000351 RID: 849
		private List<Agent> _duelPhaseAllyAgents;

		// Token: 0x04000352 RID: 850
		private List<Agent> _duelPhaseBanditAgents;

		// Token: 0x04000353 RID: 851
		private BattleAgentLogic _battleAgentLogic;

		// Token: 0x04000354 RID: 852
		private BattleEndLogic _battleEndLogic;

		// Token: 0x04000355 RID: 853
		private AgentVictoryLogic _agentVictoryLogic;

		// Token: 0x04000356 RID: 854
		private HideoutMissionController.HideoutMissionState _hideoutMissionState;

		// Token: 0x04000357 RID: 855
		private Agent _bossAgent;

		// Token: 0x04000358 RID: 856
		private Team _enemyTeam;

		// Token: 0x04000359 RID: 857
		private Timer _firstPhaseEndTimer;

		// Token: 0x0400035A RID: 858
		private CharacterObject _overriddenHideoutBossCharacterObject;

		// Token: 0x0400035B RID: 859
		private bool _troopsInitialized;

		// Token: 0x0400035C RID: 860
		private bool _isMissionInitialized;

		// Token: 0x0400035D RID: 861
		private bool _battleResolved;

		// Token: 0x0400035E RID: 862
		private int _firstPhaseEnemyTroopCount;

		// Token: 0x0400035F RID: 863
		private int _firstPhasePlayerSideTroopCount;

		// Token: 0x04000360 RID: 864
		private MissionMode _oldMissionMode;

		// Token: 0x04000361 RID: 865
		private HideoutCinematicController _cinematicController;

		// Token: 0x04000362 RID: 866
		private MissionObjectiveLogic _missionObjectiveLogic;

		// Token: 0x04000363 RID: 867
		private ClearTheMainCampObjective _clearTheMainCampObjective;

		// Token: 0x04000364 RID: 868
		private DefeatHideoutBossObjective _defeatHideoutBossObjective;

		// Token: 0x04000365 RID: 869
		private readonly List<Agent> _clearObjectiveTargetAgents = new List<Agent>();

		// Token: 0x020001A0 RID: 416
		private class MissionSide
		{
			// Token: 0x17000138 RID: 312
			// (get) Token: 0x06000EFD RID: 3837 RVA: 0x00066BED File Offset: 0x00064DED
			// (set) Token: 0x06000EFE RID: 3838 RVA: 0x00066BF5 File Offset: 0x00064DF5
			public bool TroopSpawningActive { get; private set; }

			// Token: 0x17000139 RID: 313
			// (get) Token: 0x06000EFF RID: 3839 RVA: 0x00066BFE File Offset: 0x00064DFE
			public int NumberOfActiveTroops
			{
				get
				{
					return this._numberOfSpawnedTroops - this._troopSupplier.NumRemovedTroops;
				}
			}

			// Token: 0x1700013A RID: 314
			// (get) Token: 0x06000F00 RID: 3840 RVA: 0x00066C12 File Offset: 0x00064E12
			public int NumberOfTroopsNotSupplied
			{
				get
				{
					return this._troopSupplier.NumTroopsNotSupplied;
				}
			}

			// Token: 0x06000F01 RID: 3841 RVA: 0x00066C1F File Offset: 0x00064E1F
			public MissionSide(BattleSideEnum side, IMissionTroopSupplier troopSupplier, bool isPlayerSide)
			{
				this._side = side;
				this.IsPlayerSide = isPlayerSide;
				this._troopSupplier = troopSupplier;
			}

			// Token: 0x06000F02 RID: 3842 RVA: 0x00066C3C File Offset: 0x00064E3C
			public void SpawnTroops(List<CommonAreaMarker> areaMarkers, List<PatrolArea> patrolAreas, Dictionary<Agent, HideoutMissionController.UsedObject> defenderAgentObjects, int spawnCount)
			{
				int num = 0;
				bool flag = false;
				List<StandingPoint> list = new List<StandingPoint>();
				foreach (CommonAreaMarker commonAreaMarker in areaMarkers)
				{
					foreach (UsableMachine usableMachine in commonAreaMarker.GetUsableMachinesInRange(null))
					{
						list.AddRange(usableMachine.StandingPoints);
					}
				}
				List<IAgentOriginBase> list2 = this._troopSupplier.SupplyTroops(spawnCount).ToList<IAgentOriginBase>();
				for (int i = 0; i < list2.Count; i++)
				{
					if (BattleSideEnum.Attacker == this._side)
					{
						Mission.Current.SpawnTroop(list2[i], true, true, false, false, 0, 0, true, true, null, null, null, null, FormationClass.NumberOfAllFormations, false);
						this._numberOfSpawnedTroops++;
					}
					else if (areaMarkers.Count > num)
					{
						StandingPoint standingPoint = null;
						int num2 = list2.Count - i;
						if (num2 < list.Count / 2 && num2 < 4)
						{
							flag = true;
						}
						if (!flag)
						{
							list.Shuffle<StandingPoint>();
							standingPoint = list.FirstOrDefault<StandingPoint>((StandingPoint point) => !point.IsDeactivated && !point.IsDisabled && !point.HasUser);
						}
						else
						{
							IEnumerable<PatrolArea> enumerable = patrolAreas.Where<PatrolArea>((PatrolArea area) => area.StandingPoints.All<StandingPoint>((StandingPoint point) => !point.HasUser && !point.HasAIMovingTo));
							if (!enumerable.IsEmpty<PatrolArea>())
							{
								foreach (StandingPoint standingPoint2 in enumerable.First<PatrolArea>().StandingPoints)
								{
									if (!standingPoint2.IsDisabled)
									{
										standingPoint = standingPoint2;
										break;
									}
								}
							}
						}
						if (standingPoint != null && !standingPoint.IsDisabled)
						{
							MatrixFrame globalFrame = standingPoint.GameEntity.GetGlobalFrame();
							globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
							Agent agent = Mission.Current.SpawnTroop(list2[i], false, false, false, false, 0, 0, false, false, new Vec3?(globalFrame.origin), new Vec2?(globalFrame.rotation.f.AsVec2.Normalized()), "_hideout_bandit", null, FormationClass.NumberOfAllFormations, false);
							this.InitializeBanditAgent(agent, standingPoint, flag, defenderAgentObjects);
							this._numberOfSpawnedTroops++;
							int groupId = ((AnimationPoint)standingPoint).GroupId;
							if (flag)
							{
								goto IL_02CE;
							}
							using (List<StandingPoint>.Enumerator enumerator3 = standingPoint.GameEntity.Parent.GetFirstScriptOfType<UsableMachine>().StandingPoints.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									StandingPoint standingPoint3 = enumerator3.Current;
									int groupId2 = ((AnimationPoint)standingPoint3).GroupId;
									if (groupId == groupId2 && standingPoint3 != standingPoint)
									{
										standingPoint3.SetDisabledAndMakeInvisible(false, false);
									}
								}
								goto IL_02CE;
							}
						}
						num++;
					}
					IL_02CE:;
				}
				foreach (Formation formation in Mission.Current.AttackerTeam.FormationsIncludingEmpty)
				{
					if (formation.CountOfUnits > 0)
					{
						formation.SetMovementOrder(MovementOrder.MovementOrderMove(formation.CachedMedianPosition));
					}
					formation.SetFiringOrder(FiringOrder.FiringOrderHoldYourFire);
					if (Mission.Current.AttackerTeam == Mission.Current.PlayerTeam)
					{
						formation.PlayerOwner = Mission.Current.MainAgent;
					}
				}
			}

			// Token: 0x06000F03 RID: 3843 RVA: 0x00066FF4 File Offset: 0x000651F4
			public void SpawnRemainingTroopsForBossFight(List<MatrixFrame> spawnFrames, int spawnCount, CharacterObject overriddenHideoutBossCharacterObject)
			{
				List<IAgentOriginBase> list = this._troopSupplier.SupplyTroops(spawnCount).ToList<IAgentOriginBase>();
				if (overriddenHideoutBossCharacterObject != null)
				{
					IAgentOriginBase agentOriginBase = list.Find((IAgentOriginBase t) => t.Troop == overriddenHideoutBossCharacterObject);
					MatrixFrame matrixFrame = spawnFrames.FirstOrDefault<MatrixFrame>();
					matrixFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
					Agent agent = Mission.Current.SpawnTroop(agentOriginBase, false, false, false, false, 0, 0, false, false, new Vec3?(matrixFrame.origin), new Vec2?(matrixFrame.rotation.f.AsVec2.Normalized()), "_hideout_bandit", null, FormationClass.NumberOfAllFormations, false);
					this._numberOfSpawnedTroops++;
					AgentFlag agentFlags = agent.GetAgentFlags();
					if (agentFlags.HasAnyFlag(AgentFlag.CanRetreat))
					{
						agent.SetAgentFlags(agentFlags & ~AgentFlag.CanRetreat);
					}
					list.Remove(agentOriginBase);
				}
				for (int i = 0; i < list.Count; i++)
				{
					MatrixFrame matrixFrame2 = spawnFrames.FirstOrDefault<MatrixFrame>();
					matrixFrame2.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
					Agent agent2 = Mission.Current.SpawnTroop(list[i], false, false, false, false, 0, 0, false, false, new Vec3?(matrixFrame2.origin), new Vec2?(matrixFrame2.rotation.f.AsVec2.Normalized()), "_hideout_bandit", null, FormationClass.NumberOfAllFormations, false);
					AgentFlag agentFlags2 = agent2.GetAgentFlags();
					if (agentFlags2.HasAnyFlag(AgentFlag.CanRetreat))
					{
						agent2.SetAgentFlags(agentFlags2 & ~AgentFlag.CanRetreat);
					}
					this._numberOfSpawnedTroops++;
				}
				foreach (Formation formation in Mission.Current.AttackerTeam.FormationsIncludingEmpty)
				{
					if (formation.CountOfUnits > 0)
					{
						formation.SetMovementOrder(MovementOrder.MovementOrderMove(formation.CachedMedianPosition));
					}
					formation.SetFiringOrder(FiringOrder.FiringOrderHoldYourFire);
					if (Mission.Current.AttackerTeam == Mission.Current.PlayerTeam)
					{
						formation.PlayerOwner = Mission.Current.MainAgent;
					}
				}
			}

			// Token: 0x06000F04 RID: 3844 RVA: 0x00067224 File Offset: 0x00065424
			private void InitializeBanditAgent(Agent agent, StandingPoint spawnPoint, bool isPatrolling, Dictionary<Agent, HideoutMissionController.UsedObject> defenderAgentObjects)
			{
				UsableMachine usableMachine = (isPatrolling ? spawnPoint.GameEntity.Parent.GetFirstScriptOfType<PatrolArea>() : spawnPoint.GameEntity.Parent.GetFirstScriptOfType<UsableMachine>());
				if (isPatrolling)
				{
					((IDetachment)usableMachine).AddAgent(agent, -1, Agent.AIScriptedFrameFlags.None);
					agent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp, Equipment.InitialWeaponEquipPreference.Any);
				}
				else
				{
					agent.UseGameObject(spawnPoint, -1);
				}
				defenderAgentObjects.Add(agent, new HideoutMissionController.UsedObject(usableMachine, isPatrolling));
				AgentFlag agentFlags = agent.GetAgentFlags();
				agent.SetAgentFlags((agentFlags | AgentFlag.CanGetAlarmed) & ~AgentFlag.CanRetreat);
				agent.GetComponent<CampaignAgentComponent>().CreateAgentNavigator().AddBehaviorGroup<AlarmedBehaviorGroup>()
					.AddBehavior<CautiousBehavior>();
				this.SimulateTick(agent);
			}

			// Token: 0x06000F05 RID: 3845 RVA: 0x000672C8 File Offset: 0x000654C8
			private void SimulateTick(Agent agent)
			{
				int num = MBRandom.RandomInt(1, 20);
				for (int i = 0; i < num; i++)
				{
					if (agent.IsUsingGameObject)
					{
						agent.CurrentlyUsedGameObject.SimulateTick(0.1f);
					}
				}
			}

			// Token: 0x06000F06 RID: 3846 RVA: 0x00067302 File Offset: 0x00065502
			public void SetSpawnTroops(bool spawnTroops)
			{
				this.TroopSpawningActive = spawnTroops;
			}

			// Token: 0x06000F07 RID: 3847 RVA: 0x0006730B File Offset: 0x0006550B
			public IEnumerable<IAgentOriginBase> GetAllTroops()
			{
				return this._troopSupplier.GetAllTroops();
			}

			// Token: 0x040007C3 RID: 1987
			private readonly BattleSideEnum _side;

			// Token: 0x040007C4 RID: 1988
			private readonly IMissionTroopSupplier _troopSupplier;

			// Token: 0x040007C5 RID: 1989
			public readonly bool IsPlayerSide;

			// Token: 0x040007C7 RID: 1991
			private int _numberOfSpawnedTroops;
		}

		// Token: 0x020001A1 RID: 417
		private class UsedObject
		{
			// Token: 0x06000F08 RID: 3848 RVA: 0x00067318 File Offset: 0x00065518
			public UsedObject(UsableMachine machine, bool isMachineAITicked)
			{
				this.Machine = machine;
				this.MachineAI = machine.CreateAIBehaviorObject();
				this.IsMachineAITicked = isMachineAITicked;
			}

			// Token: 0x040007C8 RID: 1992
			public readonly UsableMachine Machine;

			// Token: 0x040007C9 RID: 1993
			public readonly UsableMachineAIBase MachineAI;

			// Token: 0x040007CA RID: 1994
			public bool IsMachineAITicked;
		}

		// Token: 0x020001A2 RID: 418
		private enum HideoutMissionState
		{
			// Token: 0x040007CC RID: 1996
			NotDecided,
			// Token: 0x040007CD RID: 1997
			WithoutBossFight,
			// Token: 0x040007CE RID: 1998
			InitialFightBeforeBossFight,
			// Token: 0x040007CF RID: 1999
			CutSceneBeforeBossFight,
			// Token: 0x040007D0 RID: 2000
			ConversationBetweenLeaders,
			// Token: 0x040007D1 RID: 2001
			BossFightWithDuel,
			// Token: 0x040007D2 RID: 2002
			BossFightWithAll
		}
	}
}
