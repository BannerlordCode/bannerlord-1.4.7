using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200015F RID: 351
	public sealed class SiegeLadderAI : UsableMachineAIBase
	{
		// Token: 0x0600124B RID: 4683 RVA: 0x00039A85 File Offset: 0x00037C85
		public SiegeLadderAI(SiegeLadder ladder)
			: base(ladder)
		{
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x0600124C RID: 4684 RVA: 0x00039A8E File Offset: 0x00037C8E
		public SiegeLadder Ladder
		{
			get
			{
				return this.UsableMachine as SiegeLadder;
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x0600124D RID: 4685 RVA: 0x00039A9B File Offset: 0x00037C9B
		public override bool HasActionCompleted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x0600124E RID: 4686 RVA: 0x00039A9E File Offset: 0x00037C9E
		protected override MovementOrder NextOrder
		{
			get
			{
				return MovementOrder.MovementOrderCharge;
			}
		}
	}
}
