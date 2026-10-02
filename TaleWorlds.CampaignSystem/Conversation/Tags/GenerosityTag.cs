using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000280 RID: 640
	public class GenerosityTag : ConversationTag
	{
		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x060023BD RID: 9149 RVA: 0x0009B9AF File Offset: 0x00099BAF
		public override string StringId
		{
			get
			{
				return "GenerosityTag";
			}
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x0009B9B6 File Offset: 0x00099BB6
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) > 0;
		}

		// Token: 0x04000AB7 RID: 2743
		public const string Id = "GenerosityTag";
	}
}
