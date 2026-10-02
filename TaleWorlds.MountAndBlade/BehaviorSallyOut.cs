using System;
using System.Linq;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000126 RID: 294
	public class BehaviorSallyOut : BehaviorComponent
	{
		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000E75 RID: 3701 RVA: 0x000226BC File Offset: 0x000208BC
		private bool _calculateAreGatesOutsideOpen
		{
			get
			{
				return (this._teamAISiegeDefender.OuterGate == null || this._teamAISiegeDefender.OuterGate.IsGateOpen) && (this._teamAISiegeDefender.InnerGate == null || this._teamAISiegeDefender.InnerGate.IsGateOpen);
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x00022709 File Offset: 0x00020909
		private bool _calculateShouldStartAttacking
		{
			get
			{
				return this._calculateAreGatesOutsideOpen || !TeamAISiegeComponent.IsFormationInsideCastle(base.Formation, true, 0.4f);
			}
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00022729 File Offset: 0x00020929
		public BehaviorSallyOut(Formation formation)
			: base(formation)
		{
			this._teamAISiegeDefender = formation.Team.TeamAI as TeamAISiegeDefender;
			this._behaviorSide = formation.AI.Side;
			this.ResetOrderPositions();
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x0002275F File Offset: 0x0002095F
		protected override void CalculateCurrentOrder()
		{
			base.CalculateCurrentOrder();
			base.CurrentOrder = (this._calculateShouldStartAttacking ? this._attackOrder : this._gatherOrder);
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x00022784 File Offset: 0x00020984
		private void ResetOrderPositions()
		{
			SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == FormationAI.BehaviorSide.Middle);
			WorldFrame? worldFrame;
			if (siegeLane == null)
			{
				worldFrame = null;
			}
			else
			{
				ICastleKeyPosition castleKeyPosition = siegeLane.DefensePoints.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp.AttackerSiegeWeapon is UsableMachine && !(dp.AttackerSiegeWeapon as UsableMachine).IsDisabled);
				worldFrame = ((castleKeyPosition != null) ? new WorldFrame?(castleKeyPosition.DefenseWaitFrame) : null);
			}
			WorldFrame worldFrame2 = worldFrame ?? WorldFrame.Invalid;
			TacticalPosition tacticalPosition;
			if (siegeLane == null)
			{
				tacticalPosition = null;
			}
			else
			{
				ICastleKeyPosition castleKeyPosition2 = siegeLane.DefensePoints.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp.AttackerSiegeWeapon is UsableMachine && !(dp.AttackerSiegeWeapon as UsableMachine).IsDisabled);
				tacticalPosition = ((castleKeyPosition2 != null) ? castleKeyPosition2.WaitPosition : null);
			}
			this._gatheringTacticalPos = tacticalPosition;
			if (this._gatheringTacticalPos != null)
			{
				this._gatherOrder = MovementOrder.MovementOrderMove(this._gatheringTacticalPos.Position);
			}
			else if (worldFrame2.Origin.IsValid)
			{
				worldFrame2.Rotation.f.Normalize();
				this._gatherOrder = MovementOrder.MovementOrderMove(worldFrame2.Origin);
			}
			else
			{
				this._gatherOrder = MovementOrder.MovementOrderMove(base.Formation.CachedMedianPosition);
			}
			this._attackOrder = MovementOrder.MovementOrderCharge;
			base.CurrentOrder = (this._calculateShouldStartAttacking ? this._attackOrder : this._gatherOrder);
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x000228F8 File Offset: 0x00020AF8
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			if (!this._calculateAreGatesOutsideOpen)
			{
				CastleGate castleGate = ((this._teamAISiegeDefender.InnerGate != null && !this._teamAISiegeDefender.InnerGate.IsGateOpen) ? this._teamAISiegeDefender.InnerGate : this._teamAISiegeDefender.OuterGate);
				if (!castleGate.IsUsedByFormation(base.Formation))
				{
					base.Formation.StartUsingMachine(castleGate, false);
				}
			}
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00022980 File Offset: 0x00020B80
		protected override void OnBehaviorActivatedAux()
		{
			this._behaviorSide = base.Formation.AI.Side;
			this.ResetOrderPositions();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtEnemy);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x000229FB File Offset: 0x00020BFB
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x00022A02 File Offset: 0x00020C02
		protected override float GetAiWeight()
		{
			return 10f;
		}

		// Token: 0x04000376 RID: 886
		private readonly TeamAISiegeDefender _teamAISiegeDefender;

		// Token: 0x04000377 RID: 887
		private MovementOrder _gatherOrder;

		// Token: 0x04000378 RID: 888
		private MovementOrder _attackOrder;

		// Token: 0x04000379 RID: 889
		private TacticalPosition _gatheringTacticalPos;
	}
}
