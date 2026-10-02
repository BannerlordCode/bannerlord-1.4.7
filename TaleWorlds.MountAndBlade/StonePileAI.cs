using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200015A RID: 346
	public class StonePileAI : UsableMachineAIBase
	{
		// Token: 0x06001238 RID: 4664 RVA: 0x00039519 File Offset: 0x00037719
		public StonePileAI(StonePile stonePile)
			: base(stonePile)
		{
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x00039524 File Offset: 0x00037724
		public static Agent GetSuitableAgentForStandingPoint(StonePile usableMachine, StandingPoint standingPoint, List<Agent> agents, List<Agent> usedAgents)
		{
			float num = float.MinValue;
			Agent agent = null;
			foreach (Agent agent2 in agents)
			{
				if (StonePileAI.IsAgentAssignable(agent2) && !standingPoint.IsDisabledForAgent(agent2) && standingPoint.GetUsageScoreForAgent(agent2) > num)
				{
					num = standingPoint.GetUsageScoreForAgent(agent2);
					agent = agent2;
				}
			}
			return agent;
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x0003959C File Offset: 0x0003779C
		public static Agent GetSuitableAgentForStandingPoint(StonePile stonePile, StandingPoint standingPoint, List<ValueTuple<Agent, float>> agents, List<Agent> usedAgents, float weight)
		{
			float num = float.MinValue;
			Agent agent = null;
			foreach (ValueTuple<Agent, float> valueTuple in agents)
			{
				Agent item = valueTuple.Item1;
				if (StonePileAI.IsAgentAssignable(item) && !standingPoint.IsDisabledForAgent(item) && standingPoint.GetUsageScoreForAgent(item) > num)
				{
					num = standingPoint.GetUsageScoreForAgent(item);
					agent = item;
				}
			}
			return agent;
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x00039618 File Offset: 0x00037818
		public static bool IsAgentAssignable(Agent agent)
		{
			return agent != null && agent.IsAIControlled && agent.IsActive() && !agent.IsRunningAway && !agent.InteractingWithAnyGameObject() && (agent.Formation == null || !agent.IsDetachedFromFormation);
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x00039652 File Offset: 0x00037852
		protected override void HandleAgentStopUsingStandingPoint(Agent agent, StandingPoint standingPoint)
		{
			agent.DisableScriptedCombatMovement();
			base.HandleAgentStopUsingStandingPoint(agent, standingPoint);
		}
	}
}
