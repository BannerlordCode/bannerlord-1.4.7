using System;
using TaleWorlds.Library.EventSystem;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x0200007A RID: 122
	public class MissionPlayerMovementFlagsChangeEvent : EventBase
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x00023CB1 File Offset: 0x00021EB1
		// (set) Token: 0x060004A9 RID: 1193 RVA: 0x00023CB9 File Offset: 0x00021EB9
		public Agent.MovementControlFlag MovementFlag { get; private set; }

		// Token: 0x060004AA RID: 1194 RVA: 0x00023CC2 File Offset: 0x00021EC2
		public MissionPlayerMovementFlagsChangeEvent(Agent.MovementControlFlag movementFlag)
		{
			this.MovementFlag = movementFlag;
		}
	}
}
