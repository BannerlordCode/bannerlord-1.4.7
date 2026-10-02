using System;
using System.Diagnostics;
using SandBox.Objects;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.AI
{
	// Token: 0x0200010C RID: 268
	public class PassageAI : UsableMachineAIBase
	{
		// Token: 0x06000D60 RID: 3424 RVA: 0x00061164 File Offset: 0x0005F364
		public PassageAI(UsableMachine usableMachine)
			: base(usableMachine)
		{
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x0006116D File Offset: 0x0005F36D
		protected override Agent.AIScriptedFrameFlags GetScriptedFrameFlags(Agent agent)
		{
			if (agent.CurrentWatchState != Agent.WatchState.Alarmed)
			{
				return Agent.AIScriptedFrameFlags.NoAttack | Agent.AIScriptedFrameFlags.DoNotRun;
			}
			return Agent.AIScriptedFrameFlags.NoAttack | Agent.AIScriptedFrameFlags.NeverSlowDown;
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x00061180 File Offset: 0x0005F380
		protected override void OnTick(Agent agentToCompareTo, Formation formationToCompareTo, Team potentialUsersTeam, float dt)
		{
			foreach (StandingPoint standingPoint in this.UsableMachine.StandingPoints)
			{
				PassageUsePoint passageUsePoint = (PassageUsePoint)standingPoint;
				if ((agentToCompareTo == null || passageUsePoint.UserAgent == agentToCompareTo) && (formationToCompareTo == null || (passageUsePoint.UserAgent != null && passageUsePoint.UserAgent.IsAIControlled && passageUsePoint.UserAgent.Formation == formationToCompareTo)))
				{
					Debug.FailedAssert("isAgentManagedByThisMachineAI(standingPoint.UserAgent)", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\AI\\PassageAI.cs", "OnTick", 41);
					Agent userAgent = passageUsePoint.UserAgent;
					if (this.HasActionCompleted || (potentialUsersTeam != null && this.UsableMachine.IsDisabledForBattleSideAI(potentialUsersTeam.Side)) || userAgent.IsRunningAway)
					{
						this.HandleAgentStopUsingStandingPoint(userAgent, passageUsePoint);
					}
				}
				for (int i = passageUsePoint.MovingAgents.Count - 1; i >= 0; i--)
				{
					Agent agent = passageUsePoint.MovingAgents[i];
					if ((agentToCompareTo == null || agent == agentToCompareTo) && (formationToCompareTo == null || (agent != null && agent.IsAIControlled && agent.Formation == formationToCompareTo)))
					{
						if (this.HasActionCompleted || (potentialUsersTeam != null && this.UsableMachine.IsDisabledForBattleSideAI(potentialUsersTeam.Side)) || agent.IsRunningAway)
						{
							Debug.FailedAssert("HasActionCompleted || (potentialUsersTeam != null && UsableMachine.IsDisabledForBattleSideAI(potentialUsersTeam.Side)) || agent.IsRunningAway", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\AI\\PassageAI.cs", "OnTick", 69);
							this.HandleAgentStopUsingStandingPoint(agent, passageUsePoint);
						}
						else if (!passageUsePoint.IsDisabled && !agent.IsPaused && agent.CanReachAndUseObject(passageUsePoint, passageUsePoint.GetUserFrameForAgent(agent).Origin.GetGroundVec3().DistanceSquared(agent.Position)))
						{
							agent.UseGameObject(passageUsePoint, -1);
							agent.SetScriptedFlags(agent.GetScriptedFlags() & ~passageUsePoint.DisableScriptedFrameFlags);
						}
					}
				}
			}
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x00061368 File Offset: 0x0005F568
		[Conditional("DEBUG")]
		private void TickForDebug()
		{
			if (Input.DebugInput.IsHotKeyDown("UsableMachineAiBaseHotkeyShowMachineUsers"))
			{
				foreach (StandingPoint standingPoint in this.UsableMachine.StandingPoints)
				{
					PassageUsePoint passageUsePoint = (PassageUsePoint)standingPoint;
					foreach (Agent agent in passageUsePoint.MovingAgents)
					{
					}
					Agent userAgent = passageUsePoint.UserAgent;
				}
			}
		}
	}
}
