using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000296 RID: 662
	public class VoiceGroupPersonaIronicTribalTag : ConversationTag
	{
		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x060023FF RID: 9215 RVA: 0x0009BDD2 File Offset: 0x00099FD2
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaIronicTribalTag";
			}
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x0009BDD9 File Offset: 0x00099FD9
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaIronic && ConversationTagHelper.TribalVoiceGroup(character);
		}

		// Token: 0x04000ACD RID: 2765
		public const string Id = "VoiceGroupPersonaIronicTribalTag";
	}
}
