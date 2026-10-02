using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000286 RID: 646
	public class ValorTag : ConversationTag
	{
		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x060023CF RID: 9167 RVA: 0x0009BAC3 File Offset: 0x00099CC3
		public override string StringId
		{
			get
			{
				return "ValorTag";
			}
		}

		// Token: 0x060023D0 RID: 9168 RVA: 0x0009BACA File Offset: 0x00099CCA
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Valor) > 0;
		}

		// Token: 0x04000ABD RID: 2749
		public const string Id = "ValorTag";
	}
}
