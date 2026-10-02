using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200028B RID: 651
	public class RogueSkillsTag : ConversationTag
	{
		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x060023DE RID: 9182 RVA: 0x0009BC20 File Offset: 0x00099E20
		public override string StringId
		{
			get
			{
				return "RogueSkillsTag";
			}
		}

		// Token: 0x060023DF RID: 9183 RVA: 0x0009BC27 File Offset: 0x00099E27
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.RogueSkills) > 0;
		}

		// Token: 0x04000AC2 RID: 2754
		public const string Id = "RogueSkillsTag";
	}
}
