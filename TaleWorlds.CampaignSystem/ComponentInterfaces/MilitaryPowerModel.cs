using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001E2 RID: 482
	public abstract class MilitaryPowerModel : MBGameModel<MilitaryPowerModel>
	{
		// Token: 0x06001ECE RID: 7886
		public abstract float GetTroopPower(CharacterObject troop, BattleSideEnum side, MapEvent.PowerCalculationContext context, float leaderModifier);

		// Token: 0x06001ECF RID: 7887
		public abstract float GetPowerOfParty(PartyBase party, BattleSideEnum side, MapEvent.PowerCalculationContext context);

		// Token: 0x06001ED0 RID: 7888
		public abstract float GetContextModifier(CharacterObject troop, BattleSideEnum battleSideEnum, MapEvent.PowerCalculationContext context);

		// Token: 0x06001ED1 RID: 7889
		public abstract float GetContextModifier(Ship ship, BattleSideEnum battleSideEnum, MapEvent.PowerCalculationContext context);

		// Token: 0x06001ED2 RID: 7890
		public abstract MapEvent.PowerCalculationContext GetContextForPosition(CampaignVec2 position);

		// Token: 0x06001ED3 RID: 7891
		public abstract float GetDefaultTroopPower(CharacterObject troop);

		// Token: 0x06001ED4 RID: 7892
		public abstract float GetPowerModifierOfHero(Hero leaderHero);
	}
}
