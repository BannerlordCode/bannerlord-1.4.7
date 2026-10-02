using System;
using SandBox.Missions.MissionLogics;
using SandBox.Objects.AnimationPoints;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000B4 RID: 180
	public class WalkingBehavior : AgentBehavior
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x00032EFB File Offset: 0x000310FB
		private bool CanWander
		{
			get
			{
				return (this._isIndoor && this._indoorWanderingIsActive) || (!this._isIndoor && this._outdoorWanderingIsActive);
			}
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00032F20 File Offset: 0x00031120
		public WalkingBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
			this._missionAgentHandler = base.Mission.GetMissionBehavior<MissionAgentHandler>();
			this._wanderTarget = null;
			this._isIndoor = CampaignMission.Current.Location.IsIndoor;
			this._indoorWanderingIsActive = true;
			this._outdoorWanderingIsActive = true;
			this._wasSimulation = false;
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x00032F76 File Offset: 0x00031176
		public void SetIndoorWandering(bool isActive)
		{
			this._indoorWanderingIsActive = isActive;
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00032F7F File Offset: 0x0003117F
		public void SetOutdoorWandering(bool isActive)
		{
			this._outdoorWanderingIsActive = isActive;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00032F88 File Offset: 0x00031188
		public override void Tick(float dt, bool isSimulation)
		{
			if (this._wanderTarget == null || base.Navigator.TargetUsableMachine == null || this._wanderTarget.IsDisabled || !this._wanderTarget.IsStandingPointAvailableForAgent(base.OwnerAgent))
			{
				this._wanderTarget = this.FindTarget();
				this._lastTarget = this._wanderTarget;
			}
			else if (base.Navigator.GetDistanceToTarget(this._wanderTarget) < 5f)
			{
				bool flag = this._wasSimulation && !isSimulation && this._wanderTarget != null && this._waitTimer != null && MBRandom.RandomFloat < (this._isIndoor ? 0f : (Settlement.CurrentSettlement.IsVillage ? 0.6f : 0.1f));
				if (this._waitTimer == null)
				{
					if (!this._wanderTarget.GameEntity.HasTag("npc_idle"))
					{
						this.SetTimerForTheAgent(isSimulation);
					}
				}
				else if (this._waitTimer.Check(base.Mission.CurrentTime) || flag)
				{
					if (this.CanWander)
					{
						this._waitTimer = null;
						UsableMachine usableMachine = this.FindTarget();
						if (usableMachine == null || this.IsChildrenOfSameParent(usableMachine, this._wanderTarget))
						{
							this.SetTimerForTheAgent(isSimulation);
						}
						else
						{
							this._lastTarget = this._wanderTarget;
							this._wanderTarget = usableMachine;
						}
					}
					else
					{
						this._waitTimer.Reset(100f);
					}
				}
			}
			if (base.OwnerAgent.CurrentlyUsedGameObject != null && base.Navigator.GetDistanceToTarget(this._lastTarget) > 1f)
			{
				base.Navigator.SetTarget(this._lastTarget, this._lastTarget == this._wanderTarget, Agent.AIScriptedFrameFlags.None);
			}
			base.Navigator.SetTarget(this._wanderTarget, false, Agent.AIScriptedFrameFlags.None);
			this._wasSimulation = isSimulation;
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0003314C File Offset: 0x0003134C
		private void SetTimerForTheAgent(bool isSimulation)
		{
			AnimationPoint animationPoint;
			float num = (((animationPoint = base.OwnerAgent.CurrentlyUsedGameObject as AnimationPoint) != null) ? animationPoint.GetRandomWaitInSeconds() : 10f);
			if (isSimulation && MBRandom.RandomFloat < 0.33f)
			{
				num /= 10f + MBRandom.RandomFloat * 10f;
			}
			this._waitTimer = new Timer(base.Mission.CurrentTime, (num < 0f) ? 2.1474836E+09f : num, true);
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x000331C8 File Offset: 0x000313C8
		private bool IsChildrenOfSameParent(UsableMachine machine, UsableMachine otherMachine)
		{
			WeakGameEntity weakGameEntity = machine.GameEntity;
			while (weakGameEntity.Parent.IsValid)
			{
				weakGameEntity = weakGameEntity.Parent;
			}
			WeakGameEntity weakGameEntity2 = otherMachine.GameEntity;
			while (weakGameEntity2.Parent.IsValid)
			{
				weakGameEntity2 = weakGameEntity2.Parent;
			}
			return weakGameEntity == weakGameEntity2;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00033220 File Offset: 0x00031420
		public override void ConversationTick()
		{
			if (this._waitTimer != null)
			{
				this._waitTimer.Reset(base.Mission.CurrentTime);
			}
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00033240 File Offset: 0x00031440
		public override float GetAvailability(bool isSimulation)
		{
			if (this.FindTarget() == null)
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x00033255 File Offset: 0x00031455
		public override void SetCustomWanderTarget(UsableMachine customUsableMachine)
		{
			this._wanderTarget = customUsableMachine;
			if (this._waitTimer != null)
			{
				this._waitTimer = null;
			}
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00033270 File Offset: 0x00031470
		private UsableMachine FindRandomWalkingTarget(bool forWaiting)
		{
			if (forWaiting && (this._wanderTarget ?? base.Navigator.TargetUsableMachine) != null)
			{
				return null;
			}
			string text = base.OwnerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.SpecialTargetTag;
			if (text == null)
			{
				text = "npc_common";
			}
			else if (!this._missionAgentHandler.HasUsablePointWithTag(text))
			{
				text = "npc_common_limited";
			}
			return this._missionAgentHandler.FindUnusedPointWithTagForAgent(base.OwnerAgent, text);
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x000332E0 File Offset: 0x000314E0
		private UsableMachine FindTarget()
		{
			return this.FindRandomWalkingTarget(this._isIndoor && !this._indoorWanderingIsActive);
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x000332FC File Offset: 0x000314FC
		private float GetTargetScore(UsableMachine usableMachine)
		{
			if (base.OwnerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.SpecialTargetTag != null && !usableMachine.GameEntity.HasTag(base.OwnerAgent.GetComponent<CampaignAgentComponent>().AgentNavigator.SpecialTargetTag))
			{
				return 0f;
			}
			StandingPoint vacantStandingPointForAI = usableMachine.GetVacantStandingPointForAI(base.OwnerAgent);
			if (vacantStandingPointForAI == null || vacantStandingPointForAI.IsDisabledForAgent(base.OwnerAgent))
			{
				return 0f;
			}
			float num = 1f;
			Vec3 vec = vacantStandingPointForAI.GetUserFrameForAgent(base.OwnerAgent).Origin.GetGroundVec3() - base.OwnerAgent.Position;
			if (vec.Length < 2f)
			{
				num *= vec.Length / 2f;
			}
			return num * (0.8f + MBRandom.RandomFloat * 0.2f);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x000333D4 File Offset: 0x000315D4
		public override void OnSpecialTargetChanged()
		{
			if (this._wanderTarget == null)
			{
				return;
			}
			if (!base.Navigator.SpecialTargetTag.IsEmpty<char>() && !this._wanderTarget.GameEntity.HasTag(base.Navigator.SpecialTargetTag))
			{
				this._wanderTarget = null;
				base.Navigator.SetTarget(this._wanderTarget, false, Agent.AIScriptedFrameFlags.None);
				return;
			}
			if (base.Navigator.SpecialTargetTag.IsEmpty<char>() && !this._wanderTarget.GameEntity.HasTag("npc_common"))
			{
				this._wanderTarget = null;
				base.Navigator.SetTarget(this._wanderTarget, false, Agent.AIScriptedFrameFlags.None);
			}
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00033480 File Offset: 0x00031680
		public override string GetDebugInfo()
		{
			string text = "Walk ";
			if (this._waitTimer != null)
			{
				text = string.Concat(new object[]
				{
					text,
					"(Wait ",
					(int)this._waitTimer.ElapsedTime(),
					"/",
					this._waitTimer.Duration,
					")"
				});
			}
			else if (this._wanderTarget == null)
			{
				text += "(search for target!)";
			}
			return text;
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00033501 File Offset: 0x00031701
		protected override void OnDeactivate()
		{
			base.Navigator.ClearTarget();
			this._wanderTarget = null;
			this._waitTimer = null;
		}

		// Token: 0x04000408 RID: 1032
		private readonly MissionAgentHandler _missionAgentHandler;

		// Token: 0x04000409 RID: 1033
		private readonly bool _isIndoor;

		// Token: 0x0400040A RID: 1034
		private UsableMachine _wanderTarget;

		// Token: 0x0400040B RID: 1035
		private UsableMachine _lastTarget;

		// Token: 0x0400040C RID: 1036
		private Timer _waitTimer;

		// Token: 0x0400040D RID: 1037
		private bool _indoorWanderingIsActive;

		// Token: 0x0400040E RID: 1038
		private bool _outdoorWanderingIsActive;

		// Token: 0x0400040F RID: 1039
		private bool _wasSimulation;
	}
}
