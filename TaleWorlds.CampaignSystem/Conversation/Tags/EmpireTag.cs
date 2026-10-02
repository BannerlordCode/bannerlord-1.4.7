using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000278 RID: 632
	public class EmpireTag : ConversationTag
	{
		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x060023A5 RID: 9125 RVA: 0x0009B86F File Offset: 0x00099A6F
		public override string StringId
		{
			get
			{
				return "EmpireTag";
			}
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x0009B876 File Offset: 0x00099A76
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.Culture.StringId == "empire";
		}

		// Token: 0x04000AAF RID: 2735
		public const string Id = "EmpireTag";
	}
}
