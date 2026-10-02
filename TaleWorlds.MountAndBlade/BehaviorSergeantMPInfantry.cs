using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000128 RID: 296
	public class BehaviorSergeantMPInfantry : BehaviorComponent
	{
		// Token: 0x06000E84 RID: 3716 RVA: 0x00023250 File Offset: 0x00021450
		public BehaviorSergeantMPInfantry(Formation formation)
			: base(formation)
		{
			this._behaviorState = BehaviorSergeantMPInfantry.BehaviorState.Unset;
			this._flagpositions = base.Formation.Team.Mission.ActiveMissionObjects.FindAllWithType<FlagCapturePoint>().ToList<FlagCapturePoint>();
			this._flagDominationGameMode = base.Formation.Team.Mission.GetMissionBehavior<MissionMultiplayerFlagDomination>();
			this.CalculateCurrentOrder();
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x000232B4 File Offset: 0x000214B4
		protected override void CalculateCurrentOrder()
		{
			BehaviorSergeantMPInfantry.BehaviorState behaviorState;
			if (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation != null && ((base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.IsRangedFormation && base.Formation.CachedAveragePosition.DistanceSquared(base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2) <= ((this._behaviorState == BehaviorSergeantMPInfantry.BehaviorState.Attacking) ? 3600f : 2500f)) || (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.IsInfantryFormation && base.Formation.CachedAveragePosition.DistanceSquared(base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2) <= ((this._behaviorState == BehaviorSergeantMPInfantry.BehaviorState.Attacking) ? 900f : 400f))))
			{
				behaviorState = BehaviorSergeantMPInfantry.BehaviorState.Attacking;
			}
			else
			{
				behaviorState = BehaviorSergeantMPInfantry.BehaviorState.GoingToFlag;
			}
			if (behaviorState == BehaviorSergeantMPInfantry.BehaviorState.Attacking && (this._behaviorState != BehaviorSergeantMPInfantry.BehaviorState.Attacking || base.CurrentOrder.OrderEnum != MovementOrder.MovementOrderEnum.ChargeToTarget || base.CurrentOrder.TargetFormation.QuerySystem != base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation))
			{
				this._behaviorState = BehaviorSergeantMPInfantry.BehaviorState.Attacking;
				base.CurrentOrder = MovementOrder.MovementOrderChargeToTarget(base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation);
			}
			if (behaviorState == BehaviorSergeantMPInfantry.BehaviorState.GoingToFlag)
			{
				this._behaviorState = behaviorState;
				WorldPosition cachedMedianPosition;
				if (this._flagpositions.Any<FlagCapturePoint>((FlagCapturePoint fp) => this._flagDominationGameMode.GetFlagOwnerTeam(fp) != base.Formation.Team))
				{
					cachedMedianPosition = new WorldPosition(base.Formation.Team.Mission.Scene, UIntPtr.Zero, this._flagpositions.Where<FlagCapturePoint>((FlagCapturePoint fp) => this._flagDominationGameMode.GetFlagOwnerTeam(fp) != base.Formation.Team).MinBy<FlagCapturePoint, float>((FlagCapturePoint fp) => fp.Position.AsVec2.DistanceSquared(base.Formation.CachedAveragePosition)).Position, false);
				}
				else if (this._flagpositions.Any<FlagCapturePoint>((FlagCapturePoint fp) => this._flagDominationGameMode.GetFlagOwnerTeam(fp) == base.Formation.Team))
				{
					cachedMedianPosition = new WorldPosition(base.Formation.Team.Mission.Scene, UIntPtr.Zero, this._flagpositions.Where<FlagCapturePoint>((FlagCapturePoint fp) => this._flagDominationGameMode.GetFlagOwnerTeam(fp) == base.Formation.Team).MinBy<FlagCapturePoint, float>((FlagCapturePoint fp) => fp.Position.AsVec2.DistanceSquared(base.Formation.CachedAveragePosition)).Position, false);
				}
				else
				{
					cachedMedianPosition = base.Formation.CachedMedianPosition;
					cachedMedianPosition.SetVec2(base.Formation.CachedAveragePosition);
				}
				if (base.CurrentOrder.OrderEnum == MovementOrder.MovementOrderEnum.Invalid || base.CurrentOrder.GetPosition(base.Formation) != cachedMedianPosition.AsVec2)
				{
					Vec2 vec;
					if (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation == null)
					{
						vec = base.Formation.Direction;
					}
					else
					{
						vec = (base.Formation.QuerySystem.ClosestSignificantlyLargeEnemyFormation.Formation.CachedMedianPosition.AsVec2 - base.Formation.CachedAveragePosition).Normalized();
					}
					base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
					this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(vec);
				}
			}
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x000235B4 File Offset: 0x000217B4
		public override void TickOccasionally()
		{
			this._flagpositions.RemoveAll((FlagCapturePoint fp) => fp.IsDeactivated);
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (base.Formation.QuerySystem.HasShield && (this._behaviorState == BehaviorSergeantMPInfantry.BehaviorState.Attacking || (this._behaviorState == BehaviorSergeantMPInfantry.BehaviorState.GoingToFlag && base.CurrentOrder.GetPosition(base.Formation).IsValid && base.Formation.CachedAveragePosition.DistanceSquared(base.CurrentOrder.GetPosition(base.Formation)) <= 225f)))
			{
				base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderShieldWall);
				return;
			}
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x000236A4 File Offset: 0x000218A4
		protected override void OnBehaviorActivatedAux()
		{
			this.CalculateCurrentOrder();
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderDeep, true);
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x0002370A File Offset: 0x0002190A
		protected override float GetAiWeight()
		{
			if (base.Formation.QuerySystem.IsInfantryFormation)
			{
				return 1.2f;
			}
			return 0f;
		}

		// Token: 0x0400037C RID: 892
		private BehaviorSergeantMPInfantry.BehaviorState _behaviorState;

		// Token: 0x0400037D RID: 893
		private List<FlagCapturePoint> _flagpositions;

		// Token: 0x0400037E RID: 894
		private MissionMultiplayerFlagDomination _flagDominationGameMode;

		// Token: 0x02000445 RID: 1093
		private enum BehaviorState
		{
			// Token: 0x04001986 RID: 6534
			GoingToFlag,
			// Token: 0x04001987 RID: 6535
			Attacking,
			// Token: 0x04001988 RID: 6536
			Unset
		}
	}
}
