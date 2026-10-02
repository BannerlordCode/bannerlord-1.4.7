using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200008B RID: 139
	[EngineStruct("rglRagdoll::Ragdoll_state", true, "rds", false)]
	public enum RagdollState : ushort
	{
		// Token: 0x040001BF RID: 447
		Disabled,
		// Token: 0x040001C0 RID: 448
		NeedsActivation,
		// Token: 0x040001C1 RID: 449
		ActiveFirstTick,
		// Token: 0x040001C2 RID: 450
		Active,
		// Token: 0x040001C3 RID: 451
		NeedsDeactivation
	}
}
