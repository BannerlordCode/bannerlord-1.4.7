using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.Source.Missions.Handlers
{
	// Token: 0x020003DB RID: 987
	public class MissionFacialAnimationHandler : MissionLogic
	{
		// Token: 0x0600369F RID: 13983 RVA: 0x000E2449 File Offset: 0x000E0649
		public override void EarlyStart()
		{
			this._animRefreshTimer = new Timer(base.Mission.CurrentTime, 5f, true);
		}

		// Token: 0x060036A0 RID: 13984 RVA: 0x000E2467 File Offset: 0x000E0667
		public override void AfterStart()
		{
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x000E2469 File Offset: 0x000E0669
		public override void OnMissionTick(float dt)
		{
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x000E246C File Offset: 0x000E066C
		private void SetDefaultFacialAnimationsForAllAgents()
		{
			foreach (Agent agent in base.Mission.Agents)
			{
				if (agent.IsActive() && agent.IsHuman)
				{
					agent.SetAgentFacialAnimation(Agent.FacialAnimChannel.Low, "idle_tired", true);
				}
			}
		}

		// Token: 0x04001781 RID: 6017
		private Timer _animRefreshTimer;
	}
}
