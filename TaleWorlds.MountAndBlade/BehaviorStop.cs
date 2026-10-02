using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000134 RID: 308
	public class BehaviorStop : BehaviorComponent
	{
		// Token: 0x06000EEE RID: 3822 RVA: 0x00026E2E File Offset: 0x0002502E
		public BehaviorStop(Formation formation)
			: base(formation)
		{
			base.CurrentOrder = MovementOrder.MovementOrderStop;
			base.BehaviorCoherence = 0f;
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x00026E4D File Offset: 0x0002504D
		public override void TickOccasionally()
		{
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00026E60 File Offset: 0x00025060
		protected override void OnBehaviorActivatedAux()
		{
			base.Formation.SetArrangementOrder(base.Formation.QuerySystem.HasShield ? ArrangementOrder.ArrangementOrderShieldWall : ArrangementOrder.ArrangementOrderLine);
			base.Formation.SetFiringOrder(FiringOrder.FiringOrderFireAtWill);
			this._lastPlayerInformTime = Mission.Current.CurrentTime;
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x00026EB6 File Offset: 0x000250B6
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x00026EBD File Offset: 0x000250BD
		protected override float GetAiWeight()
		{
			return 0.01f;
		}
	}
}
