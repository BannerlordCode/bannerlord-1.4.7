using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000157 RID: 343
	public class DefaultSiegeEventModel : SiegeEventModel
	{
		// Token: 0x06001A7E RID: 6782 RVA: 0x000865E0 File Offset: 0x000847E0
		public override string GetSiegeEngineMapPrefabName(SiegeEngineType type, int wallLevel, BattleSideEnum side)
		{
			string text = null;
			if (type == DefaultSiegeEngineTypes.Onager)
			{
				text = "mangonel_a_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.Catapult)
			{
				text = "mangonel_b_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.FireOnager)
			{
				text = "mangonel_a_fire_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.FireCatapult)
			{
				text = "mangonel_b_fire_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.Ballista)
			{
				text = ((side == BattleSideEnum.Attacker) ? "ballista_a_mapicon" : "ballista_b_mapicon");
			}
			else if (type == DefaultSiegeEngineTypes.FireBallista)
			{
				text = ((side == BattleSideEnum.Attacker) ? "ballista_a_fire_mapicon" : "ballista_b_fire_mapicon");
			}
			else if (type == DefaultSiegeEngineTypes.Trebuchet)
			{
				text = "trebuchet_a_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.Bricole)
			{
				text = "trebuchet_b_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.Ram)
			{
				text = "batteringram_a_mapicon";
			}
			else if (type == DefaultSiegeEngineTypes.SiegeTower)
			{
				switch (wallLevel)
				{
				case 1:
					text = "siegetower_5m_mapicon";
					break;
				case 2:
					text = "siegetower_9m_mapicon";
					break;
				case 3:
					text = "siegetower_12m_mapicon";
					break;
				}
			}
			return text;
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x000866D8 File Offset: 0x000848D8
		public override string GetSiegeEngineMapProjectilePrefabName(SiegeEngineType type)
		{
			string text = null;
			if (type == DefaultSiegeEngineTypes.Onager || type == DefaultSiegeEngineTypes.Catapult || type == DefaultSiegeEngineTypes.Trebuchet || type == DefaultSiegeEngineTypes.Bricole)
			{
				text = "mangonel_mapicon_projectile";
			}
			else if (type == DefaultSiegeEngineTypes.FireOnager || type == DefaultSiegeEngineTypes.FireCatapult)
			{
				text = "mangonel_fire_mapicon_projectile";
			}
			else if (type == DefaultSiegeEngineTypes.Ballista)
			{
				text = "ballista_mapicon_projectile";
			}
			else if (type == DefaultSiegeEngineTypes.FireBallista)
			{
				text = "ballista_fire_mapicon_projectile";
			}
			return text;
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x00086748 File Offset: 0x00084948
		public override string GetSiegeEngineMapReloadAnimationName(SiegeEngineType type, BattleSideEnum side)
		{
			string text = null;
			if (type == DefaultSiegeEngineTypes.Onager)
			{
				text = "mangonel_a_mapicon_reload";
			}
			else if (type == DefaultSiegeEngineTypes.Catapult)
			{
				text = "mangonel_b_mapicon_reload";
			}
			else if (type == DefaultSiegeEngineTypes.FireOnager)
			{
				text = "mangonel_a_fire_mapicon_reload";
			}
			else if (type == DefaultSiegeEngineTypes.FireCatapult)
			{
				text = "mangonel_b_fire_mapicon_reload";
			}
			else if (type == DefaultSiegeEngineTypes.Ballista)
			{
				text = ((side == BattleSideEnum.Attacker) ? "ballista_a_mapicon_reload" : "ballista_b_mapicon_reload");
			}
			else if (type == DefaultSiegeEngineTypes.FireBallista)
			{
				text = ((side == BattleSideEnum.Attacker) ? "ballista_a_fire_mapicon_reload" : "ballista_b_fire_mapicon_reload");
			}
			else if (type == DefaultSiegeEngineTypes.Trebuchet)
			{
				text = "trebuchet_a_mapicon_reload";
			}
			else if (type == DefaultSiegeEngineTypes.Bricole)
			{
				text = "trebuchet_b_mapicon_reload";
			}
			return text;
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x000867F0 File Offset: 0x000849F0
		public override string GetSiegeEngineMapFireAnimationName(SiegeEngineType type, BattleSideEnum side)
		{
			string text = null;
			if (type == DefaultSiegeEngineTypes.Onager)
			{
				text = "mangonel_a_mapicon_fire";
			}
			else if (type == DefaultSiegeEngineTypes.Catapult)
			{
				text = "mangonel_b_mapicon_fire";
			}
			else if (type == DefaultSiegeEngineTypes.FireOnager)
			{
				text = "mangonel_a_fire_mapicon_fire";
			}
			else if (type == DefaultSiegeEngineTypes.FireCatapult)
			{
				text = "mangonel_b_fire_mapicon_fire";
			}
			else if (type == DefaultSiegeEngineTypes.Ballista)
			{
				text = ((side == BattleSideEnum.Attacker) ? "ballista_a_mapicon_fire" : "ballista_b_mapicon_fire");
			}
			else if (type == DefaultSiegeEngineTypes.FireBallista)
			{
				text = ((side == BattleSideEnum.Attacker) ? "ballista_a_fire_mapicon_fire" : "ballista_b_fire_mapicon_fire");
			}
			else if (type == DefaultSiegeEngineTypes.Trebuchet)
			{
				text = "trebuchet_a_mapicon_fire";
			}
			else if (type == DefaultSiegeEngineTypes.Bricole)
			{
				text = "trebuchet_b_mapicon_fire";
			}
			return text;
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x00086898 File Offset: 0x00084A98
		public override sbyte GetSiegeEngineMapProjectileBoneIndex(SiegeEngineType type, BattleSideEnum side)
		{
			if (type == DefaultSiegeEngineTypes.Onager || type == DefaultSiegeEngineTypes.FireOnager)
			{
				return 2;
			}
			if (type == DefaultSiegeEngineTypes.Catapult || type == DefaultSiegeEngineTypes.FireCatapult)
			{
				return 2;
			}
			if (type == DefaultSiegeEngineTypes.Ballista || type == DefaultSiegeEngineTypes.FireBallista)
			{
				return 7;
			}
			if (type == DefaultSiegeEngineTypes.Trebuchet)
			{
				return 4;
			}
			if (type == DefaultSiegeEngineTypes.Bricole)
			{
				return 20;
			}
			return -1;
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x000868F4 File Offset: 0x00084AF4
		public override MobileParty GetEffectiveSiegePartyForSide(SiegeEvent siegeEvent, BattleSideEnum battleSide)
		{
			MobileParty mobileParty = null;
			if (battleSide == BattleSideEnum.Attacker)
			{
				mobileParty = siegeEvent.BesiegerCamp.LeaderParty;
			}
			else
			{
				int num = 0;
				int num2 = -1;
				for (PartyBase partyBase = siegeEvent.BesiegedSettlement.GetNextInvolvedPartyForEventType(ref num2, MapEvent.BattleTypes.Siege); partyBase != null; partyBase = siegeEvent.BesiegedSettlement.GetNextInvolvedPartyForEventType(ref num2, MapEvent.BattleTypes.Siege))
				{
					if (partyBase.LeaderHero != null)
					{
						Hero effectiveEngineer = partyBase.MobileParty.EffectiveEngineer;
						int num3 = ((effectiveEngineer != null) ? effectiveEngineer.GetSkillValue(DefaultSkills.Engineering) : 0);
						if (num3 > num)
						{
							num = num3;
							mobileParty = partyBase.MobileParty;
						}
					}
				}
			}
			return mobileParty;
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x00086974 File Offset: 0x00084B74
		public override float GetCasualtyChance(MobileParty siegeParty, SiegeEvent siegeEvent, BattleSideEnum side)
		{
			float num = 1f;
			if (siegeParty != null && siegeParty.HasPerk(DefaultPerks.Engineering.CampBuilding, true))
			{
				num += DefaultPerks.Engineering.CampBuilding.SecondaryBonus;
			}
			if (siegeParty != null && siegeParty.HasPerk(DefaultPerks.Medicine.SiegeMedic, true))
			{
				num -= DefaultPerks.Medicine.SiegeMedic.SecondaryBonus;
			}
			if (side == BattleSideEnum.Defender)
			{
				Town town = siegeEvent.BesiegedSettlement.Town;
				if (((town != null) ? town.Governor : null) != null && siegeEvent.BesiegedSettlement.Town.Governor.GetPerkValue(DefaultPerks.Medicine.BattleHardened))
				{
					num += DefaultPerks.Medicine.BattleHardened.SecondaryBonus;
				}
			}
			return num;
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x00086A09 File Offset: 0x00084C09
		public override int GetSiegeEngineDestructionCasualties(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType destroyedSiegeEngine)
		{
			return 2;
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x00086A0C File Offset: 0x00084C0C
		public override int GetColleteralDamageCasualties(SiegeEngineType siegeEngineType, MobileParty party)
		{
			int num = 1;
			if (party != null && !party.IsCurrentlyAtSea && party.HasPerk(DefaultPerks.Crossbow.Terror, false) && MBRandom.RandomFloat < DefaultPerks.Crossbow.Terror.PrimaryBonus)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x00086A4C File Offset: 0x00084C4C
		public override float GetSiegeEngineHitChance(SiegeEngineType siegeEngineType, BattleSideEnum battleSide, SiegeBombardTargets target, Town town)
		{
			float num;
			if (target - SiegeBombardTargets.Wall > 1)
			{
				if (target != SiegeBombardTargets.People)
				{
					throw new ArgumentOutOfRangeException("target", target, null);
				}
				num = siegeEngineType.AntiPersonnelHitChance;
			}
			else
			{
				num = siegeEngineType.HitChance;
			}
			ExplainedNumber explainedNumber = new ExplainedNumber(num, false, null);
			if (battleSide == BattleSideEnum.Attacker && target == SiegeBombardTargets.RangedEngines)
			{
				float num2 = 0f;
				switch (town.GetWallLevel())
				{
				case 1:
					num2 = 0.05f;
					break;
				case 2:
					num2 = 0.1f;
					break;
				case 3:
					num2 = 0.15f;
					break;
				}
				explainedNumber.Add(-num2, new TextObject("{=b9NaTqyr}Extra Defender Defense", null), null);
			}
			if (battleSide == BattleSideEnum.Defender)
			{
				if (target == SiegeBombardTargets.RangedEngines && town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Engineering.DreadfulSieger))
				{
					explainedNumber.AddFactor(DefaultPerks.Engineering.DreadfulSieger.PrimaryBonus, DefaultPerks.Engineering.DreadfulSieger.Name);
				}
				if (siegeEngineType == DefaultSiegeEngineTypes.Ballista)
				{
					PerkHelper.AddPerkBonusForTown(DefaultPerks.Crossbow.Pavise, town, ref explainedNumber);
				}
			}
			SiegeEvent siegeEvent = town.Settlement.SiegeEvent;
			MobileParty effectiveSiegePartyForSide = this.GetEffectiveSiegePartyForSide(siegeEvent, battleSide);
			MobileParty effectiveSiegePartyForSide2 = this.GetEffectiveSiegePartyForSide(siegeEvent, battleSide.GetOppositeSide());
			if (effectiveSiegePartyForSide != null)
			{
				if ((siegeEngineType == DefaultSiegeEngineTypes.Trebuchet || siegeEngineType == DefaultSiegeEngineTypes.Onager || siegeEngineType == DefaultSiegeEngineTypes.FireOnager) && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.Foreman, false))
				{
					explainedNumber.AddFactor(DefaultPerks.Engineering.Foreman.PrimaryBonus, DefaultPerks.Engineering.Foreman.Name);
				}
				if ((siegeEngineType == DefaultSiegeEngineTypes.Ballista || siegeEngineType == DefaultSiegeEngineTypes.FireBallista) && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.Salvager, false))
				{
					explainedNumber.AddFactor(DefaultPerks.Engineering.Salvager.PrimaryBonus, DefaultPerks.Engineering.Salvager.Name);
				}
			}
			if (battleSide == BattleSideEnum.Defender && effectiveSiegePartyForSide2 != null && target == SiegeBombardTargets.RangedEngines && effectiveSiegePartyForSide2.HasPerk(DefaultPerks.Engineering.DungeonArchitect, false))
			{
				explainedNumber.AddFactor(DefaultPerks.Engineering.DungeonArchitect.PrimaryBonus, DefaultPerks.Engineering.DungeonArchitect.Name);
			}
			if (explainedNumber.ResultNumber < 0f)
			{
				explainedNumber = new ExplainedNumber(0f, false, null);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x00086C48 File Offset: 0x00084E48
		public override float GetSiegeStrategyScore(SiegeEvent siege, BattleSideEnum side, SiegeStrategy strategy)
		{
			if (strategy == DefaultSiegeStrategies.PreserveStrength)
			{
				return -9000f;
			}
			if (strategy != DefaultSiegeStrategies.Custom)
			{
				return MBRandom.RandomFloat;
			}
			if (siege == PlayerSiege.PlayerSiegeEvent && side == PlayerSiege.PlayerSide && siege.BesiegerCamp != null && siege.BesiegerCamp.LeaderParty == MobileParty.MainParty)
			{
				return 9000f;
			}
			return -100f;
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x00086CA8 File Offset: 0x00084EA8
		public override float GetConstructionProgressPerHour(SiegeEngineType type, SiegeEvent siegeEvent, ISiegeEventSide side)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
			float availableManDayPower = this.GetAvailableManDayPower(side);
			float num = (float)type.ManDayCost;
			explainedNumber.Add(1f / (num / availableManDayPower * (float)CampaignTime.HoursInDay), this._baseConstructionSpeedText, null);
			MobileParty effectiveSiegePartyForSide = this.GetEffectiveSiegePartyForSide(siegeEvent, side.BattleSide);
			if (effectiveSiegePartyForSide != null)
			{
				int? num2;
				if (effectiveSiegePartyForSide == null)
				{
					num2 = null;
				}
				else
				{
					Hero effectiveEngineer = effectiveSiegePartyForSide.EffectiveEngineer;
					num2 = ((effectiveEngineer != null) ? new int?(effectiveEngineer.GetSkillValue(DefaultSkills.Engineering)) : null);
				}
				if ((num2 ?? 0) > 0)
				{
					SkillHelper.AddSkillBonusForParty(DefaultSkillEffects.SiegeEngineProductionBonus, effectiveSiegePartyForSide, ref explainedNumber);
				}
			}
			if (side.BattleSide == BattleSideEnum.Defender)
			{
				siegeEvent.BesiegedSettlement.Town.AddEffectOfBuildings(BuildingEffectEnum.SiegeEngineSpeed, ref explainedNumber);
				Hero governor = siegeEvent.BesiegedSettlement.Town.Governor;
				if (((governor != null) ? governor.CurrentSettlement : null) != null && governor.CurrentSettlement == siegeEvent.BesiegedSettlement)
				{
					SkillHelper.AddSkillBonusForTown(DefaultSkillEffects.SiegeEngineProductionBonus, siegeEvent.BesiegedSettlement.Town, ref explainedNumber);
				}
			}
			if (((siegeEvent != null) ? siegeEvent.BesiegerCamp.LeaderParty : null) != null && siegeEvent.BesiegerCamp.LeaderParty.HasPerk(DefaultPerks.Steward.Sweatshops, true))
			{
				explainedNumber.AddFactor(DefaultPerks.Steward.Sweatshops.SecondaryBonus, null);
			}
			if (effectiveSiegePartyForSide != null)
			{
				SiegeEvent.SiegeEngineConstructionProgress siegePreparations = side.SiegeEngines.SiegePreparations;
				if (siegePreparations != null && !siegePreparations.IsConstructed && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.ImprovedTools, false))
				{
					explainedNumber.AddFactor(DefaultPerks.Engineering.ImprovedTools.PrimaryBonus, DefaultPerks.Engineering.ImprovedTools.Name);
				}
				else
				{
					PerkObject perkObject = (type.IsRanged ? DefaultPerks.Engineering.TorsionEngines : DefaultPerks.Engineering.Scaffolds);
					if (effectiveSiegePartyForSide.HasPerk(perkObject, false))
					{
						explainedNumber.AddFactor(perkObject.PrimaryBonus, perkObject.Name);
					}
				}
			}
			if (side.BattleSide == BattleSideEnum.Defender)
			{
				Settlement besiegedSettlement = siegeEvent.BesiegedSettlement;
				PerkObject salvager = DefaultPerks.Engineering.Salvager;
				if (PerkHelper.GetPerkValueForTown(salvager, besiegedSettlement.Town))
				{
					explainedNumber.AddFactor(salvager.SecondaryBonus * besiegedSettlement.Militia, salvager.Name);
				}
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x00086EC8 File Offset: 0x000850C8
		public override float GetAvailableManDayPower(ISiegeEventSide side)
		{
			int num = -1;
			PartyBase partyBase = side.GetNextInvolvedPartyForEventType(ref num, MapEvent.BattleTypes.Siege);
			int num2 = 0;
			while (partyBase != null)
			{
				num2 += partyBase.NumberOfHealthyMembers;
				partyBase = side.GetNextInvolvedPartyForEventType(ref num, MapEvent.BattleTypes.Siege);
			}
			return MathF.Sqrt((float)num2);
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x00086F04 File Offset: 0x00085104
		public override IEnumerable<SiegeEngineType> GetPrebuiltSiegeEnginesOfSettlement(Settlement settlement)
		{
			List<SiegeEngineType> list = new List<SiegeEngineType>();
			if (settlement.IsFortification)
			{
				Town town = settlement.Town;
				ExplainedNumber explainedNumber = new ExplainedNumber(0f, false, null);
				town.AddEffectOfBuildings(BuildingEffectEnum.BallistaOnSiegeStart, ref explainedNumber);
				int num = 0;
				while ((float)num < explainedNumber.ResultNumber)
				{
					list.Add(DefaultSiegeEngineTypes.Ballista);
					num++;
				}
				ExplainedNumber explainedNumber2 = new ExplainedNumber(0f, false, null);
				town.AddEffectOfBuildings(BuildingEffectEnum.CatapultOnSiegeStart, ref explainedNumber2);
				int num2 = 0;
				while ((float)num2 < explainedNumber2.ResultNumber)
				{
					list.Add(DefaultSiegeEngineTypes.Catapult);
					num2++;
				}
				if (town.Governor != null && town.Governor.GetPerkValue(DefaultPerks.Engineering.SiegeWorks))
				{
					list.Add(DefaultSiegeEngineTypes.Catapult);
				}
			}
			return list;
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x00086FC4 File Offset: 0x000851C4
		public override IEnumerable<SiegeEngineType> GetPrebuiltSiegeEnginesOfSiegeCamp(BesiegerCamp besiegerCamp)
		{
			List<SiegeEngineType> list = new List<SiegeEngineType>();
			if (besiegerCamp.LeaderParty.HasPerk(DefaultPerks.Engineering.Battlements, false))
			{
				list.Add(DefaultSiegeEngineTypes.Ballista);
			}
			return list;
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x00086FF8 File Offset: 0x000851F8
		public override float GetSiegeEngineHitPoints(SiegeEvent siegeEvent, SiegeEngineType siegeEngine, BattleSideEnum battleSide)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber((float)siegeEngine.BaseHitPoints, false, null);
			Settlement besiegedSettlement = siegeEvent.BesiegedSettlement;
			MobileParty effectiveSiegePartyForSide = this.GetEffectiveSiegePartyForSide(siegeEvent, battleSide);
			if (battleSide == BattleSideEnum.Defender && besiegedSettlement.Town.Governor != null && besiegedSettlement.Town.Governor.GetPerkValue(DefaultPerks.Engineering.SiegeEngineer))
			{
				explainedNumber.AddFactor(DefaultPerks.Engineering.SiegeEngineer.PrimaryBonus, DefaultPerks.Engineering.SiegeEngineer.Name);
			}
			if (siegeEngine.IsRanged)
			{
				if (effectiveSiegePartyForSide != null && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.SiegeWorks, false))
				{
					explainedNumber.AddFactor(DefaultPerks.Engineering.SiegeWorks.PrimaryBonus, DefaultPerks.Engineering.SiegeWorks.Name);
				}
			}
			else if (battleSide == BattleSideEnum.Attacker && effectiveSiegePartyForSide != null && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.Carpenters, false))
			{
				explainedNumber.AddFactor(DefaultPerks.Engineering.Carpenters.PrimaryBonus, DefaultPerks.Engineering.Carpenters.Name);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x000870D4 File Offset: 0x000852D4
		public override float GetSiegeEngineDamage(SiegeEvent siegeEvent, BattleSideEnum battleSide, SiegeEngineType siegeEngine, SiegeBombardTargets target)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber((float)siegeEngine.Damage, false, null);
			MobileParty effectiveSiegePartyForSide = this.GetEffectiveSiegePartyForSide(siegeEvent, battleSide);
			if (effectiveSiegePartyForSide != null)
			{
				if (battleSide == BattleSideEnum.Attacker)
				{
					if (target == SiegeBombardTargets.Wall && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.WallBreaker, false))
					{
						explainedNumber.AddFactor(DefaultPerks.Engineering.WallBreaker.PrimaryBonus, DefaultPerks.Engineering.WallBreaker.Name);
					}
					if (target == SiegeBombardTargets.RangedEngines && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Tactics.MakeThemPay, false))
					{
						explainedNumber.AddFactor(DefaultPerks.Tactics.MakeThemPay.PrimaryBonus, DefaultPerks.Tactics.MakeThemPay.Name);
					}
				}
				if ((target == SiegeBombardTargets.RangedEngines || target == SiegeBombardTargets.Wall) && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.Masterwork, false))
				{
					int num = effectiveSiegePartyForSide.LeaderHero.GetSkillValue(DefaultSkills.Engineering) - Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus;
					if (num > 0)
					{
						float num2 = (float)num * DefaultPerks.Engineering.Masterwork.PrimaryBonus;
						explainedNumber.AddFactor(num2, DefaultPerks.Engineering.Masterwork.Name);
					}
				}
			}
			if (battleSide == BattleSideEnum.Defender && target == SiegeBombardTargets.RangedEngines)
			{
				Hero governor = siegeEvent.BesiegedSettlement.Town.Governor;
				if (governor != null && governor.GetPerkValue(DefaultPerks.Tactics.MakeThemPay))
				{
					explainedNumber.AddFactor(DefaultPerks.Tactics.MakeThemPay.SecondaryBonus, DefaultPerks.Tactics.MakeThemPay.Name);
				}
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x00087210 File Offset: 0x00085410
		public override int GetRangedSiegeEngineReloadTime(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType siegeEngine)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(siegeEngine.CampaignRateOfFirePerDay, false, null);
			MobileParty effectiveSiegePartyForSide = this.GetEffectiveSiegePartyForSide(siegeEvent, side);
			if (effectiveSiegePartyForSide != null)
			{
				if ((siegeEngine == DefaultSiegeEngineTypes.Ballista || siegeEngine == DefaultSiegeEngineTypes.FireBallista) && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.Clockwork, false))
				{
					explainedNumber.AddFactor(DefaultPerks.Engineering.Clockwork.PrimaryBonus, DefaultPerks.Engineering.Clockwork.Name);
				}
				else if ((siegeEngine == DefaultSiegeEngineTypes.Onager || siegeEngine == DefaultSiegeEngineTypes.Trebuchet || siegeEngine == DefaultSiegeEngineTypes.FireOnager) && effectiveSiegePartyForSide.HasPerk(DefaultPerks.Engineering.ArchitecturalCommisions, false))
				{
					explainedNumber.AddFactor(DefaultPerks.Engineering.ArchitecturalCommisions.PrimaryBonus, DefaultPerks.Engineering.ArchitecturalCommisions.Name);
				}
			}
			return MathF.Round((float)(CampaignTime.MinutesInHour * CampaignTime.HoursInDay) / explainedNumber.ResultNumber);
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x000872CD File Offset: 0x000854CD
		public override IEnumerable<SiegeEngineType> GetAvailableAttackerRangedSiegeEngines(PartyBase party)
		{
			bool hasFirePerks = party.MobileParty.HasPerk(DefaultPerks.Engineering.Stonecutters, true) || party.MobileParty.HasPerk(DefaultPerks.Engineering.SiegeEngineer, true);
			yield return DefaultSiegeEngineTypes.Ballista;
			if (hasFirePerks)
			{
				yield return DefaultSiegeEngineTypes.FireBallista;
			}
			yield return DefaultSiegeEngineTypes.Onager;
			if (hasFirePerks)
			{
				yield return DefaultSiegeEngineTypes.FireOnager;
			}
			yield return DefaultSiegeEngineTypes.Trebuchet;
			yield break;
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x000872DD File Offset: 0x000854DD
		public override IEnumerable<SiegeEngineType> GetAvailableDefenderSiegeEngines(PartyBase party)
		{
			bool hasFirePerks = party.MobileParty.HasPerk(DefaultPerks.Engineering.Stonecutters, true) || party.MobileParty.HasPerk(DefaultPerks.Engineering.SiegeEngineer, true);
			yield return DefaultSiegeEngineTypes.Ballista;
			if (hasFirePerks)
			{
				yield return DefaultSiegeEngineTypes.FireBallista;
			}
			yield return DefaultSiegeEngineTypes.Catapult;
			if (hasFirePerks)
			{
				yield return DefaultSiegeEngineTypes.FireCatapult;
			}
			yield break;
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x000872ED File Offset: 0x000854ED
		public override IEnumerable<SiegeEngineType> GetAvailableAttackerRamSiegeEngines(PartyBase party)
		{
			yield return DefaultSiegeEngineTypes.Ram;
			yield break;
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x000872F6 File Offset: 0x000854F6
		public override IEnumerable<SiegeEngineType> GetAvailableAttackerTowerSiegeEngines(PartyBase party)
		{
			yield return DefaultSiegeEngineTypes.SiegeTower;
			yield break;
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x00087300 File Offset: 0x00085500
		public override FlattenedTroopRoster GetPriorityTroopsForSallyOutAmbush()
		{
			FlattenedTroopRoster flattenedTroopRoster = new FlattenedTroopRoster(4);
			foreach (TroopRosterElement troopRosterElement in MobileParty.MainParty.MemberRoster.GetTroopRoster())
			{
				if (this.IsPriorityTroopForSallyOutAmbush(troopRosterElement))
				{
					flattenedTroopRoster.Add(troopRosterElement);
				}
			}
			SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
			if (playerSiegeEvent.BesiegedSettlement.OwnerClan == Clan.PlayerClan && playerSiegeEvent.BesiegedSettlement.Town.GarrisonParty != null && playerSiegeEvent.BesiegedSettlement.Town.GarrisonParty.MemberRoster.Count > 0)
			{
				foreach (TroopRosterElement troopRosterElement2 in playerSiegeEvent.BesiegedSettlement.Town.GarrisonParty.MemberRoster.GetTroopRoster())
				{
					if (this.IsPriorityTroopForSallyOutAmbush(troopRosterElement2))
					{
						flattenedTroopRoster.Add(troopRosterElement2);
					}
				}
			}
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
			{
				foreach (PartyBase partyBase in playerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Defender).GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege))
				{
					if (partyBase != PartyBase.MainParty)
					{
						foreach (TroopRosterElement troopRosterElement3 in partyBase.MemberRoster.GetTroopRoster())
						{
							if (this.IsPriorityTroopForSallyOutAmbush(troopRosterElement3))
							{
								flattenedTroopRoster.Add(troopRosterElement3);
							}
						}
					}
				}
			}
			return flattenedTroopRoster;
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x000874E0 File Offset: 0x000856E0
		private bool IsPriorityTroopForSallyOutAmbush(TroopRosterElement troop)
		{
			CharacterObject character = troop.Character;
			return character.IsHero || character.HasMount();
		}

		// Token: 0x040008EC RID: 2284
		private readonly TextObject _baseConstructionSpeedText = new TextObject("{=MhGbcXJ4}Base construction speed", null);

		// Token: 0x040008ED RID: 2285
		private readonly TextObject _constructionSpeedProjectBonusText = new TextObject("{=xoTWC8Sm}Project Bonus", null);

		// Token: 0x040008EE RID: 2286
		private readonly TextObject _weatherConstructionPenalty = new TextObject("{=J6RjCKbk}Weather", null);
	}
}
