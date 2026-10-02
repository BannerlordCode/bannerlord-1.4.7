using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200029A RID: 666
	public class VoiceGroupPersonaSoftspokenUpperTag : ConversationTag
	{
		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x0600240B RID: 9227 RVA: 0x0009BE6A File Offset: 0x0009A06A
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaSoftspokenUpperTag";
			}
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x0009BE71 File Offset: 0x0009A071
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaSoftspoken && ConversationTagHelper.UsesHighRegister(character);
		}

		// Token: 0x04000AD1 RID: 2769
		public const string Id = "VoiceGroupPersonaSoftspokenUpperTag";
	}
}
