using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200024D RID: 589
	public class PlayerIsFatherTag : ConversationTag
	{
		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06002324 RID: 8996 RVA: 0x0009ADB5 File Offset: 0x00098FB5
		public override string StringId
		{
			get
			{
				return "PlayerIsFatherTag";
			}
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x0009ADBC File Offset: 0x00098FBC
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.Father == Hero.MainHero;
		}

		// Token: 0x04000A83 RID: 2691
		public const string Id = "PlayerIsFatherTag";
	}
}
