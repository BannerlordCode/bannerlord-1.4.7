using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.MissionLogics.Arena
{
	// Token: 0x02000098 RID: 152
	public class ArenaAgentStateDeciderLogic : MissionLogic, IAgentStateDecider, IMissionBehavior
	{
		// Token: 0x06000655 RID: 1621 RVA: 0x0002AEE4 File Offset: 0x000290E4
		public AgentState GetAgentState(Agent effectedAgent, float deathProbability, out bool usedSurgery)
		{
			usedSurgery = false;
			return AgentState.Unconscious;
		}
	}
}
