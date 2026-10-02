using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200026D RID: 621
	public class AgentCommonAILogic : MissionLogic
	{
		// Token: 0x060022DF RID: 8927 RVA: 0x0007B2D8 File Offset: 0x000794D8
		public override void OnAgentCreated(Agent agent)
		{
			base.OnAgentCreated(agent);
			if (agent.IsAIControlled)
			{
				agent.AddComponent(new CommonAIComponent(agent));
			}
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x0007B2F8 File Offset: 0x000794F8
		protected internal override void OnAgentControllerChanged(Agent agent, AgentControllerType oldController)
		{
			base.OnAgentControllerChanged(agent, oldController);
			if (agent.IsActive())
			{
				if (agent.Controller == AgentControllerType.AI)
				{
					agent.AddComponent(new CommonAIComponent(agent));
					return;
				}
				if (oldController == AgentControllerType.AI && agent.CommonAIComponent != null)
				{
					agent.RemoveComponent(agent.CommonAIComponent);
				}
			}
		}
	}
}
