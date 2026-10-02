using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200026F RID: 623
	public class PreacherNotableTypeTag : ConversationTag
	{
		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x0600238A RID: 9098 RVA: 0x0009B6C6 File Offset: 0x000998C6
		public override string StringId
		{
			get
			{
				return "PreacherNotableTypeTag";
			}
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x0009B6CD File Offset: 0x000998CD
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.Occupation == Occupation.Preacher;
		}

		// Token: 0x04000AA6 RID: 2726
		public const string Id = "PreacherNotableTypeTag";
	}
}
