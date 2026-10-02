using System;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace SandBox.GameComponents
{
	// Token: 0x020000C0 RID: 192
	public class SandboxAgentApplyDamageModel : AgentApplyDamageModel
	{
		// Token: 0x060007D6 RID: 2006 RVA: 0x00034BBC File Offset: 0x00032DBC
		public override bool IsDamageIgnored(in AttackInformation attackInformation, in AttackCollisionData collisionData)
		{
			CharacterObject characterObject = (attackInformation.IsVictimAgentMount ? attackInformation.VictimRiderAgentCharacter : attackInformation.VictimAgentCharacter) as CharacterObject;
			MissionWeapon attackerWeapon = attackInformation.AttackerWeapon;
			WeaponComponentData currentUsageItem = attackerWeapon.CurrentUsageItem;
			bool flag = false;
			if (currentUsageItem != null && currentUsageItem.IsConsumable)
			{
				AttackCollisionData attackCollisionData = collisionData;
				if (attackCollisionData.CollidedWithShieldOnBack && characterObject != null && characterObject.GetPerkValue(DefaultPerks.Crossbow.Pavise))
				{
					float num = MBMath.ClampFloat(DefaultPerks.Crossbow.Pavise.PrimaryBonus, 0f, 1f);
					flag = MBRandom.RandomFloat <= num;
				}
			}
			return flag;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00034C4C File Offset: 0x00032E4C
		public override float ApplyDamageAmplifications(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			Formation attackerFormation = attackInformation.AttackerFormation;
			BannerComponent activeBanner = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(attackerFormation);
			Agent agent = (attackInformation.IsAttackerAgentMount ? attackInformation.AttackerAgent.RiderAgent : attackInformation.AttackerAgent);
			CharacterObject characterObject = (attackInformation.IsAttackerAgentMount ? attackInformation.AttackerRiderAgentCharacter : attackInformation.AttackerAgentCharacter) as CharacterObject;
			CharacterObject characterObject2 = attackInformation.AttackerCaptainCharacter as CharacterObject;
			bool flag = attackInformation.IsAttackerAgentHuman && !attackInformation.DoesAttackerHaveMountAgent;
			bool flag2 = attackInformation.DoesAttackerHaveMountAgent || attackInformation.DoesAttackerHaveRiderAgent;
			CharacterObject characterObject3 = (attackInformation.IsVictimAgentMount ? attackInformation.VictimRiderAgentCharacter : attackInformation.VictimAgentCharacter) as CharacterObject;
			bool flag3 = attackInformation.IsVictimAgentHuman && !attackInformation.DoesVictimHaveMountAgent;
			bool flag4 = attackInformation.DoesVictimHaveMountAgent || attackInformation.DoesVictimHaveRiderAgent;
			Formation victimFormation = attackInformation.VictimFormation;
			BannerComponent activeBanner2 = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(victimFormation);
			AttackCollisionData attackCollisionData = collisionData;
			bool flag5;
			if (!attackCollisionData.AttackBlockedWithShield)
			{
				attackCollisionData = collisionData;
				flag5 = attackCollisionData.CollidedWithShieldOnBack;
			}
			else
			{
				flag5 = true;
			}
			bool flag6 = flag5;
			ExplainedNumber explainedNumber = new ExplainedNumber(baseDamage, false, null);
			MissionWeapon attackerWeapon = attackInformation.AttackerWeapon;
			WeaponComponentData currentUsageItem = attackerWeapon.CurrentUsageItem;
			if (characterObject != null)
			{
				if (currentUsageItem != null)
				{
					if (currentUsageItem.IsMeleeWeapon)
					{
						if (currentUsageItem.RelevantSkill == DefaultSkills.OneHanded)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.DeadlyPurpose, characterObject, true, ref explainedNumber, false);
							if (flag2)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.Cavalry, characterObject, true, ref explainedNumber, false);
							}
							MissionWeapon offHandItem = attackInformation.OffHandItem;
							if (offHandItem.IsEmpty)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.Duelist, characterObject, true, ref explainedNumber, false);
							}
							if (currentUsageItem.WeaponClass == WeaponClass.Mace || currentUsageItem.WeaponClass == WeaponClass.OneHandedAxe)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.ToBeBlunt, characterObject, true, ref explainedNumber, false);
							}
							if (flag6)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.Prestige, characterObject, true, ref explainedNumber, false);
							}
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Roguery.Carver, characterObject2, ref explainedNumber);
							PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.OneHanded.WayOfTheSword, characterObject, DefaultSkills.OneHanded, false, ref explainedNumber, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus, false);
						}
						else if (currentUsageItem.RelevantSkill == DefaultSkills.TwoHanded)
						{
							if (flag6)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.WoodChopper, characterObject, true, ref explainedNumber, false);
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.WoodChopper, characterObject2, ref explainedNumber);
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.ShieldBreaker, characterObject, true, ref explainedNumber, false);
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.ShieldBreaker, characterObject2, ref explainedNumber);
							}
							if (currentUsageItem.WeaponClass == WeaponClass.TwoHandedAxe || currentUsageItem.WeaponClass == WeaponClass.TwoHandedMace)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.HeadBasher, characterObject, true, ref explainedNumber, false);
							}
							if (attackInformation.IsVictimAgentMount)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.BeastSlayer, characterObject, true, ref explainedNumber, false);
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.BeastSlayer, characterObject2, ref explainedNumber);
							}
							if (attackInformation.AttackerHitPointRate < 0.5f)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.Berserker, characterObject, true, ref explainedNumber, false);
							}
							else if (attackInformation.AttackerHitPointRate > 0.9f)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.Confidence, characterObject, true, ref explainedNumber, false);
							}
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.BladeMaster, characterObject, true, ref explainedNumber, false);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Roguery.DashAndSlash, characterObject2, ref explainedNumber);
							PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.TwoHanded.WayOfTheGreatAxe, characterObject, DefaultSkills.TwoHanded, false, ref explainedNumber, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus, false);
						}
						else if (currentUsageItem.RelevantSkill == DefaultSkills.Polearm)
						{
							if (flag2)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.Cavalry, characterObject, true, ref explainedNumber, false);
							}
							else
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.Pikeman, characterObject, true, ref explainedNumber, false);
							}
							attackCollisionData = collisionData;
							if (attackCollisionData.StrikeType == 1)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.CleanThrust, characterObject, true, ref explainedNumber, false);
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.SharpenTheTip, characterObject, true, ref explainedNumber, false);
							}
							if (attackInformation.IsVictimAgentMount)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.SteedKiller, characterObject, true, ref explainedNumber, false);
								if (flag)
								{
									PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.SteedKiller, characterObject2, ref explainedNumber);
								}
							}
							if (attackInformation.IsHeadShot)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.Guards, characterObject, true, ref explainedNumber, false);
							}
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.Phalanx, characterObject2, ref explainedNumber);
							PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Polearm.WayOfTheSpear, characterObject, DefaultSkills.Polearm, false, ref explainedNumber, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus, false);
						}
						else if (currentUsageItem.IsShield)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.Basher, characterObject, true, ref explainedNumber, false);
						}
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.Powerful, characterObject, true, ref explainedNumber, false);
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Athletics.Powerful, characterObject2, ref explainedNumber);
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Engineering.ImprovedTools, characterObject2, ref explainedNumber);
						if (attackerWeapon.Item != null && attackerWeapon.Item.ItemType == ItemObject.ItemTypeEnum.Thrown)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.FlexibleFighter, characterObject, true, ref explainedNumber, false);
						}
						if (flag2)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.MountedWarrior, characterObject, true, ref explainedNumber, false);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Riding.MountedWarrior, characterObject2, ref explainedNumber);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.OneHanded.Cavalry, characterObject2, ref explainedNumber);
						}
						else
						{
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.OneHanded.DeadlyPurpose, characterObject2, ref explainedNumber);
							attackCollisionData = collisionData;
							if (attackCollisionData.StrikeType == 1)
							{
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.SharpenTheTip, characterObject2, ref explainedNumber);
							}
						}
						if (activeBanner != null)
						{
							BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedMeleeDamage, activeBanner, ref explainedNumber);
							if (attackInformation.DoesVictimHaveMountAgent)
							{
								BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedMeleeDamageAgainstMountedTroops, activeBanner, ref explainedNumber);
							}
						}
					}
					else if (currentUsageItem.IsConsumable)
					{
						if (currentUsageItem.RelevantSkill == DefaultSkills.Bow)
						{
							attackCollisionData = collisionData;
							if (attackCollisionData.CollisionBoneIndex != -1)
							{
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Bow.BowControl, characterObject2, ref explainedNumber);
								if (attackInformation.IsHeadShot)
								{
									PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.DeadAim, characterObject, true, ref explainedNumber, false);
								}
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.StrongBows, characterObject, true, ref explainedNumber, false);
								if (characterObject.Tier >= 3)
								{
									PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Bow.StrongBows, characterObject2, ref explainedNumber);
								}
								if (attackInformation.IsVictimAgentMount)
								{
									PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.HunterClan, characterObject, true, ref explainedNumber, false);
								}
								PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Bow.Deadshot, characterObject, DefaultSkills.Bow, false, ref explainedNumber, Campaign.Current.Models.CharacterDevelopmentModel.MinSkillRequiredForEpicPerkBonus, false);
								goto IL_07AA;
							}
						}
						if (currentUsageItem.RelevantSkill == DefaultSkills.Crossbow)
						{
							attackCollisionData = collisionData;
							if (attackCollisionData.CollisionBoneIndex != -1)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Engineering.TorsionEngines, characterObject, false, ref explainedNumber, false);
								if (attackInformation.IsVictimAgentMount)
								{
									PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.Unhorser, characterObject, true, ref explainedNumber, false);
									PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Crossbow.Unhorser, characterObject2, ref explainedNumber);
								}
								if (attackInformation.IsHeadShot)
								{
									PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.Sheriff, characterObject, true, ref explainedNumber, false);
								}
								if (flag3)
								{
									PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Crossbow.Sheriff, characterObject2, ref explainedNumber);
								}
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Crossbow.HammerBolts, characterObject2, ref explainedNumber);
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Engineering.DreadfulSieger, characterObject2, ref explainedNumber);
								PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Crossbow.MightyPull, characterObject, DefaultSkills.Crossbow, false, ref explainedNumber, Campaign.Current.Models.CharacterDevelopmentModel.MinSkillRequiredForEpicPerkBonus, false);
								goto IL_07AA;
							}
						}
						if (currentUsageItem.RelevantSkill == DefaultSkills.Throwing)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.StrongArms, characterObject, true, ref explainedNumber, false);
							if (flag6)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.ShieldBreaker, characterObject, true, ref explainedNumber, false);
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.ShieldBreaker, characterObject2, ref explainedNumber);
								if (currentUsageItem.WeaponClass == WeaponClass.ThrowingAxe)
								{
									PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.Splinters, characterObject, true, ref explainedNumber, false);
								}
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.Splinters, characterObject2, ref explainedNumber);
							}
							if (attackInformation.IsVictimAgentMount)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.Hunter, characterObject, true, ref explainedNumber, false);
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.Hunter, characterObject2, ref explainedNumber);
							}
							if (flag2)
							{
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.MountedSkirmisher, characterObject2, ref explainedNumber);
							}
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.Impale, characterObject2, ref explainedNumber);
							if (flag4)
							{
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.KnockOff, characterObject2, ref explainedNumber);
							}
							if (attackInformation.VictimAgentHealth <= attackInformation.VictimAgentMaxHealth * 0.5f)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.LastHit, characterObject, true, ref explainedNumber, false);
							}
							if (attackInformation.IsHeadShot)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.HeadHunter, characterObject, true, ref explainedNumber, false);
							}
							PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Throwing.UnstoppableForce, characterObject, DefaultSkills.Throwing, false, ref explainedNumber, Campaign.Current.Models.CharacterDevelopmentModel.MinSkillRequiredForEpicPerkBonus, false);
						}
						IL_07AA:
						if (flag2)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.HorseArcher, characterObject, true, ref explainedNumber, false);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Riding.HorseArcher, characterObject2, ref explainedNumber);
						}
						if (activeBanner != null)
						{
							BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedRangedDamage, activeBanner, ref explainedNumber);
						}
					}
					if (attackerWeapon.Item != null && attackerWeapon.Item.IsCivilian)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Roguery.Carver, characterObject, true, ref explainedNumber, false);
					}
				}
				attackCollisionData = collisionData;
				if (attackCollisionData.IsHorseCharge)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.FullSpeed, characterObject, true, ref explainedNumber, false);
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Riding.FullSpeed, characterObject2, ref explainedNumber);
					if (characterObject.GetPerkValue(DefaultPerks.Riding.TheWayOfTheSaddle))
					{
						float num = (float)MathF.Max(MissionGameModels.Current.AgentStatCalculateModel.GetEffectiveSkill(agent, DefaultSkills.Riding) - Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus, 0) * DefaultPerks.Riding.TheWayOfTheSaddle.PrimaryBonus;
						explainedNumber.Add(num, null, null);
					}
					if (activeBanner != null)
					{
						BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedChargeDamage, activeBanner, ref explainedNumber);
					}
					if (activeBanner2 != null)
					{
						BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.DecreasedChargeDamage, activeBanner2, ref explainedNumber);
					}
				}
				if (flag)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.HeadBasher, characterObject2, ref explainedNumber);
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.RecklessCharge, characterObject2, ref explainedNumber);
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.Pikeman, characterObject2, ref explainedNumber);
					if (flag4)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.Braced, characterObject2, ref explainedNumber);
					}
				}
				if (flag2)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.Cavalry, characterObject2, ref explainedNumber);
				}
				if (currentUsageItem == null)
				{
					attackCollisionData = collisionData;
					if (attackCollisionData.IsAlternativeAttack && characterObject.GetPerkValue(DefaultPerks.Athletics.StrongLegs))
					{
						explainedNumber.AddFactor(1f, null);
					}
				}
				if (flag6)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Engineering.WallBreaker, characterObject2, ref explainedNumber);
				}
				attackCollisionData = collisionData;
				if (attackCollisionData.EntityExists)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.Vandal, characterObject2, ref explainedNumber);
				}
				if (characterObject3 != null)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Tactics.Coaching, characterObject2, ref explainedNumber);
					if (characterObject3.Culture.IsBandit)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Tactics.LawKeeper, characterObject2, ref explainedNumber);
					}
					if (flag2 && flag3)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Tactics.Gensdarmes, characterObject2, ref explainedNumber);
					}
				}
				if (characterObject.Culture.IsBandit)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Roguery.PartnersInCrime, characterObject2, ref explainedNumber);
				}
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0003561C File Offset: 0x0003381C
		public override float ApplyDamageScaling(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			float num = 1f;
			if (Mission.Current.IsSallyOutBattle)
			{
				DestructableComponent hitObjectDestructibleComponent = attackInformation.HitObjectDestructibleComponent;
				if (hitObjectDestructibleComponent != null && hitObjectDestructibleComponent.GameEntity.GetFirstScriptOfType<SiegeWeapon>() != null)
				{
					num *= 4.5f;
				}
			}
			return baseDamage * num;
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00035664 File Offset: 0x00033864
		public override float ApplyDamageReductions(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			Formation attackerFormation = attackInformation.AttackerFormation;
			MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(attackerFormation);
			Agent agent = (attackInformation.IsAttackerAgentMount ? attackInformation.AttackerAgent.RiderAgent : attackInformation.AttackerAgent);
			bool isAttackerAgentMount = attackInformation.IsAttackerAgentMount;
			if (attackInformation.IsAttackerAgentHuman)
			{
				bool flag = !attackInformation.DoesAttackerHaveMountAgent;
			}
			if (!attackInformation.DoesAttackerHaveMountAgent)
			{
				bool doesAttackerHaveRiderAgent = attackInformation.DoesAttackerHaveRiderAgent;
			}
			CharacterObject characterObject = (attackInformation.IsVictimAgentMount ? attackInformation.VictimRiderAgentCharacter : attackInformation.VictimAgentCharacter) as CharacterObject;
			CharacterObject characterObject2 = attackInformation.VictimCaptainCharacter as CharacterObject;
			bool flag2 = attackInformation.IsVictimAgentHuman && !attackInformation.DoesVictimHaveMountAgent;
			if (!attackInformation.DoesVictimHaveMountAgent)
			{
				bool doesVictimHaveRiderAgent = attackInformation.DoesVictimHaveRiderAgent;
			}
			Formation victimFormation = attackInformation.VictimFormation;
			BannerComponent activeBanner = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(victimFormation);
			MissionWeapon missionWeapon = attackInformation.VictimMainHandWeapon;
			WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
			missionWeapon = attackInformation.VictimMainHandWeapon;
			WeaponComponentData currentUsageItem2 = missionWeapon.CurrentUsageItem;
			AttackCollisionData attackCollisionData = collisionData;
			bool flag3;
			if (!attackCollisionData.AttackBlockedWithShield)
			{
				attackCollisionData = collisionData;
				flag3 = attackCollisionData.CollidedWithShieldOnBack;
			}
			else
			{
				flag3 = true;
			}
			bool flag4 = flag3;
			ExplainedNumber explainedNumber = new ExplainedNumber(baseDamage, false, null);
			MissionWeapon attackerWeapon = attackInformation.AttackerWeapon;
			WeaponComponentData currentUsageItem3 = attackerWeapon.CurrentUsageItem;
			if (attackInformation.DoesAttackerHaveMountAgent && (currentUsageItem3 == null || currentUsageItem3.RelevantSkill != DefaultSkills.Crossbow))
			{
				int effectiveSkill = MissionGameModels.Current.AgentStatCalculateModel.GetEffectiveSkill(agent, DefaultSkills.Riding);
				SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.MountedWeaponDamagePenalty, ref explainedNumber, effectiveSkill);
			}
			if (characterObject != null)
			{
				if (currentUsageItem3 != null)
				{
					if (currentUsageItem3.IsConsumable)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.SkirmishPhaseMaster, characterObject, true, ref explainedNumber, false);
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.Skirmisher, characterObject2, ref explainedNumber);
						if (characterObject.IsRanged)
						{
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Bow.SkirmishPhaseMaster, characterObject2, ref explainedNumber);
						}
						if (currentUsageItem2 != null)
						{
							if (currentUsageItem2.WeaponClass == WeaponClass.Crossbow)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.CounterFire, characterObject, true, ref explainedNumber, false);
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Crossbow.CounterFire, characterObject2, ref explainedNumber);
							}
							else if (currentUsageItem2.RelevantSkill == DefaultSkills.Throwing)
							{
								PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.Skirmisher, characterObject, true, ref explainedNumber, false);
							}
						}
						if (activeBanner != null)
						{
							BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.DecreasedRangedAttackDamage, activeBanner, ref explainedNumber);
						}
					}
					else if (currentUsageItem3.IsMeleeWeapon)
					{
						if (characterObject2 != null)
						{
							Formation victimFormation2 = attackInformation.VictimFormation;
							if (victimFormation2 != null && victimFormation2.ArrangementOrder.OrderEnum == ArrangementOrder.ArrangementOrderEnum.ShieldWall)
							{
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.OneHanded.Basher, characterObject2, ref explainedNumber);
							}
						}
						if (activeBanner != null)
						{
							BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.DecreasedMeleeAttackDamage, activeBanner, ref explainedNumber);
						}
					}
				}
				if (flag4)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.SteelCoreShields, characterObject, true, ref explainedNumber, false);
					if (flag2)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.OneHanded.SteelCoreShields, characterObject2, ref explainedNumber);
					}
					attackCollisionData = collisionData;
					if (attackCollisionData.AttackBlockedWithShield)
					{
						attackCollisionData = collisionData;
						if (!attackCollisionData.CorrectSideShieldBlock)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.ShieldWall, characterObject, true, ref explainedNumber, false);
						}
					}
				}
				attackCollisionData = collisionData;
				if (attackCollisionData.IsHorseCharge)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.SureFooted, characterObject, true, ref explainedNumber, false);
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.Braced, characterObject, true, ref explainedNumber, false);
					if (characterObject2 != null)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.SureFooted, characterObject2, ref explainedNumber);
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Athletics.Braced, characterObject2, ref explainedNumber);
					}
				}
				attackCollisionData = collisionData;
				if (attackCollisionData.IsFallDamage)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.StrongLegs, characterObject, true, ref explainedNumber, false);
				}
				PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Tactics.EliteReserves, characterObject2, ref explainedNumber);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x000359A8 File Offset: 0x00033BA8
		public override float ApplyGeneralDamageModifiers(in AttackInformation attackInformation, in AttackCollisionData collisionData, float baseDamage)
		{
			bool isAttackerAgentMount = attackInformation.IsAttackerAgentMount;
			bool isVictimAgentMount = attackInformation.IsVictimAgentMount;
			MissionWeapon attackerWeapon = attackInformation.AttackerWeapon;
			WeaponComponentData currentUsageItem = attackerWeapon.CurrentUsageItem;
			ExplainedNumber explainedNumber = new ExplainedNumber(baseDamage, false, null);
			if (currentUsageItem != null)
			{
				if (currentUsageItem.RelevantSkill == DefaultSkills.Throwing)
				{
					explainedNumber = new ExplainedNumber(explainedNumber.ResultNumber * (1f + attackInformation.AttackerAgent.AgentDrivenProperties.ThrowingWeaponDamageMultiplierBonus), false, null);
				}
				else if (currentUsageItem.IsMeleeWeapon)
				{
					explainedNumber = new ExplainedNumber(explainedNumber.ResultNumber * (1f + attackInformation.AttackerAgent.AgentDrivenProperties.MeleeWeaponDamageMultiplierBonus), false, null);
				}
			}
			Agent attackerAgent = attackInformation.AttackerAgent;
			if (attackerAgent != null)
			{
				explainedNumber = new ExplainedNumber(explainedNumber.ResultNumber * (1f + attackerAgent.AgentDrivenProperties.DamageMultiplierBonus), false, null);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00035A78 File Offset: 0x00033C78
		public override bool DecideCrushedThrough(Agent attackerAgent, Agent defenderAgent, float totalAttackEnergy, Agent.UsageDirection attackDirection, StrikeType strikeType, WeaponComponentData defendItem, bool isPassiveUsage)
		{
			EquipmentIndex equipmentIndex = attackerAgent.GetOffhandWieldedItemIndex();
			if (equipmentIndex == EquipmentIndex.None)
			{
				equipmentIndex = attackerAgent.GetPrimaryWieldedItemIndex();
			}
			if (((equipmentIndex != EquipmentIndex.None) ? attackerAgent.Equipment[equipmentIndex].CurrentUsageItem : null) == null || isPassiveUsage || strikeType != StrikeType.Swing || attackDirection != Agent.UsageDirection.AttackUp)
			{
				return false;
			}
			float num = 58f;
			if (defendItem != null && defendItem.IsShield)
			{
				num *= 1.2f;
			}
			return totalAttackEnergy > num;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x00035AE4 File Offset: 0x00033CE4
		public override void DecideMissileWeaponFlags(Agent attackerAgent, in MissionWeapon missileWeapon, ref WeaponFlags missileWeaponFlags)
		{
			CharacterObject characterObject = ((attackerAgent != null) ? attackerAgent.Character : null) as CharacterObject;
			if (characterObject != null)
			{
				MissionWeapon missionWeapon = missileWeapon;
				if (missionWeapon.CurrentUsageItem.WeaponClass == WeaponClass.Javelin && characterObject.GetPerkValue(DefaultPerks.Throwing.Impale))
				{
					missileWeaponFlags |= WeaponFlags.CanPenetrateShield;
				}
			}
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00035B35 File Offset: 0x00033D35
		public override bool CanWeaponIgnoreFriendlyFireChecks(WeaponComponentData weapon)
		{
			return weapon != null && weapon.IsConsumable && weapon.WeaponFlags.HasAnyFlag(WeaponFlags.CanPenetrateShield) && weapon.WeaponFlags.HasAnyFlag(WeaponFlags.MultiplePenetration);
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00035B6C File Offset: 0x00033D6C
		public override bool CanWeaponDealSneakAttack(in AttackInformation attackInformation, WeaponComponentData weapon)
		{
			if (weapon != null && (weapon.IsMeleeWeapon || weapon.WeaponClass == WeaponClass.ThrowingKnife) && attackInformation.IsVictimAgentHuman && !attackInformation.IsVictimPlayer)
			{
				if ((attackInformation.VictimAgentAIStateFlags & Agent.AIStateFlag.Alarmed) == Agent.AIStateFlag.None && attackInformation.VictimAgentFlags.HasAnyFlag(AgentFlag.CanGetAlarmed))
				{
					return true;
				}
				if (!attackInformation.VictimAgentAIStateFlags.HasAllFlags(Agent.AIStateFlag.Alarmed) && !attackInformation.IsAttackerAgentNull && Vec2.DotProduct((attackInformation.AttackerAgentPosition - attackInformation.VictimAgentPosition).AsVec2.Normalized(), attackInformation.VictimAgentMovementDirection) < 0.174f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00035C0C File Offset: 0x00033E0C
		public override bool CanWeaponDismount(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
		{
			CharacterObject characterObject;
			return MBMath.IsBetween((int)blow.VictimBodyPart, 0, 6) && ((!attackerAgent.HasMount && blow.StrikeType == StrikeType.Swing && blow.WeaponRecord.WeaponFlags.HasAnyFlag(WeaponFlags.CanHook)) || (blow.StrikeType == StrikeType.Thrust && blow.WeaponRecord.WeaponFlags.HasAnyFlag(WeaponFlags.CanDismount)) || ((characterObject = attackerAgent.Character as CharacterObject) != null && ((attackerWeapon.RelevantSkill == DefaultSkills.Crossbow && attackerWeapon.IsConsumable && characterObject.GetPerkValue(DefaultPerks.Crossbow.HammerBolts)) || (attackerWeapon.RelevantSkill == DefaultSkills.Throwing && attackerWeapon.IsConsumable && characterObject.GetPerkValue(DefaultPerks.Throwing.KnockOff)))));
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00035CD0 File Offset: 0x00033ED0
		public override void CalculateDefendedBlowStunMultipliers(Agent attackerAgent, Agent defenderAgent, CombatCollisionResult collisionResult, WeaponComponentData attackerWeapon, WeaponComponentData defenderWeapon, ref float attackerStunPeriod, ref float defenderStunPeriod)
		{
			float num = 1f;
			float num2 = 1f;
			CharacterObject characterObject;
			if ((characterObject = attackerAgent.Character as CharacterObject) != null && (collisionResult == CombatCollisionResult.Blocked || collisionResult == CombatCollisionResult.Parried) && characterObject.GetPerkValue(DefaultPerks.Athletics.MightyBlow))
			{
				num += num * DefaultPerks.Athletics.MightyBlow.PrimaryBonus;
			}
			num = MathF.Max(0f, num);
			num2 = MathF.Max(0f, num2);
			attackerStunPeriod *= num;
			defenderStunPeriod *= num2;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00035D44 File Offset: 0x00033F44
		public override bool CanWeaponKnockback(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
		{
			AttackCollisionData attackCollisionData = collisionData;
			return MBMath.IsBetween((int)attackCollisionData.VictimHitBodyPart, 0, 6) && !attackerWeapon.WeaponFlags.HasAnyFlag(WeaponFlags.CanKnockDown) && (attackerWeapon.IsConsumable || (blow.BlowFlag & BlowFlags.CrushThrough) != BlowFlags.None || (blow.StrikeType == StrikeType.Thrust && blow.WeaponRecord.WeaponFlags.HasAnyFlag(WeaponFlags.WideGrip)));
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00035DB4 File Offset: 0x00033FB4
		public override bool CanWeaponKnockDown(Agent attackerAgent, Agent victimAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
		{
			if (attackerWeapon.WeaponClass == WeaponClass.Boulder || attackerWeapon.WeaponClass == WeaponClass.BallistaBoulder)
			{
				return true;
			}
			AttackCollisionData attackCollisionData = collisionData;
			BoneBodyPartType victimHitBodyPart = attackCollisionData.VictimHitBodyPart;
			bool flag = MBMath.IsBetween((int)victimHitBodyPart, 0, 6);
			if (!victimAgent.HasMount && victimHitBodyPart == BoneBodyPartType.Legs)
			{
				flag = true;
			}
			return flag && blow.WeaponRecord.WeaponFlags.HasAnyFlag(WeaponFlags.CanKnockDown) && ((attackerWeapon.IsPolearm && blow.StrikeType == StrikeType.Thrust) || (attackerWeapon.IsMeleeWeapon && blow.StrikeType == StrikeType.Swing && MissionCombatMechanicsHelper.DecideSweetSpotCollision(in collisionData)));
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00035E4C File Offset: 0x0003404C
		public override float GetDismountPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
		{
			float num = 0f;
			if (blow.StrikeType == StrikeType.Swing && blow.WeaponRecord.WeaponFlags.HasAnyFlag(WeaponFlags.CanHook))
			{
				num += 0.25f;
			}
			CharacterObject characterObject;
			if (attackerWeapon != null && (characterObject = attackerAgent.Character as CharacterObject) != null)
			{
				if (attackerWeapon.RelevantSkill == DefaultSkills.Polearm && characterObject.GetPerkValue(DefaultPerks.Polearm.Braced))
				{
					num += DefaultPerks.Polearm.Braced.PrimaryBonus;
				}
				else if (attackerWeapon.RelevantSkill == DefaultSkills.Crossbow && attackerWeapon.IsConsumable && characterObject.GetPerkValue(DefaultPerks.Crossbow.HammerBolts))
				{
					num += DefaultPerks.Crossbow.HammerBolts.PrimaryBonus;
				}
				else if (attackerWeapon.RelevantSkill == DefaultSkills.Throwing && attackerWeapon.IsConsumable && characterObject.GetPerkValue(DefaultPerks.Throwing.KnockOff))
				{
					num += DefaultPerks.Throwing.KnockOff.PrimaryBonus;
				}
			}
			return MathF.Max(0f, num);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00035F34 File Offset: 0x00034134
		public override float GetKnockBackPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
		{
			float num = 0f;
			CharacterObject characterObject;
			if (attackerWeapon != null && attackerWeapon.RelevantSkill == DefaultSkills.Polearm && (characterObject = ((attackerAgent != null) ? attackerAgent.Character : null) as CharacterObject) != null && blow.StrikeType == StrikeType.Thrust && characterObject.GetPerkValue(DefaultPerks.Polearm.KeepAtBay))
			{
				num += DefaultPerks.Polearm.KeepAtBay.PrimaryBonus;
			}
			return num;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00035F90 File Offset: 0x00034190
		public override float GetKnockDownPenetration(Agent attackerAgent, WeaponComponentData attackerWeapon, in Blow blow, in AttackCollisionData collisionData)
		{
			float num = 0f;
			if (attackerWeapon.WeaponClass == WeaponClass.Boulder || attackerWeapon.WeaponClass == WeaponClass.BallistaBoulder)
			{
				num += 0.25f;
			}
			else if (attackerWeapon.IsMeleeWeapon)
			{
				CharacterObject characterObject = ((attackerAgent != null) ? attackerAgent.Character : null) as CharacterObject;
				AttackCollisionData attackCollisionData;
				if (blow.StrikeType == StrikeType.Swing)
				{
					attackCollisionData = collisionData;
					if (attackCollisionData.VictimHitBodyPart == BoneBodyPartType.Legs)
					{
						num += 0.1f;
					}
					if (characterObject != null && attackerWeapon.RelevantSkill == DefaultSkills.TwoHanded && characterObject.GetPerkValue(DefaultPerks.TwoHanded.ShowOfStrength))
					{
						num += DefaultPerks.TwoHanded.ShowOfStrength.PrimaryBonus;
					}
				}
				attackCollisionData = collisionData;
				if (attackCollisionData.VictimHitBodyPart == BoneBodyPartType.Head)
				{
					num += 0.15f;
				}
				if (characterObject != null && attackerWeapon.RelevantSkill == DefaultSkills.Polearm && characterObject.GetPerkValue(DefaultPerks.Polearm.HardKnock))
				{
					num += DefaultPerks.Polearm.HardKnock.PrimaryBonus;
				}
			}
			return num;
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00036071 File Offset: 0x00034271
		public override float GetHorseChargePenetration()
		{
			return 0.4f;
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00036078 File Offset: 0x00034278
		public override float CalculateStaggerThresholdDamage(Agent defenderAgent, in Blow blow)
		{
			float num = 1f;
			CharacterObject characterObject = defenderAgent.Character as CharacterObject;
			Formation formation = defenderAgent.Formation;
			object obj;
			if (formation == null)
			{
				obj = null;
			}
			else
			{
				Agent captain = formation.Captain;
				obj = ((captain != null) ? captain.Character : null);
			}
			CharacterObject characterObject2 = obj as CharacterObject;
			if (characterObject != null)
			{
				if (characterObject2 == characterObject)
				{
					characterObject2 = null;
				}
				ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
				if (defenderAgent.HasMount)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.DauntlessSteed, characterObject, true, ref explainedNumber, false);
				}
				else
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.Spartan, characterObject, true, ref explainedNumber, false);
				}
				WeaponComponentData currentUsageItem = defenderAgent.WieldedWeapon.CurrentUsageItem;
				if (currentUsageItem != null && currentUsageItem.WeaponClass == WeaponClass.Crossbow && defenderAgent.WieldedWeapon.IsReloading)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.DeftHands, characterObject, true, ref explainedNumber, false);
					if (characterObject2 != null)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Crossbow.DeftHands, characterObject2, ref explainedNumber);
					}
				}
				num = explainedNumber.ResultNumber;
			}
			TaleWorlds.Core.ManagedParametersEnum managedParametersEnum;
			if (blow.DamageType == DamageTypes.Cut)
			{
				managedParametersEnum = TaleWorlds.Core.ManagedParametersEnum.DamageInterruptAttackThresholdCut;
			}
			else if (blow.DamageType == DamageTypes.Pierce)
			{
				managedParametersEnum = TaleWorlds.Core.ManagedParametersEnum.DamageInterruptAttackThresholdPierce;
			}
			else
			{
				managedParametersEnum = TaleWorlds.Core.ManagedParametersEnum.DamageInterruptAttackThresholdBlunt;
			}
			return TaleWorlds.Core.ManagedParameters.Instance.GetManagedParameter(managedParametersEnum) * num;
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00036181 File Offset: 0x00034381
		public override float CalculateAlternativeAttackDamage(in AttackInformation attackInformation, in AttackCollisionData collisionData, WeaponComponentData weapon)
		{
			if (weapon == null)
			{
				return 2f;
			}
			if (weapon.WeaponClass == WeaponClass.LargeShield)
			{
				return 2f;
			}
			if (weapon.WeaponClass == WeaponClass.SmallShield)
			{
				return 1f;
			}
			if (weapon.IsTwoHanded)
			{
				return 2f;
			}
			return 1f;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x000361C0 File Offset: 0x000343C0
		public override float CalculatePassiveAttackDamage(BasicCharacterObject attackerCharacter, in AttackCollisionData collisionData, float baseDamage)
		{
			CharacterObject characterObject = attackerCharacter as CharacterObject;
			if (characterObject != null)
			{
				AttackCollisionData attackCollisionData = collisionData;
				if (attackCollisionData.AttackBlockedWithShield && characterObject.GetPerkValue(DefaultPerks.Polearm.UnstoppableForce))
				{
					baseDamage *= DefaultPerks.Polearm.UnstoppableForce.PrimaryBonus;
				}
			}
			return baseDamage;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x00036204 File Offset: 0x00034404
		public override MeleeCollisionReaction DecidePassiveAttackCollisionReaction(Agent attacker, Agent defender, bool isFatalHit)
		{
			MeleeCollisionReaction meleeCollisionReaction = MeleeCollisionReaction.Bounced;
			if (isFatalHit && attacker.HasMount)
			{
				float num = 0.05f;
				CharacterObject characterObject;
				if ((characterObject = attacker.Character as CharacterObject) != null && characterObject.GetPerkValue(DefaultPerks.Polearm.Skewer))
				{
					num += DefaultPerks.Polearm.Skewer.PrimaryBonus;
				}
				if (MBRandom.RandomFloat < num)
				{
					meleeCollisionReaction = MeleeCollisionReaction.SlicedThrough;
				}
			}
			return meleeCollisionReaction;
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x00036258 File Offset: 0x00034458
		public override float CalculateShieldDamage(in AttackInformation attackInformation, float baseDamage)
		{
			Formation victimFormation = attackInformation.VictimFormation;
			ExplainedNumber explainedNumber = new ExplainedNumber(baseDamage, false, null);
			BannerComponent activeBanner = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(victimFormation);
			if (activeBanner != null)
			{
				BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.DecreasedShieldDamage, activeBanner, ref explainedNumber);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0003629E File Offset: 0x0003449E
		public override float CalculateSailFireDamage(Agent attackerAgent, IShipOrigin shipOrigin, float baseDamage, bool damageFromShipMachine)
		{
			return baseDamage;
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x000362A4 File Offset: 0x000344A4
		public override float CalculateHullFireDamage(float baseFireDamage, IShipOrigin shipOrigin)
		{
			return new ExplainedNumber(baseFireDamage, false, null).ResultNumber;
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x000362C4 File Offset: 0x000344C4
		public override float GetDamageMultiplierForBodyPart(BoneBodyPartType bodyPart, DamageTypes type, bool isHuman, bool isMissile)
		{
			float num = 1f;
			switch (bodyPart)
			{
			case BoneBodyPartType.None:
				num = 1f;
				break;
			case BoneBodyPartType.Head:
				switch (type)
				{
				case DamageTypes.Invalid:
					num = 1.5f;
					break;
				case DamageTypes.Cut:
					num = 1.2f;
					break;
				case DamageTypes.Pierce:
					if (isHuman)
					{
						num = (isMissile ? 2f : 1.25f);
					}
					else
					{
						num = 1.2f;
					}
					break;
				case DamageTypes.Blunt:
					num = 1.2f;
					break;
				}
				break;
			case BoneBodyPartType.Neck:
				switch (type)
				{
				case DamageTypes.Invalid:
					num = 1.5f;
					break;
				case DamageTypes.Cut:
					num = 1.2f;
					break;
				case DamageTypes.Pierce:
					if (isHuman)
					{
						num = (isMissile ? 2f : 1.25f);
					}
					else
					{
						num = 1.2f;
					}
					break;
				case DamageTypes.Blunt:
					num = 1.2f;
					break;
				}
				break;
			case BoneBodyPartType.Chest:
			case BoneBodyPartType.Abdomen:
			case BoneBodyPartType.ShoulderLeft:
			case BoneBodyPartType.ShoulderRight:
			case BoneBodyPartType.ArmLeft:
			case BoneBodyPartType.ArmRight:
				num = (isHuman ? 1f : 0.8f);
				break;
			case BoneBodyPartType.Legs:
				num = 0.8f;
				break;
			}
			return num;
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x000363DD File Offset: 0x000345DD
		public override bool DecideAgentShrugOffBlow(Agent victimAgent, in AttackCollisionData collisionData, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideAgentShrugOffBlow(victimAgent, in collisionData, in blow);
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x000363E7 File Offset: 0x000345E7
		public override bool DecideAgentDismountedByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideAgentDismountedByBlow(attackerAgent, victimAgent, in collisionData, attackerWeapon, in blow);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x000363F5 File Offset: 0x000345F5
		public override bool DecideAgentKnockedBackByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideAgentKnockedBackByBlow(attackerAgent, victimAgent, in collisionData, attackerWeapon, in blow);
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00036403 File Offset: 0x00034603
		public override bool DecideAgentKnockedDownByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideAgentKnockedDownByBlow(attackerAgent, victimAgent, in collisionData, attackerWeapon, in blow);
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00036411 File Offset: 0x00034611
		public override bool DecideMountRearedByBlow(Agent attackerAgent, Agent victimAgent, in AttackCollisionData collisionData, WeaponComponentData attackerWeapon, in Blow blow)
		{
			return MissionCombatMechanicsHelper.DecideMountRearedByBlow(attackerAgent, victimAgent, in collisionData, attackerWeapon, in blow);
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00036420 File Offset: 0x00034620
		public override void DecideWeaponCollisionReaction(in Blow registeredBlow, in AttackCollisionData collisionData, Agent attacker, Agent defender, in MissionWeapon attackerWeapon, bool isFatalHit, bool isShruggedOff, float momentumRemaining, out MeleeCollisionReaction colReaction)
		{
			MissionCombatMechanicsHelper.DecideWeaponCollisionReaction(in registeredBlow, in collisionData, attacker, defender, in attackerWeapon, isFatalHit, isShruggedOff, momentumRemaining, out colReaction);
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00036441 File Offset: 0x00034641
		public override bool ShouldMissilePassThroughAfterShieldBreak(Agent attackerAgent, WeaponComponentData attackerWeapon)
		{
			return false;
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00036444 File Offset: 0x00034644
		public override float CalculateRemainingMomentum(float originalMomentum, in Blow b, in AttackCollisionData collisionData, Agent attacker, Agent victim, in MissionWeapon attackerWeapon, bool isCrushThrough)
		{
			return base.CalculateDefaultRemainingMomentum(originalMomentum, in b, in collisionData, attacker, victim, in attackerWeapon, isCrushThrough);
		}

		// Token: 0x0400042D RID: 1069
		private const float SallyOutSiegeEngineDamageMultiplier = 4.5f;
	}
}
