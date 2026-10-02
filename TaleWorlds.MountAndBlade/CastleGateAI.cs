using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000159 RID: 345
	public class CastleGateAI : UsableMachineAIBase
	{
		// Token: 0x06001235 RID: 4661 RVA: 0x000394DE File Offset: 0x000376DE
		public void ResetInitialGateState(CastleGate.GateState newInitialState)
		{
			this._initialState = newInitialState;
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x000394E7 File Offset: 0x000376E7
		public CastleGateAI(CastleGate gate)
			: base(gate)
		{
			this._initialState = gate.State;
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001237 RID: 4663 RVA: 0x000394FC File Offset: 0x000376FC
		public override bool HasActionCompleted
		{
			get
			{
				return ((CastleGate)this.UsableMachine).State != this._initialState;
			}
		}

		// Token: 0x04000475 RID: 1141
		private CastleGate.GateState _initialState;
	}
}
