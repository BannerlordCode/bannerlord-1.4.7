using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000139 RID: 313
	public class BehaviorWaitForLadders : BehaviorComponent
	{
		// Token: 0x06000F0F RID: 3855 RVA: 0x000287D8 File Offset: 0x000269D8
		public BehaviorWaitForLadders(Formation formation)
			: base(formation)
		{
			this._behaviorSide = formation.AI.Side;
			this._ladders = Mission.Current.ActiveMissionObjects.OfType<SiegeLadder>().ToList<SiegeLadder>();
			this._ladders.RemoveAll((SiegeLadder l) => l.IsDeactivated || l.WeaponSide != this._behaviorSide);
			this._teamAISiegeComponent = (TeamAISiegeComponent)formation.Team.TeamAI;
			SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == this._behaviorSide);
			object obj;
			if (siegeLane == null)
			{
				obj = null;
			}
			else
			{
				obj = siegeLane.DefensePoints.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp is WallSegment && (dp as WallSegment).IsBreachedWall);
			}
			this._breachedWallSegment = obj as WallSegment;
			this.ResetFollowOrder();
			this._stopOrder = MovementOrder.MovementOrderStop;
			if (this._followOrder.OrderEnum != MovementOrder.MovementOrderEnum.Invalid)
			{
				base.CurrentOrder = this._followOrder;
				this._behaviorState = BehaviorWaitForLadders.BehaviorState.Follow;
				return;
			}
			base.CurrentOrder = this._stopOrder;
			this._behaviorState = BehaviorWaitForLadders.BehaviorState.Stop;
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x000288DC File Offset: 0x00026ADC
		private void ResetFollowOrder()
		{
			this._followedEntity = null;
			this._followTacticalPosition = null;
			if (this._ladders.Count > 0)
			{
				SiegeLadder siegeLadder;
				if ((siegeLadder = this._ladders.FirstOrDefault<SiegeLadder>((SiegeLadder l) => !l.IsDeactivated && l.InitialWaitPosition.HasScriptOfType<TacticalPosition>())) == null)
				{
					siegeLadder = this._ladders.FirstOrDefault<SiegeLadder>((SiegeLadder l) => !l.IsDeactivated);
				}
				this._followedEntity = siegeLadder.InitialWaitPosition;
				if (this._followedEntity == null)
				{
					this._followedEntity = this._ladders.FirstOrDefault<SiegeLadder>((SiegeLadder l) => !l.IsDeactivated).InitialWaitPosition;
				}
				this._followOrder = MovementOrder.MovementOrderFollowEntity(this._followedEntity);
			}
			else if (this._breachedWallSegment != null)
			{
				WeakGameEntity firstChildEntityWithTagRecursive = this._breachedWallSegment.GameEntity.GetFirstChildEntityWithTagRecursive("attacker_wait_pos");
				this._followedEntity = GameEntity.CreateFromWeakEntity(firstChildEntityWithTagRecursive);
				this._followOrder = MovementOrder.MovementOrderFollowEntity(this._followedEntity);
			}
			else
			{
				this._followOrder = MovementOrder.MovementOrderNull;
			}
			if (this._followedEntity != null)
			{
				this._followTacticalPosition = this._followedEntity.GetFirstScriptOfType<TacticalPosition>();
			}
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x00028A2C File Offset: 0x00026C2C
		public override void OnValidBehaviorSideChanged()
		{
			base.OnValidBehaviorSideChanged();
			this._ladders = Mission.Current.ActiveMissionObjects.OfType<SiegeLadder>().ToList<SiegeLadder>();
			this._ladders.RemoveAll((SiegeLadder l) => l.IsDeactivated || l.WeaponSide != this._behaviorSide);
			SiegeLane siegeLane = TeamAISiegeComponent.SiegeLanes.FirstOrDefault<SiegeLane>((SiegeLane sl) => sl.LaneSide == this._behaviorSide);
			object obj;
			if (siegeLane == null)
			{
				obj = null;
			}
			else
			{
				obj = siegeLane.DefensePoints.FirstOrDefault<ICastleKeyPosition>((ICastleKeyPosition dp) => dp is WallSegment && (dp as WallSegment).IsBreachedWall);
			}
			this._breachedWallSegment = obj as WallSegment;
			this.ResetFollowOrder();
			this._behaviorState = BehaviorWaitForLadders.BehaviorState.Unset;
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x00028AD0 File Offset: 0x00026CD0
		protected override void CalculateCurrentOrder()
		{
			BehaviorWaitForLadders.BehaviorState behaviorState = ((this._followOrder.OrderEnum != MovementOrder.MovementOrderEnum.Invalid) ? BehaviorWaitForLadders.BehaviorState.Follow : BehaviorWaitForLadders.BehaviorState.Stop);
			if (behaviorState != this._behaviorState)
			{
				if (behaviorState == BehaviorWaitForLadders.BehaviorState.Follow)
				{
					base.CurrentOrder = this._followOrder;
					if (this._followTacticalPosition != null)
					{
						this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(this._followTacticalPosition.Direction);
					}
					else
					{
						this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
					}
				}
				else
				{
					base.CurrentOrder = this._stopOrder;
					this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
				}
				this._behaviorState = behaviorState;
			}
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x00028B54 File Offset: 0x00026D54
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			if (this._ladders.RemoveAll((SiegeLadder l) => l.IsDeactivated) > 0)
			{
				this.ResetFollowOrder();
				this.CalculateCurrentOrder();
			}
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (this._behaviorState == BehaviorWaitForLadders.BehaviorState.Follow && this._followTacticalPosition != null)
			{
				base.Formation.SetFormOrder(FormOrder.FormOrderCustom(this._followTacticalPosition.Width), true);
			}
			foreach (SiegeLadder siegeLadder in this._ladders)
			{
				if (siegeLadder.IsUsedByFormation(base.Formation))
				{
					base.Formation.StopUsingMachine(siegeLadder, false);
				}
			}
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x00028C4C File Offset: 0x00026E4C
		protected override void OnBehaviorActivatedAux()
		{
			base.Formation.SetArrangementOrder(base.Formation.QuerySystem.HasShield ? ArrangementOrder.ArrangementOrderShieldWall : ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFacingOrder(FacingOrder.FacingOrderLookAtEnemy);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000F15 RID: 3861 RVA: 0x00028CB3 File Offset: 0x00026EB3
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x00028CBC File Offset: 0x00026EBC
		protected override float GetAiWeight()
		{
			float num = 0f;
			if (this._followOrder.OrderEnum != MovementOrder.MovementOrderEnum.Invalid && !this._teamAISiegeComponent.AreLaddersReady)
			{
				num = ((!this._teamAISiegeComponent.IsCastleBreached()) ? 1f : 0.5f);
			}
			return num;
		}

		// Token: 0x040003AE RID: 942
		private const string WallWaitPositionTag = "attacker_wait_pos";

		// Token: 0x040003AF RID: 943
		private List<SiegeLadder> _ladders;

		// Token: 0x040003B0 RID: 944
		private WallSegment _breachedWallSegment;

		// Token: 0x040003B1 RID: 945
		private TeamAISiegeComponent _teamAISiegeComponent;

		// Token: 0x040003B2 RID: 946
		private MovementOrder _stopOrder;

		// Token: 0x040003B3 RID: 947
		private MovementOrder _followOrder;

		// Token: 0x040003B4 RID: 948
		private BehaviorWaitForLadders.BehaviorState _behaviorState;

		// Token: 0x040003B5 RID: 949
		private GameEntity _followedEntity;

		// Token: 0x040003B6 RID: 950
		private TacticalPosition _followTacticalPosition;

		// Token: 0x02000451 RID: 1105
		private enum BehaviorState
		{
			// Token: 0x040019B1 RID: 6577
			Unset,
			// Token: 0x040019B2 RID: 6578
			Stop,
			// Token: 0x040019B3 RID: 6579
			Follow
		}
	}
}
