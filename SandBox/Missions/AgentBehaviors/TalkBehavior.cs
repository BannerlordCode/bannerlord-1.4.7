using System;
using SandBox.Conversation.MissionLogics;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000B3 RID: 179
	public class TalkBehavior : AgentBehavior
	{
		// Token: 0x06000771 RID: 1905 RVA: 0x00032C56 File Offset: 0x00030E56
		public TalkBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
			this._startConversation = true;
			this._doNotMove = true;
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x00032C70 File Offset: 0x00030E70
		public override void Tick(float dt, bool isSimulation)
		{
			if (!this._startConversation || base.Mission.MainAgent == null || !base.Mission.MainAgent.IsActive() || base.Mission.Mode == MissionMode.Conversation || base.Mission.Mode == MissionMode.Battle || base.Mission.Mode == MissionMode.Barter)
			{
				return;
			}
			float interactionDistanceToUsable = base.OwnerAgent.GetInteractionDistanceToUsable(base.Mission.MainAgent);
			if (base.OwnerAgent.Position.DistanceSquared(base.Mission.MainAgent.Position) < (interactionDistanceToUsable + 3f) * (interactionDistanceToUsable + 3f) && base.Navigator.CanSeeAgent(base.Mission.MainAgent))
			{
				AgentNavigator navigator = base.Navigator;
				WorldPosition worldPosition = base.OwnerAgent.GetWorldPosition();
				MatrixFrame matrixFrame = base.OwnerAgent.Frame;
				navigator.SetTargetFrame(worldPosition, matrixFrame.rotation.f.AsVec2.RotationInRadians, 1f, -10f, Agent.AIScriptedFrameFlags.DoNotRun, false);
				MissionConversationLogic missionBehavior = base.Mission.GetMissionBehavior<MissionConversationLogic>();
				if (missionBehavior != null && missionBehavior.IsReadyForConversation)
				{
					missionBehavior.OnAgentInteraction(base.Mission.MainAgent, base.OwnerAgent, -1);
					this._startConversation = false;
					return;
				}
			}
			else if (!this._doNotMove)
			{
				AgentNavigator navigator2 = base.Navigator;
				WorldPosition worldPosition2 = Agent.Main.GetWorldPosition();
				MatrixFrame matrixFrame = Agent.Main.Frame;
				navigator2.SetTargetFrame(worldPosition2, matrixFrame.rotation.f.AsVec2.RotationInRadians, 1f, -10f, Agent.AIScriptedFrameFlags.DoNotRun, false);
			}
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00032E08 File Offset: 0x00031008
		public override float GetAvailability(bool isSimulation)
		{
			if (isSimulation)
			{
				return 0f;
			}
			if (this._startConversation && base.Mission.MainAgent != null && base.Mission.MainAgent.IsActive())
			{
				float num = base.OwnerAgent.GetInteractionDistanceToUsable(base.Mission.MainAgent) + 3f;
				if (base.OwnerAgent.Position.DistanceSquared(base.Mission.MainAgent.Position) < num * num && base.Mission.Mode != MissionMode.Conversation && !base.Mission.MainAgent.IsEnemyOf(base.OwnerAgent))
				{
					return 1f;
				}
			}
			return 0f;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x00032EC1 File Offset: 0x000310C1
		public override string GetDebugInfo()
		{
			return "Talk";
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00032EC8 File Offset: 0x000310C8
		protected override void OnDeactivate()
		{
			base.Navigator.ClearTarget();
			this.Disable();
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00032EDB File Offset: 0x000310DB
		public void Disable()
		{
			this._startConversation = false;
			this._doNotMove = true;
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00032EEB File Offset: 0x000310EB
		public void Enable(bool doNotMove)
		{
			this._startConversation = true;
			this._doNotMove = doNotMove;
		}

		// Token: 0x04000406 RID: 1030
		private bool _doNotMove;

		// Token: 0x04000407 RID: 1031
		private bool _startConversation;
	}
}
