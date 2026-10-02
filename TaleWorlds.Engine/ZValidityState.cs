using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200009F RID: 159
	[EngineStruct("rglWorld_position::z_validity_state", true, "zvs", false)]
	public enum ZValidityState
	{
		// Token: 0x0400020B RID: 523
		Invalid,
		// Token: 0x0400020C RID: 524
		BatchFormationUnitPosition,
		// Token: 0x0400020D RID: 525
		ValidAccordingToNavMesh,
		// Token: 0x0400020E RID: 526
		Valid
	}
}
