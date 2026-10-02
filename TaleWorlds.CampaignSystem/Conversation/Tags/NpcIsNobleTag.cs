using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200025A RID: 602
	public class NpcIsNobleTag : ConversationTag
	{
		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x0600234B RID: 9035 RVA: 0x0009B088 File Offset: 0x00099288
		public override string StringId
		{
			get
			{
				return "NpcIsNobleTag";
			}
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x0009B090 File Offset: 0x00099290
		public override bool IsApplicableTo(CharacterObject character)
		{
			Hero heroObject = character.HeroObject;
			if (heroObject == null)
			{
				return false;
			}
			Clan clan = heroObject.Clan;
			bool? flag = ((clan != null) ? new bool?(clan.IsNoble) : null);
			bool flag2 = true;
			return (flag.GetValueOrDefault() == flag2) & (flag != null);
		}

		// Token: 0x04000A90 RID: 2704
		public const string Id = "NpcIsNobleTag";
	}
}
