using System;
using System.Linq;
using SandBox.Objects;
using SandBox.Objects.Usables;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000AF RID: 175
	public class PatrolAgentBehavior : AgentBehavior
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x00031F44 File Offset: 0x00030144
		private int NextPatrolIndex
		{
			get
			{
				int num = this._currentPatrolIndex + 1;
				if (num >= this._patrolPoints.Length)
				{
					num = 0;
				}
				return num;
			}
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x00031F68 File Offset: 0x00030168
		public PatrolAgentBehavior(AgentBehaviorGroup behaviorGroup)
			: base(behaviorGroup)
		{
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00031F74 File Offset: 0x00030174
		public void SetDynamicPatrolArea(GameEntity parentPatrolPoint)
		{
			this._patrolPoints = new PatrolPoint[parentPatrolPoint.ChildCount];
			PatrolPoint[] array = new PatrolPoint[parentPatrolPoint.ChildCount];
			for (int i = 0; i < parentPatrolPoint.ChildCount; i++)
			{
				array[i] = parentPatrolPoint.GetChild(i).GetChild(0).GetFirstScriptOfType<PatrolPoint>();
			}
			this._patrolPoints = array.OrderBy<PatrolPoint, int>((PatrolPoint x) => x.Index).ToArray<PatrolPoint>();
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00031FF4 File Offset: 0x000301F4
		protected override void OnActivate()
		{
			base.OwnerAgent.SetMaximumSpeedLimit(1.05f, false);
			this._infiniteWaitPointReached = false;
			PatrolPoint patrolPoint = null;
			float num = float.MaxValue;
			foreach (PatrolPoint patrolPoint2 in this._patrolPoints)
			{
				float num2 = patrolPoint2.GameEntity.GlobalPosition.DistanceSquared(base.OwnerAgent.Position);
				if (num2 < num)
				{
					num = num2;
					patrolPoint = patrolPoint2;
				}
			}
			this._currentPatrolIndex = this._patrolPoints.IndexOf(patrolPoint);
			this.MoveAgentToThePoint(this._currentPatrolIndex, true, false);
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0003208C File Offset: 0x0003028C
		protected override void OnDeactivate()
		{
			this._waitTimer = null;
			if (base.OwnerAgent.CurrentlyUsedGameObject != null)
			{
				base.OwnerAgent.StopUsingGameObjectMT(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
			}
			base.Navigator.SetTarget(null, false, Agent.AIScriptedFrameFlags.None);
			if (this._patrolPoints[this._currentPatrolIndex].GameEntity.GetFirstScriptOfType<PatrolPoint>().PatrollingSpeed != -1f || base.OwnerAgent.GetMaximumSpeedLimit().Equals(1.05f))
			{
				base.OwnerAgent.SetMaximumSpeedLimit(-1f, false);
			}
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0003211C File Offset: 0x0003031C
		public override void Tick(float dt, bool isSimulation)
		{
			if (!this._infiniteWaitPointReached && base.OwnerAgent.CurrentlyUsedGameObject != null)
			{
				if (this._waitTimer == null)
				{
					PatrolPoint patrolPoint;
					if ((patrolPoint = base.OwnerAgent.CurrentlyUsedGameObject as PatrolPoint) != null)
					{
						if (patrolPoint.IsInfiniteWaitPoint)
						{
							this._infiniteWaitPointReached = true;
							return;
						}
						float num = (float)patrolPoint.WaitDuration + MBRandom.RandomFloatRanged((float)(-(float)patrolPoint.WaitDeviation), (float)patrolPoint.WaitDeviation);
						if (num == 0f)
						{
							this.MoveAgentToNextPatrolPoint(isSimulation);
							return;
						}
						this._waitTimer = new Timer(base.Mission.CurrentTime, num, true);
						return;
					}
				}
				else if (this._waitTimer.Check(base.Mission.CurrentTime))
				{
					this.MoveAgentToNextPatrolPoint(isSimulation);
					return;
				}
			}
			else
			{
				if (base.Navigator.IsTargetReached())
				{
					base.Navigator.ClearTarget();
				}
				if (base.Navigator.TargetUsableMachine == null && !base.Navigator.TargetPosition.IsValid)
				{
					this.MoveAgentToNextPatrolPoint(isSimulation);
				}
			}
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0003221A File Offset: 0x0003041A
		public override float GetAvailability(bool isSimulation)
		{
			if (!base.OwnerAgent.IsAlarmed() && !base.OwnerAgent.IsPatrollingCautious())
			{
				return 0.5f;
			}
			return 0f;
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00032244 File Offset: 0x00030444
		private void MoveAgentToNextPatrolPoint(bool isSimulation)
		{
			this._waitTimer = null;
			PatrolPoint firstScriptOfType = this._patrolPoints[this._currentPatrolIndex].GameEntity.GetFirstScriptOfType<PatrolPoint>();
			base.OwnerAgent.SetMaximumSpeedLimit((firstScriptOfType.PatrollingSpeed == -1f) ? 1.05f : firstScriptOfType.PatrollingSpeed, false);
			this.MoveAgentToThePoint(this.NextPatrolIndex, false, isSimulation);
			this._currentPatrolIndex = this.NextPatrolIndex;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x000322B4 File Offset: 0x000304B4
		private void MoveAgentToThePoint(int pointIndex, bool correctRotation, bool isSimulation)
		{
			WeakGameEntity gameEntity = this._patrolPoints[pointIndex].GameEntity;
			PatrolPoint firstScriptOfType = gameEntity.GetFirstScriptOfType<PatrolPoint>();
			if (firstScriptOfType.WaitDuration == 0 && firstScriptOfType.WaitDeviation == 0)
			{
				WorldPosition worldPosition = new WorldPosition(gameEntity.Scene, gameEntity.GlobalPosition);
				base.Navigator.SetTargetFrame(worldPosition, gameEntity.GetFrame().rotation.f.RotationX, correctRotation ? 1f : (-1f), correctRotation ? 0.8f : (-10f), Agent.AIScriptedFrameFlags.None, false);
				return;
			}
			base.Navigator.SetTarget(gameEntity.Parent.GetFirstScriptOfType<UsablePlace>(), isSimulation, Agent.AIScriptedFrameFlags.NeverSlowDown | Agent.AIScriptedFrameFlags.DoNotRun);
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00032360 File Offset: 0x00030560
		public override string GetDebugInfo()
		{
			return "Patrol Agent Behavior";
		}

		// Token: 0x040003ED RID: 1005
		private const float DefaultPatrollingSpeed = 1.05f;

		// Token: 0x040003EE RID: 1006
		private PatrolPoint[] _patrolPoints;

		// Token: 0x040003EF RID: 1007
		private int _currentPatrolIndex;

		// Token: 0x040003F0 RID: 1008
		private Timer _waitTimer;

		// Token: 0x040003F1 RID: 1009
		private bool _infiniteWaitPointReached;
	}
}
