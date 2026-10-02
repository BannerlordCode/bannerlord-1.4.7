using System;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000288 RID: 648
	public class PlayerIsAtSeaTag : ConversationTag
	{
		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x060023D5 RID: 9173 RVA: 0x0009BB1F File Offset: 0x00099D1F
		public override string StringId
		{
			get
			{
				return "PlayerIsAtSeaTag";
			}
		}

		// Token: 0x060023D6 RID: 9174 RVA: 0x0009BB28 File Offset: 0x00099D28
		public override bool IsApplicableTo(CharacterObject character)
		{
			MobileParty mobileParty = (Hero.MainHero.IsPrisoner ? Hero.MainHero.PartyBelongedToAsPrisoner.MobileParty : Hero.MainHero.PartyBelongedTo);
			MobileParty mobileParty2 = (character.HeroObject.IsPrisoner ? character.HeroObject.PartyBelongedToAsPrisoner.MobileParty : character.HeroObject.PartyBelongedTo);
			return mobileParty.IsCurrentlyAtSea && mobileParty2 != mobileParty;
		}

		// Token: 0x04000ABF RID: 2751
		public const string Id = "PlayerIsAtSeaTag";
	}
}
