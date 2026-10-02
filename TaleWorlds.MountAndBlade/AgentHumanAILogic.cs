using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200026E RID: 622
	public class AgentHumanAILogic : MissionLogic
	{
		// Token: 0x060022E2 RID: 8930 RVA: 0x0007B34C File Offset: 0x0007954C
		public override void OnAgentCreated(Agent agent)
		{
			base.OnAgentCreated(agent);
			if (agent.IsAIControlled && agent.IsHuman)
			{
				agent.AddComponent(new HumanAIComponent(agent));
			}
		}

		// Token: 0x060022E3 RID: 8931 RVA: 0x0007B374 File Offset: 0x00079574
		protected internal override void OnAgentControllerChanged(Agent agent, AgentControllerType oldController)
		{
			base.OnAgentControllerChanged(agent, oldController);
			if (agent.IsHuman)
			{
				if (agent.Controller == AgentControllerType.AI)
				{
					agent.AddComponent(new HumanAIComponent(agent));
					return;
				}
				if (oldController == AgentControllerType.AI && agent.HumanAIComponent != null)
				{
					agent.RemoveComponent(agent.HumanAIComponent);
				}
			}
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x0007B3C0 File Offset: 0x000795C0
		public override void OnAgentMount(Agent agent)
		{
			base.OnAgentMount(agent);
			Mission.Current.UpdateMountReservationsAfterRiderMounts(agent, agent.MountAgent);
		}
	}
}
