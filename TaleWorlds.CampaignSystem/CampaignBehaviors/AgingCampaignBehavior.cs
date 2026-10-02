using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003CE RID: 974
	public class AgingCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06003A3F RID: 14911 RVA: 0x000EEB08 File Offset: 0x000ECD08
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.DailyTickHero));
			CampaignEvents.OnCharacterCreationIsOverEvent.AddNonSerializedListener(this, new Action(this.OnCharacterCreationIsOver));
			CampaignEvents.HeroComesOfAgeEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroComesOfAge));
			CampaignEvents.HeroReachesTeenAgeEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroReachesTeenAge));
			CampaignEvents.HeroGrowsOutOfInfancyEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroGrowsOutOfInfancy));
			CampaignEvents.PerkOpenedEvent.AddNonSerializedListener(this, new Action<Hero, PerkObject>(this.OnPerkOpened));
			CampaignEvents.HeroCreated.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroCreated));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x000EEBE4 File Offset: 0x000ECDE4
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<Hero, int>>("_extraLivesContainer", ref this._extraLivesContainer);
			dataStore.SyncData<Dictionary<Hero, int>>("_heroesYoungerThanHeroComesOfAge", ref this._heroesYoungerThanHeroComesOfAge);
		}

		// Token: 0x06003A41 RID: 14913 RVA: 0x000EEC0C File Offset: 0x000ECE0C
		private void OnHeroCreated(Hero hero, bool isBornNaturally)
		{
			int num = (int)hero.Age;
			if (num < Campaign.Current.Models.AgeModel.HeroComesOfAge)
			{
				this._heroesYoungerThanHeroComesOfAge.Add(hero, num);
			}
		}

		// Token: 0x06003A42 RID: 14914 RVA: 0x000EEC45 File Offset: 0x000ECE45
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification)
		{
			if (this._heroesYoungerThanHeroComesOfAge.ContainsKey(victim))
			{
				this._heroesYoungerThanHeroComesOfAge.Remove(victim);
			}
		}

		// Token: 0x06003A43 RID: 14915 RVA: 0x000EEC64 File Offset: 0x000ECE64
		private void AddExtraLife(Hero hero)
		{
			if (hero.IsAlive)
			{
				if (this._extraLivesContainer.ContainsKey(hero))
				{
					Dictionary<Hero, int> extraLivesContainer = this._extraLivesContainer;
					extraLivesContainer[hero]++;
					return;
				}
				this._extraLivesContainer.Add(hero, 1);
			}
		}

		// Token: 0x06003A44 RID: 14916 RVA: 0x000EECB0 File Offset: 0x000ECEB0
		private void OnPerkOpened(Hero hero, PerkObject perk)
		{
			if (perk == DefaultPerks.Medicine.CheatDeath)
			{
				this.AddExtraLife(hero);
			}
			if (perk == DefaultPerks.Medicine.HealthAdvise)
			{
				Clan clan = hero.Clan;
				if (((clan != null) ? clan.Leader : null) == hero)
				{
					foreach (Hero hero2 in hero.Clan.Heroes)
					{
						if (hero2.IsAlive)
						{
							this.AddExtraLife(hero2);
						}
					}
				}
			}
		}

		// Token: 0x06003A45 RID: 14917 RVA: 0x000EED3C File Offset: 0x000ECF3C
		private void DailyTickHero(Hero hero)
		{
			bool flag = (int)CampaignTime.Now.ToDays == this._gameStartDay;
			if (!CampaignOptions.IsLifeDeathCycleDisabled && !flag && !hero.IsTemplate)
			{
				if (hero.IsAlive && hero.CanDie(KillCharacterAction.KillCharacterActionDetail.DiedOfOldAge))
				{
					if (hero.DeathMark != KillCharacterAction.KillCharacterActionDetail.None && (hero.PartyBelongedTo == null || (hero.PartyBelongedTo.MapEvent == null && hero.PartyBelongedTo.SiegeEvent == null)))
					{
						KillCharacterAction.ApplyByDeathMark(hero, false);
					}
					else
					{
						this.IsItTimeOfDeath(hero);
					}
				}
				int num;
				if (this._heroesYoungerThanHeroComesOfAge.TryGetValue(hero, out num))
				{
					int num2 = (int)hero.Age;
					if (num != num2)
					{
						if (num2 >= Campaign.Current.Models.AgeModel.HeroComesOfAge)
						{
							this._heroesYoungerThanHeroComesOfAge.Remove(hero);
							CampaignEventDispatcher.Instance.OnHeroComesOfAge(hero);
						}
						else
						{
							this._heroesYoungerThanHeroComesOfAge[hero] = num2;
							if (num2 == Campaign.Current.Models.AgeModel.BecomeTeenagerAge)
							{
								CampaignEventDispatcher.Instance.OnHeroReachesTeenAge(hero);
							}
							else if (num2 == Campaign.Current.Models.AgeModel.BecomeChildAge)
							{
								CampaignEventDispatcher.Instance.OnHeroGrowsOutOfInfancy(hero);
							}
						}
					}
				}
				if (hero == Hero.MainHero && Hero.IsMainHeroIll && Hero.MainHero.HeroState != Hero.CharacterStates.Dead)
				{
					Campaign.Current.MainHeroIllDays++;
					if (Campaign.Current.MainHeroIllDays > 3)
					{
						Hero.MainHero.HitPoints -= MathF.Ceiling((float)Hero.MainHero.HitPoints * (0.05f * (float)Campaign.Current.MainHeroIllDays));
						if (Hero.MainHero.HitPoints <= 1 && Hero.MainHero.DeathMark == KillCharacterAction.KillCharacterActionDetail.None)
						{
							int num3;
							if (this._extraLivesContainer.TryGetValue(Hero.MainHero, out num3))
							{
								if (num3 == 0)
								{
									this.KillMainHeroWithIllness();
									return;
								}
								Campaign.Current.MainHeroIllDays = -1;
								this._extraLivesContainer[Hero.MainHero] = num3 - 1;
								if (this._extraLivesContainer[Hero.MainHero] == 0)
								{
									this._extraLivesContainer.Remove(Hero.MainHero);
									return;
								}
							}
							else
							{
								this.KillMainHeroWithIllness();
							}
						}
					}
				}
			}
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x000EEF67 File Offset: 0x000ED167
		private void KillMainHeroWithIllness()
		{
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			Hero.MainHero.AddDeathMark(null, KillCharacterAction.KillCharacterActionDetail.DiedOfOldAge);
			KillCharacterAction.ApplyByOldAge(Hero.MainHero, true);
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x000EEF8B File Offset: 0x000ED18B
		private void OnGameLoaded(CampaignGameStarter obj)
		{
			this.CheckYoungHeroes();
		}

		// Token: 0x06003A48 RID: 14920 RVA: 0x000EEF94 File Offset: 0x000ED194
		private void OnCharacterCreationIsOver()
		{
			this._gameStartDay = (int)CampaignTime.Now.ToDays;
			if (!CampaignOptions.IsLifeDeathCycleDisabled)
			{
				this.InitializeHeroesYoungerThanHeroComesOfAge();
			}
		}

		// Token: 0x06003A49 RID: 14921 RVA: 0x000EEFC2 File Offset: 0x000ED1C2
		private void OnHeroGrowsOutOfInfancy(Hero hero)
		{
			if (hero.Clan != Clan.PlayerClan)
			{
				hero.HeroDeveloper.InitializeHeroDeveloper();
			}
		}

		// Token: 0x06003A4A RID: 14922 RVA: 0x000EEFDC File Offset: 0x000ED1DC
		private void OnHeroReachesTeenAge(Hero hero)
		{
			Equipment equipmentForHeroReachesTeenAge = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentForHeroReachesTeenAge(hero);
			if (equipmentForHeroReachesTeenAge != null)
			{
				EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, equipmentForHeroReachesTeenAge);
				new Equipment(Equipment.EquipmentType.Battle).FillFrom(equipmentForHeroReachesTeenAge, false);
				EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, equipmentForHeroReachesTeenAge);
			}
			if (hero.Clan != Clan.PlayerClan)
			{
				foreach (TraitObject traitObject in DefaultTraits.Personality)
				{
					int num = hero.GetTraitLevel(traitObject);
					if (hero.Father == null && hero.Mother == null && hero.Template != null)
					{
						hero.SetTraitLevel(traitObject, hero.Template.GetTraitLevel(traitObject));
					}
					else
					{
						float randomFloat = MBRandom.RandomFloat;
						float randomFloat2 = MBRandom.RandomFloat;
						if ((double)randomFloat < 0.2 && hero.Father != null)
						{
							num = hero.Father.GetTraitLevel(traitObject);
						}
						else if ((double)randomFloat < 0.6 && !hero.CharacterObject.IsFemale && hero.Father != null)
						{
							num = hero.Father.GetTraitLevel(traitObject);
						}
						else if ((double)randomFloat < 0.6 && hero.Mother != null)
						{
							num = hero.Mother.GetTraitLevel(traitObject);
						}
						else if ((double)randomFloat < 0.7 && hero.Mother != null)
						{
							num = hero.Mother.GetTraitLevel(traitObject);
						}
						else if ((double)randomFloat2 < 0.3)
						{
							num--;
						}
						else if ((double)randomFloat2 >= 0.7)
						{
							num++;
						}
						num = MBMath.ClampInt(num, traitObject.MinValue, traitObject.MaxValue);
						if (num != hero.GetTraitLevel(traitObject))
						{
							hero.SetTraitLevel(traitObject, num);
						}
					}
				}
				hero.HeroDeveloper.InitializeHeroDeveloper();
			}
		}

		// Token: 0x06003A4B RID: 14923 RVA: 0x000EF1B8 File Offset: 0x000ED3B8
		private void OnHeroComesOfAge(Hero hero)
		{
			if (hero.HeroState != Hero.CharacterStates.Active)
			{
				if (hero.Clan != Clan.PlayerClan)
				{
					foreach (ValueTuple<SkillObject, int> valueTuple in Campaign.Current.Models.HeroCreationModel.GetInheritedSkillsForHero(hero))
					{
						hero.SetSkillValue(valueTuple.Item1, valueTuple.Item2);
					}
					hero.HeroDeveloper.InitializeHeroDeveloper();
				}
				else
				{
					hero.HeroDeveloper.SetInitialLevel(hero.Level);
				}
				Equipment equipment = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentForHeroComeOfAge(hero, Equipment.EquipmentType.Battle);
				Equipment equipment2 = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentForHeroComeOfAge(hero, Equipment.EquipmentType.Civilian);
				if (equipment == null)
				{
					Debug.FailedAssert("Battle equipment should not be empty", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\AgingCampaignBehavior.cs", "OnHeroComesOfAge", 304);
					equipment = MBEquipmentRosterExtensions.All.Find((MBEquipmentRoster x) => x.StringId == "generic_bat_dummy").GetBattleEquipments().First<Equipment>();
				}
				if (equipment2 == null)
				{
					Debug.FailedAssert("Civilian equipment should not be empty", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\AgingCampaignBehavior.cs", "OnHeroComesOfAge", 313);
					equipment2 = MBEquipmentRosterExtensions.All.Find((MBEquipmentRoster x) => x.StringId == "generic_civ_dummy").GetCivilianEquipments().First<Equipment>();
				}
				EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, equipment);
				EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, equipment2);
			}
		}

		// Token: 0x06003A4C RID: 14924 RVA: 0x000EF338 File Offset: 0x000ED538
		private void IsItTimeOfDeath(Hero hero)
		{
			if (hero.IsAlive && hero.Age >= (float)Campaign.Current.Models.AgeModel.BecomeOldAge && !CampaignOptions.IsLifeDeathCycleDisabled && hero.DeathMark == KillCharacterAction.KillCharacterActionDetail.None && MBRandom.RandomFloat < hero.ProbabilityOfDeath)
			{
				int num;
				if (this._extraLivesContainer.TryGetValue(hero, out num) && num > 0)
				{
					this._extraLivesContainer[hero] = num - 1;
					if (this._extraLivesContainer[hero] == 0)
					{
						this._extraLivesContainer.Remove(hero);
						return;
					}
				}
				else
				{
					if (hero == Hero.MainHero && !Hero.IsMainHeroIll)
					{
						Campaign.Current.MainHeroIllDays++;
						Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
						InformationManager.ShowInquiry(new InquiryData(new TextObject("{=2duoimiP}Caught Illness", null).ToString(), new TextObject("{=vo3MqtMn}You are at death's door, wracked by fever, drifting in and out of consciousness. The healers do not believe that you can recover. You should resolve your final affairs and determine a heir for your clan while you still have the strength to speak.", null).ToString(), true, false, new TextObject("{=yQtzabbe}Close", null).ToString(), "", null, null, "event:/ui/notification/quest_fail", 0f, null, null, null), false, false);
						return;
					}
					if (hero != Hero.MainHero && (hero.PartyBelongedTo == null || (hero.PartyBelongedTo.MapEvent == null && hero.PartyBelongedTo.SiegeEvent == null)))
					{
						KillCharacterAction.ApplyByOldAge(hero, true);
					}
				}
			}
		}

		// Token: 0x06003A4D RID: 14925 RVA: 0x000EF488 File Offset: 0x000ED688
		private void InitializeHeroesYoungerThanHeroComesOfAge()
		{
			foreach (Hero hero in Hero.AllAliveHeroes)
			{
				int num = (int)hero.Age;
				if (num < Campaign.Current.Models.AgeModel.HeroComesOfAge && !this._heroesYoungerThanHeroComesOfAge.ContainsKey(hero))
				{
					this._heroesYoungerThanHeroComesOfAge.Add(hero, num);
				}
			}
			foreach (Hero hero2 in Hero.DeadOrDisabledHeroes)
			{
				if (!hero2.IsDead && hero2.Age < (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && !this._heroesYoungerThanHeroComesOfAge.ContainsKey(hero2))
				{
					this._heroesYoungerThanHeroComesOfAge.Add(hero2, (int)hero2.Age);
				}
			}
		}

		// Token: 0x06003A4E RID: 14926 RVA: 0x000EF590 File Offset: 0x000ED790
		private void CheckYoungHeroes()
		{
			foreach (Hero hero in Hero.FindAll((Hero x) => !x.IsDead && x.Age < (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && !this._heroesYoungerThanHeroComesOfAge.ContainsKey(x)))
			{
				this._heroesYoungerThanHeroComesOfAge.Add(hero, (int)hero.Age);
				if (!hero.IsDisabled && !this._heroesYoungerThanHeroComesOfAge.ContainsKey(hero))
				{
					if (hero.Age > (float)Campaign.Current.Models.AgeModel.BecomeChildAge)
					{
						CampaignEventDispatcher.Instance.OnHeroGrowsOutOfInfancy(hero);
					}
					if (hero.Age > (float)Campaign.Current.Models.AgeModel.BecomeTeenagerAge)
					{
						CampaignEventDispatcher.Instance.OnHeroReachesTeenAge(hero);
					}
				}
			}
		}

		// Token: 0x04001236 RID: 4662
		private Dictionary<Hero, int> _extraLivesContainer = new Dictionary<Hero, int>();

		// Token: 0x04001237 RID: 4663
		private Dictionary<Hero, int> _heroesYoungerThanHeroComesOfAge = new Dictionary<Hero, int>();

		// Token: 0x04001238 RID: 4664
		private int _gameStartDay;
	}
}
