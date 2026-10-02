using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200028E RID: 654
	public class PersonaIronicTag : ConversationTag
	{
		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x060023E7 RID: 9191 RVA: 0x0009BC9E File Offset: 0x00099E9E
		public override string StringId
		{
			get
			{
				return "PersonaIronicTag";
			}
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x0009BCA5 File Offset: 0x00099EA5
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.GetPersona() == DefaultTraits.PersonaIronic;
		}

		// Token: 0x04000AC5 RID: 2757
		public const string Id = "PersonaIronicTag";
	}
}
