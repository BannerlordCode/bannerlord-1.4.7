using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000270 RID: 624
	public class HeadmanNotableTypeTag : ConversationTag
	{
		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x0600238D RID: 9101 RVA: 0x0009B6EB File Offset: 0x000998EB
		public override string StringId
		{
			get
			{
				return "HeadmanNotableTypeTag";
			}
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x0009B6F2 File Offset: 0x000998F2
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.Occupation == Occupation.Headman;
		}

		// Token: 0x04000AA7 RID: 2727
		public const string Id = "HeadmanNotableTypeTag";
	}
}
