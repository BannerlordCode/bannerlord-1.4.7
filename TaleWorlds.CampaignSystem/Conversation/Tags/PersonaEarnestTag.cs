using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200028C RID: 652
	public class PersonaEarnestTag : ConversationTag
	{
		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x060023E1 RID: 9185 RVA: 0x0009BC4E File Offset: 0x00099E4E
		public override string StringId
		{
			get
			{
				return "PersonaEarnestTag";
			}
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x0009BC55 File Offset: 0x00099E55
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.GetPersona() == DefaultTraits.PersonaEarnest;
		}

		// Token: 0x04000AC3 RID: 2755
		public const string Id = "PersonaEarnestTag";
	}
}
