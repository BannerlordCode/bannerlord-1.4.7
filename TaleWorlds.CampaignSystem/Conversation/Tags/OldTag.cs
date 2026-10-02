using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000269 RID: 617
	public class OldTag : ConversationTag
	{
		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06002378 RID: 9080 RVA: 0x0009B543 File Offset: 0x00099743
		public override string StringId
		{
			get
			{
				return "OldTag";
			}
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x0009B54A File Offset: 0x0009974A
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.Age > (float)Campaign.Current.Models.AgeModel.BecomeOldAge;
		}

		// Token: 0x04000AA0 RID: 2720
		public const string Id = "OldTag";
	}
}
