using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Missions.MissionLogics;
using SandBox.Objects.Usables;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000AA RID: 170
	public class FleeBehavior : AgentBehavior
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x00030149 File Offset: 0x0002E349
		// (set) Token: 0x0600071E RID: 1822 RVA: 0x00030154 File Offset: 0x0002E354
		private FleeBehavior.FleeTargetType SelectedFleeTargetType
		{
			get
			{
				return this._selectedFleeTargetType;
			}
			set
			{
				if (value != this._selectedFleeTargetType)
				{
					this._selectedFleeTargetType = value;
					MBActionSet actionSet = base.OwnerAgent.ActionSet;
					ActionIndexCache currentAction = base.OwnerAgent.GetCurrentAction(1);
					if (this._selectedFleeTargetType != FleeBehavior.FleeTargetType.Cover && !actionSet.AreActionsAlternatives(in currentAction, in ActionIndexCache.act_scared_idle_1) && !actionSet.AreActionsAlternatives(in currentAction, in ActionIndexCache.act_scared_reaction_1))
					{
						base.OwnerAgent.SetActionChannel(1, in ActionIndexCache.act_scared_reaction_1, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
					}
					if (this._selectedFleeTargetType == FleeBehavior.FleeTargetType.Cover)
					{
						this.BeAfraid();
					}
					this._selectedGoal.GoToTarget();
				}
			}
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x00030206 File Offset: 0x0002E406
		public FleeBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
			this._missionAgentHandler = base.Mission.GetMissionBehavior<MissionAgentHandler>();
			this._missionFightHandler = base.Mission.GetMissionBehavior<MissionFightHandler>();
			this._reconsiderFleeTargetTimer = new BasicMissionTimer();
			this._state = FleeBehavior.State.None;
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00030244 File Offset: 0x0002E444
		public override void Tick(float dt, bool isSimulation)
		{
			switch (this._state)
			{
			case FleeBehavior.State.None:
				base.OwnerAgent.DisableScriptedMovement();
				base.OwnerAgent.SetActionChannel(1, in ActionIndexCache.act_scared_reaction_1, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, MBRandom.RandomFloat, false, -0.2f, 0, true);
				this._selectedGoal = new FleeBehavior.FleeCoverTarget(base.Navigator, base.OwnerAgent);
				this.SelectedFleeTargetType = FleeBehavior.FleeTargetType.Cover;
				return;
			case FleeBehavior.State.Afraid:
				if (this._scareTimer.ElapsedTime > this._scareTime)
				{
					this._state = FleeBehavior.State.LookForPlace;
					this._scareTimer = null;
					return;
				}
				break;
			case FleeBehavior.State.LookForPlace:
				this.LookForPlace();
				return;
			case FleeBehavior.State.Flee:
				this.Flee();
				return;
			case FleeBehavior.State.Complain:
				if (this._complainToGuardTimer != null && this._complainToGuardTimer.ElapsedTime > 2f)
				{
					this._complainToGuardTimer = null;
					base.OwnerAgent.SetActionChannel(0, in ActionIndexCache.act_none, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
					base.OwnerAgent.SetLookAgent(null);
					(this._selectedGoal as FleeBehavior.FleeAgentTarget).Savior.SetLookAgent(null);
					AlarmedBehaviorGroup.AlarmAgent((this._selectedGoal as FleeBehavior.FleeAgentTarget).Savior);
					this._state = FleeBehavior.State.LookForPlace;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x000303A0 File Offset: 0x0002E5A0
		private Vec3 GetDangerPosition()
		{
			Vec3 vec = Vec3.Zero;
			if (this._missionFightHandler != null)
			{
				IEnumerable<Agent> dangerSources = this._missionFightHandler.GetDangerSources(base.OwnerAgent);
				if (dangerSources.Any<Agent>())
				{
					foreach (Agent agent in dangerSources)
					{
						vec += agent.Position;
					}
					vec /= (float)dangerSources.Count<Agent>();
				}
			}
			return vec;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00030428 File Offset: 0x0002E628
		private bool IsThereDanger()
		{
			return this._missionFightHandler != null && this._missionFightHandler.GetDangerSources(base.OwnerAgent).Any<Agent>();
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0003044C File Offset: 0x0002E64C
		private float GetPathScore(WorldPosition startWorldPos, WorldPosition targetWorldPos)
		{
			float num = 1f;
			NavigationPath navigationPath = new NavigationPath();
			base.Mission.Scene.GetPathBetweenAIFaces(startWorldPos.GetNearestNavMesh(), targetWorldPos.GetNearestNavMesh(), startWorldPos.AsVec2, targetWorldPos.AsVec2, 0f, navigationPath, null);
			Vec2 asVec = this.GetDangerPosition().AsVec2;
			float num2 = MBMath.WrapAngle((asVec - startWorldPos.AsVec2).RotationInRadians);
			float num3 = MathF.Abs(MBMath.GetSmallestDifferenceBetweenTwoAngles(MBMath.WrapAngle((navigationPath.Size > 0) ? (navigationPath.PathPoints[0] - startWorldPos.AsVec2).RotationInRadians : (targetWorldPos.AsVec2 - startWorldPos.AsVec2).RotationInRadians), num2)) / 3.1415927f * 1f;
			float num4 = startWorldPos.AsVec2.DistanceSquared(asVec);
			if (navigationPath.Size > 0)
			{
				float num5 = float.MaxValue;
				Vec2 vec = startWorldPos.AsVec2;
				for (int i = 0; i < navigationPath.Size; i++)
				{
					float num6 = Vec2.DistanceToLineSegmentSquared(navigationPath.PathPoints[i], vec, asVec);
					vec = navigationPath.PathPoints[i];
					if (num6 < num5)
					{
						num5 = num6;
					}
				}
				if (num4 > num5 && num5 < 25f)
				{
					num = 1f * (num5 - num4) / 225f;
				}
				else if (num4 > 4f)
				{
					num = 1f * num5 / 225f;
				}
				else
				{
					num = 1f;
				}
			}
			float num7 = 1f * (225f / startWorldPos.AsVec2.DistanceSquared(targetWorldPos.AsVec2));
			return (1f + num3) * (1f + num3) - 2f + num + num7;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00030624 File Offset: 0x0002E824
		private void LookForPlace()
		{
			FleeBehavior.FleeGoalBase fleeGoalBase = new FleeBehavior.FleeCoverTarget(base.Navigator, base.OwnerAgent);
			FleeBehavior.FleeTargetType fleeTargetType = FleeBehavior.FleeTargetType.Cover;
			if (this.IsThereDanger())
			{
				List<ValueTuple<float, Agent>> availableGuardScores = this.GetAvailableGuardScores(5);
				List<ValueTuple<float, Passage>> availablePassageScores = this.GetAvailablePassageScores(10);
				float num = float.MinValue;
				foreach (ValueTuple<float, Passage> valueTuple in availablePassageScores)
				{
					float item = valueTuple.Item1;
					if (item > num)
					{
						num = item;
						fleeTargetType = FleeBehavior.FleeTargetType.Indoor;
						fleeGoalBase = new FleeBehavior.FleePassageTarget(base.Navigator, base.OwnerAgent, valueTuple.Item2);
					}
				}
				foreach (ValueTuple<float, Agent> valueTuple2 in availableGuardScores)
				{
					float item2 = valueTuple2.Item1;
					if (item2 > num)
					{
						num = item2;
						fleeTargetType = FleeBehavior.FleeTargetType.Guard;
						fleeGoalBase = new FleeBehavior.FleeAgentTarget(base.Navigator, base.OwnerAgent, valueTuple2.Item2);
					}
				}
			}
			this._selectedGoal = fleeGoalBase;
			this.SelectedFleeTargetType = fleeTargetType;
			this._state = FleeBehavior.State.Flee;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00030748 File Offset: 0x0002E948
		private bool ShouldChangeTarget()
		{
			if (this._selectedFleeTargetType == FleeBehavior.FleeTargetType.Guard)
			{
				WorldPosition worldPosition = (this._selectedGoal as FleeBehavior.FleeAgentTarget).Savior.GetWorldPosition();
				WorldPosition worldPosition2 = base.OwnerAgent.GetWorldPosition();
				return this.GetPathScore(worldPosition2, worldPosition) <= 1f && this.IsThereASafePlaceToEscape();
			}
			if (this._selectedFleeTargetType != FleeBehavior.FleeTargetType.Indoor)
			{
				return true;
			}
			StandingPoint vacantStandingPointForAI = (this._selectedGoal as FleeBehavior.FleePassageTarget).EscapePortal.GetVacantStandingPointForAI(base.OwnerAgent);
			if (vacantStandingPointForAI == null)
			{
				return true;
			}
			WorldPosition worldPosition3 = base.OwnerAgent.GetWorldPosition();
			WorldPosition origin = vacantStandingPointForAI.GetUserFrameForAgent(base.OwnerAgent).Origin;
			return this.GetPathScore(worldPosition3, origin) <= 1f && this.IsThereASafePlaceToEscape();
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x000307FC File Offset: 0x0002E9FC
		private bool IsThereASafePlaceToEscape()
		{
			if (!this.GetAvailablePassageScores(1).Any<ValueTuple<float, Passage>>((ValueTuple<float, Passage> d) => d.Item1 > 1f))
			{
				return this.GetAvailableGuardScores(1).Any<ValueTuple<float, Agent>>((ValueTuple<float, Agent> d) => d.Item1 > 1f);
			}
			return true;
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00030864 File Offset: 0x0002EA64
		private List<ValueTuple<float, Passage>> GetAvailablePassageScores(int maxPaths = 10)
		{
			WorldPosition worldPosition = base.OwnerAgent.GetWorldPosition();
			List<ValueTuple<float, Passage>> list = new List<ValueTuple<float, Passage>>();
			List<ValueTuple<float, Passage>> list2 = new List<ValueTuple<float, Passage>>();
			List<ValueTuple<WorldPosition, Passage>> list3 = new List<ValueTuple<WorldPosition, Passage>>();
			if (this._missionAgentHandler.TownPassageProps != null)
			{
				foreach (UsableMachine usableMachine in this._missionAgentHandler.TownPassageProps)
				{
					StandingPoint vacantStandingPointForAI = usableMachine.GetVacantStandingPointForAI(base.OwnerAgent);
					Passage passage = usableMachine as Passage;
					if (vacantStandingPointForAI != null && passage != null)
					{
						WorldPosition origin = vacantStandingPointForAI.GetUserFrameForAgent(base.OwnerAgent).Origin;
						list3.Add(new ValueTuple<WorldPosition, Passage>(origin, passage));
					}
				}
			}
			list3 = list3.OrderBy<ValueTuple<WorldPosition, Passage>, float>((ValueTuple<WorldPosition, Passage> a) => base.OwnerAgent.Position.AsVec2.DistanceSquared(a.Item1.AsVec2)).ToList<ValueTuple<WorldPosition, Passage>>();
			foreach (ValueTuple<WorldPosition, Passage> valueTuple in list3)
			{
				WorldPosition item = valueTuple.Item1;
				if (item.IsValid && !(item.GetNearestNavMesh() == UIntPtr.Zero))
				{
					float pathScore = this.GetPathScore(worldPosition, item);
					ValueTuple<float, Passage> valueTuple2 = new ValueTuple<float, Passage>(pathScore, valueTuple.Item2);
					list.Add(valueTuple2);
					if (pathScore > 1f)
					{
						list2.Add(valueTuple2);
					}
					if (list2.Count >= maxPaths)
					{
						break;
					}
				}
			}
			if (list2.Count > 0)
			{
				return list2;
			}
			return list;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x000309E0 File Offset: 0x0002EBE0
		private List<ValueTuple<float, Agent>> GetAvailableGuardScores(int maxGuards = 5)
		{
			WorldPosition worldPosition = base.OwnerAgent.GetWorldPosition();
			List<ValueTuple<float, Agent>> list = new List<ValueTuple<float, Agent>>();
			List<ValueTuple<float, Agent>> list2 = new List<ValueTuple<float, Agent>>();
			List<Agent> list3 = new List<Agent>();
			foreach (Agent agent in base.OwnerAgent.Team.ActiveAgents)
			{
				CharacterObject characterObject;
				if ((characterObject = agent.Character as CharacterObject) != null && agent.IsAIControlled && agent.CurrentWatchState != Agent.WatchState.Alarmed && (characterObject.Occupation == Occupation.Soldier || characterObject.Occupation == Occupation.Guard || characterObject.Occupation == Occupation.PrisonGuard))
				{
					list3.Add(agent);
				}
			}
			list3 = list3.OrderBy<Agent, float>((Agent a) => base.OwnerAgent.Position.DistanceSquared(a.Position)).ToList<Agent>();
			foreach (Agent agent2 in list3)
			{
				WorldPosition worldPosition2 = agent2.GetWorldPosition();
				if (worldPosition2.IsValid)
				{
					float pathScore = this.GetPathScore(worldPosition, worldPosition2);
					ValueTuple<float, Agent> valueTuple = new ValueTuple<float, Agent>(pathScore, agent2);
					list.Add(valueTuple);
					if (pathScore > 1f)
					{
						list2.Add(valueTuple);
					}
					if (list2.Count >= maxGuards)
					{
						break;
					}
				}
			}
			if (list2.Count > 0)
			{
				return list2;
			}
			return list;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00030B4C File Offset: 0x0002ED4C
		protected override void OnActivate()
		{
			base.OnActivate();
			this._state = FleeBehavior.State.None;
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00030B5C File Offset: 0x0002ED5C
		private void Flee()
		{
			if (this._selectedGoal.IsGoalAchievable())
			{
				if (this._selectedGoal.IsGoalAchieved())
				{
					this._selectedGoal.TargetReached();
					FleeBehavior.FleeTargetType selectedFleeTargetType = this.SelectedFleeTargetType;
					if (selectedFleeTargetType == FleeBehavior.FleeTargetType.Guard)
					{
						this._complainToGuardTimer = new BasicMissionTimer();
						this._state = FleeBehavior.State.Complain;
						return;
					}
					if (selectedFleeTargetType == FleeBehavior.FleeTargetType.Cover && this._reconsiderFleeTargetTimer.ElapsedTime > 0.5f)
					{
						this._state = FleeBehavior.State.LookForPlace;
						this._reconsiderFleeTargetTimer.Reset();
						return;
					}
				}
				else
				{
					if (this.SelectedFleeTargetType == FleeBehavior.FleeTargetType.Guard)
					{
						this._selectedGoal.GoToTarget();
					}
					if (this._reconsiderFleeTargetTimer.ElapsedTime > 1f)
					{
						this._reconsiderFleeTargetTimer.Reset();
						if (this.ShouldChangeTarget())
						{
							this._state = FleeBehavior.State.LookForPlace;
							return;
						}
					}
				}
			}
			else
			{
				this._state = FleeBehavior.State.LookForPlace;
			}
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00030C23 File Offset: 0x0002EE23
		private void BeAfraid()
		{
			this._scareTimer = new BasicMissionTimer();
			this._scareTime = 0.5f + MBRandom.RandomFloat * 0.5f;
			this._state = FleeBehavior.State.Afraid;
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00030C4E File Offset: 0x0002EE4E
		public override string GetDebugInfo()
		{
			return "Flee " + this._state;
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00030C65 File Offset: 0x0002EE65
		public override float GetAvailability(bool isSimulation)
		{
			if (base.Mission.CurrentTime < 3f)
			{
				return 0f;
			}
			if (!MissionFightHandler.IsAgentAggressive(base.OwnerAgent))
			{
				return 0.9f;
			}
			return 0.1f;
		}

		// Token: 0x040003C3 RID: 963
		public const float ScoreThreshold = 1f;

		// Token: 0x040003C4 RID: 964
		public const float DangerDistance = 5f;

		// Token: 0x040003C5 RID: 965
		public const float ImmediateDangerDistance = 2f;

		// Token: 0x040003C6 RID: 966
		public const float DangerDistanceSquared = 25f;

		// Token: 0x040003C7 RID: 967
		public const float ImmediateDangerDistanceSquared = 4f;

		// Token: 0x040003C8 RID: 968
		private readonly MissionAgentHandler _missionAgentHandler;

		// Token: 0x040003C9 RID: 969
		private readonly MissionFightHandler _missionFightHandler;

		// Token: 0x040003CA RID: 970
		private FleeBehavior.State _state;

		// Token: 0x040003CB RID: 971
		private readonly BasicMissionTimer _reconsiderFleeTargetTimer;

		// Token: 0x040003CC RID: 972
		private const float ReconsiderImmobilizedFleeTargetTime = 0.5f;

		// Token: 0x040003CD RID: 973
		private const float ReconsiderDefaultFleeTargetTime = 1f;

		// Token: 0x040003CE RID: 974
		private FleeBehavior.FleeGoalBase _selectedGoal;

		// Token: 0x040003CF RID: 975
		private BasicMissionTimer _scareTimer;

		// Token: 0x040003D0 RID: 976
		private float _scareTime;

		// Token: 0x040003D1 RID: 977
		private BasicMissionTimer _complainToGuardTimer;

		// Token: 0x040003D2 RID: 978
		private const float ComplainToGuardTime = 2f;

		// Token: 0x040003D3 RID: 979
		private FleeBehavior.FleeTargetType _selectedFleeTargetType;

		// Token: 0x020001AB RID: 427
		private abstract class FleeGoalBase
		{
			// Token: 0x06000F23 RID: 3875 RVA: 0x00067448 File Offset: 0x00065648
			protected FleeGoalBase(AgentNavigator navigator, Agent ownerAgent)
			{
				this._navigator = navigator;
				this._ownerAgent = ownerAgent;
			}

			// Token: 0x06000F24 RID: 3876
			public abstract void TargetReached();

			// Token: 0x06000F25 RID: 3877
			public abstract void GoToTarget();

			// Token: 0x06000F26 RID: 3878
			public abstract bool IsGoalAchievable();

			// Token: 0x06000F27 RID: 3879
			public abstract bool IsGoalAchieved();

			// Token: 0x040007EA RID: 2026
			protected readonly AgentNavigator _navigator;

			// Token: 0x040007EB RID: 2027
			protected readonly Agent _ownerAgent;
		}

		// Token: 0x020001AC RID: 428
		private class FleeAgentTarget : FleeBehavior.FleeGoalBase
		{
			// Token: 0x1700013B RID: 315
			// (get) Token: 0x06000F28 RID: 3880 RVA: 0x0006745E File Offset: 0x0006565E
			// (set) Token: 0x06000F29 RID: 3881 RVA: 0x00067466 File Offset: 0x00065666
			public Agent Savior { get; private set; }

			// Token: 0x06000F2A RID: 3882 RVA: 0x0006746F File Offset: 0x0006566F
			public FleeAgentTarget(AgentNavigator navigator, Agent ownerAgent, Agent savior)
				: base(navigator, ownerAgent)
			{
				this.Savior = savior;
			}

			// Token: 0x06000F2B RID: 3883 RVA: 0x00067480 File Offset: 0x00065680
			public override void GoToTarget()
			{
				this._navigator.SetTargetFrame(this.Savior.GetWorldPosition(), this.Savior.Frame.rotation.f.AsVec2.RotationInRadians, 0.2f, 0.02f, Agent.AIScriptedFrameFlags.NoAttack | Agent.AIScriptedFrameFlags.NeverSlowDown, false);
			}

			// Token: 0x06000F2C RID: 3884 RVA: 0x000674D8 File Offset: 0x000656D8
			public override bool IsGoalAchievable()
			{
				return this.Savior.GetWorldPosition().GetNearestNavMesh() != UIntPtr.Zero && this._navigator.TargetPosition.IsValid && this.Savior.IsActive() && this.Savior.CurrentWatchState != Agent.WatchState.Alarmed;
			}

			// Token: 0x06000F2D RID: 3885 RVA: 0x0006753C File Offset: 0x0006573C
			public override bool IsGoalAchieved()
			{
				return this._navigator.TargetPosition.IsValid && this._navigator.TargetPosition.GetGroundVec3().Distance(this._ownerAgent.Position) <= this._ownerAgent.GetInteractionDistanceToUsable(this.Savior);
			}

			// Token: 0x06000F2E RID: 3886 RVA: 0x0006759C File Offset: 0x0006579C
			public override void TargetReached()
			{
				this._ownerAgent.SetActionChannel(0, in ActionIndexCache.act_cheer_1, true, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
				this._ownerAgent.SetActionChannel(1, in ActionIndexCache.act_none, true, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
				this._ownerAgent.DisableScriptedMovement();
				this.Savior.DisableScriptedMovement();
				this.Savior.SetLookAgent(this._ownerAgent);
				this._ownerAgent.SetLookAgent(this.Savior);
			}
		}

		// Token: 0x020001AD RID: 429
		private class FleePassageTarget : FleeBehavior.FleeGoalBase
		{
			// Token: 0x1700013C RID: 316
			// (get) Token: 0x06000F2F RID: 3887 RVA: 0x0006764D File Offset: 0x0006584D
			// (set) Token: 0x06000F30 RID: 3888 RVA: 0x00067655 File Offset: 0x00065855
			public Passage EscapePortal { get; private set; }

			// Token: 0x06000F31 RID: 3889 RVA: 0x0006765E File Offset: 0x0006585E
			public FleePassageTarget(AgentNavigator navigator, Agent ownerAgent, Passage escapePortal)
				: base(navigator, ownerAgent)
			{
				this.EscapePortal = escapePortal;
			}

			// Token: 0x06000F32 RID: 3890 RVA: 0x0006766F File Offset: 0x0006586F
			public override void GoToTarget()
			{
				this._navigator.SetTarget(this.EscapePortal, false, Agent.AIScriptedFrameFlags.None);
			}

			// Token: 0x06000F33 RID: 3891 RVA: 0x00067684 File Offset: 0x00065884
			public override bool IsGoalAchievable()
			{
				return this.EscapePortal.GetVacantStandingPointForAI(this._ownerAgent) != null && !this.EscapePortal.IsDestroyed;
			}

			// Token: 0x06000F34 RID: 3892 RVA: 0x000676AC File Offset: 0x000658AC
			public override bool IsGoalAchieved()
			{
				StandingPoint vacantStandingPointForAI = this.EscapePortal.GetVacantStandingPointForAI(this._ownerAgent);
				return vacantStandingPointForAI != null && vacantStandingPointForAI.IsUsableByAgent(this._ownerAgent);
			}

			// Token: 0x06000F35 RID: 3893 RVA: 0x000676DC File Offset: 0x000658DC
			public override void TargetReached()
			{
			}
		}

		// Token: 0x020001AE RID: 430
		private class FleePositionTarget : FleeBehavior.FleeGoalBase
		{
			// Token: 0x1700013D RID: 317
			// (get) Token: 0x06000F36 RID: 3894 RVA: 0x000676DE File Offset: 0x000658DE
			// (set) Token: 0x06000F37 RID: 3895 RVA: 0x000676E6 File Offset: 0x000658E6
			public Vec3 Position { get; private set; }

			// Token: 0x06000F38 RID: 3896 RVA: 0x000676EF File Offset: 0x000658EF
			public FleePositionTarget(AgentNavigator navigator, Agent ownerAgent, Vec3 position)
				: base(navigator, ownerAgent)
			{
				this.Position = position;
			}

			// Token: 0x06000F39 RID: 3897 RVA: 0x00067700 File Offset: 0x00065900
			public override void GoToTarget()
			{
			}

			// Token: 0x06000F3A RID: 3898 RVA: 0x00067704 File Offset: 0x00065904
			public override bool IsGoalAchievable()
			{
				return this._navigator.TargetPosition.IsValid;
			}

			// Token: 0x06000F3B RID: 3899 RVA: 0x00067724 File Offset: 0x00065924
			public override bool IsGoalAchieved()
			{
				return this._navigator.TargetPosition.IsValid && this._navigator.IsTargetReached();
			}

			// Token: 0x06000F3C RID: 3900 RVA: 0x00067753 File Offset: 0x00065953
			public override void TargetReached()
			{
			}
		}

		// Token: 0x020001AF RID: 431
		private class FleeCoverTarget : FleeBehavior.FleeGoalBase
		{
			// Token: 0x06000F3D RID: 3901 RVA: 0x00067755 File Offset: 0x00065955
			public FleeCoverTarget(AgentNavigator navigator, Agent ownerAgent)
				: base(navigator, ownerAgent)
			{
			}

			// Token: 0x06000F3E RID: 3902 RVA: 0x0006775F File Offset: 0x0006595F
			public override void GoToTarget()
			{
				this._ownerAgent.DisableScriptedMovement();
			}

			// Token: 0x06000F3F RID: 3903 RVA: 0x0006776C File Offset: 0x0006596C
			public override bool IsGoalAchievable()
			{
				return true;
			}

			// Token: 0x06000F40 RID: 3904 RVA: 0x0006776F File Offset: 0x0006596F
			public override bool IsGoalAchieved()
			{
				return true;
			}

			// Token: 0x06000F41 RID: 3905 RVA: 0x00067772 File Offset: 0x00065972
			public override void TargetReached()
			{
			}
		}

		// Token: 0x020001B0 RID: 432
		private enum State
		{
			// Token: 0x040007F0 RID: 2032
			None,
			// Token: 0x040007F1 RID: 2033
			Afraid,
			// Token: 0x040007F2 RID: 2034
			LookForPlace,
			// Token: 0x040007F3 RID: 2035
			Flee,
			// Token: 0x040007F4 RID: 2036
			Complain
		}

		// Token: 0x020001B1 RID: 433
		private enum FleeTargetType
		{
			// Token: 0x040007F6 RID: 2038
			Indoor,
			// Token: 0x040007F7 RID: 2039
			Guard,
			// Token: 0x040007F8 RID: 2040
			Cover
		}
	}
}
