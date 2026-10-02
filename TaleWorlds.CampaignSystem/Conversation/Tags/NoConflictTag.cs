using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200025D RID: 605
	public class NoConflictTag : ConversationTag
	{
		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06002354 RID: 9044 RVA: 0x0009B204 File Offset: 0x00099404
		public override string StringId
		{
			get
			{
				return "NoConflictTag";
			}
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x0009B20C File Offset: 0x0009940C
		public override bool IsApplicableTo(CharacterObject character)
		{
			bool flag = new HostileRelationshipTag().IsApplicableTo(character);
			bool flag2 = new PlayerIsEnemyTag().IsApplicableTo(character);
			return !flag && !flag2;
		}

		// Token: 0x04000A93 RID: 2707
		public const string Id = "NoConflictTag";
	}
}
