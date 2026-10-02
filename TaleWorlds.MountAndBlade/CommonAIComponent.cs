using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000106 RID: 262
	public class CommonAIComponent : AgentComponent
	{
		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000D31 RID: 3377 RVA: 0x00017C38 File Offset: 0x00015E38
		// (set) Token: 0x06000D32 RID: 3378 RVA: 0x00017C40 File Offset: 0x00015E40
		public bool IsPanicked { get; private set; }

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000D33 RID: 3379 RVA: 0x00017C49 File Offset: 0x00015E49
		// (set) Token: 0x06000D34 RID: 3380 RVA: 0x00017C51 File Offset: 0x00015E51
		public bool IsRetreating { get; private set; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000D35 RID: 3381 RVA: 0x00017C5A File Offset: 0x00015E5A
		// (set) Token: 0x06000D36 RID: 3382 RVA: 0x00017C62 File Offset: 0x00015E62
		public int ReservedRiderAgentIndex { get; private set; }

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000D37 RID: 3383 RVA: 0x00017C6B File Offset: 0x00015E6B
		public float InitialMorale
		{
			get
			{
				return this._initialMorale;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000D38 RID: 3384 RVA: 0x00017C73 File Offset: 0x00015E73
		public float RecoveryMorale
		{
			get
			{
				return this._recoveryMorale;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000D39 RID: 3385 RVA: 0x00017C7B File Offset: 0x00015E7B
		// (set) Token: 0x06000D3A RID: 3386 RVA: 0x00017C83 File Offset: 0x00015E83
		public float Morale
		{
			get
			{
				return this._morale;
			}
			set
			{
				this._morale = MBMath.ClampFloat(value, 0f, 100f);
			}
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00017C9C File Offset: 0x00015E9C
		public CommonAIComponent(Agent agent)
			: base(agent)
		{
			this._fadeOutTimer = new Timer(Mission.Current.CurrentTime, 0.5f + MBRandom.RandomFloat * 0.1f, true);
			float num = agent.Monster.BodyCapsuleRadius * 2f * 7.5f;
			this._retreatDistanceSquared = num * num;
			this.ReservedRiderAgentIndex = -1;
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00017D0A File Offset: 0x00015F0A
		public override void Initialize()
		{
			base.Initialize();
			this.InitializeMorale();
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00017D18 File Offset: 0x00015F18
		private void InitializeMorale()
		{
			int num = MBRandom.RandomInt(30);
			float num2 = this.Agent.Components.Sum<AgentComponent>((AgentComponent c) => c.GetMoraleAddition());
			float num3 = 35f + (float)num + num2;
			num3 = MissionGameModels.Current.BattleMoraleModel.GetEffectiveInitialMorale(this.Agent, num3);
			num3 = MBMath.ClampFloat(num3, 15f, 100f);
			this._initialMorale = num3;
			this._recoveryMorale = this._initialMorale * 0.5f;
			this.Morale = this._initialMorale;
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00017DB8 File Offset: 0x00015FB8
		public override void OnTickParallel(float dt)
		{
			if (this.Agent.Mission.AllowAiTicking && this.Agent.IsAIControlled)
			{
				if (!this.IsRetreating && this._morale < 0.01f)
				{
					if (this.CanPanic())
					{
						this._panicTriggered = true;
					}
					else
					{
						this.Morale = 0.01f;
					}
				}
				if (!this.IsPanicked && !this._panicTriggered && this._morale < this._recoveryMorale)
				{
					this.Morale = Math.Min(this._morale + 0.4f * dt, this._recoveryMorale);
				}
				if (this._fadeOutTimer.Check(Mission.Current.CurrentTime) && Mission.Current.CanAgentRout(this.Agent) && !this.Agent.IsFadingOut())
				{
					Vec3 position = this.Agent.Position;
					WorldPosition retreatPos = this.Agent.GetRetreatPos();
					this._retreatDistanceSquared = this._fadeOutTimer.Duration * this.Agent.Velocity.AsVec2.LengthSquared + 2f * this.Agent.Monster.BodyCapsuleRadius;
					if ((retreatPos.AsVec2.IsValid && retreatPos.AsVec2.DistanceSquared(position.AsVec2) < this._retreatDistanceSquared && retreatPos.GetGroundVec3MT().DistanceSquared(position) < this._retreatDistanceSquared) || !this.Agent.Mission.IsPositionInsideBoundaries(position.AsVec2) || position.DistanceSquared(this.Agent.Mission.GetClosestBoundaryPosition(position.AsVec2).ToVec3(0f)) < this._retreatDistanceSquared)
					{
						this.Agent.StartFadingOut();
					}
				}
				if (this.IsPanicked && this.Agent.Mission.MissionEnded)
				{
					MissionResult missionResult = this.Agent.Mission.MissionResult;
					if (this.Agent.Team != null && missionResult != null && ((missionResult.PlayerVictory && (this.Agent.Team.IsPlayerTeam || this.Agent.Team.IsPlayerAlly)) || (missionResult.PlayerDefeated && !this.Agent.Team.IsPlayerTeam && !this.Agent.Team.IsPlayerAlly)) && this.Agent != Agent.Main && this.Agent.IsActive())
					{
						this.StopRetreating();
					}
				}
			}
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0001804E File Offset: 0x0001624E
		public override void OnTick(float dt)
		{
			if (this._panicTriggered)
			{
				this._panicTriggered = false;
				this.Panic();
			}
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00018065 File Offset: 0x00016265
		public void Panic()
		{
			this.Agent.SetAlarmState(Agent.AIStateFlag.Alarmed);
			if (!this.IsPanicked)
			{
				this.IsPanicked = true;
				this.Agent.Mission.OnAgentPanicked(this.Agent);
			}
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0001809C File Offset: 0x0001629C
		public void Retreat(bool useCachingSystem = false)
		{
			if (!this.IsRetreating)
			{
				this.IsRetreating = true;
				this.Agent.EnforceShieldUsage(Agent.UsageDirection.None);
				WorldPosition worldPosition = WorldPosition.Invalid;
				if (useCachingSystem)
				{
					worldPosition = this.Agent.Formation.RetreatPositionCache.GetRetreatPositionFromCache(this.Agent.Position.AsVec2);
				}
				if (!worldPosition.IsValid)
				{
					worldPosition = this.Agent.Mission.GetClosestFleePositionForAgent(this.Agent);
					if (useCachingSystem)
					{
						this.Agent.Formation.RetreatPositionCache.AddNewPositionToCache(this.Agent.Position.AsVec2, worldPosition);
					}
				}
				this.Agent.Retreat(worldPosition);
			}
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x00018154 File Offset: 0x00016354
		public void StopRetreating()
		{
			if (!this.IsRetreating)
			{
				return;
			}
			this.IsRetreating = false;
			this.IsPanicked = false;
			float num = MathF.Max(0.02f, this.Morale);
			this.Agent.SetMorale(num);
			this.Agent.StopRetreating();
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x000181A0 File Offset: 0x000163A0
		public bool CanPanic()
		{
			if (!MissionGameModels.Current.BattleMoraleModel.CanPanicDueToMorale(this.Agent))
			{
				return false;
			}
			TeamAISiegeComponent teamAISiegeComponent;
			if (Mission.Current.IsSiegeBattle && this.Agent.Team.Side == BattleSideEnum.Attacker && (teamAISiegeComponent = this.Agent.Team.TeamAI as TeamAISiegeComponent) != null)
			{
				int currentNavigationFaceId = this.Agent.GetCurrentNavigationFaceId();
				if (currentNavigationFaceId % 10 == 1)
				{
					return false;
				}
				if (teamAISiegeComponent.IsPrimarySiegeWeaponNavmeshFaceId(currentNavigationFaceId))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x0001821F File Offset: 0x0001641F
		public override void OnHit(Agent affectorAgent, int damage, in MissionWeapon affectorWeapon, in Blow b, in AttackCollisionData collisionData)
		{
			base.OnHit(affectorAgent, damage, in affectorWeapon, in b, in collisionData);
			if (damage >= 1 && this.Agent.IsMount && this.Agent.IsAIControlled && this.Agent.RiderAgent == null)
			{
				this.Panic();
			}
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x00018260 File Offset: 0x00016460
		public override void OnAgentRemoved()
		{
			base.OnAgentRemoved();
			if (this.Agent.IsMount && this.Agent.RiderAgent == null)
			{
				Agent agent = this.FindReservingAgent();
				if (agent != null)
				{
					agent.HumanAIComponent.UnreserveMount(this.Agent);
				}
			}
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x000182A8 File Offset: 0x000164A8
		public override void OnComponentRemoved()
		{
			base.OnComponentRemoved();
			if (this.Agent.IsMount && this.Agent.RiderAgent == null)
			{
				Agent agent = this.FindReservingAgent();
				if (agent != null)
				{
					agent.HumanAIComponent.UnreserveMount(this.Agent);
				}
			}
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x000182F0 File Offset: 0x000164F0
		internal void OnMountReserved(int riderAgentIndex)
		{
			this.ReservedRiderAgentIndex = riderAgentIndex;
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x000182F9 File Offset: 0x000164F9
		internal void OnMountUnreserved()
		{
			this.ReservedRiderAgentIndex = -1;
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x00018304 File Offset: 0x00016504
		private Agent FindReservingAgent()
		{
			Agent agent = null;
			if (this.ReservedRiderAgentIndex >= 0)
			{
				foreach (Agent agent2 in Mission.Current.Agents)
				{
					if (agent2.Index == this.ReservedRiderAgentIndex)
					{
						agent = agent2;
						break;
					}
				}
			}
			return agent;
		}

		// Token: 0x040002F1 RID: 753
		public const float MoraleThresholdForPanicking = 0.01f;

		// Token: 0x040002F2 RID: 754
		private const float MaxRecoverableMoraleMultiplier = 0.5f;

		// Token: 0x040002F3 RID: 755
		private const float MoraleRecoveryPerSecond = 0.4f;

		// Token: 0x040002F7 RID: 759
		private float _recoveryMorale;

		// Token: 0x040002F8 RID: 760
		private float _initialMorale;

		// Token: 0x040002F9 RID: 761
		private float _morale = 50f;

		// Token: 0x040002FA RID: 762
		private bool _panicTriggered;

		// Token: 0x040002FB RID: 763
		private readonly Timer _fadeOutTimer;

		// Token: 0x040002FC RID: 764
		private float _retreatDistanceSquared;
	}
}
