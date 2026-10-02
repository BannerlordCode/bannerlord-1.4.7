using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000295 RID: 661
	public class VoiceGroupPersonaCurtLowerTag : ConversationTag
	{
		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x060023FC RID: 9212 RVA: 0x0009BDAC File Offset: 0x00099FAC
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaCurtLowerTag";
			}
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x0009BDB3 File Offset: 0x00099FB3
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaCurt && ConversationTagHelper.UsesLowRegister(character);
		}

		// Token: 0x04000ACC RID: 2764
		public const string Id = "VoiceGroupPersonaCurtLowerTag";
	}
}
