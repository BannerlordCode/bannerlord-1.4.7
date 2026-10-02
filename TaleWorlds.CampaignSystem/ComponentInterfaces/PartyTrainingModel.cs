using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000194 RID: 404
	public abstract class PartyTrainingModel : MBGameModel<PartyTrainingModel>
	{
		// Token: 0x06001C56 RID: 7254
		public abstract int GenerateSharedXp(CharacterObject troop, int xp, MobileParty mobileParty);

		// Token: 0x06001C57 RID: 7255
		public abstract ExplainedNumber CalculateXpGainFromBattles(FlattenedTroopRosterElement troopRosterElement, PartyBase party);

		// Token: 0x06001C58 RID: 7256
		public abstract int GetXpReward(CharacterObject character);

		// Token: 0x06001C59 RID: 7257
		public abstract ExplainedNumber GetEffectiveDailyExperience(MobileParty party, TroopRosterElement troop);
	}
}
