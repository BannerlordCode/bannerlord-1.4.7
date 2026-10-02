using System;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000261 RID: 609
	public class SexistTag : ConversationTag
	{
		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06002360 RID: 9056 RVA: 0x0009B2C1 File Offset: 0x000994C1
		public override string StringId
		{
			get
			{
				return "SexistTag";
			}
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x0009B2C8 File Offset: 0x000994C8
		public override bool IsApplicableTo(CharacterObject character)
		{
			bool flag = character.HeroObject.Clan.Heroes.Any<Hero>((Hero x) => x.IsFemale && x.IsCommander);
			int num = character.GetTraitLevel(DefaultTraits.Calculating) + character.GetTraitLevel(DefaultTraits.Mercy);
			int num2 = character.GetTraitLevel(DefaultTraits.Valor) + character.GetTraitLevel(DefaultTraits.Generosity);
			return num < 0 && num2 <= 0 && !flag;
		}

		// Token: 0x04000A97 RID: 2711
		public const string Id = "SexistTag";
	}
}
