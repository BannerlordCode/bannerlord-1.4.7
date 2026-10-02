using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000258 RID: 600
	public class PlayerIsFemaleTag : ConversationTag
	{
		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06002345 RID: 9029 RVA: 0x0009B04F File Offset: 0x0009924F
		public override string StringId
		{
			get
			{
				return "PlayerIsFemaleTag";
			}
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x0009B056 File Offset: 0x00099256
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Hero.MainHero.IsFemale;
		}

		// Token: 0x04000A8E RID: 2702
		public const string Id = "PlayerIsFemaleTag";
	}
}
