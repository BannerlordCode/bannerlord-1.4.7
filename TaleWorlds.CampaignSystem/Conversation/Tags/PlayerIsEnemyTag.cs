using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000247 RID: 583
	public class PlayerIsEnemyTag : ConversationTag
	{
		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06002312 RID: 8978 RVA: 0x0009AC64 File Offset: 0x00098E64
		public override string StringId
		{
			get
			{
				return "PlayerIsEnemyTag";
			}
		}

		// Token: 0x06002313 RID: 8979 RVA: 0x0009AC6B File Offset: 0x00098E6B
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && FactionManager.IsAtWarAgainstFaction(character.HeroObject.MapFaction, Hero.MainHero.MapFaction);
		}

		// Token: 0x04000A7D RID: 2685
		public const string Id = "PlayerIsEnemyTag";
	}
}
