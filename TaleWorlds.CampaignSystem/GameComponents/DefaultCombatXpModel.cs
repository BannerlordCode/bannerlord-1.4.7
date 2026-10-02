using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000108 RID: 264
	public class DefaultCombatXpModel : CombatXpModel
	{
		// Token: 0x0600174E RID: 5966 RVA: 0x0006D9E8 File Offset: 0x0006BBE8
		public override SkillObject GetSkillForWeapon(WeaponComponentData weapon, bool isSiegeEngineHit)
		{
			SkillObject skillObject = DefaultSkills.Athletics;
			if (isSiegeEngineHit)
			{
				skillObject = DefaultSkills.Engineering;
			}
			else if (weapon != null)
			{
				skillObject = weapon.RelevantSkill;
			}
			return skillObject;
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x0006DA14 File Offset: 0x0006BC14
		public override ExplainedNumber GetXpFromHit(CharacterObject attackerTroop, CharacterObject captain, CharacterObject attackedTroop, PartyBase attackerParty, int damage, bool isFatal, CombatXpModel.MissionTypeEnum missionType)
		{
			int num = attackedTroop.MaxHitPoints();
			float num2 = 0f;
			BattleSideEnum battleSideEnum = BattleSideEnum.Attacker;
			MapEvent.PowerCalculationContext powerCalculationContext = MapEvent.PowerCalculationContext.PlainBattle;
			if (((attackerParty != null) ? attackerParty.MapEvent : null) != null)
			{
				num2 = attackerParty.MapEventSide.LeaderSimulationModifier;
				battleSideEnum = attackerParty.Side;
				powerCalculationContext = attackerParty.MapEvent.SimulationContext;
			}
			float troopPower = Campaign.Current.Models.MilitaryPowerModel.GetTroopPower(attackedTroop, battleSideEnum.GetOppositeSide(), powerCalculationContext, num2);
			float num3 = Campaign.Current.Models.MilitaryPowerModel.GetTroopPower(attackerTroop, battleSideEnum, powerCalculationContext, num2) + 0.5f;
			float num4 = troopPower + 0.5f;
			int num5 = MathF.Min(damage, num) + (isFatal ? num : 0);
			float num6 = 0.4f * num3 * num4 * (float)num5;
			num6 *= DefaultCombatXpModel.GetXpfMultiplierForMissionType(missionType);
			ExplainedNumber explainedNumber = new ExplainedNumber(num6, false, null);
			if (attackerParty != null)
			{
				DefaultCombatXpModel.GetBattleXpBonusFromPerks(attackerParty, ref explainedNumber, attackerTroop);
			}
			bool flag = attackerParty == null || !attackerParty.IsMobile || attackerParty.MobileParty.IsCurrentlyAtSea;
			if (captain != null && captain.IsHero && !flag && captain.GetPerkValue(DefaultPerks.Leadership.InspiringLeader))
			{
				explainedNumber.AddFactor(DefaultPerks.Leadership.InspiringLeader.SecondaryBonus, DefaultPerks.Leadership.InspiringLeader.Name);
			}
			return explainedNumber;
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x0006DB4C File Offset: 0x0006BD4C
		private static float GetXpfMultiplierForMissionType(CombatXpModel.MissionTypeEnum missionType)
		{
			float num;
			if (missionType == CombatXpModel.MissionTypeEnum.NoXp)
			{
				num = 0f;
			}
			else if (missionType == CombatXpModel.MissionTypeEnum.PracticeFight)
			{
				num = 0.0625f;
			}
			else if (missionType == CombatXpModel.MissionTypeEnum.Tournament)
			{
				num = 0.33f;
			}
			else if (missionType == CombatXpModel.MissionTypeEnum.SimulationBattle)
			{
				num = 0.9f;
			}
			else if (missionType == CombatXpModel.MissionTypeEnum.Battle)
			{
				num = 1f;
			}
			else
			{
				num = 1f;
			}
			return num;
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x0006DB9B File Offset: 0x0006BD9B
		public override float GetXpMultiplierFromShotDifficulty(float shotDifficulty)
		{
			if (shotDifficulty > 14.4f)
			{
				shotDifficulty = 14.4f;
			}
			return MBMath.Lerp(0f, 2f, (shotDifficulty - 1f) / 13.4f, 1E-05f);
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06001752 RID: 5970 RVA: 0x0006DBCD File Offset: 0x0006BDCD
		public override float CaptainRadius
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x0006DBD4 File Offset: 0x0006BDD4
		private static void GetBattleXpBonusFromPerks(PartyBase party, ref ExplainedNumber xpToGain, CharacterObject troop)
		{
			if (party.IsMobile && party.MobileParty.LeaderHero != null)
			{
				if (!troop.IsRanged)
				{
					if (!party.MobileParty.IsCurrentlyAtSea && party.MobileParty.HasPerk(DefaultPerks.OneHanded.Trainer, true))
					{
						xpToGain.AddFactor(DefaultPerks.OneHanded.Trainer.SecondaryBonus, DefaultPerks.OneHanded.Trainer.Name);
					}
					PerkHelper.AddPerkBonusForParty(DefaultPerks.TwoHanded.BaptisedInBlood, party.MobileParty, false, ref xpToGain, party.MobileParty.IsCurrentlyAtSea);
				}
				if (troop.HasThrowingWeapon() && party.MobileParty.HasPerk(DefaultPerks.Throwing.Resourceful, true))
				{
					xpToGain.AddFactor(DefaultPerks.Throwing.Resourceful.SecondaryBonus, DefaultPerks.Throwing.Resourceful.Name);
				}
				if (troop.IsInfantry)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.OneHanded.CorpsACorps, party.MobileParty, true, ref xpToGain, party.MobileParty.IsCurrentlyAtSea);
				}
				PerkHelper.AddPerkBonusForParty(DefaultPerks.OneHanded.LeadByExample, party.MobileParty, true, ref xpToGain, party.MobileParty.IsCurrentlyAtSea);
				if (troop.IsRanged)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Crossbow.MountedCrossbowman, party.MobileParty, false, ref xpToGain, party.MobileParty.IsCurrentlyAtSea);
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Bow.BullsEye, party.MobileParty, true, ref xpToGain, party.MobileParty.IsCurrentlyAtSea);
				}
				if (troop.Culture.IsBandit && party.MobileParty.HasPerk(DefaultPerks.Roguery.NoRestForTheWicked, false))
				{
					xpToGain.AddFactor(DefaultPerks.Roguery.NoRestForTheWicked.PrimaryBonus, DefaultPerks.Roguery.NoRestForTheWicked.Name);
				}
			}
			if (party.IsMobile && party.MobileParty.IsGarrison)
			{
				Settlement currentSettlement = party.MobileParty.CurrentSettlement;
				if (((currentSettlement != null) ? currentSettlement.Town.Governor : null) != null)
				{
					PerkHelper.AddPerkBonusForTown(DefaultPerks.TwoHanded.ProjectileDeflection, party.MobileParty.CurrentSettlement.Town, ref xpToGain);
					if (troop.IsMounted)
					{
						PerkHelper.AddPerkBonusForTown(DefaultPerks.Polearm.Guards, party.MobileParty.CurrentSettlement.Town, ref xpToGain);
					}
				}
			}
		}
	}
}
