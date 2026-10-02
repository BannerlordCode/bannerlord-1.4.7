using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.MountAndBlade.Objects.Siege;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000350 RID: 848
	public class SiegeTower : SiegeWeapon, IPathHolder, IPrimarySiegeWeapon, IMoveableSiegeWeapon, ISpawnable
	{
		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06003057 RID: 12375 RVA: 0x000C20FD File Offset: 0x000C02FD
		public MissionObject TargetCastlePosition
		{
			get
			{
				return this._targetWallSegment;
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06003058 RID: 12376 RVA: 0x000C2105 File Offset: 0x000C0305
		private WeakGameEntity CleanState
		{
			get
			{
				if (!(this._cleanState == null))
				{
					return this._cleanState.WeakEntity;
				}
				return base.GameEntity;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06003059 RID: 12377 RVA: 0x000C2127 File Offset: 0x000C0327
		// (set) Token: 0x0600305A RID: 12378 RVA: 0x000C212F File Offset: 0x000C032F
		public FormationAI.BehaviorSide WeaponSide { get; private set; }

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x0600305B RID: 12379 RVA: 0x000C2138 File Offset: 0x000C0338
		// (set) Token: 0x0600305C RID: 12380 RVA: 0x000C2140 File Offset: 0x000C0340
		public string PathEntity { get; private set; }

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x0600305D RID: 12381 RVA: 0x000C2149 File Offset: 0x000C0349
		public bool EditorGhostEntityMove
		{
			get
			{
				return this.GhostEntityMove;
			}
		}

		// Token: 0x0600305E RID: 12382 RVA: 0x000C2151 File Offset: 0x000C0351
		public bool HasCompletedAction()
		{
			return !base.IsDisabled && this.IsDeactivated && this._hasArrivedAtTarget && !base.IsDestroyed;
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x0600305F RID: 12383 RVA: 0x000C2176 File Offset: 0x000C0376
		public float SiegeWeaponPriority
		{
			get
			{
				return 20f;
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06003060 RID: 12384 RVA: 0x000C217D File Offset: 0x000C037D
		public int OverTheWallNavMeshID
		{
			get
			{
				return this.GetGateNavMeshId();
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06003061 RID: 12385 RVA: 0x000C2185 File Offset: 0x000C0385
		// (set) Token: 0x06003062 RID: 12386 RVA: 0x000C218D File Offset: 0x000C038D
		public SiegeWeaponMovementComponent MovementComponent { get; private set; }

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06003063 RID: 12387 RVA: 0x000C2196 File Offset: 0x000C0396
		public bool HoldLadders
		{
			get
			{
				return !this.MovementComponent.HasArrivedAtTarget;
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06003064 RID: 12388 RVA: 0x000C21A6 File Offset: 0x000C03A6
		public bool SendLadders
		{
			get
			{
				return this.MovementComponent.HasArrivedAtTarget;
			}
		}

		// Token: 0x06003065 RID: 12389 RVA: 0x000C21B3 File Offset: 0x000C03B3
		public int GetGateNavMeshId()
		{
			if (this.GateNavMeshId != 0)
			{
				return this.GateNavMeshId;
			}
			if (this.DynamicNavmeshIdStart == 0)
			{
				return 0;
			}
			return this.DynamicNavmeshIdStart + 3;
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x000C21D8 File Offset: 0x000C03D8
		public List<int> CollectGetDifficultNavmeshIDs()
		{
			List<int> list = new List<int>();
			if (!this._hasLadders)
			{
				return list;
			}
			list.Add(this.DynamicNavmeshIdStart + 1);
			list.Add(this.DynamicNavmeshIdStart + 5);
			list.Add(this.DynamicNavmeshIdStart + 6);
			list.Add(this.DynamicNavmeshIdStart + 7);
			return list;
		}

		// Token: 0x06003067 RID: 12391 RVA: 0x000C2230 File Offset: 0x000C0430
		public List<int> CollectGetDifficultNavmeshIDsForAttackers()
		{
			List<int> list = new List<int>();
			if (!this._hasLadders)
			{
				return list;
			}
			list = this.CollectGetDifficultNavmeshIDs();
			list.Add(this.DynamicNavmeshIdStart + 3);
			return list;
		}

		// Token: 0x06003068 RID: 12392 RVA: 0x000C2264 File Offset: 0x000C0464
		public List<int> CollectGetDifficultNavmeshIDsForDefenders()
		{
			List<int> list = new List<int>();
			if (!this._hasLadders)
			{
				return list;
			}
			list = this.CollectGetDifficultNavmeshIDs();
			list.Add(this.DynamicNavmeshIdStart + 2);
			return list;
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06003069 RID: 12393 RVA: 0x000C2297 File Offset: 0x000C0497
		// (set) Token: 0x0600306A RID: 12394 RVA: 0x000C22A0 File Offset: 0x000C04A0
		public bool HasArrivedAtTarget
		{
			get
			{
				return this._hasArrivedAtTarget;
			}
			set
			{
				if (!GameNetwork.IsClientOrReplay)
				{
					this.MovementComponent.SetDestinationNavMeshIdState(!this.HasArrivedAtTarget);
				}
				if (this._hasArrivedAtTarget != value)
				{
					this._hasArrivedAtTarget = value;
					if (this._hasArrivedAtTarget)
					{
						this.ActiveWaitStandingPoint = base.WaitStandingPoints[1];
						if (GameNetwork.IsClientOrReplay)
						{
							goto IL_00CA;
						}
						using (List<LadderQueueManager>.Enumerator enumerator = this._queueManagers.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								LadderQueueManager ladderQueueManager = enumerator.Current;
								this.CleanState.Scene.SetAbilityOfFacesWithId(ladderQueueManager.ManagedNavigationFaceId, true);
								ladderQueueManager.Activate();
							}
							goto IL_00CA;
						}
					}
					if (!GameNetwork.IsClientOrReplay && this.GetGateNavMeshId() > 0)
					{
						this.CleanState.Scene.SetAbilityOfFacesWithId(this.GetGateNavMeshId(), false);
					}
					IL_00CA:
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SetSiegeTowerHasArrivedAtTarget(base.Id));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
						return;
					}
					if (GameNetwork.IsClientOrReplay)
					{
						this.MovementComponent.MoveToTargetAsClient();
					}
				}
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x0600306B RID: 12395 RVA: 0x000C23C0 File Offset: 0x000C05C0
		// (set) Token: 0x0600306C RID: 12396 RVA: 0x000C23C8 File Offset: 0x000C05C8
		public SiegeTower.GateState State
		{
			get
			{
				return this._state;
			}
			set
			{
				if (this._state != value)
				{
					if (GameNetwork.IsServerOrRecorder)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SetSiegeTowerGateState(base.Id, value));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
					}
					this._state = value;
					this.OnSiegeTowerGateStateChange();
				}
			}
		}

		// Token: 0x0600306D RID: 12397 RVA: 0x000C2405 File Offset: 0x000C0605
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			if (!gameEntity.IsValid || !gameEntity.HasScriptOfType<UsableMissionObject>() || gameEntity.HasTag("move"))
			{
				return new TextObject("{=aXjlMBiE}Siege Tower", null);
			}
			return new TextObject("{=6wZUG0ev}Gate", null);
		}

		// Token: 0x0600306E RID: 12398 RVA: 0x000C2440 File Offset: 0x000C0640
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			TextObject textObject = (usableGameObject.GameEntity.HasTag("move") ? new TextObject("{=rwZAZSvX}{KEY} Move", null) : new TextObject("{=5oozsaIb}{KEY} Open", null));
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x000C249C File Offset: 0x000C069C
		public override void WriteToNetwork()
		{
			base.WriteToNetwork();
			GameNetworkMessage.WriteBoolToPacket(this.HasArrivedAtTarget);
			GameNetworkMessage.WriteIntToPacket((int)this.State, CompressionMission.SiegeTowerGateStateCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this._fallAngularSpeed, CompressionMission.SiegeMachineComponentAngularSpeedCompressionInfo);
			GameNetworkMessage.WriteFloatToPacket(this.MovementComponent.GetTotalDistanceTraveledForPathTracker(), CompressionBasic.PositionCompressionInfo);
		}

		// Token: 0x06003070 RID: 12400 RVA: 0x000C24EF File Offset: 0x000C06EF
		public override OrderType GetOrder(BattleSideEnum side)
		{
			if (base.IsDestroyed)
			{
				return OrderType.None;
			}
			if (side != BattleSideEnum.Attacker)
			{
				return OrderType.AttackEntity;
			}
			if (this.HasCompletedAction())
			{
				return OrderType.Use;
			}
			return OrderType.FollowEntity;
		}

		// Token: 0x06003071 RID: 12401 RVA: 0x000C2510 File Offset: 0x000C0710
		public override TargetFlags GetTargetFlags()
		{
			TargetFlags targetFlags = TargetFlags.None;
			if (base.UserCountNotInStruckAction > 0)
			{
				targetFlags |= TargetFlags.IsMoving;
			}
			targetFlags |= TargetFlags.IsSiegeEngine;
			targetFlags |= TargetFlags.IsAttacker;
			if (this.HasCompletedAction() || base.IsDestroyed || this.IsDeactivated)
			{
				targetFlags |= TargetFlags.NotAThreat;
			}
			if (this.Side == BattleSideEnum.Attacker && DebugSiegeBehavior.DebugDefendState == DebugSiegeBehavior.DebugStateDefender.DebugDefendersToTower)
			{
				targetFlags |= TargetFlags.DebugThreat;
			}
			return targetFlags | TargetFlags.IsSiegeTower;
		}

		// Token: 0x06003072 RID: 12402 RVA: 0x000C2574 File Offset: 0x000C0774
		public override float GetTargetValue(List<Vec3> weaponPos)
		{
			return 90f * base.GetUserMultiplierOfWeapon() * this.GetDistanceMultiplierOfWeapon(weaponPos[0]) * base.GetHitPointMultiplierOfWeapon();
		}

		// Token: 0x06003073 RID: 12403 RVA: 0x000C2598 File Offset: 0x000C0798
		public override void Disable()
		{
			base.Disable();
			this.SetAbilityOfFaces(false);
			if (this._queueManagers != null)
			{
				foreach (LadderQueueManager ladderQueueManager in this._queueManagers)
				{
					this.CleanState.Scene.SetAbilityOfFacesWithId(ladderQueueManager.ManagedNavigationFaceId, false);
					ladderQueueManager.DeactivateImmediate();
				}
			}
		}

		// Token: 0x06003074 RID: 12404 RVA: 0x000C261C File Offset: 0x000C081C
		public override SiegeEngineType GetSiegeEngineType()
		{
			return DefaultSiegeEngineTypes.SiegeTower;
		}

		// Token: 0x06003075 RID: 12405 RVA: 0x000C2623 File Offset: 0x000C0823
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new SiegeTowerAI(this);
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06003076 RID: 12406 RVA: 0x000C262B File Offset: 0x000C082B
		public override bool IsDeactivated
		{
			get
			{
				return (this.MovementComponent.HasArrivedAtTarget && this.State == SiegeTower.GateState.Open) || base.IsDeactivated;
			}
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x000C264C File Offset: 0x000C084C
		protected internal override void OnDeploymentStateChanged(bool isDeployed)
		{
			base.OnDeploymentStateChanged(isDeployed);
			if (this._ditchFillDebris != null)
			{
				if (!GameNetwork.IsClientOrReplay)
				{
					this._ditchFillDebris.SetVisibleSynched(isDeployed, false);
				}
				if (!GameNetwork.IsClientOrReplay)
				{
					if (isDeployed)
					{
						if (this._soilGenericNavMeshID > 0)
						{
							Mission.Current.Scene.SetAbilityOfFacesWithId(this._soilGenericNavMeshID, true);
						}
						if (this._soilNavMeshID1 > 0 && this._groundToSoilNavMeshID1 > 0 && this._ditchNavMeshID1 > 0)
						{
							Mission.Current.Scene.SetAbilityOfFacesWithId(this._soilNavMeshID1, true);
							Mission.Current.Scene.SwapFaceConnectionsWithID(this._groundToSoilNavMeshID1, this._ditchNavMeshID1, this._soilNavMeshID1, false);
						}
						if (this._soilNavMeshID2 > 0 && this._groundToSoilNavMeshID2 > 0 && this._ditchNavMeshID2 > 0)
						{
							Mission.Current.Scene.SetAbilityOfFacesWithId(this._soilNavMeshID2, true);
							Mission.Current.Scene.SwapFaceConnectionsWithID(this._groundToSoilNavMeshID2, this._ditchNavMeshID2, this._soilNavMeshID2, false);
						}
						if (this._groundGenericNavMeshID > 0)
						{
							Mission.Current.Scene.SetAbilityOfFacesWithId(this._groundGenericNavMeshID, false);
						}
					}
					else
					{
						if (this._groundGenericNavMeshID > 0)
						{
							Mission.Current.Scene.SetAbilityOfFacesWithId(this._groundGenericNavMeshID, true);
						}
						if (this._soilNavMeshID1 > 0 && this._groundToSoilNavMeshID1 > 0 && this._ditchNavMeshID1 > 0)
						{
							Mission.Current.Scene.SwapFaceConnectionsWithID(this._groundToSoilNavMeshID1, this._soilNavMeshID1, this._ditchNavMeshID1, false);
							Mission.Current.Scene.SetAbilityOfFacesWithId(this._soilNavMeshID1, false);
						}
						if (this._soilNavMeshID2 > 0 && this._groundToSoilNavMeshID2 > 0 && this._ditchNavMeshID2 > 0)
						{
							Mission.Current.Scene.SwapFaceConnectionsWithID(this._groundToSoilNavMeshID2, this._soilNavMeshID2, this._ditchNavMeshID2, false);
							Mission.Current.Scene.SetAbilityOfFacesWithId(this._soilNavMeshID2, false);
						}
						if (this._soilGenericNavMeshID > 0)
						{
							Mission.Current.Scene.SetAbilityOfFacesWithId(this._soilGenericNavMeshID, false);
						}
					}
				}
			}
			if (this._sameSideSiegeLadders == null)
			{
				this._sameSideSiegeLadders = (from sl in Mission.Current.ActiveMissionObjects.FindAllWithType<SiegeLadder>()
					where sl.WeaponSide == this.WeaponSide
					select sl).ToList<SiegeLadder>();
			}
			foreach (SiegeLadder siegeLadder in this._sameSideSiegeLadders)
			{
				siegeLadder.GameEntity.SetVisibilityExcludeParents(!isDeployed);
			}
		}

		// Token: 0x06003078 RID: 12408 RVA: 0x000C28F0 File Offset: 0x000C0AF0
		protected override void AttachDynamicNavmeshToEntity()
		{
			if (this.NavMeshPrefabName.Length > 0)
			{
				this.DynamicNavmeshIdStart = Mission.Current.GetNextDynamicNavMeshIdStart();
				this.CleanState.Scene.ImportNavigationMeshPrefab(this.NavMeshPrefabName, this.DynamicNavmeshIdStart);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 1, false, false, false, false, true);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 2, true, false, false, false, true);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 4, false, true, false, true, true);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 5, false, false, false, false, true);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 6, false, false, false, false, true);
				this.GetEntityToAttachNavMeshFaces().AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 7, false, false, false, false, true);
			}
		}

		// Token: 0x06003079 RID: 12409 RVA: 0x000C29DF File Offset: 0x000C0BDF
		protected override WeakGameEntity GetEntityToAttachNavMeshFaces()
		{
			return this.CleanState;
		}

		// Token: 0x0600307A RID: 12410 RVA: 0x000C29E7 File Offset: 0x000C0BE7
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			SiegeWeaponMovementComponent movementComponent = this.MovementComponent;
			if (movementComponent == null)
			{
				return;
			}
			movementComponent.OnRemoved();
		}

		// Token: 0x0600307B RID: 12411 RVA: 0x000C2A00 File Offset: 0x000C0C00
		public override void SetAbilityOfFaces(bool enabled)
		{
			base.SetAbilityOfFaces(enabled);
			if (this._queueManagers != null)
			{
				foreach (LadderQueueManager ladderQueueManager in this._queueManagers)
				{
					this.CleanState.Scene.SetAbilityOfFacesWithId(ladderQueueManager.ManagedNavigationFaceId, enabled);
					if (ladderQueueManager.IsDeactivated != !enabled)
					{
						if (enabled)
						{
							ladderQueueManager.Activate();
						}
						else
						{
							ladderQueueManager.DeactivateImmediate();
						}
					}
				}
			}
		}

		// Token: 0x0600307C RID: 12412 RVA: 0x000C2A94 File Offset: 0x000C0C94
		protected override float GetDistanceMultiplierOfWeapon(Vec3 weaponPos)
		{
			float minimumDistanceBetweenPositions = this.GetMinimumDistanceBetweenPositions(weaponPos);
			if (minimumDistanceBetweenPositions < 10f)
			{
				return 1f;
			}
			if (minimumDistanceBetweenPositions < 25f)
			{
				return 0.8f;
			}
			return 0.6f;
		}

		// Token: 0x0600307D RID: 12413 RVA: 0x000C2ACC File Offset: 0x000C0CCC
		private bool IsNavmeshOnThisTowerAttackerDifficultNavmeshIDs(int testedNavmeshID)
		{
			return this._hasLadders && (testedNavmeshID == this.DynamicNavmeshIdStart + 1 || testedNavmeshID == this.DynamicNavmeshIdStart + 5 || testedNavmeshID == this.DynamicNavmeshIdStart + 6 || testedNavmeshID == this.DynamicNavmeshIdStart + 7 || testedNavmeshID == this.DynamicNavmeshIdStart + 3);
		}

		// Token: 0x0600307E RID: 12414 RVA: 0x000C2B1C File Offset: 0x000C0D1C
		protected override bool IsAgentOnInconvenientNavmesh(Agent agent, StandingPoint standingPoint)
		{
			if (Mission.Current.MissionTeamAIType != Mission.MissionTeamAITypeEnum.Siege)
			{
				return false;
			}
			int currentNavigationFaceId = agent.GetCurrentNavigationFaceId();
			TeamAISiegeComponent teamAISiegeComponent;
			if ((teamAISiegeComponent = agent.Team.TeamAI as TeamAISiegeComponent) != null)
			{
				if (teamAISiegeComponent is TeamAISiegeDefender && currentNavigationFaceId % 10 != 1)
				{
					return true;
				}
				foreach (int num in teamAISiegeComponent.DifficultNavmeshIDs)
				{
					if (currentNavigationFaceId == num)
					{
						return standingPoint != this._gateStandingPoint || !this.IsNavmeshOnThisTowerAttackerDifficultNavmeshIDs(currentNavigationFaceId);
					}
				}
				if (teamAISiegeComponent is TeamAISiegeAttacker && currentNavigationFaceId % 10 == 1)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600307F RID: 12415 RVA: 0x000C2BD8 File Offset: 0x000C0DD8
		protected internal override void OnInit()
		{
			this._cleanState = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity.GetFirstChildEntityWithTag("body"));
			base.OnInit();
			base.DestructionComponent.OnDestroyed += new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.OnDestroyed);
			base.DestructionComponent.BattleSide = BattleSideEnum.Attacker;
			this._aiBarriers = base.Scene.FindEntitiesWithTag(this.BarrierTagToRemove).ToList<GameEntity>();
			if (!GameNetwork.IsClientOrReplay && this._soilGenericNavMeshID > 0)
			{
				this.CleanState.Scene.SetAbilityOfFacesWithId(this._soilGenericNavMeshID, false);
			}
			List<SynchedMissionObject> list = this.CleanState.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>(this.GateTag);
			if (list.Count > 0)
			{
				this._gateObject = list[0];
			}
			this.AddRegularMovementComponent();
			List<GameEntity> list2 = base.Scene.FindEntitiesWithTag("breakable_wall").ToList<GameEntity>();
			if (!list2.IsEmpty<GameEntity>())
			{
				float num = 10000000f;
				GameEntity gameEntity = null;
				MatrixFrame targetFrame = this.MovementComponent.GetTargetFrame();
				foreach (GameEntity gameEntity2 in list2)
				{
					float lengthSquared = (gameEntity2.GlobalPosition - targetFrame.origin).LengthSquared;
					if (lengthSquared < num)
					{
						num = lengthSquared;
						gameEntity = gameEntity2;
					}
				}
				list2 = gameEntity.CollectChildrenEntitiesWithTag("destroyed");
				if (list2.Count > 0)
				{
					this._destroyedWallEntity = list2[0];
				}
				list2 = gameEntity.CollectChildrenEntitiesWithTag("non_destroyed");
				if (list2.Count > 0)
				{
					this._nonDestroyedWallEntity = list2[0];
				}
				list2 = gameEntity.CollectChildrenEntitiesWithTag("particle_spawnpoint");
				if (list2.Count > 0)
				{
					this._battlementDestroyedParticle = list2[0];
				}
			}
			list = this.CleanState.CollectScriptComponentsWithTagIncludingChildrenRecursive<SynchedMissionObject>(this.HandleTag);
			this._handleObject = ((list.Count < 1) ? null : list[0]);
			this._gateHandleIdleAnimationIndex = MBAnimation.GetAnimationIndexWithName(this.GateHandleIdleAnimation);
			this._gateTrembleAnimationIndex = MBAnimation.GetAnimationIndexWithName(this.GateTrembleAnimation);
			this._queueManagers = new List<LadderQueueManager>();
			if (!GameNetwork.IsClientOrReplay)
			{
				List<WeakGameEntity> list3 = this.CleanState.CollectChildrenEntitiesWithTag("ladder");
				if (list3.Count > 0)
				{
					this._hasLadders = true;
					WeakGameEntity weakGameEntity = list3.ElementAt<WeakGameEntity>(list3.Count / 2);
					foreach (WeakGameEntity weakGameEntity2 in list3)
					{
						if (weakGameEntity2.Name.Contains("middle"))
						{
							weakGameEntity = weakGameEntity2;
						}
						else
						{
							LadderQueueManager firstScriptOfType = weakGameEntity2.GetFirstScriptOfType<LadderQueueManager>();
							firstScriptOfType.Initialize(-1, MatrixFrame.Identity, Vec3.Zero, BattleSideEnum.None, int.MaxValue, 1f, 5f, 5f, 5f, 0f, false, 1f, 0f, 0f, false, -1, -1, int.MaxValue, int.MaxValue);
							firstScriptOfType.DeactivateImmediate();
						}
					}
					int num2 = 0;
					int num3 = 1;
					for (int i = base.GameEntity.Name.Length - 1; i >= 0; i--)
					{
						if (char.IsDigit(base.GameEntity.Name[i]))
						{
							num2 += (int)(base.GameEntity.Name[i] - '0') * num3;
							num3 *= 10;
						}
						else if (num2 > 0)
						{
							break;
						}
					}
					LadderQueueManager firstScriptOfType2 = weakGameEntity.GetFirstScriptOfType<LadderQueueManager>();
					if (firstScriptOfType2 != null)
					{
						MatrixFrame identity = MatrixFrame.Identity;
						identity.rotation.RotateAboutSide(1.5707964f);
						identity.rotation.RotateAboutForward(0.3926991f);
						firstScriptOfType2.Initialize(this.DynamicNavmeshIdStart + 5, identity, new Vec3(0f, 0f, 1f, -1f), BattleSideEnum.Attacker, list3.Count * 2, 0.7853982f, 2f, 1f, 4f, 3f, false, 0.8f, (float)num2 * 2f / 5f, 5f, list3.Count > 1, this.DynamicNavmeshIdStart + 6, this.DynamicNavmeshIdStart + 7, num2 * MathF.Round((float)list3.Count * 0.666f), list3.Count + 1);
						this._queueManagers.Add(firstScriptOfType2);
					}
					base.GameEntity.Scene.MarkFacesWithIdAsLadder(5, true);
					base.GameEntity.Scene.MarkFacesWithIdAsLadder(6, true);
					base.GameEntity.Scene.MarkFacesWithIdAsLadder(7, true);
				}
				else
				{
					this._hasLadders = false;
					LadderQueueManager firstScriptOfType3 = this.CleanState.GetFirstScriptOfType<LadderQueueManager>();
					if (firstScriptOfType3 != null)
					{
						MatrixFrame identity2 = MatrixFrame.Identity;
						identity2.origin.y = identity2.origin.y + 4f;
						identity2.rotation.RotateAboutSide(-1.5707964f);
						identity2.rotation.RotateAboutUp(3.1415927f);
						firstScriptOfType3.Initialize(this.DynamicNavmeshIdStart + 2, identity2, new Vec3(0f, -1f, 0f, -1f), BattleSideEnum.Attacker, 15, 0.7853982f, 2f, 1f, 3f, 1f, false, 0.8f, 4f, 5f, false, -2, -2, int.MaxValue, 15);
						this._queueManagers.Add(firstScriptOfType3);
					}
				}
			}
			this._state = SiegeTower.GateState.Closed;
			this._gateOpenSoundIndex = SoundEvent.GetEventIdFromString("event:/mission/siege/siegetower/dooropen");
			this._closedStateRotation = this._gateObject.GameEntity.GetFrame().rotation;
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				standingPoint.AddComponent(new ResetAnimationOnStopUsageComponent(ActionIndexCache.act_none, false));
				if (!standingPoint.GameEntity.HasTag("move"))
				{
					this._gateStandingPoint = standingPoint;
					standingPoint.IsDeactivated = true;
					MatrixFrame globalFrame = standingPoint.GameEntity.GetGlobalFrame();
					MatrixFrame globalFrame2 = this.CleanState.GetGlobalFrame();
					this._gateStandingPointLocalIKFrame = globalFrame.TransformToLocal(in globalFrame2);
					standingPoint.AddComponent(new ClearHandInverseKinematicsOnStopUsageComponent());
				}
			}
			if (base.WaitStandingPoints[0].GlobalPosition.z > base.WaitStandingPoints[1].GlobalPosition.z)
			{
				List<GameEntity> waitStandingPoints = base.WaitStandingPoints;
				List<GameEntity> waitStandingPoints2 = base.WaitStandingPoints;
				GameEntity gameEntity3 = base.WaitStandingPoints[1];
				GameEntity gameEntity4 = base.WaitStandingPoints[0];
				waitStandingPoints[0] = gameEntity3;
				waitStandingPoints2[1] = gameEntity4;
				this.ActiveWaitStandingPoint = base.WaitStandingPoints[0];
			}
			IEnumerable<WeakGameEntity> enumerable = from entity in base.Scene.FindWeakEntitiesWithTag(this._targetWallSegmentTag).ToList<WeakGameEntity>()
				where entity.HasScriptOfType<WallSegment>()
				select entity;
			if (!enumerable.IsEmpty<WeakGameEntity>())
			{
				this._targetWallSegment = enumerable.First<WeakGameEntity>().GetFirstScriptOfType<WallSegment>();
				this._targetWallSegment.AttackerSiegeWeapon = this;
			}
			string sideTag = this._sideTag;
			if (!(sideTag == "left"))
			{
				if (!(sideTag == "middle"))
				{
					if (!(sideTag == "right"))
					{
						this.WeaponSide = FormationAI.BehaviorSide.Middle;
					}
					else
					{
						this.WeaponSide = FormationAI.BehaviorSide.Right;
					}
				}
				else
				{
					this.WeaponSide = FormationAI.BehaviorSide.Middle;
				}
			}
			else
			{
				this.WeaponSide = FormationAI.BehaviorSide.Left;
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				if (this.GetGateNavMeshId() != 0)
				{
					this.CleanState.Scene.SetAbilityOfFacesWithId(this.GetGateNavMeshId(), false);
				}
				foreach (LadderQueueManager ladderQueueManager in this._queueManagers)
				{
					this.CleanState.Scene.SetAbilityOfFacesWithId(ladderQueueManager.ManagedNavigationFaceId, false);
					ladderQueueManager.DeactivateImmediate();
				}
			}
			WeakGameEntity weakGameEntity3 = base.Scene.FindWeakEntitiesWithTag("ditch_filler").FirstOrDefault<WeakGameEntity>((WeakGameEntity df) => df.HasTag(this._sideTag));
			if (weakGameEntity3 != null)
			{
				this._ditchFillDebris = weakGameEntity3.GetFirstScriptOfType<SynchedMissionObject>();
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				this._gateObject.GameEntity.AttachNavigationMeshFaces(this.DynamicNavmeshIdStart + 3, true, false, false, false, true);
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
			Mission.Current.AddToWeaponListForFriendlyFirePreventing(this);
		}

		// Token: 0x06003080 RID: 12416 RVA: 0x000C3498 File Offset: 0x000C1698
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.GameEntity.IsVisibleIncludeParents())
			{
				return base.GetTickRequirement() | ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel;
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06003081 RID: 12417 RVA: 0x000C34C8 File Offset: 0x000C16C8
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (!this.CleanState.IsVisibleIncludeParents())
			{
				return;
			}
			if (!GameNetwork.IsClientOrReplay)
			{
				foreach (StandingPoint standingPoint in base.StandingPoints)
				{
					if (standingPoint.GameEntity.HasTag("move"))
					{
						standingPoint.SetIsDeactivatedSynched(this.MovementComponent.HasArrivedAtTarget);
					}
					else
					{
						UsableMissionObject usableMissionObject = standingPoint;
						bool flag;
						if (this.MovementComponent.HasArrivedAtTarget && this.State != SiegeTower.GateState.Open)
						{
							if (this.State == SiegeTower.GateState.GateFalling || this.State == SiegeTower.GateState.GateFallingWallDestroyed)
							{
								Agent userAgent = standingPoint.UserAgent;
								flag = userAgent != null && userAgent.IsPlayerControlled;
							}
							else
							{
								flag = false;
							}
						}
						else
						{
							flag = true;
						}
						usableMissionObject.SetIsDeactivatedSynched(flag);
					}
				}
			}
			if (!GameNetwork.IsClientOrReplay && this.MovementComponent.HasArrivedAtTarget && !this.HasArrivedAtTarget)
			{
				this.HasArrivedAtTarget = true;
				this.ActiveWaitStandingPoint = base.WaitStandingPoints[1];
			}
			if (this.HasArrivedAtTarget)
			{
				switch (this.State)
				{
				case SiegeTower.GateState.Closed:
					if (!GameNetwork.IsClientOrReplay && base.UserCountNotInStruckAction > 0)
					{
						this.State = SiegeTower.GateState.GateFalling;
						return;
					}
					break;
				case SiegeTower.GateState.Open:
					break;
				case SiegeTower.GateState.GateFalling:
				{
					MatrixFrame frame = this._gateObject.GameEntity.GetFrame();
					frame.rotation.RotateAboutSide(this._fallAngularSpeed * dt);
					this._gateObject.GameEntity.SetFrame(ref frame, true);
					if (Vec3.DotProduct(frame.rotation.u, this._openStateRotation.f) < 0.025f)
					{
						this.State = SiegeTower.GateState.GateFallingWallDestroyed;
					}
					this._fallAngularSpeed += dt * 2f * MathF.Max(0.3f, 1f - frame.rotation.u.z);
					return;
				}
				case SiegeTower.GateState.GateFallingWallDestroyed:
				{
					MatrixFrame frame2 = this._gateObject.GameEntity.GetFrame();
					frame2.rotation.RotateAboutSide(this._fallAngularSpeed * dt);
					this._gateObject.GameEntity.SetFrame(ref frame2, true);
					float num = Vec3.DotProduct(frame2.rotation.u, this._openStateRotation.f);
					if (this._fallAngularSpeed > 0f && num < 0.05f)
					{
						frame2.rotation = this._openStateRotation;
						this._gateObject.GameEntity.SetFrame(ref frame2, true);
						this._gateObject.GameEntity.Skeleton.SetAnimationAtChannel(this._gateTrembleAnimationIndex, 0, 1f, -1f, 0f);
						SoundEvent gateOpenSound = this._gateOpenSound;
						if (gateOpenSound != null)
						{
							gateOpenSound.Stop();
						}
						if (!GameNetwork.IsClientOrReplay)
						{
							this.State = SiegeTower.GateState.Open;
						}
					}
					this._fallAngularSpeed += dt * 3f * MathF.Max(0.3f, 1f - frame2.rotation.u.z);
					return;
				}
				default:
					Debug.FailedAssert("Invalid gate state.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Objects\\Siege\\SiegeTower.cs", "OnTick", 960);
					break;
				}
			}
		}

		// Token: 0x06003082 RID: 12418 RVA: 0x000C37F4 File Offset: 0x000C19F4
		protected internal override void OnTickParallel(float dt)
		{
			base.OnTickParallel(dt);
			if (!this.CleanState.IsVisibleIncludeParents())
			{
				return;
			}
			this.MovementComponent.TickParallelManually(dt);
			if (this._gateStandingPoint.HasUser)
			{
				Agent userAgent = this._gateStandingPoint.UserAgent;
				if (userAgent.IsInBeingStruckAction)
				{
					userAgent.ClearHandInverseKinematics();
					return;
				}
				Agent userAgent2 = this._gateStandingPoint.UserAgent;
				MatrixFrame globalFrame = this.CleanState.GetGlobalFrame();
				userAgent2.SetHandInverseKinematicsFrameForMissionObjectUsage(in this._gateStandingPointLocalIKFrame, in globalFrame, 0f);
			}
		}

		// Token: 0x06003083 RID: 12419 RVA: 0x000C387C File Offset: 0x000C1A7C
		protected internal override void OnMissionReset()
		{
			base.OnMissionReset();
			if (!GameNetwork.IsClientOrReplay && this.GetGateNavMeshId() > 0)
			{
				this.CleanState.Scene.SetAbilityOfFacesWithId(this.GetGateNavMeshId(), false);
			}
			this._state = SiegeTower.GateState.Closed;
			this._hasArrivedAtTarget = false;
			MatrixFrame frame = this._gateObject.GameEntity.GetFrame();
			frame.rotation = this._closedStateRotation;
			SynchedMissionObject handleObject = this._handleObject;
			if (handleObject != null)
			{
				handleObject.GameEntity.Skeleton.SetAnimationAtChannel(-1, 0, 1f, -1f, 0f);
			}
			this._gateObject.GameEntity.Skeleton.SetAnimationAtChannel(-1, 0, 1f, -1f, 0f);
			this._gateObject.GameEntity.SetFrame(ref frame, true);
			if (this._destroyedWallEntity != null && this._nonDestroyedWallEntity != null)
			{
				this._nonDestroyedWallEntity.SetVisibilityExcludeParents(false);
				this._destroyedWallEntity.SetVisibilityExcludeParents(true);
			}
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				standingPoint.IsDeactivated = !standingPoint.GameEntity.HasTag("move");
			}
		}

		// Token: 0x06003084 RID: 12420 RVA: 0x000C39E4 File Offset: 0x000C1BE4
		public void OnDestroyed(DestructableComponent destroyedComponent, Agent destroyerAgent, in MissionWeapon weapon, ScriptComponentBehavior attackerScriptComponentBehavior, int inflictedDamage)
		{
			bool flag = false;
			MissionWeapon missionWeapon = weapon;
			if (missionWeapon.CurrentUsageItem != null)
			{
				missionWeapon = weapon;
				bool flag2;
				if (missionWeapon.CurrentUsageItem.WeaponFlags.HasAnyFlag(WeaponFlags.Burning))
				{
					missionWeapon = weapon;
					flag2 = missionWeapon.CurrentUsageItem.WeaponFlags.HasAnyFlag(WeaponFlags.AffectsArea | WeaponFlags.AffectsAreaBig);
				}
				else
				{
					flag2 = false;
				}
				flag = flag2;
			}
			Mission.Current.KillAgentsOnEntity(destroyedComponent.CurrentState, destroyerAgent, flag);
			foreach (GameEntity gameEntity in this._aiBarriers)
			{
				gameEntity.SetVisibilityExcludeParents(true);
			}
		}

		// Token: 0x06003085 RID: 12421 RVA: 0x000C3A9C File Offset: 0x000C1C9C
		public void HighlightPath()
		{
			this.MovementComponent.HighlightPath();
		}

		// Token: 0x06003086 RID: 12422 RVA: 0x000C3AAC File Offset: 0x000C1CAC
		public void SwitchGhostEntityMovementMode(bool isGhostEnabled)
		{
			if (isGhostEnabled)
			{
				if (!this._isGhostMovementOn)
				{
					base.RemoveComponent(this.MovementComponent);
					this.GhostEntityMove = true;
					this.MovementComponent.GhostEntitySpeedMultiplier *= 3f;
					this.MovementComponent.SetGhostVisibility(true);
				}
				this._isGhostMovementOn = true;
				return;
			}
			if (this._isGhostMovementOn)
			{
				base.RemoveComponent(this.MovementComponent);
				PathLastNodeFixer component = base.GetComponent<PathLastNodeFixer>();
				base.RemoveComponent(component);
				this.AddRegularMovementComponent();
				this.MovementComponent.SetGhostVisibility(false);
			}
			this._isGhostMovementOn = false;
		}

		// Token: 0x06003087 RID: 12423 RVA: 0x000C3B40 File Offset: 0x000C1D40
		public MatrixFrame GetInitialFrame()
		{
			SiegeWeaponMovementComponent movementComponent = this.MovementComponent;
			if (movementComponent == null)
			{
				return this.CleanState.GetGlobalFrame();
			}
			return movementComponent.GetInitialFrame();
		}

		// Token: 0x06003088 RID: 12424 RVA: 0x000C3B6C File Offset: 0x000C1D6C
		private void OnSiegeTowerGateStateChange()
		{
			switch (this.State)
			{
			case SiegeTower.GateState.Closed:
			{
				SynchedMissionObject handleObject = this._handleObject;
				if (handleObject != null)
				{
					handleObject.GameEntity.Skeleton.SetAnimationAtChannel(this._gateHandleIdleAnimationIndex, 0, 1f, -1f, 0f);
				}
				if (!GameNetwork.IsClientOrReplay && this.GetGateNavMeshId() != 0)
				{
					this.CleanState.Scene.SetAbilityOfFacesWithId(this.GetGateNavMeshId(), false);
					return;
				}
				break;
			}
			case SiegeTower.GateState.Open:
				if (this._gateObject.GameEntity.Skeleton.GetAnimationIndexAtChannel(0) != this._gateHandleIdleAnimationIndex)
				{
					MatrixFrame frame = this._gateObject.GameEntity.GetFrame();
					frame.rotation = this._openStateRotation;
					this._gateObject.GameEntity.SetFrame(ref frame, true);
					this._gateObject.GameEntity.Skeleton.SetAnimationAtChannel(this._gateTrembleAnimationIndex, 0, 1f, -1f, 0f);
					SoundEvent gateOpenSound = this._gateOpenSound;
					if (gateOpenSound != null)
					{
						gateOpenSound.Stop();
					}
					if (!GameNetwork.IsClientOrReplay && this.GetGateNavMeshId() != 0)
					{
						this.CleanState.Scene.SetAbilityOfFacesWithId(this.GetGateNavMeshId(), true);
					}
				}
				if (!GameNetwork.IsClientOrReplay)
				{
					this.CleanState.Scene.SetAbilityOfFacesWithId(this.GetGateNavMeshId(), true);
				}
				foreach (GameEntity gameEntity in this._aiBarriers)
				{
					gameEntity.SetVisibilityExcludeParents(false);
				}
				break;
			case SiegeTower.GateState.GateFalling:
				this._fallAngularSpeed = 0f;
				this._gateOpenSound = SoundEvent.CreateEvent(this._gateOpenSoundIndex, base.Scene);
				this._gateOpenSound.PlayInPosition(this._gateObject.GameEntity.GlobalPosition);
				return;
			case SiegeTower.GateState.GateFallingWallDestroyed:
				if (this._destroyedWallEntity != null && this._nonDestroyedWallEntity != null)
				{
					this._fallAngularSpeed *= 0.1f;
					this._nonDestroyedWallEntity.SetVisibilityExcludeParents(false);
					this._destroyedWallEntity.SetVisibilityExcludeParents(true);
					if (this._battlementDestroyedParticle != null)
					{
						Mission.Current.AddParticleSystemBurstByName(this.BattlementDestroyedParticle, this._battlementDestroyedParticle.GetGlobalFrame(), false);
						return;
					}
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06003089 RID: 12425 RVA: 0x000C3DE4 File Offset: 0x000C1FE4
		private void AddRegularMovementComponent()
		{
			this.MovementComponent = new SiegeWeaponMovementComponent
			{
				PathEntityName = this.PathEntity,
				MinSpeed = this.MinSpeed,
				MaxSpeed = this.MaxSpeed,
				MainObject = this,
				WheelDiameter = this.WheelDiameter,
				NavMeshIdToDisableOnDestination = this.NavMeshIdToDisableOnDestination,
				MovementSoundCodeID = SoundEvent.GetEventIdFromString("event:/mission/siege/siegetower/move"),
				GhostEntitySpeedMultiplier = this.GhostEntitySpeedMultiplier
			};
			base.AddComponent(this.MovementComponent);
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x000C3E68 File Offset: 0x000C2068
		private void SetUpGhostEntity()
		{
			PathLastNodeFixer pathLastNodeFixer = new PathLastNodeFixer
			{
				PathHolder = this
			};
			base.AddComponent(pathLastNodeFixer);
			this.MovementComponent = new SiegeWeaponMovementComponent
			{
				PathEntityName = this.PathEntity,
				MainObject = this,
				GhostEntitySpeedMultiplier = this.GhostEntitySpeedMultiplier
			};
			base.AddComponent(this.MovementComponent);
			this.MovementComponent.SetupGhostEntity();
		}

		// Token: 0x0600308B RID: 12427 RVA: 0x000C3ECC File Offset: 0x000C20CC
		private void UpdateGhostEntity()
		{
			WeakGameEntity firstChildEntityWithTag = this.CleanState.GetFirstChildEntityWithTag("ghost_object");
			if (firstChildEntityWithTag.IsValid && firstChildEntityWithTag.ChildCount > 0)
			{
				this.MovementComponent.GhostEntitySpeedMultiplier = this.GhostEntitySpeedMultiplier;
				WeakGameEntity child = firstChildEntityWithTag.GetChild(0);
				MatrixFrame frame = child.GetFrame();
				child.SetFrame(ref frame, true);
			}
		}

		// Token: 0x0600308C RID: 12428 RVA: 0x000C3F2C File Offset: 0x000C212C
		public void SetSpawnedFromSpawner()
		{
			this._spawnedFromSpawner = true;
		}

		// Token: 0x0600308D RID: 12429 RVA: 0x000C3F38 File Offset: 0x000C2138
		public override void OnAfterReadFromNetwork(ValueTuple<BaseSynchedMissionObjectReadableRecord, ISynchedMissionObjectReadableRecord> synchedMissionObjectReadableRecord, bool allowVisibilityUpdate = true)
		{
			base.OnAfterReadFromNetwork(synchedMissionObjectReadableRecord, allowVisibilityUpdate);
			SiegeTower.SiegeTowerRecord siegeTowerRecord = (SiegeTower.SiegeTowerRecord)synchedMissionObjectReadableRecord.Item2;
			this.HasArrivedAtTarget = siegeTowerRecord.HasArrivedAtTarget;
			this._state = (SiegeTower.GateState)siegeTowerRecord.State;
			this._fallAngularSpeed = siegeTowerRecord.FallAngularSpeed;
			if (this._state == SiegeTower.GateState.Open)
			{
				if (this._destroyedWallEntity != null && this._nonDestroyedWallEntity != null)
				{
					this._nonDestroyedWallEntity.SetVisibilityExcludeParents(false);
					this._destroyedWallEntity.SetVisibilityExcludeParents(true);
				}
				MatrixFrame frame = this._gateObject.GameEntity.GetFrame();
				frame.rotation = this._openStateRotation;
				this._gateObject.GameEntity.SetFrame(ref frame, true);
			}
			float num = siegeTowerRecord.TotalDistanceTraveled;
			num += 0.05f;
			this.MovementComponent.SetTotalDistanceTraveledForPathTracker(num);
			this.MovementComponent.SetTargetFrameForPathTracker();
		}

		// Token: 0x0600308E RID: 12430 RVA: 0x000C401C File Offset: 0x000C221C
		public void AssignParametersFromSpawner(string pathEntityName, string targetWallSegment, string sideTag, int soilNavMeshID1, int soilNavMeshID2, int ditchNavMeshID1, int ditchNavMeshID2, int groundToSoilNavMeshID1, int groundToSoilNavMeshID2, int soilGenericNavMeshID, int groundGenericNavMeshID, Mat3 openStateRotation, string barrierTagToRemove)
		{
			this.PathEntity = pathEntityName;
			this._targetWallSegmentTag = targetWallSegment;
			this._sideTag = sideTag;
			this._soilNavMeshID1 = soilNavMeshID1;
			this._soilNavMeshID2 = soilNavMeshID2;
			this._ditchNavMeshID1 = ditchNavMeshID1;
			this._ditchNavMeshID2 = ditchNavMeshID2;
			this._groundToSoilNavMeshID1 = groundToSoilNavMeshID1;
			this._groundToSoilNavMeshID2 = groundToSoilNavMeshID2;
			this._soilGenericNavMeshID = soilGenericNavMeshID;
			this._groundGenericNavMeshID = groundGenericNavMeshID;
			this._openStateRotation = openStateRotation;
			this.BarrierTagToRemove = barrierTagToRemove;
		}

		// Token: 0x0600308F RID: 12431 RVA: 0x000C4090 File Offset: 0x000C2290
		public bool GetNavmeshFaceIds(out List<int> navmeshFaceIds)
		{
			navmeshFaceIds = new List<int>
			{
				this.DynamicNavmeshIdStart + 1,
				this.DynamicNavmeshIdStart + 3,
				this.DynamicNavmeshIdStart + 5,
				this.DynamicNavmeshIdStart + 6,
				this.DynamicNavmeshIdStart + 7
			};
			return true;
		}

		// Token: 0x06003090 RID: 12432 RVA: 0x000C40EC File Offset: 0x000C22EC
		public void OnFormationFrameChanged(Agent agent, bool hasFrame, WorldPosition frame)
		{
			foreach (LadderQueueManager ladderQueueManager in this._queueManagers)
			{
				ladderQueueManager.OnFormationFrameChanged(agent, hasFrame, frame);
			}
		}

		// Token: 0x04001420 RID: 5152
		private const int LeftLadderNavMeshIdLocal = 5;

		// Token: 0x04001421 RID: 5153
		private const int MiddleLadderNavMeshIdLocal = 6;

		// Token: 0x04001422 RID: 5154
		private const int RightLadderNavMeshIdLocal = 7;

		// Token: 0x04001423 RID: 5155
		private const string BreakableWallTag = "breakable_wall";

		// Token: 0x04001424 RID: 5156
		private const string DestroyedWallTag = "destroyed";

		// Token: 0x04001425 RID: 5157
		private const string NonDestroyedWallTag = "non_destroyed";

		// Token: 0x04001426 RID: 5158
		private const string LadderTag = "ladder";

		// Token: 0x04001427 RID: 5159
		private const string BattlementDestroyedParticleTag = "particle_spawnpoint";

		// Token: 0x04001428 RID: 5160
		public string GateTag = "gate";

		// Token: 0x04001429 RID: 5161
		public string GateOpenTag = "gateOpen";

		// Token: 0x0400142A RID: 5162
		public string HandleTag = "handle";

		// Token: 0x0400142B RID: 5163
		public string GateHandleIdleAnimation = "siegetower_handle_idle";

		// Token: 0x0400142C RID: 5164
		private int _gateHandleIdleAnimationIndex = -1;

		// Token: 0x0400142D RID: 5165
		public string GateTrembleAnimation = "siegetower_door_stop";

		// Token: 0x0400142E RID: 5166
		private int _gateTrembleAnimationIndex = -1;

		// Token: 0x0400142F RID: 5167
		public string BattlementDestroyedParticle = "psys_adobe_battlement_destroyed";

		// Token: 0x04001430 RID: 5168
		private string _targetWallSegmentTag;

		// Token: 0x04001431 RID: 5169
		public bool GhostEntityMove = true;

		// Token: 0x04001432 RID: 5170
		public float GhostEntitySpeedMultiplier = 1f;

		// Token: 0x04001433 RID: 5171
		private string _sideTag;

		// Token: 0x04001434 RID: 5172
		private bool _hasLadders;

		// Token: 0x04001435 RID: 5173
		public float WheelDiameter = 1.3f;

		// Token: 0x04001436 RID: 5174
		public float MinSpeed = 0.5f;

		// Token: 0x04001437 RID: 5175
		public float MaxSpeed = 1f;

		// Token: 0x04001438 RID: 5176
		public int GateNavMeshId;

		// Token: 0x04001439 RID: 5177
		public int NavMeshIdToDisableOnDestination = -1;

		// Token: 0x0400143A RID: 5178
		private int _soilNavMeshID1;

		// Token: 0x0400143B RID: 5179
		private int _soilNavMeshID2;

		// Token: 0x0400143C RID: 5180
		private int _ditchNavMeshID1;

		// Token: 0x0400143D RID: 5181
		private int _ditchNavMeshID2;

		// Token: 0x0400143E RID: 5182
		private int _groundToSoilNavMeshID1;

		// Token: 0x0400143F RID: 5183
		private int _groundToSoilNavMeshID2;

		// Token: 0x04001440 RID: 5184
		private int _soilGenericNavMeshID;

		// Token: 0x04001441 RID: 5185
		private int _groundGenericNavMeshID;

		// Token: 0x04001442 RID: 5186
		public string BarrierTagToRemove = "barrier";

		// Token: 0x04001443 RID: 5187
		private List<GameEntity> _aiBarriers;

		// Token: 0x04001444 RID: 5188
		private bool _isGhostMovementOn;

		// Token: 0x04001445 RID: 5189
		private bool _hasArrivedAtTarget;

		// Token: 0x04001446 RID: 5190
		private SiegeTower.GateState _state;

		// Token: 0x04001447 RID: 5191
		private SynchedMissionObject _gateObject;

		// Token: 0x04001448 RID: 5192
		private SynchedMissionObject _handleObject;

		// Token: 0x04001449 RID: 5193
		private SoundEvent _gateOpenSound;

		// Token: 0x0400144A RID: 5194
		private int _gateOpenSoundIndex = -1;

		// Token: 0x0400144B RID: 5195
		private Mat3 _openStateRotation;

		// Token: 0x0400144C RID: 5196
		private Mat3 _closedStateRotation;

		// Token: 0x0400144D RID: 5197
		private float _fallAngularSpeed;

		// Token: 0x0400144E RID: 5198
		private GameEntity _cleanState;

		// Token: 0x0400144F RID: 5199
		private GameEntity _destroyedWallEntity;

		// Token: 0x04001450 RID: 5200
		private GameEntity _nonDestroyedWallEntity;

		// Token: 0x04001451 RID: 5201
		private GameEntity _battlementDestroyedParticle;

		// Token: 0x04001452 RID: 5202
		private StandingPoint _gateStandingPoint;

		// Token: 0x04001453 RID: 5203
		private MatrixFrame _gateStandingPointLocalIKFrame;

		// Token: 0x04001454 RID: 5204
		private SynchedMissionObject _ditchFillDebris;

		// Token: 0x04001455 RID: 5205
		private List<LadderQueueManager> _queueManagers;

		// Token: 0x04001456 RID: 5206
		private WallSegment _targetWallSegment;

		// Token: 0x04001457 RID: 5207
		private List<SiegeLadder> _sameSideSiegeLadders;

		// Token: 0x0200062D RID: 1581
		[DefineSynchedMissionObjectType(typeof(SiegeTower))]
		public struct SiegeTowerRecord : ISynchedMissionObjectReadableRecord
		{
			// Token: 0x17000ABB RID: 2747
			// (get) Token: 0x06003FF8 RID: 16376 RVA: 0x000F811B File Offset: 0x000F631B
			// (set) Token: 0x06003FF9 RID: 16377 RVA: 0x000F8123 File Offset: 0x000F6323
			public bool HasArrivedAtTarget { get; private set; }

			// Token: 0x17000ABC RID: 2748
			// (get) Token: 0x06003FFA RID: 16378 RVA: 0x000F812C File Offset: 0x000F632C
			// (set) Token: 0x06003FFB RID: 16379 RVA: 0x000F8134 File Offset: 0x000F6334
			public int State { get; private set; }

			// Token: 0x17000ABD RID: 2749
			// (get) Token: 0x06003FFC RID: 16380 RVA: 0x000F813D File Offset: 0x000F633D
			// (set) Token: 0x06003FFD RID: 16381 RVA: 0x000F8145 File Offset: 0x000F6345
			public float FallAngularSpeed { get; private set; }

			// Token: 0x17000ABE RID: 2750
			// (get) Token: 0x06003FFE RID: 16382 RVA: 0x000F814E File Offset: 0x000F634E
			// (set) Token: 0x06003FFF RID: 16383 RVA: 0x000F8156 File Offset: 0x000F6356
			public float TotalDistanceTraveled { get; private set; }

			// Token: 0x06004000 RID: 16384 RVA: 0x000F8160 File Offset: 0x000F6360
			public bool ReadFromNetwork(ref bool bufferReadValid)
			{
				this.HasArrivedAtTarget = GameNetworkMessage.ReadBoolFromPacket(ref bufferReadValid);
				this.State = GameNetworkMessage.ReadIntFromPacket(CompressionMission.SiegeTowerGateStateCompressionInfo, ref bufferReadValid);
				this.FallAngularSpeed = GameNetworkMessage.ReadFloatFromPacket(CompressionMission.SiegeMachineComponentAngularSpeedCompressionInfo, ref bufferReadValid);
				this.TotalDistanceTraveled = GameNetworkMessage.ReadFloatFromPacket(CompressionBasic.PositionCompressionInfo, ref bufferReadValid);
				return bufferReadValid;
			}
		}

		// Token: 0x0200062E RID: 1582
		public enum GateState
		{
			// Token: 0x040020CF RID: 8399
			Closed,
			// Token: 0x040020D0 RID: 8400
			Open,
			// Token: 0x040020D1 RID: 8401
			GateFalling,
			// Token: 0x040020D2 RID: 8402
			GateFallingWallDestroyed,
			// Token: 0x040020D3 RID: 8403
			NumberOfStates
		}
	}
}
