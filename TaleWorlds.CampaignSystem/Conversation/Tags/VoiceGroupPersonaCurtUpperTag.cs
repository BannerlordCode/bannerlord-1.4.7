using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000294 RID: 660
	public class VoiceGroupPersonaCurtUpperTag : ConversationTag
	{
		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x060023F9 RID: 9209 RVA: 0x0009BD86 File Offset: 0x00099F86
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaCurtUpperTag";
			}
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x0009BD8D File Offset: 0x00099F8D
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaCurt && ConversationTagHelper.UsesHighRegister(character);
		}

		// Token: 0x04000ACB RID: 2763
		public const string Id = "VoiceGroupPersonaCurtUpperTag";
	}
}
