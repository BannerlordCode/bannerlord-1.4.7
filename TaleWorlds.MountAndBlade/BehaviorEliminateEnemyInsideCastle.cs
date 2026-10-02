using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000116 RID: 278
	public class BehaviorEliminateEnemyInsideCastle : BehaviorComponent
	{
		// Token: 0x06000E0F RID: 3599 RVA: 0x0001EC16 File Offset: 0x0001CE16
		public BehaviorEliminateEnemyInsideCastle(Formation formation)
			: base(formation)
		{
			this._behaviorState = BehaviorEliminateEnemyInsideCastle.BehaviorState.UnSet;
			this._behaviorSide = formation.AI.Side;
			this.ResetOrderPositions();
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x0001EC3D File Offset: 0x0001CE3D
		protected override void CalculateCurrentOrder()
		{
			base.CalculateCurrentOrder();
			base.CurrentOrder = ((this._behaviorState == BehaviorEliminateEnemyInsideCastle.BehaviorState.Attacking) ? this._attackOrder : this._gatherOrder);
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0001EC64 File Offset: 0x0001CE64
		private void DetermineMostImportantInvadingEnemyFormation()
		{
			float num = float.MinValue;
			this._targetEnemyFormation = null;
			foreach (Team team in base.Formation.Team.Mission.Teams)
			{
				if (team.IsEnemyOf(base.Formation.Team))
				{
					for (int i = 0; i < Math.Min(team.FormationsIncludingSpecialAndEmpty.Count, 8); i++)
					{
						Formation formation = team.FormationsIncludingSpecialAndEmpty[i];
						if (formation.CountOfUnits > 0 && TeamAISiegeComponent.IsFormationInsideCastle(formation, true, 0.4f))
						{
							float formationPower = formation.QuerySystem.FormationPower;
							if (formationPower > num)
							{
								num = formationPower;
								this._targetEnemyFormation = formation;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0001ED44 File Offset: 0x0001CF44
		private void ConfirmGatheringSide()
		{
			SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == this._behaviorSide);
			if (siegeLane == null || siegeLane.LaneState >= SiegeLane.LaneStateEnum.Conceited)
			{
				this.ResetOrderPositions();
			}
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0001ED7C File Offset: 0x0001CF7C
		private FormationAI.BehaviorSide DetermineGatheringSide()
		{
			this.DetermineMostImportantInvadingEnemyFormation();
			if (this._targetEnemyFormation == null)
			{
				if (this._behaviorState == BehaviorEliminateEnemyInsideCastle.BehaviorState.Attacking)
				{
					this._behaviorState = BehaviorEliminateEnemyInsideCastle.BehaviorState.UnSet;
				}
				return this._behaviorSide;
			}
			int connectedSides = TeamAISiegeComponent.QuerySystem.DeterminePositionAssociatedSide(this._targetEnemyFormation.CachedMedianPosition.GetNavMeshVec3());
			IEnumerable<SiegeLane> enumerable = TeamAISiegeComponent.SiegeLanes.Where<SiegeLane>((SiegeLane sl) => sl.LaneState != SiegeLane.LaneStateEnum.Conceited && !SiegeQuerySystem.AreSidesRelated(sl.LaneSide, connectedSides));
			FormationAI.BehaviorSide behaviorSide = this._behaviorSide;
			if (enumerable.Any<SiegeLane>())
			{
				if (enumerable.Count<SiegeLane>() > 1)
				{
					int leastDangerousLaneState = enumerable.Min<SiegeLane>((SiegeLane pgl) => (int)pgl.LaneState);
					IEnumerable<SiegeLane> enumerable2 = enumerable.Where<SiegeLane>((SiegeLane pgl) => pgl.LaneState == (SiegeLane.LaneStateEnum)leastDangerousLaneState);
					behaviorSide = ((enumerable2.Count<SiegeLane>() > 1) ? enumerable2.MinBy<SiegeLane, int>((SiegeLane ldl) => SiegeQuerySystem.SideDistance(1 << connectedSides, 1 << (int)ldl.LaneSide)).LaneSide : enumerable2.First<SiegeLane>().LaneSide);
				}
				else
				{
					behaviorSide = enumerable.First<SiegeLane>().LaneSide;
				}
			}
			return behaviorSide;
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0001EE94 File Offset: 0x0001D094
		private void ResetOrderPositions()
		{
			this._behaviorSide = this.DetermineGatheringSide();
			SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == this._behaviorSide);
			WorldFrame? worldFrame;
			if (siegeLane == null)
			{
				worldFrame = null;
			}
			else
			{
				List<ICastleKeyPosition> defensePoints = siegeLane.DefensePoints;
				if (defensePoints == null)
				{
					worldFrame = null;
				}
				else
				{
					ICastleKeyPosition castleKeyPosition = defensePoints.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp.AttackerSiegeWeapon is UsableMachine && !(dp.AttackerSiegeWeapon as UsableMachine).IsDisabled);
					worldFrame = ((castleKeyPosition != null) ? new WorldFrame?(castleKeyPosition.DefenseWaitFrame) : null);
				}
			}
			WorldFrame worldFrame2 = worldFrame ?? WorldFrame.Invalid;
			object obj;
			if (siegeLane == null)
			{
				obj = null;
			}
			else
			{
				List<ICastleKeyPosition> defensePoints2 = siegeLane.DefensePoints;
				if (defensePoints2 == null)
				{
					obj = null;
				}
				else
				{
					ICastleKeyPosition castleKeyPosition2 = defensePoints2.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp.AttackerSiegeWeapon is UsableMachine && !(dp.AttackerSiegeWeapon as UsableMachine).IsDisabled);
					obj = ((castleKeyPosition2 != null) ? castleKeyPosition2.WaitPosition : null);
				}
			}
			object obj2;
			if ((obj2 = obj) == null)
			{
				if (siegeLane == null)
				{
					obj2 = null;
				}
				else
				{
					List<ICastleKeyPosition> defensePoints3 = siegeLane.DefensePoints;
					obj2 = ((defensePoints3 != null) ? defensePoints3.FirstOrDefault<ICastleKeyPosition>().WaitPosition : null);
				}
			}
			this._gatheringTacticalPos = obj2;
			if (this._gatheringTacticalPos != null)
			{
				this._gatherOrder = MovementOrder.MovementOrderMove(this._gatheringTacticalPos.Position);
				this._gatheringFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			}
			else if (worldFrame2.Origin.IsValid)
			{
				worldFrame2.Rotation.f.Normalize();
				this._gatherOrder = MovementOrder.MovementOrderMove(worldFrame2.Origin);
				this._gatheringFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			}
			else
			{
				this._gatherOrder = MovementOrder.MovementOrderMove(base.Formation.CachedMedianPosition);
				this._gatheringFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			}
			this._attackOrder = MovementOrder.MovementOrderChargeToTarget(this._targetEnemyFormation);
			this._attackFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			base.CurrentOrder = ((this._behaviorState == BehaviorEliminateEnemyInsideCastle.BehaviorState.Attacking) ? this._attackOrder : this._gatherOrder);
			this.CurrentFacingOrder = ((this._behaviorState == BehaviorEliminateEnemyInsideCastle.BehaviorState.Attacking) ? this._attackFacingOrder : this._gatheringFacingOrder);
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0001F087 File Offset: 0x0001D287
		public override void OnValidBehaviorSideChanged()
		{
			base.OnValidBehaviorSideChanged();
			this.ResetOrderPositions();
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0001F098 File Offset: 0x0001D298
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			if (this._behaviorState != BehaviorEliminateEnemyInsideCastle.BehaviorState.Attacking)
			{
				this.ConfirmGatheringSide();
			}
			bool flag;
			if (this._behaviorState == BehaviorEliminateEnemyInsideCastle.BehaviorState.Attacking)
			{
				flag = this._targetEnemyFormation != null;
			}
			else
			{
				flag = this._targetEnemyFormation != null && (base.Formation.CachedMedianPosition.GetNavMeshVec3().DistanceSquared(this._gatherOrder.CreateNewOrderWorldPositionMT(base.Formation, WorldPosition.WorldPositionEnforcedCache.NavMeshVec3).GetNavMeshVec3()) < 100f || base.Formation.CachedFormationIntegrityData.DeviationOfPositionsExcludeFarAgents / ((base.Formation.QuerySystem.IdealAverageDisplacement != 0f) ? base.Formation.QuerySystem.IdealAverageDisplacement : 1f) <= 3f);
			}
			BehaviorEliminateEnemyInsideCastle.BehaviorState behaviorState = (flag ? BehaviorEliminateEnemyInsideCastle.BehaviorState.Attacking : BehaviorEliminateEnemyInsideCastle.BehaviorState.Gathering);
			if (behaviorState != this._behaviorState)
			{
				this._behaviorState = behaviorState;
				base.CurrentOrder = ((this._behaviorState == BehaviorEliminateEnemyInsideCastle.BehaviorState.Attacking) ? this._attackOrder : this._gatherOrder);
				this.CurrentFacingOrder = ((this._behaviorState == BehaviorEliminateEnemyInsideCastle.BehaviorState.Attacking) ? this._attackFacingOrder : this._gatheringFacingOrder);
			}
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (this._behaviorState == BehaviorEliminateEnemyInsideCastle.BehaviorState.Gathering && this._gatheringTacticalPos != null)
			{
				base.Formation.SetFormOrder(FormOrder.FormOrderCustom(this._gatheringTacticalPos.Width), true);
			}
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0001F20C File Offset: 0x0001D40C
		protected override void OnBehaviorActivatedAux()
		{
			this._behaviorState = BehaviorEliminateEnemyInsideCastle.BehaviorState.UnSet;
			this._behaviorSide = base.Formation.AI.Side;
			this.ResetOrderPositions();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000E18 RID: 3608 RVA: 0x0001F28F File Offset: 0x0001D48F
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0001F296 File Offset: 0x0001D496
		protected override float GetAiWeight()
		{
			return 1f;
		}

		// Token: 0x04000359 RID: 857
		private BehaviorEliminateEnemyInsideCastle.BehaviorState _behaviorState;

		// Token: 0x0400035A RID: 858
		private MovementOrder _gatherOrder;

		// Token: 0x0400035B RID: 859
		private MovementOrder _attackOrder;

		// Token: 0x0400035C RID: 860
		private FacingOrder _gatheringFacingOrder;

		// Token: 0x0400035D RID: 861
		private FacingOrder _attackFacingOrder;

		// Token: 0x0400035E RID: 862
		private TacticalPosition _gatheringTacticalPos;

		// Token: 0x0400035F RID: 863
		private Formation _targetEnemyFormation;

		// Token: 0x02000437 RID: 1079
		private enum BehaviorState
		{
			// Token: 0x04001959 RID: 6489
			UnSet,
			// Token: 0x0400195A RID: 6490
			Gathering,
			// Token: 0x0400195B RID: 6491
			Attacking
		}
	}
}
