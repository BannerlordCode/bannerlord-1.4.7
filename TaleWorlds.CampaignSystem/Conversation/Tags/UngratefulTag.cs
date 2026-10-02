using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000281 RID: 641
	public class UngratefulTag : ConversationTag
	{
		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x060023C0 RID: 9152 RVA: 0x0009B9DD File Offset: 0x00099BDD
		public override string StringId
		{
			get
			{
				return "UngratefulTag";
			}
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x0009B9E4 File Offset: 0x00099BE4
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) < 0;
		}

		// Token: 0x04000AB8 RID: 2744
		public const string Id = "UngratefulTag";
	}
}
