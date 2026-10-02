using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003E2 RID: 994
	public class CompanionsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x06003D57 RID: 15703 RVA: 0x00109FA8 File Offset: 0x001081A8
		private float _desiredTotalCompanionCount
		{
			get
			{
				return (float)Town.AllTowns.Count * 0.6f;
			}
		}

		// Token: 0x06003D58 RID: 15704 RVA: 0x00109FBC File Offset: 0x001081BC
		public override void RegisterEvents()
		{
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.OnGameLoadFinished));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.HeroOccupationChangedEvent.AddNonSerializedListener(this, new Action<Hero, Occupation>(this.OnHeroOccupationChanged));
			CampaignEvents.HeroCreated.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroCreated));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.WeeklyTickEvent.AddNonSerializedListener(this, new Action(this.WeeklyTick));
		}

		// Token: 0x06003D59 RID: 15705 RVA: 0x0010A06C File Offset: 0x0010826C
		private void OnGameLoadFinished()
		{
			this.InitializeCompanionTemplateList();
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				if (hero.IsWanderer)
				{
					this.AddToAliveCompanions(hero, false);
				}
			}
			foreach (Hero hero2 in Hero.DeadOrDisabledHeroes)
			{
				if (hero2.IsAlive && hero2.IsWanderer)
				{
					this.AddToAliveCompanions(hero2, false);
				}
			}
		}

		// Token: 0x06003D5A RID: 15706 RVA: 0x0010A120 File Offset: 0x00108320
		private void DailyTick()
		{
			this.TryKillCompanion();
			this.SwapCompanions();
			this.TrySpawnNewCompanion();
		}

		// Token: 0x06003D5B RID: 15707 RVA: 0x0010A134 File Offset: 0x00108334
		private void WeeklyTick()
		{
			foreach (Hero hero in Hero.DeadOrDisabledHeroes.ToList<Hero>())
			{
				if (hero.IsWanderer && hero.DeathDay.ElapsedDaysUntilNow >= 40f)
				{
					Campaign.Current.CampaignObjectManager.UnregisterDeadHero(hero);
				}
			}
		}

		// Token: 0x06003D5C RID: 15708 RVA: 0x0010A1B4 File Offset: 0x001083B4
		private void RemoveFromAliveCompanions(Hero companion)
		{
			CharacterObject template = companion.Template;
			if (this._aliveCompanionTemplates.Contains(template))
			{
				this._aliveCompanionTemplates.Remove(template);
			}
		}

		// Token: 0x06003D5D RID: 15709 RVA: 0x0010A1E4 File Offset: 0x001083E4
		private void AddToAliveCompanions(Hero companion, bool isTemplateControlled = false)
		{
			CharacterObject template = companion.Template;
			bool flag = true;
			if (!isTemplateControlled)
			{
				flag = this.IsTemplateKnown(template);
			}
			if (flag && !this._aliveCompanionTemplates.Contains(template))
			{
				this._aliveCompanionTemplates.Add(template);
			}
		}

		// Token: 0x06003D5E RID: 15710 RVA: 0x0010A223 File Offset: 0x00108423
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification = true)
		{
			this.RemoveFromAliveCompanions(victim);
			if (victim.IsWanderer && !victim.HasMet)
			{
				Campaign.Current.CampaignObjectManager.UnregisterDeadHero(victim);
			}
		}

		// Token: 0x06003D5F RID: 15711 RVA: 0x0010A24C File Offset: 0x0010844C
		private void OnHeroOccupationChanged(Hero hero, Occupation oldOccupation)
		{
			if (oldOccupation == Occupation.Wanderer)
			{
				this.RemoveFromAliveCompanions(hero);
				return;
			}
			if (hero.Occupation == Occupation.Wanderer)
			{
				this.AddToAliveCompanions(hero, false);
			}
		}

		// Token: 0x06003D60 RID: 15712 RVA: 0x0010A26D File Offset: 0x0010846D
		private void OnHeroCreated(Hero hero, bool showNotification = true)
		{
			if (hero.IsAlive && hero.IsWanderer)
			{
				this.AddToAliveCompanions(hero, true);
			}
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x0010A288 File Offset: 0x00108488
		private void TryKillCompanion()
		{
			if (MBRandom.RandomFloat <= 0.1f && this._aliveCompanionTemplates.Count > 0)
			{
				CharacterObject randomElementInefficiently = this._aliveCompanionTemplates.GetRandomElementInefficiently<CharacterObject>();
				Hero hero = null;
				foreach (Hero hero2 in Hero.AllAliveHeroes)
				{
					if (hero2.Template == randomElementInefficiently && hero2.IsWanderer)
					{
						hero = hero2;
						break;
					}
				}
				if (hero != null && hero.CompanionOf == null && (hero.CurrentSettlement == null || hero.CurrentSettlement != Hero.MainHero.CurrentSettlement))
				{
					KillCharacterAction.ApplyByRemove(hero, false, true);
				}
			}
		}

		// Token: 0x06003D62 RID: 15714 RVA: 0x0010A340 File Offset: 0x00108540
		private void TrySpawnNewCompanion()
		{
			if ((float)this._aliveCompanionTemplates.Count < this._desiredTotalCompanionCount)
			{
				Town randomElementWithPredicate = Town.AllTowns.GetRandomElementWithPredicate<Town>(delegate(Town x)
				{
					if (x.Settlement != Hero.MainHero.CurrentSettlement && x.Settlement.SiegeEvent == null)
					{
						return x.Settlement.HeroesWithoutParty.AllQ<Hero>((Hero y) => !y.IsWanderer || y.CompanionOf != null);
					}
					return false;
				});
				Settlement settlement = ((randomElementWithPredicate != null) ? randomElementWithPredicate.Settlement : null);
				if (settlement != null)
				{
					this.CreateCompanionAndAddToSettlement(settlement);
				}
			}
		}

		// Token: 0x06003D63 RID: 15715 RVA: 0x0010A3A4 File Offset: 0x001085A4
		private void SwapCompanions()
		{
			int num = Town.AllTowns.Count / 2;
			int num2 = MBRandom.RandomInt(Town.AllTowns.Count % 2);
			Town town = Town.AllTowns[num2 + MBRandom.RandomInt(num)];
			Hero hero = town.Settlement.HeroesWithoutParty.Where<Hero>((Hero x) => x.IsWanderer && x.CompanionOf == null).GetRandomElementInefficiently<Hero>();
			for (int i = 1; i < 2; i++)
			{
				Town town2 = Town.AllTowns[i * num + num2 + MBRandom.RandomInt(num)];
				IEnumerable<Hero> enumerable = town2.Settlement.HeroesWithoutParty.Where<Hero>((Hero x) => x.IsWanderer && x.CompanionOf == null);
				Hero hero2 = null;
				if (enumerable.Any<Hero>())
				{
					hero2 = enumerable.GetRandomElementInefficiently<Hero>();
					LeaveSettlementAction.ApplyForCharacterOnly(hero2);
				}
				if (hero != null)
				{
					EnterSettlementAction.ApplyForCharacterOnly(hero, town2.Settlement);
				}
				hero = hero2;
			}
			if (hero != null)
			{
				EnterSettlementAction.ApplyForCharacterOnly(hero, town.Settlement);
			}
		}

		// Token: 0x06003D64 RID: 15716 RVA: 0x0010A4B7 File Offset: 0x001086B7
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003D65 RID: 15717 RVA: 0x0010A4BC File Offset: 0x001086BC
		private void OnNewGameCreated(CampaignGameStarter starter)
		{
			this.InitializeCompanionTemplateList();
			List<Town> list = Town.AllTowns.ToListQ<Town>();
			list.Shuffle<Town>();
			int num = 0;
			while ((float)num < this._desiredTotalCompanionCount)
			{
				this.CreateCompanionAndAddToSettlement(list[num].Settlement);
				num++;
			}
		}

		// Token: 0x06003D66 RID: 15718 RVA: 0x0010A504 File Offset: 0x00108704
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			this.InitializeCompanionTemplateList();
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				if (hero.IsWanderer)
				{
					this.AddToAliveCompanions(hero, false);
				}
			}
			foreach (Hero hero2 in Hero.DeadOrDisabledHeroes)
			{
				if (hero2.IsAlive && hero2.IsWanderer)
				{
					this.AddToAliveCompanions(hero2, false);
				}
			}
		}

		// Token: 0x06003D67 RID: 15719 RVA: 0x0010A5B8 File Offset: 0x001087B8
		private void AdjustEquipments(Hero hero)
		{
			this.AdjustEquipmentModifiers(hero.BattleEquipment);
			this.AdjustEquipmentModifiers(hero.CivilianEquipment);
		}

		// Token: 0x06003D68 RID: 15720 RVA: 0x0010A5D4 File Offset: 0x001087D4
		private void AdjustEquipmentModifiers(Equipment equipment)
		{
			ItemModifier @object = MBObjectManager.Instance.GetObject<ItemModifier>("companion_armor");
			ItemModifier object2 = MBObjectManager.Instance.GetObject<ItemModifier>("companion_weapon");
			ItemModifier object3 = MBObjectManager.Instance.GetObject<ItemModifier>("companion_horse");
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumEquipmentSetSlots; equipmentIndex++)
			{
				EquipmentElement equipmentElement = equipment[equipmentIndex];
				if (equipmentElement.Item != null)
				{
					if (equipmentElement.Item.ArmorComponent != null)
					{
						equipment[equipmentIndex] = new EquipmentElement(equipmentElement.Item, @object, null, false);
					}
					else if (equipmentElement.Item.HorseComponent != null)
					{
						equipment[equipmentIndex] = new EquipmentElement(equipmentElement.Item, object3, null, false);
					}
					else if (equipmentElement.Item.WeaponComponent != null)
					{
						equipment[equipmentIndex] = new EquipmentElement(equipmentElement.Item, object2, null, false);
					}
				}
			}
		}

		// Token: 0x06003D69 RID: 15721 RVA: 0x0010A6A8 File Offset: 0x001088A8
		private void InitializeCompanionTemplateList()
		{
			foreach (CharacterObject characterObject in MBObjectManager.Instance.GetObjectTypeList<CharacterObject>())
			{
				if (characterObject.IsTemplate && characterObject.Occupation == Occupation.Wanderer)
				{
					this._companionsOfTemplates[this.GetTemplateTypeOfCompanion(characterObject)].Add(characterObject);
				}
			}
		}

		// Token: 0x06003D6A RID: 15722 RVA: 0x0010A724 File Offset: 0x00108924
		private CompanionsCampaignBehavior.CompanionTemplateType GetTemplateTypeOfCompanion(CharacterObject character)
		{
			if (character.IsMariner)
			{
				return CompanionsCampaignBehavior.CompanionTemplateType.Sailor;
			}
			CompanionsCampaignBehavior.CompanionTemplateType companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Combat;
			int num = 20;
			foreach (SkillObject skillObject in Skills.All)
			{
				int skillValue = character.GetSkillValue(skillObject);
				if (skillValue > num)
				{
					CompanionsCampaignBehavior.CompanionTemplateType templateTypeForSkill = this.GetTemplateTypeForSkill(skillObject);
					if (templateTypeForSkill != CompanionsCampaignBehavior.CompanionTemplateType.Combat)
					{
						num = skillValue;
						companionTemplateType = templateTypeForSkill;
					}
				}
			}
			return companionTemplateType;
		}

		// Token: 0x06003D6B RID: 15723 RVA: 0x0010A7A4 File Offset: 0x001089A4
		private void CreateCompanionAndAddToSettlement(Settlement settlement)
		{
			CharacterObject companionTemplate = this.GetCompanionTemplateToSpawn();
			if (companionTemplate != null)
			{
				Town randomElementWithPredicate = Town.AllTowns.GetRandomElementWithPredicate<Town>((Town x) => x.Culture == companionTemplate.Culture);
				Settlement settlement2 = ((randomElementWithPredicate != null) ? randomElementWithPredicate.Settlement : null);
				if (settlement2 == null)
				{
					settlement2 = Town.AllTowns.GetRandomElement<Town>().Settlement;
				}
				Hero hero = HeroCreator.CreateSpecialHero(companionTemplate, settlement2, null, null, Campaign.Current.Models.AgeModel.HeroComesOfAge + 5 + MBRandom.RandomInt(12));
				this.AdjustEquipments(hero);
				hero.ChangeState(Hero.CharacterStates.Active);
				EnterSettlementAction.ApplyForCharacterOnly(hero, settlement);
			}
		}

		// Token: 0x06003D6C RID: 15724 RVA: 0x0010A844 File Offset: 0x00108A44
		private CompanionsCampaignBehavior.CompanionTemplateType GetCompanionTemplateTypeToSpawn()
		{
			CompanionsCampaignBehavior.CompanionTemplateType companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Combat;
			float num = -1f;
			foreach (KeyValuePair<CompanionsCampaignBehavior.CompanionTemplateType, List<CharacterObject>> keyValuePair in this._companionsOfTemplates)
			{
				float templateTypeScore = this.GetTemplateTypeScore(keyValuePair.Key);
				if (templateTypeScore > 0f)
				{
					int num2 = 0;
					foreach (CharacterObject characterObject in keyValuePair.Value)
					{
						if (this._aliveCompanionTemplates.Contains(characterObject))
						{
							num2++;
						}
					}
					float num3 = (float)num2 / this._desiredTotalCompanionCount;
					float num4 = (templateTypeScore - num3) / templateTypeScore;
					if (num2 < keyValuePair.Value.Count && num4 > num)
					{
						num = num4;
						companionTemplateType = keyValuePair.Key;
					}
				}
			}
			return companionTemplateType;
		}

		// Token: 0x06003D6D RID: 15725 RVA: 0x0010A944 File Offset: 0x00108B44
		private bool IsTemplateKnown(CharacterObject companionTemplate)
		{
			foreach (KeyValuePair<CompanionsCampaignBehavior.CompanionTemplateType, List<CharacterObject>> keyValuePair in this._companionsOfTemplates)
			{
				for (int i = 0; i < keyValuePair.Value.Count; i++)
				{
					if (companionTemplate == keyValuePair.Value[i])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003D6E RID: 15726 RVA: 0x0010A9B8 File Offset: 0x00108BB8
		private CharacterObject GetCompanionTemplateToSpawn()
		{
			List<CharacterObject> list = this._companionsOfTemplates[this.GetCompanionTemplateTypeToSpawn()];
			list.Shuffle<CharacterObject>();
			CharacterObject characterObject = null;
			foreach (CharacterObject characterObject2 in list)
			{
				if (!this._aliveCompanionTemplates.Contains(characterObject2))
				{
					characterObject = characterObject2;
					break;
				}
			}
			return characterObject;
		}

		// Token: 0x06003D6F RID: 15727 RVA: 0x0010AA2C File Offset: 0x00108C2C
		private float GetTemplateTypeScore(CompanionsCampaignBehavior.CompanionTemplateType templateType)
		{
			switch (templateType)
			{
			case CompanionsCampaignBehavior.CompanionTemplateType.Engineering:
				return 0.05263158f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Tactics:
				return 0.10526316f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Leadership:
				return 0.078947365f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Steward:
				return 0.078947365f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Trade:
				return 0.078947365f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Roguery:
				return 0.10526316f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Medicine:
				return 0.078947365f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Smithing:
				return 0.05263158f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Scouting:
				return 0.13157895f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Combat:
				return 0.13157895f;
			case CompanionsCampaignBehavior.CompanionTemplateType.Sailor:
				return 0.10526316f;
			default:
				return 0f;
			}
		}

		// Token: 0x06003D70 RID: 15728 RVA: 0x0010AAB4 File Offset: 0x00108CB4
		private CompanionsCampaignBehavior.CompanionTemplateType GetTemplateTypeForSkill(SkillObject skill)
		{
			CompanionsCampaignBehavior.CompanionTemplateType companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Combat;
			if (skill == DefaultSkills.Engineering)
			{
				companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Engineering;
			}
			else if (skill == DefaultSkills.Tactics)
			{
				companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Tactics;
			}
			else if (skill == DefaultSkills.Leadership)
			{
				companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Leadership;
			}
			else if (skill == DefaultSkills.Steward)
			{
				companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Steward;
			}
			else if (skill == DefaultSkills.Trade)
			{
				companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Trade;
			}
			else if (skill == DefaultSkills.Roguery)
			{
				companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Roguery;
			}
			else if (skill == DefaultSkills.Medicine)
			{
				companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Medicine;
			}
			else if (skill == DefaultSkills.Crafting)
			{
				companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Smithing;
			}
			else if (skill == DefaultSkills.Scouting)
			{
				companionTemplateType = CompanionsCampaignBehavior.CompanionTemplateType.Scouting;
			}
			return companionTemplateType;
		}

		// Token: 0x04001299 RID: 4761
		private const int CompanionMoveRandomIndex = 2;

		// Token: 0x0400129A RID: 4762
		private const float DesiredCompanionPerTown = 0.6f;

		// Token: 0x0400129B RID: 4763
		private const float KillChance = 0.1f;

		// Token: 0x0400129C RID: 4764
		private const int SkillThresholdValue = 20;

		// Token: 0x0400129D RID: 4765
		private const int RemoveWandererAfterDays = 40;

		// Token: 0x0400129E RID: 4766
		private IReadOnlyDictionary<CompanionsCampaignBehavior.CompanionTemplateType, List<CharacterObject>> _companionsOfTemplates = new Dictionary<CompanionsCampaignBehavior.CompanionTemplateType, List<CharacterObject>>
		{
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Engineering,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Tactics,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Leadership,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Steward,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Trade,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Roguery,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Medicine,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Smithing,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Scouting,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Combat,
				new List<CharacterObject>()
			},
			{
				CompanionsCampaignBehavior.CompanionTemplateType.Sailor,
				new List<CharacterObject>()
			}
		};

		// Token: 0x0400129F RID: 4767
		private HashSet<CharacterObject> _aliveCompanionTemplates = new HashSet<CharacterObject>();

		// Token: 0x040012A0 RID: 4768
		private const float EngineerScore = 2f;

		// Token: 0x040012A1 RID: 4769
		private const float TacticsScore = 4f;

		// Token: 0x040012A2 RID: 4770
		private const float LeadershipScore = 3f;

		// Token: 0x040012A3 RID: 4771
		private const float StewardScore = 3f;

		// Token: 0x040012A4 RID: 4772
		private const float TradeScore = 3f;

		// Token: 0x040012A5 RID: 4773
		private const float RogueryScore = 4f;

		// Token: 0x040012A6 RID: 4774
		private const float MedicineScore = 3f;

		// Token: 0x040012A7 RID: 4775
		private const float SmithingScore = 2f;

		// Token: 0x040012A8 RID: 4776
		private const float ScoutingScore = 5f;

		// Token: 0x040012A9 RID: 4777
		private const float CombatScore = 5f;

		// Token: 0x040012AA RID: 4778
		private const float SailorScore = 4f;

		// Token: 0x040012AB RID: 4779
		private const float AllScore = 38f;

		// Token: 0x020007D9 RID: 2009
		private enum CompanionTemplateType
		{
			// Token: 0x04001FA2 RID: 8098
			Engineering,
			// Token: 0x04001FA3 RID: 8099
			Tactics,
			// Token: 0x04001FA4 RID: 8100
			Leadership,
			// Token: 0x04001FA5 RID: 8101
			Steward,
			// Token: 0x04001FA6 RID: 8102
			Trade,
			// Token: 0x04001FA7 RID: 8103
			Roguery,
			// Token: 0x04001FA8 RID: 8104
			Medicine,
			// Token: 0x04001FA9 RID: 8105
			Smithing,
			// Token: 0x04001FAA RID: 8106
			Scouting,
			// Token: 0x04001FAB RID: 8107
			Combat,
			// Token: 0x04001FAC RID: 8108
			Sailor
		}
	}
}
