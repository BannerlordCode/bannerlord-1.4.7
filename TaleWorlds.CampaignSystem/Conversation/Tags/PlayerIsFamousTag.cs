using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000252 RID: 594
	public class PlayerIsFamousTag : ConversationTag
	{
		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06002333 RID: 9011 RVA: 0x0009AEFE File Offset: 0x000990FE
		public override string StringId
		{
			get
			{
				return "PlayerIsFamousTag";
			}
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x0009AF05 File Offset: 0x00099105
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Clan.PlayerClan.Renown >= 50f;
		}

		// Token: 0x04000A88 RID: 2696
		public const string Id = "PlayerIsFamousTag";
	}
}
