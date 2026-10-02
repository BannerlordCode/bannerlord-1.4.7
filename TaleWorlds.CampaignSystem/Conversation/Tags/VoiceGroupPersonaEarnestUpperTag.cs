using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000291 RID: 657
	public class VoiceGroupPersonaEarnestUpperTag : ConversationTag
	{
		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x060023F0 RID: 9200 RVA: 0x0009BD14 File Offset: 0x00099F14
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaEarnestUpperTag";
			}
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x0009BD1B File Offset: 0x00099F1B
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaEarnest && ConversationTagHelper.UsesHighRegister(character);
		}

		// Token: 0x04000AC8 RID: 2760
		public const string Id = "VoiceGroupPersonaEarnestUpperTag";
	}
}
