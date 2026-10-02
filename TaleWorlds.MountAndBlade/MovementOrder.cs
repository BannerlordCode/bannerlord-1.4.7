using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000156 RID: 342
	public struct MovementOrder
	{
		// Token: 0x170003DF RID: 991
		// (get) Token: 0x060011ED RID: 4589 RVA: 0x000373AD File Offset: 0x000355AD
		// (set) Token: 0x060011EE RID: 4590 RVA: 0x000373B5 File Offset: 0x000355B5
		public Formation TargetFormation { get; private set; }

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x060011EF RID: 4591 RVA: 0x000373BE File Offset: 0x000355BE
		public Agent _targetAgent { get; }

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x060011F0 RID: 4592 RVA: 0x000373C8 File Offset: 0x000355C8
		public OrderType OrderType
		{
			get
			{
				switch (this.OrderEnum)
				{
				case MovementOrder.MovementOrderEnum.AttackEntity:
					return OrderType.AttackEntity;
				case MovementOrder.MovementOrderEnum.Charge:
					return OrderType.Charge;
				case MovementOrder.MovementOrderEnum.ChargeToTarget:
					return OrderType.ChargeWithTarget;
				case MovementOrder.MovementOrderEnum.Follow:
					return OrderType.FollowMe;
				case MovementOrder.MovementOrderEnum.FollowEntity:
					return OrderType.FollowEntity;
				case MovementOrder.MovementOrderEnum.Move:
					return OrderType.Move;
				case MovementOrder.MovementOrderEnum.Retreat:
					return OrderType.Retreat;
				case MovementOrder.MovementOrderEnum.Stop:
					return OrderType.StandYourGround;
				case MovementOrder.MovementOrderEnum.Advance:
					return OrderType.Advance;
				case MovementOrder.MovementOrderEnum.FallBack:
					return OrderType.FallBack;
				}
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Orders\\MovementOrder.cs", "OrderType", 114);
				return OrderType.Move;
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x060011F1 RID: 4593 RVA: 0x00037444 File Offset: 0x00035644
		public MovementOrder.MovementStateEnum MovementState
		{
			get
			{
				MovementOrder.MovementOrderEnum orderEnum = this.OrderEnum;
				if (orderEnum - MovementOrder.MovementOrderEnum.Charge > 1)
				{
					if (orderEnum == MovementOrder.MovementOrderEnum.Retreat)
					{
						return MovementOrder.MovementStateEnum.Retreat;
					}
					if (orderEnum != MovementOrder.MovementOrderEnum.Stop)
					{
						return MovementOrder.MovementStateEnum.Hold;
					}
					return MovementOrder.MovementStateEnum.StandGround;
				}
				else
				{
					if (this._position.IsValid)
					{
						return MovementOrder.MovementStateEnum.Hold;
					}
					return MovementOrder.MovementStateEnum.Charge;
				}
			}
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x00037480 File Offset: 0x00035680
		private MovementOrder(MovementOrder.MovementOrderEnum orderEnum)
		{
			this.OrderEnum = orderEnum;
			if (orderEnum != MovementOrder.MovementOrderEnum.Charge)
			{
				switch (orderEnum)
				{
				case MovementOrder.MovementOrderEnum.Retreat:
					this._positionLambda = null;
					goto IL_0050;
				case MovementOrder.MovementOrderEnum.Advance:
					this._positionLambda = null;
					goto IL_0050;
				case MovementOrder.MovementOrderEnum.FallBack:
					this._positionLambda = null;
					goto IL_0050;
				}
				this._positionLambda = null;
			}
			else
			{
				this._positionLambda = null;
			}
			IL_0050:
			this.TargetFormation = null;
			this.TargetEntity = null;
			this._targetAgent = null;
			this._tickTimer = new Timer(Mission.Current.CurrentTime, 0.5f, true);
			this._lastPosition = WorldPosition.Invalid;
			this._isFacingDirection = false;
			this._position = WorldPosition.Invalid;
			this._getPositionResultCache = WorldPosition.Invalid;
			this._getPositionFirstSectionCache = WorldPosition.Invalid;
			this._getPositionIsNavmeshlessCache = false;
			this._engageTargetPositionCache = WorldPosition.Invalid;
			this._engageTargetPositionOffset = 0f;
			this._followState = MovementOrder.FollowState.Stop;
			this._departStartTime = -1f;
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x00037570 File Offset: 0x00035770
		private MovementOrder(MovementOrder.MovementOrderEnum orderEnum, Formation targetFormation)
		{
			this.OrderEnum = orderEnum;
			this._positionLambda = null;
			this.TargetFormation = targetFormation;
			this.TargetEntity = null;
			this._targetAgent = null;
			this._tickTimer = new Timer(Mission.Current.CurrentTime, 0.5f, true);
			this._lastPosition = WorldPosition.Invalid;
			this._isFacingDirection = false;
			this._position = WorldPosition.Invalid;
			this._getPositionResultCache = WorldPosition.Invalid;
			this._getPositionFirstSectionCache = WorldPosition.Invalid;
			this._getPositionIsNavmeshlessCache = false;
			this._engageTargetPositionCache = WorldPosition.Invalid;
			this._engageTargetPositionOffset = 0f;
			this._followState = MovementOrder.FollowState.Stop;
			this._departStartTime = -1f;
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x00037620 File Offset: 0x00035820
		private WorldPosition ComputeAttackEntityWaitPosition(Formation formation, WeakGameEntity targetEntity)
		{
			Scene scene = formation.Team.Mission.Scene;
			WorldPosition worldPosition = new WorldPosition(scene, UIntPtr.Zero, targetEntity.GlobalPosition, false);
			Vec2 vec = formation.CachedAveragePosition - worldPosition.AsVec2;
			MatrixFrame matrixFrame = targetEntity.GetGlobalFrame();
			Vec2 vec2 = matrixFrame.rotation.f.AsVec2.Normalized();
			Vec2 vec3 = ((vec.DotProduct(vec2) >= 0f) ? vec2 : (-vec2));
			WorldPosition worldPosition2 = worldPosition;
			worldPosition2.SetVec2(worldPosition.AsVec2 + vec3 * 3f);
			if (scene.DoesPathExistBetweenPositions(worldPosition2, formation.CachedMedianPosition))
			{
				return worldPosition2;
			}
			WorldPosition worldPosition3 = worldPosition;
			worldPosition3.SetVec2(worldPosition.AsVec2 - vec3 * 3f);
			if (scene.DoesPathExistBetweenPositions(worldPosition3, formation.CachedMedianPosition))
			{
				return worldPosition3;
			}
			worldPosition3 = worldPosition;
			Vec2 asVec = worldPosition.AsVec2;
			matrixFrame = targetEntity.GetGlobalFrame();
			worldPosition3.SetVec2(asVec + matrixFrame.rotation.s.AsVec2.Normalized() * 3f);
			if (scene.DoesPathExistBetweenPositions(worldPosition3, formation.CachedMedianPosition))
			{
				return worldPosition3;
			}
			worldPosition3 = worldPosition;
			Vec2 asVec2 = worldPosition.AsVec2;
			matrixFrame = targetEntity.GetGlobalFrame();
			worldPosition3.SetVec2(asVec2 - matrixFrame.rotation.s.AsVec2.Normalized() * 3f);
			if (!scene.DoesPathExistBetweenPositions(worldPosition3, formation.CachedMedianPosition))
			{
				return worldPosition2;
			}
			return worldPosition3;
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x000377BC File Offset: 0x000359BC
		private MovementOrder(MovementOrder.MovementOrderEnum orderEnum, GameEntity targetEntity, bool surroundEntity)
		{
			targetEntity.GetFirstScriptOfType<UsableMachine>();
			this.OrderEnum = orderEnum;
			this._positionLambda = delegate(Formation f)
			{
				WorldPosition worldPosition = new WorldPosition(Mission.Current.Scene, UIntPtr.Zero, targetEntity.GlobalPosition, false);
				Vec2 vec = f.CachedAveragePosition - worldPosition.AsVec2;
				MatrixFrame matrixFrame = targetEntity.GetGlobalFrame();
				Vec2 vec2 = matrixFrame.rotation.f.AsVec2.Normalized();
				Vec2 vec3 = ((vec.DotProduct(vec2) >= 0f) ? vec2 : (-vec2));
				WorldPosition worldPosition2 = worldPosition;
				worldPosition2.SetVec2MT(worldPosition.AsVec2 + vec3 * 3f);
				if (Mission.Current.Scene.DoesPathExistBetweenPositions(worldPosition2, f.CachedMedianPosition))
				{
					return worldPosition2;
				}
				WorldPosition worldPosition3 = worldPosition;
				worldPosition3.SetVec2MT(worldPosition.AsVec2 - vec3 * 3f);
				if (Mission.Current.Scene.DoesPathExistBetweenPositions(worldPosition3, f.CachedMedianPosition))
				{
					return worldPosition3;
				}
				worldPosition3 = worldPosition;
				Vec2 asVec = worldPosition.AsVec2;
				matrixFrame = targetEntity.GetGlobalFrame();
				worldPosition3.SetVec2MT(asVec + matrixFrame.rotation.s.AsVec2.Normalized() * 3f);
				if (Mission.Current.Scene.DoesPathExistBetweenPositions(worldPosition3, f.CachedMedianPosition))
				{
					return worldPosition3;
				}
				worldPosition3 = worldPosition;
				Vec2 asVec2 = worldPosition.AsVec2;
				matrixFrame = targetEntity.GetGlobalFrame();
				worldPosition3.SetVec2MT(asVec2 - matrixFrame.rotation.s.AsVec2.Normalized() * 3f);
				if (Mission.Current.Scene.DoesPathExistBetweenPositions(worldPosition3, f.CachedMedianPosition))
				{
					return worldPosition3;
				}
				return worldPosition2;
			};
			this.TargetEntity = targetEntity;
			this._tickTimer = new Timer(Mission.Current.CurrentTime, 0.5f, true);
			this.TargetFormation = null;
			this._targetAgent = null;
			this._lastPosition = WorldPosition.Invalid;
			this._isFacingDirection = false;
			this._position = WorldPosition.Invalid;
			this._getPositionResultCache = WorldPosition.Invalid;
			this._getPositionFirstSectionCache = WorldPosition.Invalid;
			this._getPositionIsNavmeshlessCache = false;
			this._engageTargetPositionCache = WorldPosition.Invalid;
			this._engageTargetPositionOffset = 0f;
			this._followState = MovementOrder.FollowState.Stop;
			this._departStartTime = -1f;
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x00037894 File Offset: 0x00035A94
		private MovementOrder(MovementOrder.MovementOrderEnum orderEnum, Agent targetAgent)
		{
			this.OrderEnum = orderEnum;
			WorldPosition targetAgentPos = targetAgent.GetWorldPosition();
			if (orderEnum == MovementOrder.MovementOrderEnum.Follow)
			{
				this._positionLambda = delegate(Formation f)
				{
					WorldPosition targetAgentPos3 = targetAgentPos;
					targetAgentPos3.SetVec2(targetAgentPos3.AsVec2 - f.GetMiddleFrontUnitPositionOffset());
					return targetAgentPos3;
				};
			}
			else
			{
				this._positionLambda = delegate(Formation f)
				{
					WorldPosition targetAgentPos2 = targetAgentPos;
					targetAgentPos2.SetVec2(targetAgentPos2.AsVec2 - 4f * (f.QuerySystem.Team.MedianTargetFormationPosition.AsVec2 - targetAgentPos.AsVec2).Normalized());
					Vec2 asVec = targetAgentPos2.AsVec2;
					WorldPosition lastPosition = f.GetReadonlyMovementOrderReference()._lastPosition;
					if (asVec.DistanceSquared(lastPosition.AsVec2) > 6.25f)
					{
						return targetAgentPos2;
					}
					return f.GetReadonlyMovementOrderReference()._lastPosition;
				};
			}
			this._targetAgent = targetAgent;
			this.TargetFormation = null;
			this.TargetEntity = null;
			this._tickTimer = new Timer(targetAgent.Mission.CurrentTime, 0.5f, true);
			this._lastPosition = targetAgentPos;
			this._isFacingDirection = false;
			this._position = WorldPosition.Invalid;
			this._getPositionResultCache = WorldPosition.Invalid;
			this._getPositionFirstSectionCache = WorldPosition.Invalid;
			this._getPositionIsNavmeshlessCache = false;
			this._engageTargetPositionCache = WorldPosition.Invalid;
			this._engageTargetPositionOffset = 0f;
			this._followState = MovementOrder.FollowState.Stop;
			this._departStartTime = -1f;
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x00037978 File Offset: 0x00035B78
		private MovementOrder(MovementOrder.MovementOrderEnum orderEnum, GameEntity targetEntity)
		{
			this.OrderEnum = orderEnum;
			this._positionLambda = delegate(Formation f)
			{
				WorldPosition worldPosition = new WorldPosition(Mission.Current.Scene, UIntPtr.Zero, targetEntity.GlobalPosition, false);
				worldPosition.SetVec2(worldPosition.AsVec2);
				return worldPosition;
			};
			this.TargetEntity = targetEntity;
			this.TargetFormation = null;
			this._targetAgent = null;
			this._tickTimer = new Timer(Mission.Current.CurrentTime, 0.5f, true);
			this._lastPosition = WorldPosition.Invalid;
			this._isFacingDirection = false;
			this._position = WorldPosition.Invalid;
			this._getPositionResultCache = WorldPosition.Invalid;
			this._getPositionFirstSectionCache = WorldPosition.Invalid;
			this._getPositionIsNavmeshlessCache = false;
			this._engageTargetPositionCache = WorldPosition.Invalid;
			this._engageTargetPositionOffset = 0f;
			this._followState = MovementOrder.FollowState.Stop;
			this._departStartTime = -1f;
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x00037A44 File Offset: 0x00035C44
		private MovementOrder(MovementOrder.MovementOrderEnum orderEnum, WorldPosition position)
		{
			this.OrderEnum = orderEnum;
			this._positionLambda = null;
			this._isFacingDirection = false;
			this.TargetFormation = null;
			this.TargetEntity = null;
			this._targetAgent = null;
			this._tickTimer = new Timer(Mission.Current.CurrentTime, 0.5f, true);
			this._lastPosition = WorldPosition.Invalid;
			this._position = position;
			this._getPositionResultCache = WorldPosition.Invalid;
			this._getPositionFirstSectionCache = WorldPosition.Invalid;
			this._getPositionIsNavmeshlessCache = false;
			this._engageTargetPositionCache = WorldPosition.Invalid;
			this._engageTargetPositionOffset = 0f;
			this._followState = MovementOrder.FollowState.Stop;
			this._departStartTime = -1f;
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x00037AF0 File Offset: 0x00035CF0
		public override bool Equals(object obj)
		{
			if (obj is MovementOrder)
			{
				MovementOrder movementOrder = (MovementOrder)obj;
				return (in movementOrder) == this;
			}
			return false;
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x00037B1B File Offset: 0x00035D1B
		public override int GetHashCode()
		{
			return (int)this.OrderEnum;
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00037B23 File Offset: 0x00035D23
		public static bool operator !=(in MovementOrder m, MovementOrder obj)
		{
			return m.OrderEnum != obj.OrderEnum;
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x00037B36 File Offset: 0x00035D36
		public static bool operator ==(in MovementOrder m, MovementOrder obj)
		{
			return m.OrderEnum == obj.OrderEnum;
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x00037B46 File Offset: 0x00035D46
		public static MovementOrder MovementOrderChargeToTarget(Formation targetFormation)
		{
			return new MovementOrder(MovementOrder.MovementOrderEnum.ChargeToTarget, targetFormation);
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00037B4F File Offset: 0x00035D4F
		public static MovementOrder MovementOrderFollow(Agent targetAgent)
		{
			return new MovementOrder(MovementOrder.MovementOrderEnum.Follow, targetAgent);
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x00037B58 File Offset: 0x00035D58
		public static MovementOrder MovementOrderFollowEntity(GameEntity targetEntity)
		{
			return new MovementOrder(MovementOrder.MovementOrderEnum.FollowEntity, targetEntity);
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x00037B61 File Offset: 0x00035D61
		public static MovementOrder MovementOrderMove(WorldPosition position)
		{
			return new MovementOrder(MovementOrder.MovementOrderEnum.Move, position);
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x00037B6A File Offset: 0x00035D6A
		public static MovementOrder MovementOrderAttackEntity(GameEntity targetEntity, bool surroundEntity)
		{
			return new MovementOrder(MovementOrder.MovementOrderEnum.AttackEntity, targetEntity, surroundEntity);
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x00037B74 File Offset: 0x00035D74
		public static int GetMovementOrderDefensiveness(MovementOrder.MovementOrderEnum orderEnum)
		{
			if (orderEnum == MovementOrder.MovementOrderEnum.Charge || orderEnum == MovementOrder.MovementOrderEnum.ChargeToTarget)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x00037B81 File Offset: 0x00035D81
		public static int GetMovementOrderDefensivenessChange(MovementOrder.MovementOrderEnum previousOrderEnum, MovementOrder.MovementOrderEnum nextOrderEnum)
		{
			if (previousOrderEnum == MovementOrder.MovementOrderEnum.Charge || previousOrderEnum == MovementOrder.MovementOrderEnum.ChargeToTarget)
			{
				if (nextOrderEnum != MovementOrder.MovementOrderEnum.Charge && nextOrderEnum != MovementOrder.MovementOrderEnum.ChargeToTarget)
				{
					return 1;
				}
				return 0;
			}
			else
			{
				if (nextOrderEnum == MovementOrder.MovementOrderEnum.Charge || nextOrderEnum == MovementOrder.MovementOrderEnum.ChargeToTarget)
				{
					return -1;
				}
				return 0;
			}
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x00037BA4 File Offset: 0x00035DA4
		private static void RetreatAux(Formation formation)
		{
			for (int i = formation.Detachments.Count - 1; i >= 0; i--)
			{
				formation.LeaveDetachment(formation.Detachments[i]);
			}
			formation.ApplyActionOnEachUnitViaBackupList(delegate(Agent agent)
			{
				if (agent.IsAIControlled)
				{
					agent.Retreat(true);
				}
			});
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x00037C00 File Offset: 0x00035E00
		private static WorldPosition GetAlternatePositionForNavmeshlessOrOutOfBoundsPosition(Formation f, WorldPosition originalPosition)
		{
			float num = 1f;
			WorldPosition alternatePositionForNavmeshlessOrOutOfBoundsPosition = Mission.Current.GetAlternatePositionForNavmeshlessOrOutOfBoundsPosition(originalPosition.AsVec2 - f.CachedAveragePosition, originalPosition, ref num);
			FormationAI ai = f.AI;
			if (((ai != null) ? ai.ActiveBehavior : null) != null)
			{
				f.AI.ActiveBehavior.NavmeshlessTargetPositionPenalty = num;
			}
			return alternatePositionForNavmeshlessOrOutOfBoundsPosition;
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x00037C58 File Offset: 0x00035E58
		private void GetPositionAuxFollow(Formation f)
		{
			Vec2 vec = Vec2.Zero;
			if (this._followState != MovementOrder.FollowState.Move && this._targetAgent.MountAgent != null)
			{
				vec += f.Direction * -2f;
			}
			if (this._followState == MovementOrder.FollowState.Move && f.PhysicalClass.IsMounted())
			{
				vec += 2f * this._targetAgent.Velocity.AsVec2;
			}
			else if (this._followState == MovementOrder.FollowState.Move)
			{
				f.PhysicalClass.IsMounted();
			}
			WorldPosition worldPosition = this._targetAgent.GetWorldPosition();
			worldPosition.SetVec2(worldPosition.AsVec2 - f.GetMiddleFrontUnitPositionOffset() + vec);
			if (this._followState == MovementOrder.FollowState.Stop || this._followState == MovementOrder.FollowState.Depart)
			{
				float num = (f.PhysicalClass.IsMounted() ? 4f : 2.5f);
				if (Mission.Current.IsTeleportingAgents || worldPosition.AsVec2.DistanceSquared(this._lastPosition.AsVec2) > num * num)
				{
					this._lastPosition = worldPosition;
					return;
				}
			}
			else
			{
				this._lastPosition = worldPosition;
			}
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x00037D7C File Offset: 0x00035F7C
		public Vec2 GetPosition(Formation f)
		{
			return this.CreateNewOrderWorldPositionMT(f, WorldPosition.WorldPositionEnforcedCache.None).AsVec2;
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x00037D9C File Offset: 0x00035F9C
		public Vec2 GetTargetVelocity()
		{
			switch (this.OrderEnum)
			{
			case MovementOrder.MovementOrderEnum.AttackEntity:
			case MovementOrder.MovementOrderEnum.Charge:
			case MovementOrder.MovementOrderEnum.ChargeToTarget:
			case MovementOrder.MovementOrderEnum.FollowEntity:
			case MovementOrder.MovementOrderEnum.Move:
			case MovementOrder.MovementOrderEnum.Retreat:
			case MovementOrder.MovementOrderEnum.Stop:
			case MovementOrder.MovementOrderEnum.Advance:
			case MovementOrder.MovementOrderEnum.FallBack:
				return Vec2.Zero;
			case MovementOrder.MovementOrderEnum.Follow:
				return this._targetAgent.AverageVelocity.AsVec2;
			}
			Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Orders\\MovementOrder.cs", "GetTargetVelocity", 842);
			return Vec2.Zero;
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x00037E20 File Offset: 0x00036020
		public WorldPosition CreateNewOrderWorldPositionMT(Formation f, WorldPosition.WorldPositionEnforcedCache worldPositionEnforcedCache)
		{
			object orderPositionLock = f.OrderPositionLock;
			WorldPosition worldPosition;
			lock (orderPositionLock)
			{
				if (!this.IsApplicable(f))
				{
					worldPosition = f.CreateNewOrderWorldPosition(worldPositionEnforcedCache);
				}
				else
				{
					MovementOrder.MovementOrderEnum orderEnum = this.OrderEnum;
					WorldPosition worldPosition2;
					if (orderEnum != MovementOrder.MovementOrderEnum.Follow)
					{
						if (orderEnum - MovementOrder.MovementOrderEnum.Advance > 1)
						{
							Func<Formation, WorldPosition> positionLambda = this._positionLambda;
							worldPosition2 = ((positionLambda != null) ? positionLambda(f) : this._position);
						}
						else
						{
							worldPosition2 = this.GetPositionAux(f, worldPositionEnforcedCache);
						}
					}
					else
					{
						this.GetPositionAuxFollow(f);
						worldPosition2 = this._lastPosition;
					}
					if (Mission.Current.Mode == MissionMode.Deployment)
					{
						if (!Mission.Current.IsOrderPositionAvailable(in worldPosition2, f.Team))
						{
							worldPosition2 = f.CreateNewOrderWorldPosition(worldPositionEnforcedCache);
						}
						else
						{
							if (Mission.Current.DeploymentPlan.SupportsNavmesh(f.Team))
							{
								Mission.Current.DeploymentPlan.ProjectPositionToDeploymentBoundaries(f.Team, ref worldPosition2);
							}
							if (!Mission.Current.IsOrderPositionAvailable(in worldPosition2, f.Team))
							{
								worldPosition2 = f.CreateNewOrderWorldPosition(worldPositionEnforcedCache);
							}
						}
					}
					bool flag2 = false;
					if (this._getPositionFirstSectionCache.AsVec2 != worldPosition2.AsVec2)
					{
						this._getPositionIsNavmeshlessCache = false;
						if (worldPosition2.IsValid)
						{
							if (worldPositionEnforcedCache != WorldPosition.WorldPositionEnforcedCache.NavMeshVec3)
							{
								if (worldPositionEnforcedCache == WorldPosition.WorldPositionEnforcedCache.GroundVec3)
								{
									worldPosition2.GetGroundVec3MT();
								}
							}
							else
							{
								worldPosition2.GetNavMeshVec3MT();
							}
							this._getPositionFirstSectionCache = worldPosition2;
							if (this.OrderEnum != MovementOrder.MovementOrderEnum.Follow && (worldPosition2.GetNavMeshMT() == UIntPtr.Zero || !Mission.Current.IsPositionInsideBoundaries(worldPosition2.AsVec2)))
							{
								worldPosition2 = MovementOrder.GetAlternatePositionForNavmeshlessOrOutOfBoundsPosition(f, worldPosition2);
								if (worldPositionEnforcedCache != WorldPosition.WorldPositionEnforcedCache.NavMeshVec3)
								{
									if (worldPositionEnforcedCache == WorldPosition.WorldPositionEnforcedCache.GroundVec3)
									{
										worldPosition2.GetGroundVec3MT();
									}
								}
								else
								{
									worldPosition2.GetNavMeshVec3MT();
								}
							}
							else
							{
								flag2 = true;
								this._getPositionIsNavmeshlessCache = true;
							}
							this._getPositionResultCache = worldPosition2;
						}
					}
					else
					{
						if (this._getPositionResultCache.IsValid)
						{
							if (worldPositionEnforcedCache != WorldPosition.WorldPositionEnforcedCache.NavMeshVec3)
							{
								if (worldPositionEnforcedCache == WorldPosition.WorldPositionEnforcedCache.GroundVec3)
								{
									this._getPositionResultCache.GetGroundVec3MT();
								}
							}
							else
							{
								this._getPositionResultCache.GetNavMeshVec3MT();
							}
						}
						worldPosition2 = this._getPositionResultCache;
					}
					if (this._getPositionIsNavmeshlessCache || flag2)
					{
						FormationAI ai = f.AI;
						if (((ai != null) ? ai.ActiveBehavior : null) != null)
						{
							f.AI.ActiveBehavior.NavmeshlessTargetPositionPenalty = 1f;
						}
					}
					worldPosition = worldPosition2;
				}
			}
			return worldPosition;
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x00038068 File Offset: 0x00036268
		public void ResetPositionCache()
		{
			this._getPositionFirstSectionCache = WorldPosition.Invalid;
			this._getPositionResultCache = WorldPosition.Invalid;
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x00038080 File Offset: 0x00036280
		public bool AreOrdersPracticallySame(MovementOrder m1, MovementOrder m2, bool isAIControlled)
		{
			if (m1.OrderEnum != m2.OrderEnum)
			{
				return false;
			}
			switch (m1.OrderEnum)
			{
			case MovementOrder.MovementOrderEnum.AttackEntity:
				return m1.TargetEntity == m2.TargetEntity;
			case MovementOrder.MovementOrderEnum.Charge:
				return true;
			case MovementOrder.MovementOrderEnum.ChargeToTarget:
				return m1.TargetFormation == m2.TargetFormation;
			case MovementOrder.MovementOrderEnum.Follow:
				return m1._targetAgent == m2._targetAgent;
			case MovementOrder.MovementOrderEnum.FollowEntity:
				return m1.TargetEntity == m2.TargetEntity;
			case MovementOrder.MovementOrderEnum.Move:
				return isAIControlled && m1._position.AsVec2.DistanceSquared(m2._position.AsVec2) < 1f;
			case MovementOrder.MovementOrderEnum.Retreat:
				return true;
			case MovementOrder.MovementOrderEnum.Stop:
				return true;
			case MovementOrder.MovementOrderEnum.Advance:
				return true;
			case MovementOrder.MovementOrderEnum.FallBack:
				return true;
			}
			return true;
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x00038158 File Offset: 0x00036358
		public void OnApply(Formation formation)
		{
			switch (this.OrderEnum)
			{
			case MovementOrder.MovementOrderEnum.AttackEntity:
				formation.FormAttackEntityDetachment(this.TargetEntity);
				break;
			case MovementOrder.MovementOrderEnum.ChargeToTarget:
				formation.SetTargetFormation(this.TargetFormation);
				break;
			case MovementOrder.MovementOrderEnum.Follow:
				formation.Arrangement.ReserveMiddleFrontUnitPosition(this._targetAgent);
				break;
			case MovementOrder.MovementOrderEnum.Move:
				formation.SetPositioning(new WorldPosition?(this.CreateNewOrderWorldPositionMT(formation, WorldPosition.WorldPositionEnforcedCache.None)), null, null);
				break;
			case MovementOrder.MovementOrderEnum.Retreat:
				MovementOrder.RetreatAux(formation);
				break;
			}
			MovementOrder.MovementOrderEnum orderEnum = this.OrderEnum;
			if ((orderEnum == MovementOrder.MovementOrderEnum.Charge || orderEnum == MovementOrder.MovementOrderEnum.ChargeToTarget) && this.GetPosition(formation).IsValid)
			{
				orderEnum = MovementOrder.MovementOrderEnum.Move;
			}
			formation.ApplyActionOnEachUnit(delegate(Agent agent)
			{
				agent.RefreshBehaviorValues(orderEnum, formation.ArrangementOrder.OrderEnum);
			}, null);
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x00038274 File Offset: 0x00036474
		public void OnCancel(Formation formation)
		{
			switch (this.OrderEnum)
			{
			case MovementOrder.MovementOrderEnum.AttackEntity:
				formation.DisbandAttackEntityDetachment();
				return;
			case MovementOrder.MovementOrderEnum.Charge:
				this.CancelChargeOrder(formation);
				return;
			case MovementOrder.MovementOrderEnum.ChargeToTarget:
				formation.SetTargetFormation(null);
				this.CancelChargeOrder(formation);
				return;
			case MovementOrder.MovementOrderEnum.Follow:
				formation.Arrangement.ReleaseMiddleFrontUnitPosition();
				return;
			case MovementOrder.MovementOrderEnum.FollowEntity:
			case (MovementOrder.MovementOrderEnum)6:
			case MovementOrder.MovementOrderEnum.Move:
			case MovementOrder.MovementOrderEnum.Stop:
			case MovementOrder.MovementOrderEnum.Advance:
				break;
			case MovementOrder.MovementOrderEnum.Retreat:
				formation.ApplyActionOnEachUnitViaBackupList(delegate(Agent agent)
				{
					if (agent.IsAIControlled)
					{
						agent.StopRetreatingMoraleComponent();
					}
				});
				return;
			case MovementOrder.MovementOrderEnum.FallBack:
				if (!Mission.Current.IsPositionInsideBoundaries(this.GetPosition(formation)))
				{
					formation.ApplyActionOnEachUnitViaBackupList(delegate(Agent agent)
					{
						if (agent.IsAIControlled)
						{
							agent.StopRetreatingMoraleComponent();
						}
					});
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x00038348 File Offset: 0x00036548
		public void OnUnitJoinOrLeave(Formation formation, Agent unit, bool isJoining)
		{
			if (!this.IsApplicable(formation))
			{
				return;
			}
			if (isJoining)
			{
				if (this.OrderEnum == MovementOrder.MovementOrderEnum.Retreat)
				{
					if (unit.IsAIControlled)
					{
						unit.Retreat(false);
						return;
					}
				}
				else
				{
					if ((this.OrderEnum == MovementOrder.MovementOrderEnum.Charge || this.OrderEnum == MovementOrder.MovementOrderEnum.ChargeToTarget) && this.GetPosition(formation).IsValid)
					{
						unit.RefreshBehaviorValues(MovementOrder.MovementOrderEnum.Move, formation.ArrangementOrder.OrderEnum);
						return;
					}
					unit.RefreshBehaviorValues(this.OrderEnum, formation.ArrangementOrder.OrderEnum);
					return;
				}
			}
			else if (this.OrderEnum == MovementOrder.MovementOrderEnum.Retreat && unit.IsAIControlled && unit.IsActive())
			{
				unit.StopRetreatingMoraleComponent();
			}
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x000383E8 File Offset: 0x000365E8
		public bool IsApplicable(Formation formation)
		{
			switch (this.OrderEnum)
			{
			case MovementOrder.MovementOrderEnum.AttackEntity:
			{
				UsableMachine firstScriptOfType = this.TargetEntity.GetFirstScriptOfType<UsableMachine>();
				if (firstScriptOfType != null)
				{
					return !firstScriptOfType.IsDestroyed;
				}
				DestructableComponent firstScriptOfType2 = this.TargetEntity.GetFirstScriptOfType<DestructableComponent>();
				return firstScriptOfType2 != null && !firstScriptOfType2.IsDestroyed;
			}
			case MovementOrder.MovementOrderEnum.Charge:
			{
				for (int i = 0; i < Mission.Current.Teams.Count; i++)
				{
					Team team = Mission.Current.Teams[i];
					if (team.IsEnemyOf(formation.Team) && team.ActiveAgents.Count > 0)
					{
						return true;
					}
				}
				return false;
			}
			case MovementOrder.MovementOrderEnum.ChargeToTarget:
				return this.TargetFormation.CountOfUnits > 0;
			case MovementOrder.MovementOrderEnum.Follow:
				return this._targetAgent.IsActive();
			case MovementOrder.MovementOrderEnum.FollowEntity:
			{
				UsableMachine firstScriptOfType3 = this.TargetEntity.GetFirstScriptOfType<UsableMachine>();
				return firstScriptOfType3 == null || !firstScriptOfType3.IsDestroyed;
			}
			default:
				return true;
			}
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x000384D9 File Offset: 0x000366D9
		private bool IsInstance()
		{
			return this.OrderEnum != MovementOrder.MovementOrderEnum.Invalid && this.OrderEnum != MovementOrder.MovementOrderEnum.Charge && this.OrderEnum != MovementOrder.MovementOrderEnum.Retreat && this.OrderEnum != MovementOrder.MovementOrderEnum.Stop && this.OrderEnum != MovementOrder.MovementOrderEnum.Advance && this.OrderEnum != MovementOrder.MovementOrderEnum.FallBack;
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x00038518 File Offset: 0x00036718
		public bool Tick(Formation formation)
		{
			object obj = !this.IsInstance() || this._tickTimer.Check(Mission.Current.CurrentTime);
			this.TickAux();
			object obj2 = obj;
			if (obj2 != null)
			{
				this.TickOccasionally(formation, this._tickTimer.PreviousDeltaTime);
			}
			return obj2 != null;
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x00038558 File Offset: 0x00036758
		private void TickOccasionally(Formation formation, float dt)
		{
			MovementOrder.MovementOrderEnum orderEnum = this.OrderEnum;
			if (orderEnum != MovementOrder.MovementOrderEnum.AttackEntity)
			{
				if (orderEnum - MovementOrder.MovementOrderEnum.Charge > 1)
				{
					if (orderEnum == MovementOrder.MovementOrderEnum.FallBack && !Mission.Current.IsPositionInsideBoundaries(this.GetPosition(formation)))
					{
						MovementOrder.RetreatAux(formation);
						return;
					}
				}
				else
				{
					Team team = formation.Team;
					TeamAISiegeComponent teamAISiegeComponent = ((team != null) ? team.TeamAI : null) as TeamAISiegeComponent;
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					bool flag4 = false;
					if (!Mission.Current.IsTeleportingAgents && teamAISiegeComponent != null)
					{
						flag4 = TeamAISiegeComponent.IsFormationInsideCastle(formation, false, 0.4f);
						bool flag5 = false;
						foreach (Team team2 in formation.Team.Mission.Teams)
						{
							if (team2.IsEnemyOf(formation.Team))
							{
								foreach (Formation formation2 in team2.FormationsIncludingEmpty)
								{
									if (formation2.CountOfUnits > 0 && flag4 == TeamAISiegeComponent.IsFormationInsideCastle(formation2, false, 0.4f))
									{
										flag5 = true;
										break;
									}
								}
								if (flag5)
								{
									break;
								}
							}
						}
						if (!flag5)
						{
							if (flag4 && !teamAISiegeComponent.CalculateIsAnyLaneOpenToGoOutside())
							{
								CastleGate gateToGetThrough = ((!teamAISiegeComponent.InnerGate.IsGateOpen) ? teamAISiegeComponent.InnerGate : teamAISiegeComponent.OuterGate);
								if (gateToGetThrough != null)
								{
									if (!gateToGetThrough.IsUsedByFormation(formation))
									{
										formation.StartUsingMachine(gateToGetThrough, true);
										SiegeLane siegeLane;
										if ((siegeLane = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == gateToGetThrough.DefenseSide)) == null)
										{
											siegeLane = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == FormationAI.BehaviorSide.Middle);
										}
										SiegeLane siegeLane2 = siegeLane;
										TacticalPosition tacticalPosition;
										if (siegeLane2 == null)
										{
											tacticalPosition = null;
										}
										else
										{
											ICastleKeyPosition castleKeyPosition = siegeLane2.DefensePoints.FirstOrDefault<ICastleKeyPosition>(delegate(ICastleKeyPosition dp)
											{
												UsableMachine usableMachine;
												return (usableMachine = dp.AttackerSiegeWeapon as UsableMachine) != null && !usableMachine.IsDisabled;
											});
											tacticalPosition = ((castleKeyPosition != null) ? castleKeyPosition.WaitPosition : null);
										}
										TacticalPosition tacticalPosition2 = tacticalPosition;
										if (tacticalPosition2 != null)
										{
											this._position = tacticalPosition2.Position;
										}
										else
										{
											WorldFrame? worldFrame;
											if (siegeLane2 == null)
											{
												worldFrame = null;
											}
											else
											{
												ICastleKeyPosition castleKeyPosition2 = siegeLane2.DefensePoints.FirstOrDefault<ICastleKeyPosition>(delegate(ICastleKeyPosition dp)
												{
													UsableMachine usableMachine2;
													return (usableMachine2 = dp.AttackerSiegeWeapon as UsableMachine) != null && !usableMachine2.IsDisabled;
												});
												worldFrame = ((castleKeyPosition2 != null) ? new WorldFrame?(castleKeyPosition2.DefenseWaitFrame) : null);
											}
											WorldFrame? worldFrame2 = worldFrame;
											WorldFrame worldFrame4;
											if (worldFrame2 == null)
											{
												WorldFrame? worldFrame3;
												if (siegeLane2 == null)
												{
													worldFrame3 = null;
												}
												else
												{
													ICastleKeyPosition castleKeyPosition3 = siegeLane2.DefensePoints.FirstOrDefault<ICastleKeyPosition>();
													worldFrame3 = ((castleKeyPosition3 != null) ? new WorldFrame?(castleKeyPosition3.DefenseWaitFrame) : null);
												}
												worldFrame4 = worldFrame3 ?? WorldFrame.Invalid;
											}
											else
											{
												worldFrame4 = worldFrame2.GetValueOrDefault();
											}
											WorldFrame worldFrame5 = worldFrame4;
											this._position = (worldFrame5.Origin.IsValid ? worldFrame5.Origin : formation.CachedMedianPosition);
										}
									}
									flag = true;
								}
							}
							else if (!teamAISiegeComponent.CalculateIsAnyLaneOpenToGetInside())
							{
								SiegeLadder siegeLadder = null;
								float num = float.MaxValue;
								foreach (SiegeLadder siegeLadder2 in teamAISiegeComponent.Ladders)
								{
									if (!siegeLadder2.IsDeactivated && !siegeLadder2.IsDisabled)
									{
										float num2 = siegeLadder2.WaitFrame.origin.DistanceSquared(formation.CachedMedianPosition.GetNavMeshVec3());
										if (num2 < num)
										{
											num = num2;
											siegeLadder = siegeLadder2;
										}
									}
								}
								if (siegeLadder != null)
								{
									if (!siegeLadder.IsUsedByFormation(formation))
									{
										formation.StartUsingMachine(siegeLadder, true);
										this._position = siegeLadder.WaitFrame.origin.ToWorldPosition();
									}
									else if (!this._position.IsValid)
									{
										this._position = siegeLadder.WaitFrame.origin.ToWorldPosition();
									}
									flag2 = true;
								}
								else
								{
									CastleGate castleGate = ((!teamAISiegeComponent.OuterGate.IsGateOpen) ? teamAISiegeComponent.OuterGate : teamAISiegeComponent.InnerGate);
									if (castleGate != null)
									{
										flag3 = true;
										if (formation.AttackEntityOrderSecondaryDetachment == null)
										{
											GameEntity gameEntity = GameEntity.CreateFromWeakEntity(castleGate.GameEntity);
											formation.FormAttackEntityDetachment(gameEntity);
											this.TargetEntity = gameEntity;
											this._position = this.ComputeAttackEntityWaitPosition(formation, castleGate.GameEntity);
										}
										else if (this.TargetEntity != castleGate.GameEntity)
										{
											GameEntity gameEntity2 = GameEntity.CreateFromWeakEntity(castleGate.GameEntity);
											formation.DisbandAttackEntityDetachment();
											formation.FormAttackEntityDetachment(gameEntity2);
											this.TargetEntity = gameEntity2;
											this._position = this.ComputeAttackEntityWaitPosition(formation, castleGate.GameEntity);
										}
										formation.AttackEntityOrderSecondaryDetachment.TickOccasionally(formation);
									}
								}
							}
						}
					}
					if (teamAISiegeComponent != null && flag4 && this._position.IsValid && !flag)
					{
						this._position = WorldPosition.Invalid;
						formation.SetPositioning(new WorldPosition?(this._position), null, null);
						formation.ApplyActionOnEachUnit(delegate(Agent agent)
						{
							agent.RefreshBehaviorValues(MovementOrder.MovementOrderEnum.Charge, formation.ArrangementOrder.OrderEnum);
						}, null);
					}
					if (teamAISiegeComponent != null && !flag4 && this._position.IsValid && !flag2 && !flag3)
					{
						this._position = WorldPosition.Invalid;
						formation.SetPositioning(new WorldPosition?(this._position), null, null);
						formation.ApplyActionOnEachUnit(delegate(Agent agent)
						{
							agent.RefreshBehaviorValues(MovementOrder.MovementOrderEnum.Charge, formation.ArrangementOrder.OrderEnum);
						}, null);
					}
					if (teamAISiegeComponent != null && formation.AttackEntityOrderSecondaryDetachment != null && !flag3)
					{
						formation.DisbandAttackEntityDetachment();
						this.TargetEntity = null;
						this._position = WorldPosition.Invalid;
						formation.SetPositioning(new WorldPosition?(this._position), null, null);
						formation.ApplyActionOnEachUnit(delegate(Agent agent)
						{
							agent.RefreshBehaviorValues(MovementOrder.MovementOrderEnum.Charge, formation.ArrangementOrder.OrderEnum);
						}, null);
					}
					if (this._position.IsValid)
					{
						formation.SetPositioning(new WorldPosition?(this._position), null, null);
						formation.ApplyActionOnEachUnit(delegate(Agent agent)
						{
							agent.RefreshBehaviorValues(MovementOrder.MovementOrderEnum.Move, formation.ArrangementOrder.OrderEnum);
						}, null);
					}
				}
				return;
			}
			formation.AttackEntityOrderSecondaryDetachment.TickOccasionally(formation);
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x00038C58 File Offset: 0x00036E58
		private void TickAux()
		{
			MovementOrder.MovementOrderEnum orderEnum = this.OrderEnum;
			if (orderEnum == MovementOrder.MovementOrderEnum.Follow)
			{
				float length = this._targetAgent.GetCurrentVelocity().Length;
				if (length < 0.01f)
				{
					this._followState = MovementOrder.FollowState.Stop;
					return;
				}
				if (length < this._targetAgent.Monster.WalkingSpeedLimit * 0.7f)
				{
					if (this._followState == MovementOrder.FollowState.Stop)
					{
						this._followState = MovementOrder.FollowState.Depart;
						this._departStartTime = Mission.Current.CurrentTime;
						return;
					}
					if (this._followState == MovementOrder.FollowState.Move)
					{
						this._followState = MovementOrder.FollowState.Arrive;
						return;
					}
				}
				else if (this._followState == MovementOrder.FollowState.Depart)
				{
					if (Mission.Current.CurrentTime - this._departStartTime > 1f)
					{
						this._followState = MovementOrder.FollowState.Move;
						return;
					}
				}
				else
				{
					this._followState = MovementOrder.FollowState.Move;
				}
			}
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x00038D14 File Offset: 0x00036F14
		public void OnArrangementChanged(Formation formation)
		{
			if (!this.IsApplicable(formation))
			{
				return;
			}
			MovementOrder.MovementOrderEnum orderEnum = this.OrderEnum;
			if (orderEnum == MovementOrder.MovementOrderEnum.Follow)
			{
				formation.Arrangement.ReserveMiddleFrontUnitPosition(this._targetAgent);
			}
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x00038D48 File Offset: 0x00036F48
		public void Advance(Formation formation, float distance)
		{
			WorldPosition currentPosition = this.CreateNewOrderWorldPositionMT(formation, WorldPosition.WorldPositionEnforcedCache.None);
			Vec2 direction = formation.Direction;
			currentPosition.SetVec2(currentPosition.AsVec2 + direction * distance);
			this._positionLambda = (Formation f) => currentPosition;
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x00038DA4 File Offset: 0x00036FA4
		public void FallBack(Formation formation, float distance)
		{
			this.Advance(formation, -distance);
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x00038DB0 File Offset: 0x00036FB0
		private ValueTuple<Agent, float> GetBestAgent(List<Agent> candidateAgents)
		{
			if (candidateAgents.IsEmpty<Agent>())
			{
				return new ValueTuple<Agent, float>(null, float.MaxValue);
			}
			GameEntity targetEntity = this.TargetEntity;
			Vec3 targetEntityPos = targetEntity.GlobalPosition;
			Agent agent = candidateAgents.MinBy<Agent, float>((Agent ca) => ca.Position.DistanceSquared(targetEntityPos));
			return new ValueTuple<Agent, float>(agent, agent.Position.DistanceSquared(targetEntityPos));
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x00038E18 File Offset: 0x00037018
		private ValueTuple<Agent, float> GetWorstAgent(List<Agent> currentAgents, int requiredAgentCount)
		{
			if (requiredAgentCount <= 0 || currentAgents.Count < requiredAgentCount)
			{
				return new ValueTuple<Agent, float>(null, float.MaxValue);
			}
			GameEntity targetEntity = this.TargetEntity;
			Vec3 targetEntityPos = targetEntity.GlobalPosition;
			Agent agent = currentAgents.MaxBy<Agent, float>((Agent ca) => ca.Position.DistanceSquared(targetEntityPos));
			return new ValueTuple<Agent, float>(agent, agent.Position.DistanceSquared(targetEntityPos));
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x00038E84 File Offset: 0x00037084
		public MovementOrder GetSubstituteOrder(Formation formation)
		{
			MovementOrder.MovementOrderEnum orderEnum = this.OrderEnum;
			if (orderEnum == MovementOrder.MovementOrderEnum.Charge)
			{
				return MovementOrder.MovementOrderStop;
			}
			return MovementOrder.MovementOrderCharge;
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x00038EA8 File Offset: 0x000370A8
		private Vec2 GetDirectionAux(Formation f)
		{
			MovementOrder.MovementOrderEnum orderEnum = this.OrderEnum;
			if (orderEnum - MovementOrder.MovementOrderEnum.Advance > 1)
			{
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Orders\\MovementOrder.cs", "GetDirectionAux", 1742);
				return Vec2.One;
			}
			Formation targetFormation = f.TargetFormation;
			FormationQuerySystem formationQuerySystem = ((targetFormation != null) ? targetFormation.QuerySystem : null) ?? f.CachedClosestEnemyFormation;
			if (formationQuerySystem != null)
			{
				return (formationQuerySystem.Formation.CachedMedianPosition.AsVec2 - f.CachedAveragePosition).Normalized();
			}
			return Vec2.One;
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x00038F30 File Offset: 0x00037130
		private WorldPosition GetPositionAux(Formation f, WorldPosition.WorldPositionEnforcedCache worldPositionEnforcedCache)
		{
			MovementOrder.MovementOrderEnum orderEnum = this.OrderEnum;
			if (orderEnum != MovementOrder.MovementOrderEnum.Advance)
			{
				if (orderEnum != MovementOrder.MovementOrderEnum.FallBack)
				{
					Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Orders\\MovementOrder.cs", "GetPositionAux", 1845);
					return WorldPosition.Invalid;
				}
				if (Mission.Current.Mode == MissionMode.Deployment)
				{
					return f.CreateNewOrderWorldPosition(worldPositionEnforcedCache);
				}
				Vec2 directionAux = this.GetDirectionAux(f);
				WorldPosition cachedMedianPosition = f.CachedMedianPosition;
				cachedMedianPosition.SetVec2(f.CachedAveragePosition - directionAux * 7f);
				return cachedMedianPosition;
			}
			else
			{
				if (Mission.Current.Mode == MissionMode.Deployment)
				{
					return f.CreateNewOrderWorldPosition(worldPositionEnforcedCache);
				}
				Vec2 vec = f.Direction;
				FormationQuerySystem querySystem = f.QuerySystem;
				Formation targetFormation = f.TargetFormation;
				FormationQuerySystem formationQuerySystem = ((targetFormation != null) ? targetFormation.QuerySystem : null) ?? f.CachedClosestEnemyFormation;
				WorldPosition worldPosition;
				if (formationQuerySystem == null)
				{
					Agent closestEnemyAgent = querySystem.ClosestEnemyAgent;
					if (closestEnemyAgent == null)
					{
						return f.CreateNewOrderWorldPosition(worldPositionEnforcedCache);
					}
					worldPosition = closestEnemyAgent.GetWorldPosition();
				}
				else
				{
					worldPosition = formationQuerySystem.Formation.CachedMedianPosition;
				}
				if (querySystem.IsRangedFormation || querySystem.IsRangedCavalryFormation)
				{
					vec = this.GetDirectionAux(f);
					worldPosition.SetVec2(worldPosition.AsVec2 - vec * querySystem.MissileRangeAdjusted);
				}
				else if (formationQuerySystem != null)
				{
					vec = (formationQuerySystem.Formation.CachedAveragePosition - f.CachedAveragePosition).Normalized();
					float num = 2f;
					if (formationQuerySystem.FormationPower < f.QuerySystem.FormationPower * 0.2f)
					{
						num = 0.1f;
					}
					worldPosition.SetVec2(worldPosition.AsVec2 - vec * num);
				}
				if (!this._engageTargetPositionCache.IsValid)
				{
					this._engageTargetPositionCache = worldPosition;
				}
				float num2 = f.QuerySystem.MovementSpeedMaximum * f.QuerySystem.MovementSpeedMaximum * 9f * f.Depth;
				if ((this._engageTargetPositionCache.AsVec2 + vec * this._engageTargetPositionOffset).DistanceSquared(worldPosition.AsVec2) > f.CurrentPosition.DistanceSquared(this._engageTargetPositionCache.AsVec2) * 0.1f || worldPosition.AsVec2.DistanceSquared(f.CurrentPosition) <= num2)
				{
					this._engageTargetPositionCache = worldPosition;
					this._engageTargetPositionOffset = 0f;
				}
				worldPosition = this._engageTargetPositionCache;
				LineFormation lineFormation;
				if (worldPosition.AsVec2.DistanceSquared(f.CurrentPosition) > num2 && vec.DotProduct(worldPosition.AsVec2 - f.CurrentPosition) > 0f && (lineFormation = f.Arrangement as LineFormation) != null && (double)lineFormation.GetUnavailableUnitPositions().Count<Vec2>() > (double)lineFormation.UnitCount * 0.03)
				{
					worldPosition.SetVec2(worldPosition.AsVec2 - vec * 10f);
					this._engageTargetPositionOffset += 10f;
				}
				this._engageTargetPositionCache = worldPosition;
				return worldPosition;
			}
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x00039230 File Offset: 0x00037430
		private void CancelChargeOrder(Formation formation)
		{
			Team team = formation.Team;
			TeamAISiegeComponent teamAISiegeComponent;
			if ((teamAISiegeComponent = ((team != null) ? team.TeamAI : null) as TeamAISiegeComponent) != null)
			{
				if (teamAISiegeComponent.InnerGate != null && teamAISiegeComponent.InnerGate.IsUsedByFormation(formation))
				{
					formation.StopUsingMachine(teamAISiegeComponent.InnerGate, true);
				}
				if (teamAISiegeComponent.OuterGate != null && teamAISiegeComponent.OuterGate.IsUsedByFormation(formation))
				{
					formation.StopUsingMachine(teamAISiegeComponent.OuterGate, true);
				}
				foreach (SiegeLadder siegeLadder in teamAISiegeComponent.Ladders)
				{
					if (siegeLadder.IsUsedByFormation(formation))
					{
						formation.StopUsingMachine(siegeLadder, true);
					}
				}
				if (formation.AttackEntityOrderSecondaryDetachment != null)
				{
					formation.DisbandAttackEntityDetachment();
					this.TargetEntity = null;
				}
				this._position = WorldPosition.Invalid;
			}
		}

		// Token: 0x04000457 RID: 1111
		public static readonly MovementOrder MovementOrderNull = new MovementOrder(MovementOrder.MovementOrderEnum.Invalid);

		// Token: 0x04000458 RID: 1112
		public static readonly MovementOrder MovementOrderCharge = new MovementOrder(MovementOrder.MovementOrderEnum.Charge);

		// Token: 0x04000459 RID: 1113
		public static readonly MovementOrder MovementOrderRetreat = new MovementOrder(MovementOrder.MovementOrderEnum.Retreat);

		// Token: 0x0400045A RID: 1114
		public static readonly MovementOrder MovementOrderStop = new MovementOrder(MovementOrder.MovementOrderEnum.Stop);

		// Token: 0x0400045B RID: 1115
		public static readonly MovementOrder MovementOrderAdvance = new MovementOrder(MovementOrder.MovementOrderEnum.Advance);

		// Token: 0x0400045C RID: 1116
		public static readonly MovementOrder MovementOrderFallBack = new MovementOrder(MovementOrder.MovementOrderEnum.FallBack);

		// Token: 0x0400045D RID: 1117
		private MovementOrder.FollowState _followState;

		// Token: 0x0400045E RID: 1118
		private float _departStartTime;

		// Token: 0x0400045F RID: 1119
		public readonly MovementOrder.MovementOrderEnum OrderEnum;

		// Token: 0x04000460 RID: 1120
		private Func<Formation, WorldPosition> _positionLambda;

		// Token: 0x04000461 RID: 1121
		private WorldPosition _position;

		// Token: 0x04000462 RID: 1122
		private WorldPosition _getPositionResultCache;

		// Token: 0x04000463 RID: 1123
		private WorldPosition _engageTargetPositionCache;

		// Token: 0x04000464 RID: 1124
		private float _engageTargetPositionOffset;

		// Token: 0x04000465 RID: 1125
		private bool _getPositionIsNavmeshlessCache;

		// Token: 0x04000466 RID: 1126
		private WorldPosition _getPositionFirstSectionCache;

		// Token: 0x04000468 RID: 1128
		public GameEntity TargetEntity;

		// Token: 0x0400046A RID: 1130
		private readonly Timer _tickTimer;

		// Token: 0x0400046B RID: 1131
		private WorldPosition _lastPosition;

		// Token: 0x0400046C RID: 1132
		public readonly bool _isFacingDirection;

		// Token: 0x02000477 RID: 1143
		public enum MovementOrderEnum
		{
			// Token: 0x04001A7B RID: 6779
			Invalid,
			// Token: 0x04001A7C RID: 6780
			AttackEntity,
			// Token: 0x04001A7D RID: 6781
			Charge,
			// Token: 0x04001A7E RID: 6782
			ChargeToTarget,
			// Token: 0x04001A7F RID: 6783
			Follow,
			// Token: 0x04001A80 RID: 6784
			FollowEntity,
			// Token: 0x04001A81 RID: 6785
			Move = 7,
			// Token: 0x04001A82 RID: 6786
			Retreat,
			// Token: 0x04001A83 RID: 6787
			Stop,
			// Token: 0x04001A84 RID: 6788
			Advance,
			// Token: 0x04001A85 RID: 6789
			FallBack
		}

		// Token: 0x02000478 RID: 1144
		public enum MovementStateEnum
		{
			// Token: 0x04001A87 RID: 6791
			Charge,
			// Token: 0x04001A88 RID: 6792
			Hold,
			// Token: 0x04001A89 RID: 6793
			Retreat,
			// Token: 0x04001A8A RID: 6794
			StandGround
		}

		// Token: 0x02000479 RID: 1145
		public enum Side
		{
			// Token: 0x04001A8C RID: 6796
			Front,
			// Token: 0x04001A8D RID: 6797
			Rear,
			// Token: 0x04001A8E RID: 6798
			Left,
			// Token: 0x04001A8F RID: 6799
			Right
		}

		// Token: 0x0200047A RID: 1146
		private enum FollowState
		{
			// Token: 0x04001A91 RID: 6801
			Stop,
			// Token: 0x04001A92 RID: 6802
			Depart,
			// Token: 0x04001A93 RID: 6803
			Move,
			// Token: 0x04001A94 RID: 6804
			Arrive
		}
	}
}
