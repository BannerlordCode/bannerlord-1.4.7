using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200024A RID: 586
	public class PlayerIsSpouseTag : ConversationTag
	{
		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x0600231B RID: 8987 RVA: 0x0009ACEE File Offset: 0x00098EEE
		public override string StringId
		{
			get
			{
				return "PlayerIsSpouseTag";
			}
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x0009ACF5 File Offset: 0x00098EF5
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && Hero.MainHero.Spouse == character.HeroObject;
		}

		// Token: 0x04000A80 RID: 2688
		public const string Id = "PlayerIsSpouseTag";
	}
}
