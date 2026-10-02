using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.AI.AgentComponents
{
	// Token: 0x02000405 RID: 1029
	public class ScriptedMovementComponent : AgentComponent
	{
		// Token: 0x060037CC RID: 14284 RVA: 0x000E5750 File Offset: 0x000E3950
		public ScriptedMovementComponent(Agent agent, bool isCharacterToTalkTo = false, float dialogueProximityOffset = 0f)
			: base(agent)
		{
			this._isCharacterToTalkTo = isCharacterToTalkTo;
			this._agentSpeedLimit = this.Agent.GetMaximumSpeedLimit();
			if (!this._isCharacterToTalkTo)
			{
				this.Agent.SetMaximumSpeedLimit(MBRandom.RandomFloatRanged(0.2f, 0.3f), true);
				this._dialogueTriggerProximity += dialogueProximityOffset;
			}
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x000E57B8 File Offset: 0x000E39B8
		public void SetTargetAgent(Agent targetAgent)
		{
			this._targetAgent = targetAgent;
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x000E57C4 File Offset: 0x000E39C4
		public override void OnTick(float dt)
		{
			if (this.Agent.Mission.AllowAiTicking && this.Agent.IsAIControlled && this._targetAgent != null)
			{
				bool flag = this._targetAgent.State != AgentState.Routed && this._targetAgent.State != AgentState.Deleted;
				if (!this._isInDialogueRange)
				{
					float num = this._targetAgent.Position.DistanceSquared(this.Agent.Position);
					this._isInDialogueRange = num <= this._dialogueTriggerProximity * this._dialogueTriggerProximity;
					if (this._isInDialogueRange)
					{
						this.Agent.SetScriptedFlags(this.Agent.GetScriptedFlags() & ~Agent.AIScriptedFrameFlags.DoNotRun);
						this.Agent.DisableScriptedMovement();
						if (flag)
						{
							this.Agent.SetLookAgent(this._targetAgent);
						}
						this.Agent.SetMaximumSpeedLimit(this._agentSpeedLimit, false);
						return;
					}
					WorldPosition worldPosition = this._targetAgent.Position.ToWorldPosition();
					this.Agent.SetScriptedPosition(ref worldPosition, false, Agent.AIScriptedFrameFlags.DoNotRun);
					return;
				}
				else if (!flag)
				{
					this.Agent.SetLookAgent(null);
				}
			}
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x000E58EB File Offset: 0x000E3AEB
		public bool ShouldConversationStartWithAgent()
		{
			return this._targetAgent != null && this._isInDialogueRange && this._isCharacterToTalkTo;
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x000E5905 File Offset: 0x000E3B05
		public void Reset()
		{
			this._targetAgent = null;
			this._isInDialogueRange = false;
		}

		// Token: 0x040017EA RID: 6122
		private bool _isInDialogueRange;

		// Token: 0x040017EB RID: 6123
		private readonly bool _isCharacterToTalkTo;

		// Token: 0x040017EC RID: 6124
		private readonly float _dialogueTriggerProximity = 10f;

		// Token: 0x040017ED RID: 6125
		private readonly float _agentSpeedLimit;

		// Token: 0x040017EE RID: 6126
		private Agent _targetAgent;
	}
}
