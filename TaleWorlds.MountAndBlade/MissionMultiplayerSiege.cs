using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.MissionRepresentatives;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.MountAndBlade.Objects;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002B7 RID: 695
	public class MissionMultiplayerSiege : MissionMultiplayerGameModeBase, IAnalyticsFlagInfo, IMissionBehavior
	{
		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x060027AE RID: 10158 RVA: 0x00094B91 File Offset: 0x00092D91
		public override bool IsGameModeHidingAllAgentVisuals
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x060027AF RID: 10159 RVA: 0x00094B94 File Offset: 0x00092D94
		public override bool IsGameModeUsingOpposingTeams
		{
			get
			{
				return true;
			}
		}

		// Token: 0x14000062 RID: 98
		// (add) Token: 0x060027B0 RID: 10160 RVA: 0x00094B98 File Offset: 0x00092D98
		// (remove) Token: 0x060027B1 RID: 10161 RVA: 0x00094BD0 File Offset: 0x00092DD0
		public event MissionMultiplayerSiege.OnDestructableComponentDestroyedDelegate OnDestructableComponentDestroyed;

		// Token: 0x14000063 RID: 99
		// (add) Token: 0x060027B2 RID: 10162 RVA: 0x00094C08 File Offset: 0x00092E08
		// (remove) Token: 0x060027B3 RID: 10163 RVA: 0x00094C40 File Offset: 0x00092E40
		public event MissionMultiplayerSiege.OnObjectiveGoldGainedDelegate OnObjectiveGoldGained;

		// Token: 0x17000793 RID: 1939
		// (get) Token: 0x060027B4 RID: 10164 RVA: 0x00094C75 File Offset: 0x00092E75
		// (set) Token: 0x060027B5 RID: 10165 RVA: 0x00094C7D File Offset: 0x00092E7D
		public MBReadOnlyList<FlagCapturePoint> AllCapturePoints { get; private set; }

		// Token: 0x060027B6 RID: 10166 RVA: 0x00094C88 File Offset: 0x00092E88
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._objectiveSystem = new MissionMultiplayerSiege.ObjectiveSystem();
			this._childDestructableComponents = new Dictionary<GameEntity, List<DestructableComponent>>();
			this._gameModeSiegeClient = Mission.Current.GetMissionBehavior<MissionMultiplayerSiegeClient>();
			this._warmupComponent = Mission.Current.GetMissionBehavior<MultiplayerWarmupComponent>();
			this._capturePointOwners = new Team[7];
			this._capturePointRemainingMoraleGains = new int[7];
			this._morales = new int[2];
			this._morales[1] = 360;
			this._morales[0] = 360;
			this.AllCapturePoints = Mission.Current.MissionObjects.FindAllWithType<FlagCapturePoint>().ToMBList<FlagCapturePoint>();
			foreach (FlagCapturePoint flagCapturePoint in this.AllCapturePoints)
			{
				flagCapturePoint.SetTeamColorsSynched(4284111450U, uint.MaxValue);
				this._capturePointOwners[flagCapturePoint.FlagIndex] = null;
				this._capturePointRemainingMoraleGains[flagCapturePoint.FlagIndex] = 90;
				if (flagCapturePoint.GameEntity.HasTag("keep_capture_point"))
				{
					this._masterFlag = flagCapturePoint;
				}
			}
			foreach (DestructableComponent destructableComponent in Mission.Current.MissionObjects.FindAllWithType<DestructableComponent>())
			{
				if (destructableComponent.BattleSide != BattleSideEnum.None)
				{
					GameEntity gameEntity = GameEntity.CreateFromWeakEntity(destructableComponent.GameEntity.Root);
					if (this._objectiveSystem.RegisterObjective(gameEntity))
					{
						this._childDestructableComponents.Add(gameEntity, new List<DestructableComponent>());
						MissionMultiplayerSiege.GetDestructableCompoenentClosestToTheRoot(gameEntity).OnDestroyed += new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.DestructableComponentOnDestroyed);
					}
					this._childDestructableComponents[gameEntity].Add(destructableComponent);
					destructableComponent.OnHitTaken += new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.DestructableComponentOnHitTaken);
				}
			}
			List<RangedSiegeWeapon> list = new List<RangedSiegeWeapon>();
			List<IMoveableSiegeWeapon> list2 = new List<IMoveableSiegeWeapon>();
			foreach (UsableMachine usableMachine in Mission.Current.MissionObjects.FindAllWithType<UsableMachine>())
			{
				RangedSiegeWeapon rangedSiegeWeapon;
				IMoveableSiegeWeapon moveableSiegeWeapon;
				if ((rangedSiegeWeapon = usableMachine as RangedSiegeWeapon) != null)
				{
					list.Add(rangedSiegeWeapon);
					rangedSiegeWeapon.OnAgentLoadsMachine += this.RangedSiegeMachineOnAgentLoadsMachine;
				}
				else if ((moveableSiegeWeapon = usableMachine as IMoveableSiegeWeapon) != null)
				{
					list2.Add(moveableSiegeWeapon);
					this._objectiveSystem.RegisterObjective(GameEntity.CreateFromWeakEntity(usableMachine.GameEntity.Root));
				}
			}
			this._lastReloadingAgentPerRangedSiegeMachine = new ValueTuple<RangedSiegeWeapon, Agent>[list.Count];
			for (int i = 0; i < this._lastReloadingAgentPerRangedSiegeMachine.Length; i++)
			{
				this._lastReloadingAgentPerRangedSiegeMachine[i] = ValueTuple.Create<RangedSiegeWeapon, Agent>(list[i], null);
			}
			this._movingObjectives = new ValueTuple<IMoveableSiegeWeapon, Vec3>[list2.Count];
			for (int j = 0; j < this._movingObjectives.Length; j++)
			{
				SiegeWeapon siegeWeapon = list2[j] as SiegeWeapon;
				this._movingObjectives[j] = ValueTuple.Create<IMoveableSiegeWeapon, Vec3>(list2[j], siegeWeapon.GameEntity.GlobalPosition);
			}
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x00094FCC File Offset: 0x000931CC
		private static DestructableComponent GetDestructableCompoenentClosestToTheRoot(GameEntity entity)
		{
			DestructableComponent destructableComponent = entity.GetFirstScriptOfType<DestructableComponent>();
			while (destructableComponent == null && entity.ChildCount != 0)
			{
				for (int i = 0; i < entity.ChildCount; i++)
				{
					destructableComponent = MissionMultiplayerSiege.GetDestructableCompoenentClosestToTheRoot(entity.GetChild(i));
					if (destructableComponent != null)
					{
						break;
					}
				}
			}
			return destructableComponent;
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x00095010 File Offset: 0x00093210
		private void RangedSiegeMachineOnAgentLoadsMachine(RangedSiegeWeapon siegeWeapon, Agent reloadingAgent)
		{
			for (int i = 0; i < this._lastReloadingAgentPerRangedSiegeMachine.Length; i++)
			{
				if (this._lastReloadingAgentPerRangedSiegeMachine[i].Item1 == siegeWeapon)
				{
					this._lastReloadingAgentPerRangedSiegeMachine[i].Item2 = reloadingAgent;
				}
			}
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x00095058 File Offset: 0x00093258
		private void DestructableComponentOnHitTaken(DestructableComponent destructableComponent, Agent attackerAgent, in MissionWeapon weapon, ScriptComponentBehavior attackerScriptComponentBehavior, int inflictedDamage)
		{
			if (!this.WarmupComponent.IsInWarmup)
			{
				GameEntity gameEntity = GameEntity.CreateFromWeakEntity(destructableComponent.GameEntity.Root);
				BatteringRam batteringRam;
				if ((batteringRam = attackerScriptComponentBehavior as BatteringRam) != null)
				{
					int userCountNotInStruckAction = batteringRam.UserCountNotInStruckAction;
					if (userCountNotInStruckAction <= 0)
					{
						goto IL_0234;
					}
					float num = (float)inflictedDamage / (float)userCountNotInStruckAction;
					using (List<StandingPoint>.Enumerator enumerator = batteringRam.StandingPoints.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							StandingPoint standingPoint = enumerator.Current;
							Agent userAgent = standingPoint.UserAgent;
							if (((userAgent != null) ? userAgent.MissionPeer : null) != null && !userAgent.IsInBeingStruckAction && userAgent.MissionPeer.Team.Side == destructableComponent.BattleSide.GetOppositeSide())
							{
								this._objectiveSystem.AddContributionForObjective(gameEntity, userAgent.MissionPeer, num);
							}
						}
						goto IL_0234;
					}
				}
				bool flag;
				if (attackerAgent == null)
				{
					flag = null != null;
				}
				else
				{
					MissionPeer missionPeer = attackerAgent.MissionPeer;
					flag = ((missionPeer != null) ? missionPeer.Team : null) != null;
				}
				if (flag && attackerAgent.MissionPeer.Team.Side == destructableComponent.BattleSide.GetOppositeSide())
				{
					StandingPoint standingPoint2;
					if (attackerAgent.CurrentlyUsedGameObject != null && (standingPoint2 = attackerAgent.CurrentlyUsedGameObject as StandingPoint) != null)
					{
						RangedSiegeWeapon firstScriptOfTypeInFamily = standingPoint2.GameEntity.GetFirstScriptOfTypeInFamily<RangedSiegeWeapon>();
						if (firstScriptOfTypeInFamily != null)
						{
							for (int i = 0; i < this._lastReloadingAgentPerRangedSiegeMachine.Length; i++)
							{
								if (this._lastReloadingAgentPerRangedSiegeMachine[i].Item1 == firstScriptOfTypeInFamily)
								{
									Agent item = this._lastReloadingAgentPerRangedSiegeMachine[i].Item2;
									if (((item != null) ? item.MissionPeer : null) != null)
									{
										Agent item2 = this._lastReloadingAgentPerRangedSiegeMachine[i].Item2;
										BattleSideEnum? battleSideEnum = ((item2 != null) ? new BattleSideEnum?(item2.MissionPeer.Team.Side) : null);
										BattleSideEnum oppositeSide = destructableComponent.BattleSide.GetOppositeSide();
										if ((battleSideEnum.GetValueOrDefault() == oppositeSide) & (battleSideEnum != null))
										{
											this._objectiveSystem.AddContributionForObjective(gameEntity, this._lastReloadingAgentPerRangedSiegeMachine[i].Item2.MissionPeer, (float)inflictedDamage * 0.33f);
										}
									}
								}
							}
						}
					}
					this._objectiveSystem.AddContributionForObjective(gameEntity, attackerAgent.MissionPeer, (float)inflictedDamage);
				}
				IL_0234:
				if (destructableComponent.IsDestroyed)
				{
					destructableComponent.OnHitTaken -= new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.DestructableComponentOnHitTaken);
					this._childDestructableComponents[gameEntity].Remove(destructableComponent);
				}
			}
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x000952D8 File Offset: 0x000934D8
		private void DestructableComponentOnDestroyed(DestructableComponent destructableComponent, Agent attackerAgent, in MissionWeapon weapon, ScriptComponentBehavior attackerScriptComponentBehavior, int inflictedDamage)
		{
			GameEntity gameEntity = GameEntity.CreateFromWeakEntity(destructableComponent.GameEntity.Root);
			List<KeyValuePair<MissionPeer, float>> allContributorsForSideAndClear = this._objectiveSystem.GetAllContributorsForSideAndClear(gameEntity, destructableComponent.BattleSide.GetOppositeSide());
			float num = allContributorsForSideAndClear.Sum<KeyValuePair<MissionPeer, float>>((KeyValuePair<MissionPeer, float> ac) => ac.Value);
			List<MissionPeer> list = new List<MissionPeer>();
			foreach (KeyValuePair<MissionPeer, float> keyValuePair in allContributorsForSideAndClear)
			{
				int goldGainsFromObjectiveAssist = (keyValuePair.Key.Representative as SiegeMissionRepresentative).GetGoldGainsFromObjectiveAssist(gameEntity, keyValuePair.Value / num, false);
				if (goldGainsFromObjectiveAssist > 0)
				{
					base.ChangeCurrentGoldForPeer(keyValuePair.Key, keyValuePair.Key.Representative.Gold + goldGainsFromObjectiveAssist);
					list.Add(keyValuePair.Key);
					MissionMultiplayerSiege.OnObjectiveGoldGainedDelegate onObjectiveGoldGained = this.OnObjectiveGoldGained;
					if (onObjectiveGoldGained != null)
					{
						onObjectiveGoldGained(keyValuePair.Key, goldGainsFromObjectiveAssist);
					}
				}
			}
			destructableComponent.OnDestroyed -= new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.DestructableComponentOnDestroyed);
			foreach (DestructableComponent destructableComponent2 in this._childDestructableComponents[gameEntity])
			{
				destructableComponent2.OnHitTaken -= new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.DestructableComponentOnHitTaken);
			}
			this._childDestructableComponents.Remove(gameEntity);
			MissionMultiplayerSiege.OnDestructableComponentDestroyedDelegate onDestructableComponentDestroyed = this.OnDestructableComponentDestroyed;
			if (onDestructableComponentDestroyed == null)
			{
				return;
			}
			onDestructableComponentDestroyed(destructableComponent, attackerScriptComponentBehavior, list.ToArray());
		}

		// Token: 0x060027BB RID: 10171 RVA: 0x00095474 File Offset: 0x00093674
		public override MultiplayerGameType GetMissionType()
		{
			return MultiplayerGameType.Siege;
		}

		// Token: 0x060027BC RID: 10172 RVA: 0x00095477 File Offset: 0x00093677
		public override bool UseRoundController()
		{
			return false;
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x0009547C File Offset: 0x0009367C
		public override void AfterStart()
		{
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(@object, object2);
			Banner banner = new Banner(@object.Banner, multiplayerBattleColors.AttackerColors.BannerBackgroundColorUint, multiplayerBattleColors.AttackerColors.BannerForegroundColorUint);
			Banner banner2 = new Banner(object2.Banner, multiplayerBattleColors.DefenderColors.BannerBackgroundColorUint, multiplayerBattleColors.DefenderColors.BannerForegroundColorUint);
			base.Mission.Teams.Add(BattleSideEnum.Attacker, multiplayerBattleColors.AttackerColors.BannerBackgroundColorUint, multiplayerBattleColors.AttackerColors.BannerForegroundColorUint, banner, true, false, true);
			base.Mission.Teams.Add(BattleSideEnum.Defender, multiplayerBattleColors.DefenderColors.BannerBackgroundColorUint, multiplayerBattleColors.DefenderColors.BannerForegroundColorUint, banner2, true, false, true);
			foreach (FlagCapturePoint flagCapturePoint in this.AllCapturePoints)
			{
				this._capturePointOwners[flagCapturePoint.FlagIndex] = base.Mission.Teams.Defender;
				flagCapturePoint.SetTeamColors(base.Mission.Teams.Defender.Color, base.Mission.Teams.Defender.Color2);
				MissionMultiplayerSiegeClient gameModeSiegeClient = this._gameModeSiegeClient;
				if (gameModeSiegeClient != null)
				{
					gameModeSiegeClient.OnCapturePointOwnerChanged(flagCapturePoint, base.Mission.Teams.Defender);
				}
			}
			if (this._warmupComponent != null)
			{
				this._warmupComponent.OnWarmupEnding += this.OnWarmupEnding;
			}
		}

		// Token: 0x060027BE RID: 10174 RVA: 0x00095628 File Offset: 0x00093828
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (!this._firstTickDone)
			{
				foreach (CastleGate castleGate in Mission.Current.MissionObjects.FindAllWithType<CastleGate>())
				{
					castleGate.OpenDoor();
					foreach (StandingPoint standingPoint in castleGate.StandingPoints)
					{
						standingPoint.SetIsDeactivatedSynched(true);
					}
				}
				this._firstTickDone = true;
			}
			if (this.MissionLobbyComponent.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.Playing && (this.WarmupComponent == null || !this.WarmupComponent.IsInWarmup))
			{
				this.CheckMorales(dt);
				if (this.CheckObjectives(dt))
				{
					this.TickFlags(dt);
					this.TickObjectives(dt);
				}
			}
		}

		// Token: 0x060027BF RID: 10175 RVA: 0x00095714 File Offset: 0x00093914
		private void CheckMorales(float dt)
		{
			this._dtSumCheckMorales += dt;
			if (this._dtSumCheckMorales >= 1f)
			{
				this._dtSumCheckMorales -= 1f;
				int num = MathF.Max(this._morales[1] + this.GetMoraleGain(BattleSideEnum.Attacker), 0);
				int num2 = MBMath.ClampInt(this._morales[0] + this.GetMoraleGain(BattleSideEnum.Defender), 0, 360);
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SiegeMoraleChangeMessage(num, num2, this._capturePointRemainingMoraleGains));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				MissionMultiplayerSiegeClient gameModeSiegeClient = this._gameModeSiegeClient;
				if (gameModeSiegeClient != null)
				{
					gameModeSiegeClient.OnMoraleChanged(num, num2, this._capturePointRemainingMoraleGains);
				}
				this._morales[1] = num;
				this._morales[0] = num2;
			}
		}

		// Token: 0x060027C0 RID: 10176 RVA: 0x000957CD File Offset: 0x000939CD
		public override bool CheckForMatchEnd()
		{
			return this._morales.Any<int>((int morale) => morale == 0);
		}

		// Token: 0x060027C1 RID: 10177 RVA: 0x000957FC File Offset: 0x000939FC
		public override Team GetWinnerTeam()
		{
			Team team = null;
			if (this._morales[1] <= 0 && this._morales[0] > 0)
			{
				team = base.Mission.Teams.Defender;
			}
			if (this._morales[0] <= 0 && this._morales[1] > 0)
			{
				team = base.Mission.Teams.Attacker;
			}
			team = team ?? base.Mission.Teams.Defender;
			base.Mission.GetMissionBehavior<MissionScoreboardComponent>().ChangeTeamScore(team, 1);
			return team;
		}

		// Token: 0x060027C2 RID: 10178 RVA: 0x00095884 File Offset: 0x00093A84
		private int GetMoraleGain(BattleSideEnum side)
		{
			int num = 0;
			bool flag2 = this._masterFlagBestAgent != null && this._masterFlagBestAgent.Team.Side == side;
			if (side == BattleSideEnum.Attacker)
			{
				if (!flag2)
				{
					num += -1;
				}
				using (IEnumerator<FlagCapturePoint> enumerator = this.AllCapturePoints.Where<FlagCapturePoint>((FlagCapturePoint flag) => flag != this._masterFlag && !flag.IsDeactivated && flag.IsFullyRaised && this.GetFlagOwnerTeam(flag).Side == BattleSideEnum.Attacker).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FlagCapturePoint flagCapturePoint = enumerator.Current;
						this._capturePointRemainingMoraleGains[flagCapturePoint.FlagIndex]--;
						num++;
						if (this._capturePointRemainingMoraleGains[flagCapturePoint.FlagIndex] == 0)
						{
							num += 90;
							foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
							{
								MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
								if (component != null)
								{
									Team team = component.Team;
									BattleSideEnum? battleSideEnum = ((team != null) ? new BattleSideEnum?(team.Side) : null);
									if ((battleSideEnum.GetValueOrDefault() == side) & (battleSideEnum != null))
									{
										base.ChangeCurrentGoldForPeer(component, base.GetCurrentGoldForPeer(component) + 35);
									}
								}
							}
							flagCapturePoint.RemovePointAsServer();
							(base.SpawnComponent.SpawnFrameBehavior as SiegeSpawnFrameBehavior).OnFlagDeactivated(flagCapturePoint);
							this._gameModeSiegeClient.OnNumberOfFlagsChanged();
							GameNetwork.BeginBroadcastModuleEvent();
							GameNetwork.WriteMessage(new FlagDominationFlagsRemovedMessage());
							GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
							this.NotificationsComponent.FlagsXRemoved(flagCapturePoint);
						}
					}
					return num;
				}
			}
			if (this._masterFlag.IsFullyRaised)
			{
				if (this.GetFlagOwnerTeam(this._masterFlag).Side == BattleSideEnum.Attacker)
				{
					if (!flag2)
					{
						int num2 = 0;
						for (int i = 0; i < this.AllCapturePoints.Count; i++)
						{
							if (this.AllCapturePoints[i] != this._masterFlag && !this.AllCapturePoints[i].IsDeactivated)
							{
								num2++;
							}
						}
						num += -6 + num2;
					}
				}
				else
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x00095AB8 File Offset: 0x00093CB8
		public Team GetFlagOwnerTeam(FlagCapturePoint flag)
		{
			return this._capturePointOwners[flag.FlagIndex];
		}

		// Token: 0x060027C4 RID: 10180 RVA: 0x00095AC7 File Offset: 0x00093CC7
		private bool CheckObjectives(float dt)
		{
			this._dtSumObjectiveCheck += dt;
			if (this._dtSumObjectiveCheck >= 0.25f)
			{
				this._dtSumObjectiveCheck -= 0.25f;
				return true;
			}
			return false;
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x00095AFC File Offset: 0x00093CFC
		private void TickFlags(float dt)
		{
			foreach (FlagCapturePoint flagCapturePoint in this.AllCapturePoints)
			{
				if (!flagCapturePoint.IsDeactivated)
				{
					Team flagOwnerTeam = this.GetFlagOwnerTeam(flagCapturePoint);
					Agent agent = null;
					float num = float.MaxValue;
					AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(Mission.Current, flagCapturePoint.Position.AsVec2, 4f, false);
					while (proximityMapSearchStruct.LastFoundAgent != null)
					{
						Agent lastFoundAgent = proximityMapSearchStruct.LastFoundAgent;
						if (!lastFoundAgent.IsMount && lastFoundAgent.IsActive())
						{
							float num2 = lastFoundAgent.Position.DistanceSquared(flagCapturePoint.Position);
							if (num2 <= 16f && num2 < num)
							{
								agent = lastFoundAgent;
								num = num2;
							}
						}
						AgentProximityMap.FindNext(Mission.Current, ref proximityMapSearchStruct);
					}
					if (flagCapturePoint == this._masterFlag)
					{
						this._masterFlagBestAgent = agent;
					}
					CaptureTheFlagFlagDirection captureTheFlagFlagDirection = CaptureTheFlagFlagDirection.None;
					bool isContested = flagCapturePoint.IsContested;
					if (flagOwnerTeam == null)
					{
						if (!isContested && agent != null)
						{
							captureTheFlagFlagDirection = CaptureTheFlagFlagDirection.Down;
						}
						else if (agent == null && isContested)
						{
							captureTheFlagFlagDirection = CaptureTheFlagFlagDirection.Up;
						}
					}
					else if (agent != null)
					{
						if (agent.Team != flagOwnerTeam && !isContested)
						{
							captureTheFlagFlagDirection = CaptureTheFlagFlagDirection.Down;
						}
						else if (agent.Team == flagOwnerTeam && isContested)
						{
							captureTheFlagFlagDirection = CaptureTheFlagFlagDirection.Up;
						}
					}
					else if (isContested)
					{
						captureTheFlagFlagDirection = CaptureTheFlagFlagDirection.Up;
					}
					if (captureTheFlagFlagDirection != CaptureTheFlagFlagDirection.None)
					{
						flagCapturePoint.SetMoveFlag(captureTheFlagFlagDirection, 1f);
					}
					bool flag;
					flagCapturePoint.OnAfterTick(agent != null, out flag);
					if (flag)
					{
						Team team = agent.Team;
						uint num3 = ((team != null) ? team.Color : 4284111450U);
						uint num4 = ((team != null) ? team.Color2 : uint.MaxValue);
						flagCapturePoint.SetTeamColorsSynched(num3, num4);
						this._capturePointOwners[flagCapturePoint.FlagIndex] = team;
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new FlagDominationCapturePointMessage(flagCapturePoint.FlagIndex, (team != null) ? team.TeamIndex : (-1)));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
						MissionMultiplayerSiegeClient gameModeSiegeClient = this._gameModeSiegeClient;
						if (gameModeSiegeClient != null)
						{
							gameModeSiegeClient.OnCapturePointOwnerChanged(flagCapturePoint, team);
						}
						this.NotificationsComponent.FlagXCapturedByTeamX(flagCapturePoint, agent.Team);
					}
				}
			}
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x00095D18 File Offset: 0x00093F18
		private void TickObjectives(float dt)
		{
			for (int i = this._movingObjectives.Length - 1; i >= 0; i--)
			{
				IMoveableSiegeWeapon item = this._movingObjectives[i].Item1;
				if (item != null)
				{
					SiegeWeapon siegeWeapon = item as SiegeWeapon;
					if (siegeWeapon.IsDeactivated || siegeWeapon.IsDestroyed || siegeWeapon.IsDisabled)
					{
						this._movingObjectives[i].Item1 = null;
					}
					else
					{
						if (item.MovementComponent.HasArrivedAtTarget)
						{
							this._movingObjectives[i].Item1 = null;
							GameEntity gameEntity = GameEntity.CreateFromWeakEntity(siegeWeapon.GameEntity.Root);
							List<KeyValuePair<MissionPeer, float>> allContributorsForSideAndClear = this._objectiveSystem.GetAllContributorsForSideAndClear(gameEntity, BattleSideEnum.Attacker);
							float num = allContributorsForSideAndClear.Sum<KeyValuePair<MissionPeer, float>>((KeyValuePair<MissionPeer, float> ac) => ac.Value);
							using (List<KeyValuePair<MissionPeer, float>>.Enumerator enumerator = allContributorsForSideAndClear.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									KeyValuePair<MissionPeer, float> keyValuePair = enumerator.Current;
									int goldGainsFromObjectiveAssist = (keyValuePair.Key.Representative as SiegeMissionRepresentative).GetGoldGainsFromObjectiveAssist(gameEntity, keyValuePair.Value / num, true);
									if (goldGainsFromObjectiveAssist > 0)
									{
										base.ChangeCurrentGoldForPeer(keyValuePair.Key, keyValuePair.Key.Representative.Gold + goldGainsFromObjectiveAssist);
										MissionMultiplayerSiege.OnObjectiveGoldGainedDelegate onObjectiveGoldGained = this.OnObjectiveGoldGained;
										if (onObjectiveGoldGained != null)
										{
											onObjectiveGoldGained(keyValuePair.Key, goldGainsFromObjectiveAssist);
										}
									}
								}
								goto IL_0231;
							}
						}
						WeakGameEntity gameEntity2 = siegeWeapon.GameEntity;
						Vec3 item2 = this._movingObjectives[i].Item2;
						Vec3 globalPosition = gameEntity2.GlobalPosition;
						float lengthSquared = (globalPosition - item2).LengthSquared;
						if (lengthSquared > 1f)
						{
							this._movingObjectives[i].Item2 = globalPosition;
							foreach (StandingPoint standingPoint in siegeWeapon.StandingPoints)
							{
								Agent userAgent = standingPoint.UserAgent;
								if (((userAgent != null) ? userAgent.MissionPeer : null) != null && userAgent.MissionPeer.Team.Side == siegeWeapon.Side)
								{
									this._objectiveSystem.AddContributionForObjective(GameEntity.CreateFromWeakEntity(gameEntity2.Root), userAgent.MissionPeer, lengthSquared);
								}
							}
						}
					}
				}
				IL_0231:;
			}
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x00095F80 File Offset: 0x00094180
		private void OnWarmupEnding()
		{
			this.NotificationsComponent.WarmupEnding();
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x00095F90 File Offset: 0x00094190
		public override bool CheckForWarmupEnd()
		{
			int[] array = new int[2];
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (networkCommunicator.IsSynchronized && ((component != null) ? component.Team : null) != null && component.Team.Side != BattleSideEnum.None)
				{
					array[(int)component.Team.Side]++;
				}
			}
			return array.Sum() >= MultiplayerOptions.OptionType.MaxNumberOfPlayers.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x00096034 File Offset: 0x00094234
		protected override void HandleEarlyNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			networkPeer.AddComponent<SiegeMissionRepresentative>();
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x00096040 File Offset: 0x00094240
		protected override void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			int num = 120;
			if (this._warmupComponent != null && this._warmupComponent.IsInWarmup)
			{
				num = 160;
			}
			base.ChangeCurrentGoldForPeer(networkPeer.GetComponent<MissionPeer>(), num);
			MissionMultiplayerSiegeClient gameModeSiegeClient = this._gameModeSiegeClient;
			if (gameModeSiegeClient != null)
			{
				gameModeSiegeClient.OnGoldAmountChangedForRepresentative(networkPeer.GetComponent<SiegeMissionRepresentative>(), num);
			}
			if (this.AllCapturePoints != null && !networkPeer.IsServerPeer)
			{
				foreach (FlagCapturePoint flagCapturePoint in this.AllCapturePoints.Where<FlagCapturePoint>((FlagCapturePoint cp) => !cp.IsDeactivated))
				{
					GameNetwork.BeginModuleEventAsServer(networkPeer);
					int flagIndex = flagCapturePoint.FlagIndex;
					Team team = this._capturePointOwners[flagCapturePoint.FlagIndex];
					GameNetwork.WriteMessage(new FlagDominationCapturePointMessage(flagIndex, (team != null) ? team.TeamIndex : (-1)));
					GameNetwork.EndModuleEventAsServer();
				}
			}
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x00096138 File Offset: 0x00094338
		public override void OnPeerChangedTeam(NetworkCommunicator peer, Team oldTeam, Team newTeam)
		{
			if (this.MissionLobbyComponent.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.Playing && oldTeam != null && oldTeam != newTeam)
			{
				base.ChangeCurrentGoldForPeer(peer.GetComponent<MissionPeer>(), 100);
			}
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x00096160 File Offset: 0x00094360
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (this.MissionLobbyComponent.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.Playing && blow.DamageType != DamageTypes.Invalid && (agentState == AgentState.Unconscious || agentState == AgentState.Killed) && affectedAgent.IsHuman)
			{
				MissionPeer missionPeer = affectedAgent.MissionPeer;
				if (missionPeer != null)
				{
					int num = 100;
					if (affectorAgent != affectedAgent)
					{
						List<MissionPeer>[] array = new List<MissionPeer>[2];
						for (int i = 0; i < array.Length; i++)
						{
							array[i] = new List<MissionPeer>();
						}
						foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
						{
							MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
							if (component != null && component.Team != null && component.Team.Side != BattleSideEnum.None)
							{
								array[(int)component.Team.Side].Add(component);
							}
						}
						int num2 = array[1].Count - array[0].Count;
						BattleSideEnum battleSideEnum = ((num2 == 0) ? BattleSideEnum.None : ((num2 < 0) ? BattleSideEnum.Attacker : BattleSideEnum.Defender));
						if (battleSideEnum != BattleSideEnum.None && battleSideEnum == missionPeer.Team.Side)
						{
							num2 = MathF.Abs(num2);
							int count = array[(int)battleSideEnum].Count;
							if (count > 0)
							{
								int num3 = num * num2 / 10 / count * 10;
								num += num3;
							}
						}
					}
					base.ChangeCurrentGoldForPeer(missionPeer, missionPeer.Representative.Gold + num);
				}
				bool flag = ((affectorAgent != null) ? affectorAgent.Team : null) != null && affectedAgent.Team != null && affectorAgent.Team.Side == affectedAgent.Team.Side;
				MultiplayerClassDivisions.MPHeroClass mpheroClassForCharacter = MultiplayerClassDivisions.GetMPHeroClassForCharacter(affectedAgent.Character);
				Agent.Hitter assistingHitter = affectedAgent.GetAssistingHitter((affectorAgent != null) ? affectorAgent.MissionPeer : null);
				if (((affectorAgent != null) ? affectorAgent.MissionPeer : null) != null && affectorAgent != affectedAgent && affectedAgent.Team != affectorAgent.Team)
				{
					SiegeMissionRepresentative siegeMissionRepresentative = affectorAgent.MissionPeer.Representative as SiegeMissionRepresentative;
					int goldGainsFromKillDataAndUpdateFlags = siegeMissionRepresentative.GetGoldGainsFromKillDataAndUpdateFlags(MPPerkObject.GetPerkHandler(affectorAgent.MissionPeer), MPPerkObject.GetPerkHandler((assistingHitter != null) ? assistingHitter.HitterPeer : null), mpheroClassForCharacter, false, blow.IsMissile, flag);
					base.ChangeCurrentGoldForPeer(affectorAgent.MissionPeer, siegeMissionRepresentative.Gold + goldGainsFromKillDataAndUpdateFlags);
				}
				if (((assistingHitter != null) ? assistingHitter.HitterPeer : null) != null && !assistingHitter.IsFriendlyHit)
				{
					SiegeMissionRepresentative siegeMissionRepresentative2 = assistingHitter.HitterPeer.Representative as SiegeMissionRepresentative;
					int goldGainsFromKillDataAndUpdateFlags2 = siegeMissionRepresentative2.GetGoldGainsFromKillDataAndUpdateFlags(MPPerkObject.GetPerkHandler((affectorAgent != null) ? affectorAgent.MissionPeer : null), MPPerkObject.GetPerkHandler(assistingHitter.HitterPeer), mpheroClassForCharacter, true, blow.IsMissile, flag);
					base.ChangeCurrentGoldForPeer(assistingHitter.HitterPeer, siegeMissionRepresentative2.Gold + goldGainsFromKillDataAndUpdateFlags2);
				}
				if (((missionPeer != null) ? missionPeer.Team : null) != null)
				{
					MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(missionPeer);
					IEnumerable<ValueTuple<MissionPeer, int>> enumerable = ((perkHandler != null) ? perkHandler.GetTeamGoldRewardsOnDeath() : null);
					if (enumerable != null)
					{
						foreach (ValueTuple<MissionPeer, int> valueTuple in enumerable)
						{
							MissionPeer item = valueTuple.Item1;
							int item2 = valueTuple.Item2;
							SiegeMissionRepresentative siegeMissionRepresentative3;
							if (item2 > 0 && (siegeMissionRepresentative3 = ((item != null) ? item.Representative : null) as SiegeMissionRepresentative) != null)
							{
								int goldGainsFromAllyDeathReward = siegeMissionRepresentative3.GetGoldGainsFromAllyDeathReward(item2);
								if (goldGainsFromAllyDeathReward > 0)
								{
									base.ChangeCurrentGoldForPeer(item, siegeMissionRepresentative3.Gold + goldGainsFromAllyDeathReward);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x000964B8 File Offset: 0x000946B8
		protected override void HandleNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new SiegeMoraleChangeMessage(this._morales[1], this._morales[0], this._capturePointRemainingMoraleGains));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
		}

		// Token: 0x060027CE RID: 10190 RVA: 0x000964E6 File Offset: 0x000946E6
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
			if (this._warmupComponent != null)
			{
				this._warmupComponent.OnWarmupEnding -= this.OnWarmupEnding;
			}
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x00096510 File Offset: 0x00094710
		public override void OnClearScene()
		{
			base.OnClearScene();
			foreach (CastleGate castleGate in Mission.Current.MissionObjects.FindAllWithType<CastleGate>())
			{
				foreach (StandingPoint standingPoint in castleGate.StandingPoints)
				{
					standingPoint.SetIsDeactivatedSynched(false);
				}
			}
		}

		// Token: 0x04000F21 RID: 3873
		public const int NumberOfFlagsInGame = 7;

		// Token: 0x04000F22 RID: 3874
		public const int NumberOfFlagsAffectingMoraleInGame = 6;

		// Token: 0x04000F23 RID: 3875
		public const int MaxMorale = 1440;

		// Token: 0x04000F24 RID: 3876
		public const int StartingMorale = 360;

		// Token: 0x04000F25 RID: 3877
		private const int FirstSpawnGold = 120;

		// Token: 0x04000F26 RID: 3878
		private const int FirstSpawnGoldForEarlyJoin = 160;

		// Token: 0x04000F27 RID: 3879
		private const int RespawnGold = 100;

		// Token: 0x04000F28 RID: 3880
		private const float ObjectiveCheckPeriod = 0.25f;

		// Token: 0x04000F29 RID: 3881
		private const float MoraleTickTimeInSeconds = 1f;

		// Token: 0x04000F2A RID: 3882
		public const int MaxMoraleGainPerFlag = 90;

		// Token: 0x04000F2B RID: 3883
		private const int MoraleBoostOnFlagRemoval = 90;

		// Token: 0x04000F2C RID: 3884
		private const int MoraleDecayInTick = -1;

		// Token: 0x04000F2D RID: 3885
		private const int MoraleDecayOnDefenderInTick = -6;

		// Token: 0x04000F2E RID: 3886
		public const int MoraleGainPerFlag = 1;

		// Token: 0x04000F2F RID: 3887
		public const int GoldBonusOnFlagRemoval = 35;

		// Token: 0x04000F30 RID: 3888
		public const string MasterFlagTag = "keep_capture_point";

		// Token: 0x04000F33 RID: 3891
		private int[] _morales;

		// Token: 0x04000F34 RID: 3892
		private Agent _masterFlagBestAgent;

		// Token: 0x04000F35 RID: 3893
		private FlagCapturePoint _masterFlag;

		// Token: 0x04000F36 RID: 3894
		private Team[] _capturePointOwners;

		// Token: 0x04000F37 RID: 3895
		private int[] _capturePointRemainingMoraleGains;

		// Token: 0x04000F39 RID: 3897
		private float _dtSumCheckMorales;

		// Token: 0x04000F3A RID: 3898
		private float _dtSumObjectiveCheck;

		// Token: 0x04000F3B RID: 3899
		private MissionMultiplayerSiege.ObjectiveSystem _objectiveSystem;

		// Token: 0x04000F3C RID: 3900
		private ValueTuple<IMoveableSiegeWeapon, Vec3>[] _movingObjectives;

		// Token: 0x04000F3D RID: 3901
		private ValueTuple<RangedSiegeWeapon, Agent>[] _lastReloadingAgentPerRangedSiegeMachine;

		// Token: 0x04000F3E RID: 3902
		private MissionMultiplayerSiegeClient _gameModeSiegeClient;

		// Token: 0x04000F3F RID: 3903
		private MultiplayerWarmupComponent _warmupComponent;

		// Token: 0x04000F40 RID: 3904
		private Dictionary<GameEntity, List<DestructableComponent>> _childDestructableComponents;

		// Token: 0x04000F41 RID: 3905
		private bool _firstTickDone;

		// Token: 0x0200059A RID: 1434
		private class ObjectiveSystem
		{
			// Token: 0x06003DB5 RID: 15797 RVA: 0x000F3D1C File Offset: 0x000F1F1C
			public ObjectiveSystem()
			{
				this._objectiveContributorMap = new Dictionary<GameEntity, List<MissionMultiplayerSiege.ObjectiveSystem.ObjectiveContributor>[]>();
			}

			// Token: 0x06003DB6 RID: 15798 RVA: 0x000F3D30 File Offset: 0x000F1F30
			public bool RegisterObjective(GameEntity entity)
			{
				if (!this._objectiveContributorMap.ContainsKey(entity))
				{
					this._objectiveContributorMap.Add(entity, new List<MissionMultiplayerSiege.ObjectiveSystem.ObjectiveContributor>[2]);
					for (int i = 0; i < 2; i++)
					{
						this._objectiveContributorMap[entity][i] = new List<MissionMultiplayerSiege.ObjectiveSystem.ObjectiveContributor>();
					}
					return true;
				}
				return false;
			}

			// Token: 0x06003DB7 RID: 15799 RVA: 0x000F3D80 File Offset: 0x000F1F80
			public void AddContributionForObjective(GameEntity objectiveEntity, MissionPeer contributorPeer, float contribution)
			{
				string text = objectiveEntity.Tags.FirstOrDefault<string>((string x) => x.StartsWith("mp_siege_objective_")) ?? "";
				bool flag = false;
				for (int i = 0; i < 2; i++)
				{
					foreach (MissionMultiplayerSiege.ObjectiveSystem.ObjectiveContributor objectiveContributor in this._objectiveContributorMap[objectiveEntity][i])
					{
						if (objectiveContributor.Peer == contributorPeer)
						{
							Debug.Print(string.Format("[CONT > {0}] Increased contribution for {1}({2}) by {3}.", new object[]
							{
								text,
								contributorPeer.Name,
								contributorPeer.Team.Side.ToString(),
								contribution
							}), 0, Debug.DebugColor.White, 17179869184UL);
							objectiveContributor.IncreaseAmount(contribution);
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (!flag)
				{
					Debug.Print(string.Format("[CONT > {0}] Adding {1} contribution for {2}({3}).", new object[]
					{
						text,
						contribution,
						contributorPeer.Name,
						contributorPeer.Team.Side.ToString()
					}), 0, Debug.DebugColor.White, 17179869184UL);
					this._objectiveContributorMap[objectiveEntity][(int)contributorPeer.Team.Side].Add(new MissionMultiplayerSiege.ObjectiveSystem.ObjectiveContributor(contributorPeer, contribution));
				}
			}

			// Token: 0x06003DB8 RID: 15800 RVA: 0x000F3F08 File Offset: 0x000F2108
			public List<KeyValuePair<MissionPeer, float>> GetAllContributorsForSideAndClear(GameEntity objectiveEntity, BattleSideEnum side)
			{
				List<KeyValuePair<MissionPeer, float>> list = new List<KeyValuePair<MissionPeer, float>>();
				string text = objectiveEntity.Tags.FirstOrDefault<string>((string x) => x.StartsWith("mp_siege_objective_")) ?? "";
				foreach (MissionMultiplayerSiege.ObjectiveSystem.ObjectiveContributor objectiveContributor in this._objectiveContributorMap[objectiveEntity][(int)side])
				{
					Debug.Print(string.Format("[CONT > {0}] Rewarding {1} contribution for {2}({3}).", new object[]
					{
						text,
						objectiveContributor.Contribution,
						objectiveContributor.Peer.Name,
						side.ToString()
					}), 0, Debug.DebugColor.White, 17179869184UL);
					list.Add(new KeyValuePair<MissionPeer, float>(objectiveContributor.Peer, objectiveContributor.Contribution));
				}
				this._objectiveContributorMap[objectiveEntity][(int)side].Clear();
				return list;
			}

			// Token: 0x04001E98 RID: 7832
			private readonly Dictionary<GameEntity, List<MissionMultiplayerSiege.ObjectiveSystem.ObjectiveContributor>[]> _objectiveContributorMap;

			// Token: 0x020006BF RID: 1727
			private class ObjectiveContributor
			{
				// Token: 0x17000B08 RID: 2824
				// (get) Token: 0x06004223 RID: 16931 RVA: 0x000FCD17 File Offset: 0x000FAF17
				// (set) Token: 0x06004224 RID: 16932 RVA: 0x000FCD1F File Offset: 0x000FAF1F
				public float Contribution { get; private set; }

				// Token: 0x06004225 RID: 16933 RVA: 0x000FCD28 File Offset: 0x000FAF28
				public ObjectiveContributor(MissionPeer peer, float initialContribution)
				{
					this.Peer = peer;
					this.Contribution = initialContribution;
				}

				// Token: 0x06004226 RID: 16934 RVA: 0x000FCD3E File Offset: 0x000FAF3E
				public void IncreaseAmount(float deltaContribution)
				{
					this.Contribution += deltaContribution;
				}

				// Token: 0x04002340 RID: 9024
				public readonly MissionPeer Peer;
			}
		}

		// Token: 0x0200059B RID: 1435
		// (Invoke) Token: 0x06003DBA RID: 15802
		public delegate void OnDestructableComponentDestroyedDelegate(DestructableComponent destructableComponent, ScriptComponentBehavior attackerScriptComponentBehaviour, MissionPeer[] contributors);

		// Token: 0x0200059C RID: 1436
		// (Invoke) Token: 0x06003DBE RID: 15806
		public delegate void OnObjectiveGoldGainedDelegate(MissionPeer peer, int goldGain);
	}
}
