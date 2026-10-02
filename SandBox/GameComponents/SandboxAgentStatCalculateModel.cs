using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.GameComponents
{
	// Token: 0x020000C2 RID: 194
	public class SandboxAgentStatCalculateModel : AgentStatCalculateModel
	{
		// Token: 0x060007FA RID: 2042 RVA: 0x00036550 File Offset: 0x00034750
		public override float GetDifficultyModifier()
		{
			Campaign campaign = Campaign.Current;
			float? num;
			if (campaign == null)
			{
				num = null;
			}
			else
			{
				GameModels models = campaign.Models;
				if (models == null)
				{
					num = null;
				}
				else
				{
					DifficultyModel difficultyModel = models.DifficultyModel;
					num = ((difficultyModel != null) ? new float?(difficultyModel.GetCombatAIDifficultyMultiplier()) : null);
				}
			}
			float? num2 = num;
			if (num2 == null)
			{
				return 1f;
			}
			return num2.GetValueOrDefault();
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x000365BA File Offset: 0x000347BA
		public override bool CanAgentRideMount(Agent agent, Agent targetMount)
		{
			return agent.CheckSkillForMounting(targetMount);
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x000365C4 File Offset: 0x000347C4
		public override void InitializeAgentStats(Agent agent, Equipment spawnEquipment, AgentDrivenProperties agentDrivenProperties, AgentBuildData agentBuildData)
		{
			agentDrivenProperties.ArmorEncumbrance = this.GetEffectiveArmorEncumbrance(agent, spawnEquipment);
			if (agent.IsHero)
			{
				CharacterObject characterObject = agent.Character as CharacterObject;
				AgentFlag agentFlag = agent.GetAgentFlags();
				if (characterObject.GetPerkValue(DefaultPerks.Bow.HorseMaster))
				{
					agentFlag |= AgentFlag.CanUseAllBowsMounted;
				}
				if (characterObject.GetPerkValue(DefaultPerks.Crossbow.MountedCrossbowman))
				{
					agentFlag |= AgentFlag.CanReloadAllXBowsMounted;
				}
				if (characterObject.GetPerkValue(DefaultPerks.TwoHanded.ProjectileDeflection))
				{
					agentFlag |= AgentFlag.CanDeflectArrowsWith2HSword;
				}
				agent.SetAgentFlags(agentFlag);
			}
			else
			{
				agent.HealthLimit = this.GetEffectiveMaxHealth(agent);
				agent.Health = agent.HealthLimit;
			}
			agentDrivenProperties.OffhandWeaponDefendSpeedMultiplier = 1f;
			MissionGameModels.Current.AgentStatCalculateModel.UpdateAgentStats(agent, agentDrivenProperties);
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00036678 File Offset: 0x00034878
		public override void InitializeMissionEquipment(Agent agent)
		{
			if (agent.IsHuman)
			{
				CharacterObject characterObject = agent.Character as CharacterObject;
				if (characterObject != null)
				{
					object obj;
					if (agent == null)
					{
						obj = null;
					}
					else
					{
						IAgentOriginBase origin = agent.Origin;
						obj = ((origin != null) ? origin.BattleCombatant : null);
					}
					PartyBase partyBase = (PartyBase)obj;
					MapEvent mapEvent = ((partyBase != null) ? partyBase.MapEvent : null);
					MobileParty mobileParty = ((partyBase != null && partyBase.IsMobile) ? partyBase.MobileParty : null);
					CharacterObject characterObject2 = PartyBaseHelper.GetVisualPartyLeader(partyBase);
					if (characterObject2 == characterObject)
					{
						characterObject2 = null;
					}
					MissionEquipment equipment = agent.Equipment;
					for (int i = 0; i < 5; i++)
					{
						EquipmentIndex equipmentIndex = (EquipmentIndex)i;
						MissionWeapon missionWeapon = equipment[equipmentIndex];
						if (!missionWeapon.IsEmpty)
						{
							WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
							if (currentUsageItem != null)
							{
								if (currentUsageItem.IsConsumable && currentUsageItem.RelevantSkill != null)
								{
									ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
									if (currentUsageItem.RelevantSkill == DefaultSkills.Bow)
									{
										PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.DeepQuivers, characterObject, true, ref explainedNumber, false);
										if (characterObject2 != null && characterObject2.GetPerkValue(DefaultPerks.Bow.DeepQuivers))
										{
											explainedNumber.Add(DefaultPerks.Bow.DeepQuivers.SecondaryBonus, null, null);
										}
									}
									else if (currentUsageItem.RelevantSkill == DefaultSkills.Crossbow)
									{
										PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.Fletcher, characterObject, true, ref explainedNumber, false);
										if (characterObject2 != null && characterObject2.GetPerkValue(DefaultPerks.Crossbow.Fletcher))
										{
											explainedNumber.Add(DefaultPerks.Crossbow.Fletcher.SecondaryBonus, null, null);
										}
									}
									else if (currentUsageItem.RelevantSkill == DefaultSkills.Throwing)
									{
										PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.WellPrepared, characterObject, true, ref explainedNumber, false);
										PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.Resourceful, characterObject, true, ref explainedNumber, false);
										if (agent.HasMount)
										{
											PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.Saddlebags, characterObject, true, ref explainedNumber, false);
										}
										PerkHelper.AddPerkBonusForParty(DefaultPerks.Throwing.WellPrepared, mobileParty, false, ref explainedNumber, false);
									}
									int num = MathF.Round(explainedNumber.ResultNumber);
									ExplainedNumber explainedNumber2 = new ExplainedNumber((float)((int)missionWeapon.Amount + num), false, null);
									if (mobileParty != null && mapEvent != null && mapEvent.AttackerSide == partyBase.MapEventSide && mapEvent.EventType == MapEvent.BattleTypes.Siege)
									{
										PerkHelper.AddPerkBonusForParty(DefaultPerks.Engineering.MilitaryPlanner, mobileParty, true, ref explainedNumber2, false);
									}
									int num2 = MathF.Round(explainedNumber2.ResultNumber);
									if (num2 != (int)missionWeapon.Amount)
									{
										equipment.SetAmountOfSlot(equipmentIndex, (short)num2, true);
									}
								}
								else if (currentUsageItem.IsShield)
								{
									ExplainedNumber explainedNumber3 = new ExplainedNumber((float)missionWeapon.HitPoints, false, null);
									PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Engineering.Scaffolds, characterObject, false, ref explainedNumber3, false);
									int num3 = MathF.Round(explainedNumber3.ResultNumber);
									if (num3 != (int)missionWeapon.HitPoints)
									{
										equipment.SetHitPointsOfSlot(equipmentIndex, (short)num3, true);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00036909 File Offset: 0x00034B09
		public override void UpdateAgentStats(Agent agent, AgentDrivenProperties agentDrivenProperties)
		{
			if (agent.IsHuman)
			{
				this.UpdateHumanStats(agent, agentDrivenProperties);
				return;
			}
			this.UpdateHorseStats(agent, agentDrivenProperties);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00036924 File Offset: 0x00034B24
		public override int GetEffectiveSkill(Agent agent, SkillObject skill)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber((float)base.GetEffectiveSkill(agent, skill), false, null);
			CharacterObject characterObject = agent.Character as CharacterObject;
			Formation formation = agent.Formation;
			IAgentOriginBase origin = agent.Origin;
			PartyBase partyBase = (PartyBase)((origin != null) ? origin.BattleCombatant : null);
			MobileParty mobileParty = ((partyBase != null && partyBase.IsMobile) ? partyBase.MobileParty : null);
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
			if (characterObject2 == characterObject)
			{
				characterObject2 = null;
			}
			if (characterObject2 != null)
			{
				bool flag = skill == DefaultSkills.Bow || skill == DefaultSkills.Crossbow || skill == DefaultSkills.Throwing;
				bool flag2 = skill == DefaultSkills.OneHanded || skill == DefaultSkills.TwoHanded || skill == DefaultSkills.Polearm;
				if ((characterObject.IsInfantry && flag) || (characterObject.IsRanged && flag2))
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.FlexibleFighter, characterObject2, ref explainedNumber);
				}
			}
			if (skill == DefaultSkills.Bow)
			{
				if (characterObject2 != null)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Bow.DeadAim, characterObject2, ref explainedNumber);
					if (characterObject.HasMount())
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Bow.HorseMaster, characterObject2, ref explainedNumber);
					}
				}
			}
			else if (skill == DefaultSkills.Throwing)
			{
				if (characterObject2 != null)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Athletics.StrongArms, characterObject2, ref explainedNumber);
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.RunningThrow, characterObject2, ref explainedNumber);
				}
			}
			else if (skill == DefaultSkills.Crossbow && characterObject2 != null)
			{
				PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Crossbow.DonkeysSwiftness, characterObject2, ref explainedNumber);
			}
			if (mobileParty != null && !mobileParty.IsCurrentlyAtSea && mobileParty.HasPerk(DefaultPerks.Roguery.OneOfTheFamily, false) && characterObject.Occupation == Occupation.Bandit)
			{
				if (skill.Attributes.Any<CharacterAttribute>((CharacterAttribute attribute) => attribute == DefaultCharacterAttributes.Vigor || attribute == DefaultCharacterAttributes.Control))
				{
					explainedNumber.Add(DefaultPerks.Roguery.OneOfTheFamily.PrimaryBonus, DefaultPerks.Roguery.OneOfTheFamily.Name, null);
				}
			}
			if (characterObject.HasMount())
			{
				if (skill == DefaultSkills.Riding && characterObject2 != null)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Riding.NimbleSteed, characterObject2, ref explainedNumber);
				}
			}
			else
			{
				if (mobileParty != null && formation != null)
				{
					bool flag3 = skill == DefaultSkills.OneHanded || skill == DefaultSkills.TwoHanded || skill == DefaultSkills.Polearm;
					bool flag4 = formation.ArrangementOrder.OrderEnum == ArrangementOrder.ArrangementOrderEnum.ShieldWall;
					if (flag3 && flag4)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Polearm.Phalanx, mobileParty, true, ref explainedNumber, false);
					}
				}
				if (characterObject2 != null)
				{
					if (skill == DefaultSkills.OneHanded)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.OneHanded.WrappedHandles, characterObject2, ref explainedNumber);
					}
					else if (skill == DefaultSkills.TwoHanded)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.StrongGrip, characterObject2, ref explainedNumber);
					}
					else if (skill == DefaultSkills.Polearm)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.CleanThrust, characterObject2, ref explainedNumber);
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.CounterWeight, characterObject2, ref explainedNumber);
					}
				}
			}
			return (int)explainedNumber.ResultNumber;
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00036BC8 File Offset: 0x00034DC8
		public override float GetWeaponDamageMultiplier(Agent agent, WeaponComponentData weapon)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
			SkillObject skillObject = ((weapon != null) ? weapon.RelevantSkill : null);
			if (agent.Character is CharacterObject && skillObject != null)
			{
				if (skillObject == DefaultSkills.OneHanded)
				{
					int effectiveSkill = this.GetEffectiveSkill(agent, skillObject);
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.OneHandedDamage, ref explainedNumber, effectiveSkill);
				}
				else if (skillObject == DefaultSkills.TwoHanded)
				{
					int effectiveSkill2 = this.GetEffectiveSkill(agent, skillObject);
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.TwoHandedDamage, ref explainedNumber, effectiveSkill2);
				}
				else if (skillObject == DefaultSkills.Polearm)
				{
					int effectiveSkill3 = this.GetEffectiveSkill(agent, skillObject);
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.PolearmDamage, ref explainedNumber, effectiveSkill3);
				}
				else if (skillObject == DefaultSkills.Bow)
				{
					int effectiveSkill4 = this.GetEffectiveSkill(agent, skillObject);
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.BowDamage, ref explainedNumber, effectiveSkill4);
				}
				else if (skillObject == DefaultSkills.Throwing)
				{
					int effectiveSkill5 = this.GetEffectiveSkill(agent, skillObject);
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.ThrowingDamage, ref explainedNumber, effectiveSkill5);
				}
			}
			return Math.Max(0f, explainedNumber.ResultNumber);
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00036CBE File Offset: 0x00034EBE
		private float GetArmorStealthBonus(EquipmentElement armorElement, int maxBodyPartBonus)
		{
			if (!armorElement.IsEmpty && armorElement.Item != null && armorElement.Item.HasArmorComponent)
			{
				return (float)armorElement.GetModifiedStealthFactor();
			}
			return 0f;
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00036CF0 File Offset: 0x00034EF0
		public override float GetEquipmentStealthBonus(Agent agent)
		{
			Equipment spawnEquipment = agent.SpawnEquipment;
			return 0f + this.GetArmorStealthBonus(spawnEquipment[EquipmentIndex.NumAllWeaponSlots], 25) + this.GetArmorStealthBonus(spawnEquipment[EquipmentIndex.Cape], 15) + this.GetArmorStealthBonus(spawnEquipment[EquipmentIndex.Body], 30) + this.GetArmorStealthBonus(spawnEquipment[EquipmentIndex.Gloves], 10) + this.GetArmorStealthBonus(spawnEquipment[EquipmentIndex.Leg], 20);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00036D5C File Offset: 0x00034F5C
		public override float GetSneakAttackMultiplier(Agent agent, WeaponComponentData weapon)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
			if (weapon != null && agent.Character is CharacterObject)
			{
				int effectiveSkill = this.GetEffectiveSkill(agent, DefaultSkills.Roguery);
				SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.SneakDamage, ref explainedNumber, effectiveSkill);
				if ((weapon != null && weapon.WeaponClass == WeaponClass.Dagger) || (weapon != null && weapon.WeaponClass == WeaponClass.ThrowingKnife))
				{
					explainedNumber.AddFactor(2f, null);
				}
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00036DD0 File Offset: 0x00034FD0
		public override float GetKnockBackResistance(Agent agent)
		{
			if (agent.IsHuman)
			{
				int effectiveSkill = this.GetEffectiveSkill(agent, DefaultSkills.Athletics);
				return DefaultSkillEffects.KnockBackResistance.GetSkillEffectValue(effectiveSkill);
			}
			return float.MaxValue;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00036E04 File Offset: 0x00035004
		public override float GetKnockDownResistance(Agent agent, StrikeType strikeType = StrikeType.Invalid)
		{
			if (agent.IsHuman)
			{
				int effectiveSkill = this.GetEffectiveSkill(agent, DefaultSkills.Athletics);
				float num = DefaultSkillEffects.KnockDownResistance.GetSkillEffectValue(effectiveSkill);
				if (agent.HasMount)
				{
					num += 0.1f;
				}
				else if (strikeType == StrikeType.Thrust)
				{
					num += 0.15f;
				}
				return num;
			}
			return float.MaxValue;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00036E58 File Offset: 0x00035058
		public override float GetDismountResistance(Agent agent)
		{
			if (agent.IsHuman)
			{
				int effectiveSkill = this.GetEffectiveSkill(agent, DefaultSkills.Riding);
				return DefaultSkillEffects.DismountResistance.GetSkillEffectValue(effectiveSkill);
			}
			return float.MaxValue;
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00036E8B File Offset: 0x0003508B
		public override float GetBreatheHoldMaxDuration(Agent agent, float baseBreatheHoldMaxDuration)
		{
			return baseBreatheHoldMaxDuration;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00036E90 File Offset: 0x00035090
		public override float GetWeaponInaccuracy(Agent agent, WeaponComponentData weapon, int weaponSkill)
		{
			CharacterObject characterObject = agent.Character as CharacterObject;
			Formation formation = agent.Formation;
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
			if (characterObject == characterObject2)
			{
				characterObject2 = null;
			}
			float num = 0f;
			if (weapon.IsRangedWeapon)
			{
				ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
				if (characterObject != null)
				{
					if (weapon.RelevantSkill == DefaultSkills.Bow)
					{
						SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.BowAccuracy, ref explainedNumber, weaponSkill);
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Bow.QuickAdjustments, characterObject2, ref explainedNumber);
					}
					else if (weapon.RelevantSkill == DefaultSkills.Crossbow)
					{
						SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.CrossbowAccuracy, ref explainedNumber, weaponSkill);
					}
					else if (weapon.RelevantSkill == DefaultSkills.Throwing)
					{
						if (weapon.WeaponClass == WeaponClass.Sling)
						{
							explainedNumber = new ExplainedNumber(MathF.Max(1f - 0.007f * (float)weaponSkill, 0.2f), false, null);
						}
						else
						{
							SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.ThrowingAccuracy, ref explainedNumber, weaponSkill);
						}
					}
				}
				num = (100f - (float)weapon.Accuracy) * explainedNumber.ResultNumber * 0.001f;
			}
			else if (weapon.WeaponFlags.HasAllFlags(WeaponFlags.WideGrip))
			{
				num = 1f - (float)weaponSkill * 0.01f;
			}
			return MathF.Max(num, 0f);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00036FCC File Offset: 0x000351CC
		public override float GetInteractionDistance(Agent agent)
		{
			CharacterObject characterObject;
			if (agent.HasMount && (characterObject = agent.Character as CharacterObject) != null && characterObject.GetPerkValue(DefaultPerks.Throwing.LongReach))
			{
				return 3f;
			}
			return base.GetInteractionDistance(agent);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0003700C File Offset: 0x0003520C
		public override float GetMaxCameraZoom(Agent agent)
		{
			CharacterObject characterObject = agent.Character as CharacterObject;
			ExplainedNumber explainedNumber = new ExplainedNumber(1f, false, null);
			if (characterObject != null)
			{
				MissionEquipment equipment = agent.Equipment;
				EquipmentIndex primaryWieldedItemIndex = agent.GetPrimaryWieldedItemIndex();
				WeaponComponentData weaponComponentData = ((primaryWieldedItemIndex != EquipmentIndex.None) ? equipment[primaryWieldedItemIndex].CurrentUsageItem : null);
				if (weaponComponentData != null)
				{
					if (weaponComponentData.RelevantSkill == DefaultSkills.Bow)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.EagleEye, characterObject, true, ref explainedNumber, false);
					}
					else if (weaponComponentData.RelevantSkill == DefaultSkills.Crossbow)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.LongShots, characterObject, true, ref explainedNumber, false);
					}
					else if (weaponComponentData.RelevantSkill == DefaultSkills.Throwing)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.Focus, characterObject, true, ref explainedNumber, false);
					}
				}
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x000370C8 File Offset: 0x000352C8
		public List<PerkObject> GetPerksOfAgent(CharacterObject agentCharacter, SkillObject skill = null, bool filterPartyRole = false, PartyRole partyRole = PartyRole.Personal)
		{
			List<PerkObject> list = new List<PerkObject>();
			if (agentCharacter != null)
			{
				foreach (PerkObject perkObject in PerkObject.All)
				{
					if (agentCharacter.GetPerkValue(perkObject) && (skill == null || skill == perkObject.Skill))
					{
						if (filterPartyRole)
						{
							if (perkObject.PrimaryRole == partyRole || perkObject.SecondaryRole == partyRole)
							{
								list.Add(perkObject);
							}
						}
						else
						{
							list.Add(perkObject);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0003715C File Offset: 0x0003535C
		public override string GetMissionDebugInfoForAgent(Agent agent)
		{
			string text = "";
			text += "Base: Initial stats modified only by skills\n";
			text += "Effective (Eff): Stats that are modified by perks & mission effects\n\n";
			string text2 = "{0,-20}";
			text = string.Concat(new string[]
			{
				text,
				string.Format(text2, "Name"),
				": ",
				agent.Name,
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				string.Format(text2, "Age"),
				": ",
				(int)agent.Age,
				"\n"
			});
			text = string.Concat(new object[]
			{
				text,
				string.Format(text2, "Health"),
				": ",
				agent.Health,
				"\n"
			});
			int num = (agent.IsHuman ? agent.Character.MaxHitPoints() : agent.Monster.HitPoints);
			text = string.Concat(new object[]
			{
				text,
				string.Format(text2, "Max.Health"),
				": ",
				num,
				"(Base)\n"
			});
			text = string.Concat(new object[]
			{
				text,
				string.Format(text2, ""),
				"  ",
				MissionGameModels.Current.AgentStatCalculateModel.GetEffectiveMaxHealth(agent),
				"(Eff)\n"
			});
			text = string.Concat(new string[]
			{
				text,
				string.Format(text2, "Team"),
				": ",
				(agent.Team != null) ? (agent.Team.IsAttacker ? "Attacker" : "Defender") : "N/A",
				"\n"
			});
			if (agent.IsHuman)
			{
				string text3 = text2 + ": {1,4:G}, {2,4:G}";
				text += "-------------------------------------\n";
				text = text + string.Format(text2 + ": {1,4}, {2,4}", "Skills", "Base", "Eff") + "\n";
				text += "-------------------------------------\n";
				foreach (SkillObject skillObject in Skills.All)
				{
					int skillValue = agent.Character.GetSkillValue(skillObject);
					int effectiveSkill = MissionGameModels.Current.AgentStatCalculateModel.GetEffectiveSkill(agent, skillObject);
					string text4 = string.Format(text3, skillObject.Name, skillValue, effectiveSkill);
					text = text + text4 + "\n";
				}
				text += "-------------------------------------\n";
				CharacterObject characterObject = agent.Character as CharacterObject;
				string debugPerkInfoForAgent = this.GetDebugPerkInfoForAgent(characterObject, false, PartyRole.Personal);
				if (debugPerkInfoForAgent.Length > 0)
				{
					text = text + string.Format(text2 + ": ", "Perks") + "\n";
					text += "-------------------------------------\n";
					text += debugPerkInfoForAgent;
					text += "-------------------------------------\n";
				}
				Formation formation = agent.Formation;
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
				string debugPerkInfoForAgent2 = this.GetDebugPerkInfoForAgent(characterObject2, true, PartyRole.Captain);
				if (debugPerkInfoForAgent2.Length > 0)
				{
					text = string.Concat(new object[]
					{
						text,
						string.Format(text2 + ": ", "Captain Perks"),
						characterObject2.Name,
						"\n"
					});
					text += "-------------------------------------\n";
					text += debugPerkInfoForAgent2;
					text += "-------------------------------------\n";
				}
				IAgentOriginBase origin = agent.Origin;
				PartyBase partyBase = (PartyBase)((origin != null) ? origin.BattleCombatant : null);
				PartyBase partyBase2;
				if (partyBase == null)
				{
					partyBase2 = null;
				}
				else
				{
					MobileParty mobileParty = partyBase.MobileParty;
					partyBase2 = ((mobileParty != null) ? mobileParty.Party : null);
				}
				CharacterObject visualPartyLeader = PartyBaseHelper.GetVisualPartyLeader(partyBase2);
				string debugPerkInfoForAgent3 = this.GetDebugPerkInfoForAgent(visualPartyLeader, true, PartyRole.PartyLeader);
				if (debugPerkInfoForAgent3.Length > 0)
				{
					text = string.Concat(new object[]
					{
						text,
						string.Format(text2 + ": ", "Party Leader Perks"),
						visualPartyLeader.Name,
						"\n"
					});
					text += "-------------------------------------\n";
					text += debugPerkInfoForAgent3;
					text += "-------------------------------------\n";
				}
			}
			return text;
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x000375D0 File Offset: 0x000357D0
		public override float GetEffectiveArmorEncumbrance(Agent agent, Equipment equipment)
		{
			float totalWeightOfArmor = equipment.GetTotalWeightOfArmor(agent.IsHuman);
			float num = 1f;
			CharacterObject characterObject;
			if ((characterObject = agent.Character as CharacterObject) != null && characterObject.GetPerkValue(DefaultPerks.Athletics.FormFittingArmor))
			{
				num += DefaultPerks.Athletics.FormFittingArmor.PrimaryBonus;
			}
			return MathF.Max(0f, totalWeightOfArmor * num);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00037628 File Offset: 0x00035828
		public override float GetEffectiveMaxHealth(Agent agent)
		{
			if (agent.IsHero)
			{
				return (float)agent.Character.MaxHitPoints();
			}
			float baseHealthLimit = agent.BaseHealthLimit;
			ExplainedNumber explainedNumber = new ExplainedNumber(baseHealthLimit, false, null);
			if (agent.IsHuman)
			{
				CharacterObject characterObject = agent.Character as CharacterObject;
				IAgentOriginBase agentOriginBase = ((agent != null) ? agent.Origin : null);
				PartyBase partyBase = (PartyBase)((agentOriginBase != null) ? agentOriginBase.BattleCombatant : null);
				MobileParty mobileParty = ((partyBase != null) ? partyBase.MobileParty : null);
				CharacterObject characterObject2;
				if (mobileParty == null)
				{
					characterObject2 = null;
				}
				else
				{
					Hero leaderHero = mobileParty.LeaderHero;
					characterObject2 = ((leaderHero != null) ? leaderHero.CharacterObject : null);
				}
				CharacterObject characterObject3 = characterObject2;
				if (characterObject != null && characterObject3 != null)
				{
					if (!mobileParty.IsCurrentlyAtSea)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.TwoHanded.ThickHides, mobileParty, false, ref explainedNumber, false);
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Polearm.HardyFrontline, mobileParty, true, ref explainedNumber, false);
					}
					if (characterObject.IsRanged)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Crossbow.PickedShots, mobileParty, false, ref explainedNumber, false);
					}
					if (!agent.HasMount)
					{
						if (!mobileParty.IsCurrentlyAtSea)
						{
							PerkHelper.AddPerkBonusForParty(DefaultPerks.Athletics.WellBuilt, mobileParty, false, ref explainedNumber, false);
						}
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Polearm.HardKnock, mobileParty, false, ref explainedNumber, false);
						if (!mobileParty.IsCurrentlyAtSea && characterObject.IsInfantry)
						{
							PerkHelper.AddPerkBonusForParty(DefaultPerks.OneHanded.UnwaveringDefense, mobileParty, false, ref explainedNumber, false);
						}
					}
					if (characterObject3.GetPerkValue(DefaultPerks.Medicine.MinisterOfHealth))
					{
						int num = (int)((float)MathF.Max(characterObject3.GetSkillValue(DefaultSkills.Medicine) - Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus, 0) * DefaultPerks.Medicine.MinisterOfHealth.PrimaryBonus);
						if (num > 0)
						{
							explainedNumber.Add((float)num, null, null);
						}
					}
				}
			}
			else
			{
				Agent riderAgent = agent.RiderAgent;
				if (riderAgent != null)
				{
					CharacterObject characterObject4 = ((riderAgent != null) ? riderAgent.Character : null) as CharacterObject;
					object obj;
					if (riderAgent == null)
					{
						obj = null;
					}
					else
					{
						IAgentOriginBase origin = riderAgent.Origin;
						obj = ((origin != null) ? origin.BattleCombatant : null);
					}
					PartyBase partyBase2 = (PartyBase)obj;
					MobileParty mobileParty2 = ((partyBase2 != null) ? partyBase2.MobileParty : null);
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.Sledges, mobileParty2, false, ref explainedNumber, false);
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.Veterinary, characterObject4, true, ref explainedNumber, false);
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Riding.Veterinary, mobileParty2, false, ref explainedNumber, false);
				}
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x00037830 File Offset: 0x00035A30
		public override float GetEnvironmentSpeedFactor(Agent agent)
		{
			Scene scene = agent.Mission.Scene;
			float num = 1f;
			if (!agent.Mission.Scene.IsAtmosphereIndoor)
			{
				if (agent.Mission.Scene.GetRainDensity() > 0f)
				{
					num *= 0.9f;
				}
				if (!agent.IsHuman && CampaignTime.Now.IsNightTime)
				{
					num *= 0.9f;
				}
			}
			return num;
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x000378A0 File Offset: 0x00035AA0
		private string GetDebugPerkInfoForAgent(CharacterObject agentCharacter, bool filterPartyRole = false, PartyRole partyRole = PartyRole.Personal)
		{
			string text = "";
			string text2 = "{0,-18}";
			if (this.GetPerksOfAgent(agentCharacter, null, filterPartyRole, partyRole).Count > 0)
			{
				foreach (SkillObject skillObject in Skills.All)
				{
					List<PerkObject> perksOfAgent = this.GetPerksOfAgent(agentCharacter, skillObject, filterPartyRole, partyRole);
					if (perksOfAgent != null && perksOfAgent.Count > 0)
					{
						string text3 = string.Format(text2, skillObject.Name) + ": ";
						int num = 5;
						int num2 = 0;
						foreach (PerkObject perkObject in perksOfAgent)
						{
							string text4 = perkObject.Name.ToString();
							if (num2 == num)
							{
								text3 = text3 + "\n" + string.Format(text2, "") + "  ";
								num2 = 0;
							}
							text3 = text3 + text4 + ", ";
							num2++;
						}
						text3 = text3.Remove(text3.LastIndexOf(","));
						text = text + text3 + "\n";
					}
				}
			}
			return text;
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x000379F4 File Offset: 0x00035BF4
		private void UpdateHumanStats(Agent agent, AgentDrivenProperties agentDrivenProperties)
		{
			Equipment spawnEquipment = agent.SpawnEquipment;
			agentDrivenProperties.ArmorHead = spawnEquipment.GetHeadArmorSum();
			agentDrivenProperties.ArmorTorso = spawnEquipment.GetHumanBodyArmorSum();
			agentDrivenProperties.ArmorLegs = spawnEquipment.GetLegArmorSum();
			agentDrivenProperties.ArmorArms = spawnEquipment.GetArmArmorSum();
			BasicCharacterObject character = agent.Character;
			CharacterObject characterObject = character as CharacterObject;
			MissionEquipment equipment = agent.Equipment;
			float num = equipment.GetTotalWeightOfWeapons();
			float effectiveArmorEncumbrance = this.GetEffectiveArmorEncumbrance(agent, spawnEquipment);
			int weight = agent.Monster.Weight;
			EquipmentIndex primaryWieldedItemIndex = agent.GetPrimaryWieldedItemIndex();
			EquipmentIndex offhandWieldedItemIndex = agent.GetOffhandWieldedItemIndex();
			if (primaryWieldedItemIndex != EquipmentIndex.None)
			{
				ItemObject item = equipment[primaryWieldedItemIndex].Item;
				WeaponComponent weaponComponent = item.WeaponComponent;
				if (weaponComponent != null)
				{
					ItemObject.ItemTypeEnum itemType = weaponComponent.GetItemType();
					bool flag = false;
					if (characterObject != null)
					{
						bool flag2 = itemType == ItemObject.ItemTypeEnum.Bow && characterObject.GetPerkValue(DefaultPerks.Bow.RangersSwiftness);
						bool flag3 = itemType == ItemObject.ItemTypeEnum.Crossbow && characterObject.GetPerkValue(DefaultPerks.Crossbow.LooseAndMove);
						flag = flag2 || flag3;
					}
					if (!flag)
					{
						float realWeaponLength = weaponComponent.PrimaryWeapon.GetRealWeaponLength();
						num += 4f * MathF.Sqrt(realWeaponLength) * item.Weight;
					}
				}
			}
			if (offhandWieldedItemIndex != EquipmentIndex.None)
			{
				ItemObject item2 = equipment[offhandWieldedItemIndex].Item;
				WeaponComponentData primaryWeapon = item2.PrimaryWeapon;
				if (primaryWeapon != null && primaryWeapon.WeaponFlags.HasAnyFlag(WeaponFlags.CanBlockRanged) && (characterObject == null || !characterObject.GetPerkValue(DefaultPerks.OneHanded.ShieldBearer)))
				{
					num += 1.5f * item2.Weight;
				}
			}
			agentDrivenProperties.WeaponExternalAccelerationAccuracyPenalty = 0f;
			agentDrivenProperties.WeaponsEncumbrance = num;
			agentDrivenProperties.ArmorEncumbrance = effectiveArmorEncumbrance;
			float num2 = effectiveArmorEncumbrance + num;
			EquipmentIndex primaryWieldedItemIndex2 = agent.GetPrimaryWieldedItemIndex();
			WeaponComponentData weaponComponentData = ((primaryWieldedItemIndex2 != EquipmentIndex.None) ? equipment[primaryWieldedItemIndex2].CurrentUsageItem : null);
			EquipmentIndex offhandWieldedItemIndex2 = agent.GetOffhandWieldedItemIndex();
			WeaponComponentData weaponComponentData2 = ((offhandWieldedItemIndex2 != EquipmentIndex.None) ? equipment[offhandWieldedItemIndex2].CurrentUsageItem : null);
			agentDrivenProperties.SwingSpeedMultiplier = 0.93f;
			agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier = 0.93f;
			agentDrivenProperties.HandlingMultiplier = 1f;
			agentDrivenProperties.ShieldBashStunDurationMultiplier = 1f;
			agentDrivenProperties.KickStunDurationMultiplier = 1f;
			agentDrivenProperties.ReloadSpeed = 0.93f;
			agentDrivenProperties.MissileSpeedMultiplier = 1f;
			agentDrivenProperties.ReloadMovementPenaltyFactor = 1f;
			agentDrivenProperties.DamageMultiplierBonus = 0f;
			base.SetAllWeaponInaccuracy(agent, agentDrivenProperties, (int)primaryWieldedItemIndex2, weaponComponentData);
			int effectiveSkill = this.GetEffectiveSkill(agent, DefaultSkills.Athletics);
			int effectiveSkill2 = this.GetEffectiveSkill(agent, DefaultSkills.Riding);
			if (weaponComponentData != null)
			{
				WeaponComponentData weaponComponentData3 = weaponComponentData;
				int effectiveSkillForWeapon = this.GetEffectiveSkillForWeapon(agent, weaponComponentData3);
				if (weaponComponentData3.IsRangedWeapon)
				{
					int thrustSpeed = weaponComponentData3.ThrustSpeed;
					if (!agent.HasMount)
					{
						float num3 = MathF.Max(0f, 1f - (float)effectiveSkillForWeapon / 500f);
						agentDrivenProperties.WeaponMaxMovementAccuracyPenalty = MathF.Max(0f, 0.125f * num3);
						agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty = MathF.Max(0f, 0.1f * num3);
					}
					else
					{
						float num4 = MathF.Max(0f, (1f - (float)effectiveSkillForWeapon / 500f) * (1f - (float)effectiveSkill2 / 1800f));
						agentDrivenProperties.WeaponMaxMovementAccuracyPenalty = MathF.Max(0f, 0.025f * num4);
						agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty = MathF.Max(0f, 0.12f * num4);
					}
					if (weaponComponentData3.RelevantSkill == DefaultSkills.Bow)
					{
						float num5 = ((float)thrustSpeed - 45f) / 90f;
						num5 = MBMath.ClampFloat(num5, 0f, 1f);
						agentDrivenProperties.WeaponMaxMovementAccuracyPenalty *= 6f;
						agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty *= 4.5f / MBMath.Lerp(0.75f, 2f, num5, 1E-05f);
					}
					else if (weaponComponentData3.RelevantSkill == DefaultSkills.Throwing)
					{
						if (weaponComponentData3.WeaponClass == WeaponClass.Sling)
						{
							float num6 = ((float)thrustSpeed - 30f) / 90f;
							num6 = MBMath.ClampFloat(num6, 0f, 1f);
							agentDrivenProperties.WeaponMaxMovementAccuracyPenalty *= 5f;
							agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty *= 2.4f * MBMath.Lerp(2.4f, 1.2f, num6, 1E-05f);
						}
						else
						{
							float num7 = ((float)thrustSpeed - 91f) / 14f;
							num7 = MBMath.ClampFloat(num7, 0f, 1f);
							agentDrivenProperties.WeaponMaxMovementAccuracyPenalty *= 0.5f;
							agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty *= 1.5f * MBMath.Lerp(1.5f, 0.8f, num7, 1E-05f);
						}
					}
					else if (weaponComponentData3.RelevantSkill == DefaultSkills.Crossbow)
					{
						agentDrivenProperties.WeaponMaxMovementAccuracyPenalty *= 2.5f;
						agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty *= 1.2f;
					}
					if (weaponComponentData3.WeaponClass == WeaponClass.Bow)
					{
						agentDrivenProperties.WeaponBestAccuracyWaitTime = 0.3f + (95.75f - (float)thrustSpeed) * 0.005f;
						float num8 = ((float)thrustSpeed - 45f) / 90f;
						num8 = MBMath.ClampFloat(num8, 0f, 1f);
						agentDrivenProperties.WeaponUnsteadyBeginTime = 0.6f + (float)effectiveSkillForWeapon * 0.01f * MBMath.Lerp(2f, 4f, num8, 1E-05f);
						if (agent.IsAIControlled)
						{
							agentDrivenProperties.WeaponUnsteadyBeginTime *= 4f;
						}
						agentDrivenProperties.WeaponUnsteadyEndTime = 2f + agentDrivenProperties.WeaponUnsteadyBeginTime;
						agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians = 0.1f;
					}
					else if (weaponComponentData3.WeaponClass == WeaponClass.Javelin || weaponComponentData3.WeaponClass == WeaponClass.ThrowingAxe || weaponComponentData3.WeaponClass == WeaponClass.ThrowingKnife)
					{
						agentDrivenProperties.WeaponBestAccuracyWaitTime = 0.2f + (89f - (float)thrustSpeed) * 0.009f;
						agentDrivenProperties.WeaponUnsteadyBeginTime = 2.5f + (float)effectiveSkillForWeapon * 0.01f;
						agentDrivenProperties.WeaponUnsteadyEndTime = 10f + agentDrivenProperties.WeaponUnsteadyBeginTime;
						agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians = 0.025f;
					}
					else if (weaponComponentData3.WeaponClass == WeaponClass.Sling)
					{
						agentDrivenProperties.WeaponBestAccuracyWaitTime = 2.6f + (89f - (float)thrustSpeed) * 0.12f;
						agentDrivenProperties.WeaponUnsteadyBeginTime = 3f + (float)effectiveSkillForWeapon * 0.064f;
						agentDrivenProperties.WeaponUnsteadyEndTime = 22f + agentDrivenProperties.WeaponUnsteadyBeginTime;
						agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians = 0.2f;
					}
					else
					{
						agentDrivenProperties.WeaponBestAccuracyWaitTime = 0.1f;
						agentDrivenProperties.WeaponUnsteadyBeginTime = 0f;
						agentDrivenProperties.WeaponUnsteadyEndTime = 0f;
						agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians = 0.1f;
					}
				}
				else if (weaponComponentData3.WeaponFlags.HasAllFlags(WeaponFlags.WideGrip))
				{
					agentDrivenProperties.WeaponUnsteadyBeginTime = 1f + (float)effectiveSkillForWeapon * 0.005f;
					agentDrivenProperties.WeaponUnsteadyEndTime = 3f + (float)effectiveSkillForWeapon * 0.01f;
				}
			}
			agentDrivenProperties.TopSpeedReachDuration = 2.5f + MathF.Max(5f - (1f + (float)effectiveSkill * 0.01f), 1f) / 3.5f - MathF.Min((float)weight / ((float)weight + num2), 0.8f);
			ExplainedNumber explainedNumber = new ExplainedNumber(0.7f, false, null);
			ExplainedNumber explainedNumber2 = new ExplainedNumber(0.7f, false, null);
			SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.AthleticsSpeedFactor, ref explainedNumber, effectiveSkill);
			SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.AthleticsSpeedFactor, ref explainedNumber2, 300);
			ExplainedNumber explainedNumber3 = new ExplainedNumber(0.2f, false, null);
			explainedNumber3.LimitMin(0f);
			SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.AthleticsWeightFactor, ref explainedNumber3, effectiveSkill);
			float num9 = explainedNumber3.ResultNumber * num2 / (float)weight;
			float num10 = MBMath.ClampFloat(explainedNumber.ResultNumber - num9, 0f, explainedNumber2.ResultNumber);
			agentDrivenProperties.MaxSpeedMultiplier = this.GetEnvironmentSpeedFactor(agent) * num10;
			float managedParameter = TaleWorlds.Core.ManagedParameters.Instance.GetManagedParameter(TaleWorlds.Core.ManagedParametersEnum.BipedalCombatSpeedMinMultiplier);
			float managedParameter2 = TaleWorlds.Core.ManagedParameters.Instance.GetManagedParameter(TaleWorlds.Core.ManagedParametersEnum.BipedalCombatSpeedMaxMultiplier);
			float num11 = MathF.Min(num2 / (float)weight, 1f);
			agentDrivenProperties.CombatMaxSpeedMultiplier = MathF.Min(MBMath.Lerp(managedParameter2, managedParameter, num11, 1E-05f), 1f);
			agentDrivenProperties.CrouchedSpeedMultiplier = 1f;
			agentDrivenProperties.AttributeShieldMissileCollisionBodySizeAdder = 0.3f;
			Agent mountAgent = agent.MountAgent;
			float num12 = ((mountAgent != null) ? mountAgent.GetAgentDrivenPropertyValue(DrivenProperty.AttributeRiding) : 1f);
			agentDrivenProperties.AttributeRiding = (float)effectiveSkill2 * num12;
			agentDrivenProperties.AttributeHorseArchery = MissionGameModels.Current.StrikeMagnitudeModel.CalculateHorseArcheryFactor(character);
			agentDrivenProperties.BipedalRangedReadySpeedMultiplier = TaleWorlds.Core.ManagedParameters.Instance.GetManagedParameter(TaleWorlds.Core.ManagedParametersEnum.BipedalRangedReadySpeedMultiplier);
			agentDrivenProperties.BipedalRangedReloadSpeedMultiplier = TaleWorlds.Core.ManagedParameters.Instance.GetManagedParameter(TaleWorlds.Core.ManagedParametersEnum.BipedalRangedReloadSpeedMultiplier);
			if (characterObject != null)
			{
				if (weaponComponentData != null)
				{
					this.SetWeaponSkillEffectsOnAgent(agent, characterObject, agentDrivenProperties, weaponComponentData);
					if (agent.HasMount)
					{
						this.SetMountedPenaltiesOnAgent(agent, agentDrivenProperties, weaponComponentData);
					}
				}
				this.SetPerkAndBannerEffectsOnAgent(agent, characterObject, agentDrivenProperties, weaponComponentData);
			}
			base.SetAiRelatedProperties(agent, agentDrivenProperties, weaponComponentData, weaponComponentData2);
			float num13 = 1f;
			if (!agent.Mission.Scene.IsAtmosphereIndoor)
			{
				float rainDensity = agent.Mission.Scene.GetRainDensity();
				float fog = agent.Mission.Scene.GetFog();
				if (rainDensity > 0f || fog > 0f)
				{
					num13 += MathF.Min(0.3f, rainDensity + fog);
				}
				if (CampaignTime.Now.IsNightTime)
				{
					num13 += 0.1f;
				}
			}
			agentDrivenProperties.AiShooterError *= num13;
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0003831C File Offset: 0x0003651C
		private void UpdateHorseStats(Agent agent, AgentDrivenProperties agentDrivenProperties)
		{
			Equipment spawnEquipment = agent.SpawnEquipment;
			EquipmentElement equipmentElement = spawnEquipment[EquipmentIndex.ArmorItemEndSlot];
			ItemObject item = equipmentElement.Item;
			EquipmentElement equipmentElement2 = spawnEquipment[EquipmentIndex.HorseHarness];
			agentDrivenProperties.AiSpeciesIndex = (int)item.Id.InternalValue;
			agentDrivenProperties.AttributeRiding = 0.8f + ((equipmentElement2.Item != null) ? 0.2f : 0f);
			float num = 0f;
			for (int i = 1; i < 12; i++)
			{
				if (spawnEquipment[i].Item != null)
				{
					num += (float)spawnEquipment[i].GetModifiedMountBodyArmor();
				}
			}
			agentDrivenProperties.ArmorTorso = num;
			int modifiedMountManeuver = equipmentElement.GetModifiedMountManeuver(in equipmentElement2);
			int num2 = equipmentElement.GetModifiedMountSpeed(in equipmentElement2) + 1;
			int num3 = 0;
			float environmentSpeedFactor = this.GetEnvironmentSpeedFactor(agent);
			bool flag = Campaign.Current.Models.MapWeatherModel.GetWeatherEffectOnTerrainForPosition(MobileParty.MainParty.Position.ToVec2()) == MapWeatherModel.WeatherEventEffectOnTerrain.Wet;
			Agent riderAgent = agent.RiderAgent;
			if (riderAgent != null)
			{
				CharacterObject characterObject = riderAgent.Character as CharacterObject;
				Formation formation = riderAgent.Formation;
				Agent agent2 = ((formation != null) ? formation.Captain : null);
				if (agent2 == riderAgent)
				{
					agent2 = null;
				}
				CharacterObject characterObject2 = ((agent2 != null) ? agent2.Character : null) as CharacterObject;
				BannerComponent activeBanner = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(formation);
				ExplainedNumber explainedNumber = new ExplainedNumber((float)modifiedMountManeuver, false, null);
				ExplainedNumber explainedNumber2 = new ExplainedNumber((float)num2, false, null);
				num3 = this.GetEffectiveSkill(agent.RiderAgent, DefaultSkills.Riding);
				SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.HorseManeuver, ref explainedNumber, num3);
				SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.HorseSpeed, ref explainedNumber2, num3);
				if (activeBanner != null)
				{
					BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedMountMovementSpeed, activeBanner, ref explainedNumber2);
				}
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.NimbleSteed, characterObject, true, ref explainedNumber, false);
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.SweepingWind, characterObject, true, ref explainedNumber2, false);
				ExplainedNumber explainedNumber3 = new ExplainedNumber(agentDrivenProperties.ArmorTorso, false, null);
				PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Riding.ToughSteed, characterObject2, ref explainedNumber3);
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.ToughSteed, characterObject, true, ref explainedNumber3, false);
				if (characterObject.GetPerkValue(DefaultPerks.Riding.TheWayOfTheSaddle))
				{
					float num4 = (float)MathF.Max(num3 - Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus, 0) * DefaultPerks.Riding.TheWayOfTheSaddle.PrimaryBonus;
					explainedNumber.Add(num4, null, null);
				}
				if (equipmentElement2.Item == null)
				{
					explainedNumber.AddFactor(-0.1f, null);
					explainedNumber2.AddFactor(-0.1f, null);
				}
				if (flag)
				{
					explainedNumber2.AddFactor(-0.25f, null);
				}
				agentDrivenProperties.ArmorTorso = explainedNumber3.ResultNumber;
				agentDrivenProperties.MountManeuver = explainedNumber.ResultNumber;
				agentDrivenProperties.MountSpeed = environmentSpeedFactor * 0.22f * (1f + explainedNumber2.ResultNumber);
			}
			else
			{
				agentDrivenProperties.MountManeuver = (float)modifiedMountManeuver;
				agentDrivenProperties.MountSpeed = environmentSpeedFactor * 0.22f * (float)(1 + num2);
			}
			float num5 = equipmentElement.Weight / 2f + (equipmentElement2.IsEmpty ? 0f : equipmentElement2.Weight);
			agentDrivenProperties.MountDashAccelerationMultiplier = ((num5 > 200f) ? ((num5 < 300f) ? (1f - (num5 - 200f) / 111f) : 0.1f) : 1f);
			if (flag)
			{
				agentDrivenProperties.MountDashAccelerationMultiplier *= 0.75f;
			}
			agentDrivenProperties.TopSpeedReachDuration = Game.Current.BasicModels.RidingModel.CalculateAcceleration(in equipmentElement, in equipmentElement2, num3);
			agentDrivenProperties.MountChargeDamage = (float)equipmentElement.GetModifiedMountCharge(in equipmentElement2) * 0.004f;
			agentDrivenProperties.MountDifficulty = (float)equipmentElement.Item.Difficulty;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x000386B4 File Offset: 0x000368B4
		private void SetPerkAndBannerEffectsOnAgent(Agent agent, CharacterObject agentCharacter, AgentDrivenProperties agentDrivenProperties, WeaponComponentData equippedWeaponComponent)
		{
			Formation formation = agent.Formation;
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
			CharacterObject characterObject = obj as CharacterObject;
			Formation formation2 = agent.Formation;
			if (((formation2 != null) ? formation2.Captain : null) == agent)
			{
				characterObject = null;
			}
			ItemObject itemObject = null;
			EquipmentIndex offhandWieldedItemIndex = agent.GetOffhandWieldedItemIndex();
			if (offhandWieldedItemIndex != EquipmentIndex.None)
			{
				itemObject = agent.Equipment[offhandWieldedItemIndex].Item;
			}
			BannerComponent activeBanner = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(agent.Formation);
			bool flag = equippedWeaponComponent != null && equippedWeaponComponent.IsRangedWeapon;
			bool flag2 = equippedWeaponComponent != null && equippedWeaponComponent.IsMeleeWeapon;
			bool flag3 = itemObject != null && itemObject.PrimaryWeapon.IsShield;
			ExplainedNumber explainedNumber = new ExplainedNumber(agentDrivenProperties.CombatMaxSpeedMultiplier, false, null);
			ExplainedNumber explainedNumber2 = new ExplainedNumber(agentDrivenProperties.MaxSpeedMultiplier, false, null);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.FleetOfFoot, agentCharacter, true, ref explainedNumber, false);
			ExplainedNumber explainedNumber3 = new ExplainedNumber(agentDrivenProperties.KickStunDurationMultiplier, false, null);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Roguery.DirtyFighting, agentCharacter, true, ref explainedNumber3, false);
			agentDrivenProperties.KickStunDurationMultiplier = explainedNumber3.ResultNumber;
			if (equippedWeaponComponent != null)
			{
				ExplainedNumber explainedNumber4 = new ExplainedNumber(agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier, false, null);
				if (flag2)
				{
					ExplainedNumber explainedNumber5 = new ExplainedNumber(agentDrivenProperties.SwingSpeedMultiplier, false, null);
					ExplainedNumber explainedNumber6 = new ExplainedNumber(agentDrivenProperties.HandlingMultiplier, false, null);
					if (!agent.HasMount)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.Fury, agentCharacter, true, ref explainedNumber6, false);
						if (characterObject != null)
						{
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Athletics.Fury, characterObject, ref explainedNumber6);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.OnTheEdge, characterObject, ref explainedNumber5);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.BladeMaster, characterObject, ref explainedNumber5);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.SwiftSwing, characterObject, ref explainedNumber5);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.BladeMaster, characterObject, ref explainedNumber4);
						}
					}
					if (equippedWeaponComponent.RelevantSkill == DefaultSkills.OneHanded)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.SwiftStrike, agentCharacter, true, ref explainedNumber5, false);
						PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.OneHanded.WayOfTheSword, agentCharacter, DefaultSkills.OneHanded, true, ref explainedNumber5, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus, false);
						PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.OneHanded.WayOfTheSword, agentCharacter, DefaultSkills.OneHanded, true, ref explainedNumber4, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus, false);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.WrappedHandles, agentCharacter, true, ref explainedNumber6, false);
					}
					else if (equippedWeaponComponent.RelevantSkill == DefaultSkills.TwoHanded)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.OnTheEdge, agentCharacter, true, ref explainedNumber5, false);
						PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.TwoHanded.WayOfTheGreatAxe, agentCharacter, DefaultSkills.TwoHanded, true, ref explainedNumber5, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus, false);
						PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.TwoHanded.WayOfTheGreatAxe, agentCharacter, DefaultSkills.TwoHanded, true, ref explainedNumber4, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus, false);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.TwoHanded.StrongGrip, agentCharacter, true, ref explainedNumber6, false);
					}
					else if (equippedWeaponComponent.RelevantSkill == DefaultSkills.Polearm)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.Footwork, agentCharacter, true, ref explainedNumber, false);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.SwiftSwing, agentCharacter, true, ref explainedNumber5, false);
						PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Polearm.WayOfTheSpear, agentCharacter, DefaultSkills.Polearm, true, ref explainedNumber5, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus, false);
						PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Polearm.WayOfTheSpear, agentCharacter, DefaultSkills.Polearm, true, ref explainedNumber4, Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus, false);
						if (equippedWeaponComponent.SwingDamageType != DamageTypes.Invalid)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Polearm.CounterWeight, agentCharacter, true, ref explainedNumber6, false);
						}
					}
					agentDrivenProperties.SwingSpeedMultiplier = explainedNumber5.ResultNumber;
					agentDrivenProperties.HandlingMultiplier = explainedNumber6.ResultNumber;
				}
				if (flag)
				{
					ExplainedNumber explainedNumber7 = new ExplainedNumber(agentDrivenProperties.WeaponInaccuracy, false, null);
					ExplainedNumber explainedNumber8 = new ExplainedNumber(agentDrivenProperties.WeaponMaxMovementAccuracyPenalty, false, null);
					ExplainedNumber explainedNumber9 = new ExplainedNumber(agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty, false, null);
					ExplainedNumber explainedNumber10 = new ExplainedNumber(agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians, false, null);
					ExplainedNumber explainedNumber11 = new ExplainedNumber(agentDrivenProperties.WeaponUnsteadyBeginTime, false, null);
					ExplainedNumber explainedNumber12 = new ExplainedNumber(agentDrivenProperties.WeaponUnsteadyEndTime, false, null);
					ExplainedNumber explainedNumber13 = new ExplainedNumber(agentDrivenProperties.ReloadMovementPenaltyFactor, false, null);
					ExplainedNumber explainedNumber14 = new ExplainedNumber(agentDrivenProperties.ReloadSpeed, false, null);
					ExplainedNumber explainedNumber15 = new ExplainedNumber(agentDrivenProperties.MissileSpeedMultiplier, false, null);
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.NockingPoint, agentCharacter, true, ref explainedNumber13, false);
					if (characterObject != null)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Crossbow.LooseAndMove, characterObject, ref explainedNumber2);
					}
					if (activeBanner != null)
					{
						BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.DecreasedRangedAccuracyPenalty, activeBanner, ref explainedNumber7);
					}
					if (agent.HasMount)
					{
						if (agentCharacter.GetPerkValue(DefaultPerks.Riding.Sagittarius))
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.Sagittarius, agentCharacter, true, ref explainedNumber8, false);
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Riding.Sagittarius, agentCharacter, true, ref explainedNumber9, false);
						}
						if (characterObject != null && characterObject.GetPerkValue(DefaultPerks.Riding.Sagittarius))
						{
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Riding.Sagittarius, characterObject, ref explainedNumber8);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Riding.Sagittarius, characterObject, ref explainedNumber9);
						}
						if (equippedWeaponComponent.RelevantSkill == DefaultSkills.Bow && agentCharacter.GetPerkValue(DefaultPerks.Bow.MountedArchery))
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.MountedArchery, agentCharacter, true, ref explainedNumber8, false);
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.MountedArchery, agentCharacter, true, ref explainedNumber9, false);
						}
						if (equippedWeaponComponent.RelevantSkill == DefaultSkills.Throwing && agentCharacter.GetPerkValue(DefaultPerks.Throwing.MountedSkirmisher))
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.MountedSkirmisher, agentCharacter, true, ref explainedNumber8, false);
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.MountedSkirmisher, agentCharacter, true, ref explainedNumber9, false);
						}
					}
					bool flag4 = false;
					if (equippedWeaponComponent.RelevantSkill == DefaultSkills.Bow)
					{
						flag4 = true;
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.BowControl, agentCharacter, true, ref explainedNumber8, false);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.RapidFire, agentCharacter, true, ref explainedNumber14, false);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.QuickAdjustments, agentCharacter, true, ref explainedNumber10, false);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.Discipline, agentCharacter, true, ref explainedNumber11, false);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.Discipline, agentCharacter, true, ref explainedNumber12, false);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Bow.QuickDraw, agentCharacter, true, ref explainedNumber4, false);
						if (characterObject != null)
						{
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Bow.RapidFire, characterObject, ref explainedNumber14);
							if (!agent.HasMount)
							{
								PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Bow.NockingPoint, characterObject, ref explainedNumber2);
							}
						}
						PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Bow.Deadshot, agentCharacter, DefaultSkills.Bow, true, ref explainedNumber14, Campaign.Current.Models.CharacterDevelopmentModel.MinSkillRequiredForEpicPerkBonus, false);
					}
					else if (equippedWeaponComponent.RelevantSkill == DefaultSkills.Crossbow)
					{
						flag4 = true;
						if (agent.HasMount)
						{
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.Steady, agentCharacter, true, ref explainedNumber8, false);
							PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.Steady, agentCharacter, true, ref explainedNumber10, false);
						}
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.WindWinder, agentCharacter, true, ref explainedNumber14, false);
						if (characterObject != null)
						{
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Crossbow.WindWinder, characterObject, ref explainedNumber14);
						}
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.DonkeysSwiftness, agentCharacter, true, ref explainedNumber8, false);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Crossbow.Marksmen, agentCharacter, true, ref explainedNumber4, false);
						PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Crossbow.MightyPull, agentCharacter, DefaultSkills.Crossbow, true, ref explainedNumber14, Campaign.Current.Models.CharacterDevelopmentModel.MinSkillRequiredForEpicPerkBonus, false);
					}
					else if (equippedWeaponComponent.RelevantSkill == DefaultSkills.Throwing)
					{
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.QuickDraw, agentCharacter, true, ref explainedNumber14, false);
						PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Throwing.PerfectTechnique, agentCharacter, true, ref explainedNumber15, false);
						if (characterObject != null)
						{
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.QuickDraw, characterObject, ref explainedNumber14);
							PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Throwing.PerfectTechnique, characterObject, ref explainedNumber15);
						}
						PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Throwing.UnstoppableForce, agentCharacter, DefaultSkills.Throwing, true, ref explainedNumber15, Campaign.Current.Models.CharacterDevelopmentModel.MinSkillRequiredForEpicPerkBonus, false);
					}
					if (flag4 && Campaign.Current.Models.MapWeatherModel.GetWeatherEffectOnTerrainForPosition(MobileParty.MainParty.Position.ToVec2()) == MapWeatherModel.WeatherEventEffectOnTerrain.Wet)
					{
						explainedNumber15.AddFactor(-0.2f, null);
					}
					agentDrivenProperties.ReloadMovementPenaltyFactor = explainedNumber13.ResultNumber;
					agentDrivenProperties.ReloadSpeed = explainedNumber14.ResultNumber;
					agentDrivenProperties.MissileSpeedMultiplier = explainedNumber15.ResultNumber;
					agentDrivenProperties.WeaponInaccuracy = explainedNumber7.ResultNumber;
					agentDrivenProperties.WeaponMaxMovementAccuracyPenalty = explainedNumber8.ResultNumber;
					agentDrivenProperties.WeaponMaxUnsteadyAccuracyPenalty = explainedNumber9.ResultNumber;
					agentDrivenProperties.WeaponUnsteadyBeginTime = explainedNumber11.ResultNumber;
					agentDrivenProperties.WeaponUnsteadyEndTime = explainedNumber12.ResultNumber;
					agentDrivenProperties.WeaponRotationalAccuracyPenaltyInRadians = explainedNumber10.ResultNumber;
				}
				agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier = explainedNumber4.ResultNumber;
			}
			if (flag3)
			{
				ExplainedNumber explainedNumber16 = new ExplainedNumber(agentDrivenProperties.AttributeShieldMissileCollisionBodySizeAdder, false, null);
				if (characterObject != null)
				{
					Formation formation3 = agent.Formation;
					if (formation3 != null && formation3.ArrangementOrder.OrderEnum == ArrangementOrder.ArrangementOrderEnum.ShieldWall)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.OneHanded.ShieldWall, characterObject, ref explainedNumber16);
					}
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.OneHanded.ArrowCatcher, characterObject, ref explainedNumber16);
				}
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.ArrowCatcher, agentCharacter, true, ref explainedNumber16, false);
				agentDrivenProperties.AttributeShieldMissileCollisionBodySizeAdder = explainedNumber16.ResultNumber;
				ExplainedNumber explainedNumber17 = new ExplainedNumber(agentDrivenProperties.ShieldBashStunDurationMultiplier, false, null);
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.OneHanded.Basher, agentCharacter, true, ref explainedNumber17, false);
				agentDrivenProperties.ShieldBashStunDurationMultiplier = explainedNumber17.ResultNumber;
			}
			else
			{
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.MorningExercise, agentCharacter, true, ref explainedNumber2, false);
				PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Medicine.SelfMedication, agentCharacter, false, ref explainedNumber2, false);
				if (!flag3 && !flag)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Athletics.Sprint, agentCharacter, true, ref explainedNumber2, false);
				}
				if (equippedWeaponComponent == null && itemObject == null)
				{
					PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Roguery.FleetFooted, agentCharacter, true, ref explainedNumber2, false);
				}
				if (characterObject != null)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Athletics.MorningExercise, characterObject, ref explainedNumber2);
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.OneHanded.ShieldBearer, characterObject, ref explainedNumber2);
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.OneHanded.FleetOfFoot, characterObject, ref explainedNumber2);
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.TwoHanded.RecklessCharge, characterObject, ref explainedNumber2);
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Polearm.Footwork, characterObject, ref explainedNumber2);
					if (agentCharacter.Tier >= 3)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Athletics.FormFittingArmor, characterObject, ref explainedNumber2);
					}
					if (agentCharacter.IsInfantry)
					{
						PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Athletics.Sprint, characterObject, ref explainedNumber2);
					}
				}
			}
			float num = 0f;
			float num2 = 0f;
			bool flag5 = false;
			if (characterObject != null)
			{
				if (agent.HasMount && characterObject.GetPerkValue(DefaultPerks.Riding.DauntlessSteed))
				{
					num += DefaultPerks.Riding.DauntlessSteed.SecondaryBonus;
					flag5 = true;
				}
				else if (!agent.HasMount && characterObject.GetPerkValue(DefaultPerks.Athletics.IgnorePain))
				{
					num += DefaultPerks.Athletics.IgnorePain.SecondaryBonus;
					flag5 = true;
				}
				if (characterObject.GetPerkValue(DefaultPerks.Engineering.Metallurgy))
				{
					num += DefaultPerks.Engineering.Metallurgy.SecondaryBonus;
					flag5 = true;
				}
			}
			if (!agent.HasMount && agentCharacter.GetPerkValue(DefaultPerks.Athletics.IgnorePain))
			{
				num2 += DefaultPerks.Athletics.IgnorePain.PrimaryBonus;
				flag5 = true;
			}
			if (flag5)
			{
				float num3 = 1f + num2;
				agentDrivenProperties.ArmorHead = MathF.Max(0f, (agentDrivenProperties.ArmorHead + num) * num3);
				agentDrivenProperties.ArmorTorso = MathF.Max(0f, (agentDrivenProperties.ArmorTorso + num) * num3);
				agentDrivenProperties.ArmorArms = MathF.Max(0f, (agentDrivenProperties.ArmorArms + num) * num3);
				agentDrivenProperties.ArmorLegs = MathF.Max(0f, (agentDrivenProperties.ArmorLegs + num) * num3);
			}
			if (Mission.Current != null && Mission.Current.HasValidTerrainType)
			{
				TerrainType terrainType = Mission.Current.TerrainType;
				if (terrainType == TerrainType.Snow || terrainType == TerrainType.Forest)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Tactics.ExtendedSkirmish, characterObject, ref explainedNumber2);
				}
				else if (terrainType == TerrainType.Plain || terrainType == TerrainType.Steppe || terrainType == TerrainType.Desert)
				{
					PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Tactics.DecisiveBattle, characterObject, ref explainedNumber2);
				}
			}
			if (agentCharacter.Tier >= 3 && agentCharacter.IsInfantry)
			{
				PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Athletics.FormFittingArmor, characterObject, ref explainedNumber2);
			}
			if (agent.Formation != null && agent.Formation.CountOfUnits <= 15)
			{
				PerkHelper.AddPerkBonusFromCaptain(DefaultPerks.Tactics.SmallUnitTactics, characterObject, ref explainedNumber2);
			}
			if (activeBanner != null)
			{
				BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedTroopMovementSpeed, activeBanner, ref explainedNumber2);
			}
			agentDrivenProperties.MaxSpeedMultiplier = explainedNumber2.ResultNumber;
			agentDrivenProperties.CombatMaxSpeedMultiplier = explainedNumber.ResultNumber;
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0003916C File Offset: 0x0003736C
		private void SetWeaponSkillEffectsOnAgent(Agent agent, CharacterObject agentCharacter, AgentDrivenProperties agentDrivenProperties, WeaponComponentData equippedWeaponComponent)
		{
			if (equippedWeaponComponent != null)
			{
				int effectiveSkill = this.GetEffectiveSkill(agent, equippedWeaponComponent.RelevantSkill);
				ExplainedNumber explainedNumber = new ExplainedNumber(agentDrivenProperties.SwingSpeedMultiplier, false, null);
				ExplainedNumber explainedNumber2 = new ExplainedNumber(agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier, false, null);
				ExplainedNumber explainedNumber3 = new ExplainedNumber(agentDrivenProperties.ReloadSpeed, false, null);
				if (equippedWeaponComponent.RelevantSkill == DefaultSkills.OneHanded)
				{
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.OneHandedSpeed, ref explainedNumber, effectiveSkill);
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.OneHandedSpeed, ref explainedNumber2, effectiveSkill);
				}
				else if (equippedWeaponComponent.RelevantSkill == DefaultSkills.TwoHanded)
				{
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.TwoHandedSpeed, ref explainedNumber, effectiveSkill);
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.TwoHandedSpeed, ref explainedNumber2, effectiveSkill);
				}
				else if (equippedWeaponComponent.RelevantSkill == DefaultSkills.Polearm)
				{
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.PolearmSpeed, ref explainedNumber, effectiveSkill);
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.PolearmSpeed, ref explainedNumber2, effectiveSkill);
				}
				else if (equippedWeaponComponent.RelevantSkill == DefaultSkills.Crossbow)
				{
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.CrossbowReloadSpeed, ref explainedNumber3, effectiveSkill);
				}
				else if (equippedWeaponComponent.RelevantSkill == DefaultSkills.Throwing)
				{
					SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.ThrowingSpeed, ref explainedNumber2, effectiveSkill);
				}
				agentDrivenProperties.SwingSpeedMultiplier = explainedNumber.ResultNumber;
				agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier = explainedNumber2.ResultNumber;
				agentDrivenProperties.ReloadSpeed = explainedNumber3.ResultNumber;
			}
			int effectiveSkill2 = this.GetEffectiveSkill(agent, DefaultSkills.Roguery);
			ExplainedNumber explainedNumber4 = new ExplainedNumber(1f, false, null);
			SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.CrouchedSpeed, ref explainedNumber4, effectiveSkill2);
			agentDrivenProperties.CrouchedSpeedMultiplier = explainedNumber4.ResultNumber;
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x000392D4 File Offset: 0x000374D4
		private void SetMountedPenaltiesOnAgent(Agent agent, AgentDrivenProperties agentDrivenProperties, WeaponComponentData equippedWeaponComponent)
		{
			int effectiveSkill = this.GetEffectiveSkill(agent, DefaultSkills.Riding);
			float skillEffectValue = DefaultSkillEffects.MountedWeaponSpeedPenalty.GetSkillEffectValue(effectiveSkill);
			if (skillEffectValue < 0f)
			{
				ExplainedNumber explainedNumber = new ExplainedNumber(agentDrivenProperties.WeaponBestAccuracyWaitTime, false, null);
				ExplainedNumber explainedNumber2 = new ExplainedNumber(agentDrivenProperties.SwingSpeedMultiplier, false, null);
				ExplainedNumber explainedNumber3 = new ExplainedNumber(agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier, false, null);
				ExplainedNumber explainedNumber4 = new ExplainedNumber(agentDrivenProperties.ReloadSpeed, false, null);
				SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.MountedWeaponSpeedPenalty, ref explainedNumber2, effectiveSkill);
				SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.MountedWeaponSpeedPenalty, ref explainedNumber3, effectiveSkill);
				SkillHelper.AddSkillBonusForSkillLevel(DefaultSkillEffects.MountedWeaponSpeedPenalty, ref explainedNumber4, effectiveSkill);
				explainedNumber.AddFactor(-1f * skillEffectValue, null);
				agentDrivenProperties.SwingSpeedMultiplier = Math.Max(0f, explainedNumber2.ResultNumber);
				agentDrivenProperties.ThrustOrRangedReadySpeedMultiplier = Math.Max(0f, explainedNumber3.ResultNumber);
				agentDrivenProperties.ReloadSpeed = Math.Max(0f, explainedNumber4.ResultNumber);
				agentDrivenProperties.WeaponBestAccuracyWaitTime = Math.Max(0f, explainedNumber.ResultNumber);
			}
			float num = 5f - (float)effectiveSkill * 0.05f;
			if (num > 0f)
			{
				ExplainedNumber explainedNumber5 = new ExplainedNumber(agentDrivenProperties.WeaponInaccuracy, false, null);
				explainedNumber5.AddFactor(num, null);
				agentDrivenProperties.WeaponInaccuracy = Math.Max(0f, explainedNumber5.ResultNumber);
			}
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00039419 File Offset: 0x00037619
		public static float CalculateMaximumSpeedMultiplier(int athletics, float baseWeight, float totalEncumbrance)
		{
			return MathF.Min((200f + (float)athletics) / 300f * (baseWeight * 2f / (baseWeight * 2f + totalEncumbrance)), 1f);
		}
	}
}
