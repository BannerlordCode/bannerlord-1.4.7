using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200027C RID: 636
	public class AseraiTag : ConversationTag
	{
		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x060023B1 RID: 9137 RVA: 0x0009B907 File Offset: 0x00099B07
		public override string StringId
		{
			get
			{
				return "AseraiTag";
			}
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x0009B90E File Offset: 0x00099B0E
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.Culture.StringId == "aserai";
		}

		// Token: 0x04000AB3 RID: 2739
		public const string Id = "AseraiTag";
	}
}
