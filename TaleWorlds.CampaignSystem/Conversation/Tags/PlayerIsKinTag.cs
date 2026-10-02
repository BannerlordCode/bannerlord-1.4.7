using System;
using System.Linq;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000251 RID: 593
	public class PlayerIsKinTag : ConversationTag
	{
		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06002330 RID: 9008 RVA: 0x0009AE87 File Offset: 0x00099087
		public override string StringId
		{
			get
			{
				return "PlayerIsKinTag";
			}
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x0009AE90 File Offset: 0x00099090
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && (character.HeroObject.Siblings.Contains(Hero.MainHero) || character.HeroObject.Mother == Hero.MainHero || character.HeroObject.Father == Hero.MainHero || character.HeroObject.Spouse == Hero.MainHero);
		}

		// Token: 0x04000A87 RID: 2695
		public const string Id = "PlayerIsKinTag";
	}
}
