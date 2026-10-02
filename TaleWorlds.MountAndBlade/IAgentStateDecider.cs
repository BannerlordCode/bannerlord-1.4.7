using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000260 RID: 608
	public interface IAgentStateDecider : IMissionBehavior
	{
		// Token: 0x0600225E RID: 8798
		AgentState GetAgentState(Agent affectedAgent, float deathProbability, out bool usedSurgery);
	}
}
