using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000256 RID: 598
	public class NpcIsFemaleTag : ConversationTag
	{
		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x0600233F RID: 9023 RVA: 0x0009B01E File Offset: 0x0009921E
		public override string StringId
		{
			get
			{
				return "NpcIsFemaleTag";
			}
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x0009B025 File Offset: 0x00099225
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsFemale;
		}

		// Token: 0x04000A8C RID: 2700
		public const string Id = "NpcIsFemaleTag";
	}
}
