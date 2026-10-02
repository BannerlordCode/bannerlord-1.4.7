using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200027F RID: 639
	public class CruelTag : ConversationTag
	{
		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x060023BA RID: 9146 RVA: 0x0009B981 File Offset: 0x00099B81
		public override string StringId
		{
			get
			{
				return "CruelTag";
			}
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x0009B988 File Offset: 0x00099B88
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) < 0;
		}

		// Token: 0x04000AB6 RID: 2742
		public const string Id = "CruelTag";
	}
}
