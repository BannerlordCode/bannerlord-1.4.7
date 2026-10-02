using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.MountAndBlade.DividableTasks;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200015E RID: 350
	public abstract class RangedSiegeWeaponAi : UsableMachineAIBase
	{
		// Token: 0x06001243 RID: 4675 RVA: 0x000396F8 File Offset: 0x000378F8
		public RangedSiegeWeaponAi(RangedSiegeWeapon rangedSiegeWeapon)
			: base(rangedSiegeWeapon)
		{
			this._threatSeeker = new RangedSiegeWeaponAi.ThreatSeeker(rangedSiegeWeapon);
			((RangedSiegeWeapon)this.UsableMachine).OnReloadDone += this.FindNextTarget;
			this._delayTimer = this._delayDuration;
			this._targetEvaluationTimer = new Timer(Mission.Current.CurrentTime, 0.5f, true);
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x00039766 File Offset: 0x00037966
		public void InitializeThreatSeeker()
		{
			this._threatSeeker.InitializeTargetableObjects();
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x00039774 File Offset: 0x00037974
		protected override void OnTick(Agent agentToCompareTo, Formation formationToCompareTo, Team potentialUsersTeam, float dt)
		{
			base.OnTick(agentToCompareTo, formationToCompareTo, potentialUsersTeam, dt);
			if (this.UsableMachine.PilotAgent != null && this.UsableMachine.PilotAgent.IsAIControlled)
			{
				RangedSiegeWeapon rangedSiegeWeapon = this.UsableMachine as RangedSiegeWeapon;
				if (rangedSiegeWeapon.State == RangedSiegeWeapon.WeaponState.WaitingAfterShooting && rangedSiegeWeapon.PilotAgent != null && rangedSiegeWeapon.PilotAgent.IsAIControlled)
				{
					rangedSiegeWeapon.AiRequestsManualReload();
				}
				this.UpdateAim(rangedSiegeWeapon, dt);
			}
			this.AfterTick(agentToCompareTo, formationToCompareTo, potentialUsersTeam, dt);
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x000397F0 File Offset: 0x000379F0
		protected virtual void UpdateAim(RangedSiegeWeapon rangedSiegeWeapon, float dt)
		{
			if (this._threatSeeker.UpdateThreatSeekerTask() && dt > 0f && this._target == null && rangedSiegeWeapon.State == RangedSiegeWeapon.WeaponState.Idle)
			{
				if (this._delayTimer <= 0f)
				{
					this.FindNextTarget();
				}
				this._delayTimer -= dt;
			}
			if (this._target != null)
			{
				if (this._target.Agent != null && !this._target.Agent.IsActive())
				{
					this._target = null;
					return;
				}
				if (rangedSiegeWeapon.State == RangedSiegeWeapon.WeaponState.Idle && rangedSiegeWeapon.UserCountNotInStruckAction > 0)
				{
					if (DebugSiegeBehavior.ToggleTargetDebug && this.UsableMachine.PilotAgent != null)
					{
						this._target.ComputeGlobalTargetingBoundingBoxMinMax();
						Vec3 targetingPosition = this._target.TargetingPosition;
					}
					if (this._targetEvaluationTimer.Check(Mission.Current.CurrentTime) && !((RangedSiegeWeapon)this.UsableMachine).CanShootAtThreat(this._target, 5))
					{
						this._cannotShootCounter++;
					}
					if (this._cannotShootCounter >= 4)
					{
						this._target = null;
						this.SetTargetingTimer();
						this._cannotShootCounter = 0;
						return;
					}
					if (rangedSiegeWeapon.AimAtThreat(this._target) && rangedSiegeWeapon.PilotAgent != null)
					{
						this._delayTimer -= dt;
						if (this._delayTimer <= 0f && rangedSiegeWeapon.CanShootAtThreat(this._target, 5))
						{
							rangedSiegeWeapon.AiRequestsShoot();
							this._target = null;
							this.SetTargetingTimer();
							this._cannotShootCounter = 0;
							this._targetEvaluationTimer.Reset(Mission.Current.CurrentTime);
							return;
						}
					}
				}
				else
				{
					this._targetEvaluationTimer.Reset(Mission.Current.CurrentTime);
				}
			}
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x0003999F File Offset: 0x00037B9F
		private void SetTargetFromThreatSeeker()
		{
			this._target = this._threatSeeker.PrepareTargetFromTask();
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x000399B2 File Offset: 0x00037BB2
		public void FindNextTarget()
		{
			if (this.UsableMachine.PilotAgent != null && this.UsableMachine.PilotAgent.IsAIControlled)
			{
				this._threatSeeker.PrepareThreatSeekerTask(new Action(this.SetTargetFromThreatSeeker));
				this.SetTargetingTimer();
			}
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x000399F0 File Offset: 0x00037BF0
		private void AfterTick(Agent agentToCompareTo, Formation formationToCompareTo, Team potentialUsersTeam, float dt)
		{
			if ((dt <= 0f || (agentToCompareTo != null && this.UsableMachine.PilotAgent != agentToCompareTo) || (formationToCompareTo != null && (this.UsableMachine.PilotAgent == null || !this.UsableMachine.PilotAgent.IsAIControlled || this.UsableMachine.PilotAgent.Formation != formationToCompareTo))) && this.UsableMachine.PilotAgent == null)
			{
				this._threatSeeker.Release();
				this._target = null;
			}
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x00039A6B File Offset: 0x00037C6B
		private void SetTargetingTimer()
		{
			this._delayTimer = this._delayDuration + MBRandom.RandomFloat * 0.5f;
		}

		// Token: 0x04000476 RID: 1142
		private const float TargetEvaluationDelay = 0.5f;

		// Token: 0x04000477 RID: 1143
		private const int MaxTargetEvaluationCount = 4;

		// Token: 0x04000478 RID: 1144
		public const string ForceTargetEntityTag = "attackMe";

		// Token: 0x04000479 RID: 1145
		private readonly RangedSiegeWeaponAi.ThreatSeeker _threatSeeker;

		// Token: 0x0400047A RID: 1146
		private Threat _target;

		// Token: 0x0400047B RID: 1147
		private float _delayTimer;

		// Token: 0x0400047C RID: 1148
		private float _delayDuration = 1f;

		// Token: 0x0400047D RID: 1149
		private int _cannotShootCounter;

		// Token: 0x0400047E RID: 1150
		private readonly Timer _targetEvaluationTimer;

		// Token: 0x02000486 RID: 1158
		public class ThreatSeeker
		{
			// Token: 0x0600392C RID: 14636 RVA: 0x000E85BC File Offset: 0x000E67BC
			public ThreatSeeker(RangedSiegeWeapon weapon)
			{
				this.Weapon = weapon;
				this.WeaponPositions = new List<Vec3> { this.Weapon.GameEntity.GlobalPosition };
				this._targetAgent = null;
				this._getMostDangerousThreat = new FindMostDangerousThreat(null);
			}

			// Token: 0x0600392D RID: 14637 RVA: 0x000E8610 File Offset: 0x000E6810
			public void InitializeTargetableObjects()
			{
				IEnumerable<MissionObject> enumerable = Mission.Current.ActiveMissionObjects.WhereQ<MissionObject>((MissionObject mo) => mo is ITargetable);
				this._potentialTargetObjects = (from to in enumerable.WhereQ<MissionObject>(delegate(MissionObject to)
					{
						ITargetable targetable;
						return (targetable = to as ITargetable) != null && targetable.IsDestructable() && targetable.GetTargetEntity() != null;
					})
					select to as ITargetable).ToList<ITargetable>();
				this._referencePositions = enumerable.OfType<ICastleKeyPosition>().ToList<ICastleKeyPosition>();
			}

			// Token: 0x0600392E RID: 14638 RVA: 0x000E86B4 File Offset: 0x000E68B4
			public Threat PrepareTargetFromTask()
			{
				Agent agent2;
				this._currentThreat = this._getMostDangerousThreat.GetResult(out agent2);
				if (this._currentThreat != null && this._currentThreat.TargetableObject == null)
				{
					this._currentThreat.Agent = this._targetAgent;
					if (this._targetAgent == null || !this._targetAgent.IsActive() || this._targetAgent.Formation != this._currentThreat.Formation || !this.Weapon.CanShootAtAgent(this._targetAgent, 5))
					{
						this._targetAgent = agent2;
						float selectedAgentScore = float.MaxValue;
						Agent selectedAgent = this._targetAgent;
						Action<Agent> action = delegate(Agent agent)
						{
							float num = agent.Position.DistanceSquared(this.Weapon.GameEntity.GlobalPosition) * (MBRandom.RandomFloat * 0.2f + 0.8f);
							if (agent == this._targetAgent)
							{
								num *= 0.5f;
							}
							if (selectedAgentScore > num && this.Weapon.CanShootAtAgent(agent, 5))
							{
								selectedAgent = agent;
								selectedAgentScore = num;
							}
						};
						if (agent2.Detachment == null)
						{
							this._currentThreat.Formation.ApplyActionOnEachAttachedUnit(action);
						}
						else
						{
							this._currentThreat.Formation.ApplyActionOnEachDetachedUnit(action);
						}
						this._targetAgent = selectedAgent ?? this._currentThreat.Formation.GetUnitWithIndex(MBRandom.RandomInt(this._currentThreat.Formation.CountOfUnits));
						this._currentThreat.Agent = this._targetAgent;
					}
				}
				if (this._currentThreat != null && this._currentThreat.TargetableObject == null && this._currentThreat.Agent == null)
				{
					this._currentThreat = null;
				}
				return this._currentThreat;
			}

			// Token: 0x0600392F RID: 14639 RVA: 0x000E881D File Offset: 0x000E6A1D
			public bool UpdateThreatSeekerTask()
			{
				Agent targetAgent = this._targetAgent;
				if (targetAgent != null && !targetAgent.IsActive())
				{
					this._targetAgent = null;
				}
				return this._getMostDangerousThreat.Update();
			}

			// Token: 0x06003930 RID: 14640 RVA: 0x000E8848 File Offset: 0x000E6A48
			public void PrepareThreatSeekerTask(Action lastAction)
			{
				this._getMostDangerousThreat.Prepare(this.GetAllThreats(), this.Weapon);
				this._getMostDangerousThreat.SetLastAction(lastAction);
			}

			// Token: 0x06003931 RID: 14641 RVA: 0x000E886D File Offset: 0x000E6A6D
			public void Release()
			{
				this._targetAgent = null;
				this._currentThreat = null;
			}

			// Token: 0x06003932 RID: 14642 RVA: 0x000E8880 File Offset: 0x000E6A80
			public List<Threat> GetAllThreats()
			{
				List<Threat> list = new List<Threat>();
				for (int i = this._potentialTargetObjects.Count - 1; i >= 0; i--)
				{
					ITargetable targetable = this._potentialTargetObjects[i];
					UsableMachine usableMachine;
					MissionObject missionObject;
					if (((usableMachine = targetable as UsableMachine) != null && (usableMachine.IsDestroyed || usableMachine.IsDeactivated || !usableMachine.GameEntity.IsValid)) || ((missionObject = targetable as MissionObject) != null && missionObject.IsDisabled) || targetable.GetSide() == this.Weapon.Side)
					{
						this._potentialTargetObjects.RemoveAt(i);
					}
					else
					{
						Threat threat = new Threat
						{
							TargetableObject = targetable,
							ThreatValue = this.Weapon.ProcessTargetValue(targetable.GetTargetValue(this.WeaponPositions), targetable.GetTargetFlags()),
							ForceTarget = targetable.Entity().HasTag("attackMe")
						};
						list.Add(threat);
					}
				}
				foreach (Team team in Mission.Current.Teams)
				{
					if (team.Side.GetOppositeSide() == this.Weapon.Side)
					{
						foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
						{
							if (formation.CountOfUnits > 0)
							{
								float targetValueOfFormation = RangedSiegeWeaponAi.ThreatSeeker.GetTargetValueOfFormation(formation, this._referencePositions);
								if (targetValueOfFormation != -1f)
								{
									list.Add(new Threat
									{
										Formation = formation,
										ThreatValue = this.Weapon.ProcessTargetValue(targetValueOfFormation, RangedSiegeWeaponAi.ThreatSeeker.GetTargetFlagsOfFormation()),
										ForceTarget = false
									});
								}
							}
						}
					}
				}
				return list;
			}

			// Token: 0x06003933 RID: 14643 RVA: 0x000E8A6C File Offset: 0x000E6C6C
			private static float GetTargetValueOfFormation(Formation formation, IEnumerable<ICastleKeyPosition> referencePositions)
			{
				if (formation.QuerySystem.LocalEnemyPower / formation.QuerySystem.LocalAllyPower > 0.5f)
				{
					return -1f;
				}
				float num = (float)formation.CountOfUnits * 3f;
				if (TeamAISiegeComponent.IsFormationInsideCastle(formation, true, 0.4f))
				{
					num *= 3f;
				}
				num *= RangedSiegeWeaponAi.ThreatSeeker.GetPositionMultiplierOfFormation(formation, referencePositions);
				float num2 = MBMath.ClampFloat(formation.QuerySystem.LocalAllyPower / (formation.QuerySystem.LocalEnemyPower + 0.01f), 0f, 5f) / 5f;
				return num * num2;
			}

			// Token: 0x06003934 RID: 14644 RVA: 0x000E8B03 File Offset: 0x000E6D03
			public static TargetFlags GetTargetFlagsOfFormation()
			{
				return TargetFlags.None | TargetFlags.IsMoving | TargetFlags.IsFlammable | TargetFlags.IsAttacker;
			}

			// Token: 0x06003935 RID: 14645 RVA: 0x000E8B10 File Offset: 0x000E6D10
			private static float GetPositionMultiplierOfFormation(Formation formation, IEnumerable<ICastleKeyPosition> referencePositions)
			{
				ICastleKeyPosition castleKeyPosition;
				float minimumDistanceBetweenPositions = RangedSiegeWeaponAi.ThreatSeeker.GetMinimumDistanceBetweenPositions(formation.GetMedianAgent(false, false, formation.GetAveragePositionOfUnits(false, false)).Position, referencePositions, out castleKeyPosition);
				bool flag = castleKeyPosition != null && castleKeyPosition.AttackerSiegeWeapon != null && castleKeyPosition.AttackerSiegeWeapon.HasCompletedAction();
				float num;
				if (formation.PhysicalClass.IsRanged())
				{
					if (minimumDistanceBetweenPositions < 20f)
					{
						num = 1f;
					}
					else if (minimumDistanceBetweenPositions < 35f)
					{
						num = 0.8f;
					}
					else
					{
						num = 0.6f;
					}
					return num + (flag ? 0.2f : 0f);
				}
				if (minimumDistanceBetweenPositions < 15f)
				{
					num = 0.2f;
				}
				else if (minimumDistanceBetweenPositions < 40f)
				{
					num = 0.15f;
				}
				else
				{
					num = 0.12f;
				}
				return num * (flag ? 7.5f : 1f);
			}

			// Token: 0x06003936 RID: 14646 RVA: 0x000E8BD4 File Offset: 0x000E6DD4
			private static float GetMinimumDistanceBetweenPositions(Vec3 position, IEnumerable<ICastleKeyPosition> referencePositions, out ICastleKeyPosition closestCastlePosition)
			{
				if (referencePositions != null && referencePositions.Count<ICastleKeyPosition>() != 0)
				{
					closestCastlePosition = referencePositions.MinBy<ICastleKeyPosition, float>((ICastleKeyPosition rp) => rp.GetPosition().DistanceSquared(position));
					return MathF.Sqrt(closestCastlePosition.GetPosition().DistanceSquared(position));
				}
				closestCastlePosition = null;
				return -1f;
			}

			// Token: 0x06003937 RID: 14647 RVA: 0x000E8C30 File Offset: 0x000E6E30
			public static Threat GetMaxThreat(List<ICastleKeyPosition> castleKeyPositions)
			{
				List<ITargetable> list = new List<ITargetable>();
				List<Threat> list2 = new List<Threat>();
				foreach (WeakGameEntity weakGameEntity in Mission.Current.ActiveMissionObjects.Select<MissionObject, WeakGameEntity>((MissionObject amo) => amo.GameEntity))
				{
					ITargetable targetable;
					if ((targetable = weakGameEntity.GetFirstScriptOfType<UsableMachine>() as ITargetable) != null)
					{
						list.Add(targetable);
					}
				}
				list.RemoveAll((ITargetable um) => um.GetSide() == BattleSideEnum.Defender);
				list2.AddRange(list.Select<ITargetable, Threat>(delegate(ITargetable um)
				{
					Threat threat = new Threat();
					threat.TargetableObject = um;
					threat.ThreatValue = um.GetTargetValue(castleKeyPositions.Select<ICastleKeyPosition, Vec3>((ICastleKeyPosition c) => c.GetPosition()).ToList<Vec3>());
					threat.ForceTarget = um.Entity().HasTag("attackMe");
					return threat;
				}));
				return list2.MaxBy<Threat, float>((Threat t) => t.ThreatValue);
			}

			// Token: 0x04001AAA RID: 6826
			private FindMostDangerousThreat _getMostDangerousThreat;

			// Token: 0x04001AAB RID: 6827
			private const float SingleUnitThreatValue = 3f;

			// Token: 0x04001AAC RID: 6828
			private const float InsideWallsThreatMultiplier = 3f;

			// Token: 0x04001AAD RID: 6829
			private Threat _currentThreat;

			// Token: 0x04001AAE RID: 6830
			private Agent _targetAgent;

			// Token: 0x04001AAF RID: 6831
			public RangedSiegeWeapon Weapon;

			// Token: 0x04001AB0 RID: 6832
			public List<Vec3> WeaponPositions;

			// Token: 0x04001AB1 RID: 6833
			private List<ITargetable> _potentialTargetObjects;

			// Token: 0x04001AB2 RID: 6834
			private List<ICastleKeyPosition> _referencePositions;
		}
	}
}
