using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000298 RID: 664
	public class VoiceGroupPersonaIronicLowerTag : ConversationTag
	{
		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06002405 RID: 9221 RVA: 0x0009BE1E File Offset: 0x0009A01E
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaIronicLowerTag";
			}
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x0009BE25 File Offset: 0x0009A025
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaIronic && ConversationTagHelper.UsesLowRegister(character);
		}

		// Token: 0x04000ACF RID: 2767
		public const string Id = "VoiceGroupPersonaIronicLowerTag";
	}
}
