using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000279 RID: 633
	public class BattanianTag : ConversationTag
	{
		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x060023A8 RID: 9128 RVA: 0x0009B895 File Offset: 0x00099A95
		public override string StringId
		{
			get
			{
				return "BattanianTag";
			}
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x0009B89C File Offset: 0x00099A9C
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.Culture.StringId == "battania";
		}

		// Token: 0x04000AB0 RID: 2736
		public const string Id = "BattanianTag";
	}
}
