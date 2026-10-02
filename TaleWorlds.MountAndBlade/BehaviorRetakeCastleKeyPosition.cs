using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000122 RID: 290
	public class BehaviorRetakeCastleKeyPosition : BehaviorComponent
	{
		// Token: 0x06000E58 RID: 3672 RVA: 0x00021F59 File Offset: 0x00020159
		public BehaviorRetakeCastleKeyPosition(Formation formation)
			: base(formation)
		{
			this._behaviorState = BehaviorRetakeCastleKeyPosition.BehaviorState.UnSet;
			this._behaviorSide = formation.AI.Side;
			this.ResetOrderPositions();
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00021F80 File Offset: 0x00020180
		protected override void CalculateCurrentOrder()
		{
			base.CalculateCurrentOrder();
			base.CurrentOrder = ((this._behaviorState == BehaviorRetakeCastleKeyPosition.BehaviorState.Attacking) ? this._attackOrder : this._gatherOrder);
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x00021FA8 File Offset: 0x000201A8
		private FormationAI.BehaviorSide DetermineGatheringSide()
		{
			IEnumerable<SiegeLane> enumerable = TeamAISiegeComponent.SiegeLanes.Where<SiegeLane>((SiegeLane sl) => sl.LaneSide != this._behaviorSide && sl.LaneState != SiegeLane.LaneStateEnum.Conceited && sl.DefenderOrigin.IsValid);
			if (enumerable.Any<SiegeLane>())
			{
				int nearestSafeSideDistance = enumerable.Min<SiegeLane>((SiegeLane pgl) => SiegeQuerySystem.SideDistance(1 << (int)this._behaviorSide, 1 << (int)pgl.LaneSide));
				return enumerable.Where<SiegeLane>((SiegeLane pgl) => SiegeQuerySystem.SideDistance(1 << (int)this._behaviorSide, 1 << (int)pgl.LaneSide) == nearestSafeSideDistance).MinBy<SiegeLane, float>((SiegeLane pgl) => pgl.DefenderOrigin.GetGroundVec3().DistanceSquared(base.Formation.CachedMedianPosition.GetGroundVec3())).LaneSide;
			}
			return this._behaviorSide;
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00022028 File Offset: 0x00020228
		private void ConfirmGatheringSide()
		{
			SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == this._gatheringSide);
			if (siegeLane == null || siegeLane.LaneState >= SiegeLane.LaneStateEnum.Conceited)
			{
				this.ResetOrderPositions();
			}
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x00022060 File Offset: 0x00020260
		private void ResetOrderPositions()
		{
			this._behaviorState = BehaviorRetakeCastleKeyPosition.BehaviorState.UnSet;
			this._gatheringSide = this.DetermineGatheringSide();
			SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == this._gatheringSide);
			ICastleKeyPosition castleKeyPosition = siegeLane.DefensePoints.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp.AttackerSiegeWeapon is UsableMachine && !(dp.AttackerSiegeWeapon as UsableMachine).IsDisabled);
			WorldFrame worldFrame = ((castleKeyPosition != null) ? castleKeyPosition.DefenseWaitFrame : siegeLane.DefensePoints.FirstOrDefault<ICastleKeyPosition>().DefenseWaitFrame);
			ICastleKeyPosition castleKeyPosition2 = siegeLane.DefensePoints.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp.AttackerSiegeWeapon is UsableMachine && !(dp.AttackerSiegeWeapon as UsableMachine).IsDisabled);
			this._gatheringTacticalPos = ((castleKeyPosition2 != null) ? castleKeyPosition2.WaitPosition : null);
			if (this._gatheringTacticalPos != null)
			{
				this._gatherOrder = MovementOrder.MovementOrderMove(this._gatheringTacticalPos.Position);
				this._gatheringFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			}
			else if (worldFrame.Origin.IsValid)
			{
				worldFrame.Rotation.f.Normalize();
				this._gatherOrder = MovementOrder.MovementOrderMove(worldFrame.Origin);
				this._gatheringFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			}
			else
			{
				this._gatherOrder = MovementOrder.MovementOrderMove(base.Formation.CachedMedianPosition);
				this._gatheringFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			}
			SiegeLane siegeLane2 = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == this._behaviorSide);
			ICastleKeyPosition castleKeyPosition3 = siegeLane2.DefensePoints.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp.AttackerSiegeWeapon is UsableMachine && !(dp.AttackerSiegeWeapon as UsableMachine).IsDisabled);
			this._attackOrder = MovementOrder.MovementOrderMove((castleKeyPosition3 != null) ? castleKeyPosition3.MiddleFrame.Origin : siegeLane2.DefensePoints.FirstOrDefault<ICastleKeyPosition>().MiddleFrame.Origin);
			this._attackFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
			base.CurrentOrder = ((this._behaviorState == BehaviorRetakeCastleKeyPosition.BehaviorState.Attacking) ? this._attackOrder : this._gatherOrder);
			this.CurrentFacingOrder = ((this._behaviorState == BehaviorRetakeCastleKeyPosition.BehaviorState.Attacking) ? this._attackFacingOrder : this._gatheringFacingOrder);
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x00022259 File Offset: 0x00020459
		public override void OnValidBehaviorSideChanged()
		{
			base.OnValidBehaviorSideChanged();
			this.ResetOrderPositions();
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x00022268 File Offset: 0x00020468
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			if (this._behaviorState != BehaviorRetakeCastleKeyPosition.BehaviorState.Attacking)
			{
				this.ConfirmGatheringSide();
			}
			bool flag = true;
			if (this._behaviorState != BehaviorRetakeCastleKeyPosition.BehaviorState.Attacking)
			{
				flag = base.Formation.CachedMedianPosition.GetNavMeshVec3().DistanceSquared(this._gatherOrder.CreateNewOrderWorldPositionMT(base.Formation, WorldPosition.WorldPositionEnforcedCache.NavMeshVec3).GetNavMeshVec3()) < 100f || base.Formation.CachedFormationIntegrityData.DeviationOfPositionsExcludeFarAgents / ((base.Formation.QuerySystem.IdealAverageDisplacement != 0f) ? base.Formation.QuerySystem.IdealAverageDisplacement : 1f) <= 3f;
			}
			BehaviorRetakeCastleKeyPosition.BehaviorState behaviorState = (flag ? BehaviorRetakeCastleKeyPosition.BehaviorState.Attacking : BehaviorRetakeCastleKeyPosition.BehaviorState.Gathering);
			if (behaviorState != this._behaviorState)
			{
				this._behaviorState = behaviorState;
				base.CurrentOrder = ((this._behaviorState == BehaviorRetakeCastleKeyPosition.BehaviorState.Attacking) ? this._attackOrder : this._gatherOrder);
				this.CurrentFacingOrder = ((this._behaviorState == BehaviorRetakeCastleKeyPosition.BehaviorState.Attacking) ? this._attackFacingOrder : this._gatheringFacingOrder);
			}
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (this._behaviorState == BehaviorRetakeCastleKeyPosition.BehaviorState.Gathering && this._gatheringTacticalPos != null)
			{
				base.Formation.SetFormOrder(FormOrder.FormOrderCustom(this._gatheringTacticalPos.Width), true);
			}
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x000223C4 File Offset: 0x000205C4
		protected override void OnBehaviorActivatedAux()
		{
			this._behaviorState = BehaviorRetakeCastleKeyPosition.BehaviorState.UnSet;
			this._behaviorSide = base.Formation.AI.Side;
			this.ResetOrderPositions();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000E60 RID: 3680 RVA: 0x00022447 File Offset: 0x00020647
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0002244E File Offset: 0x0002064E
		protected override float GetAiWeight()
		{
			return 1f;
		}

		// Token: 0x0400036F RID: 879
		private BehaviorRetakeCastleKeyPosition.BehaviorState _behaviorState;

		// Token: 0x04000370 RID: 880
		private MovementOrder _gatherOrder;

		// Token: 0x04000371 RID: 881
		private MovementOrder _attackOrder;

		// Token: 0x04000372 RID: 882
		private FacingOrder _gatheringFacingOrder;

		// Token: 0x04000373 RID: 883
		private FacingOrder _attackFacingOrder;

		// Token: 0x04000374 RID: 884
		private TacticalPosition _gatheringTacticalPos;

		// Token: 0x04000375 RID: 885
		private FormationAI.BehaviorSide _gatheringSide;

		// Token: 0x02000440 RID: 1088
		private enum BehaviorState
		{
			// Token: 0x04001975 RID: 6517
			UnSet,
			// Token: 0x04001976 RID: 6518
			Gathering,
			// Token: 0x04001977 RID: 6519
			Attacking
		}
	}
}
