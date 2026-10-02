using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000197 RID: 407
	public abstract class PartyHealingModel : MBGameModel<PartyHealingModel>
	{
		// Token: 0x06001C66 RID: 7270
		public abstract float GetSurgeryChance(PartyBase party);

		// Token: 0x06001C67 RID: 7271
		public abstract float GetSurvivalChance(PartyBase party, CharacterObject agentCharacter, DamageTypes damageType, bool canDamageKillEvenIfBlunt, PartyBase enemyParty = null);

		// Token: 0x06001C68 RID: 7272
		public abstract int GetSkillXpFromHealingTroop(PartyBase party);

		// Token: 0x06001C69 RID: 7273
		public abstract ExplainedNumber GetDailyHealingForRegulars(PartyBase partyBase, bool isPrisoner, bool includeDescriptions = false);

		// Token: 0x06001C6A RID: 7274
		public abstract ExplainedNumber GetDailyHealingHpForHeroes(PartyBase partyBase, bool isPrisoners, bool includeDescriptions = false);

		// Token: 0x06001C6B RID: 7275
		public abstract int GetHeroesEffectedHealingAmount(Hero hero, float healingRate);

		// Token: 0x06001C6C RID: 7276
		public abstract float GetSiegeBombardmentHitSurgeryChance(PartyBase party);

		// Token: 0x06001C6D RID: 7277
		public abstract ExplainedNumber GetBattleEndHealingAmount(PartyBase partyBase, Hero hero);
	}
}
