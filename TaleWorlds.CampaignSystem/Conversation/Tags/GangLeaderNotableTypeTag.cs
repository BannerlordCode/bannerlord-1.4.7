using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000271 RID: 625
	public class GangLeaderNotableTypeTag : ConversationTag
	{
		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06002390 RID: 9104 RVA: 0x0009B710 File Offset: 0x00099910
		public override string StringId
		{
			get
			{
				return "GangLeaderNotableTypeTag";
			}
		}

		// Token: 0x06002391 RID: 9105 RVA: 0x0009B717 File Offset: 0x00099917
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.Occupation == Occupation.GangLeader;
		}

		// Token: 0x04000AA8 RID: 2728
		public const string Id = "GangLeaderNotableTypeTag";
	}
}
