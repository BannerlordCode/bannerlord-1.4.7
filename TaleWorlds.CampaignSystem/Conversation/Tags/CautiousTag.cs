using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000287 RID: 647
	public class CautiousTag : ConversationTag
	{
		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x060023D2 RID: 9170 RVA: 0x0009BAF1 File Offset: 0x00099CF1
		public override string StringId
		{
			get
			{
				return "CautiousTag";
			}
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x0009BAF8 File Offset: 0x00099CF8
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Valor) < 0;
		}

		// Token: 0x04000ABE RID: 2750
		public const string Id = "CautiousTag";
	}
}
