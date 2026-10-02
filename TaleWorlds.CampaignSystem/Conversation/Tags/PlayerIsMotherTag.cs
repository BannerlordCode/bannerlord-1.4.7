using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200024E RID: 590
	public class PlayerIsMotherTag : ConversationTag
	{
		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06002327 RID: 8999 RVA: 0x0009ADE2 File Offset: 0x00098FE2
		public override string StringId
		{
			get
			{
				return "PlayerIsMotherTag";
			}
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x0009ADE9 File Offset: 0x00098FE9
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.Mother == Hero.MainHero;
		}

		// Token: 0x04000A84 RID: 2692
		public const string Id = "PlayerIsMotherTag";
	}
}
