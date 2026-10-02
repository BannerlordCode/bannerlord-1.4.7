using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200025B RID: 603
	public class FriendlyRelationshipTag : ConversationTag
	{
		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x0600234E RID: 9038 RVA: 0x0009B0E4 File Offset: 0x000992E4
		public override string StringId
		{
			get
			{
				return "FriendlyRelationshipTag";
			}
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x0009B0EC File Offset: 0x000992EC
		public override bool IsApplicableTo(CharacterObject character)
		{
			if (!character.IsHero)
			{
				return false;
			}
			float unmodifiedClanLeaderRelationshipWithPlayer = character.HeroObject.GetUnmodifiedClanLeaderRelationshipWithPlayer();
			int num = ConversationTagHelper.TraitCompatibility(character.HeroObject, Hero.MainHero, DefaultTraits.Mercy);
			int num2 = ConversationTagHelper.TraitCompatibility(character.HeroObject, Hero.MainHero, DefaultTraits.Honor);
			int num3 = ConversationTagHelper.TraitCompatibility(character.HeroObject, Hero.MainHero, DefaultTraits.Valor);
			return (num + num2 + num3 > 0 && unmodifiedClanLeaderRelationshipWithPlayer >= 5f) || unmodifiedClanLeaderRelationshipWithPlayer >= 20f;
		}

		// Token: 0x04000A91 RID: 2705
		public const string Id = "FriendlyRelationshipTag";
	}
}
