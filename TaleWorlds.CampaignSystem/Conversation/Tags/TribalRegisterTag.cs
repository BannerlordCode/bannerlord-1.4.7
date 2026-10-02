using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000246 RID: 582
	public class TribalRegisterTag : ConversationTag
	{
		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x0600230F RID: 8975 RVA: 0x0009AC40 File Offset: 0x00098E40
		public override string StringId
		{
			get
			{
				return "TribalRegisterTag";
			}
		}

		// Token: 0x06002310 RID: 8976 RVA: 0x0009AC47 File Offset: 0x00098E47
		public override bool IsApplicableTo(CharacterObject character)
		{
			return !ConversationTagHelper.UsesHighRegister(character) && !ConversationTagHelper.UsesLowRegister(character);
		}

		// Token: 0x04000A7C RID: 2684
		public const string Id = "TribalRegisterTag";
	}
}
