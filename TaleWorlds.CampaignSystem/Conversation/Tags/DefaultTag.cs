using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200023D RID: 573
	public class DefaultTag : ConversationTag
	{
		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x060022F4 RID: 8948 RVA: 0x0009AA18 File Offset: 0x00098C18
		public override string StringId
		{
			get
			{
				return "DefaultTag";
			}
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x0009AA1F File Offset: 0x00098C1F
		public override bool IsApplicableTo(CharacterObject character)
		{
			return true;
		}

		// Token: 0x04000A73 RID: 2675
		public const string Id = "DefaultTag";
	}
}
