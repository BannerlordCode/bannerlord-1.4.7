using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200023C RID: 572
	public abstract class ConversationTag
	{
		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x060022F0 RID: 8944
		public abstract string StringId { get; }

		// Token: 0x060022F1 RID: 8945
		public abstract bool IsApplicableTo(CharacterObject character);

		// Token: 0x060022F2 RID: 8946 RVA: 0x0009AA08 File Offset: 0x00098C08
		public override string ToString()
		{
			return this.StringId;
		}
	}
}
