using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000289 RID: 649
	public class NordTag : ConversationTag
	{
		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x060023D8 RID: 9176 RVA: 0x0009BBA1 File Offset: 0x00099DA1
		public override string StringId
		{
			get
			{
				return "NordTag";
			}
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x0009BBA8 File Offset: 0x00099DA8
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.Culture.StringId == "nord";
		}

		// Token: 0x04000AC0 RID: 2752
		public const string Id = "NordTag";
	}
}
