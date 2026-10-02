using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000277 RID: 631
	public class NonviolentProfessionTag : ConversationTag
	{
		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x060023A2 RID: 9122 RVA: 0x0009B834 File Offset: 0x00099A34
		public override string StringId
		{
			get
			{
				return "NonviolentProfessionTag";
			}
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x0009B83B File Offset: 0x00099A3B
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && (character.Occupation == Occupation.Artisan || character.Occupation == Occupation.Merchant || character.Occupation == Occupation.Headman);
		}

		// Token: 0x04000AAE RID: 2734
		public const string Id = "NonviolentProfessionTag";
	}
}
