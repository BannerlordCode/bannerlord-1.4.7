using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000243 RID: 579
	public class OutlawSympathyTag : ConversationTag
	{
		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06002306 RID: 8966 RVA: 0x0009ABBB File Offset: 0x00098DBB
		public override string StringId
		{
			get
			{
				return "OutlawSympathyTag";
			}
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x0009ABC2 File Offset: 0x00098DC2
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.IsWanderer && character.HeroObject.GetTraitLevel(DefaultTraits.RogueSkills) > 0;
		}

		// Token: 0x04000A79 RID: 2681
		public const string Id = "OutlawSympathyTag";
	}
}
