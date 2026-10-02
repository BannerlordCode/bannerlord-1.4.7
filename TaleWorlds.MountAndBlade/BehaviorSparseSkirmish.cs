using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000133 RID: 307
	public class BehaviorSparseSkirmish : BehaviorComponent
	{
		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000EE6 RID: 3814 RVA: 0x00026C5E File Offset: 0x00024E5E
		// (set) Token: 0x06000EE7 RID: 3815 RVA: 0x00026C66 File Offset: 0x00024E66
		public GameEntity ArcherPosition
		{
			get
			{
				return this._archerPosition;
			}
			set
			{
				if (this._archerPosition != value)
				{
					this.SetArcherPosition(value);
				}
			}
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00026C80 File Offset: 0x00024E80
		private void SetArcherPosition(GameEntity value)
		{
			this._archerPosition = value;
			if (!(this._archerPosition != null))
			{
				this._tacticalArcherPosition = null;
				WorldPosition cachedMedianPosition = base.Formation.CachedMedianPosition;
				cachedMedianPosition.SetVec2(base.Formation.CurrentPosition);
				base.CurrentOrder = MovementOrder.MovementOrderMove(cachedMedianPosition);
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
				return;
			}
			this._tacticalArcherPosition = this._archerPosition.GetFirstScriptOfType<TacticalPosition>();
			if (this._tacticalArcherPosition != null)
			{
				base.CurrentOrder = MovementOrder.MovementOrderMove(this._tacticalArcherPosition.Position);
				this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(this._tacticalArcherPosition.Direction);
				return;
			}
			base.CurrentOrder = MovementOrder.MovementOrderMove(this._archerPosition.GlobalPosition.ToWorldPosition());
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00026D4B File Offset: 0x00024F4B
		public BehaviorSparseSkirmish(Formation formation)
			: base(formation)
		{
			this.SetArcherPosition(this._archerPosition);
			base.BehaviorCoherence = 0f;
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x00026D6C File Offset: 0x00024F6C
		public override void TickOccasionally()
		{
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (this._tacticalArcherPosition != null)
			{
				base.Formation.SetFormOrder(FormOrder.FormOrderCustom(this._tacticalArcherPosition.Width), true);
			}
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00026DC0 File Offset: 0x00024FC0
		protected override void OnBehaviorActivatedAux()
		{
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderScatter);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWider, true);
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000EEC RID: 3820 RVA: 0x00026E20 File Offset: 0x00025020
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x00026E27 File Offset: 0x00025027
		protected override float GetAiWeight()
		{
			return 2f;
		}

		// Token: 0x0400039A RID: 922
		private GameEntity _archerPosition;

		// Token: 0x0400039B RID: 923
		private TacticalPosition _tacticalArcherPosition;
	}
}
