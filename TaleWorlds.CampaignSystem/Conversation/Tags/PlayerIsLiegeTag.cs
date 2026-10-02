using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200026D RID: 621
	public class PlayerIsLiegeTag : ConversationTag
	{
		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06002384 RID: 9092 RVA: 0x0009B63F File Offset: 0x0009983F
		public override string StringId
		{
			get
			{
				return "PlayerIsLiegeTag";
			}
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x0009B648 File Offset: 0x00099848
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.MapFaction.IsKingdomFaction && character.HeroObject.MapFaction == Hero.MainHero.MapFaction && Hero.MainHero.MapFaction.Leader == Hero.MainHero;
		}

		// Token: 0x04000AA4 RID: 2724
		public const string Id = "PlayerIsLiegeTag";
	}
}
