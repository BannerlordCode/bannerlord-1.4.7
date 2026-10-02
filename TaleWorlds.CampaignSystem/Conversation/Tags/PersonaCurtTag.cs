using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200028D RID: 653
	public class PersonaCurtTag : ConversationTag
	{
		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x060023E4 RID: 9188 RVA: 0x0009BC76 File Offset: 0x00099E76
		public override string StringId
		{
			get
			{
				return "PersonaCurtTag";
			}
		}

		// Token: 0x060023E5 RID: 9189 RVA: 0x0009BC7D File Offset: 0x00099E7D
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.GetPersona() == DefaultTraits.PersonaCurt;
		}

		// Token: 0x04000AC4 RID: 2756
		public const string Id = "PersonaCurtTag";
	}
}
