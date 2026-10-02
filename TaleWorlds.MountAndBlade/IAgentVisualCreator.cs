using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002C7 RID: 711
	public interface IAgentVisualCreator
	{
		// Token: 0x0600291F RID: 10527
		IAgentVisual Create(AgentVisualsData data, string name, bool needBatchedVersionForWeaponMeshes, bool forceUseFaceCache);
	}
}
