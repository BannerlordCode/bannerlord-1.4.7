using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200029B RID: 667
	public class VoiceGroupPersonaSoftspokenLowerTag : ConversationTag
	{
		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x0600240E RID: 9230 RVA: 0x0009BE90 File Offset: 0x0009A090
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaSoftspokenLowerTag";
			}
		}

		// Token: 0x0600240F RID: 9231 RVA: 0x0009BE97 File Offset: 0x0009A097
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaSoftspoken && ConversationTagHelper.UsesLowRegister(character);
		}

		// Token: 0x04000AD2 RID: 2770
		public const string Id = "VoiceGroupPersonaSoftspokenLowerTag";
	}
}
