using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200010B RID: 267
	public class BehaviorAssaultWalls : BehaviorComponent
	{
		// Token: 0x06000D98 RID: 3480 RVA: 0x0001A6E8 File Offset: 0x000188E8
		private void ResetOrderPositions()
		{
			this._primarySiegeWeapons = this._teamAISiegeComponent.PrimarySiegeWeapons.ToList<IPrimarySiegeWeapon>();
			this._primarySiegeWeapons.RemoveAll(delegate(IPrimarySiegeWeapon uM)
			{
				SiegeWeapon siegeWeapon;
				IPrimarySiegeWeapon primarySiegeWeapon2;
				return uM.WeaponSide != this._behaviorSide || (siegeWeapon = uM as SiegeWeapon) == null || (siegeWeapon.IsDeactivated && !siegeWeapon.IsDestroyed && ((primarySiegeWeapon2 = siegeWeapon as IPrimarySiegeWeapon) == null || !primarySiegeWeapon2.HasCompletedAction()));
			});
			IEnumerable<ICastleKeyPosition> enumerable = TeamAISiegeComponent.SiegeLanes.Where<SiegeLane>((SiegeLane sl) => sl.LaneSide == this._behaviorSide).SelectMany<SiegeLane, ICastleKeyPosition>((SiegeLane sila) => sila.DefensePoints);
			this._innerGate = this._teamAISiegeComponent.InnerGate;
			this._isGateLane = this._teamAISiegeComponent.OuterGate.DefenseSide == this._behaviorSide;
			if (this._isGateLane)
			{
				this._wallSegment = null;
			}
			else
			{
				WallSegment wallSegment = enumerable.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp is WallSegment && (dp as WallSegment).IsBreachedWall) as WallSegment;
				if (wallSegment != null)
				{
					this._wallSegment = wallSegment;
				}
				else
				{
					IPrimarySiegeWeapon primarySiegeWeapon = this._primarySiegeWeapons.MaxBy<IPrimarySiegeWeapon, float>((IPrimarySiegeWeapon psw) => psw.SiegeWeaponPriority);
					this._wallSegment = primarySiegeWeapon.TargetCastlePosition as WallSegment;
				}
			}
			this._stopOrder = MovementOrder.MovementOrderStop;
			this._chargeOrder = MovementOrder.MovementOrderCharge;
			bool flag = this._teamAISiegeComponent.OuterGate != null && this._behaviorSide == this._teamAISiegeComponent.OuterGate.DefenseSide;
			this._attackEntityOrderOuterGate = ((flag && !this._teamAISiegeComponent.OuterGate.IsDeactivated && this._teamAISiegeComponent.OuterGate.State != CastleGate.GateState.Open) ? MovementOrder.MovementOrderAttackEntity(GameEntity.CreateFromWeakEntity(this._teamAISiegeComponent.OuterGate.GameEntity), false) : MovementOrder.MovementOrderStop);
			this._attackEntityOrderInnerGate = ((flag && this._teamAISiegeComponent.InnerGate != null && !this._teamAISiegeComponent.InnerGate.IsDeactivated && this._teamAISiegeComponent.InnerGate.State != CastleGate.GateState.Open) ? MovementOrder.MovementOrderAttackEntity(GameEntity.CreateFromWeakEntity(this._teamAISiegeComponent.InnerGate.GameEntity), false) : MovementOrder.MovementOrderStop);
			WorldPosition origin = this._teamAISiegeComponent.OuterGate.MiddleFrame.Origin;
			this._castleGateMoveOrder = MovementOrder.MovementOrderMove(origin);
			if (this._isGateLane)
			{
				this._wallSegmentMoveOrder = this._castleGateMoveOrder;
			}
			else
			{
				WorldPosition origin2 = this._wallSegment.MiddleFrame.Origin;
				this._wallSegmentMoveOrder = MovementOrder.MovementOrderMove(origin2);
			}
			this._facingOrder = FacingOrder.FacingOrderLookAtEnemy;
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0001A960 File Offset: 0x00018B60
		public BehaviorAssaultWalls(Formation formation)
			: base(formation)
		{
			base.BehaviorCoherence = 0f;
			this._behaviorSide = formation.AI.Side;
			this._teamAISiegeComponent = (TeamAISiegeComponent)formation.Team.TeamAI;
			this._behaviorState = BehaviorAssaultWalls.BehaviorState.Deciding;
			this.ResetOrderPositions();
			base.CurrentOrder = this._stopOrder;
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0001A9C0 File Offset: 0x00018BC0
		public override TextObject GetBehaviorString()
		{
			TextObject behaviorString = base.GetBehaviorString();
			TextObject textObject = GameTexts.FindText("str_formation_ai_side_strings", base.Formation.AI.Side.ToString());
			behaviorString.SetTextVariable("SIDE_STRING", textObject);
			behaviorString.SetTextVariable("IS_GENERAL_SIDE", "0");
			return behaviorString;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0001AA1C File Offset: 0x00018C1C
		private BehaviorAssaultWalls.BehaviorState CheckAndChangeState()
		{
			switch (this._behaviorState)
			{
			case BehaviorAssaultWalls.BehaviorState.Deciding:
				if (!this._isGateLane && this._wallSegment == null)
				{
					return BehaviorAssaultWalls.BehaviorState.Charging;
				}
				if (!this._isGateLane)
				{
					return BehaviorAssaultWalls.BehaviorState.ClimbWall;
				}
				if (this._teamAISiegeComponent.OuterGate.IsGateOpen && this._teamAISiegeComponent.InnerGate.IsGateOpen)
				{
					return BehaviorAssaultWalls.BehaviorState.Charging;
				}
				return BehaviorAssaultWalls.BehaviorState.AttackEntity;
			case BehaviorAssaultWalls.BehaviorState.ClimbWall:
			{
				if (this._wallSegment == null)
				{
					return BehaviorAssaultWalls.BehaviorState.Charging;
				}
				bool flag = false;
				if (this._behaviorSide < FormationAI.BehaviorSide.BehaviorSideNotSet)
				{
					SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes[(int)this._behaviorSide];
					flag = siegeLane.IsUnderAttack() && !siegeLane.IsDefended();
				}
				flag = flag || base.Formation.CachedMedianPosition.GetNavMeshVec3().DistanceSquared(this._wallSegment.MiddleFrame.Origin.GetNavMeshVec3()) < base.Formation.Depth * base.Formation.Depth;
				if (flag)
				{
					return BehaviorAssaultWalls.BehaviorState.TakeControl;
				}
				return BehaviorAssaultWalls.BehaviorState.ClimbWall;
			}
			case BehaviorAssaultWalls.BehaviorState.AttackEntity:
				if (this._teamAISiegeComponent.OuterGate.IsGateOpen && this._teamAISiegeComponent.InnerGate.IsGateOpen)
				{
					return BehaviorAssaultWalls.BehaviorState.Charging;
				}
				return BehaviorAssaultWalls.BehaviorState.AttackEntity;
			case BehaviorAssaultWalls.BehaviorState.TakeControl:
				if (base.Formation.CachedClosestEnemyFormation == null)
				{
					return BehaviorAssaultWalls.BehaviorState.Stop;
				}
				if (TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == this._behaviorSide).IsDefended())
				{
					return BehaviorAssaultWalls.BehaviorState.TakeControl;
				}
				if (!this._teamAISiegeComponent.OuterGate.IsGateOpen || !this._teamAISiegeComponent.InnerGate.IsGateOpen)
				{
					return BehaviorAssaultWalls.BehaviorState.MoveToGate;
				}
				return BehaviorAssaultWalls.BehaviorState.Charging;
			case BehaviorAssaultWalls.BehaviorState.MoveToGate:
				if (this._teamAISiegeComponent.OuterGate.IsGateOpen && this._teamAISiegeComponent.InnerGate.IsGateOpen)
				{
					return BehaviorAssaultWalls.BehaviorState.Charging;
				}
				return BehaviorAssaultWalls.BehaviorState.MoveToGate;
			case BehaviorAssaultWalls.BehaviorState.Charging:
				if ((!this._isGateLane || !this._teamAISiegeComponent.OuterGate.IsGateOpen || !this._teamAISiegeComponent.InnerGate.IsGateOpen) && this._behaviorSide < FormationAI.BehaviorSide.BehaviorSideNotSet)
				{
					if (!TeamAISiegeComponent.SiegeLanes[(int)this._behaviorSide].IsOpen && !TeamAISiegeComponent.IsFormationInsideCastle(base.Formation, true, 0.4f))
					{
						return BehaviorAssaultWalls.BehaviorState.Deciding;
					}
					if (base.Formation.CachedClosestEnemyFormation == null)
					{
						return BehaviorAssaultWalls.BehaviorState.Stop;
					}
				}
				return BehaviorAssaultWalls.BehaviorState.Charging;
			default:
				if (base.Formation.CachedClosestEnemyFormation != null)
				{
					return BehaviorAssaultWalls.BehaviorState.Deciding;
				}
				return BehaviorAssaultWalls.BehaviorState.Stop;
			}
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0001AC58 File Offset: 0x00018E58
		protected override void CalculateCurrentOrder()
		{
			switch (this._behaviorState)
			{
			case BehaviorAssaultWalls.BehaviorState.Deciding:
				base.CurrentOrder = this._stopOrder;
				return;
			case BehaviorAssaultWalls.BehaviorState.ClimbWall:
			{
				base.CurrentOrder = this._wallSegmentMoveOrder;
				WorldFrame worldFrame = this._wallSegment.MiddleFrame;
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(-worldFrame.Rotation.f.AsVec2.Normalized());
				this.CurrentArrangementOrder = ArrangementOrder.ArrangementOrderLine;
				return;
			}
			case BehaviorAssaultWalls.BehaviorState.AttackEntity:
				base.CurrentOrder = ((!this._teamAISiegeComponent.OuterGate.IsGateOpen) ? this._attackEntityOrderOuterGate : this._attackEntityOrderInnerGate);
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
				this.CurrentArrangementOrder = ArrangementOrder.ArrangementOrderLine;
				return;
			case BehaviorAssaultWalls.BehaviorState.TakeControl:
			{
				base.CurrentOrder = ((base.Formation.CachedClosestEnemyFormation != null) ? MovementOrder.MovementOrderChargeToTarget(base.Formation.CachedClosestEnemyFormation.Formation) : MovementOrder.MovementOrderCharge);
				WorldFrame worldFrame = this._wallSegment.MiddleFrame;
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(-worldFrame.Rotation.f.AsVec2.Normalized());
				this.CurrentArrangementOrder = ArrangementOrder.ArrangementOrderLine;
				return;
			}
			case BehaviorAssaultWalls.BehaviorState.MoveToGate:
			{
				base.CurrentOrder = this._castleGateMoveOrder;
				WorldFrame worldFrame = this._innerGate.MiddleFrame;
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(-worldFrame.Rotation.f.AsVec2.Normalized());
				this.CurrentArrangementOrder = ArrangementOrder.ArrangementOrderLine;
				return;
			}
			case BehaviorAssaultWalls.BehaviorState.Charging:
				base.CurrentOrder = this._chargeOrder;
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
				this.CurrentArrangementOrder = ArrangementOrder.ArrangementOrderLoose;
				return;
			case BehaviorAssaultWalls.BehaviorState.Stop:
				base.CurrentOrder = this._chargeOrder;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0001AE12 File Offset: 0x00019012
		public override void OnValidBehaviorSideChanged()
		{
			base.OnValidBehaviorSideChanged();
			this.ResetOrderPositions();
			this._behaviorState = BehaviorAssaultWalls.BehaviorState.Deciding;
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0001AE28 File Offset: 0x00019028
		public override void TickOccasionally()
		{
			BehaviorAssaultWalls.BehaviorState behaviorState = this.CheckAndChangeState();
			this._behaviorState = behaviorState;
			this.CalculateCurrentOrder();
			foreach (IPrimarySiegeWeapon primarySiegeWeapon in this._primarySiegeWeapons)
			{
				UsableMachine usableMachine = primarySiegeWeapon as UsableMachine;
				if (!usableMachine.IsDeactivated && !primarySiegeWeapon.HasCompletedAction() && !usableMachine.IsUsedByFormation(base.Formation))
				{
					base.Formation.StartUsingMachine(primarySiegeWeapon as UsableMachine, false);
				}
			}
			if (this._behaviorState == BehaviorAssaultWalls.BehaviorState.MoveToGate || this._behaviorState == BehaviorAssaultWalls.BehaviorState.Stop || this._behaviorState == BehaviorAssaultWalls.BehaviorState.Charging || this._behaviorState == BehaviorAssaultWalls.BehaviorState.TakeControl)
			{
				CastleGate castleGate = this._teamAISiegeComponent.InnerGate;
				if (castleGate != null && !castleGate.IsGateOpen && !castleGate.IsDestroyed)
				{
					if (!castleGate.IsUsedByFormation(base.Formation))
					{
						base.Formation.StartUsingMachine(castleGate, false);
					}
				}
				else
				{
					castleGate = this._teamAISiegeComponent.OuterGate;
					if (castleGate != null && !castleGate.IsGateOpen && !castleGate.IsDestroyed && !castleGate.IsUsedByFormation(base.Formation))
					{
						base.Formation.StartUsingMachine(castleGate, false);
					}
				}
			}
			else
			{
				if (base.Formation.Detachments.Contains(this._teamAISiegeComponent.OuterGate))
				{
					base.Formation.StopUsingMachine(this._teamAISiegeComponent.OuterGate, false);
				}
				if (base.Formation.Detachments.Contains(this._teamAISiegeComponent.InnerGate))
				{
					base.Formation.StopUsingMachine(this._teamAISiegeComponent.InnerGate, false);
				}
			}
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(this.CurrentArrangementOrder);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0001B014 File Offset: 0x00019214
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderHoldYourFire);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x0001B07A File Offset: 0x0001927A
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x0001B084 File Offset: 0x00019284
		protected override float GetAiWeight()
		{
			float num = 0f;
			if (this._teamAISiegeComponent != null)
			{
				if (this._primarySiegeWeapons.Any<IPrimarySiegeWeapon>((IPrimarySiegeWeapon psw) => psw.HasCompletedAction()) || this._wallSegment != null)
				{
					if (this._teamAISiegeComponent.IsCastleBreached())
					{
						num = 0.75f;
					}
					else
					{
						num = 0.25f;
					}
				}
				else if (this._teamAISiegeComponent.OuterGate.DefenseSide == this._behaviorSide)
				{
					num = 0.1f;
				}
			}
			return num;
		}

		// Token: 0x04000315 RID: 789
		private BehaviorAssaultWalls.BehaviorState _behaviorState;

		// Token: 0x04000316 RID: 790
		private List<IPrimarySiegeWeapon> _primarySiegeWeapons;

		// Token: 0x04000317 RID: 791
		private WallSegment _wallSegment;

		// Token: 0x04000318 RID: 792
		private CastleGate _innerGate;

		// Token: 0x04000319 RID: 793
		private TeamAISiegeComponent _teamAISiegeComponent;

		// Token: 0x0400031A RID: 794
		private MovementOrder _attackEntityOrderInnerGate;

		// Token: 0x0400031B RID: 795
		private MovementOrder _attackEntityOrderOuterGate;

		// Token: 0x0400031C RID: 796
		private MovementOrder _chargeOrder;

		// Token: 0x0400031D RID: 797
		private MovementOrder _stopOrder;

		// Token: 0x0400031E RID: 798
		private MovementOrder _castleGateMoveOrder;

		// Token: 0x0400031F RID: 799
		private MovementOrder _wallSegmentMoveOrder;

		// Token: 0x04000320 RID: 800
		private FacingOrder _facingOrder;

		// Token: 0x04000321 RID: 801
		protected ArrangementOrder CurrentArrangementOrder;

		// Token: 0x04000322 RID: 802
		private bool _isGateLane;

		// Token: 0x0200042E RID: 1070
		private enum BehaviorState
		{
			// Token: 0x04001936 RID: 6454
			Deciding,
			// Token: 0x04001937 RID: 6455
			ClimbWall,
			// Token: 0x04001938 RID: 6456
			AttackEntity,
			// Token: 0x04001939 RID: 6457
			TakeControl,
			// Token: 0x0400193A RID: 6458
			MoveToGate,
			// Token: 0x0400193B RID: 6459
			Charging,
			// Token: 0x0400193C RID: 6460
			Stop
		}
	}
}
