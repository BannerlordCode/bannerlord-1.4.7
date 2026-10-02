using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000100 RID: 256
	[EngineStruct("Agent_movement_locked_state", true, "amls", false)]
	public enum AgentMovementLockedState
	{
		// Token: 0x040002C5 RID: 709
		None,
		// Token: 0x040002C6 RID: 710
		PositionLocked,
		// Token: 0x040002C7 RID: 711
		FrameLocked
	}
}
