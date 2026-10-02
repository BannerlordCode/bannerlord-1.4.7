using System;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x02000238 RID: 568
	public interface IConversationStateHandler
	{
		// Token: 0x060022C0 RID: 8896
		void OnConversationInstall();

		// Token: 0x060022C1 RID: 8897
		void OnConversationUninstall();

		// Token: 0x060022C2 RID: 8898
		void OnConversationActivate();

		// Token: 0x060022C3 RID: 8899
		void OnConversationDeactivate();

		// Token: 0x060022C4 RID: 8900
		void OnConversationContinue();

		// Token: 0x060022C5 RID: 8901
		void ExecuteConversationContinue();
	}
}
