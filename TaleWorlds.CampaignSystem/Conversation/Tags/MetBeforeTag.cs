using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000265 RID: 613
	public class MetBeforeTag : ConversationTag
	{
		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x0600236C RID: 9068 RVA: 0x0009B438 File Offset: 0x00099638
		public override string StringId
		{
			get
			{
				return "MetBeforeTag";
			}
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x0009B43F File Offset: 0x0009963F
		public override bool IsApplicableTo(CharacterObject character)
		{
			return !Campaign.Current.ConversationManager.CurrentConversationIsFirst;
		}

		// Token: 0x04000A9B RID: 2715
		public const string Id = "MetBeforeTag";
	}
}
