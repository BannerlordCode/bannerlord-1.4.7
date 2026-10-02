using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001ED RID: 493
	public abstract class PartyTroopUpgradeModel : MBGameModel<PartyTroopUpgradeModel>
	{
		// Token: 0x06001F21 RID: 7969
		public abstract bool CanPartyUpgradeTroopToTarget(PartyBase party, CharacterObject character, CharacterObject target);

		// Token: 0x06001F22 RID: 7970
		public abstract bool IsTroopUpgradeable(PartyBase party, CharacterObject character);

		// Token: 0x06001F23 RID: 7971
		public abstract bool DoesPartyHaveRequiredItemsForUpgrade(PartyBase party, CharacterObject upgradeTarget);

		// Token: 0x06001F24 RID: 7972
		public abstract bool DoesPartyHaveRequiredPerksForUpgrade(PartyBase party, CharacterObject character, CharacterObject upgradeTarget, out PerkObject requiredPerk);

		// Token: 0x06001F25 RID: 7973
		public abstract ExplainedNumber GetGoldCostForUpgrade(PartyBase party, CharacterObject characterObject, CharacterObject upgradeTarget);

		// Token: 0x06001F26 RID: 7974
		public abstract int GetXpCostForUpgrade(PartyBase party, CharacterObject characterObject, CharacterObject upgradeTarget);

		// Token: 0x06001F27 RID: 7975
		public abstract int GetSkillXpFromUpgradingTroops(PartyBase party, CharacterObject troop, int numberOfTroops);

		// Token: 0x06001F28 RID: 7976
		public abstract float GetUpgradeChanceForTroopUpgrade(PartyBase party, CharacterObject troop, int upgradeTargetIndex);
	}
}
