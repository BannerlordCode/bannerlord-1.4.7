using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000249 RID: 585
	public class PlayerIsAffiliatedTag : ConversationTag
	{
		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06002318 RID: 8984 RVA: 0x0009ACCE File Offset: 0x00098ECE
		public override string StringId
		{
			get
			{
				return "PlayerIsAffiliatedTag";
			}
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x0009ACD5 File Offset: 0x00098ED5
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Hero.MainHero.MapFaction.IsKingdomFaction;
		}

		// Token: 0x04000A7F RID: 2687
		public const string Id = "PlayerIsAffiliatedTag";
	}
}
