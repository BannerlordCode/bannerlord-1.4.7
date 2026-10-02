using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000269 RID: 617
	public static class MissionCombatMechanicsHelper
	{
		// Token: 0x060022AD RID: 8877 RVA: 0x00078C2C File Offset: 0x00076E2C
		public static bool DecideAgentShrugOffBlow(Agent victimAgent, in AttackCollisionData collisionData, in Blow blow)
		{
			bool flag = false;
			if (victimAgent.Health - (float)collisionData.InflictedDamage >= 1f)
			{
				float num = MissionGameModels.Current.AgentApplyDamageModel.CalculateStaggerThresholdDamage(victimAgent, in blow);
				flag = (float)collisionData.InflictedDamage <= num;
			}
			return flag;
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x00078C78 File Offset: 0x00076E78
		public static bool DecideAgentDismountedByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			bool flag = false;
			int inflictedDamage = collisionData.InflictedDamage;
			bool flag2 = victimAgent.Health - (float)inflictedDamage >= 1f;
			bool flag3 = (blow.BlowFlag & BlowFlags.ShrugOff) > BlowFlags.None;
			if (attackerWeapon != null && flag2 && !flag3)
			{
				int num = (int)victimAgent.HealthLimit;
				if (MissionGameModels.Current.AgentApplyDamageModel.CanWeaponDismount(attackerAgent, attackerWeapon, in blow, in collisionData))
				{
					float dismountPenetration = MissionGameModels.Current.AgentApplyDamageModel.GetDismountPenetration(attackerAgent, attackerWeapon, in blow, in collisionData);
					float dismountResistance = MissionGameModels.Current.AgentStatCalculateModel.GetDismountResistance(victimAgent);
					flag = MissionCombatMechanicsHelper.DecideCombatEffect((float)inflictedDamage, (float)num, dismountResistance, dismountPenetration);
				}
				if (!flag)
				{
					flag = MissionCombatMechanicsHelper.DecideWeaponKnockDown(attackerAgent, victimAgent, attackerWeapon, in collisionData, in blow);
				}
			}
			return flag;
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x00078D24 File Offset: 0x00076F24
		public static bool DecideAgentKnockedBackByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			bool flag = false;
			int num = (int)victimAgent.HealthLimit;
			int inflictedDamage = collisionData.InflictedDamage;
			bool flag2 = (blow.BlowFlag & BlowFlags.ShrugOff) > BlowFlags.None;
			AttackCollisionData attackCollisionData = collisionData;
			if (attackCollisionData.IsHorseCharge)
			{
				Vec3 position = victimAgent.Position;
				Vec2 movementDirection = attackerAgent.GetMovementDirection();
				attackCollisionData = collisionData;
				Vec3 collisionGlobalPosition = attackCollisionData.CollisionGlobalPosition;
				if (MissionCombatMechanicsHelper.ChargeDamageDotProduct(in position, in movementDirection, in collisionGlobalPosition) >= 0.7f)
				{
					flag = true;
				}
			}
			else
			{
				attackCollisionData = collisionData;
				if (attackCollisionData.IsAlternativeAttack)
				{
					flag = true;
				}
				else if (attackerWeapon != null && !flag2 && MissionGameModels.Current.AgentApplyDamageModel.CanWeaponKnockback(attackerAgent, attackerWeapon, in blow, in collisionData))
				{
					float knockBackPenetration = MissionGameModels.Current.AgentApplyDamageModel.GetKnockBackPenetration(attackerAgent, attackerWeapon, in blow, in collisionData);
					float knockBackResistance = MissionGameModels.Current.AgentStatCalculateModel.GetKnockBackResistance(victimAgent);
					flag = MissionCombatMechanicsHelper.DecideCombatEffect((float)inflictedDamage, (float)num, knockBackResistance, knockBackPenetration);
				}
			}
			return flag;
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x00078E04 File Offset: 0x00077004
		public static bool DecideAgentKnockedDownByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			bool flag = false;
			if ((blow.BlowFlag & BlowFlags.ShrugOff) <= BlowFlags.None)
			{
				int num = (int)victimAgent.HealthLimit;
				float num2 = (float)collisionData.InflictedDamage;
				bool flag2 = (blow.BlowFlag & BlowFlags.KnockBack) > BlowFlags.None;
				AttackCollisionData attackCollisionData = collisionData;
				if (attackCollisionData.IsHorseCharge && flag2)
				{
					float horseChargePenetration = MissionGameModels.Current.AgentApplyDamageModel.GetHorseChargePenetration();
					float knockDownResistance = MissionGameModels.Current.AgentStatCalculateModel.GetKnockDownResistance(victimAgent, StrikeType.Invalid);
					flag = MissionCombatMechanicsHelper.DecideCombatEffect(num2, (float)num, knockDownResistance, horseChargePenetration);
				}
				else if (attackerWeapon != null)
				{
					flag = MissionCombatMechanicsHelper.DecideWeaponKnockDown(attackerAgent, victimAgent, attackerWeapon, in collisionData, in blow);
				}
			}
			return flag;
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x00078E9C File Offset: 0x0007709C
		public static bool DecideMountRearedByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			float damageMultiplierOfCombatDifficulty = Mission.Current.GetDamageMultiplierOfCombatDifficulty(victimAgent, attackerAgent);
			if (attackerWeapon != null && attackerWeapon.WeaponFlags.HasAnyFlag(WeaponFlags.WideGrip) && attackerWeapon.WeaponLength > 120 && blow.StrikeType == StrikeType.Thrust)
			{
				AttackCollisionData attackCollisionData = collisionData;
				if (attackCollisionData.ThrustTipHit && attackerAgent != null && !attackerAgent.HasMount && victimAgent.GetAgentFlags().HasAnyFlag(AgentFlag.CanRear) && victimAgent.MovementVelocity.y > 5f && Vec3.DotProduct(blow.Direction, victimAgent.Frame.rotation.f) < -0.35f)
				{
					Vec3 globalPosition = blow.GlobalPosition;
					if (Vec2.DotProduct(globalPosition.AsVec2 - victimAgent.Position.AsVec2, victimAgent.GetMovementDirection()) > 0f)
					{
						return (float)collisionData.InflictedDamage >= ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.MakesRearAttackDamageThreshold) * damageMultiplierOfCombatDifficulty;
					}
				}
			}
			return false;
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x00078FA4 File Offset: 0x000771A4
		public static void DecideWeaponCollisionReaction(in Blow registeredBlow, in AttackCollisionData collisionData, Agent attacker, Agent defender, in MissionWeapon attackerWeapon, bool isFatalHit, bool isShruggedOff, float momentumRemaining, out MeleeCollisionReaction colReaction)
		{
			AttackCollisionData attackCollisionData = collisionData;
			if (attackCollisionData.IsColliderAgent)
			{
				attackCollisionData = collisionData;
				if (attackCollisionData.StrikeType == 1)
				{
					attackCollisionData = collisionData;
					if (attackCollisionData.CollisionHitResultFlags.HasAnyFlag(CombatHitResultFlags.HitWithStartOfTheAnimation))
					{
						colReaction = MeleeCollisionReaction.Staggered;
						return;
					}
				}
			}
			attackCollisionData = collisionData;
			if (!attackCollisionData.IsColliderAgent)
			{
				attackCollisionData = collisionData;
				if (attackCollisionData.PhysicsMaterialIndex != -1)
				{
					attackCollisionData = collisionData;
					if (PhysicsMaterial.GetFromIndex(attackCollisionData.PhysicsMaterialIndex).GetFlags().HasAnyFlag(PhysicsMaterialFlags.AttacksCanPassThrough))
					{
						colReaction = MeleeCollisionReaction.SlicedThrough;
						return;
					}
				}
			}
			attackCollisionData = collisionData;
			if (!attackCollisionData.IsColliderAgent || registeredBlow.InflictedDamage <= 0)
			{
				colReaction = MeleeCollisionReaction.Bounced;
				return;
			}
			attackCollisionData = collisionData;
			if (attackCollisionData.StrikeType == 1 && attacker.IsDoingPassiveAttack)
			{
				colReaction = MissionGameModels.Current.AgentApplyDamageModel.DecidePassiveAttackCollisionReaction(attacker, defender, isFatalHit);
				return;
			}
			attackCollisionData = collisionData;
			if (attackCollisionData.IsAlternativeAttack && momentumRemaining > 0f)
			{
				colReaction = MeleeCollisionReaction.ContinueChecking;
				return;
			}
			if (MissionCombatMechanicsHelper.HitWithAnotherBone(in collisionData, attacker, in attackerWeapon))
			{
				colReaction = MeleeCollisionReaction.Bounced;
				return;
			}
			MissionWeapon missionWeapon = attackerWeapon;
			WeaponClass weaponClass;
			if (missionWeapon.IsEmpty)
			{
				weaponClass = WeaponClass.Undefined;
			}
			else
			{
				missionWeapon = attackerWeapon;
				weaponClass = missionWeapon.CurrentUsageItem.WeaponClass;
			}
			WeaponClass weaponClass2 = weaponClass;
			missionWeapon = attackerWeapon;
			if (missionWeapon.IsEmpty || isFatalHit || !isShruggedOff)
			{
				missionWeapon = attackerWeapon;
				if (missionWeapon.IsEmpty && defender != null && defender.IsHuman)
				{
					attackCollisionData = collisionData;
					if (!attackCollisionData.IsAlternativeAttack)
					{
						attackCollisionData = collisionData;
						if (attackCollisionData.VictimHitBodyPart == BoneBodyPartType.Chest)
						{
							goto IL_01CC;
						}
						attackCollisionData = collisionData;
						if (attackCollisionData.VictimHitBodyPart == BoneBodyPartType.ShoulderLeft)
						{
							goto IL_01CC;
						}
						attackCollisionData = collisionData;
						if (attackCollisionData.VictimHitBodyPart == BoneBodyPartType.ShoulderRight)
						{
							goto IL_01CC;
						}
						attackCollisionData = collisionData;
						if (attackCollisionData.VictimHitBodyPart == BoneBodyPartType.Abdomen)
						{
							goto IL_01CC;
						}
						attackCollisionData = collisionData;
						if (attackCollisionData.VictimHitBodyPart == BoneBodyPartType.Legs)
						{
							goto IL_01CC;
						}
					}
				}
				if ((weaponClass2 != WeaponClass.OneHandedAxe && weaponClass2 != WeaponClass.TwoHandedAxe) || isFatalHit || (float)collisionData.InflictedDamage >= defender.HealthLimit * 0.5f)
				{
					missionWeapon = attackerWeapon;
					if (missionWeapon.IsEmpty)
					{
						attackCollisionData = collisionData;
						if (!attackCollisionData.IsAlternativeAttack)
						{
							attackCollisionData = collisionData;
							if (attackCollisionData.AttackDirection == Agent.UsageDirection.AttackUp)
							{
								goto IL_026F;
							}
						}
					}
					attackCollisionData = collisionData;
					if (attackCollisionData.ThrustTipHit)
					{
						attackCollisionData = collisionData;
						if (attackCollisionData.DamageType == 1)
						{
							missionWeapon = attackerWeapon;
							if (!missionWeapon.IsEmpty)
							{
								attackCollisionData = collisionData;
								if (defender.CanThrustAttackStickToBone(attackCollisionData.VictimHitBodyPart))
								{
									goto IL_026F;
								}
							}
						}
					}
					colReaction = MeleeCollisionReaction.SlicedThrough;
					goto IL_0279;
				}
				IL_026F:
				colReaction = MeleeCollisionReaction.Stuck;
				goto IL_0279;
			}
			IL_01CC:
			colReaction = MeleeCollisionReaction.Bounced;
			IL_0279:
			attackCollisionData = collisionData;
			if (!attackCollisionData.AttackBlockedWithShield)
			{
				attackCollisionData = collisionData;
				if (!attackCollisionData.CollidedWithShieldOnBack)
				{
					return;
				}
			}
			if (colReaction == MeleeCollisionReaction.SlicedThrough)
			{
				colReaction = MeleeCollisionReaction.Bounced;
			}
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x00079254 File Offset: 0x00077454
		public static bool IsCollisionBoneDifferentThanWeaponAttachBone(in AttackCollisionData collisionData, int weaponAttachBoneIndex)
		{
			AttackCollisionData attackCollisionData = collisionData;
			if (attackCollisionData.AttackBoneIndex != -1 && weaponAttachBoneIndex != -1)
			{
				attackCollisionData = collisionData;
				return weaponAttachBoneIndex != (int)attackCollisionData.AttackBoneIndex;
			}
			return false;
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x0007928C File Offset: 0x0007748C
		public static bool DecideSweetSpotCollision(in AttackCollisionData collisionData)
		{
			AttackCollisionData attackCollisionData = collisionData;
			if (attackCollisionData.AttackProgress >= 0.22f)
			{
				attackCollisionData = collisionData;
				return attackCollisionData.AttackProgress <= 0.55f;
			}
			return false;
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x000792C8 File Offset: 0x000774C8
		public static void GetAttackCollisionResults(in AttackInformation attackInformation, bool crushedThrough, float momentumRemaining, bool cancelDamage, ref AttackCollisionData attackCollisionData, out CombatLogData combatLog, out int speedBonus)
		{
			float num = 0f;
			if (attackCollisionData.IsMissile)
			{
				num = (attackCollisionData.MissileStartingPosition - attackCollisionData.CollisionGlobalPosition).Length;
			}
			combatLog = new CombatLogData(attackInformation.IsVictimAgentSameWithAttackerAgent, attackInformation.IsAttackerAgentHuman, attackInformation.IsAttackerAgentMine, attackInformation.DoesAttackerHaveRiderAgent, attackInformation.IsAttackerAgentRiderAgentMine, attackInformation.IsAttackerAgentMount, attackInformation.IsVictimAgentHuman, attackInformation.IsVictimAgentMine, false, attackInformation.DoesVictimHaveRiderAgent, attackInformation.IsVictimAgentRiderAgentMine, attackInformation.IsVictimAgentMount, null, attackInformation.IsVictimRiderAgentSameAsAttackerAgent, false, false, num);
			bool flag = MissionCombatMechanicsHelper.IsCollisionBoneDifferentThanWeaponAttachBone(in attackCollisionData, attackInformation.WeaponAttachBoneIndex);
			Vec2 agentVelocityContribution = MissionCombatMechanicsHelper.GetAgentVelocityContribution(attackInformation.DoesAttackerHaveMountAgent, attackInformation.AttackerAgentMovementVelocity, attackInformation.AttackerAgentMountMovementDirection, attackInformation.AttackerMovementDirectionAsAngle);
			Vec2 agentVelocityContribution2 = MissionCombatMechanicsHelper.GetAgentVelocityContribution(attackInformation.DoesVictimHaveMountAgent, attackInformation.VictimAgentMovementVelocity, attackInformation.VictimAgentMountMovementDirection, attackInformation.VictimMovementDirectionAsAngle);
			if (attackCollisionData.IsColliderAgent)
			{
				combatLog.IsRangedAttack = attackCollisionData.IsMissile;
				combatLog.HitSpeed = (attackCollisionData.IsMissile ? (agentVelocityContribution2.ToVec3(0f) - attackCollisionData.MissileVelocity).Length : (agentVelocityContribution - agentVelocityContribution2).Length);
			}
			float baseMagnitude;
			MissionCombatMechanicsHelper.ComputeBlowMagnitude(in attackCollisionData, in attackInformation, momentumRemaining, cancelDamage, flag, agentVelocityContribution, agentVelocityContribution2, out attackCollisionData.BaseMagnitude, out baseMagnitude, out attackCollisionData.MovementSpeedDamageModifier, out speedBonus);
			MissionWeapon missionWeapon = attackInformation.AttackerWeapon;
			DamageTypes damageTypes = (DamageTypes)((missionWeapon.IsEmpty || flag || attackCollisionData.IsAlternativeAttack || attackCollisionData.IsFallDamage || attackCollisionData.IsHorseCharge) ? 2 : attackCollisionData.DamageType);
			combatLog.DamageType = damageTypes;
			if (!attackCollisionData.IsColliderAgent && attackCollisionData.EntityExists)
			{
				bool flag2 = PhysicsMaterial.GetFromIndex(attackCollisionData.PhysicsMaterialIndex).GetFlags().HasAnyFlag(PhysicsMaterialFlags.Flammable);
				float baseMagnitude2 = attackCollisionData.BaseMagnitude;
				bool isAttackerAgentDoingPassiveAttack = attackInformation.IsAttackerAgentDoingPassiveAttack;
				missionWeapon = attackInformation.AttackerWeapon;
				attackCollisionData.BaseMagnitude = baseMagnitude2 * MissionCombatMechanicsHelper.GetEntityDamageMultiplier(isAttackerAgentDoingPassiveAttack, missionWeapon.CurrentUsageItem, damageTypes, flag2);
				attackCollisionData.InflictedDamage = MBMath.ClampInt((int)attackCollisionData.BaseMagnitude, 0, 2000);
				combatLog.InflictedDamage = attackCollisionData.InflictedDamage;
			}
			if (attackCollisionData.IsColliderAgent && !attackInformation.IsVictimAgentNull)
			{
				if (attackCollisionData.IsAlternativeAttack)
				{
					baseMagnitude = attackCollisionData.BaseMagnitude;
				}
				if (attackCollisionData.AttackBlockedWithShield)
				{
					missionWeapon = attackInformation.AttackerWeapon;
					MissionCombatMechanicsHelper.ComputeBlowDamageOnShield(in attackInformation, in attackCollisionData, missionWeapon.CurrentUsageItem, attackCollisionData.BaseMagnitude, out attackCollisionData.InflictedDamage);
					attackCollisionData.AbsorbedByArmor = attackCollisionData.InflictedDamage;
				}
				else if (attackCollisionData.MissileBlockedWithWeapon)
				{
					attackCollisionData.InflictedDamage = 0;
					attackCollisionData.AbsorbedByArmor = 0;
				}
				else
				{
					missionWeapon = attackInformation.AttackerWeapon;
					bool flag3;
					MissionCombatMechanicsHelper.ComputeBlowDamage(in attackInformation, in attackCollisionData, missionWeapon.CurrentUsageItem, damageTypes, baseMagnitude, speedBonus, cancelDamage, out attackCollisionData.InflictedDamage, out attackCollisionData.AbsorbedByArmor, out flag3);
					attackCollisionData.IsSneakAttack = flag3;
					combatLog.IsSneakAttack = flag3;
				}
				combatLog.InflictedDamage = attackCollisionData.InflictedDamage;
				combatLog.AbsorbedDamage = attackCollisionData.AbsorbedByArmor;
				combatLog.AttackProgress = attackCollisionData.AttackProgress;
			}
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x000795D0 File Offset: 0x000777D0
		internal static void GetDefendCollisionResults(Agent attackerAgent, Agent defenderAgent, CombatCollisionResult collisionResult, int attackerWeaponSlotIndex, bool isAlternativeAttack, StrikeType strikeType, Agent.UsageDirection attackDirection, float collisionDistanceOnWeapon, float attackProgress, bool attackIsParried, bool isPassiveUsageHit, bool isHeavyAttack, ref float defenderStunPeriod, ref float attackerStunPeriod, ref bool crushedThrough, ref bool chamber)
		{
			MissionWeapon missionWeapon = ((attackerWeaponSlotIndex >= 0) ? attackerAgent.Equipment[attackerWeaponSlotIndex] : MissionWeapon.Invalid);
			WeaponComponentData weaponComponentData = (missionWeapon.IsEmpty ? null : missionWeapon.CurrentUsageItem);
			EquipmentIndex equipmentIndex = defenderAgent.GetOffhandWieldedItemIndex();
			if (equipmentIndex == EquipmentIndex.None)
			{
				equipmentIndex = defenderAgent.GetPrimaryWieldedItemIndex();
			}
			ItemObject itemObject = ((equipmentIndex != EquipmentIndex.None) ? defenderAgent.Equipment[equipmentIndex].Item : null);
			WeaponComponentData weaponComponentData2 = ((equipmentIndex != EquipmentIndex.None) ? defenderAgent.Equipment[equipmentIndex].CurrentUsageItem : null);
			float num = 10f;
			attackerStunPeriod = ((strikeType == StrikeType.Thrust) ? ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.StunPeriodAttackerThrust) : ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.StunPeriodAttackerSwing));
			chamber = false;
			if (!missionWeapon.IsEmpty)
			{
				float z = attackerAgent.GetCurWeaponOffset().z;
				float realWeaponLength = weaponComponentData.GetRealWeaponLength();
				float num2 = realWeaponLength + z;
				float num3 = MBMath.ClampFloat((0.2f + collisionDistanceOnWeapon) / num2, 0.1f, 0.98f);
				float num4 = MissionCombatMechanicsHelper.ComputeRelativeSpeedDiffOfAgents(attackerAgent, defenderAgent);
				float num5;
				if (strikeType == StrikeType.Thrust)
				{
					num5 = CombatStatCalculator.CalculateBaseBlowMagnitudeForThrust((float)missionWeapon.GetModifiedThrustSpeedForCurrentUsage() / 11.764706f * MissionCombatMechanicsHelper.SpeedGraphFunction(attackProgress, strikeType, attackDirection), missionWeapon.Item.Weight, num4);
				}
				else
				{
					num5 = CombatStatCalculator.CalculateBaseBlowMagnitudeForSwing((float)missionWeapon.GetModifiedSwingSpeedForCurrentUsage() / 4.5454545f * MissionCombatMechanicsHelper.SpeedGraphFunction(attackProgress, strikeType, attackDirection), realWeaponLength, missionWeapon.Item.Weight, weaponComponentData.TotalInertia, weaponComponentData.CenterOfMass, num3, num4);
				}
				if (strikeType == StrikeType.Thrust)
				{
					num5 *= 0.8f;
				}
				else if (attackDirection == Agent.UsageDirection.AttackUp)
				{
					num5 *= 1.25f;
				}
				else if (isHeavyAttack)
				{
					num5 *= ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.HeavyAttackMomentumMultiplier);
				}
				num += num5;
			}
			float num6 = 1f;
			defenderStunPeriod = num * ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.StunMomentumTransferFactor);
			if (weaponComponentData2 != null)
			{
				if (weaponComponentData2.IsShield)
				{
					float managedParameter = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.StunDefendWeaponWeightOffsetShield);
					num6 += managedParameter * itemObject.Weight;
				}
				else
				{
					num6 = 0.9f;
					float managedParameter2 = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.StunDefendWeaponWeightMultiplierWeaponWeight);
					num6 += managedParameter2 * itemObject.Weight;
					ItemObject.ItemTypeEnum itemType = itemObject.ItemType;
					if (itemType == ItemObject.ItemTypeEnum.TwoHandedWeapon)
					{
						num6 += ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.StunDefendWeaponWeightBonusTwoHanded);
					}
					else if (itemType == ItemObject.ItemTypeEnum.Polearm)
					{
						num6 += ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.StunDefendWeaponWeightBonusPolearm);
					}
				}
				if (collisionResult == CombatCollisionResult.Parried)
				{
					attackerStunPeriod += MathF.Min(0.15f, 0.12f * num6);
					num6 += ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.StunDefendWeaponWeightBonusActiveBlocked);
				}
				else if (collisionResult == CombatCollisionResult.ChamberBlocked)
				{
					attackerStunPeriod += MathF.Min(0.25f, 0.25f * num6);
					num6 += ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.StunDefendWeaponWeightBonusChamberBlocked);
					chamber = true;
				}
			}
			if (!defenderAgent.GetIsLeftStance())
			{
				num6 += ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.StunDefendWeaponWeightBonusRightStance);
			}
			defenderStunPeriod /= num6;
			MissionGameModels.Current.AgentApplyDamageModel.CalculateDefendedBlowStunMultipliers(attackerAgent, defenderAgent, collisionResult, weaponComponentData, weaponComponentData2, ref attackerStunPeriod, ref defenderStunPeriod);
			float managedParameter3 = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.StunPeriodMax);
			attackerStunPeriod = MathF.Min(attackerStunPeriod, managedParameter3);
			defenderStunPeriod = MathF.Min(defenderStunPeriod, managedParameter3);
			crushedThrough = !chamber && MissionGameModels.Current.AgentApplyDamageModel.DecideCrushedThrough(attackerAgent, defenderAgent, num, attackDirection, strikeType, weaponComponentData2, isPassiveUsageHit);
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x00079914 File Offset: 0x00077B14
		public static void UpdateMomentumRemaining(ref float momentumRemaining, in Blow b, in AttackCollisionData collisionData, Agent attacker, Agent victim, in MissionWeapon attackerWeapon, bool isCrushThrough)
		{
			momentumRemaining = MissionGameModels.Current.AgentApplyDamageModel.CalculateRemainingMomentum(momentumRemaining, in b, in collisionData, attacker, victim, in attackerWeapon, isCrushThrough);
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x00079940 File Offset: 0x00077B40
		public static bool HitWithAnotherBone(in AttackCollisionData collisionData, Agent attacker, in MissionWeapon attackerWeapon)
		{
			MissionWeapon missionWeapon = attackerWeapon;
			int num;
			if (missionWeapon.IsEmpty || attacker == null || !attacker.IsHuman)
			{
				num = -1;
			}
			else
			{
				Monster monster = attacker.Monster;
				missionWeapon = attackerWeapon;
				num = (int)monster.GetBoneToAttachForItemFlags(missionWeapon.Item.ItemFlags);
			}
			int num2 = num;
			return MissionCombatMechanicsHelper.IsCollisionBoneDifferentThanWeaponAttachBone(in collisionData, num2);
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x00079994 File Offset: 0x00077B94
		private static bool DecideWeaponKnockDown(Agent attackerAgent, Agent victimAgent, WeaponComponentData attackerWeapon, in AttackCollisionData collisionData, in Blow blow)
		{
			if (MissionGameModels.Current.AgentApplyDamageModel.CanWeaponKnockDown(attackerAgent, victimAgent, attackerWeapon, in blow, in collisionData))
			{
				float knockDownPenetration = MissionGameModels.Current.AgentApplyDamageModel.GetKnockDownPenetration(attackerAgent, attackerWeapon, in blow, in collisionData);
				float knockDownResistance = MissionGameModels.Current.AgentStatCalculateModel.GetKnockDownResistance(victimAgent, blow.StrikeType);
				return MissionCombatMechanicsHelper.DecideCombatEffect((float)collisionData.InflictedDamage, victimAgent.HealthLimit, knockDownResistance, knockDownPenetration);
			}
			return false;
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x000799FC File Offset: 0x00077BFC
		private static bool DecideCombatEffect(float inflictedDamage, float victimMaxHealth, float victimResistance, float attackPenetration)
		{
			float num = victimMaxHealth * Math.Max(0f, victimResistance - attackPenetration);
			return inflictedDamage >= num;
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x00079A20 File Offset: 0x00077C20
		private static float ChargeDamageDotProduct(in Vec3 victimPosition, in Vec2 chargerMovementDirection, in Vec3 collisionPoint)
		{
			Vec3 vec = victimPosition;
			Vec2 asVec = vec.AsVec2;
			vec = collisionPoint;
			float num = Vec2.DotProduct((asVec - vec.AsVec2).Normalized(), chargerMovementDirection);
			return MathF.Max(0f, num);
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x00079A70 File Offset: 0x00077C70
		private static float SpeedGraphFunction(float progress, StrikeType strikeType, Agent.UsageDirection attackDir)
		{
			bool flag = strikeType == StrikeType.Thrust;
			bool flag2 = attackDir == Agent.UsageDirection.AttackUp;
			ManagedParametersEnum managedParametersEnum;
			ManagedParametersEnum managedParametersEnum2;
			ManagedParametersEnum managedParametersEnum3;
			ManagedParametersEnum managedParametersEnum4;
			if (flag)
			{
				managedParametersEnum = ManagedParametersEnum.ThrustCombatSpeedGraphZeroProgressValue;
				managedParametersEnum2 = ManagedParametersEnum.ThrustCombatSpeedGraphFirstMaximumPoint;
				managedParametersEnum3 = ManagedParametersEnum.ThrustCombatSpeedGraphSecondMaximumPoint;
				managedParametersEnum4 = ManagedParametersEnum.ThrustCombatSpeedGraphOneProgressValue;
			}
			else if (flag2)
			{
				managedParametersEnum = ManagedParametersEnum.OverSwingCombatSpeedGraphZeroProgressValue;
				managedParametersEnum2 = ManagedParametersEnum.OverSwingCombatSpeedGraphFirstMaximumPoint;
				managedParametersEnum3 = ManagedParametersEnum.OverSwingCombatSpeedGraphSecondMaximumPoint;
				managedParametersEnum4 = ManagedParametersEnum.OverSwingCombatSpeedGraphOneProgressValue;
			}
			else
			{
				managedParametersEnum = ManagedParametersEnum.SwingCombatSpeedGraphZeroProgressValue;
				managedParametersEnum2 = ManagedParametersEnum.SwingCombatSpeedGraphFirstMaximumPoint;
				managedParametersEnum3 = ManagedParametersEnum.SwingCombatSpeedGraphSecondMaximumPoint;
				managedParametersEnum4 = ManagedParametersEnum.SwingCombatSpeedGraphOneProgressValue;
			}
			float managedParameter = ManagedParameters.Instance.GetManagedParameter(managedParametersEnum);
			float managedParameter2 = ManagedParameters.Instance.GetManagedParameter(managedParametersEnum2);
			float managedParameter3 = ManagedParameters.Instance.GetManagedParameter(managedParametersEnum3);
			float managedParameter4 = ManagedParameters.Instance.GetManagedParameter(managedParametersEnum4);
			float num;
			if (progress < managedParameter2)
			{
				num = (1f - managedParameter) / managedParameter2 * progress + managedParameter;
			}
			else if (managedParameter3 < progress)
			{
				num = (managedParameter4 - 1f) / (1f - managedParameter3) * (progress - managedParameter3) + 1f;
			}
			else
			{
				num = 1f;
			}
			return num;
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x00079B32 File Offset: 0x00077D32
		private static float ConvertBaseAttackMagnitude(WeaponComponentData weapon, StrikeType strikeType, float baseMagnitude)
		{
			return baseMagnitude * ((strikeType == StrikeType.Thrust) ? weapon.ThrustDamageFactor : weapon.SwingDamageFactor);
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x00079B48 File Offset: 0x00077D48
		private static Vec2 GetAgentVelocityContribution(bool hasAgentMountAgent, Vec2 agentMovementVelocity, Vec2 agentMountMovementDirection, float agentMovementDirectionAsAngle)
		{
			Vec2 vec = Vec2.Zero;
			if (hasAgentMountAgent)
			{
				vec = agentMovementVelocity.y * agentMountMovementDirection;
			}
			else
			{
				vec = agentMovementVelocity;
				vec.RotateCCW(agentMovementDirectionAsAngle);
			}
			return vec;
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x00079B78 File Offset: 0x00077D78
		private static float GetEntityDamageMultiplier(bool isAttackerAgentDoingPassiveAttack, WeaponComponentData weapon, DamageTypes damageType, bool isFlammable)
		{
			float num = 1f;
			if (isAttackerAgentDoingPassiveAttack)
			{
				num *= 0.2f;
			}
			if (weapon != null)
			{
				if (weapon.WeaponFlags.HasAnyFlag(WeaponFlags.BonusAgainstShield))
				{
					num *= 1.2f;
				}
				switch (damageType)
				{
				case DamageTypes.Cut:
					num *= 0.8f;
					break;
				case DamageTypes.Pierce:
					num *= 0.1f;
					break;
				}
				if (isFlammable && weapon.WeaponFlags.HasAnyFlag(WeaponFlags.Burning))
				{
					num *= 1.5f;
				}
			}
			return num;
		}

		// Token: 0x060022C0 RID: 8896 RVA: 0x00079BF9 File Offset: 0x00077DF9
		private static float ComputeSpeedBonus(float baseMagnitude, float baseMagnitudeWithoutSpeedBonus)
		{
			return baseMagnitude / baseMagnitudeWithoutSpeedBonus - 1f;
		}

		// Token: 0x060022C1 RID: 8897 RVA: 0x00079C04 File Offset: 0x00077E04
		private static float ComputeRelativeSpeedDiffOfAgents(Agent agentA, Agent agentB)
		{
			Vec2 vec = Vec2.Zero;
			if (agentA.MountAgent != null)
			{
				vec = agentA.MountAgent.MovementVelocity.y * agentA.MountAgent.GetMovementDirection();
			}
			else
			{
				vec = agentA.MovementVelocity;
				vec.RotateCCW(agentA.MovementDirectionAsAngle);
			}
			Vec2 vec2 = Vec2.Zero;
			if (agentB.MountAgent != null)
			{
				vec2 = agentB.MountAgent.MovementVelocity.y * agentB.MountAgent.GetMovementDirection();
			}
			else
			{
				vec2 = agentB.MovementVelocity;
				vec2.RotateCCW(agentB.MovementDirectionAsAngle);
			}
			return (vec - vec2).Length;
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x00079CAC File Offset: 0x00077EAC
		private static void ComputeBlowDamage(in AttackInformation attackInformation, in AttackCollisionData attackCollisionData, WeaponComponentData attackerWeapon, DamageTypes damageType, float magnitude, int speedBonus, bool cancelDamage, out int inflictedDamage, out int absorbedByArmor, out bool isSneakAttack)
		{
			isSneakAttack = false;
			float armorAmountFloat = attackInformation.ArmorAmountFloat;
			WeaponComponentData shieldOnBack = attackInformation.ShieldOnBack;
			AgentFlag victimAgentFlags = attackInformation.VictimAgentFlags;
			float victimAgentAbsorbedDamageRatio = attackInformation.VictimAgentAbsorbedDamageRatio;
			float damageMultiplierOfBone = attackInformation.DamageMultiplierOfBone;
			float combatDifficultyMultiplier = attackInformation.CombatDifficultyMultiplier;
			AttackCollisionData attackCollisionData2 = attackCollisionData;
			bool attackBlockedWithShield = attackCollisionData2.AttackBlockedWithShield;
			attackCollisionData2 = attackCollisionData;
			bool collidedWithShieldOnBack = attackCollisionData2.CollidedWithShieldOnBack;
			attackCollisionData2 = attackCollisionData;
			bool isFallDamage = attackCollisionData2.IsFallDamage;
			BasicCharacterObject attackerAgentCharacter = attackInformation.AttackerAgentCharacter;
			BasicCharacterObject attackerCaptainCharacter = attackInformation.AttackerCaptainCharacter;
			BasicCharacterObject victimAgentCharacter = attackInformation.VictimAgentCharacter;
			BasicCharacterObject victimCaptainCharacter = attackInformation.VictimCaptainCharacter;
			float num = 0f;
			if (!isFallDamage)
			{
				num = MissionGameModels.Current.StrikeMagnitudeModel.CalculateAdjustedArmorForBlow(in attackInformation, in attackCollisionData, armorAmountFloat, attackerAgentCharacter, attackerCaptainCharacter, victimAgentCharacter, victimCaptainCharacter, attackerWeapon);
			}
			if (collidedWithShieldOnBack && shieldOnBack != null)
			{
				num += 10f;
			}
			float num2 = victimAgentAbsorbedDamageRatio;
			float num3 = MissionGameModels.Current.StrikeMagnitudeModel.ComputeRawDamage(damageType, magnitude, num, num2);
			float num4 = 1f;
			if (!attackBlockedWithShield && !isFallDamage)
			{
				num4 *= damageMultiplierOfBone;
				if (MissionGameModels.Current.AgentApplyDamageModel.CanWeaponDealSneakAttack(in attackInformation, attackerWeapon))
				{
					float sneakAttackMultiplier = MissionGameModels.Current.AgentStatCalculateModel.GetSneakAttackMultiplier(attackInformation.AttackerAgent, attackerWeapon);
					num4 *= sneakAttackMultiplier;
					isSneakAttack = true;
				}
				num4 *= combatDifficultyMultiplier;
			}
			num3 *= num4;
			inflictedDamage = MBMath.ClampInt(MathF.Ceiling(num3), 0, 2000);
			int num5 = MBMath.ClampInt(MathF.Ceiling(MissionGameModels.Current.StrikeMagnitudeModel.ComputeRawDamage(damageType, magnitude, 0f, num2) * num4), 0, 2000);
			absorbedByArmor = num5 - inflictedDamage;
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x00079E30 File Offset: 0x00078030
		private static void ComputeBlowDamageOnShield(in AttackInformation attackInformation, in AttackCollisionData attackCollisionData, WeaponComponentData attackerWeapon, float blowMagnitude, out int inflictedDamage)
		{
			inflictedDamage = 0;
			MissionWeapon victimShield = attackInformation.VictimShield;
			if (victimShield.CurrentUsageItem.WeaponFlags.HasAnyFlag(WeaponFlags.CanBlockRanged) && attackInformation.CanGiveDamageToAgentShield)
			{
				AttackCollisionData attackCollisionData2 = attackCollisionData;
				DamageTypes damageType = (DamageTypes)attackCollisionData2.DamageType;
				int getModifiedArmorForCurrentUsage = victimShield.GetGetModifiedArmorForCurrentUsage();
				float num = 1f;
				float num2 = MissionGameModels.Current.StrikeMagnitudeModel.ComputeRawDamage(damageType, blowMagnitude, (float)getModifiedArmorForCurrentUsage, num);
				attackCollisionData2 = attackCollisionData;
				if (attackCollisionData2.IsMissile)
				{
					if (attackerWeapon.WeaponClass == WeaponClass.ThrowingAxe)
					{
						num2 *= 0.3f;
					}
					else if (attackerWeapon.WeaponClass == WeaponClass.Javelin)
					{
						num2 *= 0.5f;
					}
					else if (attackerWeapon.WeaponFlags.HasAnyFlag(WeaponFlags.CanPenetrateShield) && attackerWeapon.WeaponFlags.HasAnyFlag(WeaponFlags.MultiplePenetration))
					{
						num2 *= 0.5f;
					}
					else
					{
						num2 *= 0.15f;
					}
				}
				else
				{
					attackCollisionData2 = attackCollisionData;
					switch (attackCollisionData2.DamageType)
					{
					case 0:
					case 2:
						num2 *= 0.7f;
						break;
					case 1:
						num2 *= 0.5f;
						break;
					}
				}
				if (attackerWeapon != null && attackerWeapon.WeaponFlags.HasAnyFlag(WeaponFlags.BonusAgainstShield))
				{
					num2 *= 2f;
				}
				if (num2 > 0f)
				{
					if (!attackInformation.IsVictimAgentLeftStance)
					{
						num2 *= ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.ShieldRightStanceBlockDamageMultiplier);
					}
					attackCollisionData2 = attackCollisionData;
					if (attackCollisionData2.CorrectSideShieldBlock)
					{
						num2 *= ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.ShieldCorrectSideBlockDamageMultiplier);
					}
					num2 = MissionGameModels.Current.AgentApplyDamageModel.CalculateShieldDamage(in attackInformation, num2);
					inflictedDamage = (int)num2;
				}
			}
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x00079FDC File Offset: 0x000781DC
		public static float CalculateBaseMeleeBlowMagnitude(in AttackInformation attackInformation, in AttackCollisionData collisionData, StrikeType strikeType, float progressEffect, float impactPointAsPercent, float exraLinearSpeed)
		{
			MissionWeapon missionWeapon = attackInformation.AttackerWeapon;
			WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
			float num = MathF.Sqrt(progressEffect);
			float num3;
			if (strikeType == StrikeType.Thrust)
			{
				exraLinearSpeed *= 0.5f;
				missionWeapon = attackInformation.AttackerWeapon;
				float num2 = (float)missionWeapon.GetModifiedThrustSpeedForCurrentUsage() / 11.764706f * num;
				num3 = MissionGameModels.Current.StrikeMagnitudeModel.CalculateStrikeMagnitudeForThrust(in attackInformation, in collisionData, in attackInformation.AttackerWeapon, num2, exraLinearSpeed, false);
			}
			else
			{
				exraLinearSpeed *= 0.7f;
				missionWeapon = attackInformation.AttackerWeapon;
				float num4 = (float)missionWeapon.GetModifiedSwingSpeedForCurrentUsage() / 4.5454545f * num;
				float num5 = MBMath.ClampFloat(0.4f / currentUsageItem.GetRealWeaponLength(), 0f, 1f);
				float num6 = MathF.Min(0.93f, impactPointAsPercent);
				float num7 = MathF.Min(0.93f, impactPointAsPercent + num5);
				float num8 = 0f;
				for (int i = 0; i < 5; i++)
				{
					float num9 = num6 + (float)i / 4f * (num7 - num6);
					float num10 = MissionGameModels.Current.StrikeMagnitudeModel.CalculateStrikeMagnitudeForSwing(in attackInformation, in collisionData, in attackInformation.AttackerWeapon, num4, num9, exraLinearSpeed);
					if (num8 < num10)
					{
						num8 = num10;
					}
				}
				num3 = num8;
			}
			return num3;
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x0007A100 File Offset: 0x00078300
		private static void ComputeBlowMagnitude(in AttackCollisionData acd, in AttackInformation attackInformation, float momentumRemaining, bool cancelDamage, bool hitWithAnotherBone, Vec2 attackerVelocity, Vec2 victimVelocity, out float baseMagnitude, out float specialMagnitude, out float movementSpeedDamageModifier, out int speedBonusInt)
		{
			AttackCollisionData attackCollisionData = acd;
			StrikeType strikeType = (StrikeType)attackCollisionData.StrikeType;
			attackCollisionData = acd;
			Agent.UsageDirection attackDirection = attackCollisionData.AttackDirection;
			bool flag = !attackInformation.IsAttackerAgentNull && attackInformation.IsAttackerAgentHuman && attackInformation.IsAttackerAgentActive && attackInformation.IsAttackerAgentDoingPassiveAttack;
			movementSpeedDamageModifier = 0f;
			speedBonusInt = 0;
			attackCollisionData = acd;
			if (attackCollisionData.IsMissile)
			{
				MissionCombatMechanicsHelper.ComputeBlowMagnitudeMissile(in attackInformation, in acd, momentumRemaining, in victimVelocity, out baseMagnitude, out specialMagnitude);
			}
			else
			{
				attackCollisionData = acd;
				if (attackCollisionData.IsFallDamage)
				{
					MissionCombatMechanicsHelper.ComputeBlowMagnitudeFromFall(in attackInformation, in acd, out baseMagnitude, out specialMagnitude);
				}
				else
				{
					attackCollisionData = acd;
					if (attackCollisionData.IsHorseCharge)
					{
						MissionCombatMechanicsHelper.ComputeBlowMagnitudeFromHorseCharge(in attackInformation, in acd, attackerVelocity, victimVelocity, out baseMagnitude, out specialMagnitude);
					}
					else
					{
						MissionCombatMechanicsHelper.ComputeBlowMagnitudeMelee(in attackInformation, in acd, momentumRemaining, cancelDamage, hitWithAnotherBone, strikeType, attackDirection, flag, attackerVelocity, victimVelocity, out baseMagnitude, out specialMagnitude, out movementSpeedDamageModifier, out speedBonusInt);
					}
				}
			}
			specialMagnitude = MBMath.ClampFloat(specialMagnitude, 0f, 500f);
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x0007A1E8 File Offset: 0x000783E8
		private static void ComputeBlowMagnitudeMelee(in AttackInformation attackInformation, in AttackCollisionData collisionData, float momentumRemaining, bool cancelDamage, bool hitWithAnotherBone, StrikeType strikeType, Agent.UsageDirection attackDirection, bool attackerIsDoingPassiveAttack, Vec2 attackerVelocity, Vec2 victimVelocity, out float baseMagnitude, out float specialMagnitude, out float movementSpeedDamageModifier, out int speedBonusInt)
		{
			Vec3 attackerAgentCurrentWeaponOffset = attackInformation.AttackerAgentCurrentWeaponOffset;
			movementSpeedDamageModifier = 0f;
			speedBonusInt = 0;
			BasicCharacterObject attackerAgentCharacter = attackInformation.AttackerAgentCharacter;
			AttackCollisionData attackCollisionData = collisionData;
			MissionWeapon missionWeapon;
			if (attackCollisionData.IsAlternativeAttack)
			{
				missionWeapon = attackInformation.AttackerWeapon;
				WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
				baseMagnitude = MissionGameModels.Current.AgentApplyDamageModel.CalculateAlternativeAttackDamage(in attackInformation, in collisionData, currentUsageItem);
				baseMagnitude *= momentumRemaining;
				specialMagnitude = baseMagnitude;
				return;
			}
			attackCollisionData = collisionData;
			Vec3 weaponBlowDir = attackCollisionData.WeaponBlowDir;
			Vec2 vec = attackerVelocity - victimVelocity;
			float num = vec.Normalize();
			float num2 = Vec2.DotProduct(weaponBlowDir.AsVec2, vec);
			if (num2 > 0f)
			{
				num2 += 0.2f;
				num2 = MathF.Min(num2, 1f);
			}
			float num3 = num * num2;
			missionWeapon = attackInformation.AttackerWeapon;
			if (missionWeapon.IsEmpty)
			{
				attackCollisionData = collisionData;
				float num4 = MissionCombatMechanicsHelper.SpeedGraphFunction(attackCollisionData.AttackProgress, strikeType, attackDirection);
				baseMagnitude = MissionGameModels.Current.StrikeMagnitudeModel.CalculateStrikeMagnitudeForUnarmedAttack(in attackInformation, in collisionData, num4, momentumRemaining);
				specialMagnitude = baseMagnitude;
				return;
			}
			float z = attackerAgentCurrentWeaponOffset.z;
			missionWeapon = attackInformation.AttackerWeapon;
			WeaponComponentData currentUsageItem2 = missionWeapon.CurrentUsageItem;
			float num5 = currentUsageItem2.GetRealWeaponLength() + z;
			attackCollisionData = collisionData;
			float num6 = MBMath.ClampFloat(attackCollisionData.CollisionDistanceOnWeapon, -0.2f, num5) / num5;
			if (attackerIsDoingPassiveAttack)
			{
				if (!attackInformation.DoesAttackerHaveMountAgent && !attackInformation.DoesVictimHaveMountAgent && !attackInformation.IsVictimAgentMount)
				{
					baseMagnitude = 0f;
				}
				else
				{
					baseMagnitude = MissionGameModels.Current.StrikeMagnitudeModel.CalculateBaseBlowMagnitudeForPassiveUsage(in attackInformation, in collisionData, num3);
				}
				baseMagnitude = MissionGameModels.Current.AgentApplyDamageModel.CalculatePassiveAttackDamage(attackerAgentCharacter, in collisionData, baseMagnitude);
			}
			else
			{
				attackCollisionData = collisionData;
				float num7 = MissionCombatMechanicsHelper.SpeedGraphFunction(attackCollisionData.AttackProgress, strikeType, attackDirection);
				baseMagnitude = MissionCombatMechanicsHelper.CalculateBaseMeleeBlowMagnitude(in attackInformation, in collisionData, strikeType, num7, num6, num3);
				if (baseMagnitude >= 0f && num7 > 0.7f)
				{
					float num8 = MissionCombatMechanicsHelper.CalculateBaseMeleeBlowMagnitude(in attackInformation, in collisionData, strikeType, num7, num6, 0f);
					movementSpeedDamageModifier = MissionCombatMechanicsHelper.ComputeSpeedBonus(baseMagnitude, num8);
					speedBonusInt = MathF.Round(100f * movementSpeedDamageModifier);
					speedBonusInt = MBMath.ClampInt(speedBonusInt, -1000, 1000);
				}
			}
			baseMagnitude *= momentumRemaining;
			float num9 = 1f;
			if (hitWithAnotherBone)
			{
				if (strikeType == StrikeType.Thrust)
				{
					num9 = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.ThrustHitWithArmDamageMultiplier);
				}
				else
				{
					num9 = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.SwingHitWithArmDamageMultiplier);
				}
			}
			else if (strikeType == StrikeType.Thrust)
			{
				attackCollisionData = collisionData;
				if (!attackCollisionData.ThrustTipHit)
				{
					attackCollisionData = collisionData;
					if (!attackCollisionData.AttackBlockedWithShield)
					{
						num9 = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.NonTipThrustHitDamageMultiplier);
					}
				}
			}
			baseMagnitude *= num9;
			if (attackInformation.AttackerAgent != null)
			{
				float weaponDamageMultiplier = MissionGameModels.Current.AgentStatCalculateModel.GetWeaponDamageMultiplier(attackInformation.AttackerAgent, currentUsageItem2);
				baseMagnitude *= weaponDamageMultiplier;
			}
			specialMagnitude = MissionCombatMechanicsHelper.ConvertBaseAttackMagnitude(currentUsageItem2, strikeType, baseMagnitude);
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x0007A4C4 File Offset: 0x000786C4
		private static void ComputeBlowMagnitudeFromHorseCharge(in AttackInformation attackInformation, in AttackCollisionData acd, Vec2 attackerAgentVelocity, Vec2 victimAgentVelocity, out float baseMagnitude, out float specialMagnitude)
		{
			Vec2 attackerAgentMovementDirection = attackInformation.AttackerAgentMovementDirection;
			Vec2 vec = attackerAgentMovementDirection * Vec2.DotProduct(victimAgentVelocity, attackerAgentMovementDirection);
			Vec2 vec2 = attackerAgentVelocity - vec;
			AttackCollisionData attackCollisionData = acd;
			Vec3 collisionGlobalPosition = attackCollisionData.CollisionGlobalPosition;
			float num = MissionCombatMechanicsHelper.ChargeDamageDotProduct(in attackInformation.VictimAgentPosition, in attackerAgentMovementDirection, in collisionGlobalPosition);
			float num2 = vec2.Length * num;
			baseMagnitude = num2 * num2 * num * attackInformation.AttackerAgentMountChargeDamageProperty;
			specialMagnitude = baseMagnitude;
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x0007A534 File Offset: 0x00078734
		private static void ComputeBlowMagnitudeMissile(in AttackInformation attackInformation, in AttackCollisionData collisionData, float momentumRemaining, in Vec2 victimVelocity, out float baseMagnitude, out float specialMagnitude)
		{
			float num;
			if (!attackInformation.IsVictimAgentNull)
			{
				Vec2 vec = victimVelocity;
				Vec3 vec2 = vec.ToVec3(0f);
				AttackCollisionData attackCollisionData = collisionData;
				num = (vec2 - attackCollisionData.MissileVelocity).Length;
			}
			else
			{
				AttackCollisionData attackCollisionData = collisionData;
				num = attackCollisionData.MissileVelocity.Length;
			}
			baseMagnitude = MissionGameModels.Current.StrikeMagnitudeModel.CalculateStrikeMagnitudeForMissile(in attackInformation, in collisionData, in attackInformation.AttackerWeapon, num);
			baseMagnitude *= momentumRemaining;
			if (attackInformation.AttackerAgent != null)
			{
				AgentStatCalculateModel agentStatCalculateModel = MissionGameModels.Current.AgentStatCalculateModel;
				Agent attackerAgent = attackInformation.AttackerAgent;
				MissionWeapon attackerWeapon = attackInformation.AttackerWeapon;
				float weaponDamageMultiplier = agentStatCalculateModel.GetWeaponDamageMultiplier(attackerAgent, attackerWeapon.CurrentUsageItem);
				baseMagnitude *= weaponDamageMultiplier;
			}
			specialMagnitude = baseMagnitude;
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x0007A5F4 File Offset: 0x000787F4
		private static void ComputeBlowMagnitudeFromFall(in AttackInformation attackInformation, in AttackCollisionData acd, out float baseMagnitude, out float specialMagnitude)
		{
			float victimAgentScale = attackInformation.VictimAgentScale;
			float num = attackInformation.VictimAgentWeight * victimAgentScale * victimAgentScale;
			float num2 = MathF.Sqrt(1f + attackInformation.VictimAgentTotalEncumbrance / num);
			AttackCollisionData attackCollisionData = acd;
			float num3 = -attackCollisionData.VictimAgentCurVelocity.z;
			if (attackInformation.DoesVictimHaveMountAgent)
			{
				float managedParameter = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.FallSpeedReductionMultiplierForRiderDamage);
				num3 *= managedParameter;
			}
			float num4;
			if (attackInformation.IsVictimAgentHuman)
			{
				num4 = 1f;
			}
			else
			{
				num4 = 1.41f;
			}
			float managedParameter2 = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.FallDamageMultiplier);
			float managedParameter3 = ManagedParameters.Instance.GetManagedParameter(ManagedParametersEnum.FallDamageAbsorption);
			baseMagnitude = (num3 * num3 * managedParameter2 - managedParameter3) * num2 * num4;
			if (baseMagnitude < 3f)
			{
				baseMagnitude = 0f;
			}
			else if (baseMagnitude > 499.9f)
			{
				baseMagnitude = 499.9f;
			}
			specialMagnitude = baseMagnitude;
		}

		// Token: 0x04000D57 RID: 3415
		private const float SpeedBonusFactorForSwing = 0.7f;

		// Token: 0x04000D58 RID: 3416
		private const float SpeedBonusFactorForThrust = 0.5f;
	}
}
