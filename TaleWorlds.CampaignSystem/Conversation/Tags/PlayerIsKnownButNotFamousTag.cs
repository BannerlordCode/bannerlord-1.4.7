using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000253 RID: 595
	public class PlayerIsKnownButNotFamousTag : ConversationTag
	{
		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06002336 RID: 9014 RVA: 0x0009AF23 File Offset: 0x00099123
		public override string StringId
		{
			get
			{
				return "PlayerIsKnownButNotFamousTag";
			}
		}

		// Token: 0x06002337 RID: 9015 RVA: 0x0009AF2C File Offset: 0x0009912C
		public override bool IsApplicableTo(CharacterObject character)
		{
			int num = Campaign.Current.Models.DiplomacyModel.GetBaseRelation(Hero.MainHero, Hero.OneToOneConversationHero);
			if (Hero.OneToOneConversationHero.Clan != null && num == 0)
			{
				num = Campaign.Current.Models.DiplomacyModel.GetBaseRelation(Hero.MainHero, Hero.OneToOneConversationHero.Clan.Leader);
			}
			return num != 0 && Clan.PlayerClan.Renown < 50f && Campaign.Current.ConversationManager.CurrentConversationIsFirst;
		}

		// Token: 0x04000A89 RID: 2697
		public const string Id = "PlayerIsKnownButNotFamousTag";
	}
}
