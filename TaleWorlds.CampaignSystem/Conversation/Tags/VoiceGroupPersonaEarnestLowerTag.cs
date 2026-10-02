using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000292 RID: 658
	public class VoiceGroupPersonaEarnestLowerTag : ConversationTag
	{
		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x060023F3 RID: 9203 RVA: 0x0009BD3A File Offset: 0x00099F3A
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaEarnestLowerTag";
			}
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x0009BD41 File Offset: 0x00099F41
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaEarnest && ConversationTagHelper.UsesLowRegister(character);
		}

		// Token: 0x04000AC9 RID: 2761
		public const string Id = "VoiceGroupPersonaEarnestLowerTag";
	}
}
