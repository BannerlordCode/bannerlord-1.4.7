using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000268 RID: 616
	public class EngagedToPlayerTag : ConversationTag
	{
		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06002375 RID: 9077 RVA: 0x0009B515 File Offset: 0x00099715
		public override string StringId
		{
			get
			{
				return "EngagedToPlayerTag";
			}
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x0009B51C File Offset: 0x0009971C
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && Romance.GetRomanticLevel(character.HeroObject, Hero.MainHero) == Romance.RomanceLevelEnum.CoupleAgreedOnMarriage;
		}

		// Token: 0x04000A9F RID: 2719
		public const string Id = "EngagedToPlayerTag";
	}
}
