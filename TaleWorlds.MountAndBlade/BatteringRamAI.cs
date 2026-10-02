using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200015C RID: 348
	public sealed class BatteringRamAI : UsableMachineAIBase
	{
		// Token: 0x0600123E RID: 4670 RVA: 0x0003966B File Offset: 0x0003786B
		public BatteringRamAI(BatteringRam batteringRam)
			: base(batteringRam)
		{
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x0600123F RID: 4671 RVA: 0x00039674 File Offset: 0x00037874
		private BatteringRam BatteringRam
		{
			get
			{
				return this.UsableMachine as BatteringRam;
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001240 RID: 4672 RVA: 0x00039681 File Offset: 0x00037881
		public override bool HasActionCompleted
		{
			get
			{
				return this.BatteringRam.IsDeactivated;
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06001241 RID: 4673 RVA: 0x00039690 File Offset: 0x00037890
		protected override MovementOrder NextOrder
		{
			get
			{
				TeamAISiegeComponent teamAISiegeComponent;
				if ((teamAISiegeComponent = Mission.Current.Teams[0].TeamAI as TeamAISiegeComponent) != null && teamAISiegeComponent.InnerGate != null && !teamAISiegeComponent.InnerGate.IsDestroyed)
				{
					return MovementOrder.MovementOrderAttackEntity(GameEntity.CreateFromWeakEntity(teamAISiegeComponent.InnerGate.GameEntity), false);
				}
				return MovementOrder.MovementOrderCharge;
			}
		}
	}
}
