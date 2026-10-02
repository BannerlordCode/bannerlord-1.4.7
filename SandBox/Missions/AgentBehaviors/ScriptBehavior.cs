using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000B1 RID: 177
	public class ScriptBehavior : AgentBehavior
	{
		// Token: 0x0600075F RID: 1887 RVA: 0x00032471 File Offset: 0x00030671
		public ScriptBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x00032490 File Offset: 0x00030690
		public static void AddUsableMachineTarget(Agent ownerAgent, UsableMachine targetUsableMachine)
		{
			DailyBehaviorGroup behaviorGroup = ownerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
			ScriptBehavior scriptBehavior = behaviorGroup.GetBehavior<ScriptBehavior>() ?? behaviorGroup.AddBehavior<ScriptBehavior>();
			bool flag = behaviorGroup.ScriptedBehavior != scriptBehavior;
			scriptBehavior._targetUsableMachine = targetUsableMachine;
			scriptBehavior._state = ScriptBehavior.State.GoToUsableMachine;
			scriptBehavior._sentToTarget = false;
			if (flag)
			{
				behaviorGroup.SetScriptedBehavior<ScriptBehavior>();
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x000324E8 File Offset: 0x000306E8
		public static void AddAgentTarget(Agent ownerAgent, Agent targetAgent)
		{
			DailyBehaviorGroup behaviorGroup = ownerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
			ScriptBehavior scriptBehavior = behaviorGroup.GetBehavior<ScriptBehavior>() ?? behaviorGroup.AddBehavior<ScriptBehavior>();
			bool flag = behaviorGroup.ScriptedBehavior != scriptBehavior;
			scriptBehavior._targetAgent = targetAgent;
			scriptBehavior._state = ScriptBehavior.State.GoToAgent;
			scriptBehavior._sentToTarget = false;
			if (flag)
			{
				behaviorGroup.SetScriptedBehavior<ScriptBehavior>();
			}
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00032540 File Offset: 0x00030740
		public static void AddWorldFrameTarget(Agent ownerAgent, WorldFrame targetWorldFrame)
		{
			DailyBehaviorGroup behaviorGroup = ownerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
			ScriptBehavior scriptBehavior = behaviorGroup.GetBehavior<ScriptBehavior>() ?? behaviorGroup.AddBehavior<ScriptBehavior>();
			bool flag = behaviorGroup.ScriptedBehavior != scriptBehavior;
			scriptBehavior._targetFrame = targetWorldFrame;
			scriptBehavior._state = ScriptBehavior.State.GoToTargetFrame;
			scriptBehavior._sentToTarget = false;
			if (flag)
			{
				behaviorGroup.SetScriptedBehavior<ScriptBehavior>();
			}
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x00032598 File Offset: 0x00030798
		public static void AddTargetWithDelegate(Agent ownerAgent, ScriptBehavior.SelectTargetDelegate selectTargetDelegate, ScriptBehavior.OnTargetReachedWaitDelegate onTargetReachWaitDelegate, ScriptBehavior.OnTargetReachedDelegate onTargetReachedDelegate, float initialWaitInSeconds = 0f)
		{
			DailyBehaviorGroup behaviorGroup = ownerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<DailyBehaviorGroup>();
			ScriptBehavior scriptBehavior = behaviorGroup.GetBehavior<ScriptBehavior>() ?? behaviorGroup.AddBehavior<ScriptBehavior>();
			bool flag = behaviorGroup.ScriptedBehavior != scriptBehavior;
			scriptBehavior._selectTargetDelegate = selectTargetDelegate;
			scriptBehavior._onTargetReachedDelegate = onTargetReachedDelegate;
			scriptBehavior._onTargetReachWaitDelegate = onTargetReachWaitDelegate;
			scriptBehavior._initialWaitInSeconds = initialWaitInSeconds;
			scriptBehavior._isInitiallyWaiting = initialWaitInSeconds > 0f;
			scriptBehavior._state = ScriptBehavior.State.NoTarget;
			scriptBehavior._sentToTarget = false;
			if (flag)
			{
				behaviorGroup.SetScriptedBehavior<ScriptBehavior>();
			}
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x00032615 File Offset: 0x00030815
		public bool IsNearTarget(Agent targetAgent)
		{
			return this._targetAgent == targetAgent && (this._state == ScriptBehavior.State.NearAgent || this._state == ScriptBehavior.State.NearStationaryTarget);
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x00032638 File Offset: 0x00030838
		public override void Tick(float dt, bool isSimulation)
		{
			if (this._isInitiallyWaiting)
			{
				if (this._waitTimer == null)
				{
					this._waitTimer = new MissionTimer(this._initialWaitInSeconds);
					return;
				}
				if (this._waitTimer.Check(false))
				{
					this._isInitiallyWaiting = false;
					this._waitTimer = null;
					return;
				}
			}
			else
			{
				if (this._state == ScriptBehavior.State.NoTarget)
				{
					if (this._selectTargetDelegate == null)
					{
						if (this.BehaviorGroup.ScriptedBehavior == this)
						{
							this.BehaviorGroup.DisableScriptedBehavior();
						}
						return;
					}
					this.SearchForNewTarget();
				}
				switch (this._state)
				{
				case ScriptBehavior.State.GoToUsableMachine:
					if (!this._sentToTarget)
					{
						base.Navigator.SetTarget(this._targetUsableMachine, false, Agent.AIScriptedFrameFlags.None);
						this._sentToTarget = true;
						return;
					}
					if (base.OwnerAgent.IsUsingGameObject && base.OwnerAgent.Position.DistanceSquared(this._targetUsableMachine.GameEntity.GetGlobalFrame().origin) < 1f)
					{
						if (this.CheckForSearchNewTarget(ScriptBehavior.State.NearStationaryTarget))
						{
							base.OwnerAgent.StopUsingGameObject(false, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
							return;
						}
						this.RemoveTargets();
						return;
					}
					break;
				case ScriptBehavior.State.GoToAgent:
					if (this._targetAgent.IsActive())
					{
						float interactionDistanceToUsable = base.OwnerAgent.GetInteractionDistanceToUsable(this._targetAgent);
						if (base.OwnerAgent.Position.DistanceSquared(this._targetAgent.Position) >= interactionDistanceToUsable * interactionDistanceToUsable)
						{
							AgentNavigator navigator = base.Navigator;
							WorldPosition worldPosition = this._targetAgent.GetWorldPosition();
							MatrixFrame matrixFrame = this._targetAgent.Frame;
							navigator.SetTargetFrame(worldPosition, matrixFrame.rotation.f.AsVec2.RotationInRadians, this._customTargetReachedRangeThreshold, this._customTargetReachedRotationThreshold, Agent.AIScriptedFrameFlags.None, false);
							return;
						}
						if (!this.CheckForSearchNewTarget(ScriptBehavior.State.NearAgent))
						{
							AgentNavigator navigator2 = base.Navigator;
							WorldPosition worldPosition2 = base.OwnerAgent.GetWorldPosition();
							MatrixFrame matrixFrame = base.OwnerAgent.Frame;
							navigator2.SetTargetFrame(worldPosition2, matrixFrame.rotation.f.AsVec2.RotationInRadians, this._customTargetReachedRangeThreshold, this._customTargetReachedRotationThreshold, Agent.AIScriptedFrameFlags.None, false);
							this.RemoveTargets();
							return;
						}
					}
					else if (!this.CheckForSearchNewTarget(ScriptBehavior.State.NearAgent))
					{
						AgentNavigator navigator3 = base.Navigator;
						WorldPosition worldPosition3 = base.OwnerAgent.GetWorldPosition();
						MatrixFrame matrixFrame = base.OwnerAgent.Frame;
						navigator3.SetTargetFrame(worldPosition3, matrixFrame.rotation.f.AsVec2.RotationInRadians, this._customTargetReachedRangeThreshold, this._customTargetReachedRotationThreshold, Agent.AIScriptedFrameFlags.None, false);
						this.RemoveTargets();
						return;
					}
					break;
				case ScriptBehavior.State.GoToTargetFrame:
					if (!this._sentToTarget)
					{
						base.Navigator.SetTargetFrame(this._targetFrame.Origin, this._targetFrame.Rotation.f.AsVec2.RotationInRadians, this._customTargetReachedRangeThreshold, this._customTargetReachedRotationThreshold, Agent.AIScriptedFrameFlags.DoNotRun, false);
						this._sentToTarget = true;
						return;
					}
					if (base.Navigator.IsTargetReached() && !this.CheckForSearchNewTarget(ScriptBehavior.State.NearStationaryTarget) && this._waitTimer == null)
					{
						this.RemoveTargets();
						return;
					}
					break;
				case ScriptBehavior.State.NearAgent:
				{
					if (base.OwnerAgent.Position.DistanceSquared(this._targetAgent.Position) >= 1f)
					{
						this._state = ScriptBehavior.State.GoToAgent;
						return;
					}
					AgentNavigator navigator4 = base.Navigator;
					WorldPosition worldPosition4 = base.OwnerAgent.GetWorldPosition();
					MatrixFrame matrixFrame = base.OwnerAgent.Frame;
					navigator4.SetTargetFrame(worldPosition4, matrixFrame.rotation.f.AsVec2.RotationInRadians, this._customTargetReachedRangeThreshold, this._customTargetReachedRotationThreshold, Agent.AIScriptedFrameFlags.None, false);
					this.RemoveTargets();
					break;
				}
				default:
					return;
				}
			}
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x000329AC File Offset: 0x00030BAC
		private bool CheckForSearchNewTarget(ScriptBehavior.State endState)
		{
			bool flag = false;
			bool flag2 = false;
			if (this._onTargetReachWaitDelegate != null && !this._isWaiting)
			{
				this._onTargetReachWaitDelegate(base.OwnerAgent, ref this._waitTimeInSeconds);
				this._isWaiting = this._waitTimeInSeconds > 0f;
			}
			if (this._isWaiting)
			{
				if (this._waitTimer == null)
				{
					this._waitTimer = new MissionTimer(this._waitTimeInSeconds);
				}
				else if (this._waitTimer.Check(false))
				{
					this._isWaiting = false;
					this._waitTimer = null;
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				if (this._onTargetReachedDelegate != null)
				{
					flag2 = this._onTargetReachedDelegate(base.OwnerAgent, ref this._targetAgent, ref this._targetUsableMachine, ref this._targetFrame);
				}
				if (flag2)
				{
					this.SearchForNewTarget();
				}
				else
				{
					this._state = endState;
				}
				return flag2;
			}
			return false;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00032A80 File Offset: 0x00030C80
		private void SearchForNewTarget()
		{
			Agent agent = null;
			UsableMachine usableMachine = null;
			WorldFrame invalid = WorldFrame.Invalid;
			float customTargetReachedRangeThreshold = this._customTargetReachedRangeThreshold;
			float customTargetReachedRotationThreshold = this._customTargetReachedRotationThreshold;
			if (this._selectTargetDelegate(base.OwnerAgent, ref agent, ref usableMachine, ref invalid, ref customTargetReachedRangeThreshold, ref customTargetReachedRotationThreshold))
			{
				if (agent != null)
				{
					this._targetAgent = agent;
					this._state = ScriptBehavior.State.GoToAgent;
					this._sentToTarget = false;
				}
				else if (usableMachine != null)
				{
					this._targetUsableMachine = usableMachine;
					this._state = ScriptBehavior.State.GoToUsableMachine;
					this._sentToTarget = false;
				}
				else
				{
					this._targetFrame = invalid;
					this._state = ScriptBehavior.State.GoToTargetFrame;
					this._sentToTarget = false;
				}
				this._customTargetReachedRangeThreshold = customTargetReachedRangeThreshold;
				this._customTargetReachedRotationThreshold = customTargetReachedRotationThreshold;
			}
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00032B1B File Offset: 0x00030D1B
		public override float GetAvailability(bool isSimulation)
		{
			return (float)((this._state == ScriptBehavior.State.NoTarget) ? 0 : 1);
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00032B2A File Offset: 0x00030D2A
		protected override void OnDeactivate()
		{
			base.Navigator.ClearTarget();
			this.RemoveTargets();
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00032B3D File Offset: 0x00030D3D
		private void RemoveTargets()
		{
			this._targetUsableMachine = null;
			this._targetAgent = null;
			this._targetFrame = WorldFrame.Invalid;
			this._state = ScriptBehavior.State.NoTarget;
			this._selectTargetDelegate = null;
			this._onTargetReachedDelegate = null;
			this._sentToTarget = false;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x00032B74 File Offset: 0x00030D74
		public override string GetDebugInfo()
		{
			return "Scripted";
		}

		// Token: 0x040003F4 RID: 1012
		private UsableMachine _targetUsableMachine;

		// Token: 0x040003F5 RID: 1013
		private Agent _targetAgent;

		// Token: 0x040003F6 RID: 1014
		private WorldFrame _targetFrame;

		// Token: 0x040003F7 RID: 1015
		private ScriptBehavior.State _state;

		// Token: 0x040003F8 RID: 1016
		private bool _sentToTarget;

		// Token: 0x040003F9 RID: 1017
		private float _waitTimeInSeconds;

		// Token: 0x040003FA RID: 1018
		private bool _isWaiting;

		// Token: 0x040003FB RID: 1019
		private MissionTimer _waitTimer;

		// Token: 0x040003FC RID: 1020
		private float _customTargetReachedRangeThreshold = 1f;

		// Token: 0x040003FD RID: 1021
		private float _customTargetReachedRotationThreshold = 1f;

		// Token: 0x040003FE RID: 1022
		private float _initialWaitInSeconds;

		// Token: 0x040003FF RID: 1023
		private bool _isInitiallyWaiting;

		// Token: 0x04000400 RID: 1024
		private ScriptBehavior.SelectTargetDelegate _selectTargetDelegate;

		// Token: 0x04000401 RID: 1025
		private ScriptBehavior.OnTargetReachedDelegate _onTargetReachedDelegate;

		// Token: 0x04000402 RID: 1026
		private ScriptBehavior.OnTargetReachedWaitDelegate _onTargetReachWaitDelegate;

		// Token: 0x020001B6 RID: 438
		// (Invoke) Token: 0x06000F4E RID: 3918
		public delegate bool SelectTargetDelegate(Agent agent, ref Agent targetAgent, ref UsableMachine targetUsableMachine, ref WorldFrame targetFrame, ref float customTargetReachedRangeThreshold, ref float customTargetReachedRotationThreshold);

		// Token: 0x020001B7 RID: 439
		// (Invoke) Token: 0x06000F52 RID: 3922
		public delegate bool OnTargetReachedDelegate(Agent agent, ref Agent targetAgent, ref UsableMachine targetUsableMachine, ref WorldFrame targetFrame);

		// Token: 0x020001B8 RID: 440
		// (Invoke) Token: 0x06000F56 RID: 3926
		public delegate void OnTargetReachedWaitDelegate(Agent agent, ref float waitTimeInSeconds);

		// Token: 0x020001B9 RID: 441
		private enum State
		{
			// Token: 0x04000806 RID: 2054
			NoTarget,
			// Token: 0x04000807 RID: 2055
			GoToUsableMachine,
			// Token: 0x04000808 RID: 2056
			GoToAgent,
			// Token: 0x04000809 RID: 2057
			GoToTargetFrame,
			// Token: 0x0400080A RID: 2058
			NearAgent,
			// Token: 0x0400080B RID: 2059
			NearStationaryTarget
		}
	}
}
