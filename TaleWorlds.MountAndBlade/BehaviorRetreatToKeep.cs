using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000125 RID: 293
	public class BehaviorRetreatToKeep : BehaviorComponent
	{
		// Token: 0x06000E71 RID: 3697 RVA: 0x00022663 File Offset: 0x00020863
		public BehaviorRetreatToKeep(Formation formation)
			: base(formation)
		{
			base.CurrentOrder = MovementOrder.MovementOrderRetreat;
			base.BehaviorCoherence = 0f;
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x00022682 File Offset: 0x00020882
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			if (base.Formation.AI.ActiveBehavior == this)
			{
				base.Formation.SetMovementOrder(base.CurrentOrder);
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000E73 RID: 3699 RVA: 0x000226AE File Offset: 0x000208AE
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x000226B5 File Offset: 0x000208B5
		protected override float GetAiWeight()
		{
			return 1f;
		}
	}
}
