using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.Source.Missions.Handlers.Logic
{
	// Token: 0x020003DE RID: 990
	public class BattleMissionAgentInteractionLogic : MissionLogic
	{
		// Token: 0x060036AD RID: 13997 RVA: 0x000E2C88 File Offset: 0x000E0E88
		public override bool IsThereAgentAction(Agent userAgent, Agent otherAgent)
		{
			return otherAgent.IsMount && otherAgent.IsActive() && (otherAgent.RiderAgent == userAgent || (otherAgent.RiderAgent == null && (userAgent.GetAgentFlags() & AgentFlag.CanRide) == AgentFlag.CanRide));
		}
	}
}
