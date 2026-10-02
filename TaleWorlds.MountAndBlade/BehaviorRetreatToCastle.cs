using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000124 RID: 292
	public class BehaviorRetreatToCastle : BehaviorComponent
	{
		// Token: 0x06000E6D RID: 3693 RVA: 0x000225E0 File Offset: 0x000207E0
		public BehaviorRetreatToCastle(Formation formation)
			: base(formation)
		{
			WorldPosition worldPosition = Mission.Current.DeploymentPlan.GetFormationPlan(formation.Team, FormationClass.Cavalry, false).CreateNewDeploymentWorldPosition(WorldPosition.WorldPositionEnforcedCache.GroundVec3);
			base.CurrentOrder = MovementOrder.MovementOrderMove(worldPosition);
			base.BehaviorCoherence = 0f;
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x00022629 File Offset: 0x00020829
		public override void TickOccasionally()
		{
			base.TickOccasionally();
			if (base.Formation.AI.ActiveBehavior == this)
			{
				base.Formation.SetMovementOrder(base.CurrentOrder);
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000E6F RID: 3695 RVA: 0x00022655 File Offset: 0x00020855
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x0002265C File Offset: 0x0002085C
		protected override float GetAiWeight()
		{
			return 1f;
		}
	}
}
