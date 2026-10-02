using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001FC RID: 508
	public abstract class HeroCreationModel : MBGameModel<HeroCreationModel>
	{
		// Token: 0x06001F8D RID: 8077
		[return: TupleElementNames(new string[] { "birthDay", "deathDay" })]
		public abstract ValueTuple<CampaignTime, CampaignTime> GetBirthAndDeathDay(CharacterObject character, bool createAlive, int age);

		// Token: 0x06001F8E RID: 8078
		public abstract Settlement GetBornSettlement(Hero character);

		// Token: 0x06001F8F RID: 8079
		public abstract StaticBodyProperties GetStaticBodyProperties(Hero character, bool isOffspring, float variationAmount = 0.35f);

		// Token: 0x06001F90 RID: 8080
		public abstract FormationClass GetPreferredUpgradeFormation(Hero character);

		// Token: 0x06001F91 RID: 8081
		public abstract Clan GetClan(Hero character);

		// Token: 0x06001F92 RID: 8082
		public abstract CultureObject GetCulture(Hero hero, Settlement bornSettlement, Clan clan);

		// Token: 0x06001F93 RID: 8083
		public abstract CharacterObject GetRandomTemplateByOccupation(Occupation occupation, Settlement settlement = null);

		// Token: 0x06001F94 RID: 8084
		[return: TupleElementNames(new string[] { "trait", "level" })]
		public abstract List<ValueTuple<TraitObject, int>> GetTraitsForHero(Hero hero);

		// Token: 0x06001F95 RID: 8085
		public abstract Equipment GetCivilianEquipment(Hero hero);

		// Token: 0x06001F96 RID: 8086
		public abstract Equipment GetBattleEquipment(Hero hero);

		// Token: 0x06001F97 RID: 8087
		public abstract CharacterObject GetCharacterTemplateForOffspring(Hero mother, Hero father, bool isOffspringFemale);

		// Token: 0x06001F98 RID: 8088
		[return: TupleElementNames(new string[] { "firstName", "name" })]
		public abstract ValueTuple<TextObject, TextObject> GenerateFirstAndFullName(Hero hero);

		// Token: 0x06001F99 RID: 8089
		public abstract List<ValueTuple<SkillObject, int>> GetDefaultSkillsForHero(Hero hero);

		// Token: 0x06001F9A RID: 8090
		public abstract List<ValueTuple<SkillObject, int>> GetInheritedSkillsForHero(Hero hero);

		// Token: 0x06001F9B RID: 8091
		public abstract bool IsHeroCombatant(Hero hero);
	}
}
