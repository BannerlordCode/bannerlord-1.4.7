using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200027B RID: 635
	public class KhuzaitTag : ConversationTag
	{
		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x060023AE RID: 9134 RVA: 0x0009B8E1 File Offset: 0x00099AE1
		public override string StringId
		{
			get
			{
				return "KhuzaitTag";
			}
		}

		// Token: 0x060023AF RID: 9135 RVA: 0x0009B8E8 File Offset: 0x00099AE8
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.Culture.StringId == "khuzait";
		}

		// Token: 0x04000AB2 RID: 2738
		public const string Id = "KhuzaitTag";
	}
}
