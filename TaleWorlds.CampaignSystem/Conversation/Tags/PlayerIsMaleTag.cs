using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000259 RID: 601
	public class PlayerIsMaleTag : ConversationTag
	{
		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x06002348 RID: 9032 RVA: 0x0009B06A File Offset: 0x0009926A
		public override string StringId
		{
			get
			{
				return "PlayerIsMaleTag";
			}
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x0009B071 File Offset: 0x00099271
		public override bool IsApplicableTo(CharacterObject character)
		{
			return !Hero.MainHero.IsFemale;
		}

		// Token: 0x04000A8F RID: 2703
		public const string Id = "PlayerIsMaleTag";
	}
}
