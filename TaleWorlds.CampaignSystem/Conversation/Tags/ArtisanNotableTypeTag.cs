using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000273 RID: 627
	public class ArtisanNotableTypeTag : ConversationTag
	{
		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06002396 RID: 9110 RVA: 0x0009B75A File Offset: 0x0009995A
		public override string StringId
		{
			get
			{
				return "ArtisanNotableTypeTag";
			}
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x0009B761 File Offset: 0x00099961
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.Occupation == Occupation.Artisan;
		}

		// Token: 0x04000AAA RID: 2730
		public const string Id = "ArtisanNotableTypeTag";
	}
}
