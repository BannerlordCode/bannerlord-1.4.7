using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200028F RID: 655
	public class PersonaSoftspokenTag : ConversationTag
	{
		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x060023EA RID: 9194 RVA: 0x0009BCC6 File Offset: 0x00099EC6
		public override string StringId
		{
			get
			{
				return "PersonaSoftspokenTag";
			}
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x0009BCCD File Offset: 0x00099ECD
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.GetPersona() == DefaultTraits.PersonaSoftspoken;
		}

		// Token: 0x04000AC6 RID: 2758
		public const string Id = "PersonaSoftspokenTag";
	}
}
