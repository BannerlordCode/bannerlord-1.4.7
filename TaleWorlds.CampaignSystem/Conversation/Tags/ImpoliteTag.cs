using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000263 RID: 611
	public class ImpoliteTag : ConversationTag
	{
		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06002366 RID: 9062 RVA: 0x0009B386 File Offset: 0x00099586
		public override string StringId
		{
			get
			{
				return "ImpoliteTag";
			}
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x0009B390 File Offset: 0x00099590
		public override bool IsApplicableTo(CharacterObject character)
		{
			if (!character.IsHero)
			{
				return false;
			}
			int heroRelation = CharacterRelationManager.GetHeroRelation(character.HeroObject, Hero.MainHero);
			return (character.HeroObject.IsLord || character.HeroObject.IsMerchant || character.HeroObject.IsGangLeader) && Clan.PlayerClan.Renown < 100f && heroRelation < 1 && character.GetTraitLevel(DefaultTraits.Mercy) + character.GetTraitLevel(DefaultTraits.Generosity) < 0;
		}

		// Token: 0x04000A99 RID: 2713
		public const string Id = "ImpoliteTag";
	}
}
