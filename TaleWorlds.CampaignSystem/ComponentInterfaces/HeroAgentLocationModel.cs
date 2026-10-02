using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001CF RID: 463
	public abstract class HeroAgentLocationModel : MBGameModel<HeroAgentLocationModel>
	{
		// Token: 0x06001E5C RID: 7772
		public abstract bool WillBeListedInOverlay(LocationCharacter locationCharacter);

		// Token: 0x06001E5D RID: 7773
		public abstract Location GetLocationForHero(Hero hero, Settlement settlement, out HeroAgentLocationModel.HeroLocationDetail heroSpawnDetail);

		// Token: 0x02000604 RID: 1540
		public enum HeroLocationDetail
		{
			// Token: 0x04001916 RID: 6422
			None,
			// Token: 0x04001917 RID: 6423
			SettlementKingQueen,
			// Token: 0x04001918 RID: 6424
			NobleBelongingToNoParty,
			// Token: 0x04001919 RID: 6425
			Prisoner,
			// Token: 0x0400191A RID: 6426
			PlayerClanMember,
			// Token: 0x0400191B RID: 6427
			MainPartyCompanion,
			// Token: 0x0400191C RID: 6428
			Notable,
			// Token: 0x0400191D RID: 6429
			Wanderer,
			// Token: 0x0400191E RID: 6430
			PartyLeader,
			// Token: 0x0400191F RID: 6431
			PartylessHeroInsideVillage
		}
	}
}
