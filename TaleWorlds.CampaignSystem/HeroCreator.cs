using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200008E RID: 142
	public static class HeroCreator
	{
		// Token: 0x0600124C RID: 4684 RVA: 0x00053FF4 File Offset: 0x000521F4
		public static Hero CreateNotable(Occupation occupation, Settlement settlement = null)
		{
			CharacterObject randomTemplateByOccupation = Campaign.Current.Models.HeroCreationModel.GetRandomTemplateByOccupation(occupation, settlement);
			ValueTuple<CampaignTime, CampaignTime> birthAndDeathDay = Campaign.Current.Models.HeroCreationModel.GetBirthAndDeathDay(randomTemplateByOccupation, true, -1);
			CampaignTime item = birthAndDeathDay.Item1;
			CampaignTime item2 = birthAndDeathDay.Item2;
			Hero hero = HeroCreator.CreateHero(randomTemplateByOccupation, true, item, item2);
			HeroCreator.HeroInitializationArgs heroInitializationArgs = new HeroCreator.HeroInitializationArgs(hero, false).SetGenerateFirstAndFullName(true);
			if (settlement != null)
			{
				heroInitializationArgs.SetBornSettlement(settlement);
			}
			heroInitializationArgs.SetAppearance(new StaticBodyProperties?(Campaign.Current.Models.HeroCreationModel.GetStaticBodyProperties(hero, false, 0f)), -1f, -1f, -1, -1, -1);
			HeroCreator.InitializeHeroFromSettings(heroInitializationArgs.Hero, heroInitializationArgs);
			return hero;
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x000540A8 File Offset: 0x000522A8
		public static Hero CreateSpecialHero(CharacterObject template, Settlement bornSettlement = null, Clan faction = null, Clan supporterOfClan = null, int age = -1)
		{
			ValueTuple<CampaignTime, CampaignTime> birthAndDeathDay = Campaign.Current.Models.HeroCreationModel.GetBirthAndDeathDay(template, true, age);
			CampaignTime item = birthAndDeathDay.Item1;
			CampaignTime item2 = birthAndDeathDay.Item2;
			Hero hero = HeroCreator.CreateHero(template, true, item, item2);
			HeroCreator.HeroInitializationArgs heroInitializationArgs = new HeroCreator.HeroInitializationArgs(hero, false).SetGenerateFirstAndFullName(true);
			if (bornSettlement != null)
			{
				heroInitializationArgs.SetBornSettlement(bornSettlement);
			}
			if (faction != null)
			{
				heroInitializationArgs.SetClan(faction);
			}
			if (supporterOfClan != null)
			{
				heroInitializationArgs.SetSupporterOf(supporterOfClan);
			}
			HeroCreator.InitializeHeroFromSettings(heroInitializationArgs.Hero, heroInitializationArgs);
			return hero;
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x00054120 File Offset: 0x00052320
		public static Hero CreateChild(CharacterObject template, Settlement bornSettlement, Clan clan, int age)
		{
			ValueTuple<CampaignTime, CampaignTime> birthAndDeathDay = Campaign.Current.Models.HeroCreationModel.GetBirthAndDeathDay(template, true, age);
			CampaignTime item = birthAndDeathDay.Item1;
			CampaignTime item2 = birthAndDeathDay.Item2;
			Hero hero = HeroCreator.CreateHero(template, true, item, item2);
			HeroCreator.HeroInitializationArgs heroInitializationArgs = new HeroCreator.HeroInitializationArgs(hero, false).SetGenerateFirstAndFullName(true).SetBornSettlement(bornSettlement).SetClan(clan)
				.SetLevel(1);
			HeroCreator.InitializeHeroFromSettings(heroInitializationArgs.Hero, heroInitializationArgs);
			return hero;
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x00054188 File Offset: 0x00052388
		public static Hero CreateRelativeNotableHero(Hero relative)
		{
			CharacterObject randomTemplateByOccupation = Campaign.Current.Models.HeroCreationModel.GetRandomTemplateByOccupation(relative.Occupation, relative.HomeSettlement);
			ValueTuple<CampaignTime, CampaignTime> birthAndDeathDay = Campaign.Current.Models.HeroCreationModel.GetBirthAndDeathDay(randomTemplateByOccupation, true, -1);
			CampaignTime item = birthAndDeathDay.Item1;
			CampaignTime item2 = birthAndDeathDay.Item2;
			Hero hero = HeroCreator.CreateHero(randomTemplateByOccupation, true, item, item2);
			BodyProperties bodyPropertiesMin = relative.CharacterObject.GetBodyPropertiesMin(false);
			BodyProperties bodyPropertiesMin2 = randomTemplateByOccupation.GetBodyPropertiesMin(false);
			int defaultFaceSeed = relative.CharacterObject.GetDefaultFaceSeed(1);
			MBBodyProperty bodyPropertyRange = hero.CharacterObject.BodyPropertyRange;
			BodyProperties randomBodyProperties = BodyProperties.GetRandomBodyProperties(randomTemplateByOccupation.Race, randomTemplateByOccupation.IsFemale, bodyPropertiesMin, bodyPropertiesMin2, 1, defaultFaceSeed, bodyPropertyRange.HairTags, bodyPropertyRange.BeardTags, bodyPropertyRange.TattooTags, 0f);
			HeroCreator.HeroInitializationArgs heroInitializationArgs = new HeroCreator.HeroInitializationArgs(hero, false).SetBornSettlement(relative.HomeSettlement).SetCulture(relative.Culture).SetAppearance(new StaticBodyProperties?(randomBodyProperties.StaticProperties), -1f, -1f, -1, -1, -1)
				.SetGenerateFirstAndFullName(true);
			HeroCreator.InitializeHeroFromSettings(heroInitializationArgs.Hero, heroInitializationArgs);
			return hero;
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x00054298 File Offset: 0x00052498
		public static bool CreateBasicHero(string stringId, CharacterObject character, out Hero hero, bool isAlive = true)
		{
			hero = Campaign.Current.CampaignObjectManager.Find<Hero>(stringId);
			if (hero == null)
			{
				ValueTuple<CampaignTime, CampaignTime> birthAndDeathDay = Campaign.Current.Models.HeroCreationModel.GetBirthAndDeathDay(character, isAlive, (int)character.Age);
				CampaignTime item = birthAndDeathDay.Item1;
				CampaignTime item2 = birthAndDeathDay.Item2;
				hero = HeroCreator.CreateHero(character, false, item, item2);
				HeroCreator.HeroInitializationArgs heroInitializationArgs = new HeroCreator.HeroInitializationArgs(hero, false);
				HeroCreator.InitializeHeroFromSettings(heroInitializationArgs.Hero, heroInitializationArgs);
				return true;
			}
			return false;
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x00054308 File Offset: 0x00052508
		public static Hero DeliverOffSpring(Hero mother, Hero father, bool isOffspringFemale)
		{
			Debug.SilentAssert(mother.CharacterObject.Race == father.CharacterObject.Race, "", false, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\HeroCreator.cs", "DeliverOffSpring", 272);
			CharacterObject characterTemplateForOffspring = Campaign.Current.Models.HeroCreationModel.GetCharacterTemplateForOffspring(mother, father, isOffspringFemale);
			ValueTuple<CampaignTime, CampaignTime> birthAndDeathDay = Campaign.Current.Models.HeroCreationModel.GetBirthAndDeathDay(characterTemplateForOffspring, true, 0);
			CampaignTime item = birthAndDeathDay.Item1;
			CampaignTime item2 = birthAndDeathDay.Item2;
			Hero hero = HeroCreator.CreateHero(characterTemplateForOffspring, true, item, item2);
			HeroCreator.HeroInitializationArgs heroInitializationArgs = new HeroCreator.HeroInitializationArgs(hero, true).SetMother(mother).SetFather(father).SetIsFemale(isOffspringFemale)
				.SetOccupation(isOffspringFemale ? mother.Occupation : father.Occupation)
				.SetLevel(1)
				.SetGenerateFirstAndFullName(true);
			if (mother == Hero.MainHero || father == Hero.MainHero)
			{
				heroInitializationArgs.SetClan(Hero.MainHero.Clan).SetCulture(Hero.MainHero.Culture);
			}
			else
			{
				CultureObject cultureObject = ((MBRandom.RandomFloat < 0.5f) ? father.Culture : mother.Culture);
				heroInitializationArgs.SetClan(father.Clan).SetCulture(cultureObject);
			}
			HeroCreator.InitializeHeroFromSettings(heroInitializationArgs.Hero, heroInitializationArgs);
			return hero;
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x00054438 File Offset: 0x00052638
		private static Hero CreateHero(CharacterObject character, bool useCharacterAsTemplate, CampaignTime birthDay, CampaignTime deathDay)
		{
			if (useCharacterAsTemplate)
			{
				Debug.Print("creating hero from template with id: " + character.StringId, 0, Debug.DebugColor.White, 17592186044416UL);
				character = CharacterObject.CreateFrom(character, null);
			}
			else
			{
				Debug.Print("creating hero for character with id: " + character.StringId, 0, Debug.DebugColor.White, 17592186044416UL);
			}
			return new Hero(character.StringId, character, birthDay, deathDay);
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x000544AC File Offset: 0x000526AC
		private static void InitializeHeroFromSettings(Hero hero, HeroCreator.HeroInitializationArgs initializationArgs)
		{
			hero.Mother = initializationArgs.Mother;
			hero.Father = initializationArgs.Father;
			hero.IsFemale = initializationArgs.IsFemale;
			hero.BornSettlement = (initializationArgs.HasBornSettlementBeenSet ? initializationArgs.BornSettlement : Campaign.Current.Models.HeroCreationModel.GetBornSettlement(hero));
			hero.PreferredUpgradeFormation = initializationArgs.PreferredUpgradeFormation ?? Campaign.Current.Models.HeroCreationModel.GetPreferredUpgradeFormation(hero);
			hero.Clan = (initializationArgs.HasClanBeenSet ? initializationArgs.Clan : Campaign.Current.Models.HeroCreationModel.GetClan(hero));
			hero.Culture = initializationArgs.Culture ?? Campaign.Current.Models.HeroCreationModel.GetCulture(hero, hero.BornSettlement, hero.Clan);
			hero.StaticBodyProperties = initializationArgs.StaticBodyProperties ?? Campaign.Current.Models.HeroCreationModel.GetStaticBodyProperties(hero, initializationArgs.IsOffspring, 0.35f);
			hero.SupporterOf = initializationArgs.SupporterOf;
			hero.Level = initializationArgs.Level;
			hero.Weight = initializationArgs.Weight;
			hero.Build = initializationArgs.Build;
			if (initializationArgs.GenerateFirstAndFullName)
			{
				ValueTuple<TextObject, TextObject> valueTuple = Campaign.Current.Models.HeroCreationModel.GenerateFirstAndFullName(hero);
				TextObject item = valueTuple.Item1;
				TextObject item2 = valueTuple.Item2;
				hero.SetName(item2, item);
			}
			else
			{
				hero.SetName(initializationArgs.Name, initializationArgs.FirstName);
			}
			if (initializationArgs.Occupation != hero.Occupation)
			{
				hero.SetNewOccupation(initializationArgs.Occupation);
			}
			foreach (ValueTuple<TraitObject, int> valueTuple2 in Campaign.Current.Models.HeroCreationModel.GetTraitsForHero(hero))
			{
				TraitObject item3 = valueTuple2.Item1;
				int item4 = valueTuple2.Item2;
				hero.SetTraitLevel(item3, item4);
			}
			foreach (ValueTuple<SkillObject, int> valueTuple3 in Campaign.Current.Models.HeroCreationModel.GetDefaultSkillsForHero(hero))
			{
				SkillObject item5 = valueTuple3.Item1;
				int item6 = valueTuple3.Item2;
				hero.SetSkillValue(item5, item6);
			}
			if (initializationArgs.IsOffspring)
			{
				hero.HeroDeveloper.InitializeHeroDeveloper();
				hero.ClearTraits();
			}
			else if (hero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge)
			{
				hero.HeroDeveloper.InitializeHeroDeveloper();
			}
			Equipment civilianEquipment = Campaign.Current.Models.HeroCreationModel.GetCivilianEquipment(hero);
			EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, civilianEquipment);
			Equipment battleEquipment = Campaign.Current.Models.HeroCreationModel.GetBattleEquipment(hero);
			EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, battleEquipment);
			CampaignEventDispatcher.Instance.OnHeroCreated(initializationArgs.Hero, initializationArgs.IsOffspring);
		}

		// Token: 0x02000542 RID: 1346
		private class HeroInitializationArgs
		{
			// Token: 0x17000EFD RID: 3837
			// (get) Token: 0x06004D0C RID: 19724 RVA: 0x0017FB46 File Offset: 0x0017DD46
			public Hero Hero { get; }

			// Token: 0x17000EFE RID: 3838
			// (get) Token: 0x06004D0D RID: 19725 RVA: 0x0017FB4E File Offset: 0x0017DD4E
			// (set) Token: 0x06004D0E RID: 19726 RVA: 0x0017FB56 File Offset: 0x0017DD56
			public TextObject Name { get; private set; }

			// Token: 0x17000EFF RID: 3839
			// (get) Token: 0x06004D0F RID: 19727 RVA: 0x0017FB5F File Offset: 0x0017DD5F
			// (set) Token: 0x06004D10 RID: 19728 RVA: 0x0017FB67 File Offset: 0x0017DD67
			public TextObject FirstName { get; private set; }

			// Token: 0x17000F00 RID: 3840
			// (get) Token: 0x06004D11 RID: 19729 RVA: 0x0017FB70 File Offset: 0x0017DD70
			// (set) Token: 0x06004D12 RID: 19730 RVA: 0x0017FB78 File Offset: 0x0017DD78
			public Hero Mother { get; private set; }

			// Token: 0x17000F01 RID: 3841
			// (get) Token: 0x06004D13 RID: 19731 RVA: 0x0017FB81 File Offset: 0x0017DD81
			// (set) Token: 0x06004D14 RID: 19732 RVA: 0x0017FB89 File Offset: 0x0017DD89
			public Hero Father { get; private set; }

			// Token: 0x17000F02 RID: 3842
			// (get) Token: 0x06004D15 RID: 19733 RVA: 0x0017FB92 File Offset: 0x0017DD92
			// (set) Token: 0x06004D16 RID: 19734 RVA: 0x0017FB9A File Offset: 0x0017DD9A
			public bool IsFemale { get; private set; }

			// Token: 0x17000F03 RID: 3843
			// (get) Token: 0x06004D17 RID: 19735 RVA: 0x0017FBA3 File Offset: 0x0017DDA3
			// (set) Token: 0x06004D18 RID: 19736 RVA: 0x0017FBAB File Offset: 0x0017DDAB
			public Settlement BornSettlement { get; private set; }

			// Token: 0x17000F04 RID: 3844
			// (get) Token: 0x06004D19 RID: 19737 RVA: 0x0017FBB4 File Offset: 0x0017DDB4
			// (set) Token: 0x06004D1A RID: 19738 RVA: 0x0017FBBC File Offset: 0x0017DDBC
			public int Level { get; private set; }

			// Token: 0x17000F05 RID: 3845
			// (get) Token: 0x06004D1B RID: 19739 RVA: 0x0017FBC5 File Offset: 0x0017DDC5
			// (set) Token: 0x06004D1C RID: 19740 RVA: 0x0017FBCD File Offset: 0x0017DDCD
			public float Weight { get; private set; }

			// Token: 0x17000F06 RID: 3846
			// (get) Token: 0x06004D1D RID: 19741 RVA: 0x0017FBD6 File Offset: 0x0017DDD6
			// (set) Token: 0x06004D1E RID: 19742 RVA: 0x0017FBDE File Offset: 0x0017DDDE
			public float Build { get; private set; }

			// Token: 0x17000F07 RID: 3847
			// (get) Token: 0x06004D1F RID: 19743 RVA: 0x0017FBE7 File Offset: 0x0017DDE7
			// (set) Token: 0x06004D20 RID: 19744 RVA: 0x0017FBEF File Offset: 0x0017DDEF
			public StaticBodyProperties? StaticBodyProperties { get; private set; }

			// Token: 0x17000F08 RID: 3848
			// (get) Token: 0x06004D21 RID: 19745 RVA: 0x0017FBF8 File Offset: 0x0017DDF8
			// (set) Token: 0x06004D22 RID: 19746 RVA: 0x0017FC00 File Offset: 0x0017DE00
			public FormationClass? PreferredUpgradeFormation { get; private set; }

			// Token: 0x17000F09 RID: 3849
			// (get) Token: 0x06004D23 RID: 19747 RVA: 0x0017FC09 File Offset: 0x0017DE09
			// (set) Token: 0x06004D24 RID: 19748 RVA: 0x0017FC11 File Offset: 0x0017DE11
			public Clan Clan { get; private set; }

			// Token: 0x17000F0A RID: 3850
			// (get) Token: 0x06004D25 RID: 19749 RVA: 0x0017FC1A File Offset: 0x0017DE1A
			// (set) Token: 0x06004D26 RID: 19750 RVA: 0x0017FC22 File Offset: 0x0017DE22
			public CultureObject Culture { get; private set; }

			// Token: 0x17000F0B RID: 3851
			// (get) Token: 0x06004D27 RID: 19751 RVA: 0x0017FC2B File Offset: 0x0017DE2B
			// (set) Token: 0x06004D28 RID: 19752 RVA: 0x0017FC33 File Offset: 0x0017DE33
			public Clan SupporterOf { get; private set; }

			// Token: 0x17000F0C RID: 3852
			// (get) Token: 0x06004D29 RID: 19753 RVA: 0x0017FC3C File Offset: 0x0017DE3C
			// (set) Token: 0x06004D2A RID: 19754 RVA: 0x0017FC44 File Offset: 0x0017DE44
			public Occupation Occupation { get; private set; }

			// Token: 0x17000F0D RID: 3853
			// (get) Token: 0x06004D2B RID: 19755 RVA: 0x0017FC4D File Offset: 0x0017DE4D
			// (set) Token: 0x06004D2C RID: 19756 RVA: 0x0017FC55 File Offset: 0x0017DE55
			public bool IsOffspring { get; private set; }

			// Token: 0x17000F0E RID: 3854
			// (get) Token: 0x06004D2D RID: 19757 RVA: 0x0017FC5E File Offset: 0x0017DE5E
			// (set) Token: 0x06004D2E RID: 19758 RVA: 0x0017FC66 File Offset: 0x0017DE66
			public bool GenerateFirstAndFullName { get; private set; }

			// Token: 0x17000F0F RID: 3855
			// (get) Token: 0x06004D2F RID: 19759 RVA: 0x0017FC6F File Offset: 0x0017DE6F
			// (set) Token: 0x06004D30 RID: 19760 RVA: 0x0017FC77 File Offset: 0x0017DE77
			public bool HasBornSettlementBeenSet { get; private set; }

			// Token: 0x17000F10 RID: 3856
			// (get) Token: 0x06004D31 RID: 19761 RVA: 0x0017FC80 File Offset: 0x0017DE80
			// (set) Token: 0x06004D32 RID: 19762 RVA: 0x0017FC88 File Offset: 0x0017DE88
			public bool HasClanBeenSet { get; private set; }

			// Token: 0x06004D33 RID: 19763 RVA: 0x0017FC94 File Offset: 0x0017DE94
			public HeroInitializationArgs(Hero hero, bool isOffspring)
			{
				DynamicBodyProperties dynamicBodyPropertiesBetweenMinMaxRange = CharacterHelper.GetDynamicBodyPropertiesBetweenMinMaxRange(hero.CharacterObject);
				this.Hero = hero;
				this.IsOffspring = isOffspring;
				this.Name = hero.Name;
				this.FirstName = hero.FirstName;
				this.Mother = hero.Mother;
				this.Father = hero.Father;
				this.IsFemale = hero.IsFemale;
				this.BornSettlement = null;
				this.Level = hero.Level;
				this.Weight = dynamicBodyPropertiesBetweenMinMaxRange.Weight;
				this.Build = dynamicBodyPropertiesBetweenMinMaxRange.Build;
				this.StaticBodyProperties = null;
				this.PreferredUpgradeFormation = null;
				this.Clan = null;
				this.SupporterOf = hero.SupporterOf;
				this.Occupation = hero.Occupation;
				this.Culture = null;
			}

			// Token: 0x06004D34 RID: 19764 RVA: 0x0017FD6C File Offset: 0x0017DF6C
			public HeroCreator.HeroInitializationArgs SetGenerateFirstAndFullName(bool value)
			{
				this.GenerateFirstAndFullName = value;
				return this;
			}

			// Token: 0x06004D35 RID: 19765 RVA: 0x0017FD76 File Offset: 0x0017DF76
			public HeroCreator.HeroInitializationArgs SetName(TextObject name)
			{
				this.Name = name;
				return this;
			}

			// Token: 0x06004D36 RID: 19766 RVA: 0x0017FD80 File Offset: 0x0017DF80
			public HeroCreator.HeroInitializationArgs SetFirstName(TextObject firstName)
			{
				this.FirstName = firstName;
				return this;
			}

			// Token: 0x06004D37 RID: 19767 RVA: 0x0017FD8A File Offset: 0x0017DF8A
			public HeroCreator.HeroInitializationArgs SetMother(Hero mother)
			{
				this.Mother = mother;
				return this;
			}

			// Token: 0x06004D38 RID: 19768 RVA: 0x0017FD94 File Offset: 0x0017DF94
			public HeroCreator.HeroInitializationArgs SetFather(Hero father)
			{
				this.Father = father;
				return this;
			}

			// Token: 0x06004D39 RID: 19769 RVA: 0x0017FD9E File Offset: 0x0017DF9E
			public HeroCreator.HeroInitializationArgs SetIsFemale(bool isFemale)
			{
				this.IsFemale = isFemale;
				return this;
			}

			// Token: 0x06004D3A RID: 19770 RVA: 0x0017FDA8 File Offset: 0x0017DFA8
			public HeroCreator.HeroInitializationArgs SetBornSettlement(Settlement bornSettlement)
			{
				this.BornSettlement = bornSettlement;
				this.HasBornSettlementBeenSet = true;
				return this;
			}

			// Token: 0x06004D3B RID: 19771 RVA: 0x0017FDB9 File Offset: 0x0017DFB9
			public HeroCreator.HeroInitializationArgs SetLevel(int level)
			{
				this.Level = level;
				return this;
			}

			// Token: 0x06004D3C RID: 19772 RVA: 0x0017FDC4 File Offset: 0x0017DFC4
			public HeroCreator.HeroInitializationArgs SetAppearance(StaticBodyProperties? staticBodyProperties, float weight = -1f, float build = -1f, int hair = -1, int beard = -1, int tattoo = -1)
			{
				if (weight > 0f)
				{
					this.Weight = weight;
				}
				if (build > 0f)
				{
					this.Build = build;
				}
				BodyProperties bodyProperties = new BodyProperties(new DynamicBodyProperties(this.Hero.Age, this.Weight, this.Build), staticBodyProperties ?? default(StaticBodyProperties));
				FaceGen.SetHair(ref bodyProperties, hair, beard, tattoo);
				this.StaticBodyProperties = new StaticBodyProperties?(bodyProperties.StaticProperties);
				return this;
			}

			// Token: 0x06004D3D RID: 19773 RVA: 0x0017FE4F File Offset: 0x0017E04F
			public HeroCreator.HeroInitializationArgs SetPreferredUpgradeFormation(FormationClass preferredUpgradeFormation)
			{
				this.PreferredUpgradeFormation = new FormationClass?(preferredUpgradeFormation);
				return this;
			}

			// Token: 0x06004D3E RID: 19774 RVA: 0x0017FE5E File Offset: 0x0017E05E
			public HeroCreator.HeroInitializationArgs SetClan(Clan clan)
			{
				this.Clan = clan;
				this.HasClanBeenSet = true;
				return this;
			}

			// Token: 0x06004D3F RID: 19775 RVA: 0x0017FE6F File Offset: 0x0017E06F
			public HeroCreator.HeroInitializationArgs SetCulture(CultureObject culture)
			{
				this.Culture = culture;
				return this;
			}

			// Token: 0x06004D40 RID: 19776 RVA: 0x0017FE79 File Offset: 0x0017E079
			public HeroCreator.HeroInitializationArgs SetSupporterOf(Clan supporterOf)
			{
				this.SupporterOf = supporterOf;
				return this;
			}

			// Token: 0x06004D41 RID: 19777 RVA: 0x0017FE83 File Offset: 0x0017E083
			public HeroCreator.HeroInitializationArgs SetOccupation(Occupation occupation)
			{
				this.Occupation = occupation;
				return this;
			}
		}
	}
}
