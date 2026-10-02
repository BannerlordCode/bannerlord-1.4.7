using System;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x02000239 RID: 569
	public struct ConversationSentenceOption
	{
		// Token: 0x04000A4C RID: 2636
		public int SentenceNo;

		// Token: 0x04000A4D RID: 2637
		public string Id;

		// Token: 0x04000A4E RID: 2638
		public object RepeatObject;

		// Token: 0x04000A4F RID: 2639
		public TextObject Text;

		// Token: 0x04000A50 RID: 2640
		public string DebugInfo;

		// Token: 0x04000A51 RID: 2641
		public bool IsClickable;

		// Token: 0x04000A52 RID: 2642
		public bool HasPersuasion;

		// Token: 0x04000A53 RID: 2643
		public string SkillName;

		// Token: 0x04000A54 RID: 2644
		public string TraitName;

		// Token: 0x04000A55 RID: 2645
		public bool IsSpecial;

		// Token: 0x04000A56 RID: 2646
		public bool IsUsedOnce;

		// Token: 0x04000A57 RID: 2647
		public TextObject HintText;

		// Token: 0x04000A58 RID: 2648
		public PersuasionOptionArgs PersuationOptionArgs;
	}
}
