using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000284 RID: 644
	public class CalculatingTag : ConversationTag
	{
		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x060023C9 RID: 9161 RVA: 0x0009BA67 File Offset: 0x00099C67
		public override string StringId
		{
			get
			{
				return "CalculatingTag";
			}
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x0009BA6E File Offset: 0x00099C6E
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) > 0;
		}

		// Token: 0x04000ABB RID: 2747
		public const string Id = "CalculatingTag";
	}
}
