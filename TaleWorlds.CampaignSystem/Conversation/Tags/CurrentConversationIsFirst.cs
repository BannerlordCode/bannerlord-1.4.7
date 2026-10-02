using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000264 RID: 612
	public class CurrentConversationIsFirst : ConversationTag
	{
		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06002369 RID: 9065 RVA: 0x0009B418 File Offset: 0x00099618
		public override string StringId
		{
			get
			{
				return "CurrentConversationIsFirst";
			}
		}

		// Token: 0x0600236A RID: 9066 RVA: 0x0009B41F File Offset: 0x0009961F
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Campaign.Current.ConversationManager.CurrentConversationIsFirst;
		}

		// Token: 0x04000A9A RID: 2714
		public const string Id = "CurrentConversationIsFirst";
	}
}
