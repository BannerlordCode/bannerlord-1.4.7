using System;
using SandBox.Missions.MissionLogics;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000B2 RID: 178
	public class StandGuardBehavior : AgentBehavior
	{
		// Token: 0x0600076C RID: 1900 RVA: 0x00032B7B File Offset: 0x00030D7B
		public StandGuardBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
			this._missionAgentHandler = base.Mission.GetMissionBehavior<MissionAgentHandler>();
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x00032B98 File Offset: 0x00030D98
		public override void Tick(float dt, bool isSimulation)
		{
			if (base.OwnerAgent.CurrentWatchState == Agent.WatchState.Patrolling)
			{
				if (this._standPoint == null || isSimulation)
				{
					UsableMachine usableMachine = this._oldStandPoint ?? this._missionAgentHandler.FindUnusedPointWithTagForAgent(base.OwnerAgent, base.Navigator.SpecialTargetTag);
					if (usableMachine != null)
					{
						this._oldStandPoint = null;
						this._standPoint = usableMachine;
						base.Navigator.SetTarget(this._standPoint, false, Agent.AIScriptedFrameFlags.None);
						return;
					}
				}
			}
			else if (this._standPoint != null)
			{
				this._oldStandPoint = this._standPoint;
				base.Navigator.SetTarget(null, false, Agent.AIScriptedFrameFlags.None);
				this._standPoint = null;
			}
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00032C34 File Offset: 0x00030E34
		protected override void OnDeactivate()
		{
			base.Navigator.ClearTarget();
			this._standPoint = null;
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00032C48 File Offset: 0x00030E48
		public override float GetAvailability(bool isSimulation)
		{
			return 1f;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00032C4F File Offset: 0x00030E4F
		public override string GetDebugInfo()
		{
			return "Guard stand";
		}

		// Token: 0x04000403 RID: 1027
		private UsableMachine _oldStandPoint;

		// Token: 0x04000404 RID: 1028
		private UsableMachine _standPoint;

		// Token: 0x04000405 RID: 1029
		private readonly MissionAgentHandler _missionAgentHandler;

		// Token: 0x020001BA RID: 442
		private enum GuardState
		{
			// Token: 0x0400080D RID: 2061
			StandIdle,
			// Token: 0x0400080E RID: 2062
			StandAttention,
			// Token: 0x0400080F RID: 2063
			StandCautious,
			// Token: 0x04000810 RID: 2064
			GotToStandPoint
		}
	}
}
