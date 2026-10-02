using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200025C RID: 604
	public class HostileRelationshipTag : ConversationTag
	{
		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06002351 RID: 9041 RVA: 0x0009B174 File Offset: 0x00099374
		public override string StringId
		{
			get
			{
				return "HostileRelationshipTag";
			}
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x0009B17C File Offset: 0x0009937C
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
			return (num + num2 + num3 < -1 && unmodifiedClanLeaderRelationshipWithPlayer <= -5f) || unmodifiedClanLeaderRelationshipWithPlayer <= -20f;
		}

		// Token: 0x04000A92 RID: 2706
		public const string Id = "HostileRelationshipTag";
	}
}
