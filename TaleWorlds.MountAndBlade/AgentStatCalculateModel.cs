using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001F1 RID: 497
	public abstract class AgentStatCalculateModel : MBGameModel<AgentStatCalculateModel>
	{
		// Token: 0x06001D33 RID: 7475
		public abstract void InitializeAgentStats(Agent agent, Equipment spawnEquipment, AgentDrivenProperties agentDrivenProperties, AgentBuildData agentBuildData);

		// Token: 0x06001D34 RID: 7476 RVA: 0x00063152 File Offset: 0x00061352
		public virtual void InitializeMissionEquipment(Agent agent)
		{
		}

		// Token: 0x06001D35 RID: 7477 RVA: 0x00063154 File Offset: 0x00061354
		public virtual void InitializeAgentStatsAfterDeploymentFinished(Agent agent)
		{
		}

		// Token: 0x06001D36 RID: 7478 RVA: 0x00063156 File Offset: 0x00061356
		public virtual void InitializeMissionEquipmentAfterDeploymentFinished(Agent agent)
		{
		}

		// Token: 0x06001D37 RID: 7479
		public abstract void UpdateAgentStats(Agent agent, AgentDrivenProperties agentDrivenProperties);

		// Token: 0x06001D38 RID: 7480
		public abstract float GetDifficultyModifier();

		// Token: 0x06001D39 RID: 7481
		public abstract bool CanAgentRideMount(Agent agent, Agent targetMount);

		// Token: 0x06001D3A RID: 7482 RVA: 0x00063158 File Offset: 0x00061358
		public virtual bool HasHeavyArmor(Agent agent)
		{
			return agent.GetBaseArmorEffectivenessForBodyPart(BoneBodyPartType.Chest) >= 24f;
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x0006316B File Offset: 0x0006136B
		public virtual float GetEffectiveArmorEncumbrance(Agent agent, Equipment equipment)
		{
			return equipment.GetTotalWeightOfArmor(agent.IsHuman);
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x00063179 File Offset: 0x00061379
		public virtual float GetEffectiveMaxHealth(Agent agent)
		{
			return agent.BaseHealthLimit;
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x00063184 File Offset: 0x00061384
		public virtual float GetEnvironmentSpeedFactor(Agent agent)
		{
			Scene scene = agent.Mission.Scene;
			float num = 1f;
			if (!scene.IsAtmosphereIndoor)
			{
				if (scene.GetRainDensity() > 0f)
				{
					num *= 0.9f;
				}
				if (!agent.IsHuman && !scene.IsDayTime)
				{
					num *= 0.9f;
				}
			}
			return num;
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x000631D9 File Offset: 0x000613D9
		public float CalculateAIAttackOnDecideMaxValue()
		{
			if (this.GetDifficultyModifier() <= 0.5f)
			{
				return 0.16f;
			}
			return 0.48f;
		}

		// Token: 0x06001D3F RID: 7487 RVA: 0x000631F4 File Offset: 0x000613F4
		public virtual float GetWeaponInaccuracy(Agent agent, WeaponComponentData weapon, int weaponSkill)
		{
			float num = 0f;
			if (weapon.IsRangedWeapon)
			{
				if (weapon.WeaponClass == WeaponClass.Sling)
				{
					num = (100f - (float)weapon.Accuracy) * (1f - 0.003f * (float)weaponSkill) * 0.001f;
				}
				else
				{
					num = (100f - (float)weapon.Accuracy) * (1f - 0.002f * (float)weaponSkill) * 0.001f;
				}
			}
			else if (weapon.WeaponFlags.HasAllFlags(WeaponFlags.WideGrip))
			{
				num = 1f - (float)weaponSkill * 0.01f;
			}
			return MathF.Max(num, 0f);
		}

		// Token: 0x06001D40 RID: 7488 RVA: 0x0006328D File Offset: 0x0006148D
		public virtual float GetDetachmentCostMultiplierOfAgent(Agent agent, IDetachment detachment)
		{
			if (agent.Banner != null)
			{
				return 10f;
			}
			return 1f;
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x000632A2 File Offset: 0x000614A2
		public virtual float GetInteractionDistance(Agent agent)
		{
			return 1.5f;
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x000632A9 File Offset: 0x000614A9
		public virtual float GetMaxCameraZoom(Agent agent)
		{
			return 1f;
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x000632B0 File Offset: 0x000614B0
		public virtual int GetEffectiveSkill(Agent agent, SkillObject skill)
		{
			return agent.Character.GetSkillValue(skill);
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x000632BE File Offset: 0x000614BE
		public virtual int GetEffectiveSkillForWeapon(Agent agent, WeaponComponentData weapon)
		{
			return this.GetEffectiveSkill(agent, weapon.RelevantSkill);
		}

		// Token: 0x06001D45 RID: 7493
		public abstract float GetWeaponDamageMultiplier(Agent agent, WeaponComponentData weapon);

		// Token: 0x06001D46 RID: 7494
		public abstract float GetEquipmentStealthBonus(Agent agent);

		// Token: 0x06001D47 RID: 7495
		public abstract float GetSneakAttackMultiplier(Agent agent, WeaponComponentData weapon);

		// Token: 0x06001D48 RID: 7496
		public abstract float GetKnockBackResistance(Agent agent);

		// Token: 0x06001D49 RID: 7497
		public abstract float GetKnockDownResistance(Agent agent, StrikeType strikeType = StrikeType.Invalid);

		// Token: 0x06001D4A RID: 7498
		public abstract float GetDismountResistance(Agent agent);

		// Token: 0x06001D4B RID: 7499
		public abstract float GetBreatheHoldMaxDuration(Agent agent, float baseBreatheHoldMaxDuration);

		// Token: 0x06001D4C RID: 7500 RVA: 0x000632CD File Offset: 0x000614CD
		public virtual string GetMissionDebugInfoForAgent(Agent agent)
		{
			return "Debug info not supported in this model";
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x000632D4 File Offset: 0x000614D4
		public void ResetAILevelMultiplier()
		{
			this._AILevelMultiplier = 1f;
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x000632E1 File Offset: 0x000614E1
		public void SetAILevelMultiplier(float multiplier)
		{
			this._AILevelMultiplier = multiplier;
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x000632EC File Offset: 0x000614EC
		protected int GetMeleeSkill(Agent agent, WeaponComponentData equippedItem, WeaponComponentData secondaryItem)
		{
			SkillObject skillObject = DefaultSkills.Athletics;
			if (equippedItem != null)
			{
				SkillObject relevantSkill = equippedItem.RelevantSkill;
				if (relevantSkill == DefaultSkills.OneHanded || relevantSkill == DefaultSkills.Polearm)
				{
					skillObject = relevantSkill;
				}
				else if (relevantSkill == DefaultSkills.TwoHanded)
				{
					skillObject = ((secondaryItem == null) ? DefaultSkills.TwoHanded : DefaultSkills.OneHanded);
				}
				else
				{
					skillObject = DefaultSkills.OneHanded;
				}
			}
			return this.GetEffectiveSkill(agent, skillObject);
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x00063348 File Offset: 0x00061548
		protected float CalculateAILevel(Agent agent, int relevantSkillLevel)
		{
			float difficultyModifier = this.GetDifficultyModifier();
			return MBMath.ClampFloat((float)relevantSkillLevel / 300f * ((difficultyModifier <= 0f) ? 0.1f : ((difficultyModifier <= 0.5f) ? 0.32f : 0.96f)), 0f, 1f);
		}

		// Token: 0x06001D51 RID: 7505 RVA: 0x00063398 File Offset: 0x00061598
		protected void SetAiRelatedProperties(Agent agent, AgentDrivenProperties agentDrivenProperties, WeaponComponentData equippedItem, WeaponComponentData secondaryItem)
		{
			int meleeSkill = this.GetMeleeSkill(agent, equippedItem, secondaryItem);
			SkillObject skillObject = ((equippedItem == null) ? DefaultSkills.Athletics : equippedItem.RelevantSkill);
			int effectiveSkill = this.GetEffectiveSkill(agent, skillObject);
			agentDrivenProperties.AiShooterErrorWoRangeUpdate = 0f;
			float num = this.CalculateAILevel(agent, meleeSkill) * this._AILevelMultiplier;
			float num2 = this.CalculateAILevel(agent, effectiveSkill) * this._AILevelMultiplier;
			float num3 = num + agent.Defensiveness;
			float difficultyModifier = this.GetDifficultyModifier();
			agentDrivenProperties.AiRangedHorsebackMissileRange = 0.3f + 0.4f * num2;
			agentDrivenProperties.AiFacingMissileWatch = -0.96f + num * 0.06f;
			agentDrivenProperties.AiFlyingMissileCheckRadius = 8f - 6f * num;
			agentDrivenProperties.AiShootFreq = 0.3f + 0.7f * num2;
			agentDrivenProperties.AiWaitBeforeShootFactor = (agent.PropertyModifiers.resetAiWaitBeforeShootFactor ? 0f : (1f - 0.5f * num2));
			agentDrivenProperties.AIBlockOnDecideAbility = MBMath.Lerp(0.5f, 0.99f, MBMath.ClampFloat(MathF.Pow(num, 0.5f), 0f, 1f), 1E-05f);
			agentDrivenProperties.AIParryOnDecideAbility = MBMath.Lerp(0.5f, 0.95f, MBMath.ClampFloat(num, 0f, 1f), 1E-05f);
			agentDrivenProperties.AiTryChamberAttackOnDecide = (num - 0.15f) * 0.1f;
			agentDrivenProperties.AIAttackOnParryChance = 0.08f - 0.02f * agent.Defensiveness;
			agentDrivenProperties.AiAttackOnParryTiming = -0.2f + 0.3f * num;
			agentDrivenProperties.AIDecideOnAttackChance = 0.5f * agent.Defensiveness;
			agentDrivenProperties.AIParryOnAttackAbility = MBMath.ClampFloat(num, 0f, 1f);
			agentDrivenProperties.AiKick = -0.1f + ((num > 0.4f) ? 0.4f : num);
			agentDrivenProperties.AiAttackCalculationMaxTimeFactor = num;
			agentDrivenProperties.AiDecideOnAttackWhenReceiveHitTiming = -0.25f * (1f - num);
			agentDrivenProperties.AiDecideOnAttackContinueAction = -0.5f * (1f - num);
			agentDrivenProperties.AiDecideOnAttackingContinue = 0.1f * num;
			agentDrivenProperties.AIParryOnAttackingContinueAbility = MBMath.Lerp(0.5f, 0.95f, MBMath.ClampFloat(num, 0f, 1f), 1E-05f);
			agentDrivenProperties.AIDecideOnRealizeEnemyBlockingAttackAbility = MBMath.ClampFloat(MathF.Pow(num, 2.5f) - 0.1f, 0f, 1f);
			agentDrivenProperties.AIRealizeBlockingFromIncorrectSideAbility = MBMath.ClampFloat(MathF.Pow(num, 2.5f) - 0.01f, 0f, 1f);
			agentDrivenProperties.AiAttackingShieldDefenseChance = 0.2f + 0.3f * num;
			agentDrivenProperties.AiAttackingShieldDefenseTimer = -0.3f + 0.3f * num;
			agentDrivenProperties.AiRandomizedDefendDirectionChance = 1f - MathF.Pow(num, 3f);
			agentDrivenProperties.AiShooterError = 0.008f;
			agentDrivenProperties.AISetNoAttackTimerAfterBeingHitAbility = MBMath.Lerp(0.33f, 1f, num, 1E-05f);
			agentDrivenProperties.AISetNoAttackTimerAfterBeingParriedAbility = MBMath.Lerp(0.2f, 1f, num * num, 1E-05f);
			agentDrivenProperties.AISetNoDefendTimerAfterHittingAbility = MBMath.Lerp(0.1f, 0.99f, num * num, 1E-05f);
			agentDrivenProperties.AISetNoDefendTimerAfterParryingAbility = MBMath.Lerp(0.15f, 1f, num * num, 1E-05f);
			agentDrivenProperties.AIEstimateStunDurationPrecision = 1f - MBMath.Lerp(0.2f, 1f, num, 1E-05f);
			agentDrivenProperties.AIHoldingReadyMaxDuration = MBMath.Lerp(0.25f, 0f, MathF.Min(1f, num * 2f), 1E-05f);
			agentDrivenProperties.AIHoldingReadyVariationPercentage = num;
			agentDrivenProperties.AiRaiseShieldDelayTimeBase = -0.75f + 0.5f * num;
			agentDrivenProperties.AiUseShieldAgainstEnemyMissileProbability = 0.1f + num * 0.6f + num3 * 0.2f;
			agentDrivenProperties.AiCheckApplyMovementInterval = (2f - difficultyModifier) * (0.05f + 0.005f * (1.1f - num));
			agentDrivenProperties.AiCheckCalculateMovementInterval = ((agent.HasMount || agent.IsMount) ? 0.25f : ((2f - difficultyModifier) * 0.25f));
			agentDrivenProperties.AiCheckDecideSimpleBehaviorInterval = (2f - difficultyModifier) * (agent.GetAgentFlags().HasAnyFlag(AgentFlag.CanWieldWeapon) ? 1.5f : 0.2f);
			agentDrivenProperties.AiCheckDoSimpleBehaviorInterval = 2f - difficultyModifier;
			agentDrivenProperties.AiMovementDelayFactor = 4f / (3f + num2);
			agentDrivenProperties.AiParryDecisionChangeValue = 0.05f + 0.7f * num;
			agentDrivenProperties.AiDefendWithShieldDecisionChanceValue = MathF.Min(2f, 0.5f + num + 0.6f * num3);
			agentDrivenProperties.AiMoveEnemySideTimeValue = -2.5f + 0.5f * num;
			agentDrivenProperties.AiMinimumDistanceToContinueFactor = 2f + 0.3f * (3f - num);
			agentDrivenProperties.AiChargeHorsebackTargetDistFactor = 1.5f * (3f - num);
			agentDrivenProperties.AiWaitBeforeShootFactor = (agent.PropertyModifiers.resetAiWaitBeforeShootFactor ? 0f : (1f - 0.5f * num2));
			float num4 = 1f - num2;
			agentDrivenProperties.AiRangerLeadErrorMin = -num4 * 0.35f;
			agentDrivenProperties.AiRangerLeadErrorMax = num4 * 0.2f;
			agentDrivenProperties.AiRangerVerticalErrorMultiplier = num4 * 0.1f;
			agentDrivenProperties.AiRangerHorizontalErrorMultiplier = num4 * 0.034906585f;
			agentDrivenProperties.AIAttackOnDecideChance = MathF.Clamp(0.1f * this.CalculateAIAttackOnDecideMaxValue() * (3f - agent.Defensiveness), 0.05f, 1f);
			agentDrivenProperties.SetStat(DrivenProperty.UseRealisticBlocking, (agent.Controller != AgentControllerType.Player) ? 1f : 0f);
			agentDrivenProperties.AiWeaponFavorMultiplierMelee = 1f;
			agentDrivenProperties.AiWeaponFavorMultiplierRanged = 1f;
			agentDrivenProperties.AiWeaponFavorMultiplierPolearm = 1f;
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x0006392F File Offset: 0x00061B2F
		protected void SetAllWeaponInaccuracy(Agent agent, AgentDrivenProperties agentDrivenProperties, int equippedIndex, WeaponComponentData equippedWeaponComponent)
		{
			if (equippedWeaponComponent != null)
			{
				agentDrivenProperties.WeaponInaccuracy = this.GetWeaponInaccuracy(agent, equippedWeaponComponent, this.GetEffectiveSkillForWeapon(agent, equippedWeaponComponent));
				return;
			}
			agentDrivenProperties.WeaponInaccuracy = 0f;
		}

		// Token: 0x04000A38 RID: 2616
		protected const float MaxHorizontalErrorRadian = 0.034906585f;

		// Token: 0x04000A39 RID: 2617
		private float _AILevelMultiplier = 1f;
	}
}
