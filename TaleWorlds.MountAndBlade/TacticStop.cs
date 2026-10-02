using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000176 RID: 374
	public class TacticStop : TacticComponent
	{
		// Token: 0x0600137F RID: 4991 RVA: 0x00048880 File Offset: 0x00046A80
		public TacticStop(Team team)
			: base(team)
		{
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x0004888C File Offset: 0x00046A8C
		public override void TickOccasionally()
		{
			foreach (Formation formation in base.FormationsIncludingEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					formation.AI.ResetBehaviorWeights();
					formation.AI.SetBehaviorWeight<BehaviorStop>(1f);
				}
			}
			base.TickOccasionally();
		}
	}
}
