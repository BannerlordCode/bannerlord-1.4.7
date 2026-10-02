using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Missions.MissionLogics;
using SandBox.Objects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Objects;

namespace SandBox.Missions.AgentBehaviors
{
	// Token: 0x020000A3 RID: 163
	public class AlarmedBehaviorGroup : AgentBehaviorGroup
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x0002D11D File Offset: 0x0002B31D
		// (set) Token: 0x060006C8 RID: 1736 RVA: 0x0002D125 File Offset: 0x0002B325
		public float AlarmFactor { get; private set; }

		// Token: 0x060006C9 RID: 1737 RVA: 0x0002D130 File Offset: 0x0002B330
		public AlarmedBehaviorGroup(AgentNavigator navigator, Mission mission)
			: base(navigator, mission)
		{
			this._alarmedTimer = new BasicMissionTimer();
			this._checkCalmDownTimer = new BasicMissionTimer();
			this._missionFightHandler = base.Mission.GetMissionBehavior<MissionFightHandler>();
			this._lastSuspiciousPositionTimer = new MissionTimer(10f);
			this._alarmYellTimer = new MissionTimer(10f);
			this._ignoredAgentsForAlarm = new List<Agent>(0);
			this._lastAlarmTriggerTime = MissionTime.Zero;
			base.Mission.OnAddSoundAlarmFactorToAgents += new Mission.OnAddSoundAlarmFactorToAgentsDelegate(this.OnAddSoundAlarmFactor);
			List<GameEntity> list = new List<GameEntity>();
			base.OwnerAgent.Mission.Scene.GetAllEntitiesWithScriptComponent<StealthIndoorLightingArea>(ref list);
			this._stealthIndoorLightingAreas = new MBList<GameEntity>(list);
			List<GameEntity> list2 = new List<GameEntity>();
			base.OwnerAgent.Mission.Scene.GetAllEntitiesWithScriptComponent<StealthBox>(ref list2);
			this._stealthBoxes = new MBList<StealthBox>(list2.Select<GameEntity, StealthBox>((GameEntity ge) => ge.GetFirstScriptOfType<StealthBox>()));
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0002D237 File Offset: 0x0002B437
		public void SetCanMoveWhenCautious(bool value)
		{
			this._canMoveWhenCautious = value;
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0002D240 File Offset: 0x0002B440
		private void UpdateAgentAlarmState(float dt)
		{
			if (!base.OwnerAgent.IsAlarmed())
			{
				bool flag = base.OwnerAgent.IsAIAtMoveDestination();
				if ((!base.OwnerAgent.IsCautious() || flag) && this._lastAlarmTriggerTime.ElapsedSeconds > 2f)
				{
					float alarmFactor = this.AlarmFactor;
					this.AlarmFactor = Math.Max(0f, this.AlarmFactor - (base.OwnerAgent.IsPatrollingCautious() ? 0.025f : (this._canMoveWhenCautious ? 0.125f : 0.08f)) * dt);
					if (alarmFactor >= 1f && this.AlarmFactor < 1f)
					{
						this.AlarmFactor = 0.3f;
					}
				}
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				if (!this.DoNotCheckForAlarmFactorIncrease)
				{
					Vec3 vec = ((base.OwnerAgent.IsHuman && base.OwnerAgent.AgentVisuals.IsValid()) ? base.OwnerAgent.Frame.rotation.TransformToParent(in base.OwnerAgent.GetBoneEntitialFrame(base.OwnerAgent.Monster.HeadLookDirectionBoneIndex, true).rotation.f) : base.OwnerAgent.LookDirection);
					WorldPosition worldPosition = base.OwnerAgent.GetWorldPosition();
					worldPosition.SetVec2(worldPosition.AsVec2 + vec.AsVec2.Normalized() * 1.25f);
					float num = MBMath.ClampFloat(MathF.Tan(worldPosition.GetGroundVec3().z - base.OwnerAgent.Position.z) * 1f, -0.025f, 0.55f);
					Vec3 vec2 = Vec3.CrossProduct(Vec3.Up, vec);
					vec = vec.RotateAboutAnArbitraryVector(vec2.NormalizedCopy(), 0.02f - num);
					foreach (Agent agent in base.OwnerAgent.Mission.AllAgents)
					{
						float num2 = 0f;
						float num3 = 0f;
						AgentState state = agent.State;
						bool flag5 = agent.AgentVisuals.IsValid();
						if (state != AgentState.Deleted && state != AgentState.Routed && state != AgentState.None && flag5)
						{
							AgentFlag agentFlags = agent.GetAgentFlags();
							bool flag6 = this._ignoredAgentsForAlarm.IndexOf(agent) >= 0;
							if (agent != base.OwnerAgent && agentFlags.HasAllFlags(AgentFlag.CanAttack | AgentFlag.IsHumanoid) && ((state != AgentState.Active && !flag6) || (state == AgentState.Active && (agent.IsAlarmed() || (agent.IsPatrollingCautious() && !flag6 && agent.GetComponent<CampaignAgentComponent>().AgentNavigator.GetBehaviorGroup<AlarmedBehaviorGroup>().AlarmFactor > this.AlarmFactor + 0.1f) || base.OwnerAgent.IsEnemyOf(agent)))))
							{
								if (!this.DoNotIncreaseAlarmFactorDueToSeeingOrHearingTheEnemy)
								{
									int effectiveSkill = MissionGameModels.Current.AgentStatCalculateModel.GetEffectiveSkill(agent, DefaultSkills.Roguery);
									float equipmentStealthBonus = MissionGameModels.Current.AgentStatCalculateModel.GetEquipmentStealthBonus(agent);
									float num4 = Math.Max(0f, 1f - ((float)effectiveSkill * 0.0001f + equipmentStealthBonus * 0.002f));
									num2 += this.GetSoundFactor(agent, num4);
								}
								num3 += this.GetVisualFactor(vec, agent, this._stealthIndoorLightingAreas, ref flag3, ref flag2);
								float num5 = Math.Min(3f, num2 + num3);
								if (num5 > 0f && (!flag2 || !this.DoNotIncreaseAlarmFactorDueToSeeingOrHearingTheEnemy))
								{
									this.AlarmFactor += num5 * dt * Campaign.Current.Models.DifficultyModel.GetStealthDifficultyMultiplier();
									if (state == AgentState.Active)
									{
										vec2 = agent.Position;
										if (vec2.DistanceSquared(base.OwnerAgent.Position) < 1f)
										{
											flag4 = true;
										}
									}
									this._lastAlarmTriggerTime = MissionTime.Now;
								}
								if (this.AlarmFactor >= 1f && base.OwnerAgent.IsAlarmStateNormal())
								{
									base.OwnerAgent.SetAlarmState(Agent.AIStateFlag.Cautious);
									WorldPosition worldPosition2 = agent.GetWorldPosition();
									Vec2 asVec = worldPosition2.AsVec2;
									vec2 = base.OwnerAgent.Position;
									Vec2 vec3 = (vec2.AsVec2 - worldPosition2.AsVec2).Normalized();
									vec2 = base.OwnerAgent.Position;
									worldPosition2.SetVec2(asVec + vec3 * (((vec2.AsVec2 - worldPosition2.AsVec2).LengthSquared < 25f) ? 0f : 2f));
									this.SetAILastSuspiciousPositionHelper(in worldPosition2, true);
									this._lastSuspiciousPositionTimer.Reset();
								}
								else if (num5 > 0f && (base.OwnerAgent.IsCautious() || base.OwnerAgent.IsPatrollingCautious()) && this._lastSuspiciousPositionTimer.Check(true))
								{
									WorldPosition worldPosition3 = agent.GetWorldPosition();
									Vec2 asVec2 = worldPosition3.AsVec2;
									vec2 = base.OwnerAgent.Position;
									Vec2 vec4 = (vec2.AsVec2 - worldPosition3.AsVec2).Normalized();
									vec2 = base.OwnerAgent.Position;
									worldPosition3.SetVec2(asVec2 + vec4 * (((vec2.AsVec2 - worldPosition3.AsVec2).LengthSquared < 25f) ? 0f : 2f));
									this.SetAILastSuspiciousPositionHelper(in worldPosition3, true);
								}
								if (num3 > 0f && base.OwnerAgent.IsPatrollingCautious() && (!agent.IsActive() || (!agent.IsEnemyOf(base.OwnerAgent) && !agent.IsAlarmed())))
								{
									this._ignoredAgentsForAlarm.Add(agent);
								}
							}
						}
					}
				}
				if ((this.AlarmFactor >= 2f && (flag2 || flag4)) || (this.AlarmFactor >= 1f && (flag2 && flag4)))
				{
					base.OwnerAgent.SetAlarmState(Agent.AIStateFlag.Alarmed);
					this._alarmYellTimer.Set(-9f);
					this.AlarmFactor = 2f;
				}
				else if (this._canMoveWhenCautious && this.AlarmFactor >= 2f && base.OwnerAgent.IsCautious() && flag3)
				{
					base.OwnerAgent.SetAlarmState(Agent.AIStateFlag.PatrollingCautious);
				}
				else if (this.AlarmFactor < 0.0001f)
				{
					base.OwnerAgent.SetAlarmState(Agent.AIStateFlag.None);
				}
				for (int i = this._ignoredAgentsForAlarm.Count - 1; i >= 0; i--)
				{
					Agent agent2 = this._ignoredAgentsForAlarm[i];
					if (agent2.IsActive() && (agent2.IsAlarmStateNormal() || agent2.IsAlarmed()))
					{
						this._ignoredAgentsForAlarm.RemoveAt(i);
					}
				}
				this.AlarmFactor = Math.Min(this.AlarmFactor, 2f);
				return;
			}
			if (this._alarmYellTimer.Check(true))
			{
				base.OwnerAgent.MakeVoice(SkinVoiceManager.VoiceType.Yell, SkinVoiceManager.CombatVoiceNetworkPredictionType.NoPrediction);
				Mission mission = base.OwnerAgent.Mission;
				Agent ownerAgent = base.OwnerAgent;
				Vec3 vec2 = base.OwnerAgent.Position + new Vec3(0f, 0f, base.OwnerAgent.GetEyeGlobalHeight(), -1f);
				mission.AddSoundAlarmFactorToAgents(ownerAgent, in vec2, 10f);
			}
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0002D994 File Offset: 0x0002BB94
		private void SetAILastSuspiciousPositionHelper(in WorldPosition lastSuspiciousPosition, bool checkNavMeshForCorrection)
		{
			if (this._canMoveWhenCautious)
			{
				base.OwnerAgent.SetAILastSuspiciousPosition(lastSuspiciousPosition, checkNavMeshForCorrection);
				return;
			}
			WorldPosition worldPosition = base.OwnerAgent.GetWorldPosition();
			Vec2 asVec = worldPosition.AsVec2;
			WorldPosition worldPosition2 = lastSuspiciousPosition;
			worldPosition.SetVec2(asVec + (worldPosition2.AsVec2 - base.OwnerAgent.Position.AsVec2).Normalized() * 0.1f);
			base.OwnerAgent.SetAILastSuspiciousPosition(worldPosition, false);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0002DA20 File Offset: 0x0002BC20
		private float GetSoundFactor(Agent currentAgent, float sneakingNoiseMultiplier)
		{
			if (currentAgent.Velocity.LengthSquared > 0.010000001f)
			{
				float num = (currentAgent.Position + new Vec3(0f, 0f, currentAgent.GetEyeGlobalHeight(), -1f) - (base.OwnerAgent.Position + new Vec3(0f, 0f, currentAgent.GetEyeGlobalHeight(), -1f))).Normalize();
				float num2 = 125f * Math.Min(1f, currentAgent.AverageVelocity.Length / currentAgent.GetMaximumForwardUnlimitedSpeed());
				bool flag = false;
				if (currentAgent.Mission.Scene.GetWaterLevelAtPosition(currentAgent.Position.AsVec2, !GameNetwork.IsMultiplayer, true) > currentAgent.Position.z)
				{
					BodyFlags bodyFlags;
					currentAgent.Mission.Scene.GetGroundHeightAndBodyFlagsAtPosition(currentAgent.Position, out bodyFlags, BodyFlags.CommonCollisionExcludeFlagsForAgent);
					if ((bodyFlags & (BodyFlags.Moveable | BodyFlags.Sinking)) != BodyFlags.Moveable)
					{
						flag = true;
						num2 *= 4f;
					}
				}
				if (currentAgent.HasMount || num <= currentAgent.CollisionCapsule.Radius * 2.5f)
				{
					num2 *= 12f;
				}
				else if (currentAgent.State == AgentState.Active && currentAgent.AgentVisuals.IsValid())
				{
					switch (currentAgent.AgentVisuals.GetMovementMode())
					{
					case HumanWalkingMovementMode.Walking:
						num2 *= 0.7f;
						break;
					case HumanWalkingMovementMode.CrouchRunning:
						num2 *= (flag ? 0.45f : 0.25f);
						break;
					case HumanWalkingMovementMode.CrouchWalking:
						num2 *= (flag ? 0.1f : 0f);
						break;
					}
				}
				num2 *= sneakingNoiseMultiplier;
				num2 /= 20f + num * num * 2.5f;
				if (num2 > 0.125f)
				{
					return num2;
				}
			}
			return 0f;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0002DBF4 File Offset: 0x0002BDF4
		public float GetVisualFactor(Vec3 usedGlobalLookDirection, Agent currentAgent, MBReadOnlyList<GameEntity> stealthIndoorLightingAreas, ref bool hasVisualOnCorpse, ref bool hasVisualOnEnemy)
		{
			Vec3 vec = currentAgent.Position + new Vec3(0f, 0f, currentAgent.GetEyeGlobalHeight(), -1f) - (base.OwnerAgent.Position + new Vec3(0f, 0f, currentAgent.GetEyeGlobalHeight(), -1f));
			float num = 0f;
			float num2 = Vec3.DotProduct(vec, usedGlobalLookDirection);
			bool flag = vec.LengthSquared < 1f;
			if (num2 > 0f && (flag || !this.IsAgentCoveredByAStealthBox(currentAgent)))
			{
				float num3 = vec.Normalize();
				bool flag2 = currentAgent.Velocity.LengthSquared > 0.010000001f;
				float equipmentStealthBonus = MissionGameModels.Current.AgentStatCalculateModel.GetEquipmentStealthBonus(currentAgent);
				float num4 = this.GetVisualStrength(vec, usedGlobalLookDirection, currentAgent, flag2, num3, equipmentStealthBonus);
				if (num4 > 0.001f)
				{
					bool isDayTime = base.OwnerAgent.Mission.Scene.IsDayTime;
					Vec3 position = currentAgent.Position;
					float num5 = (isDayTime ? 0.7f : 0.2f);
					float num6 = (isDayTime ? 1f : 0.15f);
					foreach (GameEntity gameEntity in stealthIndoorLightingAreas)
					{
						StealthIndoorLightingArea firstScriptOfType = gameEntity.GetFirstScriptOfType<StealthIndoorLightingArea>();
						if (firstScriptOfType.IsPointIn(position))
						{
							num5 = firstScriptOfType.AmbientLightStrength;
							num6 = firstScriptOfType.SunMoonLightStrength;
							break;
						}
					}
					float visualStrengthOfAgentVisual = base.OwnerAgent.AgentVisuals.GetVisualStrengthOfAgentVisual(currentAgent.AgentVisuals, base.OwnerAgent.Mission, num5, num6, base.OwnerAgent.Index);
					num4 *= visualStrengthOfAgentVisual;
					if (num4 > 0.3f)
					{
						num += num4;
						if (!currentAgent.IsActive())
						{
							hasVisualOnCorpse = true;
						}
						else if (base.OwnerAgent.IsEnemyOf(currentAgent))
						{
							hasVisualOnEnemy = true;
							if (currentAgent != Agent.Main && Agent.Main != null && currentAgent.IsFriendOf(Agent.Main))
							{
								num *= 0.5f;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0002DE04 File Offset: 0x0002C004
		private float GetVisualStrength(Vec3 positionDifferenceDirection, Vec3 usedGlobalLookDirection, Agent currentAgent, bool currentAgentHasSpeed, float distance, float equipmentStealthBonus)
		{
			float num = 1.4922565f;
			float num2 = 0.8953539f;
			Vec3 vec = usedGlobalLookDirection.CrossProductWithUp();
			vec = vec.NormalizedCopy();
			Mat3 mat = new Mat3(in vec, in usedGlobalLookDirection, in Vec3.Up);
			mat.u = Vec3.CrossProduct(mat.s, mat.f);
			Vec3 vec2 = mat.TransformToLocal(in positionDifferenceDirection);
			float num3 = MathF.Atan2(vec2.z, vec2.x);
			float num4 = MathF.Acos(MBMath.ClampFloat(vec2.y, 0f, 1f));
			float num5;
			float num6;
			MathF.SinCos(num3, out num5, out num6);
			float num7 = num * num2 / MathF.Sqrt(num * num * num5 * num5 + num2 * num2 * num6 * num6);
			float num8 = ((num4 >= num7) ? 0f : Math.Min(1f, 0.025f + (num7 - num4) / num7));
			num8 *= num8;
			if (currentAgent.HasMount || distance <= currentAgent.CollisionCapsule.Radius * 6.5f)
			{
				num8 *= 15f;
			}
			else if (currentAgent.AgentVisuals.IsValid() && currentAgent.CrouchMode)
			{
				num8 *= (currentAgentHasSpeed ? 0.9f : 0.8f);
			}
			if (currentAgent.State != AgentState.Active || currentAgent.IsAlarmed())
			{
				num8 *= 1.45f;
			}
			float num9 = Math.Max(0f, 1f - equipmentStealthBonus * 0.0025f);
			num8 *= 575f * num9;
			return num8 / (5f + distance * distance * 1.1f);
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0002DF88 File Offset: 0x0002C188
		public void ResetAlarmFactor()
		{
			this.AlarmFactor = 0f;
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0002DF98 File Offset: 0x0002C198
		private void AddAlarmFactor(float addedAlarmFactor, Agent suspiciousAgent)
		{
			this.AlarmFactor += addedAlarmFactor;
			this._lastAlarmTriggerTime = MissionTime.Now;
			if (this.AlarmFactor >= 1f && base.OwnerAgent.IsAlarmStateNormal())
			{
				base.OwnerAgent.SetAlarmState(Agent.AIStateFlag.Cautious);
				if (suspiciousAgent != null)
				{
					WorldPosition worldPosition = suspiciousAgent.GetWorldPosition();
					this.SetAILastSuspiciousPositionHelper(in worldPosition, true);
				}
				else
				{
					WorldPosition worldPosition = base.OwnerAgent.GetWorldPosition();
					this.SetAILastSuspiciousPositionHelper(in worldPosition, false);
				}
				this._lastSuspiciousPositionTimer.Reset();
				return;
			}
			if ((base.OwnerAgent.IsCautious() || base.OwnerAgent.IsPatrollingCautious()) && this._lastSuspiciousPositionTimer.Check(true))
			{
				WorldPosition worldPosition;
				if (suspiciousAgent != null)
				{
					worldPosition = suspiciousAgent.GetWorldPosition();
					this.SetAILastSuspiciousPositionHelper(in worldPosition, true);
					return;
				}
				worldPosition = base.OwnerAgent.GetWorldPosition();
				this.SetAILastSuspiciousPositionHelper(in worldPosition, false);
			}
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0002E06C File Offset: 0x0002C26C
		public void AddAlarmFactor(float addedAlarmFactor, in WorldPosition suspiciousPosition)
		{
			this.AlarmFactor += addedAlarmFactor;
			this._lastAlarmTriggerTime = MissionTime.Now;
			if (this.AlarmFactor >= 1f && base.OwnerAgent.IsAlarmStateNormal())
			{
				base.OwnerAgent.SetAlarmState(Agent.AIStateFlag.Cautious);
				this.SetAILastSuspiciousPositionHelper(in suspiciousPosition, true);
				this._lastSuspiciousPositionTimer.Reset();
				return;
			}
			if ((base.OwnerAgent.IsCautious() || base.OwnerAgent.IsPatrollingCautious()) && this._lastSuspiciousPositionTimer.Check(true))
			{
				this.SetAILastSuspiciousPositionHelper(in suspiciousPosition, true);
			}
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0002E100 File Offset: 0x0002C300
		public override void Tick(float dt, bool isSimulation)
		{
			if (base.Mission.AllowAiTicking && base.OwnerAgent.IsAIControlled)
			{
				this.HandleMissiles(dt);
				if (base.OwnerAgent.GetAgentFlags().HasAllFlags(AgentFlag.CanWieldWeapon | AgentFlag.CanGetAlarmed))
				{
					this.UpdateAgentAlarmState(dt);
				}
			}
			if (base.IsActive)
			{
				if (base.ScriptedBehavior != null)
				{
					if (!base.ScriptedBehavior.IsActive)
					{
						base.DisableAllBehaviors();
						base.ScriptedBehavior.IsActive = true;
					}
				}
				else
				{
					float num = 0f;
					int num2 = -1;
					for (int i = 0; i < this.Behaviors.Count; i++)
					{
						float availability = this.Behaviors[i].GetAvailability(isSimulation);
						if (availability > num)
						{
							num = availability;
							num2 = i;
						}
					}
					if (num > 0f && num2 != -1 && !this.Behaviors[num2].IsActive)
					{
						base.DisableAllBehaviors();
						this.Behaviors[num2].IsActive = true;
					}
				}
				this.TickActiveBehaviors(dt, isSimulation);
			}
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0002E1FC File Offset: 0x0002C3FC
		private void TickActiveBehaviors(float dt, bool isSimulation)
		{
			foreach (AgentBehavior agentBehavior in this.Behaviors)
			{
				if (agentBehavior.IsActive)
				{
					agentBehavior.Tick(dt, isSimulation);
				}
			}
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0002E258 File Offset: 0x0002C458
		public override float GetScore(bool isSimulation)
		{
			if (base.OwnerAgent.IsAlarmed() || base.OwnerAgent.IsPatrollingCautious() || base.OwnerAgent.IsCautious())
			{
				if (!this.DisableCalmDown && this._alarmedTimer.ElapsedTime > 10f && this._checkCalmDownTimer.ElapsedTime > 1f)
				{
					this._checkCalmDownTimer.Reset();
					if (!this.IsNearDanger())
					{
						base.OwnerAgent.DisableScriptedMovement();
					}
				}
				return 1f;
			}
			if (this.IsNearDanger())
			{
				AlarmedBehaviorGroup.AlarmAgent(base.OwnerAgent);
				return 1f;
			}
			return 0f;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0002E2FC File Offset: 0x0002C4FC
		private bool IsNearDanger()
		{
			float num;
			Agent closestAlarmSource = this.GetClosestAlarmSource(out num);
			return closestAlarmSource != null && (num < 225f || this.Navigator.CanSeeAgent(closestAlarmSource));
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0002E330 File Offset: 0x0002C530
		public Agent GetClosestAlarmSource(out float distanceSquared)
		{
			distanceSquared = float.MaxValue;
			if (this._missionFightHandler == null || !this._missionFightHandler.IsThereActiveFight())
			{
				return null;
			}
			Agent agent = null;
			foreach (Agent agent2 in this._missionFightHandler.GetDangerSources(base.OwnerAgent))
			{
				float num = agent2.Position.DistanceSquared(base.OwnerAgent.Position);
				if (num < distanceSquared)
				{
					distanceSquared = num;
					agent = agent2;
				}
			}
			return agent;
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0002E3C8 File Offset: 0x0002C5C8
		public static void AlarmAgent(Agent agent)
		{
			agent.SetWatchState(Agent.WatchState.Alarmed);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0002E3D4 File Offset: 0x0002C5D4
		protected override void OnActivate()
		{
			TextObject textObject = new TextObject("{=!}{p0} {p1} activate alarmed behavior group.", null);
			textObject.SetTextVariable("p0", base.OwnerAgent.Name);
			textObject.SetTextVariable("p1", base.OwnerAgent.Index);
			this._alarmedTimer.Reset();
			this._checkCalmDownTimer.Reset();
			base.OwnerAgent.DisableScriptedMovement();
			base.OwnerAgent.ClearTargetFrame();
			this.Navigator.SetItemsVisibility(false);
			if (CampaignMission.Current.Location != null)
			{
				LocationCharacter locationCharacter = CampaignMission.Current.Location.GetLocationCharacter(base.OwnerAgent.Origin);
				if (locationCharacter != null && locationCharacter.ActionSetCode != locationCharacter.AlarmedActionSetCode)
				{
					AnimationSystemData animationSystemData = locationCharacter.GetAgentBuildData().AgentMonster.FillAnimationSystemData(MBGlobals.GetActionSet(locationCharacter.AlarmedActionSetCode), locationCharacter.Character.GetStepSize(), false);
					base.OwnerAgent.SetActionSet(ref animationSystemData);
				}
			}
			if (this.Navigator.MemberOfAlley != null || MissionFightHandler.IsAgentAggressive(base.OwnerAgent))
			{
				this.DisableCalmDown = true;
			}
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0002E4E4 File Offset: 0x0002C6E4
		private void HandleMissiles(float dt)
		{
			foreach (Mission.Missile missile in base.Mission.MissilesList)
			{
				Vec3 position = missile.GetPosition();
				Vec3 velocity = missile.GetVelocity();
				float num = velocity.Length / 20f + 0.1f;
				float num2 = 0.1f;
				float num3 = 20f;
				float num4 = MathF.Sqrt(num * num / num2 - num3);
				if (!base.OwnerAgent.IsAlarmed() && base.OwnerAgent.IsActive() && base.OwnerAgent.IsAIControlled && base.OwnerAgent.GetAgentFlags().HasAnyFlag(AgentFlag.CanGetAlarmed) && base.OwnerAgent.RiderAgent == null && base.OwnerAgent != missile.ShooterAgent)
				{
					Vec3 position2 = base.OwnerAgent.Position;
					position2.z += base.OwnerAgent.GetEyeGlobalHeight();
					Vec3 vec = position + velocity;
					float num5 = MBMath.GetClosestPointOnLineSegmentToPoint(in position, in vec, in position2).DistanceSquared(position2);
					if (num5 < num4 * num4)
					{
						this.AddAlarmFactor(num * num / (num3 + num5) * dt, missile.ShooterAgent);
					}
				}
			}
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0002E658 File Offset: 0x0002C858
		private void OnAddSoundAlarmFactor(Agent alarmCreatorAgent, in Vec3 soundPosition, float soundLevelSquareRoot)
		{
			if (!GameNetwork.IsClientOrReplay)
			{
				float num = 0.7f;
				float num2 = 20f;
				float num3 = MathF.Sqrt(soundLevelSquareRoot * soundLevelSquareRoot / num - num2);
				if (base.OwnerAgent.IsActive() && !base.OwnerAgent.IsAlarmed() && base.OwnerAgent.IsAIControlled && base.OwnerAgent.GetAgentFlags().HasAnyFlag(AgentFlag.CanGetAlarmed) && base.OwnerAgent.RiderAgent == null && base.OwnerAgent != alarmCreatorAgent)
				{
					Vec3 position = base.OwnerAgent.Position;
					position.z += base.OwnerAgent.GetEyeGlobalHeight();
					Vec3 vec = soundPosition;
					float num4 = vec.DistanceSquared(position);
					if (num4 < num3 * num3)
					{
						float num5 = soundLevelSquareRoot * soundLevelSquareRoot / (num2 + num4);
						WorldPosition worldPosition = new WorldPosition(base.Mission.Scene, soundPosition);
						this.AddAlarmFactor(num5, in worldPosition);
					}
				}
			}
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0002E748 File Offset: 0x0002C948
		public override void OnAgentRemoved(Agent agent)
		{
			if (agent == base.OwnerAgent)
			{
				base.Mission.OnAddSoundAlarmFactorToAgents -= new Mission.OnAddSoundAlarmFactorToAgentsDelegate(this.OnAddSoundAlarmFactor);
			}
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0002E76C File Offset: 0x0002C96C
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			if (base.OwnerAgent.IsActive())
			{
				EquipmentIndex offhandWieldedItemIndex = base.OwnerAgent.GetOffhandWieldedItemIndex();
				if (offhandWieldedItemIndex != EquipmentIndex.None && offhandWieldedItemIndex != EquipmentIndex.ExtraWeaponSlot)
				{
					base.Mission.AddTickAction(Mission.MissionTickAction.TryToSheathWeaponInHand, base.OwnerAgent, 1, 0);
				}
				base.Mission.AddTickAction(Mission.MissionTickAction.TryToSheathWeaponInHand, base.OwnerAgent, 0, 3);
				base.OwnerAgent.SetWatchState(Agent.WatchState.Patrolling);
				base.OwnerAgent.ResetLookAgent();
				base.OwnerAgent.SetActionChannel(0, in ActionIndexCache.act_none, true, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
				base.OwnerAgent.SetActionChannel(1, in ActionIndexCache.act_none, true, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
			}
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0002E84E File Offset: 0x0002CA4E
		public override void ForceThink(float inSeconds)
		{
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0002E850 File Offset: 0x0002CA50
		private bool IsAgentCoveredByAStealthBox(Agent agent)
		{
			ItemObject item = agent.WieldedOffhandWeapon.Item;
			if (item != null && item.ItemFlags.HasAnyFlag(ItemFlags.HasToBeHeldUp))
			{
				return false;
			}
			foreach (StealthBox stealthBox in this._stealthBoxes)
			{
				if (stealthBox.IsAgentInside(agent) && (stealthBox.CoversStandingAgents || agent.CrouchMode || !agent.IsActive()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0002E8EC File Offset: 0x0002CAEC
		public override void ConversationTick()
		{
			foreach (AgentBehavior agentBehavior in this.Behaviors)
			{
				if (agentBehavior.IsActive)
				{
					agentBehavior.ConversationTick();
				}
			}
		}

		// Token: 0x0400039E RID: 926
		public const float SafetyDistance = 15f;

		// Token: 0x0400039F RID: 927
		public const float SafetyDistanceSquared = 225f;

		// Token: 0x040003A0 RID: 928
		private const float NearbyDistanceThreshold = 1f;

		// Token: 0x040003A1 RID: 929
		private const float NearbyDistanceThresholdSquared = 1f;

		// Token: 0x040003A2 RID: 930
		private readonly MissionFightHandler _missionFightHandler;

		// Token: 0x040003A3 RID: 931
		public bool DisableCalmDown;

		// Token: 0x040003A4 RID: 932
		private readonly BasicMissionTimer _alarmedTimer;

		// Token: 0x040003A5 RID: 933
		private readonly BasicMissionTimer _checkCalmDownTimer;

		// Token: 0x040003A6 RID: 934
		public bool DoNotCheckForAlarmFactorIncrease;

		// Token: 0x040003A7 RID: 935
		public bool DoNotIncreaseAlarmFactorDueToSeeingOrHearingTheEnemy;

		// Token: 0x040003A9 RID: 937
		private bool _canMoveWhenCautious = true;

		// Token: 0x040003AA RID: 938
		private readonly MissionTimer _lastSuspiciousPositionTimer;

		// Token: 0x040003AB RID: 939
		private readonly MissionTimer _alarmYellTimer;

		// Token: 0x040003AC RID: 940
		private readonly List<Agent> _ignoredAgentsForAlarm;

		// Token: 0x040003AD RID: 941
		private readonly MBList<GameEntity> _stealthIndoorLightingAreas;

		// Token: 0x040003AE RID: 942
		private readonly MBList<StealthBox> _stealthBoxes;

		// Token: 0x040003AF RID: 943
		private MissionTime _lastAlarmTriggerTime;
	}
}
