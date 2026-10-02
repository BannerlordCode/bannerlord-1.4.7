using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200012D RID: 301
	public class BehaviorShootFromCastleWalls : BehaviorComponent
	{
		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000EC0 RID: 3776 RVA: 0x00024E4A File Offset: 0x0002304A
		// (set) Token: 0x06000EC1 RID: 3777 RVA: 0x00024E52 File Offset: 0x00023052
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
					this.OnArcherPositionSet(value);
				}
			}
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x00024E69 File Offset: 0x00023069
		public BehaviorShootFromCastleWalls(Formation formation)
			: base(formation)
		{
			this.OnArcherPositionSet(this._archerPosition);
			base.BehaviorCoherence = 0f;
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x00024E8C File Offset: 0x0002308C
		private void OnArcherPositionSet(GameEntity value)
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
			this.CurrentFacingOrder = FacingOrder.FacingOrderLookAtDirection(this._archerPosition.GetGlobalFrame().rotation.f.AsVec2);
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x00024F78 File Offset: 0x00023178
		public override void TickOccasionally()
		{
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			if (this._tacticalArcherPosition != null)
			{
				base.Formation.SetFormOrder(FormOrder.FormOrderCustom(this._tacticalArcherPosition.Width), true);
			}
			foreach (Team team in base.Formation.Team.Mission.Teams)
			{
				if (team.IsEnemyOf(base.Formation.Team))
				{
					if (!this._areStrategicArcherAreasAbandoned)
					{
						if (team.QuerySystem.InsideWallsRatio > 0.6f)
						{
							base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderLine);
							this._areStrategicArcherAreasAbandoned = true;
							break;
						}
						break;
					}
					else
					{
						if (team.QuerySystem.InsideWallsRatio <= 0.4f)
						{
							base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderScatter);
							this._areStrategicArcherAreasAbandoned = false;
							break;
						}
						break;
					}
				}
			}
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x00025090 File Offset: 0x00023290
		protected override void OnBehaviorActivatedAux()
		{
			base.Formation.SetMovementOrder(base.CurrentOrder);
			base.Formation.SetFacingOrder(this.CurrentFacingOrder);
			base.Formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderScatter);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			base.Formation.SetFormOrder(FormOrder.FormOrderWide, true);
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000EC6 RID: 3782 RVA: 0x000250F0 File Offset: 0x000232F0
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x000250F7 File Offset: 0x000232F7
		protected override float GetAiWeight()
		{
			return 10f * (base.Formation.QuerySystem.RangedCavalryUnitRatio + base.Formation.QuerySystem.RangedUnitRatio);
		}

		// Token: 0x04000389 RID: 905
		private GameEntity _archerPosition;

		// Token: 0x0400038A RID: 906
		private TacticalPosition _tacticalArcherPosition;

		// Token: 0x0400038B RID: 907
		private bool _areStrategicArcherAreasAbandoned;
	}
}
