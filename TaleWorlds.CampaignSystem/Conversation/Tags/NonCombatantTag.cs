using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200023F RID: 575
	public class NonCombatantTag : ConversationTag
	{
		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x060022FA RID: 8954 RVA: 0x0009AA50 File Offset: 0x00098C50
		public override string StringId
		{
			get
			{
				return "NonCombatantTag";
			}
		}

		// Token: 0x060022FB RID: 8955 RVA: 0x0009AA57 File Offset: 0x00098C57
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.IsNoncombatant;
		}

		// Token: 0x04000A75 RID: 2677
		public const string Id = "NonCombatantTag";
	}
}
