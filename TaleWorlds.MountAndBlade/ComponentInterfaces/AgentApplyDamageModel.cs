using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ComponentInterfaces
{
	// Token: 0x020003F7 RID: 1015
	public abstract class AgentApplyDamageModel : MBGameModel<AgentApplyDamageModel>
	{
		// Token: 0x0600375A RID: 14170 RVA: 0x000E4CB0 File Offset: 0x000E2EB0
		public float CalculateDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			AgentApplyDamageModel agentApplyDamageModel = MissionGameModels.Current.AgentApplyDamageModel;
			if (agentApplyDamageModel.IsDamageIgnored(in attackInformation, in collisionData))
			{
				return 0f;
			}
			float num = agentApplyDamageModel.ApplyDamageAmplifications(in attackInformation, in collisionData, baseDamage);
			num = agentApplyDamageModel.ApplyDamageScaling(in attackInformation, in collisionData, num);
			num = agentApplyDamageModel.ApplyDamageReductions(in attackInformation, in collisionData, num);
			num = agentApplyDamageModel.ApplyGeneralDamageModifiers(in attackInformation, in collisionData, num);
			return MathF.Max(0f, num);
		}

		// Token: 0x0600375B RID: 14171
		public abstract bool IsDamageIgnored(in AttackInformation attackInformation, in AttackCollisionData collisionData);

		// Token: 0x0600375C RID: 14172
		public abstract float ApplyDamageAmplifications(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage);

		// Token: 0x0600375D RID: 14173
		public abstract float ApplyDamageScaling(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage);

		// Token: 0x0600375E RID: 14174
		public abstract float ApplyDamageReductions(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage);

		// Token: 0x0600375F RID: 14175
		public abstract float ApplyGeneralDamageModifiers(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage);

		// Token: 0x06003760 RID: 14176
		public abstract void DecideMissileWeaponFlags(Agent attackerAgent, in MissionWeapon missileWeapon, ref WeaponFlags missileWeaponFlags);

		// Token: 0x06003761 RID: 14177
		public abstract void CalculateDefendedBlowStunMultipliers(Agent attackerAgent, Agent defenderAgent, CombatCollisionResult collisionResult, WeaponComponentData attackerWeapon, WeaponComponentData defenderWeapon, ref float attackerStunPeriod, ref float defenderStunPeriod);

		// Token: 0x06003762 RID: 14178
		public abstract float CalculateStaggerThresholdDamage(Agent defenderAgent, in Blow blow);

		// Token: 0x06003763 RID: 14179
		public abstract float CalculateAlternativeAttackDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, WeaponComponentData weapon);

		// Token: 0x06003764 RID: 14180
		public abstract float CalculatePassiveAttackDamage(BasicCharacterObject attackerCharacter, in AttackCollisionData collisionData, float baseDamage);

		// Token: 0x06003765 RID: 14181
		public abstract MeleeCollisionReaction DecidePassiveAttackCollisionReaction(Agent attacker, Agent defender, bool isFatalHit);

		// Token: 0x06003766 RID: 14182
		public abstract void DecideWeaponCollisionReaction(in Blow registeredBlow, in AttackCollisionData collisionData, Agent attacker, Agent defender, in MissionWeapon attackerWeapon, bool isFatalHit, bool isShruggedOff, float momentumRemaining, out MeleeCollisionReaction colReaction);

		// Token: 0x06003767 RID: 14183
		public abstract float CalculateShieldDamage(in AttackInformation attackInformation, float baseDamage);

		// Token: 0x06003768 RID: 14184
		public abstract float CalculateSailFireDamage(Agent attackerAgent, IShipOrigin shipOrigin, float baseDamage, bool damageFromShipMachine);

		// Token: 0x06003769 RID: 14185
		public abstract float CalculateHullFireDamage(float baseFireDamage, IShipOrigin shipOrigin);

		// Token: 0x0600376A RID: 14186
		public abstract float GetDamageMultiplierForBodyPart(BoneBodyPartType bodyPart, DamageTypes type, bool isHuman, bool isMissile);

		// Token: 0x0600376B RID: 14187
		public abstract bool CanWeaponIgnoreFriendlyFireChecks(WeaponComponentData weapon);

		// Token: 0x0600376C RID: 14188
		public abstract bool CanWeaponDealSneakAttack(in AttackInformation attackInformation, WeaponComponentData weapon);

		// Token: 0x0600376D RID: 14189
		public abstract bool CanWeaponDismount(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData);

		// Token: 0x0600376E RID: 14190
		public abstract bool CanWeaponKnockback(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData);

		// Token: 0x0600376F RID: 14191
		public abstract bool CanWeaponKnockDown(Agent attackerAgent, Agent victimAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData);

		// Token: 0x06003770 RID: 14192
		public abstract bool DecideCrushedThrough(Agent attackerAgent, Agent defenderAgent, float totalAttackEnergy, Agent.UsageDirection attackDirection, StrikeType strikeType, WeaponComponentData defendItem, bool isPassiveUsageHit);

		// Token: 0x06003771 RID: 14193
		public abstract float CalculateRemainingMomentum(float originalMomentum, in Blow b, in AttackCollisionData collisionData, Agent attacker, Agent victim, in MissionWeapon attackerWeapon, bool isCrushThrough);

		// Token: 0x06003772 RID: 14194 RVA: 0x000E4D0C File Offset: 0x000E2F0C
		protected float CalculateDefaultRemainingMomentum(float originalMomentum, in Blow b, in AttackCollisionData collisionData, Agent attacker, Agent victim, in MissionWeapon attackerWeapon, bool isCrushThrough)
		{
			float num = 0f;
			if (isCrushThrough)
			{
				num = originalMomentum * 0.3f;
			}
			else if (b.InflictedDamage > 0)
			{
				AttackCollisionData attackCollisionData = collisionData;
				if (!attackCollisionData.AttackBlockedWithShield)
				{
					attackCollisionData = collisionData;
					if (!attackCollisionData.CollidedWithShieldOnBack)
					{
						attackCollisionData = collisionData;
						if (attackCollisionData.IsColliderAgent)
						{
							attackCollisionData = collisionData;
							if (!attackCollisionData.IsHorseCharge)
							{
								if (attacker != null && attacker.IsDoingPassiveAttack)
								{
									num = originalMomentum * 0.5f;
								}
								else if (!MissionCombatMechanicsHelper.HitWithAnotherBone(in collisionData, attacker, in attackerWeapon))
								{
									MissionWeapon missionWeapon = attackerWeapon;
									if (!missionWeapon.IsEmpty && b.StrikeType != StrikeType.Thrust)
									{
										missionWeapon = attackerWeapon;
										if (!missionWeapon.IsEmpty)
										{
											missionWeapon = attackerWeapon;
											if (missionWeapon.CurrentUsageItem.CanHitMultipleTargets)
											{
												num = originalMomentum * (1f - b.AbsorbedByArmor / (float)b.InflictedDamage);
												num *= 0.5f;
												if (num < 0.25f)
												{
													num = 0f;
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06003773 RID: 14195
		public abstract bool DecideAgentShrugOffBlow(Agent victimAgent, in AttackCollisionData collisionData, in Blow blow);

		// Token: 0x06003774 RID: 14196
		public abstract bool DecideAgentDismountedByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow);

		// Token: 0x06003775 RID: 14197
		public abstract bool DecideAgentKnockedBackByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow);

		// Token: 0x06003776 RID: 14198
		public abstract bool DecideAgentKnockedDownByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow);

		// Token: 0x06003777 RID: 14199
		public abstract bool DecideMountRearedByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow);

		// Token: 0x06003778 RID: 14200
		public abstract bool ShouldMissilePassThroughAfterShieldBreak(Agent attackerAgent, WeaponComponentData attackerWeapon);

		// Token: 0x06003779 RID: 14201
		public abstract float GetDismountPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData);

		// Token: 0x0600377A RID: 14202
		public abstract float GetKnockBackPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData);

		// Token: 0x0600377B RID: 14203
		public abstract float GetKnockDownPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData);

		// Token: 0x0600377C RID: 14204
		public abstract float GetHorseChargePenetration();
	}
}
