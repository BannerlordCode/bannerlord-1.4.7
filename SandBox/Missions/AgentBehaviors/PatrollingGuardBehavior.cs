using System;
using SandBox.Missions.MissionLogics;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000B0 RID: 176
	public class PatrollingGuardBehavior : AgentBehavior
	{
		// Token: 0x0600075A RID: 1882 RVA: 0x00032367 File Offset: 0x00030567
		public PatrollingGuardBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
			this._missionAgentHandler = base.Mission.GetMissionBehavior<MissionAgentHandler>();
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00032384 File Offset: 0x00030584
		public override void Tick(float dt, bool isSimulation)
		{
			if (this._target == null)
			{
				UsableMachine usableMachine = ((base.Navigator.SpecialTargetTag == null || base.Navigator.SpecialTargetTag.IsEmpty<char>()) ? this._missionAgentHandler.FindUnusedPointWithTagForAgent(base.OwnerAgent, "npc_common") : this._missionAgentHandler.FindUnusedPointWithTagForAgent(base.OwnerAgent, base.Navigator.SpecialTargetTag));
				if (usableMachine != null)
				{
					this._target = usableMachine;
					base.Navigator.SetTarget(this._target, false, Agent.AIScriptedFrameFlags.None);
					return;
				}
			}
			else if (base.Navigator.TargetUsableMachine == null)
			{
				base.Navigator.SetTarget(this._target, false, Agent.AIScriptedFrameFlags.None);
			}
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0003242B File Offset: 0x0003062B
		public override float GetAvailability(bool isSimulation)
		{
			if (this._missionAgentHandler.GetAllUsablePointsWithTag(base.Navigator.SpecialTargetTag).Count <= 0)
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x00032456 File Offset: 0x00030656
		protected override void OnDeactivate()
		{
			this._target = null;
			base.Navigator.ClearTarget();
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0003246A File Offset: 0x0003066A
		public override string GetDebugInfo()
		{
			return "Guard patrol";
		}

		// Token: 0x040003F2 RID: 1010
		private readonly MissionAgentHandler _missionAgentHandler;

		// Token: 0x040003F3 RID: 1011
		private UsableMachine _target;
	}
}
