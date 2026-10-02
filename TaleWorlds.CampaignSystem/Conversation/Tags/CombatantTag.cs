using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000240 RID: 576
	public class CombatantTag : ConversationTag
	{
		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x060022FD RID: 8957 RVA: 0x0009AA76 File Offset: 0x00098C76
		public override string StringId
		{
			get
			{
				return "CombatantTag";
			}
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x0009AA7D File Offset: 0x00098C7D
		public override bool IsApplicableTo(CharacterObject character)
		{
			return !character.IsHero || !character.HeroObject.IsNoncombatant;
		}

		// Token: 0x04000A76 RID: 2678
		public const string Id = "CombatantTag";
	}
}
