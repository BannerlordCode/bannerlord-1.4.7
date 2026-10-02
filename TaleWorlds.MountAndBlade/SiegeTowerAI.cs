using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000160 RID: 352
	public sealed class SiegeTowerAI : UsableMachineAIBase
	{
		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x0600124F RID: 4687 RVA: 0x00039AA5 File Offset: 0x00037CA5
		private SiegeTower SiegeTower
		{
			get
			{
				return this.UsableMachine as SiegeTower;
			}
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x00039AB2 File Offset: 0x00037CB2
		public SiegeTowerAI(SiegeTower siegeTower)
			: base(siegeTower)
		{
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001251 RID: 4689 RVA: 0x00039ABB File Offset: 0x00037CBB
		public override bool HasActionCompleted
		{
			get
			{
				return this.SiegeTower.MovementComponent.HasArrivedAtTarget && this.SiegeTower.State == SiegeTower.GateState.Open;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001252 RID: 4690 RVA: 0x00039ADF File Offset: 0x00037CDF
		protected override MovementOrder NextOrder
		{
			get
			{
				return MovementOrder.MovementOrderCharge;
			}
		}
	}
}
